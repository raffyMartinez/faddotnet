/*
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/11/2016
 * Time: 7:04 AM
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using FAD3.Database.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;
using Oware;

namespace FAD3
{
    /// <summary>
    /// Description of AOI.
    /// </summary>
    public class TargetArea
    {
        public fadSubgridStyle SubgridStyle { get; set; } = fadSubgridStyle.SubgridStyleNone;
        private string _targetAreaGuid = "";
        private string _targetAreaName = "";
        private string _targetAreaLetter = "";
        private string _majorGrids = "";
        private Dictionary<string, string> _targetAreas = new Dictionary<string, string>();
        private Dictionary<string, string> _landingSites = new Dictionary<string, string>();
        private static string _lastError;
        public fadUTMZone UTMZone { get; set; }
        private string _utmZone;
        public static PointF UpperLeftPoint { get; internal set; }
        public static PointF LowerRightPoint { get; internal set; }

        public double Width { get; internal set; }
        public double Height { get; internal set; }
        public double Area { get; internal set; }
        public PointF UpperLeftPointLL { get; internal set; }
        public PointF LowerRightPointLL { get; internal set; }
        public List<(string UpperLeft, string LowerRight, string GridDescription, PointF UpperLeftCorner, PointF LowerRightCorner)> ListGridExtents { get; private set; }

        public static string LastError
        {
            get { return _lastError; }
        }

        public bool LandingSiteExists(string landingSiteName)
        {
            bool exists = false;
            GetLandingSites();
            if (_landingSites.ContainsValue(landingSiteName))
            {
                exists = true;
            }

            return exists;
        }

        public static bool TargetAreaExists(string targetAreaName, string targetAreaGuid)
        {
            return false;
        }

        public string MajorGrids
        {
            get { return _majorGrids; }
        }

        public Dictionary<string, string> LandingSites
        {
            get
            {
                GetLandingSites();
                return _landingSites;
            }
        }

        public Dictionary<string, string> TargetAreas
        {
            get
            {
                getTargetAreas();
                return _targetAreas;
            }
        }

        public string TargetAreaLetter
        {
            get { return _targetAreaLetter; }
        }

        public string TargetAreaName
        {
            get { return _targetAreaName; }
            set { _targetAreaName = value; }
        }

        public string TargetAreaGuid
        {
            get { return _targetAreaGuid; }
            set
            {
                _targetAreaGuid = value;
                _targetAreaName = GetTargetAreaName(_targetAreaGuid);
            }
        }

        public TargetArea()
        {
            //default constructor
        }

        public static int CountSamplings(string targetAreaGuid)
        {
            string sql = $@"SELECT Count(SamplingGUID) AS n
                                FROM tblSampling
                                WHERE AOI={{{targetAreaGuid}}}
                                GROUP BY AOI
                                ";
            int count = 0;
            using (var con = new OleDbConnection(global.ConnectionString))
            {
                con.Open();

                using (OleDbCommand getCount = new OleDbCommand(sql, con))
                {
                    try
                    {
                        count = (int)getCount.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex.Message, "TargetArea", "CountSamplings");
                    }
                }
            }
            return count;
        }

        public static bool Delete(string aoiGuid)
        {
            bool Success = false;
            string updateQuery = "";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    if (CountSamplings(aoiGuid) > 0)
                    {
                    }
                    Landingsite.DeleteEx(aoiGuid);
                    FishingGrid.DeleteAdditionalFishingGroundMaps(aoiGuid);
                    Enumerators.DeleteEnumerators(aoiGuid);
                    updateQuery = $"Delete * from tblAOI where AOIGuid = {{{aoiGuid}}}";
                    conn.Open();
                    using (OleDbCommand update = new OleDbCommand(updateQuery, conn))
                    {
                        Success = (update.ExecuteNonQuery() > 0);
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    Logger.Log($"{ex.Message}\r\n{ex.Source}");
                    _lastError = ex.Message;
                }
            }

            return Success;
        }

        public List<(int year, int countSamplings)> SampledYearsEx()
        {
            DataTable dt = new DataTable();
            List<(int year, int countSamplings)> myList = new List<(int year, int countSamplings)>();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT Year([SamplingDate]) AS SamplingYear, Count(tblLandingSites.LSGUID) AS n
                                      FROM tblLandingSites INNER JOIN tblSampling ON tblLandingSites.LSGUID = tblSampling.LSGUID
                                      WHERE tblLandingSites.AOIGuid={{{_targetAreaGuid}}} GROUP BY Year([SamplingDate])";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        myList.Add((int.Parse(dr["SamplingYear"].ToString()), int.Parse(dr["n"].ToString())));
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
                return myList;
            }
        }

        public List<string> SampledYears()
        {
            DataTable dt = new DataTable();
            List<string> myList = new List<string>();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    const string query = "SELECT Year([SamplingDate]) AS sYear, Count(SamplingGUID) AS n FROM tblSampling GROUP BY Year([SamplingDate]) ORDER BY Year([SamplingDate]);";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        myList.Add(dr["sYear"].ToString() + ": " + dr["n"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
                return myList;
            }
        }

        public string GetTargetAreaName(string AOIGUID)
        {
            string myName = "";
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $"SELECT AOIName, UTMZone, Letter, MajorGridList, SubgridStyle from tblAOI WHERE AOIGuid= '{{{_targetAreaGuid}}}'";
                    var command = new OleDbCommand(query, conection);
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        myName = reader["AOIName"].ToString();
                        _targetAreaLetter = reader["Letter"].ToString();
                        _majorGrids = reader["MajorGridList"].ToString();
                        _utmZone = reader["UTMZone"].ToString();
                        SubgridStyle = (fadSubgridStyle)(int)reader["SubgridStyle"];
                        UTMZone = fadUTMZone.utmZone_Undefined;
                        if (_utmZone.Length > 0)
                        {
                            switch (_utmZone)
                            {
                                case "51N":
                                    UTMZone = fadUTMZone.utmZone51N;
                                    break;

                                case "50N":
                                    UTMZone = fadUTMZone.utmZone50N;
                                    break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
                return myName;
            }
        }

        public static Dictionary<string, string> GetTargetAreasEx(ComboBox c = null)
        {
            var myList = new Dictionary<string, string>();
            DataTable dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    const string query = "SELECT AOIGuid, AOIName FROM tblAOI order by AOIName";
                    using (var adapter = new OleDbDataAdapter(query, conection))
                    {
                        adapter.Fill(dt);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow dr = dt.Rows[i];
                            myList.Add(dr["AOIGuid"].ToString(), dr["AOIName"].ToString());
                            if (c != null)
                                c.Items.Add(new KeyValuePair<string, string>(dr["AOIGuid"].ToString(), dr["AOIName"].ToString()));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }

                return myList;
            }
        }

        private void getTargetAreas()
        {
            _targetAreas.Clear();
            DataTable dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    const string query = "SELECT AOIGuid, AOIName FROM tblAOI order by AOIName";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        _targetAreas.Add(dr["AOIGuid"].ToString(), dr["AOIName"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }
        }

        public static Landingsite LandingSiteFromName(string name, string AOIGuid)
        {
            var landingSite = new Landingsite();
            DataTable dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $"SELECT * from tblLandingSites where LSName = '{name}' and AOIGuid = {{{AOIGuid}}}";
                    using (var adapter = new OleDbDataAdapter(query, conection))
                    {
                        adapter.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            DataRow dr = dt.Rows[0];
                            landingSite.LandingSiteGUID = dr["LSGUID"].ToString();
                        }
                        else
                        {
                            landingSite = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                    landingSite = null;
                }
            }
            return landingSite;
        }

        public static Dictionary<string, string> LandingSitesFromTargetArea(string targetAreaGuid, ComboBox c = null)
        {
            Dictionary<string, string> LandingSites = new Dictionary<string, string>();
            DataTable dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $"SELECT LSGUID, LSName FROM tblLandingSites WHERE AOIGuid= {{{targetAreaGuid}}} order by LSName";
                    using (var adapter = new OleDbDataAdapter(query, conection))
                    {
                        adapter.Fill(dt);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow dr = dt.Rows[i];
                            LandingSites.Add(dr["LSGUID"].ToString(), dr["LSName"].ToString());

                            if (c != null)
                                c.Items.Add(new KeyValuePair<string, string>(dr["LSGUID"].ToString(), dr["LSName"].ToString()));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }

            return LandingSites;
        }

        public TargetArea(string AOIGUID)
        {
            _targetAreaGuid = AOIGUID;
        }

        public long SampleCount()
        {
            long myCount = 0;
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $"SELECT Count(SamplingGUID) AS n FROM tblSampling WHERE AOI= {{{_targetAreaGuid}}}";

                    var command = new OleDbCommand(query, conection);
                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        myCount = Convert.ToInt32(reader["n"]);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }
            return myCount;
        }

        public Dictionary<string, string> TargetAreaWithSamplingCount()
        {
            Dictionary<string, string> myDict = new Dictionary<string, string>();
            DataTable dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    const string query = @"SELECT tblAOI.AOIName, tblAOI.AOIGuid, Count(tblSampling.SamplingGUID) AS n FROM tblSampling
                                           RIGHT JOIN tblAOI ON tblSampling.AOI = tblAOI.AOIGuid GROUP BY tblAOI.AOIName, tblAOI.AOIGuid";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        myDict.Add(dr["AOIGuid"].ToString(), $"{dr["AOIName"].ToString()}: {dr["n"].ToString()}");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }

            return myDict;
        }

        public Dictionary<string, string> ListLandingSiteWithSamplingCount()
        {
            Dictionary<string, string> myLandingSites = new Dictionary<string, string>();
            DataTable dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT tblLandingSites.LSGUID, tblLandingSites.LSName, Count(tblSampling.SamplingGUID) AS n
                                   FROM tblLandingSites LEFT JOIN tblSampling ON tblLandingSites.LSGUID = tblSampling.LSGUID
                                   WHERE tblLandingSites.AOIGuid = {{{_targetAreaGuid}}} GROUP BY tblLandingSites.LSGUID, tblLandingSites.LSName";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        myLandingSites.Add(dr["LSGUID"].ToString(), dr["LSName"].ToString() + ": " + dr["n"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }

            return myLandingSites;
        }

        private void GetLandingSites()
        {
            _landingSites.Clear();
            DataTable dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $"SELECT LSGUID, LSName FROM tblLandingSites WHERE AOIGuid= {{{_targetAreaGuid}}} order by LSName";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        _landingSites.Add(dr["LSGUID"].ToString(), dr["LSName"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }
        }

        public static List<(string gearClass, string gearVariation, string GearVarGuid, int count)> TargetAreaGearsUsed(string aoiGuid)
        {
            var myList = new List<(string gearClass, string gearVariation, string GearVarGuid, int count)>();
            DataTable dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();

                    string query = $@"SELECT tblGearClass.GearClassName, tblGearVariations.Variation, tblGearVariations.GearVarGUID, Count(tblSampling.SamplingGUID) AS n
                                     FROM (tblGearClass INNER JOIN tblGearVariations ON tblGearClass.GearClass = tblGearVariations.GearClass) INNER JOIN tblSampling ON
                                     tblGearVariations.GearVarGUID = tblSampling.GearVarGUID WHERE tblSampling.AOI={{{aoiGuid}}}
                                     GROUP BY tblGearClass.GearClassName, tblGearVariations.Variation, tblGearVariations.GearVarGUID
                                     ORDER BY tblGearClass.GearClassName, tblGearVariations.Variation";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        myList.Add((dr["GearClassName"].ToString(), dr["Variation"].ToString(), dr["GearVarGUID"].ToString(), (int)dr["n"]));
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }
            return myList;
        }

        public static bool AddNewTargetArea(string targetAreaName, string targetAreaGuid,
            string targetAreaCode, fadSubgridStyle subGridStyle, fadUTMZone zone)
        {
            bool success = false;
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    string utmzone = "";
                    switch (zone)
                    {
                        case fadUTMZone.utmZone50N:
                            utmzone = "50N";
                            break;

                        case fadUTMZone.utmZone51N:
                            utmzone = "51N";
                            break;

                        case fadUTMZone.utmZone_Undefined:
                            break;
                    }
                    string sql = $@"Insert into tblAOI (AOIGUID, AOIName, Letter, SubgridStyle, UTMZone)
                            Values (
                                  {{{targetAreaGuid}}},
                                  '{targetAreaName}',
                                  '{targetAreaCode}',
                                   {(int)subGridStyle},
                                   '{utmzone}'
                                    )";
                    OleDbCommand update = new OleDbCommand(sql, conn);
                    conn.Open();
                    success = (update.ExecuteNonQuery() > 0);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }
            return success;
        }

        public static bool UpdateData(Dictionary<string, string> AOIData)
        {
            string updateQuery = "";
            bool Success = false;
            fadSubgridStyle subGridStyle = fadSubgridStyle.SubgridStyleNone;
            switch (AOIData["SubGridStyle"])
            {
                case "0":
                case "-1":
                    break;

                case "1":
                    subGridStyle = fadSubgridStyle.SubgridStyle4;
                    break;

                case "2":
                    subGridStyle = fadSubgridStyle.SubgridStyle9;
                    break;
            }
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    var DataStatus = fad3DataStatus.statusFromDB;
                    Enum.TryParse(AOIData["DataStatus"], out DataStatus);
                    if (DataStatus == fad3DataStatus.statusNew)
                    {
                        updateQuery = $@"Insert into tblAOI (AOIGUID, AOIName, Letter, SubgridStyle)
                            Values (
                                  {{{AOIData["AOIGUID"]}}},
                                  '{AOIData["AOIName"]}',
                                  '{AOIData["Letter"]}',
                                   {(int)subGridStyle}
                                    )";
                    }
                    else if (DataStatus == fad3DataStatus.statusEdited)
                    {
                        updateQuery = $@"Update tblAOI set
                            AOIName = '{AOIData["AOIName"]}',
                            SubgridStyle = {(int)subGridStyle}
                            Where AOIGUID = {{{AOIData["AOIGUID"]}}}";
                    }
                    else if (DataStatus == fad3DataStatus.statusForDeletion)
                    {
                        updateQuery = $"Delete * from tbaAOI where AOIGUID={{{AOIData["AOIGUID"]}}}";
                    }

                    OleDbCommand update = new OleDbCommand(updateQuery, conn);
                    conn.Open();
                    Success = (update.ExecuteNonQuery() > 0);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }
            return Success;
        }

        public Dictionary<string, string> TargetAreaDataEx()
        {
            Dictionary<string, string> myData = new Dictionary<string, string>();
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $"Select AOIName, Letter, SubGridStyle from tblAOI where AOIGuid = {{{_targetAreaGuid}}}";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        myData.Add("AOIName", dr["AOIName"].ToString());
                        myData.Add("Code", dr["Letter"].ToString());
                        myData.Add("SubgridStyle", dr["SubgridStyle"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }
            return myData;
        }

        public string TargetAreaData()
        {
            ListGridExtents?.Clear();
            string rv = "";
            string utmZone = "";
            string ulg = "";
            string lrg = "";
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();

                    string query = $"Select AOIName, Letter, MajorGridList, SubgridStyle, UTMZone, UpperLeftGrid, LowerRightGrid, GridDescription from tblAOI where AOIGuid = {{{_targetAreaGuid}}}";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            if (i == 0)
                            {
                                rv = dr[i].ToString();
                            }
                            else
                            {
                                rv += "|" + dr[i].ToString();
                            }
                        }
                        utmZone = dr["UTMZone"].ToString();
                        if (utmZone.Length > 0)
                        {
                            ListGridExtents = new List<(string UpperLeft, string LowerRight, string GridDescription, PointF UpperLeftCorner, PointF LowerRightCorner)>();
                            ulg = dr["UpperLeftGrid"].ToString();
                            lrg = dr["LowerRightGrid"].ToString();
                            PointF ulp = FishingGrid.Grid25ToUTMPoint(ulg);
                            ulp = new PointF(ulp.X - 1000, ulp.Y + 1000);
                            PointF lrp = FishingGrid.Grid25ToUTMPoint(lrg);
                            lrp = new PointF(lrp.X + 1000, lrp.Y - 1000);
                            ListGridExtents.Add((ulg, lrg, dr["GridDescription"].ToString(), ulp, lrp));
                            GetAdditionalMapExtents();
                            ComputeTargetAreaExtent();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }
            return rv;
        }

        private void ComputeTargetAreaExtent()
        {
            UpperLeftPoint = new PointF(ListGridExtents[0].UpperLeftCorner.X, ListGridExtents[0].UpperLeftCorner.Y);
            LowerRightPoint = new PointF(ListGridExtents[0].LowerRightCorner.X, ListGridExtents[0].LowerRightCorner.Y);
            if (ListGridExtents.Count > 0)
            {
                for (int n = 1; n < ListGridExtents.Count; n++)
                {
                    var ulX = ListGridExtents[n].UpperLeftCorner.X;
                    var ulY = ListGridExtents[n].UpperLeftCorner.Y;
                    if (UpperLeftPoint.Y > ulY)
                    {
                        ulY = UpperLeftPoint.Y;
                    }
                    if (UpperLeftPoint.X < ulX)
                    {
                        ulX = UpperLeftPoint.X;
                    }
                    UpperLeftPoint = new PointF(ulX, ulY);

                    var lrX = ListGridExtents[n].LowerRightCorner.X;
                    var lrY = ListGridExtents[n].LowerRightCorner.Y;
                    if (LowerRightPoint.Y < lrY)
                    {
                        lrY = LowerRightPoint.Y;
                    }
                    if (LowerRightPoint.X > lrX)
                    {
                        lrX = LowerRightPoint.X;
                    }
                    LowerRightPoint = new PointF(lrX, lrY);
                }
            }

            Width = LowerRightPoint.X - UpperLeftPoint.X;
            Height = UpperLeftPoint.Y - LowerRightPoint.Y;
            Area = Width * Height;

            int ZoneNumber = 0;
            switch (FishingGrid.UTMZone)
            {
                case fadUTMZone.utmZone51N:
                    ZoneNumber = 51;
                    break;

                case fadUTMZone.utmZone50N:
                    ZoneNumber = 50;
                    break;
            }

            LatLngUTMConverter LL2UTM = new LatLngUTMConverter("");
            var xy = LL2UTM.convertUtmToLatLng(UpperLeftPoint.X, UpperLeftPoint.Y, ZoneNumber, "N");
            UpperLeftPointLL = new PointF((float)xy.Lng, (float)xy.Lat);
            xy = LL2UTM.convertUtmToLatLng(LowerRightPoint.X, LowerRightPoint.Y, ZoneNumber, "N");
            LowerRightPointLL = new PointF((float)xy.Lng, (float)xy.Lat);
        }

        private void GetAdditionalMapExtents()
        {
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();

                    string query = $"Select UpperLeft, LowerRight, GridDescription from tblAdditionalAOIExtent where AOIGuid = {{{_targetAreaGuid}}}";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int n = 0; n < dt.Rows.Count; n++)
                        {
                            DataRow dr = dt.Rows[n];
                            string ulg = dr["UpperLeft"].ToString();
                            string lrg = dr["LowerRight"].ToString();
                            PointF ulp = FishingGrid.Grid25ToUTMPoint(ulg);
                            ulp = new PointF(ulp.X - 1000, ulp.Y + 1000);
                            PointF lrp = FishingGrid.Grid25ToUTMPoint(lrg);
                            lrp = new PointF(lrp.X + 1000, lrp.Y - 1000);
                            ListGridExtents.Add((ulg, lrg, dr["GridDescription"].ToString(), ulp, lrp));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }
        }

        public Dictionary<string, string> ListYearsWithSamplingCount()
        {
            Dictionary<string, string> myYears = new Dictionary<string, string>();
            var myDT = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT Year([SamplingDate]) AS sYear, Count(tblSampling.SamplingGUID) AS n
                                      FROM tblSampling WHERE AOI= {{{_targetAreaGuid}}} GROUP BY Year([SamplingDate])";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(myDT);
                    for (int i = 0; i < myDT.Rows.Count; i++)
                    {
                        DataRow dr = myDT.Rows[i];
                        myYears.Add(dr["sYear"].ToString(), dr["n"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }
            return myYears;
        }

        public static string TargetAreaCodeFromGuid(string AOIGuid)
        {
            var code = "";
            using (var con = new OleDbConnection(global.ConnectionString))
            {
                con.Open();
                var sql = $"Select Letter from tblAOI where AOIGuid = {{{AOIGuid}}}";

                using (OleDbCommand getCode = new OleDbCommand(sql, con))
                {
                    try
                    {
                        code = (string)getCode.ExecuteScalar();
                    }
                    catch
                    {
                        code = "";
                    }
                }
            }

            return code;
        }
    }
}
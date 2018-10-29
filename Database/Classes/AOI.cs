/*
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/11/2016
 * Time: 7:04 AM
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using FAD3.GUI.Classes;

namespace FAD3
{
    /// <summary>
    /// Description of AOI.
    /// </summary>
    public class aoi
    {
        private string _AOIGUID = "";
        private string _AOIName = "";
        private string _AOILetter = "";
        private string _MajorGrids = "";
        private Dictionary<string, string> _aois = new Dictionary<string, string>();
        private Dictionary<string, string> _landingSites = new Dictionary<string, string>();
        private static string _lastError;

        public static string LastError
        {
            get { return _lastError; }
        }

        public bool LandingSiteExists(string landingSiteName)
        {
            bool exists = false;
            getLandingSites();
            if (_landingSites.ContainsValue(landingSiteName))
            {
                exists = true;
            }

            return exists;
        }

        public string MajorGrids
        {
            get { return _MajorGrids; }
        }

        public Dictionary<string, string> LandingSites
        {
            get
            {
                getLandingSites();
                return _landingSites;
            }
        }

        public Dictionary<string, string> AOIs
        {
            get
            {
                getAOIs();
                return _aois;
            }
        }

        public string AOILetter
        {
            get { return _AOILetter; }
        }

        public string AOIName
        {
            get { return _AOIName; }
            set { _AOIName = value; }
        }

        public string AOIGUID
        {
            get { return _AOIGUID; }
            set
            {
                _AOIGUID = value;
                _AOIName = GetAOIName(_AOIGUID);
            }
        }

        public aoi()
        {
            //default constructor
        }

        public static bool Delete(string aoiGuid)
        {
            bool Success = false;
            string updateQuery = "";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {
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
                                      WHERE tblLandingSites.AOIGuid={{{_AOIGUID}}} GROUP BY Year([SamplingDate])";

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
                    Logger.Log(ex);
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
                    Logger.Log(ex);
                }
                return myList;
            }
        }

        public string GetAOIName(string AOIGUID)
        {
            string myName = "";
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $"SELECT AOIName, Letter, MajorGridList from tblAOI WHERE AOIGuid= '{{{_AOIGUID}}}'";
                    var command = new OleDbCommand(query, conection);
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        myName = reader["AOIName"].ToString();
                        _AOILetter = reader["Letter"].ToString();
                        _MajorGrids = reader["MajorGridList"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
                return myName;
            }
        }

        public static Dictionary<string, string> getAOIsEx(ComboBox c = null)
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
                    Logger.Log(ex);
                }

                return myList;
            }
        }

        private void getAOIs()
        {
            _aois.Clear();
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
                        _aois.Add(dr["AOIGuid"].ToString(), dr["AOIName"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
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
                    Logger.Log(ex);
                    landingSite = null;
                }
            }
            return landingSite;
        }

        public static Dictionary<string, string> LandingSitesFromAOI(string AOIguid, ComboBox c = null)
        {
            Dictionary<string, string> LandingSites = new Dictionary<string, string>();
            DataTable dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $"SELECT LSGUID, LSName FROM tblLandingSites WHERE AOIGuid= {{{AOIguid}}} order by LSName";
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
                    Logger.Log(ex);
                }
            }

            return LandingSites;
        }

        public aoi(string AOIGUID)
        {
            _AOIGUID = AOIGUID;
        }

        public long SampleCount()
        {
            long myCount = 0;
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $"SELECT Count(SamplingGUID) AS n FROM tblSampling WHERE AOI= {{{_AOIGUID}}}";

                    var command = new OleDbCommand(query, conection);
                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        myCount = Convert.ToInt32(reader["n"]);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
            return myCount;
        }

        public Dictionary<string, string> AOIWithSamplingCount()
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
                    Logger.Log(ex);
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
                                   WHERE tblLandingSites.AOIGuid = {{{_AOIGUID}}} GROUP BY tblLandingSites.LSGUID, tblLandingSites.LSName";
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
                    Logger.Log(ex);
                }
            }

            return myLandingSites;
        }

        private void getLandingSites()
        {
            _landingSites.Clear();
            DataTable dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $"SELECT LSGUID, LSName FROM tblLandingSites WHERE AOIGuid= {{{_AOIGUID}}} order by LSName";
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
                    Logger.Log(ex);
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
                    //string query = $@"SELECT tblGearClass.GearClassName, tblGearVariations.Variation, tblRefGearCodes.RefGearCode
                    //                FROM (tblGearClass INNER JOIN (tblGearVariations INNER JOIN (tblRefGearCodes INNER JOIN tblRefGearCodes_Usage
                    //                ON tblRefGearCodes.RefGearCode = tblRefGearCodes_Usage.RefGearCode) ON tblGearVariations.GearVarGUID = tblRefGearCodes.GearVar)
                    //                ON tblGearClass.GearClass = tblGearVariations.GearClass) INNER JOIN tblAOI ON tblRefGearCodes_Usage.TargetAreaGUID = tblAOI.AOIGuid
                    //                WHERE tblAOI.AOIGuid={{{aoiGuid}}} ORDER BY tblGearClass.GearClassName, tblGearVariations.Variation, tblRefGearCodes.RefGearCode";

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
                    Logger.Log(ex);
                }
            }
            return myList;
        }

        public static bool UpdateData(Dictionary<string, string> AOIData)
        {
            string updateQuery = "";
            bool Success = false;
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    var DataStatus = fad3DataStatus.statusFromDB;
                    Enum.TryParse(AOIData["DataStatus"], out DataStatus);
                    if (DataStatus == fad3DataStatus.statusNew)
                    {
                        updateQuery = $@"Insert into tblAOI (AOIGUID, AOIName, Letter)
                            Values (
                                  {{{AOIData["AOIGUID"]}}},
                                  '{AOIData["AOIName"]}',
                                  '{AOIData["Letter"]}')";
                    }
                    else if (DataStatus == fad3DataStatus.statusEdited)
                    {
                        updateQuery = $@"Update tblAOI set
                            AOIName = '{AOIData["AOIName"]}'
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
                    Logger.Log(ex);
                }
            }
            return Success;
        }

        public Dictionary<string, string> AOIDataEx()
        {
            Dictionary<string, string> myData = new Dictionary<string, string>();
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $"Select AOIName, Letter from tblAOI where AOIGuid = {{{_AOIGUID}}}";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        myData.Add("AOIName", dr["AOIName"].ToString());
                        myData.Add("Code", dr["Letter"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
            return myData;
        }

        public string AOIData()
        {
            string rv = "";
            var myDT = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();

                    string query = $"Select AOIName, Letter, MajorGridList from tblAOI where AOIGuid = {{{_AOIGUID}}}";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(myDT);
                    DataRow dr = myDT.Rows[0];
                    for (int i = 0; i < myDT.Columns.Count; i++)
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
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
            return rv;
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
                                      FROM tblSampling WHERE AOI= {{{_AOIGUID}}} GROUP BY Year([SamplingDate])";
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
                    Logger.Log(ex);
                }
            }
            return myYears;
        }

        public static string AOICodeFromGuid(string AOIGuid)
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
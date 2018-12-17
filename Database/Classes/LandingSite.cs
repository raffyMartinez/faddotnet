/*
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/11/2016
 * Time: 5:13 PM
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using ISO_Classes;
using FAD3.Database.Classes;

//using System.Diagnostics;

namespace FAD3
{
    /// <summary>
    /// Description of LandingSite.
    /// </summary>
    public class Landingsite : EventArgs
    {
        private string _LandingSiteGUID = "";
        private long _GearUsedCount = 0;
        private string _LandingSiteName = "";
        private double _xCoord = 0;
        private double _yCoord = 0;
        private bool _isInsideTargetArea;
        private Coordinate _coordinate;
        private bool _IsNew = false;
        private static string _lastError;

        public static string LastError
        {
            get { return _lastError; }
        }

        public Coordinate Coordinate
        {
            get { return _coordinate; }
            set { _coordinate = value; }
        }

        public void xyKMLCoordinate(double xCoordinate, double yCoordinate, bool isInsideTargetArea)
        {
            _xCoord = xCoordinate;
            _yCoord = yCoordinate;
            _isInsideTargetArea = isInsideTargetArea;
        }

        public bool IsInsideTargetArea
        {
            get { return _isInsideTargetArea; }
        }

        public double xCoord
        {
            get { return _xCoord; }
        }

        public double yCoord
        {
            get { return _yCoord; }
        }

        public string xCoordDegreeMinute
        {
            get
            { return ToDegreeMinute(_xCoord); }
        }

        public string yCoordDegreeMinute
        {
            get
            { return ToDegreeMinute(_yCoord); }
        }

        private string ToDegreeMinute(double value)
        {
            int deg = (int)value;
            value = Math.Abs(value - deg);
            double min = (value * 60);
            return deg.ToString() + "° " + string.Format("{0:0.00000}", min);
        }

        public Landingsite()
        {
            //empty default constructor
        }

        public void IsNew()
        {
            _coordinate = new Coordinate();
            _IsNew = true;
        }

        public string LandingSiteName
        {
            get { return _LandingSiteName; }
            set { _LandingSiteName = value; }
        }

        public Landingsite(string LandingSiteGUID)
        {
            _LandingSiteGUID = LandingSiteGUID;
        }

        /// <summary>
        /// returns the number of samplings from a landing site
        /// </summary>
        /// <returns></returns>
        public long SampleCount()
        {
            int myCount = 0;
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $"SELECT Count(SamplingGUID) AS n FROM tblSampling WHERE LSGUID= {{{_LandingSiteGUID}}}";
                    var command = new OleDbCommand(query, conection);
                    myCount = (int)command.ExecuteScalar();
                }
                catch (Exception ex) { Logger.Log(ex); }
            }
            return myCount;
        }

        /// <summary>
        /// Delete a landing site that has no samplings. An exception will happen if landing site has sampling
        /// </summary>
        /// <param name="landingSiteGuid"></param>
        /// <returns></returns>
        public static bool Delete(string landingSiteGuid)
        {
            bool Success = false;
            string updateQuery = "";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    updateQuery = $"Delete * from tblLandingSites where LSGUID = {{{landingSiteGuid}}}";
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

        /// <summary>
        /// Moves all sampling data in a landing site to another landing site
        /// </summary>
        /// <param name="fromLandingSiteGUID"></param>
        /// <param name="toLandingSiteGuid"></param>
        /// <returns></returns>
        public static bool MoveToLandingSite(string fromLandingSiteGUID, string toLandingSiteGuid)
        {
            bool Success = false;
            string updateQuery = "";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    updateQuery = $"UPDATE tblSampling SET tblSampling.LSGUID = {{{toLandingSiteGuid}}} WHERE LSGUID = {{{fromLandingSiteGUID}}}";
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

        public static List<(string gearVariationName, string gearVariationGuid, string gearClassName)> SampledGearsFromLandingSite(string landingSiteGuid)
        {
            DataTable dt = new DataTable();
            List<(string gearVariationName, string gearVariationGuid, string gearClassName)> myList = new List<(string gearVariationName, string gearVariationGuid, string gearClassName)>();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT DISTINCT tblGearVariations.GearVarGUID, tblGearVariations.Variation, GearClassName
                                     FROM tblGearClass INNER JOIN (tblGearVariations INNER JOIN tblSampling ON tblGearVariations.GearVarGUID =
                                     tblSampling.GearVarGUID) ON tblGearClass.GearClass = tblGearVariations.GearClass
                                     WHERE tblSampling.LSGUID={{{landingSiteGuid}}}";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        myList.Add((dr["Variation"].ToString(), dr["GearVarGUID"].ToString(), dr["GearClassName"].ToString()));
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
                return myList;
            }
        }

        public static bool Delete(string name, string aoiGuid)
        {
            bool Success = false;
            string updateQuery = "";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    updateQuery = $"Delete * from tblLandingSites where LSName = '{name}' and AOIGuid = {{{aoiGuid}}}";
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
                }
            }

            return Success;
        }

        public List<(int year, int countSamplings)> SampledYears()
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
                                            GROUP BY Year([SamplingDate]), tblLandingSites.LSGUID
                                            HAVING tblLandingSites.LSGUID={{{_LandingSiteGUID}}}";
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

        public List<(int year, int countSamplings)> SampledYears(string gearVariationGUID)
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
                                      GROUP BY Year([SamplingDate]), tblSampling.GearVarGUID, tblLandingSites.LSGUID
                                      HAVING tblSampling.GearVarGUID={{{gearVariationGUID}}} AND
                                      tblLandingSites.LSGUID={{{_LandingSiteGUID}}}";
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

        /// <summary>
        /// updates the data of a landing site
        /// </summary>
        /// <param name="isNew"></param>
        /// <param name="LSData"></param>
        /// <returns></returns>
        public bool UpdateData(bool isNew, Dictionary<string, string> LSData)
        {
            bool Success = false;
            string updateQuery = "";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    var xCoord = "null";
                    var yCoord = "null";
                    if (bool.Parse(LSData["HasCoordinate"]))
                    {
                        xCoord = _coordinate.Longitude.ToString();
                        yCoord = _coordinate.Latitude.ToString();
                    }

                    if (isNew)
                    {
                        updateQuery = $@"Insert into tblLandingSites
                            (AOIGUID, LSGUID, LSName, MunNo, cx, cy) values (
                            {{{LSData["AOIGuid"]}}},
                            {{{LSData["LSGUID"]}}},
                            '{LSData["LSName"]}',
                            {LSData["MunNo"]},
                            {xCoord},
                            {yCoord})";
                    }
                    else
                    {
                        updateQuery = $@"UPDATE tblLandingSites set
                            LSName = '{LSData["LSName"]}',
                            cx = {xCoord},
                            cy = {yCoord},
                            MunNo = {LSData["MunNo"]}
                            Where LSGUID= {{{LSData["LSGUID"]}}}";
                    }
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
                }
            }

            return Success;
        }

        public static Dictionary<int, (string landingSiteName, string Municipality, string Province, int MunicipalityNumber)> LandingSiteDictionary(string AOIGuid)
        {
            var dict = new Dictionary<int, (string landingSiteName, string Municipality, string Province, int MunicipalityNumber)>();
            var myDT = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT LSName, Municipality, ProvinceName, Municipalities.MunNo
                            FROM Provinces INNER JOIN (Municipalities INNER JOIN tblLandingSites ON Municipalities.MunNo = tblLandingSites.MunNo)
                            ON Provinces.ProvNo = Municipalities.ProvNo
                            WHERE tblLandingSites.AOIGuid={{{AOIGuid}}}";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(myDT);
                    for (int i = 0; i < myDT.Rows.Count; i++)
                    {
                        DataRow dr = myDT.Rows[i];
                        dict.Add((int)dr["MunNo"], (dr["LSName"].ToString(), dr["Municipality"].ToString(), dr["ProvinceName"].ToString(), (int)dr["MunNo"]));
                    }
                }
                catch (Exception ex) { Logger.Log(ex); }
            }
            return dict;
        }

        public string LandingSiteGUID
        {
            get { return _LandingSiteGUID; }
            set
            {
                _LandingSiteGUID = value;
                _coordinate = new Coordinate();
            }
        }

        /// <summary>
        /// provides a List that contains the months and counts of samplings done per month
        /// </summary>
        /// <returns></returns>
        public List<string> MonthsSampled()
        {
            List<string> myList = new List<string>();
            var myDT = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT Format([SamplingDate],'mmm-yyyy') AS sMonth, Count(SamplingGUID) AS n
                                      FROM tblSampling WHERE LSGUID= {{{_LandingSiteGUID}}} GROUP BY Format([SamplingDate],'mmm-yyyy')
                                      ORDER BY First(SamplingDate)";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(myDT);
                    for (int i = 0; i < myDT.Rows.Count; i++)
                    {
                        DataRow dr = myDT.Rows[i];
                        myList.Add(dr[0].ToString() + ": " + dr[1].ToString());
                    }
                }
                catch (Exception ex) { Logger.Log(ex); }
            }
            return myList;
        }

        /// <summary>
        /// returns a Dictionary containing the gears and number of samplings per gear in the Landing site
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> Gears()
        {
            Dictionary<string, string> myGears = new Dictionary<string, string>();
            var myDT = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT tblSampling.GearVarGUID, tblGearVariations.Variation, tblGearClass.GearClassName, Count(tblSampling.SamplingGUID) AS n
                                      FROM tblGearClass INNER JOIN (tblGearVariations INNER JOIN tblSampling ON tblGearVariations.GearVarGUID = tblSampling.GearVarGUID)
                                      ON tblGearClass.GearClass = tblGearVariations.GearClass WHERE tblSampling.LSGUID = {{{_LandingSiteGUID}}}
                                      GROUP BY tblSampling.GearVarGUID, tblGearVariations.Variation, tblGearClass.GearClassName ORDER BY tblGearVariations.Variation";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(myDT);
                    for (int i = 0; i < myDT.Rows.Count; i++)
                    {
                        DataRow dr = myDT.Rows[i];
                        myGears.Add(dr["GearVarGUID"].ToString(), $"{dr["Variation"].ToString()}: {dr["n"].ToString()}");
                        _GearUsedCount++;
                    }
                }
                catch (Exception ex) { Logger.Log(ex); }
            }
            return myGears;
        }

        /// <summary>
        /// Used in a landing site form to provides data about a landing site
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> LandingSiteDataEx()
        {
            Dictionary<string, string> myLSData = new Dictionary<string, string>();
            var myDT = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT tblLandingSites.LSName, Municipalities.Municipality, Provinces.ProvinceName,
                                      tblLandingSites.cx, tblLandingSites.cy, Municipalities.ProvNo
                                      FROM Provinces INNER JOIN (Municipalities INNER JOIN tblLandingSites ON Municipalities.MunNo =
                                      tblLandingSites.MunNo) ON Provinces.ProvNo = Municipalities.ProvNo
                                      WHERE tblLandingSites.LSGUID= {{{_LandingSiteGUID}}}";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(myDT);
                    for (int i = 0; i < myDT.Rows.Count; i++)
                    {
                        DataRow dr = myDT.Rows[i];
                        myLSData.Add("LSName", dr["LSname"].ToString());
                        myLSData.Add("Municipality", dr["Municipality"].ToString());
                        myLSData.Add("ProvinceName", dr["ProvinceName"].ToString());
                        myLSData.Add("CoordinateStringXY", $"{dr["cx"].ToString()},{dr["cy"].ToString()}");
                        if (myLSData["CoordinateStringXY"].Length > 1)
                        {
                            _coordinate.SetD(float.Parse(dr["cy"].ToString()), float.Parse(dr["cx"].ToString()));
                            switch (global.CoordinateDisplay)
                            {
                                case CoordinateDisplayFormat.DegreeDecimal:
                                    myLSData["CoordinateStringXY"] = Coordinate.ToString("D");
                                    break;

                                case CoordinateDisplayFormat.DegreeMinute:
                                    myLSData["CoordinateStringXY"] = Coordinate.ToString("DM");
                                    break;

                                case CoordinateDisplayFormat.DegreeMinuteSecond:
                                    myLSData["CoordinateStringXY"] = Coordinate.ToString("DMS");
                                    break;

                                case CoordinateDisplayFormat.UTM:
                                    break;
                            }
                        }
                        else
                        {
                            myLSData["CoordinateStringXY"] = "";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }

            return myLSData;
        }

        /// <summary>
        /// Provides a summary view of a landing site and is used in the main form
        /// </summary>
        /// <returns></returns>
        public string LandingSiteData()
        {
            string rv = "";
            var myDT = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT tblLandingSites.LSName, Municipalities.Municipality, Provinces.ProvinceName,
                                      tblLandingSites.cx, tblLandingSites.cy
                                      FROM Provinces INNER JOIN (Municipalities INNER JOIN tblLandingSites ON Municipalities.MunNo =
                                      tblLandingSites.MunNo) ON Provinces.ProvNo = Municipalities.ProvNo
                                      WHERE tblLandingSites.LSGUID= {{{_LandingSiteGUID}}}";

                    double num = 0;
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(myDT);
                    DataRow dr = myDT.Rows[0];
                    _LandingSiteName = dr["LSName"].ToString();

                    if (double.TryParse((dr["cx"].ToString()), out num))
                    {
                        _xCoord = num;
                    }
                    if (double.TryParse((dr["cy"].ToString()), out num))
                    {
                        _yCoord = num;
                    }

                    for (int i = 0; i < myDT.Columns.Count; i++)
                    {
                        if (i == 0)
                        {
                            rv = dr[i].ToString();
                        }
                        else
                        {
                            rv += "," + dr[i].ToString();
                        }
                    }
                }
                catch (Exception ex) { Logger.Log(ex); }
            }
            return rv;
        }
    }
}
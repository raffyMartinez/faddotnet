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

//using System.Diagnostics;

namespace FAD3
{
    /// <summary>
    /// Description of LandingSite.
    /// </summary>
    public class landingsite
    {
        private string _LandingSiteGUID = "";
        private long _GearUsedCount = 0;
        private string _LandingSiteName = "";
        private double _xCoord = 0;
        private double _yCoord = 0;
        private Coordinate _Coordinate;
        private bool _IsNew = false;

        public Coordinate Coordinate
        {
            get { return _Coordinate; }
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

        public landingsite()
        {
            //empty default constructor
        }

        public void IsNew()
        {
            _Coordinate = new Coordinate();
            _IsNew = true;
        }

        public string LandingSiteName
        {
            get { return _LandingSiteName; }
            set { _LandingSiteName = value; }
        }

        public landingsite(string LandingSiteGUID)
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
                        xCoord = _Coordinate.Longitude.ToString();
                        yCoord = _Coordinate.Latitude.ToString();
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

        public string LandingSiteGUID
        {
            get { return _LandingSiteGUID; }
            set
            {
                _LandingSiteGUID = value;
                _Coordinate = new Coordinate();
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
                        myGears.Add(dr[0].ToString(), dr[1].ToString() + ": " + dr[3].ToString());
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
                            _Coordinate.SetD(float.Parse(dr["cy"].ToString()), float.Parse(dr["cx"].ToString()));
                            switch (global.CoordinateDisplay)
                            {
                                case global.CoordinateDisplayFormat.DegreeDecimal:
                                    myLSData["CoordinateStringXY"] = Coordinate.ToString("D");
                                    break;

                                case global.CoordinateDisplayFormat.DegreeMinute:
                                    myLSData["CoordinateStringXY"] = Coordinate.ToString("DM");
                                    break;

                                case global.CoordinateDisplayFormat.DegreeMinuteSecond:
                                    myLSData["CoordinateStringXY"] = Coordinate.ToString("DMS");
                                    break;

                                case global.CoordinateDisplayFormat.UTM:
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
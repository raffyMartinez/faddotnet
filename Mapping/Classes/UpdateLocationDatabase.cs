using FAD3.Database.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using Microsoft.Win32;

namespace FAD3.Mapping.Classes
{
    public static class UpdateLocationDatabase
    {
        public static int UpdateRegions()
        {
            string fileRegionsXML = $@"{global.ApplicationPath}\regions.xml";
            int regionNo = 0;
            string regionName = "";

            if (File.Exists(fileRegionsXML))
            {
                var regions = GetRegions();
                XmlTextReader xmlReader = new XmlTextReader(fileRegionsXML);
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        switch (xmlReader.Name)
                        {
                            case "Regions":
                                break;

                            case "RegionNo":
                                regionNo = xmlReader.ReadElementContentAsInt();
                                break;

                            case "RegionName":
                                regionName = xmlReader.ReadElementContentAsString();
                                string action = "";
                                if (!regions.ContainsKey(regionNo))
                                {
                                    action = "addNew";
                                }
                                else if (regions.ContainsKey(regionNo) && regions[regionNo] != regionName)
                                {
                                    action = "update";
                                }
                                if (action.Length > 0)
                                {
                                    UpdateRegion(regionNo, regionName, action);
                                }
                                break;
                        }
                    }
                }
            }

            return 0;
        }

        private static Dictionary<int, Province> GetProvinces()
        {
            Dictionary<int, Province> provinces = new Dictionary<int, Province>();
            const string sql = "Select ProvNo, ProvinceName, Region from Provinces order by ProvNo";
            var dt = new DataTable();
            using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;data source=" + global.MDBPath))
            {
                try
                {
                    conection.Open();

                    var adapter = new OleDbDataAdapter(sql, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        Province prov = new Province((int)dr["ProvNo"], dr["ProvinceName"].ToString(), (int)dr["Region"]);
                        provinces.Add(prov.ProvinceNumber, prov);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, "UpdateLocationDatabase.cs", "GetProvinces");
                }
            }
            return provinces;
        }

        private static Dictionary<int, string> GetRegions()
        {
            Dictionary<int, string> regions = new Dictionary<int, string>();
            const string sql = "Select RegionNo, RegionName from Regions order by RegionNo";
            var dt = new DataTable();
            using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;data source=" + global.MDBPath))
            {
                try
                {
                    conection.Open();

                    var adapter = new OleDbDataAdapter(sql, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        regions.Add((int)dr["RegionNo"], dr["RegionName"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, "UpdateLocationDatabase.cs", "GetRegions");
                }
            }
            return regions;
        }

        private static bool UpdateRegion(int regionNumber, string regionName, string action)
        {
            bool success = false;
            string sql = "";

            if (action == "addNew")
            {
                sql = $@"Insert into Regions (RegionNo, RegionName)
                            values ({regionNumber}, '{regionName}')";
            }
            else if (action == "update")
            {
                sql = $@"Update Regions set
                        RegionName='{regionName}'
                        WHERE RegionNo={regionNumber}";
            }

            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                using (OleDbCommand update = new OleDbCommand(sql, conn))
                {
                    try
                    {
                        success = update.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex.Message, "UpdateLocationDatabase", "UpdateRegion");
                    }
                }
            }
            return success;
        }

        private static bool UpdateProvince(int provinceNumber, string provinceName, int regionNumber, string action)
        {
            bool success = false;
            string sql = "";
            if (action == "addNew")
            {
                sql = $@"Insert into Provinces (ProvNo,ProvinceName,Region) values
                        (
                            {provinceNumber},
                            '{provinceName}',
                            {regionNumber}
                        )";
            }
            else if (action == "update")
            {
                sql = $@"Update Provinces
                         SET ProvinceName = '{provinceName}',
                         Region = {regionNumber}
                         WHERE ProvNo= {provinceNumber}
                        ";
            }
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                using (OleDbCommand update = new OleDbCommand(sql, conn))
                {
                    try
                    {
                        success = update.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex.Message, "UpdateLocationDatabase", "UpdateProvince");
                    }
                }
            }
            return success;
        }

        private static Dictionary<int, LGU> GetMunicipalities()
        {
            Dictionary<int, LGU> municipalities = new Dictionary<int, LGU>();
            const string sql = "Select * from Municipalities order by ProvNo, Municipality";
            var dt = new DataTable();
            using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;data source=" + global.MDBPath))
            {
                try
                {
                    conection.Open();

                    var adapter = new OleDbDataAdapter(sql, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        double xCoord = 0;
                        double yCoord = 0;
                        if (double.TryParse(dr["xCoord"].ToString(), out double x))
                        {
                            xCoord = x;
                        }
                        if (double.TryParse(dr["yCoord"].ToString(), out double y))
                        {
                            yCoord = y;
                        }
                        LGU lgu = new LGU((int)dr["ProvNo"], (int)dr["MunNo"], dr["Municipality"].ToString(), xCoord, yCoord, (bool)dr["IsCoastal"], (int)dr["WRIAdminID"]);
                        municipalities.Add(lgu.MunicipalityNumber, lgu);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, "UpdateLocationDatabase.cs", "GetProvinces");
                }
            }
            return municipalities;
        }

        public static int UpdateMunicipalities()
        {
            string fileMunicipalitiesXML = $@"{global.ApplicationPath}\municipalities.xml";
            int provNo = 0;
            int munNo = 0;
            int wriAdminID = 0;
            string municipalityName = "";
            double x = 0;
            double y = 0;
            bool isCoastal = false;

            if (File.Exists(fileMunicipalitiesXML))
            {
                var lgus = GetMunicipalities();
                XmlTextReader xmlReader = new XmlTextReader(fileMunicipalitiesXML);
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        switch (xmlReader.Name)
                        {
                            case "PROVNO":
                                provNo = xmlReader.ReadElementContentAsInt();
                                break;

                            case "MUNNO":
                                munNo = xmlReader.ReadElementContentAsInt();
                                break;

                            case "XCOORD":
                                x = xmlReader.ReadElementContentAsDouble();
                                break;

                            case "Coastal":
                                isCoastal = xmlReader.ReadElementContentAsString() == "T";

                                string action = "";
                                if (!lgus.ContainsKey(munNo))
                                {
                                    action = "addNew";
                                }
                                else if (lgus.ContainsKey(munNo)
                                    &&
                                    (
                                    lgus[munNo].MunicipalityName != municipalityName)
                                    || lgus[munNo].IsCoastal != isCoastal
                                    || lgus[munNo].ProvinceNumber != provNo
                                    || lgus[munNo].xCoord != x
                                    || lgus[munNo].yCoord != y
                                    )
                                {
                                    action = "update";
                                }
                                if (action.Length > 0)
                                {
                                    UpdateMunicipality(provNo, munNo, municipalityName, x, y, wriAdminID, isCoastal, action);
                                }
                                break;

                            case "YCOORD":
                                y = xmlReader.ReadElementContentAsDouble();
                                break;

                            case "WRIADMINID":
                                wriAdminID = xmlReader.ReadElementContentAsInt();
                                break;

                            case "MUNICIPALI":
                                municipalityName = xmlReader.ReadElementContentAsString();
                                break;
                        }
                    }
                }
            }

            return 0;
        }

        private static bool UpdateMunicipality(int provinceNumber, int municipalityNumber, string municipalityName,
                                        double x, double y, int wriID, bool isCoastal, string action)
        {
            bool success = false;
            string sql = "";
            if (action == "addNew")
            {
                sql = $@"Insert into Municipalities (ProvNo, MunNo, Municipality, WRIAdminID, xCoord, yCoord, IsCoastal )values
                        (
                            {provinceNumber},
                            {municipalityNumber},
                            '{municipalityName}',
                            {wriID},
                            {x},
                            {y},
                            {isCoastal}
                        )";
            }
            else if (action == "update")
            {
                sql = $@"Update Municipalities set
                         ProvNo = {provinceNumber},
                         Municipality = ""{municipalityName}"",
                         WRIAdminID = {wriID},
                         xCoord = {x},
                         yCoord = {y},
                         IsCoastal = {isCoastal}
                         WHERE MunNo = {municipalityNumber}";
            }

            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                using (OleDbCommand update = new OleDbCommand(sql, conn))
                {
                    try
                    {
                        success = update.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex.Message, "UpdateLocationDatabase", "UpdateMunicipality");
                    }
                }
            }
            return success;
        }

        public static int UpdateProvinces()
        {
            string fileProvinceXML = $@"{global.ApplicationPath}\provinces.xml";

            string provinceName = "";
            int provinceNumber = 0;
            int regionNumber = 0;

            if (File.Exists(fileProvinceXML))
            {
                var provinces = GetProvinces();
                XmlTextReader xmlReader = new XmlTextReader(fileProvinceXML);
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        switch (xmlReader.Name)
                        {
                            case "Provinces":

                                break;

                            case "ProvNo":
                                provinceNumber = xmlReader.ReadElementContentAsInt();
                                break;

                            case "ProvinceName":
                                provinceName = xmlReader.ReadElementContentAsString();
                                break;

                            case "Region":
                                string action = "";
                                regionNumber = xmlReader.ReadElementContentAsInt();
                                if (!provinces.ContainsKey(provinceNumber))
                                {
                                    action = "addNew";
                                }
                                else if (provinces.ContainsKey(provinceNumber)
                                    && (
                                        provinces[provinceNumber].ProvinceName != provinceName
                                        || provinces[provinceNumber].RegionNumber != regionNumber
                                        )
                                    )
                                {
                                    action = "update";
                                }
                                if (action.Length > 0)
                                {
                                    UpdateProvince(provinceNumber, provinceName, regionNumber, action);
                                }
                                break;
                        }
                    }
                }
            }
            return 0;
        }
    }
}
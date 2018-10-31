using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Threading.Tasks;
using FAD3.GUI.Classes;

namespace FAD3.Database.Classes
{
    public class FishingGearInventory
    {
        public aoi AOI { get; }
        private Dictionary<string, (string InventoryName, DateTime DateConducted)> _inventories = new Dictionary<string, (string InventoryName, DateTime DateConducted)>();
        private Dictionary<string, (string Province, string Municipality, string barangay, string sitio)> _barangayInventories = new Dictionary<string, (string Province, string Municipality, string barangay, string sitio)>();

        public Dictionary<string, (string InventoryName, DateTime DateConducted)> Inventories
        {
            get { return _inventories; }
        }

        public Dictionary<string, (string Province, string Municipality, string barangay, string sitio)> BarangayInventories
        {
            get { return _barangayInventories; }
        }

        public FishingGearInventory(aoi AOI)
        {
            this.AOI = AOI;
            ReadInventories(AOI.AOIGUID);
        }

        public (int totalFishers, int totalMotorized, int totalNonMotorized, int totalCommercial) GetLevelSummary(string inventoryGuid, string provinceName = "", string municipalityName = "", string barangayName = "")
        {
            (int totalFishers, int totalMotorized, int totalNonMotorized, int totalCommercial) summary;
            summary.totalFishers = -1;
            summary.totalCommercial = -1;
            summary.totalMotorized = -1;
            summary.totalNonMotorized = -1;
            var dt = new DataTable();
            string query = "";
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    if (barangayName.Length > 0)
                    {
                        query = $@"SELECT tblGearInventoryBarangay.InventoryGuid, Provinces.ProvinceName,
                                Municipalities.Municipality, tblGearInventoryBarangay.Barangay,
                                Sum(tblGearInventoryBarangay.CountFishers) AS TotalFishers,
                                Sum(tblGearInventoryBarangay.CountMunicipalMotorized) AS TotalMotorized,
                                Sum(tblGearInventoryBarangay.CountMunicipalNonMotorized) AS TotalNonMotorized,
                                Sum(tblGearInventoryBarangay.CountCommercial) AS TotalCommercial
                                FROM Provinces INNER JOIN (Municipalities INNER JOIN tblGearInventoryBarangay ON
                                Municipalities.MunNo = tblGearInventoryBarangay.Municipality) ON
                                Provinces.ProvNo = Municipalities.ProvNo
                                GROUP BY tblGearInventoryBarangay.InventoryGuid, Provinces.ProvinceName, Municipalities.Municipality,
                                tblGearInventoryBarangay.Barangay
                                HAVING tblGearInventoryBarangay.InventoryGuid= {{{inventoryGuid}}} AND
                                    Provinces.ProvinceName='{provinceName}' AND
                                    Municipalities.Municipality='{municipalityName}' AND
                                    tblGearInventoryBarangay.Barangay='{barangayName}'";
                    }
                    else if (municipalityName.Length > 0)
                    {
                        query = $@"SELECT tblGearInventoryBarangay.InventoryGuid, Provinces.ProvinceName, Municipalities.Municipality,
                                Sum(tblGearInventoryBarangay.CountFishers) AS TotalFishers,
                                Sum(tblGearInventoryBarangay.CountMunicipalMotorized) AS TotalMotorized,
                                Sum(tblGearInventoryBarangay.CountMunicipalNonMotorized) AS TotalNonMotorized,
                                Sum(tblGearInventoryBarangay.CountCommercial) AS TotalCommercial
                                FROM Provinces INNER JOIN (Municipalities INNER JOIN tblGearInventoryBarangay ON
                                Municipalities.MunNo = tblGearInventoryBarangay.Municipality) ON
                                Provinces.ProvNo = Municipalities.ProvNo
                                GROUP BY tblGearInventoryBarangay.InventoryGuid, Provinces.ProvinceName, Municipalities.Municipality
                                HAVING tblGearInventoryBarangay.InventoryGuid={{{inventoryGuid}}}
                                    AND Provinces.ProvinceName='{provinceName}'
                                    AND Municipalities.Municipality='{municipalityName}'";
                    }
                    else if (provinceName.Length > 0)
                    {
                        query = $@"SELECT tblGearInventoryBarangay.InventoryGuid, Provinces.ProvinceName,
                                Sum(tblGearInventoryBarangay.CountFishers) AS TotalFishers,
                                Sum(tblGearInventoryBarangay.CountMunicipalMotorized) AS TotalMotorized,
                                Sum(tblGearInventoryBarangay.CountMunicipalNonMotorized) AS TotalNonMotorized,
                                Sum(tblGearInventoryBarangay.CountCommercial) AS TotalCommercial
                                FROM Provinces INNER JOIN (Municipalities INNER JOIN tblGearInventoryBarangay
                                    ON Municipalities.MunNo = tblGearInventoryBarangay.Municipality) ON Provinces.ProvNo = Municipalities.ProvNo
                                GROUP BY tblGearInventoryBarangay.InventoryGuid, Provinces.ProvinceName
                                HAVING tblGearInventoryBarangay.InventoryGuid= {{{inventoryGuid}}} AND Provinces.ProvinceName='{provinceName}'";
                    }
                    else
                    {
                        query = $@"SELECT tblGearInventoryBarangay.InventoryGuid, Sum(tblGearInventoryBarangay.CountFishers) AS TotalFishers,
                                    Sum(tblGearInventoryBarangay.CountMunicipalMotorized) AS TotalMotorized,
                                    Sum(tblGearInventoryBarangay.CountMunicipalNonMotorized) AS TotalNonMotorized,
                                    Sum(tblGearInventoryBarangay.CountCommercial) AS TotalCommercial
                                    FROM tblGearInventoryBarangay
                                    GROUP BY tblGearInventoryBarangay.InventoryGuid
                                    HAVING tblGearInventoryBarangay.InventoryGuid={{{inventoryGuid}}}";
                    }

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);

                    DataRow dr = dt.Rows[0];
                    summary = (int.Parse(dr["TotalFishers"].ToString()), int.Parse(dr["TotalMotorized"].ToString()), int.Parse(dr["TotalNonMotorized"].ToString()), int.Parse(dr["TotalCommercial"].ToString()));
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
                return summary;
            }
        }

        public (string province, string municipality, string barangay, string sitio,
                string gearClass, string gearVariation, List<string> gearLocalNames,
                int commercialCount, int motorizedCount, int nonMotorizedCount, int noBoatCount,
                List<int> monthsInUse, List<int> peakMonths) GetGearVariationInventoryData(string inventoryGuid)
        {
            List<string> gearLocalNames = new List<string>();
            List<int> monthsInUse = new List<int>();
            List<int> peakMonths = new List<int>();
            string gearClass = "";
            string gearVariation = "";
            string province = "";
            string municipality = "";
            string barangay = "";
            string sitio = "";
            int commercialCount = 0;
            int motorizedCount = 0;
            int nonMotorizedCount = 0;
            int noBoatCount = 0;

            string sql = $@"SELECT Provinces.ProvinceName, Municipalities.Municipality, tblGearInventoryBarangay.Barangay,
                                tblGearInventoryBarangay.Sitio, tblGearClass.GearClassName,
                                tblGearVariations.Variation, tblGearInventoryBarangayData.*
                                FROM (tblGearClass INNER JOIN tblGearVariations ON tblGearClass.GearClass = tblGearVariations.GearClass) INNER JOIN
                                  (Provinces INNER JOIN (Municipalities INNER JOIN (tblGearInventoryBarangay INNER JOIN
                                  tblGearInventoryBarangayData ON tblGearInventoryBarangay.BarangayInventoryGuid = tblGearInventoryBarangayData.BarangayInventoryGUID) ON
                                  Municipalities.MunNo = tblGearInventoryBarangay.Municipality) ON Provinces.ProvNo = Municipalities.ProvNo) ON
                                  tblGearVariations.GearVarGUID = tblGearInventoryBarangayData.GearVariation
                                  WHERE tblGearInventoryBarangayData.DataGuid={{{inventoryGuid}}}";
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    var adapter = new OleDbDataAdapter(sql, conection);
                    adapter.Fill(dt);
                    DataRow dr = dt.Rows[0];
                    province = dr["ProvinceName"].ToString();
                    municipality = dr["Municipality"].ToString();
                    barangay = dr["Barangay"].ToString();
                    sitio = dr["sitio"].ToString();
                    gearClass = dr["GearClassName"].ToString();
                    gearVariation = dr["Variation"].ToString();
                    commercialCount = (int)dr["CountCommercial"];
                    motorizedCount = (int)dr["CountMunicipalMotorized"];
                    nonMotorizedCount = (int)dr["CountMunicipalNonMotorized"];
                    noBoatCount = (int)dr["CountNoBoat"];
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }

            sql = $@"SELECT tblGearLocalNames.LocalName
                    FROM tblGearLocalNames INNER JOIN tblGearInventoryGearLocalNames ON
                      tblGearLocalNames.LocalNameGUID = tblGearInventoryGearLocalNames.LocalNameGuid
                    WHERE tblGearInventoryGearLocalNames.InventoryDataGuid = {{{inventoryGuid}}}";
            dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    var adapter = new OleDbDataAdapter(sql, conection);
                    adapter.Fill(dt);
                    for (int n = 0; n < dt.Rows.Count; n++)
                    {
                        DataRow dr = dt.Rows[n];
                        gearLocalNames.Add(dr["LocalName"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }

            sql = $@"SELECT tblGearInventoryMonthsUsed.MonthNumber
                    FROM tblGearInventoryMonthsUsed
                    WHERE tblGearInventoryMonthsUsed.InventoryDataGuid={{{inventoryGuid}}}
                    ORDER BY tblGearInventoryMonthsUsed.MonthNumber";
            dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    var adapter = new OleDbDataAdapter(sql, conection);
                    adapter.Fill(dt);
                    for (int n = 0; n < dt.Rows.Count; n++)
                    {
                        DataRow dr = dt.Rows[n];
                        monthsInUse.Add((int)dr["MonthNumber"]);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }

            sql = $@"SELECT tblGearInventoryPeakMonths.PeakSeasonMonthNumber
                    FROM tblGearInventoryPeakMonths
                    WHERE tblGearInventoryPeakMonths.InventoryDataGuid={{{inventoryGuid}}}
                    ORDER BY tblGearInventoryPeakMonths.PeakSeasonMonthNumber";
            dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    var adapter = new OleDbDataAdapter(sql, conection);
                    adapter.Fill(dt);
                    for (int n = 0; n < dt.Rows.Count; n++)
                    {
                        DataRow dr = dt.Rows[n];
                        peakMonths.Add((int)dr["PeakSeasonMonthNumber"]);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }

            return (province, municipality, barangay, sitio, gearClass, gearVariation, gearLocalNames,
                   commercialCount, motorizedCount, nonMotorizedCount, noBoatCount, monthsInUse, peakMonths);
        }

        public string BarangayInventoryGuid(string inventoryGuid, string province, string municipality, string barangay, string sitio = "")
        {
            string brgyInventoryGuid = "";
            if (sitio == "entireBarangay")
            {
                sitio = string.Empty;
            }
            using (var conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                string sql = $@"SELECT tblGearInventoryBarangay.BarangayInventoryGuid
                            FROM Provinces INNER JOIN
                              (Municipalities INNER JOIN
                               tblGearInventoryBarangay ON
                               Municipalities.MunNo = tblGearInventoryBarangay.Municipality) ON
                               Provinces.ProvNo = Municipalities.ProvNo
                            WHERE tblGearInventoryBarangay.InventoryGuid={{{inventoryGuid}}} AND
                              Provinces.ProvinceName='{province}' AND
                              Municipalities.Municipality='{municipality}' AND
                              tblGearInventoryBarangay.Barangay='{barangay}' AND
                              tblGearInventoryBarangay.Sitio='{sitio}'";

                using (OleDbCommand getGuid = new OleDbCommand(sql, conn))
                {
                    try
                    {
                        brgyInventoryGuid = getGuid.ExecuteScalar().ToString();
                    }
                    catch
                    {
                        brgyInventoryGuid = string.Empty;
                    }
                }
            }
            return brgyInventoryGuid;
        }

        public (int municipalityCount, int barangayCount) NumberOfMunicipalitiesBarangays(string inventoryGuid)
        {
            var dt = new DataTable();
            var municipalityCount = 0;
            var barangayCount = 0;
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT DISTINCT tblGearInventoryBarangay.Municipality
                                    FROM (Provinces INNER JOIN Municipalities ON Provinces.ProvNo = Municipalities.ProvNo) INNER JOIN
                                    tblGearInventoryBarangay ON Municipalities.MunNo = tblGearInventoryBarangay.Municipality
                                    WHERE tblGearInventoryBarangay.InventoryGuid={{{inventoryGuid}}}";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    municipalityCount = dt.Rows.Count;

                    query = $@"SELECT DISTINCT tblGearInventoryBarangay.Barangay
                                    FROM (Provinces INNER JOIN Municipalities ON Provinces.ProvNo = Municipalities.ProvNo) INNER JOIN
                                    tblGearInventoryBarangay ON Municipalities.MunNo = tblGearInventoryBarangay.Municipality
                                    WHERE tblGearInventoryBarangay.InventoryGuid={{{inventoryGuid}}}";

                    adapter = new OleDbDataAdapter(query, conection);
                    dt = new DataTable();
                    adapter.Fill(dt);
                    barangayCount = dt.Rows.Count;
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
            return (municipalityCount, barangayCount);
        }

        public bool SaveSitioGearInventoryCatchComposition(string sitioGearInventoryGuid, List<string> dominantCatchGuid, List<string> nonDominantCatchGuid)
        {
            int n = 0;
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conn.Open();
                    string sql = $"Delete * from tblGearInventoryCatchComposition where InventoryDataGuid = {{{sitioGearInventoryGuid}}}";

                    using (OleDbCommand update = new OleDbCommand(sql, conn))
                    {
                        update.ExecuteNonQuery();
                    }

                    foreach (var item in dominantCatchGuid)
                    {
                        sql = $"Insert into tblGearInventoryCatchComposition (InventoryDataGuid, NameOfCatch, RowGuid, IsDominant) values ({{{sitioGearInventoryGuid}}}, {{{item}}},{{{Guid.NewGuid().ToString()}}}, {true})";
                        using (OleDbCommand update = new OleDbCommand(sql, conn))
                        {
                            if (update.ExecuteNonQuery() > 0) n++;
                        }
                    }

                    foreach (var item in nonDominantCatchGuid)
                    {
                        sql = $"Insert into tblGearInventoryCatchComposition (InventoryDataGuid, NameOfCatch, RowGuid, IsDominant) values ({{{sitioGearInventoryGuid}}}, {{{item}}},{{{Guid.NewGuid().ToString()}}}, {false})";
                        using (OleDbCommand update = new OleDbCommand(sql, conn))
                        {
                            if (update.ExecuteNonQuery() > 0) n++;
                        }
                    }

                    conn.Close();
                }
                catch (Exception ex)
                {
                    Logger.Log($"{ex.Message}\r\n{ex.Source}");
                }
            }

            return n > 0;
        }

        public bool SaveSitioGearInventoryHistoricalCPUE(string sitioGearInventoryGuid, List<(int decade, int cpue, string unit)> cpueHistorical)
        {
            int n = 0;
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conn.Open();
                    string sql = $"Delete * from tblGearInventoryCPUEHistorical where InventoryDataGuid = {{{sitioGearInventoryGuid}}}";

                    using (OleDbCommand update = new OleDbCommand(sql, conn))
                    {
                        update.ExecuteNonQuery();
                    }

                    foreach (var item in cpueHistorical)
                    {
                        sql = $@"Insert into tblGearInventoryCPUEHistorical (InventoryDataGuid, Decade, CPUE, CPUEUnit, RowGuid) values
                              ({{{sitioGearInventoryGuid}}}, {item.decade},{item.cpue}, '{item.unit}', {{{Guid.NewGuid().ToString()}}})";
                        using (OleDbCommand update = new OleDbCommand(sql, conn))
                        {
                            if (update.ExecuteNonQuery() > 0) n++;
                        }
                    }

                    conn.Close();
                }
                catch (Exception ex)
                {
                    Logger.Log($"{ex.Message}\r\n{ex.Source}");
                }
            }

            return n > 0;
        }

        public bool SaveSitioGearInventoryFishingMonths(string sitioGearInventoryGuid, List<int> monthsFishing, List<int> monthsSeason)
        {
            int n = 0;
            int m = 0;
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conn.Open();
                    string sql = $"Delete * from tblGearInventoryMonthsUsed where InventoryDataGuid = {{{sitioGearInventoryGuid}}}";

                    using (OleDbCommand update = new OleDbCommand(sql, conn))
                    {
                        update.ExecuteNonQuery();
                    }

                    sql = $"Delete * from tblGearInventoryPeakMonths where InventoryDataGuid = {{{sitioGearInventoryGuid}}}";

                    using (OleDbCommand update = new OleDbCommand(sql, conn))
                    {
                        update.ExecuteNonQuery();
                    }

                    foreach (var item in monthsFishing)
                    {
                        sql = $"Insert into tblGearInventoryMonthsUsed (InventoryDataGuid, MonthNumber, RowGuid) values ({{{sitioGearInventoryGuid}}}, {item + 1},{{{Guid.NewGuid().ToString()}}})";
                        using (OleDbCommand update = new OleDbCommand(sql, conn))
                        {
                            if (update.ExecuteNonQuery() > 0) n++;
                        }
                    }

                    foreach (var item in monthsSeason)
                    {
                        sql = $"Insert into tblGearInventoryPeakMonths (InventoryDataGuid, PeakSeasonMonthNumber, RowGuid) values ({{{sitioGearInventoryGuid}}}, {item + 1},{{{Guid.NewGuid().ToString()}}})";
                        using (OleDbCommand update = new OleDbCommand(sql, conn))
                        {
                            if (update.ExecuteNonQuery() > 0) m++;
                        }
                    }

                    conn.Close();
                }
                catch (Exception ex)
                {
                    Logger.Log($"{ex.Message}\r\n{ex.Source}");
                }
            }

            return n > 0 && m > 0;
        }

        public bool SaveSitioGearInventoryGearLocalNames(string sitioGearInventoryGuid, List<string> localNamesGuid)

        {
            int n = 0;
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conn.Open();
                    string sql = $"Delete * from tblGearInventoryGearLocalNames where InventoryDataGuid = {{{sitioGearInventoryGuid}}}";

                    using (OleDbCommand update = new OleDbCommand(sql, conn))
                    {
                        update.ExecuteNonQuery();
                    }

                    foreach (var item in localNamesGuid)
                    {
                        sql = $"Insert into tblGearInventoryGearLocalNames (InventoryDataGuid, LocalNameGuid, RowGuid) values ({{{sitioGearInventoryGuid}}}, {{{item}}},{{{Guid.NewGuid().ToString()}}})";
                        using (OleDbCommand update = new OleDbCommand(sql, conn))
                        {
                            if (update.ExecuteNonQuery() > 0) n++;
                        }
                    }

                    conn.Close();
                }
                catch (Exception ex)
                {
                    Logger.Log($"{ex.Message}\r\n{ex.Source}");
                }
            }

            return n > 0;
        }

        public bool SaveSitioGearInventoryMain(string barangayInventoryGuid, string gearVariationGuid, int countCommercialUsage,
                        int countMunicipalMotorizedUsage, int countMunicipalNonMotorizedUsage, int countNoBoatGears,
                        int countDaysPerMonthUse, string cpueUnit, int percentageDominantCatch, string guid, fad3DataStatus dataStatus,
                        int? rangeCPUEMax, int? rangeCPUEMin, int? modeCPUEUpper, int? modeCPUELower)
        {
            bool Success = false;
            string updateQuery = "";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    if (dataStatus == fad3DataStatus.statusNew)
                    {
                        updateQuery = $@"Insert into tblGearInventoryBarangayData
                                        (BarangayInventoryGUID, GearVariation, CountCommercial,
                                         CountMunicipalMotorized, CountMunicipalNonMotorized,
                                         CountNoBoat, NumberDaysPerMonth, MaxCPUE, MinCPUE,
                                         ModeUpper, ModeLower, CPUEUnit, DataGuid, DominantCatchPercent)
                                         values
                                         ({{{barangayInventoryGuid}}}, {{{gearVariationGuid}}},
                                          {countCommercialUsage}, {countMunicipalMotorizedUsage},
                                          {countMunicipalNonMotorizedUsage}, {countNoBoatGears},
                                          {countDaysPerMonthUse},{rangeCPUEMax}, {rangeCPUEMin},
                                          {modeCPUEUpper},{modeCPUELower},'{cpueUnit}',{{{guid}}},
                                          {percentageDominantCatch})";
                    }
                    else
                    {
                        updateQuery = $@"";
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

        public List<(string gearClass, string gearVariation, string variationInventoryGuid)> ReadSitioGearInventory(string barangayInventoryGuid)
        {
            List<(string gearClass, string gearVariation, string variationInventoryGuid)> myList = new List<(string gearClass, string gearVariation, string variationInventoryGuid)>();
            string sql = $@"SELECT tblGearClass.GearClassName, tblGearVariations.Variation, tblGearInventoryBarangayData.DataGuid
                            FROM tblGearClass INNER JOIN (tblGearVariations INNER JOIN tblGearInventoryBarangayData ON
                              tblGearVariations.GearVarGUID = tblGearInventoryBarangayData.GearVariation) ON
                              tblGearClass.GearClass = tblGearVariations.GearClass
                            WHERE tblGearInventoryBarangayData.BarangayInventoryGUID={{{barangayInventoryGuid}}}";
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    var adapter = new OleDbDataAdapter(sql, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        myList.Add((dr["GearClassName"].ToString(), dr["Variation"].ToString(), dr["DataGuid"].ToString()));
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
            return myList;
        }

        public (int municipalityCount, int barangayCount) NumberOfMunicipalitiesBarangays(string inventoryGuid, string provinceName)
        {
            var dt = new DataTable();
            var municipalityCount = 0;
            var barangayCount = 0;
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT DISTINCT tblGearInventoryBarangay.Municipality
                                    FROM Provinces INNER JOIN (Municipalities INNER JOIN tblGearInventoryBarangay ON
                                    Municipalities.MunNo = tblGearInventoryBarangay.Municipality) ON Provinces.ProvNo = Municipalities.ProvNo
                                    WHERE tblGearInventoryBarangay.InventoryGuid={{{inventoryGuid}}} AND
                                      Provinces.ProvinceName='{provinceName}'";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    municipalityCount = dt.Rows.Count;

                    query = $@"SELECT DISTINCT tblGearInventoryBarangay.Barangay
                               FROM Provinces INNER JOIN (Municipalities INNER JOIN tblGearInventoryBarangay ON
                               Municipalities.MunNo = tblGearInventoryBarangay.Municipality) ON Provinces.ProvNo = Municipalities.ProvNo
                               WHERE tblGearInventoryBarangay.InventoryGuid={{{inventoryGuid}}} AND
                                 Provinces.ProvinceName='{provinceName}'";

                    adapter = new OleDbDataAdapter(query, conection);
                    dt = new DataTable();
                    adapter.Fill(dt);
                    barangayCount = dt.Rows.Count;
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
            return (municipalityCount, barangayCount);
        }

        public int NumberOfBarangays(string inventoryGuid, string provinceName, string municipalityName)
        {
            int brgyCount = 0;

            using (var conn = new OleDbConnection(global.ConnectionString))
            {
                var dt = new DataTable();
                conn.Open();
                string sql = $@"SELECT DISTINCT tblGearInventoryBarangay.Barangay
                                FROM (Provinces INNER JOIN Municipalities ON
                                  Provinces.ProvNo = Municipalities.ProvNo) INNER JOIN
                                  tblGearInventoryBarangay ON
                                  Municipalities.MunNo = tblGearInventoryBarangay.Municipality
                                WHERE tblGearInventoryBarangay.InventoryGuid={{{inventoryGuid}}} AND
                                  Provinces.ProvinceName='{provinceName}' AND
                                  Municipalities.Municipality='{municipalityName}'";

                var adapter = new OleDbDataAdapter(sql, conn);
                adapter.Fill(dt);
                try
                {
                    brgyCount = dt.Rows.Count;
                }
                catch
                {
                    //do nothing
                }
            }
            return brgyCount;
        }

        public int NumberOfSitio(string inventoryGuid, string provinceName, string municipalityName, string barangayName)
        {
            int sitioCount = 0;
            using (var conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                string sql = $@"SELECT Count(tblGearInventoryBarangay.Sitio) AS n
                                    FROM Provinces INNER JOIN (Municipalities INNER JOIN tblGearInventoryBarangay ON
                                      Municipalities.MunNo = tblGearInventoryBarangay.Municipality) ON
                                      Provinces.ProvNo = Municipalities.ProvNo
                                    WHERE tblGearInventoryBarangay.Sitio <> ''
                                    GROUP BY tblGearInventoryBarangay.InventoryGuid, Provinces.ProvinceName, Municipalities.Municipality,
                                      tblGearInventoryBarangay.Barangay
                                    HAVING tblGearInventoryBarangay.InventoryGuid={{{inventoryGuid}}} AND
                                      Provinces.ProvinceName='{provinceName}' AND
                                      Municipalities.Municipality='{municipalityName}' AND
                                      tblGearInventoryBarangay.Barangay='{barangayName}'";

                using (OleDbCommand getCount = new OleDbCommand(sql, conn))
                {
                    try
                    {
                        sitioCount = (int)getCount.ExecuteScalar();
                    }
                    catch
                    {
                        sitioCount = 0;
                    }
                }
            }
            return sitioCount;
        }

        public void ReadBarangayInventories(string inventoryGuid)
        {
            _barangayInventories.Clear();
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT Provinces.ProvinceName, Municipalities.Municipality, tblGearInventoryBarangay.Barangay,
                                    tblGearInventoryBarangay.Sitio, tblGearInventoryBarangay.BarangayInventoryGuid
                                    FROM (Provinces INNER JOIN Municipalities ON Provinces.ProvNo = Municipalities.ProvNo) INNER JOIN
                                    tblGearInventoryBarangay ON Municipalities.MunNo = tblGearInventoryBarangay.Municipality
                                    WHERE tblGearInventoryBarangay.InventoryGuid={{{inventoryGuid}}}";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        _barangayInventories.Add(dr["BarangayInventoryGuid"].ToString(), (dr["ProvinceName"].ToString(), dr["Municipality"].ToString(), dr["Barangay"].ToString(), dr["Sitio"].ToString()));
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
        }

        public bool SaveBarangayInventory(fad3DataStatus dataStatus, string inventoryGuid, long municipalityNumber, string barangay, int countFishers,
                                           int countCommercial, int countMunicipalMotorized, int countMunicipalNonMotorized, string BarangayInventoryGuid, string sitio = "")
        {
            bool Success = false;
            string updateQuery = "";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    if (dataStatus == fad3DataStatus.statusNew)
                    {
                        updateQuery = $@"Insert into tblGearInventoryBarangay (InventoryGuid, Municipality, Barangay, Sitio, CountFishers,
                                        CountMunicipalMotorized, CountMunicipalNonMotorized, CountCommercial, BarangayInventoryGuid) values (
                                      {{{inventoryGuid}}}, {municipalityNumber}, '{barangay}', '{sitio}', {countFishers},
                                       {countMunicipalMotorized}, {countMunicipalNonMotorized}, {countCommercial}, {{{BarangayInventoryGuid}}})";
                    }
                    else
                    {
                        updateQuery = $@"Update tblGearInventoryBarangay set
                                        Municipality = '{municipalityNumber}',
                                        Barangay = '{barangay}',
                                        Sito = '{sitio}',
                                        CountFishers = {countFishers},
                                        CountMunicipalMotorized = {countMunicipalMotorized},
                                        CountMunicipalNonMotorized = {countMunicipalNonMotorized},
                                        CountCommercial = {countCommercial},
                                        Where BarangayInventoryGuid = {{{BarangayInventoryGuid}}}";
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

        public (int fisherCount, int commercialCount, int motorizedCount, int nonMotorizedCount) GetSitioNumbers(string inventoryGuid, string province, string municipality, string barangay, string sitio = "")
        {
            (int fisherCount, int commercialCount, int motorizedCount, int nonMotorizedCount) numbers = (0, 0, 0, 0);
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    if (sitio == "entireBarangay")
                    {
                        sitio = string.Empty;
                    }
                    string query = $@"SELECT CountFishers, CountMunicipalMotorized, CountMunicipalNonMotorized, CountCommercial
                                    FROM Provinces INNER JOIN (Municipalities INNER JOIN tblGearInventoryBarangay ON
                                      Municipalities.MunNo = tblGearInventoryBarangay.Municipality) ON
                                      Provinces.ProvNo = Municipalities.ProvNo
                                    WHERE tblGearInventoryBarangay.InventoryGuid={{{inventoryGuid}}} AND
                                      Provinces.ProvinceName = '{province}' AND Municipalities.Municipality='{municipality}' AND
                                      tblGearInventoryBarangay.Barangay='{barangay}' AND
                                      tblGearInventoryBarangay.Sitio='{sitio}'";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);

                    DataRow dr = dt.Rows[0];
                    numbers.fisherCount = (int)dr["CountFishers"];
                    numbers.commercialCount = (int)dr["CountCommercial"];
                    numbers.motorizedCount = (int)dr["CountMunicipalMotorized"];
                    numbers.nonMotorizedCount = (int)dr["CountMunicipalNonMotorized"];
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }

            return numbers;
        }

        public bool SaveFishingGearInventory(fad3DataStatus dataStatus, string name, DateTime dateConducted, string inventoryGuid)
        {
            bool Success = false;
            string updateQuery = "";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    if (dataStatus == fad3DataStatus.statusNew)
                    {
                        updateQuery = $@"Insert into tblGearInventories (InventoryGuid, InventoryName, DateConducted, TargetArea) values (
                                      {{{inventoryGuid}}}, '{name}', '{dateConducted}', {{{AOI.AOIGUID}}})";
                    }
                    else
                    {
                        updateQuery = $@"Update tblGearInventories set
                                        InventoryName = '{name}',
                                        DateConducted = '{dateConducted}'
                                        Where InventoryGuid = {{{inventoryGuid}}}";
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

        public (string inventoryName, DateTime? dateImplemented) GetInventory(string inventoryGUID)
        {
            _inventories.Clear();
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $"SELECT InventoryName, DateConducted from tblGearInventories where InventoryGuid = {{{inventoryGUID}}}";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        return (dr["InventoryName"].ToString(), (DateTime)dr["DateConducted"]);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
            return ("", null);
        }

        private void ReadInventories(string AOIGuid)
        {
            _inventories.Clear();
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $"SELECT InventoryGuid, InventoryName, DateConducted from tblGearInventories where TargetArea = {{{AOIGuid}}}";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        _inventories.Add(dr["InventoryGuid"].ToString(), (dr["InventoryName"].ToString(), (DateTime)dr["DateConducted"]));
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
        }
    }
}
using FAD3.GUI.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace FAD3.Database.Classes
{
    /// <summary>
    /// Database helper class for fishery inventory data
    /// </summary>
    public class FishingGearInventory
    {
        private Dictionary<string, (string Province, string Municipality, string MunicipalityNumber, string barangay, string sitio)> _barangayInventories = new Dictionary<string, (string Province, string Municipality, string MunicipalityNumber, string barangay, string sitio)>();
        private Dictionary<string, (string InventoryName, DateTime DateConducted, string TargetArea)> _inventories = new Dictionary<string, (string InventoryName, DateTime DateConducted, string TargetArea)>();
        public event EventHandler<FisheriesInventoryImportEventArg> InventoryLevel;

        public static List<string> Accessories { get; internal set; }
        public static List<string> ExpenseItems { get; internal set; }
        public static List<string> PaymentSources { get; internal set; }

        public static void GetLists()
        {
            GetPaymentSources();
            GetExpenses();
            GetAccessories();
        }

        public static bool AddPaymentSource(string source)
        {
            PaymentSources.Add(source);
            return true;
        }

        public static bool AddAccessory(string accessory)
        {
            Accessories.Add(accessory);
            return true;
        }

        public static bool AddExpense(string expense)
        {
            ExpenseItems.Add(expense);
            return true;
        }

        private static void GetPaymentSources()
        {
            PaymentSources = new List<string>();
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = "SELECT DISTINCT Source from tblGearInventoryExpense Order By Source ";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        PaymentSources.Add(dr["Source"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
        }

        private static void GetExpenses()
        {
            ExpenseItems = new List<string>();
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = "SELECT DISTINCT ExpenseItem from tblGearInventoryExpense Order By ExpenseItem ";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        ExpenseItems.Add(dr["ExpenseItem"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
        }

        private static void GetAccessories()
        {
            Accessories = new List<string>();
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = "SELECT DISTINCT Accessory from tblGearInventoryAccesories Order By Accessory ";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        Accessories.Add(dr["Accessory"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
        }

        public FishingGearInventory(TargetArea targetArea)
        {
            this.TargetArea = targetArea;
            ReadInventoriesInTargetArea(TargetArea.TargetAreaGuid);
        }

        public TargetArea TargetArea { get; }

        public List<(string projectGuid, string projectName, DateTime implementDate)> ProjectsInTargetArea(string targetAreaName)
        {
            List<(string projectGuid, string projectName, DateTime implementDate)> list = new List<(string projectGuid, string projectName, DateTime implementDate)>();
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT tblGearInventories.InventoryGuid,
                                        tblGearInventories.InventoryName,
                                        tblGearInventories.DateConducted
                                    FROM tblAOI INNER JOIN
                                        tblGearInventories ON
                                        tblAOI.AOIGuid = tblGearInventories.TargetArea
                                    WHERE tblAOI.AOIName= ""{targetAreaName}"" ";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        list.Add((dr["InventoryGuid"].ToString(), dr["InventoryName"].ToString(), (DateTime)dr["DateConducted"]));
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
            return list;
        }

        public Dictionary<string, (string Province, string Municipality, string MunicipalityNumber, string barangay, string sitio)> BarangayInventories
        {
            get { return _barangayInventories; }
        }

        public Dictionary<string, (string InventoryName, DateTime DateConducted, string TargetArea)> Inventories
        {
            get { return _inventories; }
        }

        /// <summary>
        /// Gets the corresponding guid of the fishery inventory conducted in a sitio
        /// </summary>
        /// <param name="inventoryGuid"></param>
        /// <param name="province"></param>
        /// <param name="municipality"></param>
        /// <param name="barangay"></param>
        /// <param name="sitio"></param>
        /// <returns></returns>
        public string GetBarangayInventoryGuid(string inventoryGuid, string province, string municipality, string barangay, string sitio = "")
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

        /// <summary>
        /// returns a list of barangays and sitios in a municipality with corresponding numbers of gears used in the barangay-sitio
        /// </summary>
        /// <param name="inventoryGuid"></param>
        /// <param name="provinceName"></param>
        /// <param name="municipalityName"></param>
        /// <returns></returns>
        public List<(string barangay, string sitio, string gearClass, string gearVariation, string dataGuid, string barangayInventoryGUID, int total)>
                GetBarangaysGearInventory(string inventoryGuid, string provinceName, string municipalityName)
        {
            var rowsReturned = 0;
            List<string> listedItems = new List<string>();

            List<(string barangay, string sitio, string gearClass, string gearVariation, string dataGuid, string barangayInventoryGUID, int total)> list =
                new List<(string barangay, string sitio, string gearClass, string gearVariation, string dataGuid, string barangayInventoryGUID, int total)>();

            var sql = $@"SELECT tblGearInventoryBarangay.Barangay, tblGearInventoryBarangay.Sitio, tblGearClass.GearClassName,
                           tblGearVariations.Variation, tblGearInventoryBarangay.BarangayInventoryGuid, t2.DataGuid,
                           Sum([t2].[CountCommercial]+[t2].[CountMunicipalMotorized]+[t2].[CountMunicipalNonMotorized]+[t2].[CountNoBoat]) AS Total
                        FROM (tblGearClass INNER JOIN tblGearVariations ON tblGearClass.GearClass = tblGearVariations.GearClass)
                            INNER JOIN (((Provinces INNER JOIN Municipalities ON Provinces.ProvNo = Municipalities.ProvNo)
                            INNER JOIN tblGearInventoryBarangay ON Municipalities.MunNo = tblGearInventoryBarangay.Municipality)
                            INNER JOIN tblGearInventoryBarangayData AS t2 ON tblGearInventoryBarangay.BarangayInventoryGuid = t2.BarangayInventoryGUID)
                            ON tblGearVariations.GearVarGUID = t2.GearVariation
                        WHERE tblGearInventoryBarangay.InventoryGuid={{{inventoryGuid}}}
                            AND Provinces.ProvinceName='{provinceName}' AND Municipalities.Municipality='{municipalityName}'
                        GROUP BY tblGearInventoryBarangay.Barangay, tblGearInventoryBarangay.Sitio,
                            tblGearClass.GearClassName, tblGearVariations.Variation, tblGearInventoryBarangay.BarangayInventoryGuid, t2.DataGuid
                        ORDER BY tblGearInventoryBarangay.Barangay, tblGearInventoryBarangay.Sitio, tblGearClass.GearClassName, tblGearVariations.Variation";
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                var dt = new DataTable();

                try
                {
                    conection.Open();
                    var adapter = new OleDbDataAdapter(sql, conection);
                    adapter.Fill(dt);
                    rowsReturned = dt.Rows.Count;
                    for (int n = 0; n < dt.Rows.Count; n++)
                    {
                        DataRow dr = dt.Rows[n];
                        listedItems.Add(dr["Barangay"].ToString() + dr["Sitio"].ToString());
                        list.Add((dr["Barangay"].ToString(), dr["Sitio"].ToString(), dr["GearClassName"].ToString(),
                                  dr["Variation"].ToString(), dr["DataGuid"].ToString(), dr["BarangayInventoryGuid"].ToString(), int.Parse(dr["Total"].ToString())));
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }

            sql = $@"SELECT tblGearInventoryBarangay.Barangay, tblGearInventoryBarangay.Sitio, tblGearInventoryBarangay.BarangayInventoryGuid,
                    Sum([t2].[CountCommercial]+[t2].[CountMunicipalMotorized]+[t2].[CountMunicipalNonMotorized]+[t2].[CountNoBoat]) AS Total
                    FROM ((Provinces INNER JOIN Municipalities ON Provinces.ProvNo = Municipalities.ProvNo) INNER JOIN tblGearInventoryBarangay ON Municipalities.MunNo = tblGearInventoryBarangay.Municipality) LEFT JOIN tblGearInventoryBarangayData AS t2 ON tblGearInventoryBarangay.BarangayInventoryGuid = t2.BarangayInventoryGUID
                        WHERE tblGearInventoryBarangay.InventoryGuid={{{inventoryGuid}}} AND
                        Provinces.ProvinceName='{provinceName}' AND Municipalities.Municipality='{municipalityName}'
                    GROUP BY tblGearInventoryBarangay.Barangay, tblGearInventoryBarangay.Sitio ,tblGearInventoryBarangay.BarangayInventoryGuid
                    ORDER BY tblGearInventoryBarangay.Barangay, tblGearInventoryBarangay.Sitio";

            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                var dt = new DataTable();

                try
                {
                    conection.Open();
                    var adapter = new OleDbDataAdapter(sql, conection);
                    adapter.Fill(dt);
                    for (int n = 0; n < dt.Rows.Count; n++)
                    {
                        DataRow dr = dt.Rows[n];
                        if (!listedItems.Contains(dr["Barangay"].ToString() + dr["Sitio"].ToString()))
                        {
                            list.Add((dr["Barangay"].ToString(), dr["Sitio"].ToString(), "", "", "", dr["BarangayInventoryGuid"].ToString(), 0));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }

            return list;
        }

        public bool IsGearInInventory(string barangayInventoryGuid, string gearVariationGuid)
        {
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT tblGearInventoryBarangayData.GearVariation
                                      FROM tblGearInventoryBarangayData
                                      WHERE tblGearInventoryBarangayData.BarangayInventoryGUID= {{{barangayInventoryGuid}}}";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);

                    for (int n = 0; n < dt.Rows.Count; n++)
                    {
                        DataRow dr = dt.Rows[n];
                        if (dr["GearVariation"].ToString() == gearVariationGuid)
                        {
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
            return false;
        }

        public (string province, string municipality, string barangay, string sitio) GetMunicipalityBrangaySitioFromBarangayInventory(string barangayInventory)
        {
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT Provinces.ProvinceName,
                                        Municipalities.Municipality AS MunicipalityName,
                                        tblGearInventoryBarangay.Barangay,
                                        tblGearInventoryBarangay.Sitio
                                    FROM Provinces INNER JOIN (Municipalities INNER JOIN tblGearInventoryBarangay ON
                                        Municipalities.MunNo = tblGearInventoryBarangay.Municipality) ON
                                        Provinces.ProvNo = Municipalities.ProvNo
                                    WHERE tblGearInventoryBarangay.BarangayInventoryGuid = {{{barangayInventory}}}";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);

                    DataRow dr = dt.Rows[0];
                    return (dr["ProvinceName"].ToString(),
                            dr["MunicipalityName"].ToString(),
                            dr["Barangay"].ToString(),
                            dr["Sitio"].ToString());
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
            return ("", "", "", "");
        }

        public (string province, string municipality, string MunicipalityNumber, string barangay, string sitio) GetMunicipalityBrangaySitioFromGearInventory(string gearLevelInventory)
        {
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT Provinces.ProvinceName,
                                        Municipalities.Municipality AS MunicipalityName,
                                        tblGearInventoryBarangay.Municipality AS MunicipalityNumber,
                                        tblGearInventoryBarangay.Barangay,
                                        tblGearInventoryBarangay.Sitio
                                    FROM Provinces INNER JOIN (Municipalities INNER JOIN
                                        (tblGearInventoryBarangay INNER JOIN tblGearInventoryBarangayData ON
                                        tblGearInventoryBarangay.BarangayInventoryGuid = tblGearInventoryBarangayData.BarangayInventoryGUID) ON
                                        Municipalities.MunNo = tblGearInventoryBarangay.Municipality) ON
                                        Provinces.ProvNo = Municipalities.ProvNo
                                    WHERE tblGearInventoryBarangayData.DataGuid={{{gearLevelInventory}}}";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);

                    DataRow dr = dt.Rows[0];
                    return (dr["ProvinceName"].ToString(),
                            dr["MunicipalityName"].ToString(),
                            dr["MunicipalityNumber"].ToString(),
                            dr["Barangay"].ToString(),
                            dr["Sitio"].ToString());
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
            return ("", "", "", "", "");
        }

        public Task<int> ImportInventoryAsync(string fileName)
        {
            return Task.Run(() => ImportInventory(fileName));
        }

        private int ImportInventory(string fileName)
        {
            var elementCounter = 0;
            var proceed = false;
            bool cancel = false;
            switch (Path.GetExtension(fileName))
            {
                case ".xml":
                case ".XML":
                    var projectName = "";
                    DateTime projectDate = DateTime.Now;
                    var projectTargetArea = "";
                    var projectTargetAreaGuid = "";
                    var projectGuid = "";
                    XmlTextReader xmlReader = new XmlTextReader(fileName);
                    while ((elementCounter == 0 || (elementCounter > 0 && proceed)) && xmlReader.Read())
                    {
                        switch (xmlReader.NodeType)
                        {
                            case XmlNodeType.Element:
                                if (elementCounter == 0 && xmlReader.Name == "FisherVesselGearInventoryProject")
                                {
                                    projectGuid = xmlReader.GetAttribute("ProjectGuid");
                                    projectName = xmlReader.GetAttribute("ProjectName");
                                    FisheriesInventoryImportEventArg e = new FisheriesInventoryImportEventArg(projectName, projectGuid, FisheriesInventoryLevel.Project);
                                    InventoryLevel?.Invoke(this, e);
                                    cancel = e.Cancel;
                                    if (!cancel)
                                    {
                                        projectDate = DateTime.Parse(xmlReader.GetAttribute("DateStart"));
                                        projectTargetArea = xmlReader.GetAttribute("TargetArea");
                                        projectTargetAreaGuid = xmlReader.GetAttribute("TargetAreaGuid");
                                        InventoryLevel?.Invoke(this, new FisheriesInventoryImportEventArg(projectTargetArea, projectTargetAreaGuid, FisheriesInventoryLevel.TargetArea));

                                        proceed = true;
                                        elementCounter++;
                                    }
                                    else
                                    {
                                        e.CancelReason = $@"Target area does not have inventory project with a name of ""{projectName}"" ";
                                        InventoryLevel?.Invoke(this, e);
                                    }
                                }
                                if (!cancel && xmlReader.Name == "FisherVesselInventory")
                                {
                                    if (int.TryParse(xmlReader.GetAttribute("fishBaseSpNo"), out int spNo))
                                    {
                                    }
                                    elementCounter++;
                                }

                                break;
                        }
                    }
                    break;
            }
            return 0;
        }

        public (string gearClass, string gearVariation, Dictionary<string, string> gearLocalNames,
            int commercialCount, int motorizedCount, int nonMotorizedCount, int noBoatCount,
            List<int> monthsInUse, List<int> peakMonths, int numberDaysGearUsedPerMonth,
            int cpueRangeMax, int cpueRangeMin, int cpueModeUpper, int cpueModeLower, string cpueUnit,
            List<(int decade, int cpue, string unit)> historicalCPUE,
            Dictionary<string, string> dominantCatch, Dictionary<string, string> nonDominantCatch, int percentageOfDominance,
            string notes, List<string> accessories, List<(string expense, double cost, string source, string notes)> expenses)
          GetGearVariationInventoryDataEx(string gearInventoryGuid)
        {
            Dictionary<string, string> gearLocalNames = new Dictionary<string, string>();
            List<int> monthsInUse = new List<int>();
            List<int> peakMonths = new List<int>();
            List<(int decade, int cpue, string unit)> historicalCPUE = new List<(int decade, int cpue, string unit)>();
            Dictionary<string, string> dominantCatch = new Dictionary<string, string>();
            Dictionary<string, string> nonDominantCatch = new Dictionary<string, string>();
            List<string> accessories = new List<string>();
            List<(string expense, double cost, string source, string notes)> expenses = new List<(string expense, double cost, string spurce, string notes)>();
            string notes = "";
            string gearClass = "";
            string gearVariation = "";
            int commercialCount = 0;
            int motorizedCount = 0;
            int nonMotorizedCount = 0;
            int noBoatCount = 0;
            int numberDaysGearUsedPerMonth = 0;
            int cpueRangeMax = 0;
            int cpueRangeMin = 0;
            int cpueModeUpper = 0;
            int cpueModeLower = 0;
            string cpueUnit = "";
            int percentageOfDominance = 0;

            //first, we get the main inventory data of the fishing gear
            string sql = $@"SELECT tblGearClass.GearClassName,
                            tblGearVariations.Variation,
                            tblGearInventoryBarangayData.*
                        FROM (tblGearClass INNER JOIN
                            tblGearVariations ON
                            tblGearClass.GearClass = tblGearVariations.GearClass)
                            INNER JOIN tblGearInventoryBarangayData ON
                            tblGearVariations.GearVarGUID = tblGearInventoryBarangayData.GearVariation
                        WHERE tblGearInventoryBarangayData.DataGuid={{{gearInventoryGuid}}}";

            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    var adapter = new OleDbDataAdapter(sql, conection);
                    adapter.Fill(dt);
                    DataRow dr = dt.Rows[0];
                    gearClass = dr["GearClassName"].ToString();
                    gearVariation = dr["Variation"].ToString();
                    commercialCount = (int)dr["CountCommercial"];
                    motorizedCount = (int)dr["CountMunicipalMotorized"];
                    nonMotorizedCount = (int)dr["CountMunicipalNonMotorized"];
                    noBoatCount = (int)dr["CountNoBoat"];
                    numberDaysGearUsedPerMonth = (int)dr["NumberDaysPerMonth"];
                    cpueRangeMax = (int)dr["MaxCPUE"];
                    cpueRangeMin = (int)dr["MinCPUE"];
                    cpueModeUpper = (int)dr["ModeUpper"];
                    cpueModeLower = (int)dr["ModeLower"];
                    cpueUnit = dr["CPUEUnit"].ToString();
                    percentageOfDominance = (int)dr["DominantCatchPercent"];
                    notes = dr["Notes"].ToString();
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }

            //second, we get the local names of the fishing gear and put the names in a list
            sql = $@"SELECT tblGearLocalNames.LocalName,
                        tblGearLocalNames.LocalNameGUID
                    FROM tblGearLocalNames INNER JOIN
                        tblGearInventoryGearLocalNames ON
                        tblGearLocalNames.LocalNameGUID = tblGearInventoryGearLocalNames.LocalNameGuid
                    WHERE tblGearInventoryGearLocalNames.InventoryDataGuid = {{{gearInventoryGuid}}}";
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
                        gearLocalNames.Add(dr["LocalNameGUID"].ToString(), dr["LocalName"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }

            //next, we get the months of use and peak months
            sql = $@"SELECT tblGearInventoryMonthsUsed.MonthNumber
                    FROM tblGearInventoryMonthsUsed
                    WHERE tblGearInventoryMonthsUsed.InventoryDataGuid={{{gearInventoryGuid}}}
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

            //peak months
            sql = $@"SELECT tblGearInventoryPeakMonths.PeakSeasonMonthNumber
                    FROM tblGearInventoryPeakMonths
                    WHERE tblGearInventoryPeakMonths.InventoryDataGuid={{{gearInventoryGuid}}}
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

            //next, we get the historical cpue
            sql = $@"SELECT tblGearInventoryCPUEHistorical.Decade, tblGearInventoryCPUEHistorical.CPUE, tblGearInventoryCPUEHistorical.CPUEUnit
                    FROM tblGearInventoryCPUEHistorical
                    WHERE tblGearInventoryCPUEHistorical.InventoryDataGuid={{{gearInventoryGuid}}}
                    ORDER BY tblGearInventoryCPUEHistorical.Decade DESC;";
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
                        historicalCPUE.Add(((int)dr["Decade"], (int)dr["CPUE"], (string)dr["CPUEUnit"]));
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }

            //finally, we get the catch composition, we put dominant catch in one list
            sql = $@"SELECT tblBaseLocalNames.Name,tblBaseLocalNames.NameNo
                    FROM tblBaseLocalNames INNER JOIN tblGearInventoryCatchComposition ON
                      tblBaseLocalNames.NameNo = tblGearInventoryCatchComposition.NameOfCatch
                    WHERE tblGearInventoryCatchComposition.InventoryDataGuid={{{gearInventoryGuid}}} AND
                      tblGearInventoryCatchComposition.IsDominant=True ORDER BY tblBaseLocalNames.Name";
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
                        dominantCatch.Add(dr["NameNo"].ToString(), dr["Name"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }

            //and we put non-dominant catch in another list
            sql = $@"SELECT tblBaseLocalNames.Name, tblBaseLocalNames.NameNo
                    FROM tblBaseLocalNames INNER JOIN tblGearInventoryCatchComposition ON
                      tblBaseLocalNames.NameNo = tblGearInventoryCatchComposition.NameOfCatch
                    WHERE tblGearInventoryCatchComposition.InventoryDataGuid={{{gearInventoryGuid}}} AND
                      tblGearInventoryCatchComposition.IsDominant=False ORDER BY tblBaseLocalNames.Name";
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
                        nonDominantCatch.Add(dr["NameNo"].ToString(), dr["Name"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }

            //get list of accessories
            sql = $@"SELECT Accessory
                    FROM tblGearInventoryAccesories
                    WHERE InventoryDataGuid={{{gearInventoryGuid}}}
                    ORDER BY Accessory";
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
                        accessories.Add(dr["Accessory"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }

            //get list of expenses
            sql = $@"SELECT ExpenseItem,
                        Cost,
                        Source,
                        Notes
                    FROM tblGearInventoryExpense
                    WHERE InventoryDataGuid={{{gearInventoryGuid}}}
                    ORDER BY ExpenseItem";
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
                        expenses.Add((dr["ExpenseItem"].ToString(), (double)dr["Cost"], dr["Source"].ToString(), dr["Notes"].ToString()));
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }

            //we return all the values in a value tuple, hooray very convenient indeed.
            return (gearClass, gearVariation, gearLocalNames, commercialCount, motorizedCount, nonMotorizedCount,
                    noBoatCount, monthsInUse, peakMonths, numberDaysGearUsedPerMonth,
                    cpueRangeMax, cpueRangeMin, cpueModeUpper, cpueModeLower, cpueUnit, historicalCPUE,
                    dominantCatch, nonDominantCatch, percentageOfDominance, notes, accessories, expenses);
        }

        /// <summary>
        /// Gets all the associated data of an inventoried fishing gear in a sitio
        /// </summary>
        /// <param name="inventoryGuid"></param>
        /// <returns></returns>
        public (string inventoryName, DateTime dateConducted, string targetArea, string province, string municipality, string barangay, string sitio,
            string gearClass, string gearVariation, List<string> gearLocalNames,
            int commercialCount, int motorizedCount, int nonMotorizedCount, int noBoatCount,
            List<int> monthsInUse, List<int> peakMonths, int numberDaysGearUsedPerMonth,
            int cpueRangeMax, int cpueRangeMin, int cpueModeUpper, int cpueModeLower, string cpueUnit,
            List<(int decade, int cpue, string unit)> historicalCPUE,
            List<string> dominantCatch, List<string> nonDominantCatch, int percentageOfDominance,
            string notes, List<string> accessories, List<(string expense, double cost, string source, string notes)> expenses) GetGearVariationInventoryData(string inventoryGuid)
        {
            List<string> gearLocalNames = new List<string>();
            List<int> monthsInUse = new List<int>();
            List<int> peakMonths = new List<int>();
            List<(int decade, int cpue, string unit)> historicalCPUE = new List<(int decade, int cpue, string unit)>();
            List<string> dominantCatch = new List<string>();
            List<string> nonDominantCatch = new List<string>();
            List<string> accessories = new List<string>();
            List<(string expense, double cost, string source, string notes)> expenses = new List<(string expense, double cost, string spurce, string notes)>();
            string notes = "";
            string inventoryName = "";
            DateTime dateConducted = DateTime.Now;
            string targetArea = "";
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
            int numberDaysGearUsedPerMonth = 0;
            int cpueRangeMax = 0;
            int cpueRangeMin = 0;
            int cpueModeUpper = 0;
            int cpueModeLower = 0;
            string cpueUnit = "";
            int percentageOfDominance = 0;

            //first, we get the main inventory data of the fishing gear
            string sql = $@"SELECT tblGearInventories.InventoryName, tblGearInventories.DateConducted, tblAOI.AOIName, Provinces.ProvinceName,
                            Municipalities.Municipality, tblGearInventoryBarangay.Barangay, tblGearInventoryBarangay.Sitio, tblGearClass.GearClassName,
                            tblGearVariations.Variation, tblGearInventoryBarangayData.*
                            FROM tblAOI INNER JOIN (tblGearInventories INNER JOIN ((tblGearClass INNER JOIN tblGearVariations ON
                              tblGearClass.GearClass = tblGearVariations.GearClass) INNER JOIN (Provinces INNER JOIN (Municipalities INNER JOIN
                              (tblGearInventoryBarangay INNER JOIN tblGearInventoryBarangayData ON
                              tblGearInventoryBarangay.BarangayInventoryGuid = tblGearInventoryBarangayData.BarangayInventoryGUID) ON
                              Municipalities.MunNo = tblGearInventoryBarangay.Municipality) ON Provinces.ProvNo = Municipalities.ProvNo) ON
                              tblGearVariations.GearVarGUID = tblGearInventoryBarangayData.GearVariation) ON
                              tblGearInventories.InventoryGuid = tblGearInventoryBarangay.InventoryGuid) ON
                              tblAOI.AOIGuid = tblGearInventories.TargetArea
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
                    inventoryName = dr["InventoryName"].ToString();
                    dateConducted = (DateTime)dr["DateConducted"];
                    targetArea = dr["AOIName"].ToString();
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
                    numberDaysGearUsedPerMonth = (int)dr["NumberDaysPerMonth"];
                    cpueRangeMax = (int)dr["MaxCPUE"];
                    cpueRangeMin = (int)dr["MinCPUE"];
                    cpueModeUpper = (int)dr["ModeUpper"];
                    cpueModeLower = (int)dr["ModeLower"];
                    cpueUnit = dr["CPUEUnit"].ToString();
                    percentageOfDominance = (int)dr["DominantCatchPercent"];
                    notes = dr["Notes"].ToString();
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }

            //second, we get the local names of the fishing gear and put the names in a list
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

            //next, we get the months of use and peak months
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
            //peak months
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

            //next, we get the historical cpue
            sql = $@"SELECT tblGearInventoryCPUEHistorical.Decade, tblGearInventoryCPUEHistorical.CPUE, tblGearInventoryCPUEHistorical.CPUEUnit
                    FROM tblGearInventoryCPUEHistorical
                    WHERE tblGearInventoryCPUEHistorical.InventoryDataGuid={{{inventoryGuid}}}
                    ORDER BY tblGearInventoryCPUEHistorical.Decade DESC;";
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
                        historicalCPUE.Add(((int)dr["Decade"], (int)dr["CPUE"], (string)dr["CPUEUnit"]));
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }

            //we get the catch composition, we put dominant catch in one list
            sql = $@"SELECT tblBaseLocalNames.Name
                    FROM tblBaseLocalNames INNER JOIN tblGearInventoryCatchComposition ON
                      tblBaseLocalNames.NameNo = tblGearInventoryCatchComposition.NameOfCatch
                    WHERE tblGearInventoryCatchComposition.InventoryDataGuid={{{inventoryGuid}}} AND
                      tblGearInventoryCatchComposition.IsDominant=True ORDER BY tblBaseLocalNames.Name";
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
                        dominantCatch.Add(dr["Name"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
            //and we put non-dominant catch in another list
            sql = $@"SELECT tblBaseLocalNames.Name
                    FROM tblBaseLocalNames INNER JOIN tblGearInventoryCatchComposition ON
                      tblBaseLocalNames.NameNo = tblGearInventoryCatchComposition.NameOfCatch
                    WHERE tblGearInventoryCatchComposition.InventoryDataGuid={{{inventoryGuid}}} AND
                      tblGearInventoryCatchComposition.IsDominant=False ORDER BY tblBaseLocalNames.Name";
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
                        nonDominantCatch.Add(dr["Name"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }

            //get list of accessories
            sql = $@"SELECT Accessory
                    FROM tblGearInventoryAccesories
                    WHERE InventoryDataGuid={{{inventoryGuid}}}
                    ORDER BY Accessory";
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
                        accessories.Add(dr["Accessory"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }

            //get list of expenses
            sql = $@"SELECT ExpenseItem,
                        Cost,
                        Source,
                        Notes
                    FROM tblGearInventoryExpense
                    WHERE InventoryDataGuid={{{inventoryGuid}}}
                    ORDER BY ExpenseItem";
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
                        expenses.Add((dr["ExpenseItem"].ToString(), (double)dr["Cost"], dr["Source"].ToString(), dr["Notes"].ToString()));
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }

            //we return all the values in a value tuple, hooray very convenient indeed.
            return (inventoryName, dateConducted, targetArea, province, municipality, barangay, sitio,
                    gearClass, gearVariation, gearLocalNames, commercialCount, motorizedCount, nonMotorizedCount,
                    noBoatCount, monthsInUse, peakMonths, numberDaysGearUsedPerMonth,
                    cpueRangeMax, cpueRangeMin, cpueModeUpper, cpueModeLower, cpueUnit, historicalCPUE,
                    dominantCatch, nonDominantCatch, percentageOfDominance, notes, accessories, expenses);
        }

        /// <summary>
        /// Gets the details of a fishery inventory project
        /// </summary>
        /// <param name="inventoryGUID"></param>
        /// <returns></returns>
        public (string inventoryName, DateTime? dateImplemented) GetInventory(string inventoryGUID)
        {
            //_inventories.Clear();
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

        /// <summary>
        /// gets the sum of fishing gear distribution among fishing vessel types for gears inventoried in a government unit (either province, municipality, brgy or sitio)
        /// </summary>
        /// <param name="inventoryGuid"></param>
        /// <param name="provinceName"></param>
        /// <param name="municipalityName"></param>
        /// <param name="barangayName"></param>
        /// <param name="sitioName"></param>
        /// <returns></returns>
        public Dictionary<string, (string gearClass, int total, int sumCommercial, int sumMotorized, int sumNonMotorized, int sumNoBoat)> GetLevelGearsInventory(string inventoryGuid, string provinceName = "", string municipalityName = "", string barangayName = "", string sitioName = "")
        {
            Dictionary<string, (string gearClass, int total, int sumCommercial, int sumMotorized, int sumNonMotorized, int sumNoBoat)> dict = new Dictionary<string, (string gearClass, int total, int sumCommercial, int sumMotorized, int sumNonMotorized, int sumNoBoat)>();

            var dt = new DataTable();
            string query = "";
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    if (sitioName.Length > 0)
                    {
                        if (sitioName == "entireBarangay")
                        {
                            sitioName = "";
                        }
                        query = $@"SELECT tblGearClass.GearClassName, tblGearVariations.Variation,
                                Sum([t2].[CountCommercial]+[t2].[CountMunicipalMotorized]+[t2].[CountMunicipalNonMotorized]+[t2].[CountNoBoat]) AS Total,
                                Sum(t2.CountCommercial) AS TotalCommercial,
                                Sum(t2.CountMunicipalMotorized) AS TotalMotorized,
                                Sum(t2.CountMunicipalNonMotorized) AS TotalNonMotorized,
                                Sum(t2.CountNoBoat) AS TotalNoBoat
                                FROM tblGearClass INNER JOIN (tblGearVariations INNER JOIN (((Provinces INNER JOIN Municipalities ON
                                    Provinces.ProvNo = Municipalities.ProvNo) INNER JOIN tblGearInventoryBarangay ON
                                    Municipalities.MunNo = tblGearInventoryBarangay.Municipality) INNER JOIN tblGearInventoryBarangayData AS t2 ON
                                    tblGearInventoryBarangay.BarangayInventoryGuid = t2.BarangayInventoryGUID) ON
                                    tblGearVariations.GearVarGUID = t2.GearVariation) ON
                                    tblGearClass.GearClass = tblGearVariations.GearClass
                                WHERE tblGearInventoryBarangay.InventoryGuid={{{inventoryGuid}}} AND Provinces.ProvinceName='{provinceName}'
                                    AND Municipalities.Municipality= '{municipalityName}' AND tblGearInventoryBarangay.Barangay='{barangayName}'
                                    AND tblGearInventoryBarangay.Sitio='{sitioName}'
                                GROUP BY tblGearClass.GearClassName, tblGearVariations.Variation
                                ORDER BY tblGearClass.GearClassName, tblGearVariations.Variation;";
                    }
                    else if (barangayName.Length > 0)
                    {
                        query = $@"SELECT tblGearClass.GearClassName, tblGearVariations.Variation,
                                Sum([t2].[CountCommercial]+[t2].[CountMunicipalMotorized]+[t2].[CountMunicipalNonMotorized]+[t2].[CountNoBoat]) AS Total,
                                Sum(t2.CountCommercial) AS TotalCommercial,
                                Sum(t2.CountMunicipalMotorized) AS TotalMotorized,
                                Sum(t2.CountMunicipalNonMotorized) AS TotalNonMotorized,
                                Sum(t2.CountNoBoat) AS TotalNoBoat
                                FROM tblGearClass INNER JOIN (tblGearVariations INNER JOIN (((Provinces INNER JOIN Municipalities ON
                                    Provinces.ProvNo = Municipalities.ProvNo) INNER JOIN tblGearInventoryBarangay ON
                                    Municipalities.MunNo = tblGearInventoryBarangay.Municipality) INNER JOIN tblGearInventoryBarangayData AS t2 ON
                                    tblGearInventoryBarangay.BarangayInventoryGuid = t2.BarangayInventoryGUID) ON
                                    tblGearVariations.GearVarGUID = t2.GearVariation) ON
                                    tblGearClass.GearClass = tblGearVariations.GearClass
                                WHERE tblGearInventoryBarangay.InventoryGuid={{{inventoryGuid}}} AND Provinces.ProvinceName='{provinceName}'
                                    AND Municipalities.Municipality= '{municipalityName}' AND tblGearInventoryBarangay.Barangay='{barangayName}'
                                GROUP BY tblGearClass.GearClassName, tblGearVariations.Variation
                                ORDER BY tblGearClass.GearClassName, tblGearVariations.Variation;";
                    }
                    else if (municipalityName.Length > 0)
                    {
                        query = $@"SELECT tblGearClass.GearClassName, tblGearVariations.Variation,
                                Sum([t2].[CountCommercial]+[t2].[CountMunicipalMotorized]+[t2].[CountMunicipalNonMotorized]+[t2].[CountNoBoat]) AS Total,
                                Sum(t2.CountCommercial) AS TotalCommercial,
                                Sum(t2.CountMunicipalMotorized) AS TotalMotorized,
                                Sum(t2.CountMunicipalNonMotorized) AS TotalNonMotorized,
                                Sum(t2.CountNoBoat) AS TotalNoBoat
                                FROM tblGearClass INNER JOIN (tblGearVariations INNER JOIN (((Provinces INNER JOIN Municipalities ON
                                    Provinces.ProvNo = Municipalities.ProvNo) INNER JOIN tblGearInventoryBarangay ON
                                    Municipalities.MunNo = tblGearInventoryBarangay.Municipality) INNER JOIN tblGearInventoryBarangayData AS t2 ON
                                    tblGearInventoryBarangay.BarangayInventoryGuid = t2.BarangayInventoryGUID) ON
                                    tblGearVariations.GearVarGUID = t2.GearVariation) ON
                                    tblGearClass.GearClass = tblGearVariations.GearClass
                                WHERE tblGearInventoryBarangay.InventoryGuid={{{inventoryGuid}}} AND Provinces.ProvinceName='{provinceName}'  AND Municipalities.Municipality= '{municipalityName}'
                                GROUP BY tblGearClass.GearClassName, tblGearVariations.Variation
                                ORDER BY tblGearClass.GearClassName, tblGearVariations.Variation;";
                    }
                    else if (provinceName.Length > 0)
                    {
                        query = $@"SELECT tblGearClass.GearClassName, tblGearVariations.Variation,
                                Sum([t2].[CountCommercial]+[t2].[CountMunicipalMotorized]+[t2].[CountMunicipalNonMotorized]+[t2].[CountNoBoat]) AS Total,
                                Sum(t2.CountCommercial) AS TotalCommercial,
                                Sum(t2.CountMunicipalMotorized) AS TotalMotorized,
                                Sum(t2.CountMunicipalNonMotorized) AS TotalNonMotorized,
                                Sum(t2.CountNoBoat) AS TotalNoBoat
                                FROM tblGearClass INNER JOIN (tblGearVariations INNER JOIN (((Provinces INNER JOIN Municipalities ON
                                    Provinces.ProvNo = Municipalities.ProvNo) INNER JOIN tblGearInventoryBarangay ON
                                    Municipalities.MunNo = tblGearInventoryBarangay.Municipality) INNER JOIN tblGearInventoryBarangayData AS t2 ON
                                    tblGearInventoryBarangay.BarangayInventoryGuid = t2.BarangayInventoryGUID) ON
                                    tblGearVariations.GearVarGUID = t2.GearVariation) ON
                                    tblGearClass.GearClass = tblGearVariations.GearClass
                                WHERE tblGearInventoryBarangay.InventoryGuid={{{inventoryGuid}}} AND Provinces.ProvinceName='{provinceName}'
                                GROUP BY tblGearClass.GearClassName, tblGearVariations.Variation
                                ORDER BY tblGearClass.GearClassName, tblGearVariations.Variation;";
                    }
                    else
                    {
                        query = $@"SELECT tblGearClass.GearClassName, tblGearVariations.Variation,
                                Sum([t2].[CountCommercial]+[t2].[CountMunicipalMotorized]+[t2].[CountMunicipalNonMotorized]+[t2].[CountNoBoat]) AS Total,
                                Sum(t2.CountCommercial) AS TotalCommercial,
                                Sum(t2.CountMunicipalMotorized) AS TotalMotorized,
                                Sum(t2.CountMunicipalNonMotorized) AS TotalNonMotorized,
                                Sum(t2.CountNoBoat) AS TotalNoBoat
                                FROM tblGearClass INNER JOIN (tblGearVariations INNER JOIN (((Provinces INNER JOIN Municipalities ON
                                    Provinces.ProvNo = Municipalities.ProvNo) INNER JOIN tblGearInventoryBarangay ON
                                    Municipalities.MunNo = tblGearInventoryBarangay.Municipality) INNER JOIN tblGearInventoryBarangayData AS t2 ON
                                    tblGearInventoryBarangay.BarangayInventoryGuid = t2.BarangayInventoryGUID) ON
                                    tblGearVariations.GearVarGUID = t2.GearVariation) ON
                                    tblGearClass.GearClass = tblGearVariations.GearClass
                                WHERE tblGearInventoryBarangay.InventoryGuid={{{inventoryGuid}}}
                                GROUP BY tblGearClass.GearClassName, tblGearVariations.Variation
                                ORDER BY tblGearClass.GearClassName, tblGearVariations.Variation;";
                    }
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);

                    for (int n = 0; n < dt.Rows.Count; n++)
                    {
                        DataRow dr = dt.Rows[n];
                        dict.Add(dr["Variation"].ToString(), (dr["GearClassName"].ToString(), int.Parse(dr["Total"].ToString()), int.Parse(dr["TotalCommercial"].ToString()), int.Parse(dr["TotalMotorized"].ToString()), int.Parse(dr["TotalNonMotorized"].ToString()), int.Parse(dr["TotalNoBoat"].ToString())));
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }

            return dict;
        }

        /// <summary>
        /// returns sums of fishers and fishig vessel types in a government unit (either province, municipality, brgy, or sitio)
        /// </summary>
        /// <param name="inventoryGuid"></param>
        /// <param name="provinceName"></param>
        /// <param name="municipalityName"></param>
        /// <param name="barangayName"></param>
        /// <returns></returns>
        public (int totalFishers, int totalMotorized, int totalNonMotorized, int totalCommercial) GetLevelSummary(string inventoryGuid, string provinceName = "", string municipalityName = "", string barangayName = "")
        {
            (int totalFishers, int totalMotorized, int totalNonMotorized, int totalCommercial) summary;
            summary.totalFishers = 0;
            summary.totalCommercial = 0;
            summary.totalMotorized = 0;
            summary.totalNonMotorized = 0;
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

        /// <summary>
        /// returns number of fishers and number of fishing vessels in a sitio
        /// </summary>
        /// <param name="inventoryGuid"></param>
        /// <param name="province"></param>
        /// <param name="municipality"></param>
        /// <param name="barangay"></param>
        /// <param name="sitio"></param>
        /// <returns></returns>
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

        public (int fisherCount, int commercialCount, int motorizedCount, int nonMotorizedCount) GetSitioNumbers(string barangayInventoryGuid)
        {
            (int fisherCount, int commercialCount, int motorizedCount, int nonMotorizedCount) numbers = (0, 0, 0, 0);
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT CountFishers,
                                        CountMunicipalMotorized,
                                        CountMunicipalNonMotorized,
                                        CountCommercial
                                    FROM tblGearInventoryBarangay
                                    WHERE BarangayInventoryGuid={{{barangayInventoryGuid}}}";

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

        /// <summary>
        /// returns count of barangays in a municipality
        /// </summary>
        /// <param name="inventoryGuid"></param>
        /// <param name="provinceName"></param>
        /// <param name="municipalityName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// returns number of municipalities and barangays included in a fishery inventory project
        /// </summary>
        /// <param name="inventoryGuid"></param>
        /// <returns></returns>
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

        /// <summary>
        /// returns number of municipalities and barangays in a province that are included in a fishery inventory
        /// </summary>
        /// <param name="inventoryGuid"></param>
        /// <param name="provinceName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// returns number of sitios in a barangay that are included in a fishery inventory
        /// </summary>
        /// <param name="inventoryGuid"></param>
        /// <param name="provinceName"></param>
        /// <param name="municipalityName"></param>
        /// <param name="barangayName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// returns a list of government units that are included in a fishery inventory, by level of government unit
        /// </summary>
        /// <param name="inventoryGuid"></param>
        public void ReadBarangayInventories(string inventoryGuid)
        {
            _barangayInventories.Clear();
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT
                                        Provinces.ProvinceName,
                                        Municipalities.Municipality,
                                        Municipalities.MunNo,
                                        tblGearInventoryBarangay.Barangay,
                                        tblGearInventoryBarangay.Sitio,
                                        tblGearInventoryBarangay.BarangayInventoryGuid
                                    FROM (Provinces INNER JOIN
                                        Municipalities ON
                                        Provinces.ProvNo = Municipalities.ProvNo) INNER JOIN
                                        tblGearInventoryBarangay ON
                                        Municipalities.MunNo = tblGearInventoryBarangay.Municipality
                                    WHERE tblGearInventoryBarangay.InventoryGuid={{{inventoryGuid}}}
                                    ORDER BY Provinces.ProvinceName, Municipalities.Municipality,
                                      tblGearInventoryBarangay.Barangay, tblGearInventoryBarangay.Sitio";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        _barangayInventories.Add(dr["BarangayInventoryGuid"].ToString(), (dr["ProvinceName"].ToString(), dr["Municipality"].ToString(), dr["MunNo"].ToString(), dr["Barangay"].ToString(), dr["Sitio"].ToString()));
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
        }

        /// <summary>
        /// returns a list of fishing gears that were inventoried in a sitio
        /// </summary>
        /// <param name="barangayInventoryGuid"></param>
        /// <returns></returns>
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

        public void RefreshInventories(TargetArea targetArea)
        {
            ReadInventoriesInTargetArea(TargetArea.TargetAreaGuid);
        }

        /// <summary>
        /// saves fishery inventory (number of fishers and number of fishing vessels) collected in a barangay
        /// </summary>
        /// <param name="dataStatus"></param>
        /// <param name="inventoryGuid"></param>
        /// <param name="municipalityNumber"></param>
        /// <param name="barangay"></param>
        /// <param name="countFishers"></param>
        /// <param name="countCommercial"></param>
        /// <param name="countMunicipalMotorized"></param>
        /// <param name="countMunicipalNonMotorized"></param>
        /// <param name="BarangayInventoryGuid"></param>
        /// <param name="sitio"></param>
        /// <returns></returns>
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
                                        Sitio = '{sitio}',
                                        CountFishers = {countFishers},
                                        CountMunicipalMotorized = {countMunicipalMotorized},
                                        CountMunicipalNonMotorized = {countMunicipalNonMotorized},
                                        CountCommercial = {countCommercial}
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

        /// <summary>
        /// Saves details of a fishery inventory project
        /// </summary>
        /// <param name="dataStatus"></param>
        /// <param name="name"></param>
        /// <param name="dateConducted"></param>
        /// <param name="inventoryGuid"></param>
        /// <returns></returns>
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
                                      {{{inventoryGuid}}}, '{name}', '{dateConducted}', {{{TargetArea.TargetAreaGuid}}})";
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

        /// <summary>
        /// Saves catch composition of a fishing gear in a sitio included in the fishery inventory
        /// </summary>
        /// <param name="sitioGearInventoryGuid"></param>
        /// <param name="dominantCatchGuid"></param>
        /// <param name="nonDominantCatchGuid"></param>
        /// <returns></returns>
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

        /// <summary>
        /// saves months of use and peak season of a fishing gear included in a fishery inventory
        /// </summary>
        /// <param name="sitioGearInventoryGuid"></param>
        /// <param name="monthsFishing"></param>
        /// <param name="monthsSeason"></param>
        /// <returns></returns>
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

        /// <summary>
        /// saves local names of a fishing gear included in a fishery inventory
        /// </summary>
        /// <param name="sitioGearInventoryGuid"></param>
        /// <param name="localNamesGuid"></param>
        /// <returns></returns>
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

        public bool SaveSitioGearInventoryAccessories(string sitioGearInventoryGuid, List<string> accessories)

        {
            int n = 0;
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conn.Open();
                    string sql = $"Delete * from tblGearInventoryAccesories where InventoryDataGuid = {{{sitioGearInventoryGuid}}}";

                    using (OleDbCommand update = new OleDbCommand(sql, conn))
                    {
                        update.ExecuteNonQuery();
                    }

                    foreach (var item in accessories)
                    {
                        sql = $@"Insert into tblGearInventoryAccesories (InventoryDataGuid, Accessory, RowGuid) values ({{{sitioGearInventoryGuid}}}, ""{item}"", {{{Guid.NewGuid().ToString()}}})";
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

        /// <summary>
        /// saves historical CPUE (by decade) of a fishing gear included in a fishery inventory
        /// </summary>
        /// <param name="sitioGearInventoryGuid"></param>
        /// <param name="cpueHistorical"></param>
        /// <returns></returns>
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

        public bool SaveSitioGearInventoryExpenses(string sitioGearInventoryGuid, List<(string expenseItem, double cost, string source, string notes)> expenseItems)
        {
            int n = 0;
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conn.Open();
                    string sql = $"Delete * from tblGearInventoryExpense where InventoryDataGuid = {{{sitioGearInventoryGuid}}}";

                    using (OleDbCommand update = new OleDbCommand(sql, conn))
                    {
                        update.ExecuteNonQuery();
                    }

                    foreach (var item in expenseItems)
                    {
                        sql = $@"Insert into tblGearInventoryExpense (
                                    InventoryDataGuid,
                                    RowGuid,
                                    ExpenseItem,
                                    Cost,
                                    Source,
                                    Notes
                                ) VALUES (
                                    {{{sitioGearInventoryGuid}}},
                                    {{{Guid.NewGuid().ToString()}}},
                                    ""{item.expenseItem}"",
                                    {item.cost},
                                    ""{item.source}"",
                                    ""{item.notes}""
                                    )";
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

        /// <summary>
        /// saves main data of a fishing gear included in a fishery inventory
        /// </summary>
        /// <param name="barangayInventoryGuid"></param>
        /// <param name="gearVariationGuid"></param>
        /// <param name="countCommercialUsage"></param>
        /// <param name="countMunicipalMotorizedUsage"></param>
        /// <param name="countMunicipalNonMotorizedUsage"></param>
        /// <param name="countNoBoatGears"></param>
        /// <param name="countDaysPerMonthUse"></param>
        /// <param name="cpueUnit"></param>
        /// <param name="percentageDominantCatch"></param>
        /// <param name="guid"></param>
        /// <param name="dataStatus"></param>
        /// <param name="rangeCPUEMax"></param>
        /// <param name="rangeCPUEMin"></param>
        /// <param name="modeCPUEUpper"></param>
        /// <param name="modeCPUELower"></param>
        /// <returns></returns>
        public bool SaveSitioGearInventoryMain(string barangayInventoryGuid, string gearVariationGuid, int countCommercialUsage,
                        int countMunicipalMotorizedUsage, int countMunicipalNonMotorizedUsage, int countNoBoatGears,
                        int countDaysPerMonthUse, string cpueUnit, int percentageDominantCatch, string guid, fad3DataStatus dataStatus,
                        int? rangeCPUEMax, int? rangeCPUEMin, int? modeCPUEUpper, int? modeCPUELower, string notes)
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
                                         ModeUpper, ModeLower, CPUEUnit, DataGuid, DominantCatchPercent,notes)
                                         values
                                         ({{{barangayInventoryGuid}}}, {{{gearVariationGuid}}},
                                          {countCommercialUsage}, {countMunicipalMotorizedUsage},
                                          {countMunicipalNonMotorizedUsage}, {countNoBoatGears},
                                          {countDaysPerMonthUse},{rangeCPUEMax}, {rangeCPUEMin},
                                          {modeCPUEUpper},{modeCPUELower},'{cpueUnit}',{{{guid}}},
                                          {percentageDominantCatch},""{notes}"")";
                    }
                    else
                    {
                        updateQuery = $@"Update tblGearInventoryBarangayData set
                                       GearVariation = {{{gearVariationGuid}}},
                                       CountCommercial = {countCommercialUsage},
                                       CountMunicipalMotorized = {countMunicipalMotorizedUsage},
                                       CountMunicipalNonMotorized = {countMunicipalNonMotorizedUsage},
                                       CountNoBoat = {countNoBoatGears},
                                       NumberDaysPerMonth = {countDaysPerMonthUse},
                                       MaxCPUE = {rangeCPUEMax},
                                       MinCPUE = {rangeCPUEMin},
                                       ModeUpper = {modeCPUEUpper},
                                       ModeLower = {modeCPUELower},
                                       CPUEUnit = ""{cpueUnit}"",
                                       DominantCatchPercent = {percentageDominantCatch},
                                       Notes = ""{notes}""
                                       WHERE DataGuid = {{{guid}}}";
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

        public List<String> GetBarangaySitios(string province, string municipality, string barangay)
        {
            var dt = new DataTable();
            List<string> sitios = new List<string>();
            using (var conection = new OleDbConnection(global.ConnectionString))

            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT tblGearInventoryBarangay.Sitio
                                    FROM Provinces INNER JOIN (Municipalities INNER JOIN tblGearInventoryBarangay ON
                                      Municipalities.MunNo = tblGearInventoryBarangay.Municipality) ON
                                      Provinces.ProvNo = Municipalities.ProvNo
                                    WHERE tblGearInventoryBarangay.Sitio<>'' AND
                                      tblGearInventoryBarangay.Barangay='{barangay}' AND
                                      Municipalities.Municipality= '{municipality}' AND
                                      Provinces.ProvinceName='{province}'";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        sitios.Add(dr["Sitio"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
            return sitios;
        }

        /// <summary>
        /// gets inventory projects found in a target area
        /// </summary>
        /// <param name="AOIGuid"></param>
        private void ReadInventoriesInTargetArea(string AOIGuid)
        {
            _inventories.Clear();
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT tblGearInventories.InventoryGuid, tblAOI.AOIName, tblGearInventories.InventoryName, tblGearInventories.DateConducted
                                      FROM tblAOI INNER JOIN tblGearInventories ON tblAOI.AOIGuid = tblGearInventories.TargetArea
                                      WHERE tblGearInventories.[TargetArea] ={{{AOIGuid}}}";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        _inventories.Add(dr["InventoryGuid"].ToString(), (dr["InventoryName"].ToString(), (DateTime)dr["DateConducted"], dr["AOIName"].ToString()));
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
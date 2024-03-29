﻿using FAD3.Database.Classes;
using MetaphoneCOM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Text;
using System.Collections;

namespace FAD3
{
    public static class Gears
    {
        public static Dictionary<string, GearClass> GearClasses { get; internal set; } = new Dictionary<string, GearClass>();

        //private static Dictionary<string, string> _gearClassEx = new Dictionary<string, string>();
        //private static Dictionary<string, string> _gearClassLetterCodes = new Dictionary<string, string>();
        private static string _gearClassUsed = "";
        private static Dictionary<string, string> _gearLocalNameListDict = new Dictionary<string, string>();
        private static Dictionary<string, string> _gearLocalNameListReverseDict = new Dictionary<string, string>();
        private static Dictionary<string, string> _gearVar = new Dictionary<string, string>();

        //public static Dictionary<string, GearVariation> GearVariations = new Dictionary<string, GearVariation>();
        private static string _GearVarGUID = "";
        private static Dictionary<string, string> _gearVariationsUsage = new Dictionary<string, string>();

        //TODO: Change the default names of the items in the tuple from Item1, Item2, Item3 to more descriptive names
        private static List<Tuple<string, string, bool>> _gearVariationsWithSpecs = new List<Tuple<string, string, bool>>();
        public static List<string> Accessories { get; internal set; }
        public static List<string> ExpenseItems { get; internal set; }
        public static List<string> PaymentSources { get; internal set; }

        public static void GetLists()
        {
            GetGearClass();
            GetPaymentSources();
            GetExpenses();
            GetAccessories();
        }

        public static bool DeleteGearCode(string gearCode)
        {
            bool success = true;
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var sql = $"Delete * from tblRefGearCodes where RefGearCode='{gearCode}'";
                using (OleDbCommand update = new OleDbCommand(sql, conn))
                {
                    try
                    {
                        success = update.ExecuteNonQuery() > 0;
                    }
                    catch (OleDbException oleEx)
                    {
                        success = false;
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex);
                        success = false;
                    }
                }
            }
            return success;
        }

        public static int GearCodeUsageCount(string gearCode)
        {
            int samplingCount = 0;
            using (var conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var sql = $"SELECT Count(RefNo) AS n FROM tblSampling WHERE RefNo Like '*-{gearCode}-*'";

                using (OleDbCommand getCount = new OleDbCommand(sql, conn))
                {
                    samplingCount = (int)getCount.ExecuteScalar();
                }
            }
            return samplingCount;
        }

        public static int GearVariationCount()
        {
            int count = 0;
            const string sql = "SELECT Count(Variation) AS n FROM tblGearVariations";
            using (var conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
            }
            return count;
        }

        public static string GearsXML()
        {
            StringBuilder sb = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(sb);
            writer.WriteStartDocument();
            writer.WriteStartElement("FishingGears");
            {
                writer.WriteStartElement("GearClasses");
                {
                    foreach (var gearClass in GearClasses.Values)
                    {
                        writer.WriteStartElement("GearClass");
                        writer.WriteAttributeString("ClassGuid", gearClass.GearClassGUID);
                        writer.WriteAttributeString("ClassLetter", gearClass.GearClassLetter);
                        writer.WriteAttributeString("ClassName", gearClass.GearClassName);

                        {
                            writer.WriteStartElement("GearVariations");
                            {
                                foreach (var gear in GetAllVariations(gearClass.GearClassGUID))
                                {
                                    writer.WriteStartElement("GearVariation");
                                    writer.WriteAttributeString("guid", gear.Key);
                                    writer.WriteAttributeString("name", gear.Value.VariationName);
                                    writer.WriteAttributeString("gear_class", gear.Value.GearClassGuid);
                                    writer.WriteAttributeString("mph1", gear.Value.MetaphoneKey1.ToString());
                                    writer.WriteAttributeString("mph2", gear.Value.MetaphoneKey2.ToString());
                                    writer.WriteAttributeString("name2", gear.Value.VariationName2);
                                    writer.WriteEndElement();
                                }
                            }
                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();
                    }
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            return sb.ToString();
        }

        public static void RefreshGearClasses()
        {
            GetGearClass();
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
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
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
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
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
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }
        }

        static Gears()
        {
        }

        public static bool EditGearLocalName(string localName, string oldLocalName)
        {
            bool success = false;
            short key1 = 0;
            short key2 = 0;
            DoubleMetaphoneShort mph = new DoubleMetaphoneShort();
            mph.ComputeMetaphoneKeys(localName, out key1, out key2);
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var sql = $@"Update tblGearLocalNames set
                                LocalName = ""{localName}"",
                                MPH1 = {key1},
                                MPH2 = {key2}
                            WHERE LocalName=""{oldLocalName}"" ";
                using (OleDbCommand update = new OleDbCommand(sql, conn))
                {
                    success = update.ExecuteNonQuery() > 0;
                }
            }
            return success;
        }

        public static bool EditGearVariationName(string variationName, string oldVariationName)
        {
            bool success = false;
            short key1 = 0;
            short key2 = 0;
            DoubleMetaphoneShort mph = new DoubleMetaphoneShort();
            mph.ComputeMetaphoneKeys(variationName, out key1, out key2);
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var sql = $@"Update tblGearVariations set
                                Variation = ""{variationName}"",
                                Name2 = ""{variationName.Replace(" ", "")}"",
                                MPH1 = {key1},
                                MPH2 = {key2}
                            WHERE Variation=""{oldVariationName}"" ";
                using (OleDbCommand update = new OleDbCommand(sql, conn))
                {
                    try
                    {
                        success = update.ExecuteNonQuery() > 0;
                    }
                    catch
                    {
                        //ignore
                    }
                }
            }
            return success;
        }

        //public static Dictionary<string, string> GearClassEx
        //{
        //    get { return _gearClassEx; }
        //}

        //public static Dictionary<string, string> GearClassLetterCodes
        //{
        //    get { return _gearClassLetterCodes; }
        //}

        public static string GearClassUsed
        {
            get { return _gearClassUsed; }
            set
            {
                _gearClassUsed = value;
                GetGearVariations();
            }
        }

        public static Dictionary<string, string> GearLocalNames
        {
            get
            {
                return _gearLocalNameListDict;
            }
        }

        public static List<string> AllGearVariationsList
        {
            get
            {
                List<string> allVariations = new List<string>();
                const string sql = "SELECT Variation from tblGearVariations";
                using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
                {
                    conn.Open();
                    var adapter = new OleDbDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    for (int n = 0; n < dt.Rows.Count; n++)
                    {
                        DataRow dr = dt.Rows[n];
                        allVariations.Add(dr["Variation"].ToString().ToLower());
                    }
                }
                return allVariations;
            }
        }

        public static string GearVarGUID
        {
            get { return _GearVarGUID; }
            set { _GearVarGUID = value; }
        }

        public static string GetVariationGuidFromVariationName(string variationName)
        {
            string variationGUID = "";
            var sql = $@"SELECT GearVarGUID
                         FROM tblGearVariations
                         WHERE Variation='{variationName}'";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                using (OleDbCommand getVariationGUID = new OleDbCommand(sql, conn))
                {
                    try
                    {
                        variationGUID = getVariationGUID.ExecuteScalar().ToString();
                    }
                    catch
                    {
                        variationGUID = "";
                    }
                }
            }
            return variationGUID;
        }

        public static (bool success, string newGuid) SaveNewLocalName(NewFisheryObjectName newName, string guid = "")
        {
            bool success = false;
            if (newName.NewName.Length > 0)
            {
                var newLocalName = newName.NewName;
                var key1 = newName.Key1;
                var key2 = newName.Key2;
                if (guid.Length == 0)
                {
                    guid = newName.ObjectGUID;
                }
                using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
                {
                    conn.Open();
                    var sql = $@"Insert into tblGearLocalNames (LocalName, LocalNameGUID, MPH1,MPH2) values
                        ('{newLocalName}', {{{guid}}},{key1},{key2})";
                    using (OleDbCommand update = new OleDbCommand(sql, conn))
                    {
                        try
                        {
                            success = update.ExecuteNonQuery() > 0;
                            if (success)
                            {
                                _gearLocalNameListDict.Add(guid, newLocalName);
                                _gearLocalNameListReverseDict.Add(newLocalName, guid);
                            }
                        }
                        catch (OleDbException dbex)
                        {
                            if (dbex.ErrorCode == -2147467259)
                            {
                                //guid = _gearLocalNameListReverseDict[newLocalName];
                                var comparer = StringComparison.CurrentCultureIgnoreCase;
                                foreach (var element in _gearLocalNameListReverseDict)
                                {
                                    if (String.Equals(element.Key, newLocalName, comparer))
                                    {
                                        guid = element.Value;
                                        success = true;
                                        break;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(ex.Message, "Gear", "SaveNewLocalName");
                        }
                    }
                }
            }
            return (success, guid);
        }

        public static (bool success, string newGuid) SaveNewVariationName(NewFisheryObjectName newName, string gearClassGuid, string gearVariationGuid = "")
        {
            bool success = false;
            var newVariationName = newName.NewName;
            var key1 = newName.Key1;
            var key2 = newName.Key2;
            var guid = "";
            if (gearVariationGuid.Length > 0)
            {
                guid = gearVariationGuid;
            }
            else
            {
                guid = newName.ObjectGUID;
            }
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var sql = $@"Insert into tblGearVariations (Variation, Name2, GearVarGuid, MPH1,MPH2,GearClass) values
                        ('{newVariationName}', '{newName.NewName.Replace(" ", "")}', {{{guid}}}, {key1}, {key2}, {{{gearClassGuid}}})";
                using (OleDbCommand update = new OleDbCommand(sql, conn))
                {
                    try
                    {
                        success = update.ExecuteNonQuery() > 0;
                    }
                    catch
                    {
                        //duplicate key error: ignore
                    }
                }
            }
            return (success, guid);
        }

        public static Dictionary<string, string> GetSimilarSoundingVariationNames(NewFisheryObjectName newName)
        {
            var key1 = newName.Key1;
            var key2 = newName.Key2;
            Dictionary<string, string> similarNames = new Dictionary<string, string>();
            var sql = $@"SELECT tblGearVariations.GearVarGUID, tblGearVariations.Variation
                         FROM tblGearVariations
                         WHERE tblGearVariations.MPH1={key1} AND tblGearLocalNames.MPH2={key2}";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var adapter = new OleDbDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    similarNames.Add(dr["GearVarGUID"].ToString(), dr["Variation"].ToString());
                }
            }
            return similarNames;
        }

        public static Dictionary<string, string> GetSimilarSoundingLocalNames(NewFisheryObjectName newName)
        {
            var key1 = newName.Key1;
            var key2 = newName.Key2;
            Dictionary<string, string> similarNames = new Dictionary<string, string>();
            var sql = $@"SELECT tblGearLocalNames.LocalNameGUID, tblGearLocalNames.LocalName
                         FROM tblGearLocalNames
                         WHERE tblGearLocalNames.MPH1={key1} AND tblGearLocalNames.MPH2={key2}";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var adapter = new OleDbDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    similarNames.Add(dr["LocalNameGUID"].ToString(), dr["LocalName"].ToString());
                }
            }
            return similarNames;
        }

        public static List<(int year, int count)> GearUseCountInTargetArea(string aoiGuid, string gearVarGuid)
        {
            var myList = new List<(int year, int count)>();
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT Year([SamplingDate]) AS yearSampled, Count(tblSampling.SamplingGUID) AS n
                                     FROM tblGearVariations INNER JOIN tblSampling ON tblGearVariations.GearVarGUID = tblSampling.GearVarGUID
                                     GROUP BY tblSampling.AOI, tblSampling.GearVarGUID, Year([SamplingDate])
                                     HAVING tblSampling.AOI ={{{aoiGuid}}} AND tblSampling.GearVarGUID= {{{gearVarGuid}}}";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        myList.Add(((System.Int16)dr["yearSampled"], (int)dr["n"]));
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }

            return myList;
        }

        public static
            Dictionary<string, (string targetAreaName, string refNo, string landingSite, string gearClassName, string gear,
                                DateTime samplingDate, string fishingGround, string vesselType, double wtCatch, string GUIDs, string EnumeratorName)>
                                gearSamplings(string gearVarGuid)
        {
            var myRows = new Dictionary<string, (string targetAreaName, string refNo, string landingSite, string gearClassName, string gear,
                                                 DateTime samplingDate, string fishingGround, string vesselType, double wtCatch, string GUIDs, string EnumeratorName)>();

            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                conection.Open();
                string query = $@"SELECT tblSampling.SamplingGUID,
                                    tblAOI.AOIName,
                                    tblLandingSites.LSGUID,
                                    tblLandingSites.LSName,
                                    tblGearClass.GearClassName,
                                    tblGearVariations.GearVarGUID,
                                    tblGearVariations.Variation,
                                    tblSampling.SamplingDate,
                                    tblSampling.FishingGround,
                                    tblSampling.WtCatch,
                                    tblSampling.RefNo,
                                    tblSampling.VesType,
                                    tblEnumerators.EnumeratorName
                                FROM (tblAOI INNER JOIN
                                    tblLandingSites ON
                                    tblAOI.AOIGuid = tblLandingSites.AOIGuid) INNER JOIN
                                    ((tblGearClass INNER JOIN
                                    tblGearVariations ON
                                    tblGearClass.GearClass = tblGearVariations.GearClass) INNER JOIN
                                    (tblEnumerators RIGHT JOIN
                                    tblSampling ON tblEnumerators.EnumeratorID = tblSampling.Enumerator) ON
                                    tblGearVariations.GearVarGUID = tblSampling.GearVarGUID) ON
                                    tblLandingSites.LSGUID = tblSampling.LSGUID
                                WHERE tblGearVariations.GearVarGUID= {{{gearVarGuid}}}
                                ORDER BY tblAOI.AOIName,
                                    tblLandingSites.LSName,
                                    tblGearVariations.Variation,
                                    tblSampling.SamplingDate";

                DataTable dt = new DataTable();
                var adapter = new OleDbDataAdapter(query, conection);
                adapter.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    var rowSamplingGuid = dr["SamplingGUID"].ToString();
                    var rowtargetAreaName = dr["AOIName"].ToString();
                    var rowRefNo = dr["RefNo"].ToString();
                    var rowLandingSite = dr["LSName"].ToString();
                    var rowGearClass = dr["GearClassName"].ToString();
                    var rowGear = dr["Variation"].ToString();
                    var rowSamplingDate = (DateTime)dr["SamplingDate"];
                    var rowFishingGround = dr["FishingGround"].ToString();
                    var rowVesselType = FishingVessel.VesselTypeFromVesselTypeNumber((int)dr["VesType"]);
                    var rowWtCatch = double.Parse(dr["WtCatch"].ToString());
                    var rowGUIDs = $"{dr["LSGUID"].ToString()}|{dr["GearVarGUID"].ToString()}";
                    var rowEnumeratorName = dr["EnumeratorName"].ToString();

                    myRows.Add(rowSamplingGuid, (rowtargetAreaName, rowRefNo, rowLandingSite, rowGearClass, rowGear, rowSamplingDate, rowFishingGround, rowVesselType, rowWtCatch, rowGUIDs, rowEnumeratorName));
                }
            }
            return myRows;
        }

        public static List<string> AllGearVariationNames()
        {
            var myList = new List<string>();
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    const string query = "Select Variation from tblGearVariations order by Variation";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        myList.Add(dr["Variation"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }

            return myList;
        }

        //public static string ClassCode(string ClassName)
        //{
        //    string myCode = "";
        //    foreach (KeyValuePair<string, string> kv in _gearClassLetterCodes)
        //    {
        //        if (kv.Key == ClassName)
        //        {
        //            myCode = kv.Value;
        //            break;
        //        }
        //    }
        //    return myCode;
        //}

        public static Dictionary<string, (string gearClassName, string gearCode)> GetGearClassDictionary()
        {
            Dictionary<string, (string gearClassName, string gearCode)> dict = new Dictionary<string, (string gearClassName, string gearCode)>();
            var myDT = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    const string query = "Select GearClass, GearLetter, GearClassName from tblGearClass order by GearClassName";
                    using (var adapter = new OleDbDataAdapter(query, conection))
                    {
                        adapter.Fill(myDT);
                        for (int i = 0; i < myDT.Rows.Count; i++)
                        {
                            DataRow dr = myDT.Rows[i];
                            dict.Add(dr["GearClass"].ToString(), (dr["gearClassName"].ToString(), dr["GearLetter"].ToString()));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }
            return dict;
        }

        public static KeyValuePair<string, string> GearClassFromGearVar(string varGUID)
        {
            KeyValuePair<string, string> myClass = new KeyValuePair<string, string>();
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT tblGearClass.GearClass, tblGearClass.GearClassName FROM tblGearClass
                                   INNER JOIN tblGearVariations ON tblGearClass.GearClass =
                                   tblGearVariations.GearClass WHERE tblGearVariations.GearVarGUID={{{varGUID}}}";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        _gearVar.Clear();
                        DataRow dr = dt.Rows[0];
                        myClass = new KeyValuePair<string, string>(dr["GearClass"].ToString(), dr["GearClassName"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }

            return myClass;
        }

        public static string GearClassFromGearVarGUID(string GearVarGUID)
        {
            string gearClass = "";
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT GearClassName FROM tblGearClass INNER JOIN tblGearVariations ON
                                   tblGearClass.GearClass = tblGearVariations.GearClass WHERE GearVarGUID={{{GearVarGUID}}}";

                    using (OleDbCommand getGearClass = new OleDbCommand(query, conection))
                    {
                        gearClass = getGearClass.ExecuteScalar().ToString();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
                return gearClass;
            }
        }

        public static KeyValuePair<string, string> GearClassGuidNameFromGearVarGuid(string GearVarGUID)
        {
            var dt = new DataTable();
            KeyValuePair<string, string> rv = new KeyValuePair<string, string>();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT tblGearClass.GearClass, GearClassName FROM tblGearClass INNER JOIN tblGearVariations ON
                                   tblGearClass.GearClass = tblGearVariations.GearClass WHERE GearVarGUID={{{GearVarGUID}}}";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        rv = new KeyValuePair<string, string>(dr["GearClass"].ToString(), dr["GearClassName"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
                return rv;
            }
        }

        public static string GearClassImageKeyFromGearClasName(string ClassName)
        {
            var iconKey = "";
            switch (ClassName)
            {
                case "Lines":
                    iconKey = "lines";
                    break;

                case "Traps":
                    iconKey = "traps";
                    break;

                case "Impounding nets":
                    iconKey = "impound";
                    break;

                case "Seines and dragnets":
                    iconKey = "seines";
                    break;

                case "Others":
                    iconKey = "others";
                    break;

                case "Gillnets":
                    iconKey = "nets";
                    break;

                case "Jigs":
                    iconKey = "jigs";
                    break;
            }
            return iconKey;
        }

        public static List<string> GearCodeByVariation(string GearVariationGuid)
        {
            var myDT = new DataTable();
            var myList = new List<string>();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $"Select RefGearCode from tblRefGearCodes where GearVar ={{{GearVariationGuid}}}";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(myDT);
                    for (int i = 0; i < myDT.Rows.Count; i++)
                    {
                        DataRow dr = myDT.Rows[i];
                        myList.Add(dr["RefGearCode"].ToString());
                    }
                }
                catch (Exception ex) { Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name); }
            }
            return myList;
        }

        public static string RefCodeTargetAreaRowGuid(string refGearCode, string targetAreaGuid)
        {
            string rowGuid = "";

            string sql = $"Select RowNo from tblRefGearCodes_Usage where RefGearCode='{refGearCode}' and TargetAreaGuid={{{targetAreaGuid}}}";
            var myDT = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    using (OleDbCommand getRowID = new OleDbCommand(sql, conection))
                    {
                        rowGuid = getRowID.ExecuteScalar().ToString();
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex.Message, ex.StackTrace);
                }
            }
            return rowGuid;
        }

        public static Dictionary<string, (string variationGuid, string variationName, bool isSubVariation)> GetGearRefCodes()
        {
            Dictionary<string, (string variationGuid, string variationName, bool isSubVariation)> dict = new Dictionary<string, (string variationGuid, string variationName, bool isSubVariation)>();
            var myDT = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    const string query = @"SELECT tblRefGearCodes.RefGearCode,
                                            tblGearVariations.GearVarGUID,
                                            tblGearVariations.Variation,
                                            tblRefGearCodes.SubVariation
                                           FROM tblGearVariations INNER JOIN
                                            tblRefGearCodes ON
                                            tblGearVariations.GearVarGUID = tblRefGearCodes.GearVar";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(myDT);
                    for (int i = 0; i < myDT.Rows.Count; i++)
                    {
                        DataRow dr = myDT.Rows[i];
                        dict.Add(dr["RefGearCode"].ToString(), (dr["GearVarGUID"].ToString(), dr["Variation"].ToString(), (bool)dr["SubVariation"]));
                    }
                }
                catch (Exception ex) { Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name); }
            }
            return dict;
        }

        public static List<string> GearCodesByClass(string GearClassGuid)
        {
            var myDT = new DataTable();
            var myList = new List<string>();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT RefGearCode FROM tblGearVariations INNER JOIN tblRefGearCodes ON
                                   tblGearVariations.GearVarGUID = tblRefGearCodes.GearVar WHERE
                                   tblGearVariations.GearClass= {{{GearClassGuid}}}
                                   ORDER BY tblRefGearCodes.RefGearCode";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(myDT);
                    for (int i = 0; i < myDT.Rows.Count; i++)
                    {
                        DataRow dr = myDT.Rows[i];
                        myList.Add(dr["RefGearCode"].ToString());
                    }
                }
                catch (Exception ex) { Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name); }
            }
            return myList;
        }

        public static string GearLetterFromGearClass(string GearClassGuid)
        {
            var myDT = new DataTable();
            var myLetter = "";
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $"Select GearLetter from tblGearClass where GearClass ={{{GearClassGuid}}}";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(myDT);
                    if (myDT.Rows.Count > 0)
                    {
                        DataRow dr = myDT.Rows[0];
                        myLetter = dr["GearLetter"].ToString();
                    }
                }
                catch (Exception ex) { Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name); }
            }
            return myLetter;
        }

        public static Dictionary<string, (string LocalName, string RowNumber)> GearLocalName_TargetArea(string RefCode, string AOIGuid)
        {
            var myList = new Dictionary<string, (string LocalName, string RowNumber)>();
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    var query = $@"SELECT LocalNameGUID, LocalName, tblRefGearUsage_LocalName.RowNo FROM tblGearLocalNames INNER JOIN
                         (tblRefGearCodes_Usage INNER JOIN tblRefGearUsage_LocalName ON
                         tblRefGearCodes_Usage.RowNo = tblRefGearUsage_LocalName.GearUsageRow)
                         ON tblGearLocalNames.LocalNameGUID = tblRefGearUsage_LocalName.GearLocalName
                         WHERE tblRefGearCodes_Usage.RefGearCode= '{RefCode}' AND
                         tblRefGearCodes_Usage.TargetAreaGUID= {{{AOIGuid}}}";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        myList.Add(dr["LocalNameGUID"].ToString(), (dr["LocalName"].ToString(), dr["RowNo"].ToString()));
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex.Message, ex.StackTrace);
                }
            }

            return myList;
        }

        public static Dictionary<string, bool> GearSubVariations(string GearVarGUID)
        {
            var myList = new Dictionary<string, bool>();
            var dt = new DataTable();
            var query = "";
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    query = $@"SELECT RefGearCode, SubVariation FROM tblGearVariations
                            LEFT JOIN tblRefGearCodes ON tblGearVariations.GearVarGUID = tblRefGearCodes.GearVar WHERE
                            tblGearVariations.GearVarGUID = {{{GearVarGUID}}}";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        myList.Add(dr["RefGearCode"].ToString(), bool.Parse(dr["SubVariation"].ToString()));
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }

            return myList;
        }

        /// <summary>
        /// returns true if a gear variation has a spec template
        /// </summary>
        /// <param name="GearVarGuid"></param>
        /// <returns></returns>
        public static bool GearVarHasSpecsTemplate(string GearVarGuid)
        {
            bool hasRows = false;
            using (var con = new OleDbConnection(global.ConnectionString))
            {
                var sql = $@"Select Top 1 GearVarGuid from tblGearSpecs
                          where GearVarGuid = {{{GearVarGuid}}} and
                          Version = '2'";

                using (var dt = new DataTable())
                {
                    con.Open();
                    var adapter = new OleDbDataAdapter(sql, con);
                    adapter.Fill(dt);
                    hasRows = dt.Rows.Count > 0;
                    con.Close();
                }
            }
            return hasRows;
        }

        public static Dictionary<string, string> GearVariationsUsage(string GearClassGUID)
        {
            getGearVariationsUsage(GearClassGUID);
            return _gearVariationsUsage;
        }

        public static Dictionary<string, string> GearVariationsUsage(string GearClassGUID, string AOIGUID)
        {
            getGearVariationsUsage(GearClassGUID, AOIGUID);
            return _gearVariationsUsage;
        }

        public static Dictionary<string, string> GearVariationsUsage(string GearClassGUID, string AOIGUID, ComboBox c)
        {
            getGearVariationsUsage(GearClassGUID, AOIGUID, c);
            return _gearVariationsUsage;
        }

        public static List<Tuple<string, string, bool>> GearVariationsWithSpecs(string GearClassGuid, string AOIGuid = "")
        {
            getGearVariationsUsage(GearClassGuid, AOIGuid, c: null, IncludeSpecsFlag: true);
            return _gearVariationsWithSpecs;
        }

        public static string GearVarNameFromGearGuid(string GearVarGuid)
        {
            string gearName = "";
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                conection.Open();
                string query = $"SELECT Variation FROM tblGearVariations  WHERE GearVarGUID={{{GearVarGuid}}}";
                using (var adapter = new OleDbDataAdapter(query, conection))
                {
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        gearName = dr["Variation"].ToString();
                    }
                }
            }
            return gearName;
        }

        public static string GearVarNodeImageKeyFromGearVar(string GearVarGuid)
        {
            var GearCLassName = GearClassFromGearVarGUID(GearVarGuid);
            return GearClassImageKeyFromGearClasName(GearCLassName);
        }

        //public static void GetGearClassEx(ComboBox c)
        //{
        //    var myDT = new DataTable();
        //    using (var conection = new OleDbConnection(global.ConnectionString))
        //    {
        //        try
        //        {
        //            conection.Open();
        //            const string query = "Select GearClass, GearLetter, GearClassName from tblGearClass order by GearClassName";
        //            using (var adapter = new OleDbDataAdapter(query, conection))
        //            {
        //                adapter.Fill(myDT);
        //                for (int i = 0; i < myDT.Rows.Count; i++)
        //                {
        //                    DataRow dr = myDT.Rows[i];
        //                    c.Items.Add(new KeyValuePair<string, string>(dr["GearClass"].ToString(), dr["GearClassName"].ToString()));
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
        //        }
        //    }
        //}

        public static bool SaveNewGearReferenceCode(string code, string gearVariationGuid, bool isVariation)
        {
            bool success = false;
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var sql = $@"Insert into tblRefGearCodes (RefGearCode, GearVar, SubVariation) values
                        ('{code}', {{{gearVariationGuid}}}, {isVariation})";
                using (OleDbCommand update = new OleDbCommand(sql, conn))
                {
                    try
                    {
                        success = update.ExecuteNonQuery() > 0;
                    }
                    catch
                    {
                        //ignore duplicate key error
                    }
                }
            }
            return success;
        }

        public static bool SaveNewGearClass(string gearClassGuid, string gearClassName, string gearClassCode)
        {
            bool success = false;
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var sql = $@"Insert into tblGearClass (RefGearCode, GearClassName, GearLetter) values
                        ({{{gearClassGuid}}}, ""{gearClassName}"", ""{gearClassCode}"")";
                using (OleDbCommand update = new OleDbCommand(sql, conn))
                {
                    try
                    {
                        success = update.ExecuteNonQuery() > 0;
                    }
                    catch
                    {
                        //ignore duplicate key error
                    }
                }
            }
            return success;
        }

        //public static Dictionary<string, (string gearClassName, string gearCode)> GetGearClassEx()
        //{
        //    Dictionary<string, (string gearClassName, string gearCode)> dict = new Dictionary<string, (string gearClassName, string gearCode)>();
        //    var myDT = new DataTable();
        //    using (var conection = new OleDbConnection(global.ConnectionString))
        //    {
        //        try
        //        {
        //            conection.Open();
        //            const string query = "Select GearClass, GearLetter, GearClassName from tblGearClass order by GearClassName";
        //            using (var adapter = new OleDbDataAdapter(query, conection))
        //            {
        //                adapter.Fill(myDT);
        //                for (int i = 0; i < myDT.Rows.Count; i++)
        //                {
        //                    DataRow dr = myDT.Rows[i];
        //                    dict.Add(dr["GearClass"].ToString(), (dr["GearClassName"].ToString(), dr["GearLetter"].ToString()));
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
        //        }
        //    }
        //    return dict;
        //}

        public static bool GetGearCodeCounter(string GearCode, ref long counter)
        {
            var dt = new DataTable();
            bool Success = false;
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $"Select Counter from tblRefCodeCounter where GearRefCode= '{GearCode}'";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        counter = long.Parse(dr["Counter"].ToString()) + 1;

                        query = $"Update tblRefCodeCounter set [Counter] = {counter}  Where GearRefCode='{GearCode}'";
                        OleDbCommand updateCounter = new OleDbCommand(query, conection);
                        Success = updateCounter.ExecuteNonQuery() > 0;
                    }
                    else
                    {
                        Success = InsertGearCode(GearCode);
                        if (Success)
                        {
                            counter = 1;
                        }
                    }
                    conection.Close();
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }
            return Success;
        }

        /// <summary>
        /// returns a list of months and the number of samplings of a gear in a landing iste
        /// </summary>
        /// <param name="LandingSiteGUID"></param>
        /// <returns></returns>
        public static Dictionary<string, string> MonthsSampledByGear(string LandingSiteGUID)
        {
            Dictionary<string, string> myMonths = new Dictionary<string, string>();
            var myDT = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT Format([SamplingDate],'mmm-yyyy') AS sMonth, Count(tblSampling.SamplingGUID) AS n,
                                      tblSampling.LSGUID, tblSampling.GearVarGUID FROM tblSampling GROUP BY Format([SamplingDate],'mmm-yyyy'),
                                      tblSampling.LSGUID, tblSampling.GearVarGUID, Year([SamplingDate]), Month([SamplingDate])
                                      HAVING tblSampling.[LSGUID]={{{LandingSiteGUID}}} AND tblSampling.[GearVarGUID]={{{_GearVarGUID}}}
                                      ORDER BY Year([SamplingDate]), Month([SamplingDate])";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(myDT);
                    for (int i = 0; i < myDT.Rows.Count; i++)
                    {
                        DataRow dr = myDT.Rows[i];
                        var lsGUID = dr["LSGUID"].ToString();
                        var gearGUID = dr["GearVarGUID"].ToString();
                        var month = dr["sMonth"].ToString();
                        myMonths.Add($"{lsGUID}|{gearGUID}|{month}", dr["sMonth"].ToString() + ": " + dr["n"].ToString());
                    }
                }
                catch (Exception ex) { Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name); }
            }
            return myMonths;
        }

        public static (bool success, int recordCount, string reason) DeleteGearVariation(string gearVarGuid)
        {
            var recordCount = SamplingCountGearVariation(gearVarGuid);
            var success = false;
            var reason = "";
            if (recordCount == 0)
            {
                using (var conn = new OleDbConnection(global.ConnectionString))
                {
                    conn.Open();
                    var sql = $"Delete * from tblGearVariations WHERE GearVarGUID={{{gearVarGuid}}}";

                    using (OleDbCommand deleteVariation = new OleDbCommand(sql, conn))
                    {
                        try
                        {
                            success = deleteVariation.ExecuteNonQuery() > 0;
                        }
                        catch (OleDbException ex)
                        {
                            if (DeleteVariationFromOtherTables(gearVarGuid))
                            {
                                try
                                {
                                    success = deleteVariation.ExecuteNonQuery() > 0;
                                }
                                catch (OleDbException ex1)
                                {
                                    success = false;
                                    reason = "Cannot delete this variation because it is used in other tables";
                                }
                            }

                            if (!success)
                                reason = "Cannot delete this variation because it is used in other tables";
                        }
                    }
                }
            }
            return (success, recordCount, reason);
        }

        private static bool DeleteVariationFromOtherTables(string gearVariationGuid)
        {
            var success = false;
            using (var conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var sql = $"Delete * from tblGearSpecs where GearVarGuid = {{{gearVariationGuid}}}";

                using (OleDbCommand deleteOther = new OleDbCommand(sql, conn))
                {
                    success = deleteOther.ExecuteNonQuery() > 0;
                }
            }
            return success;
        }

        public static bool GearVariationGUIDExists(string variationGUID)
        {
            bool exists = false;
            string sql = $"Select Variation from tblGearVariations where GearVarGUID = '{variationGUID}'";
            using (var conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                using (OleDbCommand getExists = new OleDbCommand(sql, conn))
                {
                    try
                    {
                        exists = getExists.ExecuteScalar().ToString().Length > 0;
                    }
                    catch
                    {
                        exists = false;
                    }
                }
            }
            return exists;
        }

        public static int SamplingCountGearVariation(string gearVarGuid)
        {
            int samplingCount = 0;
            using (var conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var sql = $"SELECT Count(GearVarGUID) AS n FROM tblSampling WHERE GearVarGUID={{{gearVarGuid}}}";

                using (OleDbCommand getCount = new OleDbCommand(sql, conn))
                {
                    samplingCount = (int)getCount.ExecuteScalar();
                }
            }
            return samplingCount;
        }

        /// <summary>
        /// returns a list of gear variation with number of matches based on 2 keys
        /// </summary>
        /// <param name="gearVarGuid"></param>
        /// <returns></returns>
        public static List<(string name, string matchQuality)> GearSoundsLike(short key1, short key2)
        {
            var GearNameMatches = new List<(string name, string matchQuality)>();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $"SELECT Variation, MPH2 from tblGearVariations where MPH1 = {key1}";
                    var myDT = new DataTable();
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(myDT);
                    for (int i = 0; i < myDT.Rows.Count; i++)
                    {
                        DataRow dr = myDT.Rows[i];
                        var matchQuality = key2 == short.Parse(dr["MPH2"].ToString()) ? "Very strong" : "Strong";
                        GearNameMatches.Add((dr["Variation"].ToString(), matchQuality));
                    }
                }
                catch (Exception ex) { Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name); }
            }
            return GearNameMatches;
        }

        public static string GearClassGuidFromClassName(string className)
        {
            string classGuid = "";
            string sql = $"Select GearClass from tblGearClass where GearClassName = '{className}'";
            using (var conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conn.Open();
                    var myDT = new DataTable();
                    var adapter = new OleDbDataAdapter(sql, conn);
                    adapter.Fill(myDT);
                    DataRow dr = myDT.Rows[0];
                    classGuid = dr["GearClass"].ToString();
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }
            return classGuid;
        }

        public static bool AddGearVariation(string gearClassGuid, string gearVariationName, string gearVariationGuid)
        {
            var success = false;

            var parts = gearVariationName.Split(' ');
            var Name2 = "";

            for (int n = 0; n < parts.Length; n++)
                Name2 += parts[n];

            using (var conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var dms = new DoubleMetaphoneShort();
                dms.ComputeMetaphoneKeys(gearVariationName, out short key1, out short key2);
                var sql = $@"Insert into tblGearVariations (Variation, Name2, GearVarGUID, GearClass, MPH1, MPH2)
                         values('{gearVariationName}', '{Name2}', {{{gearVariationGuid}}}, {{{gearClassGuid}}}, {key1}, {key2})";

                using (OleDbCommand update = new OleDbCommand(sql, conn))
                {
                    try
                    {
                        success = update.ExecuteNonQuery() > 0;
                    }
                    catch (OleDbException)
                    {
                        success = false;
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex.Message, "Gears", "AddGearVariation");
                    }
                }
            }

            return success;
        }

        /// <summary>
        /// adds a new gear variation
        /// Returns a tuple: newGuid - new gear variation guid
        ///                  success - if the database update succeded
        /// </summary>
        /// <param name="gearClassGuid"></param>
        /// <param name="gearVariationName"></param>
        /// <returns></returns>
        public static (bool success, string newGuid) AddGearVariation(string gearClassGuid, string gearVariationName)
        {
            var success = false;
            var newGuid = Guid.NewGuid().ToString();

            var parts = gearVariationName.Split(' ');
            var Name2 = "";

            for (int n = 0; n < parts.Length; n++)
                Name2 += parts[n];

            using (var conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var dms = new DoubleMetaphoneShort();
                dms.ComputeMetaphoneKeys(gearVariationName, out short key1, out short key2);
                var sql = $@"Insert into tblGearVariations (Variation, Name2, GearVarGUID, GearClass, MPH1, MPH2)
                         values('{gearVariationName}', '{Name2}', {{{newGuid}}}, {{{gearClassGuid}}}, {key1}, {key2})";

                using (OleDbCommand update = new OleDbCommand(sql, conn))
                {
                    success = update.ExecuteNonQuery() > 0;
                }
            }

            return (success, newGuid);
        }

        /// <summary>
        /// Adds a new gear reference code and returns a boolean depending on how the update went
        /// </summary>
        /// <param name="referenceCode"></param>
        /// <param name="gearVariationGuid"></param>
        /// <param name="isVariation"></param>
        /// <returns></returns>
        public static bool AddGearVariationReferenceCode(string referenceCode, string gearVariationGuid, bool isVariation)
        {
            var success = false;
            using (var conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var sql = $@"Insert into tblRefGearCodes (RefGearCode, GearVar, SubVariation)
                         values('{referenceCode}', {{{gearVariationGuid}}}, {isVariation})";
                using (OleDbCommand update = new OleDbCommand(sql, conn))
                {
                    success = update.ExecuteNonQuery() > 0;
                }
            }
            return success;
        }

        /// <summary>
        /// updates local names of gear in a target area
        /// Returns a tuple: NewRow - new row number in the database table
        ///                  Success - if the database update succeded
        /// </summary>
        /// <param name="gearCode"></param>
        /// <param name="targetAreaGuid"></param>
        public static (string NewRow, bool Success) AddUsageLocalName(string targetAreaUsageGuid, string localNameGuid, string rowGuid = "")
        {
            var success = false;
            var newRow = "";
            if (rowGuid.Length > 0)
            {
                newRow = rowGuid;
            }
            else
            {
                newRow = Guid.NewGuid().ToString();
            }
            using (var conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();

                var sql = $@"Insert into tblRefGearUsage_LocalName (GearUsageRow, GearLocalName, RowNo)
                         values({{{targetAreaUsageGuid}}}, {{{localNameGuid}}}, {{{newRow}}})";
                using (OleDbCommand update = new OleDbCommand(sql, conn))
                {
                    try
                    {
                        success = update.ExecuteNonQuery() > 0;
                    }
                    catch (OleDbException)
                    {
                        success = false;
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex.Message, ex.StackTrace);
                    }
                }
            }
            return (newRow, success);
        }

        /// <summary>
        /// deletes a local name used in a target area
        /// Returns a boolean if the database row delete succeeded
        /// </summary>
        /// <param name="LocalNameUsageGuid"></param>
        /// <returns></returns>
        public static bool DeleteLocalNameUsage(string LocalNameUsageGuid)
        {
            var success = false;
            using (var conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var sql = $"Delete * from tblRefGearUsage_LocalName where RowNo = {{{LocalNameUsageGuid}}}";
                using (OleDbCommand deleteRow = new OleDbCommand(sql, conn))
                {
                    success = deleteRow.ExecuteNonQuery() > 0;
                }
            }
            return success;
        }

        /// <summary>
        /// deletes a target area where a gear code is used
        /// returns boolean if the database delete succeeded
        /// </summary>
        /// <param name="TargetAreaUsageGuid"></param>
        /// <returns></returns>
        public static (bool success, string message) DeleteTargetAreaUsage(string TargetAreaUsageGuid)
        {
            var success = false;
            var message = "";
            using (var conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var sql = $"Delete * from tblRefGearCodes_Usage where RowNo = {{{TargetAreaUsageGuid}}}";
                using (OleDbCommand update = new OleDbCommand(sql, conn))
                {
                    try
                    {
                        success = update.ExecuteNonQuery() > 0;
                    }
                    catch (OleDbException ex)
                    {
                        success = false;
                        message = "Could not delete because this target area is used in other tables";
                    }
                }
            }
            return (success, message);
        }

        /// <summary>
        /// updates target areas where a gear variation code is used.
        /// Returns a tuple: NewRow - new row number in the database table
        ///                  Success - if the database update succeeded
        /// </summary>
        /// <param name="gearCode"></param>
        /// <param name="targetAreaGuid"></param>
        public static (string NewRow, bool Success) AddGearCodeUsageTargetArea(string gearCode, string targetAreaGuid, string usageGuid = "")
        {
            string newGuid = "";
            if (usageGuid.Length > 0)
            {
                newGuid = usageGuid;
            }
            else
            {
                newGuid = Guid.NewGuid().ToString();
            }
            bool success = false;
            using (var conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var sql = $@"Insert into tblRefGearCodes_Usage (RefGearCode, TargetAreaGUID, RowNo)
                         values('{gearCode}', {{{targetAreaGuid}}}, {{{newGuid}}})";
                using (OleDbCommand update = new OleDbCommand(sql, conn))
                {
                    try
                    {
                        success = update.ExecuteNonQuery() > 0;
                    }
                    catch (OleDbException)
                    {
                        sql = $"Select RowNo from tblRefGearCodes_Usage where RefGearCode = '{gearCode}' and TargetAreaGUID = {{{targetAreaGuid}}}";
                        using (OleDbCommand getUsageID = new OleDbCommand(sql, conn))
                        {
                            newGuid = getUsageID.ExecuteScalar().ToString();
                            success = newGuid.Length > 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex.Message, ex.StackTrace);
                    }
                }
            }
            return (newGuid, success);
        }

        public static Dictionary<string, (string AOIName, string RowNumber)> TargetAreaUsed_RefCode(string RefCode)
        {
            var myList = new Dictionary<string, (string AOIName, string RowNumber)>();
            var dt = new DataTable();
            var query = "";
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    query = $@"SELECT AOIName, AOIGuid, tblRefGearCodes_Usage.RowNo FROM tblRefGearCodes_Usage INNER JOIN tblAOI ON
                        tblRefGearCodes_Usage.TargetAreaGUID = tblAOI.AOIGuid WHERE
                        tblRefGearCodes_Usage.RefGearCode= '{RefCode}' order by AOIName";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        myList.Add(dr["AOIGuid"].ToString(), (dr["AOIName"].ToString(), dr["RowNo"].ToString()));
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }

            return myList;
        }

        private static void GetGearClass()
        {
            GearClasses.Clear();
            //_gearClassEx.Clear();
            //_gearClassLetterCodes.Clear();
            var myDT = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    const string query = "Select GearClass, GearLetter, GearClassName from tblGearClass order by GearClassName";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(myDT);
                    for (int i = 0; i < myDT.Rows.Count; i++)
                    {
                        DataRow dr = myDT.Rows[i];
                        GearClass gc = new GearClass(dr["GearClassName"].ToString(), dr["GearLetter"].ToString(), dr["GearClass"].ToString());
                        GearClasses.Add(gc.GearClassGUID, gc);
                        //_gearClassEx.Add(dr["GearClass"].ToString(), dr["GearClassName"].ToString());
                        //_gearClassLetterCodes.Add(dr["GearClassName"].ToString(), dr["GearLetter"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }
        }

        public static void GetGearLocalNames()
        {
            _gearLocalNameListDict.Clear();
            _gearLocalNameListReverseDict.Clear();
            DataTable dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    const string query = "SELECT LocalName, LocalNameGUID FROM tblGearLocalNames ORDER BY LocalName";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        _gearLocalNameListDict.Add(dr["LocalNameGUID"].ToString(), dr["LocalName"].ToString());
                        _gearLocalNameListReverseDict.Add(dr["LocalName"].ToString(), dr["LocalNameGUID"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }
        }

        //public static Dictionary<string, (string gearVarName, string gearCLassGuid, int metaPhoneKey1, int metaPhoneKey2, string Name2)> GetAllVariations()
        public static Dictionary<string, GearVariation> GetAllVariations(string gearClassGuid = "")
        {
            Dictionary<string, GearVariation> dict =
                new Dictionary<string, GearVariation>();

            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = "SELECT * from tblGearVariations order by Variation";
                    if (gearClassGuid.Length > 0)
                    {
                        query = $"SELECT * from tblGearVariations where GearClass = {{{gearClassGuid}}} order by Variation";
                    }
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    //dict.Clear();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        GearVariation gv = new GearVariation(dr["GearVarGUID"].ToString(), dr["Variation"].ToString(), dr["GearClass"].ToString(), (short)(int)dr["MPH1"], (short)(int)dr["MPH2"]);
                        gv.VariationName2 = dr["Name2"].ToString();
                        //dict.Add(dr["GearVarGUID"].ToString(), (dr["Variation"].ToString(), dr["GearClass"].ToString(), (int)dr["MPH1"], (int)dr["MPH2"], dr["Name2"].ToString()));
                        dict.Add(gv.VariationGuid, gv);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }
            return dict;
        }

        private static void GetGearVariations()
        {
            _gearVar.Clear();
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $"SELECT GearVarGUID, Variation FROM tblGearVariations WHERE GearClass= '{GearClassUsed}' order by Variation";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    _gearVar.Clear();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        _gearVar.Add(dr["GearVarGUID"].ToString(), dr["Variation"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }
        }

        private static void getGearVariationsUsage(string GearClassGUID, string AOIGUID = "", ComboBox c = null, bool IncludeSpecsFlag = false)
        {
            _gearVariationsUsage.Clear();
            _gearVariationsWithSpecs.Clear();
            var dt = new DataTable();
            var query = "";
            var GearVarGuid = "";
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    if (AOIGUID.Length > 0)
                    {
                        query = $@"SELECT DISTINCT GearVarGUID, Variation FROM tblGearVariations INNER JOIN
                                   (tblRefGearCodes INNER JOIN tblRefGearCodes_Usage ON tblRefGearCodes.RefGearCode =
                                   tblRefGearCodes_Usage.RefGearCode) ON tblGearVariations.GearVarGUID =
                                   tblRefGearCodes.GearVar WHERE tblRefGearCodes_Usage.TargetAreaGUID= {{{AOIGUID}}}
                                   AND tblGearVariations.GearClass={{{GearClassGUID}}} ORDER BY Variation";
                    }
                    else
                    {
                        query = $@"Select GearVarGUID, Variation from tblGearVariations where
                                   GearClass = {{{GearClassGUID}}} ORDER BY Variation";
                    }
                    using (var adapter = new OleDbDataAdapter(query, conection))
                    {
                        adapter.Fill(dt);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow dr = dt.Rows[i];
                            GearVarGuid = dr["GearVarGUID"].ToString();

                            //fill the combobox list
                            if (c != null)
                                c.Items.Add(new KeyValuePair<string, string>(dr["GearVarGUID"].ToString(), dr["Variation"].ToString()));

                            if (IncludeSpecsFlag)
                            {
                                var HasGearSpecs = GearVarHasSpecsTemplate(GearVarGuid);
                                var t = new Tuple<string, string, bool>(GearVarGuid, dr["Variation"].ToString(), HasGearSpecs);
                                _gearVariationsWithSpecs.Add(t);
                            }
                            else
                            {
                                _gearVariationsUsage.Add(GearVarGuid, dr["Variation"].ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }
        }

        private static bool InsertGearCode(string GearCode)
        {
            bool Success = false;
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    string query = $"Insert into tblRefCodeCounter ([Counter], GearRefCode) values (1, '{GearCode}')";
                    OleDbCommand newGearCode = new OleDbCommand(query, conection);
                    conection.Open();
                    Success = (newGearCode.ExecuteNonQuery()) > 0;
                    conection.Close();
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }
            return Success;
        }
    }
}
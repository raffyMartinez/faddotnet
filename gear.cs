using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;

//using ADOX;
using dao;
using System.Windows.Forms;

namespace FAD3
{
    public static class gear
    {
        private static Dictionary<string, string> _GearVariationsUsage = new Dictionary<string, string>();

        //TODO: Change the default names of the items in the tuple from Item1, Item2, Item3 to more descriptive names
        private static List<Tuple<string, string, bool>> _GearVariationsWithSpecs = new List<Tuple<string, string, bool>>();

        private static Dictionary<string, string> _GearClass = new Dictionary<string, string>();
        private static Dictionary<string, string> _GearClassLetterCodes = new Dictionary<string, string>();
        private static Dictionary<string, string> _GearLocalNameListDict = new Dictionary<string, string>();

        static gear()
        {
        }

        public static void FillGearClasses()
        {
            GetGearClass();
        }

        public static Dictionary<string, string> GearClass
        {
            get { return _GearClass; }
        }

        public static void MakeVesselTypeTable()
        {
            var dbe = new DBEngine();
            var dbData = dbe.OpenDatabase(global.mdbPath);

            try
            {
                dbData.TableDefs.Delete("temp_VesselType");
            }
            catch { }

            string sql = "SELECT 1 AS VesselTypeNo, 'Motorized' AS VesselType into temp_VesselType";
            var qd = dbData.CreateQueryDef("", sql);
            qd.Execute();

            sql = "Insert into temp_VesselType (VesselTypeNo, VesselType) values (2,'Non-Motorized')";
            qd = dbData.CreateQueryDef("", sql);
            qd.Execute();

            sql = "Insert into temp_VesselType (VesselTypeNo, VesselType) values (3,'No vessel used')";
            qd = dbData.CreateQueryDef("", sql);
            qd.Execute();

            sql = "Insert into temp_VesselType (VesselTypeNo, VesselType) values (4,'Not provided')";
            qd = dbData.CreateQueryDef("", sql);
            qd.Execute();

            qd.Close();
            qd = null;

            dbData.Close();
            dbData = null;
        }

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
                    ErrorLogger.Log(ex);
                }
            }
            return Success;
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
                    ErrorLogger.Log(ex);
                }
            }
            return Success;
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
                    DataRow dr = myDT.Rows[0];
                    myLetter = dr["GearLetter"].ToString();
                }
                catch (Exception ex) { ErrorLogger.Log(ex); }
            }
            return myLetter;
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
                catch (Exception ex) { ErrorLogger.Log(ex); }
            }
            return myList;
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
                catch (Exception ex) { ErrorLogger.Log(ex); }
            }
            return myList;
        }

        private static void getGearVariationsUsage(string GearClassGUID, string AOIGUID = "", ComboBox c = null, bool IncludeSpecsFlag = false)
        {
            _GearVariationsUsage.Clear();
            _GearVariationsWithSpecs.Clear();
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
                                _GearVariationsWithSpecs.Add(t);
                            }
                            else
                            {
                                _GearVariationsUsage.Add(GearVarGuid, dr["Variation"].ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }
        }

        public static Dictionary<string, string> GearVariationsUsage(string GearClassGUID)
        {
            getGearVariationsUsage(GearClassGUID);
            return _GearVariationsUsage;
        }

        public static Dictionary<string, string> GearVariationsUsage(string GearClassGUID, string AOIGUID)
        {
            getGearVariationsUsage(GearClassGUID, AOIGUID);
            return _GearVariationsUsage;
        }

        public static Dictionary<string, string> GearVariationsUsage(string GearClassGUID, string AOIGUID, ComboBox c)
        {
            getGearVariationsUsage(GearClassGUID, AOIGUID, c);
            return _GearVariationsUsage;
        }

        public static void GetGearClassEx(ComboBox c)
        {
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
                            c.Items.Add(new KeyValuePair<string, string>(dr["GearClass"].ToString(), dr["GearClassName"].ToString()));
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }
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
                    ErrorLogger.Log(ex);
                }
            }

            return myList;
        }

        private static void GetGearClass()
        {
            _GearClass.Clear();
            _GearClassLetterCodes.Clear();
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
                        _GearClass.Add(dr["GearClass"].ToString(), dr["GearClassName"].ToString());
                        _GearClassLetterCodes.Add(dr["GearClassName"].ToString(), dr["GearLetter"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }
        }

        public static Dictionary<string, string> GearLocalName_TargetArea(string RefCode, string AOIGuid)
        {
            var myList = new Dictionary<string, string>();
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    var query = $@"SELECT LocalNameGUID, LocalName FROM tblGearLocalNames INNER JOIN
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
                        myList.Add(dr["LocalNameGUID"].ToString(), dr["LocalName"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }

            return myList;
        }

        public static Dictionary<string, string> TargetAreaUsed_RefCode(string RefCode)
        {
            var myList = new Dictionary<string, string>();
            var dt = new DataTable();
            var query = "";
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    query = $@"SELECT AOIName, AOIGuid FROM tblRefGearCodes_Usage INNER JOIN tblAOI ON
                        tblRefGearCodes_Usage.TargetAreaGUID = tblAOI.AOIGuid WHERE
                        tblRefGearCodes_Usage.RefGearCode= '{RefCode}' order by AOIName";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        myList.Add(dr["AOIGuid"].ToString(), dr["AOIName"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }

            return myList;
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
                    ErrorLogger.Log(ex);
                }
            }

            return myList;
        }

        public static Dictionary<string, string> GearLocalNames
        {
            get
            {
                GetGearLocalNames();
                return _GearLocalNameListDict;
            }
        }

        private static void GetGearLocalNames()
        {
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
                        _GearLocalNameListDict.Add(dr["LocalNameGUID"].ToString(), dr["LocalName"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
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
                    DataRow dr = dt.Rows[0];
                    rv = new KeyValuePair<string, string>(dr["GearClass"].ToString(), dr["GearClassName"].ToString());
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
                return rv;
            }
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
                    _GearVar.Clear();
                    DataRow dr = dt.Rows[0];
                    myClass = new KeyValuePair<string, string>(dr["GearClass"].ToString(), dr["GearClassName"].ToString());
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }

            return myClass;
        }

        private static Dictionary<string, string> _GearVar = new Dictionary<string, string>();

        public static string GearClassFromGearVarGUID(string GearVarGUID)
        {
            string gearClass = "";
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT GearClassName FROM tblGearClass INNER JOIN tblGearVariations ON
                                   tblGearClass.GearClass = tblGearVariations.GearClass WHERE GearVarGUID={{{GearVarGUID}}}";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    DataRow dr = dt.Rows[0];
                    gearClass = dr["GearClassName"].ToString();
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
                return gearClass;
            }
        }

        public static string GearClassUsed
        {
            get { return _GearClassUsed; }
            set
            {
                _GearClassUsed = value;
                GetGearVariations();
            }
        }

        private static string _GearClassUsed = "";

        private static void GetGearVariations()
        {
            _GearVar.Clear();
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $"SELECT GearVarGUID, Variation FROM tblGearVariations WHERE GearClass= '{GearClassUsed}' order by Variation";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    _GearVar.Clear();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        _GearVar.Add(dr["GearVarGUID"].ToString(), dr["Variation"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }
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
                    DataRow dr = dt.Rows[0];
                    gearName = dr["Variation"].ToString();
                }
            }
            return gearName;
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

        public static string GearVarNodeImageKeyFromGearVar(string GearVarGuid)
        {
            var GearCLassName = GearClassFromGearVarGUID(GearVarGuid);
            return GearClassImageKeyFromGearClasName(GearCLassName);
        }

        public static List<Tuple<string, string, bool>> GearVariationsWithSpecs(string GearClassGuid, string AOIGuid = "")
        {
            getGearVariationsUsage(GearClassGuid, AOIGuid, c: null, IncludeSpecsFlag: true);
            return _GearVariationsWithSpecs;
        }

        public static string ClassCode(string ClassName)
        {
            string myCode = "";
            foreach (KeyValuePair<string, string> kv in _GearClassLetterCodes)
            {
                if (kv.Key == ClassName)
                {
                    myCode = kv.Value;
                    break;
                }
            }
            return myCode;
        }

        public static Dictionary<string, string> GearClassLetterCodes
        {
            get { return _GearClassLetterCodes; }
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
    }
}
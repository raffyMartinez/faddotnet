using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using ADOX;

namespace FAD3
{
    static class names
    {
        private static List<string> _GenusList = new List<string>();
        private static List<string> _LocalNameList = new List<string>();
        private static Dictionary<string, string> _LocalNameListDict = new Dictionary<string, string>();
        private static Dictionary<string, string> _GearLocalNameListDict = new Dictionary<string, string>();
        private static Dictionary<string,string> _speciesList = new Dictionary<string,string>();
        private static string _Genus = "";
        private static long _LocalNamesCount = 0;
        private static long _SciNamesCount = 0;

        public static string Genus {
            get { return _Genus; }
            set
            { _Genus = value;
                GetSpecies();  
            }
        }

        public static long NamesCount
        {
            get { return _LocalNamesCount + _SciNamesCount; }
        }

        public static Dictionary<string,string> GearLocalNames
        {
            get
            {
                GetGearLocalNames();
                return _GearLocalNameListDict;
            }
        }

        public static long LocalNamesCount
        {
            get { return _LocalNamesCount; }
        }

        public static long SciNamesCount
        {
            get { return  _SciNamesCount; }
        }

        private static void GetSpecies()
        {
            DataTable dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = "SELECT Name2, NameNo FROM temp_AllNames " +
                          "WHERE Name1 = '" + _Genus + "'  ORDER BY Name2";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    _speciesList.Clear();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        _speciesList.Add(dr["NameNo"].ToString(), dr["Name2"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }
        }

        public static bool IsListedInFishBase(string NameGUID)
        {
            bool isListed = false;
            DataTable dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = "Select ListedFB from tblAllSpecies where SpeciesGUID = '{" + NameGUID + "}'";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    DataRow dr= dt.Rows[0];
                    isListed = bool.Parse(dr["ListedFB"].ToString());
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }
            return isListed;
        }

        public static void GetLocalNames()
        {
            DataTable dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = "SELECT Name1, NameNo FROM temp_AllNames WHERE Identification = 'Local names' ORDER BY Name1";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        _LocalNameListDict.Add(dr["NameNo"].ToString(), dr["Name1"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }
        }

         static void GetGearLocalNames()
        {
            DataTable dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = "SELECT LocalName, LocalNameGUID FROM tblGearLocalNames ORDER BY LocalName";
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

        /// <summary>
        /// Makes a new table of names (local and scientific names) derived from
        /// the the local names table and the scientific names table
        /// </summary>
        public static void MakeAllNames()
        {
            Catalog catMDB = new Catalog();
            catMDB.let_ActiveConnection(global.ConnectionString);


            try
            {
                catMDB.Tables.Delete("temp_AllNames");
            }
            catch
            { }
                using (var conection = new OleDbConnection(global.ConnectionString))
                {

                    OleDbCommand cmd = new OleDbCommand()
                    {
                        Connection = conection,                            
                    };

                    conection.Open();

                    //select into query
                    string sql = "SELECT Name AS Name1, '' AS Name2, NameNo, 'Local names' AS Identification INTO temp_AllNames FROM tblBaseLocalNames";
                    cmd.CommandText = sql;
                    try
                    {
                        _LocalNamesCount = cmd.ExecuteNonQuery();
                    }
                    catch(Exception ex)
                    {
                        ErrorLogger.Log(ex);
                    }

                    //insert into to append the results from the select into query
                    sql = "INSERT INTO temp_AllNames ( Name1, Name2, NameNo, Identification ) SELECT Genus, species, SpeciesGUID, 'Species names' AS Identification FROM tblAllSpecies";
                    cmd.CommandText = sql;
                    try
                    {
                        _SciNamesCount += cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.Log(ex);
                    }

                    conection.Close();

                }
            
        }

        public static void GetGenus_LocalNames()
        {
            DataTable dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = "SELECT DISTINCT Name1, Identification FROM temp_AllNames " +
                                 "ORDER BY Name1";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        if (dr["Identification"].ToString() == "Local names")
                        {
                            _LocalNameList.Add(dr["Name1"].ToString());
                        }
                        else
                        {
                            _GenusList.Add(dr["Name1"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }
        }

        public static Dictionary<string,string> speciesList 
        {
            get { return _speciesList; }
        }

        public static List<string> GenusList
        {
            get { return _GenusList; }
        }

        public static Dictionary<string, string> LocalNameListDict
        {
            get {
                GetLocalNames();
                return _LocalNameListDict;
            }
        }

        public static List<string> LocalNameList
        {
            get { return _LocalNameList; }
        }
    }
}

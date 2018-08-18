using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;

//using ADOX;
using dao;

namespace FAD3
{
    internal static class names
    {
        private static List<string> _GenusList = new List<string>();
        private static List<string> _LocalNameList = new List<string>();
        private static Dictionary<string, string> _LocalNameListDict = new Dictionary<string, string>();
        private static Dictionary<string, string> _speciesList = new Dictionary<string, string>();
        private static string _Genus = "";
        private static long _LocalNamesCount = 0;
        private static long _SciNamesCount = 0;

        public static string Genus
        {
            get { return _Genus; }
            set
            {
                _Genus = value;
                GetSpecies();
            }
        }

        public static long NamesCount
        {
            get { return _LocalNamesCount + _SciNamesCount; }
        }

        public static long LocalNamesCount
        {
            get { return _LocalNamesCount; }
        }

        public static long SciNamesCount
        {
            get { return _SciNamesCount; }
        }

        public static bool UpdateSpeciesData()
        {
            return true;
        }

        public static (bool inFishBase, int? fishBaseSpeciesNo) NameInFishBaseEx(string genus, string species)
        {
            var inFishBase = false;
            int? speciesNumber = null;
            var sql = $"Select SpecCode from FBSpecies where Genus = '{genus.Trim()}' AND Species = '{species.Trim()}'";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var adapter = new OleDbDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    inFishBase = true;
                    speciesNumber = (int)dr["SpecCode"];
                }
            }
            return (inFishBase, speciesNumber);
        }

        public static bool NameInFishBase(string genus, string species)
        {
            var inFishBase = false;
            var sql = $"Select ListedFB from tblAllSpecies where Genus = {genus} AND species = {species}";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                using (OleDbCommand inFB = new OleDbCommand(sql, conn))
                {
                    inFishBase = (bool)inFB.ExecuteScalar();
                }
            }
            return inFishBase;
        }

        public static List<(string fullName, string genus, string species)> RetrieveSpeciesWithSimilarMetaPhone(short genusKey1, short genusKey2, short speciesKey1, short speciesKey2)
        {
            var list = new List<(string fullName, string genus, string species)>();
            var sql = $"Select Genus, species from tblAllSpecies where MPHG1 = {genusKey1} AND MPHG2 = {genusKey2} AND MPHS1 = {speciesKey1} AND MPHS2 = {speciesKey2}";
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                conection.Open();
                var adapter = new OleDbDataAdapter(sql, conection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    sql = $"Select Genus, species from tblAllSpecies where MPHG1 = {genusKey1} AND MPHS1 = {speciesKey1}";
                    adapter = new OleDbDataAdapter(sql, conection);
                    dt = new DataTable();
                    adapter.Fill(dt);
                }

                foreach (DataRow dr in dt.Rows)
                {
                    var genus = dr["Genus"].ToString();
                    var species = dr["species"].ToString();
                    var fullName = $"{genus} {species}";
                    list.Add((fullName, genus, species));
                }
            }
            return list;
        }

        public static (bool isFound, bool inFishbase, int? fishBaseNo, string notes,
            short? genusKey1, short? genusKey2, short? speciesKey1, short? speciesKey2, GMSManager.Taxa taxa)
            RetrieveSpeciesData(string speciesGuid)
        {
            var sql = $"Select * from tblAllSpecies where SpeciesGUID = {{{speciesGuid}}}";
            var isFound = false;
            var inFishbase = false;
            int? fishBaseNo = null;
            var notes = "";
            short? genusKey1 = null;
            short? genusKey2 = null;
            short? speciesKey1 = null;
            short? speciesKey2 = null;
            GMSManager.Taxa taxa = GMSManager.Taxa.Fish;
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                conection.Open();
                var adapter = new OleDbDataAdapter(sql, conection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    isFound = true;
                    inFishbase = (bool)dr["ListedFB"];

                    if (inFishbase)
                        fishBaseNo = int.Parse(dr["FBSpNo"].ToString());

                    notes = dr["Notes"].ToString();
                    genusKey1 = (short)dr["MPHG1"];
                    genusKey2 = (short)dr["MPHG2"];
                    speciesKey1 = (short)dr["MPHS1"];
                    speciesKey2 = (short)dr["MPHS2"];
                    Enum.TryParse(dr["TaxaNo"].ToString(), out taxa);
                }
                return (isFound, inFishbase, fishBaseNo, notes, genusKey1, genusKey2, speciesKey1, speciesKey2, taxa);
            }
        }

        private static void GetSpecies()
        {
            DataTable dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT Name2, NameNo FROM temp_AllNames
                                      WHERE Name1 = '{_Genus}'  ORDER BY Name2";
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
                    Logger.Log(ex);
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
                    string query = $"Select ListedFB from tblAllSpecies where SpeciesGUID = {{{NameGUID}}}";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    DataRow dr = dt.Rows[0];
                    isListed = bool.Parse(dr["ListedFB"].ToString());
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
            return isListed;
        }

        public static void GetLocalNames()
        {
            _LocalNameListDict.Clear();
            DataTable dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    const string query = "SELECT Name1, NameNo FROM temp_AllNames WHERE Identification = 'Local names' ORDER BY Name1";
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
                    Logger.Log(ex);
                }
            }
        }

        /// <summary>
        /// Makes a new table of names (local and scientific names) derived from
        /// the the local names table and the scientific names table
        /// </summary>

        public static void MakeAllNames()
        {
            var dbe = new DBEngine();
            var dbData = dbe.OpenDatabase(global.mdbPath);

            var sql = @"SELECT Name AS Name1, '' AS Name2, NameNo, 'Local names' as Identification From tblBaseLocalNames
                                UNION ALL SELECT Genus AS Name1, species AS Name2, SpeciesGUID AS NameNo,  'Species names' as Identification
                                FROM tblAllSpecies";

            try
            {
                dbData.QueryDefs.Delete("qryAllNames");
            }
            catch { }
            try
            {
                dbData.TableDefs.Delete("temp_AllNames");
            }
            catch { }

            var qd = dbData.CreateQueryDef("qryAllNames", sql);
            dbData.QueryDefs.Refresh();
            qd.Close();

            sql = "SELECT qryAllNames.* INTO temp_AllNames FROM qryAllNames";
            qd = dbData.CreateQueryDef("", sql);
            qd.Execute();
            qd.Close();

            dbData.TableDefs.Refresh();
            dbData.QueryDefs.Delete("qryAllNames");

            qd = null;

            dbData.Close();
            dbData = null;
        }

        public static void GetGenus_LocalNames()
        {
            _LocalNameList.Clear();
            _GenusList.Clear();
            DataTable dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    const string query = "SELECT DISTINCT Name1, Identification FROM temp_AllNames ORDER BY Name1";
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
                    Logger.Log(ex);
                }
            }
        }

        public static Dictionary<string, string> speciesList
        {
            get { return _speciesList; }
        }

        public static List<string> GenusList
        {
            get { return _GenusList; }
        }

        public static Dictionary<string, string> LocalNameListDict
        {
            get
            {
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
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

        public static int CatchCompositionRecordCount(string nameGuid)
        {
            var sql = $"SELECT Count(NameType) AS n FROM tblCatchComp WHERE NameGUID={{{nameGuid}}}";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                using (OleDbCommand getCount = new OleDbCommand(sql, conn))
                {
                    return (int)getCount.ExecuteScalar();
                }
            }
        }

        public static bool UpdateSpeciesData(global.fad3DataStatus dataStatus, string nameGuid, string genus, string species, CatchName.Taxa taxa,
                              short genusMPH1, short genusMPH2, short speciesMPH1, short speciesMPH2, bool inFishbase, int? fishBaseSpeciesNo, string notes)
        {
            var sql = "";
            var Success = false;
            var fbNo = fishBaseSpeciesNo == null ? "null" : fishBaseSpeciesNo.ToString();
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                if (dataStatus == global.fad3DataStatus.statusNew)
                {
                    sql = $@"Insert into tblAllSpecies
                           (Genus, species, ListedFB, FBSpNo, Notes, TaxaNo, SpeciesGUID,
                            MPHG1, MPHG2, MPHS1, MPHS2) values (
                            '{genus}', '{species}', {inFishbase}, {fbNo}, '{notes}', {(int)taxa}, {nameGuid},
                             {genusMPH1}, {genusMPH2}, {speciesMPH1}, {speciesMPH2})";

                    using (OleDbCommand update = new OleDbCommand(sql, conn))
                    {
                        Success = update.ExecuteNonQuery() > 0;
                    }
                }
                else if (dataStatus == global.fad3DataStatus.statusEdited)
                {
                    sql = $@"Update tblAllSpecies set
                         Genus = '{genus}',
                         species = '{species}',
                         ListedFB = {inFishbase},
                         FBSpNo = {fishBaseSpeciesNo},
                         Notes = '{notes}',
                         TaxaNo = {(int)taxa},
                         MPHG1 - {genusMPH1},
                         MPHG2 - {genusMPH2},
                         MPHS1 - {speciesMPH1},
                         MPHS2 - {speciesMPH2}
                         WHERE SpeciesGUID = {{{nameGuid}}}";

                    using (OleDbCommand update = new OleDbCommand(sql, conn))
                    {
                        Success = update.ExecuteNonQuery() > 0;
                    }
                }
                else if (dataStatus == global.fad3DataStatus.statusForDeletion)
                {
                    sql = $"Delete * from tblAllSpecies where SpeciesGUID = {{{nameGuid}}}";
                    using (OleDbCommand update = new OleDbCommand(sql, conn))
                    {
                        Success = update.ExecuteNonQuery() > 0;
                    }
                }
            }
            return Success;
        }

        public static Dictionary<string, CatchName> RetrieveScientificNames(Dictionary<string, string> filters = null, bool selectOnlyWithRecords = false)
        {
            var catchNames = new Dictionary<string, CatchName>();
            var filter = "";
            if (filters != null)
            {
                foreach (var item in filters)
                {
                    if (filter.Length == 0)
                        filter += item.Value;
                    else
                        filter += $" and {item.Value}";
                }
            }

            if (selectOnlyWithRecords)
            {
                const string filter2 = " (SELECT Count(NameGUID) AS n FROM tblCatchComp GROUP BY NameGUID HAVING NameGUID = [main.SpeciesGUID])>0 ";
                if (filter.Length == 0)
                    filter = filter2;
                else
                    filter += $" and {filter2}";
            }

            if (filter.Length > 0) filter = $" WHERE {filter}";

            string sql = $"SELECT *, (SELECT Count(NameGUID) AS n FROM tblCatchComp GROUP BY NameGUID HAVING NameGUID = [main.SpeciesGUID]) AS n FROM tblAllSpecies AS main {filter} Order by Genus, species;";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var adapter = new OleDbDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    var taxa = (CatchName.Taxa)Enum.Parse(typeof(CatchName.Taxa), dr["TaxaNo"].ToString());
                    var speciesGuid = dr["SpeciesGUID"].ToString();
                    int? fbSpNo = null;
                    if (int.TryParse(dr["FBSpNo"].ToString(), out int spNo))
                        fbSpNo = spNo;

                    int catchCompositionRecordCount = 0;

                    if (dr["n"].ToString().Length > 0)
                        catchCompositionRecordCount = (int)dr["n"];

                    try
                    {
                        if (dr["MPHG1"].ToString().Length > 0)
                        {
                            catchNames.Add(speciesGuid, new CatchName(speciesGuid, dr["Genus"].ToString(), dr["species"].ToString(),
                                taxa, (bool)dr["ListedFB"], fbSpNo, dr["Notes"].ToString(), catchCompositionRecordCount,
                                (short)dr["MPHG1"], (short)dr["MPHG2"], (short)dr["MPHS1"], (short)dr["MPHS2"]));
                        }
                        else
                        {
                            catchNames.Add(speciesGuid, new CatchName(speciesGuid, dr["Genus"].ToString(), dr["species"].ToString(),
                                taxa, (bool)dr["ListedFB"], fbSpNo, dr["Notes"].ToString(), catchCompositionRecordCount));
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex.Message);
                    }
                }
            }

            return catchNames;
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
            short? genusKey1, short? genusKey2, short? speciesKey1, short? speciesKey2, CatchName.Taxa taxa)
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
            CatchName.Taxa taxa = CatchName.Taxa.Fish;
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
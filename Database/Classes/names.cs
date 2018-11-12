﻿//using ADOX;
using dao;
using FAD3.Database.Classes;
using FAD3.GUI.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;
using MetaphoneCOM;
using HtmlAgilityPack;

namespace FAD3
{
    internal static class Names
    {
        private static List<string> _genusList = new List<string>();
        private static List<string> _localNameList = new List<string>();
        private static Dictionary<string, string> _localNameListDict = new Dictionary<string, string>();
        private static Dictionary<string, string> _speciesList = new Dictionary<string, string>();
        private static string _genus = "";
        private static long _localNamesCount = 0;
        private static long _sciNamesCount = 0;
        private static Dictionary<string, string> _languages = new Dictionary<string, string>();
        private static Dictionary<string, string> _allSpeciesDictionary = new Dictionary<string, string>();
        private static Dictionary<string, string> _allSpeciesDictionaryReverse = new Dictionary<string, string>();
        private static Dictionary<string, string> _localNameListDictReverse = new Dictionary<string, string>();
        private static Dictionary<string, string> _languageDictReverse = new Dictionary<string, string>();
        private static List<string> _listSpeciesNames = new List<string>();

        public static Dictionary<string, string> LocalNamesReverseDictionary
        {
            get { return _localNameListDictReverse; }
        }

        public static Dictionary<string, string> AllSpeciesDictionary
        {
            get
            {
                if (_allSpeciesDictionary.Count == 0)
                {
                    GetAllSpecies();
                }
                return _allSpeciesDictionary;
            }
        }

        public static bool DeleteLocalNameSpeciesNamePair(string localName, string speciesName, string language)
        {
            string languageGUID = _languageDictReverse[language];
            string speciesGUID = _allSpeciesDictionaryReverse[speciesName];
            string localNameGuid = _localNameListDictReverse[localName];
            var success = false;
            using (var conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var sql = $@"Delete * from tblLocalNamesScientific where
                             LocalNameGuid = {{{localNameGuid}}} AND
                             LanguageGuid = {{{languageGUID}}} AND
                             ScientificNameGuid = {{{speciesGUID}}}";

                using (OleDbCommand deleteOther = new OleDbCommand(sql, conn))
                {
                    success = deleteOther.ExecuteNonQuery() > 0;
                }
            }
            return success;
        }

        public static Dictionary<string, string> Languages
        {
            get
            {
                if (_languages.Count == 0)
                {
                    GetLanguages();
                }
                return _languages;
            }
        }

        public static string Genus
        {
            get { return _genus; }
            set
            {
                _genus = value;
                GetSpecies();
            }
        }

        public static long NamesCount
        {
            get { return _localNamesCount + _sciNamesCount; }
        }

        public static long LocalNamesCount
        {
            get { return _localNamesCount; }
        }

        public static long SciNamesCount
        {
            get { return _sciNamesCount; }
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

        private static bool DeleteAllCatchLocalNames()
        {
            var nameCount = 0;
            var namesDeleted = 0;
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                string sql = "SELECT Count(tblBaseLocalNames.Name) AS n FROM tblBaseLocalNames";
                using (OleDbCommand getCount = new OleDbCommand(sql, conn))
                {
                    nameCount = (int)getCount.ExecuteScalar();
                }

                sql = "Delete * from tblBaseLocalNames";
                using (OleDbCommand delete = new OleDbCommand(sql, conn))
                {
                    try
                    {
                        namesDeleted = delete.ExecuteNonQuery();
                    }
                    catch
                    {
                        //ignore
                    }
                }
            }

            return nameCount == namesDeleted;
        }

        private static void RefreshLocalNamesList()
        {
            _localNameList.Clear();
            const string sql = "Select Name from tblBaseLocalNames order by Name";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var adapter = new OleDbDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                for (int n = 0; n < dt.Rows.Count; n++)
                {
                    DataRow dr = dt.Rows[n];
                    _localNameList.Add(dr["Name"].ToString());
                }
            }
        }

        public static int ImportLocalNamesFromFile(string fileName)
        {
            GetLocalNames();
            var n = 0;
            const Int32 BufferSize = 512;
            using (var fileStream = File.OpenRead(fileName))
            {
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                {
                    String line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        if (!_localNameListDictReverse.ContainsKey(line))
                        {
                            NewFisheryObjectName nfon = new NewFisheryObjectName(line, FisheryObjectNameType.CatchLocalName);
                            SaveNewLocalName(nfon);
                            n++;
                        }
                    }
                }
            }

            if (n > 0)
            {
                MakeAllNames();
            }
            return n;
        }

        public static List<string> GetLocalNameFromSpeciesNameLanguage(string speciesNameGuid, string languageGuid)
        {
            List<string> list = new List<string>();
            string sql = $@"SELECT tblBaseLocalNames.Name
                            FROM tblBaseLocalNames INNER JOIN
                              tblLocalNamesScientific ON
                              tblBaseLocalNames.NameNo = tblLocalNamesScientific.LocalNameGuid
                            WHERE tblLocalNamesScientific.ScientificNameGuid={{{speciesNameGuid}}} AND
                              tblLocalNamesScientific.LanguageGuid={{{languageGuid}}}";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var adapter = new OleDbDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                for (int n = 0; n < dt.Rows.Count; n++)
                {
                    DataRow dr = dt.Rows[n];
                    list.Add(dr["Name"].ToString());
                }
            }

            return list;
        }

        public static List<(string genus, string species)> GetSpeciesNameFromLocalNameLanguage(string localNameGuid, string languageGuid)
        {
            List<(string genus, string species)> list = new List<(string genus, string species)>();
            string sql = $@"SELECT tblAllSpecies.Genus, tblAllSpecies.species
                            FROM tblAllSpecies
                              INNER JOIN tblLocalNamesScientific ON
                              tblAllSpecies.SpeciesGUID = tblLocalNamesScientific.ScientificNameGuid
                            WHERE tblLocalNamesScientific.LocalNameGuid={{{localNameGuid}}} AND
                              tblLocalNamesScientific.LanguageGuid={{{languageGuid}}}
                            Order by tblAllSpecies.Genus, tblAllSpecies.species";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var adapter = new OleDbDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                for (int n = 0; n < dt.Rows.Count; n++)
                {
                    DataRow dr = dt.Rows[n];
                    list.Add((dr["Genus"].ToString(), dr["species"].ToString()));
                }
            }

            return list;
        }

        public static int ImportFromHTMLLocalNamestoScientificNames(string fileName, int speciesColumn, int localNameColumn, int languageColumn)
        {
            GetLanguages();
            var speciesName = "";
            var localName = "";
            var language = "";
            HtmlDocument doc = new HtmlDocument();
            int col = 0;
            int savedCount = 0;
            doc.Load(fileName);
            foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table"))
            {
                foreach (HtmlNode body in table.SelectNodes("tbody"))
                {
                    foreach (HtmlNode row in body.SelectNodes("tr"))
                    {
                        col = 0;
                        speciesName = "";
                        localName = "";
                        language = "";
                        foreach (HtmlNode cell in row.SelectNodes("th|td"))
                        {
                            if (col == speciesColumn)
                            {
                                speciesName = cell.InnerText.Trim(new char[] { ' ', '\r', '\n', '\t' });
                            }
                            else if (col == localNameColumn)
                            {
                                localName = cell.InnerText.Trim(new char[] { ' ', '\r', '\n', '\t' });
                            }
                            else if (col == languageColumn)
                            {
                                language = cell.InnerText.Trim(new char[] { ' ', '\r', '\n', '\t' });
                            }
                            if (speciesName.Length > 0
                                && language.Length > 0
                                && localName.Length > 0)
                            {
                                if (!_allSpeciesDictionaryReverse.ContainsKey(speciesName))
                                {
                                    var arr = speciesName.Split(' ');
                                    var genus = "";
                                    var species = "";
                                    for (int n = 0; n < arr.Length; n++)
                                    {
                                        if (n == 0)
                                        {
                                            genus = arr[n];
                                        }
                                        else
                                        {
                                            species += arr[n] + " ";
                                        }
                                    }
                                    species = species.Trim();
                                    SaveNewFishSpeciesName(genus, species);
                                }

                                if (!_languageDictReverse.ContainsKey(language))
                                {
                                    SaveNewLanguage(language);
                                }

                                localName = localName.ToLower();
                                if (!_localNameListDictReverse.ContainsKey(localName))
                                {
                                    NewFisheryObjectName nfo = new NewFisheryObjectName(localName, FisheryObjectNameType.CatchLocalName);
                                    SaveNewLocalName(nfo);
                                }

                                if (SaveNewLocalSpeciesNameLanguage(_allSpeciesDictionaryReverse[speciesName], _languageDictReverse[language], _localNameListDictReverse[localName]))
                                {
                                    savedCount++;
                                }
                                break;
                            }
                            else
                            {
                                col++;
                            }
                        }
                    }
                }
            }
            MakeAllNames();
            return savedCount;
        }

        public static (bool success, string guid) SaveNewLanguage(string language)
        {
            string sql;
            bool success = false;
            string guid = Guid.NewGuid().ToString();
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();

                sql = $@"Insert into tblLanguages
                           (LanguageUsedGuid, LanguageUsed) values (
                            {{{guid}}}, '{language}')";

                using (OleDbCommand update = new OleDbCommand(sql, conn))
                {
                    try
                    {
                        success = update.ExecuteNonQuery() > 0;
                        if (success)
                        {
                            _languages.Add(guid, language);
                            _languageDictReverse.Add(language, guid);
                        }
                    }
                    catch
                    {
                        //ignore
                    }
                }
            }
            return (success, guid);
        }

        public static int ImportLocalNamestoScientificNames(string fileName, ref int fail, bool isHTML = false)
        {
            Logger.Log("import local names - species name pair begin " + DateTime.Now.ToLongTimeString());
            var n = 0;
            var f = 0;
            const Int32 BufferSize = 1024;
            var speciesKey = "";
            var languageKey = "";
            var localNameKey = "";
            using (var fileStream = File.OpenRead(fileName))
            {
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                {
                    String line;
                    if (AllSpeciesDictionary.Count > 0 && Languages.Count > 0 && LocalNameListDict.Count > 0)
                    {
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            var arr = line.Split('\t');
                            var localName = arr[0].Trim();
                            var language = arr[1].Trim();
                            var species = arr[2];
                            localNameKey = "";
                            speciesKey = "";
                            languageKey = "";
                            if (_localNameListDictReverse.ContainsKey(localName))
                            {
                                localNameKey = _localNameListDictReverse[localName];
                            }
                            if (_allSpeciesDictionaryReverse.ContainsKey(species))
                            {
                                speciesKey = _allSpeciesDictionaryReverse[species];
                            }
                            languageKey = _languageDictReverse[language];

                            if (speciesKey.Length > 0 && localNameKey.Length > 0 && languageKey.Length > 0
                                && SaveNewLocalSpeciesNameLanguage(speciesKey, languageKey, localNameKey))
                            {
                                n++;
                            }
                            else
                            {
                                f++;
                            }
                        }
                    }
                }
            }
            Logger.Log("import local names - species name pair end " + DateTime.Now.ToLongTimeString());
            fail = f;
            return n;
        }

        public static void RefreshLanguages()
        {
            GetLanguages();
        }

        public static void SaveNewLocalSpeciesNameLanguage(string speciesName, string language, string localName, out bool success)
        {
            string languageGUID = _languageDictReverse[language];
            string speciesGUID = _allSpeciesDictionaryReverse[speciesName];
            string localNameGuid = _localNameListDictReverse[localName];

            success = SaveNewLocalSpeciesNameLanguage(speciesGUID, languageGUID, localNameGuid);
        }

        public static bool SaveNewLocalSpeciesNameLanguage(string speciesNameGuid, string languageGuid, string localNameGuid)
        {
            bool success = false;
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var sql = $@"Insert into tblLocalNamesScientific (LocalNameGuid, LanguageGuid, ScientificNameGuid,RowId) values
                        ({{{localNameGuid}}}, {{{languageGuid}}},{{{speciesNameGuid}}},{{{Guid.NewGuid().ToString()}}})";
                using (OleDbCommand update = new OleDbCommand(sql, conn))
                {
                    try
                    {
                        success = update.ExecuteNonQuery() > 0;
                    }
                    catch
                    {
                        // ignore success = false;
                    }
                }
            }
            return success;
        }

        private static Dictionary<string, string> GetLanguages()
        {
            _languages.Clear();
            _languageDictReverse.Clear();
            const string sql = "Select LanguageUsedGuid, LanguageUsed from tblLanguages Order By LanguageUsed";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var adapter = new OleDbDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                for (int n = 0; n < dt.Rows.Count; n++)
                {
                    DataRow dr = dt.Rows[n];
                    _languages.Add(dr["LanguageUsedGuid"].ToString().Trim(), dr["LanguageUsed"].ToString().Trim());
                    _languageDictReverse.Add(dr["LanguageUsed"].ToString().Trim(), dr["LanguageUsedGuid"].ToString().Trim());
                }
            }

            return _languages;
        }

        public static Dictionary<string, string> LocalNamesFromSpeciesNameLanguage(string genus, string species, string languageUsed)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string sql = $@"SELECT tblBaseLocalNames.Name, tblBaseLocalNames.NameNo
                            FROM tblAllSpecies INNER JOIN
                                (tblLanguages INNER JOIN
                                (tblBaseLocalNames INNER JOIN
                                tblLocalNamesScientific ON
                                tblBaseLocalNames.NameNo = tblLocalNamesScientific.LocalNameGuid) ON
                                tblLanguages.LanguageUsedGuid = tblLocalNamesScientific.LanguageGuid) ON
                                tblAllSpecies.SpeciesGUID = tblLocalNamesScientific.ScientificNameGuid
                            WHERE tblLanguages.LanguageUsed='{languageUsed}' AND
                                tblAllSpecies.Genus='{genus}' AND tblAllSpecies.species = '{species}'";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var adapter = new OleDbDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                for (int n = 0; n < dt.Rows.Count; n++)
                {
                    DataRow dr = dt.Rows[n];
                    dict.Add(dr["NameNo"].ToString().Trim(), dr["Name"].ToString().Trim());
                }
            }
            return dict;
        }

        public static bool UpdateLocalName(string editedName, string nameGuid)
        {
            bool success = false;
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();

                string sql = $@"Update tblBaseLocalNames set [Name] = ""{editedName}"" where NameNo = {{{nameGuid}}}";

                using (OleDbCommand update = new OleDbCommand(sql, conn))
                {
                    try
                    {
                        success = update.ExecuteNonQuery() > 0;
                        if (success)
                        {
                            sql = $@"Update temp_AllNames set Name1 = ""{editedName}"" where NameNo = {{{nameGuid}}}";
                            using (OleDbCommand updateAllNames = new OleDbCommand(sql, conn))
                            {
                                try
                                {
                                    success = updateAllNames.ExecuteNonQuery() > 0;
                                }
                                catch
                                {
                                    success = false;
                                }
                            }
                        }
                    }
                    catch
                    {
                        //ignore
                    }
                }
            }
            return success;
        }

        public static List<String> ScientificNamesFromLocalNameLanguage(string localName, string languageUsed)
        {
            List<string> list = new List<string>();
            string sql = $@"SELECT tblAllSpecies.Genus, tblAllSpecies.species
                            FROM tblAllSpecies INNER JOIN
                                (tblLanguages INNER JOIN (tblBaseLocalNames INNER JOIN
                                tblLocalNamesScientific ON tblBaseLocalNames.NameNo = tblLocalNamesScientific.LocalNameGuid) ON
                                tblLanguages.LanguageUsedGuid = tblLocalNamesScientific.LanguageGuid) ON
                                tblAllSpecies.SpeciesGUID = tblLocalNamesScientific.ScientificNameGuid
                            WHERE tblLanguages.LanguageUsed='{languageUsed}' AND tblBaseLocalNames.Name='{localName}'
                            Order by Genus, species";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var adapter = new OleDbDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                for (int n = 0; n < dt.Rows.Count; n++)
                {
                    DataRow dr = dt.Rows[n];
                    list.Add(dr["Genus"].ToString().Trim() + " " + dr["species"].ToString().Trim());
                }
            }
            return list;
        }

        public static int ImportSpeciesNamesFromHTML(string filename, int speciesColumn)
        {
            GetAllSpecies();
            var k = 0;
            var genus = "";
            var species = "";
            ListFishSpecies();
            HtmlDocument doc = new HtmlDocument();
            int col = 0;
            doc.Load(filename);
            foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table"))
            {
                foreach (HtmlNode body in table.SelectNodes("tbody"))
                {
                    foreach (HtmlNode row in body.SelectNodes("tr"))
                    {
                        col = 0;
                        foreach (HtmlNode cell in row.SelectNodes("th|td"))
                        {
                            if (col == speciesColumn)
                            {
                                var arr = cell.InnerText.Split(' ');
                                genus = "";
                                species = "";
                                for (int n = 0; n < arr.Length; n++)
                                {
                                    if (n == 0)
                                    {
                                        genus = arr[n];
                                    }
                                    else
                                    {
                                        species += arr[n] + " ";
                                    }
                                }
                                species = species.Trim(' ');

                                if (!_allSpeciesDictionaryReverse.ContainsKey($"{genus} {species}")
                                    && SaveNewFishSpeciesName(genus, species))
                                {
                                    k++;
                                }
                                break;
                            }
                            col++;
                        }
                    }
                }
            }
            MakeAllNames();
            return k;
        }

        public static int ImportSpeciesNamesFromFile(string fileName, bool withTaxa = false)
        {
            var n = 0;
            const Int32 BufferSize = 512;
            Taxa speciesTaxa;

            using (var fileStream = File.OpenRead(fileName))
            {
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                {
                    String line;
                    //var spList = ListFishSpecies();
                    GetAllSpecies();
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        var arr = line.Split('\t');
                        var genus = arr[0].Trim();
                        var species = arr[1].Trim();
                        var taxa = "";
                        Taxa myTaxa = Taxa.Fish;
                        if (withTaxa)
                        {
                            taxa = arr[2].Trim();
                            if (!Enum.TryParse(taxa, true, out myTaxa))
                            {
                                myTaxa = Taxa.To_be_determined;
                            }
                        }
                        if (!_allSpeciesDictionaryReverse.ContainsKey($"{genus} {species}"))
                        {
                            short genusKey1 = 0;
                            short genusKey2 = 0;
                            short speciesKey1 = 0;
                            short speciesKey2 = 0;
                            int? spNumberFishbase = null;
                            if (myTaxa == Taxa.Fish)
                            {
                                spNumberFishbase = GetFishBaseSpeciesNumber(genus, species);
                            }
                            var mph = new DoubleMetaphoneShort();
                            mph.ComputeMetaphoneKeys(genus, out genusKey1, out genusKey2);
                            mph.ComputeMetaphoneKeys(species, out speciesKey1, out speciesKey2);
                            if (UpdateSpeciesData(fad3DataStatus.statusNew,
                                                  Guid.NewGuid().ToString(),
                                                  genus,
                                                  species,
                                                  myTaxa,
                                                  genusKey1,
                                                  genusKey2,
                                                  speciesKey1,
                                                  speciesKey2,
                                                  myTaxa != Taxa.Fish ? false : spNumberFishbase != null,
                                                  spNumberFishbase,
                                                  ""))
                            {
                                n++;
                            }
                        }
                    }
                }
            }
            return n;
        }

        private static List<string> ListFishSpecies()
        {
            var sql = $"SELECT Genus, species from tblAllSpecies where TaxaNo = {(int)Taxa.Fish}";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var adapter = new OleDbDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                for (int n = 0; n < dt.Rows.Count; n++)
                {
                    DataRow dr = dt.Rows[n];
                    _listSpeciesNames.Add(dr["Genus"].ToString() + " " + dr["species"].ToString());
                }
            }
            return _listSpeciesNames;
        }

        public static Dictionary<string, string> GetSimilarSoundingLocalNames(NewFisheryObjectName newName)
        {
            Dictionary<string, string> similarNames = new Dictionary<string, string>();
            var key1 = newName.Key1;
            var key2 = newName.Key2;
            var sql = $@"SELECT tblBaseLocalNames.NameNo, tblBaseLocalNames.Name
                         FROM tblBaseLocalNames
                         WHERE tblBaseLocalNames.MPH1={key1} AND tblBaseLocalNames.MPH2={key2}";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var adapter = new OleDbDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    similarNames.Add(dr["NameNo"].ToString(), dr["Name"].ToString());
                }
            }
            return similarNames;
        }

        private static bool SaveNewFishSpeciesName(string genus, string species)
        {
            bool success = false;
            DoubleMetaphoneShort mph = new DoubleMetaphoneShort();
            short spKey1 = 0;
            short spKey2 = 0;
            short gnKey1 = 0;
            short gnKey2 = 0;
            string guid = Guid.NewGuid().ToString();
            var notes = "";
            int? spNumber = GetFishBaseSpeciesNumber(genus, species);
            var fbNo = spNumber == null ? "null" : spNumber.ToString();
            mph.ComputeMetaphoneKeys(genus, out gnKey1, out gnKey2);
            mph.ComputeMetaphoneKeys(species, out spKey1, out spKey2);

            var sql = $@"Insert into tblAllSpecies
                           (Genus, species, ListedFB, FBSpNo, Notes, TaxaNo, SpeciesGUID,
                            MPHG1, MPHG2, MPHS1, MPHS2) values (
                            '{genus}', '{species}', {true}, {fbNo}, '{notes}', {(int)Taxa.Fish}, {{{guid}}},
                             {gnKey1}, {gnKey2}, {spKey1}, {spKey2})";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                using (OleDbCommand update = new OleDbCommand(sql, conn))
                {
                    try
                    {
                        success = update.ExecuteNonQuery() > 0;
                        if (success)
                        {
                            _listSpeciesNames.Add(genus + " " + species);
                            _allSpeciesDictionary.Add(guid, genus + " " + species);
                            _allSpeciesDictionaryReverse.Add(genus + " " + species, guid);
                        }
                    }
                    catch
                    {
                        //ignore
                    }
                }
            }
            return success;
        }

        public static bool UpdateSpeciesData(fad3DataStatus dataStatus, string nameGuid, string genus, string species, Taxa taxa,
                              short genusMPH1, short genusMPH2, short speciesMPH1, short speciesMPH2, bool inFishbase, int? fishBaseSpeciesNo, string notes)
        {
            var sql = "";
            var Success = false;
            var fbNo = fishBaseSpeciesNo == null ? "null" : fishBaseSpeciesNo.ToString();
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                if (dataStatus == fad3DataStatus.statusNew)
                {
                    sql = $@"Insert into tblAllSpecies
                           (Genus, species, ListedFB, FBSpNo, Notes, TaxaNo, SpeciesGUID,
                            MPHG1, MPHG2, MPHS1, MPHS2) values (
                            '{genus}', '{species}', {inFishbase}, {fbNo}, '{notes}', {(int)taxa}, {{{nameGuid}}},
                             {genusMPH1}, {genusMPH2}, {speciesMPH1}, {speciesMPH2})";

                    using (OleDbCommand update = new OleDbCommand(sql, conn))
                    {
                        try
                        {
                            Success = update.ExecuteNonQuery() > 0;
                            if (Success)
                            {
                                _listSpeciesNames.Add(genus + " " + species);
                                _allSpeciesDictionaryReverse.Add(genus + " " + species, nameGuid);
                                _allSpeciesDictionary.Add(nameGuid, genus + " " + species);
                            }
                        }
                        catch
                        {
                            //ignore
                        }
                    }
                }
                else if (dataStatus == fad3DataStatus.statusEdited)
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
                else if (dataStatus == fad3DataStatus.statusForDeletion)
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
                    var taxa = (Taxa)Enum.Parse(typeof(Taxa), dr["TaxaNo"].ToString());
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

        public static int? GetFishBaseSpeciesNumber(string genus, string species)
        {
            var sql = $"Select SpecCode from FBSpecies where Genus = '{genus}' AND Species = '{species}'";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                using (OleDbCommand spNumber = new OleDbCommand(sql, conn))
                {
                    return (int?)spNumber.ExecuteScalar();
                }
            }
        }

        public static (bool isFound, bool inFishbase, int? fishBaseNo, string notes,
            short? genusKey1, short? genusKey2, short? speciesKey1, short? speciesKey2, Taxa taxa)
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
            Taxa taxa = Taxa.Fish;
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

        public static void GetAllSpecies()
        {
            DataTable dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    const string query = "SELECT Genus, species, SpeciesGUID FROM tblAllSpecies Order by Genus, species";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    _allSpeciesDictionary.Clear();
                    _allSpeciesDictionaryReverse.Clear();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        try
                        {
                            _allSpeciesDictionary.Add(dr["SpeciesGUID"].ToString(), dr["Genus"].ToString().Trim(' ') + " " + dr["species"].ToString().Trim(' '));
                            _allSpeciesDictionaryReverse.Add(dr["Genus"].ToString().Trim(' ') + " " + dr["species"].ToString().Trim(' '), dr["SpeciesGUID"].ToString());
                        }
                        catch (Exception ex1)
                        {
                            Logger.Log(ex1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
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
                                      WHERE Name1 = '{_genus}'  ORDER BY Name2";
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

        public static (bool success, string newGuid) SaveNewLocalName(NewFisheryObjectName newName)
        {
            var newLocalName = newName.NewName.Trim();
            var key1 = newName.Key1;
            var key2 = newName.Key2;
            var guid = newName.ObjectGUID;
            bool success = false;
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var sql = $@"Insert into tblBaseLocalNames (Name, NameNo, MPH1,MPH2) values
                        (""{newLocalName}"", {{{guid}}},{key1},{key2})";
                using (OleDbCommand update = new OleDbCommand(sql, conn))
                {
                    if (!_localNameListDictReverse.ContainsKey(newLocalName))
                    {
                        try
                        {
                            success = update.ExecuteNonQuery() > 0;
                        }
                        catch
                        {
                            //ignore
                        }
                        if (success)
                        {
                            _localNameList.Add(newLocalName);
                            _localNameListDict.Add(guid, newLocalName);
                            _localNameListDictReverse.Add(newLocalName, guid);
                        }
                    }
                }
            }
            return (success, guid);
        }

        public static void GetLocalNames()
        {
            _localNameListDict.Clear();
            _localNameListDictReverse.Clear();
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
                        _localNameListDict.Add(dr["NameNo"].ToString(), dr["Name1"].ToString());
                        _localNameListDictReverse.Add(dr["Name1"].ToString(), dr["NameNo"].ToString());
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
            _localNameList.Clear();
            _genusList.Clear();
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
                            _localNameList.Add(dr["Name1"].ToString());
                        }
                        else
                        {
                            _genusList.Add(dr["Name1"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
        }

        public static Dictionary<string, string> SpeciesList
        {
            get { return _speciesList; }
        }

        public static List<string> GenusList
        {
            get { return _genusList; }
        }

        public static Dictionary<string, string> LocalNameListDict
        {
            get
            {
                GetLocalNames();
                return _localNameListDict;
            }
        }

        public static List<string> LocalNameList
        {
            get { return _localNameList; }
        }
    }
}
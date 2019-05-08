using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Reflection;

namespace FAD3.Database.Classes
{
    public static class LGUs
    {
        /// <summary>
        /// helper class for shortening strings by removing all vowels and dashes except the first vowel
        /// </summary>
        /// <param name="s"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static string RemoveInnerVowels(string s, int length = 3)
        {
            s = s.Substring(1, s.Length - 1);
            string r = "";
            int counter = 0;
            for (int n = 0; n < s.Length; n++)
            {
                if (s[n] != 'a' && s[n] != 'e' && s[n] != 'i' && s[n] != 'o' && s[n] != 'u' && s[n] != '-')
                {
                    if (r.Length == 0)
                    {
                        r += s[n];
                        counter++;
                    }
                    else if (r[r.Length - 1] != s[n])
                    {
                        r += s[n];
                        counter++;
                    }
                    if (counter == length)
                    {
                        break;
                    }
                }
            }
            if (s.Length == 1)
            {
                r += s[0];
            }
            return r;
        }

        /// <summary>
        /// shortens strings by removing all vowels from a string except if the letter is a vowel
        /// </summary>
        /// <param name="placeName"></param>
        /// <returns></returns>
        public static string ShortenPlaceName(string placeName, int length = 4)
        {
            var name = placeName.Split(' ', '-');
            if (name.Length == 1)
            {
                return name[0].Substring(0, 1) + RemoveInnerVowels(name[0], length - 1);
            }
            else
            {
                string nm = "";
                for (int n = 0; n < name.Length; n++)
                {
                    if (nm.Length < length && name[n].Length > 1 && name[n].Substring(1, 1) != ".")
                    {
                        nm += name[n].Substring(0, 1) + RemoveInnerVowels(name[n], 1);
                    }
                }
                return nm;
            }
        }

        public static Dictionary<int, (string provinceName, Dictionary<int, string> lgus)> GetLGUTree()
        {
            Dictionary<int, (string provinceName, Dictionary<int, string> lgus)> tree = new Dictionary<int, (string provinceName, Dictionary<int, string> lgus)>();
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    const string query = @"SELECT Provinces.ProvNo,
                                             Provinces.ProvinceName
                                          From Provinces
                                          ORDER BY ProvinceName";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        int provinceNumber = (int)dr["ProvNo"];
                        var lgus = lgusFromProvince(provinceNumber);
                        tree.Add(provinceNumber, (dr["ProvinceName"].ToString(), lgus));
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
                return tree;
            }
        }

        public static bool AddProvince(string provinceName, int region)
        {
            bool success = false;
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var sql = $@"Insert into Provinces (ProvinceName, ProvNo, Region) values
                             (
                                {provinceName},
                                {Guid.NewGuid().ToString()},
                                {region}
                             )";
                using (OleDbCommand update = new OleDbCommand(sql, conn))
                {
                    success = update.ExecuteNonQuery() > 0;
                }
            }
            return success;
        }

        public static bool MoveLGuToProvince(int LGUNumber, int ProvinceNumber)
        {
            bool success = false; ;
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var sql = $@"Update Municipalities set
                                ProvNo = {ProvinceNumber}
                            WHERE MunNo={LGUNumber}";
                using (OleDbCommand update = new OleDbCommand(sql, conn))
                {
                    success = update.ExecuteNonQuery() > 0;
                }
            }
            return success;
        }

        private static Dictionary<int, string> lgusFromProvince(int provinceNumber)
        {
            Dictionary<int, string> lgus = new Dictionary<int, string>();
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $"SELECT Municipality, MunNo from Municipalities Where ProvNo = {provinceNumber} Order By Municipality ";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        lgus.Add((int)dr["MunNo"], dr["Municipality"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }
            return lgus;
        }
    }
}
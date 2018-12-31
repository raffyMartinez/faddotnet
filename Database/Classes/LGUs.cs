using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Reflection;

namespace FAD3.Database.Classes
{
    public static class LGUs
    {
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
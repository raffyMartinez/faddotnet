using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Reflection;

namespace FAD3.Database.Classes
{
    public class CatchTaxa
    {
        public static Dictionary<int, string> Taxa { get; internal set; } = new Dictionary<int, string>();

        static CatchTaxa()
        {
            ReadTaxa();
        }

        private static void ReadTaxa()
        {
            const string sql = "SELECT TaxaNo, Taxa from tblTaxa";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conn.Open();
                    var adapter = new OleDbDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    for (int n = 0; n < dt.Rows.Count; n++)
                    {
                        DataRow dr = dt.Rows[n];
                        Taxa.Add((int)dr["TaxaNo"], dr["Taxa"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }
        }
    }
}
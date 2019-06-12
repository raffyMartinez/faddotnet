using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace FAD3.Database.Classes
{
    public class TaxaCategory
    {
        public static bool AddTaxa(int taxaNumber, string taxaName)
        {
            bool success = false;
            using (var conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var sql = $@"Insert into tblTaxa (TaxaNo, Taxa)
                         values({taxaNumber}, '{taxaName}')";

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
                        Logger.Log(ex.Message, "GearCLass", "AddGearClass");
                    }
                }
            }
            return success;
        }
    }
}
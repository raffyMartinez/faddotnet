using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace FAD3.Database.Classes
{
    public class GearClass
    {
        public string GearClassName { get; set; }
        public string GearClassLetter { get; set; }
        public string GearClassGUID { get; set; }

        public GearClass(string className, string classLetter, string classGUID)
        {
            GearClassName = className;
            GearClassLetter = classLetter;
            GearClassGUID = classGUID;
        }

        public static bool AddGearClass(string className, string classLetter, string classGuid)
        {
            bool success = false;
            using (var conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var sql = $@"Insert into tblGearClass (GearClass, GearClassName, GearLetter)
                         values({{{classGuid}}}, '{className}', {classLetter})";

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
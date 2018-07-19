using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using ADOX;

namespace FAD3
{
    public static class gear
    {
        static gear()
        {
            
        }

        public static string GearCodeFromGearVar(string GearVarGUID)
        {
            string myCode = "";
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = "Select RefGearCode from tblRefGearCodes where GearVar ='{" + GearVarGUID + "}'";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    DataRow dr = dt.Rows[0];
                    myCode = dr["RefGearCode"].ToString();
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
                return myCode;
            }
        }

        public static void MakeVesselTypeTable()
        {
            Catalog catMDB = new Catalog();
            catMDB.let_ActiveConnection(global.ConnectionString);


            try
            {
                catMDB.Tables.Delete("temp_VesselType");
            }
            catch
            {
                using (var conection = new OleDbConnection(global.ConnectionString))
                {

                    OleDbCommand cmd = new OleDbCommand()
                    {
                        Connection = conection,
                    };

                    conection.Open();

                    //select into query
                    string sql = "SELECT 1 AS VesselTypeNo, 'Motorized' AS VesselType into temp_VesselType";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();

                    sql = "Insert into temp_VesselType (VesselTypeNo, VesselType) values (2,'Non-Motorized')";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();

                    sql = "Insert into temp_VesselType (VesselTypeNo, VesselType) values (3,'No vessel used')";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();

                    sql = "Insert into temp_VesselType (VesselTypeNo, VesselType) values (4,'Not provided')";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();

                    conection.Close();
                    
                }
            }
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
                    string query = "Select Counter from tblRefCodeCounter where GearRefCode= '" + GearCode + "'";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        counter = long.Parse(dr["Counter"].ToString()) + 1;

                        query = "Update tblRefCodeCounter set [Counter] = " + counter +
                                " Where GearRefCode='" + GearCode + "'";
                        OleDbCommand updateCounter = new OleDbCommand(query, conection);
                        Success = updateCounter.ExecuteNonQuery() > 0;
                    }
                    else
                    {
                        Success = InsertGearCode(GearCode);
                        if(Success)
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
                    string query = "Insert into tblRefCodeCounter ([Counter], GearRefCode) values (" +
                            1 + ", '" + GearCode + "')";
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
                    string query = "Select GearLetter from tblGearClass where GearClass ='{" + GearClassGuid + "}'";
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
                    string query = "SELECT RefGearCode FROM tblGearVariations INNER JOIN tblRefGearCodes ON tblGearVariations.GearVarGUID = " +
                                   "tblRefGearCodes.GearVar WHERE tblGearVariations.GearClass= '{" + GearClassGuid + "}' " + 
                                   "ORDER BY tblRefGearCodes.RefGearCode;";
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
                    string query = "Select RefGearCode from tblRefGearCodes where GearVar ='{" + GearVariationGuid + "}'";
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

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FAD3.GUI.Classes;
using System.Data;
using System.Data.OleDb;

namespace FAD3.Database.Classes
{
    public static class LengthFreq
    {
        private static int _LFRowsCount;

        public static bool SaveEditedLF(Dictionary<string, (double len, int freq, fad3DataStatus dataStatus)> LFData, string catchRowGuid)
        {
            var saveCount = 0;
            var n = 0;
            var fromDbCount = 0;
            foreach (var item in LFData)
            {
                if (item.Value.dataStatus != fad3DataStatus.statusFromDB)
                {
                    if (UpdateLF(item.Value.len, item.Value.freq, n, catchRowGuid, item.Key, item.Value.dataStatus))
                    {
                        saveCount++;
                    }
                }
                else
                {
                    fromDbCount++;
                }
                n++;
            }
            return saveCount + fromDbCount == LFData.Count;
        }

        public static Dictionary<string, (double len, int freq, fad3DataStatus dataStatus)> LFData(string CatchCompRowNo)
        {
            _LFRowsCount = 0;
            Dictionary<string, (double len, int freq, fad3DataStatus dataStatus)> mydata = new Dictionary<string, (double len, int freq, fad3DataStatus dataStatus)>();
            var dt = new DataTable();
            using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;data source=" + global.MDBPath))
            {
                try
                {
                    conection.Open();
                    string query = $"SELECT RowGUID, Sequence, LenClass, Freq FROM tblLF WHERE tblLF.CatchCompRow={{{CatchCompRowNo}}} ORDER BY Sequence, LenClass";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];

                        var len = (double)dr["lenClass"];
                        mydata.Add
                        (
                            dr["RowGUID"].ToString(),
                            (len,
                            (int)dr["Freq"],
                            fad3DataStatus.statusFromDB)
                        );
                    }
                    _LFRowsCount++;
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }

            return mydata;
        }

        private static bool UpdateLF(double lenClass, long classCount, int sequence,
                                      string catchCompRow, string rowGUID, fad3DataStatus dataStatus)
        {
            bool success = false;
            string query = "";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    if (dataStatus == fad3DataStatus.statusNew)
                    {
                        query = $@"Insert into tblLF (LenClass, Freq, CatchCompRow, RowGUID, Sequence) values
                                    ({lenClass}, {classCount}, {{{catchCompRow}}}, {{{rowGUID}}}, {sequence})";
                    }
                    else if (dataStatus == fad3DataStatus.statusEdited)
                    {
                        query = $@"Update tblLF set
                                   LenClass = {lenClass},
                                   Freq= {classCount},
                                   Sequence= {sequence}
                                   Where RowGUID ={{{rowGUID}}}";
                    }
                    else if (dataStatus == fad3DataStatus.statusForDeletion)
                    {
                        query = $"Delete * from tblLF where RowGUID = {{{rowGUID}}}";
                    }
                    if (query.Length > 0)
                    {
                        OleDbCommand update = new OleDbCommand(query, conn);
                        conn.Open();
                        success = (update.ExecuteNonQuery() > 0);
                        conn.Close();
                    }
                    else
                    {
                        success = true;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
            return success;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace FAD3.Database.Classes
{
    public static class TreeLevels
    {
        public static List<TreeLevel> TreeLevelsList { get; internal set; } = new List<TreeLevel>();

        public static void GetLevelTargetAreaLandingSite()
        {
            TreeLevelsList.Clear();
            using (var dataTable = new DataTable())
            {
                try
                {
                    using (var conection = new OleDbConnection($"Provider=Microsoft.JET.OLEDB.4.0;data source={global.MDBPath}"))
                    {
                        conection.Open();

                        const string query =
                            @"SELECT tblAOI.AOIGuid, tblAOI.AOIName, tblLandingSites.LSGUID, tblLandingSites.LSName
                            FROM tblAOI LEFT JOIN tblLandingSites ON tblAOI.AOIGuid = tblLandingSites.AOIGuid
                            ORDER BY tblAOI.AOIName, tblLandingSites.LSName";

                        using (var adapter = new OleDbDataAdapter(query, conection))
                        {
                            adapter.Fill(dataTable);
                            for (int i = 0; i < dataTable.Rows.Count; i++)
                            {
                                DataRow dr = dataTable.Rows[i];
                                TreeLevelsList.Add(new TreeLevel(dr["AOIGuid"].ToString(),
                                           dr["AOIName"].ToString(),
                                           dr["LSGUID"].ToString(),
                                           dr["LSName"].ToString()));

                                //TreeLevelsList.Add(
                                //          (dr["AOIGuid"].ToString(),
                                //           dr["AOIName"].ToString(),
                                //           dr["LSGUID"].ToString(),
                                //           dr["LSName"].ToString())
                                //          );
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                    Logger.LogError(ex.Message, ex.StackTrace);
                }
            }
        }
    }
}
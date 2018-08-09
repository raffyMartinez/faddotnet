using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace FAD3
{
    public static class SamplingEnumerators
    {
        private static string _AOIGuid;
        private static string _EnumeratorGuid;

        static SamplingEnumerators()
        {
        }

        public static string EnumeratorGuid
        {
            get { return _EnumeratorGuid; }
            set
            {
                _EnumeratorGuid = value;
            }
        }

        public static string AOIGuid
        {
            get { return _AOIGuid; }
            set
            {
                _AOIGuid = value;
            }
        }

        public static int NumberOfSamplingsOfEnumerator(string enumeratorGuid)
        {
            var sql = $"SELECT Count(Enumerator) AS n FROM tblSampling WHERE Enumerator={{{enumeratorGuid}}}";
            using (var conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                using (OleDbCommand count = new OleDbCommand(sql, conn))
                {
                    return (int)count.ExecuteScalar();
                }
            }
        }

        public static bool SaveTargetAreaEnumerators(Dictionary<string, (string enumeratorName, DateTime dateHired, bool isActive, global.fad3DataStatus DataStatus)> enumeratorsData)
        {
            var n = 0;
            if (enumeratorsData.Count > 0)
            {
                using (var conn = new OleDbConnection(global.ConnectionString))
                {
                    conn.Open();
                    foreach (KeyValuePair<string, (string enumeratorName, DateTime dateHired, bool isActive, global.fad3DataStatus DataStatus)> kv in enumeratorsData)
                    {
                        var sql = "";
                        if (kv.Value.DataStatus == global.fad3DataStatus.statusEdited)
                        {
                            sql = $@"Update tblEnumerators set
                                EnumeratorName ='{kv.Value.enumeratorName}',
                                HireDate = '{kv.Value.dateHired}',
                                Active={kv.Value.isActive}
                                where EnumeratorID = {{{kv.Key}}}";
                        }
                        else if (kv.Value.DataStatus == global.fad3DataStatus.statusNew)
                        {
                            sql = $@"Insert into tblEnumerators (EnumeratorID, EnumeratorName, HireDate, Active, TargetArea) values (
                             {{{kv.Key}}}, '{kv.Value.enumeratorName}', '{kv.Value.dateHired}', {kv.Value.isActive}, {{{_AOIGuid}}})";
                        }
                        else if (kv.Value.DataStatus == global.fad3DataStatus.statusForDeletion)
                        {
                            sql = $"Delete * from tblEnumerators where EnumeratorID= {{{kv.Key}}}";
                        }

                        using (OleDbCommand update = new OleDbCommand(sql, conn))
                        {
                            if (update.ExecuteNonQuery() > 0) n++;
                        }
                    }
                }
            }

            return n > 0;
        }

        public static Dictionary<string, (string EnumeratorName, DateTime DateHired, bool IsActive, global.fad3DataStatus DataStatus)> GetTargetAreaEnumerators()
        {
            var myRows = new Dictionary<string, (string EnumeratorName, DateTime DateHired, bool IsActive, global.fad3DataStatus DataStatus)>();

            var sql = $@"SELECT EnumeratorID, EnumeratorName, HireDate, Active FROM tblEnumerators WHERE
                         tblEnumerators.TargetArea={{{_AOIGuid}}}";

            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                conection.Open();
                DataTable dt = new DataTable();
                var adapter = new OleDbDataAdapter(sql, conection);
                adapter.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    myRows.Add(dr["EnumeratorID"].ToString(), (dr["EnumeratorName"].ToString(),
                               (DateTime)dr["HireDate"], bool.Parse(dr["Active"].ToString()), global.fad3DataStatus.statusFromDB));
                }
            }

            return myRows;
        }

        public static
            Dictionary<string, (string RefNo, string LandingSite, string Gear, DateTime SamplingDate, double WtCatch, int Rows, string GUIDs)>
            GetEnumeratorSamplings()
        {
            var myRows = new Dictionary<string, (string RefNo, string LandingSite, string Gear, DateTime SamplingDate, double WtCatch, int Rows, string GUIDs)>();

            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                conection.Open();
                string query = $@"SELECT SamplingDate, tblSampling.SamplingGUID,  tblSampling.GearVarGUID, tblSampling.LSGUID,
                               tblSampling.RefNo, tblLandingSites.LSName,
                               tblGearVariations.Variation, tblSampling.WtCatch, Count(tblCatchComp.RowGUID) AS n
                               FROM tblLandingSites INNER JOIN (tblGearVariations INNER JOIN(tblSampling LEFT
                               JOIN tblCatchComp ON tblSampling.SamplingGUID = tblCatchComp.SamplingGUID) ON
                               tblGearVariations.GearVarGUID = tblSampling.GearVarGUID) ON tblLandingSites.LSGUID = tblSampling.LSGUID
                               WHERE tblSampling.Enumerator = {{{_EnumeratorGuid}}}
                               GROUP BY tblSampling.SamplingDate, tblSampling.SamplingGUID, tblSampling.GearVarGUID, tblSampling.LSGUID,
                               tblSampling.RefNo, tblLandingSites.LSName,
                               tblGearVariations.Variation, tblSampling.WtCatch ORDER BY tblSampling.SamplingDate";

                DataTable dt = new DataTable();
                var adapter = new OleDbDataAdapter(query, conection);
                adapter.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    var rowRefNo = dr["RefNo"].ToString();
                    var rowLandingSite = dr["LSName"].ToString();
                    var rowGear = dr["Variation"].ToString();
                    var rowSamplingDate = (DateTime)dr["SamplingDate"];
                    var rowWtCatch = double.Parse(dr["WtCatch"].ToString());
                    var rowRows = int.Parse(dr["n"].ToString());
                    var rowGUIDs = $"{dr["LSGUID"].ToString()}|{dr["GearVarGUID"].ToString()}";

                    myRows.Add(dr["SamplingGUID"].ToString(), (rowRefNo, rowLandingSite, rowGear, rowSamplingDate, rowWtCatch, rowRows, rowGUIDs));
                }
            }
            return myRows;
        }
    }
}
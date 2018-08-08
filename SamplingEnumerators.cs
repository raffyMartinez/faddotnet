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

        public static
            Dictionary<string, (string RefNo, string LandingSite, string Gear, DateTime SamplingDate, double WtCatch, int Rows)>
            GetEnumeratorSamplings()
        {
            var myRows = new Dictionary<string, (string RefNo, string LandingSite, string Gear, DateTime SamplingDate, double WtCatch, int Rows)>();
            DataTable dt = new DataTable();
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

                    myRows.Add(dr["SamplingGUID"].ToString(), (rowRefNo, rowLandingSite, rowGear, rowSamplingDate, rowWtCatch, rowRows));
                }
            }
            return myRows;
        }
    }
}
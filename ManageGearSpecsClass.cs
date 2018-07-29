using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;

namespace FAD3
{
    public static class ManageGearSpecsClass
    {
        static string _GearVarGuid;
        static string _GearVarName;
        static string _SamplingGuid;
        static bool _HasSampledGearSpecs;

        //this field contains the template for gear specs of a given gear variation
        static List<GearSpecification> _GearSpecifications = new List<GearSpecification>();

        //this field contains the data of each spec of the sampled gear
        static Dictionary<string, SampledGearSpecData> _SampledGearSpecs = new Dictionary<string, SampledGearSpecData>();

        public static string SamplingGuid
        {
            get { return _SamplingGuid; }
            set
            {
                _SamplingGuid = value;

                //if a gear has a specs template then we get the sampled gear specs
                //GetSampledGearSpecs will fill the _SampledGearSpecs Dictionary
                if (_GearSpecifications.Count > 0) GetSampledGearSpecs();
            }
        }

        public static Dictionary<string, SampledGearSpecData> SampledGearSpecs
        {
            get { return _SampledGearSpecs; }
        }

        public struct GearSpecification
        {
            public string Property { get; set; }
            public string Type { get; set; }
            public string Notes { get; set; }
            public string RowGuid { get; set; }
            public global.fad3DataStatus DataStatus { get; set; }
            public int Sequence { get; set; }
        }

        public struct SampledGearSpecData
        {
            public string SpecGUID { get; set; }
            public string SpecValue { get; set; }
            public string SamplingGUID { get; set; }
            public string RowID { get; set; }
            public global.fad3DataStatus DataStatus { get; set; }
        }

        //if the dictionary has specs data for the sampled gear
        public static bool HasSampledGearSpecs
        {
            get { return _HasSampledGearSpecs; }
        }


        /// <summary>
        /// Set gear variation GUID and then read the specs template of the gear
        /// </summary>
        /// <param name="GearVarGuid"></param>
        public static void GearVarGuid(string GearVarGuid)
        {
            _GearVarGuid = GearVarGuid;
            GetGearSpecs();  //get spec template of the gear variation
        }

        /// <summary>
        /// Set gear variation GUID and variation name and then read the specs template of the gear
        /// </summary>
        /// <param name="GearVarGuid"></param>
        /// <param name="GearVarName"></param>
        public static void GearVariation(string GearVarGuid, string GearVarName)
        {
            _GearVarGuid = GearVarGuid;
            _GearVarName = GearVarName;
            GetGearSpecs();  //get spec template of the gear variation
        }

        public static List<GearSpecification> GearSpecifications
        {
            get { return _GearSpecifications; }
        }


        /// <summary>
        /// save the specs of the gear that was sampled
        /// </summary>
        /// <param name="SampledGearSpecs"></param>
        /// <returns></returns>
        public static bool SaveSampledGearSpecs(Dictionary<string, SampledGearSpecData> SampledGearSpecs)
        {
            using (var con = new OleDbConnection(global.ConnectionString))
            {
                con.Open();
                var sql = "";
                var Success = false;
                foreach (KeyValuePair<string, SampledGearSpecData> kv in SampledGearSpecs)
                {
                    if (kv.Value.DataStatus == global.fad3DataStatus.statusNew)
                    {
                        sql = "Insert into tblSampledGearSpec (RowID, SamplingGUID, SpecID, [Value]) values ('{" +
                                kv.Value.RowID + "}', '{" +
                                _SamplingGuid + "}', '{" +
                                kv.Value.SpecGUID + "}', '" +
                                kv.Value.SpecValue + "')";
                    }
                    else if (kv.Value.DataStatus == global.fad3DataStatus.statusEdited)
                    {
                        sql = "Update tblSampledGearSpec set [Value] = '" +
                            kv.Value.SpecValue + "' where SamplingGUID = '{" +
                            kv.Value.SamplingGUID + "}' and SpecID = '{" +
                            kv.Value.SpecGUID + "}'";
                    }
                    else if (kv.Value.DataStatus == global.fad3DataStatus.statusForDeletion)
                    {
                        sql = "Delete * from tblSampleGearSpec where RowID = '{" + kv.Value.RowID + "}'";
                    }

                    if (sql.Length > 0)
                        using (OleDbCommand update = new OleDbCommand(sql, con))
                        {
                            Success = (update.ExecuteNonQuery() > 0);
                            //TODO: what to do if Success=false?
                            sql = "";
                        }
                }
            }
            return true;
        }

        /// <summary>
        /// retrieve the specs of the gear that was sampled
        /// </summary>
        static void GetSampledGearSpecs()
        {
            using (var con = new OleDbConnection(global.ConnectionString))
            {
                _HasSampledGearSpecs = false;
                _SampledGearSpecs.Clear();
                var sql = "Select RowID, SpecID, Value from tblSampledGearSpec " +
                    "where SamplingGUID = '{" + _SamplingGuid + "}'";

                using (var dt = new DataTable())
                {
                    con.Open();
                    var adapter = new OleDbDataAdapter(sql, con);
                    adapter.Fill(dt);
                    foreach (DataRow dr in dt.Rows)
                    {
                        _HasSampledGearSpecs = true;
                        var s = new SampledGearSpecData();

                        s.RowID = dr["RowID"].ToString();
                        s.SpecValue = dr["Value"].ToString();
                        s.SamplingGUID = _SamplingGuid;
                        s.SpecGUID = dr["SpecID"].ToString();
                        s.DataStatus = global.fad3DataStatus.statusFromDB;

                        _SampledGearSpecs.Add(s.SpecGUID, s);
                    }
                    con.Close();
                }
            }
        }


        /// <summary>
        /// Fills the template List gear specifications of a given gear variation
        /// </summary>
        static void GetGearSpecs()
        {
            _GearSpecifications.Clear();
            using (var con = new OleDbConnection(global.ConnectionString))
            {
                con.Open();
                string query = "Select RowID, ElementName, Description, ElementType, Sequence " +
                               "from tblGearSpecs where Version = '2' and GearVarGuid ='{" +
                               _GearVarGuid + "}' order by sequence";
                using (var dt = new DataTable())
                {
                    var adapter = new OleDbDataAdapter(query, con);
                    adapter.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        var st = new GearSpecification();
                        st.Property = row["ElementName"].ToString();
                        st.Type = row["ElementType"].ToString();
                        st.Notes = row["Description"].ToString();
                        st.RowGuid = row["RowID"].ToString();
                        st.DataStatus = global.fad3DataStatus.statusFromDB;
                        var seq = 0;
                        if (int.TryParse(row["Sequence"].ToString(), out seq)) st.Sequence = seq;
                        _GearSpecifications.Add(st);
                    }
                }
                con.Close();
            }
        }

        /// <summary>
        /// Save a gear spec template of a gear variation
        /// </summary>
        /// <param name="specs"></param>
        /// <returns></returns>
        public static bool SaveGearSpecs(List<GearSpecification> specs)
        {
            var sql = "";
            int Version = 2;
            bool Success;
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                foreach (ManageGearSpecsClass.GearSpecification spec in specs)
                {
                    if (spec.DataStatus == global.fad3DataStatus.statusEdited)
                    {
                        sql = "Update tblGearSpecs set " +
                              "ElementName ='" + spec.Property + "', " +
                              "ElementType = '" + spec.Type + "', " +
                              "Description = '" + spec.Notes + "', " +
                              "Sequence = " + spec.Sequence + ", " +
                              "Version = '2' where RowID = '{" + spec.RowGuid + "}'";
                    }
                    else if (spec.DataStatus == global.fad3DataStatus.statusNew)
                    {
                        sql = "Insert into tblGearSpecs (ElementName, ElementType, Description, Sequence, Version, RowId, GearVarGuid) " +
                              "values ('" +
                              spec.Property + "', '" +
                              spec.Type + "', '" +
                              spec.Notes + "', " +
                              spec.Sequence + ", '" +
                              Version + "', '{" +
                              Guid.NewGuid().ToString() + "}', '{" +
                              _GearVarGuid + "}')";
                    }
                    else if (spec.DataStatus == global.fad3DataStatus.statusForDeletion)
                    {
                        sql = "Delete * from tblGearSpecs where RowID ='{" + spec.RowGuid + "}'";
                    }

                    if (sql.Length > 0)
                        using (OleDbCommand update = new OleDbCommand(sql, conn))
                        {
                            Success = (update.ExecuteNonQuery() > 0);
                            //TODO: what to do if Success=false?
                            sql = "";
                        }
                }
                conn.Close();
            }
            return true;
        }

        /// <summary>
        /// returns a sampled gear specs in string format
        /// </summary>
        /// <param name="SamplingGuid"></param>
        /// <param name="Truncated"></param>
        /// <param name="TruncateLength"></param>
        /// <returns></returns>
        public static string GetSampledSpecsEx(string SamplingGuid, bool Truncated = false, int TruncateLength = 0)
        {
            var s = "";
            var isDone = false;
            var FirstRow = "";
            using (var con = new OleDbConnection(global.ConnectionString))
            {
                con.Open();
                var sql = "SELECT ElementName, Value FROM tblGearSpecs INNER JOIN " +
                      "tblSampledGearSpec ON tblGearSpecs.RowID = tblSampledGearSpec.SpecID " +
                      "WHERE SamplingGUID = '{" + SamplingGuid + "}' AND Version = '2' " +
                      "ORDER BY sequence";
                using (var dt = new DataTable())
                {
                    var adapter = new OleDbDataAdapter(sql, con);
                    adapter.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        s += row["ElementName"] + ": " + row["Value"] + "\r\n";
                        if (!isDone)
                        {
                            FirstRow = row["ElementName"] + ": " + row["Value"];
                            isDone = true;
                        }
                    }
                }

            }

            if (Truncated && s.Length > 0)
            {
                if (TruncateLength == 0)
                    return FirstRow + " ...";
                else
                {
                    if (s.Length > 0)
                        return s.Substring(0, TruncateLength) + " ...";
                    else
                        return s;
                }
            }
            else
                return s;
        }


    }
}

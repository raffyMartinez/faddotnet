using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;

namespace FAD3
{
    public static class ManageGearSpecsClass
    {
        private static string _GearVarGuid;
        private static string _GearVarName;
        private static string _SamplingGuid;
        private static bool _HasSampledGearSpecs;
        private static bool _HasUnsavedSampledGearSpecEdits;

        //this field contains the template for gear specs of a given gear variation
        private static List<GearSpecification> _GearSpecifications = new List<GearSpecification>();

        //this field contains the spec data of the sampled gear
        private static Dictionary<string, SampledGearSpecData> _SampledGearSpecs = new Dictionary<string, SampledGearSpecData>();

        /// <summary>
        /// Assigns the SamplingGuid.
        /// A new GUID sets the _HasUnsavedSampledGearSpecEdits to false
        /// </summary>
        public static string SamplingGuid
        {
            get { return _SamplingGuid; }
            set
            {
                //set flag to false if sampling guid changes
                if (_SamplingGuid != value)
                    _HasUnsavedSampledGearSpecEdits = false;

                _SamplingGuid = value;

                //get the specs of the sampled gear from the database
                if (!_HasUnsavedSampledGearSpecEdits && _GearSpecifications.Count > 0)
                    //if a gear has a specs template then we get the sampled gear specs
                    //GetSampledGearSpecs will fill the _SampledGearSpecs Dictionary
                    GetSampledGearSpecs();
            }
        }

        /// <summary>
        /// Boolean. Returns if there are unsaved edits in the sampled gear's specifications
        /// </summary>
        public static bool HasUnsavedSampledGearSpecEdits
        {
            get { return _HasUnsavedSampledGearSpecEdits; }
        }

        /// <summary>
        /// Clears the sampled gear spec dictionary so that it will
        /// receive the edited or new data and then sets the
        /// _HasUnsavedSampledGearSpecEdits flag to true
        /// </summary>
        public static void SetSampledGearSpecsForPreSave()
        {
            _SampledGearSpecs.Clear();
            _HasUnsavedSampledGearSpecEdits = true;
        }

        /// <summary>
        /// Dictionary. Returns the specifications of the sampled gear
        /// </summary>
        public static Dictionary<string, SampledGearSpecData> SampledGearSpecs
        {
            get { return _SampledGearSpecs; }
        }

        /// <summary>
        /// struct that holds the template for the specs of a gear variation
        /// </summary>
        public struct GearSpecification
        {
            public string Property { get; set; }
            public string Type { get; set; }
            public string Notes { get; set; }
            public string RowGuid { get; set; }
            public global.fad3DataStatus DataStatus { get; set; }
            public int Sequence { get; set; }
        }

        /// <summary>
        /// structure that holds the data of sampled gear's specs
        /// </summary>
        public struct SampledGearSpecData
        {
            private static global.fad3DataStatus _DataStatus;
            public global.fad3DataStatus DataStatus { get; set; }
            public string SpecGUID { get; set; }
            public string SpecName { get; set; }
            public string SamplingGUID { get; set; }
            public string RowID { get; set; }
            public string SpecValue { get; set; }
        }

        /// <summary>
        /// Boolean. If there is specs data for the sampled gear in the database
        /// </summary>
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

        /// <summary>
        /// List. Returns the specifications template for the gear variation
        /// </summary>
        public static List<GearSpecification> GearSpecifications
        {
            get { return _GearSpecifications; }
        }

        /// <summary>
        /// save the specs of the gear that was sampled
        /// </summary>
        public static bool SaveSampledGearSpecs()
        {
            using (var con = new OleDbConnection(global.ConnectionString))
            {
                con.Open();
                var sql = "";
                var Success = false;
                bool[] SaveSuccess = new bool[_SampledGearSpecs.Count];
                var n = 0;
                foreach (KeyValuePair<string, SampledGearSpecData> kv in _SampledGearSpecs)
                {
                    sql = "";
                    if (kv.Value.DataStatus == global.fad3DataStatus.statusNew)
                    {
                        sql = $@"Insert into tblSampledGearSpec (RowID, SamplingGUID, SpecID, [Value]) values (
                                '{kv.Value.RowID}',
                                {{{ _SamplingGuid}}},
                                '{kv.Value.SpecGUID}',
                                '{kv.Value.SpecValue}')";
                    }
                    else if (kv.Value.DataStatus == global.fad3DataStatus.statusEdited)
                    {
                        sql = $@"Update tblSampledGearSpec set
                            [Value] = '{kv.Value.SpecValue}' where
                            SamplingGUID = {{{kv.Value.SamplingGUID}}} and
                            SpecID = {{{kv.Value.SpecGUID}}}";
                    }
                    else if (kv.Value.DataStatus == global.fad3DataStatus.statusForDeletion)
                    {
                        sql = $"Delete * from tblSampleGearSpec where RowID = {{{kv.Value.RowID}}}";
                    }

                    if (sql.Length > 0)
                        using (OleDbCommand update = new OleDbCommand(sql, con))
                        {
                            Success = (update.ExecuteNonQuery() > 0);
                            //TODO: what to do if Success=false?

                            SaveSuccess[n] = Success;
                            sql = "";
                        }
                    n++;
                }

                //If all specs were successfully saved then set flag to false
                _HasUnsavedSampledGearSpecEdits = false;
                for (int i = 0; i < SaveSuccess.Length - 1; i++)
                {
                    if (!SaveSuccess[i])
                    {
                        _HasUnsavedSampledGearSpecEdits = true;
                        break;
                    }
                }
            }

            //return true if there are no unsaved edits
            return !_HasUnsavedSampledGearSpecEdits;
        }

        /// <summary>
        /// Boolean. Determine if the sampled gear has a specifications template
        /// </summary>
        /// <param name="SamplingGuid"></param>
        /// <returns></returns>
        public static bool SampledGearHasSpecs(string SamplingGuid)
        {
            var HasSpecs = false;
            using (var con = new OleDbConnection(global.ConnectionString))
            {
                var sql = $@"SELECT TOP 1 SamplingGUID FROM tblGearSpecs INNER JOIN tblSampledGearSpec ON
                          tblGearSpecs.RowID = tblSampledGearSpec.SpecID WHERE tblGearSpecs.Version = '2'
                          AND tblSampledGearSpec.SamplingGUID ={{{SamplingGuid}}}";

                using (var dt = new DataTable())
                {
                    con.Open();
                    var adapter = new OleDbDataAdapter(sql, con);
                    adapter.Fill(dt);
                    HasSpecs = dt.Rows.Count > 0;
                }
            }
            return HasSpecs;
        }

        /// <summary>
        /// Retrieve the specs of the gear that was sampled.
        /// A Dictionary (_SampledGearSpecs) is filled.
        /// </summary>
        private static void GetSampledGearSpecs()
        {
            using (var con = new OleDbConnection(global.ConnectionString))
            {
                _HasSampledGearSpecs = false;
                _SampledGearSpecs.Clear();
                var sql = $@"SELECT tblGearSpecs.RowID, ElementName, SpecID, Value
                    FROM tblGearSpecs INNER JOIN tblSampledGearSpec ON tblGearSpecs.RowID = tblSampledGearSpec.SpecID
                    WHERE tblGearSpecs.Version = '2' AND tblSampledGearSpec.SamplingGUID = {{{_SamplingGuid}}}";

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
                        s.SpecName = dr["ElementName"].ToString();
                        s.DataStatus = global.fad3DataStatus.statusFromDB;

                        _SampledGearSpecs.Add(s.SpecGUID, s);
                    }
                    con.Close();
                }
            }
        }

        /// <summary>
        /// Gets the template for the gear specifications of a given gear variation
        /// A List (_GearSpecifications) is filled
        /// </summary>
        private static void GetGearSpecs()
        {
            _GearSpecifications.Clear();
            using (var con = new OleDbConnection(global.ConnectionString))
            {
                con.Open();
                string query = $@"Select RowID, ElementName, Description, ElementType, Sequence
                               from tblGearSpecs where Version = '2' and GearVarGuid ={{{_GearVarGuid}}}
                               order by sequence";
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
                        sql = $@"Update tblGearSpecs set
                              ElementName ='{spec.Property}',
                              ElementType = '{spec.Type}',
                              Description = '{spec.Notes}',
                              Sequence =  {spec.Sequence},
                              Version = '2' where RowID = {{{spec.RowGuid}}}";
                    }
                    else if (spec.DataStatus == global.fad3DataStatus.statusNew)
                    {
                        sql = $@"Insert into tblGearSpecs (ElementName, ElementType, Description, Sequence, Version, RowId, GearVarGuid)
                              values (
                              '{spec.Property}',
                              '{spec.Type}',
                              '{spec.Notes}',
                              {spec.Sequence},
                              '{Version}',
                              {{{Guid.NewGuid().ToString()}}},
                              {{{_GearVarGuid}}})";
                    }
                    else if (spec.DataStatus == global.fad3DataStatus.statusForDeletion)
                    {
                        sql = $"Delete * from tblGearSpecs where RowID = {{{spec.RowGuid}}}";
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
        /// Returns the Property/Specification name given a specification guid
        /// </summary>
        /// <param name="SpecGuid"></param>
        /// <returns></returns>
        public static string SpecNameFromSpecGUID(string SpecGuid)
        {
            var SpecName = "";
            foreach (GearSpecification item in _GearSpecifications)
            {
                if (item.RowGuid == SpecGuid)
                {
                    SpecName = item.Property;
                    break;
                }
            }
            return SpecName;
        }

        /// <summary>
        /// returns a string of the presaved specifications of the sampled gear
        /// </summary>
        /// <returns></returns>
        public static string PreSavedSampledGearSpec()
        {
            var s = "";
            foreach (KeyValuePair<string, SampledGearSpecData> kv in _SampledGearSpecs)
            {
                s += kv.Value.SpecName + ": " + kv.Value.SpecValue + "\r\n";
            }
            return s;
        }

        /// <summary>
        /// returns the saved sampled gear specs in string format
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
                var sql = $@"SELECT ElementName, Value FROM tblGearSpecs INNER JOIN
                      tblSampledGearSpec ON tblGearSpecs.RowID = tblSampledGearSpec.SpecID
                      WHERE SamplingGUID = {{{SamplingGuid}}} AND Version = '2'
                      ORDER BY sequence";
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
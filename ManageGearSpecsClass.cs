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
        static List<GearSpecification> _GearSpecifications = new List<GearSpecification>();


        public struct GearSpecification
        {
            public string Property { get; set; }
            public string Type { get; set; }
            public string Notes { get; set; }
            public string RowGuid { get; set; }
            public global.fad3DataStatus DataStatus { get; set; }
        }

        static ManageGearSpecsClass()
        {

        }

        public static void GearVariation(string GearVarGuid, string GearVarName)
        {
            _GearVarGuid = GearVarGuid;
            _GearVarName = GearVarName;
            GetGearSpecs();
        }

        public static List<GearSpecification> GearSpecifications
        {
            get { return _GearSpecifications; }
        }

        static void GetGearSpecs()
        {
            _GearSpecifications.Clear();
            using (var con = new OleDbConnection(global.ConnectionString))
            {
                con.Open();
                string query = "Select RowID, ElementName, Description, ElementType " +
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
                        _GearSpecifications.Add(st);
                    }
                }
                con.Close();
            }
        }

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
                              "Version = '2' where RowID = '{" + spec.RowGuid + "}'";
                    }
                    else if (spec.DataStatus == global.fad3DataStatus.statusNew)
                    {
                        sql = "Insert into tblGearSpecs (ElementName, ElementType, Description, Version, RowId, GearVarGuid) " +
                              "values ('" +
                              spec.Property + "', '" +
                              spec.Type + "', '" +
                              spec.Notes + "', '" +
                              Version + "', '{" +
                              Guid.NewGuid().ToString() + "}', '{" +
                              _GearVarGuid + "}')";
                    }
                    else if(spec.DataStatus == global.fad3DataStatus.statusForDeletion)
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

    }
}

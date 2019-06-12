using FAD3.Database.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace FAD3
{
    public static class Enumerators
    {
        private static string _targetAreaGuid;
        private static string _EnumeratorGuid;

        static Enumerators()
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

        public static string TargetAreaGUID
        {
            get { return _targetAreaGuid; }
            set
            {
                _targetAreaGuid = value;
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

        public static bool ImportEnumerators(string fileName, string targetAreaGuid)
        {
            var success = false;
            var elementCounter = 0;
            var proceed = false;
            switch (Path.GetExtension(fileName))
            {
                case ".txt":
                    break;

                case ".xml":
                case ".XML":
                    Dictionary<string, (string enumeratorName, DateTime dateHired, bool isActive, fad3DataStatus DataStatus)> enumDict = new Dictionary<string, (string enumeratorName, DateTime dateHired, bool isActive, fad3DataStatus DataStatus)>();
                    XmlTextReader xmlReader = new XmlTextReader(fileName);
                    var enumeratorName = "";
                    var enumeratorGuid = "";
                    DateTime dateHired = DateTime.Now;
                    while ((elementCounter == 0 || (elementCounter > 0 && proceed)) && xmlReader.Read())
                    {
                        switch (xmlReader.NodeType)
                        {
                            case XmlNodeType.Element:
                                if (elementCounter == 0 && xmlReader.Name == "Enumerators")
                                {
                                    proceed = true;
                                }
                                if (xmlReader.Name == "Enumerator")
                                {
                                    enumeratorGuid = xmlReader.GetAttribute("guid");
                                    dateHired = DateTime.Parse(xmlReader.GetAttribute("DateHired"));
                                    elementCounter++;
                                }

                                break;

                            case XmlNodeType.Text:
                                enumeratorName = xmlReader.Value;
                                break;
                        }

                        if (enumeratorName?.Length > 0 && enumeratorGuid?.Length > 0)
                        {
                            enumDict.Add(enumeratorGuid, (enumeratorName, dateHired, true, fad3DataStatus.statusNew));
                            enumeratorName = "";
                            enumeratorGuid = "";
                        }
                    }
                    success = SaveTargetAreaEnumerators(enumDict, targetAreaGuid);
                    break;
            }
            return success;
        }

        public static int ExportEnumerators(string targetAreaGuid)
        {
            FileDialogHelper.Title = "Provide filename for exporting enumerators";
            FileDialogHelper.DialogType = FileDialogType.FileSave;
            FileDialogHelper.DataFileType = DataFileType.Text | DataFileType.XML | DataFileType.CSV;
            FileDialogHelper.ShowDialog();
            var fileName = FileDialogHelper.FileName;
            var exportCount = 0;
            if (fileName.Length > 0)
            {
                switch (Path.GetExtension(fileName))
                {
                    case ".txt":

                        break;

                    case ".XML":
                    case ".xml":
                        var enumerators = Enumerators.GetTargetAreaEnumerators(targetAreaGuid);
                        var count = enumerators.Count;
                        if (count > 0)
                        {
                            var n = 0;
                            XmlWriter writer = XmlWriter.Create(fileName);
                            writer.WriteStartDocument();
                            writer.WriteStartElement("Enumerators");
                            foreach (var item in enumerators)
                            {
                                writer.WriteStartElement("Enumerator");
                                writer.WriteAttributeString("guid", item.Key);
                                writer.WriteAttributeString("DateHired", string.Format("{0:MMM-dd-yyyy}", item.Value.DateHired));
                                writer.WriteString(item.Value.EnumeratorName);
                                if (count == 1)
                                {
                                    writer.WriteEndDocument();
                                }
                                else
                                {
                                    if (n < (count - 1))
                                    {
                                        writer.WriteEndElement();
                                    }
                                    else
                                    {
                                        writer.WriteEndDocument();
                                    }
                                }
                                n++;
                            }
                            writer.Close();
                            exportCount = n;
                        }
                        break;

                    case ".xlsx":
                        break;
                }
            }
            return exportCount;
        }

        public static (bool success, string newGuid) SaveNewTargetAreaEnumerator(string targetAreaGuid, string name, DateTime hireDate, bool isActive)
        {
            var success = false;
            string sql = "";
            string newEnumeratorGuid = Guid.NewGuid().ToString();

            sql = $@"Insert into tblEnumerators (EnumeratorID, EnumeratorName, HireDate, Active, TargetArea) values (
                       {{{newEnumeratorGuid}}}, '{name}', '{hireDate}', {isActive}, {{{targetAreaGuid}}})";

            using (var conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                using (OleDbCommand update = new OleDbCommand(sql, conn))
                {
                    success = update.ExecuteNonQuery() > 0;
                }
            }
            return (success, newEnumeratorGuid);
        }

        /// <summary>
        /// deletes enumerators belonging to a target area
        /// </summary>
        /// <param name="targetAreaGuid"></param>
        public static void DeleteEnumerators(string targetAreaGuid)
        {
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                var sql = $"Delete * from tblEnumerators where TargetArea = {{{targetAreaGuid}}}";
                OleDbCommand update = new OleDbCommand(sql, conn);
                conn.Open();
                try
                {
                    update.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, "Enumerators", "DeleteEnumerators");
                }
            }
        }

        public static bool SaveNewTargetAreaEnumerator(string targetAreaGuid, string name, DateTime hireDate, bool isActive, string enumeratorGuid)
        {
            var success = false;
            string sql = "";

            sql = $@"Insert into tblEnumerators (EnumeratorID, EnumeratorName, HireDate, Active, TargetArea) values (
                       {{{enumeratorGuid}}}, '{name}', '{hireDate}', {isActive}, {{{targetAreaGuid}}})";

            using (var conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                using (OleDbCommand update = new OleDbCommand(sql, conn))
                {
                    try
                    {
                        success = update.ExecuteNonQuery() > 0;
                    }
                    catch (OleDbException oledbEx)
                    {
                        //ignore: dulplicate primary key error
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex.Message, "Enumerators", "SaveNewTargetAreaEnumerator");
                    }
                }
            }
            return success;
        }

        public static bool SaveTargetAreaEnumerators(Dictionary<string, (string enumeratorName, DateTime dateHired, bool isActive, fad3DataStatus dataStatus)> enumeratorsData, string targetAreaGuid)
        {
            _targetAreaGuid = targetAreaGuid;
            return SaveTargetAreaEnumerators(enumeratorsData);
        }

        public static bool SaveTargetAreaEnumerators(Dictionary<string, (string enumeratorName, DateTime dateHired, bool isActive, fad3DataStatus DataStatus)> enumeratorsData)
        {
            var n = 0;
            if (enumeratorsData.Count > 0)
            {
                using (var conn = new OleDbConnection(global.ConnectionString))
                {
                    conn.Open();
                    foreach (KeyValuePair<string, (string enumeratorName, DateTime dateHired, bool isActive, fad3DataStatus DataStatus)> kv in enumeratorsData)
                    {
                        var sql = "";
                        if (kv.Value.DataStatus == fad3DataStatus.statusEdited)
                        {
                            sql = $@"Update tblEnumerators set
                                EnumeratorName ='{kv.Value.enumeratorName}',
                                HireDate = '{kv.Value.dateHired}',
                                Active={kv.Value.isActive}
                                where EnumeratorID = {{{kv.Key}}}";
                        }
                        else if (kv.Value.DataStatus == fad3DataStatus.statusNew)
                        {
                            sql = $@"Insert into tblEnumerators (EnumeratorID, EnumeratorName, HireDate, Active, TargetArea) values (
                             {{{kv.Key}}}, '{kv.Value.enumeratorName}', '{kv.Value.dateHired}', {kv.Value.isActive}, {{{_targetAreaGuid}}})";
                        }
                        else if (kv.Value.DataStatus == fad3DataStatus.statusForDeletion)
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

        public static Dictionary<string, (string EnumeratorName, DateTime DateHired, bool IsActive, fad3DataStatus DataStatus)> GetTargetAreaEnumerators(string targetAreaGuid)
        {
            _targetAreaGuid = targetAreaGuid;
            return GetTargetAreaEnumerators();
        }

        public static Dictionary<string, (string EnumeratorName, DateTime DateHired, bool IsActive, fad3DataStatus DataStatus)> GetTargetAreaEnumerators()
        {
            var myRows = new Dictionary<string, (string EnumeratorName, DateTime DateHired, bool IsActive, fad3DataStatus DataStatus)>();

            var sql = $@"SELECT EnumeratorID, EnumeratorName, HireDate, Active FROM tblEnumerators WHERE
                         tblEnumerators.TargetArea={{{_targetAreaGuid}}}";

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
                               (DateTime)dr["HireDate"], bool.Parse(dr["Active"].ToString()), fad3DataStatus.statusFromDB));
                }
            }

            return myRows;
        }

        public static
            Dictionary<string, (string targetAreaName, string refNo, string landingSite, string gearClassName, string gear,
                                DateTime samplingDate, string fishingGround, string vesselType, double wtCatch, int rows, string GUIDs)>
            GetEnumeratorSamplings()
        {
            var myRows = new Dictionary<string, (string targetAreaName, string refNo, string landingSite, string gearClassName, string gear,
                                                 DateTime samplingDate, string fishingGround, string vesselType, double wtCatch, int rows, string GUIDs)>();

            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                conection.Open();
                string query = $@"SELECT tblSampling.SamplingGUID, tblAOI.AOIName, tblLandingSites.LSName, tblSampling.LSGUID,
                                tblGearClass.GearClassName, tblGearVariations.Variation, tblSampling.GearVarGUID, tblSampling.SamplingDate,
                                tblSampling.RefNo, tblSampling.FishingGround, tblSampling.WtCatch, tblSampling.VesType,
                                Count(tblCatchComp.RowGUID) AS n FROM tblGearClass INNER JOIN (tblAOI INNER JOIN (tblLandingSites INNER JOIN
                                (tblGearVariations INNER JOIN (tblSampling LEFT JOIN tblCatchComp ON tblSampling.SamplingGUID = tblCatchComp.SamplingGUID)
                                ON tblGearVariations.GearVarGUID = tblSampling.GearVarGUID) ON tblLandingSites.LSGUID = tblSampling.LSGUID)
                                ON tblAOI.AOIGuid = tblLandingSites.AOIGuid) ON tblGearClass.GearClass = tblGearVariations.GearClass
                                WHERE tblSampling.Enumerator={{{_EnumeratorGuid}}} GROUP BY tblSampling.SamplingGUID, tblAOI.AOIName,
                                tblLandingSites.LSName, tblSampling.LSGUID, tblGearClass.GearClassName, tblGearVariations.Variation,
                                tblSampling.GearVarGUID, tblSampling.SamplingDate, tblSampling.RefNo, tblSampling.FishingGround, tblSampling.WtCatch,
                                tblSampling.VesType ORDER BY tblSampling.SamplingDate";

                DataTable dt = new DataTable();
                var adapter = new OleDbDataAdapter(query, conection);
                adapter.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    var rowSamplingGuid = dr["SamplingGUID"].ToString();
                    var rowtargetAreaName = dr["AOIName"].ToString();
                    var rowRefNo = dr["RefNo"].ToString();
                    var rowLandingSite = dr["LSName"].ToString();
                    var rowGearClass = dr["GearClassName"].ToString();
                    var rowGear = dr["Variation"].ToString();
                    var rowSamplingDate = (DateTime)dr["SamplingDate"];
                    var rowFishingGround = dr["FishingGround"].ToString();
                    var rowVesselType = FishingVessel.VesselTypeFromVesselTypeNumber((int)dr["VesType"]);
                    var rowWtCatch = double.Parse(dr["WtCatch"].ToString());
                    var rowRows = int.Parse(dr["n"].ToString());
                    var rowGUIDs = $"{dr["LSGUID"].ToString()}|{dr["GearVarGUID"].ToString()}";

                    myRows.Add(rowSamplingGuid, (rowtargetAreaName, rowRefNo, rowLandingSite, rowGearClass, rowGear, rowSamplingDate, rowFishingGround, rowVesselType, rowWtCatch, rowRows, rowGUIDs));
                }
            }
            return myRows;
        }

        static public bool TargetAreaHasEnumerators(string AOIGuid)
        {
            var HasEnumerator = false;
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $"SELECT TOP 1 EnumeratorID FROM tblEnumerators WHERE TargetArea ={{{AOIGuid}}}";
                    var command = new OleDbCommand(query, conection);
                    var reader = command.ExecuteReader();
                    HasEnumerator = reader.HasRows;
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }

            return HasEnumerator;
        }

        static public Dictionary<string, string> AOIEnumeratorsList(string AOIGuid, ComboBox c = null)
        {
            Dictionary<string, string> myAOIEnumerators = new Dictionary<string, string>();
            var myDT = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();

                    string query = $@"Select EnumeratorID, EnumeratorName from tblEnumerators where TargetArea = {{{AOIGuid}}}
                                      Order by EnumeratorName";
                    using (var adapter = new OleDbDataAdapter(query, conection))
                    {
                        adapter.Fill(myDT);
                        for (int i = 0; i < myDT.Rows.Count; i++)
                        {
                            DataRow dr = myDT.Rows[i];
                            myAOIEnumerators.Add(dr["EnumeratorID"].ToString(), dr["EnumeratorName"].ToString());
                            if (c != null)
                            {
                                c.Items.Add(new KeyValuePair<string, string>(dr["EnumeratorID"].ToString(), dr["EnumeratorName"].ToString()));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }

            return myAOIEnumerators;
        }

        public static Dictionary<string, string> EnumeratorsWithCount(string AOIGuid)
        {
            Dictionary<string, string> myList = new Dictionary<string, string>();
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT EnumeratorID, EnumeratorName, Count(Enumerator) AS n
                                      FROM tblEnumerators LEFT JOIN tblSampling ON tblEnumerators.EnumeratorID = tblSampling.Enumerator
                                      GROUP BY tblEnumerators.EnumeratorID, tblEnumerators.EnumeratorName, tblEnumerators.TargetArea
                                      HAVING tblEnumerators.TargetArea ={{{AOIGuid}}}";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        myList.Add(dr["EnumeratorName"].ToString(), dr["EnumeratorID"].ToString() + "," + dr["n"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }
            return myList;
        }
    }
}
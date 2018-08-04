/*
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/12/2016
 * Time: 9:20 AM
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Xml;

namespace FAD3
{
    /// <summary>
    /// Description of Sampling.
    /// </summary>
    public class sampling
    {
        private static Dictionary<string, UserInterfaceStructure> _uis = new Dictionary<string, UserInterfaceStructure>();

        private long _CatchAndEffortPropertyCount = 0;

        private int _LFRowsCount;

        private string _RererenceNo = "";

        private double _SampleWtFromTotalCatch = 0;

        private string _SamplingGUID = "";

        private double _TotalCatchWt = 0;

        private double _TotalWtOfFromTotal = 0;

        static sampling()
        {
            //SetUpUIElement();
        }

        public sampling(string SamplingGUID)
        {
            _SamplingGUID = SamplingGUID;
        }

        public sampling()
        {
        }

        public delegate void ReadUIElement(sampling s, UIRowFromXML e);

        public event ReadUIElement OnUIRowRead;

        static public Dictionary<string, UserInterfaceStructure> uis
        {
            get { return _uis; }
        }

        public long CatchAndEffortPropertyCount
        {
            get { return _CatchAndEffortPropertyCount; }
        }

        public long LFRowsCount
        {
            get { return _LFRowsCount; }
        }

        public string ReferenceNo
        {
            get { return _RererenceNo; }
        }

        public double SampleWtFromTotalCatch
        {
            get { return _SampleWtFromTotalCatch; }
        }

        public string SamplingGUID
        {
            get { return _SamplingGUID; }
            set { _SamplingGUID = value; }
        }

        public double TotalCatchWt
        {
            get { return _TotalCatchWt; }
        }

        public double TotalWtOfFromTotal
        {
            get { return _TotalWtOfFromTotal; }
        }

        public static bool DeleteCatchLine(string CatchLineGUID)
        {
            bool Success = false;
            string query = "";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    query = $"Delete * from tblCatchDetail where CatchCompRow ={{{CatchLineGUID}}}";
                    OleDbCommand update = new OleDbCommand(query, conn);
                    conn.Open();
                    Success = (update.ExecuteNonQuery() > 0);
                    conn.Close();

                    query = $"Delete * from tblGMS where CatchCompRow ={{{CatchLineGUID}}}";
                    update = new OleDbCommand(query, conn);
                    conn.Open();
                    update.ExecuteNonQuery();
                    conn.Close();

                    query = $"Delete * from tblLF where CatchCompRow ={{{CatchLineGUID}}}";
                    update = new OleDbCommand(query, conn);
                    conn.Open();
                    update.ExecuteNonQuery();
                    conn.Close();

                    if (Success)
                    {
                        query = $"Delete * from tblCatchComp where RowGUID ={{{CatchLineGUID}}}";
                        update = new OleDbCommand(query, conn);
                        conn.Open();
                        Success = (update.ExecuteNonQuery() > 0);
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }
            return Success;
        }

        public static bool DeleteGMSLine(string RowGUID)
        {
            bool Success = false;
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    string query = $"Delete * from tblGMS where RowGUID = {{{RowGUID}}}";
                    OleDbCommand update = new OleDbCommand(query, conn);
                    conn.Open();
                    Success = (update.ExecuteNonQuery() > 0);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }
            return Success;
        }

        public static bool DeleteLFLine(string RowGUID)
        {
            bool Success = false;
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    string query = $"Delete * from tblLF where RowGUID = {{{RowGUID}}}";
                    OleDbCommand update = new OleDbCommand(query, conn);
                    conn.Open();
                    Success = (update.ExecuteNonQuery() > 0);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }
            return Success;
        }

        public static bool DeleteSampling(string samplingGUID)
        {
            bool Success = false;
            List<string> myList = new List<string>();
            string query = "";
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                conection.Open();
                //Delete GMS data
                query = $@"DELETE tblGMS.* FROM tblCatchComp INNER JOIN tblGMS ON
                        tblCatchComp.RowGUID = tblGMS.CatchCompRow WHERE SamplingGUID= {{{samplingGUID}}}";
                using (OleDbCommand update = new OleDbCommand(query, conection))
                {
                    update.ExecuteNonQuery();
                }

                //Delete LF data
                query = $@"DELETE tblLF.* FROM tblCatchComp INNER JOIN tblLF ON
                        tblCatchComp.RowGUID = tblLF.CatchCompRow
                        WHERE tblCatchComp.SamplingGUID = {{{samplingGUID}}}";
                using (OleDbCommand update = new OleDbCommand(query, conection))
                {
                    update.ExecuteNonQuery();
                }

                //Delete CatchDetail data
                query = $@"DELETE tblCatchDetail.* FROM tblCatchComp INNER JOIN tblCatchDetail
                        ON tblCatchComp.RowGUID = tblCatchDetail.CatchCompRow
                        WHERE tblCatchComp.SamplingGUID = {{{samplingGUID}}}";
                using (OleDbCommand update = new OleDbCommand(query, conection))
                {
                    update.ExecuteNonQuery();
                }

                //we delete the children catch composition data

                query = $@"Delete * from tblCatchComp WHERE tblCatchComp.SamplingGUID = {{{samplingGUID}}}";
                using (OleDbCommand update = new OleDbCommand(query, conection))
                {
                    update.ExecuteNonQuery();
                }

                //now we delete the parent catch and effort data

                query = $"Delete * from tblSampling where SamplingGUID = {{{samplingGUID}}}";
                using (OleDbCommand update = new OleDbCommand(query, conection))
                {
                    Success = (update.ExecuteNonQuery() > 0);
                }
            }

            return Success;
        }

        public Dictionary<string, GMSLine> GMSData(string CatchCompRowNo)
        {
            Dictionary<string, GMSLine> mydata = new Dictionary<string, GMSLine>();
            var dt = new DataTable();
            using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;data source=" + global.mdbPath))
            {
                try
                {
                    conection.Open();

                    string query = $@"SELECT tblGMS.RowGUID, tblGMS.Gonadwt, tblGMS.GMS, tblGMS.Sex, tblGMS.Wt, tblGMS.Len,
                                   tblGMS.CatchCompRow, tblTaxa.TaxaNo, tblTaxa.Taxa FROM tblTaxa INNER JOIN
                                   (tblAllSpecies INNER JOIN(tblCatchComp INNER JOIN tblGMS ON
                                   tblCatchComp.RowGUID = tblGMS.CatchCompRow) ON
                                   tblAllSpecies.SpeciesGUID = tblCatchComp.NameGUID)
                                   ON tblTaxa.TaxaNo = tblAllSpecies.TaxaNo
                                   WHERE tblGMS.CatchCompRow ={{{CatchCompRowNo}}}";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];

                        global.FishCrabGMS gms;
                        Enum.TryParse(dr["GMS"].ToString(), out gms);
                        global.sex sex;
                        Enum.TryParse(dr["Sex"].ToString(), out sex);
                        global.Taxa taxa;
                        Enum.TryParse(dr["TaxaNo"].ToString(), out taxa);
                        var myGMS = new GMSLine
                        {
                            TaxaName = dr["Taxa"].ToString(),
                            Taxa = taxa,
                            Sex = sex,
                            GMS = gms,
                            DataStatus = global.fad3DataStatus.statusFromDB
                        };
                        if (dr["Len"].ToString().Length > 0)
                            myGMS.Length = double.Parse(dr["Len"].ToString());
                        if (dr["Wt"].ToString().Length > 0)
                            myGMS.Weight = double.Parse(dr["GonadWt"].ToString());
                        if (dr["GonadWt"].ToString().Length > 0)
                            myGMS.GonadWeight = double.Parse(dr["GonadWt"].ToString());

                        mydata.Add(dr["RowGUID"].ToString(), myGMS);
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }
            return mydata;
        }

        /// <summary>
        /// setup a struct that contain elements of fish landing sampling
        /// to be used later for validation and error checking the sampling form
        /// </summary>
        static public void SetUpUIElement()
        {
            string xmlFile = global.AppPath + "\\UITable.xml";
            var doc = new XmlDocument();
            doc.Load(xmlFile);
            XmlElement root = doc.DocumentElement;
            XmlNodeList nodeList = root.SelectNodes("//UIRow");
            foreach (XmlNode n in nodeList)
            {
                //UIRowFromXML row = new UIRowFromXML();
                UserInterfaceStructure uistruct = new UserInterfaceStructure();
                foreach (XmlNode c in n)
                {
                    string val = c.Name;
                    switch (val)
                    {
                        case "Required":
                            uistruct.Required = bool.Parse(c.InnerText);
                            break;

                        case "ToolTip":
                            uistruct.ToolTip = c.InnerText;
                            break;

                        case "ReadOnly":
                            uistruct.ReadOnly = bool.Parse(c.InnerText);
                            break;

                        case "DataType":
                            uistruct.DataType = c.InnerText;
                            break;

                        case "Key":
                            uistruct.Key = c.InnerText;
                            break;

                        case "Label":
                            uistruct.Label = c.InnerText;
                            break;

                        case "Height":
                            uistruct.Height = int.Parse(c.InnerText);
                            break;

                        case "ButtonText":
                            uistruct.ButtonText = c.InnerText;
                            break;

                        case "control":
                            switch (c.InnerText)
                            {
                                case "TextBox":
                                    uistruct.Control = UserInterfaceStructure.UIControlType.TextBox;
                                    break;

                                case "ComboBox":
                                    uistruct.Control = UserInterfaceStructure.UIControlType.ComboBox;
                                    break;

                                case "CheckBox":
                                    uistruct.Control = UserInterfaceStructure.UIControlType.Check;
                                    break;

                                case "MaskDate":
                                    uistruct.Control = UserInterfaceStructure.UIControlType.DateMask;
                                    break;

                                case "MaskTime":
                                    uistruct.Control = UserInterfaceStructure.UIControlType.TimeMask;
                                    break;

                                default:
                                    uistruct.Control = UserInterfaceStructure.UIControlType.Spacer;
                                    break;
                            }
                            break;
                    }
                }

                if (uistruct.Key != "spacer")
                {
                    _uis.Add(uistruct.Key, uistruct);
                }
            }
        }

        public static bool UpdateGMS(bool isNew, Double? wt, double? len, global.sex sex,
                                             global.FishCrabGMS stage, double? gonadwt,
                                             string CatchCompRow, string RowGUID)
        {
            bool Success = false;
            string query = "";

            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    string slen = len == null ? "null" : len.ToString();
                    string swt = wt == null ? "null" : wt.ToString();
                    string sgonadwt = gonadwt == null ? "null" : gonadwt.ToString();

                    if (isNew)
                    {
                        query = $@"Insert into tblGMS (Len, Wt, Sex, GMS, CatchCompRow, RowGUID, Gonadwt) values
                                 ({slen} , {swt} , {(int)sex} , {(int)stage} , {{{CatchCompRow}}}, {{{RowGUID}}}, {sgonadwt})";
                    }
                    else
                    {
                        query = $@"Update tblGMS set Len= {slen},
                                                  Wt= {swt},
                                                  Sex= {(int)sex},
                                                  GMS= {(int)stage},
                                                  GonadWt= {sgonadwt}
                                                  Where RowGUID = {{{RowGUID}}}";
                    }
                    OleDbCommand update = new OleDbCommand(query, conn);
                    conn.Open();
                    Success = (update.ExecuteNonQuery() > 0);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }
            return Success;
        }

        /// <summary>
        /// reads the catch and effort data of a sampled fish landing and returs a
        /// Dictionary object of keys and values
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> CatchAndEffort()
        {
            string myVal = "";
            var myDT = new DataTable();
            Dictionary<string, string> PropertyValue = new Dictionary<string, string>();
            string VesType = "";
            try
            {
                using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;data source=" + global.mdbPath))
                {
                    conection.Open();
                    string query =
                        $@"SELECT tblSampling.*, tblAOI.AOIName, tblLandingSites.LSName, tblGearVariations.Variation, tblGearClass.GearClassName, tblGearClass.GearClass,
                        tblEnumerators.EnumeratorName FROM (tblAOI RIGHT JOIN tblLandingSites ON tblAOI.AOIGuid = tblLandingSites.AOIGuid) RIGHT JOIN
                        ((tblGearClass RIGHT JOIN tblGearVariations ON tblGearClass.GearClass = tblGearVariations.GearClass) RIGHT JOIN (tblEnumerators
                        RIGHT JOIN tblSampling ON tblEnumerators.EnumeratorID = tblSampling.Enumerator) ON tblGearVariations.GearVarGUID =
                        tblSampling.GearVarGUID) ON tblLandingSites.LSGUID = tblSampling.LSGUID
                        WHERE tblSampling.SamplingGUID= {{{_SamplingGUID}}}";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(myDT);
                    DataRow dr = myDT.Rows[0];

                    PropertyValue.Add("ReferenceNumber", dr["RefNo"].ToString());
                    try { PropertyValue.Add("Enumerator", dr["EnumeratorName"].ToString() + "|" + dr["Enumerator"].ToString()); }
                    catch { PropertyValue.Add("Enumerator", ""); }
                    PropertyValue.Add("TargetArea", dr["AOIName"].ToString());
                    PropertyValue.Add("LandingSite", dr["LSName"].ToString() + "|" + dr["LSGuid"].ToString());
                    PropertyValue.Add("GearClass", dr["GearClassName"].ToString() + "|" + dr["GearClass"].ToString());
                    PropertyValue.Add("FishingGear", dr["Variation"].ToString() + "|" + dr["GearVarGUID"].ToString());
                    try { PropertyValue.Add("FishingGround", dr["FishingGround"].ToString()); }
                    catch { PropertyValue.Add("FishingGround", ""); }

                    DateTime dt = new DateTime();

                    myVal = dr["SamplingDate"].ToString();
                    if (myVal != "")
                    {
                        dt = Convert.ToDateTime(myVal);
                        PropertyValue.Add("SamplingDate", string.Format("{0:MMM-dd-yyyy}", dt));
                    }
                    else
                    {
                        PropertyValue.Add("SamplingDate", "");
                    }

                    myVal = dr["SamplingTime"].ToString();
                    if (myVal != "")
                    {
                        dt = Convert.ToDateTime(myVal);
                        PropertyValue.Add("SamplingTime", string.Format("{0:HH:mm}", dt));
                    }
                    else
                    {
                        PropertyValue.Add("SamplingTime", "");
                    }

                    myVal = dr["DateSet"].ToString();
                    if (myVal != "")
                    {
                        dt = Convert.ToDateTime(myVal);
                        PropertyValue.Add("DateSet", string.Format("{0:MMM-dd-yyyy}", dt));
                    }
                    else
                    {
                        PropertyValue.Add("DateSet", "");
                    }

                    myVal = dr["TimeSet"].ToString();
                    if (myVal != "")
                    {
                        dt = Convert.ToDateTime(myVal);
                        PropertyValue.Add("TimeSet", string.Format("{0:HH:mm}", dt));
                    }
                    else
                    {
                        PropertyValue.Add("TimeSet", "");
                    }

                    myVal = dr["DateHauled"].ToString();
                    if (myVal != "")
                    {
                        dt = Convert.ToDateTime(myVal);
                        PropertyValue.Add("DateHauled", string.Format("{0:MMM-dd-yyyy}", dt));
                    }
                    else
                    {
                        PropertyValue.Add("DateHauled", "");
                    }

                    myVal = dr["TimeHauled"].ToString();
                    if (myVal != "")
                    {
                        dt = Convert.ToDateTime(myVal);
                        PropertyValue.Add("TimeHauled", string.Format("{0:HH:mm}", dt));
                    }
                    else
                    {
                        PropertyValue.Add("TimeHauled", "");
                    }

                    PropertyValue.Add("NumberOfHauls", dr["NoHauls"].ToString());
                    PropertyValue.Add("NumberOfFishers", dr["NoFishers"].ToString());
                    PropertyValue.Add("WeightOfCatch", dr["WtCatch"].ToString());
                    PropertyValue.Add("WeightOfSample", dr["WtSample"].ToString());

                    string HasLiveFish = "False";
                    if (Convert.ToBoolean(dr["HasLiveFish"]))
                    {
                        HasLiveFish = "True";
                    }
                    PropertyValue.Add("HasLiveFish", HasLiveFish);

                    switch (dr["VesType"].ToString())
                    {
                        case "1":
                            VesType = "Motorized";
                            break;

                        case "2":
                            VesType = "Non-Motorized";
                            break;

                        case "3":
                            VesType = "No vessel used";
                            break;

                        case "4":
                            VesType = "Not provided";
                            break;
                    }
                    PropertyValue.Add("TypeOfVesselUsed", VesType);

                    PropertyValue.Add("Engine", dr["Engine"].ToString());
                    PropertyValue.Add("EngineHorsepower", dr["hp"].ToString());

                    PropertyValue.Add("VesLength", dr["len"].ToString());
                    PropertyValue.Add("VesWidth", dr["wdt"].ToString());
                    PropertyValue.Add("VesHeight", dr["hgt"].ToString());

                    string VesDimension = "(LxWxH): " + dr["len"].ToString() + " x " + dr["wdt"].ToString() + " x " + dr["hgt"].ToString();
                    PropertyValue.Add("VesselDimension", VesDimension);
                    PropertyValue.Add("Notes", dr["Notes"].ToString());

                    //the following are class properties that can be accessed by property get
                    _RererenceNo = dr["RefNo"].ToString();
                    _TotalCatchWt = double.Parse(dr["WtCatch"].ToString());
                    _SampleWtFromTotalCatch = 0;
                    if (dr["WtSample"].ToString().Length > 0)
                    {
                        _SampleWtFromTotalCatch = double.Parse(dr["WtSample"].ToString());
                    }

                    myVal = dr["DateEncoded"].ToString();
                    if (myVal != "")
                    {
                        dt = Convert.ToDateTime(myVal);
                        PropertyValue.Add("DateEncoded", string.Format("{0:MMM-dd-yyyy HH:mm}", dt));
                    }
                    else
                    {
                        PropertyValue.Add("DateEncoded", "");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex);
            }

            _CatchAndEffortPropertyCount = PropertyValue.Count;
            return PropertyValue;
        }

        public Dictionary<string, CatchLine> CatchComp()
        {
            Dictionary<string, CatchLine> myCatch = new Dictionary<string, CatchLine>();
            DataTable dt = new DataTable();
            string CatchName = "";
            long? CatchCount = null;
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT tblSampling.SamplingGUID, tblCatchDetail.CatchCompRow, tblCatchComp.NameGUID, temp_AllNames.Name1,
                                   temp_AllNames.Name2, tblSampling.WtCatch, tblSampling.WtSample, tblCatchDetail.Live, tblCatchDetail.wt,
                                   tblCatchDetail.ct, tblCatchDetail.swt, tblCatchDetail.sct, tblCatchDetail.FromTotal
                                   FROM(tblSampling INNER JOIN(tblCatchComp INNER JOIN temp_AllNames ON tblCatchComp.NameGUID = temp_AllNames.NameNo)
                                   ON tblSampling.SamplingGUID = tblCatchComp.SamplingGUID) INNER JOIN tblCatchDetail ON tblCatchComp.RowGUID =
                                   tblCatchDetail.CatchCompRow WHERE tblSampling.SamplingGUID = {{{_SamplingGUID}}} ORDER BY tblCatchComp.Sequence";

                    Debug.WriteLine(query);
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    _TotalWtOfFromTotal = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        CatchCount = null;
                        DataRow dr = dt.Rows[i];
                        try { CatchName = dr["Name1"].ToString() + " " + dr["Name2"].ToString(); }
                        catch { CatchName = dr["Name1"].ToString(); }

                        if (dr["ct"].ToString().Length > 0)
                        {
                            try { CatchCount = int.Parse(dr["ct"].ToString()); }
                            catch { CatchCount = null; }
                        }

                        CatchLine myLine = new CatchLine(CatchName, dr["SamplingGUID"].ToString(),
                                        dr["CatchCompRow"].ToString(), dr["NameGUID"].ToString(),
                                        Convert.ToDouble(dr["wt"]), CatchCount);

                        myLine.CatchSubsampleWt = null;
                        myLine.CatchSubsampleCount = null;

                        if (dr["swt"] != DBNull.Value)
                        {
                            myLine.CatchSubsampleWt = Convert.ToDouble(dr["swt"]);
                        }

                        if (dr["sct"] != DBNull.Value)
                        {
                            myLine.CatchSubsampleCount = Convert.ToInt32(dr["sct"]);
                        }

                        myLine.FromTotalCatch = bool.Parse(dr["FromTotal"].ToString());
                        myCatch.Add(dr["CatchCompRow"].ToString(), myLine);

                        if (dr["FromTotal"].ToString() == "True")
                        {
                            _TotalWtOfFromTotal += Convert.ToDouble(dr["wt"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }

            return myCatch;
        }

        public Dictionary<string, string> CatchCompRow(string RowGUID)
        {
            Dictionary<string, string> myRow = new Dictionary<string, string>();
            DataTable dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT tblCatchComp.NameGUID, tblCatchComp.RowGUID, temp_AllNames.Name1, temp_AllNames.Name2,
                         tblSampling.WtCatch, tblSampling.WtSample, tblCatchDetail.Live, tblCatchDetail.wt, tblCatchDetail.ct,
                         tblCatchDetail.swt, tblCatchDetail.RowGUID as DetailRow, tblCatchDetail.sct, tblCatchDetail.FromTotal, tblCatchComp.NameType, tblTaxa.TaxaNo, tblTaxa.Taxa
                         FROM((tblSampling INNER JOIN(tblCatchComp INNER JOIN temp_AllNames ON tblCatchComp.NameGUID = temp_AllNames.NameNo)
                         ON tblSampling.SamplingGUID = tblCatchComp.SamplingGUID) INNER JOIN tblCatchDetail ON
                         tblCatchComp.RowGUID = tblCatchDetail.CatchCompRow) LEFT JOIN tblTaxa ON tblCatchComp.Taxa = tblTaxa.TaxaNo
                         WHERE tblCatchDetail.CatchCompRow ={{{RowGUID}}}";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        myRow.Add("DetailRow", dr["DetailRow"].ToString());
                        myRow.Add("RowGUID", dr["RowGUID"].ToString());
                        myRow.Add("NameGUID", dr["NameGUID"].ToString());
                        myRow.Add("NameType", dr["NameType"].ToString());
                        myRow.Add("Name1", dr["Name1"].ToString());
                        myRow.Add("Name2", dr["Name2"].ToString());
                        myRow.Add("WtCatch", dr["WtCatch"].ToString());
                        myRow.Add("WtSample", dr["WtSample"].ToString());
                        myRow.Add("wt", dr["wt"].ToString());
                        myRow.Add("ct", dr["ct"].ToString());
                        myRow.Add("swt", dr["swt"].ToString());
                        myRow.Add("sct", dr["sct"].ToString());
                        myRow.Add("FromTotal", dr["FromTotal"].ToString());
                        myRow.Add("Live", dr["Live"].ToString());
                        myRow.Add("TaxaNo", dr["TaxaNo"].ToString());
                        myRow.Add("Taxa", dr["Taxa"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }
            return myRow;
        }

        public Dictionary<string, string> GearsFromLandingSite(string lsguid)
        {
            Dictionary<string, string> myList = new Dictionary<string, string>();
            var dt = new DataTable();
            using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;data source=" + global.mdbPath))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT DISTINCT tblSampling.GearVarGUID, Variation FROM tblGearVariations
                                   INNER JOIN tblSampling ON tblGearVariations.GearVarGUID = tblSampling.GearVarGUID
                                   WHERE tblSampling.LSGUID ={{{lsguid}}}";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        myList.Add(dr["GearVarGUID"].ToString(), dr["Variation"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
                return myList;
            }
        }

        /// <summary>
        /// Reads LF data from the database and returns it as as Dictionary
        /// with the catch row as key and LF structure as value
        /// </summary>
        /// <param name="CatchCompRowNo"></param>
        /// <returns></returns>
        public Dictionary<string, LFLine> LFData(string CatchCompRowNo)
        {
            _LFRowsCount = 0;
            Dictionary<string, LFLine> mydata = new Dictionary<string, LFLine>();
            var dt = new DataTable();
            using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;data source=" + global.mdbPath))
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

                        var myLF = new LFLine
                        {
                            Length = double.Parse(dr["lenClass"].ToString()),
                            Freq = int.Parse(dr["Freq"].ToString()),
                            DataStatus = global.fad3DataStatus.statusFromDB,
                            LFRowGuid = dr["RowGUID"].ToString(),
                            CatchRowGuid = CatchCompRowNo,
                            Sequence = dr["Sequence"] == DBNull.Value ? -1 : int.Parse(dr["Sequence"].ToString())
                        };
                        mydata.Add(dr["RowGUID"].ToString(), myLF);
                    }
                    _LFRowsCount++;
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }

            return mydata;
        }

        public List<string> MonthsFromLSandGear(string landingsiteguid, string gearguid)
        {
            List<string> myList = new List<string>();
            var dt = new DataTable();
            using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;data source=" + global.mdbPath))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT DISTINCT Format([SamplingDate],'mmm - yyyy') AS sMonthYear
                                   FROM tblSampling WHERE GearVarGUID='{" + gearguid + "}' AND
                                   LSGUID={{{landingsiteguid}}} ORDER BY
                                   Format([SamplingDate],'mmm - yyyy')";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        myList.Add(dr["sMonthYear"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }
            return myList;
        }

        public List<string> OtherFishingGround()
        {
            List<string> myList = new List<string>();
            var dt = new DataTable();
            using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;data source=" + global.mdbPath))
            {
                try
                {
                    conection.Open();
                    string query = $"Select GridName from tblGrid where SamplingGUID = {{{_SamplingGUID}}}";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        myList.Add(dr["GridName"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }
            return myList;
        }

        public void ReadUIFromXML()
        {
            string xmlFile = global.AppPath + "\\UITable.xml";
            if (System.IO.File.Exists(xmlFile))
            {
                var doc = new XmlDocument();
                doc.Load(xmlFile);
                XmlElement root = doc.DocumentElement;
                XmlNodeList nodeList = root.SelectNodes("//UIRow");
                foreach (XmlNode n in nodeList)
                {
                    UIRowFromXML row = new UIRowFromXML();
                    foreach (XmlNode c in n)
                    {
                        ; string val = c.Name;
                        switch (val)
                        {
                            case "Required":
                                row.Required = bool.Parse(c.InnerText);
                                break;

                            case "ToolTip":
                                row.ToolTip = c.InnerText;
                                break;

                            case "ReadOnly":
                                row.ReadOnly = bool.Parse(c.InnerText);
                                break;

                            case "DataType":
                                row.DataType = c.InnerText;
                                break;

                            case "Key":
                                row.Key = c.InnerText;
                                break;

                            case "Label":
                                row.RowLabel = c.InnerText;
                                break;

                            case "Height":
                                row.Height = int.Parse(c.InnerText);
                                break;

                            case "ButtonText":
                                row.ButtonText = c.InnerText;
                                break;

                            case "control":
                                switch (c.InnerText)
                                {
                                    case "TextBox":
                                        row.Control = UserInterfaceStructure.UIControlType.TextBox;
                                        break;

                                    case "ComboBox":
                                        row.Control = UserInterfaceStructure.UIControlType.ComboBox;
                                        break;

                                    case "CheckBox":
                                        row.Control = UserInterfaceStructure.UIControlType.Check;
                                        break;

                                    case "MaskDate":
                                        row.Control = UserInterfaceStructure.UIControlType.DateMask;
                                        break;

                                    case "MaskTime":
                                        row.Control = UserInterfaceStructure.UIControlType.TimeMask;
                                        break;

                                    default:
                                        row.Control = UserInterfaceStructure.UIControlType.Spacer;
                                        break;
                                }
                                break;
                        }
                    }
                    OnUIRowRead(this, row);
                }
            }
        }

        public bool SaveEditedLF(Dictionary<string, LFLine> LFData)
        {
            var SaveCount = 0;
            foreach (KeyValuePair<string, LFLine> kv in LFData)
            {
                if (UpdateLF(kv.Value.Length, kv.Value.Freq, kv.Value.Sequence,
                          kv.Value.CatchRowGuid, kv.Value.LFRowGuid, kv.Value.DataStatus))
                    SaveCount++;
            }

            return SaveCount == LFData.Count;
        }

        public bool UpdateEffort(bool isNew, Dictionary<string, string> EffortData, List<string> FishingGrounds)
        {
            bool Success = false;
            string updateQuery = "";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    var quote = "'";
                    var hp = EffortData["EngineHorsepower"].Length == 0 ? "null" : EffortData["EngineHorsepower"];
                    var wtSample = EffortData["WeightOfSample"].Length == 0 ? "null" : EffortData["WeightOfSample"];
                    var wtCatch = EffortData["WeightOfCatch"].Length == 0 ? "null" : EffortData["WeightOfCatch"];
                    var engine = EffortData["Engine"].Length == 0 ? "" : EffortData["Engine"];
                    var vesL = string.IsNullOrWhiteSpace(EffortData["VesLength"]) ? "null" : EffortData["VesLength"];
                    var vesH = string.IsNullOrWhiteSpace(EffortData["VesHeight"]) ? "null" : EffortData["VesHeight"];
                    var vesW = string.IsNullOrWhiteSpace(EffortData["VesWidth"]) ? "null" : EffortData["VesWidth"];
                    var NoHauls = string.IsNullOrWhiteSpace(EffortData["NumberOfHauls"]) ? "null" : EffortData["NumberOfHauls"];
                    var NoFishers = string.IsNullOrWhiteSpace(EffortData["NumberOfFishers"]) ? "null" : EffortData["NumberOfFishers"];
                    var VesselType = string.IsNullOrWhiteSpace(EffortData["TypeOfVesselUsed"]) ? "null" : EffortData["TypeOfVesselUsed"];
                    var FishingGround = FishingGrounds.Count > 0 ? FishingGrounds[0] : "";
                    var DateSet = string.IsNullOrWhiteSpace(EffortData["DateSet"]) ? "null" : quote + EffortData["DateSet"] + quote;
                    var TimeSet = string.IsNullOrWhiteSpace(EffortData["TimeSet"]) ? "null" : quote + EffortData["TimeSet"] + quote;
                    var DateHauled = string.IsNullOrWhiteSpace(EffortData["DateHauled"]) ? "null" : quote + EffortData["DateHauled"] + quote;
                    var TimeHauled = string.IsNullOrWhiteSpace(EffortData["TimeHauled"]) ? "null" : quote + EffortData["TimeHauled"] + quote;
                    var SamplingGuid = EffortData["SamplingGUID"];

                    if (isNew)
                    {
                        updateQuery = $@"Insert into tblSampling (SamplingGUID, GearVarGUID, AOI, RefNo, SamplingDate, SamplingTime,
                            FishingGround, TimeSet, DateSet, TimeHauled, DateHauled, NoHauls, NoFishers, Engine, hp,
                            WtCatch, WtSample, len, wdt, hgt, LSGUID,  Notes, VesType, SamplingType, HasLiveFish, Enumerator,
                            DateEncoded) values (
                            {{{SamplingGuid}}},
                            {{{EffortData["FishingGear"]}}},
                            {{{EffortData["TargetArea"]}}},
                            '{EffortData["ReferenceNumber"]}',
                            '{EffortData["SamplingDate"]}',
                            '{EffortData["SamplingTime"]}',
                            '{FishingGround}',
                            {TimeSet},
                            {DateSet},
                            {TimeHauled},
                            {DateHauled},
                            {NoHauls},
                            {NoFishers},
                            '{engine}',
                            {hp},
                            {wtCatch},
                            {wtSample},
                            {vesL}, {vesW}, {vesH},
                            {{{EffortData["LandingSite"]}}},
                            '{EffortData["Notes"]}',
                            '{VesselType}',
                            {EffortData["SamplingType"]},
                            {EffortData["HasLiveFish"]},
                            {{{EffortData["Enumerator"]}}},
                            '{DateTime.Now}')";
                    }
                    else
                    {
                        updateQuery = $@"Update tblSampling set
                            GearVarGUID ={{{EffortData["FishingGear"]}}},
                            AOI ={{{EffortData["TargetArea"]}}},
                            RefNo ='{EffortData["ReferenceNumber"]}',
                            SamplingDate ='{EffortData["SamplingDate"]}',
                            SamplingTime ='{EffortData["SamplingTime"]}',
                            FishingGround = '{FishingGround}',
                            TimeSet ={TimeSet},
                            DateSet = {DateSet},
                            TimeHauled = {TimeHauled},
                            DateHauled = {DateHauled},
                            NoHauls = {NoHauls},
                            NoFishers = {NoFishers},
                            Engine ='{engine}',
                            hp = {hp},
                            WtCatch ={wtCatch},
                            WtSample ={wtSample},
                            len ={vesL}, wdt ={vesW}, hgt ={vesH},
                            LSGUID ={{{EffortData["LandingSite"]}}},
                            Notes = '{EffortData["Notes"]}',
                            VesType ={VesselType},
                            SamplingType ={EffortData["SamplingType"]},
                            HasLiveFish = {EffortData["HasLiveFish"]},
                            Enumerator = {{{EffortData["Enumerator"]}}}
                            Where SamplingGUID = {{{SamplingGuid}}}";
                    }

                    using (OleDbCommand update = new OleDbCommand(updateQuery, conn))
                    {
                        conn.Open();
                        Success = (update.ExecuteNonQuery() > 0);
                        conn.Close();
                    }

                    if (Success && FishingGrounds.Count > 1)
                        SaveAdditionalFishingGrounds(FishingGrounds, SamplingGuid);
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }

            return Success;
        }

        private void SaveAdditionalFishingGrounds(List<string> FishingGrounds, string SamplingGUID)
        {
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                var sql = $"Delete * from tblGrid where SamplingGuid = {{{SamplingGUID}}}";
                using (OleDbCommand update = new OleDbCommand(sql, conn))
                {
                    update.ExecuteNonQuery();
                }

                for (int n = 1; n < FishingGrounds.Count; n++)
                {
                    sql = $@"Insert into tblGrid (SamplingGuid, GridName,RowGUID) values
                            (
                              {{{SamplingGUID}}},
                              '{FishingGrounds[n]}',
                              {{{Guid.NewGuid()}}}
                            )";
                    using (OleDbCommand update = new OleDbCommand(sql, conn))
                    {
                        update.ExecuteNonQuery();
                    }
                }
                conn.Close();
            }
        }

        private bool UpdateLF(double LenClass, long ClassCount, int Sequence,
                                              string CatchCompRow, string RowGUID, global.fad3DataStatus DataStatus)
        {
            bool Success = false;
            string query = "";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    if (DataStatus == global.fad3DataStatus.statusNew)
                    {
                        query = $@"Insert into tblLF (LenClass, Freq, CatchCompRow, RowGUID, Sequence) values
                                    ({LenClass}, {ClassCount}, {{{CatchCompRow}}}, {{{RowGUID}}}, {Sequence})";
                    }
                    else if (DataStatus == global.fad3DataStatus.statusEdited)
                    {
                        query = $@"Update tblLF set
                                   LenClass = {LenClass},
                                   Freq= {ClassCount},
                                   Sequence= {Sequence}
                                   Where RowGUID ={{{RowGUID}}}";
                    }
                    else if (DataStatus == global.fad3DataStatus.statusForDeletion)
                    {
                        query = $"Delete * from tblLF where RowGUID = {{{RowGUID}}}";
                    }
                    if (query.Length > 0)
                    {
                        OleDbCommand update = new OleDbCommand(query, conn);
                        conn.Open();
                        Success = (update.ExecuteNonQuery() > 0);
                        conn.Close();
                    }
                    else
                    {
                        Success = true;
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }
            return Success;
        }

        public struct CatchLine
        {
            private readonly string _CatchLineGUID;

            private readonly string _CatchName;

            private readonly string _CatchNameGIUD;

            private readonly double _CatchWeight;

            private readonly string _SamplingGUID;

            private long? _CatchCount;

            private string _CatchDetailRowGUID;

            private long? _CatchSubsampleCount;

            private double? _CatchSubsampleWt;

            private bool _FromTotalCatch;

            private bool _LiveFish;

            private Identification _NameType;

            public CatchLine(string CatchName, string SamplingGUID,
                                          string CatchLineGUID, string CatchNameGUID,
                                          double CatchWeight, long? CatchCount = null)
            {
                _CatchName = CatchName;
                _SamplingGUID = SamplingGUID;
                _CatchLineGUID = CatchLineGUID;
                _CatchNameGIUD = CatchNameGUID;
                _CatchWeight = CatchWeight;
                _CatchCount = CatchCount;
                _CatchSubsampleWt = 0;
                _CatchSubsampleCount = 0;
                _FromTotalCatch = false;
                _NameType = Identification.Scientific;
                _LiveFish = false;
                _CatchDetailRowGUID = "";
            }

            public CatchLine(string CatchName, string SamplingGUID,
                                          string CatchLineGUID, string CatchNameGUID,
                                          double CatchWeight)
            {
                _CatchName = CatchName;
                _SamplingGUID = SamplingGUID;
                _CatchLineGUID = CatchLineGUID;
                _CatchNameGIUD = CatchNameGUID;
                _CatchWeight = CatchWeight;
                _CatchCount = null;
                _CatchSubsampleWt = 0;
                _CatchSubsampleCount = 0;
                _FromTotalCatch = false;
                _NameType = Identification.Scientific;
                _LiveFish = false;
                _CatchDetailRowGUID = "";
            }

            /// <summary>
            /// A structure representing an individual catch which includes catch wt, count, subsample wt and count,
            /// the guid of the parent sampling (SamplingGUID) and the guid of the individual catch.
            /// </summary>
            public enum Identification
            {
                Scientific = 1,
                LocalName
            }

            public long? CatchCount
            {
                set { _CatchCount = value; }
                get { return _CatchCount; }
            }

            public string CatchDetailRowGUID
            {
                get { return _CatchDetailRowGUID; }
                set { _CatchDetailRowGUID = value; }
            }

            public string CatchLineGUID
            {
                get { return _CatchLineGUID; }
            }

            public string CatchName
            {
                get { return _CatchName; }
            }

            public string CatchNameGUID
            {
                get { return _CatchNameGIUD; }
            }

            public long? CatchSubsampleCount
            {
                get { return _CatchSubsampleCount; }
                set { _CatchSubsampleCount = value; }
            }

            public double? CatchSubsampleWt
            {
                get { return _CatchSubsampleWt; }
                set { _CatchSubsampleWt = value; }
            }

            public double CatchWeight
            {
                get { return _CatchWeight; }
            }

            public bool FromTotalCatch
            {
                get { return _FromTotalCatch; }
                set { _FromTotalCatch = value; }
            }

            public bool LiveFish
            {
                get { return _LiveFish; }
                set { _LiveFish = value; }
            }

            public Identification NameType
            {
                get { return _NameType; }
                set { _NameType = value; }
            }

            public string SamplingGUID
            {
                get { return _SamplingGUID; }
            }

            public bool Save(bool isNew)
            {
                //string RowGUID = "";
                bool Success = false;
                string updateQuery = "";
                using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
                {
                    try
                    {
                        if (isNew)
                        {
                            //RowGUID = Guid.NewGuid().ToString();
                            updateQuery = $@"Insert into tblCatchComp (NameGUID, NameType,RowGUID,SamplingGUID) values (
                                          {{{ _CatchNameGIUD}}}, {Convert.ToInt16(_NameType)}, {{{_CatchLineGUID}}}, {{{_SamplingGUID}}})";
                        }
                        else
                        {
                            if (SaveCatchDetail(isNew, "", _CatchDetailRowGUID))
                            {
                                updateQuery = $@"Update tblCatchComp set
                                    NameGUID = {{{_CatchNameGIUD}}},
                                    NameType = {Convert.ToInt16(_NameType)}
                                    WHERE RowGUID = {{{_CatchLineGUID}}}";
                            }
                        }
                        OleDbCommand update = new OleDbCommand(updateQuery, conn);
                        conn.Open();
                        Success = (update.ExecuteNonQuery() > 0);
                        conn.Close();
                        if (Success)
                        {
                            if (isNew)
                            {
                                Success = SaveCatchDetail(isNew, _CatchLineGUID);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.Log(ex);
                    }
                }
                return Success;
            }

            private bool SaveCatchDetail(bool isNew, string CatchCompRow = "", string CatchDetailRowGUID = "")
            {
                bool Success = false;
                string updateQuery = "";
                string RowGUID = "";
                using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
                {
                    try
                    {
                        if (isNew)
                        {
                            RowGUID = Guid.NewGuid().ToString();

                            if (_CatchCount == null)
                            {
                                updateQuery = $@"Insert into tblCatchDetail (wt,swt,sct,CatchCompRow,RowGUID,FromTotal,Live) values (
                                               {_CatchWeight}, {_CatchSubsampleWt}, {_CatchSubsampleCount}, {{{CatchCompRow}}}, {{{RowGUID}}},
                                               {_FromTotalCatch}, {_LiveFish})";
                            }
                            else
                            {
                                updateQuery = $@"Insert into tblCatchDetail (wt,ct,swt,sct,CatchCompRow,RowGUID,FromTotal,Live) values (
                                               {_CatchWeight}, {(long)_CatchCount},  {_CatchSubsampleWt}, {_CatchSubsampleCount},
                                               {{{CatchCompRow}}}, {{{RowGUID}}}, {_FromTotalCatch}, {_LiveFish})";
                            }
                        }
                        else
                        {
                            if (_CatchCount == null)
                            {
                                updateQuery = $@"Update tblCatchDetail set
                                           wt = {CatchWeight}, swt= {_CatchSubsampleWt},
                                           sct = {_CatchSubsampleCount}, FromTotal = {_FromTotalCatch},
                                           Live = {_LiveFish} Where RowGUID = {{{CatchDetailRowGUID}}}";
                            }
                            else
                            {
                                updateQuery = $@"Update tblCatchDetail set
                                           wt = {_CatchWeight},
                                           ct = {_CatchCount},
                                           swt = {_CatchSubsampleWt},
                                           sct = {_CatchSubsampleCount},
                                           FromTotal = {_FromTotalCatch},
                                           Live = {_LiveFish}
                                           Where RowGUID = {{{CatchDetailRowGUID}}}";
                            }
                        }
                        OleDbCommand update = new OleDbCommand(updateQuery, conn);
                        conn.Open();
                        Success = (update.ExecuteNonQuery() > 0);
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.Log(ex);
                    }
                }
                return Success;
            }
        }

        public struct GMSLine
        {
            public string CatchRowGUID { get; set; }
            public global.FishCrabGMS GMS { get; set; }
            public double? GonadWeight { get; set; }
            public double? Length { get; set; }
            public global.sex Sex { get; set; }
            public global.Taxa Taxa { get; set; }
            public string TaxaName { get; set; }
            public double? Weight { get; set; }
            public global.fad3DataStatus DataStatus { get; set; }
        }

        public struct LFLine
        {
            public string CatchRowGuid { get; set; }
            public global.fad3DataStatus DataStatus { get; set; }
            public int Freq { get; set; }
            public double Length { get; set; }
            public string LFRowGuid { get; set; }
            public int Sequence { get; set; }
        }

        /// <summary>
        /// represents the data structure of effort data and will be
        /// used to generate the user interface of the
        /// effort data edit form
        /// </summary>
        public struct UserInterfaceStructure
        {
            public UIControlType _control;

            private string _ButtonText;

            private string _DataType;

            private int _Height;

            private string _Key;

            private string _Label;

            private bool _ReadOnly;

            private bool _Required;

            private string _ToolTip;

            //CONSTRUCTOR
            public UserInterfaceStructure(UIControlType c, string Label, string Key, int Height,
                                            string DataType, bool ReadOnly, string ToolTip,
                                            bool Required, string ButtonText = "")
            {
                _control = c;
                _Label = Label;
                _Key = Key;
                _ButtonText = ButtonText;
                _Height = Height;
                _DataType = DataType;
                _ReadOnly = ReadOnly;
                _ToolTip = ToolTip;
                _Required = Required;
            }

            public enum UIControlType
            {
                TextBox,
                ComboBox,
                Spacer,
                DateMask,
                TimeMask,
                Check
            }

            public string ButtonText
            {
                get { return _ButtonText; }
                set { _ButtonText = value; }
            }

            public UIControlType Control
            {
                get { return _control; }
                set { _control = value; }
            }

            public string DataType
            {
                get { return _DataType; }
                set { _DataType = value; }
            }

            public int Height
            {
                get { return _Height; }
                set { _Height = value; }
            }

            public string Key
            {
                get { return _Key; }
                set { _Key = value; }
            }

            public string Label
            {
                get { return _Label; }
                set { _Label = value; }
            }

            public bool ReadOnly
            {
                get { return _ReadOnly; }
                set { _ReadOnly = value; }
            }

            public bool Required
            {
                get { return _Required; }
                set { _Required = value; }
            }

            public string ToolTip
            {
                get { return _ToolTip; }
                set { _ToolTip = value; }
            }
        }
    }
}
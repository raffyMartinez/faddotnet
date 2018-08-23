/*
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/8/2016
 * Time: 11:01 AM
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Data.OleDb;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Drawing;

namespace FAD3
{
    /// <summary>
    /// Description of global.
    /// </summary>
    ///
    public static class global
    {
        private static Dictionary<string, string> _VesselTypeDict = new Dictionary<string, string>();
        private static Dictionary<long, string> _provinceDict = new Dictionary<long, string>();
        private static Dictionary<long, string> _munDict = new Dictionary<long, string>();
        private static string _mdbPath = "";
        private static string _ConnectionString = "";
        private static string _AppPath = "";
        private static bool _ShowErrorMessage = false;
        private static bool _HasMPH = false;
        private static bool _MapIsOpen;
        private static string _templateMDBFile = "";
        private static readonly string _ConnectionStringTemplate = "";
        private static bool _TemplateFileExists = true;
        private static bool _UITemplateFileExists = true;
        private static bool _InlandGridDBFileExists = true;
        private static bool _AllRequiredFilesExists = true;
        private static CoordinateDisplayFormat _CoordDisplayFormat = CoordinateDisplayFormat.DegreeDecimal;
        private static Color _MissingFieldBackColor = global.MissingFieldBackColor;

        //private static string _MissingRequiredFiles;

        /// <summary>
        /// String. Returns the files required by the application that are missing
        /// </summary>
        public static string MissingRequiredFiles
        {
            get
            {
                var s = _TemplateFileExists ? "" : "\r\n- template.mdb";
                s += _UITemplateFileExists ? "" : "\r\n- UITable.xml";
                s += _InlandGridDBFileExists ? "" : "\r\n- grid25inland.mdb";

                return s;
            }
        }

        public static string TemplateMDBFile
        {
            get { return _templateMDBFile; }
        }

        public static MainForm mainForm { get; set; }

        public static Color ConflictColor1
        {
            get { return Color.Yellow; }
        }

        public static Color ConflictColor2
        {
            get { return Color.Gold; }
        }

        public static Color MissingFieldBackColor
        {
            get { return _MissingFieldBackColor; }
        }

        private static void GetAppPreferences()
        {
            //the values here should be from an external source to prevent hardcoding
            _MissingFieldBackColor = Color.Orange;
        }

        public static
            List<(string GearClassName, string Variation, string LandingSiteGuid, string GearVariationGuid, string SamplingMonthYear)>
            TreeSubNodes(string treeLevel, string landingSiteGuid, string gearVariationGuid = "")
        {
            var list = new List<(string GearClassName, string Variation, string LandingSiteGuid, string GearVariationGuid, string SamplingMonthYear)>();
            var query = "";
            using (var dataTable = new DataTable())
            {
                try
                {
                    using (var conection = new OleDbConnection($"Provider=Microsoft.JET.OLEDB.4.0;data source={global.mdbPath}"))
                    {
                        conection.Open();

                        if (treeLevel == "landing_site")
                        {
                            query = $@"SELECT DISTINCT tblGearClass.GearClassName, tblGearVariations.Variation, tblSampling.LSGUID,
                                     tblGearVariations.GearVarGUID FROM tblGearClass INNER JOIN
                                     (tblGearVariations INNER JOIN tblSampling ON tblGearVariations.GearVarGUID = tblSampling.GearVarGUID)
                                     ON tblGearClass.GearClass = tblGearVariations.GearClass
                                     WHERE tblSampling.LSGUID = {{{landingSiteGuid}}}
                                     ORDER BY tblGearClass.GearClassName, tblGearVariations.Variation";
                        }
                        else if (treeLevel == "gear")
                        {
                            query = $@"SELECT Format([SamplingDate],'mmm-yyyy') AS sDate FROM tblSampling
                                    GROUP BY Format([SamplingDate],'mmm-yyyy'), tblSampling.LSGUID, tblSampling.GearVarGUID,
                                    Year([SamplingDate]), Month([SamplingDate])
                                    HAVING LSGUID ={{{landingSiteGuid}}} AND GearVarGUID = {{{gearVariationGuid}}}
                                    ORDER BY Year([SamplingDate]), Month([SamplingDate])";
                        }

                        using (var adapter = new OleDbDataAdapter(query, conection))
                        {
                            adapter.Fill(dataTable);
                            for (int i = 0; i < dataTable.Rows.Count; i++)
                            {
                                DataRow dr = dataTable.Rows[i];
                                if (treeLevel == "landing_site")
                                {
                                    list.Add(
                                              (dr["GearClassName"].ToString(),
                                               dr["Variation"].ToString(),
                                               dr["LSGUID"].ToString(),
                                               dr["GearVarGUID"].ToString(),
                                               "")
                                              );
                                }
                                else if (treeLevel == "gear")
                                {
                                    list.Add(
                                              ("",
                                               "",
                                               "",
                                               "",
                                                dr["sDate"].ToString())
                                              );
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
                return list;
            }
        }

        /// <summary>
        /// returns a list that will fill a tree containing target areas and landing sites
        /// </summary>
        /// <returns></returns>
        public static List<(string AOIGuid, string AOIName, string LandingSiteGuid, string LandingSiteName)> TreeNodes()
        {
            var list = new List<(string AOIGuid, string AOIName, string LandingSiteGuid, string LandingSiteName)>();
            using (var dataTable = new DataTable())
            {
                try
                {
                    using (var conection = new OleDbConnection($"Provider=Microsoft.JET.OLEDB.4.0;data source={global.mdbPath}"))
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
                                list.Add(
                                          (dr["AOIGuid"].ToString(),
                                           dr["AOIName"].ToString(),
                                           dr["LSGUID"].ToString(),
                                           dr["LSName"].ToString())
                                          );
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
            return list;
        }

        public static CoordinateDisplayFormat CoordinateDisplay
        {
            get { return _CoordDisplayFormat; }
            set
            {
                _CoordDisplayFormat = value;
                SaveCoordinateDisplayFormat();
            }
        }

        public static string MDBPath
        {
            get { return _mdbPath; }
        }

        /// <summary>
        /// Boolean. returns a boolean if all files required by the application are present
        /// </summary>
        public static bool AllRequiredFilesExists
        {
            get { return _AllRequiredFilesExists; }
        }

        /// <summary>
        /// Boolean. Whether or not the template MDB file exists
        /// </summary>
        public static bool UITemplateFileExists
        {
            get { return _UITemplateFileExists; }
        }

        /// <summary>
        /// Boolean. Whether or not the mdb of inland grid is present
        /// </summary>
        public static bool InlandGridDBFileExists
        {
            get { return _InlandGridDBFileExists; }
        }

        /// <summary>
        /// Boolean. Whether not the template mdb file is present
        /// </summary>
        public static bool TemplateFileExists
        {
            get { return _TemplateFileExists; }
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        static global()
        {
            GetAppPreferences();
            TestMPH();
            _AppPath = Application.StartupPath.ToString();
            _templateMDBFile = ApplicationPath + "\\template.mdb";
            _ConnectionStringTemplate = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + _AppPath + "\\template.mdb";
            ReferenceNumberManager.ReadRefNoRange();
            GetCoordinateDisplayFormat();
        }

        /// <summary>
        /// Test if the files required by the application to run are present
        /// </summary>
        public static void TestRequiredFilesExists()
        {
            _UITemplateFileExists = File.Exists(ApplicationPath + "\\UITable.xml");
            _TemplateFileExists = File.Exists(_templateMDBFile);
            _InlandGridDBFileExists = File.Exists(ApplicationPath + "\\grid25inland.mdb");
            _AllRequiredFilesExists = _UITemplateFileExists && _TemplateFileExists && _InlandGridDBFileExists;
        }

        /// <summary>
        /// String. Returns the application path
        /// </summary>
        public static string ApplicationPath
        {
            get { return _AppPath; }
        }

        /// <summary>
        /// Dictionary. Returns vessel types
        /// </summary>
        public static Dictionary<string, string> VesselTypeDict
        {
            get { return _VesselTypeDict; }
        }

        /// <summary>
        /// set window position from registry
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="GetPosition"></param>
        public static void LoadFormSettings(Form frm, bool GetPosition = false)
        {
            // Load form settings.
            if (GetPosition)
            {
                frm.Location = new System.Drawing.Point(
                    (int)GetSetting(frm.Name, "FormLeft", frm.Left),
                    (int)GetSetting(frm.Name, "FormTop", frm.Top)
                    );
            }
            else
            {
                frm.SetBounds(
                    (int)GetSetting(frm.Name, "FormLeft", frm.Left),
                    (int)GetSetting(frm.Name, "FormTop", frm.Top),
                    (int)GetSetting(frm.Name, "FormWidth", frm.Width),
                    (int)GetSetting(frm.Name, "FormHeight", frm.Height));
            }
            frm.WindowState = (FormWindowState)GetSetting(frm.Name, "FormWindowState", frm.WindowState);
        }

        /// <summary>
        /// Get one of a form's position value
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="name"></param>
        /// <param name="default_value"></param>
        /// <returns></returns>
        private static object GetSetting(string formName, string name, object default_value)
        {
            RegistryKey reg_key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\FAD3", true);
            RegistryKey sub_key = reg_key.CreateSubKey("FORMSTATE");
            RegistryKey sub_key1 = sub_key.CreateSubKey(formName);
            return sub_key1.GetValue(name, default_value);
        }

        /// <summary>
        /// save window position to registry
        /// </summary>
        /// <param name="frm"></param>
        public static void SaveFormSettings(Form frm)
        {
            // Save form settings.
            SaveSetting(frm.Name, "FormWindowState", (int)frm.WindowState);
            if (frm.WindowState == FormWindowState.Normal)
            {
                // Save current bounds.
                SaveSetting(frm.Name, "FormLeft", frm.Left);
                SaveSetting(frm.Name, "FormTop", frm.Top);
                SaveSetting(frm.Name, "FormWidth", frm.Width);
                SaveSetting(frm.Name, "FormHeight", frm.Height);
            }
            else
            {
                // Save bounds when we're restored.
                SaveSetting(frm.Name, "FormLeft", frm.RestoreBounds.Left);
                SaveSetting(frm.Name, "FormTop", frm.RestoreBounds.Top);
                SaveSetting(frm.Name, "FormWidth", frm.RestoreBounds.Width);
                SaveSetting(frm.Name, "FormHeight", frm.RestoreBounds.Height);
            }
        }

        public static bool TextIsAlphaNumeric(string text)
        {
            var arr = text.ToLower().ToCharArray();
            var IsAlphaNumeric = true;
            for (int n = 0; n < arr.Length; n++)
            {
                if ((arr[n] < '0' || arr[n] > '9') && (arr[n] < 'a' || arr[n] > 'z'))
                {
                    IsAlphaNumeric = false;
                    break;
                }
            }
            return IsAlphaNumeric;
        }

        /// <summary>
        /// save window position to registry
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        private static void SaveSetting(string formName, string name, object value)
        {
            RegistryKey reg_key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\FAD3", true);
            RegistryKey sub_key = reg_key.CreateSubKey("FORMSTATE");
            RegistryKey sub_key1 = sub_key.CreateSubKey(formName);
            sub_key1.SetValue(name, value);
        }

        private static void GetCoordinateDisplayFormat()
        {
            var rv = "";
            RegistryKey reg_key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\FAD3", true);
            try
            {
                rv = reg_key.GetValue("CoordinateFormat").ToString();
            }
            catch
            {
                rv = CoordinateDisplayFormat.DegreeDecimal.ToString();
            }
            _CoordDisplayFormat = (CoordinateDisplayFormat)Enum.Parse(typeof(CoordinateDisplayFormat), rv);
        }

        private static void SaveCoordinateDisplayFormat()
        {
            RegistryKey reg_key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\FAD3", true);
            reg_key.SetValue("CoordinateFormat", _CoordDisplayFormat.ToString());
        }

        public static bool HasMPH
        {
            get { return _HasMPH; }
        }

        private static void TestMPH()
        {
            short k1 = 0;
            short k2 = 0;
            MetaphoneCOM.DoubleMetaphoneShort mph = new MetaphoneCOM.DoubleMetaphoneShort();
            mph.ComputeMetaphoneKeys("test", out k1, out k2);
            _HasMPH = k1 > 0;
        }

        public enum fad3CatchSubRow
        {
            none,
            LF,
            GMS
        }

        public enum fad3DataStatus
        {
            statusFromDB,
            statusNew,
            statusEdited,
            statusForDeletion
        }

        public enum fad3GearEditAction
        {
            addAOI,
            addLocalName,
            addGearCode,
            addGearVariation,
            editLocalName,
            editGearVariation,
            editGearCode
        }

        public enum lvContext
        {
            None = 0,
            CatchAndEffort,
            CatchComposition,
            EnumeratorSampling,
            LengthFreq,
            GMS
        }

        public enum CatchMeristics
        {
            LengtFreq,
            LengthWeightSexMaturity,
            LenghtWeightSex,
        }

        public enum CoordinateDisplayFormat
        {
            DegreeDecimal,
            DegreeMinute,
            DegreeMinuteSecond,
            UTM
        }

        public static bool ShowErrorMessage
        {
            get { return _ShowErrorMessage; }
            set { _ShowErrorMessage = value; }
        }

        public static string AppPath
        {
            get { return _AppPath; }
        }

        /// <summary>
        /// fills a dictionary variable with municipalities given ProvinceNo
        /// </summary>
        /// <param name="ProvinceNo"></param>
		public static void MunicipalitiesFromProvinceNo(long ProvinceNo)
        {
            var myDT = new DataTable();
            using (var conection = new OleDbConnection(_ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $"Select MunNo, Municipality from Municipalities where ProvNo=  {ProvinceNo} Order By Municipality";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(myDT);
                    _munDict.Clear();
                    for (int i = 0; i < myDT.Rows.Count; i++)
                    {
                        DataRow dr = myDT.Rows[i];
                        _munDict.Add(Convert.ToInt32(dr["MunNo"]), dr["Municipality"].ToString());
                    }
                }
                catch (Exception ex) { Logger.Log(ex); }
            }
        }

        public static Dictionary<long, string> munDict
        {
            get { return _munDict; }
        }

        public static Dictionary<long, string> provinceDict
        {
            get { return _provinceDict; }
        }

        /// <summary>
        /// getter and setter for the path to the mdb. After setting, some variables are filled up
        /// </summary>
        public static string mdbPath
        {
            get { return _mdbPath; }
            set
            {
                if (DBCheck.CheckDB(value))
                {
                    _mdbPath = value;
                    _ConnectionString = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + _mdbPath;
                    names.MakeAllNames();
                    FishingVessel.MakeVesselTypeTable();
                    GetProvinces();
                    gear.FillGearClasses();
                    GetVesselTypes();
                }
            }
        }

        private static void GetVesselTypes()
        {
            _VesselTypeDict.Clear();
            var myDT = new DataTable();
            using (var conection = new OleDbConnection(_ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = "Select VesselTypeNo, VesselType from temp_VesselType";
                    using (var adapter = new OleDbDataAdapter(query, conection))
                    {
                        adapter.Fill(myDT);
                        for (int i = 0; i < myDT.Rows.Count; i++)
                        {
                            DataRow dr = myDT.Rows[i];
                            _VesselTypeDict.Add(dr[0].ToString(), dr[1].ToString());
                        }
                    }
                }
                catch (Exception ex) { Logger.Log(ex); }
            }
        }

        public static string ConnectionString
        {
            get
            {
                return _ConnectionString;
            }
        }

        /// <summary>
        /// fills a local dictionary variable with provinces
        /// </summary>
        private static void GetProvinces()
        {
            _provinceDict.Clear();
            var myDT = new DataTable();
            using (var conection = new OleDbConnection(_ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = "Select ProvNo, ProvinceName from Provinces Order By ProvinceName";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(myDT);
                    for (int i = 0; i < myDT.Rows.Count; i++)
                    {
                        DataRow dr = myDT.Rows[i];
                        _provinceDict.Add(Convert.ToInt32(dr[0]), dr[1].ToString());
                    }
                }
                catch (Exception ex) { Logger.Log(ex); }
            }
        }

        public static void Export2Excel(ListView lv, string xlFileName)
        {
            try
            {
                //lvPDF is nothing but the listview control name
                string[] st = new string[lv.Columns.Count];
                /*DirectoryInfo di = new DirectoryInfo(@"c:\PDFExtraction\");
				if (di.Exists == false)
					di.Create();*/
                StreamWriter sw = new StreamWriter(xlFileName, false);
                sw.AutoFlush = true;
                for (int col = 0; col < lv.Columns.Count; col++)
                {
                    if (col == 0)
                    {
                        sw.Write(lv.Columns[col].Text.ToString());
                    }
                    else
                    {
                        sw.Write("\t" + lv.Columns[col].Text.ToString());
                    }
                }
                //sw.WriteLine("\n");
                int rowIndex = 1;
                int row = 0;
                string st1 = "\n";
                for (row = 0; row < lv.Items.Count; row++)
                {
                    if (rowIndex <= lv.Items.Count)
                        rowIndex++;

                    for (int col = 0; col < lv.Columns.Count; col++)
                    {
                        if (col == 0)
                        {
                            st1 = st1 + lv.Items[row].SubItems[col].Text.ToString();
                        }
                        else
                        {
                            st1 = st1 + "\t" + lv.Items[row].SubItems[col].Text.ToString();
                        }
                    }
                    sw.WriteLine(st1);
                    st1 = "";
                }
                sw.Close();
                FileInfo fil = new FileInfo(xlFileName);
                if (fil.Exists == true)
                    MessageBox.Show("Process Completed", "Export to Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

        public static bool MapIsOpen
        {
            get { return _MapIsOpen; }
            set { _MapIsOpen = value; }
        }
    }
}
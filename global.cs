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
using ADOX;
using Microsoft.Win32;

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
        private static readonly string _ConnectionStringTemplate = "";
        private static bool _TemplateFileExists = true;
        private static bool _UITemplateFileExists = true;
        private static bool _InlandGridDBFileExists = true;
        private static bool _AllRequiredFilesExists = true;
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
            TestMPH();
            _AppPath = Application.StartupPath.ToString();
            _ConnectionStringTemplate = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + _AppPath + "\\template.mdb";
            ReferenceNumberManager.ReadRefNoRange();
        }

        /// <summary>
        /// Test if the files required by the application to run are present
        /// </summary>
        public static void TestRequiredFilesExists()
        {
            _UITemplateFileExists = File.Exists(ApplicationPath + "\\UITable.xml");
            _TemplateFileExists = File.Exists(ApplicationPath + "\\template.mdb");
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

        public enum sex
        {
            Juvenile,
            Male,
            Female
        }

        public enum Taxa
        {
            To_be_determined,
            Fish,
            Shrimps,
            Cephalopods,
            Crabs,
            Shells,
            Lobsters,
            Sea_cucumbers,
            Sea_urchins,
        }

        public enum FishCrabGMS
        {
            AllTaxaNotDetermined,
            FishJuvenile = 1,
            FishStg1Immature,
            FishStg2Maturing,
            FishStg3Mature,
            FishStg4Gravid,
            FishStg5Spent,
            FemaleCrabImmature = 2,
            FemaleCrabMature = 4,
            FemaleCrabBerried,
        }

        public static bool ShowErrorMessage
        {
            get { return _ShowErrorMessage; }
            set { _ShowErrorMessage = value; }
        }

        public static Dictionary<FishCrabGMS, string> GMSStages(Taxa taxa, ref bool Success)
        {
            Success = false;
            Dictionary<FishCrabGMS, string> myStages = new Dictionary<FishCrabGMS, string>();
            switch (taxa)
            {
                case Taxa.Fish:
                    myStages.Add(FishCrabGMS.AllTaxaNotDetermined, "Not determined");
                    myStages.Add(FishCrabGMS.FishJuvenile, "Juvenile");
                    myStages.Add(FishCrabGMS.FishStg1Immature, "Immature");
                    myStages.Add(FishCrabGMS.FishStg2Maturing, "Maturing");
                    myStages.Add(FishCrabGMS.FishStg3Mature, "Mature");
                    myStages.Add(FishCrabGMS.FishStg4Gravid, "Gravid");
                    myStages.Add(FishCrabGMS.FishStg5Spent, "Spent");
                    Success = true;
                    break;

                case Taxa.Crabs:
                    myStages.Add(FishCrabGMS.AllTaxaNotDetermined, "Not determined");
                    myStages.Add(FishCrabGMS.FemaleCrabImmature, "Immature");
                    myStages.Add(FishCrabGMS.FemaleCrabMature, "Mature");
                    myStages.Add(FishCrabGMS.FemaleCrabBerried, "Berried");
                    Success = true;
                    break;

                case Taxa.Lobsters:
                case Taxa.Sea_cucumbers:
                case Taxa.Sea_urchins:
                case Taxa.Shells:
                case Taxa.Shrimps:
                case Taxa.To_be_determined:
                    break;
            }
            return myStages;
        }

        public static string AppPath
        {
            get { return _AppPath; }
        }

        public static string GMSStage(Taxa taxa, FishCrabGMS stage)
        {
            string gms_stage = "";
            switch (taxa.ToString())
            {
                case "To_be_determined":
                    break;

                case "Fish":
                    switch (stage)
                    {
                        case FishCrabGMS.AllTaxaNotDetermined:
                            gms_stage = "Not determined";
                            break;

                        case FishCrabGMS.FishJuvenile:
                            gms_stage = "Juvenile";
                            break;

                        case FishCrabGMS.FishStg1Immature:
                            gms_stage = "Immature";
                            break;

                        case FishCrabGMS.FishStg2Maturing:
                            gms_stage = "Maturing";
                            break;

                        case FishCrabGMS.FishStg3Mature:
                            gms_stage = "Mature";
                            break;

                        case FishCrabGMS.FishStg4Gravid:
                            gms_stage = "Gravid";
                            break;

                        case FishCrabGMS.FishStg5Spent:
                            gms_stage = "Spent";
                            break;
                    }
                    break;

                case "Shrimps":
                    break;

                case "Cephalopods":
                    break;

                case "Crabs":
                    switch (stage)
                    {
                        case FishCrabGMS.AllTaxaNotDetermined:
                            gms_stage = "Not determined";
                            break;

                        case FishCrabGMS.FemaleCrabImmature:
                            gms_stage = "Immature";
                            break;

                        case FishCrabGMS.FemaleCrabMature:
                            gms_stage = "Mature";
                            break;

                        case FishCrabGMS.FemaleCrabBerried:
                            gms_stage = "Berried";
                            break;
                    }
                    break;

                case "Shells":
                    break;

                case "Lobsters":
                    break;

                case "Sea_cucumbers":
                    break;

                case "Sea_urchins":
                    break;
            }
            return gms_stage;
        }

        public static FishCrabGMS MaturityStageFromText(string stage, Taxa taxa)
        {
            FishCrabGMS myStage = FishCrabGMS.AllTaxaNotDetermined;
            switch (taxa)
            {
                case Taxa.Fish:
                    switch (stage)
                    {
                        case "Not determined":
                            myStage = FishCrabGMS.AllTaxaNotDetermined;
                            break;

                        case "Juvenile":
                            myStage = FishCrabGMS.FishJuvenile;
                            break;

                        case "Immature":
                            myStage = FishCrabGMS.FishStg1Immature;
                            break;

                        case "Maturing":
                            myStage = FishCrabGMS.FishStg2Maturing;
                            break;

                        case "Mature":
                            myStage = FishCrabGMS.FishStg3Mature;
                            break;

                        case "Gravid":
                            myStage = FishCrabGMS.FishStg4Gravid;
                            break;

                        case "Spent":
                            myStage = FishCrabGMS.FishStg5Spent;
                            break;
                    }
                    break;

                case Taxa.Crabs:
                    switch (stage)
                    {
                        case "Not determined":
                            myStage = FishCrabGMS.AllTaxaNotDetermined;
                            break;

                        case "Immature":
                            myStage = FishCrabGMS.FemaleCrabImmature;
                            break;

                        case "Mature":
                            myStage = FishCrabGMS.FemaleCrabMature;
                            break;

                        case "Berried":
                            myStage = FishCrabGMS.FemaleCrabBerried;
                            break;
                    }
                    break;

                default:
                    myStage = FishCrabGMS.AllTaxaNotDetermined;
                    break;
            }

            return myStage;
        }

        public static bool CheckDB(string mdbPath)
        {
            bool cancel = false;

            //put data tables here
            List<string> tableList = new List<string>();

            //put columns here
            List<string> colList = new List<string>();
            //if (File.Exists(_AppPath + "\\template.mdb"))
            //{
            string connTemplate = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + _AppPath + "\\template.mdb";
            string connMDB = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + mdbPath;
            Catalog catTemplate = new Catalog();
            Catalog catMDB = new Catalog();
            try
            {
                catTemplate.let_ActiveConnection(connTemplate);
                catMDB.let_ActiveConnection(connMDB);

                //fill up list of data tables
                foreach (Table tblData in catMDB.Tables)
                {
                    string tdName = tblData.Name;
                    if (tdName.Substring(0, 4) != "MSys" && tdName.Substring(0, 5) != "temp_")
                    {
                        tableList.Add(tdName);
                    }
                }

                //get table names  in the template database
                foreach (Table tblTemplate in catTemplate.Tables)
                {
                    string tdName = tblTemplate.Name;
                    if (tdName.Substring(0, 4) != "MSys" && tdName.Substring(0, 5) != "temp_")
                    {
                        if (tableList.Contains(tdName))
                        {
                            //check for columns
                            foreach (Column col in catMDB.Tables[tdName].Columns)
                            {
                                colList.Add(col.Name);
                            }

                            foreach (Column col in tblTemplate.Columns)
                            {
                                var mdbCol = new Column();
                                if (!colList.Contains(col.Name))
                                {
                                    catMDB.Tables[tdName].Columns.Append(col.Name, col.Type, col.DefinedSize);
                                }
                                mdbCol = catMDB.Tables[tdName].Columns[col.Name];
                                try
                                {
                                    mdbCol.Properties["Jet OLEDB:Allow Zero Length"].Value = col.Properties["Jet OLEDB:Allow Zero Length"].Value;
                                    mdbCol.Properties["Description"].Value = col.Properties["Description"].Value;
                                }
                                catch { }
                            }

                            //check for indexes
                            colList.Clear();

                            foreach (Index i in catMDB.Tables[tdName].Indexes)
                            {
                                colList.Add(i.Name);
                            }

                            foreach (Index i in tblTemplate.Indexes)
                            {
                                if (!colList.Contains(i.Name))
                                {
                                    Index newIndex = new Index
                                    {
                                        Name = i.Name,
                                        Unique = i.Unique,
                                        PrimaryKey = i.PrimaryKey
                                    };
                                    foreach (Column c in i.Columns)
                                    {
                                        newIndex.Columns.Append(c.Name);
                                    }
                                    try
                                    {
                                        catMDB.Tables[tdName].Indexes.Append(newIndex);
                                    }
                                    catch (Exception ex)
                                    {
                                        //TODO: Find out this error
                                        ErrorLogger.Log(ex);
                                        // adding a new index to an existing table
                                        // always end in an COM error
                                    }
                                }
                            }
                        }
                        else
                        {
                            //append new tables that are not found in the template database
                            Table newTable = new Table();
                            newTable.Name = tdName;

                            //create columns
                            foreach (Column col in tblTemplate.Columns)
                            {
                                newTable.Columns.Append(col.Name, col.Type, col.DefinedSize);
                            }

                            //create indexes
                            foreach (Index i in tblTemplate.Indexes)
                            {
                                Index newIndex = new Index();
                                foreach (Column c in i.Columns)
                                {
                                    newIndex.Columns.Append(c.Name);
                                }
                                newIndex.Name = i.Name;
                                newIndex.PrimaryKey = i.PrimaryKey;
                                newTable.Indexes.Append(newIndex);
                            }

                            //finally append the new table
                            catMDB.Tables.Append(newTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, true);
                cancel = true;
            }
            //}
            //else
            //{
            //    FileExists = false;
            //    cancel = true;
            //}
            return !cancel;
        }

        public static Taxa TaxaFromCatchNameGUID(string CatchNameGUID)
        {
            Taxa taxa = Taxa.To_be_determined;
            var dt = new DataTable();
            using (var conection = new OleDbConnection(_ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $"Select TaxaNo from tblAllSpecies where SpeciesGUID = {{{CatchNameGUID}}}";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    DataRow dr = dt.Rows[0];
                    taxa = (Taxa)int.Parse(dr["TaxaNo"].ToString());
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
                return taxa;
            }
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
                        _munDict.Add(Convert.ToInt32(dr[0]), dr[1].ToString());
                    }
                }
                catch (Exception ex) { ErrorLogger.Log(ex); }
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
                if (CheckDB(value))
                {
                    _mdbPath = value;
                    _ConnectionString = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + _mdbPath;
                    names.MakeAllNames();
                    gear.MakeVesselTypeTable();
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
                catch (Exception ex) { ErrorLogger.Log(ex); }
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
                catch (Exception ex) { ErrorLogger.Log(ex); }
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
                ErrorLogger.Log(ex);
            }
        }

        public static bool MapIsOpen
        {
            get { return _MapIsOpen; }
            set { _MapIsOpen = value; }
        }
    }
}
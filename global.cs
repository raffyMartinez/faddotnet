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
        private static Dictionary<string, string> _GearClass = new Dictionary<string, string>();
        private static Dictionary<string, string> _GearVar = new Dictionary<string, string>();
        private static Dictionary<string, string> _GearClassLetterCodes = new Dictionary<string, string>();
        private static Dictionary<string, string> _GearVariationsUsage = new Dictionary<string, string>();
        private static string _GearClassUsed = "";
        private static string _mdbPath = "";
        private static string _ConnectionString = "";
        private static string _AppPath = "";
        private static bool _ShowErrorMessage = false;
        private static bool _HasMPH = false;
        private static bool _MapIsOpen;
        private static readonly string _ConnectionStringTemplate = "";
        private static long _RefNoRangeMin = 0;
        private static long _RefNoRangeMax = 0;
        

        static global()
        {
            TestMPH();
            _AppPath = Application.StartupPath.ToString();
            _ConnectionStringTemplate = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + _AppPath + "\\template.mdb";
            ReadRefNoRange();
        }

        /// <summary>
        /// Reads from the registry the range of reference numbers
        /// that is assigned to a computer
        /// </summary>
        public static void ReadRefNoRange()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\FAD3");
            string ReturnValue = rk.GetValue("RNRange", "NULL").ToString();
            if (ReturnValue.Length > 0 && ReturnValue != "NULL")
            {
                string[] arr = ReturnValue.Split('|');
                _RefNoRangeMin = long.Parse(arr[0]);
                _RefNoRangeMax = long.Parse(arr[1]);
            }
        }

        public static Dictionary<string, string> GearVariationsUsage(string GearClassGUID)
        {
            getGearVariationsUsage(GearClassGUID);
            return _GearVariationsUsage;
        }

        public static Dictionary<string, string> GearVariationsUsage(string GearClassGUID, string AOIGUID)
        {
            getGearVariationsUsage(GearClassGUID, AOIGUID);
            return _GearVariationsUsage;
        }

        public static Dictionary<string, string> VesselTypeDict
        {
            get { return _VesselTypeDict; }
        }

        public static List<string> TargetAreaUsed_RefCode(string RefCode)
        {
            var myList = new List<string>();
            var dt = new DataTable();
            var query = "";
            using (var conection = new OleDbConnection(_ConnectionString))
            {
                try
                {
                    conection.Open();
                    query = "SELECT AOIName FROM tblRefGearCodes_Usage INNER JOIN tblAOI ON " +
                        "tblRefGearCodes_Usage.TargetAreaGUID = tblAOI.AOIGuid WHERE " +
                        "tblRefGearCodes_Usage.RefGearCode= '" + RefCode + "' order by AOIName";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        myList.Add( dr["AOIName"].ToString());
                    }

                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }

            return myList;
        }

        public static Dictionary<string,bool>GearSubVariations(string GearVarGUID)
        {
            var myList = new Dictionary<string, bool>();
            var dt = new DataTable();
            var query = "";
            using (var conection = new OleDbConnection(_ConnectionString))
            {
                try
                {
                    conection.Open();
                    query = "SELECT RefGearCode, SubVariation FROM tblGearVariations " +
                            "LEFT JOIN tblRefGearCodes ON tblGearVariations.GearVarGUID = tblRefGearCodes.GearVar WHERE " +
                            "tblGearVariations.GearVarGUID = '{" + GearVarGUID + "}'";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        myList.Add(dr["RefGearCode"].ToString(), bool.Parse(dr["SubVariation"].ToString()));
                    }

                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }

            return myList;
        }

        static void getGearVariationsUsage(string GearClassGUID, string AOIGUID="")
        {
            _GearVariationsUsage.Clear();
            var dt = new DataTable();
            var query = "";
            using (var conection = new OleDbConnection(_ConnectionString))
            {
                try
                {
                    conection.Open();
                    if (AOIGUID.Length > 0)
                    {
                         query = "SELECT DISTINCT GearVarGUID, Variation FROM tblGearVariations INNER JOIN " +
                                       "(tblRefGearCodes INNER JOIN tblRefGearCodes_Usage ON tblRefGearCodes.RefGearCode = " +
                                       "tblRefGearCodes_Usage.RefGearCode) ON tblGearVariations.GearVarGUID = " +
                                       "tblRefGearCodes.GearVar WHERE tblRefGearCodes_Usage.TargetAreaGUID= {" + AOIGUID +
                                       "} AND tblGearVariations.GearClass={" + GearClassGUID + "} ORDER BY Variation";
                    }
                    else
                    {
                         query = "Select GearVarGUID, Variation from tblGearVariations where " +
                                 "GearClass = '{" + GearClassGUID + "}' ORDER BY Variation";
                    }
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        _GearVariationsUsage.Add(dr["GearVarGUID"].ToString(), dr["Variation"].ToString());
     
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }
        }

        public static string GearClassGUIDFromGearVarGUID(string GearVarGUID, bool SetAsGobalGearClass=false)
        {
            string gearClassGUID = "";
            var dt = new DataTable();
            using (var conection = new OleDbConnection(_ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = "SELECT GearClass FROM tblGearVariations WHERE GearVarGUID='{" + GearVarGUID + "}'";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    DataRow dr = dt.Rows[0];
                    gearClassGUID = dr["GearClass"].ToString();
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }

                if (SetAsGobalGearClass)
                    _GearClassUsed = gearClassGUID;

                return gearClassGUID;
            }
        }

        public static string GearClassFromGearVarGUID(string GearVarGUID)
         {
            string gearClass = "";
            var dt = new DataTable();
            using (var conection = new OleDbConnection(_ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = "SELECT GearClassName FROM tblGearClass INNER JOIN tblGearVariations ON " +
                                   "tblGearClass.GearClass = tblGearVariations.GearClass WHERE GearVarGUID='{" + GearVarGUID + "}'";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    DataRow dr = dt.Rows[0];
                    gearClass = dr["GearClassName"].ToString();
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
                return gearClass;
            }
        }

        public static string GearClassUsed
        {
            get { return _GearClassUsed; }
            set {
                _GearClassUsed = value;
                GetGearVariations();
            }
        }

        //set window position from registry
        public static void LoadFormSettings( Form frm, bool GetPosition=false )
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

        // Get one of a form's position value
        static object GetSetting(string formName,  string name, object default_value)
        {
            RegistryKey reg_key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\FAD3", true);
            RegistryKey sub_key = reg_key.CreateSubKey("FORMSTATE");
            RegistryKey sub_key1 = sub_key.CreateSubKey(formName);
            return sub_key1.GetValue(name, default_value);
        }

        //save window position to registry
        public static void SaveFormSettings( Form frm)
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

        //save a form position setting to the registry
         static void SaveSetting(string formName, string name, object value)
        {
            RegistryKey reg_key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\FAD3", true);
            RegistryKey sub_key = reg_key.CreateSubKey("FORMSTATE");
            RegistryKey sub_key1 = sub_key.CreateSubKey(formName);
            sub_key1.SetValue(name, value);
        }

        public static void GetRefNoRange(out long min, out long max)
        {
            min = _RefNoRangeMin;
            max = _RefNoRangeMax;

        }

        public static void SetRefNoRange(bool Reset=false, long min=0, long max=0)
        {
            RegistryKey rk = Registry.CurrentUser.CreateSubKey("SOFTWARE\\FAD3");

            if (Reset)
            {
                rk.DeleteValue("RNRange");
                _RefNoRangeMin = _RefNoRangeMax = 0;
            }
            else
                rk.SetValue("RNRange", min.ToString() + "|" + max.ToString(), RegistryValueKind.String);

            rk.Close();
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

		public static bool ShowErrorMessage{
			get{return _ShowErrorMessage;}
			set{_ShowErrorMessage = value;}
		}



		public static Dictionary<FishCrabGMS,string> GMSStages(Taxa taxa, ref bool Success)
		{
			Success = false;
			Dictionary<FishCrabGMS, string> myStages = new Dictionary<FishCrabGMS, string>();
			switch (taxa)
			{
				case Taxa.Fish:
					myStages.Add(FishCrabGMS.AllTaxaNotDetermined,"Not determined");
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

		public static string ClassCode(string ClassName){
			string myCode="";
			foreach(KeyValuePair<string,string>kv in _GearClassLetterCodes){
				if(kv.Key ==ClassName){
					myCode=kv.Value;
					break;	
				}
			}
			return myCode;
		}

		public static string AppPath
		{
			get {return _AppPath;}
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
		public static Dictionary<string,string>GearClassLetterCodes{
			get{return _GearClassLetterCodes;}
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
                    if (tdName.Substring(0,4) !="MSys" && tdName.Substring(0,5) != "temp_")
                    {
                        if (tableList.Contains(tdName))
                        {
                            //check if data table fields and template table fields are the same

                            //check for columns
                            foreach(Column col in catMDB.Tables[tdName].Columns)
                            {
                                colList.Add(col.Name);
                            }

                            foreach(Column col in tblTemplate.Columns)
                            {
                                if (!colList.Contains(col.Name))
                                {
                                    catMDB.Tables[tdName].Columns.Append(col.Name, col.Type, col.DefinedSize);
                                }
                            }

                            //check for indexes
                            colList.Clear();

                            foreach(Index i in catMDB.Tables[tdName].Indexes)
                            {
                                colList.Add(i.Name);
                            }

                            foreach(Index i in tblTemplate.Indexes)
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
                                        catMDB.Tables[tdName].Indexes.Append((object)newIndex);
                                    }
                                    catch(Exception ex)
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
					string query = "Select TaxaNo from tblAllSpecies where SpeciesGUID = '{" + CatchNameGUID + "}'";
					var adapter = new OleDbDataAdapter(query, conection);
					adapter.Fill(dt);
					_GearVar.Clear();
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

		//public static Dictionary<string, string> GearClassFromGearVar(string varGUID)
        public static KeyValuePair<string, string> GearClassFromGearVar(string varGUID)
        {
            KeyValuePair<string, string> myClass = new KeyValuePair<string, string>();
			var dt = new DataTable();
			using (var conection = new OleDbConnection(_ConnectionString))
			{
				try
				{
					conection.Open();
					string query = "SELECT tblGearClass.GearClass, tblGearClass.GearClassName FROM tblGearClass " +
								   "INNER JOIN tblGearVariations ON tblGearClass.GearClass = " +
								   "tblGearVariations.GearClass WHERE tblGearVariations.GearVarGUID='{" + varGUID + "}'";
					var adapter = new OleDbDataAdapter(query, conection);
					adapter.Fill(dt);
					_GearVar.Clear();
					DataRow dr = dt.Rows[0];
                    //myClass.Add("ClassGUID", dr["KeyValuePair"].ToString());
                    //myClass.Add("ClassName",dr["GearClassName"].ToString());
                    myClass = new KeyValuePair<string, string>(dr["GearClass"].ToString(), dr["GearClassName"].ToString());
				}
				catch (Exception ex)
				{
					ErrorLogger.Log(ex);
				}
			}

			return myClass;
		}
		private static void GetGearClass(){
			var myDT =  new DataTable();
			using(var conection = new OleDbConnection(_ConnectionString))
			{
				try{
					conection.Open();
					string query = "Select GearClass, GearLetter, GearClassName from tblGearClass order by GearClassName";
					var adapter = new OleDbDataAdapter(query, conection);
					adapter.Fill(myDT);
					for (int i = 0; i < myDT.Rows.Count; i++){
						DataRow dr = myDT.Rows[i];
						_GearClass.Add(dr["GearClass"].ToString(), dr["GearClassName"].ToString());
						_GearClassLetterCodes.Add(dr["GearClassName"].ToString(), dr["GearLetter"].ToString());
					}					
				}
				catch (Exception ex){
					ErrorLogger.Log(ex);
				}
			}
		}
		
		 static void GetGearVariations(){
            _GearVar.Clear();
            var dt = new DataTable();
			using(var conection = new OleDbConnection(_ConnectionString))
			{
				try{
						conection.Open();
						string query = "SELECT GearVarGUID, Variation FROM tblGearVariations WHERE GearClass=\"" + _GearClassUsed + "\" order by Variation";
						var adapter = new OleDbDataAdapter(query,conection);
						adapter.Fill(dt);
						_GearVar.Clear();
						for(int i=0; i< dt.Rows.Count;i++){
							DataRow dr = dt.Rows[i];
							_GearVar.Add(dr["GearVarGUID"].ToString(), dr["Variation"].ToString());
						}
				}
				catch (Exception ex){
					ErrorLogger.Log(ex);
				}
			}
		}
		
		public static void ProvinceNo(long ProvinceNo)
		{
			var myDT =  new DataTable();
			using(var conection = new OleDbConnection(_ConnectionString))
			{
				try{
					conection.Open();
					string query = "Select MunNo, Municipality from Municipalities where ProvNo= " + ProvinceNo + " Order By Municipality";
					var adapter = new OleDbDataAdapter(query, conection);
					adapter.Fill(myDT);	
					_munDict.Clear();
					for(int i=0; i< myDT.Rows.Count;i++)
					{
						DataRow dr = myDT.Rows[i];
						_munDict.Add(Convert.ToInt32(dr[0]),dr[1].ToString());
					}
				}
				catch(Exception ex){ErrorLogger.Log(ex);}
			}			
		}
		
		public static Dictionary<string, string> GearVariations{
			get {return _GearVar;}
		}
		
		public static Dictionary <string, string> GearClass
		{
			get {return _GearClass;}
		} 
		
		public static Dictionary<long, string> munDict
		{
			get {return _munDict;}
		}

		public static Dictionary<long, string> provinceDict
		{
			get {return _provinceDict;}
		}
		
		public static string mdbPath
		{
			get {return _mdbPath;}
			set
			{
                if (CheckDB(value))
                {
                    _mdbPath = value;
                    _ConnectionString = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + _mdbPath;
                    names.MakeAllNames();
                    gear.MakeVesselTypeTable();
                    GetProvinces();
                    GetGearClass();
                    GetVesselTypes();
                }
			}
		}

        private static void GetVesselTypes()
        {
            var myDT = new DataTable();
            using (var conection = new OleDbConnection(_ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = "Select VesselTypeNo, VesselType from temp_VesselType";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(myDT);
                    for (int i = 0; i < myDT.Rows.Count; i++)
                    {
                        DataRow dr = myDT.Rows[i];
                        _VesselTypeDict.Add(dr[0].ToString(), dr[1].ToString());
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
		


		private static void  GetProvinces()
		{
			var myDT =  new DataTable();
			using(var conection = new OleDbConnection(_ConnectionString))
			{
				try{
					conection.Open();
					string query = "Select ProvNo, ProvinceName from Provinces Order By ProvinceName";
					var adapter = new OleDbDataAdapter(query, conection);
					adapter.Fill(myDT);	
					for(int i=0; i< myDT.Rows.Count;i++)
					{
						DataRow dr = myDT.Rows[i];
						_provinceDict.Add(Convert.ToInt32(dr[0]),dr[1].ToString());
					}
				}
				catch(Exception ex){ErrorLogger.Log(ex);}
			}
		}

		public static void Export2Excel(ListView lv,string xlFileName)
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

					for (int col = 0; col < lv.Columns.Count; col++) {
						if (col==0)
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
            get {return _MapIsOpen; }
            set { _MapIsOpen = value; }
        }
    }
}

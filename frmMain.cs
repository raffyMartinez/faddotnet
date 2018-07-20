/*
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/7/2016
 * Time: 1:53 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

using System.Data;
using System.Data.OleDb;
using Microsoft.Win32;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Collections;

namespace FAD3
{
	/// <summary>
	/// Description of frmMain.
	/// </summary>
	public partial class frmMain : Form
	{
		private frmCatch _frmCatch;
		private frmLenFreq _frmLF;
		private string _oldMDB = "";
		private int _statusPanelWidth = 200;
		private sampling _Sampling = new sampling();
		private aoi _AOI = new aoi();
		private landingsite _ls = new landingsite();
		private string _TreeLevel = "";
		private mru _mrulist = new mru();
		private TreeNode _LSNode;

        private string _LandingSiteGuid = "";
        private string _LandingSiteName = "";
        private string _GearVarGUID = "";
        private string _GearVarName = "";
        private string _AOIGuid = "";
        private string _AOIName = "";
        private string _GearClassGUID = "";
        private string _GearClassName = "";
        private string _SamplingMonth = "";
        private string _SamplingGUID = "";
        private int _topLVItemIndex = 0;

        private string _VesLength = "";
        private string _VesWidth = "";
        private string _VesHeight = "";


        public string AOIGUID
        {
            get { return _AOIGuid; }
            set { _AOIGuid = value;  }
        }

		public mru MRUList
		{
			get { return _mrulist; }
		}

		public sampling Sampling
		{
			get { return _Sampling; }
		}

		public aoi AOI
		{
			get { return _AOI; }
		}




		public void SamplingUpdate(bool IsNew, string AOIGuid, string LSGUID, string GearName, string GearGUID, string SamplingDate)
		{
			TreeNode ls = _LSNode.Parent.Nodes[LSGUID];
			TreeNode nd_gear;
			TreeNode nd_samplingDate;
			DateTime dt = DateTime.Parse(SamplingDate);
			string myDate = string.Format("{0:MMM-yyyy}", dt);
			try
			{
				if (true)
				{
					nd_gear = ls.Nodes[LSGUID + "|" + GearGUID];
					if (nd_gear == null)
					{
						int i = 0;
						Dictionary<string, string> gears = _Sampling.GearsFromLandingSite(LSGUID);
						ls.Nodes.Clear();
						foreach (KeyValuePair<string,string>kv in gears)
						{
							nd_gear = ls.Nodes.Add(LSGUID + "|" + kv.Key, kv.Value );
							nd_gear.Tag = LSGUID + "|" + kv.Key + ",gears";
							nd_gear.Nodes.Add("**dummy*");
							i++;
						}

						nd_gear = ls.Nodes[LSGUID + "|" + GearGUID];
						nd_samplingDate = nd_gear.FirstNode;
						nd_samplingDate.Text = myDate;
						nd_samplingDate.Name = LSGUID + "|" + GearGUID + "|" + myDate;
						nd_samplingDate.Tag = LSGUID + "|" + GearGUID + ",sampling";
					}
					else
					{
						nd_samplingDate = nd_gear.Nodes[LSGUID + "|" + GearGUID + "|" + myDate];
						if (nd_samplingDate == null)
						{
							nd_samplingDate = nd_gear.Nodes.Add(LSGUID + "|" + GearGUID + "|" + myDate, myDate);
							nd_samplingDate.Tag = LSGUID + "|" + GearGUID + ",sampling";
						}
					}
					FillLVSamplingSummary(LSGUID, GearGUID, myDate);
					treeView1.SelectedNode = nd_samplingDate;

					if (IsNew==false)
					{
						GetSamplingDetailEx1();
					}
				}
				else
				{
					//FillLVSamplingSummary(LSGUID, GearGUID, myDate);
					//GetSamplingDetailEx1();
				}
			}
			catch (Exception ex)
			{
				ErrorLogger.Log(ex);
			}
			
			
		}


		public frmMain()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}


        private void OnUIRowRead(object sender, UIRowFromXML e)
        {
            ListViewItem lvi = listView1.Items.Add(e.RowLabel);
            lvi.Name = e.Key;
            lvi.SubItems.Add("");
        }



        void FrmMainLoad(object sender, EventArgs e)
		{
			this.splitContainer1.Panel1MinSize = 200;
			this.splitContainer1.Panel2MinSize = this.Width - (this.splitContainer1.Panel1MinSize + 100);
			this.splitContainer1.SplitterWidth = 3;
			

            sampling.SetUpUIElement();
            _Sampling.OnUIRowRead += new sampling.ReadUIElement(OnUIRowRead);

			toolStripRecentlyOpened.DropDownItems.Clear();
			_mrulist = new mru("FAD3", toolStripRecentlyOpened,5);
			_mrulist.FileSelected += _mrulist_FileSelected;
			RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\FAD3");
			string ReturnValue = "";

			try
			{
				ReturnValue = rk.GetValue("mdbPath", "NULL").ToString();
				if (TestFileExist(ReturnValue))
				{
					PopulateTree();
				}
			}
			catch
			{
				ErrorLogger.Log("Registry entry for mdb path not found");
				lblErrorFormOpen.Visible = true;
				lblTitle.Text = "";
				lblErrorFormOpen.Text = "Please locate the database file where fisheries data is saved." +
										 System.Environment.NewLine + "You can use the file open menu";
			}

            statusPanelTargetArea.Width = _statusPanelWidth;
            statusPanelLandingSite.Width = _statusPanelWidth;
            statusPanelGearUsed.Width = _statusPanelWidth;

            ConfigDropDownMenu(treeView1);
            SetupSamplingButtonFrame(false);

        }

        private void menuDropDown_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "menuNewTargetArea":
                    break;
                case "menuNewLandingSite":
                    break;
                case "menuNewSampling":
                    NewSamplingForm();
                    break;
                case "menuNewEnumerator":
                    break;
                case "menuSamplingDetail":
                    ShowSamplingDetailForm();
                    break;
            }
        }

        private void ConfigDropDownMenu(Control Source, ListViewHitTestInfo lvh)
        {
            menuDropDown.Items.Clear();
            if (lvh.Item != null)
            {
                //Note: commented out because I can't make this work for now
                
                //var tsi1 = menuDropDown.Items.Add("Detail of selected sampling");
                //tsi1.Name = "menuSamplingDetail";
                //_SamplingGUID = lvh.Item.Name;

                //menuDropDown.Items.Add("-");
            }

            var tsi = menuDropDown.Items.Add("New sampling");
            tsi.Name = "menuNewSampling";
 

        }

        private void ConfigDropDownMenu(Control Source)
        {
            menuDropDown.Items.Clear();
            switch (Source.Name)
            {
                case "treeView1":
                    var tsi = menuDropDown.Items.Add("New target area");
                    tsi.Name = "menuNewTargetArea";
                    tsi.Enabled = _TreeLevel == "root";

                    tsi = menuDropDown.Items.Add("New landing site");
                    tsi.Name = "menuNewLandingSite";
                    tsi.Enabled = _TreeLevel == "aoi";

                    tsi = menuDropDown.Items.Add("New sampling");
                    tsi.Name = "menuNewSampling";
                    tsi.Enabled = _TreeLevel == "sampling" || _TreeLevel=="landing_site" || _TreeLevel =="gear" ;

                    var sep = menuDropDown.Items.Add("-");
                    sep.Name = "menuSeparator1";

                    tsi = menuDropDown.Items.Add("Enumerators");
                    tsi.Name = "menuEnumerators";
                    tsi.Enabled = _TreeLevel == "aoi";

                    tsi = menuDropDown.Items.Add("Landing site properties");
                    tsi.Name = "menuLandingSiteProp";
                    tsi.Enabled = _TreeLevel == "landing_site";

                    break;
                case "listView1":
                    tsi = menuDropDown.Items.Add("New sampling");
                    tsi.Name = "menuNewSampling";
                    tsi.Visible = _TreeLevel == "sampling" || _TreeLevel == "landing_site" || _TreeLevel == "gear";


                    sep = menuDropDown.Items.Add("-");
                    sep.Name = "menuSeparator1";
                    sep.Visible = _TreeLevel == "landing_site";

                    tsi = menuDropDown.Items.Add("Landing site properties");
                    tsi.Name = "menuLandingSiteProp";
                    tsi.Visible = _TreeLevel == "landing_site";

                    tsi = menuDropDown.Items.Add("Target area properties");
                    tsi.Name = "menuTargetAreaProperties";
                    tsi.Visible = _TreeLevel == "aoi";
                    break;
            }


        }

		private bool TestFileExist(string filename)
		{
			bool exists = File.Exists(filename);

			if (exists)
			{
				if (filename != "NULL" && filename != "")
				{
					_oldMDB = filename;
					global.mdbPath = filename;
					names.GetGenus_LocalNames();
					names.GetLocalNames();
					//_StatusStripLabelWidth = statusPanelDBPath.Width;
					statusPanelDBPath.Text = filename;
					lblErrorFormOpen.Visible = false;
					//PopulateTree();
				}
			}
			else
			{
				lblTitle.Text = "File not found";
				MessageBox.Show("Last opened database file" + Environment.NewLine +
								 filename + Environment.NewLine +
								 "is not found. Maybe it was deleted, moved, or renamed");

			}

			return exists;
		}
		public void NewDBFile(string filename)
		{
			if (TestFileExist(filename)) {
				global.mdbPath = filename;
				RegistryKey rk = Registry.CurrentUser.CreateSubKey("SOFTWARE\\FAD3");
				rk.SetValue("mdbPath", filename, RegistryValueKind.String);
				rk.Close();
				lblErrorFormOpen.Visible = false;
				PopulateTree();
				statusPanelDBPath.Text = filename;
			}
			else
			{

			}
		}

		private void _mrulist_FileSelected(string filename)
		{
			if (TestFileExist(filename))
			{
				global.mdbPath = filename;
				RegistryKey rk = Registry.CurrentUser.CreateSubKey("SOFTWARE\\FAD3");
				rk.SetValue("mdbPath", filename, RegistryValueKind.String);
				rk.Close();
				lblErrorFormOpen.Visible = false;

				PopulateTree();
				statusPanelDBPath.Text = filename;
			}
			else
			{
				_mrulist.RemoveFile(filename);
			}
		}

		void PopulateTree()
		{
			var myDataTable = new DataTable();
			try
			{
				using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;data source=" + global.mdbPath))
				{
					conection.Open();

					string query = "SELECT tblAOI.AOIGuid, tblAOI.AOIName, tblLandingSites.LSGUID, tblLandingSites.LSName " +
								   "FROM tblAOI LEFT JOIN tblLandingSites ON tblAOI.AOIGuid = tblLandingSites.AOIGuid " +
									"ORDER BY tblAOI.AOIName, tblLandingSites.LSName";

					var adapter = new OleDbDataAdapter(query, conection);
					adapter.Fill(myDataTable);
				}

			}
			catch (Exception ex)
			{
				ErrorLogger.Log(ex);
			}

			this.treeView1.Nodes.Clear();
			//this.treeView1.
			TreeNode root = this.treeView1.Nodes.Add("Fisheries Data");
			root.Name = "root";
			root.Tag = ",root";
            root.ImageKey = "db";
			for (int i = 0; i < myDataTable.Rows.Count; i++)
			{
				DataRow dr = myDataTable.Rows[i];
                //bool Exists = root.Nodes.ContainsKey(dr["AOIName"].ToString());
                bool Exists = root.Nodes.ContainsKey(dr["AOIGuid"].ToString());
                if (Exists)
				{
                    //Debug.WriteLine(dr["AOIName"]);
                    //TreeNode myNode = root.Nodes[dr["AOIName"].ToString()];
                    TreeNode myNode = root.Nodes[dr["AOIGuid"].ToString()];
                    TreeNode myChild = new TreeNode(dr["LSName"].ToString());
					myNode.Nodes.Add(myChild);
					myChild.Name = dr["LSGUID"].ToString();
					myChild.Nodes.Add("*dummy*");
					myChild.Tag = dr["LSGUID"].ToString() + ",landing_site";
                    myChild.ImageKey = "LandingSite";

				}
				else
				{
					TreeNode myNode = new TreeNode(dr["AOIName"].ToString());
                    //myNode.Name = dr["AOIName"].ToString();
                    myNode.Name = dr["AOIGuid"].ToString();
                    root.Nodes.Add(myNode);
					myNode.Tag = dr["AOIGuid"].ToString() + ",aoi";
                    myNode.ImageKey = "AOI";

					if (string.IsNullOrWhiteSpace(dr["LSName"].ToString()))
					{
						myNode.Nodes.Add("*dummy*");
            
					}
					else
					{
						TreeNode myChild = new TreeNode(dr["LSName"].ToString());
						myNode.Nodes.Add(myChild);
						myChild.Nodes.Add("*dummy*");
						myChild.Tag = dr["LSGUID"].ToString() + ",landing_site";
						myChild.Name = dr["LSGUID"].ToString();
                        myChild.ImageKey = "LandingSite";
                    }
				}
			}
			root.Expand();
			SetUPLV("root");
		}

		public void RefreshLV(string NodeName, string TreeLevel, bool IsNew = false, string nodeGUID = "")
		{

			if (IsNew)
			{
				TreeNode NewNode = new TreeNode();
				NewNode.Text = NodeName;
				NewNode.Tag = nodeGUID + "," + TreeLevel;
				NewNode.Name = NodeName;
				treeView1.SelectedNode.Nodes.Add(NewNode);
				treeView1.SelectedNode = NewNode;
			}
			else
			{
				treeView1.SelectedNode.Text = NodeName;
			}
			SetUPLV(TreeLevel);
		}

		public void RefreshLVEx(string TreeLevel)
		{
			SetUPLV(TreeLevel);
		}

		private void SetUPLV(string TreeLevel)
		{
			// this will show a list of summaries depending on the level of the node selected
			//level could be root, AOI, Landing site, gear used, and sampling month
			int i = 0;

			this.listView1.Clear();
			this.listView1.View = View.Details;
			this.listView1.FullRowSelect = true;
			string myData = "";
			string tag = "";
			tag = TreeLevel;
			switch (TreeLevel)
			{
				case "root":
					listView1.Columns.Add("Property");
					listView1.Columns.Add("Value");
					ListViewItem lvi = listView1.Items.Add("Database path");
					lvi.SubItems.Add(global.mdbPath);

					//add sampled years with count for entire database
					lvi = listView1.Items.Add("");
					lvi = listView1.Items.Add("Sampled years");
					foreach (string item in _AOI.SampledYears())
					{
						if (i > 0)
						{
							lvi = listView1.Items.Add("");
						}
						lvi.SubItems.Add(item.ToString());
						i++;
					}

					//add AOI with count for entire database
					lvi = listView1.Items.Add("");
					lvi = listView1.Items.Add("AOI");
					i = 0;
					foreach (KeyValuePair<string, string> kv in _AOI.AOIWithSamplingCount())
					{
						if (i > 0)
						{
							lvi = listView1.Items.Add("");
						}
						lvi.SubItems.Add(kv.Value);
						i++;
					}

					tag = "database";
					lblTitle.Text = "Database summary";
					break;
				case "aoi":
					listView1.Columns.Add("Property");
					listView1.Columns.Add("Value");
					string[] arr = treeView1.SelectedNode.Tag.ToString().Split(',');
					_AOI.AOIGUID = arr[0].ToString();
					myData = _AOI.AOIData();
					arr = myData.Split('|');

					//add AOI data for form
					lvi = listView1.Items.Add("Name");
					lvi.SubItems.Add(arr[0].ToString());
					lvi.Tag = "aoi_data";
					lvi = listView1.Items.Add("Letter");
					lvi.SubItems.Add(arr[1].ToString());
					lvi.Tag = "aoi_data";
					lvi = listView1.Items.Add("Major grids");
					lvi.SubItems.Add(arr[2].ToString());
					lvi.Tag = "aoi_data";

					// add no. of samplings for this AOI
					lvi = listView1.Items.Add("");
					lvi = listView1.Items.Add("No. of samplings");
					lvi.SubItems.Add(_AOI.SampleCount().ToString());

					//add sampled years with count
					lvi = listView1.Items.Add("Years sampling");
					i = 0;
					foreach (KeyValuePair<string, string> item in _AOI.ListYearsWithSamplingCount())
					{
						if (i > 0)
						{
							lvi = listView1.Items.Add("");
						}
						lvi.Name = "YearSampled";
						lvi.SubItems.Add(item.Key + " : " + item.Value);
						i++;
					}

					//add enumerators with count
					lvi = listView1.Items.Add("");
					lvi = listView1.Items.Add("Enumerators");
					i = 0;
					foreach (KeyValuePair<string, string> item in _AOI.EnumeratorsWithCount())
					{
						if (i > 0)
						{
							lvi = listView1.Items.Add("");
						}
						lvi.Name = "Enumerators";
						arr = item.Value.ToString().Split(',');
						lvi.SubItems.Add(item.Key + ": " + arr[1].ToString());
						lvi.Tag = arr[0].ToString();
						i++;
					}

					// if there is no sampling then we cannot show enumerators with count
					// so we just show a simple list
					if (i==0)
					{
						foreach (KeyValuePair<string,string>item in aoi.AOIEnumeratorsList(_AOIGuid))
						{
							if (i > 0)
							{
								lvi = listView1.Items.Add("");
							}
							lvi.Name = "Enumerators";
							lvi.SubItems.Add(item.Value);
							lvi.Tag = item.Key;
							i++;
						}
					}

					//add landing sites
					listView1.Items.Add("");
					lvi = listView1.Items.Add("Landing sites");
					i = 0;
					foreach (string item in _AOI.ListLandingSiteWithSamplingCount())
					{
						if (i > 0)
						{
							lvi = listView1.Items.Add("");
						}
						lvi.SubItems.Add(item);
						i++;
					}

					lblTitle.Text = "Area of interest";
					break;

				case "landing_site":
					listView1.Columns.Add("Property");
					listView1.Columns.Add("Value");

					arr = treeView1.SelectedNode.Tag.ToString().Split(',');

					//add landing site form data
					_ls.LandingSiteGUID = arr[0];
					myData = _ls.LandingSiteData();
					arr = myData.Split(',');
					lvi = listView1.Items.Add("Name");
					lvi.SubItems.Add(arr[0]);
					lvi = listView1.Items.Add("Municipality");
					lvi.SubItems.Add(arr[1]);
					lvi = listView1.Items.Add("Province");
					lvi.SubItems.Add(arr[2]);
					lvi = listView1.Items.Add("Longitude");
					lvi.SubItems.Add(arr[3]);
					lvi = listView1.Items.Add("Latitude");
					lvi.SubItems.Add(arr[4]);

					//add total number of sampling
					listView1.Items.Add("");
					lvi = listView1.Items.Add("No. of samplings");
					lvi.SubItems.Add(_ls.SampleCount().ToString());
					listView1.Items.Add("");

					//add gears sampled
					Dictionary<string, string> myGears = new Dictionary<string, string>();
					myGears = _ls.Gears();
					lvi = listView1.Items.Add("Gears sampled");
					long n = 0;
					foreach (KeyValuePair<string, string> item in myGears)
					{
						if (n > 0)
						{
							lvi = listView1.Items.Add("");
						}
						lvi.Tag = item.Key;
						lvi.SubItems.Add(item.Value);
						n++;
					}

					//Add months sampled
					listView1.Items.Add("");
					lvi = listView1.Items.Add("Months sampled");
					List<string> myMonths = _ls.MonthsSampled();
					n = 0;
					foreach (string item in myMonths)
					{
						if (n > 0)
						{
							lvi = listView1.Items.Add("");
						}
						lvi.SubItems.Add(item);
						n++;
					}

					lblTitle.Text = "Landing site";
					break;

				case "gear":
					listView1.Columns.Add("Property");
					listView1.Columns.Add("Value");
					lvi = listView1.Items.Add("Months sampled");
					arr = treeView1.SelectedNode.Tag.ToString().Split(',');
					string[] arr2 = arr[0].Split('|');
					string lsguid = arr2[0];
					string gearguid = arr2[1];
					_ls.LandingSiteGUID = lsguid;
					_ls.GearVarGUID = gearguid;
					n = 0;
					foreach (string item in _ls.MonthsSampledEx(gearguid))
					{
						if (n > 0)
						{
							lvi = listView1.Items.Add("");
						}
						lvi.SubItems.Add(item);
						n++;
					}
					lblTitle.Text = "Fishing gear";
					break;

				case "sampling":
					this.listView1.Columns.Add("Reference #");
					this.listView1.Columns.Add("Area of interest");
					this.listView1.Columns.Add("Landing site");
					this.listView1.Columns.Add("Fishing ground");
					this.listView1.Columns.Add("Gear used");
					this.listView1.Columns.Add("Sampling date");
					this.listView1.Columns.Add("Weight of catch");
					this.listView1.Columns.Add("Enumerator");
					this.listView1.Columns.Add("Catch rows");
					this.listView1.Columns.Add("Notes");
					lblTitle.Text = "Sampling";
					break;
                case "samplingDetail":
                    listView1.Columns.Add("Property");
                    listView1.Columns.Add("Value");
                    lblTitle.Text = "Sampling detail";
                    tag = "samplingDetail";
                    break;

			}

			//apply column widths  saved in registry
			listView1.Tag = tag;
			try
			{
				RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\FAD3\\ColWidth");
				string rv = rk.GetValue(tag, "NULL").ToString();
				string[] arr = rv.Split(',');
				i = 0;
				foreach (var item in listView1.Columns)
				{
					ColumnHeader ch = (ColumnHeader)item;
					ch.Width = Convert.ToInt32(arr[i]);
					i++;
				}
			}
			catch (Exception ex)
			{
				ErrorLogger.Log(ex);
			}

		}


		string DatabaseSummary(string SummaryTopic)
		{
			return "x";
		}



		void SetupLV2()
		{

			//apply the saved column widths		
			RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\FAD3\\ColWidth");
			try
			{
				string rv = rk.GetValue(global.lvContext.CatchAndEffort.ToString(), "NULL").ToString();
				string[] arr = rv.Split(',');
				int i = 0;
			}
			catch
			{
				ErrorLogger.Log("Catch and effort column width not found in registry");
			}
		}

		void SetupLV3()
		{

			//apply the saved column widths	
			RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\FAD3\\ColWidth");
			try
			{
				string rv = rk.GetValue(global.lvContext.CatchComposition.ToString(), "NULL").ToString();
				string[] arr = rv.Split(',');
				int i = 0;
			}
			catch
			{
				ErrorLogger.Log("Catch composition column widths not found in registry");
			}
		}

		void FillLVSamplingSummary(string LSGUID, string GearGUID, string SamplingMonth)
		{
            listView1.SuspendLayout();
            this.listView1.Items.Clear();
			string[] arr = SamplingMonth.Split('-');
			string MonthNumber = "1";
			switch (arr[0])
			{
				case "Jan":
					MonthNumber = "1";
					break;
				case "Feb":
					MonthNumber = "2";
					break;
				case "Mar":
					MonthNumber = "3";
					break;
				case "Apr":
					MonthNumber = "4";
					break;
				case "May":
					MonthNumber = "5";
					break;
				case "Jun":
					MonthNumber = "6";
					break;
				case "Jul":
					MonthNumber = "7";
					break;
				case "Aug":
					MonthNumber = "8";
					break;
				case "Sep":
					MonthNumber = "9";
					break;
				case "Oct":
					MonthNumber = "10";
					break;
				case "Nov":
					MonthNumber = "11";
					break;
				case "Dec":
					MonthNumber = "12";
					break;
			}

			string StartDate = MonthNumber + "/1/" + arr[1];
			string EndDate = (Convert.ToInt32(MonthNumber) + 1).ToString();
			if (arr[0] == "Dec")
			{
				string newYear = (Convert.ToInt32(arr[1]) + 1).ToString();
				EndDate = "1/1/" + newYear;
			}
			else
			{
				EndDate += "/1/" + arr[1];
			}


			var myDT = new DataTable();
			try
			{
				using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;data source=" + global.mdbPath))
				{
					conection.Open();
					/*string query = "SELECT tblAOI.AOIName, tblLandingSites.LSName, tblGearVariations.Variation, tblSampling.* " +
								   "FROM tblGearVariations INNER JOIN (tblAOI INNER JOIN (tblLandingSites INNER JOIN tblSampling " +
								   "ON tblLandingSites.LSGUID = tblSampling.LSGUID) ON tblAOI.AOIGuid = tblLandingSites.AOIGuid) " +
								   "ON tblGearVariations.GearVarGUID = tblSampling.GearVarGUID " +
								   "WHERE tblSampling.GearVarGUID=\"" + GearGUID + "\" AND tblSampling.LSGUID= \"" + LSGUID + "\" AND tblSampling.SamplingDate >=#" +
									StartDate + "# AND tblSampling.SamplingDate <#" + EndDate + "# Order by tblSampling.SamplingDate";   
				   */

					string query = "SELECT tblSampling.RefNo, tblAOI.AOIName, tblLandingSites.LSName, tblGearVariations.Variation, tblSampling.SamplingDate, tblSampling.WtCatch, tblSampling.FishingGround, tblSampling.NoFishers, tblSampling.VesType, Count(tblCatchComp.RowGUID) AS [Rows], tblSampling.SamplingGUID, tblEnumerators.EnumeratorName, tblSampling.Notes " +
									"FROM (tblAOI RIGHT JOIN tblLandingSites ON tblAOI.AOIGuid = tblLandingSites.AOIGuid) RIGHT JOIN (tblGearVariations RIGHT JOIN " +
									   "(tblEnumerators RIGHT JOIN (tblSampling LEFT JOIN tblCatchComp ON tblSampling.SamplingGUID = tblCatchComp.SamplingGUID) ON " +
									   "tblEnumerators.EnumeratorID = tblSampling.Enumerator) ON tblGearVariations.GearVarGUID = tblSampling.GearVarGUID) ON tblLandingSites.LSGUID = " +
									   "tblSampling.LSGUID GROUP BY tblSampling.RefNo, tblAOI.AOIName, tblLandingSites.LSName, tblGearVariations.Variation, " +
									   "tblSampling.SamplingDate, tblSampling.WtCatch, tblSampling.FishingGround, tblSampling.NoFishers, tblSampling.VesType, " +
									"tblSampling.SamplingGUID, tblEnumerators.EnumeratorName, tblSampling.Notes, tblSampling.SamplingDate, tblSampling.GearVarGUID, tblSampling.LSGUID " +
								   "HAVING (tblSampling.SamplingDate >=#" + StartDate + "# And tblSampling.SamplingDate <#" + EndDate + "# )AND " +
									  "tblSampling.GearVarGUID=\"" + GearGUID + "\" AND tblSampling.LSGUID=\"" + LSGUID + "\" " +
								   "ORDER BY tblSampling.SamplingDate;";
					var adapter = new OleDbDataAdapter(query, conection);
					adapter.Fill(myDT);
					ListViewItem lvi;
					for (int i = 0; i < myDT.Rows.Count; i++)
					{
						DataRow dr = myDT.Rows[i];
						ListViewItem row = new ListViewItem(dr[0].ToString());
						row.SubItems.Add(dr[1].ToString());
						row.SubItems.Add(dr[2].ToString());
						row.SubItems.Add(dr[6].ToString());
						row.SubItems.Add(dr[3].ToString());
						DateTime dt = (DateTime)dr[4];
						row.SubItems.Add(string.Format("{0:MMM-dd-yyyy}", dt));
						row.SubItems.Add(dr[5].ToString());
						row.SubItems.Add(dr[11].ToString());
						row.SubItems.Add(dr[9].ToString());
						row.SubItems.Add(dr[12].ToString());
						//string.Format( "{0:MMM-dd-YYYY}",  row.SubItems.Add (dr[7].ToString())   );
						lvi = this.listView1.Items.Add(row);
						lvi.Tag = dr[10].ToString();
						lvi.Name = dr[10].ToString();
					}

				}
			}
			catch (Exception ex)
			{
				ErrorLogger.Log(ex);
			}

            listView1.ResumeLayout();
		}


		void TreeView1AfterExpand(object sender, TreeViewEventArgs e)
		{

			if (e.Node.FirstNode.Text == "*dummy*")
			{
				// we are in a landing site node then we will add gear used nodes

				//node tag contains guid of landing site
				string[] arr = e.Node.Tag.ToString().Split(',');
				var myDT = new DataTable();
				try
				{
					using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;data source=" + global.mdbPath))
					{
						conection.Open();
						// arr[0] contains the landing site guid
						string lsguid = arr[0];

                        /*
                          var query = "SELECT DISTINCT tblGearVariations.Variation, tblSampling.LSGUID, tblGearVariations.GearVarGUID " +
									 "FROM tblGearVariations INNER JOIN tblSampling ON tblGearVariations.GearVarGUID = tblSampling.GearVarGUID " +
									 "WHERE tblSampling.LSGUID = '{" + lsguid + "}'";
                        */

                        var query = "SELECT DISTINCT tblGearClass.GearClassName, tblGearVariations.Variation, tblSampling.LSGUID, " +
                                     "tblGearVariations.GearVarGUID FROM tblGearClass INNER JOIN " +
                                     "(tblGearVariations INNER JOIN tblSampling ON tblGearVariations.GearVarGUID = tblSampling.GearVarGUID) " +
                                     "ON tblGearClass.GearClass = tblGearVariations.GearClass " +
                                     "WHERE tblSampling.LSGUID = '{" + lsguid + "}'" +
                                     "ORDER BY tblGearClass.GearClassName, tblGearVariations.Variation";

						var adapter = new OleDbDataAdapter(query, conection);
						adapter.Fill(myDT);
						if (myDT.Rows.Count > 0)
						{
							for (int i = 0; i < myDT.Rows.Count; i++)
							{
								DataRow dr = myDT.Rows[i];
								TreeNode nd = e.Node; //represents the current node which is the landing site node
								TreeNode nd1 = new TreeNode();
								if (i == 0)
								{
									//if the node contains the dummy placeholder, rename it to the name of the gear
									nd.FirstNode.Text = dr["Variation"].ToString();
									nd1 = nd.FirstNode;
								}
								else
								{
									//add a new gearvar node and name with the variation field in the query
									nd1 = nd.Nodes.Add(dr["Variation"].ToString());
								}
								//add a dummy placeholder for a new gear node
								// this will be replaced by a sampling month-year node
								//later on
								nd1.Nodes.Add("**dummy*");

								//tag and name the new gear node with the landingsite and gearvar guid
								nd1.Tag = dr["LSGUID"].ToString() + "|" + dr["GearVarGUID"].ToString() + ",gear";
								nd1.Name = dr["LSGUID"].ToString() + "|" + dr["GearVarGUID"].ToString();

                                string iconKey = "";
                                switch (dr["GearClassName"])
                                {
                                    case "Lines":
                                        iconKey = "lines";
                                        break;
                                    case "Traps":
                                        iconKey = "traps";
                                        break;
                                    case "Impounding nets":
                                        iconKey = "impound";
                                        break;
                                    case "Seines and dragnets":
                                        iconKey = "seines";
                                        break;
                                    case "Others":
                                        iconKey = "others";
                                        break;
                                    case "Gillnets":
                                        iconKey = "nets";
                                        break;
                                    case "Jigs":
                                        iconKey = "jigs";
                                        break;

                                }
                                nd1.ImageKey = iconKey;
                                    
								
							}
						}
						else
						{
							e.Node.Nodes.Clear();
						}

					}
				}
				catch (Exception ex)
				{
					ErrorLogger.Log(ex);
				}
			}
			else if (e.Node.FirstNode.Text == "**dummy*")
			{

				//we are in a gear variation node

				var myDT = new DataTable();
				try
				{
					using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;data source=" + global.mdbPath))
					{
						

						conection.Open();

						//node tag contains landing site and gearvar guid
						string[] arr = e.Node.Tag.ToString().Split(',');

						//arr1 will contain landing site and gearvar guid
						string[] arr1 = arr[0].ToString().Split('|');
						string lsguid = arr1[0];
						string gearguid = arr1[1];
						var query = "SELECT Format([SamplingDate],'mmm-yyyy') AS sDate FROM tblSampling " +
									"GROUP BY Format([SamplingDate],'mmm-yyyy'), tblSampling.LSGUID, tblSampling.GearVarGUID, " +
									"Year([SamplingDate]), Month([SamplingDate]) " +
									 "HAVING LSGUID ='{" + lsguid + "}' AND GearVarGUID = '{" + gearguid + "}' " +
									 "ORDER BY Year([SamplingDate]), Month([SamplingDate])";


						//Debug.WriteLine(query);
						var adapter = new OleDbDataAdapter(query, conection);
						adapter.Fill(myDT);
						for (int i = 0; i < myDT.Rows.Count; i++)
						{
							DataRow dr = myDT.Rows[i];
							TreeNode nd1 = new TreeNode();
							if (i == 0)
							{
								//if node contains a dummy placeholder, 
								//rename it to the sampling month-year field
								e.Node.FirstNode.Text = dr["sDate"].ToString();
								nd1 = e.Node.FirstNode;
							}
							else
							{
								nd1 = e.Node.Nodes.Add(dr["sDate"].ToString());
							}
							nd1.Tag = lsguid + "|" + gearguid + ",sampling";
							//nd1.Name = lsguid + "|" + gearguid;
							nd1.Name = lsguid + "|" + gearguid + "|" + dr["sDate"];
                            nd1.ImageKey = "MonthGear";
						}
					}

				}
				catch (Exception ex)
				{
					ErrorLogger.Log(ex);
				}
			}
		}



		public void EnumeratorSelectedSampling(Dictionary<string,string>SamplingIdentifiers)
		{
			TreeNode[] nd = treeView1.Nodes.Find(SamplingIdentifiers["LSGUID"], true);
			treeView1.SelectedNode = nd[0];
			nd[0].Expand();

			TreeNode[] nd1 = nd[0].Nodes.Find(SamplingIdentifiers["LSGUID"] + "|" + SamplingIdentifiers["GearID"], true);
			treeView1.SelectedNode = nd1[0];
			nd1[0].Expand();

			DateTime dt = DateTime.Parse(SamplingIdentifiers["SamplingDate"]);
			string myDate = string.Format("{0:MMM-yyyy}", dt);
			TreeNode[] nd2 = nd1[0].Nodes.Find(SamplingIdentifiers["LSGUID"] + "|" + SamplingIdentifiers["GearID"] + "|" + myDate, true);
			treeView1.SelectedNode = nd2[0];
			nd2[0].Expand();

			_Sampling.SamplingGUID = SamplingIdentifiers["SamplingID"];
			SetupLV3();
			GetSamplingDetailEx1();
			GetCatchComposition();

		}


        void TreeView1AfterSelect(object sender, TreeViewEventArgs e)
		{
			_LSNode = null;
            SetupSamplingButtonFrame(false);

            try
			{

                _AOIName = "";
                _LandingSiteName = "";
                _GearVarName = "";
                _GearClassName = "";
                _AOIGuid = "";
                _LandingSiteGuid = "";
                _GearVarGUID = "";
                _GearClassGUID = "";

                TreeNode nd = treeView1.SelectedNode;
				ResetTheBackColor(treeView1);
				nd.BackColor = Color.Gainsboro;
				string tag = nd.Tag.ToString();
				if (tag != "")
				{
					string[] arr = tag.Split(',');
					_TreeLevel = arr[1];
					SetUPLV(_TreeLevel);
					statusPanelTargetArea.Width = _statusPanelWidth;
					switch (_TreeLevel)
					{
						case "gear":
                            _AOIName = treeView1.SelectedNode.Parent.Parent.Text;
                            _AOIGuid = treeView1.SelectedNode.Parent.Parent.Name;
                            _LandingSiteName = treeView1.SelectedNode.Parent.Text;
                            _LandingSiteGuid = treeView1.SelectedNode.Parent.Name;
                            _GearVarName = treeView1.SelectedNode.Text;
                            var arr1 = treeView1.SelectedNode.Name.Split('|');
                            _GearVarGUID = arr1[1];
                            var rv = global.GearClassGuidNameFromGearVarGuid(_GearVarGUID);
                            _GearClassName = rv.Value;
                            _GearClassGUID = rv.Key;
                            _LSNode = e.Node.Parent;
							break;
						case "sampling":
							//arr2 will contain the landing site and gearvar guids
							string[] arr2 = arr[0].Split('|');
                            _LandingSiteGuid = arr2[0];
                            _GearVarGUID = arr2[1];

							//we will look for the aoi ancestor node and get the aoi guid
							string[] arr3 = treeView1.SelectedNode.Parent.Parent.Parent.Tag.ToString().Split(',');
                            this.AOIGUID = arr3[0];
                            _AOI.AOIGUID = this.AOIGUID;

                            _SamplingMonth = e.Node.Text;


                            //this will fill the listview with the samplings for a month-year
                            FillLVSamplingSummary(_LandingSiteGuid, _GearVarGUID, _SamplingMonth);

                            _AOIName = treeView1.SelectedNode.Parent.Parent.Parent.Text;
                            _AOIGuid = treeView1.SelectedNode.Parent.Parent.Parent.Name;
                            _LandingSiteName = treeView1.SelectedNode.Parent.Parent.Text;
                            _LandingSiteGuid = treeView1.SelectedNode.Parent.Parent.Name;
                            _GearVarName = treeView1.SelectedNode.Parent.Text;

                            rv = global.GearClassGuidNameFromGearVarGuid(_GearVarGUID);
                            _GearClassName = rv.Value;
                            _GearClassGUID = rv.Key;
                            _LSNode = e.Node.Parent.Parent;
							break;
						case "aoi":
                            _AOIName = treeView1.SelectedNode.Text;
                            _AOIGuid = treeView1.SelectedNode.Name;
                            _LandingSiteName = "";
                            _LandingSiteGuid = "";
                            _GearVarName = "";
                            _GearVarGUID = "";
							break;
						case "root":

                            break;
						case "landing_site":
                            _AOIName = treeView1.SelectedNode.Parent.Text;
                            _AOIGuid = treeView1.SelectedNode.Parent.Name;
                            _LandingSiteName = treeView1.SelectedNode.Text;
                            _LandingSiteGuid = treeView1.SelectedNode.Name;
                            _GearVarName = "";
                            _GearVarGUID = "";
                            _LSNode = e.Node;

							break;
					}

                    statusPanelTargetArea.Text = _AOIName;
                    statusPanelLandingSite.Text = _LandingSiteName;
                    statusPanelGearUsed.Text = _GearVarName;

				}
			}
			catch (Exception ex)
            {
                ErrorLogger.Log(ex);
            }

            e.Node.SelectedImageKey = e.Node.ImageKey;
            
        }


        void ShowCatchDetailEx(string SamplingGUID)
        {
            listView1.Items.Clear();
            _Sampling.ReadUIFromXML();

            //we fill up the list view from the _Sampling class variable.
            _Sampling.SamplingGUID = SamplingGUID;
            Dictionary<string, string> effortData = _Sampling.CatchAndEffort();

            

            //the array splits the dictionary item from the name [0] and its guid [1]
            //we make the guid the tag of the listitem
            string[] arr1 = effortData["LandingSite"].Split('|');
            listView1.Items["LandingSite"].SubItems[1].Text = arr1[0];
            listView1.Items["LandingSite"].Tag = arr1[1];

            arr1 = effortData["Enumerator"].Split('|');
            listView1.Items["Enumerator"].SubItems[1].Text = arr1[0];
            listView1.Items["Enumerator"].Tag = arr1[1];

            arr1 = effortData["GearClass"].Split('|');
            listView1.Items["GearClass"].SubItems[1].Text = arr1[0];
            listView1.Items["GearClass"].Tag = arr1[1];

            arr1 = effortData["FishingGear"].Split('|');
            listView1.Items["FishingGear"].SubItems[1].Text = arr1[0];
            listView1.Items["FishingGear"].Tag = arr1[1];

            foreach (ListViewItem lvi in listView1.Items)
            {
                switch (lvi.Name){ 
                    case "LandingSite":
                    case "Enumerator":
                    case "GearClass":
                    case "FishingGear":
                    case "GearSpecs":
                    case "AdditionalFishingGround":
                    case "spacer":
                        break;
                    default:
                        lvi.SubItems[1].Text = effortData[lvi.Name];
                        break;
                }
            }

            _VesHeight = effortData["VesHeight"];
            _VesLength = effortData["VesLength"];
            _VesWidth = effortData["VesWidth"];

            //position sampling buttons and make it visible
            SetupSamplingButtonFrame(true);


        }

        void SetupSamplingButtonFrame(bool Visible)
        {
            panelSamplingButtons.Visible = Visible;
            if (Visible)
            {
                this.panelSamplingButtons.Location = new Point(listView1.Width - panelSamplingButtons.Width-10, listView1.Top + 50);
            }
            CancelButton = buttonOK.Visible ? buttonOK : null;
        }

        void ResetTheBackColor(Control c)
		{
			switch (c.Name)
			{
				case "listView1":
				case "listView3":
					foreach (ListViewItem item in ((ListView)c).Items)
					{
						item.BackColor = Color.White;
					}
					break;
				case "treeView1":
					TreeNodeCollection nodes = treeView1.Nodes;
					TraverseTree(treeView1.Nodes);
					break;
			}

		}

		void TraverseTree(TreeNodeCollection nodes)
		{
			foreach (TreeNode child in nodes)
			{
				child.BackColor = Color.White;
				TraverseTree(child.Nodes);
			}
		}

		void ShowCatchDetail(ListViewItem lvi)
	   {
			_Sampling.SamplingGUID = lvi.Tag.ToString();

			//this will fill the 2 lower list views
			//SetupLV2();
			SetupLV3();
			//GetSamplingDetail();
			GetSamplingDetailEx1();
			GetCatchComposition();


			listView1.EnsureVisible(lvi.Index);
		}

		void GetSamplingDetailEx()
		{
			Dictionary<string, string> ce = _Sampling.CatchAndEffort();
			
		}

		void GetSamplingDetailEx1()
		{
            ;
		}
		void GetSamplingDetail()
		{

            ;
		}

		void GetCatchComposition()
		{
            ;
		}

		public static void SaveColumnWidthEx(object sender, string context = "", global.lvContext myContext = global.lvContext.None)
		{
			string cw = "";
			int i = 0;
			string subKey = "";
			string SaveContext = "";
			subKey = "SOFTWARE\\FAD3\\ColWidth";
			ListView lv = (ListView)sender;

			foreach (var item in lv.Columns)
			{
				ColumnHeader ch = (ColumnHeader)item;
				if (i == 0)
				{
					cw = ch.Width.ToString();
				}
				else
				{
					cw = cw + "," + ch.Width.ToString();
				}
				i++;
			}

			if (context != "")
			{
				SaveContext = context;
			}
			else if (myContext != global.lvContext.None)
			{
				SaveContext = myContext.ToString();
			}
			RegistryKey rk = Registry.CurrentUser.CreateSubKey(subKey);
			rk.SetValue(SaveContext, cw, RegistryValueKind.String);
			rk.Close();

		}

		public void SaveColumnWidth(object sender)
		{
			string cw = "";
			string myContext = listView1.Tag.ToString();
			int i = 0;
			string subKey = "";
			ListView lv = (ListView)sender;
			switch (lv.Name)
			{
				case "listView1":
					subKey = "SOFTWARE\\FAD3\\ColWidth";
					break;
				case "listView2":
					subKey = "SOFTWARE\\FAD3\\ColWidth\\lv2";
					break;
				case "listView3":
					subKey = "SOFTWARE\\FAD3\\ColWidth\\lv3";
					break;
				case "listEnumeratorSampling":
					subKey = "SOFTWARE\\FAD3\\ColWidth\\Enumerator";
					myContext = "Enumerator";
					break;
			}

			foreach (var item in lv.Columns)
			{
				ColumnHeader ch = (ColumnHeader)item;
				if (i == 0)
				{
					cw = ch.Width.ToString();
				}
				else
				{
					cw = cw + "," + ch.Width.ToString();
				}
				i++;
			}
			RegistryKey rk = Registry.CurrentUser.CreateSubKey(subKey);
			rk.SetValue(myContext, cw, RegistryValueKind.String);
			rk.Close();
		}

		void ListView1Leave(object sender, EventArgs e)
		{
			SaveColumnWidthEx(sender, listView1.Tag.ToString());
		}

		void ListView2Leave(object sender, EventArgs e)
		{
			SaveColumnWidthEx(sender, myContext:global.lvContext.CatchAndEffort);
		}

       void ProcessFileOpen()
        {
            string filename = "";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open an existing fisheries database file";
            if (global.mdbPath.Length == 0)
            {
                ofd.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            }
            else
            {
                ofd.InitialDirectory = global.mdbPath;
            }
            ofd.Filter = "Microsoft Access Data File (.mdb)|*.mdb";
            ofd.ShowDialog();
            filename = ofd.FileName;
            if (filename != "")
            {
                global.mdbPath = filename;
                RegistryKey rk = Registry.CurrentUser.CreateSubKey("SOFTWARE\\FAD3");
                rk.SetValue("mdbPath", filename, RegistryValueKind.String);
                rk.Close();
                lblErrorFormOpen.Visible = false;
                PopulateTree();
                statusPanelDBPath.Text = filename;

                //add the recently opened file to the MRU
                _mrulist.AddFile(filename);
            }
        }

		


		public void UpdatedCatchLine(bool isNew, sampling.CatchLine CatchCompLine)
		{
            ;
		}

        void statusPanelDBPath_DoubleClick(object sender, EventArgs e)
		{
			Process.Start(Path.GetDirectoryName(global.mdbPath));
		}

		void ListView3Leave(object sender, EventArgs e)
		{
			//SaveColumnWidth(sender);
			SaveColumnWidthEx(sender, myContext: global.lvContext.CatchComposition);
		}






        /*
		void addAOIToolStripMenuItemClick(object sender, EventArgs e)
		{
			frmAOI f = new frmAOI();
			f.AddNew();
			f.Text = "New AOI";
			f.ShowDialog(this);
		}

		void AddLandingSiteToolStripMenuItemClick(object sender, EventArgs e)
		{
			string[] arr = new string[1];
			arr = treeView1.SelectedNode.Tag.ToString().Split(',');
			frmLandingSite f = new frmLandingSite();

			f.AddNew();            
			f.AOIGUID = arr[0];
			f.Text = "New landing site";
			f.ShowDialog(this);
			//f.Show(this);
		}

		void AddSamplingToolStripMenuItemClick(object sender, EventArgs e)
		{
			string[] arr = new string[1];
			TreeNode nd = new TreeNode();
			nd = treeView1.SelectedNode;
			arr = nd.Tag.ToString().Split(',');

			switch (arr[1])
			{
				case "sampling":
					arr = nd.Parent.Parent.Parent.Tag.ToString().Split(',');
					_AOI.AOIGUID = arr[0];
					_AOI.AOIName = nd.Parent.Parent.Parent.Text;
					break;
				case "landing_site":
					arr = nd.Parent.Tag.ToString().Split(',');
					_AOI.AOIGUID = arr[0];
					_AOI.AOIName = nd.Parent.Text;
					break;
				case "gear":
					arr = nd.Parent.Parent.Tag.ToString().Split(',');
					_AOI.AOIGUID = arr[0];
					_AOI.AOIName = nd.Parent.Parent.Text;
					break;
			}
			arr = nd.Tag.ToString().Split(',');
			if (_AOI.HaveEnumerators) {
				switch (arr[1])
				{
					case "sampling":
						_LSNode = nd.Parent.Parent;
						arr = _LSNode.Tag.ToString().Split(',');
						_ls.LandingSiteGUID = arr[0];
						_ls.LandingSiteName = nd.Parent.Parent.Text;
						_ls.GearVariationName = nd.Parent.Text;
						arr = nd.Tag.ToString().Split(',');
						string[] arr1 = arr[0].Split('|');
						_ls.GearVarGUID = arr1[1];
						break;
					case "landing_site":
						_LSNode = nd;    
						_ls.LandingSiteGUID = arr[0];
						_ls.LandingSiteName = nd.Text;
						break;
					case "gear":
						_LSNode = nd.Parent;
						arr = _LSNode.Tag.ToString().Split(',');
						_ls.LandingSiteGUID = arr[0];
						_ls.LandingSiteName = _LSNode.Text;
						_ls.GearVariationName = nd.Text;
						arr = nd.Tag.ToString().Split(',');
						arr1 = arr[0].Split('|');
						_ls.GearVarGUID = arr1[1];
						break;
				}
				frmEffort f = new frmEffort();
				f.AOI = _AOI;
				f.InvokedFromList = false;
				f.AddNew();
				f.ParentForm(this);
				f.LandingSite = _ls;
				f.Text = "New sampling";
				f.ShowDialog(this);

			}
			else
			{
				MessageBox.Show("You need to add enumerators first" + Environment.NewLine +
								"before you can add your first sampling");
			}

		}

		void AddEnumeratorToolStripMenuItemClick(object sender, EventArgs e)
		{
			frmEnumerator f = new frmEnumerator();
			f.AOI = _AOI;
			f.AddNew();
			f.ShowDialog();
		}
        */

		void TreeView1BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
		}

		void OptionsToolStripMenuItemClick(object sender, EventArgs e)
		{

		}

		void ShowErrorMessagesToolStripMenuItemClick(object sender, EventArgs e)
		{
			//showErrorMessagesToolStripMenuItem.Checked = !showErrorMessagesToolStripMenuItem.Checked;
			//global.ShowErrorMessage = showErrorMessagesToolStripMenuItem.Checked;
		}



		void ShowFormCatch()
		{
			bool Proceed = true;
			if (_frmLF!=null)
			{
				_frmLF.Close();
			}
			if (_frmCatch == null && _frmLF == null)
			{
				_frmCatch = new frmCatch();
				_frmCatch.FormClosed += (o, ea) => _frmCatch = null;
			}
			else if (_frmLF == null)
			{
				_frmCatch.WindowState = FormWindowState.Normal;
			}
			else
			{
				Proceed = false;
			}

			if (Proceed)
			{
				try
				{
					_frmCatch.Show(this);
				}
				catch
				{
					_frmCatch.Focus();
				}


				_frmCatch.ParentForm = this;
				_frmCatch.Sampling = _Sampling;
			}
		}
		void ListView3_DoubleClick(object sender, EventArgs e)
		{
			ShowFormCatch();
		}

        /*
		void AddNewToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string myTag = contextListViewMenuStrip.Tag.ToString();
			if (myTag == "treeview" || myTag =="ListView3")
			{
				ListView3_DoubleClick(sender, e);
			}
			else
			{
				TreeNode nd = treeView1.SelectedNode;
				Dictionary<string, string> newData = new Dictionary<string, string>();
				newData.Add("GearName", nd.Parent.Text);
				newData.Add("LandingSite",nd.Parent.Parent.Text);
				string[] arr = nd.Tag.ToString().Split(',');
				string[] arr1 = arr[0].Split('|');
				newData.Add("LSGUID", arr1[0]);
				newData.Add("GearGUID", arr1[1]);
				foreach(var item in global.GearClassFromGearVar(newData["GearGUID"]))
				{
					newData.Add(item.Key,item.Value);
				}
				frmEffort f = new frmEffort();
				f.InvokedFromList = true;
				f.NewSamplingData = newData;
				f.AOI = _AOI;
				f.AddNew();
				f.ParentForm(this);
				f.LandingSite = _ls;
				
				f.ShowDialog();

			}
		}

        */



		private void buttonEdit_Click(object sender, EventArgs e)
		{
			frmEffort frm = new frmEffort();
			frm.AOI = _AOI;
			frm.ParentForm(this);
			frm.LandingSite = _ls;
			frm.Sampling = _Sampling;
			frm.ShowDialog();
		}


        /*
		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{

            if (contextListViewMenuStrip.Tag.ToString()=="Listview1")
			{
				if (_frmCatch == null && _frmLF == null)
				{
					ListViewItem lvi = listView1.SelectedItems[0];
					if (sampling.DeleteSampling(lvi.Tag.ToString()))
					{
						listView1.Items.Remove(lvi);
					}
				}
				else
				{
					MessageBox.Show("Please close catch data windows and the LF/GMS window");
				}
			}
		}
        */
		










        /*
		private void exportXlsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Title = "Provide filename of Excel file";
			if (global.mdbPath.Length == 0)
			{
				sfd.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
			}
			else
			{
				sfd.InitialDirectory = global.mdbPath;
			}
			//ofd.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
			sfd.Filter = "Microsoft Excel (.xls)|*.xls";
			sfd.ShowDialog();

			string newXLS = sfd.FileName;
			if (newXLS.Length>0)
			{
				global.Export2Excel(listView1, newXLS);
			}
		}
        */


        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem tsi = e.ClickedItem;
            switch (tsi.Tag)
            {
                case "about":
                    frmAbout f = new frmAbout();
                    f.ShowDialog(this);
                    break;
                case "gear":
                    frmGearUsage form = frmGearUsage.GetInstance();
                    if (!form.Visible)
                    {
                        form.Show(this);
                    }
                    else
                    {
                        form.BringToFront();
                    }
                        break;
                case "fish":
                    break;
                case "report":
                    break;
                case "exit":
                    this.Close();
                    break;
                case "map":
                    frmMap fm = new frmMap();
                    fm.Show(this);
                    break;

            }
        }



        private void toolsToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem tsi = e.ClickedItem;
            switch (tsi.Tag)
            {
                case "resetRefNos":
                    break;
                case "refNoRange":
                    frmRefNoRange f = new frmRefNoRange();
                    f.ShowDialog();
                    break;
                case "coordFormat":
                    break;
                case "symbolFonts":
                    break;
                case "showError":
                    showErrorMessagesToolStripMenuItem.Checked = !showErrorMessagesToolStripMenuItem.Checked;
                    global.ShowErrorMessage = showErrorMessagesToolStripMenuItem.Checked;
                    break;
            }
        }



        private void generateGridMapToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem tsi = e.ClickedItem;
            switch (tsi.Tag)
            {
                case "zone50":
                    break;
                case "zone51":
                    break;
            }
        }

        private void helpToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem tsi = e.ClickedItem;
            switch (tsi.Tag)
            {
                case "about":
                    frmAbout f = new frmAbout();
                    f.ShowDialog();
                    break;
                case "onlineManual":
                    break;
            }
        }

        private void fileToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem tsi = e.ClickedItem;
            switch (tsi.Tag)
            {
                case "new":
                    frmNewDB f = new frmNewDB();
                    f.ParentForm(this);
                    f.ShowDialog(this);
                    break;
                case "open":
                    ProcessFileOpen();
                    break;
                case "exit":
                    this.Close();
                    break;
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem lvi = new ListViewItem();
            _topLVItemIndex = listView1.TopItem.Index;
            foreach (var item in listView1.SelectedItems)
            {
                lvi = (ListViewItem)item;
                string tag = listView1.Tag.ToString();
                switch (tag)
                {
                    case "aoi":
                        if (lvi.Tag != null)
                        {
                            if (lvi.Tag.ToString() == "aoi_data")
                            {
                                string[] arr = treeView1.SelectedNode.Tag.ToString().Split(',');
                                _AOI.AOIGUID = arr[0];
                                frmAOI f = new frmAOI();
                                f.AOI = _AOI;
                                f.Show();
                            }
                            else if (lvi.Name == "Enumerators")
                            {
                                frmEnumerator frm = new frmEnumerator(lvi.Tag.ToString());
                                frm.AOI = _AOI;
                                frm.ParentForm = this;
                                frm.Show(this);
                            }
                        }
                        break;
                    case "database":
                        if (lvi.Text == "Database path")
                        {
                            Process.Start(Path.GetDirectoryName(global.mdbPath));
                        }
                        break;
                    case "landing_site":

                        string[] arr1 = treeView1.SelectedNode.Tag.ToString().Split(',');
                        //landingsite ls = new landingsite(arr1[0]);
                        frmLandingSite fls = new frmLandingSite();
                        fls.LandingSite = _ls;
                        arr1 = treeView1.SelectedNode.Parent.Tag.ToString().Split(',');
                        fls.AOIGUID = arr1[0].ToString();
                        fls.Show();
                        break;
                    case "gear":
                        break;
                    case "sampling":
                        SetUPLV("samplingDetail");
                        _SamplingGUID = lvi.Tag.ToString();
                        ShowCatchDetailEx(_SamplingGUID);
                        lvi.BackColor = Color.Gainsboro;
                        break;
                    case "samplingDetail":
                        ShowSamplingDetailForm();
                        break;
                }
            }
        }

        private void ShowSamplingDetailForm()
        {
            frmSamplingDetail fs = new frmSamplingDetail();
            fs.SamplingGUID = _SamplingGUID;
            fs.LVInterface(listView1);
            fs.AOI = _AOI;
            fs.AOIGuid = _AOIGuid;
            fs.Parent_Form = this;
            fs.VesselDimension(_VesLength, _VesWidth, _VesHeight);
            fs.Show(this);
        }

        private void BackToSamplingMonth()
        {
            SetupSamplingButtonFrame(false);
            SetUPLV(_TreeLevel);
            FillLVSamplingSummary(_LandingSiteGuid, _GearVarGUID, _SamplingMonth);
            listView1.Focus();
            listView1.Items[_SamplingGUID].Selected = true;
            listView1.Items[_SamplingGUID].EnsureVisible();
            listView1.TopItem = listView1.Items[_topLVItemIndex];
        }

        private void buttonSamplingClick (object sender, EventArgs e)
        {
            Button b = (Button)sender;
            switch (b.Name)
            {
                case "buttonOK":
                    BackToSamplingMonth();
                    break;
                case "buttonCatch":
                    break;
                case "buttonMap":
                    break;
            }

        }

        private void frmMain_ResizeEnd(object sender, EventArgs e)
        {
            splitContainer1.Width = this.Width;
            listView1.Width = splitContainer1.Panel2.Width;
            listView1.Height = splitContainer1.Panel2.Height;
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string[] arr = new string[0];
            string[] arr1 = new string[0];

            ToolStripItem tsi = e.ClickedItem;
            switch (tsi.Name)
            {
                case "addAOIToolStripMenuItem":
                    frmAOI f1 = new frmAOI();
                    f1.AddNew();
                    f1.Text = "New AOI";
                    f1.ShowDialog(this);
                    break;
                case "addLandingSiteToolStripMenuItem":
                    arr = treeView1.SelectedNode.Tag.ToString().Split(',');
                    frmLandingSite f2 = new frmLandingSite();

                    f2.AddNew();
                    f2.AOIGUID = arr[0];
                    f2.Text = "New landing site";
                    f2.ShowDialog(this);
                    break;
                case "addSamplingToolStripMenuItem":
                    TreeNode nd = new TreeNode();
                    nd = treeView1.SelectedNode;
                    arr = nd.Tag.ToString().Split(',');

                    //arr[1] represents the treeview level clicked
                    switch (arr[1])
                    {
                        case "sampling":
                            arr = nd.Parent.Parent.Parent.Tag.ToString().Split(',');
                            _AOI.AOIGUID = arr[0];
                            _AOI.AOIName = nd.Parent.Parent.Parent.Text;

                            SamplingLevelNewSampling();
                            break;
                        case "landing_site":
                            arr = nd.Parent.Tag.ToString().Split(',');
                            _AOI.AOIGUID = arr[0];
                            _AOI.AOIName = nd.Parent.Text;

                            _AOIName = _AOI.AOIName;
                            _AOIGuid = _AOI.AOIGUID;
                            _LandingSiteName = nd.Text;
                            _LandingSiteGuid = nd.Name;
                            break;
                        case "gear":
                            arr = nd.Parent.Parent.Tag.ToString().Split(',');
                            _AOI.AOIGUID = arr[0];
                            _AOI.AOIName = nd.Parent.Parent.Text;
                            break;
                    }
                    arr = nd.Tag.ToString().Split(',');
                    if (_AOI.HaveEnumerators)
                    {
                        switch (arr[1])
                        {
                            case "sampling":
                                _LSNode = nd.Parent.Parent;
                                arr = _LSNode.Tag.ToString().Split(',');
                                _ls.LandingSiteGUID = arr[0];
                                _ls.LandingSiteName = nd.Parent.Parent.Text;
                                _ls.GearVariationName = nd.Parent.Text;
                                arr = nd.Tag.ToString().Split(',');
                                arr1 = arr[0].Split('|');
                                _ls.GearVarGUID = arr1[1];
                                break;
                            case "landing_site":
                                _LSNode = nd;
                                _ls.LandingSiteGUID = arr[0];
                                _ls.LandingSiteName = nd.Text;
                                break;
                            case "gear":
                                _LSNode = nd.Parent;
                                arr = _LSNode.Tag.ToString().Split(',');
                                _ls.LandingSiteGUID = arr[0];
                                _ls.LandingSiteName = _LSNode.Text;
                                _ls.GearVariationName = nd.Text;
                                arr = nd.Tag.ToString().Split(',');
                                arr1 = arr[0].Split('|');
                                _ls.GearVarGUID = arr1[1];
                                break;
                        }

                        NewSamplingForm();

                    }
                    else
                    {
                        MessageBox.Show("You need to add enumerators first" + Environment.NewLine +
                                        "before you can add your first sampling");
                    }
                    break;
                case "addEnumeratorToolStripMenuItem":
                    frmEnumerator f4 = new frmEnumerator();
                    f4.AOI = _AOI;
                    f4.AddNew();
                    f4.ShowDialog();
                    break;
            }
        }

        private void NewSamplingForm()
        {
            var f3 = new frmSamplingDetail
            {
                IsNew = true,
                GearClassName = _GearClassName,
                GearClassGuid = _GearClassGUID,
                GearVarGuid = _GearVarGUID,
                GearVarName = _GearVarName,
                AOIGuid = _AOIGuid,
                AOIName = _AOIName,
                LandingSiteName = _LandingSiteName,
                LandingSiteGuid = _LandingSiteGuid,
                AOI = _AOI
            };
            f3.LVInterface(listView1);
            f3.Parent_Form = this;
            f3.ShowDialog(this);

        }

        private void SamplingLevelNewSampling()
        {
            if (_TreeLevel == "sampling" || _TreeLevel=="landing_site")
            {
                string[] arr;
                var nd = treeView1.SelectedNode;
                _AOIGuid = _AOI.AOIGUID;
                _AOIName = _AOI.AOIName;
                arr = nd.Parent.Name.Split('|');
                _GearVarName = nd.Parent.Text;
                _GearVarGUID = arr[1];
                var kvGearClass = global.GearClassFromGearVar(_GearVarGUID);
                _GearClassGUID = kvGearClass.Key;
                _GearClassName = kvGearClass.Value;
                _LandingSiteName = nd.Parent.Parent.Text;
                _LandingSiteGuid = nd.Parent.Parent.Name;
            }
        }

        private void contextListViewMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem tsi = e.ClickedItem;
            switch (tsi.Name)
            {
                case "addNewToolStripMenuItem":
                    SamplingLevelNewSampling();
                    NewSamplingForm();
                    break;
                case "deleteToolStripMenuItem":
                    if (true)
                    {
                        if (_frmCatch == null && _frmLF == null)
                        {
                            ListViewItem lvi = listView1.SelectedItems[0];
                            if (sampling.DeleteSampling(lvi.Tag.ToString()))
                            {
                                listView1.Items.Remove(lvi);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please close catch data windows and the LF/GMS window");
                        }
                    }
                    break;
                case "lengthFreqToolStripMenuItem":
                    break;
                case "gMSToolStripMenuItem":
                    break;
                case "catchDataToolStripMenuItem":
                    ShowFormCatch();
                    break;
                case "exportXlsToolStripMenuItem":
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Title = "Provide filename of Excel file";
                    if (global.mdbPath.Length == 0)
                    {
                        sfd.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
                    }
                    else
                    {
                        sfd.InitialDirectory = global.mdbPath;
                    }
                    //ofd.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
                    sfd.Filter = "Microsoft Excel (.xls)|*.xls";
                    sfd.ShowDialog();

                    string newXLS = sfd.FileName;
                    if (newXLS.Length > 0)
                    {
                        global.Export2Excel(listView1, newXLS);
                    }
                    break;
            }
        }

        private void frmMain_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void frmMain_Activated(object sender, EventArgs e)
        {
            CancelButton = buttonOK.Visible ? buttonOK : null;
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
              ConfigDropDownMenu(treeView1);
        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListView lv = (ListView)sender;
                ListViewHitTestInfo lvh = lv.HitTest(e.X, e.Y);
                if (_TreeLevel == "sampling")
                {
                    ConfigDropDownMenu(listView1, lvh);
                }
                else
                {
                    ConfigDropDownMenu(listView1);
                }
            }
        }
    }
}

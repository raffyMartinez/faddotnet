﻿/*
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
using System.Data.SqlClient;
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
        //private frmCatch _frmCatch;
        //private frmLenFreq _frmLF;
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

        public static int TextWidth(string text, Font f)
        {
            Label l = new Label
            {
                Text=text,
                Font=f
            };
            return l.Width;

        }

        public string AOIGUID
        {
            get { return _AOIGuid; }
            set
            {
                if (value != _AOIGuid)
                {
                    _AOIGuid = value;
                    _AOI.AOIGUID = _AOIGuid;
                    FishingGrid.AOIGuid = _AOIGuid;
                }
            }
        }

        public string AOIName
        {
            get { return _AOIName; }
            set
            {
                _AOIName = value;
                _AOI.AOIName = _AOIName;
            }
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

            if (global.AllRequiredFilesExists)
            {
                sampling.SetUpUIElement();

                _Sampling.OnUIRowRead += new sampling.ReadUIElement(OnUIRowRead);

                toolStripRecentlyOpened.DropDownItems.Clear();
                
                //setup an MRU that contains 5 items
                _mrulist = new mru("FAD3", toolStripRecentlyOpened, 5);

                //setup the event handlers
                _mrulist.FileSelected += _mrulist_FileSelected;
                _mrulist.ManageMRU += _mrulist_ManageMRU;
                
                RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\FAD3");
                try
                {
                    var SavedMDBPath = rk.GetValue("mdbPath", "NULL").ToString();
                    if (SavedMDBPath !="NULL" && File.Exists(SavedMDBPath))
                    {
                        _oldMDB = SavedMDBPath;
                        global.mdbPath = SavedMDBPath;
                        names.GetGenus_LocalNames();
                        names.GetLocalNames();
                        statusPanelDBPath.Text = SavedMDBPath;
                        lblErrorFormOpen.Visible = false;
                        PopulateTree();
                    }
                    else
                    {
                        ErrorLogger.Log("MDB file saved in registry not found");
                        lblErrorFormOpen.Visible = true;
                        lblTitle.Text = "";
                        lblErrorFormOpen.Text = "Please locate the database file where fisheries data is saved.\r\n" +
                                                 "You can use the file open menu";
                        LockTheApp(true);
                    }
                }
                catch
                {

                    ErrorLogger.Log("Registry entry for mdb path not found");
                    lblErrorFormOpen.Visible = true;
                    lblTitle.Text = "";
                    lblErrorFormOpen.Text = "Please locate the database file where fisheries data is saved.\r\n" +
                                             "You can use the file open menu";
                    LockTheApp(true);
                }
            }
            else
            {
                ErrorLogger.Log("Not all required files found");
                lblErrorFormOpen.Visible = true;
                lblTitle.Text = "";
                lblErrorFormOpen.Text = "Some files needed by FAD3 to run were not found.\r\n";
                labelErrorDetail.With(o =>
                   {
                       o.Visible = true;
                       o.Text = "The following files were not found:" + global.MissingRequiredFiles;
                       o.Top = lblErrorFormOpen.Top + lblErrorFormOpen.Height;
                       o.Left = lblErrorFormOpen.Left + (lblErrorFormOpen.Width / 2) - (o.Width / 2);
                   });

                LockTheApp();
            }

            statusPanelTargetArea.Width = _statusPanelWidth;
            statusPanelLandingSite.Width = _statusPanelWidth;
            statusPanelGearUsed.Width = _statusPanelWidth;

            ConfigDropDownMenu(treeView1);
            SetupSamplingButtonFrame(false);
        }

        /// <summary>
        /// Locks the user interface by disabling most of the menu items
        /// except for file exit commands
        /// </summary>
        /// <param name="EnableFileOpen"></param>
        private void LockTheApp(bool EnableFileOpen = false)
        {
            foreach (var c in menuMenuBar.Items)
            {
                ((ToolStripMenuItem)c).With(o =>
                    {
                        foreach (var cc in o.DropDownItems)
                        {
                            if (cc.GetType().Name == "ToolStripMenuItem")
                            {
                                ((ToolStripDropDownItem)cc).With(oo =>
                                {
                                    oo.Enabled = oo.Tag != null && (oo.Tag.ToString() == "exit" ||
                                    oo.Tag.ToString() == "onlineManual" || (EnableFileOpen && oo.Tag.ToString() == "open"));
                                });
                            }
                        }
                    });
            }

            foreach (var c in tsToolbar.Items)
            {
                ((ToolStripButton)c).With(o =>
                {
                    o.Enabled = o.Tag != null && o.Tag.ToString() == "exit";
                });
            }
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
                    if (FishingGrid.IsCompleteGrid25)
                    {
                        if (aoi.AOIHaveEnumeratorsEx(_AOIGuid))
                            NewSamplingForm();
                        else
                            MessageBox.Show("Cannot create a new sampling because\n\r" +
                                "Target area has no enumerator", "Cannot create sampling", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Cannot create a new sampling because\n\r" +
                            "target area is not setup for Grid25", "Cannot create sampling", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    break;
                case "menuNewEnumerator":
                    break;
                case "menuSamplingDetail":
                    ShowSamplingDetailForm();
                    break;
                case "menuTargetAreaProp":
                    frmAOI AOIForm = frmAOI.GetInstance();
                    if (!AOIForm.Visible)
                    {
                        //AOIForm = new frmAOI();
                        AOIForm.Parent_form = this;
                        AOIForm.Show(this);
                    }
                    else
                    {
                        AOIForm.BringToFront();
                    }
                    AOIForm.AOI = _AOI;
                    break;
                case "menuLandingSiteProp":
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
                    tsi.Enabled = _TreeLevel == "sampling" || _TreeLevel == "landing_site" || _TreeLevel == "gear";

                    var sep = menuDropDown.Items.Add("-");
                    sep.Name = "menuSeparator1";

                    tsi = menuDropDown.Items.Add("Enumerators");
                    tsi.Name = "menuEnumerators";
                    tsi.Enabled = _TreeLevel == "aoi";

                    tsi = menuDropDown.Items.Add("Target area properties");
                    tsi.Name = "menuTargetAreaProp";
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
                    break;
            }
        }

        /// <summary>
        /// setup the treeview after an mdb file is loaded
        /// </summary>
        /// <param name="MDBFile"></param>
        /// <param name="FromMRU"></param>
        /// <returns></returns>
        private bool SetupTree(string MDBFile, bool FromMRU = false)
        {
            if (File.Exists(MDBFile))
            {
                global.mdbPath = MDBFile;
                RegistryKey rk = Registry.CurrentUser.CreateSubKey("SOFTWARE\\FAD3");
                rk.SetValue("mdbPath", MDBFile, RegistryValueKind.String);
                rk.Close();
                lblErrorFormOpen.Visible = false;
                names.GetGenus_LocalNames();
                names.GetLocalNames();
                PopulateTree();
                statusPanelDBPath.Text = MDBFile;
                return true;
            }
            else
            {
                ErrorLogger.Log("MDB file saved in registry not found");
                lblErrorFormOpen.Visible = true;
                lblTitle.Text = "";
                lblErrorFormOpen.Text = "Please locate the database file where fisheries data is saved.\r\n" +
                                         "You can use the file open menu";
                LockTheApp(true);
                return false;
            }
        }

        public void NewDBFile(string filename)
        {
            SetupTree(filename);

        }

        private void _mrulist_FileSelected(string filename)
        {
            _TreeLevel = "root";
            if (!SetupTree(filename, FromMRU: true))
            {
                _mrulist.RemoveFile(filename);
            }

        }

        void PopulateTree()
        {
            using (var myDataTable = new DataTable())
            {
                try
                {
                    using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;data source=" + global.mdbPath))
                    {
                        conection.Open();

                        string query = "SELECT tblAOI.AOIGuid, tblAOI.AOIName, tblLandingSites.LSGUID, tblLandingSites.LSName " +
                                       "FROM tblAOI LEFT JOIN tblLandingSites ON tblAOI.AOIGuid = tblLandingSites.AOIGuid " +
                                        "ORDER BY tblAOI.AOIName, tblLandingSites.LSName";

                        using (var adapter = new OleDbDataAdapter(query, conection))
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

        /// <summary>
        /// this will show a list of summaries depending on the level of the node selected
        /// level could be root, AOI, Landing site, gear used, and sampling month
        /// </summary>
        /// <param name="TreeLevel"></param>
        private void SetUPLV(string TreeLevel)
        {
            listView1.SuspendLayout();

            int i = 0;

            this.listView1.Clear();
            this.listView1.View = View.Details;
            this.listView1.FullRowSelect = true;

            string myData = "";
            ListViewItem lvi;

            //setup columns in the listview
            switch (TreeLevel)
            {
                case "root":
                    listView1.Columns.Add("Property");
                    listView1.Columns.Add("Value");
                    lblTitle.Text = "Database summary";
                    break;
                case "aoi":
                    listView1.Columns.Add("Property");
                    listView1.Columns.Add("Value");
                    lblTitle.Text = "Area of interest";
                    break;

                case "landing_site":
                    listView1.Columns.Add("Property");
                    listView1.Columns.Add("Value");
                    lblTitle.Text = "Landing site";
                    break;

                case "gear":
                    listView1.Columns.Add("Property");
                    listView1.Columns.Add("Value");
                    lblTitle.Text = "Fishing gear";
                    break;

                case "sampling":
                    this.listView1.Columns.Add("Reference #");
                    this.listView1.Columns.Add("Sampling date");
                    this.listView1.Columns.Add("Catch composition");
                    this.listView1.Columns.Add("Weight of catch");
                    this.listView1.Columns.Add("Fishing ground");
                    this.listView1.Columns.Add("Position");
                    this.listView1.Columns.Add("Enumerator");
                    this.listView1.Columns.Add("Gear specs");
                    this.listView1.Columns.Add("Notes");
                    lblTitle.Text = "Sampling";
                    break;
                case "samplingDetail":
                    listView1.Columns.Add("Property");
                    listView1.Columns.Add("Value");
                    lblTitle.Text = "Sampling detail";
                    break;

            }

            //apply column widths saved in registry
            listView1.Tag = TreeLevel;
            try
            {
                RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\FAD3\\ColWidth");
                string rv = rk.GetValue(listView1.Tag.ToString(), "NULL").ToString();
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


            //add rows to the listview
            switch (_TreeLevel)
            {
                case "sampling":
                    FillLVSamplingSummary(_LandingSiteGuid, _GearVarGUID, _SamplingMonth);
                    break;
                case "samplingDetail":
                    break;
                case "gear":
                    lvi = listView1.Items.Add("Months sampled");
                    var arr = treeView1.SelectedNode.Tag.ToString().Split(',');
                    var arr2 = arr[0].Split('|');
                    var lsguid = arr2[0];
                    var gearguid = arr2[1];
                    _ls.LandingSiteGUID = lsguid;
                    _ls.GearVarGUID = gearguid;
                    var n = 0;
                    foreach (string item in _ls.MonthsSampledEx(gearguid))
                    {
                        if (n > 0)
                        {
                            lvi = listView1.Items.Add("");
                        }
                        lvi.SubItems.Add(item);
                        n++;
                    }
                    break;
                case "root":
                    lvi = listView1.Items.Add("Database path");
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
                    break;
                case "aoi":
                    arr = treeView1.SelectedNode.Tag.ToString().Split(',');
                    myData = _AOI.AOIData();
                    arr = myData.Split('|');

                    //add AOI data for form
                    lvi = listView1.Items.Add("Name");
                    lvi.SubItems.Add(arr[0].ToString());
                    lvi.Tag = "aoi_data";
                    lvi = listView1.Items.Add("Code");
                    lvi.SubItems.Add(arr[1].ToString());
                    lvi.Tag = "aoi_data";
                    lvi = listView1.Items.Add("MBR");
                    lvi.SubItems.Add("");
                    lvi.Tag = "aoi_data";
                    lvi = listView1.Items.Add("Grid system");
                    lvi.Tag = "aoi_data";
                    lvi.SubItems.Add(FishingGrid.GridTypeName);


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
                    if (i == 0)
                    {
                        foreach (KeyValuePair<string, string> item in aoi.AOIEnumeratorsList(_AOIGuid))
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
                    break;
                case "landing_site":
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
                    n = 0;
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
                    break;
            }



            listView1.ResumeLayout();
        }


        string DatabaseSummary(string SummaryTopic)
        {
            return "x";
        }


        /// <summary>
        /// Fills the listview with the samplings that took place in a month-year
        /// </summary>
        /// <param name="LSGUID"></param>
        /// <param name="GearGUID"></param>
        /// <param name="SamplingMonth"></param>
		void FillLVSamplingSummary(string LSGUID, string GearGUID, string SamplingMonth)
        {
            var CompleteGrid25 = FishingGrid.IsCompleteGrid25;
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




            using (var myDT = new DataTable())
            {
                try
                {
                    using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;data source=" + global.mdbPath))
                    {
                        conection.Open();

                        string query = "SELECT tblSampling.RefNo, SamplingDate, FishingGround, EnumeratorName, Notes, WtCatch, tblSampling.SamplingGUID, IsGrid25FG, Count(tblCatchComp.RowGUID) AS [rows] " +
                                        "FROM (tblEnumerators RIGHT JOIN tblSampling ON tblEnumerators.EnumeratorID = tblSampling.Enumerator) LEFT JOIN tblCatchComp " +
                                        "ON tblSampling.SamplingGUID = tblCatchComp.SamplingGUID GROUP BY tblSampling.SamplingDate, tblSampling.RefNo, tblSampling.FishingGround, " +
                                        "tblEnumerators.EnumeratorName, tblSampling.Notes, tblSampling.WtCatch, tblSampling.SamplingGUID, tblSampling.IsGrid25FG, tblSampling.LSGUID, " +
                                        "tblSampling.[GearVarGUID], tblSampling.[SamplingDate] HAVING tblSampling.LSGUID= '{" + LSGUID + "}' AND tblSampling.[GearVarGUID]= '{" + GearGUID + "}'" +
                                        "AND SamplingDate >=#" + StartDate + "# And SamplingDate < #" + EndDate + "#  ORDER BY SamplingDate";


                        using (var adapter = new OleDbDataAdapter(query, conection))
                        {
                            adapter.Fill(myDT);
                            ListViewItem lvi;
                            for (int i = 0; i < myDT.Rows.Count; i++)
                            {
                                DataRow dr = myDT.Rows[i];
                                ListViewItem row = new ListViewItem(dr[0].ToString());      //ref no
                                DateTime dt = (DateTime)dr[1];
                                row.SubItems.Add(string.Format("{0:MMM-dd-yyyy}", dt));     //sampling date
                                row.SubItems.Add(dr[8].ToString());                         //number of catch rows
                                row.SubItems.Add(dr[5].ToString());                         //wt of catch
                                row.SubItems.Add(dr[2].ToString());                         //fishing ground


                                if (CompleteGrid25)                                         //position
                                {
                                    row.SubItems.Add(FishingGrid.Grid25_to_UTM(dr[2].ToString()));
                                }
                                else
                                    row.SubItems.Add("");


                                row.SubItems.Add(dr[3].ToString());                         //enumerator
                                row.SubItems.Add("");                                       //gear specs
                                row.SubItems.Add(dr[4].ToString());                         //notes

                                lvi = this.listView1.Items.Add(row);
                                lvi.Tag = dr[6].ToString();                                 //sampling guid
                                lvi.Name = dr[6].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }
        }


        /// <summary>
        /// this will fill the tree with the nodes below the level of Landing site
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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



        public void EnumeratorSelectedSampling(Dictionary<string, string> SamplingIdentifiers)
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
        }


        void TreeView1AfterSelect(object sender, TreeViewEventArgs e)
        {
            _LSNode = null;
            SetupSamplingButtonFrame(false);

            try
            {

                _LandingSiteName = "";
                _GearVarName = "";
                _GearClassName = "";
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
                    statusPanelTargetArea.Width = _statusPanelWidth;
                    switch (_TreeLevel)
                    {
                        case "gear":
                            _AOIName = treeView1.SelectedNode.Parent.Parent.Text;
                            this.AOIGUID = treeView1.SelectedNode.Parent.Parent.Name;
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
                            _AOIName = treeView1.SelectedNode.Parent.Parent.Parent.Text;
                            this.AOIGUID = treeView1.SelectedNode.Parent.Parent.Parent.Name;
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
                            this.AOIGUID = treeView1.SelectedNode.Name;
                            _LandingSiteName = "";
                            _LandingSiteGuid = "";
                            _GearVarName = "";
                            _GearVarGUID = "";
                            break;
                        case "root":
                            break;
                        case "landing_site":
                            _AOIName = treeView1.SelectedNode.Parent.Text;
                            this.AOIGUID = treeView1.SelectedNode.Parent.Name;
                            _LandingSiteName = treeView1.SelectedNode.Text;
                            _LandingSiteGuid = treeView1.SelectedNode.Name;
                            _GearVarName = "";
                            _GearVarGUID = "";
                            _LSNode = e.Node;

                            break;
                    }

                    SetUPLV(_TreeLevel);
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

        public void RefreshCatchDetail(string SamplingGUID)
        {
            ShowCatchDetailEx(SamplingGUID);
        }

        /// <summary>
        /// fills the listview with the complete effort data from a fish landing sampling
        /// </summary>
        /// <param name="SamplingGUID"></param>
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
                switch (lvi.Name)
                {
                    case "LandingSite":
                    case "Enumerator":
                    case "GearClass":
                    case "FishingGear":
                    case "GearSpecs":
                        break;
                    case "AdditionalFishingGround":
                        foreach (var item in FishingGrid.AdditionalFishingGrounds(_SamplingGUID))
                        {
                            lvi.SubItems[1].Text += item + ", ";
                        }

                        if (lvi.SubItems[1].Text.Length > 0)
                            lvi.SubItems[1].Text = lvi.SubItems[1].Text.Substring(0, lvi.SubItems[1].Text.Length - 2);
                        break;
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

        /// <summary>
        /// positions, shows or hides the buttons of a sampled fishing effort
        /// </summary>
        /// <param name="Visible"></param>
        void SetupSamplingButtonFrame(bool Visible)
        {
            panelSamplingButtons.Visible = Visible;
            if (Visible)
            {
                this.panelSamplingButtons.Location = new Point(listView1.Width - panelSamplingButtons.Width - 10, listView1.Top + 50);
            }
            CancelButton = buttonOK.Visible ? buttonOK : null;
        }

        void ResetTheBackColor(Control c)
        {
            switch (c.Name)
            {
                case "listView1":
                    foreach (ListViewItem item in ((ListView)c).Items)
                    {
                        item.BackColor = Color.White;
                    }
                    break;
                case "treeView1":
                    TreeNodeCollection nodes = treeView1.Nodes;
                    TraverseTreeAndResetColor(treeView1.Nodes);
                    break;
            }

        }

        void TraverseTreeAndResetColor(TreeNodeCollection nodes)
        {
            foreach (TreeNode child in nodes)
            {
                child.BackColor = Color.White;
                TraverseTreeAndResetColor(child.Nodes);
            }
        }

        /// <summary>
        /// saves the column widths of a list view to the registry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="context"></param>
        /// <param name="myContext"></param>
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

        void ListView1Leave(object sender, EventArgs e)
        {
            if (listView1.Columns.Count > 0)
                SaveColumnWidthEx(sender, listView1.Tag.ToString());

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
                SetupTree(filename);

                //add the recently opened file to the MRU
                _mrulist.AddFile(filename);
            }
        }


        void statusPanelDBPath_DoubleClick(object sender, EventArgs e)
        {
            Process.Start(Path.GetDirectoryName(global.mdbPath));
        }

        private void OnToolbar_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
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
                    form.PopulateLists();
                    break;
                case "fish":
                    break;
                case "report":
                    break;
                case "map":
                    frmMap fm = new frmMap();
                    fm.Show(this);
                    break;
                case "exit":
                    AppExit();

                    break;
            }
        }

        private void AppExit()
        {
            this.Close();
        }

        private void OnMenuTools_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
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

        private void OnMenuHelp_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
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

        private void OnMenuFile_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
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
                    AppExit();
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

        private void buttonSamplingClick(object sender, EventArgs e)
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
                    if (_AOI.HaveEnumerators)
                    {
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
                AOIGuid = AOIGUID,
                AOIName = AOIName,
                LandingSiteName = _LandingSiteName,
                LandingSiteGuid = _LandingSiteGuid,
                AOI = _AOI
            };
            f3.LVInterface(listView1);
            f3.Parent_Form = this;
            f3.ShowDialog(this);

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

        private void _mrulist_ManageMRU(object sender, EventArgs e )
        {
            ManageMRUForm f = new ManageMRUForm();
            f.Parent_form = this;
            f.ShowDialog(this);
        }

    }
}

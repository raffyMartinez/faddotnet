/*
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/7/2016
 * Time: 1:53 PM
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FAD3
{
    /// <summary>
    /// Description of frmMain.
    /// </summary>
    public partial class frmMain : Form
    {
        private aoi _AOI = new aoi();
        private string _AOIGuid = "";
        private string _AOIName = "";
        private global.fad3CatchSubRow _CatchSubRow;
        private string _GearClassGUID = "";
        private string _GearClassName = "";
        private string _GearVarGUID = "";
        private string _GearVarName = "";
        private string _LandingSiteGuid = "";
        private string _LandingSiteName = "";
        private landingsite _ls = new landingsite();
        private TreeNode _LSNode;
        private int _MouseX = 0;
        private int _MouseY = 0;
        private mru _mrulist = new mru();
        private string _oldMDB = "";
        private sampling _Sampling = new sampling();
        private string _SamplingGUID = "";
        private string _SamplingMonth = "";
        private int _statusPanelWidth = 200;
        private bool _subListExisting = false;
        private int _topLVItemIndex = 0;
        private string _TreeLevel = "";
        private string _VesHeight = "";
        private string _VesLength = "";
        private string _VesWidth = "";
        private bool _newSamplingEntered;
        private string _MonthYear;
        private string _MonthYearGearVar;
        private string _MonthYearLandingSite;

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

        public aoi AOI
        {
            get { return _AOI; }
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

        public string SamplingGUID
        {
            get { return _SamplingGUID; }
            set
            {
                _SamplingGUID = value;
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

        public static int TextWidth(string text, Font f)
        {
            Label l = new Label
            {
                Text = text,
                Font = f
            };
            return l.Width;
        }

        public void EnumeratorSelectedSampling(Dictionary<string, string> SamplingIdentifiers)
        {
            TreeNode[] nd = treeMain.Nodes.Find(SamplingIdentifiers["LSGUID"], true);
            treeMain.SelectedNode = nd[0];
            nd[0].Expand();

            TreeNode[] nd1 = nd[0].Nodes.Find(SamplingIdentifiers["LSGUID"] + "|" + SamplingIdentifiers["GearID"], true);
            treeMain.SelectedNode = nd1[0];
            nd1[0].Expand();

            DateTime dt = DateTime.Parse(SamplingIdentifiers["SamplingDate"]);
            string myDate = string.Format("{0:MMM-yyyy}", dt);
            TreeNode[] nd2 = nd1[0].Nodes.Find(SamplingIdentifiers["LSGUID"] + "|" + SamplingIdentifiers["GearID"] + "|" + myDate, true);
            treeMain.SelectedNode = nd2[0];
            nd2[0].Expand();

            _Sampling.SamplingGUID = SamplingIdentifiers["SamplingID"];
        }

        public void NewDBFile(string filename)
        {
            SetupTree(filename);
        }

        public void NewSamplingDataEntryCancelled()
        {
            //if (listView1.Tag.ToString() == "samplingDetail") BackToSamplingMonth();
        }

        public void RefreshCatchDetail(string SamplingGUID, bool NewSampling,
                                       DateTime SamplingDate, string GearVarGuid, string LandingSiteGuid)
        {
            _SamplingGUID = SamplingGUID;
            _newSamplingEntered = NewSampling;
            lvMain.Columns.With(o =>
            {
                o.Clear();
                o.Add("Property");
                o.Add("Value");
            });
            ApplyListViewColumnWidth("samplingDetail");
            if (_newSamplingEntered)
            {
                _MonthYear = SamplingDate.ToString("MMM-yyyy");
                _MonthYearGearVar = GearVarGuid;
                _MonthYearLandingSite = LandingSiteGuid;
                RefreshTreeForNewSampling();
            }
            SetUPLV("samplingDetail");
            ShowCatchDetailEx(_SamplingGUID);
        }

        public void RefreshLV(string NodeName, string TreeLevel, bool IsNew = false, string nodeGUID = "")
        {
            if (IsNew)
            {
                TreeNode NewNode = new TreeNode();
                NewNode.Text = NodeName;
                NewNode.Tag = nodeGUID + "," + TreeLevel;
                NewNode.Name = NodeName;
                treeMain.SelectedNode.Nodes.Add(NewNode);
                treeMain.SelectedNode = NewNode;
            }
            else
            {
                treeMain.SelectedNode.Text = NodeName;
            }
            SetUPLV(TreeLevel);
        }

        /// <summary>
        /// refreshes contents of main listview given tree level
        /// </summary>
        /// <param name="TreeLevel"></param>
        public void RefreshLVEx(string TreeLevel)
        {
            SetUPLV(TreeLevel);
        }

        private void _mrulist_FileSelected(string filename)
        {
            _TreeLevel = "root";
            if (!SetupTree(filename, FromMRU: true))
            {
                _mrulist.RemoveFile(filename);
            }
        }

        private void _mrulist_ManageMRU(object sender, EventArgs e)
        {
            ManageMRUForm f = new ManageMRUForm();
            f.Parent_form = this;
            f.ShowDialog(this);
        }

        private void AppExit()
        {
            this.Close();
        }

        private void ApplyListViewColumnWidth(string TreeLevel)
        {
            //apply column widths saved in registry
            lvMain.Tag = TreeLevel;
            try
            {
                RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\FAD3\\ColWidth");
                string rv = rk.GetValue(lvMain.Tag.ToString(), "NULL").ToString();
                string[] arr = rv.Split(',');
                var i = 0;
                foreach (var item in lvMain.Columns)
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

        /// <summary>
        /// refreshes tree for any new sampling
        /// </summary>
        private void RefreshTreeForNewSampling()
        {
            if (_newSamplingEntered)
            {
                var LandingSiteNode = new TreeNode();

                var gearNode = new TreeNode();
                gearNode.Name = $"{_MonthYearLandingSite}|{_MonthYearGearVar}";
                gearNode.Text = gear.GearVarNameFromGearGuid(_MonthYearGearVar);
                gearNode.Tag = Tuple.Create(_MonthYearLandingSite, _MonthYearGearVar, "gear");
                gearNode.ImageKey = gear.GearVarNodeImageKeyFromGearVar(_MonthYearGearVar);

                var node = new TreeNode(_MonthYear);
                node.Name = $"{_MonthYearLandingSite}|{_MonthYearGearVar}|{_MonthYear}";
                node.Tag = Tuple.Create(_MonthYearLandingSite, _MonthYearGearVar, "sampling");
                node.ImageKey = "MonthGear";

                var myTag = (Tuple<string, string, string>)treeMain.SelectedNode.Tag;
                var treeLevel = myTag.Item3;

                if (!treeMain.SelectedNode.IsExpanded) treeMain.SelectedNode.Expand();
                switch (treeLevel)
                {
                    case "sampling":
                        LandingSiteNode = treeMain.SelectedNode.Parent.Parent;
                        break;

                    case "gear":
                        LandingSiteNode = treeMain.SelectedNode.Parent;
                        break;

                    case "landing_site":
                        LandingSiteNode = treeMain.SelectedNode;
                        break;
                }

                if (!LandingSiteNode.Nodes.ContainsKey(gearNode.Name))
                {
                    LandingSiteNode.Nodes.Add(gearNode);
                    gearNode.Nodes.Add(node);
                }
                else
                {
                    gearNode = LandingSiteNode.Nodes[gearNode.Name];
                    if (!gearNode.IsExpanded) gearNode.Expand();
                    if (!gearNode.Nodes.ContainsKey(node.Name))
                    {
                        gearNode.Nodes.Add(node);
                    }
                    else
                    {
                        node = gearNode.Nodes[node.Name];
                    }
                }
                treeMain.SelectedNode = node;
            }
        }

        /// <summary>
        /// adds to the tree
        /// </summary>
        private void BackToSamplingMonth()
        {
            SetupSamplingButtonFrame(false);
            SetUPLV(_TreeLevel);
            lvMain.Focus();
            var lvi = lvMain.Items[_SamplingGUID];
            if (lvMain.Items.Contains(lvi))
            {
                lvi.Selected = true;
                lvi.EnsureVisible();
                lvMain.TopItem = lvMain.Items[_topLVItemIndex];
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

            tsi = menuDropDown.Items.Add("Delete sampling");
            tsi.Name = "menuDeleteSampling";
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

                case "lvMain":
                    tsi = menuDropDown.Items.Add("New sampling");
                    tsi.Name = "menuNewSampling";
                    tsi.Visible = _TreeLevel == "sampling" || _TreeLevel == "landing_site" || _TreeLevel == "gear";

                    sep = menuDropDown.Items.Add("-");
                    sep.Name = "menuSeparator1";
                    sep.Visible = _TreeLevel == "landing_site";
                    break;

                case "lvCatch":
                    break;

                case "lvLF_GMS":
                    var MenuPrompt = "";
                    var MenuName = "";
                    if (((ListView)Source).Items.Count > 0)
                    {
                        if (_CatchSubRow == global.fad3CatchSubRow.GMS)
                        {
                            MenuPrompt = "Edit GMS table";
                            MenuName = "menuEditGMSTable";
                        }
                        else if (_CatchSubRow == global.fad3CatchSubRow.LF)
                        {
                            MenuPrompt = "Edit Length-frequency table";
                            MenuName = "menuEditLFTable";
                        }
                    }
                    else
                    {
                        if (_CatchSubRow == global.fad3CatchSubRow.GMS)
                        {
                            MenuPrompt = "New GMS table";
                            MenuName = "menuNewGMSTable";
                        }
                        else if (_CatchSubRow == global.fad3CatchSubRow.LF)
                        {
                            MenuPrompt = "New Length-frequency table";
                            MenuName = "menuNewLFTable";
                        }
                    }

                    tsi = menuDropDown.Items.Add(MenuPrompt);
                    tsi.Name = MenuName;
                    break;
            }
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string[] arr = new string[0];
            string[] arr1 = new string[0];

            ToolStripItem tsi = e.ClickedItem;
            switch (tsi.Name)
            {
                case "addAOIToolStripMenuItem":
                    TargetAreaForm f1 = new TargetAreaForm(this, IsNew: true);
                    f1.AddNew();
                    f1.Text = "New AOI";
                    f1.ShowDialog(this);
                    break;

                case "addLandingSiteToolStripMenuItem":
                    arr = treeMain.SelectedNode.Tag.ToString().Split(',');
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
                        MessageBox.Show(@"You need to add enumerators first
                                        before you can add your first sampling", "Missing enumerators",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private string DatabaseSummary(string SummaryTopic)
        {
            return "x";
        }

        /// <summary>
        /// Fills the listview with the samplings that took place in a month-year
        /// </summary>
        /// <param name="LSGUID"></param>
        /// <param name="GearGUID"></param>
        /// <param name="SamplingMonth"></param>
        private void FillLVSamplingSummary(string LSGUID, string GearGUID, string SamplingMonth)
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

                        string query = $@"SELECT tblSampling.RefNo, SamplingDate, FishingGround, EnumeratorName, Notes, WtCatch, tblSampling.SamplingGUID,
                                        IsGrid25FG, Count(tblCatchComp.RowGUID) AS [rows] FROM (tblEnumerators RIGHT JOIN tblSampling ON
                                        tblEnumerators.EnumeratorID = tblSampling.Enumerator) LEFT JOIN tblCatchComp ON
                                        tblSampling.SamplingGUID = tblCatchComp.SamplingGUID GROUP BY tblSampling.SamplingDate,
                                        tblSampling.RefNo, tblSampling.FishingGround, tblEnumerators.EnumeratorName, tblSampling.Notes, tblSampling.WtCatch,
                                        tblSampling.SamplingGUID, tblSampling.IsGrid25FG, tblSampling.LSGUID, tblSampling.[GearVarGUID], tblSampling.[SamplingDate],
                                        tblSampling.DateEncoded HAVING tblSampling.LSGUID= {{{LSGUID}}} AND tblSampling.[GearVarGUID]= {{{GearGUID}}} AND
                                        SamplingDate >=# {StartDate} # And SamplingDate < # {EndDate} #  ORDER BY DateEncoded";

                        using (var adapter = new OleDbDataAdapter(query, conection))
                        {
                            adapter.Fill(myDT);
                            ListViewItem lvi;
                            for (int i = 0; i < myDT.Rows.Count; i++)
                            {
                                DataRow dr = myDT.Rows[i];
                                ListViewItem row = new ListViewItem(dr["RefNo"].ToString());      //ref no
                                DateTime dt = (DateTime)dr["SamplingDate"];
                                row.SubItems.Add(string.Format("{0:MMM-dd-yyyy}", dt));     //sampling date
                                row.SubItems.Add(dr["rows"].ToString());                    //number of catch rows
                                row.SubItems.Add(dr["WtCatch"].ToString());                 //wt of catch
                                var fishingGround = dr["FishingGround"].ToString();         //fishing ground
                                row.SubItems.Add(fishingGround);

                                if (CompleteGrid25)                                         //position
                                {
                                    if (fishingGround.Length > 0)
                                    {
                                        row.SubItems.Add(FishingGrid.Grid25_to_UTM(dr[2].ToString()));
                                    }
                                    else
                                    {
                                        row.SubItems.Add("");
                                    }
                                }
                                else
                                    row.SubItems.Add("");

                                row.SubItems.Add(dr["EnumeratorName"].ToString());          //enumerator

                                //gear specs
                                row.SubItems.Add(ManageGearSpecsClass.SampledGearHasSpecs(dr[6].ToString()) ? "x" : "");                                       //gear specs

                                row.SubItems.Add(dr["Notes"].ToString());                   //notes

                                lvi = this.lvMain.Items.Add(row);
                                lvi.Tag = dr["SamplingGUID"].ToString();                    //sampling guid
                                lvi.Name = dr["SamplingGUID"].ToString();
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

        private void frmMain_Activated(object sender, EventArgs e)
        {
            CancelButton = buttonOK.Visible ? buttonOK : null;
        }

        private void frmMain_ResizeEnd(object sender, EventArgs e)
        {
            splitContainer1.Width = this.Width;
            lvMain.Width = splitContainer1.Panel2.Width;
            lvMain.Height = splitContainer1.Panel2.Height;
        }

        private void FrmMainLoad(object sender, EventArgs e)
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

                //setup the event handlers for the mru
                _mrulist.FileSelected += _mrulist_FileSelected;
                _mrulist.ManageMRU += _mrulist_ManageMRU;

                RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\FAD3");
                try
                {
                    var SavedMDBPath = rk.GetValue("mdbPath", "NULL").ToString();
                    if (SavedMDBPath != "NULL" && File.Exists(SavedMDBPath))
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
                        lblErrorFormOpen.Text = @"Please locate the database file where fisheries data is saved.
                                                 You can use the file open menu";
                        LockTheApp(true);
                    }
                }
                catch
                {
                    ErrorLogger.Log("Registry entry for mdb path not found");
                    lblErrorFormOpen.Visible = true;
                    lblTitle.Text = "";
                    lblErrorFormOpen.Text = @"Please locate the database file where fisheries data is saved.
                                             You can use the file open menu";
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
                       o.Text = $"The following files were not found: {global.MissingRequiredFiles}";
                       o.Top = lblErrorFormOpen.Top + lblErrorFormOpen.Height;
                       o.Left = lblErrorFormOpen.Left + (lblErrorFormOpen.Width / 2) - (o.Width / 2);
                   });

                LockTheApp();
            }

            statusPanelTargetArea.Width = _statusPanelWidth;
            statusPanelLandingSite.Width = _statusPanelWidth;
            statusPanelGearUsed.Width = _statusPanelWidth;

            ConfigDropDownMenu(treeMain);
            SetupSamplingButtonFrame(false);
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

        private ListView GetLVCatch()
        {
            if (_subListExisting)
                return (ListView)splitContainer1.Panel2.Controls["lvCatch"];
            else
                return null;
        }

        private ListView GetLVLF_GMS()
        {
            if (_subListExisting)
                return (ListView)splitContainer1.Panel2.Controls["lvLF_GMS"];
            else
                return null;
        }

        private void OnListViewLeave(object sender, EventArgs e)
        {
            if (lvMain.Columns.Count > 0)
                SaveColumnWidthEx(sender, lvMain.Tag.ToString());
        }

        /// <summary>
        /// shows the sampling detail listview but without any text on the sub-items
        /// </summary>
        private void ListViewNewSampling()
        {
            if (lvMain.Tag.ToString() == "samplingDetail")
            {
                ShowCatchDetailEx();
            }
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
            var ItemName = e.ClickedItem.Name;
            e.ClickedItem.Owner.Hide();
            switch (ItemName)
            {
                case "menuNewTargetArea":
                    TargetAreaForm tf = new TargetAreaForm(this, IsNew: true);
                    tf.ShowDialog(this);
                    break;

                case "menuNewLandingSite":
                    break;

                case "menuNewSampling":
                    if (FishingGrid.IsCompleteGrid25)
                    {
                        if (aoi.AOIHaveEnumeratorsEx(_AOIGuid))
                            NewSamplingForm();
                        else
                        {
                            MessageBox.Show("Cannot create a new sampling because target area has no enumerator",
                                              "Cannot create sampling",
                                              MessageBoxButtons.OK,
                                              MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Cannot create a new sampling because target area is not setup for Grid25",
                                          "Cannot create sampling",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Information);
                    }
                    break;

                case "menuNewEnumerator":
                    break;

                case "menuSamplingDetail":
                    ShowSamplingDetailForm();
                    break;

                case "menuTargetAreaProp":
                    TargetAreaForm AOIForm = TargetAreaForm.GetInstance(this, IsNew: false);
                    if (!AOIForm.Visible)
                    {
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

                case "menuEditLFTable":
                    ShowLFForm();
                    break;

                case "menuNewLFTable":
                    ShowLFForm(IsNew: true);
                    break;

                case "menuNewGMSTable":
                    ShowGMSForm(isNew: true);
                    break;

                case "menuEditGMSTable":
                    ShowGMSForm();
                    break;

                case "menuDeleteSampling":
                    DeleteSampling();
                    break;
            }
        }

        private void DeleteSampling()
        {
            DialogResult = MessageBox.Show("Please confirm deleting of sampling", "Confirmation",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (DialogResult == DialogResult.OK)
            {
                if (lvMain.Tag.ToString() == "sampling")
                {
                    if (sampling.DeleteSampling(SamplingGUID))
                    {
                        lvMain.Items.Remove(lvMain.Items[SamplingGUID]);
                        if (lvMain.Items.Count >= 1)
                        {
                            lvMain.Items[0].Selected = true;
                        }
                        else
                        {
                            var parentNode = treeMain.SelectedNode.Parent;
                            if (parentNode.Nodes.Count == 1)
                            {
                                parentNode.Parent.Nodes.Remove(parentNode);
                            }
                            else
                            {
                                treeMain.Nodes.Remove(treeMain.SelectedNode);
                                treeMain.SelectedNode = parentNode;
                            }
                        }
                    }
                }
                else if (lvMain.Tag.ToString() == "samplingDetail")
                {
                }
            }
        }

        private void ShowGMSForm(bool isNew = false)
        {
            var lvCatch = GetLVCatch();
            GMSDataEntryForm fgms = new GMSDataEntryForm(isNew, _Sampling,
                                      lvCatch.SelectedItems[0].Name,
                                      lvCatch.SelectedItems[0].SubItems[1].Text);
            fgms.ShowDialog(this);
        }

        private void NewSamplingForm()
        {
            ListViewNewSampling();
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
            f3.ListViewSamplingDetail(lvMain);
            f3.Parent_Form = this;
            f3.ShowDialog(this);
        }

        private void OnbuttonSamplingClick(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            switch (b.Name)
            {
                case "buttonOK":
                    SetupCatchListView(Show: false);
                    BackToSamplingMonth();
                    break;

                case "buttonCatch":
                    SetupCatchListView();
                    ShowCatchComposition(_SamplingGUID);
                    break;

                case "buttonMap":
                    break;

                case "btnSubClose":
                    var lvLF_GMS = GetLVLF_GMS();
                    if (lvLF_GMS.Visible)
                    {
                        lvLF_GMS.Visible = false;
                    }
                    else
                    {
                        SetupCatchListView(Show: false);
                    }
                    break;

                case "btnSubLF":
                    if (SetupLF_GMSListView(Show: true, Content: global.fad3CatchSubRow.LF))
                    {
                        _CatchSubRow = global.fad3CatchSubRow.LF;
                        Show_LF_GMS_List(GetLVCatch().SelectedItems[0].Name);
                    }
                    break;

                case "btnSubGMS":
                    if (SetupLF_GMSListView(Show: true, Content: global.fad3CatchSubRow.GMS))
                    {
                        _CatchSubRow = global.fad3CatchSubRow.GMS;
                        Show_LF_GMS_List(GetLVCatch().SelectedItems[0].Name);
                    }
                    break;
            }
        }

        private void OnListView_DoubleClick(object sender, EventArgs e)
        {
            switch (((ListView)sender).Name)
            {
                case "lvMain":
                    SetupCatchListView(Show: false);
                    ListViewItem lvi = new ListViewItem();
                    _topLVItemIndex = lvMain.TopItem.Index;
                    foreach (var item in lvMain.SelectedItems)
                    {
                        lvi = (ListViewItem)item;
                        string tag = lvMain.Tag.ToString();
                        switch (tag)
                        {
                            case "aoi":
                                if (lvi.Tag != null)
                                {
                                    if (lvi.Tag.ToString() == "aoi_data")
                                    {
                                        var myTag = (Tuple<string, string, string>)treeMain.SelectedNode.Tag;
                                        _AOI.AOIGUID = myTag.Item1;
                                        TargetAreaForm f = new TargetAreaForm(this, IsNew: false);
                                        f.Show();
                                        f.AOI = _AOI;
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

                                string[] arr1 = treeMain.SelectedNode.Tag.ToString().Split(',');
                                frmLandingSite fls = new frmLandingSite();
                                fls.LandingSite = _ls;
                                arr1 = treeMain.SelectedNode.Parent.Tag.ToString().Split(',');
                                fls.AOIGUID = arr1[0].ToString();
                                fls.Show();
                                break;

                            case "gear":
                                break;

                            case "sampling":
                                SetUPLV("samplingDetail");
                                SamplingGUID = lvi.Tag.ToString();
                                ShowCatchDetailEx(_SamplingGUID);
                                lvi.BackColor = Color.Gainsboro;
                                break;

                            case "samplingDetail":
                                if (lvi.Name == "GearSpecs")
                                {
                                    var s = ManageGearSpecsClass.GetSampledSpecsEx(_SamplingGUID);
                                    if (s.Length == 0)
                                        MessageBox.Show("Gear specs not found",
                                                        "Gear specifications",
                                                        MessageBoxButtons.OK,
                                                        MessageBoxIcon.Information);
                                }
                                else
                                    ShowSamplingDetailForm();
                                break;
                        }
                    }
                    break;

                case "lvCatch":

                    break;

                case "lvLF_GMS":
                    ShowLFForm();
                    break;
            }
        }

        private void OnListView_MouseDown(object sender, MouseEventArgs e)
        {
            ListView lv = (ListView)sender;
            _MouseX = e.Location.X;
            _MouseY = e.Location.Y;
            ListViewHitTestInfo lvh = lv.HitTest(_MouseX, _MouseY);
            switch (lv.Name)
            {
                case "lvMain":
                    if (lvMain.Tag.ToString() == "sampling" && lvh.Item != null) SamplingGUID = lvh.Item.Tag.ToString();
                    if (e.Button == MouseButtons.Right)
                    {
                        if (_TreeLevel == "sampling")
                        {
                            Text = "x: " + _MouseX + " y: " + _MouseY;
                            ConfigDropDownMenu(lvMain, lvh);
                        }
                        else
                        {
                            ConfigDropDownMenu(lvMain);
                        }
                    }
                    break;

                case "lvCatch":
                    if (e.Button == MouseButtons.Right)
                    {
                        ConfigDropDownMenu(GetLVCatch());
                    }
                    else
                    {
                        if (lvh.Item != null)
                        {
                            Show_LF_GMS_List(lvh.Item.Name);
                        }
                    }
                    break;

                case "lvLF_GMS":
                    if (e.Button == MouseButtons.Right)
                    {
                        Text = "x: " + _MouseX + " y: " + _MouseY;
                        ConfigDropDownMenu(lv);
                        menuDropDown.Show(lv, new Point(_MouseX, _MouseY));
                    }
                    else
                    {
                    }
                    break;
            }
        }

        private void OnMenuFile_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem tsi = e.ClickedItem;
            e.ClickedItem.Owner.Hide();
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

        private void OnMenuHelp_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem tsi = e.ClickedItem;
            e.ClickedItem.Owner.Hide();
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

        private void OnMenuTools_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem tsi = e.ClickedItem;
            e.ClickedItem.Owner.Hide();
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

        private void OnUIRowRead(object sender, UIRowFromXML e)
        {
            ListViewItem lvi = lvMain.Items.Add(e.RowLabel);
            lvi.Name = e.Key;
            lvi.SubItems.Add("");
        }

        private void PopulateTree()
        {
            using (var myDataTable = new DataTable())
            {
                try
                {
                    using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;data source=" + global.mdbPath))
                    {
                        conection.Open();

                        const string query =
                            @"SELECT tblAOI.AOIGuid, tblAOI.AOIName, tblLandingSites.LSGUID, tblLandingSites.LSName
                            FROM tblAOI LEFT JOIN tblLandingSites ON tblAOI.AOIGuid = tblLandingSites.AOIGuid
                            ORDER BY tblAOI.AOIName, tblLandingSites.LSName";

                        using (var adapter = new OleDbDataAdapter(query, conection))
                            adapter.Fill(myDataTable);
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }

                this.treeMain.Nodes.Clear();
                TreeNode root = this.treeMain.Nodes.Add("Fisheries Data");
                root.Name = "root";
                root.Tag = Tuple.Create("root", "", "root");
                root.ImageKey = "db";
                for (int i = 0; i < myDataTable.Rows.Count; i++)
                {
                    DataRow dr = myDataTable.Rows[i];
                    bool Exists = root.Nodes.ContainsKey(dr["AOIGuid"].ToString());
                    if (Exists)
                    {
                        TreeNode myNode = root.Nodes[dr["AOIGuid"].ToString()];
                        TreeNode myChild = new TreeNode(dr["LSName"].ToString());
                        myNode.Nodes.Add(myChild);
                        myChild.Name = dr["LSGUID"].ToString();
                        myChild.Nodes.Add("*dummy*");
                        myChild.Tag = Tuple.Create(dr["LSGUID"].ToString(), "", "landing_site");
                        myChild.ImageKey = "LandingSite";
                    }
                    else
                    {
                        TreeNode myNode = new TreeNode(dr["AOIName"].ToString());
                        myNode.Name = dr["AOIGuid"].ToString();
                        root.Nodes.Add(myNode);
                        //myNode.Tag = Tuple.Create(dr["AOIGuid"].ToString(), "", "aoi");
                        myNode.Tag = Tuple.Create(myNode.Name, "", "aoi");
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
                            myChild.Tag = Tuple.Create(dr["LSGUID"].ToString(), "", "landing_site");
                            myChild.Name = dr["LSGUID"].ToString();
                            myChild.ImageKey = "LandingSite";
                        }
                    }
                }

                root.Expand();
                SetUPLV("root");
            }
        }

        private void ProcessFileOpen()
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

        private void ResetTheBackColor(Control c)
        {
            switch (c.Name)
            {
                case "lvMain":
                    foreach (ListViewItem item in ((ListView)c).Items)
                    {
                        item.BackColor = Color.White;
                    }
                    break;

                case "treeView1":
                    TreeNodeCollection nodes = treeMain.Nodes;
                    TraverseTreeAndResetColor(treeMain.Nodes);
                    break;
            }
        }

        private void SetupCatchListView(bool Show = true)
        {
            if (Show)
            {
                if (_subListExisting)
                {
                    splitContainer1.Panel2.Controls["panelSub"].Visible = true;
                    splitContainer1.Panel2.Controls["lvCatch"].Visible = true;
                }
                else
                {
                    _subListExisting = true;
                    ListView lvCatch = new ListView();
                    splitContainer1.Panel2.Controls.Add(lvCatch);
                    lvCatch.With(o =>
                    {
                        o.Name = "lvCatch";
                        o.Font = lvMain.Font;
                        o.Width = lvMain.Width;
                        o.Height = lvMain.Height / 2;
                        o.Location = new Point(lvMain.Location.X, lvMain.Top + lvMain.Height / 2);
                        o.View = View.Details;
                        o.FullRowSelect = true;
                        o.Columns.Add("Row", "Row");
                        o.Columns.Add("NameOfCatch", "Name of catch");
                        o.Columns.Add("Weight", "Weight");
                        o.Columns.Add("Count", "Count");
                        o.Columns.Add("SubWt", "Subsample weight");
                        o.Columns.Add("SubCt", "Subsample count");
                        o.Columns.Add("FromTotal", "From total");
                        o.Columns.Add("CompWt", "Computed weight");
                        o.Columns.Add("CompCt", "Computed count");
                        o.Columns.Add("spacer", "");
                        o.BringToFront();
                        o.DoubleClick += OnListView_DoubleClick;
                        o.MouseDown += OnListView_MouseDown;
                        o.ContextMenu = lvMain.ContextMenu;
                        o.HideSelection = false;
                    });

                    ListView lvLF_GMS = new ListView();
                    splitContainer1.Panel2.Controls.Add(lvLF_GMS);
                    lvLF_GMS.With(o =>
                    {
                        o.Name = "lvLF_GMS";
                        o.Font = lvCatch.Font;
                        o.Height = lvCatch.Height;
                        o.View = View.Details;
                        o.Top = lvCatch.Top;
                        o.BringToFront();
                        o.FullRowSelect = true;
                        o.Visible = false;
                        o.DoubleClick += OnListView_DoubleClick;
                        o.MouseDown += OnListView_MouseDown;
                        o.ContextMenu = lvMain.ContextMenu;
                        o.HideSelection = false;
                    });

                    Panel SubPanel = new Panel
                    {
                        Height = panelSamplingButtons.Height,
                        Width = panelSamplingButtons.Width,
                        BackColor = panelSamplingButtons.BackColor,
                        Name = "panelSub"
                    };

                    Button btnSubClose = new Button
                    {
                        Name = "btnSubClose",
                        Text = "Close",
                        Font = lvCatch.Font,
                        Height = ((Button)panelSamplingButtons.Controls["buttonOK"]).Height,
                        Width = ((Button)panelSamplingButtons.Controls["buttonOK"]).Width,
                        Left = ((Button)panelSamplingButtons.Controls["buttonOK"]).Left,
                        FlatStyle = FlatStyle.Standard,
                        UseVisualStyleBackColor = true,
                    };

                    Button btnSubLF = new Button
                    {
                        Name = "btnSubLF",
                        Text = "LF",
                        Font = lvCatch.Font,
                        Height = btnSubClose.Height,
                        Width = btnSubClose.Width,
                        Left = btnSubClose.Left,
                        FlatStyle = FlatStyle.Standard,
                        UseVisualStyleBackColor = true
                    };

                    Button btnSubGMS = new Button
                    {
                        Name = "btnSubGMS",
                        Text = "GMS",
                        Font = lvCatch.Font,
                        Height = btnSubClose.Height,
                        Width = btnSubClose.Width,
                        Left = btnSubClose.Left,
                        FlatStyle = FlatStyle.Standard,
                        UseVisualStyleBackColor = true
                    };

                    btnSubClose.Click += OnbuttonSamplingClick;
                    btnSubLF.Click += OnbuttonSamplingClick;
                    btnSubGMS.Click += OnbuttonSamplingClick;

                    SubPanel.Controls.Add(btnSubClose);
                    SubPanel.Controls.Add(btnSubLF);
                    SubPanel.Controls.Add(btnSubGMS);

                    splitContainer1.Panel2.Controls.Add(SubPanel);
                    SubPanel.Location = new Point(panelSamplingButtons.Location.X,
                                                   lvCatch.Top - (lvMain.Top - panelSamplingButtons.Top));
                    btnSubLF.Top = btnSubClose.Top + btnSubClose.Height + 5;
                    btnSubGMS.Top = btnSubLF.Top + btnSubLF.Height + 5;
                    SubPanel.BringToFront();
                }
            }
            else
            {
                if (_subListExisting)
                {
                    splitContainer1.Panel2.Controls["panelSub"].Visible = false;
                    ((ListView)splitContainer1.Panel2.Controls["lvCatch"]).With(o =>
                    {
                        o.Items.Clear();
                        o.Visible = false;
                    });

                    ((ListView)splitContainer1.Panel2.Controls["lvLF_GMS"]).With(o =>
                    {
                        o.Items.Clear();
                        o.Visible = false;
                    });
                }
            }
        }

        private bool SetupLF_GMSListView(bool Show = true, global.fad3CatchSubRow Content = global.fad3CatchSubRow.none)
        {
            var Proceed = false;
            if (_subListExisting)
            {
                var lv = (ListView)splitContainer1.Panel2.Controls["lvLF_GMS"];
                var lvCatch = (ListView)splitContainer1.Panel2.Controls["lvCatch"];

                lv.Items.Clear();
                lv.Columns.Clear();
                {
                    if (Show && lvCatch.Items.Count > 0)
                    {
                        lv.Visible = true;
                        lv.Left = lvCatch.Columns["NameOfCatch"].Width + lvCatch.Columns["Row"].Width +
                                  lvCatch.Columns["Weight"].Width + lvCatch.Columns["Count"].Width;

                        lv.Width = lvCatch.Columns["SubWt"].Width + lvCatch.Columns["SubCt"].Width +
                                   lvCatch.Columns["FromTotal"].Width + lvCatch.Columns["CompWt"].Width +
                                   lvCatch.Columns["CompCt"].Width + lvCatch.Columns["spacer"].Width + 3;

                        if (Content == global.fad3CatchSubRow.LF)
                        {
                            lv.Columns.Add("Row");
                            lv.Columns.Add("Length");
                            lv.Columns.Add("Frequency");
                            lv.Columns.Add("");
                        }
                        else if (Content == global.fad3CatchSubRow.GMS)
                        {
                            lv.Columns.Add("Row");
                            lv.Columns.Add("Length");
                            lv.Columns.Add("Weight");
                            lv.Columns.Add("Sex");
                            lv.Columns.Add("GMS");
                            lv.Columns.Add("Gonad wt");
                            lv.Columns.Add("");
                        }
                        Proceed = true;
                    }
                    else
                    {
                        lv.Visible = false;
                        MessageBox.Show("Catch composition is empty", "Validation error",
                                         MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }

            return Proceed;
        }

        /// <summary>
        /// this will show a list of summaries depending on the level of the node selected
        /// level could be root, AOI, Landing site, gear used, and sampling month
        /// </summary>
        /// <param name="TreeLevel"></param>
        private void SetUPLV(string TreeLevel)
        {
            lvMain.SuspendLayout();

            int i = 0;

            lvMain.Clear();
            lvMain.View = View.Details;
            lvMain.FullRowSelect = true;

            string myData = "";
            ListViewItem lvi;

            //setup columns in the listview
            lvMain.Columns.Add("Property");
            lvMain.Columns.Add("Value");
            switch (TreeLevel)
            {
                case "root":
                    lblTitle.Text = "Database summary";
                    break;

                case "aoi":
                    lblTitle.Text = "Area of interest";
                    break;

                case "landing_site":
                    lblTitle.Text = "Landing site";
                    break;

                case "gear":
                    lblTitle.Text = "Fishing gear";
                    break;

                case "sampling":
                    lvMain.Columns.Clear();
                    lvMain.Columns.Add("Reference #");
                    lvMain.Columns.Add("Sampling date");
                    lvMain.Columns.Add("Catch composition");
                    lvMain.Columns.Add("Weight of catch");
                    lvMain.Columns.Add("Fishing ground");
                    lvMain.Columns.Add("Position");
                    lvMain.Columns.Add("Enumerator");
                    lvMain.Columns.Add("Gear specs");
                    lvMain.Columns.Add("Notes");
                    lblTitle.Text = "Sampling";
                    break;

                case "samplingDetail":
                    lblTitle.Text = "Sampling detail";
                    break;
            }

            //apply column widths saved in registry
            ApplyListViewColumnWidth(TreeLevel);

            //add rows to the listview
            switch (_TreeLevel)
            {
                case "sampling":
                    FillLVSamplingSummary(_LandingSiteGuid, _GearVarGUID, _SamplingMonth);
                    break;

                case "samplingDetail":
                    break;

                case "gear":
                    lvi = lvMain.Items.Add("Months sampled");
                    var myTag = (Tuple<string, string, string>)treeMain.SelectedNode.Tag;
                    _ls.LandingSiteGUID = myTag.Item1;
                    _ls.GearVarGUID = myTag.Item2;
                    var n = 0;
                    foreach (string item in _ls.MonthsSampledEx(_ls.GearVarGUID))
                    {
                        if (n > 0)
                        {
                            lvi = lvMain.Items.Add("");
                        }
                        lvi.SubItems.Add(item);
                        n++;
                    }
                    break;

                case "root":
                    lvi = lvMain.Items.Add("Database path");
                    lvi.SubItems.Add(global.mdbPath);

                    //add sampled years with count for entire database
                    lvi = lvMain.Items.Add("");
                    lvi = lvMain.Items.Add("Sampled years");
                    foreach (string item in _AOI.SampledYears())
                    {
                        if (i > 0)
                        {
                            lvi = lvMain.Items.Add("");
                        }
                        lvi.SubItems.Add(item.ToString());
                        i++;
                    }

                    //add AOI with count for entire database
                    lvi = lvMain.Items.Add("");
                    lvi = lvMain.Items.Add("AOI");
                    i = 0;
                    foreach (KeyValuePair<string, string> kv in _AOI.AOIWithSamplingCount())
                    {
                        if (i > 0)
                        {
                            lvi = lvMain.Items.Add("");
                        }
                        lvi.SubItems.Add(kv.Value);
                        i++;
                    }
                    break;

                case "aoi":
                    //arr = treeView1.SelectedNode.Tag.ToString().Split(',');
                    myData = _AOI.AOIData();
                    var arr = myData.Split('|');

                    //add AOI data for form
                    lvi = lvMain.Items.Add("Name");
                    lvi.SubItems.Add(arr[0].ToString());
                    lvi.Tag = "aoi_data";
                    lvi = lvMain.Items.Add("Code");
                    lvi.SubItems.Add(arr[1].ToString());
                    lvi.Tag = "aoi_data";
                    lvi = lvMain.Items.Add("MBR");
                    lvi.SubItems.Add("");
                    lvi.Tag = "aoi_data";
                    lvi = lvMain.Items.Add("Grid system");
                    lvi.Tag = "aoi_data";
                    lvi.SubItems.Add(FishingGrid.GridTypeName);

                    // add no. of samplings for this AOI
                    lvi = lvMain.Items.Add("");
                    lvi = lvMain.Items.Add("No. of samplings");
                    lvi.SubItems.Add(_AOI.SampleCount().ToString());

                    //add sampled years with count
                    lvi = lvMain.Items.Add("Years sampling");
                    i = 0;
                    foreach (KeyValuePair<string, string> item in _AOI.ListYearsWithSamplingCount())
                    {
                        if (i > 0)
                        {
                            lvi = lvMain.Items.Add("");
                        }
                        lvi.Name = "YearSampled";
                        lvi.SubItems.Add(item.Key + " : " + item.Value);
                        i++;
                    }

                    //add enumerators with count
                    lvi = lvMain.Items.Add("");
                    lvi = lvMain.Items.Add("Enumerators");
                    i = 0;
                    foreach (KeyValuePair<string, string> item in _AOI.EnumeratorsWithCount())
                    {
                        if (i > 0)
                        {
                            lvi = lvMain.Items.Add("");
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
                                lvi = lvMain.Items.Add("");
                            }
                            lvi.Name = "Enumerators";
                            lvi.SubItems.Add(item.Value);
                            lvi.Tag = item.Key;
                            i++;
                        }
                    }

                    //add landing sites
                    lvMain.Items.Add("");
                    lvi = lvMain.Items.Add("Landing sites");
                    i = 0;
                    foreach (string item in _AOI.ListLandingSiteWithSamplingCount())
                    {
                        if (i > 0)
                        {
                            lvi = lvMain.Items.Add("");
                        }
                        lvi.SubItems.Add(item);
                        i++;
                    }
                    break;

                case "landing_site":

                    //add landing site form data
                    _ls.LandingSiteGUID = _LandingSiteGuid;
                    myData = _ls.LandingSiteData();
                    arr = myData.Split(',');
                    lvi = lvMain.Items.Add("Name");
                    lvi.SubItems.Add(arr[0]);
                    lvi = lvMain.Items.Add("Municipality");
                    lvi.SubItems.Add(arr[1]);
                    lvi = lvMain.Items.Add("Province");
                    lvi.SubItems.Add(arr[2]);
                    lvi = lvMain.Items.Add("Longitude");
                    lvi.SubItems.Add(arr[3]);
                    lvi = lvMain.Items.Add("Latitude");
                    lvi.SubItems.Add(arr[4]);

                    //add total number of sampling
                    lvMain.Items.Add("");
                    lvi = lvMain.Items.Add("No. of samplings");
                    lvi.SubItems.Add(_ls.SampleCount().ToString());
                    lvMain.Items.Add("");

                    //add gears sampled
                    Dictionary<string, string> myGears = new Dictionary<string, string>();
                    myGears = _ls.Gears();
                    lvi = lvMain.Items.Add("Gears sampled");
                    n = 0;
                    foreach (KeyValuePair<string, string> item in myGears)
                    {
                        if (n > 0)
                        {
                            lvi = lvMain.Items.Add("");
                        }
                        lvi.Tag = item.Key;
                        lvi.SubItems.Add(item.Value);
                        n++;
                    }

                    //Add months sampled
                    lvMain.Items.Add("");
                    lvi = lvMain.Items.Add("Months sampled");
                    List<string> myMonths = _ls.MonthsSampled();
                    n = 0;
                    foreach (string item in myMonths)
                    {
                        if (n > 0)
                        {
                            lvi = lvMain.Items.Add("");
                        }
                        lvi.SubItems.Add(item);
                        n++;
                    }
                    break;
            }

            lvMain.ResumeLayout();
        }

        public void NewTargetArea(string TargetAreaName, string TargetAreaGuid)
        {
            TreeNode nd = new TreeNode
            {
                Text = TargetAreaName,
                Name = TargetAreaGuid,
                ImageKey = "AOI"
            };
            nd.Tag = Tuple.Create(TargetAreaGuid, "", "aoi");
            treeMain.Nodes["root"].Nodes.Add(nd);
            treeMain.SelectedNode = nd;
        }

        /// <summary>
        /// positions, shows or hides the buttons of a sampled fishing effort
        /// </summary>
        /// <param name="Visible"></param>
        private void SetupSamplingButtonFrame(bool Visible)
        {
            panelSamplingButtons.Visible = Visible;
            if (Visible)
            {
                this.panelSamplingButtons.Location = new Point(lvMain.Width - panelSamplingButtons.Width - 10, lvMain.Top + 50);
            }
            CancelButton = buttonOK.Visible ? buttonOK : null;
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

        private void Show_LF_GMS_List(string CatchRowGuid)
        {
            var lvc = GetLVLF_GMS();
            lvc.Items.Clear();
            int n = 1;
            if (_CatchSubRow == global.fad3CatchSubRow.LF)
            {
                foreach (KeyValuePair<string, sampling.LFLine> kv in _Sampling.LFData(CatchRowGuid))
                {
                    var lvi = new ListViewItem(new string[]
                    {
                                    n.ToString(),
                                    kv.Value.Length.ToString(),
                                    kv.Value.Freq.ToString()
                    });
                    lvi.Name = kv.Key;
                    lvc.Items.Add(lvi);
                    n++;
                }
            }
            else if (_CatchSubRow == global.fad3CatchSubRow.GMS)
            {
                foreach (KeyValuePair<string, sampling.GMSLine> kv in _Sampling.GMSData(CatchRowGuid))
                {
                    var lvi = new ListViewItem(new string[]
                    {
                                    n.ToString(),
                                    kv.Value.Length.ToString(),
                                    kv.Value.Weight.ToString(),
                                    kv.Value.Sex.ToString(),
                                    kv.Value.GMS.ToString(),
                                    kv.Value.GonadWeight.ToString()
                    });
                    lvi.Name = kv.Key;
                    lvc.Items.Add(lvi);
                    n++;
                }
            }

            foreach (ColumnHeader c in lvc.Columns)
            {
                c.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }

        private void ShowCatchComposition(string SamplingGuid)
        {
            if (_subListExisting)
            {
                ListView lv = GetLVCatch();
                lv.Items.Clear();
                int n = 1;
                foreach (KeyValuePair<string, sampling.CatchLine> kv in _Sampling.CatchComp())
                {
                    var lvi = new ListViewItem(new string[]
                    {
                        n.ToString(),
                        kv.Value.CatchName,
                        kv.Value.CatchWeight.ToString(),
                        kv.Value.CatchCount.ToString(),
                        kv.Value.CatchSubsampleWt.ToString(),
                        kv.Value.CatchSubsampleCount.ToString(),
                        kv.Value.FromTotalCatch.ToString()
                    });
                    lvi.Name = kv.Key;
                    lv.Items.Add(lvi);
                    n++;
                }

                if (lv.Items.Count > 0)
                    lv.Items[0].Selected = true;

                foreach (ColumnHeader c in lv.Columns)
                {
                    switch (c.Text)
                    {
                        case "Row":
                        case "Weight":
                        case "Count":
                        case "Subsample weight":
                        case "Subsample count":
                        case "From total":
                        case "Computed weight":
                        case "Computed count":
                            c.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                            break;

                        case "Name of catch":
                            if (lv.Items.Count > 0)
                                c.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                            else
                                c.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// fills the listview with the complete effort data from a fish landing sampling
        /// </summary>
        /// <param name="SamplingGUID"></param>
        private void ShowCatchDetailEx(string SamplingGUID = "")
        {
            lvMain.Items.Clear();
            _Sampling.ReadUIFromXML();
            var DateEncoded = "";

            if (SamplingGUID.Length > 0)
            {
                //we fill up the list view from the _Sampling class variable.
                _Sampling.SamplingGUID = SamplingGUID;
                Dictionary<string, string> effortData = _Sampling.CatchAndEffort();

                //the array splits the dictionary item from the name [0] and its guid [1]
                //we make the guid the tag of the listitem
                string[] arr1 = effortData["LandingSite"].Split('|');
                lvMain.Items["LandingSite"].SubItems[1].Text = arr1[0];
                lvMain.Items["LandingSite"].Tag = arr1[1];

                arr1 = effortData["Enumerator"].Split('|');
                lvMain.Items["Enumerator"].SubItems[1].Text = arr1[0];
                lvMain.Items["Enumerator"].Tag = arr1[1];

                arr1 = effortData["GearClass"].Split('|');
                lvMain.Items["GearClass"].SubItems[1].Text = arr1[0];
                lvMain.Items["GearClass"].Tag = arr1[1];

                arr1 = effortData["FishingGear"].Split('|');
                lvMain.Items["FishingGear"].SubItems[1].Text = arr1[0];
                lvMain.Items["FishingGear"].Tag = arr1[1];

                foreach (ListViewItem lvi in lvMain.Items)
                {
                    switch (lvi.Name)
                    {
                        case "LandingSite":
                        case "Enumerator":
                        case "GearClass":
                        case "FishingGear":
                            break;

                        case "GearSpecs":
                            lvi.SubItems[1].Text = ManageGearSpecsClass.GetSampledSpecsEx(_SamplingGUID, Truncated: true);
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

                DateEncoded = effortData["DateEncoded"];
            }

            var lvi1 = lvMain.Items.Add("");
            lvi1 = lvMain.Items.Add("DateEncoded", "Date encoded", null);
            if (SamplingGUID.Length > 0) lvi1.SubItems.Add(DateEncoded);

            //position sampling buttons and make it visible
            SetupSamplingButtonFrame(true);
        }

        private void ShowLFForm(bool IsNew = false)
        {
            var lvCatch = GetLVCatch();
            LengthFreqForm lff;

            lff = new LengthFreqForm(IsNew, _Sampling,
                                      lvCatch.SelectedItems[0].Name,
                                      lvCatch.SelectedItems[0].SubItems[1].Text);

            lff.ShowDialog(this);
            Show_LF_GMS_List(lvCatch.SelectedItems[0].Name);
        }

        private void ShowSamplingDetailForm()
        {
            frmSamplingDetail fs = new frmSamplingDetail();
            fs.SamplingGUID = _SamplingGUID;
            fs.ListViewSamplingDetail(lvMain);
            fs.AOI = _AOI;
            fs.AOIGuid = _AOIGuid;
            fs.Parent_Form = this;
            fs.VesselDimension(_VesLength, _VesWidth, _VesHeight);
            fs.Show(this);
        }

        private void statusPanelDBPath_DoubleClick(object sender, EventArgs e)
        {
            Process.Start(Path.GetDirectoryName(global.mdbPath));
        }

        private void TraverseTreeAndResetColor(TreeNodeCollection nodes)
        {
            foreach (TreeNode child in nodes)
            {
                child.BackColor = Color.White;
                TraverseTreeAndResetColor(child.Nodes);
            }
        }

        private void OntreeMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                ConfigDropDownMenu(treeMain);

            SetupCatchListView(Show: false);
        }

        /// <summary>
        /// this will fill the tree with the nodes below the level of Landing site
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OntreeMainAfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node.FirstNode.Text == "*dummy*")
            {
                // we are in a landing site node then we will add gear used nodes
                var myDT = new DataTable();
                try
                {
                    using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;data source=" + global.mdbPath))
                    {
                        conection.Open();
                        string lsguid = ((Tuple<string, string, string>)e.Node.Tag).Item1;

                        var query = $@"SELECT DISTINCT tblGearClass.GearClassName, tblGearVariations.Variation, tblSampling.LSGUID,
                                     tblGearVariations.GearVarGUID FROM tblGearClass INNER JOIN
                                     (tblGearVariations INNER JOIN tblSampling ON tblGearVariations.GearVarGUID = tblSampling.GearVarGUID)
                                     ON tblGearClass.GearClass = tblGearVariations.GearClass
                                     WHERE tblSampling.LSGUID = {{{lsguid}}}
                                     ORDER BY tblGearClass.GearClassName, tblGearVariations.Variation";

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
                                nd1.Tag = Tuple.Create(dr["LSGUID"].ToString(), dr["GearVarGUID"].ToString(), "gear");
                                nd1.Name = dr["LSGUID"].ToString() + "|" + dr["GearVarGUID"].ToString();
                                nd1.ImageKey = gear.GearClassImageKeyFromGearClasName(dr["GearClassName"].ToString());
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
                //we are in a gear variation node and then add month-year sampling nodes

                var myDT = new DataTable();
                try
                {
                    using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;data source=" + global.mdbPath))
                    {
                        conection.Open();
                        var myTag = (Tuple<string, string, string>)e.Node.Tag;
                        var lsguid = myTag.Item1;
                        var gearguid = myTag.Item2;
                        var query = $@"SELECT Format([SamplingDate],'mmm-yyyy') AS sDate FROM tblSampling
                                    GROUP BY Format([SamplingDate],'mmm-yyyy'), tblSampling.LSGUID, tblSampling.GearVarGUID,
                                    Year([SamplingDate]), Month([SamplingDate])
                                    HAVING LSGUID ={{{lsguid}}} AND GearVarGUID = {{{gearguid}}}
                                    ORDER BY Year([SamplingDate]), Month([SamplingDate])";

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
                            nd1.Tag = Tuple.Create(lsguid, gearguid, "sampling");
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

        private void OnTreeMainAfterSelect(object sender, TreeViewEventArgs e)
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

                TreeNode nd = treeMain.SelectedNode;
                ResetTheBackColor(treeMain);
                nd.BackColor = Color.Gainsboro;

                var myTag = (Tuple<string, string, string>)nd.Tag;
                _TreeLevel = myTag.Item3;

                statusPanelTargetArea.Width = _statusPanelWidth;
                switch (_TreeLevel)
                {
                    case "gear":
                        _AOIName = treeMain.SelectedNode.Parent.Parent.Text;
                        this.AOIGUID = treeMain.SelectedNode.Parent.Parent.Name;
                        _LandingSiteName = treeMain.SelectedNode.Parent.Text;
                        _LandingSiteGuid = treeMain.SelectedNode.Parent.Name;
                        _GearVarName = treeMain.SelectedNode.Text;
                        _GearVarGUID = myTag.Item2;
                        var rv = gear.GearClassGuidNameFromGearVarGuid(_GearVarGUID);
                        _GearClassName = rv.Value;
                        _GearClassGUID = rv.Key;
                        _LSNode = e.Node.Parent;
                        break;

                    case "sampling":
                        _LandingSiteGuid = myTag.Item1;
                        _GearVarGUID = myTag.Item2;

                        //we will look for the aoi ancestor node and get the aoi guid
                        this.AOIGUID = ((Tuple<string, string, string>)treeMain.SelectedNode.Parent.Parent.Parent.Tag).Item1;
                        _AOI.AOIGUID = this.AOIGUID;

                        _SamplingMonth = e.Node.Text;
                        _AOIName = treeMain.SelectedNode.Parent.Parent.Parent.Text;
                        _LandingSiteName = treeMain.SelectedNode.Parent.Parent.Text;
                        _GearVarName = treeMain.SelectedNode.Parent.Text;

                        rv = gear.GearClassGuidNameFromGearVarGuid(_GearVarGUID);
                        _GearClassName = rv.Value;
                        _GearClassGUID = rv.Key;
                        _LSNode = e.Node.Parent.Parent;
                        break;

                    case "aoi":
                        _AOIName = treeMain.SelectedNode.Text;
                        this.AOIGUID = treeMain.SelectedNode.Name;
                        _LandingSiteName = "";
                        _LandingSiteGuid = "";
                        _GearVarName = "";
                        _GearVarGUID = "";
                        break;

                    case "root":
                        break;

                    case "landing_site":
                        _AOIName = treeMain.SelectedNode.Parent.Text;
                        this.AOIGUID = treeMain.SelectedNode.Parent.Name;
                        _LandingSiteName = treeMain.SelectedNode.Text;
                        _LandingSiteGuid = treeMain.SelectedNode.Name;
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
            catch (Exception ex)
            {
                ErrorLogger.Log(ex);
            }

            e.Node.SelectedImageKey = e.Node.ImageKey;
        }
    }
}
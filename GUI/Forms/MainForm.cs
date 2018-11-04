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
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FAD3.Database.Forms;
using FAD3.Database.Classes;
using FAD3.GUI.Classes;

//using dao;

namespace FAD3
{
    /// <summary>
    /// Description of frmMain.
    /// </summary>
    public partial class MainForm : Form
    {
        private MapEffortHelperForm _formEffortMapper;
        private aoi _aoi = new aoi();
        private string _aoiGuid = "";
        private string _aoiName = "";
        private fad3CatchSubRow _catchSubRow;
        private string _gearClassGUID = "";
        private string _gearClassName = "";
        private string _GearVarGUID = "";
        private string _GearVarName = "";
        private string _LandingSiteGuid = "";
        private string _LandingSiteName = "";
        private Landingsite _ls = new Landingsite();
        private TreeNode _LSNode;
        private int _mouseX = 0;
        private int _mouseY = 0;
        private mru _mrulist = new mru();
        private bool _newSamplingEntered;
        private string _oldMDB = "";
        private sampling _Sampling = new sampling();
        private string _SamplingGUID = "";
        private string _SamplingMonth = "";
        private double _weightOfCatch;
        private double? _weightOfSample;
        private int _statusPanelWidth = 200;
        private bool _subListExisting = false;
        private int _topLVItemIndex = 0;
        private string _treeLevel = "";
        private string _vesHeight = "";
        private string _vesLength = "";
        private string _vesWidth = "";
        private Taxa _taxa = Taxa.To_be_determined;
        private string _referenceNumber = "";

        private ListView lvCatch;
        private ListView lvLF_GMS;

        //for column sort
        private int _sortColumn = -1;
        private TreeNode _nodeParent;

        public MainForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }

        public string GearVariationGUID
        {
            get { return _GearVarGUID; }
        }

        public Landingsite LandingSite
        {
            get { return _ls; }
        }

        public string SamplingMonth
        {
            get { return _SamplingMonth; }
        }

        public string GearVariationName
        {
            get { return _GearVarName; }
        }

        public string LandingSiteName
        {
            get { return _LandingSiteName; }
        }

        public string LandingSiteGUID
        {
            get { return _LandingSiteGuid; }
        }

        public ImageList treeImages
        {
            get { return imageList16; }
        }

        public aoi AOI
        {
            get { return _aoi; }
        }

        public string AOIGUID
        {
            get { return _aoiGuid; }
            set
            {
                if (value != _aoiGuid)
                {
                    _aoiGuid = value;
                    _aoi.AOIGUID = _aoiGuid;
                    FishingGrid.AOIGuid = _aoiGuid;
                    Enumerators.AOIGuid = _aoiGuid;
                }
            }
        }

        public string AOIName
        {
            get { return _aoiName; }
            set
            {
                _aoiName = value;
                _aoi.AOIName = _aoiName;
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
        public static void SaveColumnWidthEx(object sender, string context = "", lvContext myContext = lvContext.None)
        {
            string cw = "";
            int i = 0;
            string SaveContext = "";
            var subKey = "SOFTWARE\\FAD3\\ColWidth";
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
            else if (myContext != lvContext.None)
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

        // Implements the manual sorting of items by columns.
        private class ListViewItemComparer : IComparer
        {
            private int col;
            private SortOrder order;

            public ListViewItemComparer()
            {
                col = 0;
                order = SortOrder.Ascending;
            }

            public ListViewItemComparer(int column, SortOrder order)
            {
                col = column;
                this.order = order;
            }

            public int Compare(object x, object y)
            {
                int returnVal = 0;

                // Determine whether the type being compared is a date type.
                try
                {
                    // Parse the two objects passed as a parameter as a DateTime.
                    DateTime firstDate = DateTime.Parse(((ListViewItem)x).SubItems[col].Text);
                    DateTime secondDate = DateTime.Parse(((ListViewItem)y).SubItems[col].Text);
                    // Compare the two dates.
                    returnVal = DateTime.Compare(firstDate, secondDate);
                }
                // If neither compared object has a valid date format, compare
                // first as a number, if not then compare as a string.
                catch
                {
                    try
                    {
                        //returnVal = 0;
                        //test if we can compare as numeric values
                        double firstValue = double.Parse(((ListViewItem)x).SubItems[col].Text);
                        double secondValue = double.Parse(((ListViewItem)y).SubItems[col].Text);

                        returnVal = firstValue.CompareTo(secondValue);
                    }
                    catch
                    {
                        // Compare the two items as a string.
                        returnVal = String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
                    }
                }
                // Determine whether the sort order is descending.
                if (order == SortOrder.Descending)
                {
                    // Invert the value returned by String.Compare.
                    returnVal *= -1;
                }
                return returnVal;
            }
        }

        private void OnlistView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            //var currentListView = (ListView)sender;

            //// Determine whether the column is the same as the last column clicked.
            //if (e.Column != _sortColumn)
            //{
            //    // Set the sort column to the new column.
            //    _sortColumn = e.Column;
            //    // Set the sort order to ascending by default.
            //    currentListView.Sorting = SortOrder.Ascending;
            //}
            //else
            //{
            //    // Determine what the last sort order was and change it.
            //    if (currentListView.Sorting == SortOrder.Ascending)
            //        currentListView.Sorting = SortOrder.Descending;
            //    else
            //        currentListView.Sorting = SortOrder.Ascending;
            //}

            //// Call the sort method to manually sort.
            ////currentListView.Sort();
            //// Set the ListViewItemSorter property to a new ListViewItemComparer object.
            //currentListView.ListViewItemSorter = new ListViewItemComparer(e.Column, currentListView.Sorting);
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

            //_Sampling.SamplingGUID = SamplingIdentifiers["SamplingID"];
            SamplingGUID = SamplingIdentifiers["SamplingID"];
            if (lvMain.Items.ContainsKey(SamplingIdentifiers["SamplingID"]))
            {
                lvMain.Focus();
                SetUPLV("samplingDetail");
                ShowCatchDetailEx(_SamplingGUID);
                //lvMain.Items[SamplingIdentifiers["SamplingID"]].Selected = true;
                //lvMain.Items[SamplingIdentifiers["SamplingID"]].EnsureVisible();
            }
        }

        public void NewDBFile(string filename)
        {
            SetupTree(filename);
        }

        public void NewLandingSite(string NodeName, string nodeGUID, string aoiGUID)
        {
            treeMain.SelectedNode = treeMain.Nodes["root"].Nodes[aoiGUID];
            NewLandingSite(NodeName, nodeGUID);
        }

        public void NewLandingSite(string NodeName, string nodeGUID)
        {
            TreeNode NewNode = new TreeNode();
            NewNode.Text = NodeName;
            NewNode.Tag = Tuple.Create(nodeGUID, "", "landing_site");
            NewNode.Name = nodeGUID;
            NewNode.ImageKey = "LandingSite";
            treeMain.SelectedNode.Expand();
            treeMain.SelectedNode.Nodes.Add(NewNode);
            treeMain.SelectedNode = NewNode;
        }

        public void NewSamplingDataEntryCancelled()
        {
            //if (listView1.Tag.ToString() == "samplingDetail") BackToSamplingMonth();
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
                RefreshTreeForNewSampling(LandingSiteGuid, GearVarGuid, SamplingDate.ToString("MMM-yyyy"));
            }
            SetUPLV("samplingDetail");
            ShowCatchDetailEx(_SamplingGUID);
        }

        /// <summary>
        /// refreshes contents of main listview given tree level
        /// </summary>
        /// <param name="TreeLevel"></param>
        public void RefreshLV(string TreeLevel)
        {
            SetUPLV(TreeLevel);
            if (TreeLevel == "landing_site" || TreeLevel == "aoi")
            {
                treeMain.SelectedNode.Text = lvMain.Items["name"].SubItems[1].Text;
            }
        }

        private void OnMRUlist_FileSelected(string filename)
        {
            if (_subListExisting)
            {
                SetupCatchListView(Show: false);
            }

            _treeLevel = "root";
            if (!SetupTree(filename, FromMRU: true))
            {
                _mrulist.RemoveFile(filename);
            }
            treeMain.SelectedNode = treeMain.Nodes["root"].FirstNode;
        }

        private void OnMRUlist_ManageMRU(object sender, EventArgs e)
        {
            ManageMRUForm f = new ManageMRUForm();
            f.Parent_form = this;
            f.ShowDialog(this);
        }

        private void AppExit()
        {
            Close();
        }

        private bool ApplyListViewColumnWidth(string TreeLevel)
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
                return i > 0;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
            return false;
        }

        /// <summary>
        /// adds to the tree
        /// </summary>
        private void BackToSamplingMonth()
        {
            SetupSamplingButtonFrame(false);
            SetUPLV(_treeLevel);
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

            var tsi = menuDropDown.Items.Add("New sampling");
            tsi.Name = "menuNewSampling";
            tsi.Visible = lvMain.Tag.ToString() == "sampling";

            if (lvh.Item == null)
            {
            }
            else
            {
                var tsi1 = menuDropDown.Items.Add("Enumerator detail");
                tsi1.Name = "menuEnumeratorDetail";
                tsi1.Visible = lvh.Item.Tag != null && _treeLevel == "aoi" && lvh.Item.Tag.ToString() == "enumerator";

                tsi1 = menuDropDown.Items.Add("Landing site detail");
                tsi1.Name = "menuLandingSiteDetail";
                tsi1.Visible = lvh.Item.Tag != null && _treeLevel == "landing_site" && lvh.Item.Tag.ToString() == "landing_site";

                tsi1 = menuDropDown.Items.Add("Delete sampling");
                tsi1.Name = "menuDeleteSampling";
                tsi1.Visible = lvMain.Tag.ToString() == "sampling";
            }
        }

        private void ConfigDropDownMenu(Control Source)
        {
            menuDropDown.Items.Clear();
            switch (Source.Name)
            {
                case "treeMain":
                    var tsi = menuDropDown.Items.Add("New target area");
                    tsi.Name = "menuNewTargetArea";
                    tsi.Enabled = _treeLevel == "root";

                    tsi = menuDropDown.Items.Add("Enumerators");
                    tsi.Name = "menuEnumerators";
                    tsi.Enabled = _treeLevel == "aoi";

                    tsi = menuDropDown.Items.Add("Fishing gears used");
                    tsi.Name = "menuGearsInTargetArea";
                    tsi.Enabled = _treeLevel == "aoi";

                    tsi = menuDropDown.Items.Add("Fishing gear inventory");
                    tsi.Name = "menuGearInventory";
                    tsi.Enabled = _treeLevel == "aoi";

                    tsi = menuDropDown.Items.Add("Target area properties");
                    tsi.Name = "menuTargetAreaProp";
                    tsi.Enabled = _treeLevel == "aoi";

                    var sep = menuDropDown.Items.Add("-");
                    sep.Name = "menuSeparator1";

                    tsi = menuDropDown.Items.Add("New landing site");
                    tsi.Name = "menuNewLandingSite";
                    tsi.Enabled = _treeLevel == "aoi";

                    tsi = menuDropDown.Items.Add("Get landing sites from Google Earth KML");
                    tsi.Name = "menuLandingSiteFromKML";
                    tsi.Enabled = _treeLevel == "aoi";

                    tsi = menuDropDown.Items.Add("Show landing sites on map");
                    tsi.Name = "menuShowOnMapLandingSites";
                    tsi.Enabled = _treeLevel == "aoi" && global.MapIsOpen;

                    tsi = menuDropDown.Items.Add("Landing site properties");
                    tsi.Name = "menuLandingSiteProp";
                    tsi.Enabled = _treeLevel == "landing_site";

                    sep = menuDropDown.Items.Add("-");
                    sep.Name = "menuSeparator2";

                    tsi = menuDropDown.Items.Add("New sampling");
                    tsi.Name = "menuNewSampling";
                    tsi.Enabled = _treeLevel == "sampling" || _treeLevel == "landing_site" || _treeLevel == "gear";

                    tsi = menuDropDown.Items.Add("Map fishing effort");
                    tsi.Name = "menuMapEffort";
                    tsi.Enabled = global.MapIsOpen;

                    sep = menuDropDown.Items.Add("-");
                    sep.Name = "menuSeparator3";

                    tsi = menuDropDown.Items.Add("Delete");
                    tsi.Name = "menuDeleteTreeItem";
                    tsi.Enabled = _treeLevel != "root";

                    break;

                case "lvMain":
                    tsi = menuDropDown.Items.Add("New sampling");
                    tsi.Name = "menuNewSampling";
                    tsi.Visible = _treeLevel == "sampling" || _treeLevel == "landing_site" || _treeLevel == "gear";

                    sep = menuDropDown.Items.Add("-");
                    sep.Name = "menuSeparator1";
                    sep.Visible = _treeLevel == "landing_site";
                    break;

                case "lvCatch":
                    tsi = menuDropDown.Items.Add("New catch composition");
                    tsi.Name = "menuNewCatchComposition";
                    tsi.Visible = ((ListView)Source).Items.Count == 0;

                    tsi = menuDropDown.Items.Add("Edit catch composition");
                    tsi.Name = "menuEditCatchComposition";
                    tsi.Visible = ((ListView)Source).Items.Count > 0;
                    break;

                case "lvLF_GMS":
                    var MenuPrompt = "";
                    var MenuName = "";
                    if (((ListView)Source).Items.Count > 0)
                    {
                        if (_catchSubRow == fad3CatchSubRow.GMS)
                        {
                            MenuPrompt = "Edit GMS table";
                            MenuName = "menuEditGMSTable";
                        }
                        else if (_catchSubRow == fad3CatchSubRow.LF)
                        {
                            MenuPrompt = "Edit Length-frequency table";
                            MenuName = "menuEditLFTable";
                        }
                    }
                    else
                    {
                        if (_catchSubRow == fad3CatchSubRow.GMS)
                        {
                            MenuPrompt = "New GMS table";
                            MenuName = "menuNewGMSTable";
                        }
                        else if (_catchSubRow == fad3CatchSubRow.LF)
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

        private string DatabaseSummary(string SummaryTopic)
        {
            return "x";
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

        /// <summary>
        /// Fills the listview with the samplings that took place in a month-year
        /// </summary>
        /// <param name="LSGUID"></param>
        /// <param name="GearGUID"></param>
        /// <param name="SamplingMonth"></param>
        ///
        private void FillLVSamplingSummary(string LSGUID, string GearGUID, string SamplingMonth)
        {
            var CompleteGrid25 = FishingGrid.IsCompleteGrid25;
            foreach (var item in sampling.SamplingSummaryForMonth(LSGUID, GearGUID, SamplingMonth))
            {
                var row = lvMain.Items.Add(item.Key, item.Value.RefNo, null);                           //reference number
                DateTime dt = (DateTime)item.Value.SamplingDate;
                row.SubItems.Add(string.Format("{0:MMM-dd-yyyy}", dt));                                 //sampling date
                row.SubItems.Add(item.Value.CatchRows.ToString());                                      //number of catch rows
                row.SubItems.Add(item.Value.WtCatch != null ? item.Value.WtCatch.ToString() : "");      //wt of catch
                var fishingGround = item.Value.FishingGround;                                           //fishing ground
                row.SubItems.Add(fishingGround);

                if (CompleteGrid25)                                                                     //position
                {
                    if (fishingGround.Length > 0)
                    {
                        row.SubItems.Add(FishingGrid.Grid25_to_UTM(fishingGround));
                    }
                    else
                    {
                        row.SubItems.Add("");
                    }
                }
                else
                {
                    row.SubItems.Add("");
                }

                row.SubItems.Add(item.Value.EnumeratorName);                                          //enumerator
                row.SubItems.Add(item.Value.HasSpecs);                                                //gear specs
                row.SubItems.Add(item.Value.Notes);                                                   //notes
                row.Tag = item.Key;                                                                   //sampling guid
                row.Name = item.Key;
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

        private void OnMainForm_Load(object sender, EventArgs e)
        {
            splitContainer1.Panel1MinSize = 200;
            splitContainer1.Panel2MinSize = this.Width - (this.splitContainer1.Panel1MinSize + 100);
            splitContainer1.SplitterWidth = 3;

            if (global.AllRequiredFilesExists)
            {
                sampling.SetUpUIElement();

                _Sampling.OnUIRowRead += new sampling.ReadUIElement(OnUIRowRead);

                toolStripRecentlyOpened.DropDownItems.Clear();

                //setup an MRU that contains 5 items
                _mrulist = new mru("FAD3", toolStripRecentlyOpened, 5);

                //setup the event handlers for the mru
                _mrulist.FileSelected += OnMRUlist_FileSelected;
                _mrulist.ManageMRU += OnMRUlist_ManageMRU;

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
                        Logger.Log("MDB file saved in registry not found");
                        lblErrorFormOpen.Visible = true;
                        lblTitle.Text = "";
                        lblErrorFormOpen.Text = @"Please locate the database file where fisheries data is saved.
                                                 You can use the file open menu";
                        LockTheApp(true);
                    }
                }
                catch
                {
                    Logger.Log("Registry entry for mdb path not found");
                    lblErrorFormOpen.Visible = true;
                    lblTitle.Text = "";
                    lblErrorFormOpen.Text = @"Please locate the database file where fisheries data is saved.
                                             You can use the file open menu";
                    LockTheApp(true);
                }
            }
            else
            {
                Logger.Log("Not all required files found");
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
            global.mainForm = this;
        }

        private void OnGenerateGridMapToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            e.ClickedItem.OwnerItem.Owner.Hide();
            global.MappingMode = fad3MappingMode.grid25Mode;
            var mf = MapperForm.GetInstance(this);
            if (!mf.Visible)
            {
                mf.Show(this);
                ToolStripItem tsi = e.ClickedItem;
                fadUTMZone utmZone = fadUTMZone.utmZone_Undefined;
                switch (tsi.Tag.ToString())
                {
                    case "zone50":
                        utmZone = fadUTMZone.utmZone50N;
                        break;

                    case "zone51":
                        utmZone = fadUTMZone.utmZone51N;
                        break;
                }
                global.MappingForm.CreateGrid25MajorGrid(utmZone);
                Grid25GenerateForm ggf = new Grid25GenerateForm(global.MappingForm);
                ggf.set_UTMZone(utmZone);
                ggf.Show(global.MappingForm);
            }
            else
            {
                mf.BringToFront();
            }
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

            foreach (var c in toolbar.Items)
            {
                ((ToolStripButton)c).With(o =>
                {
                    o.Enabled = o.Name == "tsButtonExit";
                });
            }
        }

        private void SetInventoryWindow(string inventoryGuid = "")
        {
            var gearInventoryForm = GearInventoryForm.GetInstance(_aoi, inventoryGuid);

            if (gearInventoryForm.Visible)
            {
                gearInventoryForm.BringToFront();
            }
            else
            {
                gearInventoryForm.Show(this);
            }
        }

        private void OnMenuDropDown_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var ItemName = e.ClickedItem.Name;
            e.ClickedItem.Owner.Hide();
            switch (ItemName)
            {
                case "menuDeleteTreeItem":
                    var myTag = (Tuple<string, string, string>)treeMain.SelectedNode.Tag;
                    switch (_treeLevel)
                    {
                        case "aoi":
                            var result = MessageBox.Show("Are you sure you want to delete the selected target area", "Confirmation needed", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                            if (result == DialogResult.Yes)
                            {
                                if (aoi.Delete(myTag.Item1))
                                {
                                    treeMain.Nodes.Remove(treeMain.SelectedNode);
                                }
                                else
                                {
                                    if (aoi.LastError.Length > 0)
                                    {
                                        MessageBox.Show(aoi.LastError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                            }
                            break;

                        case "landing_site":
                            result = MessageBox.Show("Are you sure you want to delete the selected landing site", "Confirmation needed", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                            if (result == DialogResult.Yes)
                            {
                                if (Landingsite.Delete(myTag.Item1))
                                {
                                    treeMain.Nodes.Remove(treeMain.SelectedNode);
                                }
                                else
                                {
                                    if (Landingsite.LastError.Length > 0)
                                    {
                                        MessageBox.Show(Landingsite.LastError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                            }
                            break;
                    }
                    break;

                case "menuGearsInTargetArea":
                    var tagf = TargetAreaGearsForm.GetInstance(_aoi);
                    if (tagf.Visible)
                    {
                        tagf.BringToFront();
                    }
                    else
                    {
                        tagf.Show(this);
                    }
                    break;

                case "menuGearInventory":
                    //var gearInventoryForm = GearInventoryForm.GetInstance(_aoi);
                    //if (gearInventoryForm.Visible)
                    //{
                    //    gearInventoryForm.BringToFront();
                    //}
                    //else
                    //{
                    //    gearInventoryForm.Show(this);
                    //}
                    SetInventoryWindow();
                    break;

                case "menuNewTargetArea":
                    TargetAreaForm tf = new TargetAreaForm(this, IsNew: true);
                    tf.ShowDialog(this);
                    break;

                case "menuNewLandingSite":
                    LandingSiteForm fls = new LandingSiteForm(_aoi, this, _ls, isNew: true);
                    fls.ShowDialog(this);
                    break;

                case "menuLandingSiteFromKML":
                    LandingSiteFromKMLForm lskf = LandingSiteFromKMLForm.GetInstance(_aoi);
                    if (!lskf.Visible)
                    {
                        lskf.Show(this);
                    }
                    else
                    {
                        lskf.BringToFront();
                    }
                    break;

                case "menuShowOnMapLandingSites":
                    if (!LandingSiteMappingHandler.ShowLandingSitesOnMap(global.MappingForm.MapLayersHandler, _aoiGuid, global.MappingForm.GeoProjection, true))
                    {
                        MessageBox.Show("Landing sites could not be shown on the map.\r\nPlease check landing site coordinates", "Mapping error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

                case "menuNewSampling":
                    if (FishingGrid.IsCompleteGrid25)
                    {
                        if (Enumerators.AOIHaveEnumerators(_aoiGuid))
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

                //this will show a list of enumerators and their corresponding details
                case "menuEnumerators":
                    EnumeratorForm ef = EnumeratorForm.GetInstance();
                    if (!ef.Visible)
                    {
                        ef.Show(this);
                    }
                    else
                    {
                        ef.BringToFront();
                    }
                    break;

                //this will show the samplings done by an enumerator
                case "menuEnumeratorDetail":
                    ef = EnumeratorForm.GetInstance(lvMain.SelectedItems[0].SubItems[1].Name);
                    if (!ef.Visible)
                    {
                        ef.Show(this);
                    }
                    else
                    {
                        ef.BringToFront();
                        ef.SetParent(lvMain.SelectedItems[0].SubItems[1].Name);
                    }
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
                    AOIForm.AOI = _aoi;
                    break;

                case "menuLandingSiteProp":
                    fls = new LandingSiteForm(_aoi, this, _ls);
                    fls.Show();
                    break;

                case "menuEditLFTable":
                    ShowLFForm();
                    break;

                case "menuNewLFTable":
                    ShowLFForm(IsNew: true);
                    break;

                case "menuEditCatchComposition":
                case "menuNewCatchComposition":

                    ShowCatchCompositionForm(ItemName == "menuNewCatchComposition");
                    break;

                case "menuNewGMSTable":
                case "menuEditGMSTable":
                    var Proceed = true;
                    var lvi = lvCatch.SelectedItems[0];
                    if (Enum.TryParse(lvi.SubItems[7].Text, out _taxa))
                    {
                        Proceed = _taxa != Taxa.To_be_determined;
                    }
                    else
                    {
                        Proceed = false;
                    }
                    if (Proceed)
                    {
                        ShowGMSForm(_taxa, isNew: ItemName == "menuNewGMSTable");
                    }
                    else
                    {
                        MessageBox.Show("Cannot edit gonad maturity because taxa is unknown",
                            "Cannot edit GMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

                case "menuDeleteSampling":
                    DeleteSampling();
                    break;

                case "menuMapEffort":
                    _formEffortMapper = MapEffortHelperForm.GetInstance(this);
                    if (!_formEffortMapper.Visible)
                    {
                        _formEffortMapper.Show(this);
                    }
                    else
                    {
                        _formEffortMapper.BringToFront();
                    }
                    _formEffortMapper.SetUpMapping(_treeLevel);
                    global.MappingMode = fad3MappingMode.fishingGroundMappingMode;
                    break;
            }
        }

        private void SetMappingEffortSource()
        {
            if (_formEffortMapper != null) _formEffortMapper.SetUpMapping(_treeLevel);
        }

        public void EffortMapperClosed()
        {
            _formEffortMapper = null;
        }

        private void ShowCatchCompositionForm(bool IsNew = false)
        {
            CatchCompositionForm ccf = new CatchCompositionForm(IsNew, this, _SamplingGUID, _referenceNumber, _weightOfCatch, _weightOfSample);
            ccf.ShowDialog(this);
        }

        private void NewSamplingForm()
        {
            ListViewNewSampling();
            var f3 = new SamplingForm
            {
                IsNew = true,
                GearClassName = _gearClassName,
                GearClassGuid = _gearClassGUID,
                GearVarGuid = _GearVarGUID,
                GearVarName = _GearVarName,
                AOIGuid = AOIGUID,
                AOIName = AOIName,
                LandingSiteName = _LandingSiteName,
                LandingSiteGuid = _LandingSiteGuid,
                AOI = _aoi
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
                    SizeColumns(lvCatch);
                    ShowCatchComposition(_SamplingGUID);
                    //SizeColumns(lvCatch, false);
                    break;

                case "buttonMap":
                    break;

                case "btnSubClose":
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
                    if (SetupLF_GMSListView(Show: true, Content: fad3CatchSubRow.LF))
                    {
                        _catchSubRow = fad3CatchSubRow.LF;
                        Show_LF_GMS_List(lvCatch.SelectedItems[0].Name);
                    }
                    break;

                case "btnSubGMS":
                    if (SetupLF_GMSListView(Show: true, Content: fad3CatchSubRow.GMS))
                    {
                        _catchSubRow = fad3CatchSubRow.GMS;
                        if (Enum.TryParse(lvCatch.SelectedItems[0].SubItems[7].Text, out Taxa myTaxa))
                        {
                            _taxa = myTaxa;
                        }
                        Show_LF_GMS_List(lvCatch.SelectedItems[0].Name, _taxa);
                    }
                    break;
            }
        }

        private void OnListView_DoubleClick(object sender, EventArgs e)
        {
            ((ListView)sender).With(o =>
            {
                switch (o.Name)
                {
                    case "lvMain":
                        SetupCatchListView(Show: false);
                        _topLVItemIndex = lvMain.TopItem.Index;
                        foreach (ListViewItem item in lvMain.SelectedItems)
                        {
                            switch (lvMain.Tag.ToString())
                            {
                                case "root":
                                    if (item.Tag != null && item.Tag.ToString() == "targetArea")
                                    {
                                        var nd = treeMain.Nodes["root"].Nodes[item.Name];
                                        treeMain.SelectedNode = nd;
                                        nd.Expand();
                                    }

                                    break;

                                case "aoi":
                                    if (item.Tag != null)
                                    {
                                        if (item.Tag.ToString() == "aoi_data")
                                        {
                                            var myTag = (Tuple<string, string, string>)treeMain.SelectedNode.Tag;
                                            _aoi.AOIGUID = myTag.Item1;
                                            TargetAreaForm f = new TargetAreaForm(this, IsNew: false);
                                            f.Show();
                                            f.AOI = _aoi;
                                        }
                                        else if (item.Name == "Enumerators")
                                        {
                                            var ef = EnumeratorForm.GetInstance(lvMain.SelectedItems[0].SubItems[1].Name);
                                            if (!ef.Visible)
                                            {
                                                ef.Show(this);
                                            }
                                            else
                                            {
                                                ef.BringToFront();
                                                ef.SetParent(lvMain.SelectedItems[0].SubItems[1].Name);
                                            }
                                        }
                                        else if (item.Tag.ToString() == "landingSite")
                                        {
                                            var nd = treeMain.SelectedNode.Nodes[item.Name];
                                            treeMain.SelectedNode = nd;
                                            nd.Expand();
                                        }
                                        else if (item.Tag.ToString() == "inventory" && item.SubItems[1].Text != "0")
                                        {
                                            SetInventoryWindow(item.Name);
                                        }
                                    }
                                    break;

                                case "database":
                                    if (item.Text == "Database path")
                                    {
                                        Process.Start(Path.GetDirectoryName(global.mdbPath));
                                    }
                                    break;

                                case "landing_site":
                                    if (item.Tag != null)
                                    {
                                        if (item.Tag.ToString() == "gearSampled")
                                        {
                                            var nd = treeMain.SelectedNode.Nodes[$"{_LandingSiteGuid}|{item.Name}"];
                                            if (nd != null)
                                            {
                                                treeMain.SelectedNode = nd;
                                                nd.Expand();
                                            }
                                        }
                                        else if (item.Tag.ToString() == "landing_site")
                                        {
                                            LandingSiteForm fls = new LandingSiteForm(_aoi, this, _ls);
                                            fls.Show();
                                        }
                                    }

                                    break;

                                case "gear":
                                    if (item.Tag != null && item.Tag.ToString() == "monthSampled")
                                    {
                                        var nd = treeMain.SelectedNode.Nodes[item.Name];
                                        if (nd != null)
                                        {
                                            treeMain.SelectedNode = nd;
                                            nd.Expand();
                                        }
                                    }
                                    break;

                                case "sampling":
                                    SetUPLV("samplingDetail");
                                    SamplingGUID = item.Tag.ToString();
                                    ShowCatchDetailEx(_SamplingGUID);
                                    item.BackColor = Color.Gainsboro;
                                    break;

                                case "samplingDetail":
                                    if (item.Name == "GearSpecs")
                                    {
                                        var s = ManageGearSpecsClass.GetSampledSpecsEx(_SamplingGUID);
                                        if (s.Length == 0)
                                            MessageBox.Show("Gear specs not found",
                                                            "Gear specifications",
                                                            MessageBoxButtons.OK,
                                                            MessageBoxIcon.Information);
                                        else
                                        {
                                            MessageBox.Show(s, "Gear specifications", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                    else
                                    {
                                        ShowSamplingDetailForm();
                                    }
                                    break;
                            }
                        }
                        break;

                    case "lvCatch":
                        ShowCatchCompositionForm();
                        break;

                    case "lvLF_GMS":
                        if (_catchSubRow == fad3CatchSubRow.LF)
                            ShowLFForm();
                        else if (_catchSubRow == fad3CatchSubRow.GMS)
                            ShowGMSForm(_taxa);
                        break;
                }
            });
        }

        private void OnListView_MouseDown(object sender, MouseEventArgs e)
        {
            ListView lv = (ListView)sender;
            _mouseX = e.Location.X;
            _mouseY = e.Location.Y;

            ListViewHitTestInfo lvh = lv.HitTest(_mouseX, _mouseY);
            switch (lv.Name)
            {
                case "lvMain":
                    if (lvMain.Tag.ToString() == "sampling" && lvh.Item != null)
                    {
                        SamplingGUID = lvh.Item.Tag.ToString();
                        if (FishingGrid.GridType == fadGridType.gridTypeGrid25 && global.MapIsOpen)
                        {
                            global.MappingForm.MapFishingGround(lvh.Item.SubItems[4].Text, FishingGrid.UTMZone);
                        }
                        _referenceNumber = lvh.Item.Text;
                    }
                    if (e.Button == MouseButtons.Right)
                    {
                        if (_treeLevel == "sampling" || _treeLevel == "aoi" || _treeLevel == "landing_site")
                        {
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
                        ConfigDropDownMenu(lvCatch);
                        menuDropDown.Show(lv, new Point(_mouseX, _mouseY));
                    }
                    else
                    {
                        if (lvh.Item != null)
                        {
                            if (Enum.TryParse(lvh.Item.SubItems[7].Text, out Taxa myTaxa))
                            {
                                _taxa = myTaxa;
                            }
                            Show_LF_GMS_List(lvh.Item.Name, _taxa);
                        }
                    }
                    break;

                case "lvLF_GMS":
                    if (e.Button == MouseButtons.Right)
                    {
                        ConfigDropDownMenu(lv);
                        menuDropDown.Show(lv, new Point(_mouseX, _mouseY));
                    }
                    else
                    {
                    }
                    break;
            }
        }

        private void SaveColumnWidthToRegistry()
        {
            if (lvMain.Columns.Count > 0)
                SaveColumnWidthEx(lvMain, lvMain.Tag.ToString());
        }

        private void OnListViewLeave(object sender, EventArgs e)
        {
            SaveColumnWidthToRegistry();
        }

        private void OnMenuFile_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem tsi = e.ClickedItem;
            e.ClickedItem.Owner.Hide();
            switch (tsi.Tag)
            {
                case "new":
                    CreateNewDatabaseForm f = new CreateNewDatabaseForm(this);
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
                    AboutFadForm f = new AboutFadForm();
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
                    ReferenceNumberRangeForm f = new ReferenceNumberRangeForm();
                    f.ShowDialog();
                    break;

                case "coordFormat":
                    CoordinateFormatSelectForm cdf = new CoordinateFormatSelectForm();
                    cdf.ShowDialog(this);
                    break;

                case "symbolFonts":
                    break;

                case "showError":
                    showErrorMessagesToolStripMenuItem.Checked = !showErrorMessagesToolStripMenuItem.Checked;
                    global.ShowErrorMessage = showErrorMessagesToolStripMenuItem.Checked;
                    break;

                case "createInland":
                    global.Grid25GenerateForm?.CreateInlandGridDB();
                    break;

                case "chlorophyll":
                    var chForm = Mapping.Forms.ChlorophyllForm.GetInstance();
                    if (chForm.Visible)
                    {
                        chForm.BringToFront();
                    }
                    else

                    {
                        chForm.Show(this);
                    }
                    break;
            }
        }

        public void SetMapDependendMenus()
        {
            cholorophyllGridMappingToolStripMenuItem.Enabled = global.MapIsOpen;
        }

        private void OnToolbar_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem tsi = e.ClickedItem;
            switch (tsi.Name)
            {
                case "tsButtonAbout":
                    AboutFadForm f = new AboutFadForm();
                    f.ShowDialog(this);
                    break;

                case "tsButtonGear":
                    GearCodesUsageForm form = GearCodesUsageForm.GetInstance();
                    if (!form.Visible)
                    {
                        form.Show(this);
                    }
                    else
                    {
                        form.BringToFront();
                    }
                    break;

                case "tsButtonFish":
                    AllSpeciesForm asf = new AllSpeciesForm(parent: this);
                    asf.ShowDialog(this);
                    break;

                case "tsButtonReport":

                    break;

                case "tsButtonMap":
                    var mf = MapperForm.GetInstance(this);
                    if (!mf.Visible)
                    {
                        mf.Show(this);
                    }
                    else
                    {
                        mf.BringToFront();
                        mf.Focus();
                    }
                    SetMapDependendMenus();
                    break;

                case "tsButtonExit":
                    AppExit();

                    break;
            }
        }

        private void OntreeMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                ConfigDropDownMenu(treeMain);

            SetupCatchListView(Show: false);
        }

        private void OntreeMainAfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node.FirstNode.Text == "*dummy*")
            {
                var myTag = (Tuple<string, string, string>)e.Node.Tag;
                int n = 0;
                foreach (var item in global.TreeSubNodes(myTag.Item3, myTag.Item1, myTag.Item2))
                {
                    TreeNode nd1 = new TreeNode();
                    if (myTag.Item3 == "landing_site")
                    {
                        _LandingSiteGuid = myTag.Item1;
                        if (n == 0)
                        {
                            nd1 = e.Node.FirstNode;
                            nd1.Text = item.Variation;
                        }
                        else
                        {
                            nd1 = e.Node.Nodes.Add(item.Variation);
                        }
                        nd1.Nodes.Add("*dummy*");

                        nd1.Tag = Tuple.Create(item.LandingSiteGuid, item.GearVariationGuid, "gear");
                        nd1.Name = $"{item.LandingSiteGuid}|{item.GearVariationGuid}";
                        nd1.ImageKey = gear.GearClassImageKeyFromGearClasName(item.GearClassName);
                    }
                    else if (myTag.Item3 == "gear")
                    {
                        _LandingSiteGuid = myTag.Item1;
                        _GearVarGUID = myTag.Item2;
                        if (n == 0)
                        {
                            e.Node.FirstNode.Text = item.SamplingMonthYear;
                            nd1 = e.Node.FirstNode;
                        }
                        else
                        {
                            nd1 = e.Node.Nodes.Add(item.SamplingMonthYear);
                        }
                        if (_LandingSiteGuid.Length == 0 && _GearVarGUID.Length == 0)
                        {
                            myTag = (Tuple<string, string, string>)e.Node.Tag;
                            _LandingSiteGuid = myTag.Item1;
                            _GearVarGUID = myTag.Item2;
                        }
                        nd1.Tag = Tuple.Create(_LandingSiteGuid, _GearVarGUID, "sampling");
                        nd1.Name = $"{_LandingSiteGuid}|{_GearVarGUID}|{item.SamplingMonthYear}";
                        nd1.ImageKey = "MonthGear";
                    }
                    n++;
                }
                if (n == 0) e.Node.Nodes.Clear();
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
                _gearClassName = "";
                _LandingSiteGuid = "";
                _GearVarGUID = "";
                _gearClassGUID = "";

                TreeNode nd = treeMain.SelectedNode;
                ResetTheBackColor(treeMain);
                nd.BackColor = Color.Gainsboro;

                var myTag = (Tuple<string, string, string>)nd.Tag;
                _treeLevel = myTag.Item3;

                statusPanelTargetArea.Width = _statusPanelWidth;
                switch (_treeLevel)
                {
                    case "gear":
                        _aoiName = treeMain.SelectedNode.Parent.Parent.Text;
                        this.AOIGUID = treeMain.SelectedNode.Parent.Parent.Name;
                        _LandingSiteName = treeMain.SelectedNode.Parent.Text;
                        _LandingSiteGuid = treeMain.SelectedNode.Parent.Name;
                        _GearVarName = treeMain.SelectedNode.Text;
                        _GearVarGUID = myTag.Item2;
                        var rv = gear.GearClassGuidNameFromGearVarGuid(_GearVarGUID);
                        _gearClassName = rv.Value;
                        _gearClassGUID = rv.Key;
                        _LSNode = e.Node.Parent;

                        break;

                    case "sampling":
                        _aoiName = treeMain.SelectedNode.Parent.Parent.Parent.Text;
                        this.AOIGUID = ((Tuple<string, string, string>)treeMain.SelectedNode.Parent.Parent.Parent.Tag).Item1;
                        _LandingSiteName = treeMain.SelectedNode.Parent.Parent.Text;
                        _LandingSiteGuid = myTag.Item1;
                        _GearVarName = treeMain.SelectedNode.Parent.Text;
                        _GearVarGUID = myTag.Item2;
                        rv = gear.GearClassGuidNameFromGearVarGuid(_GearVarGUID);
                        _gearClassName = rv.Value;
                        _gearClassGUID = rv.Key;
                        _LSNode = e.Node.Parent.Parent;
                        _SamplingMonth = e.Node.Text;

                        break;

                    case "aoi":
                        _aoiName = treeMain.SelectedNode.Text;
                        this.AOIGUID = treeMain.SelectedNode.Name;
                        _LandingSiteName = "";
                        _LandingSiteGuid = "";
                        _GearVarName = "";
                        _GearVarGUID = "";
                        break;

                    case "root":
                        break;

                    case "landing_site":
                        _aoiName = treeMain.SelectedNode.Parent.Text;
                        this.AOIGUID = treeMain.SelectedNode.Parent.Name;
                        _LandingSiteName = treeMain.SelectedNode.Text;
                        _LandingSiteGuid = treeMain.SelectedNode.Name;
                        _GearVarName = "";
                        _GearVarGUID = "";
                        _LSNode = e.Node;

                        break;
                }

                SetUPLV(_treeLevel);
                statusPanelTargetArea.Text = _aoiName;
                statusPanelLandingSite.Text = _LandingSiteName;
                statusPanelGearUsed.Text = _GearVarName;
                if (_treeLevel != "root") SetMappingEffortSource();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }

            e.Node.SelectedImageKey = e.Node.ImageKey;
        }

        private void OnUIRowRead(object sender, UIRowFromXML e)
        {
            ListViewItem lvi = lvMain.Items.Add(e.RowLabel);
            lvi.Name = e.Key;
            lvi.SubItems.Add("");
        }

        /// <summary>
        /// populates main tree with target area and landing site nodes
        /// </summary>
        private void PopulateTree()
        {
            this.treeMain.Nodes.Clear();
            TreeNode root = this.treeMain.Nodes.Add("Fisheries data");
            root.Name = "root";
            root.Tag = Tuple.Create("root", "", "root");
            root.ImageKey = "db";
            foreach (var item in global.TreeNodes())
            {
                if (root.Nodes.ContainsKey(item.AOIGuid))
                {
                    TreeNode myNode = root.Nodes[item.AOIGuid];
                    TreeNode myChild = new TreeNode(item.LandingSiteName);
                    myNode.Nodes.Add(myChild);
                    myChild.Name = item.LandingSiteGuid;
                    myChild.Nodes.Add("*dummy*");
                    myChild.Tag = Tuple.Create(item.LandingSiteGuid, "", "landing_site");
                    myChild.ImageKey = "LandingSite";
                }
                else
                {
                    TreeNode myNode = new TreeNode(item.AOIName);
                    myNode.Name = item.AOIGuid;
                    root.Nodes.Add(myNode);
                    myNode.Tag = Tuple.Create(item.AOIGuid, "", "aoi");
                    myNode.ImageKey = "AOI";

                    if (item.LandingSiteName.Length > 0)
                    {
                        TreeNode myChild = new TreeNode(item.LandingSiteName);
                        myNode.Nodes.Add(myChild);
                        myChild.Nodes.Add("*dummy*");
                        myChild.Tag = Tuple.Create(item.LandingSiteGuid, "", "landing_site");
                        myChild.Name = item.LandingSiteGuid;
                        myChild.ImageKey = "LandingSite";
                    }
                    else
                    {
                        TreeNode myChild = new TreeNode("*dummy*");
                        myNode.Nodes.Add(myChild);
                    }
                }
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

        /// <summary>
        /// refreshes tree for any new sampling
        /// </summary>
        private void RefreshTreeForNewSampling(string LandingSiteGuid, string GearVarGuid, string MonthYear)
        {
            if (_newSamplingEntered)
            {
                var LandingSiteNode = new TreeNode();

                var gearNode = new TreeNode();
                gearNode.Name = $"{LandingSiteGuid}|{GearVarGuid}";
                gearNode.Text = gear.GearVarNameFromGearGuid(GearVarGuid);
                gearNode.Tag = Tuple.Create(LandingSiteGuid, GearVarGuid, "gear");
                gearNode.ImageKey = gear.GearVarNodeImageKeyFromGearVar(GearVarGuid);

                var samplingMonthNode = new TreeNode(MonthYear);
                samplingMonthNode.Name = $"{LandingSiteGuid}|{GearVarGuid}|{MonthYear}";
                samplingMonthNode.Tag = Tuple.Create(LandingSiteGuid, GearVarGuid, "sampling");
                samplingMonthNode.ImageKey = "MonthGear";

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

                try
                {
                    if (!LandingSiteNode.Nodes.ContainsKey(gearNode.Name))
                    {
                        LandingSiteNode.Nodes.Add(gearNode);
                        gearNode.Nodes.Add(samplingMonthNode);
                    }
                    else
                    {
                        gearNode = LandingSiteNode.Nodes[gearNode.Name];
                        if (!gearNode.IsExpanded) gearNode.Expand();
                        if (!gearNode.Nodes.ContainsKey(samplingMonthNode.Name))
                        {
                            gearNode.Nodes.Add(samplingMonthNode);
                        }
                        else
                        {
                            samplingMonthNode = gearNode.Nodes[samplingMonthNode.Name];
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log("error in MainForm.RefreshTreeForNewSampling\r\n" +
                                ex.Message);
                }
                treeMain.SelectedNode = samplingMonthNode;
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

                case "treeMain":
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
                    lvCatch = new ListView();
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

                    lvLF_GMS = new ListView();
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

        private bool SetupLF_GMSListView(bool Show = true, fad3CatchSubRow Content = fad3CatchSubRow.none)
        {
            var Proceed = false;
            if (_subListExisting)
            {
                //var lv = (ListView)splitContainer1.Panel2.Controls["lvLF_GMS"];
                //var lvCatch = (ListView)splitContainer1.Panel2.Controls["lvCatch"];

                lvLF_GMS.Items.Clear();
                lvLF_GMS.Columns.Clear();
                {
                    if (Show && lvCatch.Items.Count > 0)
                    {
                        lvLF_GMS.Visible = true;
                        lvLF_GMS.Left = lvCatch.Columns["NameOfCatch"].Width + lvCatch.Columns["Row"].Width +
                                  lvCatch.Columns["Weight"].Width + lvCatch.Columns["Count"].Width;

                        lvLF_GMS.Width = lvCatch.Columns["SubWt"].Width + lvCatch.Columns["SubCt"].Width +
                                   lvCatch.Columns["FromTotal"].Width + lvCatch.Columns["CompWt"].Width +
                                   lvCatch.Columns["CompCt"].Width + lvCatch.Columns["spacer"].Width + 3;

                        if (Content == fad3CatchSubRow.LF)
                        {
                            lvLF_GMS.Columns.Add("Row");
                            lvLF_GMS.Columns.Add("Length");
                            lvLF_GMS.Columns.Add("Frequency");
                            lvLF_GMS.Columns.Add("");
                        }
                        else if (Content == fad3CatchSubRow.GMS)
                        {
                            lvLF_GMS.Columns.Add("Row");
                            lvLF_GMS.Columns.Add("Length");
                            lvLF_GMS.Columns.Add("Weight");
                            lvLF_GMS.Columns.Add("Sex");
                            lvLF_GMS.Columns.Add("GMS");
                            lvLF_GMS.Columns.Add("Gonad wt");
                            lvLF_GMS.Columns.Add("");
                        }
                        Proceed = true;
                    }
                    else
                    {
                        lvLF_GMS.Visible = false;
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
            var savedColWidthExist = true;
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
                    lblTitle.Text = "Target area";
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
            if (!ApplyListViewColumnWidth(TreeLevel))
            {
                savedColWidthExist = false;
                SizeColumns(lvMain);
            }

            //add rows to the listview
            switch (_treeLevel)
            {
                case "sampling":
                    FillLVSamplingSummary(_LandingSiteGuid, _GearVarGUID, _SamplingMonth);
                    break;

                case "samplingDetail":
                    break;

                case "gear":
                    var myTag = (Tuple<string, string, string>)treeMain.SelectedNode.Tag;
                    gear.GearVarGUID = myTag.Item2;
                    var n = 0;
                    lvi = lvMain.Items.Add("Months sampled");
                    foreach (var item in gear.MonthsSampledByGear(myTag.Item1))
                    {
                        if (n > 0)
                        {
                            lvi = lvMain.Items.Add(item.Key, "", null);
                        }
                        else
                        {
                            lvi.Name = item.Key;
                        }
                        lvi.SubItems.Add(item.Value);
                        lvi.Tag = "monthSampled";
                        n++;
                    }
                    break;

                case "root":
                    lvi = lvMain.Items.Add("Database path");
                    lvi.SubItems.Add(global.mdbPath);
                    lvi.Tag = "databasePath";

                    //add sampled years with count for entire database
                    lvi = lvMain.Items.Add("");
                    lvi = lvMain.Items.Add("Sampled years");
                    foreach (string item in _aoi.SampledYears())
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

                    foreach (KeyValuePair<string, string> kv in _aoi.AOIWithSamplingCount())
                    {
                        if (i > 0)
                        {
                            lvi = lvMain.Items.Add(kv.Key, "", null);
                        }
                        else
                        {
                            lvi.Name = kv.Key;
                        }
                        lvi.SubItems.Add(kv.Value);
                        lvi.Tag = "targetArea";
                        i++;
                    }
                    break;

                case "aoi":
                    //arr = treeMain.SelectedNode.Tag.ToString().Split(',');
                    myData = _aoi.AOIData();
                    var arr = myData.Split('|');

                    //add AOI data for form
                    lvi = lvMain.Items.Add("name", "Name", null);
                    lvi.SubItems.Add(arr[0]);
                    lvi.Tag = "aoi_data";
                    lvi = lvMain.Items.Add("Code");
                    lvi.SubItems.Add(arr[1]);
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
                    lvi.SubItems.Add(_aoi.SampleCount().ToString());

                    //add sampled years with count
                    lvi = lvMain.Items.Add("Years sampling");
                    i = 0;
                    foreach (KeyValuePair<string, string> item in _aoi.ListYearsWithSamplingCount())
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
                    foreach (KeyValuePair<string, string> item in Enumerators.EnumeratorsWithCount(_aoiGuid))
                    {
                        if (i > 0)
                        {
                            lvi = lvMain.Items.Add("");
                        }
                        lvi.Name = "Enumerators";
                        arr = item.Value.ToString().Split(',');
                        lvi.SubItems.Add(item.Key + ": " + arr[1].ToString()).Name = arr[0];
                        lvi.Tag = "enumerator";
                        i++;
                    }

                    // if there is no sampling then we cannot show enumerators with count
                    // so we just show a simple list
                    if (i == 0)
                    {
                        foreach (KeyValuePair<string, string> item in Enumerators.AOIEnumeratorsList(_aoiGuid))
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
                    foreach (var item in _aoi.ListLandingSiteWithSamplingCount())
                    {
                        if (i > 0)
                        {
                            lvi = lvMain.Items.Add(item.Key, "", null);
                        }
                        else
                        {
                            lvi.Name = item.Key;
                        }
                        lvi.SubItems.Add(item.Value);
                        lvi.Tag = "landingSite";
                        i++;
                    }

                    //add fishery inventories
                    n = 0;
                    var inventory = new FishingGearInventory(_aoi);
                    lvMain.Items.Add("");
                    lvi = lvMain.Items.Add("Fishery inventories");
                    if (inventory.Inventories.Count > 0)
                    {
                        foreach (var item in inventory.Inventories)
                        {
                            if (n > 0)
                            {
                                lvi = lvMain.Items.Add("");
                            }
                            lvi.Name = item.Key;
                            lvi.SubItems.Add(item.Value.InventoryName);
                            lvi.Tag = "inventory";
                            n++;
                        }
                    }
                    else
                    {
                        lvi.SubItems.Add("0");
                    }

                    break;

                case "landing_site":

                    //add landing site form data
                    _ls.LandingSiteGUID = _LandingSiteGuid;
                    _ls.LandingSiteDataEx().With(o =>
                    {
                        lvi = lvMain.Items.Add("name", "Name", null);
                        lvi.SubItems.Add(o["LSName"]);
                        lvi.Tag = "landing_site";
                        lvi = lvMain.Items.Add("Municipality");
                        lvi.SubItems.Add(o["Municipality"]);
                        lvi.Tag = "landing_site";
                        lvi = lvMain.Items.Add("Province");
                        lvi.SubItems.Add(o["ProvinceName"]);
                        lvi.Tag = "landing_site";
                        lvi = lvMain.Items.Add("Coordinate");
                        lvi.SubItems.Add(o["CoordinateStringXY"]);
                        lvi.Tag = "landing_site";
                    });

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
                            lvi = lvMain.Items.Add(item.Key, "", null);
                        }
                        else
                        {
                            lvi.Name = item.Key;
                        }
                        lvi.Tag = "gearSampled";
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
            if (!savedColWidthExist)
            {
                SizeColumns(lvMain, false);
                SaveColumnWidthToRegistry();
            }
            lvMain.ResumeLayout();
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
                this.panelSamplingButtons.Location = new Point(lvMain.Width - panelSamplingButtons.Width - 30, lvMain.Top + 50);
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
                Logger.Log("MDB file saved in registry not found");
                lblErrorFormOpen.Visible = true;
                lblTitle.Text = "";
                lblErrorFormOpen.Text = "Please locate the database file where fisheries data is saved.\r\n" +
                                         "You can use the file open menu";
                LockTheApp(true);
                return false;
            }
        }

        public void RefreshLF_GMS()
        {
            Show_LF_GMS_List(lvCatch.SelectedItems[0].Name, _taxa);
        }

        private void Show_LF_GMS_List(string CatchRowGuid, Taxa taxa = Taxa.To_be_determined)
        {
            lvLF_GMS.Items.Clear();
            int n = 1;
            if (_catchSubRow == fad3CatchSubRow.LF)
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
                    lvLF_GMS.Items.Add(lvi);
                    n++;
                }
            }
            else if (_catchSubRow == fad3CatchSubRow.GMS)
            {
                foreach (KeyValuePair<string, GMSLineClass> kv in GMSManager.RetrieveGMSData(CatchRowGuid))
                {
                    var lvi = new ListViewItem(new string[]
                    {
                                    n.ToString(),
                                    kv.Value.Length.ToString(),
                                    kv.Value.Weight.ToString(),
                                    kv.Value.Sex.ToString(),
                                    GMSManager.GMSStageToString(taxa, kv.Value.GMS),
                                    kv.Value.GonadWeight.ToString()
                    });
                    lvi.Name = kv.Key;
                    lvLF_GMS.Items.Add(lvi);
                    n++;
                }
            }

            foreach (ColumnHeader c in lvLF_GMS.Columns)
            {
                c.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }

        public void RefreshCatchComposition()
        {
            ShowCatchComposition(_SamplingGUID);
        }

        private void ShowCatchComposition(string SamplingGuid)
        {
            if (_subListExisting)
            {
                lvCatch.Items.Clear();
                int n = 1;
                //foreach (KeyValuePair<string, sampling.CatchLine> kv in _Sampling.CatchComp())
                foreach (KeyValuePair<string, CatchLine> kv in CatchComposition.RetrieveCatchComposition(_SamplingGUID))
                {
                    var lvi = new ListViewItem(new string[]
                    {
                        n.ToString(),
                        kv.Value.CatchName,
                        kv.Value.CatchWeight.ToString(),
                        kv.Value.CatchCount.ToString(),
                        kv.Value.CatchSubsampleWt.ToString(),
                        kv.Value.CatchSubsampleCount.ToString(),
                        kv.Value.FromTotalCatch.ToString(),
                        kv.Value.TaxaNumber.ToString()
                    });
                    lvi.Name = kv.Key;
                    lvCatch.Items.Add(lvi);
                    n++;
                }

                if (lvCatch.Items.Count > 0)
                    lvCatch.Items[0].Selected = true;

                foreach (ColumnHeader c in lvCatch.Columns)
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
                            if (lvCatch.Items.Count > 0)
                                c.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                            else
                                c.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Sizes all columns so that it fits the widest column content or the column header content
        /// </summary>
        private void SizeColumns(ListView lv, bool init = true)
        {
            foreach (ColumnHeader c in lv.Columns)
            {
                if (init)
                {
                    c.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                    c.Tag = c.Width;
                }
                else
                {
                    c.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                    c.Width = c.Width > (int)c.Tag ? c.Width : (int)c.Tag;
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

                        case "WeightOfSample":
                            _weightOfSample = null;
                            if (double.TryParse(effortData[lvi.Name], out double sampleWt))
                            {
                                _weightOfSample = sampleWt;
                            }
                            lvi.SubItems[1].Text = _weightOfSample == null ? "" : _weightOfSample.ToString();
                            break;

                        case "WeightOfCatch":
                            _weightOfCatch = double.Parse(effortData[lvi.Name]);
                            lvi.SubItems[1].Text = _weightOfCatch.ToString();
                            break;

                        default:
                            lvi.SubItems[1].Text = effortData[lvi.Name];
                            break;
                    }
                }

                _vesHeight = effortData["VesHeight"];
                _vesLength = effortData["VesLength"];
                _vesWidth = effortData["VesWidth"];

                DateEncoded = effortData["DateEncoded"];
            }

            var lvi1 = lvMain.Items.Add("");
            lvi1 = lvMain.Items.Add("DateEncoded", "Date encoded", null);
            if (SamplingGUID.Length > 0) lvi1.SubItems.Add(DateEncoded);

            //position sampling buttons and make it visible
            SetupSamplingButtonFrame(true);
        }

        private void ShowGMSForm(Taxa taxa, bool isNew = false)
        {
            if (taxa != Taxa.To_be_determined)
            {
                GMSDataEntryForm fgms = new GMSDataEntryForm(isNew, _Sampling,
                                          lvCatch.SelectedItems[0].Name,
                                          lvCatch.SelectedItems[0].SubItems[1].Text, _taxa, this);
                fgms.ShowDialog(this);
            }
        }

        private void ShowLFForm(bool IsNew = false)
        {
            LengthFreqForm lff;

            lff = new LengthFreqForm(IsNew, _Sampling,
                                      lvCatch.SelectedItems[0].Name,
                                      lvCatch.SelectedItems[0].SubItems[1].Text, this);

            lff.ShowDialog(this);
            Show_LF_GMS_List(lvCatch.SelectedItems[0].Name);
        }

        private void ShowSamplingDetailForm()
        {
            SamplingForm fs = new SamplingForm();
            fs.SamplingGUID = _SamplingGUID;
            fs.ListViewSamplingDetail(lvMain);
            fs.AOI = _aoi;
            fs.AOIGuid = _aoiGuid;
            fs.Parent_Form = this;
            fs.VesselDimension(_vesLength, _vesWidth, _vesHeight);
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

        private void Cleanup()
        {
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Cleanup();
        }

        private void OnmenuMenuBar_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            generateInlandDbToolStripMenuItem.Visible = global.Grid25GenerateForm != null;
        }

        private void OnItemDrag(object sender, ItemDragEventArgs e)
        {
            _nodeParent = ((TreeNode)e.Item).Parent;
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void OnDragDrop(object sender, DragEventArgs e)
        {
            // Retrieve the client coordinates of the drop location.
            Point targetPoint = treeMain.PointToClient(new Point(e.X, e.Y));

            // Retrieve the node at the drop location.
            TreeNode targetNode = treeMain.GetNodeAt(targetPoint);

            // Retrieve the node that was dragged.
            //TreeNode draggedNode = e.Data.GetData(typeof(TreeNode));
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

            if (targetNode.Parent == _nodeParent)
            {
                // Sanity check
                if (draggedNode == null)
                {
                    return;
                }

                // Did the user drop on a valid target node?
                if (targetNode == null)
                {
                    // The user dropped the node on the treeview control instead
                    // of another node so lets place the node at the bottom of the tree.
                    draggedNode.Remove();
                    treeMain.Nodes.Add(draggedNode);
                    draggedNode.Expand();
                }
                else
                {
                    TreeNode parentNode = targetNode;

                    // Confirm that the node at the drop location is not
                    // the dragged node and that target node isn't null
                    // (for example if you drag outside the control)
                    if (!draggedNode.Equals(targetNode) && targetNode != null)
                    {
                        bool canDrop = true;

                        // Crawl our way up from the node we dropped on to find out if
                        // if the target node is our parent.
                        while (canDrop && (parentNode != null))
                        {
                            canDrop = !Object.ReferenceEquals(draggedNode, parentNode);
                            parentNode = parentNode.Parent;
                        }

                        // Is this a valid drop location?
                        if (canDrop)
                        {
                            // Yes. Move the node, expand it, and select it.
                            var sourceTag = (Tuple<string, string, string>)draggedNode.Tag;
                            var destinationTag = (Tuple<string, string, string>)targetNode.Tag;
                            var result = MessageBox.Show($"Are you sure you want to transfer sampling data from {draggedNode.Text} to {targetNode.Text}?",
                                                            "Confirmation needed", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes && Landingsite.MoveToLandingSite(sourceTag.Item1, destinationTag.Item1))
                            {
                                //targetNode.Nodes.Add(draggedNode);
                                draggedNode.Nodes.Clear();
                                RefreshLandingSiteNodeNodes(targetNode);
                                targetNode.Expand();
                            }
                        }
                    }
                }

                // Optional: Select the dropped node and navigate (however you do it)
                treeMain.SelectedNode = draggedNode;
                // NavigateToContent(draggedNode.Tag);
            }
            else
            {
                MessageBox.Show("Can only transfer sampling data to landing sites of the same target area", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RefreshLandingSiteNodeNodes(TreeNode landingSiteNode)
        {
            landingSiteNode.Nodes.Clear();
            var tag = (Tuple<string, string, string>)landingSiteNode.Tag;
            var gearList = Landingsite.SampledGearsFromLandingSite(tag.Item1);
            if (gearList.Count > 0)
            {
                foreach (var item in gearList)
                {
                    var node = landingSiteNode.Nodes.Add(item.gearVariationGuid, item.gearVariationName);
                    node.Tag = Tuple.Create(tag.Item1, item.gearVariationGuid, "gear");
                    node.Name = $"{tag.Item1}|{item.gearVariationGuid}";
                    node.ImageKey = gear.GearClassImageKeyFromGearClasName(item.gearClassName);
                    node.Nodes.Add("*dummy*");
                }
            }
            else
            {
                landingSiteNode.Nodes.Add("*dummy*");
            }
        }

        private void OnDragOver1(object sender, DragEventArgs e)
        {
            TreeNode node = treeMain.GetNodeAt(treeMain.PointToClient(new Point(e.X, e.Y)));
            var tag = (Tuple<string, string, string>)node.Tag;
            if (tag.Item3 == "landing_site")
            {
                if (node.Parent == _nodeParent)
                {
                    treeMain.SelectedNode = node;
                }
            }
            else
            {
                treeMain.SelectedNode = null;
            }
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            Point p = treeMain.PointToClient(new Point(e.X, e.Y));
            TreeNode node = treeMain.GetNodeAt(p.X, p.Y);
            var tag = (Tuple<string, string, string>)node.Tag;
            if (node.PrevVisibleNode != null)
            {
                node.PrevVisibleNode.BackColor = Color.White;
            }
            if (node.NextVisibleNode != null)
            {
                node.NextVisibleNode.BackColor = Color.White;
            }
            if (tag.Item3 == "landing_site" && node.Parent == _nodeParent)
            {
                node.BackColor = Color.Aquamarine;
            }
        }

        private void OntreeMain_MouseUp(object sender, MouseEventArgs e)
        {
            treeMain.Cursor = Cursors.Default;
        }
    }
}
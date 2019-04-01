/*
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/7/2016
 * Time: 1:53 PM
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using FAD3.Database.Classes;
using FAD3.Database.Forms;
using FAD3.GUI.Forms;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using FAD3.Mapping.Forms;

//using dao;

namespace FAD3
{
    /// <summary>
    /// Description of frmMain.
    /// </summary>
    public partial class MainForm : Form
    {
        private MapEffortHelperForm _formEffortMapper;
        private TargetArea _targetArea = new TargetArea();
        private string _targetAreaGuid = "";
        private string _targetAreaName = "";
        private fad3CatchSubRow _catchSubRow;
        private string _gearClassGUID = "";
        private string _gearClassName = "";
        private string _gearVarGUID = "";
        private string _gearVarName = "";
        private string _landingSiteGuid = "";
        private string _landingSiteName = "";
        private Landingsite _ls = new Landingsite();
        private TreeNode _LSNode;
        private int _mouseX = 0;
        private int _mouseY = 0;
        private mru _mrulist = new mru();
        private bool _newSamplingEntered;
        private string _oldMDB = "";
        private Sampling _sampling;// =  new sampling();
        private string _samplingGUID = "";
        private string _samplingMonth = "";
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
        private bool _appIsLocked;

        private ListView lvCatch;
        private ListView lvLF_GMS;

        //for column sort
        private int _sortColumn = -1;
        private TreeNode _nodeParent;
        public event EventHandler SamplingDetailClosed;
        private CatchLocalNamesForm _catchLocalNamesForm;
        private (DateTime SamplingDate, string GearVariationGuid, string LandingSiteGuid) _updatedEffortMonth;
        private (string SampledMonth, string GearVariationGuid, string LandingSiteGuid) _effortMonth;
        private bool _readEfforMonth;
        private bool _enableUIEvent;
        private Grid25GenerateForm _grid25GenerateForm;

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
            get { return _gearVarGUID; }
        }

        public Landingsite LandingSite
        {
            get { return _ls; }
        }

        public string SamplingMonth
        {
            get { return _samplingMonth; }
        }

        public string GearVariationName
        {
            get { return _gearVarName; }
        }

        public string LandingSiteName
        {
            get { return _landingSiteName; }
        }

        public string LandingSiteGUID
        {
            get { return _landingSiteGuid; }
        }

        public ImageList treeImages
        {
            get { return imageList16; }
        }

        public TargetArea TargetArea
        {
            get { return _targetArea; }
        }

        public string TargetAreaGuid
        {
            get { return _targetAreaGuid; }
            set
            {
                if (value != _targetAreaGuid)
                {
                    _targetAreaGuid = value;
                    if (value.Length > 0)
                    {
                        _targetArea.TargetAreaGuid = _targetAreaGuid;
                        FishingGrid.TargetAreaGuid = _targetAreaGuid;
                        Enumerators.AOIGuid = _targetAreaGuid;
                    }
                }
            }
        }

        public string TargetAreaName
        {
            get { return _targetAreaName; }
            set
            {
                _targetAreaName = value;
                if (value.Length > 0)
                {
                    _targetArea.TargetAreaName = _targetAreaName;
                }
            }
        }

        public mru MRUList
        {
            get { return _mrulist; }
        }

        public Sampling Sampling
        {
            get { return _sampling; }
        }

        public string SamplingGUID
        {
            get { return _samplingGUID; }
            set
            {
                _samplingGUID = value;
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
                ShowCatchDetailEx(_samplingGUID);
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
                ImageKey = "target_area"
            };
            nd.Tag = Tuple.Create(TargetAreaGuid, "", "target_area");
            treeMain.Nodes["root"].Nodes.Add(nd);
            treeMain.SelectedNode = nd;
        }

        public void RefreshCatchDetail(string SamplingGUID, bool NewSampling,
                                               DateTime SamplingDate, string GearVarGuid, string LandingSiteGuid)
        {
            _samplingGUID = SamplingGUID;
            _newSamplingEntered = NewSampling;
            lvMain.Columns.With(o =>
            {
                o.Clear();
                o.Add("Property");
                o.Add("Value");
            });
            //ApplyListViewColumnWidth("samplingDetail");
            if (_newSamplingEntered)
            {
                RefreshTreeForNewSampling(LandingSiteGuid, GearVarGuid, SamplingDate.ToString("MMM-yyyy"));
            }
            SetUPLV("samplingDetail");
            ShowCatchDetailEx(_samplingGUID);
        }

        /// <summary>
        /// refreshes contents of main listview given tree level
        /// </summary>
        /// <param name="TreeLevel"></param>
        public void RefreshLV(string TreeLevel)
        {
            SetUPLV(TreeLevel);
            if (TreeLevel == "landing_site" || TreeLevel == "target_area")
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
                Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
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
            var lvi = lvMain.Items[_samplingGUID];
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
                tsi1.Visible = lvh.Item.Tag != null && _treeLevel == "target_area" && lvh.Item.Tag.ToString() == "enumerator";

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
                    tsi.Enabled = _treeLevel == "target_area";

                    tsi = menuDropDown.Items.Add("Fishing gears used");
                    tsi.Name = "menuGearsInTargetArea";
                    tsi.Enabled = _treeLevel == "target_area";

                    tsi = menuDropDown.Items.Add("Fishing boats");
                    tsi.Name = "menuFishingBoats";
                    tsi.Enabled = _treeLevel == "target_area" || _treeLevel == "landing_site";

                    tsi = menuDropDown.Items.Add("Inventory of fishers, gears, and vessels");
                    tsi.Name = "menuGearInventory";
                    tsi.Enabled = _treeLevel == "target_area";

                    tsi = menuDropDown.Items.Add("Target area properties");
                    tsi.Name = "menuTargetAreaProp";
                    tsi.Enabled = _treeLevel == "target_area";

                    var sep = menuDropDown.Items.Add("-");
                    sep.Name = "menuSeparator1";

                    tsi = menuDropDown.Items.Add("New landing site");
                    tsi.Name = "menuNewLandingSite";
                    tsi.Enabled = _treeLevel == "target_area";

                    tsi = menuDropDown.Items.Add("Get landing sites from Google Earth KML");
                    tsi.Name = "menuLandingSiteFromKML";
                    tsi.Enabled = _treeLevel == "target_area" && FishingGrid.IsCompleteGrid25;

                    tsi = menuDropDown.Items.Add("Show landing sites on map");
                    tsi.Name = "menuShowOnMapLandingSites";
                    tsi.Enabled = _treeLevel == "target_area" && global.MapIsOpen;

                    tsi = menuDropDown.Items.Add("Landing site properties");
                    tsi.Name = "menuLandingSiteProp";
                    tsi.Enabled = _treeLevel == "landing_site";

                    sep = menuDropDown.Items.Add("-");
                    sep.Name = "menuSeparator2";

                    tsi = menuDropDown.Items.Add("New sampling");
                    tsi.Name = "menuNewSampling";
                    tsi.Enabled = (_treeLevel == "sampling" || _treeLevel == "landing_site" || _treeLevel == "gear") && FishingGrid.IsCompleteGrid25;

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

                    //only show browse submenu if catch name is scientific name
                    if (((ListView)Source).SelectedItems[0].Tag.ToString() == "Scientific")
                    {
                        tsi = menuDropDown.Items.Add("Species details");
                        tsi.Name = "menuSpeciesDetail";
                        tsi.Tag = ((ListView)Source).SelectedItems[0].Name;

                        tsi = menuDropDown.Items.Add("Local names");
                        tsi.Name = "menuCatchLocalNames";
                        tsi.Tag = ((ListView)Source).SelectedItems[0].Name;

                        ToolStripMenuItem subMenu = new ToolStripMenuItem();
                        subMenu.Text = "Browse on WWW";

                        CatchNameURLGenerator.CatchName = ((ListView)Source).SelectedItems[0].SubItems[1].Text;

                        foreach (var url in CatchNameURLGenerator.URLS)
                        {
                            ToolStripMenuItem subItem = new ToolStripMenuItem();
                            subItem.Text = url.Key;
                            subItem.Tag = url.Value;
                            subMenu.DropDownItems.Add(subItem);
                        }

                        subMenu.DropDownItemClicked += OnSubMenuDropDownClick;

                        menuDropDown.Items.Add(subMenu);
                    }
                    else
                    {
                        tsi = menuDropDown.Items.Add("Species names");
                        tsi.Name = "menuCatchSpeciesNames";
                        tsi.Tag = ((ListView)Source).SelectedItems[0].Name;
                    }
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

        private void OnSubMenuDropDownClick(object sender, ToolStripItemClickedEventArgs e)
        {
            e.ClickedItem.OwnerItem.Owner.Hide();
            Process.Start(e.ClickedItem.Tag.ToString());
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
                    if (Sampling.DeleteSampling(SamplingGUID))
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
            var n = 0;
            SizeColumns(lvMain);
            var CompleteGrid25 = FishingGrid.IsCompleteGrid25;
            if (_readEfforMonth || (_updatedEffortMonth.LandingSiteGuid.Length > 0 && _updatedEffortMonth.GearVariationGuid.Length > 0))
            {
                Sampling.SamplingSummaryForMonth(LSGUID, GearGUID, SamplingMonth);
                if (_updatedEffortMonth.LandingSiteGuid.Length > 0)
                {
                    _updatedEffortMonth.LandingSiteGuid = "";
                    _updatedEffortMonth.GearVariationGuid = "";
                }
                _readEfforMonth = false;
            }
            foreach (var item in Sampling.EffortMonth)
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
                n++;
            }
            SizeColumns(lvMain, false);
            Logger.Log($"filled sampling summary list:{n} rows");
        }

        private void frmMain_Activated(object sender, EventArgs e)
        {
            CancelButton = buttonOK.Visible ? buttonOK : null;
        }

        //public void EffortUpdated(string samplingMonth, string landingSiteGuid, string gearVariationGuid)
        //{
        //    _updatedEffortMonth.SamplingDate = samplingMonth;
        //    _updatedEffortMonth.GearVariationGuid = gearVariationGuid;
        //    _updatedEffortMonth.LandingSiteGuid = landingSiteGuid;
        //}

        private void OnMainForm_Load(object sender, EventArgs e)
        {
            if (global.AllRequiredFilesExists)
            {
                toolStripRecentlyOpened.DropDownItems.Clear();

                //setup an MRU that contains 10 items
                _mrulist = new mru("FAD3", toolStripRecentlyOpened, 10);

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
                        global.MDBPath = SavedMDBPath;
                        Names.GetGenus_LocalNames();
                        Names.GetLocalNames();
                        Gear.GetGearLocalNames();
                        statusPanelDBPath.Text = SavedMDBPath;
                        lblErrorFormOpen.Visible = false;
                        PopulateTree();

                        _sampling = new Sampling();
                        //_sampling.OnEffortUpdated += new Sampling.EffortUpdateHandler(EffortUpdated);
                        _sampling.OnEffortUpdated += EffortUpdated;

                        Sampling.SetUpUIElement();
                        _sampling.OnUIRowRead += new Sampling.ReadUIElement(OnUIRowRead);
                    }
                    else
                    {
                        Logger.Log("MDB file saved in registry not found");
                        lblErrorFormOpen.Visible = true;
                        lblTitle.Text = "";
                        lblErrorFormOpen.Text = "Please locate the database file where fisheries data is saved.\r\nYou can use the file open menu";
                        LockTheApp(true);
                    }
                }
                catch
                {
                    Logger.Log("Registry entry for mdb path not found");
                    lblErrorFormOpen.Visible = true;
                    lblTitle.Text = "";
                    lblErrorFormOpen.Text = "Please locate the database file where fisheries data is saved.\r\nYou can use the file open menu";
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
            menuItemZone50.Enabled = global.IsMapComponentRegistered;
            menuItemZone51.Enabled = global.IsMapComponentRegistered;
            menuItemBrowseLGUs.Visible = false;
            _updatedEffortMonth.GearVariationGuid = "";
            _updatedEffortMonth.LandingSiteGuid = "";

            _effortMonth.SampledMonth = "";
            _effortMonth.LandingSiteGuid = "";
            _effortMonth.GearVariationGuid = "";
            _enableUIEvent = true;
            global.mainForm = this;
        }

        private void EffortUpdated(Sampling s, EffortEventArg e)
        {
            _updatedEffortMonth.SamplingDate = e.SamplingDate;
            _updatedEffortMonth.GearVariationGuid = e.GearVarGuid;
            _updatedEffortMonth.LandingSiteGuid = e.LandingSiteGuid;
        }

        private void OnGenerateGridMapToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            e.ClickedItem.OwnerItem.Owner.Hide();

            var mf = MapperForm.GetInstance(this);
            if (!mf.Visible)
            {
                global.MappingMode = fad3MappingMode.grid25Mode;
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
                _grid25GenerateForm = new Grid25GenerateForm(global.MappingForm);
                _grid25GenerateForm.set_UTMZone(utmZone);
                _grid25GenerateForm.Show(global.MappingForm);
            }
            else
            {
                mf.BringToFront();
                if (e.ClickedItem.Name == "menuItemLayoutTemplateOpen")
                {
                    if (_grid25GenerateForm != null)
                    {
                        _grid25GenerateForm.OpenTemplate();
                    }
                    else
                    {
                        var ofd = new OpenFileDialog();
                        ofd.Title = "Open layout template";
                        ofd.Filter = "Layout file|*.lay|All files|*.*";
                        ofd.FilterIndex = 1;
                        DialogResult dr = ofd.ShowDialog();
                        if (dr == DialogResult.OK && Path.GetExtension(ofd.FileName) == ".lay")
                        {
                            Grid25LayoutHelperForm glhf = Grid25LayoutHelperForm.GetInstance(ofd.FileName);
                            if (glhf.Visible)
                            {
                                glhf.BringToFront();
                            }
                            else
                            {
                                glhf.Show(global.MappingForm);
                            }
                        }
                    }
                }
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
            else
            {
                //_sampling.OnUIRowRead -= new Sampling.ReadUIElement(OnUIRowRead);
                _enableUIEvent = false;
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
            lvMain.Enabled = false;
            treeMain.Enabled = false;
            _appIsLocked = true;
        }

        private void UnlockApp()
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
                                    oo.Enabled = true;
                                });
                            }
                        }
                    });
            }

            foreach (var c in toolbar.Items)
            {
                ((ToolStripButton)c).With(o =>
                {
                    o.Enabled = true;
                });
            }
            lvMain.Enabled = true;
            treeMain.Enabled = true;
            _appIsLocked = false;

            treeMain.SelectedNode = treeMain.Nodes["root"];
            TreeViewEventArgs e = new TreeViewEventArgs(treeMain.SelectedNode);
            OnTreeMainAfterSelect(null, e);
        }

        public void RefreshFisheriesInventory(string targetAreaGuid)
        {
            if (_treeLevel == "target_area" && _targetAreaGuid == targetAreaGuid)
            {
                SetUPLV("target_area");
            }
        }

        private void SetInventoryWindow(string inventoryGuid = "")
        {
            var gearInventoryForm = GearInventoryForm.GetInstance(_targetArea, this, inventoryGuid);

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

                case "menuFishingBoats":
                    FishingBoatForm fbf = FishingBoatForm.GetInstance();
                    if(fbf.Visible)
                    {
                        fbf.BringToFront();
                    }
                    else
                    {
                        fbf.TreeLevel = _treeLevel;
                        fbf.TargetArea = _targetArea;
                        if(_treeLevel=="landing_site")
                        {
                             fbf.LandingSite = _ls;
                        }
                        else
                        {
                            fbf.LandingSite = null;
                        }

                        fbf.Show(this);
                    }
                    
                    break;
                case "menuDeleteTreeItem":
                    var myTag = (Tuple<string, string, string>)treeMain.SelectedNode.Tag;
                    switch (_treeLevel)
                    {
                        case "target_area":
                            var result = MessageBox.Show("Are you sure you want to delete the selected target area", "Confirmation needed", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                            if (result == DialogResult.Yes)
                            {
                                if (TargetArea.Delete(myTag.Item1))
                                {
                                    treeMain.Nodes.Remove(treeMain.SelectedNode);
                                }
                                else
                                {
                                    if (TargetArea.LastError.Length > 0)
                                    {
                                        MessageBox.Show(TargetArea.LastError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    var tagf = TargetAreaGearsForm.GetInstance(_targetArea);
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
                    if (Enumerators.TargetAreaHasEnumerators(_targetAreaGuid))
                    {
                        SetInventoryWindow();
                    }
                    else
                    {
                        MessageBox.Show("Cannot create a new fisheries inventory project because target area has no enumerator",
                            "Cannot create inventory project",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Information);
                    }
                    break;

                case "menuNewTargetArea":
                    TargetAreaForm tf = new TargetAreaForm(this, IsNew: true);
                    tf.ShowDialog(this);
                    break;

                case "menuNewLandingSite":
                    LandingSiteForm fls = new LandingSiteForm(_targetArea, this, _ls, isNew: true);
                    fls.ShowDialog(this);
                    break;

                case "menuLandingSiteFromKML":
                    LandingSiteFromKMLForm lskf = LandingSiteFromKMLForm.GetInstance(_targetArea);
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
                    if (!LandingSiteMappingHandler.ShowLandingSitesOnMap(global.MappingForm.MapLayersHandler, _targetAreaGuid, global.MappingForm.GeoProjection, true))
                    {
                        MessageBox.Show("Landing sites could not be shown on the map.\r\nPlease check landing site coordinates", "Mapping error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

                case "menuNewSampling":
                    if (FishingGrid.IsCompleteGrid25)
                    {
                        if (Enumerators.TargetAreaHasEnumerators(_targetAreaGuid))
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
                    //EnumeratorForm ef = EnumeratorForm.GetInstance();
                    //ef.TargetArea = _targetArea;
                    //if (!ef.Visible)
                    //{
                    //    ef.Show(this);
                    //}
                    //else
                    //{
                    //    ef.BringToFront();
                    //}
                    EnumeratorForm ef = new EnumeratorForm();
                    ef.TargetArea = _targetArea;
                    ef.ShowDialog(this);
                    break;

                //this will show the samplings done by an enumerator
                case "menuEnumeratorDetail":
                    ef = EnumeratorForm.GetInstance(lvMain.SelectedItems[0].SubItems[1].Name);
                    ef.TargetArea = _targetArea;
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
                    AOIForm.TargetArea = _targetArea;
                    break;

                case "menuLandingSiteProp":
                    fls = new LandingSiteForm(_targetArea, this, _ls);
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

                case "menuSpeciesDetail":
                    lvi = lvCatch.SelectedItems[0];

                    SpeciesNameForm snf = new SpeciesNameForm(this);
                    snf.SpeciesGuid = lvCatch.SelectedItems[0].SubItems[1].Name;
                    snf.ReadOnly = true;
                    snf.ShowDialog(this);

                    break;

                case "menuCatchLocalNames":
                    ShowLocalSpeciesNames(lvCatch.SelectedItems[0].SubItems[1].Name, lvCatch.SelectedItems[0].SubItems[1].Text, Identification.Scientific);
                    break;

                case "menuCatchSpeciesNames":
                    ShowLocalSpeciesNames(lvCatch.SelectedItems[0].SubItems[1].Name, lvCatch.SelectedItems[0].SubItems[1].Text, Identification.LocalName);
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
            CatchCompositionForm ccf = new CatchCompositionForm(IsNew, this, _samplingGUID, _referenceNumber, _weightOfCatch, _weightOfSample);
            ccf.ShowDialog(this);
        }

        private void NewSamplingForm()
        {
            ListViewNewSampling();
            //lvMain.Items.Clear();
            //_enableUIEvent = true;
            var f3 = new SamplingForm
            {
                IsNew = true,
                GearClassName = _gearClassName,
                GearClassGuid = _gearClassGUID,
                GearVarGuid = _gearVarGUID,
                GearVarName = _gearVarName,
                TargetAreaGuid = TargetAreaGuid,
                TargetAreaName = TargetAreaName,
                LandingSiteName = _landingSiteName,
                LandingSiteGuid = _landingSiteGuid,
                TargetArea = _targetArea
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
                    SamplingDetailClosed?.Invoke(null, EventArgs.Empty);
                    SetupCatchListView(Show: false);
                    BackToSamplingMonth();
                    break;

                case "buttonCatch":
                    SetupCatchListView();
                    SizeColumns(lvCatch);
                    ShowCatchComposition(_samplingGUID);
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
            ((ListView)sender).With((Action<ListView>)(o =>
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

                                case "target_area":
                                    if (item.Tag != null)
                                    {
                                        if (item.Tag.ToString() == "target_area_data")
                                        {
                                            var myTag = (Tuple<string, string, string>)treeMain.SelectedNode.Tag;
                                            _targetArea.TargetAreaGuid = myTag.Item1;
                                            TargetAreaForm f = new TargetAreaForm(this, IsNew: false);
                                            f.Show();
                                            f.TargetArea = _targetArea;
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
                                        Process.Start(Path.GetDirectoryName((string)global.MDBPath));
                                    }
                                    break;

                                case "landing_site":
                                    if (item.Tag != null)
                                    {
                                        if (item.Tag.ToString() == "gearSampled")
                                        {
                                            var nd = treeMain.SelectedNode.Nodes[$"{_landingSiteGuid}|{item.Name}"];
                                            if (nd != null)
                                            {
                                                treeMain.SelectedNode = nd;
                                                nd.Expand();
                                            }
                                        }
                                        else if (item.Tag.ToString() == "landing_site")
                                        {
                                            LandingSiteForm fls = new LandingSiteForm(_targetArea, this, _ls);
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
                                    ShowCatchDetailEx(_samplingGUID);
                                    item.BackColor = Color.Gainsboro;
                                    break;

                                case "samplingDetail":
                                    if (item.Name == "GearSpecs")
                                    {
                                        var s = ManageGearSpecsClass.GetSampledSpecsEx(_samplingGUID);
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
                                        _enableUIEvent = false;
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
            }));
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
                            //global.MappingForm.MapFishingGround(lvh.Item.SubItems[4].Text, FishingGrid.UTMZone);
                            if (lvMain.SelectedItems.Count > 0)
                            {
                                global.MappingForm.MapFishingGround(lvMain.SelectedItems[0].Name, FishingGrid.UTMZone);
                            }
                        }
                        _referenceNumber = lvh.Item.Text;
                    }
                    if (e.Button == MouseButtons.Right)
                    {
                        if (_treeLevel == "sampling" || _treeLevel == "target_area" || _treeLevel == "landing_site")
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
                        if (e.Clicks == 2 && lvCatch.Items.Count == 0)
                        {
                            ShowCatchCompositionForm(true);
                        }
                        else if (lvh.Item != null)
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
                        if (e.Clicks == 2 && lvLF_GMS.Items.Count == 0)
                        {
                            if (_catchSubRow == fad3CatchSubRow.LF)
                            {
                                ShowLFForm(IsNew: true);
                            }
                            else
                            {
                                ShowGMSForm(_taxa, true);
                            }
                        }
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
                    try
                    {
                        CreateNewDatabaseForm f = new CreateNewDatabaseForm(this);
                        f.ShowDialog(this);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex.Message, "MainForm.cs", "MainForm.OnMenuFile_DropDownItemClicked");
                    }
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
                    if (global.IsMapComponentRegistered)
                    {
                        AboutFadForm f = new AboutFadForm();
                        f.ShowDialog(this);
                    }
                    else
                    {
                        AboutFADForm2 f = new AboutFADForm2();
                        f.ShowDialog(this);
                    }
                    break;

                case "onlineManual":
                    var helpFile = $@"{global.ApplicationPath}\FAD3 Manual.chm";
                    if (File.Exists(helpFile))
                    {
                        Help.ShowHelp(this, helpFile);
                    }
                    break;

                case "diagnostics":
                    //DBCheck.ListDBTables(global.MDBPath, Application.ProductVersion);
                    FADDiagnostics.Diagnose(global.MDBPath, Application.ProductVersion);
                    MessageBox.Show("Finished writing diagnostics to log!", "Diagnostics finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                case "spatio-temporal":
                    var spatioTemporalMappingForm = Mapping.Forms.SpatioTemporalMappingForm.GetInstance();
                    if (spatioTemporalMappingForm.Visible)
                    {
                        spatioTemporalMappingForm.BringToFront();
                    }
                    else

                    {
                        spatioTemporalMappingForm.Show(global.MappingForm);
                    }
                    break;

                case "downloadSpatioTemporal":
                    var downloadSpTempForm = Mapping.Forms.ERDDAPSelectDatasetForm.GetInstance();
                    if (downloadSpTempForm.Visible)
                    {
                        downloadSpTempForm.BringToFront();
                    }
                    else

                    {
                        downloadSpTempForm.Show(global.MappingForm);
                    }
                    break;

                case "browseLGUs":
                    LGUSForm lf = LGUSForm.GetInstance();
                    if (lf.Visible)
                    {
                        lf.BringToFront();
                    }
                    else
                    {
                        lf.Show(this);
                    }
                    break;
            }
        }

        public void SetMapDependendMenus()
        {
            spatioTemporalMapMenuItem.Enabled = global.MapIsOpen;
            downloadSpatiotemporalDataToolStripMenuItem.Enabled = global.MapIsOpen;
            menuItemZone50.Enabled = !global.MapIsOpen;
            menuItemZone51.Enabled = !global.MapIsOpen;
            menuItemLayoutTemplateOpen.Enabled = global.MapIsOpen;

            if (!global.MapIsOpen)
            {
                _grid25GenerateForm = null;
            }
        }

        private void OnToolbar_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem tsi = e.ClickedItem;
            switch (tsi.Name)
            {
                case "tsButtonAbout":
                    if (global.IsMapComponentRegistered)
                    {
                        AboutFadForm f = new AboutFadForm();
                        f.ShowDialog(this);
                    }
                    else
                    {
                        AboutFADForm2 f = new AboutFADForm2();
                        f.ShowDialog(this);
                    }
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
                    AllSpeciesForm asf = AllSpeciesForm.GetInstance(this);
                    if (asf.Visible)
                    {
                        asf.BringToFront();
                    }
                    else
                    {
                        asf.Show(this);
                    }
                    break;

                case "tsButtonLN2SN":
                    CatchLocalNamesForm clnf = CatchLocalNamesForm.GetInstance(Identification.Scientific);
                    if (clnf.Visible)
                    {
                        clnf.BringToFront();
                    }
                    else
                    {
                        clnf.Show(this);
                    }
                    break;

                case "tsButtonReport":
                    if (_treeLevel == "target_area" && _targetAreaName.Length > 0 && _targetAreaGuid.Length > 0)
                    {
                        DatabaseReportForm drf = DatabaseReportForm.GetInstance(_treeLevel, _targetArea);
                        if (drf.Visible)
                        {
                            drf.BringToFront();
                        }
                        else
                        {
                            drf.Show(this);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select a target area", "Select a target area", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

                case "tsButtonMap":
                    if (global.IsMapComponentRegistered)
                    {
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
                    }
                    else
                    {
                        MessageBox.Show("Mapwindows mapping component is not installed\r\n" +
                                         "You will not be able to use the map", "Mapping component is not installed",
                                         MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

                case "tsButtonExit":
                    AppExit();

                    break;
            }
        }

        private void OntreeMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ConfigDropDownMenu(treeMain);
            }

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
                        _landingSiteGuid = myTag.Item1;
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
                        nd1.ImageKey = Gear.GearClassImageKeyFromGearClasName(item.GearClassName);
                    }
                    else if (myTag.Item3 == "gear")
                    {
                        _landingSiteGuid = myTag.Item1;
                        _gearVarGUID = myTag.Item2;
                        if (n == 0)
                        {
                            e.Node.FirstNode.Text = item.SamplingMonthYear;
                            nd1 = e.Node.FirstNode;
                        }
                        else
                        {
                            nd1 = e.Node.Nodes.Add(item.SamplingMonthYear);
                        }
                        if (_landingSiteGuid.Length == 0 && _gearVarGUID.Length == 0)
                        {
                            myTag = (Tuple<string, string, string>)e.Node.Tag;
                            _landingSiteGuid = myTag.Item1;
                            _gearVarGUID = myTag.Item2;
                        }
                        nd1.Tag = Tuple.Create(_landingSiteGuid, _gearVarGUID, "sampling");
                        nd1.Name = $"{_landingSiteGuid}|{_gearVarGUID}|{item.SamplingMonthYear}";
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
                _landingSiteName = "";
                _gearVarName = "";
                _gearClassName = "";
                _landingSiteGuid = "";
                _gearVarGUID = "";
                _gearClassGUID = "";
                TargetAreaGuid = "";
                TargetAreaName = "";

                TreeNode nd = treeMain.SelectedNode;
                ResetTheBackColor(treeMain);
                nd.BackColor = Color.Gainsboro;

                var myTag = (Tuple<string, string, string>)nd.Tag;
                _treeLevel = myTag.Item3;

                statusPanelTargetArea.Width = _statusPanelWidth;
                switch (_treeLevel)
                {
                    case "gear":
                        _targetAreaName = treeMain.SelectedNode.Parent.Parent.Text;
                        this.TargetAreaGuid = treeMain.SelectedNode.Parent.Parent.Name;
                        _landingSiteName = treeMain.SelectedNode.Parent.Text;
                        _landingSiteGuid = treeMain.SelectedNode.Parent.Name;
                        _gearVarName = treeMain.SelectedNode.Text;
                        _gearVarGUID = myTag.Item2;
                        var rv = Gear.GearClassGuidNameFromGearVarGuid(_gearVarGUID);
                        _gearClassName = rv.Value;
                        _gearClassGUID = rv.Key;
                        _LSNode = e.Node.Parent;

                        break;

                    case "sampling":
                        _targetAreaName = treeMain.SelectedNode.Parent.Parent.Parent.Text;
                        this.TargetAreaGuid = ((Tuple<string, string, string>)treeMain.SelectedNode.Parent.Parent.Parent.Tag).Item1;
                        _landingSiteName = treeMain.SelectedNode.Parent.Parent.Text;
                        _landingSiteGuid = myTag.Item1;
                        _gearVarName = treeMain.SelectedNode.Parent.Text;
                        _gearVarGUID = myTag.Item2;
                        rv = Gear.GearClassGuidNameFromGearVarGuid(_gearVarGUID);
                        _gearClassName = rv.Value;
                        _gearClassGUID = rv.Key;
                        _LSNode = e.Node.Parent.Parent;
                        _samplingMonth = e.Node.Text;
                        _readEfforMonth = _effortMonth.SampledMonth != _samplingMonth || _effortMonth.GearVariationGuid != _gearVarGUID || _effortMonth.LandingSiteGuid != _landingSiteGuid;
                        _effortMonth.SampledMonth = _samplingMonth;
                        _effortMonth.GearVariationGuid = _gearVarGUID;
                        _effortMonth.LandingSiteGuid = _landingSiteGuid;

                        break;

                    case "target_area":
                        _targetAreaName = treeMain.SelectedNode.Text;
                        this.TargetAreaGuid = treeMain.SelectedNode.Name;
                        _landingSiteName = "";
                        _landingSiteGuid = "";
                        _gearVarName = "";
                        _gearVarGUID = "";
                        DatabaseReportForm drf = DatabaseReportForm.GetInstance();
                        if (drf != null)
                        {
                            drf.TargetArea = _targetArea;
                            drf.BringToFront();
                        }
                        break;

                    case "root":
                        break;

                    case "landing_site":
                        _targetAreaName = treeMain.SelectedNode.Parent.Text;
                        this.TargetAreaGuid = treeMain.SelectedNode.Parent.Name;
                        _landingSiteName = treeMain.SelectedNode.Text;
                        _landingSiteGuid = treeMain.SelectedNode.Name;
                        _gearVarName = "";
                        _gearVarGUID = "";
                        _LSNode = e.Node;
                        _ls = new Landingsite(_landingSiteGuid, _targetAreaGuid);
                        _ls.LandingSiteName = _landingSiteName;
                        break;
                }

                SetUPLV(_treeLevel);
                statusPanelTargetArea.Text = _targetAreaName;
                statusPanelLandingSite.Text = _landingSiteName;
                statusPanelGearUsed.Text = _gearVarName;
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
            if (_enableUIEvent)
            {
                ListViewItem lvi = lvMain.Items.Add(e.RowLabel);
                lvi.Name = e.Key;
                lvi.SubItems.Add("");
            }
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
                    myNode.Tag = Tuple.Create(item.AOIGuid, "", "target_area");
                    myNode.ImageKey = "target_area";

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
            if (global.MDBPath.Length == 0)
            {
                ofd.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            }
            else
            {
                ofd.InitialDirectory = global.MDBPath;
            }
            ofd.Filter = "Microsoft Access Data File (.mdb)|*.mdb";
            ofd.ShowDialog();
            filename = ofd.FileName;
            if (filename.Length > 0)
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
                gearNode.Text = Gear.GearVarNameFromGearGuid(GearVarGuid);
                gearNode.Tag = Tuple.Create(LandingSiteGuid, GearVarGuid, "gear");
                gearNode.ImageKey = Gear.GearVarNodeImageKeyFromGearVar(GearVarGuid);

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
                    panel1.Controls["panelSub"].Visible = true;
                    panel1.Controls["lvCatch"].Visible = true;
                }
                else
                {
                    _subListExisting = true;
                    lvCatch = new ListView();
                    panel1.Controls.Add(lvCatch);
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
                        o.MouseClick += OnListView_MouseClick;
                        o.ContextMenu = lvMain.ContextMenu;
                        o.HideSelection = false;
                    });

                    lvLF_GMS = new ListView();
                    panel1.Controls.Add(lvLF_GMS);
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

                    panel1.Controls.Add(SubPanel);
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
                    panel1.Controls["panelSub"].Visible = false;
                    ((ListView)panel1.Controls["lvCatch"]).With(o =>
                    {
                        o.Items.Clear();
                        o.Visible = false;
                    });

                    ((ListView)panel1.Controls["lvLF_GMS"]).With(o =>
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

        public void RefreshTargetAreaEnumerators(string targetAreaGUID)
        {
            if (_targetAreaGuid == targetAreaGUID && _treeLevel == "target_area")
            {
                SetUPLV(_treeLevel);
            }
        }

        /// <summary>
        /// this will show a list of summaries depending on the level of the node selected
        /// level could be root, target area, Landing site, gear used, and sampling month
        /// </summary>
        /// <param name="treeLevel"></param>
        public void SetUPLV(string treeLevel)
        {
            lvMain.Visible = false;
            //var savedColWidthExist = true;
            //var colWidthAdjusted = false;
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
            switch (treeLevel)
            {
                case "root":
                    lblTitle.Text = "Database summary";
                    break;

                case "target_area":
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

            lvMain.Tag = treeLevel;
            SizeColumns(lvMain);
            //}

            //add rows to the listview
            switch (treeLevel)
            {
                case "sampling":
                    FillLVSamplingSummary(_landingSiteGuid, _gearVarGUID, _samplingMonth);
                    //colWidthAdjusted = true;
                    break;

                case "samplingDetail":
                    break;

                case "gear":
                    var myTag = (Tuple<string, string, string>)treeMain.SelectedNode.Tag;
                    Gear.GearVarGUID = myTag.Item2;
                    var n = 0;
                    lvi = lvMain.Items.Add("Months sampled");
                    foreach (var item in Gear.MonthsSampledByGear(myTag.Item1))
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
                    lvi.SubItems.Add(global.MDBPath);
                    lvi.Tag = "databasePath";

                    //add sampled years with count for entire database
                    lvi = lvMain.Items.Add("");
                    lvi = lvMain.Items.Add("Sampled years");
                    foreach (string item in _targetArea.SampledYears())
                    {
                        if (i > 0)
                        {
                            lvi = lvMain.Items.Add("");
                        }
                        lvi.SubItems.Add(item.ToString());
                        i++;
                    }

                    //add target area with count for entire database
                    lvi = lvMain.Items.Add("");
                    lvi = lvMain.Items.Add("Target areas");
                    i = 0;

                    foreach (KeyValuePair<string, string> kv in _targetArea.TargetAreaWithSamplingCount())
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

                case "target_area":
                    //arr = treeMain.SelectedNode.Tag.ToString().Split(',');
                    myData = _targetArea.TargetAreaData();
                    var arr = myData.Split('|');

                    //add AOI data for form
                    lvi = lvMain.Items.Add("name", "Name", null);
                    lvi.SubItems.Add(arr[0]);
                    lvi.Tag = "target_area_data";
                    lvi = lvMain.Items.Add("Code");
                    lvi.SubItems.Add(arr[1]);
                    lvi.Tag = "target_area_data";
                    lvi = lvMain.Items.Add("MBR");
                    lvi.SubItems.Add("");
                    lvi.Tag = "target_area_data";
                    lvi = lvMain.Items.Add("Grid system");
                    lvi.Tag = "target_area_data";
                    lvi.SubItems.Add(FishingGrid.GridTypeName);

                    // add no. of samplings for this AOI
                    lvi = lvMain.Items.Add("");
                    lvi = lvMain.Items.Add("No. of samplings");
                    lvi.SubItems.Add(_targetArea.SampleCount().ToString());

                    //add sampled years with count
                    lvi = lvMain.Items.Add("Years sampling");
                    i = 0;
                    foreach (KeyValuePair<string, string> item in _targetArea.ListYearsWithSamplingCount())
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
                    foreach (KeyValuePair<string, string> item in Enumerators.EnumeratorsWithCount(_targetAreaGuid))
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
                        foreach (KeyValuePair<string, string> item in Enumerators.AOIEnumeratorsList(_targetAreaGuid))
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
                    foreach (var item in _targetArea.ListLandingSiteWithSamplingCount())
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
                    var inventory = new FishingGearInventory(_targetArea);
                    lvMain.Items.Add("");
                    lvi = lvMain.Items.Add("fisheryInventories", "Fishery inventories", null);
                    lvi.Tag = "inventory";
                    if (inventory.Inventories.Count > 0)
                    {
                        foreach (var item in inventory.Inventories)
                        {
                            if (n > 0)
                            {
                                lvi = lvMain.Items.Add("");
                            }
                            lvi.Name = item.Key;
                            lvi.Tag = "inventory";
                            lvi.SubItems.Add(item.Value.InventoryName);
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
                    _ls.LandingSiteGUID = _landingSiteGuid;
                    _ls.LandingSiteName = _landingSiteName;
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

            SizeColumns(lvMain, false);
            lvMain.ResumeLayout();
            lvMain.Visible = true;
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
        /// updates the list of fishery inventories in a target area
        /// </summary>
        /// <param name="inventory"></param>
        public void UpdateInventoryList(FishingGearInventory inventory)
        {
            int n = 0;
            if (_treeLevel == "target_area")
            {
                foreach (ListViewItem item in lvMain.Items)
                {
                    if (item.Tag?.ToString() == "inventory")
                    {
                        if (n == 0)
                        {
                            item.SubItems[1].Text = "0";
                            item.Name = "fisheryInventories";
                        }
                        else
                        {
                            lvMain.Items.Remove(item);
                        }
                        n++;
                    }
                }

                n = 0;
                ListViewItem lvi = new ListViewItem();
                foreach (var item in inventory.Inventories)
                {
                    if (n == 0)
                    {
                        lvi = lvMain.Items["fisheryInventories"];
                    }
                    else
                    {
                        lvi = lvMain.Items.Add("");
                    }
                    lvi.Name = item.Key;
                    lvi.Tag = "inventory";
                    lvi.SubItems[1].Text = item.Value.InventoryName;
                    n++;
                }
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
                global.MDBPath = MDBFile;
                RegistryKey rk = Registry.CurrentUser.CreateSubKey("SOFTWARE\\FAD3");
                rk.SetValue("mdbPath", MDBFile, RegistryValueKind.String);
                rk.Close();
                lblErrorFormOpen.Visible = false;
                Names.GetGenus_LocalNames();
                Names.GetLocalNames();
                Gear.GetGearLocalNames();
                PopulateTree();
                treeMain.SelectedNode = treeMain.Nodes["root"];
                statusPanelDBPath.Text = MDBFile;
                if (_appIsLocked)
                {
                    UnlockApp();
                }
                return true;
            }
            else
            {
                Logger.Log("MDB file saved in registry not found");
                lblErrorFormOpen.Visible = true;
                lblTitle.Text = "";
                lblErrorFormOpen.Text = "Please locate the database file where fisheries data is saved.\r\nYou can use the file open menu";
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
                foreach (var item in LengthFreq.LFData(CatchRowGuid))
                {
                    var lvi = new ListViewItem(new string[]
                    {
                                        n.ToString(),
                                        item.Value.len.ToString(),
                                        item.Value.freq.ToString()
                    });
                    lvi.Name = item.Key;
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
            ShowCatchComposition(_samplingGUID);
        }

        private void ShowLocalSpeciesNames(string nameGuid, string catchName, Identification idType)
        {
            //CatchLocalNamesForm clnf = CatchLocalNamesForm.GetInstance(nameGuid, this);
            CatchLocalNamesForm clnf = CatchLocalNamesForm.GetInstance(nameGuid, idType, this);
            if (clnf.Visible)
            {
                clnf.BringToFront();
            }
            else
            {
                switch (idType)
                {
                    case Identification.LocalName:
                        clnf.LocalName = catchName;
                        break;

                    case Identification.Scientific:
                        clnf.SpeciesName = catchName;
                        break;
                }

                clnf.Show(this);
                _catchLocalNamesForm = clnf;
            }
        }

        public void CatchhLocalNamesFormClosed()
        {
            _catchLocalNamesForm = null;
        }

        private void ShowCatchComposition(string SamplingGuid)
        {
            if (_subListExisting)
            {
                lvCatch.Items.Clear();
                int n = 1;
                double computedWeight = 0;
                double totalCatchWt = 0;
                int totalCatchCount = 0;
                double totalComputedWeight = 0;
                int totalComputedCount = 0;
                int computedCount = 0;
                foreach (KeyValuePair<string, CatchLine> kv in CatchComposition.RetrieveCatchComposition(_samplingGUID))
                {
                    if (kv.Value.FromTotalCatch)
                    {
                        computedWeight = kv.Value.CatchWeight;
                        if (kv.Value.CatchCount == null)
                        {
                            computedCount = (int)((kv.Value.CatchWeight / kv.Value.CatchSubsampleWt) * kv.Value.CatchSubsampleCount);
                        }
                        else
                        {
                            computedCount = (int)kv.Value.CatchCount;
                        }
                    }
                    else
                    {
                        if (_weightOfSample != null)
                        {
                            computedWeight = (_weightOfCatch / (double)_weightOfSample) * kv.Value.CatchWeight;
                            if (kv.Value.CatchCount == null)
                            {
                                computedCount = (int)((kv.Value.CatchWeight / kv.Value.CatchSubsampleWt) * kv.Value.CatchSubsampleCount);
                            }
                            else
                            {
                                computedCount = (int)((_weightOfCatch / (double)_weightOfSample) * kv.Value.CatchCount);
                            }
                        }
                        else
                        {
                        }
                    }
                    var lvi = new ListViewItem(new string[]
                    {
                        n.ToString(),
                        kv.Value.CatchName,
                        kv.Value.CatchWeight.ToString(),
                        kv.Value.CatchCount.ToString(),
                        kv.Value.CatchSubsampleWt.ToString(),
                        kv.Value.CatchSubsampleCount.ToString(),
                        kv.Value.FromTotalCatch.ToString(),
                        //kv.Value.TaxaNumber.ToString()
                        computedWeight.ToString("N2"),
                        computedCount.ToString()
                    });
                    lvi.Name = kv.Key;
                    lvi.SubItems[1].Name = kv.Value.CatchNameGUID;
                    lvi.Tag = kv.Value.NameType.ToString();
                    lvCatch.Items.Add(lvi);
                    totalCatchWt += kv.Value.CatchWeight;
                    totalCatchCount += kv.Value.CatchCount == null ? 0 : (int)kv.Value.CatchCount;
                    totalComputedWeight += computedWeight;
                    totalComputedCount += computedCount;
                    n++;
                }

                lvCatch.Items.Add("");
                var lviTotal = lvCatch.Items.Add("");
                lviTotal.SubItems.Add("Totals");
                lviTotal.SubItems.Add(totalCatchWt.ToString());
                lviTotal.SubItems.Add(totalCatchCount.ToString());
                lviTotal.SubItems.Add("");
                lviTotal.SubItems.Add("");
                lviTotal.SubItems.Add("");
                lviTotal.SubItems.Add(totalComputedWeight.ToString("N2"));
                lviTotal.SubItems.Add(totalComputedCount.ToString());

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
            //_sampling.OnUIRowRead += new Sampling.ReadUIElement(OnUIRowRead);
            _enableUIEvent = true;
            _sampling.ReadUIFromXML();
            var DateEncoded = "";

            if (SamplingGUID.Length > 0)
            {
                //we fill up the list view from the _Sampling class variable.
                _sampling.SamplingGUID = SamplingGUID;
                Dictionary<string, string> effortData = _sampling.CatchAndEffort();

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
                            lvi.SubItems[1].Text = ManageGearSpecsClass.GetSampledSpecsEx(_samplingGUID, Truncated: true);
                            break;

                        case "AdditionalFishingGround":
                            foreach (var item in FishingGrid.AdditionalFishingGrounds(_samplingGUID))
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
                            //double? wtCatch = null;
                            if (double.TryParse(effortData[lvi.Name], out double v))
                            {
                                _weightOfCatch = v;
                            }
                            //_weightOfCatch = double.Parse(effortData[lvi.Name]);
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
            SizeColumns(lvMain, false);
        }

        private void ShowGMSForm(Taxa taxa, bool isNew = false)
        {
            if (taxa != Taxa.To_be_determined)
            {
                GMSDataEntryForm fgms = new GMSDataEntryForm(isNew, _sampling,
                                          lvCatch.SelectedItems[0].Name,
                                          lvCatch.SelectedItems[0].SubItems[1].Text, _taxa, this);
                fgms.ShowDialog(this);
            }
        }

        private void ShowLFForm(bool IsNew = false)
        {
            LengthFreqForm lff = new LengthFreqForm(IsNew, _sampling,
                                      lvCatch.SelectedItems[0].Name,
                                      lvCatch.SelectedItems[0].SubItems[1].Text,
                                      int.Parse(lvCatch.SelectedItems[0].SubItems[3].Text),
                                      this);

            lff.ShowDialog(this);
            Show_LF_GMS_List(lvCatch.SelectedItems[0].Name);
        }

        private void ShowSamplingDetailForm()
        {
            SamplingForm fs = new SamplingForm();
            fs.SamplingGUID = _samplingGUID;
            fs.ListViewSamplingDetail(lvMain);
            fs.TargetArea = _targetArea;
            fs.TargetAreaGuid = _targetAreaGuid;
            fs.Parent_Form = this;
            fs.VesselDimension(_vesLength, _vesWidth, _vesHeight);
            fs.Show(this);
        }

        private void statusPanelDBPath_DoubleClick(object sender, EventArgs e)
        {
            Process.Start(Path.GetDirectoryName(global.MDBPath));
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
                    node.ImageKey = Gear.GearClassImageKeyFromGearClasName(item.gearClassName);
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

        private void OnMenuLocalNameItemClick(object sender, ToolStripItemClickedEventArgs e)
        {
            e.ClickedItem.OwnerItem.Owner.Hide();
            Identification idType = Identification.LocalName;
            switch (e.ClickedItem.Name)
            {
                case "itemViewBySciName":
                    idType = Identification.Scientific;
                    break;

                case "itemViewByLocalName":
                    idType = Identification.LocalName;
                    break;
            }

            CatchLocalNamesForm clnf = CatchLocalNamesForm.GetInstance(idType);
            if (clnf.Visible)
            {
                clnf.BringToFront();
                clnf.IDType = idType;
            }
            else
            {
                clnf.Show(this);
            }
        }

        private void OnListView_MouseClick(object sender, MouseEventArgs e)
        {
            switch (((ListView)sender).Name)
            {
                case "lvCatch":
                    switch (lvCatch.SelectedItems[0].Tag.ToString())
                    {
                        case "Scientific":
                            var speciesName = lvCatch.SelectedItems[0].SubItems[1].Text;
                            if (_catchLocalNamesForm != null)
                            {
                                _catchLocalNamesForm.SpeciesName = speciesName;
                                _catchLocalNamesForm.SpeciesGuid = lvCatch.SelectedItems[0].SubItems[1].Name;
                            }
                            break;

                        case "LocalName":
                            var localName = lvCatch.SelectedItems[0].SubItems[1].Text;
                            if (_catchLocalNamesForm != null)
                            {
                                _catchLocalNamesForm.LocalName = localName;
                                _catchLocalNamesForm.LocalNameGuid = lvCatch.SelectedItems[0].SubItems[1].Name;
                            }
                            break;
                    }

                    break;
            }
        }

        private void OnDropDownOpening(object sender, EventArgs e)
        {
            switch (((ToolStripMenuItem)sender).Name)
            {
                case "generateGridMapToolStripMenuItem":
                    menuItemLayoutTemplateOpen.Enabled = global.MapIsOpen;
                    break;
            }
        }
    }
}
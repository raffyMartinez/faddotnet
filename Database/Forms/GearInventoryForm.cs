using FAD3.Database.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace FAD3.Database.Forms
{
    /// <summary>
    /// UI for viewing fisheries inventory
    /// </summary>
    public partial class GearInventoryForm : Form
    {
        private static GearInventoryForm _instance;
        private TargetArea _targetArea;
        private FishingGearInventory _inventory;
        private string _treeLevel;
        private string _inventoryGuid;
        private string _barangayInventoryGuid;

        private int _sitioCountCommercial;
        private int _sitioCountFishers;
        private int _sitioCountMunicipalMotorized;
        private int _sitioCountMunicipalNonMotorized;
        private TreeNode _sitioNode;
        private TreeNode _provinceNode;
        private bool _refreshSitioNumbers;
        private string _currentGearInventoryGuid;
        private ListViewItem _listTargetHit;

        private bool _clickFromTree;
        private MainForm _parentForm;
        private Dictionary<string, string> _deleteInventoryArgs = new Dictionary<string, string>();
        private bool _importInventorySuccess;
        private string _importInventoryProjectName;
        private string _importInventoryProjectGuid;

        //function to ensure that only one instance of the form is open
        public static GearInventoryForm GetInstance(TargetArea aoi, MainForm parentForm, string inventoryGuid = "")
        {
            if (_instance == null) _instance = new GearInventoryForm(aoi, parentForm, inventoryGuid);
            return _instance;
        }

        /// <summary>
        /// form constructor
        /// </summary>
        /// <param name="targetArea"></param>
        /// <param name="inventoryGuid">this value is "" if we open the form using right click menu on tree. This is not blank if we double click on a project on the main form targetarea listview summary</param>
        public GearInventoryForm(TargetArea targetArea, MainForm parentForm, string inventoryGuid)
        {
            InitializeComponent();
            _targetArea = targetArea;
            _inventory = new FishingGearInventory(targetArea);
            _inventory.InventoryLevel += OnReadInventoryLevel;
            _inventoryGuid = inventoryGuid;
            _parentForm = parentForm;
            treeInventory.ImageList = global.mainForm.treeImages;
        }

        /// <summary>
        /// event handler during different stages of importing a fisheries inventory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReadInventoryLevel(object sender, FisheriesInventoryImportEventArg e)
        {
            if (e.Cancel)
            {
                MessageBox.Show(e.CancelReason, "Import cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (e.ImportCompleted)
            {
                if (treeInventory.Nodes["root"].FirstNode.Text == "***dummy***")
                {
                    ConfigListView_CrossThread(treeInventory.Nodes["root"]);
                }
                else
                {
                }
            }
            else
            {
                switch (e.InventoryLevel)
                {
                    case FisheriesInventoryLevel.Project:
                        bool projectFound = false;
                        foreach (var item in _inventory.ProjectsInTargetArea(_targetArea.TargetAreaName))
                        {
                            if (e.ProjectName == item.projectName)
                            {
                                projectFound = true;
                                break;
                            }
                        }
                        if (!projectFound)
                        {
                            if (e.ImportInventoryAction == ImportInventoryAction.ImportIntoNew)
                            {
                                DialogResult dr = MessageBox.Show("Do you want to create a new inventory project entitled\r\n" +
                                    e.ProjectName + "?", "Create new inventory project", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                                e.Cancel = dr == DialogResult.Cancel;
                            }
                            else if (e.ImportInventoryAction == ImportInventoryAction.ImportIntoExisting)
                            {
                                e.Cancel = false;
                            }
                            else
                            {
                                e.Cancel = true;
                            }
                        }
                        else
                        {
                            //project with similar name as imported project exists
                        }
                        break;

                    case FisheriesInventoryLevel.TargetArea:
                        switch (e.ImportInventoryAction)
                        {
                            case ImportInventoryAction.ImportDoNothing:
                                break;

                            case ImportInventoryAction.ImportIntoExisting:
                                break;

                            case ImportInventoryAction.ImportIntoNew:
                                e.Cancel = false;
                                break;
                        }
                        break;

                    case FisheriesInventoryLevel.Province:
                        break;

                    case FisheriesInventoryLevel.Municipality:
                        break;

                    case FisheriesInventoryLevel.Barangay:
                        break;

                    case FisheriesInventoryLevel.Sitio:
                        break;

                    case FisheriesInventoryLevel.FisherVessel:
                        break;

                    case FisheriesInventoryLevel.Gear:
                        break;
                }
            }
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
            global.SaveFormSettings(this);
        }

        private void SetupListview()
        {
            lvInventory.View = View.Details;
            lvInventory.FullRowSelect = true;
            lvInventory.SmallImageList = global.mainForm.treeImages;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            Text = "Inventory of fishers, fishing vessels and gears";
            SetupListview();
            var ndRoot = treeInventory.Nodes.Add("root", "Fishery inventory");
            ndRoot.ImageKey = "ListFolder";
            ndRoot.Tag = "root";
            ndRoot.SelectedImageKey = ndRoot.ImageKey;

            //add inventory project nodes to root
            foreach (var item in _inventory.Inventories)
            {
                var subInventoryProjectNode = ndRoot.Nodes.Add(item.Key, item.Value.InventoryName);
                subInventoryProjectNode.Tag = "targetAreaInventory";
                subInventoryProjectNode.Nodes.Add("***dummy***");
                subInventoryProjectNode.ImageKey = "LayoutTransform";
            }
            if (ndRoot.Nodes.Count == 0)
            {
                ndRoot.Nodes.Add("***dummy***");
            }
            _treeLevel = ndRoot.Tag.ToString();
            ConfigListView(treeInventory.Nodes[_treeLevel]);

            //_inventoryGuid.Lenght > 0 when we double click on an inventory project in main form listview
            if (_inventoryGuid.Length > 0)
            {
                var subNode = treeInventory.Nodes["root"].Nodes[_inventoryGuid];
                treeInventory.SelectedNode = subNode;
                TreeNodeMouseClickEventArgs ee = new TreeNodeMouseClickEventArgs(subNode, MouseButtons.Left, 0, 0, 0);
                OnNodeClicked(subNode, ee);
            }
            global.LoadFormSettings(this);
            ConfigureContextMenu();
        }

        public void RefreshSitioLevelInventory(string province, string municipality, string barangay, string sitio)
        {
            TreeNode nd = new TreeNode();
            if (_provinceNode == null)
            {
                nd = treeInventory.Nodes["root"].Nodes[_inventoryGuid];
            }
            else
            {
                nd = _provinceNode.Parent;
            }
            TreeViewEventArgs e = new TreeViewEventArgs(nd);
            _refreshSitioNumbers = true;
            OnTreeAfterExpand(null, e);

            e.Node.Nodes[province].Expand();
            e.Node.Nodes[province].Nodes[municipality].Expand();
            e.Node.Nodes[province].Nodes[municipality].Nodes[barangay].Expand();
            if (sitio.Length == 0)
            {
                sitio = "entireBarangay";
            }
            treeInventory.SelectedNode = e.Node.Nodes[province].Nodes[municipality].Nodes[barangay].Nodes[sitio];
            if (treeInventory.SelectedNode != null)
            {
                treeInventory.SelectedNode.Tag = "sitio";
                treeInventory.SelectedNode.SelectedImageKey = treeInventory.SelectedNode.ImageKey;
                ConfigListView(treeInventory.SelectedNode);
            }
        }

        private void OnTreeAfterExpand(object sender, TreeViewEventArgs e)
        {
            _treeLevel = e.Node.Tag.ToString();
            if (_refreshSitioNumbers || e.Node.FirstNode.Text == "***dummy***")
            {
                e.Node.Nodes.Clear();
                switch (_treeLevel)
                {
                    case "root":
                        break;

                    case "targetAreaInventory":
                        _inventory.ReadBarangayInventories(e.Node.Name);
                        foreach (var item in _inventory.BarangayInventories)
                        {
                            TreeNode provNode;
                            TreeNode munNode;
                            TreeNode brgyNode;
                            TreeNode sitioNode;
                            if (!e.Node.Nodes.ContainsKey(item.Value.Province))
                            {
                                provNode = e.Node.Nodes.Add(item.Value.Province, item.Value.Province);
                            }
                            else
                            {
                                provNode = e.Node.Nodes[item.Value.Province];
                            }
                            provNode.Tag = "province";
                            provNode.ImageKey = "Level04";

                            if (!provNode.Nodes.ContainsKey(item.Value.Municipality))
                            {
                                munNode = provNode.Nodes.Add(item.Value.Municipality, item.Value.Municipality);
                            }
                            else
                            {
                                munNode = provNode.Nodes[item.Value.Municipality];
                            }
                            munNode.Tag = "municipality";
                            munNode.ImageKey = "Level03";

                            if (!munNode.Nodes.ContainsKey(item.Value.barangay))
                            {
                                brgyNode = munNode.Nodes.Add(item.Value.barangay, item.Value.barangay);
                            }
                            else
                            {
                                brgyNode = munNode.Nodes[item.Value.barangay];
                            }
                            brgyNode.Tag = "barangay";
                            brgyNode.ImageKey = "Level02";

                            if (!brgyNode.Nodes.ContainsKey(item.Value.sitio))
                            {
                                var sitio = item.Value.sitio;
                                if (sitio.Length == 0)
                                {
                                    sitioNode = brgyNode.Nodes.Add("entireBarangay", "Entire barangay");
                                }
                                else
                                {
                                    sitioNode = brgyNode.Nodes.Add(item.Value.sitio, item.Value.sitio);
                                }
                                sitioNode.Tag = "sitio";
                                sitioNode.Nodes.Add("***dummy***");
                                sitioNode.ImageKey = "Level01";
                            }
                        }
                        if (_refreshSitioNumbers)
                        {
                            _refreshSitioNumbers = false;
                        }
                        break;

                    case "barangayInventory":

                        break;
                }
            }
        }

        /// <summary>
        /// set up right click menu to facilitate adding of inventory project, fisher-boat inventory (barangay), or gear inventory (sitio)
        /// </summary>
        private void ConfigureContextMenu()
        {
            contextMenu.Items.Clear();

            if (!_clickFromTree && lvInventory.SelectedItems.Count > 0 && lvInventory.SelectedItems[0].Tag != null)
            {
                _treeLevel = lvInventory.SelectedItems[0].Tag.ToString();
                switch (_treeLevel)
                {
                    case "sitio":
                        _barangayInventoryGuid = lvInventory.SelectedItems[0].Name;
                        break;

                    case "gearVariation":
                        break;

                    default:
                        return;
                }
            }

            var menuItem = contextMenu.Items.Add("Add fishery inventory project");
            menuItem.Name = "itemAddInventory";
            menuItem.Enabled = _treeLevel == "root";

            if (_treeLevel == "barangay")
            {
                menuItem = contextMenu.Items.Add("Add fisher and fishing boat inventory");
            }
            else
            {
                menuItem = contextMenu.Items.Add("Add fisher and fishing boat inventory");
            }
            menuItem.Name = "itemAddBarangay";
            menuItem.Enabled = _treeLevel == "targetAreaInventory" || _treeLevel == "province"
                                || _treeLevel == "municipality" || _treeLevel == "barangay";

            menuItem = contextMenu.Items.Add("Add fishing gear inventory");
            menuItem.Name = "itemAddFishingGear";
            menuItem.Enabled = _treeLevel == "sitio";

            if (_clickFromTree)
            {
                string itemCaption = "";
                bool proceed = true;
                TreeNode nd = treeInventory.SelectedNode;
                _deleteInventoryArgs.Clear();
                switch (_treeLevel)
                {
                    case "gearVariation":
                        itemCaption = "Delete gear inventory";
                        _deleteInventoryArgs.Add("gearInventoryGuid", nd.Name);
                        break;

                    case "sitio":
                        itemCaption = "Delete sitio and gear inventory";
                        _deleteInventoryArgs.Add("sitio", nd.Text);
                        _deleteInventoryArgs.Add("barangay", nd.Parent.Text);
                        _deleteInventoryArgs.Add("municipality", nd.Parent.Parent.Text);
                        _deleteInventoryArgs.Add("province", nd.Parent.Parent.Parent.Text);
                        _deleteInventoryArgs.Add("projectGuid", nd.Parent.Parent.Parent.Parent.Name);
                        break;

                    case "barangay":
                        itemCaption = "Delete all inventories in barangay";
                        _deleteInventoryArgs.Add("sitio", "");
                        _deleteInventoryArgs.Add("barangay", nd.Text);
                        _deleteInventoryArgs.Add("municipality", nd.Parent.Text);
                        _deleteInventoryArgs.Add("province", nd.Parent.Parent.Text);
                        _deleteInventoryArgs.Add("projectGuid", nd.Parent.Parent.Parent.Name);
                        break;

                    case "municipality":
                        itemCaption = "Delete all inventories in municipality";
                        _deleteInventoryArgs.Add("sitio", "");
                        _deleteInventoryArgs.Add("barangay", "");
                        _deleteInventoryArgs.Add("municipality", nd.Text);
                        _deleteInventoryArgs.Add("province", nd.Parent.Text);
                        _deleteInventoryArgs.Add("projectGuid", nd.Parent.Parent.Name);
                        break;

                    case "province":
                        itemCaption = "Delete all inventories in province";
                        _deleteInventoryArgs.Add("sitio", "");
                        _deleteInventoryArgs.Add("barangay", "");
                        _deleteInventoryArgs.Add("municipality", "");
                        _deleteInventoryArgs.Add("province", nd.Text);
                        _deleteInventoryArgs.Add("projectGuid", nd.Parent.Name);
                        break;

                    case "targetAreaInventory":
                        itemCaption = "Delete all inventories";
                        _deleteInventoryArgs.Add("sitio", "");
                        _deleteInventoryArgs.Add("barangay", "");
                        _deleteInventoryArgs.Add("municipality", "");
                        _deleteInventoryArgs.Add("province", "");
                        _deleteInventoryArgs.Add("projectGuid", nd.Name);
                        break;

                    default:
                        proceed = false;
                        break;
                }
                if (proceed)
                {
                    contextMenu.Items.Add("-");
                    menuItem = contextMenu.Items.Add(itemCaption);
                    menuItem.Name = "itemDeleteInventory";
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

        private void ConfigToolbar()
        {
            tsButtonAddSitioLevelInventory.Enabled = false;
            tsButtonAddInventory.Enabled = false;
            tsButtonAddGear.Enabled = false;
            tsButtonTable.Enabled = false;
            switch (_treeLevel)
            {
                case "root":
                    tsButtonAddInventory.Enabled = true;
                    break;

                case "targetAreaInventory":
                    tsButtonAddSitioLevelInventory.Enabled = true;
                    tsButtonTable.Enabled = true;
                    break;

                case "province":
                case "municipality":
                case "barangay":
                    tsButtonAddSitioLevelInventory.Enabled = true;
                    break;

                case "sitio":
                    tsButtonAddGear.Enabled = true;
                    break;

                case "gearVariation":
                    break;
            }
        }

        private void OnNodeClicked(object sender, TreeNodeMouseClickEventArgs e)
        {
            _treeLevel = e.Node.Tag.ToString();
            _clickFromTree = true;
            if (e.Button == MouseButtons.Left)
            {
                _barangayInventoryGuid = string.Empty;

                ConfigToolbar();
                if (_treeLevel == "gearVariation")
                {
                    ConfigListViewGear(e.Node.Name);
                }
                else
                {
                    ConfigListView(e.Node);
                }
                e.Node.SelectedImageKey = e.Node.ImageKey;
            }
            else if (e.Button == MouseButtons.Right)
            {
                ConfigureContextMenu();
            }
        }

        /// <summary>
        /// method to setup lisview to display details of selected inventoried gear
        /// </summary>
        /// <param name="variationInventoryGuid"></param>
        private void ConfigListViewGear(string variationInventoryGuid)
        {
            _currentGearInventoryGuid = variationInventoryGuid;
            var item = _inventory.GetGearVariationInventoryData(variationInventoryGuid);
            lvInventory.Clear();
            lvInventory.Columns.Add("Property");
            lvInventory.Columns.Add("Value");
            lvInventory.Columns.Add("  ");
            lvInventory.Columns.Add("");
            SizeColumns(lvInventory);

            //inventory header and location
            var lvi = lvInventory.Items.Add("Inventory");
            lvi.SubItems.Add(item.inventoryName);
            lvi = lvInventory.Items.Add("Date conducted");
            lvi.SubItems.Add(string.Format("{0:MMM-dd-yyyy}", item.dateConducted));
            lvi = lvInventory.Items.Add("");

            //barangay survey date and enumerator
            lvi = lvInventory.Items.Add("Date of barangay survey");
            lvi.SubItems.Add(string.Format("{0:MMM-dd-yyyy}", item.barangaySurveyDate));
            lvi = lvInventory.Items.Add("Enumerator");
            lvi.SubItems.Add(item.enumerator);
            lvi = lvInventory.Items.Add("");

            lvi = lvInventory.Items.Add("Target area");
            lvi.SubItems.Add(item.targetArea);
            lvi = lvInventory.Items.Add("Province");
            lvi.SubItems.Add(item.province);
            lvi = lvInventory.Items.Add("Municipality");
            lvi.SubItems.Add(item.municipality);
            lvi = lvInventory.Items.Add("Barangay");
            lvi.SubItems.Add(item.barangay);
            lvi = lvInventory.Items.Add("Sitio");
            lvi.SubItems.Add(item.sitio);
            lvi = lvInventory.Items.Add("");

            //gear used and local names
            lvi = lvInventory.Items.Add("Gear class");
            lvi.SubItems.Add(item.gearClass);
            lvi = lvInventory.Items.Add("Gear variation");
            lvi.SubItems.Add(item.gearVariation);
            var n = 0;
            foreach (var name in item.gearLocalNames)
            {
                if (n == 0)
                {
                    lvi = lvInventory.Items.Add("Gear local names");
                }
                else
                {
                    lvi = lvInventory.Items.Add("");
                }
                lvi.SubItems.Add(name);
                n++;
            }
            lvi = lvInventory.Items.Add("");

            //gear count
            lvi = lvInventory.Items.Add("Gears used in commercial fishing vessels");
            lvi.SubItems.Add(item.commercialCount.ToString());
            lvi = lvInventory.Items.Add("Gears used in municipal motorized fishing vessels");
            lvi.SubItems.Add(item.motorizedCount.ToString());
            lvi = lvInventory.Items.Add("Gears used in municipal motorized fishing vessels");
            lvi.SubItems.Add(item.nonMotorizedCount.ToString());
            lvi = lvInventory.Items.Add("No boat gears");
            lvi.SubItems.Add(item.noBoatCount.ToString());
            lvi = lvInventory.Items.Add("Total number of gears");
            lvi.SubItems.Add((item.noBoatCount + item.commercialCount + item.motorizedCount + item.nonMotorizedCount).ToString());
            lvi = lvInventory.Items.Add("");

            //months and seasonality
            for (int m = 1; m < 13; m++)
            {
                if (m == 1)
                {
                    lvi = lvInventory.Items.Add("Months gear is in use");
                }
                else
                {
                    lvi = lvInventory.Items.Add("");
                }
                lvi.SubItems.Add(MonthFromNumber(m));
                if (item.monthsInUse.Contains(m))
                {
                    lvi.SubItems.Add("x");
                }
            }

            for (int m = 1; m < 13; m++)
            {
                if (m == 1)
                {
                    lvi = lvInventory.Items.Add("Months peak season");
                }
                else
                {
                    lvi = lvInventory.Items.Add("");
                }
                lvi.SubItems.Add(MonthFromNumber(m));
                if (item.peakMonths.Contains(m))
                {
                    lvi.SubItems.Add("x");
                }
            }

            lvi = lvInventory.Items.Add("Number of days in a month gear is used");
            lvi.SubItems.Add(item.numberDaysGearUsedPerMonth.ToString());
            lvi = lvInventory.Items.Add("");

            //CPUE and historical averages
            var unit = " " + item.cpueUnit;
            if (item.cpueAverage != null)
            {
                lvi = lvInventory.Items.Add("Average CPUE");
                lvi.SubItems.Add(item.cpueAverage.ToString() + unit);
            }
            else
            {
                lvi = lvInventory.Items.Add("Maximum reported CPUE");
                lvi.SubItems.Add(item.cpueRangeMax.ToString() + unit + (item.cpueRangeMax == 1 ? "" : "s"));
                lvi = lvInventory.Items.Add("Minimum reported CPUE");
                lvi.SubItems.Add(item.cpueRangeMin.ToString() + unit + (item.cpueRangeMin == 1 ? "" : "s"));
            }
            if (item.cpueMode != null)
            {
                lvi = lvInventory.Items.Add("CPUE mode");
                lvi.SubItems.Add(item.cpueMode.ToString() + unit);
            }
            else
            {
                lvi = lvInventory.Items.Add("Upper mode of CPUE");
                lvi.SubItems.Add(item.cpueModeUpper.ToString() + unit + (item.cpueModeUpper == 1 ? "" : "s"));
                lvi = lvInventory.Items.Add("Lower mode of CPUE");
                lvi.SubItems.Add(item.cpueModeLower.ToString() + unit + (item.cpueModeLower == 1 ? "" : "s"));
            }
            if (unit != " kilo")
            {
                lvi = lvInventory.Items.Add($"Kilos per {unit}");
                lvi.SubItems.Add(item.equivalentKg.ToString());
            }

            //historical cpue
            n = 0;
            if (item.historicalCPUE.Count > 0)
            {
                bool byDecade = item.historicalCPUE[0].decade != null;
                foreach (var hist in item.historicalCPUE)
                {
                    if (n == 0)
                    {
                        lvi = lvInventory.Items.Add("Historical CPUE averages");
                    }
                    else
                    {
                        lvi = lvInventory.Items.Add("");
                    }

                    string rowItem = "";
                    if (byDecade)
                    {
                        rowItem = $"{hist.decade.ToString()}s: {hist.cpue.ToString()} {hist.unit}";
                    }
                    else
                    {
                        rowItem = $"{hist.historyYear.ToString()}: {hist.cpue.ToString()} {hist.unit}";
                    }

                    if (hist.notes.Length > 0)
                    {
                        rowItem += $", {hist.notes}";
                    }
                    lvi.SubItems.Add(rowItem);
                    n++;
                }
                lvi = lvInventory.Items.Add("");
            }

            //catch composition
            n = 0;
            foreach (var name in item.dominantCatch)
            {
                if (n == 0)
                {
                    lvi = lvInventory.Items.Add("Composition of dominant catch");
                }
                else
                {
                    lvi = lvInventory.Items.Add("");
                }
                lvi.SubItems.Add(name);
                n++;
            }

            n = 0;
            foreach (var name in item.nonDominantCatch)
            {
                if (n == 0)
                {
                    lvi = lvInventory.Items.Add("Composition of non-dominant catch");
                }
                else
                {
                    lvi = lvInventory.Items.Add("");
                }
                lvi.SubItems.Add(name);
                n++;
            }

            lvi = lvInventory.Items.Add("Percentage of dominant catch");
            if (item.percentageOfDominance != null)
            {
                lvi.SubItems.Add(item.percentageOfDominance.ToString() + "%");
            }
            lvInventory.Items.Add("");

            //accessories
            if (item.accessories.Count > 0)
            {
                n = 0;
                foreach (var accessory in item.accessories)
                {
                    if (n == 0)
                    {
                        lvi = lvInventory.Items.Add("Accessories used");
                    }
                    else
                    {
                        lvi = lvInventory.Items.Add("");
                    }
                    lvi.SubItems.Add(accessory);
                    n++;
                }
                lvInventory.Items.Add("");
            }

            if (item.expenses.Count > 0)
            {
                //expense items
                n = 0;
                foreach (var expense in item.expenses)
                {
                    if (n == 0)
                    {
                        lvi = lvInventory.Items.Add("Expenses");
                    }
                    else
                    {
                        lvi = lvInventory.Items.Add("");
                    }
                    lvi.SubItems.Add($"{expense.expense}: PhP{expense.cost}, source:{expense.source}, notes:{expense.notes}");
                    n++;
                }
                lvInventory.Items.Add("");
            }

            lvi = lvInventory.Items.Add("Notes");
            lvi.SubItems.Add(item.notes);

            lblGuide.Text = $"Fishing gear: {item.gearVariation}";

            SizeColumns(lvInventory, false);
        }

        /// <summary>
        /// returns month string based on inputted month number
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private string MonthFromNumber(int m)
        {
            string month = "";
            switch (m)
            {
                case 1:
                    month = "January";
                    break;

                case 2:
                    month = "February";
                    break;

                case 3:
                    month = "March";
                    break;

                case 4:
                    month = "April";
                    break;

                case 5:
                    month = "May";
                    break;

                case 6:
                    month = "June";
                    break;

                case 7:
                    month = "July";
                    break;

                case 8:
                    month = "August";
                    break;

                case 9:
                    month = "September";
                    break;

                case 10:
                    month = "October";
                    break;

                case 11:
                    month = "November";
                    break;

                case 12:
                    month = "December";
                    break;
            }
            return month;
        }

        private void ConfigListView_CrossThread(TreeNode node)
        {
            lvInventory.Invoke(new Action(() =>
            {
                lvInventory.Items.Clear();
                lvInventory.Columns.Clear();
                lvInventory.Columns.Add("Property");
                lvInventory.Columns.Add("Value");
                SizeColumns(lvInventory);
                ListViewItem lvi;
                _treeLevel = node.Tag.ToString();
                switch (_treeLevel)
                {
                    case "root":
                        lvi = lvInventory.Items.Add("Number of fishery inventories");
                        lvi.SubItems.Add(_inventory.Inventories.Count.ToString());
                        lblGuide.Text = "Fishery inventories";
                        break;

                    case "targetAreaInventory":
                        _inventoryGuid = node.Name;
                        var values = _inventory.GetLevelSummary(_inventoryGuid);
                        var gearLevelDict = _inventory.GetLevelGearsInventory(_inventoryGuid);
                        SetupListSummaryView(values.totalFishers, values.totalCommercial, values.totalMotorized, values.totalNonMotorized, gearLevelDict, node);
                        lblGuide.Text = $"Fishery inventory project: {node.Text}";
                        break;

                    case "province":
                        _provinceNode = node;
                        values = _inventory.GetLevelSummary(node.Parent.Name, node.Name);
                        gearLevelDict = _inventory.GetLevelGearsInventory(node.Parent.Name, node.Name);
                        SetupListSummaryView(values.totalFishers, values.totalCommercial, values.totalMotorized, values.totalNonMotorized, gearLevelDict, node);
                        lblGuide.Text = $"Province: {node.Name}";
                        break;

                    case "municipality":
                        _provinceNode = node.Parent;
                        values = _inventory.GetLevelSummary(node.Parent.Parent.Name, node.Parent.Name, node.Name);
                        gearLevelDict = _inventory.GetLevelGearsInventory(node.Parent.Parent.Name, node.Parent.Name, node.Name);
                        SetupListSummaryView(values.totalFishers, values.totalCommercial, values.totalMotorized, values.totalNonMotorized, gearLevelDict, node);
                        lblGuide.Text = $"Municipality: {node.Name}, {node.Parent.Name}";
                        break;

                    case "barangay":
                        _provinceNode = node.Parent.Parent;
                        values = _inventory.GetLevelSummary(node.Parent.Parent.Parent.Name, node.Parent.Parent.Name, node.Parent.Name, node.Name);
                        gearLevelDict = _inventory.GetLevelGearsInventory(node.Parent.Parent.Parent.Name, node.Parent.Parent.Name, node.Parent.Name, node.Name);
                        SetupListSummaryView(values.totalFishers, values.totalCommercial, values.totalMotorized, values.totalNonMotorized, gearLevelDict, node);
                        lblGuide.Text = $"Barangay: {node.Name}, {node.Parent.Name}, {node.Parent.Parent.Name}";
                        break;

                    case "sitio":
                        _provinceNode = node.Parent.Parent.Parent;
                        var numbers = _inventory.GetSitioNumbers(node.Parent.Parent.Parent.Parent.Name, node.Parent.Parent.Parent.Name, node.Parent.Parent.Name, node.Parent.Name, node.Name);
                        gearLevelDict = _inventory.GetLevelGearsInventory(node.Parent.Parent.Parent.Parent.Name, node.Parent.Parent.Parent.Name, node.Parent.Parent.Name, node.Parent.Name, node.Name);
                        _sitioCountCommercial = numbers.commercialCount;
                        _sitioCountFishers = numbers.fisherCount;
                        _sitioCountMunicipalMotorized = numbers.motorizedCount;
                        _sitioCountMunicipalNonMotorized = numbers.nonMotorizedCount;
                        lvi = lvInventory.Items.Add("Number of fishers");
                        lvi.SubItems.Add(_sitioCountFishers.ToString());
                        lvi.ImageKey = "Actor_16xMD";
                        lvi = lvInventory.Items.Add("Number of municipal motorized vessels");
                        lvi.SubItems.Add(_sitioCountMunicipalMotorized.ToString());
                        lvi.ImageKey = "propSmall";
                        lvi = lvInventory.Items.Add("Number of municipal non-motorized vessels");
                        lvi.SubItems.Add(_sitioCountMunicipalNonMotorized.ToString());
                        lvi.ImageKey = "paddle";
                        lvi = lvInventory.Items.Add("Number of commercial vessels");
                        lvi.SubItems.Add(_sitioCountCommercial.ToString());
                        lvi.ImageKey = "propMed";
                        lvi = lvInventory.Items.Add("");
                        lvi = lvInventory.Items.Add("Date surveyed");
                        lvi.SubItems.Add(numbers.dateSurvey == null ? "" : string.Format("{0:MMM-dd-yyyy}", numbers.dateSurvey));
                        lvi = lvInventory.Items.Add("Enumerator");
                        lvi.SubItems.Add(numbers.enumerator);

                        int row = 0;
                        foreach (var item in gearLevelDict)
                        {
                            if (row == 0)
                            {
                                lvi = lvInventory.Items.Add("");
                                lvi = lvInventory.Items.Add("");
                            }
                            lvi = lvInventory.Items.Add($"{item.Value.gearClass} - {item.Value.gearVariation} ({item.Value.localNames})");
                            lvi.SubItems.Add(item.Value.total.ToString());
                            lvi.ImageKey = Gear.GearClassImageKeyFromGearClasName(item.Value.gearClass);
                            row++;
                        }

                        row = 0;
                        foreach (var item in _inventory.GetSitioRespondents(numbers.brgySurveyGuid))
                        {
                            if (row == 0)
                            {
                                lvi = lvInventory.Items.Add("");
                                lvi = lvInventory.Items.Add("");
                                lvi = lvInventory.Items.Add("Respondents");
                            }
                            else
                            {
                                lvi = lvInventory.Items.Add("");
                            }
                            lvi.SubItems.Add(item);
                            row++;
                        }

                        _sitioNode = node;
                        ShowSitioGearInventory(_sitioNode);
                        if (node.Name == "entireBarangay")

                        {
                            lblGuide.Text = $"Sitio: {node.Parent.Name}, {node.Parent.Parent.Name}, {node.Parent.Parent.Parent.Name} (Entire barangay)";
                        }
                        else
                        {
                            lblGuide.Text = $"Sitio: {node.Name}, {node.Parent.Name}, {node.Parent.Parent.Name}, {node.Parent.Parent.Parent.Name}";
                        }

                        break;
                }
                SizeColumns(lvInventory, false);
            }
            ));
        }

        /// <summary>
        /// set up listview to summarize inventory depending on governance unit. WIill call a helper function SetupListSummaryView that will
        /// ultimately setup the listview to display inventory summaries
        /// </summary>
        /// <param name="node"></param>
        private void ConfigListView(TreeNode node)
        {
            lvInventory.Items.Clear();
            lvInventory.Columns.Clear();
            lvInventory.Columns.Add("Property");
            lvInventory.Columns.Add("Value");
            SizeColumns(lvInventory);
            ListViewItem lvi;
            _treeLevel = node.Tag.ToString();
            switch (_treeLevel)
            {
                case "root":
                    lvi = lvInventory.Items.Add("Number of fishery inventories");
                    lvi.SubItems.Add(_inventory.Inventories.Count.ToString());
                    lblGuide.Text = "Fishery inventories";
                    break;

                case "targetAreaInventory":
                    _inventoryGuid = node.Name;
                    var values = _inventory.GetLevelSummary(_inventoryGuid);
                    var gearLevelDict = _inventory.GetLevelGearsInventory(_inventoryGuid);
                    SetupListSummaryView(values.totalFishers, values.totalCommercial, values.totalMotorized, values.totalNonMotorized, gearLevelDict, node);
                    lblGuide.Text = $"Fishery inventory project: {node.Text}";
                    break;

                case "province":
                    _provinceNode = node;
                    values = _inventory.GetLevelSummary(node.Parent.Name, node.Name);
                    gearLevelDict = _inventory.GetLevelGearsInventory(node.Parent.Name, node.Name);
                    SetupListSummaryView(values.totalFishers, values.totalCommercial, values.totalMotorized, values.totalNonMotorized, gearLevelDict, node);
                    lblGuide.Text = $"Province: {node.Name}";
                    break;

                case "municipality":
                    _provinceNode = node.Parent;
                    values = _inventory.GetLevelSummary(node.Parent.Parent.Name, node.Parent.Name, node.Name);
                    gearLevelDict = _inventory.GetLevelGearsInventory(node.Parent.Parent.Name, node.Parent.Name, node.Name);
                    SetupListSummaryView(values.totalFishers, values.totalCommercial, values.totalMotorized, values.totalNonMotorized, gearLevelDict, node);
                    lblGuide.Text = $"Municipality: {node.Name}, {node.Parent.Name}";
                    break;

                case "barangay":
                    _provinceNode = node.Parent.Parent;
                    values = _inventory.GetLevelSummary(node.Parent.Parent.Parent.Name, node.Parent.Parent.Name, node.Parent.Name, node.Name);
                    gearLevelDict = _inventory.GetLevelGearsInventory(node.Parent.Parent.Parent.Name, node.Parent.Parent.Name, node.Parent.Name, node.Name);
                    SetupListSummaryView(values.totalFishers, values.totalCommercial, values.totalMotorized, values.totalNonMotorized, gearLevelDict, node);
                    lblGuide.Text = $"Barangay: {node.Name}, {node.Parent.Name}, {node.Parent.Parent.Name}";
                    break;

                case "sitio":
                    _provinceNode = node.Parent.Parent.Parent;
                    var numbers = _inventory.GetSitioNumbers(node.Parent.Parent.Parent.Parent.Name, node.Parent.Parent.Parent.Name, node.Parent.Parent.Name, node.Parent.Name, node.Name);
                    gearLevelDict = _inventory.GetLevelGearsInventory(node.Parent.Parent.Parent.Parent.Name, node.Parent.Parent.Parent.Name, node.Parent.Parent.Name, node.Parent.Name, node.Name);
                    _sitioCountCommercial = numbers.commercialCount;
                    _sitioCountFishers = numbers.fisherCount;
                    _sitioCountMunicipalMotorized = numbers.motorizedCount;
                    _sitioCountMunicipalNonMotorized = numbers.nonMotorizedCount;
                    lvi = lvInventory.Items.Add("Number of fishers");
                    lvi.SubItems.Add(_sitioCountFishers.ToString());
                    lvi.ImageKey = "Actor_16xMD";
                    lvi = lvInventory.Items.Add("Number of municipal motorized vessels");
                    lvi.SubItems.Add(_sitioCountMunicipalMotorized.ToString());
                    lvi.ImageKey = "propSmall";
                    lvi = lvInventory.Items.Add("Number of municipal non-motorized vessels");
                    lvi.SubItems.Add(_sitioCountMunicipalNonMotorized.ToString());
                    lvi.ImageKey = "paddle";
                    lvi = lvInventory.Items.Add("Number of commercial vessels");
                    lvi.SubItems.Add(_sitioCountCommercial.ToString());
                    lvi.ImageKey = "propMed";
                    lvi = lvInventory.Items.Add("");
                    lvi = lvInventory.Items.Add("Date surveyed");
                    lvi.SubItems.Add(numbers.dateSurvey == null ? "" : string.Format("{0:MMM-dd-yyyy}", numbers.dateSurvey));
                    lvi = lvInventory.Items.Add("Enumerator");
                    lvi.SubItems.Add(numbers.enumerator);

                    int row = 0;
                    foreach (var item in gearLevelDict)
                    {
                        if (row == 0)
                        {
                            lvi = lvInventory.Items.Add("");
                            lvi = lvInventory.Items.Add("");
                        }
                        lvi = lvInventory.Items.Add($"{item.Value.gearClass} - {item.Value.gearVariation} ({item.Value.localNames})");
                        lvi.SubItems.Add(item.Value.total.ToString());
                        lvi.ImageKey = Gear.GearClassImageKeyFromGearClasName(item.Value.gearClass);
                        row++;
                    }

                    row = 0;
                    foreach (var item in _inventory.GetSitioRespondents(numbers.brgySurveyGuid))
                    {
                        if (row == 0)
                        {
                            lvi = lvInventory.Items.Add("");
                            lvi = lvInventory.Items.Add("");
                            lvi = lvInventory.Items.Add("Respondents");
                        }
                        else
                        {
                            lvi = lvInventory.Items.Add("");
                        }
                        lvi.SubItems.Add(item);
                        row++;
                    }

                    _sitioNode = node;
                    ShowSitioGearInventory(_sitioNode);
                    if (node.Name == "entireBarangay")

                    {
                        lblGuide.Text = $"Sitio: {node.Parent.Name}, {node.Parent.Parent.Name}, {node.Parent.Parent.Parent.Name} (Entire barangay)";
                    }
                    else
                    {
                        lblGuide.Text = $"Sitio: {node.Name}, {node.Parent.Name}, {node.Parent.Parent.Name}, {node.Parent.Parent.Parent.Name}";
                    }

                    break;
            }
            SizeColumns(lvInventory, false);
        }

        public void RefreshMainInventory(string inventoryName)
        {
            treeInventory.Nodes["root"].Nodes[_inventoryGuid].Text = inventoryName;
            TreeNodeMouseClickEventArgs e = new TreeNodeMouseClickEventArgs(treeInventory.Nodes["root"].Nodes[_inventoryGuid], MouseButtons.Left, 0, 0, 0);
            OnNodeClicked(null, e);
            _parentForm.RefreshFisheriesInventory(_targetArea.TargetAreaGuid);
        }

        /// <summary>
        /// refreshes listview to show details of updated inventory project
        /// </summary>
        public void RefreshMainInventory()
        {
            _inventory.RefreshInventories(_targetArea);
            foreach (var item in _inventory.Inventories)
            {
                if (!treeInventory.Nodes["root"].Nodes.ContainsKey(item.Key))
                {
                    treeInventory.Nodes["root"].Expand();
                    var nd = treeInventory.Nodes["root"].Nodes.Add(item.Key, item.Value.InventoryName);
                    nd.Tag = "targetAreaInventory";
                    nd.ImageKey = "LayoutTransform";
                    treeInventory.SelectedNode = nd;
                    nd.Nodes.Add("***dummy***");
                    TreeNodeMouseClickEventArgs e = new TreeNodeMouseClickEventArgs(nd, MouseButtons.Left, 0, 0, 0);
                    OnNodeClicked(nd, e);
                }
            }
            _parentForm.RefreshFisheriesInventory(_targetArea.TargetAreaGuid);
        }

        /// <summary>
        /// refresh listview to show details of updated inventoried gear
        /// </summary>
        /// <param name="sitioGearInventoryGuid"></param>
        /// <param name="isNew"></param>
        public void RefreshSitioGearInventory(string sitioGearInventoryGuid, bool isNew = false)
        {
            if (!_clickFromTree)
            {
                switch (treeInventory.SelectedNode.Tag)
                {
                    case "gearVariation":
                        _sitioNode = treeInventory.SelectedNode.Parent;
                        break;

                    default:
                        var item = _inventory.GetMunicipalityBrangaySitioFromGearInventory(sitioGearInventoryGuid);
                        treeInventory.SelectedNode.Expand();
                        try
                        {
                            treeInventory.SelectedNode.Nodes[item.barangay].Expand();
                            _sitioNode = treeInventory.SelectedNode.Nodes[item.barangay].Nodes[item.sitio];
                        }
                        catch (NullReferenceException nre)
                        {
                            //ignore
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(ex.Message, "GearInventoryForm", "RefreshSitioGearInventory");
                        }
                        break;
                }
            }

            ShowSitioGearInventory(_sitioNode);

            if (_clickFromTree)
            {
                var gearNode = _sitioNode.Nodes[sitioGearInventoryGuid];
                treeInventory.SelectedNode = gearNode;
                TreeNodeMouseClickEventArgs e = new TreeNodeMouseClickEventArgs(gearNode, MouseButtons.Left, 0, 0, 0);
                OnNodeClicked(gearNode, e);
            }
            else
            {
                TreeNodeMouseClickEventArgs e = new TreeNodeMouseClickEventArgs(treeInventory.SelectedNode, MouseButtons.Left, 0, 0, 0);
                OnNodeClicked(treeInventory.SelectedNode, e);
            }
        }

        private void ShowSitioGearInventory(TreeNode node)
        {
            _barangayInventoryGuid = _inventory.GetBarangayInventoryGuid(node.Parent.Parent.Parent.Parent.Name, node.Parent.Parent.Parent.Name, node.Parent.Parent.Name, node.Parent.Name, node.Name);
            foreach (var item in _inventory.ReadSitioGearInventory(_barangayInventoryGuid))
            {
                if (!node.Nodes.ContainsKey(item.variationInventoryGuid))
                {
                    if (node.Nodes.Count > 0 && node.FirstNode.Text == "***dummy***")
                    {
                        node.Nodes.Clear();
                    }
                    var nd = node.Nodes.Add(item.variationInventoryGuid, $"{item.gearVariation} ({item.localNames})");
                    nd.Tag = "gearVariation";
                    nd.ImageKey = Gear.GearClassImageKeyFromGearClasName(item.gearClass);
                }
            }
        }

        /// <summary>
        /// helper function that actually sets up listview rows to display inventory summary per governance unit
        /// </summary>
        /// <param name="fisherCount"></param>
        /// <param name="commercialCount"></param>
        /// <param name="motorizedCount"></param>
        /// <param name="nonMotorizedCount"></param>
        /// <param name="gearInventoryDict"></param>
        /// <param name="node"></param>
        private void SetupListSummaryView(int fisherCount, int commercialCount, int motorizedCount, int nonMotorizedCount,
            Dictionary<string, (string gearClass, string variation, string localNames, int total, int sumCommercial, int sumMotorized, int sumNonMotorized, int sumNoBoat)> gearInventoryDict, TreeNode node = null)
        {
            ListViewItem lvi;

            switch (_treeLevel)
            {
                case "targetAreaInventory":
                    node.Expand();

                    //list the detail of the inventory project
                    lvi = lvInventory.Items.Add("Name of inventory");
                    lvi.SubItems.Add(node.Text);
                    lvi = lvInventory.Items.Add("Date implemented");
                    lvi.SubItems.Add(string.Format("{0:MMM-dd-yyyy}", _inventory.Inventories[node.Name].DateConducted));
                    lvi = lvInventory.Items.Add("Target area");
                    lvi.SubItems.Add(_inventory.Inventories[node.Name].TargetArea);
                    lvi = lvInventory.Items.Add("");

                    //adds summary of government units at the prject level
                    lvi = lvInventory.Items.Add("Number of provinces");
                    lvi.SubItems.Add(node.Nodes.Count.ToString());
                    lvi.ImageKey = "Level04";
                    var inventoryLevels = _inventory.NumberOfMunicipalitiesBarangays(node.Name);
                    lvi = lvInventory.Items.Add("Number of municipalities");
                    lvi.SubItems.Add(inventoryLevels.municipalityCount.ToString());
                    lvi.ImageKey = "Level03";
                    lvi = lvInventory.Items.Add("Number of barangays");
                    lvi.SubItems.Add(inventoryLevels.barangayCount.ToString());
                    lvi.ImageKey = "Level02";
                    break;

                case "province":
                    //adds summary of government units at the province level
                    var provinceLevels = _inventory.NumberOfMunicipalitiesBarangays(node.Parent.Name, node.Name);
                    lvi = lvInventory.Items.Add("Number of municipalities");
                    lvi.ImageKey = "Level03";
                    lvi.SubItems.Add(provinceLevels.municipalityCount.ToString());
                    lvi = lvInventory.Items.Add("Number of barangays");
                    lvi.SubItems.Add(provinceLevels.barangayCount.ToString());
                    lvi.ImageKey = "Level02";
                    break;

                case "municipality":
                    //adds summary of government units at the municipality level
                    lvi = lvInventory.Items.Add("Number of barangays");
                    lvi.SubItems.Add(_inventory.NumberOfBarangays(node.Parent.Parent.Name, node.Parent.Name, node.Name).ToString());
                    lvi.ImageKey = "Level02";
                    break;

                case "barangay":
                    //adds summary of government units at barangay level
                    lvi = lvInventory.Items.Add("Number of sitios");
                    lvi.SubItems.Add(_inventory.NumberOfSitio(node.Parent.Parent.Parent.Name, node.Parent.Parent.Name, node.Parent.Name, node.Name).ToString());
                    lvi.ImageKey = "Level01";
                    break;

                case "sitio":
                    break;
            }

            //add summary of fisher and boat inventory at the selected govenrment unit level
            lvi = lvInventory.Items.Add("Total number of fishers");
            lvi.SubItems.Add(fisherCount.ToString());
            lvi.ImageKey = "Actor_16xMD";
            lvi = lvInventory.Items.Add("Total number of municipal motorized vessels");
            lvi.SubItems.Add(motorizedCount.ToString());
            lvi.ImageKey = "propSmall";
            lvi = lvInventory.Items.Add("Total number of municipal non-motorized vessels");
            lvi.SubItems.Add(nonMotorizedCount.ToString());
            lvi.ImageKey = "paddle";
            lvi = lvInventory.Items.Add("Total number of commercial vessels");
            lvi.SubItems.Add(commercialCount.ToString());
            lvi.ImageKey = "propMed";

            //add summary of inventoried gear at selected gorvenrment unit level
            if (_treeLevel == "municipality")
            {
                //lists barangays, sitios in barangays, and gears inventoried in each sitio together with count of gears inventoried
                var ngeCount = 0;
                var currentBarangay = "";
                var currentSitio = "";
                var list = _inventory.GetBarangaysGearInventory(node.Parent.Parent.Name, node.Parent.Name, node.Name);
                if (list.Count > 0)
                {
                    lvi = lvInventory.Items.Add("");
                    foreach (var item in list)
                    {
                        if (currentBarangay != item.barangay)
                        {
                            currentBarangay = item.barangay;
                            lvi = lvInventory.Items.Add("");
                            lvi = lvInventory.Items.Add(currentBarangay);
                            lvi.Tag = "barangay";
                            lvi.ImageKey = "Level02";
                        }
                        if (currentSitio != item.sitio)
                        {
                            currentSitio = item.sitio;
                            if (item.sitio.Length > 0)
                            {
                                lvi = lvInventory.Items.Add(item.barangayInventoryGUID, "  " + currentSitio, null);
                            }
                            else
                            {
                                lvi = lvInventory.Items.Add(item.barangayInventoryGUID, "  Entire barangay", null);
                            }
                            lvi.ImageKey = "Level01";
                            lvi.Tag = "sitio";
                        }
                        if (item.gearClass != "" && item.gearVariation != "")
                        {
                            lvi = lvInventory.Items.Add("       " + item.gearClass + "-" + item.gearVariation + " (" + item.localNames + ")");
                            lvi.Name = (item.dataGuid);
                            lvi.SubItems.Add(item.total.ToString());
                            lvi.ImageKey = Gear.GearClassImageKeyFromGearClasName(item.gearClass);
                            lvi.Tag = "gearVariation";
                        }
                        else
                        {
                            lvi.SubItems.Add("NGI");
                            ngeCount++;
                        }
                    }

                    if (ngeCount > 0)
                    {
                        lvInventory.Items.Add("");
                        lvInventory.Items.Add("NGI = No gears inventoried");
                    }
                }
            }
            else
            {
                //for other gov. levels not municipality simply list down gears that were counted
                lvi = lvInventory.Items.Add("");
                lvi = lvInventory.Items.Add("");
                foreach (var item in gearInventoryDict)
                {
                    if (item.Value.localNames.Length > 0)
                    {
                        lvi = lvInventory.Items.Add($"{item.Value.gearClass} - {item.Value.variation} ({item.Value.localNames})");
                    }
                    else
                    {
                        lvi = lvInventory.Items.Add($"{item.Value.gearClass} - {item.Value.variation}");
                    }
                    lvi.SubItems.Add(item.Value.total.ToString());
                    lvi.ImageKey = Gear.GearClassImageKeyFromGearClasName(item.Value.gearClass);
                }
            }
        }

        /// <summary>
        /// called by context or toolbar to add an inventory project
        /// </summary>
        private void AddInventory(GearInventoryEditForm gearInventoryEditForm = null)
        {
            GearInventoryEditForm inventoryEditForm;
            if (gearInventoryEditForm == null)
            {
                inventoryEditForm = new GearInventoryEditForm(_treeLevel, _targetArea, _inventory, this);
            }
            else
            {
                inventoryEditForm = gearInventoryEditForm;
            }
            inventoryEditForm.AddNewInventory();
            inventoryEditForm.ShowDialog(this);
        }

        /// <summary>
        /// implements what was selected in right click menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            e.ClickedItem.Owner.Visible = false;

            var inventoryEditForm = new GearInventoryEditForm(_treeLevel, _targetArea, _inventory, this);
            switch (e.ClickedItem.Name)
            {
                case "itemAddInventory":
                    AddInventory(inventoryEditForm);
                    return;

                case "itemDeleteInventory":
                    TreeNode parent = treeInventory.SelectedNode.Parent;
                    switch (_treeLevel)
                    {
                        case "gearVariation":
                            if (_inventory.DeleteGearVariationInventory(_deleteInventoryArgs["gearInventoryGuid"]))
                            {
                                parent.Nodes.Remove(treeInventory.SelectedNode);
                            }
                            break;

                        default:
                            if (_inventory.DeleteInventory(_deleteInventoryArgs))
                            {
                                _inventory.RefreshInventories(_targetArea.TargetAreaGuid);
                                parent.Nodes.Remove(treeInventory.SelectedNode);
                                if (parent.Nodes.Count == 0)
                                {
                                    parent.Nodes.Add("***dummy***");
                                    parent.Collapse();
                                }
                            }
                            if (_treeLevel == "targetAreaInventory")
                            {
                                global.mainForm.UpdateInventoryList(_inventory);
                            }
                            break;
                    }
                    TreeNodeMouseClickEventArgs ee = new TreeNodeMouseClickEventArgs(parent, MouseButtons.Left, 1, 0, 0);
                    OnNodeClicked(null, ee);
                    return;

                case "itemAddBarangay":
                    TreeNode nd = treeInventory.SelectedNode;
                    switch (_treeLevel)
                    {
                        case "targetAreaInventory":
                            inventoryEditForm.AddNewBarangyInventory(_inventoryGuid);
                            break;

                        case "province":
                            inventoryEditForm.AddNewBarangyInventory(_inventoryGuid, nd.Name);

                            break;

                        case "municipality":
                            inventoryEditForm.AddNewBarangyInventory(_inventoryGuid, nd.Parent.Name, nd.Name);

                            break;

                        case "barangay":
                            if (nd.FirstNode.Name == "entireBarangay")
                            {
                                MessageBox.Show("Cannot add a new sitio because sitios are already included in 'Entire barangay'",
                                                "Cannot add new sitio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            else
                            {
                                inventoryEditForm.AddNewBarangyInventory(_inventoryGuid, nd.Parent.Parent.Name, nd.Parent.Name, nd.Name);
                            }
                            break;
                    }
                    break;

                case "itemAddFishingGear":
                    inventoryEditForm.AddNewGearInventory(_barangayInventoryGuid, _sitioCountCommercial, _sitioCountMunicipalMotorized, _sitioCountMunicipalNonMotorized, _sitioCountFishers);
                    break;
            }
            inventoryEditForm.ShowDialog(this);
        }

        private void OnListViewDoubleClick(object sender, EventArgs e)
        {
            var inventoryEditForm = new GearInventoryEditForm(_treeLevel, _targetArea, _inventory, this);
            switch (_treeLevel)
            {
                case "targetAreaInventory":
                    inventoryEditForm.EditInventoryLevel(_inventoryGuid, lvInventory.Items[0].SubItems[1].Text, DateTime.Parse(lvInventory.Items[1].SubItems[1].Text));
                    break;

                case "sitio":
                    var nd = treeInventory.SelectedNode;
                    DateTime? surveyDate = null;
                    try
                    {
                        if (DateTime.TryParse(lvInventory.Items[5].SubItems[1].Text, out DateTime d))
                        {
                            surveyDate = d;
                        }

                        inventoryEditForm.EditInventoryLevel(_barangayInventoryGuid,
                                                            global.MunicipalityNumberFromString(nd.Parent.Parent.Parent.Text, nd.Parent.Parent.Text), nd.Parent.Text, nd.Text,
                                                            int.Parse(lvInventory.Items[0].SubItems[1].Text), int.Parse(lvInventory.Items[1].SubItems[1].Text),
                                                            int.Parse(lvInventory.Items[2].SubItems[1].Text), int.Parse(lvInventory.Items[3].SubItems[1].Text),
                                                            surveyDate, lvInventory.Items[6].SubItems[1].Text);
                    }
                    catch
                    {
                        return;
                    }
                    break;

                case "gearVariation":
                    if (_listTargetHit.Tag != null && _listTargetHit.Tag.ToString() == "gearVariation")
                    {
                        inventoryEditForm.EditInventoryLevel(_listTargetHit.Name);
                    }
                    else
                    {
                        inventoryEditForm.EditInventoryLevel(_currentGearInventoryGuid);
                    }
                    break;

                case "municipality":
                    if (_listTargetHit.Tag != null && _listTargetHit.Tag.ToString() == "gearVariation")
                    {
                        inventoryEditForm.EditInventoryLevel(_listTargetHit.Name);
                    }
                    else
                    {
                        inventoryEditForm = null;
                        return;
                    }
                    break;

                default:
                    inventoryEditForm = null;
                    return;
            }
            inventoryEditForm.ShowDialog(this);
        }

        private void OnToolStripItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "tsButtonTable":
                    GearInventoryTabularForm gitf = GearInventoryTabularForm.GetInstance(_inventory, _inventoryGuid);
                    gitf.InventoryProjectName = treeInventory.SelectedNode.Text;
                    if (gitf.Visible)
                    {
                        gitf.BringToFront();
                    }
                    else
                    {
                        gitf.Show(this);
                    }
                    break;

                case "tsButtonExit":
                    Close();
                    break;

                case "tsButtonAddInventory":
                    if (_treeLevel == "root") AddInventory();
                    break;

                case "tsButtonAddSitioLevelInventory":
                    if (_treeLevel != "root" && _treeLevel != "gearVariation" && _treeLevel != "sitio")
                    {
                        ToolStripItemClickedEventArgs ee = new ToolStripItemClickedEventArgs(contextMenu.Items["itemAddBarangay"]);
                        OnContextMenu_ItemClicked(null, ee);
                    }
                    break;

                case "tsButtonAddGear":
                    if (_treeLevel == "sitio")
                    {
                        ToolStripItemClickedEventArgs ee = new ToolStripItemClickedEventArgs(contextMenu.Items["itemAddFishingGear"]);
                        OnContextMenu_ItemClicked(null, ee);
                    }
                    break;

                case "tsButtonExport":
                case "tsButtonImport":
                    var actionType = ExportImportAction.ActionExport;
                    if (e.ClickedItem.Name == "tsButtonImport")
                    {
                        actionType = ExportImportAction.ActionImport;
                    }
                    using (ExportImportDialogForm eidf = new ExportImportDialogForm(ExportImportDataType.GearInventoryDataSelect, actionType))
                    {
                        eidf.ShowDialog(this);
                        if (eidf.DialogResult == DialogResult.OK)
                        {
                            if ((eidf.Selection & ExportImportDataType.Enumerators) == ExportImportDataType.Enumerators)
                            {
                                switch (actionType)
                                {
                                    case ExportImportAction.ActionExport:
                                        var exportCount = Enumerators.ExportEnumerators(_targetArea.TargetAreaGuid);
                                        if (exportCount > 0)
                                        {
                                            MessageBox.Show($"Successfully exported {exportCount} enumerators");
                                        }

                                        break;

                                    case ExportImportAction.ActionImport:
                                        FileDialogHelper.Title = "Import enumerators";
                                        FileDialogHelper.DialogType = FileDialogType.FileOpen;
                                        FileDialogHelper.DataFileType = DataFileType.Text | DataFileType.XML | DataFileType.HTML;
                                        FileDialogHelper.ShowDialog();
                                        var fileName = FileDialogHelper.FileName;
                                        if (fileName.Length > 0)
                                        {
                                            if (Enumerators.ImportEnumerators(fileName, _targetArea.TargetAreaGuid))
                                            {
                                                global.mainForm.SetUPLV("target_area");
                                                MessageBox.Show("Successfully imported enumerators to the database");
                                            }
                                        }
                                        break;
                                }
                            }
                            else if ((eidf.Selection & ExportImportDataType.GearInventory) == ExportImportDataType.GearInventory)
                            {
                                switch (actionType)
                                {
                                    case ExportImportAction.ActionExport:
                                        ExportInventory();
                                        break;

                                    case ExportImportAction.ActionImport:
                                        ImportGearInventory();
                                        break;
                                }
                            }
                        }
                    }
                    break;
            }
        }

        private void ExportInventoryXML(string fileName)
        {
            XmlWriter writer = XmlWriter.Create(fileName);
            writer.WriteStartDocument();

            //inventory project data
            writer.WriteStartElement("FisherVesselGearInventoryProject");
            writer.WriteAttributeString("ProjectGuid", _inventoryGuid);
            writer.WriteAttributeString("TargetArea", _inventory.Inventories[_inventoryGuid].TargetArea);
            writer.WriteAttributeString("TargetAreaGuid", _targetArea.TargetAreaGuid);
            writer.WriteAttributeString("DateStart", _inventory.Inventories[_inventoryGuid].DateConducted.ToShortDateString());
            writer.WriteAttributeString("ProjectName", _inventory.Inventories[_inventoryGuid].InventoryName);

            //list inventory enumerators
            {
                writer.WriteStartElement("Enumerators");
                foreach (var enumerator in _inventory.GetInventoryEnumerators(_inventoryGuid))
                {
                    writer.WriteStartElement("Enumerator");
                    writer.WriteAttributeString("Name", enumerator.enumeratorName);
                    writer.WriteAttributeString("EnumeratorGuid", enumerator.enumeratorGUID);
                    writer.WriteAttributeString("DateHired", enumerator.hired.ToShortDateString());
                    writer.WriteAttributeString("Active", enumerator.Active.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }

            foreach (var item in _inventory.BarangayInventories)
            {
                //barangay fisher-vessel inventory
                writer.WriteStartElement("FisherVesselInventory");
                writer.WriteAttributeString("Province", item.Value.Province);
                writer.WriteAttributeString("Municipality", item.Value.Municipality);
                writer.WriteAttributeString("MunicipalityNumber", item.Value.MunicipalityNumber);
                writer.WriteAttributeString("Barangay", item.Value.barangay);
                writer.WriteAttributeString("Sitio", item.Value.sitio);
                writer.WriteAttributeString("EnumeratorGUID", item.Value.EnumeratorGuid);
                writer.WriteAttributeString("SurveyDate", string.Format("{0:MMM-dd-yyyy}", item.Value.dateSurvey.ToShortDateString()));
                writer.WriteAttributeString("BarangayInventoryGuid", item.Key);
                var numbers = _inventory.GetSitioNumbers(item.Key);
                writer.WriteAttributeString("CountCommercial", numbers.commercialCount.ToString());
                writer.WriteAttributeString("CountMunicipalMotorized", numbers.motorizedCount.ToString());
                writer.WriteAttributeString("CountMunicipalNonMotorized", numbers.nonMotorizedCount.ToString());
                writer.WriteAttributeString("CountFishers", numbers.fisherCount.ToString());

                //respondents
                writer.WriteStartElement("Respondents");
                foreach (string respondent in _inventory.GetSitioRespondents(item.Key))
                {
                    writer.WriteStartElement("Respondent");
                    writer.WriteAttributeString("Name", respondent);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();

                //gear inventory in sitio
                foreach (var sitioGear in _inventory.ReadSitioGearInventory(item.Key))
                {
                    //gear used in sitio
                    writer.WriteStartElement("GearInventory");
                    writer.WriteAttributeString("GearClass", sitioGear.gearClass);
                    writer.WriteAttributeString("GearVariation", sitioGear.gearVariation);
                    writer.WriteAttributeString("GearVariationGuid", sitioGear.gearVariationGuid);
                    writer.WriteAttributeString("GearInventoryGuid", sitioGear.variationInventoryGuid);

                    {
                        var gearDetail = _inventory.GetGearVariationInventoryDataEx(sitioGear.variationInventoryGuid);

                        //local names of gear used
                        {
                            writer.WriteStartElement("GearLocalNames");
                            foreach (var localName in gearDetail.gearLocalNames)
                            {
                                writer.WriteStartElement("GearLocalName");
                                writer.WriteAttributeString("Value", localName.Value);
                                writer.WriteAttributeString("key", localName.Key);
                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement();
                        }

                        //distribution of gear within vessel types
                        {
                            writer.WriteStartElement("UsageCount");
                            writer.WriteAttributeString("CountCommercialUse", gearDetail.commercialCount.ToString());
                            writer.WriteAttributeString("CountMotorizedUse", gearDetail.motorizedCount.ToString());
                            writer.WriteAttributeString("CountNonCommercialUse", gearDetail.nonMotorizedCount.ToString());
                            writer.WriteAttributeString("CountNoBoatUse", gearDetail.noBoatCount.ToString());
                            writer.WriteEndElement();
                        }

                        //seasonality of gear
                        {
                            writer.WriteStartElement("Seaonality");
                            writer.WriteAttributeString("NumberDaysPerMonth", gearDetail.numberDaysGearUsedPerMonth.ToString());
                            {
                                writer.WriteStartElement("MonthsInUse");
                                foreach (var monthOperation in gearDetail.monthsInUse)
                                {
                                    writer.WriteStartElement("Month");
                                    writer.WriteString(MonthFromNumber(monthOperation));
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();

                                writer.WriteStartElement("MonthsPeak");
                                foreach (var monthPeak in gearDetail.peakMonths)
                                {
                                    writer.WriteStartElement("Month");
                                    writer.WriteString(MonthFromNumber(monthPeak));
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement();
                        }

                        //cpue of the gear
                        {
                            writer.WriteStartElement("CPUE");
                            writer.WriteAttributeString("CatchRangeMaximum", gearDetail.cpueRangeMax.ToString());
                            writer.WriteAttributeString("CatchRangeMinimum", gearDetail.cpueRangeMin.ToString());
                            writer.WriteAttributeString("CatchAverageCPUE", gearDetail.cpueAverage.ToString());
                            writer.WriteAttributeString("CatchModeUpper", gearDetail.cpueModeUpper.ToString());
                            writer.WriteAttributeString("CatchModeLower", gearDetail.cpueModeLower.ToString());
                            writer.WriteAttributeString("CatchMode", gearDetail.cpueMode.ToString());
                            writer.WriteAttributeString("Unit", gearDetail.cpueUnit);
                            writer.WriteAttributeString("KiloEquivalent", gearDetail.equivalentKg.ToString());

                            //historical cpue
                            {
                                writer.WriteStartElement("CPUEHistoricalTrend");
                                foreach (var trend in gearDetail.historicalCPUE)
                                {
                                    writer.WriteStartElement("CPUEHistory");
                                    if (trend.decade != null)
                                    {
                                        writer.WriteAttributeString("DecadeStart", trend.decade.ToString() + "s");
                                    }
                                    else
                                    {
                                        writer.WriteAttributeString("SpecificYear", trend.historyYear.ToString());
                                    }
                                    writer.WriteAttributeString("AverageCPUE", trend.cpue.ToString());
                                    writer.WriteAttributeString("Unit", trend.unit);
                                    writer.WriteAttributeString("Notes", trend.notes);
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                            }

                            writer.WriteEndElement();
                        }

                        //catch composition
                        {
                            writer.WriteStartElement("CatchComposition");
                            writer.WriteAttributeString("PercentageOfDominance", gearDetail.percentageOfDominance.ToString());
                            {
                                //Dominant catch
                                writer.WriteStartElement("DominantCatch");
                                foreach (var dCatch in gearDetail.dominantCatch)
                                {
                                    writer.WriteStartElement("Name");
                                    writer.WriteAttributeString("Value", dCatch.Value);
                                    writer.WriteAttributeString("guid", dCatch.Key);
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();

                                //Non-dominant catch
                                writer.WriteStartElement("NonDominantCatch");
                                foreach (var ndCatch in gearDetail.nonDominantCatch)
                                {
                                    writer.WriteStartElement("Name");
                                    writer.WriteAttributeString("Value", ndCatch.Value);
                                    writer.WriteAttributeString("guid", ndCatch.Key);
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                            }

                            writer.WriteEndElement();
                        }

                        //accessories
                        {
                            writer.WriteStartElement("Accessories");
                            foreach (string accessory in gearDetail.accessories)
                            {
                                writer.WriteStartElement("Accessory");
                                writer.WriteAttributeString("Name", accessory);
                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement();
                        }

                        //expenses
                        {
                            writer.WriteStartElement("Expenses");
                            foreach (var expense in gearDetail.expenses)
                            {
                                writer.WriteStartElement("Expense");
                                writer.WriteAttributeString("Name", expense.expense);
                                writer.WriteAttributeString("Cost", expense.cost.ToString());
                                writer.WriteAttributeString("SourceOfFunds", expense.source);
                                writer.WriteAttributeString("Notes", expense.notes);
                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement();
                        }

                        //final notes
                        {
                            writer.WriteStartElement("Notes");
                            writer.WriteAttributeString("Value", gearDetail.notes);
                            writer.WriteEndElement();
                        }
                    }

                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
            MessageBox.Show("Fishing gear inventory was exported to XML", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ExportInventory()
        {
            bool notImplemented = true;
            FileDialogHelper.Title = "Provide filename for exporting fisher, vessel and fishing gear inventory";
            FileDialogHelper.DialogType = FileDialogType.FileSave;
            FileDialogHelper.DataFileType = DataFileType.Text | DataFileType.XML | DataFileType.CSV | DataFileType.Excel;
            FileDialogHelper.ShowDialog();
            var fileName = FileDialogHelper.FileName;
            if (fileName.Length > 0)
            {
                switch (Path.GetExtension(fileName))
                {
                    case ".txt":

                        break;

                    case ".XML":
                    case ".xml":
                        ExportInventoryXML(fileName);
                        notImplemented = false;
                        break;

                    case ".xlsx":
                        break;
                }
            }

            if (notImplemented)
            {
                MessageBox.Show("This is not yet implemented");
            }
        }

        private async void ImportGearInventory()
        {
            using (ImportInventoryXMLForm iifx = new ImportInventoryXMLForm(_inventory))
            {
                iifx.ShowDialog(this);
                if (iifx.DialogResult == DialogResult.OK)
                {
                    _importInventoryProjectName = iifx.ImportedInventoryProjectName;
                    _importInventoryProjectGuid = iifx.ImportedInventoryProjectGuid;
                    int result = await _inventory.ImportInventoryAsync(iifx.ImportedInventoryFileName, iifx.ImportInventoryAction, _importInventoryProjectGuid);
                    string msg = "";
                    if (result > 0)
                    {
                        msg = $"Finished importing {result} gear variation inventories into the database";
                        _importInventorySuccess = true;
                        _inventory.RefreshInventories(_targetArea.TargetAreaGuid);
                        TreeNode ndRoot = treeInventory.Nodes["root"];

                        treeInventory.Invoke(new Action(() =>
                        {
                            ndRoot.Nodes.Clear();
                            var subInventoryProjectNode = ndRoot.Nodes.Add(iifx.ImportedInventoryProjectGuid, iifx.ImportedInventoryProjectName);
                            subInventoryProjectNode.Tag = "targetAreaInventory";
                            subInventoryProjectNode.Nodes.Add("***dummy***");
                            subInventoryProjectNode.ImageKey = "LayoutTransform";
                            ndRoot.Expand();
                            ConfigListView_CrossThread(ndRoot);
                        }
                        ));
                    }
                    else if (result == -1)
                    {
                        msg = "Import was cancelled";
                    }
                    else
                    {
                        msg = "Finished importing inventory to the database but zero gear variation inventories was imported";
                    }
                    if (result != -1)
                    {
                        global.mainForm.UpdateInventoryList(_inventory);
                    }
                    MessageBox.Show(msg, "Import inventory", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            _clickFromTree = false;
            _listTargetHit = lvInventory.HitTest(e.X, e.Y).Item;
            ConfigureContextMenu();
        }

        private void OnPanelSizeChanged(object sender, EventArgs e)
        {
        }
    }
}
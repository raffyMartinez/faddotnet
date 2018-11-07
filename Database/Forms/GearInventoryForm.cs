using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FAD3.Database.Classes;

namespace FAD3.Database.Forms
{
    public partial class GearInventoryForm : Form
    {
        private static GearInventoryForm _instance;
        private TargetArea _aoi;
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

        public static GearInventoryForm GetInstance(TargetArea aoi, string inventoryGuid = "")
        {
            if (_instance == null) return new GearInventoryForm(aoi, inventoryGuid);
            return _instance;
        }

        public GearInventoryForm(TargetArea aoi, string inventoryGuid)
        {
            InitializeComponent();
            _aoi = aoi;
            _inventory = new FishingGearInventory(aoi);
            _inventoryGuid = inventoryGuid;
            treeInventory.ImageList = global.mainForm.treeImages;
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
            var nd = treeInventory.Nodes.Add("root", "Fishery inventory");
            nd.ImageKey = "ListFolder";
            nd.Tag = "root";
            nd.SelectedImageKey = nd.ImageKey;

            foreach (var item in _inventory.Inventories)
            {
                var subNode = nd.Nodes.Add(item.Key, item.Value.InventoryName);
                subNode.Tag = "targetAreaInventory";
                subNode.Nodes.Add("***dummy***");
                subNode.ImageKey = "LayoutTransform";
            }
            if (nd.Nodes.Count == 0)
            {
                nd.Nodes.Add("***dummy***");
            }
            _treeLevel = nd.Tag.ToString();
            ConfigListView(treeInventory.Nodes["root"]);
            if (_inventoryGuid.Length > 0)
            {
                var subNode = treeInventory.Nodes["root"].Nodes[_inventoryGuid];
                treeInventory.SelectedNode = subNode;
                TreeNodeMouseClickEventArgs ee = new TreeNodeMouseClickEventArgs(subNode, MouseButtons.Left, 0, 0, 0);
                OnNodeClicked(subNode, ee);
            }
            global.LoadFormSettings(this);
        }

        public void RefreshSitioLevelInventory(string province, string municipality, string barangay, string sitio)
        {
            TreeNode nd = new TreeNode();
            if (_provinceNode == null)
            {
                nd = treeInventory.Nodes["root"].Nodes[_inventoryGuid];
                //TreeViewEventArgs e = new TreeViewEventArgs(treeInventory.Nodes["root"].Nodes[_inventoryGuid]);
            }
            else
            {
                nd = _provinceNode.Parent;
                //TreeViewEventArgs e = new TreeViewEventArgs(_provinceNode.Parent);
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

        private void ConfigureContextMenu()
        {
            contextMenu.Items.Clear();
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

        private void OnNodeClicked(object sender, TreeNodeMouseClickEventArgs e)
        {
            _barangayInventoryGuid = string.Empty;
            _treeLevel = e.Node.Tag.ToString();
            ConfigureContextMenu();
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
            lvi = lvInventory.Items.Add("Maximum reported CPUE");
            lvi.SubItems.Add(item.cpueRangeMax.ToString() + unit + (item.cpueRangeMax == 1 ? "" : "s"));
            lvi = lvInventory.Items.Add("Minimum reported CPUE");
            lvi.SubItems.Add(item.cpueRangeMin.ToString() + unit + (item.cpueRangeMin == 1 ? "" : "s"));
            lvi = lvInventory.Items.Add("Upper mode of CPUE");
            lvi.SubItems.Add(item.cpueModeUpper.ToString() + unit + (item.cpueModeUpper == 1 ? "" : "s"));
            lvi = lvInventory.Items.Add("Lower mode of CPUE");
            lvi.SubItems.Add(item.cpueModeLower.ToString() + unit + (item.cpueModeLower == 1 ? "" : "s"));

            n = 0;
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
                lvi.SubItems.Add(hist.decade.ToString() + "s: " + hist.cpue.ToString() + " " + hist.unit);
                n++;
            }
            lvi = lvInventory.Items.Add("");

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
            lvi.SubItems.Add(item.percentageOfDominance.ToString() + "%");

            lblGuide.Text = $"Fishing gear: {item.gearVariation}";

            SizeColumns(lvInventory, false);
        }

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
                    //_inventoryGuid = treeInventory.SelectedNode.Name;
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
                    lvi = lvInventory.Items.Add("");
                    foreach (var item in gearLevelDict)
                    {
                        lvi = lvInventory.Items.Add(item.Value.gearClass + "-" + item.Key);
                        lvi.SubItems.Add(item.Value.total.ToString());
                        lvi.ImageKey = gear.GearClassImageKeyFromGearClasName(item.Value.gearClass);
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
        }

        public void RefreshMainInventory()
        {
            _inventory.RefreshInventories(_aoi);
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
        }

        public void RefreshSitioGearInventory(string sitioGearInventoryGuid, bool isNew = false)
        {
            ShowSitioGearInventory(_sitioNode);
            var gearNode = _sitioNode.Nodes[sitioGearInventoryGuid];
            treeInventory.SelectedNode = gearNode;
            TreeNodeMouseClickEventArgs e = new TreeNodeMouseClickEventArgs(gearNode, MouseButtons.Left, 0, 0, 0);
            OnNodeClicked(gearNode, e);
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
                    var nd = node.Nodes.Add(item.variationInventoryGuid, item.gearVariation);
                    nd.Tag = "gearVariation";
                    nd.ImageKey = gear.GearClassImageKeyFromGearClasName(item.gearClass);
                }
            }
        }

        private void SetupListSummaryView(int fisherCount, int commercialCount, int motorizedCount, int nonMotorizedCount,
            Dictionary<string, (string gearClass, int total, int sumCommercial, int sumMotorized, int sumNonMotorized, int sumNoBoat)> gearInventoryDict, TreeNode node = null)
        {
            ListViewItem lvi;

            switch (_treeLevel)
            {
                case "targetAreaInventory":
                    node.Expand();

                    lvi = lvInventory.Items.Add("Name of inventory");
                    lvi.SubItems.Add(node.Text);
                    lvi = lvInventory.Items.Add("Date implemented");
                    lvi.SubItems.Add(string.Format("{0:MMM-dd-yyyy}", _inventory.Inventories[node.Name].DateConducted));
                    lvi = lvInventory.Items.Add("Target area");
                    lvi.SubItems.Add(_inventory.Inventories[node.Name].TargetArea);
                    lvi = lvInventory.Items.Add("");

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
                    var provinceLevels = _inventory.NumberOfMunicipalitiesBarangays(node.Parent.Name, node.Name);
                    lvi = lvInventory.Items.Add("Number of municipalities");
                    lvi.ImageKey = "Level03";
                    lvi.SubItems.Add(provinceLevels.municipalityCount.ToString());
                    lvi = lvInventory.Items.Add("Number of barangays");
                    lvi.SubItems.Add(provinceLevels.barangayCount.ToString());
                    lvi.ImageKey = "Level02";
                    break;

                case "municipality":
                    lvi = lvInventory.Items.Add("Number of barangays");
                    lvi.SubItems.Add(_inventory.NumberOfBarangays(node.Parent.Parent.Name, node.Parent.Name, node.Name).ToString());
                    lvi.ImageKey = "Level02";
                    break;

                case "barangay":
                    lvi = lvInventory.Items.Add("Number of sitios");
                    lvi.SubItems.Add(_inventory.NumberOfSitio(node.Parent.Parent.Parent.Name, node.Parent.Parent.Name, node.Parent.Name, node.Name).ToString());
                    lvi.ImageKey = "Level01";
                    break;

                case "sitio":
                    break;
            }

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

            if (_treeLevel == "municipality")
            {
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
                                lvi = lvInventory.Items.Add("  " + currentSitio);
                            }
                            else
                            {
                                lvi = lvInventory.Items.Add("  Entire barangay");
                            }
                            lvi.ImageKey = "Level01";
                            lvi.Tag = "sitio";
                            lvi.Name = item.barangayInventoryGUID;
                        }
                        if (item.gearClass != "" && item.gearVariation != "")
                        {
                            lvi = lvInventory.Items.Add("       " + item.gearClass + "-" + item.gearVariation);
                            lvi.Name = (item.dataGuid);
                            lvi.SubItems.Add(item.total.ToString());
                            lvi.ImageKey = gear.GearClassImageKeyFromGearClasName(item.gearClass);
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
                lvi = lvInventory.Items.Add("");
                lvi = lvInventory.Items.Add("");
                foreach (var item in gearInventoryDict)
                {
                    lvi = lvInventory.Items.Add(item.Value.gearClass + "-" + item.Key);
                    lvi.SubItems.Add(item.Value.total.ToString());
                    lvi.ImageKey = gear.GearClassImageKeyFromGearClasName(item.Value.gearClass);
                }
            }
        }

        private void AddInventory()
        {
            var inventoryEditForm = new GearInventoryEditForm(_treeLevel, _aoi, _inventory, this);
            inventoryEditForm.AddNewInventory();
            inventoryEditForm.ShowDialog(this);
        }

        private void OnContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            e.ClickedItem.Owner.Visible = false;
            var inventoryEditForm = new GearInventoryEditForm(_treeLevel, _aoi, _inventory, this);
            switch (e.ClickedItem.Name)
            {
                case "itemAddInventory":
                    AddInventory();
                    break;

                case "itemAddBarangay":
                    TreeNode nd = treeInventory.SelectedNode;
                    switch (_treeLevel)
                    {
                        case "targetAreaInventory":
                            inventoryEditForm.AddNewBarangyInventory(_inventoryGuid);
                            break;

                        case "province":
                            //SetLGULevel(_inventoryGuid, nd.Name);
                            inventoryEditForm.AddNewBarangyInventory(_inventoryGuid, nd.Name);

                            break;

                        case "municipality":
                            inventoryEditForm.AddNewBarangyInventory(_inventoryGuid, nd.Parent.Name, nd.Name);

                            break;

                        case "barangay":
                            if (nd.FirstNode.Name == "entireBarangay")
                            {
                                MessageBox.Show("Cannot add a new sitio because sitios are already included in 'Entire barangay'",
                                                "Cannot a new sitio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            else
                            {
                                inventoryEditForm.AddNewBarangyInventory(_inventoryGuid, nd.Parent.Parent.Name, nd.Parent.Name, nd.Name);
                            }
                            break;
                    }
                    //inventoryEditForm.AddNewBarangyInventory(_inventoryGuid);
                    break;

                case "itemAddFishingGear":
                    inventoryEditForm.AddNewGearInventory(_barangayInventoryGuid, _sitioCountCommercial, _sitioCountMunicipalMotorized, _sitioCountMunicipalNonMotorized, _sitioCountFishers);
                    break;
            }
            inventoryEditForm.ShowDialog(this);
        }

        private void OnListViewDoubleClick(object sender, EventArgs e)
        {
            var inventoryEditForm = new GearInventoryEditForm(_treeLevel, _aoi, _inventory, this);
            switch (_treeLevel)
            {
                case "targetAreaInventory":
                    inventoryEditForm.EditInventoryLevel(_inventoryGuid, lvInventory.Items[0].SubItems[1].Text, DateTime.Parse(lvInventory.Items[1].SubItems[1].Text));
                    break;

                case "sitio":
                    var nd = treeInventory.SelectedNode;
                    inventoryEditForm.EditInventoryLevel(_barangayInventoryGuid,
                                                        global.MunicipalityNumberFromString(nd.Parent.Parent.Parent.Text, nd.Parent.Parent.Text), nd.Parent.Text, nd.Text,
                                                        int.Parse(lvInventory.Items[0].SubItems[1].Text), int.Parse(lvInventory.Items[1].SubItems[1].Text),
                                                        int.Parse(lvInventory.Items[2].SubItems[1].Text), int.Parse(lvInventory.Items[3].SubItems[1].Text));
                    break;

                case "gearVariation":
                    inventoryEditForm.EditInventoryLevel(_currentGearInventoryGuid);
                    break;

                case "municipality":
                    if (_listTargetHit.Tag != null && _listTargetHit.Tag.ToString() == "gearVariation")
                    {
                        inventoryEditForm.EditInventoryLevel(_listTargetHit.Name);
                    }
                    else if (_listTargetHit.Tag != null && _listTargetHit.Tag.ToString() == "sitio")
                    {
                        return;
                    }
                    else
                    {
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
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            _listTargetHit = lvInventory.HitTest(e.X, e.Y).Item;
        }
    }
}
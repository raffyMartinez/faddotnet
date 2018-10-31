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
        private aoi _aoi;
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

        public static GearInventoryForm GetInstance(aoi aoi)
        {
            if (_instance == null) return new GearInventoryForm(aoi);
            return _instance;
        }

        public GearInventoryForm(aoi aoi)
        {
            InitializeComponent();
            _aoi = aoi;
            _inventory = new FishingGearInventory(aoi);
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
        }

        public void SetLGULevel(string inventoryGUID, string province = "", string municipality = "", string barangay = "")
        {
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            SetupListview();
            var nd = treeInventory.Nodes.Add("root", "Fishing gear inventory");
            nd.Tag = "root";
            foreach (var item in _inventory.Inventories)
            {
                var subNode = nd.Nodes.Add(item.Key, item.Value.InventoryName);
                subNode.Tag = "targetAreaInventory";
                subNode.Nodes.Add("***dummy***");
            }
            if (nd.Nodes.Count == 0)
            {
                nd.Nodes.Add("***dummy***");
            }
            _treeLevel = nd.Tag.ToString();
            ConfigListView(treeInventory.Nodes["root"]);
            global.LoadFormSettings(this, true);
        }

        public void RefreshSitioLevelInventory(string province, string municipality, string barangay)
        {
            TreeViewEventArgs e = new TreeViewEventArgs(_provinceNode.Parent);
            _refreshSitioNumbers = true;
            OnTreeAfterExpand(null, e);

            e.Node.Nodes[province].Expand();
            e.Node.Nodes[province].Nodes[municipality].Expand();
            e.Node.Nodes[province].Nodes[municipality].Nodes[barangay].Expand();
            treeInventory.SelectedNode = e.Node.Nodes[province].Nodes[municipality].Nodes[barangay];
            treeInventory.SelectedNode.Tag = "barangay";
            ConfigListView(treeInventory.SelectedNode);
        }

        private void OnTreeAfterExpand(object sender, TreeViewEventArgs e)
        {
            _treeLevel = e.Node.Tag.ToString();
            if (e.Node.FirstNode.Text == "***dummy***" || _refreshSitioNumbers)
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

                            if (!provNode.Nodes.ContainsKey(item.Value.Municipality))
                            {
                                munNode = provNode.Nodes.Add(item.Value.Municipality, item.Value.Municipality);
                            }
                            else
                            {
                                munNode = provNode.Nodes[item.Value.Municipality];
                            }
                            munNode.Tag = "municipality";

                            if (!munNode.Nodes.ContainsKey(item.Value.barangay))
                            {
                                brgyNode = munNode.Nodes.Add(item.Value.barangay, item.Value.barangay);
                            }
                            else
                            {
                                brgyNode = munNode.Nodes[item.Value.barangay];
                            }
                            brgyNode.Tag = "barangay";

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
            var menuItem = contextMenu.Items.Add("Add gear inventory");
            menuItem.Name = "itemAddInventory";
            menuItem.Enabled = _treeLevel == "root";

            if (_treeLevel == "barangay")
            {
                menuItem = contextMenu.Items.Add("Add sitio");
            }
            else
            {
                menuItem = contextMenu.Items.Add("Add barangay");
            }
            menuItem.Name = "itemAddBarangay";
            menuItem.Enabled = _treeLevel == "targetAreaInventory" || _treeLevel == "province"
                                || _treeLevel == "municipality" || _treeLevel == "barangay";

            menuItem = contextMenu.Items.Add("Add fishing gear");
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
        }

        private void ConfigListViewGear(string variationInventoryGuid)
        {
            var item = _inventory.GetGearVariationInventoryData(variationInventoryGuid);
            lvInventory.Clear();
            lvInventory.Columns.Add("Property");
            lvInventory.Columns.Add("Value");
            lvInventory.Columns.Add("Value unit");
            SizeColumns(lvInventory);

            var lvi = lvInventory.Items.Add("Province");
            lvi.SubItems.Add(item.province);
            lvi = lvInventory.Items.Add("Municipality");
            lvi.SubItems.Add(item.municipality);
            lvi = lvInventory.Items.Add("Barangay");
            lvi.SubItems.Add(item.barangay);
            lvi = lvInventory.Items.Add("Sitio");
            lvi.SubItems.Add(item.sitio);
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

            lvi = lvInventory.Items.Add("Gears used in commercial fishing vessels");
            lvi.SubItems.Add(item.commercialCount.ToString());
            lvi = lvInventory.Items.Add("Gears used in municipal motorized fishing vessels");
            lvi.SubItems.Add(item.motorizedCount.ToString());
            lvi = lvInventory.Items.Add("Gears used in municipal motorized fishing vessels");
            lvi.SubItems.Add(item.nonMotorizedCount.ToString());
            lvi = lvInventory.Items.Add("No boat gears");
            lvi.SubItems.Add(item.noBoatCount.ToString());

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
            switch (_treeLevel)
            {
                case "root":
                    lvi = lvInventory.Items.Add("Number of gear inventories");
                    lvi.SubItems.Add(_inventory.Inventories.Count.ToString());
                    break;

                case "targetAreaInventory":
                    //_inventoryGuid = treeInventory.SelectedNode.Name;
                    _inventoryGuid = node.Name;
                    var values = _inventory.GetLevelSummary(_inventoryGuid);
                    SetupListSummaryView(values.totalFishers, values.totalCommercial, values.totalMotorized, values.totalNonMotorized, node);

                    break;

                case "province":
                    _provinceNode = node;
                    values = _inventory.GetLevelSummary(node.Parent.Name, node.Name);
                    SetupListSummaryView(values.totalFishers, values.totalCommercial, values.totalMotorized, values.totalNonMotorized, node);
                    break;

                case "municipality":
                    _provinceNode = node.Parent;
                    values = _inventory.GetLevelSummary(node.Parent.Parent.Name, node.Parent.Name, node.Name);
                    SetupListSummaryView(values.totalFishers, values.totalCommercial, values.totalMotorized, values.totalNonMotorized, node);
                    break;

                case "barangay":
                    _provinceNode = node.Parent.Parent;
                    values = _inventory.GetLevelSummary(node.Parent.Parent.Parent.Name, node.Parent.Parent.Name, node.Parent.Name, node.Name);
                    SetupListSummaryView(values.totalFishers, values.totalCommercial, values.totalMotorized, values.totalNonMotorized, node);
                    break;

                case "sitio":
                    _provinceNode = node.Parent.Parent.Parent;
                    var numbers = _inventory.GetSitioNumbers(node.Parent.Parent.Parent.Parent.Name, node.Parent.Parent.Parent.Name, node.Parent.Parent.Name, node.Parent.Name, node.Name);
                    _sitioCountCommercial = numbers.commercialCount;
                    _sitioCountFishers = numbers.fisherCount;
                    _sitioCountMunicipalMotorized = numbers.motorizedCount;
                    _sitioCountMunicipalNonMotorized = numbers.nonMotorizedCount;
                    lvi = lvInventory.Items.Add("Number of fishers");
                    lvi.SubItems.Add(_sitioCountFishers.ToString());
                    lvi = lvInventory.Items.Add("Number of municipal motorized vessels");
                    lvi.SubItems.Add(_sitioCountMunicipalMotorized.ToString());
                    lvi = lvInventory.Items.Add("Number of municipal non-motorized vessels");
                    lvi.SubItems.Add(_sitioCountMunicipalNonMotorized.ToString());
                    lvi = lvInventory.Items.Add("Number of commercial vessels");
                    lvi.SubItems.Add(_sitioCountCommercial.ToString());

                    lvi = lvInventory.Items.Add("Total number of fishing gear types");
                    lvi = lvInventory.Items.Add("Total number of fishing gears inventoried");

                    _sitioNode = node;
                    ShowSitioGearInventory(_sitioNode);
                    break;
            }
            SizeColumns(lvInventory, false);
        }

        public void RefreshSitioGearInventory()
        {
            ShowSitioGearInventory(_sitioNode);
        }

        private void ShowSitioGearInventory(TreeNode node)
        {
            _barangayInventoryGuid = _inventory.BarangayInventoryGuid(node.Parent.Parent.Parent.Parent.Name, node.Parent.Parent.Parent.Name, node.Parent.Parent.Name, node.Parent.Name, node.Name);
            foreach (var item in _inventory.ReadSitioGearInventory(_barangayInventoryGuid))
            {
                if (!node.Nodes.ContainsKey(item.variationInventoryGuid))
                {
                    var nd = node.Nodes.Add(item.variationInventoryGuid, item.gearVariation);
                    nd.Tag = "gearVariation";
                }
            }
        }

        private void SetupListSummaryView(int fisherCount, int commercialCount, int motorizedCount, int nonMotorizedCount, TreeNode node = null)
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
                    lvi = lvInventory.Items.Add("Number of provinces");
                    lvi.SubItems.Add(node.Nodes.Count.ToString());
                    var inventoryLevels = _inventory.NumberOfMunicipalitiesBarangays(node.Name);
                    lvi = lvInventory.Items.Add("Number of municipalities");
                    lvi.SubItems.Add(inventoryLevels.municipalityCount.ToString());
                    lvi = lvInventory.Items.Add("Number of barangays");
                    lvi.SubItems.Add(inventoryLevels.barangayCount.ToString());
                    break;

                case "province":
                    var provinceLevels = _inventory.NumberOfMunicipalitiesBarangays(node.Parent.Name, node.Name);
                    lvi = lvInventory.Items.Add("Number of municipalities");
                    lvi.SubItems.Add(provinceLevels.municipalityCount.ToString());
                    lvi = lvInventory.Items.Add("Number of barangays");
                    lvi.SubItems.Add(provinceLevels.barangayCount.ToString());
                    break;

                case "municipality":
                    lvi = lvInventory.Items.Add("Number of barangays");
                    lvi.SubItems.Add(_inventory.NumberOfBarangays(node.Parent.Parent.Name, node.Parent.Name, node.Name).ToString());
                    break;

                case "barangay":
                    lvi = lvInventory.Items.Add("Number of sitios");
                    lvi.SubItems.Add(_inventory.NumberOfSitio(node.Parent.Parent.Parent.Name, node.Parent.Parent.Name, node.Parent.Name, node.Name).ToString());
                    break;

                case "sitio":
                    break;
            }

            lvi = lvInventory.Items.Add("Total number of fishers");
            lvi.SubItems.Add(fisherCount.ToString());
            lvi = lvInventory.Items.Add("Total number of municipal motorized vessels");
            lvi.SubItems.Add(motorizedCount.ToString());
            lvi = lvInventory.Items.Add("Total number of municipal non-motorized vessels");
            lvi.SubItems.Add(nonMotorizedCount.ToString());
            lvi = lvInventory.Items.Add("Total number of commercial vessels");
            lvi.SubItems.Add(commercialCount.ToString());
        }

        private void OnContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            e.ClickedItem.Owner.Visible = false;
            var inventoryEditForm = new GearInventoryEditForm(_treeLevel, _aoi, _inventory, this);
            switch (e.ClickedItem.Name)
            {
                case "itemAddInventory":
                    inventoryEditForm.AddNewInventory();
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
                            inventoryEditForm.AddNewBarangyInventory(_inventoryGuid, nd.Parent.Parent.Name, nd.Parent.Name, nd.Name);
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
    }
}
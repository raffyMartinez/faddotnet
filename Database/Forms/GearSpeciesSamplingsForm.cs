using FAD3.Database.Classes;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FAD3
{
    public partial class GearSpeciesSamplingsForm : Form
    {
        private string _itemNameGuid;
        private string _itemName;
        private Form _parentForm;
        private static GearSpeciesSamplingsForm _instance;
        private OccurenceDataType _mappingType;

        private Dictionary<string, (string targetAreaName, string refNo, string landingSite, string gearClassName, string gear,
                        DateTime samplingDate, string fishingGround, string vesselType, double wtCatch, string GUIDs, string EnumeratorName)> _samplings;

        public static GearSpeciesSamplingsForm GetInstance(string itemNameGuid, string itemName, Form parent, OccurenceDataType mappingType)
        {
            if (_instance == null) _instance = new GearSpeciesSamplingsForm(itemNameGuid, itemName, parent, mappingType);
            return _instance;
        }

        public GearSpeciesSamplingsForm(string itemNameGuid, string itemName, Form parent, OccurenceDataType mappingType)
        {
            InitializeComponent();
            _itemNameGuid = itemNameGuid;
            _itemName = itemName;
            _parentForm = parent;
            _mappingType = mappingType;
        }

        public void setItemGuid_Name_Parent(string itemNameGuid, string itemName, Form parent)
        {
            _itemNameGuid = itemNameGuid;
            _itemName = itemName;
            _parentForm = parent;
            setUpUI();
            treeList.SelectedNode = treeList.Nodes["root"];
        }

        private void setUpUI()
        {
            treeList.ImageList = global.mainForm.treeImages;

            var nodeTargetArea = new TreeNode();
            var nodeLandingSite = new TreeNode();
            var nodeGear = new TreeNode();
            var nodeMonth = new TreeNode();
            var samplingMonth = "";
            var imageKey = "";

            if (_parentForm.GetType().Name == "AllSpeciesForm")
            {
                _samplings = CatchName.RetrieveSamplingsFromCatchName(_itemNameGuid);
            }
            else if (_parentForm.GetType().Name == "GearCodesUsageForm")
            {
                _samplings = Gear.gearSamplings(_itemNameGuid);
            }

            if (_samplings.Count > 0)
            {
                treeList.Nodes.Clear();
                lvSamplings.Items.Clear();

                var node = treeList.Nodes.Add("root", _itemName, "db");
                node.Tag = "db";

                foreach (var item in _samplings)
                {
                    if (node.Nodes.ContainsKey(item.Value.targetAreaName))
                    {
                        nodeTargetArea = node.Nodes[item.Value.targetAreaName];
                        if (nodeTargetArea.Nodes.ContainsKey(item.Value.landingSite))
                        {
                            nodeLandingSite = nodeTargetArea.Nodes[item.Value.landingSite];
                            if (nodeLandingSite.Nodes.ContainsKey(item.Value.gear))
                            {
                                nodeGear = nodeLandingSite.Nodes[item.Value.gear];
                                samplingMonth = string.Format("{0:MMM-yyyy}", item.Value.samplingDate);
                                if (!nodeGear.Nodes.ContainsKey(samplingMonth))
                                {
                                    nodeMonth = nodeGear.Nodes.Add(samplingMonth, samplingMonth, "MonthGear");
                                    nodeMonth.Tag = "samplingMonth";
                                }
                            }
                            else
                            {
                                imageKey = Gear.GearClassImageKeyFromGearClasName(item.Value.gearClassName);
                                nodeGear = nodeLandingSite.Nodes.Add(item.Value.gear, item.Value.gear, imageKey);
                                nodeGear.Tag = "gear";
                                samplingMonth = string.Format("{0:MMM-yyyy}", item.Value.samplingDate);
                                nodeMonth = nodeGear.Nodes.Add(samplingMonth, samplingMonth, "MonthGear");
                                nodeMonth.Tag = "samplingMonth";
                            }
                        }
                        else
                        {
                            nodeLandingSite = nodeTargetArea.Nodes.Add(item.Value.landingSite, item.Value.landingSite, "LandingSite");
                            nodeLandingSite.Tag = "landingSite";
                            imageKey = Gear.GearClassImageKeyFromGearClasName(item.Value.gearClassName);
                            nodeGear = nodeLandingSite.Nodes.Add(item.Value.gear, item.Value.gear, imageKey);
                            nodeGear.Tag = "gear";
                            samplingMonth = string.Format("{0:MMM-yyyy}", item.Value.samplingDate);
                            nodeMonth = nodeGear.Nodes.Add(samplingMonth, samplingMonth, "MonthGear");
                            nodeMonth.Tag = "samplingMonth";
                        }
                    }
                    else
                    {
                        nodeTargetArea = node.Nodes.Add(item.Value.targetAreaName, item.Value.targetAreaName, "AOI");
                        nodeTargetArea.Tag = "aoi";
                        nodeLandingSite = nodeTargetArea.Nodes.Add(item.Value.landingSite, item.Value.landingSite, "LandingSite");
                        nodeLandingSite.Tag = "landingSite";
                        imageKey = Gear.GearClassImageKeyFromGearClasName(item.Value.gearClassName);
                        nodeGear = nodeLandingSite.Nodes.Add(item.Value.gear, item.Value.gear, imageKey);
                        nodeGear.Tag = "gear";
                        samplingMonth = string.Format("{0:MMM-yyyy}", item.Value.samplingDate);
                        nodeMonth = nodeGear.Nodes.Add(samplingMonth, samplingMonth, "MonthGear");
                        nodeMonth.Tag = "samplingMonth";
                    }
                }
            }
            else
            {
                MessageBox.Show("There are no sampling records to show", "No samplings", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
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

        private void Form_Load(object sender, EventArgs e)
        {
            SetupListView();
            setUpUI();
            switch (_mappingType)
            {
                case OccurenceDataType.Gear:
                    Text = $"List of samplings of gear: {_itemName}";
                    break;

                case OccurenceDataType.Species:
                    Text = $"List of samplings of species: {_itemName}";
                    break;
            }
        }

        private void SetupListView()
        {
            lvSamplings.With(o =>
            {
                o.Columns.Clear();
                o.View = View.Details;
                o.FullRowSelect = true;

                o.Columns.Add("Target area name");
                o.Columns.Add("Landing site name");
                o.Columns.Add("Type of gear used");
                o.Columns.Add("Month sampled");
                o.Columns.Add("Sampling reference number");
                o.Columns.Add("Fishing ground");
                o.Columns.Add("Sampling date");
                o.Columns.Add("Weight of catch");
                o.Columns.Add("Type of vessel used");
                o.Columns.Add("Name of enumerator");

                //foreach (ColumnHeader c in lvSamplings.Columns)
                //{
                //    c.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                //}
            });
            SizeColumns(lvSamplings);
        }

        private void PopulateList(TreeNode node)
        {
            lvSamplings.Visible = false;
            node.With(nd =>
            {
                bool Proceed = false;

                foreach (var item in _samplings)
                {
                    switch (nd.Tag.ToString())
                    {
                        case "db":
                            Proceed = true;

                            break;

                        case "aoi":
                            Proceed = item.Value.targetAreaName == nd.Text;

                            break;

                        case "landingSite":
                            Proceed = item.Value.targetAreaName == nd.Parent.Text
                                && item.Value.landingSite == nd.Text;

                            break;

                        case "gear":
                            Proceed = item.Value.targetAreaName == nd.Parent.Parent.Text
                                 && item.Value.landingSite == nd.Parent.Text
                                 && item.Value.gear == nd.Text;

                            break;

                        case "samplingMonth":
                            Proceed = item.Value.targetAreaName == nd.Parent.Parent.Parent.Text
                                && item.Value.landingSite == nd.Parent.Parent.Text
                                && item.Value.gear == nd.Parent.Text
                                && string.Format("{0:MMM-yyyy}", item.Value.samplingDate) == nd.Text;

                            break;
                    }

                    if (Proceed)
                    {
                        var lvi = new ListViewItem(new string[] { item.Value.targetAreaName, item.Value.landingSite, item.Value.gear, string.Format("{0:MMM-yyyy}",
                            item.Value.samplingDate), item.Value.refNo, item.Value.fishingGround, string.Format("{0:MMM-dd-yyyy}", item.Value.samplingDate),
                            item.Value.wtCatch.ToString(), item.Value.vesselType, item.Value.EnumeratorName });

                        lvi.Name = item.Key;
                        lvi.Tag = item.Value.GUIDs;
                        lvSamplings.Items.Add(lvi);
                    }
                }
            });
            SizeColumns(lvSamplings, false);
            lvSamplings.Visible = true;
        }

        private void OnTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            lvSamplings.Items.Clear();
            e.Node.SelectedImageKey = e.Node.ImageKey;
            PopulateList(e.Node);
        }

        private void OnForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
        }
    }
}
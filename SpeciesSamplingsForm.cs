using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FAD3
{
    public partial class SpeciesSamplingsForm : Form
    {
        private string _catchNameGuid;
        private string _catchName;
        private AllSpeciesForm _parentForm;

        private List<(string samplingGuid, string targetArea, string landingSite, string GearClass, string gear,
                            string referenceNumber, DateTime? samplingDate, string fishingGround, Double? catchWeight,
                            string EnumeratorName, string VesselType)> _samplings;

        public SpeciesSamplingsForm(string catchNameGuid, string catchName, AllSpeciesForm parent)
        {
            InitializeComponent();
            _catchNameGuid = catchNameGuid;
            _catchName = catchName;
            _parentForm = parent;
        }

        private void SpeciesSamplingsForm_Load(object sender, EventArgs e)
        {
            SetupListView();

            treeList.ImageList = _parentForm.parentForm.treeImages;
            var node = treeList.Nodes.Add("root", _catchName, "db");
            node.Tag = "db";
            var nodeTargetArea = new TreeNode();
            var nodeLandingSite = new TreeNode();
            var nodeGear = new TreeNode();
            var nodeMonth = new TreeNode();
            var samplingMonth = "";
            var imageKey = "";
            _samplings = CatchName.RetrieveSamplingsFromCatchName(_catchNameGuid);
            foreach (var item in _samplings)
            {
                if (node.Nodes.ContainsKey(item.targetArea))
                {
                    nodeTargetArea = node.Nodes[item.targetArea];
                    if (nodeTargetArea.Nodes.ContainsKey(item.landingSite))
                    {
                        nodeLandingSite = nodeTargetArea.Nodes[item.landingSite];
                        if (nodeLandingSite.Nodes.ContainsKey(item.gear))
                        {
                            nodeGear = nodeLandingSite.Nodes[item.gear];
                            samplingMonth = string.Format("{0:MMM-yyyy}", item.samplingDate);
                            if (!nodeGear.Nodes.ContainsKey(samplingMonth))
                            {
                                nodeMonth = nodeGear.Nodes.Add(samplingMonth, samplingMonth, "MonthGear");
                                nodeMonth.Tag = "samplingMonth";
                            }
                        }
                        else
                        {
                            imageKey = gear.GearClassImageKeyFromGearClasName(item.GearClass);
                            nodeGear = nodeLandingSite.Nodes.Add(item.gear, item.gear, imageKey);
                            nodeGear.Tag = "gear";
                            samplingMonth = string.Format("{0:MMM-yyyy}", item.samplingDate);
                            nodeMonth = nodeGear.Nodes.Add(samplingMonth, samplingMonth, "MonthGear");
                            nodeMonth.Tag = "samplingMonth";
                        }
                    }
                    else
                    {
                        nodeLandingSite = nodeTargetArea.Nodes.Add(item.landingSite, item.landingSite, "LandingSite");
                        nodeLandingSite.Tag = "landingSite";
                        imageKey = gear.GearClassImageKeyFromGearClasName(item.GearClass);
                        nodeGear = nodeLandingSite.Nodes.Add(item.gear, item.gear, imageKey);
                        nodeGear.Tag = "gear";
                        samplingMonth = string.Format("{0:MMM-yyyy}", item.samplingDate);
                        nodeMonth = nodeGear.Nodes.Add(samplingMonth, samplingMonth, "MonthGear");
                        nodeMonth.Tag = "samplingMonth";
                    }
                }
                else
                {
                    nodeTargetArea = node.Nodes.Add(item.targetArea, item.targetArea, "AOI");
                    nodeTargetArea.Tag = "aoi";
                    nodeLandingSite = nodeTargetArea.Nodes.Add(item.landingSite, item.landingSite, "LandingSite");
                    nodeLandingSite.Tag = "landingSite";
                    imageKey = gear.GearClassImageKeyFromGearClasName(item.GearClass);
                    nodeGear = nodeLandingSite.Nodes.Add(item.gear, item.gear, imageKey);
                    nodeGear.Tag = "gear";
                    samplingMonth = string.Format("{0:MMM-yyyy}", item.samplingDate);
                    nodeMonth = nodeGear.Nodes.Add(samplingMonth, samplingMonth, "MonthGear");
                    nodeMonth.Tag = "samplingMonth";
                }
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

                foreach (ColumnHeader c in lvSamplings.Columns)
                {
                    c.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                }
            });
        }

        private void PopulateList(TreeNode node)
        {
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
                            Proceed = item.targetArea == nd.Text;

                            break;

                        case "landingSite":
                            Proceed = item.targetArea == nd.Parent.Text
                                && item.landingSite == nd.Text;

                            break;

                        case "gear":
                            Proceed = item.targetArea == nd.Parent.Parent.Text
                                 && item.landingSite == nd.Parent.Text
                                 && item.gear == nd.Text;

                            break;

                        case "samplingMonth":
                            Proceed = item.targetArea == nd.Parent.Parent.Parent.Text
                                && item.landingSite == nd.Parent.Parent.Text
                                && item.gear == nd.Parent.Text
                                && string.Format("{0:MMM-yyyy}", item.samplingDate) == nd.Text;

                            break;
                    }

                    if (Proceed)
                    {
                        var lvi = new ListViewItem(new string[] { item.targetArea, item.landingSite, item.gear, string.Format("{0:MMM-yyyy}",
                            item.samplingDate), item.referenceNumber, item.fishingGround, string.Format("{0:MMM-dd-yyyy}", item.samplingDate),
                            item.catchWeight.ToString(), item.VesselType, item.EnumeratorName });

                        lvi.Name = item.samplingGuid;
                        lvSamplings.Items.Add(lvi);
                    }
                }
            });
        }

        private void treeList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            lvSamplings.Items.Clear();
            e.Node.SelectedImageKey = e.Node.ImageKey;
            PopulateList(e.Node);
        }
    }
}
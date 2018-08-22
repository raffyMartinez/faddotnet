/*
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/13/2016
 * Time: 11:50 AM
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace FAD3
{
    /// <summary>
    /// Description of frmEnumerator.
    /// </summary>
    public partial class EnumeratorForm : Form
    {
        private aoi _AOI = new aoi();
        private string _enumeratorGuid = "";
        private string _enumeratorName = "";
        private bool _IsNew = false;
        private MainForm _parentForm;
        private List<string> _removedEnumerators = new List<string>();
        private static EnumeratorForm _instance;

        private Dictionary<string, (string targetAreaName, string refNo, string landingSite, string gearClassName, string gear,
                                DateTime samplingDate, string fishingGround, string vesselType, double wtCatch, int rows, string GUIDs)> _samplings;

        public static EnumeratorForm GetInstance(MainForm Parent)
        {
            if (_instance == null) _instance = new EnumeratorForm(Parent);
            return _instance;
        }

        public void SetParentAndEnumerator(string enumeratorGuid, MainForm Parent)
        {
            _parentForm = Parent;
            _enumeratorGuid = enumeratorGuid;
            Enumerators.EnumeratorGuid = _enumeratorGuid;
            ConfigureListEnumeratorSamplings();
        }

        public static EnumeratorForm GetInstance(string enumeratorGuid, MainForm Parent)
        {
            if (_instance == null) _instance = new EnumeratorForm(enumeratorGuid, Parent);
            return _instance;
        }

        public EnumeratorForm(MainForm Parent)
        {
            //default conructor
            InitializeComponent();
            _parentForm = Parent;
        }

        public EnumeratorForm(string enumeratorGuid, MainForm Parent)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
            _parentForm = Parent;
            _enumeratorGuid = enumeratorGuid;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            if (_enumeratorGuid.Length > 0)
            {
                global.LoadFormSettings(this);
                Enumerators.EnumeratorGuid = _enumeratorGuid;
                ConfigureListEnumeratorSamplings();
                Text = "Enumerator details";
                FormBorderStyle = FormBorderStyle.Sizable;
            }
            else
            {
                panelTop.Hide();
                tree.Hide();
                ConfigureListEnumerators();
                Text = "Landing site enumerators";
            }
        }

        public void EditedEnumerator(string enumeratorGuid, string enumeratorName, DateTime dateHired, bool isActive, global.fad3DataStatus dataStatus)

        {
            if (dataStatus == global.fad3DataStatus.statusNew)
            {
                var lvi = lvEnumerators.Items.Add(enumeratorGuid, enumeratorName, null);
                lvi.SubItems.Add(dateHired.ToString("MMM-dd-yyyy"));
                lvi.SubItems.Add(isActive.ToString());
                lvi.Tag = dataStatus;
            }
            else if (dataStatus == global.fad3DataStatus.statusEdited)
            {
                var lvi = lvEnumerators.Items[enumeratorGuid];
                lvi.Text = enumeratorName;
                lvi.SubItems[1].Text = dateHired.ToString("MMM-dd-yyyy");
                lvi.SubItems[2].Text = isActive.ToString();
                lvi.Tag = dataStatus;
            }

            foreach (ColumnHeader col in lvEnumerators.Columns)
            {
                switch (col.Text)
                {
                    case "Enumerator name":
                    case "Date hired":
                        col.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                        break;

                    default:
                        col.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                        break;
                }
            }
        }

        public aoi AOI
        {
            get { return _AOI; }
            set { _AOI = value; }
        }

        public void AddNew()
        {
            _IsNew = true;
            this.Text = "Add a new enumerator for " + _AOI.AOIName;
        }

        protected bool CheckDate(String date)
        {
            DateTime Temp;

            if (DateTime.TryParse(date, out Temp) == true)
                return true;
            else
                return false;
        }

        private void ConfigureListEnumerators()
        {
            labelListView.Visible = false;
            labelHireDate.Visible = false;
            labelEnumeratorName.Visible = false;

            txtHireDate.Visible = false;
            txtName.Visible = false;
            chkActive.Visible = false;

            Width = 450;

            lvEnumerators.With(o =>
            {
                o.Width = Width - 60;
                o.Location = new Point(0, 10);
                o.View = View.Details;
                o.FullRowSelect = true;
                o.Columns.Add("Enumerator name");
                o.Columns.Add("Date hired");
                o.Columns.Add("Active");
                o.Columns.Add("");
                o.Height = buttonOK.Top - o.Top - 10;

                foreach (ColumnHeader col in o.Columns)
                {
                    col.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                }
            });

            Button buttonAdd = new Button
            {
                Width = 30,
                Height = 30,
                Text = "+",
                Location = new Point(lvEnumerators.Left + lvEnumerators.Width + 10, lvEnumerators.Top),
                Name = "buttonAdd"
            };
            Button buttonRemove = new Button
            {
                Width = buttonAdd.Width,
                Height = buttonAdd.Height,
                Text = "-",
                Location = new Point(buttonAdd.Left, buttonAdd.Top + buttonAdd.Height + 10),
                Name = "buttonRemove"
            };

            buttonAdd.Font = new Font(Font.FontFamily.Name, 14, FontStyle.Bold);
            buttonRemove.Font = buttonAdd.Font;

            Controls.Add(buttonAdd);
            Controls.Add(buttonRemove);
            buttonAdd.Click += OnButtonClick;
            buttonRemove.Click += OnButtonClick;

            foreach (KeyValuePair<string, (string EnumeratorName, DateTime DateHired, bool IsActive, global.fad3DataStatus DataStatus)> kv in Enumerators.GetTargetAreaEnumerators())
            {
                var lvi = lvEnumerators.Items.Add(kv.Key, kv.Value.EnumeratorName, null);
                lvi.Tag = kv.Value.DataStatus;
                lvi.SubItems.Add(kv.Value.DateHired.ToString("MMM-dd-yyyy"));
                lvi.SubItems.Add(kv.Value.IsActive.ToString());
            }

            if (lvEnumerators.Items.Count > 0)
            {
                foreach (ColumnHeader col in lvEnumerators.Columns)
                {
                    switch (col.Text)
                    {
                        case "Enumerator name":
                            col.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                            break;

                        case "Date hired":
                        case "":
                        case "Active":
                            col.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                            break;
                    }
                }
            }
        }

        private void ConfigureListEnumeratorSamplings()
        {
            buttonCancel.Visible = false;

            labelListView.Visible = true;
            labelHireDate.Visible = true;
            labelEnumeratorName.Visible = true;

            txtHireDate.Visible = true;
            txtName.Visible = true;
            chkActive.Visible = true;

            lvEnumerators.With(o =>
            {
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
                o.Columns.Add("Rows");

                foreach (ColumnHeader c in o.Columns)
                {
                    c.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                }

                o.HideSelection = false;
                o.Location = new Point(tree.Width, tree.Top);
                o.Width = Width - tree.Width;
            });

            if (!_IsNew)
            {
                ReadData();
                _samplings = Enumerators.GetEnumeratorSamplings();
                ConfigureTree();
                PopulateList(tree.Nodes["root"]);
                Text = "Enumerator data and sampling list";
            }
        }

        private void tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            lvEnumerators.Items.Clear();
            e.Node.SelectedImageKey = e.Node.ImageKey;
            PopulateList(e.Node);
        }

        private void PopulateList(TreeNode node)
        {
            lvEnumerators.Items.Clear();
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
                            item.Value.wtCatch.ToString(), item.Value.vesselType, item.Value.rows.ToString() });

                        lvi.Name = item.Key;
                        lvi.Tag = item.Value.GUIDs;
                        lvEnumerators.Items.Add(lvi);
                    }
                }
            });
        }

        private void ConfigureTree()
        {
            tree.Nodes.Clear();
            tree.ImageList = _parentForm.treeImages;
            var node = tree.Nodes.Add("root", _enumeratorName, "db");
            node.Tag = "db";
            var nodeTargetArea = new TreeNode();
            var nodeLandingSite = new TreeNode();
            var nodeGear = new TreeNode();
            var nodeMonth = new TreeNode();
            var samplingMonth = "";
            var imageKey = "";
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
                            imageKey = gear.GearClassImageKeyFromGearClasName(item.Value.gearClassName);
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
                        imageKey = gear.GearClassImageKeyFromGearClasName(item.Value.gearClassName);
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
                    imageKey = gear.GearClassImageKeyFromGearClasName(item.Value.gearClassName);
                    nodeGear = nodeLandingSite.Nodes.Add(item.Value.gear, item.Value.gear, imageKey);
                    nodeGear.Tag = "gear";
                    samplingMonth = string.Format("{0:MMM-yyyy}", item.Value.samplingDate);
                    nodeMonth = nodeGear.Nodes.Add(samplingMonth, samplingMonth, "MonthGear");
                    nodeMonth.Tag = "samplingMonth";
                }
            }
        }

        private void EnumeratorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_enumeratorGuid.Length > 0)
                global.SaveFormSettings(this);

            _instance = null;
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonOK":
                    if (_enumeratorGuid.Length == 0)
                    {
                        if (lvEnumerators.Items.Count > 0 || _removedEnumerators.Count > 0)
                        {
                            var enumeratorsData = new Dictionary<string, (string enumeratorName, DateTime dateHired, bool isActive, global.fad3DataStatus DataStatus)>();

                            foreach (ListViewItem lvi in lvEnumerators.Items)
                            {
                                var DataStatus = (global.fad3DataStatus)lvi.Tag;

                                if (DataStatus != global.fad3DataStatus.statusFromDB)
                                    enumeratorsData.Add(lvi.Name, (lvi.Text, DateTime.Parse(lvi.SubItems[1].Text), bool.Parse(lvi.SubItems[2].Text), DataStatus));
                            }

                            foreach (var item in _removedEnumerators)
                            {
                                //actually all that is needed to delete an enumerator is the Guid and the DataStatus.
                                //The rest of the values in the tuple are not needed but they have to be filled up because
                                //the tuple does not accept optional parameters.
                                enumeratorsData.Add(item, ("", DateTime.Today, false, global.fad3DataStatus.statusForDeletion));
                            }

                            if ((enumeratorsData.Count > 0 || _removedEnumerators.Count > 0) && Enumerators.SaveTargetAreaEnumerators(enumeratorsData))
                            {
                                Close();

                                //then we refresh mainform to reflect any changes
                                _parentForm.RefreshLV("aoi");
                            }
                            else
                            {
                                if (enumeratorsData.Count == 0 && _removedEnumerators.Count == 0)
                                {
                                    Close();
                                }
                                else
                                {
                                    //something went wrong
                                    Logger.Log("Message = saving enumerator data was not completed\r\n" +
                                                    "Location = EnumeratorForm.cs.OnButtonClick");
                                }
                            }
                        }
                        else if (lvEnumerators.Items.Count == 0)
                        {
                            MessageBox.Show("Please add at least one enumerator", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        Close();
                    }
                    break;

                case "buttonCancel":
                    Close();
                    break;

                case "buttonAdd":

                    var eef = new EnumeratorEntryForm(this);
                    eef.ShowDialog(this);

                    break;

                case "buttonRemove":
                    var selectedEnumeratorGuid = lvEnumerators.SelectedItems[0].Name;
                    var NumberOfSampling = Enumerators.NumberOfSamplingsOfEnumerator(selectedEnumeratorGuid);
                    if (NumberOfSampling == 0)
                    {
                        if (MessageBox.Show("This enumerator was not able to do any sampling.\r\n" +
                                 "Do you still want to remove this enumerator?", "Please verify",
                                 MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
                        {
                            lvEnumerators.Items.Remove(lvEnumerators.Items[selectedEnumeratorGuid]);
                            _removedEnumerators.Add(selectedEnumeratorGuid);
                        }
                    }
                    else
                    {
                        var term = NumberOfSampling == 1 ? "sampling" : "samplings";
                        if (NumberOfSampling > 0)
                        {
                            if (MessageBox.Show($"This enumerator has conducted {NumberOfSampling} {term}.\r\n" +
                                               "You cannot remove this enumerator.\r\n\r\n" +
                                               "Instead, you can make the enumerator inactive by clicking on the OK button.",
                                               "Validation error", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                            {
                                lvEnumerators.SelectedItems[0].SubItems[2].Text = "False";
                                lvEnumerators.SelectedItems[0].Tag = global.fad3DataStatus.statusEdited;
                            }
                        }
                    }
                    break;
            }
        }

        private void OnlistEnumeratorSampling_DoubleClick(object sender, EventArgs e)
        {
            if (_enumeratorGuid.Length > 0)
            {
                lvEnumerators.SelectedItems[0].With(o =>
                {
                    string[] arr = o.Tag.ToString().Split('|');
                    Dictionary<string, string> mySampling = new Dictionary<string, string>();
                    mySampling.Add("SamplingID", o.Name);
                    mySampling.Add("LSGUID", arr[0]);
                    mySampling.Add("GearID", arr[1]);
                    mySampling.Add("SamplingDate", o.SubItems[3].Text);
                    _parentForm.EnumeratorSelectedSampling(mySampling);
                });
            }
            else
            {
                lvEnumerators.SelectedItems[0].With(o =>
                {
                    var eef = new EnumeratorEntryForm(o.Name, o.Text, DateTime.Parse(o.SubItems[1].Text), bool.Parse(o.SubItems[2].Text), this);
                    eef.ShowDialog(this);
                });
            }
        }

        private void OnListEnumeratorSamplingLeave(object sender, EventArgs e)
        {
            using (MainForm frm = new MainForm())
            {
                MainForm.SaveColumnWidthEx(sender, myContext: global.lvContext.EnumeratorSampling);
            }
        }

        private void OnTextBoxValidating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string c = txtHireDate.Text;
            if (c != "")
            {
                if (!CheckDate(c))
                {
                    MessageBox.Show("Please provide a proper date");
                    e.Cancel = true;
                }
            }
        }

        private void ReadData()
        {
            DataTable dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = "Select * from tblEnumerators where EnumeratorID =\"" + _enumeratorGuid + "\"";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        _enumeratorName = dr["EnumeratorName"].ToString();
                        txtName.Text = _enumeratorName;
                        DateTime dtm = (DateTime)dr["HireDate"];
                        txtHireDate.Text = string.Format("{0:MMM-dd-yyyy}", dtm);
                        chkActive.Checked = Convert.ToBoolean(dr["Active"]);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
        }
    }
}
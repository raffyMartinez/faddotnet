using FAD3.Database.Classes;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FAD3.Database.Forms
{
    public partial class DatabaseReportForm : Form
    {
        private static DatabaseReportForm _instance;
        private string _treeLevel;
        private TargetArea _targetArea;
        private Dictionary<string, string> _sampledYears;
        private string _topic;

        public TargetArea TargetArea
        {
            get { return _targetArea; }
            set
            {
                _targetArea = value;
                ShowSampledYears();
                Text = $"Reports: {_targetArea.TargetAreaName}";
                ReportGeneratorClass.TargetArea = TargetArea;
            }
        }

        public static DatabaseReportForm GetInstance()
        {
            if (_instance != null) return _instance;
            return null;
        }

        public static DatabaseReportForm GetInstance(string treeLevel, TargetArea targetArea)
        {
            if (_instance == null) _instance = new DatabaseReportForm(treeLevel, targetArea);
            return _instance;
        }

        public DatabaseReportForm(string treeLevel, TargetArea targetArea)
        {
            InitializeComponent();
            _treeLevel = treeLevel;
            _targetArea = targetArea;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            global.LoadFormSettings(this);
            var ndRoot = tvTopics.Nodes.Add("root", "Topics");
            ndRoot.Nodes.Add("nodeEffort", "Effort");
            ndRoot.Nodes.Add("nodeCatch", "Catch composition");
            Text = $"Reports: {_targetArea.TargetAreaName}";

            lvYears.View = View.Details;
            lvYears.Columns.Add("Year");
            lvYears.Columns.Add("Samples");
            lvYears.FullRowSelect = true;

            lvReports.View = View.Details;
            lvReports.Columns.Add("Row");
            lvReports.Columns.Add("Report title");
            lvReports.FullRowSelect = true;

            SizeColumns(lvYears);
            SizeColumns(lvReports);
            ShowSampledYears();
            ReportGeneratorClass.TargetArea = TargetArea;
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

        private void ShowSampledYears()
        {
            lvYears.Items.Clear();
            _sampledYears = _targetArea.ListYearsWithSamplingCount();
            if (_sampledYears.Count > 0)
            {
                foreach (var item in _sampledYears)
                {
                    var lvi = lvYears.Items.Add(item.Key, $"{item.Key}", null);
                    lvi.SubItems.Add(item.Value);
                }
                SizeColumns(lvYears, false);
            }
            else
            {
                SizeColumns(lvYears);
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            _instance = null;
            global.SaveFormSettings(this);
        }

        private void OnToolbarItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "tsbClose":
                    Close();
                    break;

                case "tsbExcel":
                    break;
            }
        }

        private void OnTreeNodeClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            lvReports.Items.Clear();
            switch (e.Node.Name)
            {
                case "nodeEffort":
                    int row = lvReports.Items.Count + 1;
                    var lvi = lvReports.Items.Add("effort", row.ToString(), null);
                    lvi.SubItems.Add($"Effort data of {TargetArea.TargetAreaName} target area.");
                    _topic = "effort";
                    break;

                case "nodeCatch":
                    row = lvReports.Items.Count + 1;
                    lvi = lvReports.Items.Add("catch", row.ToString(), null);
                    lvi.SubItems.Add($"Catch composition data of {TargetArea.TargetAreaName} target area.");
                    _topic = "catch";
                    break;
            }
            if (lvReports.Items.Count > 0)
            {
                SizeColumns(lvReports, false);
            }
        }

        private void OnListDoubleClick(object sender, EventArgs e)
        {
            List<int> years = new List<int>();
            foreach (ListViewItem lvi in lvYears.Items)
            {
                if (lvi.Checked)
                {
                    years.Add(int.Parse(lvi.Text));
                }
            }
            if (years.Count > 0)
            {
                ReportTableForm rtf = ReportTableForm.GetInstance(TargetArea, _topic, years);
                if (rtf.Visible)
                {
                    rtf.BringToFront();
                }
                else
                {
                    rtf.Show(this);
                }
            }
            else
            {
                MessageBox.Show("Please select one or more years", "No year selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
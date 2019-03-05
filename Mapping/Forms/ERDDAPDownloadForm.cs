using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MapWinGIS;

namespace FAD3.Mapping.Forms
{
    public partial class ERDDAPDownloadForm : Form
    {
        private static ERDDAPDownloadForm _instance;
        public Dictionary<string, (string unit, string description)> GridParameters { get; set; }
        public Dictionary<string, (string name, int size, double spacing)> Dimensions { get; set; }
        public string Title { get; set; }
        public string Credits { get; set; }
        public string DataAbstract { get; set; }
        public Extents DataExtents { get; set; }
        public string LegalConstraint { get; set; }
        public DateTime BeginPosition { get; set; }
        public DateTime EndPosition { get; set; }
        public Extents GridExtents { get; set; }

        public ERDDAPDownloadForm()
        {
            InitializeComponent();
        }

        public static ERDDAPDownloadForm GetInstance()
        {
            if (_instance == null) _instance = new ERDDAPDownloadForm();
            return _instance;
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

        private void OnFormLoad(object sender, EventArgs e)
        {
            lblTitle.Text = Title;
            lblTitle.BorderStyle = BorderStyle.None;
            txtboundEast.Text = DataExtents.xMin.ToString();
            txtboundWest.Text = DataExtents.xMax.ToString();
            txtboundNorth.Text = DataExtents.yMax.ToString();
            txtboundSouth.Text = DataExtents.yMin.ToString();

            lvGridParameters.View = View.Details;
            lvGridParameters.Columns.Clear();
            lvGridParameters.Columns.Add("Variable");
            lvGridParameters.Columns.Add("Description");
            lvGridParameters.Columns.Add("Type");
            lvGridParameters.CheckBoxes = true;
            lvGridParameters.FullRowSelect = true;
            SizeColumns(lvGridParameters);

            foreach (var item in GridParameters)
            {
                var lvi = lvGridParameters.Items.Add(item.Key);
                lvi.SubItems.Add(item.Value.description);
                lvi.SubItems.Add(item.Value.unit);
                lvi.Checked = true;
            }
            SizeColumns(lvGridParameters, false);

            TimeSpan diff = EndPosition - BeginPosition;
            double days = diff.TotalDays;
            var freq = Math.Round(days / Dimensions["temporal"].size, MidpointRounding.AwayFromZero);
            string dataFreq = "1 Day";
            switch (freq)
            {
                case 1:
                    break;

                case 30:
                    dataFreq = $"{freq} days";
                    break;

                default:
                    dataFreq = $"Every {freq} days";
                    break;
            }

            lblLatSpacing.Text = $"{Dimensions["column"].spacing.ToString("N5")} degrees";
            lblLonSpacing.Text = $"{Dimensions["row"].spacing.ToString("N5")} degrees";
            lblTimeSpacing.Text = dataFreq;
            lblLatSize.Text = $"{Dimensions["row"].size.ToString()}";
            lblLonSize.Text = $"{Dimensions["column"].size.ToString()}";
            lblTimeSize.Text = $"{Dimensions["temporal"].size.ToString()}";

            chkAltitude.Enabled = Dimensions["vertical"].name.Length > 0;
            txtAltitudeStride.Enabled = chkAltitude.Enabled;
            txtEndAltitude.Enabled = chkAltitude.Enabled;
            txtStartAltitude.Enabled = chkAltitude.Enabled;
            btnAltitudeHelp.Enabled = chkAltitude.Enabled;
            if (chkAltitude.Enabled)
            {
                lblAltitudeSpacing.Text = $"{Dimensions["vertical"].spacing.ToString("N5")}";
                lblAltitudeSize.Text = $"{Dimensions["vertical"].size.ToString()}";
            }
            else
            {
                lblAltitudeSpacing.Text = "";
                lblAltitudeSize.Text = "";
            }
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
        }

        private void OnLinkClick(object sender, LinkLabelLinkClickedEventArgs e)
        {
            switch (((LinkLabel)sender).Text)
            {
                case "Abstract":
                    MessageBox.Show(DataAbstract, "Abstract");
                    break;

                case "Legal constraints":
                    MessageBox.Show(LegalConstraint, "Legal constraints");
                    break;

                case "Credits":
                    MessageBox.Show(Credits, "Credits");
                    break;
            }
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnTimeHelp":
                    MessageBox.Show($"Available time range starts from {BeginPosition.ToString("MMM-dd-yyyy")} to {EndPosition.ToString("MMM-dd-yyyy")}");
                    break;

                case "btnLatHelp":
                    MessageBox.Show($"Available latitude range is from {GridExtents.yMin.ToString()} degrees to {GridExtents.yMax.ToString()} degrees");
                    break;

                case "btnLonHelp":
                    MessageBox.Show($"Available longitude range is from {GridExtents.xMin.ToString()} degrees to {GridExtents.xMax.ToString()} degrees");
                    break;
            }
        }
    }
}
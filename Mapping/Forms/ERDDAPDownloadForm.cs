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
using System.Net;
using System.IO;
using System.Xml;
using Microsoft.VisualBasic.FileIO;

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
        public string URL { get; set; }
        public string Identifier { get; set; }
        public string MetadataFileName { get; set; }
        private string _freq;
        private SpatioTemporalHelperForm _parent;

        public ERDDAPDownloadForm(SpatioTemporalHelperForm parent)
        {
            InitializeComponent();
            _parent = parent;
        }

        public static ERDDAPDownloadForm GetInstance(SpatioTemporalHelperForm parent)
        {
            if (_instance == null) _instance = new ERDDAPDownloadForm(parent);
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
                    _freq = "daily";
                    break;

                case 30:
                    dataFreq = $"{freq} days";
                    _freq = "monthly";
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

            chkAltitude.Enabled = Dimensions["vertical"].size > 0;
            chkAltitude.Checked = chkAltitude.Enabled;
            txtAltitudeStride.Enabled = chkAltitude.Enabled;
            txtEndAltitude.Enabled = chkAltitude.Enabled;
            txtStartAltitude.Enabled = chkAltitude.Enabled;
            btnAltitudeHelp.Enabled = chkAltitude.Enabled;
            if (chkAltitude.Enabled)
            {
                lblAltitudeSpacing.Text = $"{Dimensions["vertical"].spacing.ToString("N5")}";
                lblAltitudeSize.Text = $"{Dimensions["vertical"].size.ToString()}";
                txtAltitudeStride.Text = "1";
                txtStartAltitude.Text = "0";
                txtEndAltitude.Text = "0";
            }
            else
            {
                lblAltitudeSpacing.Text = "";
                lblAltitudeSize.Text = "";
                txtAltitudeStride.Text = "";
                txtStartAltitude.Text = "";
                txtEndAltitude.Text = "";
            }

            KeyValuePair<string, string> kv = new KeyValuePair<string, string>(".csv", ".csv - Download a ISO-8859-1 comma-separated text table (line 1: names; line 2: units; ISO 8601 times)");
            cboFileType.Items.Add(kv);

            kv = new KeyValuePair<string, string>(".csvp", ".cvsp - Download a ISO-8859-1 .csv file with line 1: name (units). Times are ISO 8601 strings");
            cboFileType.Items.Add(kv);

            kv = new KeyValuePair<string, string>(".csv0", ".cvs0 - Download a ISO-8859-1 .csv file without column names or units. Times are ISO 8601 strings");
            cboFileType.Items.Add(kv);

            kv = new KeyValuePair<string, string>(".nccsv", ".nccsv - Download a NetCDF-3-like 7-bit ASCII NCCSV .csv file with COARDS/CF/ACDD metadata");
            cboFileType.Items.Add(kv);

            cboFileType.ValueMember = "key";
            cboFileType.DisplayMember = "value";
            cboFileType.SelectedIndex = 3;

            dtPickerEnd.Value = EndPosition;
            dtPickerStart.Value = EndPosition;
        }

        private void updateMetadataXML(string updateValue)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(MetadataFileName);
            foreach (XmlNode nd in doc.GetElementsByTagName("gml:endPosition"))
            {
                nd.InnerXml = updateValue;
            }
            doc.Save(MetadataFileName);
            _parent.UpdateEndPosition(MetadataFileName, Identifier, updateValue);
        }

        private void RefreshMetadata()
        {
            if (global.HasInternetConnection())
            {
                WebClient wc = new WebClient();
                try
                {
                    string dasText = wc.DownloadString($"{URL}.das");
                    string updateEndPosition = GetDateCoverageEnd(dasText);
                    if (EndPosition != DateTime.Parse(updateEndPosition))
                    {
                        EndPosition = DateTime.Parse(updateEndPosition);

                        updateMetadataXML(updateEndPosition);
                        dtPickerStart.Value = EndPosition;
                        dtPickerEnd.Value = EndPosition;
                    }
                }
                catch (WebException wex)
                {
                    MessageBox.Show(wex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, "ERDDAPDownloadForm.cs", "RefreshMetadata");
                }
            }
        }

        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

        private string GetDateCoverageEnd(string dasText)
        {
            string returnDate = getBetween(dasText, "time_coverage_end ", ";");
            if (returnDate.Length > 0)
            {
                return returnDate.Trim('\"');
            }
            return "";
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

                case "Download URL":
                    if (ValidateDownload())
                    {
                        DisplayCopyableTextForm dcf = DisplayCopyableTextForm.GetInstance();
                        if (dcf.Visible)
                        {
                            dcf.BringToFront();
                        }
                        else
                        {
                            dcf.Show(this);
                        }

                        dcf.TextToDisplay = GetDownloadURL();
                    }
                    break;
            }
        }

        private bool ValidateDownload()
        {
            string msg = "";
            bool isValid = true;
            if (chkAltitude.Enabled)
            {
                isValid = txtAltitudeStride.Text.Length > 0
                    && txtStartAltitude.Text.Length > 0
                    && txtEndAltitude.Text.Length > 0;

                if (!isValid)
                {
                    msg = "Altitude parameters must be completed";
                }
            }
            if (isValid)
            {
                isValid = txtboundEast.Text.Length > 0
                    && txtboundNorth.Text.Length > 0
                    && txtboundSouth.Text.Length > 0
                    && txtboundWest.Text.Length > 0
                    && txtLatStride.Text.Length > 0
                    && txtLonStride.Text.Length > 0;

                if (!isValid)
                {
                    msg = "Longitude and altitude parameters must be completed";
                }
            }
            if (isValid)
            {
                isValid = lvGridParameters.CheckedItems.Count > 0;
                if (!isValid)
                {
                    msg = "At least one grid variable must be checked";
                }
            }
            if (isValid)
            {
                isValid = BeginPosition <= DateTime.Parse(dtPickerStart.Value.ToString("yyyy-MM-ddT12:00:00Z"));
                if (isValid)
                {
                    isValid = dtPickerEnd.Value <= DateTime.Now;
                    if (!isValid)
                    {
                        msg = "End date must not be a future date";
                    }
                    if (isValid)
                    {
                        isValid = dtPickerEnd.Value >= dtPickerStart.Value;
                        if (!isValid)
                        {
                            msg = $"End date ({dtPickerEnd.Text}) must be the same or after start date ({dtPickerStart.Text})";
                        }
                    }
                }
                else
                {
                    msg = $"Start date ({dtPickerStart.Text}) must not be earlier than beginning date of data ({BeginPosition.ToString("MMM-dd-yyyy")})";
                }
            }
            if (isValid)
            {
                isValid = dtPickerEnd.Value <= EndPosition;
                if (!isValid)
                {
                    msg = $"End date ({dtPickerEnd.Text}) must not be after end position of data ({EndPosition.ToString("MMM-dd-yyyy")})";
                }
            }
            if (!isValid)
            {
                MessageBox.Show(msg, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return isValid;
        }

        private string GetDownloadURL()
        {
            if (ValidateDownload())
            {
                string startUTC = "";
                string endUTC = "";
                string altitudeQuery = "";
                switch (_freq)
                {
                    case "daily":
                        startUTC = dtPickerStart.Value.ToString("yyyy-MM-ddT12:00:00Z");
                        endUTC = dtPickerEnd.Value.ToString("yyyy-MM-ddT12:00:00Z");
                        break;

                    case "monthly":
                        startUTC = dtPickerStart.Value.ToString("yyyy-MM-T12:00:00Z");
                        endUTC = dtPickerEnd.Value.ToString("yyyy-MM-T12:00:00Z");
                        break;
                }

                string completeURL = $"{URL.Replace("http", "https")}{((KeyValuePair<string, string>)cboFileType.SelectedItem).Key}?";
                foreach (ListViewItem item in lvGridParameters.Items)
                {
                    if (item.Checked)
                    {
                        if (chkAltitude.Enabled)
                        {
                            altitudeQuery = $"[({txtStartAltitude.Text}):{txtAltitudeStride.Text}:({txtEndAltitude.Text})]";
                        }
                        completeURL += $"{item.Text}[({startUTC}):{txtTimeStride.Text}:({endUTC})]{altitudeQuery}[({txtboundNorth.Text}):{txtLatStride.Text}:({txtboundSouth.Text})][({txtboundEast.Text}):{txtLonStride.Text}:({txtboundWest.Text})],";
                    }
                }
                return completeURL.Trim(new char[] { ',', ' ' });
            }
            else
            {
                return "";
            }
        }

        private void DownloadERDDAPData(string url)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save data file";
            sfd.FileName = $"{Identifier}_{DateTime.Now.ToString("MMM-dd-yyyy")}_nccsv.csv";
            sfd.ShowDialog();
            if (sfd.FileName.Length > 0)
            {
                FileDownloadForm fdf = new FileDownloadForm(url, sfd.FileName);
                fdf.StartPosition = FormStartPosition.CenterScreen;
                fdf.Show(this);
            }
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnRefresh":
                    RefreshMetadata();
                    break;

                case "btnTimeHelp":
                    MessageBox.Show($"Available time range starts from {BeginPosition.ToString("MMM-dd-yyyy")} to {EndPosition.ToString("MMM-dd-yyyy")}");
                    break;

                case "btnLatHelp":
                    MessageBox.Show($"Available latitude range is from {GridExtents.yMin.ToString()} degrees to {GridExtents.yMax.ToString()} degrees");
                    break;

                case "btnLonHelp":
                    MessageBox.Show($"Available longitude range is from {GridExtents.xMin.ToString()} degrees to {GridExtents.xMax.ToString()} degrees");
                    break;

                case "btnCancel":
                    Close();
                    break;

                case "btnOk":
                    string downloadURL = GetDownloadURL();
                    if (downloadURL.Length > 0)
                    {
                        DownloadERDDAPData(GetDownloadURL());
                    }
                    break;
            }
        }
    }
}
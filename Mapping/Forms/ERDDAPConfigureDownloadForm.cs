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
using FAD3.Mapping.Classes;
using FAD3.GUI.Classes;

namespace FAD3.Mapping.Forms
{
    public partial class ERDDAPConfigureDownloadForm : Form
    {
        private static ERDDAPConfigureDownloadForm _instance;
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
        private ERDDAPSelectDatasetForm _parent;
        private Extents _selectionExtent;
        private bool _isNonNumeric = false;
        private bool _validNS;
        private bool _validEW;
        private bool _validAltitiude;
        private bool _validTime;

        public ERDDAPConfigureDownloadForm(ERDDAPSelectDatasetForm parent)
        {
            InitializeComponent();
            _parent = parent;
        }

        public static ERDDAPConfigureDownloadForm GetInstance(ERDDAPSelectDatasetForm parent)
        {
            if (_instance == null) _instance = new ERDDAPConfigureDownloadForm(parent);
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

        private void SetupTooltips()
        {
            // Create the ToolTip and associate with the Form container.
            ToolTip tt = new ToolTip();

            // Set up the delays for the ToolTip.
            tt.AutoPopDelay = TooltipGlobal.AutoPopDelay;
            tt.InitialDelay = TooltipGlobal.InitialDelay;
            tt.ReshowDelay = TooltipGlobal.ReshowDelay;

            // Force the ToolTip text to be displayed whether or not the form is active.
            tt.ShowAlways = TooltipGlobal.ShowAlways;

            // Set up the ToolTip text for the Button and Checkbox.
            tt.SetToolTip(btnCancel, "Closes this window");
            tt.SetToolTip(btnDownload, "Download selected data using ERDDAP");
            tt.SetToolTip(btnRefresh, "Refresh metadata by downloading updates over the interet");
            tt.SetToolTip(btnAltitudeHelp, "Shows minimum and maximum altitude for this dataset");
            tt.SetToolTip(btnLatHelp, "Shows minimum and maximum latitude for this dataset");
            tt.SetToolTip(btnLonHelp, "Shows minimum and maximum longitude for this dataset");
            tt.SetToolTip(btnTimeHelp, "Shows time start and time end for this dataset");
            tt.SetToolTip(chkAltitude, "Check to include altitude data in downloaded data");
            tt.SetToolTip(chkLat, "Check to include latitude data in downloaded data");
            tt.SetToolTip(chkLon, "Check to include longitude data in downloaded data");
            tt.SetToolTip(chkTime, "Check to include temporal data in downloaded data");
            tt.SetToolTip(dtPickerEnd, "Ending time period of data to download");
            tt.SetToolTip(dtPickerStart, "Starting time period of data to download");
            tt.SetToolTip(txtTimeStride, "1 - downloads all time periods in the dataset\r\nOther values - skips n-1 periods to download");
            tt.SetToolTip(txtLatStride, "1 - downloads all latitude rows in the dataset\r\nOther values - skips n-1 rows to download");
            tt.SetToolTip(txtLonStride, "1 - downloads all longitude columns in the dataset\r\nOther values - skips n-1 columns to download");
            tt.SetToolTip(txtAltitudeStride, "1 - downloads all altitude levels in the dataset\r\nOther values - skips n-1 levels to download");
            tt.SetToolTip(txtboundEast, "Left longitude of the bounding box enclosing the dataset to download");
            tt.SetToolTip(txtboundWest, "Right longitude of the bounding box enclosing the dataset to download");
            tt.SetToolTip(txtboundNorth, "Upper latitude of the bounding box enclosing the dataset to download");
            tt.SetToolTip(txtboundSouth, "Lower latitude of the bounding box enclosing the dataset to download");
            tt.SetToolTip(txtStartAltitude, "If applicable, sets the lowest horizontal slice of the dataset");
            tt.SetToolTip(txtEndAltitude, "If applicable, sets the topmost horizontal slice of the dataset");
            tt.SetToolTip(lvGridParameters, "Lists all downloadble grid variables of the dataset");
            tt.SetToolTip(cboFileType, "Select filetype of downloaded data");
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            SetupTooltips();

            lblTitle.Text = Title;
            lblTitle.BorderStyle = BorderStyle.None;
            txtboundEast.Text = DataExtents.xMin.ToString();
            txtboundWest.Text = DataExtents.xMax.ToString();
            txtboundNorth.Text = DataExtents.yMax.ToString();
            txtboundSouth.Text = DataExtents.yMin.ToString();

            ValidateExtents();

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
            MakeGridFromPoints.OnExtentDefined += OnExtentDefined;
            MakeGridFromPoints.MapLayersHandler = global.MappingForm.MapLayersHandler;
            Text = "Setup ERDDAP>griddap data access";
        }

        /// <summary>
        /// responds to a the selection box dragged in the map and updates the corresponding fields in the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExtentDefined(object sender, ExtentDraggedBoxEventArgs e)
        {
            txtboundNorth.Text = e.Top.ToString();
            txtboundSouth.Text = e.Bottom.ToString();
            txtboundEast.Text = e.Left.ToString();
            txtboundWest.Text = e.Right.ToString();
            _selectionExtent = new Extents();
            _selectionExtent.SetBounds(e.Left, e.Bottom, 0, e.Right, e.Top, 0);
            if (!e.InDrag)
            {
                MakeGridFromPoints.MakeExtentShapeFile();
            }
            ValidateExtents();
        }

        /// <summary>
        /// edits metadata of current dataset and updates all values of gml:endPosition node
        /// gml:endPosition is the value of the latest date of the dataset
        /// </summary>
        /// <param name="updateValue"></param>
        private void updateMetadataXML(string identifier, string updateValue)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(MetadataFileName);
            foreach (XmlNode nd in doc.GetElementsByTagName("gml:endPosition"))
            {
                nd.InnerXml = updateValue;
            }
            doc.Save(MetadataFileName);
            _parent.UpdateEndPosition(identifier, updateValue);
        }

        /// <summary>
        /// download the .das file of the current dataset and locates value of "time_coverage_end" property
        /// updates corresponding temporal end position of xml metadata of current dataset
        /// </summary>
        private void RefreshMetadata()
        {
            string msg = "";
            string titleString = "";
            if (global.HasInternetConnection())
            {
                WebClient wc = new WebClient();
                try
                {
                    string dasText = wc.DownloadString($"{URL}.das");
                    string updateEndPosition = GetDateCoverageEnd(dasText);
                    bool updateStartPosition = dtPickerStart.Value == EndPosition;
                    if (EndPosition.Date != DateTime.Parse(updateEndPosition).Date)
                    {
                        EndPosition = DateTime.Parse(updateEndPosition);

                        if (updateStartPosition)
                        {
                            dtPickerStart.Value = EndPosition;
                        }
                        dtPickerEnd.Value = EndPosition;
                        updateMetadataXML(Identifier, updateEndPosition);
                        msg = "Metadata was successfuly updated";
                        titleString = "Update successful";
                    }
                    else
                    {
                        msg = "Metadata does not have any update";
                        titleString = "No update for metadata";
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
            else
            {
                msg = "Computer is not connected to the internet";
                titleString = "No connection";
            }
            MessageBox.Show(msg, titleString, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        /// <summary>
        /// searches downloaded .das file for occurence of time_coverage_end and returns corresponding value
        /// </summary>
        /// <param name="dasText"></param>
        /// <returns></returns>
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

        /// <summary>
        /// handles clicks on link labels
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLinkClick(object sender, LinkLabelLinkClickedEventArgs e)
        {
            switch (((LinkLabel)sender).Text)
            {
                case "Abstract":
                    global.ShowCopyableText("Abstract", DataAbstract, this);
                    break;

                case "Legal constraints":
                    global.ShowCopyableText("Legal constraints", LegalConstraint, this);
                    break;

                case "Credits":
                    global.ShowCopyableText("Credits", Credits, this);
                    break;

                case "Download URL":
                    if (ValidateForm())
                    {
                        global.ShowCopyableText("URL to download dataset using ERDDAP server", GetDownloadURL(), this);
                    }
                    else
                    {
                        MessageBox.Show("Check for blank fields and errors marked by exclamation points\r\nAt least one grid variable must be checked\r\nStart values must not exceed end values", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;
            }
        }

        private bool ValidateTime()
        {
            VisibleErrorLabels(false, includeExtents: false, includeVertical: false, includeStrides: false);
            bool isStartDateValid = DateTime.Compare(dtPickerStart.Value, BeginPosition) >= 0
                && DateTime.Compare(dtPickerStart.Value, EndPosition) <= 0
                && DateTime.Compare(dtPickerStart.Value, dtPickerEnd.Value) <= 0;
            lblErrStartTime.Visible = !isStartDateValid;

            bool isEndDateValid = DateTime.Compare(dtPickerEnd.Value, BeginPosition) >= 0
                && DateTime.Compare(dtPickerEnd.Value, EndPosition) <= 0
                && DateTime.Compare(dtPickerStart.Value, dtPickerEnd.Value) <= 0;
            lblErrEndTime.Visible = !isEndDateValid;

            return isStartDateValid && isEndDateValid;
        }

        private bool ValidateForm()
        {
            bool isValid = ValidateExtents();

            if (isValid)
            {
                isValid = ValidateTime();
                if (isValid)
                {
                    isValid = ValidateStrides();
                    if (isValid)
                    {
                        isValid = ValidateAltitudes();
                        if (isValid)
                        {
                            isValid = txtboundEast.Text.Length > 0
                                && txtboundNorth.Text.Length > 0
                                && txtboundSouth.Text.Length > 0
                                && txtboundWest.Text.Length > 0
                                && txtLatStride.Text.Length > 0
                                && txtLonStride.Text.Length > 0
                                && txtTimeStride.Text.Length > 0;

                            if (isValid && chkAltitude.Enabled)
                            {
                                isValid = txtStartAltitude.Text.Length > 0
                                    && txtEndAltitude.Text.Length > 0
                                    && txtAltitudeStride.Text.Length > 0;
                                if (isValid)
                                {
                                    isValid = lvGridParameters.CheckedItems.Count > 0;
                                    if (isValid)
                                    {
                                        _validEW = (double.Parse(txtboundEast.Text) < double.Parse(txtboundWest.Text));
                                        _validNS = (double.Parse(txtboundSouth.Text) < double.Parse(txtboundNorth.Text));
                                        _validTime = DateTime.Compare(dtPickerStart.Value, dtPickerEnd.Value) <= 0;
                                        if (chkAltitude.Enabled)
                                        {
                                            _validAltitiude = (double.Parse(txtStartAltitude.Text) <= double.Parse(txtEndAltitude.Text));
                                        }
                                        else
                                        {
                                            _validAltitiude = true;
                                        }
                                        isValid = _validAltitiude && _validEW && _validNS && _validTime;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return isValid;
        }

        /// <summary>
        /// returns the URL for downloading gridded data
        /// </summary>
        /// <returns></returns>
        private string GetDownloadURL()
        {
            if (ValidateForm())
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
                MessageBox.Show("Check for blank fields and errors marked by exclamation points\r\nAt least one grid variable must be checked\r\nStart values must not exceed end values", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return "";
            }
        }

        /// <summary>
        /// opens the form that downloads data using ERDDAP
        /// </summary>
        /// <param name="url"></param>
        private void DownloadERDDAPData(string url)
        {
            if (global.HasInternetConnection())
            {
                SaveFileDialog sfd = new SaveFileDialog();

                sfd.Title = "Save data file";
                sfd.FileName = $"{Identifier}_{dtPickerStart.Value.ToString("MMM-dd-yyyy")}_{dtPickerEnd.Value.ToString("MMM-dd-yyyy")}_nccsv.csv";
                DialogResult result = sfd.ShowDialog();
                if (result == DialogResult.OK && sfd.FileName.Length > 0)
                {
                    FileDownloadForm fdf = new FileDownloadForm(url, sfd.FileName);
                    fdf.StartPosition = FormStartPosition.CenterScreen;
                    fdf.Show(this);
                }
            }
            else
            {
                MessageBox.Show("Computer is not connected to the internet", "No connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                case "btnDownload":
                    string downloadURL = GetDownloadURL();
                    if (downloadURL.Length > 0)
                    {
                        DownloadERDDAPData(GetDownloadURL());
                    }
                    break;
            }
        }

        private void OnFieldValidate(object sender, CancelEventArgs e)
        {
            string ctlName = ((Control)sender).Name;
            string s = "";
            string msg = "";
            switch (((Control)sender).GetType().Name)
            {
                case "TextBox":
                case "DateTimePicker":
                    s = ((Control)sender).Text;
                    break;
            }

            if (s.Length > 0)
            {
                switch (ctlName)
                {
                    case "dtPickerStart":
                    case "dtPickerEnd":
                        ValidateTime();
                        break;

                    case "txtTimeStride":
                    case "txtAltitudeStride":
                    case "txtLatStride":
                    case "txtLonStride":
                        ValidateStrides();
                        break;

                    case "txtStartAltitude":
                    case "txtEndAltitude":
                        ValidateAltitudes();
                        break;

                    case "txtboundNorth":
                    case "txtboundSouth":
                    case "txtboundEast":
                    case "txtboundWest":

                        ValidateExtents();

                        break;
                }
            }
            e.Cancel = _isNonNumeric;
        }

        private bool ValidateAltitudes()
        {
            _isNonNumeric = false;
            bool isValid = true;
            VisibleErrorLabels(false, includeTemporal: false, includeExtents: false, includeStrides: false);
            foreach (Control ctl in Controls)
            {
                switch (ctl.Name)
                {
                    case "txtStartAltitude":
                    case "txtEndAltitude":
                        string s = ctl.Text;
                        if (chkAltitude.Enabled && s.Length > 0)
                        {
                            if (int.TryParse(s, out int v))
                            {
                                switch (ctl.Name)
                                {
                                    case "txtStartAltitude":
                                        isValid = v <= Dimensions["vertical"].size && v >= 0;
                                        lblErrStartAltitude.Visible = !isValid;
                                        break;

                                    case "txtEndAltitude":

                                        isValid = v <= Dimensions["vertical"].size && v >= 0;
                                        lblErrEndAltitude.Visible = !isValid;
                                        break;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Expected values are whole numbers starting from zero", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                isValid = false;
                                _isNonNumeric = true;
                                switch (ctl.Name)
                                {
                                    case "txtStartAltitude":
                                        lblErrStartAltitude.Visible = true;
                                        break;

                                    case "txtEndAltitude":
                                        lblErrEndAltitude.Visible = true;
                                        break;
                                }
                            }
                        }
                        break;
                }
            }
            return isValid;
        }

        private bool ValidateStrides()

        {
            _isNonNumeric = false;
            VisibleErrorLabels(false, includeTemporal: false, includeExtents: false, includeVertical: false);
            bool isValid = true;
            bool returnValue = true;
            string s = "";
            foreach (Control ctl in Controls)
            {
                switch (ctl.Name)
                {
                    case "txtTimeStride":
                    case "txtLatStride":
                    case "txtLonStride":
                    case "txtAltitudeStride":
                        s = ctl.Text;
                        if (s.Length > 0)
                        {
                            if (int.TryParse(s, out int v))
                            {
                                switch (ctl.Name)
                                {
                                    case "txtTimeStride":
                                        isValid = v <= Dimensions["temporal"].size && v > 0;
                                        lblErrStrideTime.Visible = !isValid;
                                        break;

                                    case "txtLatStride":

                                        isValid = v <= Dimensions["row"].size && v > 0;
                                        lblErrStrideLatitude.Visible = !isValid;
                                        break;

                                    case "txtLonStride":

                                        isValid = v <= Dimensions["column"].size && v > 0;
                                        lblErrStrideLongitude.Visible = !isValid;
                                        break;

                                    case "txtAltitudeStride":
                                        isValid = v <= Dimensions["vertical"].size && v > 0;
                                        lblErrStrideAltitude.Visible = !isValid;
                                        break;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Expected values are whole numbers greater than zero", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                isValid = false;
                                _isNonNumeric = true;
                                switch (ctl.Name)
                                {
                                    case "txtTimeStride":
                                        lblErrStrideTime.Visible = true;
                                        break;

                                    case "txtLatStride":
                                        lblErrStrideLatitude.Visible = true;
                                        break;

                                    case "txtLonStride":
                                        lblErrStrideLongitude.Visible = true;
                                        break;

                                    case "txtAltitudeStride":
                                        lblErrStrideAltitude.Visible = true;
                                        break;
                                }
                            }
                        }
                        break;
                }
                if (returnValue)
                {
                    returnValue = isValid;
                }
            }

            return returnValue;
        }

        private void VisibleErrorLabels(bool labelsVisible = true, bool includeExtents = true, bool includeTemporal = true, bool includeVertical = true, bool includeStrides = true)
        {
            if (includeExtents)
            {
                lblErrStartLatitude.Visible = labelsVisible;
                lblErrEndLatitude.Visible = labelsVisible;
                lblErrStartLongitude.Visible = labelsVisible;
                lblErrEndLongitude.Visible = labelsVisible;
            }
            if (includeTemporal)
            {
                lblErrStartTime.Visible = labelsVisible;
                lblErrEndTime.Visible = labelsVisible;
            }
            if (includeVertical)
            {
                lblErrStartAltitude.Visible = labelsVisible;
                lblErrEndAltitude.Visible = labelsVisible;
            }
            if (includeStrides)
            {
                lblErrStrideLatitude.Visible = labelsVisible;
                lblErrStrideLongitude.Visible = labelsVisible;
                lblErrStrideAltitude.Visible = labelsVisible;
                lblErrStrideTime.Visible = labelsVisible;
            }
        }

        private bool ValidateExtents(bool hideErrorLables = false)
        {
            _isNonNumeric = false;
            VisibleErrorLabels(false, includeTemporal: false, includeVertical: false, includeStrides: false);
            bool isValid = true;
            string msg = "";
            string s = "";

            foreach (Control c in Controls)
            {
                if (c.Tag?.ToString() == "extent")
                {
                    s = c.Text;
                    if (s.Length > 0)
                    {
                        if (double.TryParse(s, out double dbl))
                        {
                            switch (c.Name)
                            {
                                case "txtboundNorth":
                                    if (dbl < GridExtents.yMin || dbl > GridExtents.yMax)
                                    {
                                        isValid = false;
                                        lblErrStartLatitude.Visible = !isValid;
                                    }
                                    break;

                                case "txtboundSouth":
                                    if (dbl < GridExtents.yMin || dbl > GridExtents.yMax)
                                    {
                                        isValid = false;
                                        lblErrEndLatitude.Visible = !isValid;
                                    }
                                    break;

                                case "txtboundEast":
                                    if (dbl < GridExtents.xMin || dbl > GridExtents.xMax)
                                    {
                                        isValid = false;
                                        lblErrStartLongitude.Visible = !isValid;
                                    }
                                    break;

                                case "txtboundWest":
                                    if (dbl < GridExtents.xMin || dbl > GridExtents.xMax)
                                    {
                                        isValid = false;
                                        lblErrEndLongitude.Visible = !isValid;
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            msg += $"{c.Name.Replace("txtbound", string.Empty) + "ern"} extent value must be a number";
                            isValid = false;
                            _isNonNumeric = true;
                            switch (c.Name)
                            {
                                case "txtboundNorth":
                                    lblErrStartLatitude.Visible = true;
                                    break;

                                case "txtboundSouth":
                                    lblErrEndLatitude.Visible = true;
                                    break;

                                case "txtboundEast":
                                    lblErrStartLongitude.Visible = true;
                                    break;

                                case "txtboundWest":
                                    lblErrEndLongitude.Visible = true;
                                    break;
                            }
                            MessageBox.Show(msg, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }

            return isValid;
        }
    }
}
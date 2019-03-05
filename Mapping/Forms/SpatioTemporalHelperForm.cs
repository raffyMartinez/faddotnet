using FAD3.Database.Classes;
using FAD3.Mapping.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading.Tasks;
using Microsoft.Win32;
using MapWinGIS;

namespace FAD3.Mapping.Forms
{
    public partial class SpatioTemporalHelperForm : Form
    {
        private string _dataSourceFileName;
        private static SpatioTemporalHelperForm _instance;
        private List<double> _dataValues = new List<double>();
        private DataTable _dt;
        private int _firstColIndex;
        private int _lastColIndex;
        private int _latitudeColIndex;
        private int _longitudeColIndex;
        private int _columnCount;
        private bool _hasMesh;
        private List<double?> _columnValues = new List<double?>();
        private int _dataPoints;
        private int _selectionIndex = 0;
        private fadUTMZone _utmZone;
        private bool _createFileWithoutInland;
        private bool _hasReadCoordinates;
        private string _ERDDAPMetadataFolder;
        private Extents _selectionExtent;

        private Dictionary<string, (Dictionary<string, (string unit, string description)> parameters, string title, Extents gridExtents, string url, string credits, string legalConstraint, string dataAbstract, Dictionary<string, (string name, int size, double spacing)> dimensions, DateTime beginPosition, DateTime endPosition)> _dictERDDAP =
            new Dictionary<string, (Dictionary<string, (string unit, string description)> parameters, string title, Extents gridExtents, string url, string credits, string legalConstraint, string dataAbstract, Dictionary<string, (string name, int size, double spacing)> dimensions, DateTime beginPosition, DateTime endPosition)>();

        public static SpatioTemporalHelperForm GetInstance()
        {
            if (_instance == null) _instance = new SpatioTemporalHelperForm();
            return _instance;
        }

        public SpatioTemporalHelperForm()
        {
            InitializeComponent();
        }

        private void OnGridSummaryCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                bool isVisible = (bool)dgSheetSummary[0, e.RowIndex].Value;
                dgSheetSummary[0, e.RowIndex].Value = !isVisible;
                MakeGridFromPoints.GridShapefile.Categories.Item[e.RowIndex].DrawingOptions.FillVisible = !isVisible;
                MakeGridFromPoints.GridShapefile.Categories.Item[e.RowIndex].DrawingOptions.LineVisible = !isVisible;
                global.MappingForm.MapControl.Redraw();
            }
        }

        /// <summary>
        /// opens a data file and fills up comboboxes with column headers
        /// </summary>
        private void OpenFile()
        {
            var fileOpen = new OpenFileDialog
            {
                Title = "Open MS Excel file",
                Filter = "CSV (comma separated values) file|*.csv|Excel file|*.xls;*.xlsx|All file types|*.*",
                FilterIndex = 1
            };
            fileOpen.ShowDialog();
            if (fileOpen.FileName.Length > 0 && File.Exists(fileOpen.FileName))
            {
                _dataSourceFileName = fileOpen.FileName;
                txtFile.Text = _dataSourceFileName;
                switch (Path.GetExtension(_dataSourceFileName))
                {
                    case ".xlsx":
                        MessageBox.Show("Excel file not yet supported");
                        break;

                    case ".csv":
                        MakeGridFromPoints.SingleDimensionCSV = _dataSourceFileName;
                        List<string> csvFields = MakeGridFromPoints.GetFields();

                        cboLatitude.Items.Clear();
                        cboLongitude.Items.Clear();
                        cboTemporal.Items.Clear();
                        cboValue.Items.Clear();

                        foreach (string item in csvFields)
                        {
                            cboLatitude.Items.Add(item);
                            cboLongitude.Items.Add(item);
                            cboTemporal.Items.Add(item);
                            cboValue.Items.Add(item);
                        }

                        cboLatitude.Enabled = true;
                        cboLongitude.Enabled = true;
                        cboValue.Enabled = true;
                        cboTemporal.Enabled = true;
                        break;
                }
            }
        }

        /// <summary>
        /// calls method that reads csv data as a separate process. When reading of CSV is done, it will update the fields
        /// </summary>
        private async void ReadCSVFile()
        {
            bool result = await ReadCVSFileTask();

            //this will wait until ReadCVSFileTask() is finished then
            //the fields and comboBoxes is filled up
            _dataPoints = MakeGridFromPoints.Coordinates.Count;
            txtRows.Text = _dataPoints.ToString();

            cboFirstData.Items.Clear();
            cboLastData.Items.Clear();
            foreach (var item in MakeGridFromPoints.DictTemporalValues.Keys)
            {
                cboFirstData.Items.Add(item);
                cboLastData.Items.Add(item);
            }
            cboFirstData.Enabled = cboFirstData.Items.Count > 0;
            cboLastData.Enabled = cboLastData.Items.Count > 0;

            txtInlandPoints.Text = MakeGridFromPoints.InlandPointCount.ToString();
            btnReadFile.Enabled = true;
        }

        /// <summary>
        /// reads CSV data as a separate process
        /// </summary>
        /// <returns></returns>
        private Task<bool> ReadCVSFileTask()
        {
            return Task.Run(() => MakeGridFromPoints.ParseSingleDimensionCSV());
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnDownload":
                    if (txtMinLat.Text.Length > 0
                        && txtMaxLat.Text.Length > 0
                        && txtMinLon.Text.Length > 0
                        && txtMaxLon.Text.Length > 0
                        && lvERDDAP.SelectedItems.Count > 0)
                    {
                        ERDDAPDownloadForm edf = ERDDAPDownloadForm.GetInstance();
                        if (edf.Visible)
                        {
                            edf.BringToFront();
                        }
                        else
                        {
                            string identifier = lvERDDAP.SelectedItems[0].Name;
                            edf.BeginPosition = _dictERDDAP[identifier].beginPosition;
                            edf.EndPosition = _dictERDDAP[identifier].endPosition;
                            edf.Dimensions = _dictERDDAP[identifier].dimensions;
                            edf.DataExtents = _selectionExtent;
                            edf.GridParameters = _dictERDDAP[identifier].parameters;
                            edf.Credits = _dictERDDAP[identifier].credits;
                            edf.Title = _dictERDDAP[identifier].title;
                            edf.DataAbstract = _dictERDDAP[identifier].dataAbstract;
                            edf.LegalConstraint = _dictERDDAP[identifier].legalConstraint;
                            edf.GridExtents = _dictERDDAP[identifier].gridExtents;
                            edf.Show(this);
                        }
                    }
                    break;

                case "btnGetMetadataFolder":
                    var folderBrowser = new FolderBrowserDialog();
                    folderBrowser.ShowNewFolderButton = true;
                    if (_ERDDAPMetadataFolder == "")
                    {
                        folderBrowser.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    }
                    else
                    {
                        folderBrowser.SelectedPath = _ERDDAPMetadataFolder;
                    }
                    folderBrowser.Description = "Locate folder containing ERDDAP metadata";
                    DialogResult result = FolderBrowserLauncher.ShowFolderBrowser(folderBrowser);
                    if (result == DialogResult.OK)
                    {
                        ERDDAPMetadataHandler.MetadataDirectory = folderBrowser.SelectedPath;
                        ERDDAPMetadataHandler.ReadISO9115Metadata();
                        ERDDAPMetadataHandler.SaveMetadataDirectorySetting(folderBrowser.SelectedPath);
                    }
                    break;

                case "btnCreateExtent":

                    MakeGridFromPoints.MapLayers = global.MappingForm.MapLayersHandler;
                    MakeGridFromPoints.MakeExtentShapeFile();

                    break;

                case "btnOpen":
                    MakeGridFromPoints.Reset();
                    txtRows.Text = "";
                    OpenFile();
                    break;

                case "btnReadFile":

                    //reads the file that was opened

                    btnReadFile.Enabled = false;
                    MakeGridFromPoints.LatitudeColumn = cboLatitude.SelectedIndex;
                    MakeGridFromPoints.LongitudeColumn = cboLongitude.SelectedIndex;
                    MakeGridFromPoints.TemporalColumn = cboTemporal.SelectedIndex;
                    MakeGridFromPoints.ValuesColumn = cboValue.SelectedIndex;

                    //ask UTMzone of area of interest
                    using (SelectUTMZoneForm szf = new SelectUTMZoneForm())
                    {
                        szf.ShowDialog();
                        if (szf.DialogResult == DialogResult.OK)
                        {
                            _utmZone = szf.UTMZone;
                            _createFileWithoutInland = szf.CreateFileWithoutInland;
                            MakeGridFromPoints.IgnoreInlandPoints = false;
                            MakeGridFromPoints.UTMZone = _utmZone;
                            MakeGridFromPoints.CreateFileWithoutInlandPoints = _createFileWithoutInland;
                        }
                        else if (szf.DialogResult == DialogResult.Cancel)
                        {
                            MakeGridFromPoints.IgnoreInlandPoints = true;
                        }
                    }

                    lblParameter.Text = cboValue.Text;

                    //reads csv data in a separate thread
                    ReadCSVFile();

                    break;

                case "btnCategorize":
                    btnCategorize.Enabled = false;

                    if (txtCategoryCount.Text.Length > 0 && cboLatitude.SelectedIndex >= 0
                        && cboLongitude.SelectedIndex >= 0 && cboFirstData.SelectedIndex >= 0
                        && cboLastData.SelectedIndex >= 0)
                    {
                        lblStatus.Text = "Start categorizing data";
                        listTimePeriodsForMapping();
                        listCoordinates();
                        getDataValues();
                        switch (cboClassificationScheme.Text)
                        {
                            case "Jenk's-Fisher's":
                                btnShowGridPoints.Enabled = DoJenksFisher();
                                break;

                            case "Equal interval":
                            case "User defined":
                                using (ShapefileClassificationSchemeForm scsf = new ShapefileClassificationSchemeForm())
                                {
                                    scsf.ClassificationScheme = cboClassificationScheme.Text;
                                    scsf.MinimumValue = _dataValues.Min();
                                    scsf.MaximumValue = _dataValues.Max();
                                    scsf.NumberOfClasses = int.Parse(txtCategoryCount.Text);
                                    scsf.ShowDialog();
                                    if (scsf.DialogResult == DialogResult.OK)
                                    {
                                    }
                                    else if (scsf.DialogResult == DialogResult.Cancel)
                                    {
                                    }
                                }
                                break;
                        }
                        lblStatus.Text = "Finished categorizing data";
                    }
                    else
                    {
                        MessageBox.Show("Specify longitude, latitude, and, first and last data columns",
                                        "Required data is missing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    btnCategorize.Enabled = true;
                    break;

                case "btnShowGridPoints":

                    //shows the data points of the data

                    MapGridPoints();
                    break;

                case "btnShowGridPolygons":

                    //creates a grid with the datapoints at the center of the grid

                    if (MakeGridFromPoints.MakeGridShapefile())
                    {
                        _hasMesh = global.MappingForm.MapLayersHandler.AddLayer(MakeGridFromPoints.GridShapefile, "Mesh", uniqueLayer: true) > 0;
                    }
                    listSelectedTimePeriods.Enabled = _hasMesh;
                    break;

                case "btnUp":
                    MapSheet(false);
                    break;

                case "btnDown":
                    MapSheet(true);
                    break;
            }
        }

        private void MapGridPoints()
        {
            MakeGridFromPoints.MapInteractionHandler = global.MappingForm.MapInterActionHandler;
            MakeGridFromPoints.GeoProjection = global.MappingForm.MapControl.GeoProjection;

            if (MakeGridFromPoints.MakePointShapefile(!MakeGridFromPoints.IgnoreInlandPoints))
            {
                global.MappingForm.MapLayersHandler.AddLayer(MakeGridFromPoints.PointShapefile, "Grid points");
            }
        }

        /// <summary>
        /// returns number of values within a class size
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="greaterThan"></param>
        /// <returns></returns>
        private int GetClassSize(double value1, double value2 = 0, bool greaterThan = false)
        {
            int count = 0;
            if (!greaterThan)
            {
                foreach (var item in _dataValues)
                {
                    if (item >= value1 && item < value2)
                    {
                        count++;
                    }
                    else if (item == value2)
                    {
                        break;
                    }
                }
            }
            else
            {
                foreach (var item in _dataValues)
                {
                    if (item >= value1)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// do a Jenk's-Fisher categorization of the values. The category breaks are found in List<Double> listBreaks
        /// </summary>
        /// <returns></returns>
        private bool DoJenksFisher()
        {
            if (txtCategoryCount.Text.Length > 0)
            {
                dgCategories.Rows.Clear();
                _dataValues.Sort();
                var listBreaks = JenksFisher.CreateJenksFisherBreaksArray(_dataValues, int.Parse(txtCategoryCount.Text));
                var n = 0;
                var lower = listBreaks.Min();
                var upper = 0D;
                int row;

                Color color;

                MakeGridFromPoints.Categories.Clear();
                ColorBlend blend = (ColorBlend)icbColorScheme.ColorSchemes.List[icbColorScheme.SelectedIndex];
                MakeGridFromPoints.NumberOfCategories = int.Parse(txtCategoryCount.Text);
                MakeGridFromPoints.ColorBlend = blend;

                //make categories from the breaks defined in Jenk's-Fisher's
                //add the category range and color to a datagridview
                foreach (var item in listBreaks)
                {
                    if (n > 0)
                    {
                        upper = item;
                        color = MakeGridFromPoints.AddCategory(lower, upper);
                        row = dgCategories.Rows.Add(new object[] { $"{lower.ToString("N5")} - {upper.ToString("N5")}", GetClassSize(lower, upper).ToString(), "" });
                        dgCategories[2, row].Style.BackColor = color;
                        lower = item;
                    }
                    n++;
                }
                //add the last category to the datagridview
                color = MakeGridFromPoints.AddCategory(upper, _dataValues.Max() + 1);
                row = dgCategories.Rows.Add(new object[] { $"> {listBreaks.Max().ToString("N5")}", GetClassSize(listBreaks.Max(), 0, true).ToString(), "" });
                dgCategories[2, row].Style.BackColor = color;

                //add an empty null category
                MakeGridFromPoints.AddNullCategory();

                //summarize the data
                txtValuesCount.Text = _dataValues.Count.ToString();
                txtMinimum.Text = _dataValues.Min().ToString();
                txtMaximum.Text = _dataValues.Max().ToString();

                SizeColumns(dgCategories, false, true);
                return listBreaks.Count > 0;
            }
            return false;
        }

        /// <summary>
        /// adds all non-null values to a List<double>
        /// this List<> will be used for Jenk's Fisher clustering
        /// </summary>
        private void getDataValues()
        {
            switch (Path.GetExtension(_dataSourceFileName))
            {
                case ".xlsx":
                    _dataValues.Clear();

                    for (int row = 0; row < _dt.Rows.Count; row++)
                    {
                        var arr = _dt.Rows[row].ItemArray;
                        for (int col = _firstColIndex + 2; col <= _lastColIndex + 2; col++)
                        {
                            if (double.TryParse(arr[col].ToString(), out double d))
                            {
                                _dataValues.Add(d);
                            }
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        private void OnSelectedSheetsClick(object sender, EventArgs e)
        {
            MapSheet(listSelectedTimePeriods.SelectedIndices[0]);
        }

        private void listCoordinates()
        {
            switch (Path.GetExtension(_dataSourceFileName))
            {
                case ".csv":
                    break;

                case ".xlsx":
                    for (int row = 0; row < _dt.Rows.Count; row++)
                    {
                        var arr = _dt.Rows[row].ItemArray;
                        var lat = 0d;
                        var lon = 0d;

                        IConvertible convert = arr[_latitudeColIndex] as IConvertible;
                        if (convert != null)
                        {
                            lat = convert.ToDouble(null);
                        }
                        else
                        {
                            lat = 0d;
                        }

                        convert = arr[_longitudeColIndex] as IConvertible;
                        if (convert != null)
                        {
                            lon = convert.ToDouble(null);
                        }
                        else
                        {
                            lon = 0d;
                        }
                        MakeGridFromPoints.AddCoordinate(row, lat, lon, false, row == 0);
                    }
                    break;
            }
        }

        /// <summary>
        /// lists the headers of the data columns in a listbox
        /// </summary>
        private void listTimePeriodsForMapping()
        {
            listSelectedTimePeriods.Items.Clear();
            var includeColumn = false;
            if (cboLongitude.Text.Length > 0 && cboLatitude.Text.Length > 0 && cboFirstData.Text.Length > 0 && cboLastData.Text.Length > 0)
            {
                switch (Path.GetExtension(_dataSourceFileName))
                {
                    case ".csv":
                        int i = 0;
                        foreach (string item in MakeGridFromPoints.DictTemporalValues.Keys)
                        {
                            if (!includeColumn && i == _firstColIndex)
                            {
                                includeColumn = true;
                            }
                            if (includeColumn)
                            {
                                listSelectedTimePeriods.Items.Add(item);
                                foreach (double? value in MakeGridFromPoints.DictTemporalValues[item].Values)
                                {
                                    if (value != null)
                                    {
                                        _dataValues.Add((double)value);
                                    }
                                }
                            }
                            if (includeColumn && i == _lastColIndex)
                            {
                                includeColumn = false;
                            }
                            i++;
                        }
                        break;

                    case ".xlsx":

                        for (int n = 0; n < _columnCount; n++)
                        {
                            if (!includeColumn && (n - 2) == _firstColIndex)
                            {
                                includeColumn = true;
                            }
                            if (includeColumn)
                            {
                                listSelectedTimePeriods.Items.Add(_dt.Columns[n].ColumnName);
                            }
                            if (includeColumn && (n - 2) == _lastColIndex)
                            {
                                includeColumn = false;
                            }
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// maps the data corresponding to the selected item in the listbox
        /// </summary>
        /// <param name="index"> the selected item in the listbox</param>
        private void MapSheet(int index)
        {
            if (_hasMesh)
            {
                lblMappedSheet.Text = $"{listSelectedTimePeriods.Items[index]}";
                lblMappedSheet.Visible = true;
                _columnValues.Clear();       // _columnValues will contain values corresponding to the item we are interested in
                double? v = null;
                switch (Path.GetExtension(_dataSourceFileName))
                {
                    case ".csv":
                        foreach (double? value in MakeGridFromPoints.DictTemporalValues[lblMappedSheet.Text].Values)
                        {
                            _columnValues.Add(value);
                        }
                        break;

                    case ".xlsx":
                        //read all rows but only select the data point in the row corresponding to the selected listbox item
                        for (int row = 0; row < _dt.Rows.Count; row++)
                        {
                            v = null;
                            var arr = _dt.Rows[row].ItemArray;
                            var col = index + _firstColIndex + 2;

                            if (arr[col].GetType().Name == "String")
                            {
                                if (double.TryParse((string)arr[col], out double d))
                                {
                                    v = d;
                                }
                            }
                            else
                            {
                                v = arr[col] as double?;
                            }
                            _columnValues.Add(v);
                        }
                        break;
                }

                MakeGridFromPoints.MapColumn(_columnValues, lblMappedSheet.Text);

                global.MappingForm.MapControl.Redraw();

                graphSheet.Series.Clear();
                graphSheet.ChartAreas[0].AxisY.Minimum = 0;
                graphSheet.ChartAreas[0].AxisY.Maximum = 100;
                graphSheet.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                graphSheet.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                graphSheet.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
                var series = new Series
                {
                    ChartType = SeriesChartType.Column,
                    Name = "summary",
                };
                graphSheet.Series.Add(series);

                UpdateSheetSummary(MakeGridFromPoints.SheetMapSummary);
            }
            else
            {
                MessageBox.Show("Mesh layer not found", "Layer not found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdateSheetSummary(Dictionary<string, int> summary)
        {
            double percent = 0D;
            var row = 0;
            dgSheetSummary.Rows.Clear();
            foreach (KeyValuePair<string, int> kv in summary)
            {
                var firstColText = "";
                if (kv.Key == "Null")
                {
                    firstColText = "Null";
                    percent = ((double)(kv.Value - MakeGridFromPoints.InlandPointCount) / (double)(_dataPoints - MakeGridFromPoints.InlandPointCount)) * 100;
                    row = dgSheetSummary.Rows.Add(new object[] { true, firstColText, (kv.Value - MakeGridFromPoints.InlandPointCount).ToString(), percent.ToString("N1") });
                }
                else
                {
                    percent = ((double)kv.Value / (double)(_dataPoints - MakeGridFromPoints.InlandPointCount)) * 100;
                    row = dgSheetSummary.Rows.Add(new object[] { true, firstColText, kv.Value.ToString(), percent.ToString("N1") });
                }

                var pt = graphSheet.Series["summary"].Points.AddY(percent);
                dgSheetSummary[1, row].Style.BackColor = MakeGridFromPoints.CategoryColor(row);
                graphSheet.Series["summary"].Points[pt].Color = dgSheetSummary[1, row].Style.BackColor;
                graphSheet.Series["summary"].Points[pt].BorderColor = Color.Black;
                dgSheetSummary[0, row].Value = MakeGridFromPoints.GridShapefile.Categories.Item[row].DrawingOptions.FillVisible;
                if (MakeGridFromPoints.GridShapefile.Categories.Item[row].Name == "nullCategory")
                {
                    dgSheetSummary[0, row].Value = !MakeGridFromPoints.GridShapefile.Categories.Item[row].DrawingOptions.FillVisible;
                }
            }
            dgSheetSummary.ClearSelection();
        }

        private void MapSheet(bool forward, bool fromArrowKeys = false)
        {
            if (listSelectedTimePeriods.Items.Count > 0)
            {
                if (listSelectedTimePeriods.SelectedItems.Count == 1)
                {
                    var selection = listSelectedTimePeriods.SelectedIndex;
                    listSelectedTimePeriods.SelectedItems.Clear();
                    if (forward)
                    {
                        selection++;
                    }
                    else
                    {
                        selection--;
                    }
                    if (selection == -1)
                    {
                        listSelectedTimePeriods.SelectedIndex = listSelectedTimePeriods.Items.Count - 1;
                    }
                    else if (listSelectedTimePeriods.Items.Count > selection)
                    {
                        listSelectedTimePeriods.SelectedIndex = selection;
                    }
                    else
                    {
                        listSelectedTimePeriods.SelectedIndex = 0;
                    }
                    _selectionIndex = 0;
                }
                else if (listSelectedTimePeriods.SelectedItems.Count == 0)
                {
                    listSelectedTimePeriods.SelectedIndex = 0;
                    _selectionIndex = 0;
                }
                else if (listSelectedTimePeriods.SelectedItems.Count > 1)
                {
                    if (forward)
                    {
                        _selectionIndex++;
                        if (_selectionIndex == listSelectedTimePeriods.SelectedIndices.Count)
                        {
                            _selectionIndex = 0;
                        }
                    }
                    else
                    {
                        _selectionIndex--;
                        if (_selectionIndex == -1)
                        {
                            _selectionIndex = listSelectedTimePeriods.SelectedIndices.Count - 1;
                        }
                    }
                }

                if (listSelectedTimePeriods.SelectedIndices.Count > 0 && _selectionIndex <= listSelectedTimePeriods.SelectedIndices.Count)
                {
                    MapSheet(listSelectedTimePeriods.SelectedIndices[_selectionIndex]);
                }
                else
                {
                    _selectionIndex = 0;
                }
            }
        }

        private void OnComboSelectionIndexChanged(object sender, EventArgs e)
        {
            List<int> columns = new List<int>();
            btnReadFile.Enabled = false;
            if (cboLatitude.Text.Length > 0
                && cboLongitude.Text.Length > 0
                && cboTemporal.Text.Length > 0
                && cboValue.Text.Length > 0)
            {
                int c = 0;
                foreach (Control ctl in tabStart.Controls)
                {
                    if (ctl.Tag?.ToString() == "dc")
                    {
                        c = ((ComboBox)ctl).SelectedIndex;
                        if (columns.Count > 0 && columns.Contains(c))
                        {
                            MessageBox.Show("All selected fields must be unique");
                        }
                        else if (columns.Count == 3)
                        {
                            btnReadFile.Enabled = true;
                        }
                        else
                        {
                            columns.Add(c);
                        }
                    }
                }
            }

            btnCategorize.Enabled = cboFirstData.Text.Length > 0
                && cboLastData.Text.Length > 0;

            if (cboFirstData.Text.Length > 0)
            {
                _firstColIndex = cboFirstData.SelectedIndex;
            }

            if (cboLastData.Text.Length > 0)
            {
                _lastColIndex = cboLastData.SelectedIndex;
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

        /// <summary>
        /// Sizes all columns so that it fits the widest column content or the column header content
        /// </summary>
        private void SizeColumns(DataGridView dg, bool init = true, bool PreserveLastColWidth = false)
        {
            var n = 0;
            foreach (DataGridViewColumn c in dg.Columns)
            {
                n++;
                if (init)
                {
                    c.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                    c.Tag = c.Width;
                }
                else
                {
                    if (n == dg.ColumnCount && PreserveLastColWidth) return;

                    c.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
                    c.Width = c.Width > (int)c.Tag ? c.Width : (int)c.Tag;
                }
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            btnOk.Enabled = false;
            //btnReadSheet.Enabled = false;
            btnReadFile.Enabled = false;
            btnCategorize.Enabled = false;
            btnShowGridPoints.Enabled = false;
            lblMappedSheet.Visible = false;
            SizeColumns(dgCategories);
            SizeColumns(dgSheetSummary);
            icbColorScheme.ComboStyle = UserControls.ImageComboStyle.ColorSchemeGraduated;
            icbColorScheme.ColorSchemes = global.MappingForm.MapLayersHandler.LayerColors;
            if (icbColorScheme.Items.Count > 0)
            {
                icbColorScheme.SelectedIndex = 0;
            }
            txtCategoryCount.Text = "5";
            listSelectedTimePeriods.Enabled = false;
            Text = "Spatio-Temporal Mapping";
            global.LoadFormSettings(this, true);

            //ERDDAP data download
            ERDDAPMetadataHandler.OnMetadataRead += OnERDDAPMetadataRead;
            _ERDDAPMetadataFolder = ERDDAPMetadataHandler.GetMetadataDirectorySetting();
            txtMetadataFolderPath.Text = _ERDDAPMetadataFolder;

            _dictERDDAP.Clear();
            if (_ERDDAPMetadataFolder.Length > 0)
            {
                //the xml metadatafiles in the metadata folder determines what FAD3 can download from ERDDAP
                ERDDAPMetadataHandler.MetadataDirectory = _ERDDAPMetadataFolder;

                //read xml metadata for ERDDAP download
                ERDDAPMetadataHandler.ReadISO9115Metadata();
            }

            cboClassificationScheme.Items.Add("Jenk's-Fisher's");
            cboClassificationScheme.Items.Add("Equal interval");
            cboClassificationScheme.Items.Add("User defined");
            cboClassificationScheme.SelectedIndex = 0;
            MakeGridFromPoints.OnCSVRead += OnReadCSV;
            MakeGridFromPoints.OnExtentDefined += OnExtentDefined;

            lvERDDAP.Columns.Clear();
            lvERDDAP.Columns.Add("Title");
            lvERDDAP.Columns.Add("Data start");
            lvERDDAP.Columns.Add("Data end");
            lvERDDAP.Columns.Add("Frequency");
            lvERDDAP.Columns.Add("Cell size");
            lvERDDAP.ShowItemToolTips = true;
            SizeColumns(lvERDDAP);
        }

        private void OnERDDAPMetadataRead(object sender, ERDDAPMetadataReadEventArgs e)
        {
            var item = lvERDDAP.Items.Add(e.FileIdentifier, e.DataTitle, null);
            item.SubItems.Add(e.BeginPosition.ToShortDateString());
            item.SubItems.Add(e.EndPosition.ToShortDateString());
            TimeSpan diff = e.EndPosition - e.BeginPosition;
            double days = diff.TotalDays;
            var freq = Math.Round(days / e.TemporalSize, MidpointRounding.AwayFromZero);
            string dataFreq = "Daily";
            switch (freq)
            {
                case 1:
                    break;

                case 30:
                    dataFreq = "Monthly";
                    break;

                default:
                    dataFreq = $"Every {freq} days";
                    break;
            }
            item.SubItems.Add(dataFreq);
            item.SubItems.Add(e.RowSize.ToString("N4"));
            item.ToolTipText = e.DataAbstract;
            Extents ext = new Extents();
            ext.SetBounds(e.EastBound, e.SouthBound, 0, e.WestBound, e.NorthBound, 0);
            _dictERDDAP.Add(e.FileIdentifier, (e.DataParameters, e.DataTitle, ext, e.URL, e.Credit, e.LegalConstraints, e.DataAbstract, e.Dimensions, e.BeginPosition, e.EndPosition));
            SizeColumns(lvERDDAP, false);
        }

        private void OnExtentDefined(object sender, ExtentDraggedBoxEventArgs e)
        {
            txtMaxLat.Text = e.Top.ToString();
            txtMinLat.Text = e.Bottom.ToString();
            txtMinLon.Text = e.Left.ToString();
            txtMaxLon.Text = e.Right.ToString();
            _selectionExtent = new Extents();
            _selectionExtent.SetBounds(e.Left, e.Bottom, 0, e.Right, e.Top, 0);
            if (!e.InDrag)
            {
                MakeGridFromPoints.MapLayers = global.MappingForm.MapLayersHandler;
                MakeGridFromPoints.MakeExtentShapeFile();
            }
        }

        private void OnReadCSV(object sender, ParseCSVEventArgs e)
        {
            try
            {
                lblStatus.Invoke((MethodInvoker)delegate
                {
                    if (!_hasReadCoordinates)
                    {
                        _hasReadCoordinates = true;
                        txtRows.Text = e.CoordinatesRead.ToString();
                    }

                    if (e.FinishedRead)
                    {
                        lblStatus.Text = "Finished reading data";
                    }
                    else
                    {
                        lblStatus.Text = $"Read time period: {e.TimePeriodProcessed}";
                    }
                });
            }
            catch
            {
                //ignore
            }
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
            _dictERDDAP.Clear();
            MakeGridFromPoints.Cleanup();
        }

        private void OnListBoxKeyDown(object sender, KeyEventArgs e)
        {
        }

        private void OnListBoxKeyUp(object sender, KeyEventArgs e)
        {
            //switch (e.KeyCode)
            //{
            //    case Keys.Up:
            //        MapSheet(false, fromArrowKeys: true);
            //        break;

            //    case Keys.Down:
            //        MapSheet(true, fromArrowKeys: true);
            //        break;
            //}
            OnSelectedSheetsClick(null, null);
        }

        private void UnsetMap()
        {
            if (MakeGridFromPoints.MapControl != null)
            {
                MakeGridFromPoints.UnsetMap();
                global.MappingForm.MapInterActionHandler.EnableMapInteraction = true;
            }
        }

        private void OnRadioButtonCheckChange(object sender, EventArgs e)
        {
            switch (((RadioButton)sender).Name)
            {
                case "rbtnUseSelectionBox":
                    global.MappingForm.MapInterActionHandler.EnableMapInteraction = false;
                    MakeGridFromPoints.MapControl = global.MappingForm.MapControl;
                    break;

                case "rbtnUseSelectedLayer":
                    UnsetMap();
                    break;

                case "rbtnManual":
                    UnsetMap();
                    break;
            }
        }

        private void OnTabMapIndexChanged(object sender, EventArgs e)
        {
        }
    }
}
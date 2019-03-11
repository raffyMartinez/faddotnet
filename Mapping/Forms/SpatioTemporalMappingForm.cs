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
    public partial class SpatioTemporalMappingForm : Form
    {
        private string _dataSourceFileName;
        private static SpatioTemporalMappingForm _instance;
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
        private bool _willUpdateEndPosition;
        private string _identiferToUpdate;
        private HashSet<double> _hashSelectedValues = new HashSet<double>();
        private string _classificationScheme;

        public static SpatioTemporalMappingForm GetInstance()
        {
            if (_instance == null) _instance = new SpatioTemporalMappingForm();
            return _instance;
        }

        public SpatioTemporalMappingForm()
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
            txtDatasetUniqueCount.Text = MakeGridFromPoints.HashedValues.Count.ToString();
            txtDatasetNumberValues.Text = MakeGridFromPoints.CountNonNullValues.ToString();
            txtDatasetMax.Text = MakeGridFromPoints.MaximumValue.ToString();
            txtDatasetMin.Text = MakeGridFromPoints.MinimumValue.ToString();
            lblStatus.Text = $"Finished reading data in {MakeGridFromPoints.ParsingTimeSeconds.ToString("N2")} seconds";
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
                case "btnExclude":
                    break;

                case "btnShowMetadata":
                    global.ShowCopyableText("Metadata", txtMetadata.Text, this);
                    break;

                case "btnOpen":
                    MakeGridFromPoints.Reset();
                    txtRows.Text = "";
                    OpenFile();
                    if (MakeGridFromPoints.IsNCCSVFormat)
                    {
                        txtMetadata.Text = MakeGridFromPoints.Metadata;
                    }
                    else
                    {
                        txtMetadata.Text = "No metadata avalailable in this file format\r\nNCCVS format includes data and metadata";
                    }
                    break;

                case "btnReadFile":

                    //reads the file that was opened

                    btnReadFile.Enabled = false;
                    MakeGridFromPoints.LatitudeColumn = cboLatitude.SelectedIndex;
                    MakeGridFromPoints.LongitudeColumn = cboLongitude.SelectedIndex;
                    MakeGridFromPoints.TemporalColumn = cboTemporal.SelectedIndex;
                    MakeGridFromPoints.ParameterColumn = cboValue.SelectedIndex;
                    MakeGridFromPoints.SelectedParameter = cboValue.Text;

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
                        _classificationScheme = cboClassificationScheme.Text;
                        switch (_classificationScheme)
                        {
                            case "Jenk's-Fisher's":
                                btnShowGridPoints.Enabled = DoJenksFisherCategorization();
                                break;

                            case "Unique values":
                                btnShowGridPoints.Enabled = DoUniqueValuesCategorization();
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
                                MessageBox.Show("Not yet implemented", "Not functioning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                        }
                        //summarize the data
                        txtSelectedValuesCount.Text = _dataValues.Count.ToString();
                        txtSelectedMinimum.Text = _dataValues.Min().ToString();
                        txtSelectedMaximum.Text = _dataValues.Max().ToString();
                        txtSelectedUnique.Text = _hashSelectedValues.Count.ToString();
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

                case "btnOk":
                    Close();
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

        private int GetClassSize(double uniqueValue)
        {
            var g = _dataValues.GroupBy(i => i);
            foreach (var grp in g)
            {
                if (grp.Key == uniqueValue)
                {
                    return grp.Count();
                }
            }
            return 0;
        }

        private bool DoUniqueValuesCategorization()
        {
            if (_hashSelectedValues.Count > 0)
            {
                dgCategories.Rows.Clear();
                int row;
                Color color;

                MakeGridFromPoints.Categories.Clear();
                ColorBlend blend = (ColorBlend)icbColorScheme.ColorSchemes.List[icbColorScheme.SelectedIndex];
                MakeGridFromPoints.NumberOfCategories = _hashSelectedValues.Count;
                MakeGridFromPoints.ColorBlend = blend;

                foreach (var item in _hashSelectedValues)
                {
                    color = MakeGridFromPoints.AddUniqueItemCategory(item);
                    row = dgCategories.Rows.Add(new object[] { item.ToString(), GetClassSize(item), "" });
                    dgCategories[2, row].Style.BackColor = color;
                }
                //add an empty null category
                MakeGridFromPoints.AddNullCategory();
            }
            return _hashSelectedValues.Count > 0;
        }

        /// <summary>
        /// do a Jenk's-Fisher categorization of the values. The category breaks are found in List<Double> listBreaks
        /// </summary>
        /// <returns></returns>
        private bool DoJenksFisherCategorization()
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
            _hashSelectedValues.Clear();
            _dataValues.Clear();
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
                                        _hashSelectedValues.Add((double)value);
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

                MakeGridFromPoints.MapColumn(_columnValues, lblMappedSheet.Text, _classificationScheme);

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
        /// Sizes all columns of listview so that it fits the widest column content or the column header content
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
        /// Sizes all columns of the gridview so that it fits the widest column content or the column header content
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
            SetUpTooltips();
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

            cboClassificationScheme.Items.Add("Jenk's-Fisher's");
            cboClassificationScheme.Items.Add("Equal interval");
            cboClassificationScheme.Items.Add("Unique values");
            cboClassificationScheme.Items.Add("User defined");
            cboClassificationScheme.SelectedIndex = 0;
            MakeGridFromPoints.OnCSVRead += OnReadCSV;
        }

        private void SetUpTooltips()
        {
            // Create the ToolTip and associate with the Form container.
            ToolTip tt = new ToolTip();

            // Set up the delays for the ToolTip.
            tt.AutoPopDelay = 5000;
            tt.InitialDelay = 1000;
            tt.ReshowDelay = 500;
            // Force the ToolTip text to be displayed whether or not the form is active.
            tt.ShowAlways = true;

            tt.SetToolTip(cboLongitude, "Column in the data representing longitude");
            tt.SetToolTip(cboLatitude, "Column in the data representing latitude");
            tt.SetToolTip(cboTemporal, "Column in the data representing time");
            tt.SetToolTip(cboValue, "Column in the data representing the value to map");
            tt.SetToolTip(cboFirstData, "First time slice to include in the map");
            tt.SetToolTip(cboLastData, "Last time slice to include in the map");
            tt.SetToolTip(txtRows, "Number of point coordinates in the map");
            tt.SetToolTip(txtInlandPoints, "Number of point coordinates that are inland");
            tt.SetToolTip(btnOpen, "Loads and then opens a data file");
            tt.SetToolTip(btnReadFile, "Reads the loaded data file");
            tt.SetToolTip(btnOk, "Closes this window");
            tt.SetToolTip(txtMetadata, "Metadata of the dataset, if available");
            tt.SetToolTip(txtFile, "Filename of the currenty loaded dataset");
            tt.SetToolTip(icbColorScheme, "Select color scheme to symbolize categories");
            tt.SetToolTip(cboClassificationScheme, "Select classification scheme for creating categories");
            tt.SetToolTip(txtCategoryCount, "Number of categories to create");
            tt.SetToolTip(btnCategorize, "Click to create categories using selected classificaton scheme");
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
                        lblStatus.Text = $"Finished reading data in {MakeGridFromPoints.ParsingTimeSeconds.ToString()} seconds";
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
            MakeGridFromPoints.Cleanup();
        }

        private void OnListBoxKeyDown(object sender, KeyEventArgs e)
        {
        }

        private void OnListBoxKeyUp(object sender, KeyEventArgs e)
        {
            OnSelectedSheetsClick(null, null);
        }

        private void OnTabMapIndexChanged(object sender, EventArgs e)
        {
        }
    }
}
using FAD3.Database.Classes;
using FAD3.Mapping.Classes;
using FAD3.GUI.Forms;
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
using FAD3.GUI.Classes;

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
        private bool _hasReadCoordinates;
        private string _ERDDAPMetadataFolder;
        private Extents _selectionExtent;
        private bool _willUpdateEndPosition;
        private string _identiferToUpdate;
        private HashSet<double> _hashSelectedValues = new HashSet<double>();
        private string _classificationScheme;
        private string _exportedTimeSeriesFileName;
        private bool _datafileRead;
        private string _startTimePeriod = "";
        private string _endTimePeriod = "";
        private bool _classificationCancelled = false;

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
        private bool OpenFile()
        {
            bool success = false;
            var fileOpen = new OpenFileDialog
            {
                Title = "Open MS Excel file",
                Filter = "CSV (comma separated values) file|*.csv|Excel file|*.xls;*.xlsx|All file types|*.*",
                FilterIndex = 1
            };
            DialogResult dr = fileOpen.ShowDialog();
            if (dr == DialogResult.OK && fileOpen.FileName.Length > 0 && File.Exists(fileOpen.FileName))
            {
                MakeGridFromPoints.Reset(true);
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
                        if (MakeGridFromPoints.CSVReadError.Length > 0)
                        {
                            MessageBox.Show(MakeGridFromPoints.CSVReadError, "File open error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
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
                        }
                        break;
                }
                success = true;
            }

            return success;
        }

        /// <summary>
        /// calls method that reads csv data as a separate process. When reading of CSV is done, it will update the fields
        /// </summary>
        private async void ReadCSVFile()
        {
            bool result = await ReadCVSFileTask();

            //this will wait until ReadCVSFileTask() is finished then
            //the fields and comboBoxes is filled up
            _dataPoints = MakeGridFromPoints.PointCountPerTimeEra;
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
            txtDatasetNumberOfTimePeriods.Text = MakeGridFromPoints.DictTemporalValues.Count.ToString();
            lblStatus.Text = $"Finished reading data in {MakeGridFromPoints.ParsingTimeSeconds.ToString("N2")} seconds";
            btnReadFile.Enabled = true;

            if (_startTimePeriod.Length > 0)
            {
                cboFirstData.Text = _startTimePeriod;
            }
            if (_endTimePeriod.Length > 0)
            {
                cboLastData.Text = _endTimePeriod;
            }
            _datafileRead = true;
        }

        /// <summary>
        /// reads CSV data as a separate process
        /// </summary>
        /// <returns></returns>
        private Task<bool> ReadCVSFileTask()
        {
            return Task.Run(() => MakeGridFromPoints.ParseSingleDimensionCSV());
        }

        private void RemoveMappingResults()
        {
            listSelectedTimePeriods.Items.Clear();
            dgCategories.Rows.Clear();
            dgSheetSummary.Rows.Clear();
            graphSheet.Series.Clear();
            foreach (Control c in groupBoxSummary.Controls)
            {
                if (c.GetType().Name == "TextBox")
                {
                    c.Text = "";
                }
            }
        }

        private void ClearSummary()
        {
            foreach (Control c in tabSummary.Controls)
            {
                if (c.GetType().Name == "TextBox")
                {
                    c.Text = "";
                }
            }
        }

        private void ResetCategoryAndMappingResults()
        {
            dgCategories.Rows.Clear();
            dgSheetSummary.Rows.Clear();
            listSelectedTimePeriods.Items.Clear();
            graphSheet.Series.Clear();
            lblParameter.Text = "";
            lblMappedSheet.Text = "";
            foreach (Control c in groupBoxSummary.Controls)
            {
                if (c.GetType().Name == "TextBox")
                {
                    c.Text = "";
                }
            }
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
                    _startTimePeriod = "";
                    _endTimePeriod = "";
                    cboFirstData.Items.Clear();
                    cboLastData.Items.Clear();
                    cboFirstData.Text = "";
                    cboLastData.Text = "";
                    txtRows.Text = "";
                    txtMetadata.Text = "";
                    if (OpenFile())
                    {
                        ClearSummary();
                        MakeGridFromPoints.RemoveMappingLayers();
                        txtMetadata.Text = "";
                        if (MakeGridFromPoints.IsNCCSVFormat)
                        {
                            txtMetadata.Text = MakeGridFromPoints.Metadata;
                        }
                        else
                        {
                            txtMetadata.Text = "No metadata avalailable in this file format\r\nNCCVS format includes data and metadata";
                        }
                        btnReadFile.Enabled = false;
                        btnShowGridPoints.Enabled = false;
                        btnShowGridPolygons.Enabled = false;
                        btnCategorize.Enabled = false;
                        btnExport.Enabled = false;
                        lblStatus.Text = "";
                        _hasMesh = false;
                        ResetCategoryAndMappingResults();
                        _datafileRead = false;
                    }
                    break;

                case "btnReadFile":
                    bool proceedReadFile = true;
                    cboFirstData.Items.Clear();
                    cboLastData.Items.Clear();

                    btnReadFile.Enabled = false;
                    MakeGridFromPoints.LatitudeColumn = cboLatitude.SelectedIndex;
                    MakeGridFromPoints.LongitudeColumn = cboLongitude.SelectedIndex;
                    MakeGridFromPoints.TemporalColumn = cboTemporal.SelectedIndex;
                    MakeGridFromPoints.ParameterColumn = cboValue.SelectedIndex;
                    MakeGridFromPoints.SelectedParameter = cboValue.Text;
                    MakeGridFromPoints.OtherTimeUnit = cboTemporal.Text;

                    //ask UTMzone of area of interest
                    using (SelectUTMZoneForm szf = new SelectUTMZoneForm())
                    {
                        szf.ShowDialog();
                        if (szf.DialogResult == DialogResult.OK)
                        {
                            _utmZone = szf.UTMZone;
                            MakeGridFromPoints.IgnoreInlandPoints = false;
                            MakeGridFromPoints.UTMZone = _utmZone;
                        }
                        else if (szf.DialogResult == DialogResult.Ignore)
                        {
                            MakeGridFromPoints.IgnoreInlandPoints = true;
                        }
                        else if (szf.DialogResult == DialogResult.Cancel)
                        {
                            proceedReadFile = false;
                            btnReadFile.Enabled = true;
                        }
                    }

                    if (proceedReadFile)
                    {
                        ResetCategoryAndMappingResults();

                        //reads csv data in a separate thread
                        ReadCSVFile();

                        lblParameter.Text = cboValue.Text;
                    }
                    break;

                case "btnCategorize":
                    btnCategorize.Enabled = false;
                    bool categorizationSuccess = false;
                    _classificationCancelled = false;

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
                                categorizationSuccess = DoJenksFisherCategorization();
                                break;

                            case "Unique values":
                                categorizationSuccess = DoUniqueValuesCategorization();
                                txtCategoryCount.Text = _hashSelectedValues.Count.ToString();
                                break;

                            case "User defined":
                                MessageBox.Show("Not yet implemented", "Not functioning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;

                            case "Equal interval":

                                using (ShapefileClassificationSchemeForm scsf = new ShapefileClassificationSchemeForm())
                                {
                                    scsf.ParameterToClassify = cboValue.Text;
                                    scsf.ClassificationScheme = cboClassificationScheme.Text;
                                    scsf.MinimumValue = _dataValues.Min();
                                    scsf.MaximumValue = _dataValues.Max();
                                    scsf.ShowDialog();
                                    if (scsf.DialogResult == DialogResult.OK)
                                    {
                                        categorizationSuccess = DoEqualIntervalCategorization(scsf.Intervals);
                                        txtCategoryCount.Text = (scsf.NumberOfCategories - 1).ToString();
                                    }
                                    else if (scsf.DialogResult == DialogResult.Cancel)
                                    {
                                        _classificationCancelled = true;
                                    }
                                }

                                break;
                        }
                        //summarize the data
                        if (categorizationSuccess)
                        {
                            txtSelectedValuesCount.Text = _dataValues.Count.ToString();
                            txtSelectedMinimum.Text = _dataValues.Min().ToString();
                            txtSelectedMaximum.Text = _dataValues.Max().ToString();
                            txtSelectedUnique.Text = _hashSelectedValues.Count.ToString();
                            txtSelectedNumberOfPeriods.Text = listSelectedTimePeriods.Items.Count.ToString();
                            if (_hasMesh)
                            {
                                MakeGridFromPoints.SetMeshCategories();
                                btnExport.Enabled = true;
                            }
                            lblStatus.Text = "Finished categorizing data";
                        }
                        else
                        {
                            if (!_classificationCancelled)
                            {
                                MessageBox.Show("Categorization was not successful. Please close this window and try again", "Categrorization failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
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

                    if (MakeGridFromPoints.RegularShapeGrid)
                    {
                        btnCategorize.Enabled = MapGridPoints();
                        listSelectedTimePeriods.Enabled = _hasMesh;
                    }
                    else
                    {
                        btnShowGridPolygons.Enabled = MapGridPoints();
                    }
                    txtInlandPoints.Text = MakeGridFromPoints.InlandPointCount.ToString();
                    break;

                case "btnShowGridPolygons":

                    //creates a grid with the datapoints at the center of the grid

                    if (MakeGridFromPoints.MakeGridShapefile())
                    {
                        _hasMesh = MakeGridFromPoints.MeshShapefileHandle > 0;
                        if (_hasMesh)
                        {
                            btnCategorize.Enabled = cboFirstData.SelectedIndex >= 0 && cboLastData.SelectedIndex >= 0;
                        }
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

                case "btnExport":
                    ExportSheetsToText();
                    break;
            }
        }

        /// <summary>
        /// export as a time series textfile containing categories and number per category
        /// </summary>
        private void ExportSheetsToText()
        {
            Dictionary<string, int> dictCategory = new Dictionary<string, int>();
            var saveDialog = new SaveFileDialog();
            saveDialog.Title = "Export data to time series text file";
            saveDialog.Filter = "Text file|*.txt|All files|*.*";
            saveDialog.FilterIndex = 1;
            string firstDate, lastDate = "";
            try
            {
                firstDate = DateTime.Parse(cboFirstData.Text).ToString("MMM-dd-yyyy");
            }
            catch (FormatException fex)
            {
                firstDate = cboFirstData.Text;
            }

            try
            {
                lastDate = DateTime.Parse(cboLastData.Text).ToString("MMM-dd-yyyy");
            }
            catch (FormatException fex)
            {
                lastDate = cboLastData.Text;
            }
            saveDialog.FileName = $"time_series_{cboValue.Text}_{firstDate}-{lastDate}.txt";
            DialogResult dr = saveDialog.ShowDialog();
            if (dr == DialogResult.OK)
            {
                if (saveDialog.FileName.Length > 0)
                {
                    _exportedTimeSeriesFileName = saveDialog.FileName;
                    using (StreamWriter strm = new StreamWriter(_exportedTimeSeriesFileName, false))
                    {
                        //write number of datapoints
                        strm.WriteLine($"Data filename: {_dataSourceFileName}");
                        strm.WriteLine($"Data points: {txtRows.Text}");
                        strm.WriteLine($"Time periods: {txtSelectedNumberOfPeriods.Text}");
                        strm.WriteLine($"Grid variable: {cboValue.Text}");
                        strm.WriteLine($"Classification used: {_classificationScheme}");
                        strm.WriteLine($"Number of classes: {dgCategories.Rows.Count.ToString()}");
                        strm.WriteLine($"Inland points: {txtInlandPoints.Text}");
                        if (MakeGridFromPoints.Metadata?.Length > 0)
                        {
                            strm.WriteLine($"Variable long name: { MakeGridFromPoints.ValueParametersDictionary[MakeGridFromPoints.SelectedParameter].longName}");
                            strm.WriteLine($"Variable unit: {MakeGridFromPoints.ValueParametersDictionary[MakeGridFromPoints.SelectedParameter].units}");
                        }
                        //write category ranges
                        string categoryRange = "";
                        for (int brk = 0; brk < dgCategories.RowCount; brk++)
                        {
                            if (brk == dgCategories.RowCount - 1)
                            {
                                if (_classificationScheme == "Unique values")
                                {
                                    categoryRange = dgCategories.Rows[brk].Cells[0].Value.ToString();
                                }
                                else
                                {
                                    categoryRange = $"{dgCategories.Rows[brk].Cells[0].Value.ToString().Replace("> ", "")}";
                                }
                            }
                            else
                            {
                                categoryRange = dgCategories.Rows[brk].Cells[0].Value.ToString();
                            }
                            strm.WriteLine($"Category {brk + 1}: {categoryRange} Color:{dgCategories[2, brk].Style.BackColor.ToArgb().ToString()}");
                        }
                        strm.WriteLine($"Category {dgCategories.RowCount + 1}: Null Color:{Color.White.ToArgb().ToString()}");
                        strm.WriteLine("#BeginData#");

                        //setup category dictionay and the same time write column headers
                        strm.Write("Time period\t");
                        for (int col = 1; col <= dgCategories.RowCount; col++)
                        {
                            strm.Write($"{col.ToString()}\t");
                            dictCategory.Add(col.ToString(), 0);
                        }
                        strm.Write("Null\r\n");
                        dictCategory.Add("Null", 0);

                        //read entire dataset and categorize the value of the current time period
                        for (int n = 0; n < listSelectedTimePeriods.Items.Count; n++)
                        {
                            string timePeriod = $"{listSelectedTimePeriods.Items[n]}";

                            //reset category dictionary values to zero
                            for (int col = 1; col <= dgCategories.RowCount; col++)
                            {
                                dictCategory[col.ToString()] = 0;
                            }
                            dictCategory["Null"] = 0;

                            switch (Path.GetExtension(_dataSourceFileName))
                            {
                                case ".xlsx":

                                    break;

                                case ".csv":
                                    foreach (var item in MakeGridFromPoints.DictTemporalValues[timePeriod].Values)
                                    {
                                        if (item == null)
                                        {
                                            dictCategory["Null"]++;
                                        }
                                        else
                                        {
                                            dictCategory[MakeGridFromPoints.WhatCategory(item, _classificationScheme).ToString()]++;
                                        }
                                    }
                                    break;
                            }

                            //write the current timeperiod and the number of values per category
                            strm.Write($"{timePeriod}\t");
                            for (int col = 1; col <= dgCategories.RowCount; col++)
                            {
                                strm.Write($"{dictCategory[col.ToString()]}\t");
                            }
                            strm.Write($"{dictCategory["Null"]}\r\n");
                        }
                        strm.WriteLine("#EndData#");
                    }
                    MessageBox.Show("Finished exporting to time series text file");
                    if (chkViewTimeSeriesChart.Checked)
                    {
                        GraphForm gf = GraphForm.GetInstance();
                        if (gf.Visible)
                        {
                            gf.BringToFront();
                        }
                        else
                        {
                            gf.DataFile = _exportedTimeSeriesFileName;
                            gf.MapSpatioTemporalData(SeriesChartType.StackedArea100);
                            gf.Show(this);
                        }
                    }
                }
            }
        }

        private void OnCellDblClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                ColorDialog cd = new ColorDialog()
                {
                    AllowFullOpen = true,
                    AnyColor = true,
                    Color = dgCategories[e.ColumnIndex, e.RowIndex].Style.BackColor
                };
                dgCategories.ClearSelection();
                cd.ShowDialog();
                dgCategories[e.ColumnIndex, e.RowIndex].Style.BackColor = cd.Color;
                MakeGridFromPoints.Categories.Item[e.RowIndex].DrawingOptions.FillColor = Colors.ColorToUInteger(cd.Color);
                MakeGridFromPoints.Categories.Item[e.RowIndex].DrawingOptions.LineColor = MakeGridFromPoints.Categories.Item[e.RowIndex].DrawingOptions.FillColor;
            }
        }

        private bool MapGridPoints()
        {
            MakeGridFromPoints.MapInteractionHandler = global.MappingForm.MapInterActionHandler;
            MakeGridFromPoints.GeoProjection = global.MappingForm.MapControl.GeoProjection;

            if (MakeGridFromPoints.RegularShapeGrid)
            {
                if (MakeGridFromPoints.MakeMeshShapefile(!MakeGridFromPoints.IgnoreInlandPoints))
                {
                    _hasMesh = MakeGridFromPoints.MeshShapefileHandle > 0;
                    return _hasMesh;
                }
            }
            else
            {
                if (MakeGridFromPoints.MakePointShapefile(!MakeGridFromPoints.IgnoreInlandPoints))
                {
                    return MakeGridFromPoints.GridPointShapefileHandle > 0;
                }
            }

            return false;
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

        private bool DoEqualIntervalCategorization(List<double> ListOfIntervals)
        {
            if (ListOfIntervals.Count > 0)
            {
                dgCategories.Rows.Clear();
                _dataValues.Sort();
                var n = 0;
                var lower = ListOfIntervals.Min();
                var upper = 0D;
                int row;

                Color color;

                MakeGridFromPoints.Categories.Clear();
                ColorBlend blend = (ColorBlend)icbColorScheme.ColorSchemes.List[icbColorScheme.SelectedIndex];
                MakeGridFromPoints.NumberOfCategories = ListOfIntervals.Count;
                MakeGridFromPoints.ColorBlend = blend;

                //make categories from the breaks defined in Jenk's-Fisher's
                //add the category range and color to a datagridview
                foreach (var item in ListOfIntervals)
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

                //add an empty null category
                MakeGridFromPoints.AddNullCategory();
                return MakeGridFromPoints.Categories.Count > 0;
            }
            return false;
        }

        private bool DoUniqueValuesCategorization()
        {
            int uniqueValuesCount = int.Parse(txtDatasetUniqueCount.Text);
            if (uniqueValuesCount > 50)
            {
                DialogResult dr = MessageBox.Show($"Dataset contains {uniqueValuesCount} unique values.\r\n Proceed in doing unique values classification?",
                    "Continue with unique value classification", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (dr == DialogResult.No)
                {
                    _classificationCancelled = true;
                    return false;
                }
            }
            if (_hashSelectedValues.Count > 0)
            {
                dgCategories.Rows.Clear();
                int row;
                Color color;

                MakeGridFromPoints.Categories.Clear();
                ColorBlend blend = (ColorBlend)icbColorScheme.ColorSchemes.List[icbColorScheme.SelectedIndex];
                MakeGridFromPoints.NumberOfCategories = _hashSelectedValues.Count;
                MakeGridFromPoints.ColorBlend = blend;

                List<double> values = _hashSelectedValues.ToList();
                values.Sort();
                foreach (var item in values)
                {
                    color = MakeGridFromPoints.AddUniqueItemCategory(item);
                    row = dgCategories.Rows.Add(new object[] { item.ToString(), GetClassSize(item), "" });
                    dgCategories[2, row].Style.BackColor = color;
                }
                //add an empty null category
                MakeGridFromPoints.AddNullCategory();
            }
            return MakeGridFromPoints.Categories.Count > 0;
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
                row = dgCategories.Rows.Add(new object[] { $"{listBreaks.Max().ToString("N5")} - {_dataValues.Max().ToString("N5")}", GetClassSize(listBreaks.Max(), 0, true).ToString(), "" });
                dgCategories[2, row].Style.BackColor = color;

                //add an empty null category
                MakeGridFromPoints.AddNullCategory();

                return MakeGridFromPoints.Categories.Count > 0;
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
                        MakeGridFromPoints.AddCoordinate(row, lat, lon, row == 0);
                    }
                    break;
            }
        }

        /// <summary>
        /// lists the time periods of the data columns in a listbox and puts all the associated
        /// grid parameter values in a list<double> and a hashset<>.  The list will be used for subsequent classification.
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

                //update mesh with values from the selected month then categorize the column
                if (MakeGridFromPoints.MapColumn(_columnValues, lblMappedSheet.Text, _classificationScheme))
                {
                    ClassificationType classificationType = ClassificationType.None;
                    switch (_classificationScheme)
                    {
                        case "Unique values":
                            classificationType = ClassificationType.UniqueValues;
                            break;

                        case "Jenk's-Fisher's":
                            classificationType = ClassificationType.JenksFisher;
                            break;

                        case "Equal interval":
                            classificationType = ClassificationType.EqualIntervals;
                            break;
                    }
                    MakeGridFromPoints.MapLayersHandler[MakeGridFromPoints.MeshShapefileHandle].ClassificationType = classificationType;
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
            }
            else
            {
                MessageBox.Show("Mesh layer not found", "Layer not found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdateSheetSummary(Dictionary<string, int> summary)
        {
            if (MakeGridFromPoints.Categories.Count > 0)
            {
                double percent = 0D;
                var row = 0;
                dgSheetSummary.Rows.Clear();
                foreach (KeyValuePair<string, int> kv in summary)
                {
                    if (kv.Key == "Null")
                    {
                        percent = ((double)(kv.Value - MakeGridFromPoints.InlandPointCount) / (double)(_dataPoints - MakeGridFromPoints.InlandPointCount)) * 100;
                        row = dgSheetSummary.Rows.Add(new object[] { true, "Null", "", (kv.Value - MakeGridFromPoints.InlandPointCount).ToString(), percent.ToString("N1") });
                    }
                    else
                    {
                        percent = ((double)kv.Value / (double)(_dataPoints - MakeGridFromPoints.InlandPointCount)) * 100;
                        row = dgSheetSummary.Rows.Add(new object[] { true, "", "", kv.Value.ToString(), percent.ToString("N1") });
                    }

                    var pt = graphSheet.Series["summary"].Points.AddY(percent);
                    if (row < dgCategories.Rows.Count)
                    {
                        dgSheetSummary[1, row].Value = dgCategories[0, row].Value.ToString();
                    }
                    dgSheetSummary[2, row].Style.BackColor = MakeGridFromPoints.CategoryColor(row);
                    graphSheet.Series["summary"].Points[pt].Color = dgSheetSummary[2, row].Style.BackColor;
                    graphSheet.Series["summary"].Points[pt].BorderColor = Color.Black;
                    dgSheetSummary[0, row].Value = MakeGridFromPoints.GridShapefile.Categories.Item[row].DrawingOptions.FillVisible;
                    if (MakeGridFromPoints.GridShapefile.Categories.Item[row].Name == "nullCategory")
                    {
                        dgSheetSummary[0, row].Value = !MakeGridFromPoints.GridShapefile.Categories.Item[row].DrawingOptions.FillVisible;
                    }
                }
                dgSheetSummary.ClearSelection();
                dgSheetSummary.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
                dgSheetSummary.Columns[1].MinimumWidth = 100;
            }
            else
            {
                Logger.Log("mesh shapefile categories was not created");
            }
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
            if (cboFirstData.Text.Length > 0 && cboLastData.Text.Length > 0)
            {
                btnShowGridPoints.Enabled = _datafileRead;
                btnCategorize.Enabled = _hasMesh;
                _startTimePeriod = cboFirstData.Text;
                _endTimePeriod = cboLastData.Text;
                _firstColIndex = cboFirstData.SelectedIndex;
                _lastColIndex = cboLastData.SelectedIndex;
            }

            //if (cboFirstData.Text.Length > 0)
            //{
            //}

            //if (cboLastData.Text.Length > 0)
            //{
            //}
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
                    c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
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
            btnShowGridPolygons.Enabled = false;
            btnExport.Enabled = false;
            lblMappedSheet.Visible = false;
            dgCategories.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            dgCategories.Columns[0].MinimumWidth = 100;
            dgSheetSummary.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            dgSheetSummary.Columns[1].MinimumWidth = 100;
            icbColorScheme.ComboStyle = UserControls.ImageComboStyle.ColorSchemeGraduated;
            icbColorScheme.ColorSchemes = global.MappingForm.MapLayersHandler.LayerColors;
            if (icbColorScheme.Items.Count > 0)
            {
                icbColorScheme.SelectedIndex = 0;
            }
            txtMetadata.Text = "Only NCCVS format includes  metadata";
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
            MakeGridFromPoints.MapLayersHandler = global.MappingForm.MapLayersHandler;
            ResetCategoryAndMappingResults();
        }

        private void SetUpTooltips()
        {
            // Create the ToolTip and associate with the Form container.
            ToolTip tt = new ToolTip();

            // Set up the delays for the ToolTip.
            tt.AutoPopDelay = TooltipGlobal.AutoPopDelay;
            tt.InitialDelay = TooltipGlobal.InitialDelay;
            tt.ReshowDelay = TooltipGlobal.ReshowDelay;

            // Force the ToolTip text to be displayed whether or not the form is active.
            tt.ShowAlways = TooltipGlobal.ShowAlways;

            tt.SetToolTip(btnOpen, "Loads and then opens a data file");
            tt.SetToolTip(txtFile, "Filename of the currenty loaded dataset");
            tt.SetToolTip(cboLongitude, "Column in the data representing longitude");
            tt.SetToolTip(cboLatitude, "Column in the data representing latitude");
            tt.SetToolTip(cboTemporal, "Column in the data representing time");
            tt.SetToolTip(cboValue, "Column in the data representing the value to map");
            tt.SetToolTip(btnReadFile, "Reads the loaded data file");

            tt.SetToolTip(cboFirstData, "First time slice to include in the map");
            tt.SetToolTip(cboLastData, "Last time slice to include in the map");
            tt.SetToolTip(btnExclude, "");

            tt.SetToolTip(txtMetadata, "Contents of metadata of dataset, but only for NCCSV dataset type");

            tt.SetToolTip(txtDatasetNumberOfTimePeriods, "Number of time periods (time slices) in the dataset");
            tt.SetToolTip(txtRows, "Number of point coordinates in the map");
            tt.SetToolTip(txtInlandPoints, "Number of point coordinates that are inland.Inland points could be made null based on user input");
            tt.SetToolTip(txtDatasetNumberValues, "Number of non-null values in the dataset");
            tt.SetToolTip(txtDatasetUniqueCount, "Number of unique values in the dataset");
            tt.SetToolTip(txtDatasetMax, "Maximum value in the dataset");
            tt.SetToolTip(txtDatasetMin, "Minimum value in the dataset");

            tt.SetToolTip(icbColorScheme, "Select color scheme to symbolize categories");
            tt.SetToolTip(cboClassificationScheme, "Select classification scheme for creating categories");
            tt.SetToolTip(txtCategoryCount, "Number of categories to create");
            tt.SetToolTip(btnCategorize, "Click to categorize the data");
            tt.SetToolTip(dgCategories, "Lists the categories/intervals");

            tt.SetToolTip(txtSelectedMaximum, "Maximum value in the selected time periods in the dataset");
            tt.SetToolTip(txtSelectedMinimum, "Miniumum value in the selected time periods in the dataset");
            tt.SetToolTip(txtSelectedUnique, "Number of unique values in the selected time periods in the dataset");
            tt.SetToolTip(txtSelectedValuesCount, "Number pf values in the selected time periods in the dataset");
            tt.SetToolTip(txtSelectedNumberOfPeriods, "Number of time periods selected in the dataset");

            tt.SetToolTip(btnShowGridPoints, "Click to show the data points in the dataset. If the dataset is  rectangular, the data grid will be automatically created");
            tt.SetToolTip(btnShowGridPolygons, "Click to construct the data grid from the data points");

            tt.SetToolTip(listSelectedTimePeriods, "Shows a list of time periods that were selected. Click on a time period to see spatial distribution of the data");
            tt.SetToolTip(btnUp, "Click to map the next item in the time periods list");
            tt.SetToolTip(btnDown, "Click to map the previous item in the time periods list");
            tt.SetToolTip(dgSheetSummary, "Distribution of values within the categories/intervals");
            tt.SetToolTip(graphSheet, "Columnar chart of how values are distributed within the categories/intervals");
            tt.SetToolTip(btnExport, "Export a time series of distribution of values within categories/intervals for all time periods in the list");
            tt.SetToolTip(chkViewTimeSeriesChart, "View a 100 percent area chart of the time series");

            tt.SetToolTip(btnOk, "Closes this window");

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
                    else if (e.ReadingCoordinates)
                    {
                        lblStatus.Text = "Reading coordinates";
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
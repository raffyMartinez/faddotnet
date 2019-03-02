using ClosedXML.Excel;
using FAD3.Mapping.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using sds = Microsoft.Research.Science.Data;
using FAD3.Database.Classes;
using Microsoft.VisualBasic.FileIO;

namespace FAD3.Mapping.Forms
{
    /// <summary>
    /// interface for mapping spatio-temporal parameters
    /// </summary>
    public partial class SpatioTemporalMappingForm : Form
    {
        private static SpatioTemporalMappingForm _instance;
        private string _dataSourceFileName;
        private int _firstColIndex;
        private int _lastColIndex;
        private int _latitudeColIndex;
        private int _longitudeColIndex;
        private List<double> _dataValues = new List<double>();
        private List<double?> _columnValues = new List<double?>();
        private int _dataPoints;
        private XLWorkbook _workBook;
        private IXLWorksheet _workSheet;
        private int _selectionIndex = 0;
        private int _columnCount;
        private DataTable _dt;
        private bool _hasMesh;
        private fadUTMZone _utmZone;
        private int _inlandPointsCount;
        private bool _createFileWithoutInland;

        public static SpatioTemporalMappingForm GetInstance()
        {
            if (_instance == null) _instance = new SpatioTemporalMappingForm();
            return _instance;
        }

        public SpatioTemporalMappingForm()
        {
            InitializeComponent();
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            btnOk.Enabled = false;
            btnReadSheet.Enabled = false;
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
            listSelectedSheets.Enabled = false;
            Text = "Spatio-Temporal Mapping";

            cboClassificationScheme.Items.Add("Jenk's-Fisher's");
            cboClassificationScheme.Items.Add("Equal interval");
            cboClassificationScheme.Items.Add("User defined");
            cboClassificationScheme.SelectedIndex = 0;
        }

        private void OpenFile()
        {
            var fileOpen = new OpenFileDialog
            {
                Title = "Open MS Excel file",
                Filter = "Excel file|*.xls;*.xlsx|CSV files|*.csv|All file types|*.*",
                FilterIndex = 1
            };
            fileOpen.ShowDialog();
            if (fileOpen.FileName.Length > 0 && File.Exists(fileOpen.FileName))
            {
                _dataSourceFileName = fileOpen.FileName;
                txtFile.Text = _dataSourceFileName;
            }
        }

        /// <summary>
        /// reads sheets in an excel workbook
        /// </summary>
        private async void GetExcelSheets()
        {
            bool result = await ReadExcelFIllAsync();
        }

        /// <summary>
        /// calls  ReadExcelFile() async
        /// </summary>
        /// <returns></returns>
        private Task<bool> ReadExcelFIllAsync()
        {
            return Task.Run(() => ReadExcelFile());
        }

        /// <summary>
        /// fills up a listview with the names of sheets in an excel workbook
        /// </summary>
        /// <returns></returns>
        private bool ReadExcelFile()
        {
            try
            {
                _workBook = new XLWorkbook(_dataSourceFileName, XLEventTracking.Disabled);

                foreach (IXLWorksheet worksheet in _workBook.Worksheets)
                {
                    this.Invoke((MethodInvoker)(() => listSheets.Items.Add(worksheet.Name)));
                }
                return listSheets.Items.Count > 0;
            }
            catch (IOException iox)
            {
                MessageBox.Show(iox.Message);
                return false;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "SpatioTemporalMappingForm.cs", "ReadExcelFile");
                return false;
            }
        }

        /// <summary>
        /// reads the data inside a sheet and fill up a datatable with the sheet's contents
        /// </summary>
        private void ReadSheet()
        {
            _dataPoints = 0;
            var sheetIndex = listSheets.SelectedIndex + 1;
            _workSheet = _workBook.Worksheet(sheetIndex);
            _dt = new DataTable();
            bool firstRow = true;
            _columnCount = 0;
            var cells = 0;
            foreach (IXLRow row in _workSheet.Rows())
            {
                if (firstRow)
                {
                    cboLatitude.Items.Clear();
                    cboLongitude.Items.Clear();
                    cboFirstData.Items.Clear();
                    cboLastData.Items.Clear();
                    foreach (IXLCell cell in row.Cells())
                    {
                        _dt.Columns.Add(cell.Value.ToString());

                        //assuming that first 2 column contain x and y value of a point
                        //we populate Longitude and Latitude comboboxes with the row headers of the x and y column
                        if (_columnCount == 0)
                        {
                            cboLatitude.Items.Add(cell.Value.ToString());
                            cboLongitude.Items.Add(cell.Value.ToString());
                        }
                        else if (_columnCount == 1)
                        {
                            cboLatitude.Items.Add(cell.Value.ToString());
                            cboLongitude.Items.Add(cell.Value.ToString());
                        }

                        //we populate first and last data column comboboxes with the value header
                        else
                        {
                            cboFirstData.Items.Add(cell.Value.ToString());
                            cboLastData.Items.Add(cell.Value.ToString());
                        }
                        _columnCount++;
                    }
                    firstRow = false;
                    cboLatitude.Enabled = true;
                    cboLongitude.Enabled = true;
                    cboFirstData.Enabled = true;
                    cboLastData.Enabled = true;
                }
                else
                {
                    //we add a row to the datatable
                    _dt.Rows.Add();

                    int i = 0;

                    //populate each cell of the new row by reading the values of the columns starting from the first going to the last column
                    foreach (IXLCell cell in row.Cells(row.FirstCellUsed().Address.ColumnNumber, row.LastCellUsed().Address.ColumnNumber))
                    {
                        _dt.Rows[_dt.Rows.Count - 1][i] = cell.Value.ToString();
                        i++;
                        if (i > 1) cells++;
                    }

                    //we increment _dataPoint after we finish reading a row
                    _dataPoints++;
                }
            }

            //we fill up txtRows with the number of datapoints
            txtRows.Text = _dataPoints.ToString();
        }

        /// <summary>
        /// adds the coordinates of a datapoint to a List<double,double>
        /// </summary>
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

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOpen":
                    MakeGridFromPoints.Reset();
                    txtRows.Text = "";
                    OpenFile();
                    btnReadFile.Enabled = _dataSourceFileName?.Length > 0 && File.Exists(_dataSourceFileName);
                    if (Path.GetExtension(_dataSourceFileName) == ".csv")
                    {
                        sds.DataSet dataset = sds.DataSet.Open($"{_dataSourceFileName}?openMode=readOnly");
                        if (dataset.Dimensions.Count == 1)
                        {
                        }
                    }
                    break;

                case "btnReadFile":
                    listSheets.Items.Clear();
                    switch (Path.GetExtension(_dataSourceFileName))
                    {
                        case ".nc":
                            MessageBox.Show("NetCDF support not yet supported");
                            break;

                        case ".csv":
                            btnReadFile.Enabled = false;
                            try
                            {
                                sds.DataSet dataset = sds.DataSet.Open($"{_dataSourceFileName}?openMode=readOnly");
                                if (dataset.Dimensions.Count == 1)
                                {
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

                                    MakeGridFromPoints.SingleDimensionCSV = _dataSourceFileName;
                                    _dataPoints = MakeGridFromPoints.Coordinates.Count;
                                    txtRows.Text = _dataPoints.ToString();
                                    _inlandPointsCount = MakeGridFromPoints.InlandPointCount;
                                    if (MakeGridFromPoints.IgnoreInlandPoints)
                                    {
                                        txtInlandPoints.Text = "Ignored";
                                    }
                                    else
                                    {
                                        txtInlandPoints.Text = _inlandPointsCount.ToString();
                                    }

                                    cboLatitude.SelectedIndex = 0;
                                    cboLongitude.SelectedIndex = 0;
                                    foreach (var item in MakeGridFromPoints.DictTemporalValues)
                                    {
                                        cboFirstData.Items.Add(item.Key);
                                        cboLastData.Items.Add(item.Key);
                                    }
                                    cboFirstData.Enabled = true;
                                    cboLastData.Enabled = true;
                                }
                                else if (dataset.Dimensions.Count == 2)
                                {
                                }
                                btnReadFile.Enabled = true;
                                btnCategorize.Enabled = true;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                Logger.Log(ex.Message, "SpatioTemporalMappingForm.cs", "OnButtonClick.btnReadWorkBook.csv");
                            }
                            break;

                        case ".xlsx":
                            GetExcelSheets();
                            break;
                    }

                    break;

                case "btnReadSheet":
                    txtRows.Text = "";
                    if (listSheets.SelectedIndex >= 0)
                    {
                        ReadSheet();
                    }
                    break;

                case "btnCategorize":
                    if (txtCategoryCount.Text.Length > 0 && cboLatitude.SelectedIndex >= 0
                        && cboLongitude.SelectedIndex >= 0 && cboFirstData.SelectedIndex >= 0
                        && cboLastData.SelectedIndex >= 0)
                    {
                        listSheetsForMapping();
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
                    }
                    else
                    {
                        MessageBox.Show("Specify longitude, latitude, and, first and last data columns",
                                        "Required data is missing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    break;

                case "btnShowGridPoints":
                    MapGridPoints();
                    break;

                case "btnShowGridPolygons":
                    if (MakeGridFromPoints.MakeGridShapefile())
                    {
                        _hasMesh = global.MappingForm.MapLayersHandler.AddLayer(MakeGridFromPoints.GridShapefile, "Mesh", uniqueLayer: true) > 0;
                    }
                    listSelectedSheets.Enabled = _hasMesh;
                    break;

                case "btnUp":
                    MapSheet(false);
                    break;

                case "btnDown":
                    MapSheet(true);
                    break;

                case "btnInstructions":
                    InstructionsForm form = new InstructionsForm();
                    form.ShowDialog(this);
                    break;

                case "btnColorScheme":
                    ColorSchemesForm csf = new ColorSchemesForm(ref global.MappingForm.MapLayersHandler.LayerColors);
                    break;

                case "btnOk":
                    break;

                case "btnCancel":
                    Close();
                    break;

                case "btnExport":
                    ExportSheetsToText();
                    break;
            }
        }

        private void OnCSVReadEvent(object sender, ParseCSVEventArgs e)
        {
            string[] fields = e.Fields;
            for (int n = 0; n < fields.Length; n++)
            {
                cboLatitude.Items.Add(fields[n]);
                cboLongitude.Items.Add(fields[n]);
            }
        }

        /// <summary>
        /// handles mapping of items when the up and down buttons are pressed
        /// </summary>
        /// <param name="forward"></param>
        private void MapSheet(bool forward)
        {
            if (listSelectedSheets.Items.Count > 0)
            {
                if (listSelectedSheets.SelectedItems.Count == 1)
                {
                    var selection = listSelectedSheets.SelectedIndex;
                    listSelectedSheets.SelectedItems.Clear();
                    if (forward)
                    {
                        selection++;
                    }
                    else
                    {
                        selection--;
                    }
                    listSelectedSheets.SelectedIndex = selection;
                    _selectionIndex = 0;
                }
                else if (listSelectedSheets.SelectedItems.Count == 0)
                {
                    listSelectedSheets.SelectedIndex = 0;
                    _selectionIndex = 0;
                }
                else if (listSelectedSheets.SelectedItems.Count > 1)
                {
                    if (forward)
                    {
                        _selectionIndex++;
                        if (_selectionIndex == listSelectedSheets.SelectedIndices.Count)
                        {
                            _selectionIndex = 0;
                        }
                    }
                    else
                    {
                        _selectionIndex--;
                        if (_selectionIndex == -1)
                        {
                            _selectionIndex = listSelectedSheets.SelectedIndices.Count - 1;
                        }
                    }
                }

                if (listSelectedSheets.SelectedIndices.Count > 0 && _selectionIndex <= listSelectedSheets.SelectedIndices.Count)
                {
                    MapSheet(listSelectedSheets.SelectedIndices[_selectionIndex]);
                }
                else
                {
                    _selectionIndex = 0;
                }
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
            saveDialog.ShowDialog();
            if (saveDialog.FileName.Length > 0)
            {
                using (StreamWriter strm = new StreamWriter(saveDialog.FileName, false))
                {
                    //write number of datapoints
                    strm.WriteLine($"Data points: {_dt.Rows.Count.ToString()}");

                    //write category ranges
                    string categoryRange = "";
                    for (int brk = 0; brk < dgCategories.RowCount; brk++)
                    {
                        if (brk == dgCategories.RowCount - 1)
                        {
                            categoryRange = $"{dgCategories.Rows[brk].Cells[0].Value.ToString().Replace("> ", "")} - {txtMaximum.Text}";
                        }
                        else
                        {
                            categoryRange = dgCategories.Rows[brk].Cells[0].Value.ToString();
                        }
                        strm.WriteLine($"Category {brk + 1}: {categoryRange}");
                    }

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
                    for (int n = 0; n < listSelectedSheets.Items.Count; n++)
                    {
                        string timePeriod = $"{listSelectedSheets.Items[n]}";

                        //reset category dictionary values to zero
                        for (int col = 1; col <= dgCategories.RowCount; col++)
                        {
                            dictCategory[col.ToString()] = 0;
                        }
                        dictCategory["Null"] = 0;

                        //read all rows but only pick the value corresponding to the current time period
                        for (int row = 0; row < _dt.Rows.Count; row++)
                        {
                            var arr = _dt.Rows[row].ItemArray;    //put data in current row to an array
                            var col = n + _firstColIndex + 2;     //col points to the relevant timeperiod

                            double? v = null;
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

                            //increment value of corresponding category
                            if (v == null)
                            {
                                dictCategory["Null"]++;
                            }
                            else
                            {
                                dictCategory[MakeGridFromPoints.WhatCategory(v).ToString()]++;
                            }
                        }

                        //write the current timeperiod and the number of values per category
                        strm.Write($"{timePeriod}\t");
                        for (int col = 1; col <= dgCategories.RowCount; col++)
                        {
                            strm.Write($"{dictCategory[col.ToString()]}\t");
                        }
                        strm.Write($"{dictCategory["Null"]}\r\n");
                    }
                }
                MessageBox.Show("Finished exporting to time series text file");
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
                lblMappedSheet.Text = $"{listSelectedSheets.Items[index]}";
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

        private void MapGridPoints()
        {
            MakeGridFromPoints.MapInteractionHandler = global.MappingForm.MapInterActionHandler;
            MakeGridFromPoints.GeoProjection = global.MappingForm.MapControl.GeoProjection;

            if (MakeGridFromPoints.MakePointShapefile(!MakeGridFromPoints.IgnoreInlandPoints))
                global.MappingForm.MapLayersHandler.AddLayer(MakeGridFromPoints.PointShapefile, "Grid points");
        }

        private void OnListBoxClick(object sender, EventArgs e)
        {
            btnReadSheet.Enabled = true;
        }

        /// <summary>
        /// lists the headers of the data columns in a listbox
        /// </summary>
        private void listSheetsForMapping()
        {
            listSelectedSheets.Items.Clear();
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
                                listSelectedSheets.Items.Add(item);
                                foreach (double? value in MakeGridFromPoints.DictTemporalValues[item].Values)
                                {
                                    //if (value != "NaN" && double.TryParse(value, out double d))
                                    //{
                                    //    _dataValues.Add(d);
                                    //}
                                    if (value != null)
                                        _dataValues.Add((double)value);
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
                                listSelectedSheets.Items.Add(_dt.Columns[n].ColumnName);
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

        private void OnComboIndexChanged(object sender, EventArgs e)
        {
            var cbo = (ComboBox)sender;
            switch (cbo.Name)
            {
                case "cboFirstData":
                    _firstColIndex = cbo.SelectedIndex;
                    break;

                case "cboLastData":
                    _lastColIndex = cbo.SelectedIndex;
                    break;

                case "cboLongitude":
                    _longitudeColIndex = cbo.SelectedIndex;
                    break;

                case "cboLatitude":
                    _latitudeColIndex = cbo.SelectedIndex;
                    break;
            }

            btnCategorize.Enabled = cboFirstData.SelectedIndex >= 0 && cboLastData.SelectedIndex >= 0 && cboLatitude.SelectedIndex >= 0 && cboLongitude.SelectedIndex >= 0;
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

        private void OnSelectedSheetsClick(object sender, EventArgs e)
        {
            MapSheet(listSelectedSheets.SelectedIndices[0]);
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
    }
}
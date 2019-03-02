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
            }

            cboLatitude.Enabled = true;
            cboLongitude.Enabled = true;
            cboValue.Enabled = true;
            cboTemporal.Enabled = true;
        }

        private async void ReadCVSFile()
        {
            bool result = await ReadCVSFileTask();
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

        private Task<bool> ReadCVSFileTask()
        {
            return Task.Run(() => MakeGridFromPoints.ParseSingleDimensionCSV());
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOpen":
                    MakeGridFromPoints.Reset();
                    txtRows.Text = "";
                    OpenFile();
                    break;

                case "btnReadFile":
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

                    // if (MakeGridFromPoints.ParseSingleDimensionCSV())
                    // {
                    ReadCVSFile();
                    //_dataPoints = MakeGridFromPoints.Coordinates.Count;
                    //txtRows.Text = _dataPoints.ToString();

                    //cboFirstData.Items.Clear();
                    //cboLastData.Items.Clear();
                    //foreach (var item in MakeGridFromPoints.DictTemporalValues.Keys)
                    //{
                    //    cboFirstData.Items.Add(item);
                    //    cboLastData.Items.Add(item);
                    //}
                    //cboFirstData.Enabled = cboFirstData.Items.Count > 0;
                    //cboLastData.Enabled = cboLastData.Items.Count > 0;

                    //txtInlandPoints.Text = MakeGridFromPoints.InlandPointCount.ToString();
                    //btnReadFile.Enabled = true;
                    //}
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
            }
        }

        private void MapGridPoints()
        {
            MakeGridFromPoints.MapInteractionHandler = global.MappingForm.MapInterActionHandler;
            MakeGridFromPoints.GeoProjection = global.MappingForm.MapControl.GeoProjection;

            if (MakeGridFromPoints.MakePointShapefile(!MakeGridFromPoints.IgnoreInlandPoints))
                global.MappingForm.MapLayersHandler.AddLayer(MakeGridFromPoints.PointShapefile, "Grid points");
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
            MapSheet(listSelectedSheets.SelectedIndices[0]);
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
            listSelectedSheets.Enabled = false;
            Text = "Spatio-Temporal Mapping";

            cboClassificationScheme.Items.Add("Jenk's-Fisher's");
            cboClassificationScheme.Items.Add("Equal interval");
            cboClassificationScheme.Items.Add("User defined");
            cboClassificationScheme.SelectedIndex = 0;
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
        }
    }
}
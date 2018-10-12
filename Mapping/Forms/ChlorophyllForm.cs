using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using ISO_Classes;
using FAD3.Mapping.Classes;
using MapWinGIS;
using System.Drawing.Drawing2D;

namespace FAD3.Mapping.Forms
{
    public partial class ChlorophyllForm : Form
    {
        private string _connString;
        private static ChlorophyllForm _instance;
        private string _excelFileName;
        private DataSet _excelData;
        private int _firstColIndex;
        private int _lastColIndex;
        private int _latitudeColIndex;
        private int _longitudeColIndex;
        private Dictionary<int, (double latitude, double longitude)> _dictGridCentroidCoordinates = new Dictionary<int, (double latitude, double longitude)>();
        private List<double> _dataValues = new List<double>();
        private int _currentSheetIndex;
        private int _listIndex;
        private List<double?> _columnValues = new List<double?>();
        private int _dataPoints;

        public static ChlorophyllForm GetInstance()
        {
            if (_instance == null) return new ChlorophyllForm();
            return _instance;
        }

        public ChlorophyllForm()
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
            SizeColumns(dgCategories);
            SizeColumns(dgSheetSummary);
            icbColorScheme.ComboStyle = UserControls.ImageComboStyle.ColorSchemeGraduated;
            icbColorScheme.ColorSchemes = global.MappingForm.MapLayersHandler.LayerColors;
            if (icbColorScheme.Items.Count > 0)
            {
                icbColorScheme.SelectedIndex = 0;
            }
        }

        private void OpenFile()
        {
            var fileOpen = new OpenFileDialog
            {
                Title = "Open MS Excel file",
                Filter = "Excel file|*.xls;*.xlsx|All file types|*.*",
                FilterIndex = 1
            };
            fileOpen.ShowDialog();
            if (fileOpen.FileName.Length > 0 && File.Exists(fileOpen.FileName))
            {
                _excelFileName = fileOpen.FileName;
                txtFile.Text = _excelFileName;
            }
        }

        private string GetConnectionString(bool UseHeader)
        {
            Dictionary<string, string> props = new Dictionary<string, string>();

            // XLSX - Excel 2007, 2010, 2012, 2013
            props["Provider"] = "Microsoft.ACE.OLEDB.12.0;";
            if (UseHeader)
            {
                props["Extended Properties"] = @"""Excel 12.0 XML;HDR=YES;IMEX=1"";";
            }
            else
            {
                props["Extended Properties"] = @"""Excel 12.0 XML;HDR=NO;IMEX=1;"";";
            }
            props["Data Source"] = _excelFileName;

            // XLS - Excel 2003 and Older
            //props["Provider"] = "Microsoft.Jet.OLEDB.4.0";
            //props["Extended Properties"] = "Excel 8.0";
            //props["Data Source"] = "C:\\MyExcel.xls";

            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, string> prop in props)
            {
                sb.Append(prop.Key);
                sb.Append('=');
                sb.Append(prop.Value);
                //sb.Append(';');
            }

            return sb.ToString();
        }

        private void ReadExcelFile()
        {
            _connString = GetConnectionString(chkHasHeader.Checked);
            OleDbConnection connection = new OleDbConnection(_connString);
            connection.Open();
            DataTable dtTables = new DataTable();
            dtTables = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (dtTables != null)
            {
                foreach (DataRow r in dtTables.Rows)
                {
                    listSheets.Items.Add(r["TABLE_NAME"].ToString());
                }
            }
        }

        private void ReadSheet()
        {
            OleDbConnection connection = new OleDbConnection(_connString);
            connection.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = connection;
            cmd.CommandText = "Select * from [" + listSheets.Text + "]";
            DataTable excelTable = new DataTable();
            excelTable.TableName = listSheets.Text;

            var adapter = new OleDbDataAdapter();
            adapter.SelectCommand = cmd;

            adapter.Fill(excelTable);

            _excelData = new DataSet();
            _excelData.Tables.Add(excelTable);

            _dataPoints = _excelData.Tables[0].Rows.Count;
            txtRows.Text = _dataPoints.ToString();
            cboLatitude.Items.Clear();
            cboLongitude.Items.Clear();
            for (int n = 0; n < _excelData.Tables[0].Columns.Count; n++)
            {
                if (n == 0)
                {
                    cboLatitude.Items.Add(_excelData.Tables[0].Columns[n].ColumnName);
                    cboLongitude.Items.Add(_excelData.Tables[0].Columns[n].ColumnName);
                }
                else if (n == 1)
                {
                    cboLatitude.Items.Add(_excelData.Tables[0].Columns[n].ColumnName);
                    cboLongitude.Items.Add(_excelData.Tables[0].Columns[n].ColumnName);
                }
                else
                {
                    cboFirstData.Items.Add(_excelData.Tables[0].Columns[n].ColumnName);
                    cboLastData.Items.Add(_excelData.Tables[0].Columns[n].ColumnName);
                }
            }

            cboLatitude.Enabled = true;
            cboLongitude.Enabled = true;
            cboFirstData.Enabled = true;
            cboLastData.Enabled = true;
        }

        private void listCoordinates()
        {
            _dictGridCentroidCoordinates.Clear();
            var table = _excelData.Tables[0];

            for (int row = 0; row < table.Rows.Count; row++)
            {
                var arr = table.Rows[row].ItemArray;
                var latitude = (double)arr[_latitudeColIndex];
                var longitude = (double)arr[_longitudeColIndex];
                _dictGridCentroidCoordinates.Add(row, (latitude, longitude));
            }
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnInstructions":
                    InstructionsForm form = new InstructionsForm();
                    form.ShowDialog(this);
                    break;

                case "btnColorScheme":
                    ColorSchemesForm csf = new ColorSchemesForm(ref MappingUtilities.LayerColors);
                    break;

                case "btnDefineGrid":
                    if (MakeGridFromPoints.MakeGridShapefile())
                    {
                        global.MappingForm.MapLayersHandler.AddLayer(MakeGridFromPoints.GridShapefile, "Mesh");
                    }
                    break;

                case "btnOk":
                    break;

                case "btnCancel":
                    Close();
                    break;

                case "btnOpen":
                    OpenFile();
                    btnReadWorkbook.Enabled = _excelFileName?.Length > 0 && File.Exists(_excelFileName);
                    break;

                case "btnReadSheet":
                    if (listSheets.SelectedIndex >= 0)
                    {
                        ReadSheet();
                    }
                    break;

                case "btnReadWorkbook":
                    ReadExcelFile();
                    break;

                case "btnShowGrid":
                    MapGridPoints();
                    break;

                case "btnMapSelected":
                    if (listSelectedSheets.SelectedItems.Count > 0)
                    {
                        lblMappedSheet.Visible = true;
                        if (_currentSheetIndex < listSelectedSheets.SelectedItems.Count)
                        {
                            _listIndex = listSelectedSheets.SelectedIndices[_currentSheetIndex];
                            MapSheet(_currentSheetIndex);
                            _currentSheetIndex++;
                        }
                        else
                        {
                            _currentSheetIndex = 0;
                        }
                    }
                    break;

                case "btnCategorize":
                    if (txtCategoryCount.Text.Length > 0)
                    {
                        listSheetsForMapping();
                        listCoordinates();
                        getDataValues();
                        doJenksFisher();
                    }

                    break;

                case "btnGetSheets":

                    break;
            }
        }

        private void MapSheet(int index)
        {
            lblMappedSheet.Text = $"Sheet mapped: {listSelectedSheets.SelectedItems[index]}";
            lblMappedSheet.Tag = listSelectedSheets.SelectedItems[index];
            var table = _excelData.Tables[0];
            _columnValues.Clear();
            for (int row = 0; row < table.Rows.Count; row++)
            {
                var arr = table.Rows[row].ItemArray;
                var col = _listIndex + _firstColIndex + 2;
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
                _columnValues.Add(v);
            }
            MakeGridFromPoints.MapColumn(_columnValues, lblMappedSheet.Tag.ToString());
            global.MappingForm.MapControl.Redraw();
            UpdateSheetSummary(MakeGridFromPoints.SheetMapSummary);
        }

        private void UpdateSheetSummary(Dictionary<string, int> summary)
        {
            var row = 0;
            dgSheetSummary.Rows.Clear();
            foreach (KeyValuePair<string, int> kv in summary)
            {
                var firstColText = "";
                if (kv.Key == "Null")
                {
                    firstColText = "Null";
                }
                row = dgSheetSummary.Rows.Add(new object[] { firstColText, kv.Value.ToString(), (((double)kv.Value / (double)_dataPoints) * 100).ToString() });
                dgSheetSummary[0, row].Style.BackColor = MakeGridFromPoints.CategoryColor(row);
            }
            dgSheetSummary.ClearSelection();
        }

        private void MapGridPoints()
        {
            MakeGridFromPoints.MapInteractionHandler = global.MappingForm.MapInterActionHandler;
            MakeGridFromPoints.GeoProjection = global.MappingForm.MapControl.GeoProjection;
            MakeGridFromPoints.Coordinates = _dictGridCentroidCoordinates;

            if (MakeGridFromPoints.MakePointShapefile())
                global.MappingForm.MapLayersHandler.AddLayer(MakeGridFromPoints.PointShapefile, "Grid points");
        }

        private void OnListBoxClick(object sender, EventArgs e)
        {
            btnReadSheet.Enabled = true;
        }

        private void listSheetsForMapping()
        {
            listSelectedSheets.Items.Clear();
            var includeColumn = false;
            if (cboLongitude.Text.Length > 0 && cboLatitude.Text.Length > 0 && cboFirstData.Text.Length > 0 && cboLastData.Text.Length > 0)
            {
                for (int n = 0; n < _excelData.Tables[0].Columns.Count; n++)
                {
                    if (!includeColumn && (n - 2) == _firstColIndex)
                    {
                        includeColumn = true;
                    }
                    if (includeColumn)
                    {
                        listSelectedSheets.Items.Add(_excelData.Tables[0].Columns[n].ColumnName);
                    }
                    if (includeColumn && (n - 2) == _lastColIndex)
                    {
                        includeColumn = false;
                    }
                }
            }
        }

        private void getDataValues()
        {
            _dataValues.Clear();
            var table = _excelData.Tables[0];

            for (int row = 0; row < table.Rows.Count; row++)
            {
                var arr = table.Rows[row].ItemArray;
                for (int col = _firstColIndex + 2; col <= _lastColIndex + 2; col++)
                {
                    double? v = arr[col] as double?;
                    if (v != null)
                    {
                        _dataValues.Add((double)v);
                    }
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

        private void doJenksFisher()
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
                //MakeGridFromPoints.GenerateColorScheme(int.Parse(txtCategoryCount.Text));

                foreach (var item in listBreaks)
                {
                    if (n > 0)
                    {
                        upper = item;
                        color = MakeGridFromPoints.AddCategory(lower, upper);
                        row = dgCategories.Rows.Add(new object[] { true, $"{lower.ToString()} - {upper.ToString()}", GetClassSize(lower, upper).ToString(), "" });
                        dgCategories[3, row].Style.BackColor = color;
                        lower = item;
                    }
                    n++;
                }
                txtValuesCount.Text = _dataValues.Count.ToString();
                txtMinimum.Text = _dataValues.Min().ToString();
                txtMaximum.Text = _dataValues.Max().ToString();
                color = MakeGridFromPoints.AddCategory(upper, _dataValues.Max() + 1);
                row = dgCategories.Rows.Add(new object[] { true, $"> {listBreaks.Max().ToString()}", GetClassSize(listBreaks.Max(), 0, true).ToString(), "" });
                dgCategories[3, row].Style.BackColor = color;
                SizeColumns(dgCategories, false, true);
                MakeGridFromPoints.AddNullCategory();
            }
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
        }
    }
}
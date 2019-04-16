using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FAD3.Mapping.Classes;
using MapWinGIS;

namespace FAD3.Mapping.Forms
{
    public partial class EditShapeAttributeForm : Form
    {
        public Dictionary<string, string> FieldValueDict { get; set; }
        public MapInterActionHandler MapInterActionHandler { get; }
        private MapperForm _parentForm;
        private MapLayer _currentMapLayer;
        private static EditShapeAttributeForm _instance;
        private int _sfSelectedCount = 0;

        public static EditShapeAttributeForm GetInstance(MapperForm parent, MapInterActionHandler mapInterActionHandler)
        {
            if (_instance == null) _instance = new EditShapeAttributeForm(parent, mapInterActionHandler);
            return _instance;
        }

        public EditShapeAttributeForm(MapperForm parent, MapInterActionHandler mapInterActionHandler)
        {
            InitializeComponent();
            MapInterActionHandler = mapInterActionHandler;
            MapInterActionHandler.Selection += OnLayerSelection;
            MapInterActionHandler.MapLayersHandler.CurrentLayer += OnCurrentLayer;
            _parentForm = parent;
            _currentMapLayer = MapInterActionHandler.MapLayersHandler.CurrentMapLayer;
        }

        /// <summary>
        /// Event fired when a layer was selected in the layers form
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void OnCurrentLayer(MapLayersHandler s, LayerEventArg e)
        {
            _currentMapLayer = MapInterActionHandler.MapLayersHandler.CurrentMapLayer;
            DisplayAttributes();
        }

        private void DisplayAttributes()
        {
            int rows = 0;
            DataGridViewRow row = new DataGridViewRow();
            gridAttributes.Visible = false;

            if (_currentMapLayer != null && _currentMapLayer.LayerType == "ShapefileClass")
            {
                Text = $"Shapefile: {_currentMapLayer.Name}";

                gridAttributes.RowEnter -= OnRowEnter;
                gridAttributes.Columns.Clear();
                gridAttributes.Rows.Clear();
                gridAttributes.RowHeadersVisible = true;
                gridAttributes.AllowUserToAddRows = false;

                //set up the column headers
                _sfSelectedCount = 0;
                Shapefile sf = _currentMapLayer.LayerObject as Shapefile;
                object queryResult = new object();
                int[] rowIndices = new int[0];
                string errMsg = "";
                if (sf.VisibilityExpression.Length > 0)
                {
                    sf.Table.Query(sf.VisibilityExpression, ref queryResult, ref errMsg);
                    rowIndices = (int[])queryResult;
                }
                _sfSelectedCount = sf.NumSelected;
                if (!chkRemember.Checked)
                {
                    if (_sfSelectedCount == 1)
                    {
                        DataGridViewTextBoxColumn valueColumn = new DataGridViewTextBoxColumn();
                        gridAttributes.Columns.Add(valueColumn);
                        valueColumn.ReadOnly = false;
                        valueColumn.HeaderText = "Value";
                    }
                    else
                    {
                        gridAttributes.ColumnCount = sf.NumFields;
                        for (int n = 0; n < sf.NumFields; n++)
                        {
                            string headerText = sf.Field[n].Alias;
                            if (headerText.Length == 0)
                            {
                                headerText = sf.Field[n].Name;
                            }
                            gridAttributes.Columns[n].Name = headerText;
                            gridAttributes.Columns[n].Tag = sf.Field[n].Name;
                        }
                    }

                    if (_sfSelectedCount == 0)
                    {
                        if (sf.VisibilityExpression.Length == 0)
                        {
                            labelShapeFileName.Text = $"Attribute data of the shapefile (n={sf.NumShapes.ToString()})";
                            rows = sf.NumShapes;
                            for (int ishp = 0; ishp < rows; ishp++)
                            {
                                row = new DataGridViewRow();
                                row.CreateCells(gridAttributes);
                                object[] arr = new object[sf.NumFields];
                                for (int ifld = 0; ifld < sf.NumFields; ifld++)
                                {
                                    arr[ifld] = sf.CellValue[ifld, ishp];
                                }
                                row.SetValues(arr);
                                row.Tag = ishp;
                                gridAttributes.Rows.Add(row);
                            }
                        }
                        else
                        {
                            rows = rowIndices.Length;
                            labelShapeFileName.Text = $"Attribute data of the shapefile (n={rowIndices.Length.ToString()})";
                            for (int vShp = 0; vShp < rows; vShp++)
                            {
                                row = new DataGridViewRow();
                                row.CreateCells(gridAttributes);
                                object[] arr = new object[sf.NumFields];
                                for (int ifld = 0; ifld < sf.NumFields; ifld++)
                                {
                                    arr[ifld] = sf.CellValue[ifld, rowIndices[vShp]];
                                }
                                row.SetValues(arr);
                                row.Tag = rowIndices[vShp];
                                gridAttributes.Rows.Add(row);
                            }
                        }
                    }
                    else
                    {
                        if (sf.NumSelected == 1 && _currentMapLayer.SelectedIndexes != null)
                        {
                            labelShapeFileName.Text = "Attribute data of the selected shape (n=1)";
                            int index = _currentMapLayer.SelectedIndexes[0];

                            //show attribute of selected shape and display in 2 column property-value format
                            for (int n = 0; n < sf.NumFields; n++)
                            {
                                row = new DataGridViewRow();

                                string headerText = sf.Field[n].Alias;

                                if (headerText.Length == 0)
                                {
                                    headerText = sf.Field[n].Name;
                                }
                                string valueText = sf.CellValue[n, index].ToString();
                                row.CreateCells(gridAttributes);
                                row.Cells[0].Value = valueText;
                                row.HeaderCell.Value = headerText;
                                gridAttributes.Rows.Add(row);
                            }
                        }
                        else
                        {
                            if (_currentMapLayer.SelectedIndexes != null)
                            {
                                if (sf.VisibilityExpression.Length > 0)
                                {
                                    int c = 0;
                                    var intersect = rowIndices.Intersect(_currentMapLayer.SelectedIndexes);
                                    foreach (int v in rowIndices.Intersect(_currentMapLayer.SelectedIndexes))
                                    {
                                        c++;
                                        row = new DataGridViewRow();
                                        row.CreateCells(gridAttributes);
                                        object[] arr = new object[sf.NumFields];

                                        for (int col = 0; col < sf.NumFields; col++)
                                        {
                                            try
                                            {
                                                arr[col] = sf.CellValue[col, v].ToString();
                                            }
                                            catch
                                            {
                                                arr[col] = "";
                                            }
                                        }
                                        row.SetValues(arr);
                                        row.Tag = v;
                                        gridAttributes.Rows.Add(row);
                                    }
                                    labelShapeFileName.Text = $"Attribute data of the selected shapes (n={c.ToString()})";
                                }
                                else
                                {
                                    labelShapeFileName.Text = $"Attribute data of the selected shapes (n={_currentMapLayer.SelectedIndexes.Length.ToString()})";
                                    //show attributes of selected shapes
                                    for (int r = 0; r < _currentMapLayer.SelectedIndexes.Length; r++)
                                    {
                                        row = new DataGridViewRow();
                                        row.CreateCells(gridAttributes);
                                        object[] arr = new object[sf.NumFields];

                                        for (int col = 0; col < sf.NumFields; col++)
                                        {
                                            try
                                            {
                                                arr[col] = sf.CellValue[col, _currentMapLayer.SelectedIndexes[r]].ToString();
                                            }
                                            catch
                                            {
                                                arr[col] = "";
                                            }
                                        }
                                        row.SetValues(arr);
                                        row.Tag = _currentMapLayer.SelectedIndexes[r];
                                        gridAttributes.Rows.Add(row);
                                    }
                                }
                            }
                        }
                    }
                    if (sf.NumSelected != 1)
                    {
                        foreach (DataGridViewRow r in gridAttributes.Rows)
                        {
                            r.HeaderCell.Value = String.Format("{0}", r.Index + 1);
                        }
                    }
                }

                gridAttributes.Visible = true;
            }
            else
            {
                labelShapeFileName.Text = "There is no selected layer";
                MessageBox.Show("There is no layer selected", "No layer selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OnLayerSelection(object sender, EventArgs e)
        {
            {
                DisplayAttributes();
            }
        }

        private void Cleanup()
        {
            _currentMapLayer = null;
            _parentForm = null;
            _instance = null;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            global.LoadFormSettings(this);
            gridAttributes.AutoGenerateColumns = false;
            DisplayAttributes();
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            MapInterActionHandler.MapLayersHandler.CurrentLayer -= OnCurrentLayer;
            MapInterActionHandler.Selection -= OnLayerSelection;
            global.SaveFormSettings(this);
            Cleanup();
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    break;

                case "btnCancel":
                    Close();
                    break;
            }
        }

        private void OnRowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (_sfSelectedCount != 1)
            {
                if (gridAttributes.Rows[e.RowIndex].Tag != null)
                {
                    MapInterActionHandler.SelectedShapeIndex = (int)gridAttributes.Rows[e.RowIndex].Tag;
                }
            }
        }

        private void OnGridRowPrepaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            int firstDisplayedCellIndex = gridAttributes.FirstDisplayedCell.RowIndex;
            int lastDisplayedCellIndex = firstDisplayedCellIndex + gridAttributes.DisplayedRowCount(true);

            Graphics graphics = gridAttributes.CreateGraphics();
            int measureFirstDisplayed = (int)(graphics.MeasureString(gridAttributes.Rows[firstDisplayedCellIndex].HeaderCell.Value.ToString(), gridAttributes.Font).Width);
            int measureLastDisplayed = (int)(graphics.MeasureString(gridAttributes.Rows[lastDisplayedCellIndex - 1].HeaderCell.Value.ToString(), gridAttributes.Font).Width);
            int rowHeaderWidth = System.Math.Max(measureFirstDisplayed, measureLastDisplayed);
            gridAttributes.RowHeadersWidth = rowHeaderWidth + 35;
        }

        private void OnCellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (_sfSelectedCount == 1)
            {
            }
            else
            {
                string fieldName = gridAttributes.Columns[e.ColumnIndex].Tag.ToString();
                _currentMapLayer.EditShapeFileField(fieldName, e.RowIndex, gridAttributes.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
            }
        }
    }
}
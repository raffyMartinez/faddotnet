using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using MapWinGIS;

namespace FAD3
{
    public partial class VisibilityQueryForm : Form
    {
        private static VisibilityQueryForm _instance;
        private Shapefile _shapeFile;
        private MapLayersHandler _layersHandler;
        private bool _selectionMode;
        public VisibilityExpressionTarget ExpressionTarget { get; set; }
        public string VisibilityExpression { get; set; }

        public static VisibilityQueryForm GetInstance(MapLayersHandler handler)
        {
            if (_instance == null) _instance = new VisibilityQueryForm(handler);
            return _instance;
        }

        public VisibilityQueryForm(MapLayersHandler handler)
        {
            InitializeComponent();
            _layersHandler = handler;
            _shapeFile = _layersHandler.CurrentMapLayer.LayerObject as Shapefile;
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnTest":
                    TestExpression();
                    break;

                case "btnClear":
                    textQuery.Clear();
                    break;

                case "btnShowValues":
                    break;

                case "btnCancel":
                    Close();
                    break;

                case "btnOk":
                    if (TestExpression())
                    {
                        _layersHandler.VisibilityExpression(textQuery.Text, ExpressionTarget);
                        Close();
                    }
                    break;
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            _instance = null;
        }

        private void GetFields()
        {
            for (int n = 0; n < _shapeFile.NumFields; n++)
            {
                listBoxFields.Items.Add(_shapeFile.Field[n].Name);
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            GetFields();
            textQuery.Text = VisibilityExpression;
        }

        private void OnQueryButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnLT":
                    textQuery.SelectedText = "< ";
                    break;

                case "btnLTE":
                    textQuery.SelectedText = "<= ";
                    break;

                case "btnGTE":
                    textQuery.SelectedText = ">= ";
                    break;

                case "btnGT":
                    textQuery.SelectedText = "> ";
                    break;

                case "btnE":
                    textQuery.SelectedText = "= ";
                    break;

                case "btnAnd":
                    textQuery.SelectedText = "AND ";
                    break;

                case "btnOr":
                    textQuery.SelectedText = "OR ";
                    break;

                case "btnNot":
                    textQuery.SelectedText = "NOT ";
                    break;

                case "btnNE":
                    textQuery.SelectedText = "<> ";
                    break;

                case "btnLP":
                    textQuery.SelectedText = "( ";
                    break;

                case "btnRP":
                    textQuery.SelectedText = " )";
                    break;
            }
        }

        private void OnlistBoxFields_Click(object sender, EventArgs e)
        {
            ShowValues(listBoxFields.SelectedIndex);
        }

        private void OnlistBoxFields_DoubleClick(object sender, EventArgs e)
        {
            var lb = (ListBox)sender;
            if (lb.SelectedIndex < 0) return;
            textQuery.SelectedText = "[" + listBoxFields.SelectedItem + "] ";
        }

        /// <summary>
        /// Showing values
        /// </summary>
        private void ShowValues(int FieldIndex)
        {
            dgvValues.Rows.Clear();
            if (_shapeFile.NumFields - 1 < FieldIndex)
            {
                return;
            }

            Table tbl = _shapeFile.Table;
            object obj = null;
            SortedDictionary<object, int> hashTable = new SortedDictionary<object, int>();

            bool isString = (_shapeFile.get_Field(FieldIndex).Type == FieldType.STRING_FIELD);

            if (true)
            {
                this.Cursor = Cursors.WaitCursor;

                for (int i = 0; i < tbl.NumRows; i++)
                {
                    obj = tbl.get_CellValue(FieldIndex, i);
                    if (hashTable.ContainsKey(obj))
                    {
                        hashTable[obj] += 1;
                    }
                    else
                    {
                        hashTable.Add(obj, 1);
                    }
                }
                int[] values = hashTable.Values.ToArray();
                object[] keys = hashTable.Keys.ToArray();

                dgvValues.Rows.Add(values.Length);
                for (int i = 0; i < values.Length; i++)
                {
                    if (isString)
                    {
                        dgvValues[1, i].Value = "\"" + keys[i].ToString() + "\"";
                    }
                    else
                    {
                        dgvValues[1, i].Value = keys[i].ToString();
                    }
                    dgvValues[0, i].Value = values[i];
                }

                this.Cursor = Cursors.Default;
            }
            else

            // field stats: aren't used currently
            {
                // for numeric fields we shall provide statistics
                dgvValues.Rows.Add(7);
                dgvValues[0, 0].Value = "Avg";
                dgvValues[0, 1].Value = "StDev";
                dgvValues[0, 2].Value = "0%";
                dgvValues[0, 3].Value = "25%";
                dgvValues[0, 4].Value = "50%";
                dgvValues[0, 5].Value = "75%";
                dgvValues[0, 6].Value = "100%";

                List<object> list = new List<object>();
                for (int i = 0; i < tbl.NumRows; i++)
                {
                    list.Add((object)tbl.get_CellValue(FieldIndex, i));
                }
                list.Sort();

                int quater = list.Count / 4;
                for (int i = 0; i < list.Count; i++)
                {
                    if (i == quater)
                    {
                        dgvValues[1, 3].Value = list[i];
                    }
                    else if (i == quater * 2)
                    {
                        dgvValues[1, 4].Value = list[i];
                    }
                    else if (i == quater * 3)
                    {
                        dgvValues[1, 5].Value = list[i];
                    }
                }

                dgvValues[1, 0].Value = (float)tbl.get_MeanValue(FieldIndex);
                dgvValues[1, 1].Value = (float)tbl.get_StandardDeviation(FieldIndex);
                dgvValues[1, 2].Value = tbl.get_MinValue(FieldIndex);
                dgvValues[1, 6].Value = tbl.get_MaxValue(FieldIndex);
            }

            dgvValues.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private bool TestExpression()
        {
            var success = false;
            Table tbl = _shapeFile.Table;
            if (textQuery.Text == string.Empty)
            {
                MessageBox.Show("No expression is entered", "MapWindow 4", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                object result = null;
                string err = string.Empty;

                //if (tbl.ParseExpression(richTextBox1.Text, ref err))
                if (tbl.Query(textQuery.Text, ref result, ref err))
                {
                    lblResult.ForeColor = Color.Green;
                    int[] arr = result as int[];
                    if (arr != null)
                    {
                        lblResult.Text = "Number of shapes = " + arr.Length.ToString();

                        // updating shapefile selection
                        if (_selectionMode)
                        {
                            ArrayList options = new ArrayList();
                            options.Add("1 - New selection");
                            options.Add("2 - Add to selection");
                            options.Add("3 - Exclude from selection");
                            options.Add("4 - Invert in selection");
                            string s = string.Format("Number of shapes = {0}. Choose the way to update selection", arr.Length);
                            //int option = MapWindow.Controls.Dialogs.ChooseOptions(options, 0, s, "Update selection");

                            // updating selection
                            //if (option != -1)
                            //{
                            //_mapWin.View.UpdateSelection(_layerHandle, ref arr, (SelectionOperation)option);
                            //_mapWin.View.Redraw();
                            //}
                        }
                    }
                    success = (arr != null && arr.Length > 0);
                }
                else
                {
                    if (err.ToLower() == "selection is empty")
                    {
                        lblResult.ForeColor = Color.Blue;
                        lblResult.Text = err;
                    }
                    else
                    {
                        lblResult.ForeColor = Color.Red;
                        lblResult.Text = err;
                    }
                }
            }
            return success;
        }

        private void OndgvValues_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            textQuery.SelectedText = dgvValues[1, e.RowIndex].Value.ToString() + " ";
        }
    }
}
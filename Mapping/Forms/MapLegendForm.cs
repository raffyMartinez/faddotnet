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

namespace FAD3.Mapping.Forms
{
    public partial class MapLegendForm : Form
    {
        private static MapLegendForm _instance;
        private DataGridViewImageColumn colSymbol;
        private DataGridViewTextBoxColumn colLayerName;
        private DataGridViewCheckBoxColumn colCheck;

        public static MapLegendForm GetInstance()
        {
            if (_instance == null) _instance = new MapLegendForm();
            return _instance;
        }

        public MapLayersHandler MapLayersHandler { get; set; }

        public MapLegendForm()
        {
            InitializeComponent();
        }

        private void SetUpLegend()
        {
            DataGridViewTextBoxColumn colCheck = new DataGridViewTextBoxColumn();
            colCheck.HeaderText = "";
            colCheck.Name = "colAddToLegend";
            colCheck.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            colCheck.DefaultCellStyle.NullValue = "";
            colCheck.DefaultCellStyle.Font = new Font("Wingdings 2", 10);
            dgLegend.Columns.Add(colCheck);

            colLayerName = new DataGridViewTextBoxColumn();
            colLayerName.HeaderText = "Layer";
            colLayerName.Name = "colLayerName";
            colLayerName.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgLegend.Columns.Add(colLayerName);

            colSymbol = new DataGridViewImageColumn();
            colSymbol.HeaderText = "Symbol";
            colSymbol.Name = "colSymbol";
            colSymbol.Width = 96;
            colSymbol.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colSymbol.DefaultCellStyle.NullValue = null;
            dgLegend.Columns.Add(colSymbol);

            dgLegend.RowHeadersVisible = false;
            dgLegend.AllowUserToAddRows = false;
            dgLegend.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgLegend.ColumnHeadersVisible = false;
            dgLegend.EditMode = DataGridViewEditMode.EditProgrammatically;

            dgLegend.DefaultCellStyle.SelectionBackColor = SystemColors.Window;
            dgLegend.DefaultCellStyle.SelectionForeColor = SystemColors.WindowText;
        }

        public void DrawLayers()
        {
            dgLegend.Rows.Clear();
            foreach (MapLayer layer in MapLayersHandler)
            {
                if (layer.Visible)
                {
                    if (layer.LayerType == "ShapefileClass")
                    {
                        Shapefile sf = layer.LayerObject as Shapefile;
                        if (sf.Categories.Count == 0)
                        {
                            PictureBox pic = new PictureBox
                            {
                                Height = dgLegend.RowTemplate.Height,
                                Width = colSymbol.Width,
                                Visible = false
                            };
                            MapLayersHandler.LayerSymbol(layer.Handle, pic, layer.LayerType);
                            dgLegend.Rows.Add(new object[] { "S", layer.Name, pic.Image });
                        }
                        else
                        {
                            dgLegend.Rows.Add(new object[] { "S", layer.Name, null });
                            foreach (var item in layer.ClassificationItems.Values)
                            {
                                PictureBox pic = new PictureBox
                                {
                                    Height = ((int)item.DrawingOptions.PointSize) * 2,
                                    Width = ((int)item.DrawingOptions.PointSize) * 2,
                                    Visible = false
                                };
                                MapLayersHandler.LayerSymbol(layer.Handle, pic, layer.LayerType, item.DrawingOptions);
                                int r = dgLegend.Rows.Add(new object[] { "", item.Caption, pic.Image });
                                if ((pic.Height / 2) > dgLegend.Rows[r].Height)
                                {
                                    dgLegend.Rows[r].Height = pic.Height / 2;
                                }
                                dgLegend.Rows[r].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                dgLegend.Rows[r].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                            }
                        }
                    }
                    else
                    {
                    }
                }
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            SetUpLegend();
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
        }

        private void OnCellClick(object sender, DataGridViewCellEventArgs e)
        {
            string cellContent = dgLegend.Rows[e.RowIndex].Cells[0].Value.ToString();
            if (e.ColumnIndex == 0
                && e.RowIndex >= 0
                && cellContent.Length > 0)
            {
                switch (cellContent)
                {
                    case "S":
                        cellContent = "£";
                        break;

                    case "£":
                        cellContent = "S";
                        break;
                }
                dgLegend.Rows[e.RowIndex].Cells[0].Value = cellContent;
            }
        }
    }
}
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
        private DataGridViewImageColumn _colSymbol;
        private DataGridViewTextBoxColumn _colLayerName;
        private DataGridViewTextBoxColumn _colCheck;
        private DataGridViewTextBoxColumn _colLayerHandle;
        private const string BOX_CHECKED = "S";
        private const string BOX_UNCHECKED = "£";
        private const int SYMBOL_COLUMN_WIDTH = 50;
        public Graticule Graticule { get; internal set; }
        private bool _mapHasLegendGraphic;

        public static MapLegendForm GetInstance(MapLayersHandler mapLayersHandler)
        {
            if (_instance == null) _instance = new MapLegendForm(mapLayersHandler);
            return _instance;
        }

        public MapLayersHandler MapLayersHandler { get; internal set; }

        public MapLegendForm(MapLayersHandler mapLayersHandler)
        {
            InitializeComponent();
            MapLayersHandler = mapLayersHandler;
            Graticule.GraticuleLoaded += OnGraticuleLoaded;
            Graticule.GraticuleUnloaded += OnGraticuleUnloaded;
        }

        private void OnGraticuleUnloaded(object sender, EventArgs e)
        {
            btnApplyLegend.Enabled = false;
            Graticule = null;
            global.MappingForm.MapLegend.RemoveLegend();
            _mapHasLegendGraphic = false;
        }

        private void OnGraticuleLoaded(object sender, EventArgs e)
        {
            if (Graticule == null)
            {
                Graticule = global.MappingForm.Graticule;
                Graticule.MapRedrawNeeded += OnGraticuleMapRedraw;
                btnApplyLegend.Enabled = Graticule != null && Graticule.GridVisible;
            }
        }

        private void OnGraticuleMapRedraw(object sender, EventArgs e)
        {
            btnApplyLegend.Enabled = Graticule != null && Graticule.GridVisible;
        }

        private void SetUpLegend()
        {
            _colCheck = new DataGridViewTextBoxColumn();
            _colCheck.HeaderText = "";
            _colCheck.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            _colCheck.DefaultCellStyle.NullValue = "";
            _colCheck.DefaultCellStyle.Font = new Font("Wingdings 2", 10);
            dgLegend.Columns.Add(_colCheck);

            _colLayerName = new DataGridViewTextBoxColumn();
            _colLayerName.HeaderText = "Layer";
            _colLayerName.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgLegend.Columns.Add(_colLayerName);

            _colSymbol = new DataGridViewImageColumn();
            _colSymbol.HeaderText = "Symbol";
            _colSymbol.Width = SYMBOL_COLUMN_WIDTH;
            _colSymbol.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            _colSymbol.DefaultCellStyle.NullValue = null;
            dgLegend.Columns.Add(_colSymbol);

            _colLayerHandle = new DataGridViewTextBoxColumn();
            _colLayerHandle.Visible = false;
            _colLayerHandle.DefaultCellStyle.NullValue = null;
            dgLegend.Columns.Add(_colLayerHandle);

            dgLegend.RowHeadersVisible = false;
            dgLegend.AllowUserToAddRows = false;
            dgLegend.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgLegend.ColumnHeadersVisible = false;
            dgLegend.EditMode = DataGridViewEditMode.EditProgrammatically;

            dgLegend.DefaultCellStyle.SelectionBackColor = SystemColors.Window;
            dgLegend.DefaultCellStyle.SelectionForeColor = SystemColors.WindowText;
        }

        public void DrawLegendLayers()
        {
            dgLegend.SuspendLayout();
            int row = 0;
            dgLegend.Rows.Clear();
            foreach (MapLayer layer in MapLayersHandler)
            {
                if (layer.Visible
                    && !layer.IsMaskLayer
                    && !layer.IsGraticule)
                {
                    if (layer.LayerType == "ShapefileClass")
                    {
                        Shapefile sf = layer.LayerObject as Shapefile;
                        if (sf.Categories.Count == 0)
                        {
                            PictureBox pic = new PictureBox
                            {
                                Height = dgLegend.RowTemplate.Height,
                                Width = SYMBOL_COLUMN_WIDTH,
                                Visible = false
                            };
                            MapLayersHandler.LayerSymbol(layer.Handle, pic, layer.LayerType);
                            row = 0;
                            dgLegend.Rows.Insert(row, new object[] { BOX_CHECKED, layer.Name, pic.Image, layer.Handle });
                        }
                        else
                        {
                            row = 0;
                            dgLegend.Rows.Insert(row, new object[] { BOX_CHECKED, layer.Name, null, layer.Handle });
                            foreach (var item in layer.ClassificationItems.Values)
                            {
                                PictureBox pic = new PictureBox
                                {
                                    Height = ((int)item.DrawingOptions.PointSize) * 2,
                                    Width = ((int)item.DrawingOptions.PointSize) * 2,
                                    Visible = false
                                };
                                MapLayersHandler.LayerSymbol(layer.Handle, pic, layer.LayerType, item.DrawingOptions);
                                row++;
                                dgLegend.Rows.Insert(row, new object[] { "", item.Caption, pic.Image, null });
                                if ((pic.Height / 2) > dgLegend.Rows[row].Height)
                                {
                                    dgLegend.Rows[row].Height = pic.Height / 2;
                                }
                                if ((pic.Width) > dgLegend.Columns[2].Width)
                                {
                                    dgLegend.Columns[2].Width = pic.Width;
                                }
                                dgLegend.Rows[row].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                dgLegend.Rows[row].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                            }
                        }
                    }
                    else
                    {
                    }
                }
            }
            dgLegend.ResumeLayout();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            global.LoadFormSettings(this);
            MapLayersHandler.LayerRefreshNeeded += OnLayerRefreshNeeded;
            MapLayersHandler.LayerRemoved += OnMapLayerRemoved;
            MapLayersHandler.MapRedrawNeeded += OnMapRedrawNeeded;
            MapLayersHandler.LayerRead += OnMapLayerRead;
            MapLayersHandler.LayerClassificationFinished += OnLayerClassificationFinished;
            MapLayersHandler.OnLayerVisibilityChanged += OnLayerVisibilityChanged;
            Graticule = global.MappingForm.Graticule;
            btnApplyLegend.Enabled = Graticule?.GridVisible == true;
            SetUpLegend();
        }

        private void OnMapLayerRead(MapLayersHandler s, LayerEventArg e)
        {
            DrawLegendLayers();
        }

        private void OnLayerRefreshNeeded(object sender, EventArgs e)
        {
            DrawLegendLayers();
        }

        private void OnLayerVisibilityChanged(MapLayersHandler s, LayerEventArg e)
        {
            DrawLegendLayers();
            if (_mapHasLegendGraphic)
            {
                ApplyLegend();
            }
        }

        private void OnLayerClassificationFinished(object sender, EventArgs e)
        {
            DrawLegendLayers();
        }

        private void OnMapRedrawNeeded(object sender, EventArgs e)
        {
            DrawLegendLayers();
        }

        private void OnMapLayerRemoved(MapLayersHandler s, LayerEventArg e)
        {
            DrawLegendLayers();
            if (_mapHasLegendGraphic)
            {
                ApplyLegend();
            }
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
            MapLayersHandler.LayerRemoved -= OnMapLayerRemoved;
            MapLayersHandler.MapRedrawNeeded -= OnMapRedrawNeeded;
            MapLayersHandler.LayerClassificationFinished -= OnLayerClassificationFinished;
            MapLayersHandler.OnLayerVisibilityChanged -= OnLayerVisibilityChanged;
            MapLayersHandler.LayerRefreshNeeded -= OnLayerRefreshNeeded;
            MapLayersHandler.LayerRead -= OnMapLayerRead;

            global.SaveFormSettings(this);
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
                    case BOX_CHECKED:
                        cellContent = BOX_UNCHECKED;
                        break;

                    case BOX_UNCHECKED:
                        cellContent = BOX_CHECKED;
                        break;
                }
                dgLegend.Rows[e.RowIndex].Cells[0].Value = cellContent;
                ApplyLegend();
            }
        }

        private void OnToolBarItemClick(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "btnClose":
                    Close();
                    break;

                case "btnApplyLegend":
                    _mapHasLegendGraphic = ApplyLegend();
                    break;
            }
        }

        private bool ApplyLegend()
        {
            IncludeLayersInLegend();
            string position = "";
            if (itemTopLeft.Checked)
            {
                position = "topLeft";
            }
            else if (itemTopRight.Checked)
            {
                position = "topRight";
            }
            else if (itemBottomRight.Checked)
            {
                position = "bottomRight";
            }
            else if (itemBottomLeft.Checked)
            {
                position = "bottomLeft";
            }
            global.MappingForm.MapLegend.LegendPositionInMap = position;
            return global.MappingForm.MapLegend.DrawLegend();
        }

        public void IncludeLayersInLegend()
        {
            foreach (MapLayer ly in MapLayersHandler)
            {
                ly.IncludeInLegend = false;
                foreach (DataGridViewRow r in dgLegend.Rows)
                {
                    if (r.Cells[0].Value?.ToString().Length > 0
                        && (int)r.Cells[3].Value == ly.Handle)
                    {
                        ly.IncludeInLegend = r.Cells[0].Value.ToString() == BOX_CHECKED;
                        break;
                    }
                }
            }
        }

        private void OnSelectPositionDropdownClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            foreach (ToolStripMenuItem item in btnSelectPosition.DropDown.Items)
            {
                item.Checked = false;
            }
            ((ToolStripMenuItem)(e.ClickedItem)).Checked = true;
            global.MappingForm.MapLegend.MapLegendPosition = Database.Classes.MapLegendPosition.PositionFromCorner;
        }
    }
}
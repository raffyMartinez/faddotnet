using FAD3.Database.Classes;
using MapWinGIS;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using FAD3.Mapping.Classes;
using FAD3.Mapping.Forms;

namespace FAD3
{
    /// <summary>
    /// shows layers that are on the map window and allows access to each layer's attributes and properties
    /// </summary>
    public partial class MapLayersForm : Form
    {
        private MapLayersHandler _mapLayersHandler;
        private static MapLayersForm _instance;
        private MapperForm _parentForm;

        private Rectangle _dragBoxFromMouseDown;
        private int _rowIndexFromMouseDown;
        private int _rowIndexOfItemUnderMouseToDrop;
        private int _layerRow;
        private int _layerCol;
        private int _layerHandle;

        private void OnLayerGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (_dragBoxFromMouseDown != Rectangle.Empty &&
                !_dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    // Proceed with the drag and drop, passing in the list item.
                    DragDropEffects dropEffect = layerGrid.DoDragDrop(
                          layerGrid.Rows[_rowIndexFromMouseDown],
                          DragDropEffects.Move);
                }
            }
        }

        public void ShapefileLayerPropertyChanged()
        {
            global.MappingForm.MapControl.Redraw();
            _mapLayersHandler.RefreshMap();
            RefreshLayerList();
        }

        private void OnLayerGrid_MouseDown(object sender, MouseEventArgs e)
        {
            // Get the index of the item the mouse is below.
            _rowIndexFromMouseDown = layerGrid.HitTest(e.X, e.Y).RowIndex;

            if (e.Button == MouseButtons.Right)
            {
                itemAddLayer.Visible = true;
                itemRemoveLayer.Visible = true;
                itemLayerProperty.Visible = true;
                if (_rowIndexFromMouseDown == -1)
                {
                    itemRemoveLayer.Visible = false;
                    itemLayerProperty.Visible = false;
                }
                menuLayers.Show(layerGrid, e.X, e.Y);
            }
            else if (e.Button == MouseButtons.Left && _rowIndexFromMouseDown != -1)
            {
                // Remember the point where the mouse down occurred.
                // The DragSize indicates the size that the mouse can move
                // before a drag event should be started.
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                _dragBoxFromMouseDown = new Rectangle(
                          new System.Drawing.Point(
                            e.X - (dragSize.Width / 2),
                            e.Y - (dragSize.Height / 2)),
                      dragSize);
            }
            else
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                _dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void OnLayerGrid_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void OnLayerGrid_DragDrop(object sender, DragEventArgs e)
        {
            int layerHandle = 0;

            // The mouse locations are relative to the screen, so they must be
            // converted to client coordinates.
            System.Drawing.Point clientPoint = layerGrid.PointToClient(new System.Drawing.Point(e.X, e.Y));

            // Get the row index of the item the mouse is below.
            _rowIndexOfItemUnderMouseToDrop = layerGrid.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            if (_rowIndexOfItemUnderMouseToDrop < 0)
            {
                e.Effect = DragDropEffects.None;
            }
            else
            {
                // If the drag operation was a move then remove and insert the row.
                if (e.Effect == DragDropEffects.Move)
                {
                    DataGridViewRow rowToMove = e.Data.GetData(typeof(DataGridViewRow)) as DataGridViewRow;
                    layerGrid.Rows.RemoveAt(_rowIndexFromMouseDown);
                    layerGrid.Rows.Insert(_rowIndexOfItemUnderMouseToDrop, rowToMove);

                    for (int row = 0; row < layerGrid.RowCount; row++)
                    {
                        if (row > 0)
                        {
                            layerHandle = (int)layerGrid[0, row].Tag;
                            var pos = _mapLayersHandler.get_LayerPosition(layerHandle);
                            _mapLayersHandler.MoveLayerBottom(pos);
                        }
                    }
                    _mapLayersHandler.RefreshMap();
                }
            }
        }

        public MapLayersHandler MapLayers
        {
            get { return _mapLayersHandler; }
        }

        public MapLayersForm(MapLayersHandler mapLayers, MapperForm parent)
        {
            InitializeComponent();
            _parentForm = parent;
            _mapLayersHandler = mapLayers;
            _mapLayersHandler.LayerRead += OnLayerRead;
            _mapLayersHandler.LayerRemoved += OnLayerDeleted;
            _mapLayersHandler.OnLayerNameUpdate += OnLayerNameUpdated;
            _mapLayersHandler.LayerRefreshNeeded += OnLayerRefreshRequest;
            layerGrid.CellClick += OnCellClick;
            layerGrid.CellMouseDown += OnCellMouseDown;
            layerGrid.CellDoubleClick += OnCellDoubleClick;
            layerGrid.DragDrop += OnLayerGrid_DragDrop;
            layerGrid.DragOver += OnLayerGrid_DragOver;
            layerGrid.MouseDown += OnLayerGrid_MouseDown;
            layerGrid.MouseMove += OnLayerGrid_MouseMove;
        }

        private void OnLayerRefreshRequest(object sender, EventArgs e)
        {
            RefreshLayerList();
        }

        private void OnLayerNameUpdated(MapLayersHandler s, LayerEventArg e)
        {
            layerGrid[_layerCol, _layerRow].Value = e.LayerName;
        }

        private void OnCellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                menuLayers.Show(layerGrid, e.X, e.Y);
                _rowIndexFromMouseDown = e.RowIndex;
            }
        }

        private void OnCellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //we only respond to double-click on the name column
            if (e.ColumnIndex == 1)
            {
                var lpf = LayerPropertyForm.GetInstance(this, (int)layerGrid[0, _rowIndexFromMouseDown].Tag);
                if (!lpf.Visible)
                {
                    lpf.Show(this);
                }
                else
                {
                    lpf.BringToFront();
                }
            }
        }

        private void OnLayerDeleted(MapLayersHandler s, LayerEventArg e)
        {
            for (int n = 0; n < layerGrid.Rows.Count; n++)
            {
                if ((int)layerGrid[0, n].Tag == e.LayerHandle)
                {
                    layerGrid.Rows.Remove(layerGrid.Rows[n]);
                }
            }
        }

        /// <summary>
        /// handles the event when a new visible layer is added into the map
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="e"></param>
        private void OnLayerRead(MapLayersHandler layer, LayerEventArg e)
        {
            if (e.ShowInLayerUI)
            {
                //set the layer preview thumbnail
                PictureBox pic = new PictureBox
                {
                    Height = layerGrid.RowTemplate.Height,
                    Width = layerGrid.Columns[2].Width,
                    Visible = false
                };
                _mapLayersHandler.LayerSymbol(e.LayerHandle, pic, e.LayerType);

                //we always insert a new layer in the first row of the dataGrid
                layerGrid.Rows.Insert(0, new object[] { e.LayerVisible, e.LayerName, pic.Image });

                //we assign the layerhandle to the tag of cell 0,0
                layerGrid[0, 0].Tag = e.LayerHandle;

                //symbolize the current layer by making it bold font
                MarkCurrentLayerName(CurrentLayerRow());

            }
        }

        private int CurrentLayerRow()
        {
            int currentLayerHandle = _mapLayersHandler.CurrentMapLayer.Handle;
            foreach(DataGridViewRow item in layerGrid.Rows)
            {
                if((int)item.Cells[0].Tag==currentLayerHandle)
                {
                    return item.Index;
                }

            }
            return 0;
        }

        public static MapLayersForm GetInstance(MapLayersHandler mapLayers, MapperForm parent)
        {
            if (_instance == null) _instance = new MapLayersForm(mapLayers, parent);
            return _instance;
        }

        private void OnMapLayersForm_Load(object sender, EventArgs e)
        {
            global.LoadFormSettings(this);
            _mapLayersHandler.ReadLayers();

            layerGrid.DefaultCellStyle.SelectionBackColor = SystemColors.Window;
            layerGrid.DefaultCellStyle.SelectionForeColor = SystemColors.WindowText;
            itemAlwaysKeepOnTop.Visible = global.MappingMode == fad3MappingMode.grid25Mode;
        }

        private void MapLayersForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
            _mapLayersHandler.LayerRead -= OnLayerRead;
            global.SaveFormSettings(this);
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonOK":
                    Close();
                    break;
            }
        }

        private void MapLayersForm_Resize(object sender, EventArgs e)
        {
            var gridWidth = layerGrid.Width;
            var col0Width = layerGrid.Columns[0].Width;
            var col2Width = layerGrid.Columns[2].Width;
            layerGrid.Columns[1].Width = gridWidth - 3 - (col0Width + col2Width);
        }

        private void OnCellClick(object sender, DataGridViewCellEventArgs e)
        {
            _layerHandle = (int)layerGrid[0, e.RowIndex].Tag;
            if (e.ColumnIndex == 0)
            {
                bool isVisible = (bool)layerGrid[0, e.RowIndex].Value;
                layerGrid[0, e.RowIndex].Value = !isVisible;
                var layerName = layerGrid[e.ColumnIndex + 1, e.RowIndex].Value.ToString();
                _mapLayersHandler.EditLayer(_layerHandle, layerName, !isVisible);
            }

            if (e.ColumnIndex == 1)
            {
                MarkCurrentLayerName(e.RowIndex);
                _mapLayersHandler.set_MapLayer(_layerHandle);
                itemConvertToGrid25.Enabled = false;
                if (global.MappingMode == fad3MappingMode.grid25Mode)
                {
                    itemAlwaysKeepOnTop.Checked = _mapLayersHandler.CurrentMapLayer.KeepOnTop;
                    if (_mapLayersHandler.CurrentMapLayer.LayerType == "ShapefileClass")
                    {
                        var sf = _mapLayersHandler.CurrentMapLayer.LayerObject as Shapefile;
                        itemConvertToGrid25.Enabled = sf.ShapefileType == ShpfileType.SHP_POINT;
                    }
                }
            }
        }

        /// <summary>
        /// makes the layer name on the clicked row bold and makes the rest of the names not bold
        /// </summary>
        /// <param name="currentRow"></param>
        private void MarkCurrentLayerName(int currentRow)
        {
            foreach (DataGridViewRow row in layerGrid.Rows)
            {
                row.Cells[1].Style.Font = new Font(Font.FontFamily.Name, Font.Size, FontStyle.Regular);
            }
            layerGrid[1, currentRow].Style.Font = new Font(Font.FontFamily.Name, Font.Size, FontStyle.Bold);
        }

        /// <summary>
        /// event handler an item in the shortcut menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMenuLayers_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            e.ClickedItem.Owner.Hide();
            switch (e.ClickedItem.Name)
            {
                case "itemAttributes":
                    var sfa = ShapefileAttributesForm.GetInstance(global.MappingForm, global.MappingForm.MapInterActionHandler);
                    if (!sfa.Visible)
                    {
                        sfa.Show(this);
                    }
                    else
                    {
                        sfa.BringToFront();
                    }
                    break;

                case "itemAddLayer":
                     if( _parentForm.OpenFileDialog()==DialogResult.OK)
                    {

                    }
                    break;

                case "itemRemoveLayer":
                    MapLayers.RemoveLayer((int)layerGrid[0, _rowIndexFromMouseDown].Tag);
                    break;

                case "itemLayerProperty":
                    var lpf = LayerPropertyForm.GetInstance(this, (int)layerGrid[0, _rowIndexFromMouseDown].Tag);
                    if (!lpf.Visible)
                    {
                        lpf.Show(this);
                    }
                    else
                    {
                        lpf.BringToFront();
                    }
                    break;

                case "itemLayerExport":
                    switch (_mapLayersHandler.CurrentMapLayer.LayerType)
                    {
                        case "ShapefileClass":
                            if (!_mapLayersHandler.CurrentMapLayer.IsFishingGridLayoutTemplate)
                            {
                                SaveTemplateToFile();
                            }
                            break;

                        case "ImageClass":
                            break;
                    }
                    break;
            }
        }

        public void SaveTemplateToFile()
        {
            var saveAs = new SaveFileDialog();
            saveAs.Filter = "Shapefile *.shp|*.shp|All files *.*|*.*";
            saveAs.FilterIndex = 1;
            saveAs.FileName = $"{_mapLayersHandler.CurrentMapLayer.Name}_template.shp";
            DialogResult dr = saveAs.ShowDialog();
            if (dr == DialogResult.OK)
            {
                _parentForm.Grid25MajorGrid.LayoutHelper.SaveLayoutTemplate(saveAs.FileName);
            }
        }

        private void OnoptionsToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            e.ClickedItem.OwnerItem.Owner.Hide();
            switch (e.ClickedItem.Name)
            {
                case "itemConvertToGrid25":
                    var cgf = ConvertToGrid25Form.GetInstance();
                    cgf.MapLayer = _mapLayersHandler.CurrentMapLayer;
                    if (!cgf.Visible)
                    {
                        cgf.Show(this);
                    }
                    else
                    {
                        cgf.BringToFront();
                    }

                    break;

                case "itemAlwaysKeepOnTop":
                    ToolStripMenuItem tsmi = (ToolStripMenuItem)e.ClickedItem;
                    tsmi.Checked = !tsmi.Checked;
                    _mapLayersHandler.CurrentMapLayer.KeepOnTop = tsmi.Checked;
                    break;
            }
        }

        private void OnToolbarItemClick(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "buttonAddLayer":
                    if(_parentForm.OpenFileDialog()==DialogResult.OK)
                    {
                        RefreshLayerList();
                    }
                    break;

                case "buttonRemoveLayer":
                    MapLayers.RemoveLayer((int)layerGrid[0, _rowIndexFromMouseDown].Tag);
                    break;

                case "buttonAttributes":
                    if (_mapLayersHandler.CurrentMapLayer != null)
                    {
                        EditShapeAttributeForm esaf = EditShapeAttributeForm.GetInstance(global.MappingForm, global.MappingForm.MapInterActionHandler);
                        if (esaf.Visible)
                        {
                            esaf.BringToFront();
                        }
                        else
                        {
                            esaf.Show(this);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select a layer", "No selected layer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

                case "buttonZoomToLayer":
                    _mapLayersHandler.ZoomToLayer(_layerHandle);
                    break;

                case "buttonClose":
                    Close();
                    break;
            }
        }

        public void RefreshLayerList()
        {
            layerGrid.Rows.Clear();
            _mapLayersHandler.ReadLayers();
            MarkCurrentLayerName(CurrentLayerRow());
        }

        private void OnLayerMoveDropDownClick(object sender, ToolStripItemClickedEventArgs e)
        {
            e.ClickedItem.OwnerItem.Owner.Hide();
            switch (e.ClickedItem.Name)
            {
                case "itemMoveTop":
                    _mapLayersHandler.MoveToTop();
                    break;

                case "itemMoveBottom":
                    _mapLayersHandler.MoveToBottom();
                    break;

                case "itemMoveUp":
                    _mapLayersHandler.MoveUp();
                    break;

                case "itemMoveDown":
                    _mapLayersHandler.MoveDown();
                    break;
            }
            RefreshLayerList();
        }

        private void onCheckStateChange(object sender, EventArgs e)
        {
        }
    }
}
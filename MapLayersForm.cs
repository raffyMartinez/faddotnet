using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FAD3
{
    public partial class MapLayersForm : Form
    {
        private MapLayers _mapLayers;
        private static MapLayersForm _instance;
        private MapForm _parentForm;

        private Rectangle _dragBoxFromMouseDown;
        private int _rowIndexFromMouseDown;
        private int _rowIndexOfItemUnderMouseToDrop;

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

        private void OnLayerGrid_MouseDown(object sender, MouseEventArgs e)
        {
            // Get the index of the item the mouse is below.
            _rowIndexFromMouseDown = layerGrid.HitTest(e.X, e.Y).RowIndex;

            if (_rowIndexFromMouseDown != -1)
            {
                // Remember the point where the mouse down occurred.
                // The DragSize indicates the size that the mouse can move
                // before a drag event should be started.
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                _dragBoxFromMouseDown = new Rectangle(
                          new Point(
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
            Point clientPoint = layerGrid.PointToClient(new Point(e.X, e.Y));

            // Get the row index of the item the mouse is below.
            _rowIndexOfItemUnderMouseToDrop = layerGrid.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            if (_rowIndexOfItemUnderMouseToDrop < 0)
                e.Effect = DragDropEffects.None;
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
                            var pos = _mapLayers.get_LayerPosition(layerHandle);
                            _mapLayers.MoveLayerBottom(pos);
                        }
                    }
                }
            }
        }

        public MapLayers mapLayers
        {
            get { return _mapLayers; }
        }

        public MapLayersForm(MapLayers mapLayers, MapForm parent)
        {
            InitializeComponent();
            _parentForm = parent;
            _mapLayers = mapLayers;
            _mapLayers.LayerPropertyRead += OnLayerPropertyRead;
            _mapLayers.LayerDeleted += OnLayerDeleted;
            layerGrid.CellClick += OnCellClick;
            layerGrid.CellDoubleClick += OnCellDoubleClick;
            layerGrid.DragDrop += OnLayerGrid_DragDrop;
            layerGrid.DragOver += OnLayerGrid_DragOver;
            layerGrid.MouseDown += OnLayerGrid_MouseDown;
            layerGrid.MouseMove += OnLayerGrid_MouseMove;
        }

        private void OnCellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //we only respond to double-click on the name column
            if (e.ColumnIndex == 1)
            {
                var hLyr = (int)layerGrid[0, e.RowIndex].Tag;
                var lpf = LayerPropertyForm.GetInstance(this, hLyr);
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

        private void OnLayerDeleted(MapLayers s, LayerProperty e)
        {
            for (int n = 0; n < layerGrid.Rows.Count; n++)
            {
                if ((int)layerGrid[0, n].Tag == e.LayerHandle)
                {
                    layerGrid.Rows.Remove(layerGrid.Rows[n]);
                }
            }
        }

        private void OnLayerPropertyRead(MapLayers layer, LayerProperty e)
        {
            if (e.ShowInLayerUI)
            {
                PictureBox pic = new PictureBox
                {
                    Height = layerGrid.RowTemplate.Height,
                    Width = layerGrid.Columns[2].Width,
                    Visible = false
                };

                _mapLayers.layerSymbol(e.LayerHandle, pic, e.LayerType);
                layerGrid.Rows.Insert(0, new object[] { e.LayerVisible, e.LayerName, pic.Image });
                layerGrid[0, 0].Tag = e.LayerHandle;
                MarkCurrentLayerName(0);
            }
        }

        public static MapLayersForm GetInstance(MapLayers mapLayers, MapForm parent)
        {
            if (_instance == null) _instance = new MapLayersForm(mapLayers, parent);
            return _instance;
        }

        private void OnMapLayersForm_Load(object sender, EventArgs e)
        {
            global.LoadFormSettings(this);
            _mapLayers.ReadLayers();
            layerGrid.DefaultCellStyle.SelectionBackColor = SystemColors.Window;
            layerGrid.DefaultCellStyle.SelectionForeColor = SystemColors.WindowText;
        }

        private void MapLayersForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
            _mapLayers.LayerPropertyRead -= OnLayerPropertyRead;
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
            if (e.ColumnIndex == 0)
            {
                bool isVisible = (bool)layerGrid[0, e.RowIndex].Value;
                layerGrid[0, e.RowIndex].Value = !isVisible;
                var h = (int)layerGrid[0, e.RowIndex].Tag;
                var layerName = layerGrid[e.ColumnIndex + 1, e.RowIndex].Value.ToString();
                _mapLayers.EditLayer(h, layerName, !isVisible);
            }
            if (e.ColumnIndex == 1)
            {
                MarkCurrentLayerName(e.RowIndex);
            }
        }

        private void MarkCurrentLayerName(int currentRow)
        {
            foreach (DataGridViewRow row in layerGrid.Rows)
            {
                row.Cells[1].Style.Font = new Font(Font.FontFamily.Name, Font.Size, FontStyle.Regular);
            }
            layerGrid[1, currentRow].Style.Font = new Font(Font.FontFamily.Name, Font.Size, FontStyle.Bold);
        }
    }
}
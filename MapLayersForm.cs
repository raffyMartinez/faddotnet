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

        public MapLayersForm(MapLayers mapLayers)
        {
            InitializeComponent();
            _mapLayers = mapLayers;
            _mapLayers.LayerPropertyRead += OnLayerPropertyRead;
            _mapLayers.LayerDeleted += OnLayerDeleted;
            layerGrid.CellClick += OnCellClick;
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

        private void OnLayerPropertyRead(MapLayers s, LayerProperty e)
        {
            if (e.ShowInLayerUI)
            {
                layerGrid.Rows.Insert(0, new object[] { e.LayerVisible, e.LayerName, null });
                layerGrid[0, 0].Tag = e.LayerHandle;
            }
        }

        public static MapLayersForm GetInstance(MapLayers mapLayers)
        {
            if (_instance == null) _instance = new MapLayersForm(mapLayers);
            return _instance;
        }

        private void OnMapLayersForm_Load(object sender, EventArgs e)
        {
            global.LoadFormSettings(this);
            _mapLayers.ReadLayers();
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
        }
    }
}
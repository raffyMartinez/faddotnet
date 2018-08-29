using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MapWinGIS;

namespace FAD3
{
    public partial class LayerPropertyForm : Form
    {
        private static LayerPropertyForm _instance;
        private MapLayersForm _parentForm;
        private int _layerHandle;
        private MapLayer _mapLayer;
        private string _layerType;
        private Shapefile _shapefileLayer;
        private string _layerName;
        private MapLayers _mapLayers;

        public LayerPropertyForm(MapLayersForm parent, int layerHandle)
        {
            InitializeComponent();
            _parentForm = parent;
            _layerHandle = layerHandle;
            _mapLayers = _parentForm.mapLayers;
            _mapLayer = _parentForm.mapLayers.get_MapLayer(_layerHandle);
        }

        private void ProcessMapLayer(MapLayer mapLayer)
        {
            txtGeoProjection.Text = mapLayer.GeoProjectionName;
            txtLayerType.Text = mapLayer.LayerType;
            txtFileName.Text = mapLayer.FileName;
            txtLayerName.Text = mapLayer.Name;
        }

        public static LayerPropertyForm GetInstance(MapLayersForm parent, int layerHandle)
        {
            if (_instance == null) _instance = new LayerPropertyForm(parent, layerHandle);
            return _instance;
        }

        private void LayerPropertyForm_Load(object sender, EventArgs e)
        {
            global.LoadFormSettings(this);

            ProcessMapLayer(_mapLayer);
        }

        private void LayerPropertyForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
            global.SaveFormSettings(this);
        }
    }
}
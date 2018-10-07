using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MapWinGIS;
using FAD3.Mapping.Forms;

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
        private MapLayersHandler _mapLayers;

        public MapLayersForm Parentform
        {
            get { return _parentForm; }
        }

        public LayerPropertyForm(MapLayersForm parent, int layerHandle)
        {
            InitializeComponent();
            _parentForm = parent;
            _layerHandle = layerHandle;
            _mapLayers = _parentForm.MapLayers;
            _mapLayers.OnVisibilityExpressionSet += OnVisibilityExpression;
            _mapLayer = _parentForm.MapLayers.get_MapLayer(_layerHandle);
            _shapefileLayer = _mapLayer.LayerObject as Shapefile;
        }

        private void OnVisibilityExpression(MapLayersHandler s, LayerEventArg e)
        {
            txtVisibilityExpression.Text = e.VisibilityExpression;
        }

        private void ProcessMapLayer(MapLayer mapLayer)
        {
            txtGeoProjection.Text = mapLayer.GeoProjectionName;
            txtLayerType.Text = mapLayer.LayerType;
            txtFileName.Text = mapLayer.FileName;
            txtLayerName.Text = mapLayer.Name;
            txtVisibilityExpression.Text = ((Shapefile)mapLayer.LayerObject).VisibilityExpression;
        }

        public static LayerPropertyForm GetInstance(MapLayersForm parent, int layerHandle)
        {
            if (_instance == null) _instance = new LayerPropertyForm(parent, layerHandle);
            return _instance;
        }

        private void LayerPropertyForm_Load(object sender, EventArgs e)
        {
            global.LoadFormSettings(this, true);

            ProcessMapLayer(_mapLayer);
            transpSelection.FixLayout();
        }

        private void LayerPropertyForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
            global.SaveFormSettings(this);
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnLabelFeatures":
                    var labelsForm = LabelsForm.GetInstance(_mapLayers);
                    if (!labelsForm.Visible)
                    {
                        labelsForm.Show(this);
                    }
                    else
                    {
                        labelsForm.BringToFront();
                    }
                    break;

                case "btnLabelCategories":
                    break;

                case "btnFeatureSymbols":
                    switch (_shapefileLayer.ShapefileType)
                    {
                        case ShpfileType.SHP_POINT:
                            var pointSymbologyForm = PointLayerSymbologyForm.GetInstance(this, _mapLayer);
                            if (pointSymbologyForm.Visible)
                            {
                                pointSymbologyForm.BringToFront();
                            }
                            else
                            {
                                pointSymbologyForm.Show(this);
                            }
                            break;

                        case ShpfileType.SHP_POLYGON:
                            var polygonForm = PolygonLayerSymbologyForm.GetInstance(this, _mapLayer);
                            if (polygonForm.Visible)
                            {
                                polygonForm.BringToFront();
                            }
                            else
                            {
                                polygonForm.Show(this);
                            }
                            break;

                        case ShpfileType.SHP_POLYLINE:
                            break;
                    }

                    break;

                case "btnFeatureCategories":
                    break;

                case "btnClose":
                    Close();
                    break;

                case "btnDefineVisibilityExpression":
                    var visibilityQueryForm = VisibilityQueryForm.GetInstance(_mapLayers);
                    visibilityQueryForm.VisibilityExpression = txtVisibilityExpression.Text;
                    visibilityQueryForm.ExpressionTarget = VisibilityExpressionTarget.ExpressionTargetShape;
                    if (!visibilityQueryForm.Visible)
                    {
                        visibilityQueryForm.Show(this);
                    }
                    else
                    {
                        visibilityQueryForm.BringToFront();
                    }
                    break;

                case "btnApplyVisibility":
                    _shapefileLayer.VisibilityExpression = _mapLayer.ShapesVisibilityExpression;
                    global.MappingForm.MapControl.Redraw();
                    break;
            }
        }
    }
}
using FAD3.Mapping.Forms;
using MapWinGIS;
using System;
using System.Windows.Forms;
using System.IO;

namespace FAD3
{
    /// <summary>
    /// access the properties of a shapefile layer
    /// labels
    /// symbols
    /// visibility query
    /// </summary>
    public partial class LayerPropertyForm : Form
    {
        private static LayerPropertyForm _instance;
        private MapLayersForm _parentForm;
        private int _layerHandle;
        private MapLayer _mapLayer;
        private string _layerType;
        private Shapefile _shapefileLayer;
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
            if (mapLayer.LayerType == "ShapefileClass")
            {
                txtVisibilityExpression.Text = ((Shapefile)mapLayer.LayerObject).VisibilityExpression;
            }
        }

        public static LayerPropertyForm GetInstance(MapLayersForm parent, int layerHandle)
        {
            if (_instance == null) _instance = new LayerPropertyForm(parent, layerHandle);
            return _instance;
        }

        private void OnLayerPropertyForm_Load(object sender, EventArgs e)
        {
            global.LoadFormSettings(this, true);

            ProcessMapLayer(_mapLayer);
            transpSelection.FixLayout();
            btnApply.Enabled = false;
            lvLayerProps.View = View.Details;
            lvLayerProps.Columns.Clear();
            lvLayerProps.Columns.Add("Property");
            lvLayerProps.Columns.Add("Value");
            lvLayerProps.FullRowSelect = true;
            SizeColumns();

            ListViewItem lvi = lvLayerProps.Items.Add("Number of shapes");
            lvi.SubItems.Add(_shapefileLayer.NumShapes.ToString());

            lvi = lvLayerProps.Items.Add("Extents minimum xy");
            lvi.SubItems.Add($"{_shapefileLayer.Extents.xMin.ToString()}, {_shapefileLayer.Extents.yMin.ToString()}");

            lvi = lvLayerProps.Items.Add("Extents maximum xy");
            lvi.SubItems.Add($"{_shapefileLayer.Extents.xMax.ToString()}, {_shapefileLayer.Extents.yMax.ToString()}");

            lvi = lvLayerProps.Items.Add("Centroid xy");
            lvi.SubItems.Add($"{_shapefileLayer.Extents.Center.x.ToString()}, {_shapefileLayer.Extents.Center.y.ToString()}");
            SizeColumns(false);
        }

        private void SizeColumns(bool init = true)
        {
            foreach (ColumnHeader c in lvLayerProps.Columns)
            {
                if (init)
                {
                    c.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                    c.Tag = c.Width;
                }
                else
                {
                    c.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                    c.Width = c.Width > (int)c.Tag ? c.Width : (int)c.Tag;
                }
            }
        }

        private void OnLayerPropertyForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
            global.SaveFormSettings(this);
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnClear":
                    txtVisibilityExpression.Text = "";
                    _shapefileLayer.VisibilityExpression = "";
                    global.MappingForm.MapControl.Redraw();
                    break;

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
                        case ShpfileType.SHP_POLYLINE:
                            var symbologyForm = PolygonLineLayerSymbologyForm.GetInstance(this, _mapLayer);
                            if (symbologyForm.Visible)
                            {
                                symbologyForm.BringToFront();
                            }
                            else
                            {
                                symbologyForm.Show(this);
                            }
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
                    if (txtVisibilityExpression.Text.Length == 0)
                    {
                        _shapefileLayer.VisibilityExpression = "";
                    }
                    else
                    {
                        _shapefileLayer.VisibilityExpression = _mapLayer.ShapesVisibilityExpression;
                    }
                    global.MappingForm.MapControl.Redraw();
                    break;

                case "btnApply":
                    _mapLayers.UpdateCurrentLayerName(txtLayerName.Text);
                    btnApply.Enabled = false;
                    break;
            }
        }

        private void OntxtLayerName_TextChanged(object sender, EventArgs e)
        {
            btnApply.Enabled = true;
        }

        private void OnMenuItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            e.ClickedItem.Owner.Hide();
            switch (e.ClickedItem.Name)
            {
                case "menuSavePropsToFile":
                    var sfd = new SaveFileDialog();
                    sfd.Title = "Save layer propeties to text file";
                    sfd.Filter = "Text file|*.txt|All files|*.*";
                    sfd.FilterIndex = 1;
                    sfd.FileName = $"{_mapLayer.Name}_properties.txt";
                    sfd.ShowDialog();
                    if (sfd.FileName.Length > 0)
                    {
                        try
                        {
                            using (StreamWriter writer = new StreamWriter(sfd.FileName, false))
                            {
                                writer.WriteLine($"Layer name: {_mapLayer.Name}");
                                writer.WriteLine($"Projection: {_mapLayer.GeoProjectionName}");
                                writer.WriteLine($"Layer type: {_mapLayer.LayerType}");
                                writer.WriteLine($"Filename: {_mapLayer.FileName}");
                                foreach (ListViewItem lvi in lvLayerProps.Items)
                                {
                                    writer.WriteLine($"{lvi.Text}: {lvi.SubItems[1].Text}");
                                }
                            }
                        }
                        catch (IOException ioex)
                        {
                            MessageBox.Show($"{ioex.Message} Try using another filename", "IO exception error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(ex.Message, "LayerPropertyForm.cs", "OnMenuItemClicked.menuSavePropsToFile");
                        }
                    }
                    break;
            }
        }
    }
}
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
    public partial class MapperForm : Form
    {
        private static MapperForm _instance;
        private Grid25MajorGrid _grid25MajorGrid;                                   //handles grid25 fishing ground grid maps
        private MapInterActionHandler _mapInterActionHandler;                       //handles interaction with the map control
        private MapLayersHandler _mapLayersHandler;                                 //handles map layers
        private Form _parentForm;
        private MapLayer _currentMapLayer;

        public MapInterActionHandler MapInterActionHandler
        {
            get { return _mapInterActionHandler; }
        }

        public static MapperForm GetInstance(Form parentForm)
        {
            if (_instance == null) _instance = new MapperForm(parentForm);
            return _instance;
        }

        private void CleanUp()
        {
            if (_grid25MajorGrid != null)
            {
                _grid25MajorGrid.Dispose();
                _grid25MajorGrid = null;

                _mapLayersHandler.Dispose();
                _mapInterActionHandler.Dispose();
            }
        }

        public MapLayer CurrentMapLayer
        {
            get { return _currentMapLayer; }
        }

        public Grid25MajorGrid grid25MajorGrid
        {
            get { return _grid25MajorGrid; }
        }

        public void createGrid25MajorGrid(FishingGrid.fadUTMZone UTMZone)
        {
            _grid25MajorGrid = new Grid25MajorGrid(axMap);
            _grid25MajorGrid.UTMZone = UTMZone;
            _grid25MajorGrid.MapLayers = _mapLayersHandler;
            _grid25MajorGrid.MapInterActionHandler = _mapInterActionHandler;
            axMap.GeoProjection.SetWgs84Projection(_grid25MajorGrid.Grid25Geoprojection);
            axMap.MapUnits = tkUnitsOfMeasure.umMeters;

            _mapLayersHandler.AddLayer(grid25MajorGrid.Grid25Grid, "Grid25", true, true);
        }

        public MapperForm(Form parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;
        }

        private void OnMapperForm_Load(object sender, EventArgs e)
        {
            toolstripToolBar.ClickThrough = true;
            Text = "Map";
            global.MappingForm = this;
            global.LoadFormSettings(this);
            _mapLayersHandler = new MapLayersHandler(axMap);
            _mapLayersHandler.CurrentLayer += OnCurrentMapLayer;
            _mapInterActionHandler = new MapInterActionHandler(axMap, _mapLayersHandler);
            SetCursorToSelect();
        }

        private void OnCurrentMapLayer(MapLayersHandler s, LayerProperty e)
        {
            _currentMapLayer = _mapLayersHandler.get_MapLayer(e.LayerHandle);
        }

        public void SetCursorToSelect()
        {
            axMap.MapCursor = tkCursor.crsrUserDefined;
            axMap.UDCursorHandle = (int)((Bitmap)ilCursors.Images["arrow32"]).GetHicon();
            axMap.CursorMode = tkCursorMode.cmSelection;
        }

        private void OnMapperForm_Closed(object sender, FormClosedEventArgs e)
        {
            global.MappingForm = null;
            CleanUp();

            _instance = null;
            global.SaveFormSettings(this);
        }

        private void OpenFileDialog()
        {
            string filename = "";
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Open a GIS layer",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            var initialDirectory = RegistryTools.GetSetting("FAD3", "LastOpenedLayerDirectory", "");
            if (initialDirectory.ToString().Length > 0)
            {
                ofd.InitialDirectory = initialDirectory.ToString();
            }

            ofd.Filter = "ESRI Shapefile (shp)|*.shp|" +
                           "KML files (kml)|*.kml|" +
                           "Georeferenced raster files (jpg, tiff,bmp)|*.jpg;*.tif;*.tiff;*.bmp|" +
                           "Other files |*.*)";
            ofd.FilterIndex = 1;
            ofd.ShowDialog();
            filename = ofd.FileName;
            if (filename != "")
            {
                var (success, errMsg) = _mapLayersHandler.FileOpenHandler(filename);
                if (!success)
                {
                    MessageBox.Show(errMsg, "Error in opening file", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void OnToolbarClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            axMap.UDCursorHandle = -1;
            ToolStripItem tsi = e.ClickedItem;
            switch (tsi.Name)
            {
                case "tsButtonLayers":
                    var mlf = MapLayersForm.GetInstance(_mapLayersHandler, this);
                    if (!mlf.Visible)
                    {
                        mlf.Show(this);
                    }
                    else
                    {
                        mlf.BringToFront();
                        mlf.Focus();
                    }
                    break;

                case "tsButtonLayerAdd":
                    OpenFileDialog();
                    break;

                case "tsButtonAttributes":
                    var sfa = ShapefileAttributesForm.GetInstance(this, _mapInterActionHandler);
                    if (!sfa.Visible)
                    {
                        sfa.Show(this);
                    }
                    else
                    {
                        sfa.BringToFront();
                    }
                    break;

                case "tsButtonZoomIn":
                    axMap.MapCursor = tkCursor.crsrUserDefined;
                    axMap.UDCursorHandle = (int)((Bitmap)e.ClickedItem.Image).GetHicon();
                    axMap.CursorMode = tkCursorMode.cmZoomIn;
                    break;

                case "tsButtonZoomOut":
                    axMap.MapCursor = tkCursor.crsrUserDefined;
                    axMap.UDCursorHandle = (int)((Bitmap)e.ClickedItem.Image).GetHicon();
                    axMap.CursorMode = tkCursorMode.cmZoomOut;
                    break;

                case "tsButtonZoomAll":
                    axMap.ZoomToMaxExtents();
                    break;

                case "tsButtonFitMap":
                    break;

                case "tsButtonZoomPrevious":
                    axMap.ZoomToPrev();
                    break;

                case "tsButtonPan":
                    axMap.MapCursor = tkCursor.crsrUserDefined;
                    axMap.UDCursorHandle = (int)((Bitmap)e.ClickedItem.Image).GetHicon();
                    axMap.CursorMode = tkCursorMode.cmPan;
                    break;

                case "tsButtonBlackArrow":
                    SetCursorToSelect();
                    break;

                case "tsButtonMeasure":
                    axMap.CursorMode = tkCursorMode.cmMeasure;
                    axMap.MapCursor = tkCursor.crsrMapDefault;
                    break;

                case "tsButtonClearSelection":
                    if (_currentMapLayer != null && _currentMapLayer.LayerType == "ShapefileClass")
                    {
                        _mapLayersHandler.ClearSelection(_currentMapLayer.Handle);
                    }
                    break;

                case "tsButtonClearAllSelection":
                    _mapLayersHandler.ClearAllSelections();
                    break;
            }
        }

        private void axMap_MouseDownEvent(object sender, AxMapWinGIS._DMapEvents_MouseDownEvent e)
        {
        }
    }
}
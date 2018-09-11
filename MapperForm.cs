using AxMapWinGIS;
using MapWinGIS;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

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
        public event EventHandler MapperClosed;

        public ContextMenuStrip MapDropDownMenu
        {
            get { return menuDropDown; }
        }

        public MapLayersHandler MapLayersHandler
        {
            get { return _mapLayersHandler; }
        }

        public GeoProjection GeoProjection
        {
            get { return axMap.GeoProjection; }
        }

        public MapInterActionHandler MapInterActionHandler
        {
            get { return _mapInterActionHandler; }
        }

        public static MapperForm GetInstance(Form parentForm)
        {
            if (_instance == null) _instance = new MapperForm(parentForm);
            return _instance;
        }

        public int NumLayers()
        {
            return axMap.NumLayers;
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

        public void MapDecorationsVisibility(bool visible)
        {
            axMap.ScalebarVisible = visible;
            if (visible)
            {
                axMap.ShowCoordinates = tkCoordinatesDisplay.cdmAuto;
                axMap.ScalebarUnits = tkScalebarUnits.GoogleStyle;
                axMap.Redraw();
            }
            else
            {
                axMap.ShowCoordinates = tkCoordinatesDisplay.cdmNone;
            }
        }

        public MapLayer CurrentMapLayer
        {
            get { return _currentMapLayer; }
        }

        public Grid25MajorGrid Grid25MajorGrid
        {
            get { return _grid25MajorGrid; }
        }

        public AxMap MapControl { get; internal set; }

        public void MapFishingGround(string grid25Name, FishingGrid.fadUTMZone utmZone)
        {
            FishingGroundMappingHandler fgmh = new FishingGroundMappingHandler(axMap.GeoProjection);
            fgmh.MapLayersHandler = _mapLayersHandler;
            fgmh.MapFishingGround(grid25Name, utmZone);
        }

        public void CreateGrid25MajorGrid(FishingGrid.fadUTMZone UTMZone)
        {
            _grid25MajorGrid = new Grid25MajorGrid(axMap);
            _grid25MajorGrid.UTMZone = UTMZone;
            _grid25MajorGrid.GenerateMajorGrids();
            _grid25MajorGrid.MapLayers = _mapLayersHandler;
            _grid25MajorGrid.MapInterActionHandler = _mapInterActionHandler;
            axMap.GeoProjection.SetWgs84Projection(_grid25MajorGrid.Grid25Geoprojection);
            axMap.MapUnits = tkUnitsOfMeasure.umMeters;
            var h = _mapLayersHandler.AddLayer(Grid25MajorGrid.Grid25Grid, "Grid25", true, true);
            _mapLayersHandler.LoadMapState(false);
            _grid25MajorGrid.MoveToTop();
            _mapLayersHandler.set_MapLayer(0);
        }

        public MapperForm(Form parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;
        }

        private void OnMapperForm_Load(object sender, EventArgs e)
        {
            MapControl = axMap;
            toolstripToolBar.ClickThrough = true;
            Text = "Map";
            global.MappingForm = this;
            global.LoadFormSettings(this);
            _mapLayersHandler = new MapLayersHandler(axMap);
            _mapLayersHandler.CurrentLayer += OnCurrentMapLayer;
            _mapInterActionHandler = new MapInterActionHandler(axMap, _mapLayersHandler);
            _mapInterActionHandler.MapContextMenuStrip = menuDropDown;
            SetCursorToSelect();

            if (global.MappingMode == global.fad3MappingMode.defaultMode)
            {
                _mapLayersHandler.LoadMapState();
            }
            else
            {
                tsButtonSave.Enabled = false;
            }
        }

        private void OnCurrentMapLayer(MapLayersHandler s, LayerEventArg e)
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
            EventHandler handler = MapperClosed;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public void OpenFileDialog()
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
                case "tsButtonSave":
                    _mapLayersHandler.SaveMapState();
                    break;

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
    }
}
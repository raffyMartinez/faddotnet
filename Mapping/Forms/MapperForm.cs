using AxMapWinGIS;
using FAD3.Database.Classes;
using FAD3.Mapping.Classes;
using MapWinGIS;
using System;
using System.Drawing;
using System.Windows.Forms;
using FAD3.Mapping.Forms;

namespace FAD3
{
    public partial class MapperForm : Form
    {
        private static MapperForm _instance;
        private Grid25MajorGrid _grid25MajorGrid;                                   //handles grid25 fishing ground grid maps
        private MapInterActionHandler _mapInterActionHandler;                       //handles interaction with the map control
        private MapLayersHandler _mapLayersHandler;                                 //handles map layers
        private Graticule _graticule;                                               //handles map graticule
        private SaveMapImage _saveMapImage;                                         //handles saving of map image
        private Form _parentForm;
        private tkCursorMode _cursorMode;
        private MapLayer _currentMapLayer;
        public event EventHandler MapperClosed;
        public event EventHandler MapperOpen;

        public float SuggestedDPI = 0;
        public fadUTMZone UTMZone { get; private set; }
        public MapLegend MapLegend { get; internal set; }

        public Graticule Graticule
        {
            get { return _graticule; }
        }

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

        private void DeleteTempFiles()
        {
        }

        private void CleanUp()
        {
            if (_grid25MajorGrid != null)
            {
                _grid25MajorGrid.Dispose();
                _grid25MajorGrid = null;

                _mapLayersHandler.Dispose();
                _mapInterActionHandler.Dispose();
                MapLegend.Dispose();
                MapLegend = null;
                GC.Collect();
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

        public void MapFishingGround(string samplingGuid, fadUTMZone utmZone)
        {
            FishingGroundMappingHandler fgmh = new FishingGroundMappingHandler(axMap.GeoProjection);
            fgmh.MapLayersHandler = _mapLayersHandler;
            fgmh.MapSamplingFishingGround(samplingGuid, utmZone, "Fishing ground");
            MapControl.Redraw();
        }

        public void MapFishingGround(string grid25Name, fadUTMZone utmZone, string pointName = "", bool testIfInland = false)
        {
            FishingGroundMappingHandler fgmh = new FishingGroundMappingHandler(axMap.GeoProjection);
            fgmh.MapLayersHandler = _mapLayersHandler;
            fgmh.MapFishingGround(grid25Name, utmZone, pointName, testIfInland);
        }

        public void DisplayGraticule()
        {
        }

        public void CreateGrid25MajorGrid(fadUTMZone utmZone)
        {
            UTMZone = utmZone;
            _grid25MajorGrid = new Grid25MajorGrid(axMap);
            _grid25MajorGrid.UTMZone = utmZone;
            _grid25MajorGrid.GenerateMajorGrids();
            _grid25MajorGrid.MaplayersHandler = _mapLayersHandler;
            _grid25MajorGrid.MapInterActionHandler = _mapInterActionHandler;
            axMap.GeoProjection.SetWgs84Projection(_grid25MajorGrid.Grid25Geoprojection);
            axMap.MapUnits = tkUnitsOfMeasure.umMeters;

            var h = _mapLayersHandler.AddLayer(Grid25MajorGrid.Grid25Grid, "Grid25", true, true);
            _mapLayersHandler.LoadMapState(false);
            _grid25MajorGrid.MoveToTop();
            _mapLayersHandler.set_MapLayer(h);
        }

        public MapperForm(Form parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;
        }

        private void ConfigureMapControl()
        {
            axMap.ZoomBehavior = tkZoomBehavior.zbDefault;
        }

        private void OnMapperForm_Load(object sender, EventArgs e)
        {
            MapControl = axMap;
            toolstripToolBar.ClickThrough = true;
            Text = "Map";
            global.LoadFormSettings(this);
            _mapLayersHandler = new MapLayersHandler(axMap);
            _mapLayersHandler.OnLayerVisibilityChanged += OnMapLayerVisibilityChanged;
            _mapLayersHandler.CurrentLayer += OnCurrentMapLayer;
            _mapLayersHandler.LayerRead += OnMapLayerRead;
            _mapInterActionHandler = new MapInterActionHandler(axMap, _mapLayersHandler)
            {
                MapContextMenuStrip = menuDropDown
            };

            if (global.MappingMode == fad3MappingMode.defaultMode)
            {
                _mapLayersHandler.LoadMapState();
            }
            else
            {
                tsButtonSave.Enabled = false;
            }
            ConfigureMapControl();
            SetCursor(tkCursorMode.cmSelection);
            EventHandler handler = MapperOpen;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
            global.MappingForm = this;
            MapLegend = new MapLegend(MapControl, _mapLayersHandler);
        }

        public void SetGraticuleTitle(string title)
        {
            bool formFound = false;
            if (Graticule != null)
            {
                foreach (Form f in Application.OpenForms)
                {
                    if (f.Name == "GraticuleForm")
                    {
                        GraticuleForm frm = (GraticuleForm)f;
                        frm.SetGraticuleTitle(title);
                        formFound = true;
                        break;
                    }
                }
                if (!formFound)
                {
                    Graticule.MapTitle = title;
                    Graticule.Refresh();
                }
            }
        }

        private void OnMapLayerRead(MapLayersHandler s, LayerEventArg e)
        {
        }

        private void OnMapLayerVisibilityChanged(MapLayersHandler s, LayerEventArg e)
        {
            int n = 0;
            int position = 0;
            string title = "";
            if (Graticule != null)
            {
                foreach (MapLayer ml in MapLayersHandler)
                {
                    if (ml.IsPointDatabaseLayer && ml.Visible)
                    {
                        n++;

                        if (n == 0)
                        {
                            position = ml.LayerPosition;
                            title = ml.Name;
                        }
                        if (n > 0 && ml.LayerPosition < position)
                        {
                            title = ml.Name;
                        }
                    }
                }
            }
            SetGraticuleTitle(title);
        }

        private void OnCurrentMapLayer(MapLayersHandler s, LayerEventArg e)
        {
            _currentMapLayer = _mapLayersHandler.get_MapLayer(e.LayerHandle);
        }

        public void SetCursor(tkCursorMode cursorMode)
        {
            Bitmap b = new Bitmap(Properties.Resources.pan);
            switch (cursorMode)
            {
                case tkCursorMode.cmPan:
                    axMap.CursorMode = tkCursorMode.cmPan;
                    break;

                case tkCursorMode.cmSelection:
                    b = new Bitmap(Properties.Resources.arrow32);
                    axMap.CursorMode = tkCursorMode.cmSelection;
                    break;
            }

            b.MakeTransparent(b.GetPixel(0, 0));
            Graphics g = Graphics.FromImage(b);
            IntPtr ptr = b.GetHicon();
            axMap.MapCursor = tkCursor.crsrUserDefined;
            axMap.UDCursorHandle = (int)ptr;
        }

        private void OnMapperForm_Closed(object sender, FormClosedEventArgs e)
        {
            if (_saveMapImage != null)
            {
                _saveMapImage.PointSizeExceed100Error -= OnPointSizeRenderError;
            }
            global.MappingForm = null;
            CleanUp();

            _instance = null;
            global.SaveFormSettings(this);
            EventHandler handler = MapperClosed;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
            global.mainForm.SetMapDependendMenus();
        }

        public DialogResult OpenFileDialog()
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

            DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK && ofd.FileName.Length > 0)
            {
                filename = ofd.FileName;

                var (success, errMsg) = _mapLayersHandler.FileOpenHandler(filename);
                if (!success)
                {
                    MessageBox.Show(errMsg, "Error in opening file", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dr = DialogResult.Cancel;
                }
            }
            return dr;
        }

        public void SaveMapImageBatch(double dpi, MapTextGraticuleHelper helper, string fileName)
        {
            var tempFile = SaveTempMapToImage(dpi);
            Bitmap b = new Bitmap(tempFile);
            var h = b.Height * 1.2;
            Rectangle r = new Rectangle(0, 0, b.Width, (int)h);
            Graphics g = Graphics.FromImage(b);
        }

        public string SaveTempMapToImage(double dpi = 96)
        {
            string fileName = string.Empty;
            _saveMapImage = new SaveMapImage(axMap);
            _saveMapImage.MapLayersHandler = _mapLayersHandler;
            if (_saveMapImage.SaveToTempFile(dpi))
            {
                fileName = _saveMapImage.TempMapFileName;
                _saveMapImage.Dispose();
                _saveMapImage = null;
            }
            return fileName;
        }

        public bool SaveMapToImage(double dpi, string fileName, bool Preview = true, bool maintainOnePointLineWidth = false)
        {
            var success = false;
            _saveMapImage = new SaveMapImage(fileName, dpi, axMap);
            //_saveMapImage = SaveMapImage.GetInstance(axMap, dpi);
            //_saveMapImage = SaveMapImage.GetInstance();
            //_saveMapImage.MapControl = axMap;
            //_saveMapImage.FileName = fileName;
            _saveMapImage.PreviewImage = Preview;
            _saveMapImage.MapLayersHandler = _mapLayersHandler;
            _saveMapImage.MaintainOnePointLineWidth = maintainOnePointLineWidth;
            _saveMapImage.PointSizeExceed100Error += OnPointSizeRenderError;
            success = _saveMapImage.Save(_grid25MajorGrid != null);
            try
            {
                _saveMapImage.Dispose();
            }
            catch
            {
                //ignore
            }
            return success;
        }

        private void OnPointSizeRenderError(object sender, EventArgs e)
        {
            MessageBox.Show("Error in rendering map being saved. A point size exceeded 100\r\n"
                + "Use lower resolution when saving map",
                "Map rendering error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            SuggestedDPI = _saveMapImage.SuggestedDPI;
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

                    if (MapLayersHandler.CurrentMapLayer != null)
                    {
                        EditShapeAttributeForm esaf = EditShapeAttributeForm.GetInstance(this, _mapInterActionHandler);
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
                    SetCursor(tkCursorMode.cmPan);
                    break;

                case "tsButtonBlackArrow":
                    SetCursor(tkCursorMode.cmSelection);
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

                case "tsButtonGraticule":
                    ShowGraticuleForm();
                    break;

                case "tsButtonSaveImage":
                    var saveForm = new SaveMapForm();
                    saveForm.SaveType(SaveAsShapefile: false);
                    saveForm.ShowDialog(this);
                    break;

                case "tsButtonCloseMap":
                    Close();
                    return;
            }

            switch (axMap.CursorMode)
            {
                case tkCursorMode.cmSelection:
                    SetCursor(tkCursorMode.cmSelection);
                    break;

                case tkCursorMode.cmPan:
                    SetCursor(tkCursorMode.cmPan);
                    break;

                default:

                    break;
            }
        }

        public void ShowGraticuleForm()
        {
            _graticule = new Graticule(axMap, _mapLayersHandler);
            var gf = GraticuleForm.GetInstance(this);
            if (!gf.Visible)
            {
                gf.Show(this);
            }
            else
            {
                gf.BringToFront();
            }
            if (MapLegend != null)
            {
                MapLegend.Graticule = _graticule;
            }
            gf.GraticuleRemoved += OnGraticuleRemoved;
        }

        private void OnGraticuleRemoved(object sender, EventArgs e)
        {
            if (_graticule != null)
            {
                _graticule.Dispose();
                _graticule = null;
                MapLegend.Graticule = null;
            }
        }
    }
}
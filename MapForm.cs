using MapWinGIS;
using System;
using System.Windows.Forms;
using System.Drawing;

namespace FAD3
{
    public partial class MapForm : Form
    {
        private static MapForm _instance;
        private Grid25MajorGrid _grid25MajorGrid;
        private MapLayers _mapLayers;
        private Form _parentForm;

        public static MapForm GetInstance(Form parentForm)
        {
            if (_instance == null) _instance = new MapForm(parentForm);
            return _instance;
        }

        private void CleanUp()
        {
            if (_grid25MajorGrid != null)
            {
                _grid25MajorGrid.Dispose();
                _grid25MajorGrid = null;
            }
        }

        public Grid25MajorGrid grid25MajorGrid
        {
            get { return _grid25MajorGrid; }
        }

        public void createGrid25MajorGrid(FishingGrid.fadUTMZone UTMZone)
        {
            _grid25MajorGrid = new Grid25MajorGrid(axMap);
            _grid25MajorGrid.UTMZone = UTMZone;
            _grid25MajorGrid.mapLayers = _mapLayers;

            axMap.GeoProjection.SetWgs84Projection(_grid25MajorGrid.Grid25Geoprojection);
            axMap.MapUnits = tkUnitsOfMeasure.umMeters;

            _mapLayers.AddLayer(grid25MajorGrid.Grid25Grid, "Grid25", true, true);
        }

        public MapForm(Form parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;
        }

        private void frmMap_Load(object sender, EventArgs e)
        {
            Text = "Map";
            global.MappingForm = this;
            global.LoadFormSettings(this);
            _mapLayers = new MapLayers(axMap);
            SetCursorToSelect();
        }

        public void SetCursorToSelect()
        {
            axMap.MapCursor = tkCursor.crsrUserDefined;
            axMap.UDCursorHandle = (int)((Bitmap)ilCursors.Images["arrow32"]).GetHicon();
            axMap.CursorMode = tkCursorMode.cmSelection;
        }

        private void frmMap_FormClosed(object sender, FormClosedEventArgs e)
        {
            global.MappingForm = null;
            CleanUp();

            _instance = null;
            global.SaveFormSettings(this);
        }

        private void OpenFileDialog()
        {
            string filename = "";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open a GIS layer";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

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
                var result = _mapLayers.FileOpenHandler(filename);
                if (!result.success)
                {
                    MessageBox.Show(result.errMsg, "Error in opening file", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    var mlf = MapLayersForm.GetInstance(_mapLayers, this);
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
            }
        }

        private void axMap_MouseDownEvent(object sender, AxMapWinGIS._DMapEvents_MouseDownEvent e)
        {
        }
    }
}
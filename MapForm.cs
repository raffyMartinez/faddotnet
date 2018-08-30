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

        private void OnToolbarClicked(object sender, ToolStripItemClickedEventArgs e)
        {
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

                case "tsButtonMeasure":
                    axMap.CursorMode = tkCursorMode.cmMeasure;
                    axMap.MapCursor = tkCursor.crsrMapDefault;
                    break;

                case "tsButtonBlackArrow":
                    SetCursorToSelect();
                    break;

                case "tsButtonLayerAdd":
                    break;

                case "tsButtonZoomOut":
                    axMap.MapCursor = tkCursor.crsrUserDefined;
                    axMap.UDCursorHandle = (int)((Bitmap)e.ClickedItem.Image).GetHicon();
                    axMap.CursorMode = tkCursorMode.cmZoomOut;
                    break;

                case "tsButtonZoomIn":
                    axMap.MapCursor = tkCursor.crsrUserDefined;
                    axMap.UDCursorHandle = (int)((Bitmap)e.ClickedItem.Image).GetHicon();
                    axMap.CursorMode = tkCursorMode.cmZoomIn;
                    break;

                case "tsButtonPan":
                    axMap.MapCursor = tkCursor.crsrUserDefined;
                    axMap.UDCursorHandle = (int)((Bitmap)e.ClickedItem.Image).GetHicon();
                    axMap.CursorMode = tkCursorMode.cmPan;

                    //axMap.UDCursorHandle = (int)((Bitmap)imList.Images["gridCursor"]).GetHicon();

                    break;
            }
        }
    }
}
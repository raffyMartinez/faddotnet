using MapWinGIS;
using System;
using System.Windows.Forms;

namespace FAD3
{
    public partial class MapForm : Form
    {
        private static MapForm _instance;
        private Grid25MajorGrid _grid25MajorGrid;
        private Form _parentForm;

        public static MapForm GetInstance(Form parentForm)
        {
            if (_instance == null) _instance = new MapForm(parentForm);
            return _instance;
        }

        private void CleanUp()
        {
            _grid25MajorGrid.Dispose();
            _grid25MajorGrid = null;
        }

        public Grid25MajorGrid grid25MajorGrid
        {
            get { return _grid25MajorGrid; }
        }

        public void createGrid25MajorGrid(FishingGrid.fadUTMZone UTMZone)
        {
            _grid25MajorGrid = new Grid25MajorGrid(axMap);
            _grid25MajorGrid.UTMZone = UTMZone;

            axMap.GeoProjection.SetWgs84Projection(_grid25MajorGrid.Grid25Geoprojection);
            axMap.MapUnits = tkUnitsOfMeasure.umMeters;
            axMap.AddLayer(_grid25MajorGrid.Grid25Grid, true);

            axMap.Refresh();
        }

        public MapForm(Form parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;
        }

        private void frmMap_Load(object sender, EventArgs e)
        {
            global.MappingForm = this;
            global.LoadFormSettings(this);
        }

        private void frmMap_FormClosed(object sender, FormClosedEventArgs e)
        {
            global.MappingForm = null;
            CleanUp();

            _instance = null;
            global.SaveFormSettings(this);
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem tsi = e.ClickedItem;
            switch (tsi.Name)
            {
                case "tsButtonLayers":
                    break;

                case "tsButtonLayerAdd":
                    break;
            }
        }
    }
}
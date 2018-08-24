using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AxMapWinGIS;
using MapWinGIS;

namespace FAD3
{
    public partial class MapForm : Form
    {
        private static MapForm _instance;
        private Grid25MapHelper _grid25MapHelper;

        public static MapForm GetInstance()
        {
            if (_instance == null) _instance = new MapForm();
            return _instance;
        }

        public void grid25MapHelper(string UTMZone)
        {
            _grid25MapHelper = new Grid25MapHelper(axMap, UTMZone);
            if (_grid25MapHelper.Grid25Grid != null)
            {
                axMap.GeoProjection.SetWgs84Projection(_grid25MapHelper.Grid25Geoprojection);
                axMap.MapUnits = tkUnitsOfMeasure.umMeters;
                axMap.AddLayer(_grid25MapHelper.Grid25Grid, true);

                axMap.Refresh();
            }
        }

        public MapForm()
        {
            InitializeComponent();
        }

        private void frmMap_Load(object sender, EventArgs e)
        {
            global.MappingForm = this;
        }

        private void frmMap_FormClosed(object sender, FormClosedEventArgs e)
        {
            global.MappingForm = null;
            if (_grid25MapHelper != null)
            {
                _grid25MapHelper.Dispose();
                _grid25MapHelper = null;
            }
            _instance = null;
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
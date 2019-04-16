using AxMapWinGIS;
using System;
using System.Windows.Forms;

namespace FAD3.Mapping.Forms
{
    public partial class Grid25CoordinateForm : Form
    {
        private static Grid25CoordinateForm _instance;
        private AxMap _mapControl;
        private Grid25MajorGrid _majorGrid;
        private string _utmZone;

        public Grid25CoordinateForm(AxMap mapControl, Grid25MajorGrid majorGrid)
        {
            _mapControl = mapControl;
            _majorGrid = majorGrid;
            InitializeComponent();
        }

        public static Grid25CoordinateForm GetInstance(AxMap mapControl, Grid25MajorGrid majorGrid)
        {
            if (_instance == null) _instance = new Grid25CoordinateForm(mapControl, majorGrid);
            return _instance;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            global.LoadFormSettings(this, true);
            _mapControl.SendMouseMove = true;
            _mapControl.MouseMoveEvent += OnMapMouseMove;
            Text = "Capture coordinates";
            _utmZone = _majorGrid.UTMZone.ToString().Replace("utmZone", "");
        }

        private void OnMapMouseDown(object sender, _DMapEvents_MouseDownEvent e)
        {
            throw new NotImplementedException();
        }

        private void OnMapMouseMove(object sender, _DMapEvents_MouseMoveEvent e)
        {
            double utmX = 0, utmY = 0;
            _mapControl.PixelToProj(e.x, e.y, ref utmX, ref utmY);
            var g25Coord = FishingGrid.utmCoordinatesToGrid25(utmX, utmY, _majorGrid.UTMZone);
            txtCoord.Text = $"UTM: {_utmZone} {utmX.ToString("N0")}, {utmY.ToString("N0")}\r\nGrid25: {g25Coord.grid25Name}";
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            _instance = null;
            _mapControl.MouseMoveEvent -= OnMapMouseMove;
            _mapControl = null;
            global.SaveFormSettings(this);
        }
    }
}
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
            // _mapControl.MouseDownEvent += OnMapMouseDown;
        }

        private void OnMapMouseDown(object sender, _DMapEvents_MouseDownEvent e)
        {
            throw new NotImplementedException();
        }

        private void OnMapMouseMove(object sender, _DMapEvents_MouseMoveEvent e)
        {
            double mapX = 0, mapY = 0;
            _mapControl.PixelToProj(e.x, e.y, ref mapX, ref mapY);
            var g25Coord = FishingGrid.utmCoordinatesToGrid25(mapX, mapY, _majorGrid.UTMZone);
            txtCoord.Text = $"UTM: {_utmZone} {mapX.ToString("N0")}, {mapY.ToString("N0")}\r\nGrid25: {g25Coord.grid25Name}";
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            _instance = null;
            //_mapControl.MouseDownEvent -= OnMapMouseDown;
            _mapControl.MouseMoveEvent -= OnMapMouseMove;
            _mapControl = null;
            global.SaveFormSettings(this);
        }
    }
}
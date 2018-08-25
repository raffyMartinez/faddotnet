using System;
using AxMapWinGIS;
using MapWinGIS;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FAD3
{
    public class Grid25MinorGrid : IDisposable
    {
        private AxMap _axMap;
        private bool _disposed = false;
        private tkWgs84Projection _grid25Geoprojection;
        private Shapefile _shapefileMinorGridLines = null;
        private Grid25MajorGrid _grid25MajorGrid;
        private const int _minorGridSideLength = 2000;

        /// <summary>
        /// returns the projection of the map control
        /// </summary>
        public tkWgs84Projection Grid25Geoprojection
        {
            get { return _grid25Geoprojection; }
        }

        public Grid25MinorGrid(AxMap mapControl, tkWgs84Projection grid25Geoprojection, Grid25MajorGrid grid25MajorGrid)
        {
            _grid25Geoprojection = grid25Geoprojection;
            _grid25MajorGrid = grid25MajorGrid;
            _axMap = mapControl;

            _axMap.SendMouseUp = true;
            _axMap.SendMouseDown = true;
            _axMap.SendSelectBoxFinal = true;

            _axMap.MouseUpEvent += OnMapMouseUp;
            _axMap.MouseDownEvent += OnMapMouseDown;
            _axMap.SelectBoxFinal += OnMapSelectBoxFinal;
            //_axMap.DblClick += OnMapDoubleClick;
            _axMap.CursorMode = tkCursorMode.cmSelection;
        }

        private void OnMapSelectBoxFinal(object sender, _DMapEvents_SelectBoxFinalEvent e)
        {
            var extL = 0D;
            var extR = 0D;
            var extT = 0D;
            var extB = 0D;
            Extents ext = new Extents();

            _axMap.PixelToProj(e.left, e.top, ref extL, ref extT);
            _axMap.PixelToProj(e.right, e.bottom, ref extR, ref extB);
            ext.SetBounds(extL, extB, 0, extR, extT, 0);
            ConstructMinorGrids(ext);
        }

        private void ConstructMinorGrids(Extents ext)
        {
        }

        private void OnMapMouseDown(object sender, _DMapEvents_MouseDownEvent e)
        {
        }

        private void OnMapMouseUp(object sender, _DMapEvents_MouseUpEvent e)
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                }
                if (_shapefileMinorGridLines != null)
                {
                    _shapefileMinorGridLines.Close();
                    _shapefileMinorGridLines = null;
                }
                _axMap = null;
                _disposed = true;
            }
        }
    }
}
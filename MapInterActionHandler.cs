using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxMapWinGIS;
using MapWinGIS;

namespace FAD3
{
    /// <summary>
    /// Handles interaction with the mouse control. However, interaction is disabled and is refered to grid25MajorClass when the class is active.
    /// </summary>
    public class MapInterActionHandler : IDisposable
    {
        private bool _disposed;
        private AxMap _axMap;
        private MapLayersHandler _mapLayersHandler;
        private bool _selectionFromSelectBox;
        private const int CURSORWIDTH = 5;
        private MapLayer _currentMapLayer;
        private int[] _selectedShapeIndexes;
        public EventHandler Selection;

        public MapLayersHandler MapLayersHandler
        {
            get { return _mapLayersHandler; }
        }

        public int[] SelectedShapeIndexes
        {
            get { return _selectedShapeIndexes; }
        }

        public MapInterActionHandler(AxMap mapControl, MapLayersHandler layersHandler)
        {
            _mapLayersHandler = layersHandler;
            _mapLayersHandler.CurrentLayer += OnCurrentMapLayer;
            _axMap = mapControl;
            _axMap.MouseUpEvent += OnMapMouseUp;
            _axMap.MouseDownEvent += OnMapMouseDown;
            _axMap.SelectBoxFinal += OnMapSelectBoxFinal;
            _axMap.MouseMoveEvent += OnMapMouseMove;
            _axMap.DblClick += OnMapDoubleClick;
            _axMap.SendMouseDown = true;
            _axMap.SendMouseMove = true;
            _axMap.SendSelectBoxFinal = true;
            _axMap.SendMouseUp = true;
            EnableMapInteraction = true;
        }

        private void OnMapDoubleClick(object sender, EventArgs e)
        {
            if (EnableMapInteraction)
            {
            }
        }

        private void OnCurrentMapLayer(MapLayersHandler s, LayerProperty e)
        {
            _currentMapLayer = _mapLayersHandler.get_MapLayer(e.LayerHandle);
        }

        private void OnMapMouseMove(object sender, _DMapEvents_MouseMoveEvent e)
        {
            if (EnableMapInteraction)
            {
            }
        }

        private void OnMapSelectBoxFinal(object sender, _DMapEvents_SelectBoxFinalEvent e)
        {
            if (EnableMapInteraction)
            {
                var extL = 0D;
                var extR = 0D;
                var extT = 0D;
                var extB = 0D;
                Extents selectionBoxExtent = new Extents();

                _axMap.PixelToProj(e.left, e.top, ref extL, ref extT);
                _axMap.PixelToProj(e.right, e.bottom, ref extR, ref extB);
                selectionBoxExtent.SetBounds(extL, extB, 0, extR, extT, 0);
                Select(selectionBoxExtent, SelectionFromSelectBox: true);
            }
        }

        private void Select(Extents selectExtents, bool SelectionFromSelectBox = false)
        {
            _selectionFromSelectBox = SelectionFromSelectBox;
            if (_currentMapLayer.LayerType == "ShapefileClass")
            {
                var sf = _axMap.get_Shapefile(_currentMapLayer.Handle);
                sf.SelectNone();
                sf.SelectionAppearance = tkSelectionAppearance.saDrawingOptions;

                switch (sf.ShapefileType)
                {
                    case ShpfileType.SHP_POINT:
                        break;

                    case ShpfileType.SHP_POLYGON:
                        break;

                    case ShpfileType.SHP_POLYLINE:

                        break;
                }

                var objSelection = new object();
                if (sf.SelectShapes(selectExtents, 0, SelectMode.INTERSECTION, ref objSelection))
                {
                    _selectedShapeIndexes = (int[])objSelection;
                    for (int n = 0; n < _selectedShapeIndexes.Length; n++)
                    {
                        sf.ShapeSelected[_selectedShapeIndexes[n]] = true;
                    }
                }
                _axMap.Redraw();
                EventHandler handler = Selection;
                if (null != handler) handler(this, EventArgs.Empty);
            }
            else if (_currentMapLayer.LayerType == "ImageClass")
            {
            }
        }

        private void OnMapMouseDown(object sender, _DMapEvents_MouseDownEvent e)
        {
            if (EnableMapInteraction)
            {
                _selectionFromSelectBox = false;
            }
        }

        private void OnMapMouseUp(object sender, _DMapEvents_MouseUpEvent e)
        {
            //we only proceed if a drag-select was not done
            if (EnableMapInteraction && !_selectionFromSelectBox && _axMap.CursorMode == tkCursorMode.cmSelection)
            {
                var extL = 0D;
                var extR = 0D;
                var extT = 0D;
                var extB = 0D;
                Extents ext = new Extents();

                _axMap.PixelToProj(e.x - CURSORWIDTH, e.y - CURSORWIDTH, ref extL, ref extT);
                _axMap.PixelToProj(e.x + CURSORWIDTH, e.y + CURSORWIDTH, ref extR, ref extB);
                ext.SetBounds(extL, extB, 0, extR, extT, 0);
                Select(ext, SelectionFromSelectBox: false);
            }
        }

        public bool EnableMapInteraction { get; set; }

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
                _mapLayersHandler = null;
                _currentMapLayer = null;
                _axMap = null;
                _disposed = true;
            }
        }
    }
}
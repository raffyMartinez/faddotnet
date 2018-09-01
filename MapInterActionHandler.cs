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
        private bool _enableMapInteraction;
        private MapLayersHandler _mapLayersHandler;
        private bool _selectionFromSelectBox;
        private const int CURSORWIDTH = 5;
        private MapLayer _currentMapLayer;
        private int[] _selectedShapeIndexes;

        public MapInterActionHandler(AxMap mapControl, MapLayersHandler layersHandler)
        {
            _mapLayersHandler = layersHandler;
            _mapLayersHandler.CurrentLayer += OnCurrentMapLayer;
            _axMap = mapControl;
            _axMap.MouseUpEvent += OnMapMouseUp;
            _axMap.MouseDownEvent += OnMapMouseDown;
            _axMap.SelectBoxFinal += OnMapSelectBoxFinal;
            _axMap.MouseMoveEvent += OnMapMouseMove;
            EnableMapInteraction = true;
        }

        private void OnCurrentMapLayer(MapLayersHandler s, LayerProperty e)
        {
            _currentMapLayer = _mapLayersHandler.get_MapLayer(e.LayerHandle);
        }

        private void OnMapMouseMove(object sender, _DMapEvents_MouseMoveEvent e)
        {
        }

        private void OnMapSelectBoxFinal(object sender, _DMapEvents_SelectBoxFinalEvent e)
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
            }
            else if (_currentMapLayer.LayerType == "ImageClass")
            {
            }
        }

        private void OnMapMouseDown(object sender, _DMapEvents_MouseDownEvent e)
        {
            _selectionFromSelectBox = false;
        }

        private void OnMapMouseUp(object sender, _DMapEvents_MouseUpEvent e)
        {
            //we only proceed if a drag-select was not done
            if (!_selectionFromSelectBox && _axMap.CursorMode == tkCursorMode.cmSelection)
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

        public bool EnableMapInteraction
        {
            get { return _enableMapInteraction; }
            set
            {
                _enableMapInteraction = value;
                _axMap.SendMouseDown = _enableMapInteraction;
                _axMap.SendMouseUp = _enableMapInteraction;
                _axMap.SendSelectBoxFinal = _enableMapInteraction;
                _axMap.SendMouseMove = _enableMapInteraction;
            }
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
                _axMap = null;
                _disposed = true;
            }
        }
    }
}
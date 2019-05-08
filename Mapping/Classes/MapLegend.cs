using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapWinGIS;
using AxMapWinGIS;

namespace FAD3.Mapping.Classes
{
    public class MapLegend : IDisposable
    {
        private AxMap _mapControl;
        private bool _disposed;
        public List<MapLayer> Layers { get; set; }
        private Extents _legendExtents;
        private int _legendDrawingLayer;

        public MapLegend(AxMap mapControl)
        {
            _mapControl = mapControl;
            _mapControl.SendMouseMove = true;
            _mapControl.SendMouseDown = true;
            _mapControl.SendSelectBoxDrag = true;
            _mapControl.SendSelectBoxFinal = true;
            _mapControl.SelectBoxFinal += OnMapSelectBoxFinal;
        }

        private void OnMapSelectBoxFinal(object sender, _DMapEvents_SelectBoxFinalEvent e)
        {
            _legendExtents = new Extents();
            _legendExtents.SetBounds(e.left, e.bottom, 0, e.right, e.top, 0);
            DrawLegend();
        }

        public bool DrawLegend()
        {
            bool success = false;
            if (_legendExtents != null)
            {
                _mapControl.ClearDrawing(_legendDrawingLayer);
                _legendDrawingLayer = _mapControl.NewDrawing(tkDrawReferenceList.dlScreenReferencedList);
                foreach (MapLayer ml in Layers)
                {
                    if (ml.LayerType == "ShapefileClass")
                    {
                        var sf = ml.LayerObject as Shapefile;
                        if (sf.Categories.Count == 0)
                        {
                            switch (sf.ShapefileType)
                            {
                                case ShpfileType.SHP_POLYGON:
                                    break;

                                case ShpfileType.SHP_POLYLINE:
                                    break;

                                case ShpfileType.SHP_POINT:

                                    break;
                            }
                        }
                        else
                        {
                            for (int n = 0; n < sf.Categories.Count; n++)
                            {
                                switch (sf.ShapefileType)
                                {
                                    case ShpfileType.SHP_POLYGON:
                                        break;

                                    case ShpfileType.SHP_POLYLINE:
                                        break;

                                    case ShpfileType.SHP_POINT:
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {
                        //not a shapefile
                    }
                }
                success = true;
            }
            else
            {
                success = false;
                throw new Exception("Legend position not set");
            }

            return success;
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

                _mapControl = null;
                _mapControl.SelectBoxFinal -= OnMapSelectBoxFinal;
                _disposed = true;
            }
        }
    }
}
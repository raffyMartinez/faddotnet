using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapWinGIS;
using AxMapWinGIS;
using System.Drawing;
using FAD3.Database.Classes;
using System.Windows.Forms;

namespace FAD3.Mapping.Classes
{
    public class MapLegend : IDisposable, IEnumerable<LegendLabel>
    {
        private AxMap _mapControl;
        private bool _disposed;
        public MapLayersHandler MapLayersHandler { get; set; }
        private Extents _legendExtents;
        private int _legendDrawingLayer;
        private string _legendCornerPosition;
        private MapWinGIS.Point _legendAnchorCorner;
        private Dictionary<int, LegendLabel> _legendLabels = new Dictionary<int, LegendLabel>();
        private LegendFrame _legendFrame;
        public Graticule Graticule { get; set; }
        private double _labelMaxWidth;
        private MapLegendPosition _mapLegendPosition;

        public MapLegendPosition MapLegendPosition
        {
            get { return _mapLegendPosition; }
            set
            {
                _mapLegendPosition = value;
                _legendExtents = null;
            }
        }

        public string Caption { get; set; }

        public string LegendPositionInMap
        {
            get { return _legendCornerPosition; }
            set
            {
                _legendCornerPosition = value;
                if (_legendAnchorCorner == null)
                {
                    SetLegendPositionFromCorner();
                }
            }
        }

        public MapLegend(AxMap mapControl, MapLayersHandler mapLayersHandler)
        {
            _mapControl = mapControl;
            _mapControl.SendMouseMove = true;
            _mapControl.SendMouseDown = true;
            //_mapControl.SendSelectBoxDrag = true;
            //_mapControl.SendSelectBoxFinal = true;
            _mapControl.ShowZoomBar = false;
            _mapControl.SelectBoxFinal += OnMapSelectBoxFinal;
            MapLayersHandler = mapLayersHandler;
            Caption = "Legend";
        }

        private void SetLegendPositionFromCorner()
        {
            MapWinGIS.Point pt = new MapWinGIS.Point();
            switch (_legendCornerPosition)
            {
                case "topLeft":
                    pt.x = Graticule.GraticuleExtents.xMin;
                    pt.y = Graticule.GraticuleExtents.yMax;
                    break;

                case "topRight":
                    pt.x = Graticule.GraticuleExtents.xMax;
                    pt.y = Graticule.GraticuleExtents.yMax;
                    break;

                case "bottomLeft":
                    pt.x = Graticule.GraticuleExtents.xMin;
                    pt.y = Graticule.GraticuleExtents.yMin;
                    break;

                case "bottomRight":
                    pt.x = Graticule.GraticuleExtents.xMax;
                    pt.y = Graticule.GraticuleExtents.yMin;
                    break;
            }
            double pixelX = 0;
            double pixelY = 0;
            _mapControl.ProjToPixel(pt.x, pt.y, ref pixelX, ref pixelY);
            _legendAnchorCorner = new MapWinGIS.Point()
            {
                x = pixelX,
                y = pixelY
            };
        }

        private void OnMapSelectBoxFinal(object sender, _DMapEvents_SelectBoxFinalEvent e)
        {
            _legendExtents = new Extents();
            _legendExtents.SetBounds(e.left, e.bottom, 0, e.right, e.top, 0);
        }

        private void ComputeExtent()
        {
            foreach (MapLayer ml in MapLayersHandler)
            {
                if (ml.IncludeInLegend)
                {
                }
            }
        }

        IEnumerator<LegendLabel> IEnumerable<LegendLabel>.GetEnumerator()
        {
            return _legendLabels.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_legendLabels.Values).GetEnumerator();
        }

        public LegendLabel this[int index]
        {
            set { _legendLabels[index] = value; }
            get { return _legendLabels[index]; }
        }

        public void RemoveLegend()
        {
            _mapControl.ClearDrawing(_legendDrawingLayer);
            _legendLabels.Clear();
            _labelMaxWidth = 0;
        }

        public bool DrawLegend()
        {
            bool success = false;
            RemoveLegend();
            if (Graticule != null && Graticule.GridVisible)
            {
                if (_legendExtents != null)
                {
                    _mapLegendPosition = MapLegendPosition.PositionFromDefinedExtent;
                }
                else
                {
                    //SetLegendPositionFromCorner();
                    _mapLegendPosition = MapLegendPosition.PositionFromCorner;
                }
                _legendDrawingLayer = _mapControl.NewDrawing(tkDrawReferenceList.dlScreenReferencedList);
                using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
                {
                    g.PageUnit = GraphicsUnit.Pixel;
                    foreach (MapLayer ml in MapLayersHandler)
                    {
                        if (ml.IncludeInLegend)
                        {
                            SizeF size = g.MeasureString(ml.Name, SystemFonts.DefaultFont);
                            LegendLabel label = new LegendLabel(ml.Handle, ml.Name, size.Width);
                            label.Height = size.Height;
                            _legendLabels.Add(ml.Handle, label);
                            if (size.Width > _labelMaxWidth)
                            {
                                _labelMaxWidth = size.Width;
                            }

                            if (ml.LayerType == "ShapefileClass")
                            {
                                var sf = ml.LayerObject as Shapefile;
                                if (sf.Categories.Count == 0)
                                {
                                }
                                else if (sf.Categories.Count > 0)
                                {
                                    foreach (var item in ml.ClassificationItems)
                                    {
                                        size = g.MeasureString(item.Value.Caption, SystemFonts.DefaultFont);
                                    }
                                }
                            }
                            else
                            {
                                //not a shapefile
                            }
                        }
                    }
                }
                success = SetLegendFrame();
            }
            _mapControl.Redraw();
            return _legendLabels.Count > 0;
        }

        private bool SetLegendFrame()
        {
            if (_legendLabels.Count > 0)
            {
                //SetLegendPositionFromCorner();
                _legendFrame = new LegendFrame(Caption, _legendDrawingLayer, _mapControl);
                _legendFrame.MapLegendPosition = _mapLegendPosition;
                _legendFrame.LegendCornerPosition = _legendCornerPosition;
                _legendFrame.LegendAnchorCorner = _legendAnchorCorner;
                _legendFrame.Width = _labelMaxWidth;
                _legendFrame.BorderColor = new Utils().ColorByName(tkMapColor.Black);
                _legendFrame.LegendLabels = _legendLabels;
                return _legendFrame.SetUpFrame();
            }
            return false;
            //double[] x = new double[5];
            //double[] y = new double[5];
            //switch (_mapLegendPosition)
            //{
            //    case MapLegendPosition.PositionFromCorner:
            //        switch (_legendPosition)
            //        {
            //            case "topLeft":
            //                x[0] = _legendAnchorCorner.x;
            //                y[0] = _legendAnchorCorner.y;

            //                x[1] = _legendAnchorCorner.x + _labelMaxWidth;
            //                y[1] = _legendAnchorCorner.y;

            //                x[2] = _legendAnchorCorner.x + _labelMaxWidth;
            //                y[2] = _legendAnchorCorner.y + 100;

            //                x[3] = _legendAnchorCorner.x;
            //                y[3] = _legendAnchorCorner.y + 100;

            //                x[4] = _legendAnchorCorner.x;
            //                y[4] = _legendAnchorCorner.y;
            //                break;

            //            case "topRight":

            //                break;

            //            case "bottomLeft":

            //                break;

            //            case "bottomRight":

            //                break;
            //        }
            //        break;

            //    case MapLegendPosition.PositionFromDefinedExtent:
            //        break;
            //}
            //object xObj = x;
            //object yObj = y;
            //_mapControl.DrawWidePolygonEx(_legendDrawingLayer, ref xObj, ref yObj, 5, new Utils().ColorByName(tkMapColor.White), true, 2);
            //_mapControl.DrawWidePolygonEx(_legendDrawingLayer, ref xObj, ref yObj, 5, new Utils().ColorByName(tkMapColor.Black), false, 2);
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
                _mapControl.ShowZoomBar = true;
                _mapControl.SelectBoxFinal -= OnMapSelectBoxFinal;
                _mapControl = null;

                _disposed = true;
            }
        }
    }
}
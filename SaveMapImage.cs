using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxMapWinGIS;
using MapWinGIS;

namespace FAD3
{
    public class SaveMapImage : IDisposable
    {
        private bool _disposed;
        private AxMap _axMap;
        private Shapefile _shapeFileMask;

        private string _fileName;
        private int _dpi;
        public Dictionary<int, MapLayer> MapLayerDictionary { get; set; }
        public bool Reset { get; set; }
        private int _handleGridBoundary;
        private int _handleLabels;
        private int _handleMajorGrid;
        private int _handleMinorGrid;

        public SaveMapImage(string fileName, int DPI, AxMap mapControl)
        {
            _fileName = fileName;
            _dpi = DPI;
            _axMap = mapControl;
        }

        private double AdjustLabelProperty(double value, bool reset)
        {
            if (reset)
            {
                value /= (_dpi / 96);
            }
            else
            {
                value *= (_dpi / 96);
            }
            return value;
        }

        private void AdjustLabelProperties(Labels labels, int dpi, bool reset)
        {
            labels.With(lbl =>
            {
                lbl.VerticalPosition = tkVerticalPosition.vpAboveParentLayer;
                lbl.FontSize = (int)AdjustLabelProperty(lbl.FontSize, Reset);
                lbl.OffsetX = AdjustLabelProperty(lbl.OffsetX, Reset);
                lbl.OffsetY = AdjustLabelProperty(lbl.OffsetY, Reset);
                lbl.FontOutlineWidth = (int)AdjustLabelProperty(lbl.FontOutlineWidth, Reset);
                lbl.FrameOutlineWidth = (int)AdjustLabelProperty(lbl.FrameOutlineWidth, Reset);
                lbl.FramePaddingX = (int)AdjustLabelProperty(lbl.FramePaddingX, Reset);
                lbl.FramePaddingY = (int)AdjustLabelProperty(lbl.FramePaddingY, Reset);
                lbl.ShadowOffsetX = (int)AdjustLabelProperty(lbl.ShadowOffsetX, Reset);
                lbl.ShadowOffsetY = (int)AdjustLabelProperty(lbl.ShadowOffsetY, Reset);

                if (lbl.NumCategories > 0)
                {
                    for (int n = 0; n < lbl.NumCategories; n++)
                    {
                        lbl.Category[n].FontSize = (int)AdjustLabelProperty(lbl.Category[n].FontSize, Reset);
                        lbl.Category[n].OffsetX = AdjustLabelProperty(lbl.Category[n].OffsetX, Reset);
                        lbl.Category[n].OffsetY = AdjustLabelProperty(lbl.Category[n].OffsetY, Reset);
                        lbl.Category[n].FontOutlineWidth = (int)AdjustLabelProperty(lbl.Category[n].FontOutlineWidth, Reset);
                        lbl.Category[n].FrameOutlineWidth = (int)AdjustLabelProperty(lbl.Category[n].FrameOutlineWidth, Reset);
                        lbl.Category[n].FramePaddingX = (int)AdjustLabelProperty(lbl.Category[n].FramePaddingX, Reset);
                        lbl.Category[n].FramePaddingY = (int)AdjustLabelProperty(lbl.Category[n].FramePaddingY, Reset);
                        lbl.Category[n].ShadowOffsetX = (int)AdjustLabelProperty(lbl.Category[n].ShadowOffsetX, Reset);
                        lbl.Category[n].ShadowOffsetY = (int)AdjustLabelProperty(lbl.Category[n].ShadowOffsetY, Reset);
                    }
                }
            });
        }

        public bool Save()
        {
            for (int n = 0; n < _axMap.NumLayers; n++)
            {
                var h = _axMap.get_LayerHandle(n);
                var categoryCount = _axMap.get_Shapefile(h).Categories.Count;

                if (_axMap.get_LayerVisible(h))
                {
                    AdjustLabelProperties(_axMap.get_Shapefile(h).Labels, _dpi, Reset);
                }

                _axMap.get_Shapefile(h).DefaultDrawingOptions.With(ddo =>
                {
                    if (Reset)
                    {
                        if (categoryCount > 0)
                        {
                            for (int y = 0; y < categoryCount; y++)
                            {
                                _axMap.get_Shapefile(h).Categories.Item[y].DrawingOptions.LineWidth /= (_dpi / 96);
                                _axMap.get_Shapefile(h).Categories.Item[y].DrawingOptions.PointSize /= (_dpi / 96);
                            }
                        }
                        else
                        {
                            ddo.PointSize /= (_dpi / 96);
                            ddo.LineWidth /= (_dpi / 96);
                        }
                    }
                    else
                    {
                        if (categoryCount > 0)
                        {
                            for (int y = 0; y < categoryCount; y++)
                            {
                                _axMap.get_Shapefile(h).Categories.Item[y].DrawingOptions.LineWidth *= (_dpi / 96);
                                _axMap.get_Shapefile(h).Categories.Item[y].DrawingOptions.PointSize *= (_dpi / 96);
                            }
                        }
                        else
                        {
                            ddo.PointSize *= (_dpi / 96);
                            ddo.LineWidth *= (_dpi / 96);
                        }
                    }
                });
            }

            return SaveMapToImage();
        }

        private Shapefile Mask()
        {
            var sf = new Shapefile();
            if (sf.CreateNew("", ShpfileType.SHP_POLYGON))
            {
                var shp = _axMap.Extents.ToShape().Clip(_axMap.get_Shapefile(_handleGridBoundary).Extents.ToShape(), tkClipOperation.clDifference);
                sf.EditAddShape(shp);
                sf.DefaultDrawingOptions.VerticesVisible = false;
                sf.DefaultDrawingOptions.FillColor = new Utils().ColorByName(tkMapColor.White);
            }
            return sf;
        }

        private bool SaveMapToImage()
        {
            foreach (KeyValuePair<int, MapLayer> kv in MapLayerDictionary.OrderByDescending(r => r.Value.LayerWeight).Take(MapLayerDictionary.Count))
            {
                if (kv.Value.LayerWeight != null)
                {
                    _axMap.MoveLayerBottom(_axMap.get_LayerPosition(kv.Value.Handle));
                }
                switch (kv.Value.Name)
                {
                    case "MBR":
                        _handleGridBoundary = kv.Value.Handle;
                        break;

                    case "Labels":
                        _handleLabels = kv.Value.Handle;
                        break;

                    case "Major grid":
                        _handleMajorGrid = kv.Value.Handle;
                        break;

                    case "Minor grid":
                        _handleMinorGrid = kv.Value.Handle;
                        break;
                }
            }
            var h = _axMap.AddLayer(Mask(), true);
            _axMap.MoveLayerTop(_axMap.get_LayerPosition(h));
            _axMap.get_Shapefile(_handleLabels).Labels.VerticalPosition = tkVerticalPosition.vpAboveAllLayers;
            _axMap.get_Shapefile(_handleLabels).Labels.AvoidCollisions = false;
            return true;
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
                if (_shapeFileMask != null)
                {
                    _shapeFileMask.Close();
                    _shapeFileMask = null;
                }
                _axMap = null;
                _disposed = true;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxMapWinGIS;
using MapWinGIS;
using System.Drawing;

namespace FAD3
{
    public class MapLayers : IDisposable
    {
        public bool _disposed;
        private AxMap _axmap;
        private Dictionary<int, MapLayer> _layerPropertiesDictionary = new Dictionary<int, MapLayer>();

        public delegate void LayerPropertyReadHandler(MapLayers s, LayerProperty e);

        public event LayerPropertyReadHandler LayerPropertyRead;

        public delegate void LayerDeletedHandler(MapLayers s, LayerProperty e);

        public event LayerDeletedHandler LayerDeleted;

        public AxMap MapControl
        {
            get { return _axmap; }
        }

        public void layerSymbol(int layerHandle, System.Windows.Forms.PictureBox pic)
        {
            if (pic.Image != null) pic.Image.Dispose();
            Rectangle rect = pic.ClientRectangle;
            Bitmap bmp = new Bitmap(rect.Width, rect.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);
            IntPtr ptr = g.GetHdc();

            var ly = _axmap.get_GetObject(layerHandle);
            switch (ly.GetType().Name)
            {
                case "ShapefileClass":
                    ((Shapefile)ly).With(shp =>
                    {
                        switch (shp.ShapefileType)
                        {
                            case ShpfileType.SHP_POINT:
                                shp.DefaultDrawingOptions.DrawPoint(ptr, 0.0f, 0.0f, rect.Width, rect.Height);
                                break;

                            case ShpfileType.SHP_POLYGON:
                                int w = rect.Width - 40;
                                int h = rect.Height - 10;
                                shp.DefaultDrawingOptions.DrawRectangle(ptr, (rect.Width - w) / 2, (rect.Height - h) / 2, w, h, shp.DefaultDrawingOptions.LineVisible, 0, 0);
                                break;

                            case ShpfileType.SHP_POLYLINE:
                                w = rect.Width - 40;
                                h = rect.Height - 10;
                                shp.DefaultDrawingOptions.DrawLine(ptr, (rect.Width - w) / 2, (rect.Height - h) / 2, w, h, true, rect.Width, rect.Height);
                                //shp.DefaultDrawingOptions.DrawLine(ptr, 0, rect.Height / 2, rect.Width, rect.Height, true, 0, 0);
                                break;
                        }
                        g.ReleaseHdc(ptr);
                        pic.Image = bmp;
                    });

                    break;
            }
        }

        public Dictionary<int, MapLayer> LayerDictionary
        {
            get { return _layerPropertiesDictionary; }
        }

        public MapLayer get_MapLayer(int layerHandle)
        {
            return _layerPropertiesDictionary[layerHandle];
        }

        public MapLayers(AxMap mapControl)
        {
            _axmap = mapControl;
            _axmap.LayerAdded += OnMapLayerAdded;
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
                    _layerPropertiesDictionary = null;
                }
                _axmap = null;
                _disposed = true;
            }
        }

        public void ReadLayers()
        {
            foreach (var item in _layerPropertiesDictionary)
            {
                if (LayerPropertyRead != null)
                {
                    LayerProperty lp = new LayerProperty(item.Key, item.Value.Name, item.Value.Visible, item.Value.VisibleInLayersUI);
                    LayerPropertyRead(this, lp);
                }
            }
        }

        public void RemoveLayer(int layerHandle)
        {
            _layerPropertiesDictionary.Remove(layerHandle);
            _axmap.RemoveLayer(layerHandle);
            if (LayerDeleted != null)
            {
                LayerProperty lp = new LayerProperty(layerHandle, layerDeleted: true);
                LayerDeleted(this, lp);
            }
        }

        public void EditLayer(int layerHandle, string layerName, bool visible, bool isShown = true)
        {
            if (_layerPropertiesDictionary.ContainsKey(layerHandle))
            {
                var ly = _layerPropertiesDictionary[layerHandle];
                ly.Name = layerName;
                ly.Visible = visible;
                ly.VisibleInLayersUI = isShown;
            }

            _axmap.set_LayerName(layerHandle, layerName);
            _axmap.set_LayerVisible(layerHandle, visible);
            _axmap.Redraw();
        }

        public int AddLayer(object layer, string layerName, bool visible, bool showInLayerUI)
        {
            int h = 0;
            var layerType = layer.GetType().Name;
            var geoProjectionName = "";
            switch (layerType)
            {
                case "ShapefileClass":
                    h = _axmap.AddLayer((Shapefile)layer, visible);
                    geoProjectionName = ((Shapefile)layer).GeoProjection.Name;
                    break;
            }

            _axmap.set_LayerName(h, layerName);
            var mapLayer = new MapLayer(h, layerName, visible, showInLayerUI);
            mapLayer.LayerType = layerType;
            mapLayer.FileName = _axmap.get_LayerFilename(h);
            mapLayer.GeoProjectionName = geoProjectionName;
            _layerPropertiesDictionary.Add(h, mapLayer);
            _axmap.Redraw();

            if (LayerPropertyRead != null)
            {
                LayerProperty lp = new LayerProperty(h, layerName, visible, showInLayerUI);
                LayerPropertyRead(this, lp);
            }

            return h;
        }

        private void OnMapLayerAdded(object sender, _DMapEvents_LayerAddedEvent e)
        {
        }
    }
}
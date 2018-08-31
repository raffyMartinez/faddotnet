using System;
using System.IO;
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
        private int _picWidth;
        private int _picHeight;
        private Dictionary<int, MapLayer> _layerPropertiesDictionary = new Dictionary<int, MapLayer>();

        public delegate void LayerPropertyReadHandler(MapLayers s, LayerProperty e);

        public event LayerPropertyReadHandler LayerPropertyRead;

        public delegate void LayerDeletedHandler(MapLayers s, LayerProperty e);

        public event LayerDeletedHandler LayerDeleted;

        public AxMap MapControl
        {
            get { return _axmap; }
        }

        /// <summary>
        /// sets the legend image in the layers form
        /// </summary>
        /// <param name="layerHandle"></param>
        /// <param name="pic"></param>
        /// <param name="layerType"></param>
        public void layerSymbol(int layerHandle, System.Windows.Forms.PictureBox pic, string layerType)
        {
            if (pic.Image != null) pic.Image.Dispose();
            int w = 0;
            int h = 0;
            Rectangle rect = pic.ClientRectangle;
            Bitmap bmp = new Bitmap(rect.Width, rect.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);
            IntPtr ptr = g.GetHdc();

            var ly = _axmap.get_GetObject(layerHandle);
            switch (layerType)
            {
                case "ShapefileClass":
                    ((Shapefile)ly).With(shp =>
                    {
                        switch (shp.ShapefileType)
                        {
                            case ShpfileType.SHP_POINT:
                                shp.DefaultDrawingOptions.DrawPoint(ptr, (rect.Width / 5) * 2, rect.Height / 2, 0, 0);
                                break;

                            case ShpfileType.SHP_POLYGON:
                                w = rect.Width / 4;
                                h = (rect.Height / 4) * 3;
                                shp.DefaultDrawingOptions.DrawRectangle(ptr, rect.Width / 3, rect.Height / 4, w, h, shp.DefaultDrawingOptions.LineVisible, rect.Width, rect.Height);
                                break;

                            case ShpfileType.SHP_POLYLINE:
                                w = rect.Width / 4;
                                h = (rect.Height / 4) * 3;
                                shp.DefaultDrawingOptions.DrawLine(ptr, rect.Width / 3, rect.Height / 4, w, h, true, rect.Width, rect.Height);
                                //shp.DefaultDrawingOptions.DrawLine(ptr, 0, rect.Height / 2, rect.Width, rect.Height, true, 0, 0);
                                break;
                        }
                        g.ReleaseHdc(ptr);
                        pic.Image = bmp;
                    });

                    break;

                case "ImageClass":
                    if (_layerPropertiesDictionary[layerHandle].ImageThumbnail == null)
                    {
                        rect = pic.ClientRectangle;
                        w = rect.Width / 4;
                        h = (rect.Height / 4) * 3;
                        string filename = _axmap.get_Image(layerHandle).Filename;
                        bmp = new Bitmap(w, h);
                        g = Graphics.FromImage(bmp);
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.FillRectangle(Brushes.White, 0, 0, w, h);
                        g.DrawImage(new Bitmap(filename), 0, 0, w, h);
                        pic.Image = bmp;
                        _layerPropertiesDictionary[layerHandle].ImageThumbnail = bmp;
                    }

                    break;
            }
        }

        /// <summary>
        /// sets the image preview thumbnail of an image layertype. This method is only called if the layers form is already open
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private Bitmap GetImageThumbnail(string filename)
        {
            int w = _picWidth / 4;
            int h = (_picHeight / 4) * 3;
            Bitmap bmp = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(bmp);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.FillRectangle(Brushes.White, 0, 0, w, h);
            g.DrawImage(new Bitmap(filename), 0, 0, w, h);
            return bmp;
        }

        public Dictionary<int, MapLayer> LayerDictionary
        {
            get { return _layerPropertiesDictionary; }
        }

        public MapLayer get_MapLayer(int layerHandle)
        {
            return _layerPropertiesDictionary[layerHandle];
        }

        public int get_LayerPosition(int layerHandle)
        {
            return _axmap.get_LayerPosition(layerHandle);
        }

        public bool MoveLayerBottom(int layerHandle)
        {
            var layerMoved = false;
            if (_axmap.MoveLayerBottom(layerHandle))
            {
                _layerPropertiesDictionary[layerHandle].LayerPosition = _axmap.get_LayerPosition(layerHandle);
                layerMoved = true;
            }
            return layerMoved;
        }

        /// <summary>
        /// sets the class variable that represents the layers form legend picturebox
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void PictureBoxRectangle(int width, int height)
        {
            _picHeight = height;
            _picWidth = width;
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="mapControl"></param>
        public MapLayers(AxMap mapControl)
        {
            _axmap = mapControl;
            _axmap.LayerAdded += OnMapLayerAdded;
            _axmap.ProjectionMismatch += OnProjectionMismatch;
        }

        /// <summary>
        /// reprojects a mismatched layer to the map's projection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProjectionMismatch(object sender, _DMapEvents_ProjectionMismatchEvent e)
        {
            e.reproject = tkMwBoolean.blnTrue;
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

        /// <summary>
        /// gets the layer in the map and retrieves the corresponding layer item in the dictionary. Fires an event after a layer item is read
        /// </summary>
        public void ReadLayers()
        {
            for (int n = 0; n < _axmap.NumLayers; n++)
            {
                var h = _axmap.get_LayerHandle(n);
                if (_layerPropertiesDictionary[h].VisibleInLayersUI)
                {
                    //if there is a listener to the event
                    if (LayerPropertyRead != null)
                    {
                        //get the corresponding layer item in the dictionary
                        var item = _layerPropertiesDictionary[h];

                        //fill up the event argument class with the layer item
                        LayerProperty lp = new LayerProperty(item.Handle, item.Name, item.Visible, item.VisibleInLayersUI, item.LayerType);
                        if (item.ImageThumbnail != null) lp.ImageThumb = item.ImageThumbnail;
                        LayerPropertyRead(this, lp);
                    }
                }
            }
        }

        /// <summary>
        /// removes a layer
        /// </summary>
        /// <param name="layerHandle"></param>
        public void RemoveLayer(int layerHandle)
        {
            _layerPropertiesDictionary.Remove(layerHandle);
            _axmap.RemoveLayer(layerHandle);

            //fire the layer deleted event
            if (LayerDeleted != null)
            {
                LayerProperty lp = new LayerProperty(layerHandle, layerDeleted: true);
                LayerDeleted(this, lp);
            }
        }

        /// <summary>
        /// handles editing of layer name and layer visibility
        /// </summary>
        /// <param name="layerHandle"></param>
        /// <param name="layerName"></param>
        /// <param name="visible"></param>
        /// <param name="isShown"></param>
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

        /// <summary>
        /// handles the opening of map layer files
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public (bool success, string errMsg) FileOpenHandler(string fileName)
        {
            var success = false;
            var errMsg = "";
            var fm = new FileManager();

            if (!fm.get_IsSupported(fileName))
            {
                errMsg = "Datasource isn't supported by MapWinGIS";
            }
            else
            {
                var obj = fm.Open(fileName, tkFileOpenStrategy.fosAutoDetect, null);
                if (fm.LastOpenIsSuccess)
                {
                    if (fm.LastOpenStrategy == tkFileOpenStrategy.fosVectorLayer)
                    {
                        var shapefile = obj as Shapefile;
                        success = shapefile != null;
                        if (success)
                        {
                            if (AddLayer(shapefile) < 0)
                            {
                                success = false;
                                errMsg = "Failed to add layer to map";
                            }
                        }
                    }
                    else if (fm.LastOpenStrategy == tkFileOpenStrategy.fosRgbImage)
                    {
                        var image = obj as MapWinGIS.Image;
                        success = image != null;
                        if (success)
                        {
                            if (AddLayer(image) < 0)
                            {
                                success = false;
                                errMsg = "Failed to add layer to map";
                            }
                        }
                    }
                    else if (fm.LastOpenStrategy == tkFileOpenStrategy.fosDirectGrid
                          || fm.LastOpenStrategy == tkFileOpenStrategy.fosProxyForGrid)
                    {
                    }
                }
                else
                {
                    errMsg = "Failed to open datasource: " + fm.get_ErrorMsg(fm.LastErrorCode);
                }
            }
            if (success)
            {
                //save directory to the registry
                RegistryTools.SaveSetting("FAD3", "LastOpenedLayerDirectory", Path.GetDirectoryName(fileName));
            }
            return (success, errMsg);
        }

        private MapLayer SetMapLayer(int layerHandle, string layerName, bool Visible,
                                bool ShowInLayerUI, GeoProjection gp, string layerType = "ShapefileClass")
        {
            var mapLayer = new MapLayer(layerHandle, layerName, Visible, ShowInLayerUI);
            mapLayer.LayerType = layerType;
            mapLayer.FileName = _axmap.get_LayerFilename(layerHandle);
            mapLayer.GeoProjectionName = gp.Name;
            mapLayer.LayerPosition = _axmap.get_LayerPosition(layerHandle);

            _layerPropertiesDictionary.Add(layerHandle, mapLayer);
            _axmap.Redraw();
            return mapLayer;
        }

        public int AddLayer(Shapefile sf)
        {
            var h = _axmap.AddLayer(sf, true);
            MapLayer mapLayer;
            if (h >= 0)
            {
                var layerName = Path.GetFileName(sf.Filename);
                _axmap.set_LayerName(h, layerName);
                mapLayer = SetMapLayer(h, layerName, true, true, sf.GeoProjection);

                if (LayerPropertyRead != null)
                {
                    LayerProperty lp = new LayerProperty(h, layerName, true, true, mapLayer.LayerType);
                    LayerPropertyRead(this, lp);
                }
            }
            return h;
        }

        public int AddLayer(MapWinGIS.Image image)
        {
            var h = _axmap.AddLayer(image, true);
            MapLayer mapLayer;
            if (h >= 0)
            {
                var layerName = Path.GetFileName(image.Filename);
                _axmap.set_LayerName(h, layerName);
                mapLayer = SetMapLayer(h, layerName, true, true, image.GeoProjection, "ImageClass");

                if (LayerPropertyRead != null)
                {
                    LayerProperty lp = new LayerProperty(h, layerName, true, true, mapLayer.LayerType);
                    if (_picHeight > 0 && _picWidth > 0)
                    {
                        Bitmap bmp = GetImageThumbnail(image.Filename);
                        mapLayer.ImageThumbnail = bmp;
                        lp.ImageThumb = bmp;
                    }
                    LayerPropertyRead(this, lp);
                }
            }
            return h;
        }

        public int AddLayer(object layer, string layerName, bool visible, bool showInLayerUI)
        {
            int h = 0;
            MapLayer mapLayer;
            GeoProjection gp = new GeoProjection();
            if (h >= 0)
            {
                var layerType = layer.GetType().Name;
                switch (layerType)
                {
                    case "ShapefileClass":
                        h = _axmap.AddLayer((Shapefile)layer, visible);
                        gp = ((Shapefile)layer).GeoProjection;
                        break;
                }

                _axmap.set_LayerName(h, layerName);
                mapLayer = SetMapLayer(h, layerName, true, showInLayerUI, gp);

                if (LayerPropertyRead != null)
                {
                    LayerProperty lp = new LayerProperty(h, layerName, visible, showInLayerUI, mapLayer.LayerType);
                    LayerPropertyRead(this, lp);
                }
            }

            return h;
        }

        private void OnMapLayerAdded(object sender, _DMapEvents_LayerAddedEvent e)
        {
        }
    }
}
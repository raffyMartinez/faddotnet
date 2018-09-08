﻿using AxMapWinGIS;
using MapWinGIS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Xml;

namespace FAD3
{
    public class MapLayersHandler : IDisposable
    {
        private string _fileMapState;
        public bool _disposed;
        private AxMap _axmap;                                                                       //reference to tha map control in the mapping form
        private Dictionary<int, MapLayer> _mapLayerDictionary = new Dictionary<int, MapLayer>();    //contains MapLayers with the layer handle as key
        private MapLayer _currentMapLayer;                                                          //the current layer selected in the map layers form

        public delegate void LayerReadHandler(MapLayersHandler s, LayerProperty e);                 //an event that is raised when a layer from the mapcontrol is retrieved
        public event LayerReadHandler LayerRead;                                                    //in order for the listener is able to add the layer to the layers list

        public delegate void LayerRemovedHandler(MapLayersHandler s, LayerProperty e);
        public event LayerRemovedHandler LayerRemoved;

        public delegate void CurrentLayerHandler(MapLayersHandler s, LayerProperty e);              //event raised when a layer is selected from the list found in the layers form
        public event CurrentLayerHandler CurrentLayer;

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
        public void LayerSymbol(int layerHandle, System.Windows.Forms.PictureBox pic, string layerType)
        {
            if (pic.Image != null) pic.Image.Dispose();
            Rectangle rect = pic.ClientRectangle;
            int w = rect.Width / 4;
            int h = (rect.Height / 4) * 3;

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
                                shp.DefaultDrawingOptions.DrawRectangle(ptr, rect.Width / 3, rect.Height / 4, w, h, shp.DefaultDrawingOptions.LineVisible, rect.Width, rect.Height);
                                break;

                            case ShpfileType.SHP_POLYLINE:
                                shp.DefaultDrawingOptions.DrawLine(ptr, rect.Width / 3, rect.Height / 4, w, h, true, rect.Width, rect.Height);
                                break;
                        }
                        g.ReleaseHdc(ptr);
                        pic.Image = bmp;
                    });

                    break;

                case "ImageClass":
                    if (_mapLayerDictionary[layerHandle].ImageThumbnail == null)
                    {
                        string filename = _axmap.get_Image(layerHandle).Filename;
                        bmp = new Bitmap(w, h);
                        g = Graphics.FromImage(bmp);
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.FillRectangle(Brushes.White, (rect.Width / 5) * 1, rect.Height / 4, w, h);
                        g.DrawImage(new Bitmap(filename), 0, 0, w, h);
                        pic.Image = bmp;
                        _mapLayerDictionary[layerHandle].ImageThumbnail = bmp;
                    }
                    else
                    {
                        pic.Image = _mapLayerDictionary[layerHandle].ImageThumbnail;
                    }
                    break;
            }
        }

        public Dictionary<int, MapLayer> LayerDictionary
        {
            get { return _mapLayerDictionary; }
        }

        public void ClearAllSelections()
        {
            foreach (var item in _mapLayerDictionary)
            {
                if (item.Value.LayerType == "ShapefileClass")
                {
                    _axmap.get_Shapefile(item.Key).SelectNone();
                }
            }
            _axmap.Redraw();
        }

        public MapLayer CurrentMapLayer
        {
            get { return _currentMapLayer; }
        }

        public void set_MapLayer(int layerHandle)
        {
            _currentMapLayer = _mapLayerDictionary[layerHandle];
            //if there are listeners to the event
            if (CurrentLayer != null)
            {
                //fill up the event argument class with the layer item
                LayerProperty lp = new LayerProperty(_currentMapLayer.Handle, _currentMapLayer.Name, _currentMapLayer.Visible, _currentMapLayer.VisibleInLayersUI, _currentMapLayer.LayerType);
                CurrentLayer(this, lp);
            }
        }

        public MapLayer get_MapLayer(int layerHandle)
        {
            return _mapLayerDictionary[layerHandle];
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
                _mapLayerDictionary[layerHandle].LayerPosition = _axmap.get_LayerPosition(layerHandle);
                layerMoved = true;
            }
            return layerMoved;
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="mapControl"></param>
        public MapLayersHandler(AxMap mapControl)
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
            //var rcount = 0;
            //_axmap.get_Shapefile(e.layerHandle).Reproject(_axmap.GeoProjection, ref rcount);
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
                    foreach (var item in _mapLayerDictionary)
                    {
                        item.Value.Dispose();
                    }
                    _mapLayerDictionary = null;
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
                if (_mapLayerDictionary[h].VisibleInLayersUI)
                {
                    //if there is a listener to the event
                    if (LayerRead != null)
                    {
                        //get the corresponding layer item in the dictionary
                        var item = _mapLayerDictionary[h];

                        //fill up the event argument class with the layer item
                        LayerProperty lp = new LayerProperty(item.Handle, item.Name, item.Visible, item.VisibleInLayersUI, item.LayerType);
                        LayerRead(this, lp);
                    }
                }
            }
        }

        public void ClearSelection(int handle)
        {
            try
            {
                _axmap.get_Shapefile(handle).SelectNone();
                _axmap.Redraw();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// Remove a layer using layer name
        /// </summary>
        /// <param name="layerName"></param>
        public void RemoveLayer(string layerName)
        {
            foreach (var item in _mapLayerDictionary)
            {
                if (item.Value.Name == layerName)
                {
                    RemoveLayer(item.Key);
                    break;
                }
            }
        }

        /// <summary>
        /// Removes a layer using layer handle and raises a Layer removed event.
        /// </summary>
        /// <param name="layerHandle"></param>
        public void RemoveLayer(int layerHandle)
        {
            _mapLayerDictionary[layerHandle].Dispose();
            _mapLayerDictionary.Remove(layerHandle);
            _axmap.RemoveLayer(layerHandle);
            _axmap.Redraw();

            //fire the layer deleted event
            if (LayerRemoved != null)
            {
                LayerProperty lp = new LayerProperty(layerHandle, layerRemoved: true);
                LayerRemoved(this, lp);
            }
        }

        private void RemoveInMemoryLayers()
        {
            for (int n = 0; n < _axmap.NumLayers; n++)
            {
                var h = _axmap.get_LayerHandle(n);
                if (_mapLayerDictionary[h].FileName.Length == 0)
                {
                    RemoveLayer(h);
                }
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
            if (_mapLayerDictionary.ContainsKey(layerHandle))
            {
                var ly = _mapLayerDictionary[layerHandle];
                ly.Name = layerName;
                ly.Visible = visible;
                ly.VisibleInLayersUI = isShown;
            }

            _axmap.set_LayerName(layerHandle, layerName);
            _axmap.set_LayerVisible(layerHandle, visible);
            _axmap.Redraw();
        }

        /// <summary>
        /// Handles the opening of map layer files from a file open dialog
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
                        var prjFile = $@"{Path.GetDirectoryName(fileName)}\{Path.GetFileNameWithoutExtension(fileName)}.prj";
                        var worldFile = $"{fileName}w";
                        if (File.Exists(prjFile) && File.Exists(worldFile))
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
                        else
                        {
                            errMsg = $"{fileName} does not have a projection or world file";
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

        /// <summary>
        /// Sets up a mapLayer object from a newly added map layer and adds it to the layers dictionary
        /// </summary>
        /// <param name="layerHandle"></param>
        /// <param name="layerName"></param>
        /// <param name="Visible"></param>
        /// <param name="ShowInLayerUI"></param>
        /// <param name="gp"></param>
        /// <param name="layerType"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private MapLayer SetMapLayer(int layerHandle, string layerName, bool Visible,
                                bool ShowInLayerUI, GeoProjection gp,
                                string layerType = "ShapefileClass", string fileName = "")
        {
            var mapLayer = new MapLayer(layerHandle, layerName, Visible, ShowInLayerUI);
            mapLayer.LayerType = layerType;
            mapLayer.FileName = _axmap.get_LayerFilename(layerHandle);
            if (mapLayer.FileName.Length == 0)
            {
                mapLayer.FileName = fileName;
            }

            mapLayer.GeoProjectionName = gp.Name;
            mapLayer.LayerPosition = _axmap.get_LayerPosition(layerHandle);
            mapLayer.LayerObject = _axmap.get_GetObject(layerHandle);

            _mapLayerDictionary.Add(layerHandle, mapLayer);
            _axmap.Redraw();
            set_MapLayer(layerHandle);
            return mapLayer;
        }

        /// <summary>
        /// handles a shapefile added to the map using file open dialog
        /// </summary>
        /// <param name="sf"></param>
        /// <returns></returns>
        public int AddLayer(Shapefile sf, string layerName = "")
        {
            var h = _axmap.AddLayer(sf, true);
            MapLayer mapLayer;
            if (h >= 0)
            {
                if (layerName.Length == 0)
                {
                    layerName = Path.GetFileName(sf.Filename);
                }
                _axmap.set_LayerName(h, layerName);
                mapLayer = SetMapLayer(h, layerName, true, true, sf.GeoProjection, "ShapefileClass", sf.Filename);

                if (LayerRead != null)
                {
                    LayerProperty lp = new LayerProperty(h, layerName, true, true, mapLayer.LayerType);
                    LayerRead(this, lp);
                }
            }
            return h;
        }

        /// <summary>
        /// handles an image added to the map using file open dialog
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public int AddLayer(MapWinGIS.Image image, string layerName = "")
        {
            var h = _axmap.AddLayer(image, true);
            MapLayer mapLayer;
            if (h >= 0)
            {
                if (layerName.Length == 0)
                {
                    layerName = Path.GetFileName(image.Filename);
                }

                _axmap.set_LayerName(h, layerName);
                mapLayer = SetMapLayer(h, layerName, true, true, image.GeoProjection, "ImageClass", image.Filename);

                if (LayerRead != null)
                {
                    LayerProperty lp = new LayerProperty(h, layerName, true, true, mapLayer.LayerType);
                    LayerRead(this, lp);
                }
            }
            return h;
        }

        /// <summary>
        /// handles dynmically created shapefiles added to the map
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="layerName"></param>
        /// <param name="visible"></param>
        /// <param name="showInLayerUI"></param>
        /// <param name="layerHandle"></param>
        /// <returns></returns>
        public int AddLayer(object layer, string layerName, bool visible, bool showInLayerUI, string fileName = "")
        {
            int h = 0;
            MapLayer mapLayer;
            GeoProjection gp = new GeoProjection();

            var layerType = layer.GetType().Name;

            switch (layerType)
            {
                case "ShapefileClass":
                    h = _axmap.AddLayer((Shapefile)layer, visible);
                    gp = ((Shapefile)layer).GeoProjection;
                    break;

                case "ImageClass":
                    h = _axmap.AddLayer((MapWinGIS.Image)layer, visible);
                    gp = ((MapWinGIS.Image)layer).GeoProjection;
                    break;
            }

            _axmap.set_LayerName(h, layerName);
            mapLayer = SetMapLayer(h, layerName, visible, showInLayerUI, gp, layerType, fileName);

            if (LayerRead != null)
            {
                LayerProperty lp = new LayerProperty(h, layerName, visible, showInLayerUI, mapLayer.LayerType);
                LayerRead(this, lp);
            }

            return h;
        }

        /// <summary>
        /// save additional map options including AvoidCollision which is not saved by SaveMapState
        /// </summary>
        private void SaveOtherMapOptions()
        {
            for (int n = 0; n < _axmap.NumLayers; n++)
            {
                var h = _axmap.get_LayerHandle(n);
                if (_mapLayerDictionary[h].LayerType == "ShapefileClass")
                {
                    var sf = _axmap.get_Shapefile(h);
                    if (sf.Labels.Count > 0)
                    {
                        ShapefileLabelHandler.SaveLabelParameters(_fileMapState, h, sf.Labels.AvoidCollisions);
                    }
                }
            }
        }

        /// <summary>
        /// Check for blank filenames in the mapstatefile. Blank filenames happen when a layer is reprojected
        /// and is then saved in the mapstate file.
        /// </summary>
        private void CheckFileNameInMapStateFile()
        {
            var doc = new XmlDocument();
            var n = 0;
            doc.Load(_fileMapState);
            foreach (XmlNode ly in doc.DocumentElement.SelectNodes("//Layer"))
            {
                if (ly.Attributes["Filename"].Value.Length == 0)
                {
                    ly.Attributes["Filename"].Value = _mapLayerDictionary[_axmap.get_LayerHandle(n)].FileName;
                }
                n++;
            }
            doc.Save(_fileMapState);
        }

        /// <summary>
        /// saves the map state to an xml file
        /// </summary>
        public void SaveMapState()
        {
            RemoveInMemoryLayers();
            if (global.MappingMode == global.fad3MappingMode.defaultMode
                && _axmap.SaveMapState(_fileMapState, false, true))
            {
                SaveOtherMapOptions();
                CheckFileNameInMapStateFile();
            }
        }

        /// <summary>
        /// Load map layers from XML file generated by axmap.SaveMapState.
        ///
        /// Layers are added to the map and is followed by restoring the map extent.
        /// The first added layer automatically sets the map control's projection.
        /// </summary>
        /// <param name="restoreMapState">
        /// When restoreMapState:true, map state is restored
        /// We use restoreMapState:false to load the layers but not restore axMap extent.
        /// </param>
        public void LoadMapState(bool restoreMapState = true)
        {
            _fileMapState = $@"{global.ApplicationPath}\mapstate";
            if (File.Exists(_fileMapState))
            {
                var doc = new XmlDocument();
                var proceed = true;
                var fileName = "";
                try
                {
                    doc.Load(_fileMapState);
                }
                catch (XmlException ex)
                {
                    Logger.Log(ex.Message, "MapLayersHandler", "LoadMapState");
                    proceed = false;
                }
                if (proceed)
                {
                    foreach (XmlNode ly in doc.DocumentElement.SelectNodes("//Layer"))
                    {
                        fileName = ly.Attributes["Filename"].Value;
                        if (ly.Attributes["LayerType"].Value == "Shapefile")
                        {
                            var sf = new Shapefile();
                            if (sf.Open(fileName))
                            {
                                //the first layer added will determine the projection of the map control
                                //var h = AddLayer(sf, ly.Attributes["LayerName"].Value, ly.Attributes["LayerVisible"].Value == "1", true, fileName);
                                var h = AddLayer(sf, ly.Attributes["LayerName"].Value);
                                _axmap.get_Shapefile(h).Deserialize(false, ly.InnerXml);
                                ShapefileLabelHandler sflh = new ShapefileLabelHandler(_axmap.get_Shapefile(h));
                                foreach (XmlNode child in ly.FirstChild.ChildNodes)
                                {
                                    if (child.Name == "LabelsClass" && child.Attributes["Generated"].Value == "1")
                                    {
                                        sflh.LabelShapefile(child.OuterXml);
                                    }
                                }
                            }
                        }
                        else if (ly.Attributes["LayerType"].Value == "Image")
                        {
                            //code when layertype is image
                            var img = new MapWinGIS.Image();
                            if (img.Open(fileName))
                            {
                                var h = AddLayer(img, ly.Attributes["LayerName"].Value);
                                //var h = AddLayer(img, ly.Attributes["LayerName"].Value, ly.Attributes["LayerVisible"].Value == "1", true, fileName);
                            }
                        }
                    }
                    if (restoreMapState)
                    {
                        //We restore saved extent of the map but not the projection. Since layers
                        //were already added to the map, the first layer sets the map's projection.
                        foreach (XmlNode ms in doc.DocumentElement.SelectNodes("//MapState "))
                        {
                            var ext = new Extents();
                            ext.SetBounds(
                                double.Parse(ms.Attributes["ExtentsLeft"].Value),
                                double.Parse(ms.Attributes["ExtentsBottom"].Value),
                                0,
                                double.Parse(ms.Attributes["ExtentsRight"].Value),
                                double.Parse(ms.Attributes["ExtentsTop"].Value),
                                0);
                            _axmap.Extents = ext;
                            _axmap.ExtentPad = double.Parse(ms.Attributes["ExtentsPad"].Value);
                        }
                    }
                }
            }
            else
            {
                File.Create(_fileMapState);
            }
        }

        private void OnMapLayerAdded(object sender, _DMapEvents_LayerAddedEvent e)
        {
        }
    }
}
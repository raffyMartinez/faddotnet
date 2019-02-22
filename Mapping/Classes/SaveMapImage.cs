﻿using AxMapWinGIS;
using MapWinGIS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FAD3
{
    /// <summary>
    /// Helper class for saving a map to an image
    /// </summary>
    public class SaveMapImage : IDisposable
    {
        private bool _disposed;
        private AxMap _axMap;
        private Shapefile _shapeFileMask;
        private bool _saveToTempFile;

        private string _fileName;
        private double _dpi;
        public MapLayersHandler MapLayersHandler { get; set; }
        public bool Reset { get; set; }
        private int _handleGridBoundary;
        private int _handleLabels;
        private int _handleMajorGrid;
        private int _handleMinorGrid;
        private Dictionary<int, int> _frameWidthDict = new Dictionary<int, int>();
        public bool PreviewImage { get; set; }
        public string TempMapFileName { get { return _fileName; } }

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
                    _frameWidthDict.Clear();
                    _frameWidthDict = null;
                }
                if (_shapeFileMask != null)
                {
                    _shapeFileMask.EditClear();
                    _shapeFileMask.Close();
                    _shapeFileMask = null;
                }
                _axMap = null;
                _disposed = true;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="DPI"></param>
        /// <param name="mapControl"></param>
        public SaveMapImage(string fileName, double DPI, AxMap mapControl)
        {
            _fileName = fileName;
            _dpi = DPI;
            _axMap = mapControl;
        }

        public SaveMapImage(AxMap mapControl)
        {
            _axMap = mapControl;
        }

        /// <summary>
        /// Helper function to Set numeric properties of labels to fit the desired DPI of the output map. When Reset is true,
        /// scales back these properties to fit the screen resolution.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private double AdjustLabelProperty(double value)
        {
            if (Reset)
            {
                value /= (_dpi / 96);
            }
            else
            {
                value *= (_dpi / 96);
            }
            return value;
        }

        private int AdjustLabelProperty(int value)
        {
            if (Reset)
            {
                value /= ((int)_dpi / 96);
            }
            else
            {
                value *= ((int)_dpi / 96);
            }
            return value;
        }

        /// <summary>
        /// Sets different numeric properties of labels to fit the desired DPI of the output map
        /// </summary>
        /// <param name="labels"></param>
        private void AdjustLabelProperties(int layerHandle)
        {
            _axMap.get_Shapefile(layerHandle).Labels.With(lbl =>
           {
               lbl.VerticalPosition = tkVerticalPosition.vpAboveParentLayer;
               lbl.FontSize = AdjustLabelProperty(lbl.FontSize);
               lbl.OffsetX = AdjustLabelProperty(lbl.OffsetX);
               lbl.OffsetY = AdjustLabelProperty(lbl.OffsetY);
               lbl.FontOutlineWidth = AdjustLabelProperty(lbl.FontOutlineWidth);
               lbl.FramePaddingX = AdjustLabelProperty(lbl.FramePaddingX);
               lbl.FramePaddingY = AdjustLabelProperty(lbl.FramePaddingY);
               lbl.ShadowOffsetX = AdjustLabelProperty(lbl.ShadowOffsetX);
               lbl.ShadowOffsetY = AdjustLabelProperty(lbl.ShadowOffsetY);

               if (Reset)
               {
                   lbl.FrameOutlineWidth = _frameWidthDict[layerHandle];
                   _frameWidthDict[layerHandle] = lbl.FrameOutlineWidth;
               }
               else
               {
                   if (!_frameWidthDict.ContainsKey(layerHandle))
                   {
                       _frameWidthDict.Add(layerHandle, lbl.FrameOutlineWidth);
                   }
                   lbl.FrameOutlineWidth = (int)AdjustLabelProperty(lbl.FrameOutlineWidth);
               }

               if (lbl.NumCategories > 0)
               {
                   for (int n = 0; n < lbl.NumCategories; n++)
                   {
                       lbl.Category[n].FontSize = AdjustLabelProperty(lbl.Category[n].FontSize);
                       lbl.Category[n].OffsetX = AdjustLabelProperty(lbl.Category[n].OffsetX);
                       lbl.Category[n].OffsetY = AdjustLabelProperty(lbl.Category[n].OffsetY);
                       lbl.Category[n].FontOutlineWidth = AdjustLabelProperty(lbl.Category[n].FontOutlineWidth);
                       lbl.Category[n].FramePaddingX = AdjustLabelProperty(lbl.Category[n].FramePaddingX);
                       lbl.Category[n].FramePaddingY = AdjustLabelProperty(lbl.Category[n].FramePaddingY);
                       lbl.Category[n].ShadowOffsetX = AdjustLabelProperty(lbl.Category[n].ShadowOffsetX);
                       lbl.Category[n].ShadowOffsetY = AdjustLabelProperty(lbl.Category[n].ShadowOffsetY);
                       lbl.Category[n].FrameOutlineWidth = AdjustLabelProperty(lbl.Category[n].FrameOutlineWidth);
                   }
               }
           });
        }

        /// <summary>
        /// Sets the line thickness and point size of layers to match the desired DPI of output map
        /// </summary>
        private void AdjustFeatureSize()
        {
            for (int n = 0; n < _axMap.NumLayers; n++)
            {
                var h = _axMap.get_LayerHandle(n);
                if (this.MapLayersHandler.get_MapLayer(h).LayerType == "ShapefileClass")
                //if (_axMap.get_GetObject(h).GetType().Name == "ShapefileClass")
                {
                    var categoryCount = _axMap.get_Shapefile(h).Categories.Count;

                    if (_axMap.get_LayerVisible(h))
                    {
                        AdjustLabelProperties(h);

                        _axMap.get_Shapefile(h).DefaultDrawingOptions.With(ddo =>
                        {
                            if (Reset)
                            {
                                if (categoryCount > 0)
                                {
                                    for (int y = 0; y < categoryCount; y++)
                                    {
                                        _axMap.get_Shapefile(h).Categories.Item[y].DrawingOptions.LineWidth /= (float)(_dpi / 96);
                                        _axMap.get_Shapefile(h).Categories.Item[y].DrawingOptions.PointSize /= (float)(_dpi / 96);
                                        _axMap.get_Shapefile(h).Categories.Item[y].DrawingOptions.VerticesSize /= (int)(_dpi / 96);
                                    }
                                }
                                else
                                {
                                    ddo.PointSize /= (float)(_dpi / 96);
                                    ddo.LineWidth /= (float)(_dpi / 96);
                                    ddo.VerticesSize /= (int)(_dpi / 96);
                                }
                            }
                            else
                            {
                                if (categoryCount > 0)
                                {
                                    for (int y = 0; y < categoryCount; y++)
                                    {
                                        _axMap.get_Shapefile(h).Categories.Item[y].DrawingOptions.LineWidth *= (float)(_dpi / 96);
                                        _axMap.get_Shapefile(h).Categories.Item[y].DrawingOptions.PointSize *= (float)(_dpi / 96);
                                        _axMap.get_Shapefile(h).Categories.Item[y].DrawingOptions.VerticesSize *= (int)(_dpi / 96);
                                    }
                                }
                                else
                                {
                                    ddo.PointSize *= (float)(_dpi / 96);
                                    ddo.LineWidth *= (float)(_dpi / 96);
                                    ddo.VerticesSize *= (int)(_dpi / 96);
                                }
                            }
                        });
                    }
                }
            }
        }

        public bool SaveToTempFile(double dpi = 96)
        {
            _dpi = dpi;
            _saveToTempFile = true;
            AdjustFeatureSize();
            return SaveMaptoImage();
        }

        /// <summary>
        /// Initiates saving of a map to an image
        /// </summary>
        /// <returns></returns>
        public bool Save(bool SaveGrid25 = true)
        {
            //MapLayersHandler.SaveLayerSettingsToXML();
            _saveToTempFile = false;
            AdjustFeatureSize();
            if (SaveGrid25)
            {
                return SaveGrid25MapToImage();
            }
            else
            {
                return SaveMaptoImage();
            }
        }

        /// <summary>
        ///Creates a mask that will hide features that fall outside the extent of the fishing grid
        /// </summary>
        /// <returns></returns>
        private void set_Mask()
        {
            _shapeFileMask = new Shapefile();
            var sf = new Shapefile();
            if (sf.CreateNew("", ShpfileType.SHP_POLYGON))
            {
                var ext = _axMap.Extents;

                //ef is expansion factor
                var ef = (ext.xMax - ext.xMin) * 0.01;

                ext.SetBounds(ext.xMin - ef, ext.yMin - ef, 0, ext.xMax + ef, ext.yMax + ef, 0);

                //var shp = _axMap.Extents.ToShape().Clip(_axMap.get_Shapefile(_handleGridBoundary).Extents.ToShape(), tkClipOperation.clDifference);
                var shp = ext.ToShape().Clip(_axMap.get_Shapefile(_handleGridBoundary).Extents.ToShape(), tkClipOperation.clDifference);
                sf.EditAddShape(shp);
                sf.DefaultDrawingOptions.LineVisible = false;
                sf.DefaultDrawingOptions.FillColor = new Utils().ColorByName(tkMapColor.White);
            }

            _shapeFileMask = sf;
        }

        /// <summary>
        /// Saves a map image to a file
        /// </summary>
        /// <returns></returns>
        private bool SaveMaptoImage()
        {
            var layerDictionary = MapLayersHandler.LayerDictionary;
            foreach (KeyValuePair<int, MapLayer> kv in layerDictionary)
            {
                if (kv.Value.Visible)
                {
                    if (_axMap.get_GetObject(kv.Key).GetType().Name == "ShapefileClass")
                    {
                        _axMap.get_Shapefile(kv.Key).Labels.VerticalPosition = tkVerticalPosition.vpAboveParentLayer;
                    }
                }
            }
            return SaveMapHelper(null);
        }

        /// <summary>
        /// Actual functionality to save a map image to a file
        /// </summary>
        /// <param name="handleMask"> shapefile mask layer</param>
        /// <returns></returns>
        private bool SaveMapHelper(int? handleMask)
        {
            //we now compute the width (w) that corresponds to a map whose width fits the required dpi
            var ext = _axMap.Extents;
            var w = ((double)_axMap.Width) * ((double)_dpi / 96);

            //create an image whose width (w) will result in a map whose width in pixels fits the the required dpi
            var img = _axMap.SnapShot3(ext.xMin, ext.xMax, ext.yMax, ext.yMin, (int)w);

            //restore the map to its previous state by removing the mask and setting Reset to true
            if (handleMask != null) MapLayersHandler.RemoveLayer((int)handleMask);
            Reset = true;
            AdjustFeatureSize();

            if (_saveToTempFile)
            {
                _fileName = $@"{global.AppPath}\tempMap.jpg";
                global.ListTemporaryFile(_fileName);
                if (img.Save(_fileName))
                {
                    return true;
                }
                else
                {
                    _fileName = $@"{global.AppPath}\tempMap1.jpg";
                    return img.Save(_fileName);
                }
            }
            else
            {
                //specify filename of projection file for the image
                var prjFileName = _fileName.Replace(Path.GetExtension(_fileName), ".prj");

                //save the image to disk and create a worldfile. Image format is specified by USE_FILE_EXTENSION.
                //also save the projection file
                if (img.Save(_fileName, WriteWorldFile: true, FileType: ImageType.USE_FILE_EXTENSION) && _axMap.GeoProjection.WriteToFile(prjFileName))
                {
                    //show the image file using the default image viewer
                    if (PreviewImage) Process.Start(_fileName);
                    img = null;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Saves a grid25 fishing map image  to the filename specified
        /// </summary>
        /// <returns></returns>
        private bool SaveGrid25MapToImage()
        {
            var layerDictionary = MapLayersHandler.LayerDictionary;
            List<int> layersOnTop = new List<int>();

            //sort the dictionary according to the weight of the layer. Layers with bigger weights are placed below those with smaller weights.
            foreach (KeyValuePair<int, MapLayer> kv in layerDictionary.OrderByDescending(r => r.Value.LayerWeight).Take(layerDictionary.Count))
            {
                if (kv.Value.Visible)
                {
                    //the layers with heavier weights are moved to the top. As the loop progresses, layers with ligter weights are placed on
                    //top. When the loop finishes, the lightest layer is on top while the heaviest layer is at the bottom.
                    if (kv.Value.LayerWeight != null)
                    {
                        _axMap.MoveLayerTop(_axMap.get_LayerPosition(kv.Value.Handle));
                    }
                    else
                    {
                        //layers whose weights are null will be placed below layers with weight values
                        //_axMap.get_Shapefile(kv.Key).Labels.VerticalPosition = tkVerticalPosition.vpAboveAllLayers;
                        _axMap.get_Shapefile(kv.Key).Labels.VerticalPosition = tkVerticalPosition.vpAboveParentLayer;
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

                    if (kv.Value.KeepOnTop)
                    {
                        layersOnTop.Add(_axMap.get_LayerPosition(kv.Value.Handle));
                    }
                }
            }

            foreach (int lyr in layersOnTop)
            {
                _axMap.MoveLayerTop(lyr);
            }

            //add a mask to the map control
            set_Mask();
            var handleMask = MapLayersHandler.AddLayer(_shapeFileMask, "Grid mask", true, false);

            //move the mask layer to the top
            _axMap.MoveLayerTop(_axMap.get_LayerPosition(handleMask));

            //move the boundary layer on top so that it won't be hidden by the mask
            _axMap.MoveLayerTop(_axMap.get_LayerPosition(_handleGridBoundary));

            //make sure that map labels are placed on top of all layers
            _axMap.get_Shapefile(_handleLabels).Labels.VerticalPosition = tkVerticalPosition.vpAboveAllLayers;

            _axMap.get_Shapefile(_handleLabels).Labels.AvoidCollisions = false;

            return SaveMapHelper(handleMask);
        }
    }
}
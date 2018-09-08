﻿using System;
using System.Drawing;
using MapWinGIS;

namespace FAD3
{
    public class MapLayer : IDisposable
    {
        public string Name { get; set; }
        public bool Visible { get; set; }
        public bool VisibleInLayersUI { get; set; }
        public int Handle { get; set; }
        public string FileName { get; set; }
        public string GeoProjectionName { get; set; }
        public int LayerPosition { get; set; }
        public Bitmap ImageThumbnail { get; set; }
        public string LayerType { get; set; }
        public object LayerObject { get; set; }
        private bool _disposed;
        public int[] SelectedIndexes { get; set; }
        public bool IsGraticule { get; set; }
        public bool IsFishingGrid { get; set; }

        public MapLayer(int handle, string name, bool visible, bool visibleInLayersUI)
        {
            Handle = handle;
            Name = name;
            Visible = visible;
            VisibleInLayersUI = visibleInLayersUI;
            IsFishingGrid = false;
        }

        public void Save(string fileName)
        {
            if (LayerType == "ShapefileClass")
            {
                ((Shapefile)LayerObject).With(sf =>
                {
                    sf.SaveAs(fileName + ".shp");
                    sf.GeoProjection.WriteToFile(fileName + ".prj");
                });
            }
            else
            {
                //for image type, possibly
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
                if (ImageThumbnail != null)
                {
                    ImageThumbnail.Dispose();
                }
                ImageThumbnail = null;
                LayerObject = null;
                _disposed = true;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

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

        public MapLayer(int handle, string name, bool visible, bool visibleInLayersUI)
        {
            Handle = handle;
            Name = name;
            Visible = visible;
            VisibleInLayersUI = visibleInLayersUI;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FAD3
{
    public class MapLayer
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

        public MapLayer(int handle, string name, bool visible, bool visibleInLayersUI)
        {
            Handle = handle;
            Name = name;
            Visible = visible;
            VisibleInLayersUI = visibleInLayersUI;
        }
    }
}
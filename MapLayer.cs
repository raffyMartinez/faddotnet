using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FAD3
{
    public class MapLayer
    {
        private string _layerType;
        public string Name { get; set; }
        public bool Visible { get; set; }
        public bool VisibleInLayersUI { get; set; }
        public int Handle { get; set; }
        public string FileName { get; set; }
        public string GeoProjectionName { get; set; }

        public string LayerType
        {
            get { return _layerType; }
            set { _layerType = value; }
        }

        public MapLayer(int handle, string name, bool visible, bool visibleInLayersUI)
        {
            Handle = handle;
            Name = name;
            Visible = visible;
            VisibleInLayersUI = visibleInLayersUI;
        }
    }
}
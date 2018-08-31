using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FAD3
{
    public class LayerProperty : EventArgs
    {
        public int LayerHandle { get; }
        public string LayerName { get; }
        public bool ShowInLayerUI { get; }
        public bool LayerVisible { get; }
        public bool LayerDeleted { get; }
        public string LayerType { get; }
        public Bitmap ImageThumb { get; set; }

        public LayerProperty(int layerHandle, string layerName, bool layerVIsible, bool showInLayerUI, string layerType)
        {
            LayerHandle = layerHandle;
            LayerName = layerName;
            LayerVisible = layerVIsible;
            ShowInLayerUI = showInLayerUI;
            LayerType = layerType;
        }

        public LayerProperty(int layerHandle, bool layerDeleted)
        {
            LayerHandle = layerHandle;
            LayerDeleted = LayerDeleted;
        }
    }
}
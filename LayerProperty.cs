using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FAD3
{
    public class LayerProperty : EventArgs
    {
        public int LayerHandle { get; }
        public string LayerName { get; }
        public bool ShowInLayerUI { get; }
        public bool LayerVisible { get; }
        public bool LayerDeleted { get; }

        public LayerProperty(int layerHandle, string layerName, bool layerVIsible, bool showInLayerUI)
        {
            LayerHandle = layerHandle;
            LayerName = layerName;
            LayerVisible = layerVIsible;
            ShowInLayerUI = showInLayerUI;
        }

        public LayerProperty(int layerHandle, bool layerDeleted)
        {
            LayerHandle = layerHandle;
            LayerDeleted = LayerDeleted;
        }
    }
}
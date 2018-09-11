using System;

namespace FAD3
{
    public class LayerEventArg : EventArgs
    {
        public int LayerHandle { get; }
        public string LayerName { get; }
        public bool ShowInLayerUI { get; }
        public bool LayerVisible { get; }
        public bool LayerRemoved { get; }
        public string LayerType { get; }
        public bool LayerSaved { get; }
        public string FileName { get; }
        public int SelectedIndex { get; set; }
        public string Action { get; set; }
        public string MapTitle { get; set; }
        public int[] SelectedIndexes { get; set; }

        public LayerEventArg(int layerHandle, string layerName, bool layerVIsible, bool showInLayerUI, string layerType)
        {
            LayerHandle = layerHandle;
            LayerName = layerName;
            LayerVisible = layerVIsible;
            ShowInLayerUI = showInLayerUI;
            LayerType = layerType;
        }

        public LayerEventArg(int layerHandle, int[] selectedIndexes)
        {
            LayerHandle = layerHandle;
            SelectedIndexes = selectedIndexes;
        }

        public LayerEventArg(int layerHandle, string mapTitle)
        {
            LayerHandle = layerHandle;
            MapTitle = mapTitle;
        }

        public LayerEventArg(int layerHandle, bool layerRemoved)
        {
            LayerHandle = layerHandle;
            LayerRemoved = layerRemoved;
        }

        public LayerEventArg(int layerHandle, bool layerSaved, string fileName)
        {
            LayerHandle = layerHandle;
            LayerSaved = layerSaved;
            FileName = fileName;
        }
    }
}
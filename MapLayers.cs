using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxMapWinGIS;
using MapWinGIS;

namespace FAD3
{
    public class MapLayers
    {
        private AxMap _axmap;
        private Dictionary<int, (string layerName, bool layerVisible, bool shownInLayer)> _layerPropertiesDictionary = new Dictionary<int, (string layerName, bool layerVisible, bool shownInLayer)>();

        public AxMap MapControl
        {
            get { return _axmap; }
        }

        public delegate void LayerPropertyReadHandler(MapLayers s, LayerProperty e);

        public event LayerPropertyReadHandler LayerPropertyRead;

        public delegate void LayerDeletedHandler(MapLayers s, LayerProperty e);

        public event LayerDeletedHandler LayerDeleted;

        public MapLayers(AxMap mapControl)
        {
            _axmap = mapControl;
            _axmap.LayerAdded += OnMapLayerAdded;
        }

        public void ReadLayers()
        {
            foreach (var item in _layerPropertiesDictionary)
            {
                if (LayerPropertyRead != null)
                {
                    LayerProperty lp = new LayerProperty(item.Key, item.Value.layerName, item.Value.layerVisible, item.Value.shownInLayer);
                    LayerPropertyRead(this, lp);
                }
            }
        }

        public void RemoveLayer(int layerHandle)
        {
            _layerPropertiesDictionary.Remove(layerHandle);
            _axmap.RemoveLayer(layerHandle);
            LayerProperty lp = new LayerProperty(layerHandle, layerDeleted: true);
            LayerDeleted(this, lp);
        }

        public void EditLayer(int layerHandle, string layerName, bool visible, bool isShown = true)
        {
            if (_layerPropertiesDictionary.ContainsKey(layerHandle))
            {
                _layerPropertiesDictionary.Remove(layerHandle);
            }
            _layerPropertiesDictionary.Add(layerHandle, (layerName, visible, isShown));

            if (LayerPropertyRead != null)
            {
                LayerProperty lp = new LayerProperty(layerHandle, layerName, visible, isShown);
            }

            _axmap.set_LayerName(layerHandle, layerName);
            _axmap.set_LayerVisible(layerHandle, visible);
            _axmap.Redraw();
        }

        public int AddLayer(object layer, string layerName, bool visible, bool showInLayerUI)
        {
            int h = 0;
            switch (layer.GetType().Name)
            {
                case "ShapefileClass":
                    h = _axmap.AddLayer((Shapefile)layer, visible);
                    break;
            }

            _axmap.set_LayerName(h, layerName);
            _layerPropertiesDictionary.Add(h, (layerName, visible, showInLayerUI));
            _axmap.Redraw();

            if (LayerPropertyRead != null)
            {
                LayerProperty lp = new LayerProperty(h, layerName, visible, showInLayerUI);
                LayerPropertyRead(this, lp);
            }

            return h;
        }

        private void OnMapLayerAdded(object sender, _DMapEvents_LayerAddedEvent e)
        {
        }
    }
}
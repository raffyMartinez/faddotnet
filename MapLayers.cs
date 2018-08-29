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
        private Dictionary<int, MapLayer> _layerPropertiesDictionary = new Dictionary<int, MapLayer>();

        public delegate void LayerPropertyReadHandler(MapLayers s, LayerProperty e);

        public event LayerPropertyReadHandler LayerPropertyRead;

        public delegate void LayerDeletedHandler(MapLayers s, LayerProperty e);

        public event LayerDeletedHandler LayerDeleted;

        public AxMap MapControl
        {
            get { return _axmap; }
        }

        public Dictionary<int, MapLayer> LayerDictionary
        {
            get { return _layerPropertiesDictionary; }
        }

        public MapLayer get_MapLayer(int layerHandle)
        {
            return _layerPropertiesDictionary[layerHandle];
        }

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
                    LayerProperty lp = new LayerProperty(item.Key, item.Value.Name, item.Value.Visible, item.Value.VisibleInLayersUI);
                    LayerPropertyRead(this, lp);
                }
            }
        }

        public void RemoveLayer(int layerHandle)
        {
            _layerPropertiesDictionary.Remove(layerHandle);
            _axmap.RemoveLayer(layerHandle);
            if (LayerDeleted != null)
            {
                LayerProperty lp = new LayerProperty(layerHandle, layerDeleted: true);
                LayerDeleted(this, lp);
            }
        }

        public void EditLayer(int layerHandle, string layerName, bool visible, bool isShown = true)
        {
            if (_layerPropertiesDictionary.ContainsKey(layerHandle))
            {
                var ly = _layerPropertiesDictionary[layerHandle];
                ly.Name = layerName;
                ly.Visible = visible;
                ly.VisibleInLayersUI = isShown;
            }

            _axmap.set_LayerName(layerHandle, layerName);
            _axmap.set_LayerVisible(layerHandle, visible);
            _axmap.Redraw();
        }

        public int AddLayer(object layer, string layerName, bool visible, bool showInLayerUI)
        {
            int h = 0;
            var layerType = layer.GetType().Name;
            var geoProjectionName = "";
            switch (layerType)
            {
                case "ShapefileClass":
                    h = _axmap.AddLayer((Shapefile)layer, visible);
                    geoProjectionName = ((Shapefile)layer).GeoProjection.Name;
                    break;
            }

            _axmap.set_LayerName(h, layerName);
            var mapLayer = new MapLayer(h, layerName, visible, showInLayerUI);
            mapLayer.LayerType = layerType;
            mapLayer.FileName = _axmap.get_LayerFilename(h);
            mapLayer.GeoProjectionName = geoProjectionName;
            _layerPropertiesDictionary.Add(h, mapLayer);
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
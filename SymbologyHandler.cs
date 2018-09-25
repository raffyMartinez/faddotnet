using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapWinGIS;
using System.Xml;

namespace FAD3
{
    public class SymbologyHandler
    {
        private MapLayer _mapLayer;
        private Shapefile _shapeFile;

        public SymbologyHandler(MapLayer mapLayer)
        {
            _mapLayer = mapLayer;
            _shapeFile = _mapLayer.LayerObject as Shapefile;
        }

        public void SymbolizeLayer(string symbologyXML)
        {
            _shapeFile.Deserialize(LoadSelection: false, symbologyXML);
            //we convert string labelXML to an xml object
            var doc = new XmlDocument();
            doc.LoadXml(symbologyXML);
        }
    }
}
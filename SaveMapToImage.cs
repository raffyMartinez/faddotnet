using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapWinGIS;
using AxMapWinGIS;

namespace FAD3
{
    public class SaveMapToImage
    {
        private string _fileName;
        private int _dpi;
        private AxMap _axMap;
        public Dictionary<int, MapLayer> MapLayerDictionary { get; set; }

        public SaveMapToImage(string fileName, int DPI, AxMap mapControl)
        {
            _fileName = fileName;
            _dpi = DPI;
            _axMap = mapControl;
        }

        public bool Save()
        {
            return true;
        }
    }
}
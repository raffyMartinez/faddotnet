using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxMapWinGIS;
using MapWinGIS;

namespace FAD3
{
    public class SaveMapImage : IDisposable
    {
        private bool _disposed;
        private AxMap _axMap;
        private Shapefile _shapeFileMask;

        private string _fileName;
        private int _dpi;
        public Dictionary<int, MapLayer> MapLayerDictionary { get; set; }

        public SaveMapImage(string fileName, int DPI, AxMap mapControl)
        {
            _fileName = fileName;
            _dpi = DPI;
            _axMap = mapControl;
        }

        public bool Save()
        {
            return true;
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
                if (_shapeFileMask != null)
                {
                    _shapeFileMask.Close();
                    _shapeFileMask = null;
                }
                _axMap = null;
                _disposed = true;
            }
        }
    }
}
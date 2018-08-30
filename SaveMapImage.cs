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
        public string FileName { get; set; }
        public int dpi { get; set; }
        public int _hGraticule { get; set; }
        public Shapefile _shapeFileMask;

        public SaveMapImage(AxMap mapControl)
        {
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
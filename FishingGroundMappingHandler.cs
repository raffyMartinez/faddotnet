using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapWinGIS;

namespace FAD3
{
    public class FishingGroundMappingHandler : IDisposable
    {
        private bool _disposed;
        private GeoProjection _geoProjection;
        public MapLayersHandler MapLayersHandler { get; set; }

        public FishingGroundMappingHandler(GeoProjection geoProjection)
        {
            _geoProjection = geoProjection;
        }

        //public FishingGroundMappingHandler(string fishingGround, FishingGrid.fadUTMZone utmZone)
        //{
        //    MapFishingGround(fishingGround, utmZone);
        //}

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool MapFishingGround(string fishingGround, FishingGrid.fadUTMZone utmZone)
        {
            var sf = new Shapefile();
            bool success = false;
            if (sf.CreateNew("", ShpfileType.SHP_POINT))
            {
                var shp = new Shape();
                if (shp.Create(ShpfileType.SHP_POINT))
                {
                    var result = FishingGrid.Grid25ToLatLong(fishingGround, utmZone);
                    var iShp = shp.AddPoint(result.longitude, result.latitude);
                    if (iShp >= 0 && sf.EditInsertShape(shp, 0))
                    {
                        MapLayersHandler.RemoveLayer("Fishing ground");
                        sf.GeoProjection = _geoProjection;
                        success = MapLayersHandler.AddLayer(sf, "Fishing ground", true, true) >= 0;
                    }
                }
            }
            return success;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                }
                _disposed = true;
            }
        }
    }
}
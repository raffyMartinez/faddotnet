using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapWinGIS;
using ISO_Classes;

namespace FAD3.Mapping.Classes
{
    public static class MakeGridFromPoints
    {
        public static Shapefile GridShapefile { get; }
        public static Dictionary<int, (double latitude, double longitude)> Coordinates { get; set; }
        public static Shapefile PointShapefile { get; internal set; }
        public static GeoProjection GeoProjection { get; set; }

        public static void MakeGridShapefile()
        {
        }

        public static void MakePointShapefile()
        {
            var sf = new Shapefile();
            if (sf.CreateNewWithShapeID("", ShpfileType.SHP_POINT))
            {
                var iRow = sf.EditAddField("row", FieldType.INTEGER_FIELD, 4, 1);
                sf.GeoProjection = GeoProjection;
                foreach (KeyValuePair<int, (double latitude, double longitude)> kv in Coordinates)
                {
                    var shp = new Shape();
                    if (shp.Create(ShpfileType.SHP_POINT) && shp.AddPoint(kv.Value.longitude, kv.Value.latitude) >= 0)
                    {
                        var iShp = sf.EditAddShape(shp);
                        sf.EditCellValue(iRow, iShp, kv.Key);
                    }
                }
            }

            PointShapefile = sf;
        }
    }
}
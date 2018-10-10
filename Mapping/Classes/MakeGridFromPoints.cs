using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapWinGIS;
using ISO_Classes;
using System.Windows.Forms;

namespace FAD3.Mapping.Classes
{
    public static class MakeGridFromPoints
    {
        public static Shapefile GridShapefile { get; }
        public static Dictionary<int, (double latitude, double longitude)> Coordinates { get; set; }
        public static Shapefile PointShapefile { get; internal set; }
        public static GeoProjection GeoProjection { get; set; }
        public static MapInterActionHandler MapInteractionHandler { get; set; }
        private static int iFldRow;
        private static Shape _cell;
        private static Shapefile _gridShapeFile;

        public static void MakeGridShapefile()
        {
            if (PointShapefile.NumSelected == 4)
            {
                var shpDict = new SortedDictionary<int, Shape>();
                for (int n = 0; n < MapInteractionHandler.SelectedShapeIndexes.Length; n++)
                {
                    var iShp = MapInteractionHandler.SelectedShapeIndexes[n];
                    var shp = PointShapefile.Shape[iShp];
                    shpDict.Add((int)PointShapefile.CellValue[iFldRow, iShp], shp);
                }
                _cell = new Shape();
                if (_cell.Create(ShpfileType.SHP_POLYGON))
                {
                    var shpPoint = new Shape();

                    shpPoint = shpDict[shpDict.Keys.ToList()[0]];
                    _cell.AddPoint(shpPoint.Point[0].x, shpPoint.Point[0].y);

                    shpPoint = shpDict[shpDict.Keys.ToList()[1]];
                    _cell.AddPoint(shpPoint.Point[0].x, shpPoint.Point[0].y);

                    shpPoint = shpDict[shpDict.Keys.ToList()[3]];
                    _cell.AddPoint(shpPoint.Point[0].x, shpPoint.Point[0].y);

                    shpPoint = shpDict[shpDict.Keys.ToList()[2]];
                    _cell.AddPoint(shpPoint.Point[0].x, shpPoint.Point[0].y);

                    shpPoint = shpDict[shpDict.Keys.ToList()[0]];
                    _cell.AddPoint(shpPoint.Point[0].x, shpPoint.Point[0].y);

                    var sfGridCell = new Shapefile();
                    if (sfGridCell.CreateNew("", ShpfileType.SHP_POLYGON))
                    {
                        sfGridCell.GeoProjection = GeoProjection;
                        sfGridCell.EditAddShape(_cell);
                        global.MappingForm.MapLayersHandler.AddLayer(sfGridCell, "cell");
                        sfGridCell.DefaultDrawingOptions.FillVisible = false;
                        ConstructGrid();
                    }
                }
            }
        }

        private static void ConstructGrid()
        {
            _gridShapeFile = new Shapefile();
            if (_gridShapeFile.CreateNewWithShapeID("", ShpfileType.SHP_POLYGON))
            {
                _gridShapeFile.GeoProjection = GeoProjection;

                for (int n = 0; n < PointShapefile.NumShapes; n++)
                {
                    var shp = PointShapefile.Shape[n];
                    var cPt = _cell.Centroid;
                    _cell.Move(shp.Center.x - cPt.x, shp.Center.y - cPt.y);
                    var gridCell = new Shape();
                    if (gridCell.Create(ShpfileType.SHP_POLYGON))
                    {
                        for (int c = 0; c < _cell.numPoints; c++)
                        {
                            gridCell.AddPoint(_cell.Point[c].x, _cell.Point[c].y);
                        }
                        gridCell.AddPoint(_cell.Point[0].x, _cell.Point[0].y);
                        _gridShapeFile.EditAddShape(gridCell);
                    }
                }
                global.MappingForm.MapLayersHandler.AddLayer(_gridShapeFile, "Mesh");
            }
        }

        public static void MakePointShapefile()
        {
            var sf = new Shapefile();
            if (sf.CreateNewWithShapeID("", ShpfileType.SHP_POINT))
            {
                iFldRow = sf.EditAddField("row", FieldType.INTEGER_FIELD, 4, 1);
                sf.GeoProjection = GeoProjection;
                foreach (KeyValuePair<int, (double latitude, double longitude)> kv in Coordinates)
                {
                    var shp = new Shape();
                    if (shp.Create(ShpfileType.SHP_POINT) && shp.AddPoint(kv.Value.longitude, kv.Value.latitude) >= 0)
                    {
                        var iShp = sf.EditAddShape(shp);
                        sf.EditCellValue(iFldRow, iShp, kv.Key);
                    }
                }
            }

            PointShapefile = sf;
        }

        private static void GridPoints(Shapefile pointSF)
        {
            for (int iPt = 0; iPt < pointSF.NumShapes; iPt++)
            {
                var shp = pointSF.Shape[iPt];
            }
        }

        public static void MakeUTMPointShapefile(FishingGrid.fadUTMZone zone)
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
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
        public static Shapefile GridShapefile
        {
            get { return _gridShapeFile; }
        }

        public static Dictionary<int, (double latitude, double longitude)> Coordinates { get; set; }
        public static Shapefile PointShapefile { get; internal set; }
        public static GeoProjection GeoProjection { get; set; }
        public static MapInterActionHandler MapInteractionHandler { get; set; }
        private static int iFldRow;
        private static Shape _cell;
        private static Shapefile _gridShapeFile;
        private static ShapefileCategories _categories = new ShapefileCategories();

        public static ShapefileCategories Categories
        {
            get { return _categories; }
        }

        private static bool DefineGridCell()
        {
            if (PointShapefile?.NumSelected == 4)
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
                }
                return _cell.numPoints > 0;
            }
            return false;
        }

        public static void AddCategory(double min, double max)
        {
            var cat = new ShapefileCategory();
            cat.MinValue = min;
            cat.MaxValue = max;
            cat.Name = _categories.Count.ToString();
            cat.ValueType = tkCategoryValue.cvRange;
            _categories.Add2(cat);
            switch (_categories.Count)
            {
                case 1:
                    cat.DrawingOptions.FillColor = new Utils().ColorByName(tkMapColor.LightCyan);
                    break;

                case 2:
                    cat.DrawingOptions.FillColor = new Utils().ColorByName(tkMapColor.Blue);
                    break;

                case 3:
                    cat.DrawingOptions.FillColor = new Utils().ColorByName(tkMapColor.LightGreen);
                    break;

                case 4:
                    cat.DrawingOptions.FillColor = new Utils().ColorByName(tkMapColor.ForestGreen);
                    break;

                case 5:
                    cat.DrawingOptions.FillColor = new Utils().ColorByName(tkMapColor.Orange);
                    break;
            }
            cat.DrawingOptions.LineColor = cat.DrawingOptions.FillColor;
            cat.DrawingOptions.LineWidth = 1.1F;
        }

        public static bool MakeGridShapefile()
        {
            if (DefineGridCell())
            {
                return ConstructGrid();
            }
            return false;
        }

        public static void MapColumn(List<double?> values, string name)
        {
            //_gridShapeFile.Categories = _categories;
            _gridShapeFile.EditDeleteField(2, null);
            var iFld = _gridShapeFile.EditAddField(name, FieldType.DOUBLE_FIELD, 9, 13);
            for (int n = 0; n < _gridShapeFile.NumShapes; n++)
            {
                if (_gridShapeFile.EditCellValue(iFld, n, values[n]))
                {
                    for (int c = 0; c < _categories.Count; c++)
                    {
                        double min = (double)_categories.Item[c].MinValue;
                        double max = (double)_categories.Item[c].MaxValue;
                        if (values[n] >= min && values[n] < max)
                        {
                            _gridShapeFile.ShapeCategory[n] = c;
                            break;
                        }
                    }
                }
                else
                {
                    _gridShapeFile.ShapeCategory2[n] = "nullCategory";
                }
            }
        }

        private static bool ConstructGrid()
        {
            int iShp = -1;
            _gridShapeFile = new Shapefile();
            if (_gridShapeFile.CreateNewWithShapeID("", ShpfileType.SHP_POLYGON))
            {
                _gridShapeFile.GeoProjection = GeoProjection;
                var ifldRowField = _gridShapeFile.EditAddField("row", FieldType.INTEGER_FIELD, 1, 1);

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
                        iShp = _gridShapeFile.EditAddShape(gridCell);
                        if (iShp >= 0)
                        {
                            _gridShapeFile.EditCellValue(ifldRowField, iShp, PointShapefile.CellValue[iFldRow, n]);
                        }
                    }
                }
            }
            _cell = null;
            _gridShapeFile.DefaultDrawingOptions.FillVisible = false;
            _gridShapeFile.DefaultDrawingOptions.LineColor = new Utils().ColorByName(tkMapColor.DimGray);
            _gridShapeFile.Categories = _categories;

            var nullCategory = new ShapefileCategory();
            nullCategory.Name = "nullCategory";
            nullCategory.DrawingOptions.FillVisible = false;
            nullCategory.DrawingOptions.LineVisible = false;
            _categories.Add2(nullCategory);
            return iShp >= 0;
        }

        public static bool MakePointShapefile()
        {
            int iShp = -1;
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
                        iShp = sf.EditAddShape(shp);
                        sf.EditCellValue(iFldRow, iShp, kv.Key);
                    }
                }
            }

            if (iShp >= 0)
            {
                sf.DefaultDrawingOptions.PointShape = tkPointShapeType.ptShapeCircle;
                sf.DefaultDrawingOptions.PointSize = 6;
                sf.DefaultDrawingOptions.FillColor = new Utils().ColorByName(tkMapColor.Red);
                sf.DefaultDrawingOptions.LineVisible = false;
                PointShapefile = sf;
            }
            return iShp >= 0;
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
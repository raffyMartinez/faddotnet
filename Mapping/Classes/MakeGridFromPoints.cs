using FAD3.Database.Classes;
using MapWinGIS;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace FAD3.Mapping.Classes
{
    /// <summary>
    /// helper class that handles point to polygon grid shaapefiles
    /// </summary>
    public static class MakeGridFromPoints
    {
        public static Shapefile GridShapefile
        {
            get { return _meshShapeFile; }
        }

        public static Dictionary<int, (double latitude, double longitude)> Coordinates { get; set; }
        public static Shapefile PointShapefile { get; internal set; }                                         //pointshapefile of datapoints
        public static GeoProjection GeoProjection { get; set; }
        public static MapInterActionHandler MapInteractionHandler { get; set; }
        private static int _iFldRow;
        private static Shape _cell;
        private static Shapefile _meshShapeFile;                                                              //polygonal shapefile centered on a datapoint
        private static ShapefileCategories _categories = new ShapefileCategories();
        public static ColorScheme _scheme = new ColorScheme();
        public static ColorBlend _colorBlend = new ColorBlend();
        private static int _numberOfCategories;
        private static LinkedList<(int index, MapWinGIS.Point pt, double distance)> _cell4Points = new LinkedList<(int index, MapWinGIS.Point pt, double distance)>();
        private static Dictionary<string, int> _sheetMapSummary = new Dictionary<string, int>();

        public static int NumberOfCategories
        {
            get { return _numberOfCategories; }
            set
            {
                if (_numberOfCategories != value)
                {
                    _sheetMapSummary.Clear();
                }
                _numberOfCategories = value;
            }
        }

        public static Dictionary<string, int> SheetMapSummary
        {
            get { return _sheetMapSummary; }
        }

        public static ColorBlend ColorBlend
        {
            get { return _colorBlend; }
            set
            {
                _colorBlend = value;
                _scheme = ColorSchemes.ColorBlend2ColorScheme(_colorBlend);
                _sheetMapSummary.Clear();
            }
        }

        public static Color CategoryColor(int index)
        {
            return Colors.UintToColor(_meshShapeFile.Categories.Item[index].DrawingOptions.FillColor);
        }

        public static ShapefileCategories Categories
        {
            get { return _categories; }
        }

        /// <summary>
        /// create a shape using the 4 selected points
        /// </summary>
        /// <returns></returns>
        private static bool DefineGridCell()
        {
            if (PointShapefile?.NumSelected == 4)
            {
                var shpDict = new SortedDictionary<int, Shape>();
                for (int n = 0; n < MapInteractionHandler.SelectedShapeIndexes.Length; n++)
                {
                    var iShp = MapInteractionHandler.SelectedShapeIndexes[n];
                    var shp = PointShapefile.Shape[iShp];
                    shpDict.Add((int)PointShapefile.CellValue[_iFldRow, iShp], shp);
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

        public static void AddNullCategory()
        {
            var nullCategory = new ShapefileCategory();
            nullCategory.Name = "nullCategory";
            nullCategory.DrawingOptions.FillColor = new Utils().ColorByName(tkMapColor.White);
            nullCategory.DrawingOptions.FillVisible = false;
            nullCategory.DrawingOptions.LineVisible = false;
            _categories.Add2(nullCategory);
            _sheetMapSummary.Add("Null", 0);
        }

        /// <summary>
        /// add a category where each break from the jenk's fishers is a category
        /// returns the fill color that was assigned to the category
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Color AddCategory(double min, double max)
        {
            var cat = new ShapefileCategory();
            cat.MinValue = min;
            cat.MaxValue = max;
            cat.Name = _categories.Count.ToString();
            cat.ValueType = tkCategoryValue.cvRange;
            _categories.Add2(cat);

            cat.DrawingOptions.FillColor = _scheme.get_GraduatedColor((double)(_categories.Count) / (double)_numberOfCategories);
            cat.DrawingOptions.LineColor = cat.DrawingOptions.FillColor;
            cat.DrawingOptions.LineWidth = 1.1F;
            _sheetMapSummary.Add(cat.Name, 0);

            return Colors.UintToColor(cat.DrawingOptions.FillColor);
        }

        public static bool MakeGridShapefile()
        {
            if (DefineGridCell())
            {
                return ConstructGrid();
            }
            return false;
        }

        private static void ClearSummaryValues()
        {
            foreach (var item in _sheetMapSummary.Keys.ToList<string>())
            {
                _sheetMapSummary[item] = 0;
            }
        }

        /// <summary>
        /// returns index of category that corresponds to the input parameter
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int WhatCategory(double? value)
        {
            int catIndex = 0;
            for (int c = 0; c < _categories.Count; c++)
            {
                if (value >= (double)_categories.Item[c].MinValue && value <= (double)_categories.Item[c].MaxValue)
                {
                    catIndex = c + 1;
                    break;
                }
            }
            return catIndex;
        }

        /// <summary>
        /// assign temporal values to all the shapes in the mesh shapefile and categorize the shape
        /// </summary>
        /// <param name="values"> a list that contain temporal values for mapping</param>
        /// <param name="name"></param>
        public static void MapColumn(List<double?> values, string name)
        {
            if (_meshShapeFile != null)
            {
                ClearSummaryValues();
                _meshShapeFile.EditDeleteField(2, null);
                var iFld = _meshShapeFile.EditAddField(name, FieldType.DOUBLE_FIELD, 9, 13);
                for (int n = 0; n < _meshShapeFile.NumShapes; n++)
                {
                    if (_meshShapeFile.EditCellValue(iFld, n, values[n]))
                    {
                        for (int c = 0; c < _categories.Count; c++)
                        {
                            double min = (double)_categories.Item[c].MinValue;
                            double max = (double)_categories.Item[c].MaxValue;
                            if (values[n] >= min && values[n] < max)
                            {
                                _meshShapeFile.ShapeCategory[n] = c;

                                _sheetMapSummary[_categories.Item[c].Name]++;
                                break;
                            }
                        }
                    }
                    else
                    {
                        _meshShapeFile.ShapeCategory2[n] = "nullCategory";
                        _sheetMapSummary["Null"]++;
                    }
                }
            }
        }

        /// <summary>
        /// construct a grid using all points, with each point as the center of the grid
        /// based on a shape polygon constructed from the 4 selected points
        /// </summary>
        /// <returns></returns>
        private static bool ConstructGrid()
        {
            int iShp = -1;
            _meshShapeFile = new Shapefile();
            if (_meshShapeFile.CreateNewWithShapeID("", ShpfileType.SHP_POLYGON))
            {
                _meshShapeFile.GeoProjection = GeoProjection;
                var ifldRowField = _meshShapeFile.EditAddField("row", FieldType.INTEGER_FIELD, 1, 1);

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
                        iShp = _meshShapeFile.EditAddShape(gridCell);
                        if (iShp >= 0)
                        {
                            _meshShapeFile.EditCellValue(ifldRowField, iShp, PointShapefile.CellValue[_iFldRow, n]);
                        }
                    }
                }
            }
            _cell = null;
            _meshShapeFile.DefaultDrawingOptions.FillColor = new Utils().ColorByName(tkMapColor.White);
            _meshShapeFile.DefaultDrawingOptions.FillVisible = false;
            _meshShapeFile.DefaultDrawingOptions.LineColor = new Utils().ColorByName(tkMapColor.DimGray);
            _meshShapeFile.Categories = _categories;

            return iShp >= 0;
        }

        /// <summary>
        /// make a point shapefile using the coordinates in the data
        /// </summary>
        /// <returns></returns>
        public static bool MakePointShapefile()
        {
            int iShp = -1;
            var sf = new Shapefile();
            double distance = 0;
            if (sf.CreateNewWithShapeID("", ShpfileType.SHP_POINT))
            {
                _iFldRow = sf.EditAddField("row", FieldType.INTEGER_FIELD, 4, 1);
                sf.GeoProjection = GeoProjection;
                foreach (KeyValuePair<int, (double latitude, double longitude)> kv in Coordinates)
                {
                    var shp = new Shape();
                    var pnt = new MapWinGIS.Point();
                    pnt.x = kv.Value.longitude;
                    pnt.y = kv.Value.latitude;
                    if (shp.Create(ShpfileType.SHP_POINT) && shp.AddPoint(pnt.x, pnt.y) >= 0)
                    {
                        iShp = sf.EditAddShape(shp);
                        sf.EditCellValue(_iFldRow, iShp, kv.Key);

                        if (iShp != 0)
                        {
                            distance = new Utils().GeodesicDistance(_cell4Points.First.Value.pt.y, _cell4Points.First.Value.pt.x, pnt.y, pnt.x);
                        }

                        if (_cell4Points.Count == 0)
                        {
                            _cell4Points.AddFirst((iShp, pnt, 0));
                        }
                        else if (_cell4Points.Count == 4)
                        {
                            if (distance < _cell4Points.ElementAt(1).distance)
                            {
                                _cell4Points.AddAfter(_cell4Points.First, (iShp, pnt, distance));
                                _cell4Points.RemoveLast();
                            }
                            else if (distance < _cell4Points.ElementAt(2).distance)
                            {
                                _cell4Points.AddAfter(_cell4Points.First.Next, (iShp, pnt, distance));
                                _cell4Points.RemoveLast();
                            }
                            else if (distance < _cell4Points.Last.Value.distance)
                            {
                                _cell4Points.AddBefore(_cell4Points.Last, (iShp, pnt, distance));
                                _cell4Points.RemoveLast();
                            }
                        }
                        else
                        {
                            _cell4Points.AddLast((iShp, pnt, distance));
                        }
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

        public static void MakeUTMPointShapefile(fadUTMZone zone)
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
﻿using FAD3.Database.Classes;
using MapWinGIS;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System;

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

        private static Dictionary<string, Dictionary<int, double?>> _dictTemporalValues = new Dictionary<string, Dictionary<int, double?>>();
        public static Dictionary<int, (double latitude, double longitude, bool Inland)> Coordinates { get; set; }
        public static Shapefile PointShapefile { get; internal set; }                                         //pointshapefile of datapoints
        public static GeoProjection GeoProjection { get; set; }
        public static MapInterActionHandler MapInteractionHandler { get; set; }
        private static int _iFldRow;
        private static int _ifldDateColumn;
        private static Shape _cell;
        private static Shapefile _meshShapeFile;                                                              //polygonal shapefile centered on a datapoint
        private static ShapefileCategories _categories = new ShapefileCategories();
        private static ColorScheme _scheme = new ColorScheme();
        private static ColorBlend _colorBlend = new ColorBlend();
        private static int _numberOfCategories;
        private static string _singleDimensionCSV;
        private static LinkedList<(int index, MapWinGIS.Point pt, double distance)> _cell4Points = new LinkedList<(int index, MapWinGIS.Point pt, double distance)>();
        private static Dictionary<string, int> _sheetMapSummary = new Dictionary<string, int>();
        public static int InlandPointCount { get; internal set; }
        public static fadUTMZone UTMZone { get; set; }
        public static bool CreateFileWithoutInlandPoints { get; set; }
        public static bool IgnoreInlandPoints { get; set; }

        public static int LatitudeColumn { get; set; }
        public static int LongitudeColumn { get; set; }
        public static int TemporalColumn { get; set; }
        public static int ValuesColumn { get; set; }

        static MakeGridFromPoints()
        {
            UTMZone = fadUTMZone.utmZone51N;
        }

        public static void AddCoordinate(int key, double lat, double lon, bool inland, bool initial = false)
        {
            if (initial)
            {
                Coordinates = new Dictionary<int, (double latitude, double longitude, bool Inland)>();
            }
            Coordinates.Add(key, (lat, lon, inland));
        }

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

        public static Dictionary<string, Dictionary<int, double?>> DictTemporalValues
        {
            get { return _dictTemporalValues; }
        }

        public static string SingleDimensionCSV
        {
            get { return _singleDimensionCSV; }
            set
            {
                _singleDimensionCSV = value;
                //ParseSingleDimensionCSV();
            }
        }

        public static List<string> GetFields()
        {
            List<string> csvFields = new List<string>();
            string[] fields;
            string line;
            StreamReader sr = new StreamReader(_singleDimensionCSV, true);

            line = sr.ReadLine();
            if (line != null)
            {
                TextFieldParser tfp = new TextFieldParser(new StringReader(line));
                tfp.HasFieldsEnclosedInQuotes = true;
                tfp.SetDelimiters(",");

                while (!tfp.EndOfData)
                {
                    fields = tfp.ReadFields();
                    for (int n = 0; n < fields.Length; n++)
                    {
                        csvFields.Add(fields[n]);
                    }
                }
                tfp.Close();
                tfp = null;
                sr.Close();
                sr = null;
            }
            return csvFields;
        }

        public static void ParseSingleDimensionCSV()
        {
            _dictTemporalValues.Clear();

            FishingGrid.UTMZone = UTMZone;
            Coordinates = new Dictionary<int, (double latitude, double longitude, bool Inland)>();
            StreamReader sr = new StreamReader(_singleDimensionCSV, true);
            string line;
            Dictionary<int, double?> pointValue = new Dictionary<int, double?>();
            Dictionary<int, double?> pointValueCopy = new Dictionary<int, double?>(pointValue);
            string timeEra = "";
            bool beginTimeEra = true;
            bool readCoordinates = true;
            string[] fields;
            int row = 0;
            InlandPointCount = 0;
            List<string> inlandPoints = new List<string>();
            if (!IgnoreInlandPoints)
            {
                inlandPoints = FishingGrid.InlandPoints;
            }
            while ((line = sr.ReadLine()) != null)
            {
                TextFieldParser tfp = new TextFieldParser(new StringReader(line));
                tfp.HasFieldsEnclosedInQuotes = true;
                tfp.SetDelimiters(",");

                while (!tfp.EndOfData)
                {
                    fields = tfp.ReadFields();

                    switch (fields[TemporalColumn])
                    {
                        case "time":
                        case "UTC":
                        case "time (UTC)":

                            break;

                        default:

                            if (beginTimeEra)
                            {
                                beginTimeEra = false;
                                timeEra = fields[TemporalColumn];
                            }
                            else
                            {
                                if (fields[TemporalColumn] != timeEra)
                                {
                                    pointValueCopy = new Dictionary<int, double?>(pointValue);
                                    _dictTemporalValues.Add(timeEra, pointValueCopy);
                                    pointValue.Clear();
                                    readCoordinates = false;
                                    beginTimeEra = true;
                                    row = 0;
                                }
                            }

                            //double? v = null;
                            if (double.TryParse(fields[ValuesColumn], out double vv))
                            {
                                if (double.IsNaN(vv))
                                {
                                    pointValue.Add(row, null);
                                    //v = null;
                                }
                                else if (vv != -999999)
                                {
                                    //v = vv;
                                    pointValue.Add(row, vv);
                                }
                                else
                                {
                                    pointValue.Add(row, null);
                                }
                            }
                            //pointValue.Add(row, v);

                            if (readCoordinates)
                            {
                                double lat = double.Parse(fields[LatitudeColumn]);
                                double lon = double.Parse(fields[LongitudeColumn]);
                                bool isInland = false;
                                if (!IgnoreInlandPoints)
                                {
                                    var grid25Point = FishingGrid.LongLatToGrid25(lon, lat, UTMZone);
                                    isInland = inlandPoints.Contains(grid25Point.grid25Grid);
                                    if (isInland)
                                    {
                                        InlandPointCount++;
                                    }
                                }
                                Coordinates.Add(row, (lat, lon, isInland));
                            }
                            row++;
                            break;
                    }
                }
                tfp.Close();
                tfp = null;
            }
            if (pointValue.Count > 0)
            {
                pointValueCopy = new Dictionary<int, double?>(pointValue);
                _dictTemporalValues.Add(timeEra, pointValueCopy);
                pointValue.Clear();
            }
            sr.Close();
            sr = null;
        }

        public static void Reset()
        {
            InlandPointCount = 0;
            if (Coordinates != null)
            {
                Coordinates.Clear();
            }
            if (PointShapefile != null)
            {
                PointShapefile.EditClear();
            }
            _sheetMapSummary.Clear();
            _dictTemporalValues.Clear();
            if (_meshShapeFile != null)
            {
                _meshShapeFile.EditClear();
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
                if (_ifldDateColumn > 0)
                {
                    _meshShapeFile.EditDeleteField(_ifldDateColumn, null);
                }
                //_meshShapeFile.EditDeleteField(2, null);
                _ifldDateColumn = _meshShapeFile.EditAddField(name, MapWinGIS.FieldType.DOUBLE_FIELD, 9, 13);
                for (int n = 0; n < _meshShapeFile.NumShapes; n++)
                {
                    if (_meshShapeFile.EditCellValue(_ifldDateColumn, n, values[n]))
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
                var ifldRowField = _meshShapeFile.EditAddField("row", MapWinGIS.FieldType.INTEGER_FIELD, 1, 1);
                var ifldLatField = _meshShapeFile.EditAddField("lat", MapWinGIS.FieldType.DOUBLE_FIELD, 9, 12);
                var ifldLonField = _meshShapeFile.EditAddField("lon", MapWinGIS.FieldType.DOUBLE_FIELD, 9, 13);
                var ifldInlandField = _meshShapeFile.EditAddField("inland", MapWinGIS.FieldType.STRING_FIELD, 1, 1);
                var ifldSourceInlandField = PointShapefile.FieldIndexByName["inland"];
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
                            _meshShapeFile.EditCellValue(ifldInlandField, iShp, PointShapefile.CellValue[ifldSourceInlandField, n]);
                            _meshShapeFile.EditCellValue(ifldLonField, iShp, PointShapefile.Shape[n].Center.x);
                            _meshShapeFile.EditCellValue(ifldLatField, iShp, PointShapefile.Shape[n].Center.y);
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
        public static bool MakePointShapefile(bool ShowInland = false)
        {
            int iShp = -1;
            int ifldInland = -1;
            var sf = new Shapefile();
            double distance = 0;
            if (sf.CreateNewWithShapeID("", ShpfileType.SHP_POINT))
            {
                _iFldRow = sf.EditAddField("row", MapWinGIS.FieldType.INTEGER_FIELD, 4, 1);

                if (ShowInland)
                {
                    ifldInland = sf.EditAddField("inland", MapWinGIS.FieldType.STRING_FIELD, 1, 1);
                }

                sf.GeoProjection = GeoProjection;
                foreach (KeyValuePair<int, (double latitude, double longitude, bool inland)> kv in Coordinates)
                {
                    var shp = new Shape();
                    var pnt = new MapWinGIS.Point();
                    pnt.x = kv.Value.longitude;
                    pnt.y = kv.Value.latitude;
                    if (shp.Create(ShpfileType.SHP_POINT) && shp.AddPoint(pnt.x, pnt.y) >= 0)
                    {
                        iShp = sf.EditAddShape(shp);
                        sf.EditCellValue(_iFldRow, iShp, kv.Key);

                        if (ShowInland)
                        {
                            string isInland = "F";
                            if (kv.Value.inland)
                            {
                                isInland = "T";
                            }
                            sf.EditCellValue(ifldInland, iShp, isInland);
                        }

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
                if (ShowInland)
                {
                    sf.Categories.Add("Inland");
                    sf.Categories.Item[0].Expression = $@"[inland] =  ""T""";
                    sf.Categories.Item[0].DrawingOptions.PointShape = tkPointShapeType.ptShapeCircle;
                    sf.Categories.Item[0].DrawingOptions.FillColor = new Utils().ColorByName(tkMapColor.Red);
                    sf.Categories.Item[0].DrawingOptions.PointShape = tkPointShapeType.ptShapeCircle;
                    sf.Categories.Item[0].DrawingOptions.PointSize = 5;
                    sf.Categories.Item[0].DrawingOptions.LineVisible = false;

                    sf.Categories.Add("NotInland");
                    sf.Categories.Item[1].Expression = $@"[inland] =  ""F""";
                    sf.Categories.Item[1].DrawingOptions.PointShape = tkPointShapeType.ptShapeCircle;
                    sf.Categories.Item[1].DrawingOptions.FillColor = new Utils().ColorByName(tkMapColor.Blue);
                    sf.Categories.Item[1].DrawingOptions.PointShape = tkPointShapeType.ptShapeCircle;
                    sf.Categories.Item[1].DrawingOptions.PointSize = 5;
                    sf.Categories.Item[1].DrawingOptions.LineVisible = false;

                    sf.Categories.ApplyExpressions();
                }
                else
                {
                    sf.DefaultDrawingOptions.PointShape = tkPointShapeType.ptShapeCircle;
                    sf.DefaultDrawingOptions.PointSize = 4;
                    sf.DefaultDrawingOptions.FillColor = new Utils().ColorByName(tkMapColor.Red);
                    sf.DefaultDrawingOptions.LineVisible = false;
                }
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
                var iRow = sf.EditAddField("row", MapWinGIS.FieldType.INTEGER_FIELD, 4, 1);
                sf.GeoProjection = GeoProjection;
                foreach (KeyValuePair<int, (double latitude, double longitude, bool Inland)> kv in Coordinates)
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
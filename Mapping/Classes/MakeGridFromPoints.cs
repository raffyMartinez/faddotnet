﻿using AxMapWinGIS;
using FAD3.Database.Classes;
using MapWinGIS;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Globalization;

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
        public static Dictionary<int, (double latitude, double longitude)> Coordinates { get; internal set; }
        private static Dictionary<int, bool> _rowsInland;
        public static Shapefile PointShapefile { get; internal set; }                                         //pointshapefile of datapoints
        public static GeoProjection GeoProjection { get; set; }
        public static MapInterActionHandler MapInteractionHandler { get; set; }
        private static int _iFldRow;
        private static int _ifldDateColumn;
        private static int _ifldInlandColumn;
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
        public static bool IgnoreInlandPoints { get; set; }

        public static int LatitudeColumn { get; set; }
        public static int LongitudeColumn { get; set; }
        public static int TemporalColumn { get; set; }
        public static int ParameterColumn { get; set; }
        public static string Metadata { get; internal set; }
        private static AxMap _mapControl;
        private static bool _enableMapInteraction;
        private static Extents _extents;
        public static int ExtentShapefileHandle { get; internal set; }
        public static int GridPointShapefileHandle { get; internal set; }
        public static int MeshShapefileHandle { get; internal set; }
        public static string OtherTimeUnit { get; set; }
        public static int PointCountPerTimeEra { get { return _pointCountPerTimeEra; } }

        public static EventHandler<ParseCSVEventArgs> OnCSVRead;
        public static EventHandler<ExtentDraggedBoxEventArgs> OnExtentDefined;
        public static MapLayersHandler MapLayersHandler { get; set; }
        public static bool IsNCCSVFormat { get; internal set; }
        private static int _metadataRows;
        public static string SelectedParameter { get; set; }
        private static HashSet<double> _hashSet = new HashSet<double>();
        public static double ParsingTimeSeconds { get; internal set; }
        public static int CountNonNullValues { get; internal set; }
        private static bool _newCSVFile;
        public static string CSVReadError { get; internal set; }
        private static int _pointCountPerTimeEra;

        private static List<double> _latList = new List<double>();
        private static List<double> _lonList = new List<double>();
        public static bool RegularShapeGrid { get; internal set; }
        private static List<(double lon, double lat)> _inlandCoordinates;

        public static double MaximumValue
        {
            get
            {
                return _hashSet.Max();
            }
        }

        public static double MinimumValue
        {
            get
            {
                return _hashSet.Min();
            }
        }

        public static Dictionary<string, (string dataType, string fillValue, string longName, string missingValue, string units)> ValueParametersDictionary { get { return _dictValueParameters; } }

        private static Dictionary<string, (string dataType, string fillValue, string longName, string missingValue, string units)> _dictValueParameters = new Dictionary<string, (string dataType, string fillValue, string longName, string missingValue, string units)>();

        public static HashSet<double> HashedValues { get { return _hashSet; } }

        public static AxMap MapControl
        {
            get { return _mapControl; }
            set
            {
                _mapControl = value;
                _enableMapInteraction = true;
                _mapControl.SelectBoxFinal += OnSelectBoxFinal;
            }
        }

        public static void MakeExtentShapeFile()
        {
            var sfExtent = new Shapefile();
            if (sfExtent.CreateNew("", ShpfileType.SHP_POLYGON))
            {
                var iShp = sfExtent.EditAddShape(_extents.ToShape());
                if (iShp >= 0)
                {
                    ExtentShapefileHandle = MapLayersHandler.AddLayer(sfExtent, "ExtentForDownload", true, true);
                    sfExtent.DefaultDrawingOptions.FillVisible = false;
                    sfExtent.DefaultDrawingOptions.LineWidth = 1.5f;
                    sfExtent.DefaultDrawingOptions.LineColor = new Utils().ColorByName(tkMapColor.DarkBlue);
                    MapLayersHandler.MapControl.Redraw();
                }
            }
        }

        public static void Cleanup()
        {
            _extents = null;
            Reset();
            if (_mapControl != null)
            {
                _mapControl = null;
            }

            if (ExtentShapefileHandle > 0)
            {
                MapLayersHandler.RemoveLayer(ExtentShapefileHandle);
                ExtentShapefileHandle = 0;
            }

            if (MeshShapefileHandle > 0)
            {
                MapLayersHandler.RemoveLayer(MeshShapefileHandle);
                MeshShapefileHandle = 0;
            }

            if (GridPointShapefileHandle > 0)
            {
                MapLayersHandler.RemoveLayer(GridPointShapefileHandle);
                GridPointShapefileHandle = 0;
            }

            MapLayersHandler = null;
            _newCSVFile = true;
            _singleDimensionCSV = "";
            _metadataRows = 0;
        }

        public static void UnsetMap()
        {
            _enableMapInteraction = false;
            _mapControl.SelectBoxFinal -= OnSelectBoxFinal;
            _mapControl = null;
        }

        public static void SetDataSetExtent(Extents ext)
        {
            _extents = ext;
        }

        private static void OnSelectBoxFinal(object sender, _DMapEvents_SelectBoxFinalEvent e)
        {
            if (MapControl != null)
            {
                if (_enableMapInteraction && MapControl.CursorMode == tkCursorMode.cmSelection)
                {
                    _extents = new Extents();

                    double minLat = 0;
                    double maxLat = 0;
                    double minLon = 0;
                    double maxLon = 0;
                    _mapControl.PixelToDegrees(e.left, e.bottom, ref minLon, ref minLat);
                    _mapControl.PixelToDegrees(e.right, e.top, ref maxLon, ref maxLat);
                    _extents.SetBounds(minLon, minLat, 0, maxLon, maxLat, 0);
                    OnExtentDefined?.Invoke(null, new ExtentDraggedBoxEventArgs(maxLat, minLat, minLon, maxLon, false));
                }
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        static MakeGridFromPoints()
        {
            UTMZone = fadUTMZone.utmZone51N;
        }

        public static void AddCoordinate(int key, double lat, double lon, bool initial = false)
        {
            if (initial)
            {
                Coordinates = new Dictionary<int, (double latitude, double longitude)>();
            }
            Coordinates.Add(key, (lat, lon));
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
                _newCSVFile = value != _singleDimensionCSV;
                _singleDimensionCSV = value;
            }
        }

        private static List<string> ReadNCCSVMetadata()
        {
            _dictValueParameters.Clear();
            List<string> csvFields = new List<string>();
            _metadataRows = 0;
            string[] fields;
            string line;
            string currentValue = "";
            StreamReader sr = new StreamReader(_singleDimensionCSV, true);
            TextFieldParser tfp = new TextFieldParser(new StringReader(""));

            string dataType = "";
            string fillValue = "";
            string longName = "";
            string missingValue = "";
            string units = "";
            bool addToDict = false;

            //read until end of metadata
            while ((line = sr.ReadLine()) != "*END_METADATA*")
            {
                Metadata += $"{line}\r\n";

                tfp = new TextFieldParser(new StringReader(line));
                tfp.HasFieldsEnclosedInQuotes = true;
                tfp.SetDelimiters(",");

                while (!tfp.EndOfData)
                {
                    fields = tfp.ReadFields();
                    for (int n = 0; n < fields.Length; n++)
                    {
                        switch (fields[0])
                        {
                            case "*GLOBAL*":
                            case "time":
                            case "altitude":
                            case "latitude":
                            case "longitude":
                                break;

                            default:
                                if (!_dictValueParameters.Keys.Contains(fields[0]))
                                {
                                    currentValue = fields[0];
                                    addToDict = true;
                                }
                                switch (fields[1])
                                {
                                    case "*DATA_TYPE*":
                                        dataType = fields[2];
                                        break;

                                    case "_FillValue":
                                        fillValue = fields[2];
                                        break;

                                    case "long_name":
                                        longName = fields[2];
                                        break;

                                    case "missing_value":
                                        missingValue = fields[2];
                                        break;

                                    case "units":
                                        units = fields[2];
                                        if (addToDict)
                                        {
                                            _dictValueParameters.Add(currentValue, (dataType, fillValue, longName, missingValue, units));
                                            addToDict = false;
                                        }
                                        break;
                                }
                                break;
                        }
                    }
                }
                _metadataRows++;
            }

            //read fields
            line = sr.ReadLine();
            tfp = new TextFieldParser(new StringReader(line));
            tfp.HasFieldsEnclosedInQuotes = true;
            tfp.SetDelimiters(",");
            fields = tfp.ReadFields();
            for (int n = 0; n < fields.Length; n++)
            {
                csvFields.Add(fields[n]);
            }

            //find out how many coordinate points are in a time period
            line = sr.ReadLine();
            tfp = new TextFieldParser(new StringReader(line));
            tfp.HasFieldsEnclosedInQuotes = true;
            tfp.SetDelimiters(",");
            string initTime = tfp.ReadFields()[0];
            while ((line = sr.ReadLine()) != null)
            {
                tfp = new TextFieldParser(new StringReader(line));
                tfp.HasFieldsEnclosedInQuotes = true;
                tfp.SetDelimiters(",");
                if (tfp.ReadFields()[0] == initTime)
                {
                    _pointCountPerTimeEra++;
                }
                else
                {
                    break;
                }
            }
            _pointCountPerTimeEra++;
            sr.Close();
            sr = null;
            return csvFields;
        }

        public static List<string> GetFields()
        {
            Metadata = "";
            _pointCountPerTimeEra = 0;
            int timeRow = 0;
            CSVReadError = "";
            List<string> csvFields = new List<string>();
            string[] fields;
            string line;
            string timeEra = "";
            int timeCol = 0;
            bool timeColSet = false;
            try
            {
                StreamReader sr = new StreamReader(_singleDimensionCSV, true);

                while ((line = sr.ReadLine()) != null && line.Length > 0)
                {
                    TextFieldParser tfp = new TextFieldParser(new StringReader(line));
                    tfp.HasFieldsEnclosedInQuotes = true;
                    tfp.SetDelimiters(",");

                    fields = tfp.ReadFields();

                    if (fields[0] == "*GLOBAL*" && fields[1] == "Conventions")
                    {
                        var arr = fields[2].Split(new char[] { ',', ' ' });
                        if (arr[arr.Length - 1] == "NCCSV-1.0")
                        {
                            IsNCCSVFormat = true;
                            csvFields = ReadNCCSVMetadata();
                            break;
                        }
                    }
                    else if (csvFields.Count > 0 && !timeColSet)
                    {
                        for (int n = 0; n < fields.Length; n++)
                        {
                            if (DateTime.TryParseExact(fields[n], "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt) && dt.Year > 1970)
                            {
                                timeCol = n;
                                timeColSet = true;
                                _pointCountPerTimeEra = 1;
                                break;
                            }
                            else if (DateTime.TryParse(fields[n], out dt) && dt.Year > 1970)
                            {
                                bool proceed = false;
                                if (dt.Year == DateTime.Now.Year && fields[n].Contains(dt.Year.ToString()))
                                {
                                    proceed = true;
                                }
                                else if (dt.Year < DateTime.Now.Year)
                                {
                                    proceed = true;
                                }
                                if (proceed)
                                {
                                    timeCol = n;
                                    timeColSet = true;
                                    _pointCountPerTimeEra = 1;
                                    break;
                                }
                            }
                        }
                    }
                    else if (timeColSet)
                    {
                        if (_pointCountPerTimeEra == 1)
                        {
                            timeEra = fields[timeCol];
                        }
                        if (timeEra != fields[timeCol])
                        {
                            break;
                        }
                        _pointCountPerTimeEra++;
                    }
                    else
                    {
                        if (csvFields.Count == 0)
                        {
                            for (int n = 0; n < fields.Length; n++)
                            {
                                csvFields.Add(fields[n]);
                            }
                        }
                    }

                    tfp.Close();
                    tfp = null;
                }
                sr.Close();
                sr = null;
            }
            catch (IOException ioex)
            {
                CSVReadError = ioex.Message;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "MakeGridFromPoints.cs", "GeFields");
            }
            return csvFields;
        }

        public static void RemoveMappingLayers()
        {
            if (GridPointShapefileHandle > 0)
            {
                MapLayersHandler.RemoveLayer(GridPointShapefileHandle);
                GridPointShapefileHandle = 0;
            }
            if (MeshShapefileHandle > 0)
            {
                MapLayersHandler.RemoveLayer(MeshShapefileHandle);
                MeshShapefileHandle = 0;
            }
        }

        private static void ResetMesh()
        {
            if (_meshShapeFile?.NumShapes > 0)
            {
                _meshShapeFile.Categories.Clear();
                _meshShapeFile.DefaultDrawingOptions.FillVisible = false;
                _meshShapeFile.DefaultDrawingOptions.LineColor = new Utils().ColorByName(tkMapColor.Black);
                _meshShapeFile.DefaultDrawingOptions.LineWidth = 1.1f;
                MapLayersHandler.MapControl.Redraw();
            }
        }

        /// <summary>
        /// read gridded csv data, extract point coordinates, optionally make null inland points
        /// </summary>
        /// <returns></returns>
        public static bool ParseSingleDimensionCSV()
        {
            RegularShapeGrid = false;
            CountNonNullValues = 0;
            ResetMesh();
            DateTime start = DateTime.Now;
            if (_newCSVFile)
            {
                Coordinates = new Dictionary<int, (double latitude, double longitude)>();
                _rowsInland = new Dictionary<int, bool>();
                _latList = new List<double>();
                _lonList = new List<double>();
                _inlandCoordinates = new List<(double lon, double lat)>();
            }

            List<string> inlandPoints = new List<string>();

            if (!IgnoreInlandPoints)
            {
                FishingGrid.UTMZone = UTMZone;
                inlandPoints = FishingGrid.InlandPoints;
            }

            _hashSet.Clear();
            _dictTemporalValues.Clear();

            string fv = "";
            string mv = "";
            string dataType = "double";
            if (IsNCCSVFormat)
            {
                dataType = _dictValueParameters[SelectedParameter].dataType;

                //look for  fill value and missing value in the metadata
                fv = _dictValueParameters[SelectedParameter].fillValue;
                mv = _dictValueParameters[SelectedParameter].missingValue;
                if (mv.Length == 0)
                {
                    mv = fv;
                }

                if (fv.Last() > '0')
                {
                    fv = fv.Trim(fv.Last());
                }

                if (mv.Last() > '0')
                {
                    mv = mv.Trim(mv.Last());
                }

                double fillValue = 0;
                double missingValue = 0;
                string fillValueb = "";
                string missingValueb = "";
                switch (dataType)
                {
                    case "float":
                    case "double":
                        fillValue = double.Parse(fv);
                        missingValue = double.Parse(mv);
                        break;

                    case "byte":
                        fillValueb = fv;
                        missingValueb = mv;
                        break;
                }
            }

            Dictionary<int, double?> pointValue = new Dictionary<int, double?>();
            Dictionary<int, double?> pointValueCopy = new Dictionary<int, double?>(pointValue);
            string timeEra = "";

            using (StreamReader sr = new StreamReader(_singleDimensionCSV, true))
            {
                //skip metadata lines because we already read the metadata
                if (_metadataRows > 0)
                {
                    for (int r = 0; r < _metadataRows + 1; r++)
                    {
                        sr.ReadLine();
                    }
                }

                string[] fields;
                string line = "";
                bool inData = false;
                int row = 0;
                bool proceesCoordinates = true;

                //read the rest of the file until EOF
                while ((line = sr.ReadLine()) != null)
                {
                    //each row we put in a textfieldparser
                    TextFieldParser tfp = new TextFieldParser(new StringReader(line));
                    tfp.HasFieldsEnclosedInQuotes = true;
                    tfp.SetDelimiters(",");

                    //get an array from the current row
                    fields = tfp.ReadFields();

                    //if we are in the data rows
                    if (inData)
                    {
                        if (row == _pointCountPerTimeEra)
                        {
                            //we reached the number of rows per time period

                            row = 0;

                            //we stop reading coordinates for the next set of time periods
                            proceesCoordinates = false;

                            pointValueCopy = new Dictionary<int, double?>(pointValue);

                            //add the grid variable values to a dictionary keyed by timeperiod
                            _dictTemporalValues.Add(timeEra, pointValueCopy);

                            pointValue.Clear();

                            EventHandler<ParseCSVEventArgs> readCSVEvent = OnCSVRead;
                            if (readCSVEvent != null)
                            {
                                if (proceesCoordinates)
                                {
                                    readCSVEvent(null, new ParseCSVEventArgs(Coordinates.Count, timeEra));
                                }
                                else
                                {
                                    readCSVEvent(null, new ParseCSVEventArgs(timeEra, false));
                                }
                            }

                            timeEra = fields[TemporalColumn];
                        }

                        inData = line != "*END_DATA*";
                    }

                    //we are not yet in data rows so inData=false.
                    //if time column actually contains a time value then inData=true
                    else if (line.Length > 0)
                    {
                        DateTime dt;

                        //test for conditions where time stamp is just a year, for example 2010
                        if (DateTime.TryParseExact(fields[TemporalColumn], "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt)
                            && dt.Year > 1970)
                        {
                            inData = true;
                            timeEra = fields[TemporalColumn];
                        }

                        //test for condidions where timestamp is a date
                        else if (DateTime.TryParse(fields[TemporalColumn], out dt)
                            && fields[TemporalColumn].Contains(dt.Year.ToString()))
                        {
                            inData = true;
                            timeEra = fields[TemporalColumn];
                        }
                    }

                    //we process grid variables and coordinates here
                    if (inData)
                    {
                        //we only process coordinates for the first time slice of the data
                        if (_newCSVFile && proceesCoordinates)
                        {
                            double lat = double.Parse(fields[LatitudeColumn]);
                            double lon = double.Parse(fields[LongitudeColumn]);
                            if (!_latList.Contains(lat))
                            {
                                _latList.Add(lat);
                            }
                            if (!_lonList.Contains(lon))
                            {
                                _lonList.Add(lon);
                            }

                            //process inland points here.
                            if (!IgnoreInlandPoints)
                            {
                                //we find out what grid25 cell contains a coordinate
                                var grid25Point = FishingGrid.LongLatToGrid25(lon, lat, UTMZone);

                                //if the grid25 name is in the inlandPoints list then that coordinate is inland
                                bool isInland = inlandPoints.Contains(grid25Point.grid25Grid);

                                if (isInland)
                                {
                                    _inlandCoordinates.Add((lon, lat));
                                    InlandPointCount++;
                                }
                                _rowsInland.Add(row, isInland);
                            }

                            Coordinates.Add(row, (lat, lon));
                            OnCSVRead?.Invoke(null, new ParseCSVEventArgs(true));
                        }

                        //we process the grid variables of the data
                        switch (dataType)
                        {
                            case "float":
                            case "double":
                                if (fields[ParameterColumn] == fv || fields[ParameterColumn] == mv)
                                {
                                    pointValue.Add(row, null);
                                }
                                else if (double.TryParse(fields[ParameterColumn], out double vv))
                                {
                                    if (double.IsNaN(vv))
                                    {
                                        pointValue.Add(row, null);
                                    }
                                    else
                                    {
                                        if (!IgnoreInlandPoints)
                                        {
                                            if (_rowsInland[row])
                                            {
                                                //if the datapoint is inland we change its value to null
                                                pointValue.Add(row, null);
                                            }
                                            else
                                            {
                                                pointValue.Add(row, vv);
                                                CountNonNullValues++;
                                                _hashSet.Add(vv);
                                            }
                                        }
                                        else
                                        {
                                            pointValue.Add(row, vv);
                                            CountNonNullValues++;
                                            _hashSet.Add(vv);
                                        }
                                    }
                                }
                                break;

                            case "byte":
                                if (fields[ParameterColumn] == fv || fields[ParameterColumn] == mv)
                                {
                                    pointValue.Add(row, null);
                                }
                                else
                                {
                                    if (byte.TryParse(fields[ParameterColumn], out byte bb))
                                    {
                                        if (!IgnoreInlandPoints)
                                        {
                                            if (_rowsInland[row])
                                            {
                                                //if the datapoint is inland we change its value to null
                                                pointValue.Add(row, null);
                                            }
                                            else
                                            {
                                                pointValue.Add(row, bb);
                                                CountNonNullValues++;
                                                _hashSet.Add(bb);
                                            }
                                        }
                                        else
                                        {
                                            pointValue.Add(row, bb);
                                            CountNonNullValues++;
                                            _hashSet.Add(bb);
                                        }
                                    }
                                }
                                break;
                        }
                        row++;
                    }
                }
            }

            //special case when data only contains one time period
            if (_dictTemporalValues.Count == 0)
            {
                pointValueCopy = new Dictionary<int, double?>(pointValue);
                _dictTemporalValues.Add(timeEra, pointValueCopy);
                pointValue.Clear();
            }

            //determine if data grid is a regularly shaped grid with rectangular shaped cells
            if (_newCSVFile && Coordinates.Count == (_latList.Count * _lonList.Count))
            {
                RegularShapeGrid = true;
                Coordinates.Clear();
                Coordinates = null;
            }

            ParsingTimeSeconds = (DateTime.Now - start).TotalSeconds;
            _newCSVFile = false;

            return _latList.Count > 0 && _lonList.Count > 0;
        }

        public static void Reset(bool resetCoordinates = true)
        {
            IsNCCSVFormat = false;
            _pointCountPerTimeEra = 0;
            _categories.Clear();
            _metadataRows = 0;
            InlandPointCount = 0;
            if (resetCoordinates && Coordinates != null)
            {
                Coordinates.Clear();
                _rowsInland.Clear();
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
            _singleDimensionCSV = "";
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

        /// <summary>
        /// this function will bug out
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Color CategoryColor(int index)
        {
            if (_meshShapeFile.Categories.Count > 0)
            {
                return Colors.UintToColor(_meshShapeFile.Categories.Item[index].DrawingOptions.FillColor);
            }
            else
            {
                return Color.Transparent;
            }
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

        public static Color AddUniqueItemCategory(double categoryItem)
        {
            var cat = new ShapefileCategory();
            cat.MinValue = categoryItem;
            cat.Name = categoryItem.ToString();
            cat.ValueType = tkCategoryValue.cvSingleValue;
            _categories.Add2(cat);

            cat.DrawingOptions.FillColor = _scheme.get_GraduatedColor((double)(_categories.Count) / (double)_numberOfCategories);
            cat.DrawingOptions.LineColor = cat.DrawingOptions.FillColor;
            cat.DrawingOptions.LineWidth = 1.1F;
            _sheetMapSummary.Add(cat.Name, 0);

            return Colors.UintToColor(cat.DrawingOptions.FillColor);
        }

        public static void SetMeshCategories()
        {
            _meshShapeFile.Categories = _categories;
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
        public static int WhatCategory(double? value, string classificationUsed)
        {
            int catIndex = 0;
            switch (classificationUsed)
            {
                case "Jenk's-Fisher's":
                case "Equal interval":
                    for (int c = 0; c < _categories.Count; c++)
                    {
                        if (_categories.Item[c].MinValue != null
                            && _categories.Item[c].MaxValue != null
                            && value >= (double)_categories.Item[c].MinValue
                            && value <= (double)_categories.Item[c].MaxValue)
                        {
                            catIndex = c + 1;
                            break;
                        }
                    }
                    break;

                case "Unique values":
                    for (int c = 0; c < _categories.Count; c++)
                    {
                        if (value == (double)_categories.Item[c].MinValue)
                        {
                            catIndex = c + 1;
                            break;
                        }
                    }
                    break;
            }

            return catIndex;
        }

        /// <summary>
        /// assign grid values to all the shapes in the mesh shapefile and categorize the shape
        /// </summary>
        /// <param name="values"> a list that contain temporal values for mapping</param>
        /// <param name="name"></param>
        public static bool MapColumn(List<double?> values, string name, string categorizationOption = "Jenk's-Fisher's")

        {
            bool success = false;
            if (_meshShapeFile != null && _categories.Count > 0)
            {
                if (_meshShapeFile.NumShapes == values.Count)
                {
                    _ifldInlandColumn = _meshShapeFile.FieldIndexByName["inland"];
                    ClearSummaryValues();

                    if (_ifldDateColumn > 0)
                    {
                        _meshShapeFile.EditDeleteField(_ifldDateColumn, null);
                    }

                    _ifldDateColumn = _meshShapeFile.EditAddField(name, MapWinGIS.FieldType.DOUBLE_FIELD, 9, 13);
                    if (_ifldDateColumn > 0)
                    {
                        for (int n = 0; n < _meshShapeFile.NumShapes; n++)
                        {
                            //if cell is inland (CellValue[_ifldInlandColumn, n].ToString() == "T")
                            //then make the parameter column value  null
                            if (_ifldInlandColumn >= 0
                                && _meshShapeFile.CellValue[_ifldInlandColumn, n].ToString() == "T")
                            {
                                _meshShapeFile.EditCellValue(_ifldDateColumn, n, null);
                                _meshShapeFile.ShapeCategory2[n] = "nullCategory";
                                _sheetMapSummary["Null"]++;
                            }
                            //make the parameter column value of the nth shape the corresponding row in values[n]
                            // sometimes an error happens here when _meshfile.NumShapes > 1 over values[]
                            else if (_meshShapeFile.EditCellValue(_ifldDateColumn, n, values[n]))
                            {
                                bool isFound = false;
                                for (int c = 0; c < _categories.Count; c++)
                                {
                                    if (_categories.Item[c].MinValue != null)
                                    {
                                        double min = (double)_categories.Item[c].MinValue;

                                        switch (categorizationOption)
                                        {
                                            case "Unique values":
                                                if (values[n] == min)
                                                {
                                                    _meshShapeFile.ShapeCategory[n] = c;

                                                    _sheetMapSummary[_categories.Item[c].Name]++;
                                                    isFound = true;
                                                    break;
                                                }
                                                break;

                                            case "Jenk's-Fisher's":
                                            case "Equal interval":
                                                double max = (double)_categories.Item[c].MaxValue;
                                                if (values[n] >= min && values[n] < max)
                                                {
                                                    _meshShapeFile.ShapeCategory[n] = c;

                                                    _sheetMapSummary[_categories.Item[c].Name]++;
                                                    isFound = true;
                                                    break;
                                                }
                                                break;
                                        }
                                        if (isFound)
                                        {
                                            break;
                                        }
                                    }
                                }
                                _meshShapeFile.Categories.ClassificationField = _ifldDateColumn;
                            }
                            else
                            {
                                _meshShapeFile.ShapeCategory2[n] = "nullCategory";
                                _sheetMapSummary["Null"]++;
                            }
                        }
                        //redraw the map to reflect the updated values and their categories
                        MapLayersHandler.MapControl.Redraw();

                        success = true;
                    }
                }
                else
                {
                    Logger.Log("Mesh shapes count does not match values list count", "MakeGridFromPoints.cs", "MapColumn");
                }
            }
            return success;
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
            MeshShapefileHandle = MapLayersHandler.AddLayer(_meshShapeFile, "Mesh", true, true);

            return MeshShapefileHandle >= 0;
        }

        /// <summary>
        /// makes a mesh from a grid of points
        /// </summary>
        /// <param name="showInland"></param>
        /// <returns></returns>
        public static bool MakeMeshShapefile(bool showInland = false)
        {
            List<string> inlandPoints = new List<string>();
            int latCount = 0;
            int lonCount = 0;
            var ifldRowField = 0; ;
            var ifldLatField = 0;
            var ifldLonField = 0;
            var ifldInlandField = 0;
            double distanceLat = 0;
            double distanceLon = 0;
            bool isInland = false;

            _meshShapeFile = new Shapefile();
            if (_meshShapeFile.CreateNewWithShapeID("", ShpfileType.SHP_POLYGON))
            {
                _meshShapeFile.GeoProjection = GeoProjection;
                ifldRowField = _meshShapeFile.EditAddField("row", MapWinGIS.FieldType.INTEGER_FIELD, 1, 1);
                ifldLatField = _meshShapeFile.EditAddField("lat", MapWinGIS.FieldType.DOUBLE_FIELD, 9, 12);
                ifldLonField = _meshShapeFile.EditAddField("lon", MapWinGIS.FieldType.DOUBLE_FIELD, 9, 13);
                if (showInland)
                {
                    ifldInlandField = _meshShapeFile.EditAddField("inland", MapWinGIS.FieldType.STRING_FIELD, 1, 1);
                }

                int iMeshShp = -1;
                foreach (double lat in _latList)
                {
                    foreach (double lon in _lonList)
                    {
                        var shp = new Shape();
                        var pnt = new MapWinGIS.Point();
                        pnt.x = lon;
                        pnt.y = lat;

                        if (showInland)
                        {
                            isInland = _inlandCoordinates.Contains((lon, lat));
                        }

                        var gridCell = new Shape();
                        if (gridCell.Create(ShpfileType.SHP_POLYGON))
                        {
                            if (latCount < _latList.Count - 1)
                            {
                                distanceLat = Math.Abs(_latList[latCount + 1] - lat);
                            }
                            if (lonCount < _lonList.Count - 1)
                            {
                                distanceLon = Math.Abs(_lonList[lonCount + 1] - lon);
                            }
                            var gridExtent = new Extents();
                            gridExtent.SetBounds(lon - distanceLon / 2, lat - distanceLat / 2, 0, lon + distanceLon / 2, lat + distanceLat / 2, 0);
                            gridCell = gridExtent.ToShape();
                            iMeshShp = _meshShapeFile.EditAddShape(gridCell);
                            if (iMeshShp >= 0)
                            {
                                _meshShapeFile.EditCellValue(ifldRowField, iMeshShp, iMeshShp + 1);
                                _meshShapeFile.EditCellValue(ifldLonField, iMeshShp, lon);
                                _meshShapeFile.EditCellValue(ifldLatField, iMeshShp, lat);
                                if (showInland)
                                {
                                    _meshShapeFile.EditCellValue(ifldInlandField, iMeshShp, isInland ? "T" : "F");
                                }
                            }
                        }

                        lonCount++;
                    }
                    latCount++;
                }
            }

            _meshShapeFile.DefaultDrawingOptions.FillColor = new Utils().ColorByName(tkMapColor.White);
            _meshShapeFile.DefaultDrawingOptions.FillVisible = false;
            _meshShapeFile.DefaultDrawingOptions.LineColor = new Utils().ColorByName(tkMapColor.DimGray);
            MeshShapefileHandle = MapLayersHandler.AddLayer(_meshShapeFile, "Mesh", true, true);
            return MeshShapefileHandle >= 0;
        }

        /// <summary>
        /// make a point shapefile using the coordinates in the data and setup its appearance
        /// </summary>
        /// <returns></returns>
        public static bool MakePointShapefile(bool ShowInland = false)
        {
            List<string> inlandPoints = new List<string>();
            int iShp = -1;
            int ifldInland = -1;
            var sf = new Shapefile();
            if (sf.CreateNewWithShapeID("", ShpfileType.SHP_POINT))
            {
                _iFldRow = sf.EditAddField("row", MapWinGIS.FieldType.INTEGER_FIELD, 4, 1);

                if (ShowInland)
                {
                    ifldInland = sf.EditAddField("inland", MapWinGIS.FieldType.STRING_FIELD, 1, 1);
                }

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

                        if (ShowInland)
                        {
                            bool isInland = _inlandCoordinates.Contains((pnt.x, pnt.y));
                            if (isInland)
                            {
                                InlandPointCount++;
                            }
                            sf.EditCellValue(ifldInland, iShp, isInland ? "T" : "F");
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
                    sf.Categories.Item[0].DrawingOptions.PointSize = 4;
                    sf.Categories.Item[0].DrawingOptions.LineVisible = false;

                    sf.Categories.Add("NotInland");
                    sf.Categories.Item[1].Expression = $@"[inland] =  ""F""";
                    sf.Categories.Item[1].DrawingOptions.PointShape = tkPointShapeType.ptShapeCircle;
                    sf.Categories.Item[1].DrawingOptions.FillColor = new Utils().ColorByName(tkMapColor.Blue);
                    sf.Categories.Item[1].DrawingOptions.PointShape = tkPointShapeType.ptShapeCircle;
                    sf.Categories.Item[1].DrawingOptions.PointSize = 4;
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
                GridPointShapefileHandle = MapLayersHandler.AddLayer(PointShapefile, "Grid points", true, true);
            }
            return GridPointShapefileHandle >= 0;
        }

        /// <summary>
        /// make a point shapefile using the coordinates in the data and setup its appearance
        /// </summary>
        /// <returns></returns>

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
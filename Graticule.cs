using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapWinGIS;
using AxMapWinGIS;

namespace FAD3
{
    public class Graticule
    {
        private MapLayersHandler _mapLayersHandler;
        private AxMap _axMap;
        private Shapefile _sfGraticule;
        private int _ifldPart;
        private int _ifldLabel;
        private int _ifldSide;
        private GeoProjection _geoProjection;
        private Extents _graticuleExtents;
        private Extents _boundaryExtents;
        private Extents _mapExtents;
        private double _xOrigin;
        private double _yOrigin;
        private Double _gridHeight;
        private double _gridWidth;
        private double _intervalSize;

        public bool BottomHasLabel { get; set; }
        public bool TopHasLabel { get; set; }
        public bool RightHasLabel { get; set; }
        public bool LeftHasLabel { get; set; }
        public string Name { get; set; }
        public int LabelFontSize { get; set; }
        public int NumberOfGridlines { get; set; }
        public int BorderWidth { get; set; }
        public int GridlinesWidth { get; set; }
        public bool BoldLabels { get; set; }
        public bool GridVisible { get; set; }

        public string CoordFormat { get; set; }

        public Shapefile GraticuleShapeFile
        {
            get { return _sfGraticule; }
        }

        public void Configure(string name, int sizeLabelFont, int numberGridlines, int widthBorder, int widthGridlines,
                              bool gridVisible, bool boldLabels, bool leftHasLabel, bool rightHasLabel,
                              bool topHasLabel, bool bottomHasLabel)
        {
            Name = name;
            LabelFontSize = sizeLabelFont;
            NumberOfGridlines = numberGridlines;
            BorderWidth = widthBorder;
            GridlinesWidth = widthGridlines;
            BoldLabels = boldLabels;
            GridVisible = gridVisible;
            BottomHasLabel = bottomHasLabel;
            TopHasLabel = topHasLabel;
            LeftHasLabel = leftHasLabel;
            RightHasLabel = rightHasLabel;
        }

        private void SetupShapefile()
        {
            _sfGraticule = new Shapefile();
            if (_sfGraticule.CreateNew("", ShpfileType.SHP_POLYLINE))
            {
                _ifldPart = _sfGraticule.EditAddField("Part", FieldType.STRING_FIELD, 1, 25);
                _ifldLabel = _sfGraticule.EditAddField("Label", FieldType.STRING_FIELD, 1, 25);
                _ifldSide = _sfGraticule.EditAddField("Side", FieldType.STRING_FIELD, 1, 1);
                _sfGraticule.GeoProjection = _geoProjection;
            }
        }

        public Shapefile Mask()
        {
            var sf = new Shapefile();
            if (sf.CreateNew("", ShpfileType.SHP_POLYGON))
            {
                var shp = new Shape();
                if (shp.Create(ShpfileType.SHP_POLYGON))
                {
                    shp = _mapExtents.ToShape().Clip(_boundaryExtents.ToShape(), tkClipOperation.clDifference);
                    var iShp = sf.EditAddShape(shp);
                    sf.DefaultDrawingOptions.LineVisible = false;
                    sf.DefaultDrawingOptions.FillColor = new Utils().ColorByName(tkMapColor.White);
                    return sf;
                }
                return null;
            }
            return null;
        }

        public Graticule(AxMap mapControl, MapLayersHandler layersHandler)
        {
            Name = "Graticule";
            LabelFontSize = 8;
            NumberOfGridlines = 5;
            _axMap = mapControl;
            _axMap.ExtentsChanged += OnMapExtentsChanged;
            _geoProjection = _axMap.GeoProjection;
            _mapLayersHandler = layersHandler;

            switch (global.CoordinateDisplay)
            {
                case global.CoordinateDisplayFormat.DegreeDecimal:
                    CoordFormat = "D";
                    break;

                case global.CoordinateDisplayFormat.DegreeMinute:
                    CoordFormat = "DM";
                    break;

                case global.CoordinateDisplayFormat.DegreeMinuteSecond:
                    CoordFormat = "DMS";
                    break;

                default:
                    throw new Exception("Graticule: Invalid coordinate format");
            }
        }

        private void OnMapExtentsChanged(object sender, EventArgs e)
        {
            _sfGraticule.EditClear();
            _sfGraticule.Labels.Clear();
            ShowGraticule();
        }

        public void ShowGraticule()
        {
            _mapLayersHandler.RemoveLayer(Name);
            _mapLayersHandler.RemoveLayer("Mask");
            SetupShapefile();
            ComputeGraticule(_axMap.Extents.xMax, _axMap.Extents.yMax, _axMap.Extents.xMin, _axMap.Extents.yMin);
            _sfGraticule.Labels.VerticalPosition = tkVerticalPosition.vpAboveAllLayers;
            var maskHandle = _mapLayersHandler.AddLayer(Mask(), "Mask", true, false);
            _mapLayersHandler.get_MapLayer(maskHandle).IsMaskLayer = true;
            _mapLayersHandler.AddLayer(_sfGraticule, Name, true, true);
        }

        private void ComputeGraticule(double xMax, double yMax, double xMin, double yMin)
        {
            var coordinateString = "";
            var coord = new ISO_Classes.Coordinate();
            _mapExtents = new Extents();
            _mapExtents.SetBounds(xMin, yMin, 0, xMax, yMax, 0);

            _graticuleExtents = new Extents();

            var tempW = (xMax - xMin) * 0.93;
            var tempH = (yMax - yMin) * 0.93;

            _xOrigin = (xMin + ((xMax - xMin) / 2)) - (tempW / 2);
            _yOrigin = (yMin + ((yMax - yMin) / 2)) - (tempH / 2);

            var ticLength = Math.Abs(xMin - _xOrigin) / 4;

            _graticuleExtents.SetBounds(_xOrigin, _yOrigin, 0, _xOrigin + tempW, _yOrigin + tempH, 0);
            _boundaryExtents = new Extents();
            _boundaryExtents.SetBounds(_xOrigin, _yOrigin, 0, _xOrigin + tempW, _yOrigin + tempH, 0);

            var graticuleShape = _graticuleExtents.ToShape();

            Point pt1, pt2;
            Shape shp;
            for (int n = 0; n < 4; n++)
            {
                pt1 = new Point();
                pt2 = new Point();
                pt1.x = graticuleShape.Point[n].x;
                pt1.y = graticuleShape.Point[n].y;
                pt2.x = graticuleShape.Point[n + 1].x;
                pt2.y = graticuleShape.Point[n + 1].y;
                shp = new Shape();
                if (shp.Create(ShpfileType.SHP_POLYLINE))
                {
                    shp.AddPoint(pt1.x, pt1.y);
                    shp.AddPoint(pt2.x, pt2.y);
                    var iShp = _sfGraticule.EditAddShape(shp);
                    _sfGraticule.EditCellValue(_ifldPart, iShp, "Border");
                }
            }

            xMax = graticuleShape.Extents.xMax;
            yMax = graticuleShape.Extents.yMax;
            xMin = graticuleShape.Extents.xMin;
            yMin = graticuleShape.Extents.yMin;
            _gridHeight = yMax - yMin;
            _gridWidth = xMax - xMin;

            var sideLength = _gridHeight;
            if (_gridWidth > _gridHeight) sideLength = _gridWidth;

            var factor = 60;
            _intervalSize = (sideLength / (NumberOfGridlines - 1)) * factor;
            var roundTo = (int)(_intervalSize / 5) * 5;

            if (roundTo == 0)
            {
                factor = 60 * 60;
                _intervalSize = (sideLength / (NumberOfGridlines - 1)) * factor;
                roundTo = (int)(_intervalSize / 5) * 5;
            }

            roundTo = SetInterval((int)roundTo);

            var cat_T = _sfGraticule.Labels.AddCategory("Top");
            var cat_B = _sfGraticule.Labels.AddCategory("Bottom");
            var cat_R = _sfGraticule.Labels.AddCategory("Right");
            var cat_L = _sfGraticule.Labels.AddCategory("Left");

            for (int n = 0; n < 4; n++)
            {
                _sfGraticule.Labels.Category[n].FontOutlineVisible = false;
                _sfGraticule.Labels.Category[n].FontBold = BoldLabels;
                _sfGraticule.Labels.Category[n].FontSize = LabelFontSize;
                _sfGraticule.Labels.Category[n].FrameVisible = false;
                _sfGraticule.Labels.Category[n].LineOrientation = tkLineLabelOrientation.lorPerpindicular;
                _sfGraticule.Labels.AvoidCollisions = false;
                switch (_sfGraticule.Labels.Category[n].Name)
                {
                    case "Top":
                        _sfGraticule.Labels.Category[n].Alignment = tkLabelAlignment.laTopCenter;
                        break;

                    case "Bottom":
                        _sfGraticule.Labels.Category[n].Alignment = tkLabelAlignment.laBottomCenter;
                        break;

                    case "Left":
                    case "Right":
                        _sfGraticule.Labels.Category[n].Alignment = tkLabelAlignment.laCenter;
                        break;
                }
            }

            _intervalSize = (int)(_intervalSize / roundTo) * roundTo;

            if (_intervalSize >= 1)
            {
                _intervalSize /= factor;
                double baseX = ((int)(_xOrigin - (int)_xOrigin) * factor);
                if (baseX / roundTo != (baseX / 10))
                {
                    baseX = ((baseX / roundTo) + 1) * roundTo;
                }
                baseX = (int)_xOrigin + (baseX / factor);

                _sfGraticule.Labels.AddCategory("Top");
                while (baseX < xMax)
                {
                    if (baseX > xMin)
                    {
                        shp = new Shape();
                        if (shp.Create(ShpfileType.SHP_POLYLINE))
                        {
                            shp.AddPoint(baseX, yMin);
                            shp.AddPoint(baseX, yMax);
                            var iShp = _sfGraticule.EditAddShape(shp);
                            _sfGraticule.EditCellValue(_ifldPart, iShp, "Gridline");

                            //set tics on the bottom side
                            if (BottomHasLabel)
                            {
                                shp = new Shape();
                                if (shp.Create(ShpfileType.SHP_POLYLINE))
                                {
                                    shp.AddPoint(baseX, yMin);
                                    shp.AddPoint(baseX, yMin - ticLength);
                                    iShp = _sfGraticule.EditAddShape(shp);
                                    _sfGraticule.EditCellValue(_ifldPart, iShp, "tic");
                                    coord.SetD((float)shp.Center.y, (float)shp.Center.x);
                                    coordinateString = coord.ToString(false, CoordFormat);
                                    _sfGraticule.EditCellValue(_ifldLabel, iShp, coordinateString);
                                    _sfGraticule.EditCellValue(_ifldSide, iShp, "B");
                                    _sfGraticule.Labels.AddLabel(coordinateString, baseX, yMin - ticLength, 0, 1);
                                }
                            }

                            //set tics on the top side
                            if (TopHasLabel)
                            {
                                shp = new Shape();
                                if (shp.Create(ShpfileType.SHP_POLYLINE))
                                {
                                    shp.AddPoint(baseX, yMax);
                                    shp.AddPoint(baseX, yMax + ticLength);
                                    iShp = _sfGraticule.EditAddShape(shp);
                                    _sfGraticule.EditCellValue(_ifldPart, iShp, "tic");
                                    coord.SetD((float)shp.Center.y, (float)shp.Center.x);
                                    coordinateString = coord.ToString(false, CoordFormat);
                                    _sfGraticule.EditCellValue(_ifldLabel, iShp, coordinateString);
                                    _sfGraticule.EditCellValue(_ifldSide, iShp, "T");
                                    _sfGraticule.Labels.AddLabel(coordinateString, baseX, yMax + ticLength, 0, 0);
                                }
                            }
                        }
                    }
                    baseX += _intervalSize;
                }

                double baseY = (int)((_yOrigin - (int)_yOrigin) * factor);
                if (baseY / roundTo != (int)(baseY / roundTo))
                {
                    baseY = ((int)(baseY / roundTo) + 1) * roundTo;
                }
                baseY = (int)_yOrigin + (baseY / factor);

                while (baseY < yMax)
                {
                    if (baseY > yMin)
                    {
                        shp = new Shape();
                        if (shp.Create(ShpfileType.SHP_POLYLINE))
                        {
                            shp.AddPoint(xMin, baseY);
                            shp.AddPoint(xMax, baseY);
                            var iShp = _sfGraticule.EditAddShape(shp);
                            _sfGraticule.EditCellValue(_ifldPart, iShp, "GridLine");
                        }

                        if (LeftHasLabel)
                        {
                            shp = new Shape();
                            if (shp.Create(ShpfileType.SHP_POLYLINE))
                            {
                                shp.AddPoint(xMin, baseY);
                                shp.AddPoint(xMin - ticLength, baseY);
                                var iShp = _sfGraticule.EditAddShape(shp);
                                _sfGraticule.EditCellValue(_ifldPart, iShp, "tic");
                                coord.SetD((float)shp.Center.y, (float)shp.Center.x);
                                coordinateString = coord.ToString(isYcoord: true, format: CoordFormat);
                                _sfGraticule.EditCellValue(_ifldLabel, iShp, coordinateString);
                                _sfGraticule.EditCellValue(_ifldSide, iShp, "L");
                                _sfGraticule.Labels.AddLabel(coordinateString, xMin - (ticLength * 2), baseY, 270, 3);
                            }
                        }

                        if (RightHasLabel)
                        {
                            shp = new Shape();
                            if (shp.Create(ShpfileType.SHP_POLYLINE))
                            {
                                shp.AddPoint(xMax, baseY);
                                shp.AddPoint(xMax + ticLength, baseY);
                                var iShp = _sfGraticule.EditAddShape(shp);
                                _sfGraticule.EditCellValue(_ifldPart, iShp, "tic");
                                coord.SetD((float)shp.Center.y, (float)shp.Center.x);
                                coordinateString = coord.ToString(isYcoord: true, format: CoordFormat);
                                _sfGraticule.EditCellValue(_ifldLabel, iShp, coordinateString);
                                _sfGraticule.EditCellValue(_ifldSide, iShp, "R");
                                _sfGraticule.Labels.AddLabel(coordinateString, xMax + (ticLength * 2), baseY, 90, 2);
                            }
                        }
                    }

                    baseY += _intervalSize;
                }

                if (_sfGraticule.Categories.Generate(_ifldPart, tkClassificationType.ctUniqueValues, 1))
                {
                    for (int n = 0; n < _sfGraticule.Categories.Count; n++)
                    {
                        var category = _sfGraticule.Categories.Item[n];
                        switch (category.Expression)
                        {
                            case @"[Part] = ""Border""":
                            case @"[Part] = ""tic""":
                                category.DrawingOptions.LineWidth = BorderWidth;
                                category.DrawingOptions.VerticesColor = new Utils().ColorByName(tkMapColor.Black);
                                break;

                            default:
                                category.DrawingOptions.VerticesColor = new Utils().ColorByName(tkMapColor.Gray);
                                category.DrawingOptions.LineWidth = GridlinesWidth;
                                category.DrawingOptions.LineVisible = GridVisible;
                                break;
                        }
                    }

                    _sfGraticule.Categories.ApplyExpressions();
                }
            }
            else
            {
                throw new Exception("Graticule: Interval size is invalid");
            }
        }

        private int SetInterval(int inValue)
        {
            if (inValue < 2)
            {
                return 1;
            }
            else if (inValue < 5)
            {
                return 2;
            }
            else if (inValue < 10)
            {
                return 5;
            }
            else if (inValue < 15)
            {
                return 10;
            }
            else if (inValue < 20)
            {
                return 15;
            }
            else if (inValue < 30)
            {
                return 20;
            }
            else if (inValue < 60)
            {
                return 30;
            }
            else if (inValue < 90)
            {
                return 60;
            }
            else if (inValue < 120)
            {
                return 90;
            }
            else if (inValue < 150)
            {
                return 120;
            }
            else if (inValue < 180)
            {
                return 150;
            }
            else if (inValue < 240)
            {
                return 180;
            }
            else if (inValue >= 240)
            {
                return 240;
            }

            return 0;
        }
    }
}
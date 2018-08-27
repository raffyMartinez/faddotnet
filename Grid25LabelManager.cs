using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapWinGIS;

namespace FAD3
{
    public class Grid25LabelManager
    {
        private Shapefile _shapeFileGrid25Labels;
        private Dictionary<string, uint> _labelPropertiesDictionary;

        public Shapefile Grid25Labels
        {
            get { return _shapeFileGrid25Labels; }
        }

        public void ClearLabels()
        {
            _shapeFileGrid25Labels.Labels.Clear();
            _shapeFileGrid25Labels.EditClear();
        }

        public Grid25LabelManager(Extents extentToLabel, List<string> SidesToLabel,
                                    GeoProjection geoProjection, Dictionary<string, uint> labelProperties)
        {
            _labelPropertiesDictionary = labelProperties;
            var labelDistance = (int)_labelPropertiesDictionary["minorGridLabelDistance"];
            _shapeFileGrid25Labels = new Shapefile();

            if (_shapeFileGrid25Labels.CreateNewWithShapeID("", ShpfileType.SHP_POINT))
            {
                _shapeFileGrid25Labels.EditAddField("Location", FieldType.STRING_FIELD, 1, 2);
                _shapeFileGrid25Labels.EditAddField("Label", FieldType.STRING_FIELD, 1, 4);
                _shapeFileGrid25Labels.GeoProjection = geoProjection;
            }

            var originX = (int)extentToLabel.xMin;
            var originY = (int)extentToLabel.yMin;
            var width = (int)Math.Abs((extentToLabel.xMax - extentToLabel.xMin));
            var height = (int)Math.Abs((extentToLabel.yMax - extentToLabel.yMin));
            var cols = width / FishingGrid.MinorGridSizeMeters;
            var rows = height / FishingGrid.MinorGridSizeMeters;

            int categoryIndex = 0;
            foreach (var position in SidesToLabel)
            {
                switch (position)
                {
                    case "top":
                        categoryIndex = 0;
                        SetupLabels(position, originX, originY, cols, rows, labelDistance, height, 0, categoryIndex);
                        break;

                    case "right":
                        categoryIndex = 1;
                        SetupLabels(position, originX, originY, cols, rows, labelDistance, 0, width, categoryIndex);
                        break;

                    case "bottom":
                        categoryIndex = 2;
                        SetupLabels(position, originX, originY, cols, rows, labelDistance, 0, 0, categoryIndex);
                        break;

                    case "left":
                        categoryIndex = 3;
                        SetupLabels(position, originX, originY, cols, rows, labelDistance, 0, 0, categoryIndex);
                        break;
                }
            }
        }

        private void SetupLabels(string position, int originX, int originY, int columns, int rows, int distance = 1000, int height = 0, int width = 0, int CategoryIndex = 0)
        {
            var ifldLocation = _shapeFileGrid25Labels.FieldIndexByName["Location"];
            var ifldLabel = _shapeFileGrid25Labels.FieldIndexByName["Label"];
            var labelDistance = distance;

            SetupLabelProperties();

            switch (position)
            {
                case "left":
                case "right":
                    var baseY = originY / FishingGrid.MajorGridSizeMeters;
                    baseY *= FishingGrid.MajorGridSizeMeters;

                    if (baseY == originY)
                    {
                        baseY = 25;
                    }
                    else
                    {
                        baseY = 25 - (originY - baseY) / FishingGrid.MinorGridSizeMeters;
                    }

                    for (int n = 0; n < rows; n++)
                    {
                        labelDistance = Math.Abs(labelDistance);
                        Point pt = new Point();
                        if (position == "left")
                        {
                            labelDistance *= -1;
                            pt.x = originX + labelDistance;
                        }
                        else if (position == "right")
                        {
                            pt.x = originX + labelDistance + width;
                        }

                        pt.y = originY + (n * FishingGrid.MinorGridSizeMeters) + (FishingGrid.MinorGridSizeMeters / 2);

                        var shp = new Shape();
                        if (shp.Create(ShpfileType.SHP_POINT))
                        {
                            if (shp.InsertPoint(pt, 0))
                            {
                                var iShp = _shapeFileGrid25Labels.EditAddShape(shp);
                                if (iShp >= 0)
                                {
                                    _shapeFileGrid25Labels.EditCellValue(ifldLabel, iShp, baseY);
                                    _shapeFileGrid25Labels.EditCellValue(ifldLocation, iShp, position.Substring(0, 1).ToUpper());
                                    _shapeFileGrid25Labels.Labels.AddLabel(baseY.ToString(), pt.x, pt.y, 0, CategoryIndex);
                                }
                            }
                        }
                        baseY--;
                        if (baseY == 0) baseY = 25;
                    }

                    break;

                case "top":
                case "bottom":
                    var baseX = originX / FishingGrid.MajorGridSizeMeters;
                    baseX *= FishingGrid.MajorGridSizeMeters;

                    if (baseX == originX)
                    {
                        baseX = 0;
                    }
                    else
                    {
                        baseX = 25 - (25 - (originX - baseX) / FishingGrid.MinorGridSizeMeters);
                    }

                    for (int n = 0; n < columns; n++)
                    {
                        labelDistance = Math.Abs(labelDistance);
                        Point pt = new Point();
                        if (position == "top")
                        {
                            pt.y = originY + labelDistance + height;
                        }
                        else if (position == "bottom")
                        {
                            labelDistance *= -1;
                            pt.y = originY + labelDistance + height;
                        }

                        pt.x = originX + (n * FishingGrid.MinorGridSizeMeters) + (FishingGrid.MinorGridSizeMeters / 2);

                        var shp = new Shape();
                        if (shp.Create(ShpfileType.SHP_POINT))
                        {
                            if (shp.InsertPoint(pt, 0))
                            {
                                var iShp = _shapeFileGrid25Labels.EditAddShape(shp);
                                if (iShp >= 0)
                                {
                                    _shapeFileGrid25Labels.EditCellValue(ifldLabel, iShp, (char)('A' + baseX));
                                    _shapeFileGrid25Labels.EditCellValue(ifldLocation, iShp, position.Substring(0, 1).ToUpper());
                                    _shapeFileGrid25Labels.Labels.AddLabel(((char)('A' + baseX)).ToString(), pt.x, pt.y, 0, CategoryIndex);
                                }
                            }
                        }
                        baseX++;
                        if (baseX == 25) baseX = 0;
                    }

                    break;
            }
        }

        public void SetupLabelProperties()
        {
            _shapeFileGrid25Labels.DefaultDrawingOptions.FillVisible = false;
            _shapeFileGrid25Labels.DefaultDrawingOptions.LineColor = new Utils().ColorByName(tkMapColor.White);
            _shapeFileGrid25Labels.DefaultDrawingOptions.PointSize = 0;

            _shapeFileGrid25Labels.Labels.Alignment = tkLabelAlignment.laCenter;
            _shapeFileGrid25Labels.Labels.FrameVisible = false;
            _shapeFileGrid25Labels.Labels.AutoOffset = false;
            _shapeFileGrid25Labels.Labels.AvoidCollisions = false;
            _shapeFileGrid25Labels.Labels.Visible = true;

            _shapeFileGrid25Labels.Labels.AddCategory("L");
            _shapeFileGrid25Labels.Labels.AddCategory("T");
            _shapeFileGrid25Labels.Labels.AddCategory("R");
            _shapeFileGrid25Labels.Labels.AddCategory("B");
            _shapeFileGrid25Labels.Labels.AddCategory("MG");
            _shapeFileGrid25Labels.Labels.AddCategory("MT");
            _shapeFileGrid25Labels.Labels.AddCategory("MZ");
            _shapeFileGrid25Labels.Labels.AddCategory("C");

            for (int n = 0; n < _shapeFileGrid25Labels.Labels.NumCategories - 1; n++)
            {
                _shapeFileGrid25Labels.Labels.Category[n].With(c =>
                {
                    switch (c.Name)
                    {
                        //Minor grid labels
                        case "T":
                        case "L":
                        case "B":
                        case "R":
                        case "C":
                            c.Expression = $@"[Location] =  ""{c.Name}""";
                            c.FontSize = (int)_labelPropertiesDictionary["minorGridLabelSize"];
                            c.FontColor = _labelPropertiesDictionary["minorGridLabelColor"];
                            c.FontBold = _labelPropertiesDictionary["minorGridLabelFontBold"] == 1;
                            break;

                        //Major grid labels
                        case "MG":
                            c.Expression = $@"[Location] =  ""MG""";
                            c.FontSize = (int)_labelPropertiesDictionary["majorGridLabelSize"];
                            c.FontBold = true;
                            c.FontColor = (uint)_labelPropertiesDictionary["majorGridLabelColor"];
                            break;

                        //Map title label
                        case "MT":
                            c.Expression = $@"[Location] =  ""MT""";
                            c.FontSize = 15;
                            c.FontBold = true;
                            c.Alignment = tkLabelAlignment.laCenterRight;
                            break;

                        //Map zone label
                        case "MZ":
                            c.Expression = $@"[Location] =  ""MZ""";
                            c.FontSize = 12;
                            c.Alignment = tkLabelAlignment.laCenterRight;
                            break;
                    }
                });
            }
        }
    }
}
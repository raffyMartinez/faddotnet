using MapWinGIS;
using System;
using System.Collections.Generic;

namespace FAD3
{
    public class Grid25LabelManager : IDisposable
    {
        private Shapefile _shapeFileGrid25Labels = new Shapefile();                 //the point shapefile that holds one label
        private Dictionary<string, uint> _labelPropertiesDictionary;                //reference to a dictionary that holds grid and label properties
        private int _cellSize = FishingGrid.Grid25.MinorGridCellSizeMeters;         //refers to the cell size of the minor grid
        private int _mgSize = FishingGrid.Grid25.MajorGridSizeMeters;               //refers to the cell size of a major grid
        private bool _wrappedLabels;                                                //if labels are wrapped if the minor grid is not a 4 sided polygon
        private int _iFldLocation;                                                  //refers to the table field for Location
        private int _iFLdLabel;                                                     //refers to the table field for Label
        public string MapTitle { get; set; }                                        //refers to the map title
        public bool _disposed;
        private int _labelDistance;

        /// <summary>
        /// returns the shapefile containing point labels
        /// </summary>
        public Shapefile Grid25Labels
        {
            get { return _shapeFileGrid25Labels; }
        }

        /// <summary>
        /// creates a shapefile for grid labels given filename
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="labelProperties"></param>
        public void LoadLabelShapefile(string fileName, Dictionary<string, uint> labelProperties, Shapefile sfLabelPath)
        {
            _labelPropertiesDictionary = labelProperties;

            _shapeFileGrid25Labels = new Shapefile();
            _shapeFileGrid25Labels.Open(fileName, null);
            _shapeFileGrid25Labels.GenerateLabels(_shapeFileGrid25Labels.FieldIndexByName["Label"], tkLabelPositioning.lpCentroid);
            SetupLabelProperties();
        }

        /// <summary>
        /// removes all points from the shapefile and clears labels
        /// </summary>
        public void ClearLabels()
        {
            _shapeFileGrid25Labels.Labels.Clear();
            _shapeFileGrid25Labels.EditClear();
        }

        public bool LabelGrid(Shapefile sfLabelPath, Dictionary<string, uint> labelProperties, string mapTitle)
        {
            bool success = false;
            if (_shapeFileGrid25Labels != null)
            {
                _labelPropertiesDictionary = labelProperties;
                MapTitle = mapTitle;

                ClearLabels();

                _wrappedLabels = _labelPropertiesDictionary["minorGridLabelWrapped"] == 1;
                _labelDistance = (int)_labelPropertiesDictionary["minorGridLabelDistance"];
                if (_wrappedLabels)
                {
                    for (int shp = 0; shp < sfLabelPath.NumShapes; shp++)
                    {
                        GetLines(sfLabelPath.Shape[shp]);
                    }
                }
                else
                {
                    GetLines(sfLabelPath.Extents.ToShape());
                }

                SetMapTitle(sfLabelPath.Extents);
                success = _shapeFileGrid25Labels.NumShapes > 0;
            }

            return success;
        }

        /// <summary>
        /// Extracts the lines that make up the label path.
        /// A direction variable stores the direction of a line.
        /// </summary>
        /// <param name="shapePathSource"></param>
        private void GetLines(Shape shapePathSource)
        {
            for (int n = 0; n < shapePathSource.numPoints; n++)
            {
                var pt1 = shapePathSource.Point[n];
                var pt2 = shapePathSource.Point[n + 1];
                if (pt2 != null)
                {
                    var shp = new Shape();
                    if (shp.Create(ShpfileType.SHP_POLYLINE))
                    {
                        var direction = "";
                        shp.InsertPoint(pt1, 0);
                        shp.InsertPoint(pt2, 1);

                        if (pt1.x < pt2.x)
                            direction = "right";

                        if (pt1.x > pt2.x)
                            direction = "left";

                        if (pt1.y > pt2.y)
                            direction = "down";

                        if (pt1.y < pt2.y)
                            direction = "up";

                        shp.Key = direction;
                    }

                    PrepareLabelPath(shp);
                }
            }
        }

        /// <summary>
        /// Receives individual line segments and calculates start points and location of label points
        /// </summary>
        /// <param name="shapePath"></param>
        private void PrepareLabelPath(Shape shapePath)
        {
            var key = shapePath.Key;
            var startNode = 0;
            var offset = 0;
            var segments = 0;
            var labelY = 0;
            var labelX = 0;
            var pt0 = shapePath.Point[0];
            var pt1 = shapePath.Point[1];
            var position = "";
            var categoryIndex = 0;
            switch (key)
            {
                case "right":
                case "left":
                    segments = Math.Abs((int)pt0.x - (int)pt1.x) / _cellSize;
                    if (pt0.x.CompareTo(pt1.x) < 0)
                    {
                        startNode = ((int)(pt0.x / _mgSize)) * _mgSize;
                        offset = (int)pt0.x - startNode;
                    }
                    else
                    {
                        startNode = ((int)(pt1.x / _mgSize)) * _mgSize;
                        offset = (int)pt1.x - startNode;
                    }

                    labelY = (int)pt0.y + _labelDistance;
                    position = "top";
                    categoryIndex = 0;
                    if (key == "left")
                    {
                        categoryIndex = 2;
                        position = "bottom";
                        labelY = (int)pt0.y - _labelDistance;
                    }
                    PlaceLabels(segments, "horizontal", offset, 0, labelY, startNode, position, categoryIndex);
                    break;

                case "up":
                case "down":
                    segments = Math.Abs((int)pt0.y - (int)pt1.y) / _cellSize;
                    if (pt0.y.CompareTo(pt1.y) < 0)
                    {
                        startNode = ((int)(pt0.y / _mgSize)) * _mgSize;
                        offset = (int)pt0.y - startNode;
                    }
                    else
                    {
                        startNode = ((int)(pt1.y / _mgSize)) * _mgSize;
                        offset = (int)pt1.y - startNode;
                    }

                    labelX = (int)pt0.x + _labelDistance;
                    categoryIndex = 1;
                    position = "right";
                    if (key == "up")
                    {
                        categoryIndex = 3;
                        position = "left";
                        labelX = (int)pt0.x - _labelDistance;
                    }

                    SetupLabelProperties();

                    PlaceLabels(segments, "vertical", offset, labelX, 0, startNode, position, categoryIndex);
                    break;
            }
        }

        /// <summary>
        /// Creates point shapefiles and labels
        /// </summary>
        /// <param name="segments"></param>
        /// <param name="orientation"></param>
        /// <param name="offset"></param>
        /// <param name="labelX"></param>
        /// <param name="labelY"></param>
        /// <param name="startNode"></param>
        /// <param name="position"></param>
        /// <param name="categoryIndex"></param>
        private void PlaceLabels(int segments, string orientation, int offset, int labelX, int labelY, int startNode, string position, int categoryIndex)
        {
            _iFldLocation = _shapeFileGrid25Labels.FieldIndexByName["Location"];
            _iFLdLabel = _shapeFileGrid25Labels.FieldIndexByName["Label"];
            var labelValue = "";
            var labelNode = 0;
            int offsetSegments = offset / _cellSize;
            var labelDistance = 0;
            var start = 0;
            int y = 0;

            start = offsetSegments;
            for (int n = offsetSegments; n < segments + offsetSegments; n++)
            {
                Point pt = new Point();
                if (_wrappedLabels)
                {
                    if (y > 24) y = 0;
                }
                else
                {
                    if (labelNode == 1 || labelNode == 'Y')
                    {
                        y = 0;
                        start = 0;
                    }
                }
                if (orientation == "vertical")
                {
                    if (position == "left")
                    {
                        pt.x = labelX - labelDistance;
                    }
                    else
                    {
                        pt.x = labelX + labelDistance;
                    }

                    pt.y = startNode + (n * _cellSize) + _cellSize / 2;
                    labelNode = 25 - start - y;
                    labelValue = labelNode.ToString();
                }
                else
                {
                    if (position == "top")
                    {
                        pt.y = labelY + labelDistance;
                    }
                    else
                    {
                        pt.y = labelY - labelDistance;
                    }
                    pt.x = startNode + (n * _cellSize) + _cellSize / 2;
                    labelNode = 'A' + start + y;
                    labelValue = ((char)labelNode).ToString();
                }

                Shape shp = new Shape();
                if (shp.Create(ShpfileType.SHP_POINT) && shp.InsertPoint(pt, 0))
                {
                    var iShp = _shapeFileGrid25Labels.EditAddShape(shp);
                    if (iShp >= 0)
                    {
                        _shapeFileGrid25Labels.EditCellValue(_iFLdLabel, iShp, labelValue);
                        _shapeFileGrid25Labels.EditCellValue(_iFldLocation, iShp, position.Substring(0, 1).ToUpper());
                        _shapeFileGrid25Labels.Labels.AddLabel(labelValue, pt.x, pt.y, 0, categoryIndex);
                    }
                }
                y++;
            }
        }

        /// <summary>
        /// Adds the labels of the major grids that form part of the fishing ground map
        /// </summary>
        /// <param name="labels"></param>
        public void AddMajorGridLabels(List<(int GridNo, double x, double y)> labels)
        {
            foreach (var item in labels)
            {
                var shp = new Shape();
                if (shp.Create(ShpfileType.SHP_POINT) && shp.AddPoint(item.x, item.y) >= 0)
                {
                    var iShp = _shapeFileGrid25Labels.EditAddShape(shp);
                    if (iShp >= 0)
                    {
                        _shapeFileGrid25Labels.EditCellValue(_iFldLocation, iShp, "MG");
                        _shapeFileGrid25Labels.EditCellValue(_iFLdLabel, iShp, item.GridNo);
                        _shapeFileGrid25Labels.Labels.AddLabel(item.GridNo.ToString(), shp.Point[0].x, shp.Point[0].y, 0, 4);
                    }
                }
            }
        }

        /// <summary>
        /// Places map title and footer
        /// </summary>
        /// <param name="minorGridExtent"></param>
        private void SetMapTitle(Extents minorGridExtent)
        {
            //set up map title
            var shp = new Shape();
            if (shp.Create(ShpfileType.SHP_POINT))
            {
                if (shp.AddPoint(minorGridExtent.xMin, minorGridExtent.yMax + 4000 + _labelDistance) >= 0)
                {
                    var iShp = _shapeFileGrid25Labels.EditAddShape(shp);
                    if (iShp >= 0)
                    {
                        if (MapTitle.Length == 0)
                        {
                            MapTitle = "New fishing ground map";
                        }
                        _shapeFileGrid25Labels.EditCellValue(_iFldLocation, iShp, "MT");
                        _shapeFileGrid25Labels.EditCellValue(_iFLdLabel, iShp, MapTitle);
                        _shapeFileGrid25Labels.Labels.AddLabel(MapTitle, shp.Point[0].x, shp.Point[0].y, 0, 5);
                    }
                }
            }

            //setup UTM zone footer
            shp = new Shape();
            if (shp.Create(ShpfileType.SHP_POINT))
            {
                if (shp.AddPoint(minorGridExtent.xMin, minorGridExtent.yMin - 3000 - _labelDistance) >= 0)
                {
                    var iShp = _shapeFileGrid25Labels.EditAddShape(shp);
                    if (iShp >= 0)
                    {
                        _shapeFileGrid25Labels.EditCellValue(_iFldLocation, iShp, "MZ");
                        var arr = _shapeFileGrid25Labels.GeoProjection.ProjectionName.Split(' ');
                        var zone = $"Zone: {arr[5]}";
                        _shapeFileGrid25Labels.EditCellValue(_iFLdLabel, iShp, zone);
                        _shapeFileGrid25Labels.Labels.AddLabel(zone, shp.Point[0].x, shp.Point[0].y, 0, 6);
                    }
                }
            }
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
                    _labelPropertiesDictionary = null;
                }
                if (_shapeFileGrid25Labels != null)
                {
                    _shapeFileGrid25Labels.Close();
                    _shapeFileGrid25Labels = null;
                }
                _disposed = true;
            }
        }

        /// <summary>
        /// Class constructor and receives the shapefile path where the labels will follow.
        /// The shapefile path is a 0 distance buffer of the minor grids and may not be a 4 sided polygon
        /// </summary>
        /// <param name="sfLabelPath"></param>
        /// <param name="labelProperties"></param>
        public Grid25LabelManager(GeoProjection geoProjection)
        {
            if (_shapeFileGrid25Labels.CreateNewWithShapeID("", ShpfileType.SHP_POINT))
            {
                _shapeFileGrid25Labels.EditAddField("Location", FieldType.STRING_FIELD, 1, 2);
                _shapeFileGrid25Labels.EditAddField("Label", FieldType.STRING_FIELD, 1, 4);
                _shapeFileGrid25Labels.GeoProjection = geoProjection;
            }
        }

        /// <summary>
        /// Sets the appearance of labels and label categories
        /// </summary>
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
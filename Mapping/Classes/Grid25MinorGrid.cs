using AxMapWinGIS;
using MapWinGIS;
using System;
using System.Linq;

namespace FAD3
{
    public class Grid25MinorGrid : IDisposable
    {
        private bool _disposed = false;
        private Shapefile _shapefileMinorGridLines = null;                  //shapefile of the minor grid lines which are composed of
                                                                            //vertical and horizontal lines crisscrossing to form a grid

        private AxMap _axMap;                                               //holds a reference to the map control in the map form

        private bool _isIntersect;                                          //if the MBR is an intersection of the selected
                                                                            //major grid and the selection box

        private Extents _minorGridExtents;                                  //extent of the minor grid
        private int _minorGridMBRHeight;                                    //height of the MBR of the minor grid
        private int _minorGridMBRWidth;                                     //width of the MBR of the minor grid
        private int _minorGridOriginX;                                      // x origin of the minor grid
        private int _minorGridOriginY;                                      // y origin of the minor grid
        private int _minorGridRows;                                         //number of minor grid rows
        private int _minorGridColumns;                                      //number of minor grid columns
        private const int CELLSIDE = 2000;                                  //length of one side of a minor grid

        private Grid25MajorGrid _grid25MajorGrid;

        /// <summary>
        /// Passes a map control reference.
        /// </summary>
        /// <param name="mapControl"></param>
        public Grid25MinorGrid(AxMap mapControl, Grid25MajorGrid majorGrid)
        {
            _axMap = mapControl;
            _grid25MajorGrid = majorGrid;
        }

        /// <summary>
        /// Returns the minor grid shapefile.
        /// </summary>
        public Shapefile MinorGridLinesShapeFile
        {
            get { return _shapefileMinorGridLines; }
        }

        /// <summary>
        /// Public entry point to start creating minor grids. It also sets the extents of the minor grid.
        /// </summary>
        /// <param name="selectedMajorGridShapesExtent"></param>
        /// <param name="definitionExtent"></param>
        /// <returns></returns>
        public bool DefineMinorGrids(Extents selectedMajorGridShapesExtent, Extents definitionExtent)
        {
            var success = false;
            var mbrExtentIsMaxExtent = false;
            if (definitionExtent.ToShape().Contains(selectedMajorGridShapesExtent.ToShape()))
            {
                //minor grid extent is the extent of the selected major grid shapes
                _minorGridExtents = selectedMajorGridShapesExtent;
                mbrExtentIsMaxExtent = true;
            }
            else if (selectedMajorGridShapesExtent.ToShape().Contains(definitionExtent.ToShape()))
            {
                //minor grid extent is the extent of the selection box
                _minorGridExtents = definitionExtent;
            }
            else if (definitionExtent.ToShape().Intersects(selectedMajorGridShapesExtent.ToShape()))
            {
                var results = new object();
                if (definitionExtent.ToShape().GetIntersection(selectedMajorGridShapesExtent.ToShape(), ref results))
                {
                    //convets results object to an array of shapes that is a product of the intersection
                    object[] shapeArray = results as object[];
                    if (shapeArray != null)
                    {
                        Shape[] shapes = shapeArray.OfType<Shape>().ToArray();

                        //minor grid extent is the intersection of the selected major grids and the selection box
                        _minorGridExtents = shapes[0].Extents;

                        _isIntersect = true;
                    }
                }
            }
            else
            {
                //mbrExtent is outside of extent of selected major grid
                _minorGridExtents = null;
            }

            if (_minorGridExtents != null)
            {
                success = ConstructMinorGridLines(extentIsMaxExtent: mbrExtentIsMaxExtent);
            }

            //success means that a valid extent was created
            return success;
        }

        public bool LoadMinorGridShapefile(string fileName)
        {
            _shapefileMinorGridLines = new Shapefile();
            return _shapefileMinorGridLines.Open(fileName, null);
        }

        /// <summary>
        /// Clips the rectangular shaped minor grid lines to the selected major grids whose shape may not be a 4 sided polygon
        /// </summary>
        /// <param name="clippingShapefile"></param>
        /// <returns></returns>
        public bool ClipMinorGrid(Shapefile clippingShapefile)
        {
            _shapefileMinorGridLines = clippingShapefile.GetIntersection(false, _shapefileMinorGridLines, false, ShpfileType.SHP_POLYLINE);

            //When we clip a shapefile and the result is the same shapefile, the shapeID field is duplicated.
            //The original ID field is renamed to MWShapeI_1.
            //We delete the duplicate and rename MWShapeI_1 to MWShapeID.
            _shapefileMinorGridLines.EditDeleteField(0, null);
            _shapefileMinorGridLines.Field[0].Name = "MWShapeID";
            return _shapefileMinorGridLines.NumShapes > 0;
        }

        /// <summary>
        /// Constructs the minor grid lines. This yields a rectangular grid of lines which may be clipped later on.
        /// </summary>
        /// <param name="extentIsMaxExtent"></param>
        private bool ConstructMinorGridLines(bool extentIsMaxExtent)
        {
            var success = false;
            _shapefileMinorGridLines = new Shapefile();
            if (_shapefileMinorGridLines.CreateNewWithShapeID("", ShpfileType.SHP_POLYLINE))
            {
                _shapefileMinorGridLines.GeoProjection = _axMap.GeoProjection;
                _shapefileMinorGridLines.With(sf =>
                {
                    var ifldBoundary = sf.EditAddField("isBoundary", FieldType.STRING_FIELD, 1, 1);
                    var iFldDirection = sf.EditAddField("RowOrCol", FieldType.STRING_FIELD, 1, 1);
                    var ifldLineType = sf.EditAddField("LineType", FieldType.STRING_FIELD, 1, 2);

                    var ht = (int)Math.Abs(_minorGridExtents.yMax - _minorGridExtents.yMin);
                    var wdt = (int)Math.Abs(_minorGridExtents.xMax - _minorGridExtents.xMin);

                    if (extentIsMaxExtent)
                    {
                        _minorGridMBRHeight = ht;
                        _minorGridMBRWidth = wdt;
                    }
                    else
                    {
                        if (_grid25MajorGrid.InDefindeGridFromLayout)
                        {
                            _minorGridMBRHeight = ((ht / CELLSIDE) * CELLSIDE);
                            _minorGridMBRWidth = ((wdt / CELLSIDE) * CELLSIDE);
                        }
                        else
                        {
                            _minorGridMBRHeight = ((ht / CELLSIDE) * CELLSIDE) + CELLSIDE;
                            _minorGridMBRWidth = ((wdt / CELLSIDE) * CELLSIDE) + CELLSIDE;
                        }

                        if (_isIntersect)
                        {
                            if (_minorGridMBRWidth - wdt == CELLSIDE)
                                _minorGridMBRWidth = wdt;

                            if (_minorGridMBRHeight - ht == CELLSIDE)
                                _minorGridMBRHeight = ht;
                        }
                    }

                    _minorGridRows = (int)_minorGridMBRHeight / CELLSIDE;
                    _minorGridColumns = (int)_minorGridMBRWidth / CELLSIDE;
                    _minorGridOriginX = (int)(_minorGridExtents.xMin / CELLSIDE) * CELLSIDE;
                    _minorGridOriginY = (int)(_minorGridExtents.yMin / CELLSIDE) * CELLSIDE;

                    _minorGridExtents.SetBounds(_minorGridOriginX, _minorGridOriginY, 0, _minorGridMBRWidth, _minorGridMBRHeight, 0);

                    //now construct the row lines of the minor grid
                    for (int row = 1; row < _minorGridRows; row++)
                    {
                        Shape shp = new Shape();
                        if (shp.Create(ShpfileType.SHP_POLYLINE))
                        {
                            var ptX = _minorGridOriginX;
                            var ptY = (row * CELLSIDE) + _minorGridOriginY;
                            shp.AddPoint(ptX, ptY);

                            ptX = _minorGridOriginX + _minorGridMBRWidth;
                            ptY = (row * CELLSIDE) + _minorGridOriginY;
                            shp.AddPoint(ptX, ptY);

                            var shpIndex = _shapefileMinorGridLines.EditAddShape(shp);
                            _shapefileMinorGridLines.EditCellValue(iFldDirection, shpIndex, "R");
                            _shapefileMinorGridLines.EditCellValue(ifldBoundary, shpIndex, "F");

                            if (ptY % FishingGrid.MajorGridSizeMeters == 0)
                                _shapefileMinorGridLines.EditCellValue(ifldLineType, shpIndex, "MG");

                            if (row == 1)
                                _shapefileMinorGridLines.EditCellValue(ifldBoundary, shpIndex, "T");

                            if (row == _minorGridRows)
                                _shapefileMinorGridLines.EditCellValue(ifldBoundary, shpIndex, "T");
                        }
                    }

                    //construct the column lines
                    for (int col = 1; col < _minorGridColumns; col++)
                    {
                        Shape shp = new Shape();
                        if (shp.Create(ShpfileType.SHP_POLYLINE))
                        {
                            var ptX = (col * CELLSIDE) + _minorGridOriginX;
                            var ptY = _minorGridOriginY;
                            shp.AddPoint(ptX, ptY);

                            ptX = (col * CELLSIDE) + _minorGridOriginX;
                            ptY = _minorGridOriginY + _minorGridMBRHeight;
                            shp.AddPoint(ptX, ptY);

                            var shpIndex = _shapefileMinorGridLines.EditAddShape(shp);
                            _shapefileMinorGridLines.EditCellValue(iFldDirection, shpIndex, "C");
                            _shapefileMinorGridLines.EditCellValue(ifldBoundary, shpIndex, "F");

                            if (ptX % FishingGrid.MajorGridSizeMeters == 0)
                                _shapefileMinorGridLines.EditCellValue(ifldLineType, shpIndex, "MG");

                            if (col == 1)
                                _shapefileMinorGridLines.EditCellValue(ifldBoundary, shpIndex, "T");

                            if (col == _minorGridColumns)
                                _shapefileMinorGridLines.EditCellValue(ifldBoundary, shpIndex, "T");
                        }
                    }

                    success = true;
                });
            }

            return success;
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
                }
                if (_shapefileMinorGridLines != null)
                {
                    _shapefileMinorGridLines.Close();
                    _shapefileMinorGridLines = null;
                    _minorGridExtents = null;
                    _axMap = null;
                }
                //_grid25MajorGrid = null;
                _disposed = true;
            }
        }
    }
}
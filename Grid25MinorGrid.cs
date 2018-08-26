using System;
using AxMapWinGIS;
using MapWinGIS;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FAD3
{
    public class Grid25MinorGrid : IDisposable
    {
        private bool _disposed = false;
        private Shapefile _shapefileMinorGridLines = null;                 //shapefile of the minor grid lines which are composed of
                                                                           //vertical and horizontal lines crisscrossing to form a grid

        private AxMap _axMap;

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

        //public Grid25MinorGrid(AxMap mapControl, tkWgs84Projection grid25Geoprojection, Grid25MajorGrid grid25MajorGrid)
        public Grid25MinorGrid(AxMap mapControl)
        {
            _axMap = mapControl;
        }

        public Shapefile MinorGridLinesShapeFile
        {
            get { return _shapefileMinorGridLines; }
        }

        public bool DefineMinorGrids(Extents selectedShapesExtent, Extents selectionBoxExtent)
        {
            var success = false;
            var mbrExtentIsMaxExtent = false;
            if (selectionBoxExtent.ToShape().Contains(selectedShapesExtent.ToShape()))
            {
                _minorGridExtents = selectedShapesExtent;
                mbrExtentIsMaxExtent = true;
            }
            else if (selectedShapesExtent.ToShape().Contains(selectionBoxExtent.ToShape()))
            {
                _minorGridExtents = selectionBoxExtent;
            }
            else if (selectionBoxExtent.ToShape().Intersects(selectedShapesExtent.ToShape()))
            {
                var results = new object();
                if (selectionBoxExtent.ToShape().GetIntersection(selectedShapesExtent.ToShape(), ref results))
                {
                    object[] shapeArray = results as object[];
                    if (shapeArray != null)
                    {
                        Shape[] shapes = shapeArray.OfType<Shape>().ToArray();
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

            return success;
        }

        /// <summary>
        /// constructs the minor grid lines
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
                        _minorGridMBRHeight = ((ht / CELLSIDE) * CELLSIDE) + CELLSIDE;
                        _minorGridMBRWidth = ((wdt / CELLSIDE) * CELLSIDE) + CELLSIDE;
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
                }
                //_grid25MajorGrid = null;
                _disposed = true;
            }
        }
    }
}
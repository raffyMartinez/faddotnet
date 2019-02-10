using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxMapWinGIS;
using MapWinGIS;

namespace FAD3.Mapping.Classes
{
    public class Grid25LayoutHelper : IDisposable
    {
        private AxMap _axMap;
        private Grid25MajorGrid _majorGrid;
        private Shapefile _sfLayout;
        private int _rows;
        private int _columns;
        private int _overLap;
        private Extents _layoutExtents;
        private Extents _selectedMajorGridShapesExtent;
        private bool _disposed = false;
        private int _hsfLayout;
        private int _hCursorDefineLayout;

        public delegate void LayoutCreatededEvent(Grid25LayoutHelper s, Grid25LayoutHelperEventArgs e);
        public event LayoutCreatededEvent LayerCreated;

        public Extents SelectedMajorGridExtents
        {
            get { return _selectedMajorGridShapesExtent; }
        }

        public Shapefile LayoutShapeFile
        {
            get { return _sfLayout; }
        }

        public Grid25MajorGrid MajorGrid
        {
            get { return _majorGrid; }
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
                    _majorGrid = null;
                }
                _sfLayout.Close();
                _sfLayout = null;
                _layoutExtents = null;
                _selectedMajorGridShapesExtent = null;

                _axMap.SelectBoxFinal -= OnMapSelectBoxFinal;
                _axMap = null;
                _disposed = true;
            }
        }

        public void LayoutDetails(int rows, int columns, int overlap)
        {
            _rows = rows;
            _columns = columns;
            _overLap = overlap;
        }

        public Grid25LayoutHelper(Grid25MajorGrid majorGrid, int iconHandle)
        {
            _majorGrid = majorGrid;
            _axMap = _majorGrid.MapControl;
            _axMap.SelectBoxFinal += OnMapSelectBoxFinal;
            _axMap.SendSelectBoxFinal = true;
            //_axMap.CursorMode = tkCursorMode.cmSelection;
            _hCursorDefineLayout = iconHandle;
            //_axMap.MapCursor = tkCursor.crsrUserDefined;
            //_axMap.UDCursorHandle = _hCursorDefineLayout;
            //_sfLayout = new MapWinGIS.Shapefile();
            for (int n = 0; n < _majorGrid.SelectedShapeGridNumbers.Count; n++)
            {
                _majorGrid.Grid25Grid.ShapeSelected[_majorGrid.SelectedShapeGridNumbers[n]] = true;
            }
            _selectedMajorGridShapesExtent = _majorGrid.Grid25Grid.BufferByDistance(0, 0, true, true).Extents;
            _majorGrid.Grid25Grid.SelectNone();
        }

        private void OnMapSelectBoxFinal(object sender, _DMapEvents_SelectBoxFinalEvent e)
        {
            if (_axMap.CursorMode == tkCursorMode.cmSelection && _axMap.UDCursorHandle == _hCursorDefineLayout)
            {
                var extL = 0D;
                var extR = 0D;
                var extT = 0D;
                var extB = 0D;

                _axMap.PixelToProj(e.left, e.top, ref extL, ref extT);
                _axMap.PixelToProj(e.right, e.bottom, ref extR, ref extB);

                _layoutExtents = new Extents();
                _layoutExtents.SetBounds(
                    (int)(extL / 2000) * 2000,
                    (int)(extB / 2000) * 2000,
                    0,
                    (int)(extR / 2000) * 2000,
                    (int)(extT / 2000) * 2000,
                    0);

                _layoutExtents = IntersectSelectionBoxWithMajorGridSelection();

                if (_layoutExtents != null && LayerCreated != null)
                {
                    Grid25LayoutHelperEventArgs lhe = new Grid25LayoutHelperEventArgs(_layoutExtents);
                    LayerCreated(this, lhe);

                    SetupLayout(lhe.Rows, lhe.Columns, lhe.Overlap);
                }
            }
        }

        private Extents IntersectSelectionBoxWithMajorGridSelection()
        {
            if (_layoutExtents.ToShape().Contains(_selectedMajorGridShapesExtent.ToShape()))
            {
                return _selectedMajorGridShapesExtent;
            }
            else if (_selectedMajorGridShapesExtent.ToShape().Contains(_layoutExtents.ToShape()))
            {
                return _layoutExtents;
            }
            else if (_layoutExtents.ToShape().Intersects(_selectedMajorGridShapesExtent.ToShape()))
            {
                var results = new object();
                if (_layoutExtents.ToShape().GetIntersection(_selectedMajorGridShapesExtent.ToShape(), ref results))
                {
                    //convets results object to an array of shapes that is a product of the intersection
                    object[] shapeArray = results as object[];
                    if (shapeArray != null)
                    {
                        Shape[] shapes = shapeArray.OfType<Shape>().ToArray();

                        //minor grid extent is the intersection of the selected major grids and the selection box
                        return shapes[0].Extents;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public bool HasCompletePanelTitles()
        {
            if (_sfLayout.NumShapes == 0)
            {
                return false;
            }
            else
            {
                int fldTitle = _sfLayout.FieldIndexByName["Title"];
                for (int n = 0; n < _sfLayout.NumShapes; n++)
                {
                    string title = _sfLayout.CellValue[fldTitle, n].ToString();
                    if (title.Length == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool SetupLayout(int rows, int columns, int overlap)
        {
            if (_layoutExtents != null)
            {
                _overLap = overlap;
                _rows = rows;
                _columns = columns;

                int expandLeft = 0;
                int expandRight = 0;
                int expandTop = 0;
                int expandBottom = 0;

                if (_hsfLayout > 0)
                {
                    _majorGrid.MapLayers.RemoveLayer(_hsfLayout);
                    _sfLayout.Close();
                }

                int panelHeight = ((int)(_layoutExtents.yMax - _layoutExtents.yMin)) / rows;
                int panelWidth = ((int)(_layoutExtents.xMax - _layoutExtents.xMin)) / columns;

                Point origin = new Point
                {
                    x = _layoutExtents.xMin,
                    y = _layoutExtents.yMin
                };

                _sfLayout = new Shapefile();
                _sfLayout.CreateNewWithShapeID("", ShpfileType.SHP_POLYGON);
                _sfLayout.GeoProjection = _axMap.GeoProjection;
                int fldPanelNo = _sfLayout.EditAddField("PanelNumber", FieldType.INTEGER_FIELD, 0, 2);
                int fldTitle = _sfLayout.EditAddField("Title", FieldType.STRING_FIELD, 0, 50);
                _sfLayout.DefaultDrawingOptions.FillVisible = false;
                _sfLayout.DefaultDrawingOptions.LineWidth = 2;
                _sfLayout.DefaultDrawingOptions.LineColor = new Utils().ColorByName(tkMapColor.DarkBlue);

                if (rows == 1 && columns == 1)
                {
                    _sfLayout.EditAddShape(_layoutExtents.ToShape());
                    _sfLayout.EditCellValue(fldPanelNo, 0, 1);
                }
                else
                {
                    int x = 0;
                    int y = 0;
                    int iShp = 0;
                    for (int r = 1; r <= _rows; r++)
                    {
                        for (int c = 1; c <= _columns; c++)
                        {
                            expandTop = 0;
                            if (r < rows)
                            {
                                expandTop = 1;
                            }

                            expandBottom = 0;
                            if (r > 1)
                            {
                                expandBottom = 1;
                            }

                            expandLeft = 0;
                            if (c > 1)
                            {
                                expandLeft = 1;
                            }

                            expandRight = 0;
                            if (c < columns)
                            {
                                expandRight = 1;
                            }

                            Extents panelExtent = new Extents();
                            panelExtent.SetBounds(
                                origin.x + ((c - 1) * panelWidth) - (expandLeft * _overLap * 2000),
                                origin.y + ((r - 1) * panelHeight) - (expandBottom * _overLap * 2000),
                                0,
                                origin.x + (c * panelWidth) + (expandRight * (_overLap) * 2000),
                                origin.y + (r * panelHeight) + (expandTop * (_overLap) * 2000),
                                0
                                );
                            iShp = _sfLayout.EditAddShape(panelExtent.ToShape());
                            _sfLayout.EditCellValue(fldPanelNo, iShp, iShp + 1);
                        }
                    }
                }
                _hsfLayout = _majorGrid.MapLayers.AddLayer(_sfLayout, "Layout frame", true, true);
                _majorGrid.MapLayers.ClearAllSelections();
                _axMap.Redraw();
                return _sfLayout.NumShapes > 0;
            }
            else
            {
                return false;
            }
        }

        public Extents LayoutExtents
        {
            get { return _layoutExtents; }
        }
    }
}
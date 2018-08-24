using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxMapWinGIS;
using MapWinGIS;

namespace FAD3
{
    public class Grid25MapHelper : IDisposable
    {
        private string _utmZone;
        private AxMap _axMap;
        private tkWgs84Projection _Grid25Geoprojection;
        private const int GRIDSIZE = 50000;
        private const int CURSORWIDTH = 5;
        private Shapefile _grid25Grid = null;
        private bool _disposed = false;
        private bool _selectionFromSelectBox = false;
        private int[] _selectedShapeIndexes;
        private List<int> _selectedShapeGridNumbers = new List<int>();

        public tkWgs84Projection Grid25Geoprojection
        {
            get { return _Grid25Geoprojection; }
        }

        public List<int> SelectedShapeGridNumbers
        {
            get { return _selectedShapeGridNumbers; }
        }

        public void ClearSelectedGrids()
        {
            var ifldToGrid = _grid25Grid.FieldIndexByName["toGrid"];
            foreach (var item in _selectedShapeGridNumbers)
            {
                _grid25Grid.EditCellValue(ifldToGrid, item, "");
            }
            _selectedShapeGridNumbers.Clear();
            _grid25Grid.Categories.ApplyExpression(0);
            _axMap.Redraw();
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
                _grid25Grid.Close();
                _axMap = null;
                _disposed = true;
            }
        }

        public Shapefile Grid25Grid
        {
            get { return _grid25Grid; }
        }

        /// <summary>
        /// constructor and sets default behaviour and WGS UTM projection of map control
        /// </summary>
        /// <param name="mapControl"></param>
        /// <param name="utmZone"></param>
        public Grid25MapHelper(AxMap mapControl, string utmZone)
        {
            _utmZone = utmZone;
            _axMap = mapControl;

            _axMap.SendMouseUp = true;
            _axMap.SendMouseDown = true;
            _axMap.SendSelectBoxFinal = true;

            _axMap.MouseUpEvent += OnMapMouseUp;
            _axMap.MouseDownEvent += OmMapMouseDown;
            _axMap.SelectBoxFinal += OnMapSelectBoxFinal;
            _axMap.DblClick += OnMapDoubleClick;
            _axMap.CursorMode = tkCursorMode.cmSelection;

            switch (_utmZone)
            {
                case "zone50":
                    _Grid25Geoprojection = tkWgs84Projection.Wgs84_UTM_zone_50N;
                    break;

                case "zone51":
                    _Grid25Geoprojection = tkWgs84Projection.Wgs84_UTM_zone_51N;
                    break;
            }

            GenerateMajorGrids();
        }

        /// <summary>
        /// adds to selected grids if it touches previously selected (red colored) grids or
        /// removes a selected grid if it is double-clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMapDoubleClick(object sender, EventArgs e)
        {
            var isTouching = false;
            var cellValue = "";
            var proceed = true;
            if (_grid25Grid.NumSelected == 1)
            {
                var ifldToGrid = _grid25Grid.FieldIndexByName["toGrid"];

                if (_selectedShapeGridNumbers.Contains(_selectedShapeIndexes[0]))
                {
                    //a grid that is found in List<>int_selectedShapeGridNumbers will be removed
                    _selectedShapeGridNumbers.Remove(_selectedShapeIndexes[0]);
                    cellValue = "";
                }
                else
                {
                    //test whether a new selected grid touches those previously selected
                    foreach (var item in _selectedShapeGridNumbers)
                    {
                        isTouching = _grid25Grid.Shape[item].Touches(_grid25Grid.Shape[_selectedShapeIndexes[0]]);
                        if (isTouching) break;
                    }

                    //if List<>int_selectedShapeGridNumbers is empty or
                    //a new selected grid touches previously selected grid,
                    //then it is added to the selection (red colored)
                    if (_selectedShapeGridNumbers.Count == 0 || isTouching)
                    {
                        cellValue = "T";
                        _selectedShapeGridNumbers.Add(_selectedShapeIndexes[0]);
                    }
                    else
                    {
                        proceed = false;
                    }
                }

                //categorize grids to change fill color
                if (proceed && _grid25Grid.EditCellValue(ifldToGrid, _selectedShapeIndexes[0], cellValue))
                {
                    //shapes with cellvalue of "T", upon categorizing will have a light red fill color
                    //otherwise it will have no fill color
                    _grid25Grid.Categories.ApplyExpression(0);
                    _grid25Grid.SelectNone();
                    _axMap.Redraw();
                }
            }
        }

        private void OmMapMouseDown(object sender, _DMapEvents_MouseDownEvent e)
        {
            _selectionFromSelectBox = false;
        }

        /// <summary>
        /// creates an extent defined by the selection box
        /// the extent will be used to select major grids
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMapSelectBoxFinal(object sender, _DMapEvents_SelectBoxFinalEvent e)
        {
            var extL = 0D;
            var extR = 0D;
            var extT = 0D;
            var extB = 0D;
            Extents ext = new Extents();

            _axMap.PixelToProj(e.left, e.top, ref extL, ref extT);
            _axMap.PixelToProj(e.right, e.bottom, ref extR, ref extB);
            ext.SetBounds(extL, extB, 0, extR, extT, 0);

            //pass the extent to SelectGrids function
            //FromSelectionBox is true because dragged a selection box
            SelectGrids(ext, SelectionFromSelectBox: true);
        }

        /// <summary>
        /// selects grids that intersects the extent
        /// selected grids are categorized and colored light red
        /// </summary>
        /// <param name="ext"></param>
        /// <param name="SelectionFromSelectBox"></param>
        private void SelectGrids(Extents ext, bool SelectionFromSelectBox = false)
        {
            var ifldToGrid = _grid25Grid.FieldIndexByName["toGrid"];
            _selectionFromSelectBox = SelectionFromSelectBox;

            //selects the grid using the extent
            object selectionResult = new object();
            if (_grid25Grid.SelectShapes(ext, 0, SelectMode.INTERSECTION, ref selectionResult))
            {
                _selectedShapeIndexes = (int[])selectionResult;

                if (_selectedShapeIndexes.Length > 0)
                {
                    //if the selection was made by drag-selecting on the grid
                    if (SelectionFromSelectBox)
                    {
                        //clears the previously selected, red colored grids
                        if (_selectedShapeGridNumbers.Count > 0)
                        {
                            foreach (var item in _selectedShapeGridNumbers)
                                _grid25Grid.EditCellValue(ifldToGrid, item, "");
                        }
                        _selectedShapeGridNumbers.Clear();

                        //adds selected shapes to a List<int> and sets toGrid column to "T"
                        for (int n = 0; n < _selectedShapeIndexes.Length; n++)
                        {
                            _grid25Grid.EditCellValue(ifldToGrid, _selectedShapeIndexes[n], "T");
                            _selectedShapeGridNumbers.Add(_selectedShapeIndexes[n]);
                        }

                        //categorizes grid and makes those with "T" in toGrid column  have a light red fill color
                        _grid25Grid.Categories.ApplyExpression(0);
                    }

                    //if the selection was made by clicking on a grid
                    else
                    {
                        //sets the color of the selected grid
                        Grid25Grid.SelectNone();
                        _grid25Grid.ShapeSelected[_selectedShapeIndexes[0]] = true;
                        _grid25Grid.SelectionAppearance = tkSelectionAppearance.saDrawingOptions;
                        _grid25Grid.SelectionDrawingOptions.FillColor = new Utils().ColorByName(tkMapColor.Yellow);
                        _grid25Grid.SelectionDrawingOptions.FillTransparency = 75;
                        _grid25Grid.SelectionDrawingOptions.LineColor = new Utils().ColorByName(tkMapColor.Red);
                        _grid25Grid.SelectionDrawingOptions.LineWidth = 2;
                    }
                    _axMap.Redraw();
                }
            }
        }

        private void OnMapMouseUp(object sender, _DMapEvents_MouseUpEvent e)
        {
            //we only proceed if a drag-select was not done
            if (!_selectionFromSelectBox)
            {
                var extL = 0D;
                var extR = 0D;
                var extT = 0D;
                var extB = 0D;
                Extents ext = new Extents();

                _axMap.PixelToProj(e.x - CURSORWIDTH, e.y - CURSORWIDTH, ref extL, ref extT);
                _axMap.PixelToProj(e.x + CURSORWIDTH, e.y + CURSORWIDTH, ref extR, ref extB);
                ext.SetBounds(extL, extB, 0, extR, extT, 0);

                //pass the extent to SelectGrids function
                //FromSelectionBox is false because we did not select-drag the mouse
                SelectGrids(ext, SelectionFromSelectBox: false);
            }
        }

        /// <summary>
        /// creates major grids on a utm zone
        /// </summary>
        private void GenerateMajorGrids()
        {
            var sf = new Shapefile();
            Shape shp;
            Point pt;
            var xOrigin = 0;
            var yOrigin = 0;
            var cols = 0;
            var rows = 0;
            var gridNumber = 0;
            var iFld = 0;
            var offsetColumns = 0;
            var iShp = 0;

            if (sf.CreateNewWithShapeID("", ShpfileType.SHP_POLYGON))
            {
                iFld = sf.EditAddField("Grid_no", FieldType.INTEGER_FIELD, 1, 4);
                sf.EditAddField("toGrid", FieldType.STRING_FIELD, 1, 1);
                sf.GeoProjection.SetWgs84Projection(_Grid25Geoprojection);

                //set the origin, rows and columns
                switch (_Grid25Geoprojection)
                {
                    case tkWgs84Projection.Wgs84_UTM_zone_50N:
                        xOrigin = 300000;
                        yOrigin = 800000;
                        cols = 15;
                        rows = 18;
                        offsetColumns = 0;
                        gridNumber = 1;
                        break;

                    case tkWgs84Projection.Wgs84_UTM_zone_51N:
                        xOrigin = 0;
                        yOrigin = 350000;
                        cols = 20;
                        rows = 41;
                        offsetColumns = 10;
                        gridNumber = 11;
                        break;
                }

                //build the major grids
                for (int row = 0; row < rows; row++)
                {
                    for (int col = 0; col < cols; col++)
                    {
                        shp = new Shape();
                        if (shp.Create(ShpfileType.SHP_POLYGON))
                        {
                            for (int n = 0; n < 5; n++)
                            {
                                pt = new MapWinGIS.Point();
                                switch (n)
                                {
                                    case 0:
                                        pt.x = xOrigin + (col * GRIDSIZE);
                                        pt.y = yOrigin + (row * GRIDSIZE);
                                        break;

                                    case 1:
                                        pt.x = xOrigin + (col * GRIDSIZE);
                                        pt.y = yOrigin + GRIDSIZE + (row * GRIDSIZE);
                                        break;

                                    case 2:
                                        pt.x = xOrigin + GRIDSIZE + (col * GRIDSIZE);
                                        pt.y = yOrigin + GRIDSIZE + (row * GRIDSIZE);
                                        break;

                                    case 3:
                                        pt.x = xOrigin + GRIDSIZE + (col * GRIDSIZE);
                                        pt.y = yOrigin + (row * GRIDSIZE);
                                        break;

                                    case 4:
                                        pt.x = xOrigin + (col * GRIDSIZE);
                                        pt.y = yOrigin + (row * GRIDSIZE);
                                        break;
                                }
                                shp.AddPoint(pt.x, pt.y);
                            }
                            iShp = sf.EditAddShape(shp);

                            if (iShp > 0)
                                sf.EditCellValue(iFld, iShp, gridNumber);

                            gridNumber++;
                        }
                    }
                    gridNumber += offsetColumns;
                }
            }

            if (sf.NumShapes > 0)
            {
                _grid25Grid = sf;
                ConfigureGridAppearance();
            }
        }

        /// <summary>
        /// set the appearance of selected and unselected grids
        /// </summary>
        private void ConfigureGridAppearance()
        {
            _grid25Grid.With(sf =>
            {
                //appearance for unselected grids
                sf.DefaultDrawingOptions.FillVisible = false;
                sf.DefaultDrawingOptions.LineWidth = 2;
                sf.DefaultDrawingOptions.LineColor = new Utils().ColorByName(tkMapColor.Red);
                var fldIndex = sf.FieldIndexByName["grid_no"];
                sf.GenerateLabels(fldIndex, tkLabelPositioning.lpCenter);
                sf.Labels.FontSize = 12;
                sf.Labels.FontBold = true;
                sf.Labels.FrameVisible = false;

                //create a category which will set the appearance of selected grids
                if (sf.StartEditingTable(null))
                {
                    var category = new ShapefileCategory();
                    category.Name = "Selected grid";
                    category.Expression = @"[toGrid] =""T""";
                    category.DrawingOptions.FillColor = new Utils().ColorByName(tkMapColor.Red);
                    category.DrawingOptions.FillTransparency = 25;
                    category.DrawingOptions.LineWidth = 2;
                    category.DrawingOptions.LineColor = new Utils().ColorByName(tkMapColor.Red);
                    sf.Categories.Add2(category);
                }
            });
        }
    }
}
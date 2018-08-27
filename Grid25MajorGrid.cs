using AxMapWinGIS;
using MapWinGIS;
using System;
using System.Linq;
using System.Collections.Generic;

namespace FAD3
{
    /// <summary>
    /// 1. creates major grids.
    /// 2. manages selection, deselection and coloring of major grids
    /// 3. makes available a List<int> of selected grids
    /// </summary>
    public class Grid25MajorGrid : IDisposable
    {
        private static FishingGrid.fadUTMZone _utmZone =
                FishingGrid.fadUTMZone.utmZone_Undefined;
        private AxMap _axMap;
        private tkWgs84Projection _Grid25Geoprojection;
        private const int GRIDSIZE = 50000;
        private const int CURSORWIDTH = 5;
        private Grid25MinorGrid _grid25MinorGrid;                               //helper class for managing minor grids
        private Grid25LabelManager _grid25LabelManager;                         //helper class for managing labels
        private Shapefile _shapefileMajorGrid;                                  //shapefile of major grids
        private Shapefile _shapefileMajorGridIntersect;                         //shapefile of major grid intersected with extent of minor grids
        private Shapefile _shapefileBoundingRectangle;                          //shapefile of MBR of fishing grid
        private bool _disposed = false;
        private bool _selectionFromSelectBox = false;
        private int[] _selectedShapeIndexes;                                    //holds the indexes of selected shapes
        private List<int> _listSelectedShapeGridNumbers = new List<int>();      //list major grid numbers
        private List<string> _listSidesToLabel;                                 //list of sides to be labelled
        private Extents _selectedShapesExtent = new Extents();
        private bool _inDefineMinorGrid;
        private int _minorGridLabelDistance;
        private int _minorGridLabelSize;
        private Dictionary<string, uint> _gridAndLabelProperties;
        private MapLayers _mapLayers;
        private List<int> _listGridLayers = new List<int>();                        //list containing layer handle of grid layers

        public MapLayers mapLayers
        {
            get { return _mapLayers; }
            set { _mapLayers = value; }
        }

        public Dictionary<string, uint> LabelAndGridProperties
        {
            get { return _gridAndLabelProperties; }
            set { _gridAndLabelProperties = value; }
        }

        /// <summary>
        /// returns the projection of the map control
        /// </summary>
        public tkWgs84Projection Grid25Geoprojection
        {
            get { return _Grid25Geoprojection; }
        }

        /// <summary>
        /// returns true if there is a major grid selection
        /// calculates extent of the selected major grids
        /// </summary>
        /// <param name="minorGridLabelDistance"></param>
        /// <param name="minorGridLabelSize"></param>
        /// <param name="SidesToLabel - contains a list of which sides of the map to label"></param>
        /// <param name="IconHandle - the int handle of icon"></param>
        /// <returns></returns>
        public bool DefineMinorGrid(int minorGridLabelDistance, int minorGridLabelSize, List<string> SidesToLabel, int IconHandle)
        {
            _inDefineMinorGrid = _listSelectedShapeGridNumbers.Count > 0;
            if (_inDefineMinorGrid)
            {
                foreach (var item in _listSelectedShapeGridNumbers)
                {
                    _shapefileMajorGrid.ShapeSelected[item] = true;
                }
                _listSidesToLabel = SidesToLabel;
                _minorGridLabelDistance = minorGridLabelDistance;
                _minorGridLabelSize = minorGridLabelSize;
                _selectedShapesExtent = _shapefileMajorGrid.BufferByDistance(0, 0, true, true).Extents;
                _shapefileMajorGrid.SelectNone();
                _axMap.MapCursor = tkCursor.crsrUserDefined;
                _axMap.UDCursorHandle = IconHandle;
            }
            return _inDefineMinorGrid;
        }

        /// <summary>
        /// setsup the UTM zone of the fishing grid
        /// </summary>
        public FishingGrid.fadUTMZone UTMZone
        {
            get { return _utmZone; }
            set
            {
                _utmZone = value;
                switch (_utmZone)
                {
                    case FishingGrid.fadUTMZone.utmZone50N:
                        _Grid25Geoprojection = tkWgs84Projection.Wgs84_UTM_zone_50N;
                        break;

                    case FishingGrid.fadUTMZone.utmZone51N:
                        _Grid25Geoprojection = tkWgs84Projection.Wgs84_UTM_zone_51N;
                        break;
                }
                FishingGrid.UTMZone = _utmZone;
                GenerateMajorGrids();
            }
        }

        /// <summary>
        /// makes available List<int> containing shape index of selected grids
        /// </summary>
        public List<int> SelectedShapeGridNumbers
        {
            get { return _listSelectedShapeGridNumbers; }
        }

        /// <summary>
        /// clears the grid of any selected (red-colored) cells
        /// </summary>
        public void ClearSelectedGrids()
        {
            var ifldToGrid = _shapefileMajorGrid.FieldIndexByName["toGrid"];
            foreach (var item in _listSelectedShapeGridNumbers)
            {
                _shapefileMajorGrid.EditCellValue(ifldToGrid, item, "");
            }
            _listSelectedShapeGridNumbers.Clear();
            _shapefileMajorGrid.Categories.ApplyExpression(0);
            _inDefineMinorGrid = false;
            _axMap.MapCursor = tkCursor.crsrMapDefault;

            foreach (var hLyr in _listGridLayers)
            {
                switch (_axMap.get_LayerName(hLyr))
                {
                    case "Labels":
                        _grid25LabelManager.ClearLabels();
                        break;

                    default:
                        _axMap.get_Shapefile(hLyr).EditClear();
                        break;
                }
                _mapLayers.RemoveLayer(hLyr);
            }
            _listGridLayers.Clear();
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
                    _listSelectedShapeGridNumbers.Clear();
                    _listSelectedShapeGridNumbers = null;
                }
                _shapefileMajorGrid.Close();
                _shapefileMajorGrid = null;
                _axMap = null;
                _disposed = true;
            }
        }

        /// <summary>
        /// returns the grid25 major grid shapefile
        /// </summary>
        public Shapefile Grid25Grid
        {
            get { return _shapefileMajorGrid; }
        }

        public Grid25LabelManager grid25LabelManager
        {
            get { return _grid25LabelManager; }
        }

        public Grid25MinorGrid minorGrids
        {
            get { return _grid25MinorGrid; }
        }

        /// <summary>
        /// constructor and sets default behaviour and WGS UTM projection of map control
        /// </summary>
        /// <param name="mapControl"></param>
        /// <param name="utmZone"></param>
        public Grid25MajorGrid(AxMap mapControl)
        {
            _axMap = mapControl;

            _axMap.SendMouseUp = true;
            _axMap.SendMouseDown = true;
            _axMap.SendSelectBoxFinal = true;

            _axMap.MouseUpEvent += OnMapMouseUp;
            _axMap.MouseDownEvent += OnMapMouseDown;
            _axMap.SelectBoxFinal += OnMapSelectBoxFinal;
            _axMap.DblClick += OnMapDoubleClick;
            _axMap.CursorMode = tkCursorMode.cmSelection;
            _utmZone = FishingGrid.fadUTMZone.utmZone51N;

            _grid25MinorGrid = new Grid25MinorGrid(_axMap);
        }

        /// <summary>
        /// adds to selected grids if it touches previously selected (red colored) grids or
        /// removes a selected grid if it is double-clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMapDoubleClick(object sender, EventArgs e)
        {
            if (!_inDefineMinorGrid)
            {
                var isTouching = false;
                var cellValue = "";
                var proceed = true;
                if (_shapefileMajorGrid.NumSelected == 1)
                {
                    var ifldToGrid = _shapefileMajorGrid.FieldIndexByName["toGrid"];

                    if (_listSelectedShapeGridNumbers.Contains(_selectedShapeIndexes[0]))
                    {
                        //a grid that is found in List<>int_selectedShapeGridNumbers will be removed
                        _listSelectedShapeGridNumbers.Remove(_selectedShapeIndexes[0]);
                        cellValue = "";
                    }
                    else
                    {
                        //test whether a new selected grid touches those previously selected
                        foreach (var item in _listSelectedShapeGridNumbers)
                        {
                            isTouching = _shapefileMajorGrid.Shape[item].Touches(_shapefileMajorGrid.Shape[_selectedShapeIndexes[0]]);
                            if (isTouching) break;
                        }

                        //if List<>int_selectedShapeGridNumbers is empty or
                        //a new selected grid touches previously selected grid,
                        //then toGrid column content is changed from "" to "T"
                        if (_listSelectedShapeGridNumbers.Count == 0 || isTouching)
                        {
                            cellValue = "T";
                            _listSelectedShapeGridNumbers.Add(_selectedShapeIndexes[0]);
                        }
                        else
                        {
                            proceed = false;
                        }
                    }

                    //categorize grids to change fill color
                    if (proceed && _shapefileMajorGrid.EditCellValue(ifldToGrid, _selectedShapeIndexes[0], cellValue))
                    {
                        //shapes with cellvalue of "T", upon categorizing will have a light red fill color
                        //otherwise it will have no fill color
                        _shapefileMajorGrid.Categories.ApplyExpression(0);
                    }
                }
                _shapefileMajorGrid.SelectNone();
            }
        }

        private void OnMapMouseDown(object sender, _DMapEvents_MouseDownEvent e)
        {
            _selectionFromSelectBox = false;
        }

        /// <summary>
        /// creates an extent defined by the selection box
        /// the extent will be used to select major grids or,
        /// if defining minor grid, then we pass the extent of the
        /// selected major grid and the extent of the selection box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMapSelectBoxFinal(object sender, _DMapEvents_SelectBoxFinalEvent e)
        {
            var extL = 0D;
            var extR = 0D;
            var extT = 0D;
            var extB = 0D;
            Extents selectionBoxExtent = new Extents();

            _axMap.PixelToProj(e.left, e.top, ref extL, ref extT);
            _axMap.PixelToProj(e.right, e.bottom, ref extR, ref extB);
            selectionBoxExtent.SetBounds(extL, extB, 0, extR, extT, 0);

            if (_inDefineMinorGrid)
            {
                //pass the extents of the selected major grids and the extent defined by the select box
                //if minor grid is defined successfully, we proceed with the next steps
                if (_grid25MinorGrid.DefineMinorGrids(_selectedShapesExtent, selectionBoxExtent))
                {
                    var minorGridExtent = _grid25MinorGrid.MinorGridLinesShapeFile.Extents;

                    //get the intersection of the selectionBox and the selected major grids
                    if (MajorGridsIntersectMinorGridExtent(minorGridExtent))
                    {
                        if (DefineBoundingRectangle(minorGridExtent))
                        {
                            //we add minorgrid row and column labels
                            _grid25LabelManager = new Grid25LabelManager(minorGridExtent, _listSidesToLabel,
                                                                         _axMap.GeoProjection, _gridAndLabelProperties);

                            _listGridLayers.Add(_mapLayers.AddLayer(_grid25MinorGrid.MinorGridLinesShapeFile, "Minor grid", true, true));
                            _listGridLayers.Add(_mapLayers.AddLayer(_grid25LabelManager.Grid25Labels, "Labels", true, true));
                            _listGridLayers.Add(_mapLayers.AddLayer(_shapefileMajorGridIntersect, "Major grid", true, true));
                        }
                    }
                }
            }
            else
            {
                //pass the extent to SelectGrids function
                //FromSelectionBox is true because we dragged a selection box
                SelectGrids(selectionBoxExtent, SelectionFromSelectBox: true);
            }
        }

        /// <summary>
        /// define the bounding rectangle of the minor grids which is the bounding rectangle of the fishing ground map
        /// </summary>
        /// <param name="minorGridExtent"></param>
        private bool DefineBoundingRectangle(Extents minorGridExtent)
        {
            bool success = false;
            _shapefileBoundingRectangle = new Shapefile();
            if (_shapefileBoundingRectangle.CreateNewWithShapeID("", ShpfileType.SHP_POLYGON))
            {
                _shapefileBoundingRectangle.GeoProjection = _axMap.GeoProjection;
                _shapefileBoundingRectangle.EditAddField("Name", FieldType.STRING_FIELD, 1, 50);
                _shapefileBoundingRectangle.EditAddField("MapTitle", FieldType.STRING_FIELD, 1, 255);
                _shapefileBoundingRectangle.EditAddShape(minorGridExtent.ToShape());

                var shp = new Shape();
                if (shp.Create(ShpfileType.SHP_POLYLINE))
                {
                    var ptX = _shapefileBoundingRectangle.Shape[0].Point[0].x;
                    var ptY = _shapefileBoundingRectangle.Shape[0].Point[0].y;
                    shp.AddPoint(ptX, ptY);

                    ptX = _shapefileBoundingRectangle.Shape[0].Point[1].x;
                    ptY = _shapefileBoundingRectangle.Shape[0].Point[1].y;
                    shp.AddPoint(ptX, ptY);

                    ptX = _shapefileBoundingRectangle.Shape[0].Point[2].x;
                    ptY = _shapefileBoundingRectangle.Shape[0].Point[2].y;
                    shp.AddPoint(ptX, ptY);

                    ptX = _shapefileBoundingRectangle.Shape[0].Point[3].x;
                    ptY = _shapefileBoundingRectangle.Shape[0].Point[3].y;
                    shp.AddPoint(ptX, ptY);

                    ptX = _shapefileBoundingRectangle.Shape[0].Point[4].x;
                    ptY = _shapefileBoundingRectangle.Shape[0].Point[4].y;
                    shp.AddPoint(ptX, ptY);

                    if (_grid25MinorGrid.MinorGridLinesShapeFile.EditInsertShape(shp, 0))
                    {
                        var ifld = _grid25MinorGrid.MinorGridLinesShapeFile.FieldIndexByName["LineType"];
                        _grid25MinorGrid.MinorGridLinesShapeFile.EditCellValue(ifld, 0, "BN");
                        success = true;
                    }
                }
            }
            return success;
        }

        /// <summary>
        /// gets the intersections of each major grid to the selection box extent
        /// </summary>
        /// <param name="minorGridExtent"></param>
        private bool MajorGridsIntersectMinorGridExtent(Extents minorGridExtent)
        {
            var success = false;
            _shapefileMajorGridIntersect = new Shapefile();
            if (_shapefileMajorGridIntersect.CreateNewWithShapeID("", ShpfileType.SHP_POLYGON))
            {
                _shapefileMajorGridIntersect.GeoProjection = _axMap.GeoProjection;
                var ifldMGNo = _shapefileMajorGridIntersect.EditAddField("GridNo", FieldType.STRING_FIELD, 1, 4);
                var ifldGridHandle = _shapefileMajorGridIntersect.EditAddField("hGrid", FieldType.STRING_FIELD, 1, 4);
                foreach (var item in _listSelectedShapeGridNumbers)
                {
                    var intersectionResults = new Object();
                    if (_shapefileMajorGrid.Shape[item].Extents.ToShape().GetIntersection(minorGridExtent.ToShape(), ref intersectionResults))
                    {
                        var gridNo = _shapefileMajorGrid.CellValue[_shapefileMajorGrid.FieldIndexByName["grid_no"], item];
                        object[] shapeArray = intersectionResults as object[];
                        if (shapeArray != null)
                        {
                            Shape[] shapes = shapeArray.OfType<Shape>().ToArray();
                            for (int n = 0; n < shapes.Length; n++)
                            {
                                var iShp = _shapefileMajorGridIntersect.EditAddShape(shapes[n]);
                                _shapefileMajorGridIntersect.EditCellValue(ifldMGNo, iShp, gridNo);
                                _shapefileMajorGridIntersect.EditCellValue(ifldGridHandle, iShp, item);
                            }
                        }
                        success = true;
                    }
                }
            }
            _shapefileMajorGridIntersect.DefaultDrawingOptions.FillVisible = false;
            _shapefileMajorGridIntersect.DefaultDrawingOptions.LineColor = new Utils().ColorByName(tkMapColor.Red);
            _shapefileMajorGridIntersect.DefaultDrawingOptions.LineWidth = 2;

            return success;
        }

        /// <summary>
        /// selects major grids that intersects the extent
        /// selected grids are categorized and colored light red.
        ///
        /// this function is only called if _inDefineMinorGrid=false
        /// </summary>
        /// <param name="ext"></param>
        /// <param name="SelectionFromSelectBox"></param>
        private void SelectGrids(Extents ext, bool SelectionFromSelectBox = false)
        {
            var ifldToGrid = _shapefileMajorGrid.FieldIndexByName["toGrid"];
            _selectionFromSelectBox = SelectionFromSelectBox;

            object selectionResult = new object();
            if (_shapefileMajorGrid.SelectShapes(ext, 0, SelectMode.INTERSECTION, ref selectionResult))
            {
                _selectedShapeIndexes = (int[])selectionResult;

                if (_selectedShapeIndexes.Length > 0)
                {
                    //if the selection was made by drag-selecting on the grid
                    if (SelectionFromSelectBox)
                    {
                        //clears the previously selected, red colored grids
                        if (_listSelectedShapeGridNumbers.Count > 0)
                        {
                            foreach (var item in _listSelectedShapeGridNumbers)
                                _shapefileMajorGrid.EditCellValue(ifldToGrid, item, "");
                        }
                        _listSelectedShapeGridNumbers.Clear();

                        //adds selected shapes to a List<int> and sets toGrid column to "T"
                        for (int n = 0; n < _selectedShapeIndexes.Length; n++)
                        {
                            _shapefileMajorGrid.EditCellValue(ifldToGrid, _selectedShapeIndexes[n], "T");
                            _listSelectedShapeGridNumbers.Add(_selectedShapeIndexes[n]);
                        }

                        //categorizes grid and makes those with "T" in toGrid column  have a light red fill color
                        _shapefileMajorGrid.Categories.ApplyExpression(0);
                    }

                    //if the selection was made by clicking on a grid
                    else
                    {
                        //sets the color of the selected grid
                        Grid25Grid.SelectNone();
                        _shapefileMajorGrid.ShapeSelected[_selectedShapeIndexes[0]] = true;
                        _shapefileMajorGrid.SelectionAppearance = tkSelectionAppearance.saDrawingOptions;
                        _shapefileMajorGrid.SelectionDrawingOptions.FillColor = new Utils().ColorByName(tkMapColor.Yellow);
                        _shapefileMajorGrid.SelectionDrawingOptions.FillTransparency = 75;
                        _shapefileMajorGrid.SelectionDrawingOptions.LineColor = new Utils().ColorByName(tkMapColor.Red);
                        _shapefileMajorGrid.SelectionDrawingOptions.LineWidth = 2;
                    }
                    _axMap.Redraw();
                }
            }
        }

        /// <summary>
        /// processes a mouse-click selection, contrasted with a mouse-drag selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                _shapefileMajorGrid = sf;
                ConfigureGridAppearance();
            }
        }

        /// <summary>
        /// set the appearance of selected and unselected grids
        /// </summary>
        private void ConfigureGridAppearance()
        {
            _shapefileMajorGrid.With(sf =>
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
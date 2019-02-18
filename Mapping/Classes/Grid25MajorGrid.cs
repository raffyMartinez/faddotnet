using AxMapWinGIS;
using FAD3.Database.Classes;
using FAD3.Mapping.Classes;
using MapWinGIS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace FAD3
{
    /// <summary>
    /// 1. creates major grids.
    /// 2. manages selection, deselection and coloring of major grids
    /// 3. makes available a List<int> of selected grids
    /// </summary>
    public class Grid25MajorGrid : IDisposable
    {
        private const int CURSORWIDTH = 5;
        private const int GRIDSIZE = 50000;

        private static fadUTMZone _utmZone =                                    //holds the utm zone of the grid
                                fadUTMZone.utmZone_Undefined;
        private AxMap _axMap;                                                   //reference to the map control found in MapForm
        private bool _disposed = false;
        private tkWgs84Projection _Grid25Geoprojection;                         //reference to the map control projection
        private Grid25LabelManager _grid25LabelManager;                         //helper class for managing labels
        private Grid25MinorGrid _grid25MinorGrid;                               //helper class for managing minor grids
        private Dictionary<string, uint> _gridAndLabelProperties;               //holds the various properties of grid labels and lines
        private bool _inDefineMinorGrid;                                        //if we are in the part of creating the minor grid

        private bool _selectionFromSelectBox = false;                           //if selection was made by dragging a select box or by clicking
        private List<int> _listGridLayers = new List<int>();                    //list of layer handles of shapefiles that make up a fishing grid
        private List<int> _listSelectedShapeGridNumbers = new List<int>();      //list major grid numbers

        private List<(int GridNo, double x, double y)>
            _listIntersectedMajorGrids = new List<(int, double, double)>();     //list major grids that become part of the grid map

        private Grid25LayoutHelper _layoutHelper;
        private MapLayersHandler _mapLayers;                                    //reference to the map layers class
        private MapInterActionHandler _mapInterActionHandler;                   //reference to the MapInterActionHandler class
        private int[] _selectedShapeIndexes;                                    //holds the indexes of selected shapes
        private Extents _selectedMajorGridShapesExtent = new Extents();         //extent of the selected major grid

        private Shapefile _shapefileBoundingRectangle;                          //shapefile of the MBR of the fishing grid
        private Shapefile _shapefileMajorGrid;                                  //shapefile of the entire grid25 major grids
        private Shapefile _shapefileMajorGridIntersect;                         //shapefile of major grid intersected with extent of minor grids
        private Shapefile _shapeFileSelectedMajorGridBuffer;                    //a shapefile that holds a convex hull of the selected major grids
        private int _hCursorDefineGrid;                                         //the handle of the cursor used when defining selection extent of major grid

        private int _hCursorDefineLayout;
        private string _mapTitle;                                               //title of the fishing ground grid map
        private MapLayer _currentMapLayer;
        private bool _enableMapInteraction;                                     //allows the class to interact with the axMap map control
        private bool _loadGridInPanel;
        private string _folderToSave;
        private bool _inDefineGridFromLayout;

        public delegate void FishingGridLayerSavedHandler(Grid25MajorGrid s, LayerEventArg e);              //event raised when a layer is selected from the list found in the layers form
        public event FishingGridLayerSavedHandler LayerSaved;

        public delegate void FishingGridRetrievedHandler(Grid25MajorGrid s, LayerEventArg e);              //event raised when a layer is selected from the list found in the layers form
        public event FishingGridRetrievedHandler GridRetrieved;

        public delegate void MapExtentCreated(Grid25MajorGrid s, ExtentDraggedBoxEventArgs e);                          //event raised when an extent is created
        public event MapExtentCreated ExtentCreatedInLayer;

        //public delegate void GridLayoutOriginDefined(Grid25MajorGrid s, ExtentDraggedBoxEventArgs e);
        //public event GridLayoutOriginDefined LayoutDefined;

        public delegate void GridInPanelCreated(Grid25MajorGrid s, LayerEventArg e);
        public event GridInPanelCreated OnGridInPanelCreated;

        public int LayoutRows { get; internal set; }
        public int LayoutCols { get; internal set; }
        public int LayoutOverlap { get; internal set; }

        private static List<string> _filesToDeleteOnClose = new List<string>();

        public bool InDefindeGridFromLayout
        {
            get { return _inDefineGridFromLayout; }
        }

        public string FolderToSave
        {
            get { return _folderToSave; }
            set { _folderToSave = value; }
        }

        public Grid25LayoutHelper LayoutHelper
        {
            get { return _layoutHelper; }
        }

        public AxMap MapControl
        {
            get { return _axMap; }
        }

        public Extents SelectedMajorGridShapesExtent
        {
            get { return _selectedMajorGridShapesExtent; }
        }

        public MapInterActionHandler MapInterActionHandler
        {
            get { return _mapInterActionHandler; }
            set
            {
                _mapInterActionHandler = value;
                _mapInterActionHandler.GridSelected += OnFishingGridSelected;
            }
        }

        public static List<string> FilesToDeleteOnClose
        {
            get { return _filesToDeleteOnClose; }
        }

        /// <summary>
        /// Apply symbology (colors, widths, fills) to various elements of the grid
        /// </summary>
        /// <param name="mapName"></param>
        public void ApplyGridSymbology(string mapName = "")
        {
            _grid25MinorGrid.MinorGridLinesShapeFile.DefaultDrawingOptions.LineColor = _gridAndLabelProperties["minorGridLineColor"];
            _grid25MinorGrid.MinorGridLinesShapeFile.DefaultDrawingOptions.LineWidth = (float)_gridAndLabelProperties["minorGridThickness"] / 100;

            _shapefileMajorGridIntersect.DefaultDrawingOptions.FillVisible = false;
            _shapefileMajorGridIntersect.DefaultDrawingOptions.LineColor = _gridAndLabelProperties["majorGridLineColor"];
            _shapefileMajorGridIntersect.DefaultDrawingOptions.LineWidth = (float)_gridAndLabelProperties["majorGridThickness"] / 100;

            _shapefileBoundingRectangle.DefaultDrawingOptions.LineWidth = (float)_gridAndLabelProperties["borderThickness"] / 100;
            _shapefileBoundingRectangle.DefaultDrawingOptions.LineColor = _gridAndLabelProperties["borderColor"];
            _shapefileBoundingRectangle.DefaultDrawingOptions.FillVisible = false;
            if (mapName.Length > 0)
            {
                _shapefileBoundingRectangle.EditCellValue(_shapefileBoundingRectangle.FieldIndexByName["MapTitle"], 0, mapName);
            }
        }

        private void AssignLayerWeight(MapLayer layer)
        {
            switch (layer.Name)
            {
                case "Minor grid":
                    layer.LayerWeight = 4;
                    break;

                case "Labels":
                    layer.LayerWeight = 2;
                    break;

                case "Major grid":
                    layer.LayerWeight = 3;
                    break;

                case "MBR":
                    layer.LayerWeight = 1;
                    break;
            }
        }

        public void LoadPanelGrid(bool autoExpand, LayerEventArg e)
        {
            _loadGridInPanel = true;
            OnFishingGridSelected(null, e);

            if (e.Action == "LoadGridMap" && autoExpand)
                FitGridToMap();
        }

        /// <summary>
        /// Load the fishing map whose booundary was selected in the map control
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void OnFishingGridSelected(MapInterActionHandler s, LayerEventArg e)
        {
            string fgFileName = "";
            Shapefile sf = new Shapefile();
            if (_loadGridInPanel)
            {
                fgFileName = $@"{_folderToSave}\{e.FileName}";
                _mapTitle = e.LayerName;
            }
            else
            {
                //in this context, CurrentMapLayer contains the grid boundaries of fishing ground maps
                sf = ((Shapefile)_mapLayers.CurrentMapLayer.LayerObject);

                //let us get the base filename of the selected fishing ground boundary
                fgFileName = sf.CellValue[sf.FieldIndexByName["BaseName"], e.SelectedIndex].ToString();

                //let us get the map title of the selected fishing ground
                _mapTitle = (string)sf.CellValue[sf.FieldIndexByName["MapName"], e.SelectedIndex];
            }

            if (e.Action == "LoadGridMap")
            {
                int h = 0;

                //load the minor grid shapefile
                if (_grid25MinorGrid.LoadMinorGridShapefile($"{fgFileName}_gridlines.shp"))
                {
                    //load the major grids of the fishing ground
                    _shapefileMajorGridIntersect = new Shapefile();
                    if (_shapefileMajorGridIntersect.Open($"{fgFileName}_majorgrid.shp"))
                    {
                        //populate the list with the shape indexes of the major grids
                        _listSelectedShapeGridNumbers.Clear();
                        for (int n = 0; n < _shapefileMajorGridIntersect.NumShapes; n++)
                        {
                            string shpIndex = (string)_shapefileMajorGridIntersect.CellValue[_shapefileMajorGridIntersect.FieldIndexByName["hGrid"], n];
                            if (shpIndex.Length > 0)
                            {
                                _listSelectedShapeGridNumbers.Add(int.Parse(shpIndex));
                                //_listSelectedShapeGridNumbers.Add(shpIndex);
                            }
                        }

                        //get the intersection of the minorGridExtent and the selected major grids
                        if (MajorGridsIntersectMinorGridExtent(_grid25MinorGrid.MinorGridLinesShapeFile.Extents))
                        {
                            _shapeFileSelectedMajorGridBuffer = _shapefileMajorGridIntersect.BufferByDistance(0, 1, false, true);
                            _shapeFileSelectedMajorGridBuffer.GeoProjection = _axMap.GeoProjection;
                        }

                        //using Grid25LabelManager, create labels that follow the path defined by  _shapeFileSelectedMajorGridBuffer
                        _grid25LabelManager = new Grid25LabelManager(_axMap.GeoProjection);
                        if (_grid25LabelManager.LabelGrid(_shapeFileSelectedMajorGridBuffer, _gridAndLabelProperties, _mapTitle))
                        {
                            //add label shapefile to the map
                            _grid25LabelManager.AddMajorGridLabels(_listIntersectedMajorGrids);
                            _grid25LabelManager.Grid25Labels.Labels.ApplyCategories();
                            h = _mapLayers.AddLayer(_grid25LabelManager.Grid25Labels, "Labels", true, true);
                            _mapLayers.LayerDictionary[h].IsFishingGrid = true;
                            AssignLayerWeight(_mapLayers.LayerDictionary[h]);
                            _listGridLayers.Add(h);

                            //add minor grid shapefile to the map
                            h = _mapLayers.AddLayer(_grid25MinorGrid.MinorGridLinesShapeFile, "Minor grid", true, true);
                            _mapLayers.LayerDictionary[h].IsGraticule = true;
                            _mapLayers.LayerDictionary[h].IsFishingGrid = true;
                            AssignLayerWeight(_mapLayers.LayerDictionary[h]);
                            _listGridLayers.Add(h);

                            //add major grids
                            h = _mapLayers.AddLayer(_shapefileMajorGridIntersect, "Major grid", true, true);
                            _mapLayers.LayerDictionary[h].IsGraticule = true;
                            _mapLayers.LayerDictionary[h].IsFishingGrid = true;
                            AssignLayerWeight(_mapLayers.LayerDictionary[h]);
                            _listGridLayers.Add(h);

                            //add boundary
                            _shapefileBoundingRectangle = new Shapefile();
                            _shapefileBoundingRectangle.Open($"{fgFileName}_gridboundary.shp");
                            h = _mapLayers.AddLayer(_shapefileBoundingRectangle, "MBR", true, true);
                            _mapLayers.LayerDictionary[h].IsGraticule = true;
                            _mapLayers.LayerDictionary[h].IsFishingGrid = true;
                            AssignLayerWeight(_mapLayers.LayerDictionary[h]);
                            _listGridLayers.Add(h);

                            ApplyGridSymbology();

                            //raise event declaring that a fishing grid was retrieved from file
                            if (GridRetrieved != null)
                            {
                                var mapName = _shapefileBoundingRectangle.CellValue[_shapefileBoundingRectangle.FieldIndexByName["MapTitle"], 0];
                                LayerEventArg lp = new LayerEventArg(h, (string)mapName);
                                GridRetrieved(this, lp);
                            }
                        }
                    }
                }
            }
            else if (e.Action == "DeleteGridMap")
            {
                DeleteGrid25File($"{fgFileName}_gridlines");

                DeleteGrid25File($"{fgFileName}_gridlabels");

                DeleteGrid25File($"{fgFileName}_majorgrid");

                if (File.Exists($"{fgFileName}_gridstate.xml")) File.Delete($"{fgFileName}_gridstate.xml");

                sf.EditDeleteShape(e.SelectedIndex);
                sf.Labels.RemoveLabel(e.SelectedIndex);
                _axMap.Redraw();

                _filesToDeleteOnClose.Add($"{fgFileName}_gridboundary");
            }
            else if (e.Action == "UnloadGridMap")
            {
                _mapLayers.RemoveLayer("Labels");
                _mapLayers.RemoveLayer("MBR");
                _mapLayers.RemoveLayer("Major grid");
                _mapLayers.RemoveLayer("Minor grid");
            }
        }

        /// <summary>
        /// Constructor and sets default behaviour and WGS UTM projection of map control
        /// </summary>
        /// <param name="mapControl"></param>
        /// <param name="utmZone"></param>
        public Grid25MajorGrid(AxMap mapControl)
        {
            _axMap = mapControl;

            _axMap.SendMouseUp = true;
            _axMap.SendMouseDown = true;
            _axMap.SendSelectBoxFinal = true;
            _axMap.SendSelectBoxDrag = true;

            _axMap.MouseUpEvent += OnMapMouseUp;
            _axMap.MouseDownEvent += OnMapMouseDown;
            _axMap.SelectBoxFinal += OnMapSelectBoxFinal;
            _axMap.SelectBoxDrag += OnSelectBoxDrag;

            _axMap.DblClick += OnMapDoubleClick;
            _axMap.CursorMode = tkCursorMode.cmSelection;
            _utmZone = fadUTMZone.utmZone51N;

            _grid25MinorGrid = new Grid25MinorGrid(_axMap, this);
            ShapefileDiskStorageHelper.MapControl = _axMap;

            LayoutRows = 1;
            LayoutCols = 1;
            LayoutOverlap = 0;
        }

        private void OnSelectBoxDrag(object sender, _DMapEvents_SelectBoxDragEvent e)
        {
            double pLeft = 0;
            double pRight = 0;
            double pTop = 0;
            double pBottom = 0;
            _axMap.PixelToProj(e.left, e.top, ref pLeft, ref pTop);
            _axMap.PixelToProj(e.right, e.bottom, ref pRight, ref pBottom);
            if (ExtentCreatedInLayer != null)
            {
                ExtentDraggedBoxEventArgs arg = new ExtentDraggedBoxEventArgs(pTop, pBottom, pLeft, pRight, true);
                ExtentCreatedInLayer(this, arg);
            }
        }

        /// <summary>
        /// save the fishing grid map to an image
        /// </summary>
        /// <param name="DPI"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool Save(double DPI, string fileName)
        {
            SaveMapImage smi = new SaveMapImage(fileName, DPI, _axMap);
            smi.MapLayersHandler = _mapLayers;
            return smi.Save();
        }

        /// <summary>
        /// saves grid 25 shapefiles
        /// </summary>
        /// <param name="baseFileName">
        /// This is the name we provided in a dialog box
        /// </param>
        /// <returns></returns>
        public bool Save(string baseFileName)
        {
            var saveCount = 0;
            var fileName = "";

            foreach (MapLayer ml in _mapLayers.LayerDictionary.Values)
            {
                //select only those shapefiles that are part of the fishing grid
                if (ml.IsFishingGrid)
                {
                    switch (ml.Name)
                    {
                        case "Minor grid":
                            fileName = $"{baseFileName}_gridlines";
                            break;

                        case "Labels":
                            fileName = $"{baseFileName}_gridlabels";
                            break;

                        case "Major grid":
                            fileName = $"{baseFileName}_majorgrid";
                            break;

                        case "MBR":
                            fileName = $"{baseFileName}_gridboundary";
                            var sf = _axMap.get_Shapefile(ml.Handle);
                            sf.EditCellValue(sf.FieldIndexByName["Name"], 0, Path.GetFileName(baseFileName));
                            break;
                    }

                    if (ShapefileDiskStorageHelper.Delete(fileName))
                    {
                        var result = ml.Save(fileName);
                        if (result) saveCount++;

                        if (LayerSaved != null)
                        {
                            LayerEventArg lp = new LayerEventArg(ml.Handle, result, fileName);
                            LayerSaved(this, lp);
                        }
                    }
                }
            }
            if (saveCount == 4)
            {
                //save metadata file for the fishing grid
                Serialize(baseFileName);
            }
            return saveCount == 4;
        }

        private void Serialize(string fileName)
        {
            XmlWriter writer = XmlWriter.Create($"{fileName}_gridstate.xml");
            writer.WriteStartDocument();

            writer.WriteStartElement("FishingGroundGridMap");
            writer.WriteAttributeString("MapTitle", _mapTitle);
            writer.WriteAttributeString("Name", fileName);
            writer.WriteAttributeString("UTMZone", _utmZone.ToString());
            foreach (KeyValuePair<string, uint> kv in _gridAndLabelProperties)
            {
                writer.WriteAttributeString(kv.Key, kv.Value.ToString());
            }

            {
                if (_inDefineGridFromLayout)
                {
                    writer.WriteStartElement("Layout");
                    writer.WriteAttributeString("FishingGround", _layoutHelper.FishingGround);
                    writer.WriteAttributeString("LowerLeftCornerXY", $"{_layoutHelper.LayoutExtents.xMin.ToString()},{_layoutHelper.LayoutExtents.yMin.ToString()}");
                    writer.WriteAttributeString("UpperRightCornerXY", $"{_layoutHelper.LayoutExtents.xMax.ToString()},{_layoutHelper.LayoutExtents.yMax.ToString()}");
                    writer.WriteAttributeString("Rows", _layoutHelper.Rows.ToString());
                    writer.WriteAttributeString("Columns", _layoutHelper.Columns.ToString());
                    writer.WriteAttributeString("Overlap", _layoutHelper.Overlap.ToString());

                    {
                        writer.WriteStartElement("Cells");
                        int fldTitle = _layoutHelper.LayoutShapeFile.FieldIndexByName["Title"];
                        for (int n = 0; n < _layoutHelper.LayoutShapeFile.NumShapes; n++)
                        {
                            writer.WriteStartElement("Cell");
                            writer.WriteAttributeString("Number", n.ToString());
                            writer.WriteAttributeString("Title", _layoutHelper.LayoutShapeFile.CellValue[fldTitle, n].ToString());
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                }
            }
            writer.WriteEndElement();

            writer.WriteEndDocument();
            writer.Close();
        }

        /// <summary>
        /// loads fishing grid boundary in a utm zone to the map control
        /// </summary>
        /// <param name="startFolderPath"></param>
        /// <param name="utmZone"></param>
        public void ShowGridBoundaries(string startFolderPath, fadUTMZone utmZone, Dictionary<string, uint> gridAndLabelProperties)
        {
            _gridAndLabelProperties = gridAndLabelProperties;
            var selectedZone = fadUTMZone.utmZone_Undefined;
            var mapBoundaries = new List<string>();
            var results = Directory.GetFiles(startFolderPath, "*_gridboundary.shp", SearchOption.AllDirectories);
            for (int n = 0; n < results.Length; n++)
            {
                var prjFile = $@"{Path.GetDirectoryName(results[n])}\{Path.GetFileNameWithoutExtension(results[n])}.prj";
                using (StreamReader sr = File.OpenText(prjFile))
                {
                    string s = String.Empty;
                    while ((s = sr.ReadLine()) != null)
                    {
                        switch (s.Substring(8, 21))
                        {
                            case "WGS_1984_UTM_Zone_51N":
                                selectedZone = fadUTMZone.utmZone51N;
                                break;

                            case "WGS_1984_UTM_Zone_50N":
                                selectedZone = fadUTMZone.utmZone50N;
                                break;

                            default:
                                switch (s.Substring(17, 12))
                                {
                                    case "UTM zone 51N":
                                        selectedZone = fadUTMZone.utmZone51N;
                                        break;

                                    case "UTM zone 50N":
                                        selectedZone = fadUTMZone.utmZone50N;
                                        break;
                                }
                                break;
                        }
                    }
                }

                if (selectedZone == utmZone)
                {
                    if (File.Exists(results[n].Replace("_gridboundary.shp", "_gridlines.shp")))
                    {
                        mapBoundaries.Add(results[n]);
                    }
                    else
                    {
                        DeleteGrid25File(results[n].Replace(".shp", ""));
                    }
                }
            }

            LoadBoundaryFile(mapBoundaries);
        }

        /// <summary>
        /// helper function to load fishing grid boundaries to the map control
        /// </summary>
        /// <param name="boundaries"></param>
        private void LoadBoundaryFile(List<string> boundaries)
        {
            var sf = new Shapefile();
            if (sf.CreateNew("", ShpfileType.SHP_POLYGON))
            {
                var ifldName = sf.EditAddField("MapName", FieldType.STRING_FIELD, 0, 100);
                var ifldBaseFilename = sf.EditAddField("BaseName", FieldType.STRING_FIELD, 0, 255);
                var n = 0;
                foreach (var item in boundaries)
                {
                    var sfBoundary = new Shapefile();
                    if (sfBoundary.Open(item, null) && sf.EditAddShape(sfBoundary.Extents.ToShape()) >= 0)
                    {
                        var mapTitle = (string)sfBoundary.CellValue[sfBoundary.FieldIndexByName["MapTitle"], 0];
                        sf.EditCellValue(ifldName, n, mapTitle);
                        var baseName = $@"{Path.GetDirectoryName(item)}\{(string)sfBoundary.CellValue[sfBoundary.FieldIndexByName["Name"], 0]}";
                        sf.EditCellValue(ifldBaseFilename, n, baseName);
                        sf.Labels.AddLabel(mapTitle, sfBoundary.Extents.Center.x, sfBoundary.Extents.Center.y);
                    }
                    n++;
                }

                //set options for appearance of selected shapes
                sf.SelectionAppearance = tkSelectionAppearance.saDrawingOptions;
                sf.SelectionDrawingOptions.FillTransparency = 100;
                sf.SelectionColor = new Utils().ColorByName(tkMapColor.Yellow);

                //set options for appearance of unselected shapes
                sf.DefaultDrawingOptions.FillVisible = false;
                sf.DefaultDrawingOptions.LineColor = new Utils().ColorByName(tkMapColor.Blue);
                sf.DefaultDrawingOptions.LineWidth = 2;
                sf.Labels.AvoidCollisions = false;

                _mapLayers.AddLayer(sf, "Fishing grid boundaries", true, true);
                sf.StopEditingShapes();
            }
        }

        public static void DeleteGrid25File(string fileName)
        {
            if (File.Exists($"{fileName}.shp")) File.Delete($"{fileName}.shp");
            if (File.Exists($"{fileName}.prj")) File.Delete($"{fileName}.prj");
            if (File.Exists($"{fileName}.dbf")) File.Delete($"{fileName}.dbf");
            if (File.Exists($"{fileName}.shx")) File.Delete($"{fileName}.shx");
        }

        /// <summary>
        /// fit extents of fishing grid to extent of map control
        /// </summary>
        public void FitGridToMap()
        {
            if (_shapefileBoundingRectangle != null)
            {
                int labelDistance = (int)_gridAndLabelProperties["minorGridLabelDistance"];
                const int plusYTop = 4000;
                const int plusYBottom = 3000;
                const int plusX = 1500;

                _shapefileBoundingRectangle.Extents.With(e =>
                {
                    var ext = new Extents();
                    ext.SetBounds(e.xMin - (labelDistance * 3) - plusX, e.yMin - (labelDistance * 3) - plusYBottom, 0, e.xMax + (labelDistance * 3) + plusX, e.yMax + (labelDistance * 3) + plusYTop, 0);
                    _axMap.Extents = ext;
                });
            }
        }

        /// <summary>
        /// Returns the projection of the map control
        /// </summary>
        public tkWgs84Projection Grid25Geoprojection
        {
            get { return _Grid25Geoprojection; }
        }

        /// <summary>
        /// Sets the map title
        /// </summary>
        public string MapTitle
        {
            get { return _mapTitle; }
            set { _mapTitle = value; }
        }

        /// <summary>
        /// returns the major grid shapefile
        /// </summary>
        public Shapefile Grid25Grid
        {
            get { return _shapefileMajorGrid; }
        }

        /// <summary>
        /// returns the grid label manager class
        /// </summary>
        public Grid25LabelManager grid25LabelManager
        {
            get { return _grid25LabelManager; }
        }

        /// <summary>
        /// Dictionary for holding properties of labels and grid lines
        /// </summary>
        public Dictionary<string, uint> LabelAndGridProperties
        {
            get { return _gridAndLabelProperties; }
            set { _gridAndLabelProperties = value; }
        }

        /// <summary>
        /// References the collection of map layers
        /// </summary>
        public MapLayersHandler MapLayers
        {
            get { return _mapLayers; }
            set
            {
                _mapLayers = value;
                _mapLayers.CurrentLayer += OnCurrentLayer;
            }
        }

        public bool EnableMapInteraction

        {
            get { return _enableMapInteraction; }
            set
            {
                _enableMapInteraction = value;
            }
        }

        /// <summary>
        /// Sets the current layer which is the layer selected in the mapLayers list
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void OnCurrentLayer(MapLayersHandler s, LayerEventArg e)
        {
            _enableMapInteraction = false;
            _currentMapLayer = _mapLayers.get_MapLayer(e.LayerHandle);

            //let the current class handle map interaction if the current layer is Grid25
            _enableMapInteraction = _currentMapLayer.Name == "Grid25";

            //let MapInteractionHandler handle map interaction if property EnableMapInteraction is false
            if (_mapInterActionHandler != null)
            {
                _mapInterActionHandler.EnableMapInteraction = !EnableMapInteraction;
            }
        }

        public void MoveToTop()
        {
            _axMap.MoveLayerTop(0);
        }

        /// <summary>
        /// Returns the minor grid class
        /// </summary>
        public Grid25MinorGrid MinorGrids
        {
            get { return _grid25MinorGrid; }
        }

        /// <summary>
        /// makes available List<int> containing shape index of selected grids
        /// </summary>
        public List<int> SelectedShapeGridNumbers
        {
            get { return _listSelectedShapeGridNumbers; }
        }

        /// <summary>
        /// setsup the UTM zone of the fishing grid
        /// </summary>
        public fadUTMZone UTMZone
        {
            get { return _utmZone; }
            set
            {
                _utmZone = value;
                switch (_utmZone)
                {
                    case fadUTMZone.utmZone50N:
                        _Grid25Geoprojection = tkWgs84Projection.Wgs84_UTM_zone_50N;
                        break;

                    case fadUTMZone.utmZone51N:
                        _Grid25Geoprojection = tkWgs84Projection.Wgs84_UTM_zone_51N;
                        break;
                }
                FishingGrid.UTMZone = _utmZone;
                //GenerateMajorGrids();
            }
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
            _axMap.Redraw();
        }

        public void ReleaseLayoutHelper()
        {
            if (_layoutHelper != null)
            {
                _layoutHelper.ClearLayout();
                LayoutCols = _layoutHelper.Columns;
                LayoutRows = _layoutHelper.Rows;
                LayoutOverlap = _layoutHelper.Overlap;

                _layoutHelper.Dispose();
                _layoutHelper = null;
                _enableMapInteraction = true;
                _axMap.Redraw();
            }
        }

        public void DefineGridLayout()
        {
            DefineGridLayout(-1);
        }

        public void DefineGridLayout(int iconHandle)
        {
            if (_layoutHelper == null)
            {
                _inDefineMinorGrid = false;
                _enableMapInteraction = false;
                if (iconHandle >= 0)
                {
                    _hCursorDefineLayout = iconHandle;
                    _axMap.CursorMode = tkCursorMode.cmSelection;
                    _axMap.MapCursor = tkCursor.crsrUserDefined;
                    _axMap.UDCursorHandle = _hCursorDefineLayout;
                    _layoutHelper = new Grid25LayoutHelper(this, iconHandle);
                }
                else
                {
                    _layoutHelper = new Grid25LayoutHelper(this);
                }
            }
        }

        /// <summary>
        /// Returns true if there is a major grid selection.
        /// This will calculate extent of the selected major grids
        /// </summary>
        /// <param name="iconHandle - the int handle of icon"></param>
        /// <returns></returns>
        public bool DefineMinorGrid(int iconHandle)
        {
            _hCursorDefineGrid = iconHandle;
            _inDefineMinorGrid = _listSelectedShapeGridNumbers.Count > 0;
            _inDefineGridFromLayout = false;
            if (_inDefineMinorGrid)
            {
                _shapefileMajorGrid.SelectNone();
                foreach (var item in _listSelectedShapeGridNumbers)
                {
                    _shapefileMajorGrid.ShapeSelected[item] = true;
                }
                _selectedMajorGridShapesExtent = _shapefileMajorGrid.BufferByDistance(0, 0, true, true).Extents;
                _shapefileMajorGrid.SelectNone();
                _axMap.CursorMode = tkCursorMode.cmSelection;
                _axMap.MapCursor = tkCursor.crsrUserDefined;
                _axMap.UDCursorHandle = _hCursorDefineGrid;
                _axMap.Redraw();
            }
            return _inDefineMinorGrid;
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
                    _listGridLayers.Clear();
                    _listIntersectedMajorGrids.Clear();
                    _listIntersectedMajorGrids = null;
                    _listGridLayers = null;
                    _listSelectedShapeGridNumbers = null;
                    _gridAndLabelProperties = null;
                }
                _grid25MinorGrid.Dispose();
                _grid25MinorGrid = null;
                if (_grid25LabelManager != null)
                {
                    _grid25LabelManager.Dispose();
                    _grid25LabelManager = null;
                }

                _selectedMajorGridShapesExtent = null;

                _shapefileMajorGrid.Close();
                _shapefileMajorGrid = null;

                if (_shapefileBoundingRectangle != null)
                {
                    _shapefileBoundingRectangle.Close();
                    _shapefileBoundingRectangle = null;
                }

                if (_shapefileMajorGridIntersect != null)
                {
                    _shapefileMajorGridIntersect.Close();
                    _shapefileMajorGridIntersect = null;
                }

                if (_shapeFileSelectedMajorGridBuffer != null)
                {
                    _shapeFileSelectedMajorGridBuffer.Close();
                    _shapeFileSelectedMajorGridBuffer = null;
                }

                _mapLayers = null;

                _axMap = null;
                _disposed = true;
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
                    var category = new ShapefileCategory
                    {
                        Name = "Selected grid",
                        Expression = @"[toGrid] =""T"""
                    };
                    category.DrawingOptions.FillColor = new Utils().ColorByName(tkMapColor.Red);
                    category.DrawingOptions.FillTransparency = 25;
                    category.DrawingOptions.LineWidth = 2;
                    category.DrawingOptions.LineColor = new Utils().ColorByName(tkMapColor.Red);
                    sf.Categories.Add2(category);
                }
            });
        }

        /// <summary>
        /// Defines the bounding rectangle of the minor grids which is the bounding rectangle of the fishing ground map
        /// </summary>
        /// <param name="minorGridExtent"></param>
        private bool DefineBoundingRectangle(Extents minorGridExtent)
        {
            bool success = false;
            _shapefileBoundingRectangle = new Shapefile();
            if (_shapefileBoundingRectangle.CreateNewWithShapeID("", ShpfileType.SHP_POLYGON))
            {
                _shapefileBoundingRectangle.GeoProjection = _axMap.GeoProjection;
                var ifldName = _shapefileBoundingRectangle.EditAddField("Name", FieldType.STRING_FIELD, 1, 50);
                var ifldTitle = _shapefileBoundingRectangle.EditAddField("MapTitle", FieldType.STRING_FIELD, 1, 255);
                _shapefileBoundingRectangle.EditAddShape(minorGridExtent.ToShape());

                _shapefileBoundingRectangle.EditCellValue(ifldTitle, 0, _mapTitle);

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
        /// creates major grids on a utm zone
        /// </summary>
        public void GenerateMajorGrids()
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
                                pt = new Point();
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

                            if (iShp >= 0)
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

        private bool MajorGridIntersectTemplatePanel(Extents panelExtent)
        {
            var ifldMGNo = 0;
            var ifldGridHandle = 0;
            if (_shapefileMajorGridIntersect.CreateNewWithShapeID("", ShpfileType.SHP_POLYGON))
            {
                _shapefileMajorGridIntersect.GeoProjection = _axMap.GeoProjection;
                ifldMGNo = _shapefileMajorGridIntersect.EditAddField("GridNo", FieldType.STRING_FIELD, 1, 4);
                ifldGridHandle = _shapefileMajorGridIntersect.EditAddField("hGrid", FieldType.STRING_FIELD, 1, 4);
            }
            return true;
        }

        /// <summary>
        /// Gets the intersections of each major grid to the minor grid extent
        /// and creates a shapefile of the intersected major grids.
        /// </summary>
        /// <param name="minorGridExtent"></param>
        private bool MajorGridsIntersectMinorGridExtent(Extents minorGridExtent)
        {
            _listIntersectedMajorGrids.Clear();
            var success = false;
            var ifldMGNo = 0;
            _shapefileMajorGridIntersect = new Shapefile();
            if (_shapefileMajorGridIntersect.CreateNewWithShapeID("", ShpfileType.SHP_POLYGON))
            {
                _shapefileMajorGridIntersect.GeoProjection = _axMap.GeoProjection;
                ifldMGNo = _shapefileMajorGridIntersect.EditAddField("GridNo", FieldType.STRING_FIELD, 1, 4);
                var ifldGridHandle = _shapefileMajorGridIntersect.EditAddField("hGrid", FieldType.STRING_FIELD, 1, 4);
                foreach (var item in _listSelectedShapeGridNumbers)
                {
                    var intersectionResults = new Object();
                    if (_shapefileMajorGrid.Shape[item].Extents.ToShape().GetIntersection(minorGridExtent.ToShape(), ref intersectionResults))
                    {
                        var gridNo = (int)_shapefileMajorGrid.CellValue[_shapefileMajorGrid.FieldIndexByName["grid_no"], item];
                        object[] shapeArray = intersectionResults as object[];
                        if (shapeArray != null)
                        {
                            Shape[] shapes = shapeArray.OfType<Shape>().ToArray();
                            for (int n = 0; n < shapes.Length; n++)
                            {
                                var iShp = _shapefileMajorGridIntersect.EditAddShape(shapes[n]);
                                if (iShp >= 0)
                                {
                                    _shapefileMajorGridIntersect.EditCellValue(ifldMGNo, iShp, gridNo);
                                    _shapefileMajorGridIntersect.EditCellValue(ifldGridHandle, iShp, item);
                                    _listIntersectedMajorGrids.Add((gridNo, (int)shapes[n].Centroid.x, shapes[n].Centroid.y));
                                }
                            }
                        }
                        success = true;
                    }
                }
            }

            return success;
        }

        /// <summary>
        /// Adds to selected major grids if it touches previously selected (red colored) grids or
        /// removes a selected grid if it is double-clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMapDoubleClick(object sender, EventArgs e)
        {
            if (!_inDefineMinorGrid && _axMap.CursorMode == tkCursorMode.cmSelection && EnableMapInteraction)
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
                _axMap.Redraw();
            }
        }

        /// <summary>
        /// Relabels the grid and allows changing of grid and label properties
        /// </summary>
        /// <param name="mapTitle"></param>
        /// <param name="gridAndLabelProperties"></param>
        public void RedoLabels(string mapTitle, Dictionary<string, uint> gridAndLabelProperties)
        {
            _mapTitle = mapTitle;
            _grid25LabelManager.LabelGrid(_shapeFileSelectedMajorGridBuffer, gridAndLabelProperties, _mapTitle);
            _grid25LabelManager.AddMajorGridLabels(_listIntersectedMajorGrids);
            _grid25LabelManager.Grid25Labels.Labels.ApplyCategories();
            _axMap.Redraw();
        }

        private void OnMapMouseDown(object sender, _DMapEvents_MouseDownEvent e)
        {
            if (_enableMapInteraction)
            {
                _selectionFromSelectBox = false;
            }
        }

        /// <summary>
        /// Processes a mouse-click selection, and not a mouse-drag selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMapMouseUp(object sender, _DMapEvents_MouseUpEvent e)
        {
            //we only proceed if a drag-select was not done
            if (_enableMapInteraction
                && !_selectionFromSelectBox
                && _axMap.CursorMode == tkCursorMode.cmSelection)
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

        public bool GenerateMinorGridInsidePanelExtent(Extents panelExtent, string mapTitle)
        {
            _listIntersectedMajorGrids.Clear();
            _inDefineGridFromLayout = true;
            var minorGridExtent = panelExtent;
            int ifldMGNo = 0;
            int ifldGridHandle = 0;
            if (mapTitle.Length > 0)
            {
                _mapTitle = mapTitle;
            }
            _shapefileMajorGridIntersect = new Shapefile();
            if (_shapefileMajorGridIntersect.CreateNewWithShapeID("", ShpfileType.SHP_POLYGON)
                && _grid25MinorGrid.SetExtent(panelExtent))
            {
                _shapefileMajorGridIntersect.GeoProjection = _axMap.GeoProjection;
                ifldMGNo = _shapefileMajorGridIntersect.EditAddField("GridNo", FieldType.STRING_FIELD, 1, 4);
                ifldGridHandle = _shapefileMajorGridIntersect.EditAddField("hGrid", FieldType.STRING_FIELD, 1, 4);

                var definitionSF = new Shapefile();
                if (definitionSF.CreateNew("", ShpfileType.SHP_POLYGON))
                {
                    definitionSF.GeoProjection = _axMap.GeoProjection;
                    var iShp = definitionSF.EditAddShape(_grid25MinorGrid.MinorGridLinesShapeFile.Extents.ToShape());
                    if (iShp >= 0)
                    {
                        object intersectionResults = new Object();

                        //get indexes of shapes in majorgrid that intersect with the panel extent
                        if (_shapefileMajorGrid.SelectByShapefile(definitionSF, tkSpatialRelation.srIntersects, false, ref intersectionResults)
                            && intersectionResults != null)
                        {
                            int[] indexes = intersectionResults as int[];

                            //using indexes, get a shape that is an intersection with  panel extents
                            for (int n = 0; n < indexes.Length; n++)
                            {
                                var shapeIntesected = new Object();
                                if (_shapefileMajorGrid.Shape[indexes[n]].Extents.ToShape().GetIntersection(_grid25MinorGrid.MinorGridLinesShapeFile.Extents.ToShape(), ref intersectionResults))
                                {
                                    object[] shapeArray = intersectionResults as object[];
                                    if (shapeArray != null)
                                    {
                                        Shape[] shapes = shapeArray.OfType<Shape>().ToArray();

                                        Extents intersectedExtent = shapes[0].Extents;
                                        if (intersectedExtent.Width > 0 && intersectedExtent.Height > 0)
                                        {
                                            var iShp2 = _shapefileMajorGridIntersect.EditAddShape(shapes[0]);
                                            _shapefileMajorGridIntersect.EditCellValue(ifldMGNo, iShp2, _shapefileMajorGrid.CellValue[1, indexes[n]]);
                                            _shapefileMajorGridIntersect.EditCellValue(ifldGridHandle, iShp2, indexes[n]);
                                            _listIntersectedMajorGrids.Add((int.Parse(_shapefileMajorGrid.CellValue[1, indexes[n]].ToString()), intersectedExtent.Center.x, intersectedExtent.Center.y));
                                            Console.WriteLine($"grid number:{_shapefileMajorGrid.CellValue[1, indexes[n]]}");
                                            Console.WriteLine($"width:{intersectedExtent.Width} height:{intersectedExtent.Height}");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (_shapefileMajorGridIntersect.NumShapes > 0 && DefineBoundingRectangle(_grid25MinorGrid.MinorGridLinesShapeFile.Extents))
            {
                _shapeFileSelectedMajorGridBuffer = _shapefileMajorGridIntersect.BufferByDistance(0, 1, false, true);
                _shapeFileSelectedMajorGridBuffer.GeoProjection = _axMap.GeoProjection;

                _grid25LabelManager = new Grid25LabelManager(_axMap.GeoProjection);

                if (_grid25LabelManager.LabelGrid(_shapeFileSelectedMajorGridBuffer, _gridAndLabelProperties, _mapTitle))
                {
                    _grid25LabelManager.AddMajorGridLabels(_listIntersectedMajorGrids);
                    _grid25LabelManager.Grid25Labels.Labels.ApplyCategories();

                    //we add the layers using the maplayer class, then we add the layer handles to listGridLayers
                    var h = _mapLayers.AddLayer(_grid25MinorGrid.MinorGridLinesShapeFile, "Minor grid", true, true);
                    _mapLayers.LayerDictionary[h].IsGraticule = true;
                    _mapLayers.LayerDictionary[h].IsFishingGrid = true;
                    //_mapLayers.LayerDictionary[h].LayerWeight = 2;
                    AssignLayerWeight(_mapLayers.LayerDictionary[h]);
                    _listGridLayers.Add(h);

                    h = _mapLayers.AddLayer(_grid25LabelManager.Grid25Labels, "Labels", true, true);
                    _mapLayers.LayerDictionary[h].IsFishingGrid = true;
                    //_mapLayers.LayerDictionary[h].LayerWeight = 1;
                    AssignLayerWeight(_mapLayers.LayerDictionary[h]);
                    _listGridLayers.Add(h);

                    h = _mapLayers.AddLayer(_shapefileMajorGridIntersect, "Major grid", true, true);
                    _mapLayers.LayerDictionary[h].IsGraticule = true;
                    _mapLayers.LayerDictionary[h].IsFishingGrid = true;
                    //_mapLayers.LayerDictionary[h].LayerWeight = 4;
                    AssignLayerWeight(_mapLayers.LayerDictionary[h]);
                    _listGridLayers.Add(h);

                    h = _mapLayers.AddLayer(_shapefileBoundingRectangle, "MBR", true, true);
                    _mapLayers.LayerDictionary[h].IsGraticule = true;
                    _mapLayers.LayerDictionary[h].IsFishingGrid = true;
                    //_mapLayers.LayerDictionary[h].LayerWeight = 3;
                    AssignLayerWeight(_mapLayers.LayerDictionary[h]);
                    _listGridLayers.Add(h);

                    ApplyGridSymbology();

                    if (ExtentCreatedInLayer != null)
                    {
                        ExtentDraggedBoxEventArgs arg = new ExtentDraggedBoxEventArgs(_shapefileBoundingRectangle.Extents.yMax, _shapefileBoundingRectangle.Extents.yMin, _shapefileBoundingRectangle.Extents.xMin, _shapefileBoundingRectangle.Extents.xMax, false);
                        ExtentCreatedInLayer(this, arg);
                    }
                }
                _axMap.MapCursor = tkCursor.crsrMapDefault;
            }
            return _shapefileMajorGridIntersect.NumShapes > 0;
        }

        public bool GenerateMinorGridInsideExtent(Extents definitionExtent, string mapTitle = "")
        {
            bool minorGridCreated = false;
            if (mapTitle.Length > 0)
            {
                _mapTitle = mapTitle;
            }

            //pass the extents of the selected major grids and the extent defined by the select box
            //if minor grid is defined successfully, we proceed with the next steps
            if (_grid25MinorGrid.DefineMinorGrids(_selectedMajorGridShapesExtent, definitionExtent))
            {
                var minorGridExtent = _grid25MinorGrid.MinorGridLinesShapeFile.Extents;

                //get the intersection of the minorGridExtent and the selected major grids
                if (MajorGridsIntersectMinorGridExtent(minorGridExtent))
                {
                    _shapeFileSelectedMajorGridBuffer = _shapefileMajorGridIntersect.BufferByDistance(0, 1, false, true);
                    _shapeFileSelectedMajorGridBuffer.GeoProjection = _axMap.GeoProjection;

                    if (_shapefileMajorGridIntersect.NumShapes > 0
                        && _grid25MinorGrid.ClipMinorGrid(_shapeFileSelectedMajorGridBuffer)
                        && DefineBoundingRectangle(minorGridExtent))
                    {
                        _grid25LabelManager = new Grid25LabelManager(_axMap.GeoProjection);

                        if (_grid25LabelManager.LabelGrid(_shapeFileSelectedMajorGridBuffer, _gridAndLabelProperties, _mapTitle))
                        {
                            _grid25LabelManager.AddMajorGridLabels(_listIntersectedMajorGrids);
                            _grid25LabelManager.Grid25Labels.Labels.ApplyCategories();

                            //we add the layers using the maplayer class, then we add the layer handles to listGridLayers
                            var h = _mapLayers.AddLayer(_grid25MinorGrid.MinorGridLinesShapeFile, "Minor grid", true, true);
                            _mapLayers.LayerDictionary[h].IsGraticule = true;
                            _mapLayers.LayerDictionary[h].IsFishingGrid = true;
                            //_mapLayers.LayerDictionary[h].LayerWeight = 2;
                            AssignLayerWeight(_mapLayers.LayerDictionary[h]);
                            _listGridLayers.Add(h);

                            h = _mapLayers.AddLayer(_grid25LabelManager.Grid25Labels, "Labels", true, true);
                            _mapLayers.LayerDictionary[h].IsFishingGrid = true;
                            //_mapLayers.LayerDictionary[h].LayerWeight = 1;
                            AssignLayerWeight(_mapLayers.LayerDictionary[h]);
                            _listGridLayers.Add(h);

                            h = _mapLayers.AddLayer(_shapefileMajorGridIntersect, "Major grid", true, true);
                            _mapLayers.LayerDictionary[h].IsGraticule = true;
                            _mapLayers.LayerDictionary[h].IsFishingGrid = true;
                            //_mapLayers.LayerDictionary[h].LayerWeight = 4;
                            AssignLayerWeight(_mapLayers.LayerDictionary[h]);
                            _listGridLayers.Add(h);

                            h = _mapLayers.AddLayer(_shapefileBoundingRectangle, "MBR", true, true);
                            _mapLayers.LayerDictionary[h].IsGraticule = true;
                            _mapLayers.LayerDictionary[h].IsFishingGrid = true;
                            //_mapLayers.LayerDictionary[h].LayerWeight = 3;
                            AssignLayerWeight(_mapLayers.LayerDictionary[h]);
                            _listGridLayers.Add(h);

                            ApplyGridSymbology();

                            if (ExtentCreatedInLayer != null)
                            {
                                ExtentDraggedBoxEventArgs arg = new ExtentDraggedBoxEventArgs(_shapefileBoundingRectangle.Extents.yMax, _shapefileBoundingRectangle.Extents.yMin, _shapefileBoundingRectangle.Extents.xMin, _shapefileBoundingRectangle.Extents.xMax, false);
                                ExtentCreatedInLayer(this, arg);
                            }
                            minorGridCreated = true;
                        }
                        _axMap.MapCursor = tkCursor.crsrMapDefault;
                    }
                }
            }

            return minorGridCreated;
        }

        /// <summary>
        /// Creates an extent defined by the selection box.
        /// The extent will be used to select major grids or,
        /// if defining minor grid, then we pass the extent of the
        /// selected major grid and the extent of the selection box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMapSelectBoxFinal(object sender, _DMapEvents_SelectBoxFinalEvent e)
        {
            if (_enableMapInteraction && _axMap.CursorMode == tkCursorMode.cmSelection)
            {
                var extL = 0D;
                var extR = 0D;
                var extT = 0D;
                var extB = 0D;
                Extents selectionBoxExtent = new Extents();

                _axMap.PixelToProj(e.left, e.top, ref extL, ref extT);
                _axMap.PixelToProj(e.right, e.bottom, ref extR, ref extB);
                selectionBoxExtent.SetBounds(extL, extB, 0, extR, extT, 0);

                if (_inDefineMinorGrid && _axMap.UDCursorHandle == _hCursorDefineGrid)
                {
                    //This is where the actual work of constructing the grid map takes place
                    if (!GenerateMinorGridInsideExtent(selectionBoxExtent))
                    {
                    }
                }
                else
                {
                    //pass the extent to SelectGrids function
                    //FromSelectionBox is true because we dragged a selection box
                    SelectGrids(selectionBoxExtent, SelectionFromSelectBox: true, CreateSelectionInMajorGrid: false);
                }
            }
        }

        /// <summary>
        /// Perform gridding using the layout
        /// </summary>
        /// <returns></returns>
        public bool GenerateMinorGridFromLayout(string fishingGround, string folder)
        {
            if (_layoutHelper != null
                && _layoutHelper.LayoutShapeFile.NumShapes > 0
                && _layoutHelper.HasCompletePanelTitles())
            {
                _inDefineGridFromLayout = true;
                _layoutHelper.FishingGround = fishingGround;
                _folderToSave = folder;
                _layoutHelper.GridFromLayoutSaveFolder = _folderToSave;
                _selectedMajorGridShapesExtent = _layoutHelper.SelectedMajorGridExtents;
                for (int n = 0; n < _layoutHelper.LayoutShapeFile.NumShapes; n++)
                {
                    int fldTitle = _layoutHelper.LayoutShapeFile.FieldIndexByName["Title"];
                    string mapTitle = _layoutHelper.LayoutShapeFile.CellValue[fldTitle, n].ToString();
                    if (GenerateMinorGridInsideExtent(_layoutHelper.LayoutShapeFile.Shape[n].Extents, mapTitle))
                    {
                        Save($@"{_folderToSave}\{fishingGround}-{mapTitle}");
                        if (OnGridInPanelCreated != null)
                        {
                            LayerEventArg e = new LayerEventArg(mapTitle);
                            OnGridInPanelCreated(this, e);
                        }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///Selects major grids that intersects the extent.
        ///Selected grids are categorized and will have a fill color.
        ///
        /// This function is only called if _inDefineMinorGrid=false
        /// </summary>
        /// <param name="ext"></param>
        /// <param name="SelectionFromSelectBox"></param>
        private void SelectGrids(Extents ext, bool SelectionFromSelectBox = false, bool CreateSelectionInMajorGrid = false)
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
                            {
                                _shapefileMajorGrid.EditCellValue(ifldToGrid, item, "");
                            }
                        }
                        _listSelectedShapeGridNumbers.Clear();

                        //adds selected shapes to a List<int> and sets toGrid column to "T"
                        for (int n = 0; n < _selectedShapeIndexes.Length; n++)
                        {
                            _shapefileMajorGrid.EditCellValue(ifldToGrid, _selectedShapeIndexes[n], "T");
                            _listSelectedShapeGridNumbers.Add(_selectedShapeIndexes[n]);
                            if (CreateSelectionInMajorGrid)
                            {
                                _shapefileMajorGrid.ShapeSelected[_selectedShapeIndexes[n]] = true;
                            }
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
    }
}
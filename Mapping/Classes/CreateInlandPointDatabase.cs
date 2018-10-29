using dao;          //we are using DAO because ADOx does not work in well creating new database and examining tables in existing databases
using MapWinGIS;
using System;
using System.IO;
using FAD3.GUI.Classes;

namespace FAD3
{
    /// <summary>
    /// Creates a database of minor grids that are within land areas. Used for validating fishing grounds for avoiding inland fishing grids.
    /// </summary>
    public static class CreateInlandPointDatabase
    {
        public static Shapefile LandShapefile { get; set; }
        public static Shapefile Grid25Shapefile { get; set; }
        public static Shapefile MinorGridsShapefile { get; private set; }
        private static Shapefile _inlandMinorGrids;
        public static string FileName { get; set; }
        private static int[] _intersectedMajorGrids;
        private static int[] _withinMinorGrids;
        public static fadUTMZone UTMZone { get; set; }
        private static int _iFldName;
        private static int _iFldInland;

        public static MapInterActionHandler MapInterActionHandler { get; set; }

        public delegate void StatusUpdateHandler(CreateInlandGridEventArgs e);
        public static event StatusUpdateHandler StatusUpdate;

        public static int MajorGridsSelectedCount
        {
            get { return _intersectedMajorGrids.Length; }
        }

        /// <summary>
        /// Start the process of creating a database of inland minor grids
        /// </summary>
        public static void Start()
        {
            if (LandShapefile?.ShapefileType == ShpfileType.SHP_POLYGON
                && Grid25Shapefile?.ShapefileType == ShpfileType.SHP_POLYGON
                && FileName.Length > 0)
            {
                var proceed = true;

                if (StatusUpdate != null)
                {
                    CreateInlandGridEventArgs e = new CreateInlandGridEventArgs();
                    e.Status = "Point creation started";
                    e.StatusDescription = DateTime.Now.ToShortTimeString();
                    StatusUpdate(e);
                }

                proceed = CheckandCreateTable(FileName);

                if (proceed)
                {
                    if (LandShapefile.NumSelected > 0)
                    {
                        ListIntersectSelectedLandArea();
                    }
                    else
                    {
                        ListIntersectLandArea();
                    }

                    UpdateDatabase();

                    global.MappingForm.MapLayersHandler.AddLayer(_inlandMinorGrids, "Inland grids", true, true);

                    CreateInlandGridEventArgs e = new CreateInlandGridEventArgs();
                    e.Status = "Point creation finished";
                    e.StatusDescription = DateTime.Now.ToShortTimeString();
                    StatusUpdate(e);
                }
            }
        }

        /// <summary>
        ///Minor grids that are inside land will be copied to another shapefile of inland minor grids
        /// </summary>
        private static void ProcessInlandGrids()
        {
            if (_inlandMinorGrids.NumShapes == 0)
            {
                _inlandMinorGrids = MinorGridsShapefile.ExportSelection();
            }
            else
            {
                _inlandMinorGrids = _inlandMinorGrids.Merge(false, MinorGridsShapefile, true);
            }
            _inlandMinorGrids.DefaultDrawingOptions.FillVisible = false;
            _inlandMinorGrids.DefaultDrawingOptions.LineWidth = 1.1f;
            _inlandMinorGrids.DefaultDrawingOptions.LineColor = new Utils().ColorByName(tkMapColor.Red);
        }

        /// <summary>
        /// Populate the database with values from the inland minor grid shapefile
        /// </summary>
        private static void UpdateDatabase()
        {
            var zoneName = FishingGrid.UTMZoneName;
            var gridName = "";
            var db = new DBEngine();
            var dbData = db.OpenDatabase(FileName);
            for (int n = 0; n < _inlandMinorGrids.NumShapes; n++)
            {
                gridName = (string)_inlandMinorGrids.CellValue[_iFldName, n];
                var sql = $@"Insert into tblGrid25Inland (grid_name,[zone],x,y) values(
                              '{gridName}',
                              '{zoneName}',
                              {_inlandMinorGrids.Shape[n].Centroid.x},
                              {_inlandMinorGrids.Shape[n].Centroid.y}
                    )";

                dbData.Execute(sql);
            }

            dbData.Close();
            dbData = null;
            db = null;
        }

        /// <summary>
        /// Creates minor grid cells inside major grids intersected with land shapefiles
        /// </summary>
        private static void PopulateMinorGrid()
        {
            MinorGridsShapefile.EditClear();
            FishingGrid.UTMZone = UTMZone;

            int ifldGridNo = Grid25Shapefile.FieldIndexByName["grid_no"];
            var gridNumber = 0;

            //enumerate all intersected major grids
            for (int n = 0; n < _intersectedMajorGrids.Length; n++)
            {
                gridNumber = (int)Grid25Shapefile.CellValue[Grid25Shapefile.FieldIndexByName["grid_no"], _intersectedMajorGrids[n]];

                //get the origin of the current major grid
                var origin = FishingGrid.MajorGridOrigin(gridNumber);

                //build a minor grid, a point at a time. Here we will be creating 5 points, one point for each corner of the grid.
                //The 5th point is needed to close the polygon.
                for (int row = 25; row > 0; row--)
                {
                    for (int col = 0; col < 25; col++)
                    {
                        var shp = new Shape();
                        if (shp.Create(ShpfileType.SHP_POLYGON))
                        {
                            for (int pt = 0; pt < 5; pt++)
                            {
                                switch (pt)
                                {
                                    case 0:
                                    case 4:
                                        shp.AddPoint(origin.x + (col * 2000), origin.y + ((25 - row) * 2000));
                                        break;

                                    case 1:
                                        shp.AddPoint(origin.x + (col * 2000) + 2000, origin.y + ((25 - row) * 2000));
                                        break;

                                    case 2:
                                        shp.AddPoint(origin.x + (col * 2000) + 2000, origin.y + ((25 - row) * 2000) + 2000);
                                        break;

                                    case 3:
                                        shp.AddPoint(origin.x + (col * 2000), origin.y + ((25 - row) * 2000) + 2000);
                                        break;
                                }
                            }

                            //add the new shape to the shapefile. iShp is the index of the newest added shape
                            var iShp = MinorGridsShapefile.EditAddShape(shp);

                            //a new shape will have an index (iShp) of zero or greater
                            if (iShp >= 0)
                            {
                                //name the cell
                                MinorGridsShapefile.EditCellValue(_iFldName, iShp, $"{gridNumber.ToString()}-{(char)('A' + col)}{row}");

                                //set the inland attribute to false
                                MinorGridsShapefile.EditCellValue(_iFldInland, iShp, false);
                            }
                        }
                    }
                }
            }

            //raise the event that minor grids were created
            if (StatusUpdate != null)
            {
                CreateInlandGridEventArgs e = new CreateInlandGridEventArgs();
                e.Status = "Minor grids created";
                e.GridCount = MinorGridsShapefile.NumShapes;
                StatusUpdate(e);
            }
        }

        /// <summary>
        /// Intersects selected land shapes with grid25 major grid shapefiles
        /// </summary>
        private static void ListIntersectSelectedLandArea()
        {
            if (LandShapefile.NumSelected > 0 && SetUpMInorGridShapefile())
            {
                //get the selected shape indexes
                var selectedShapesIndexes = MapInterActionHandler.SelectedShapeIndexes;

                //enumerate all selected shapes
                for (int n = 0; n < selectedShapesIndexes.Length; n++)
                {
                    //clear all selections
                    LandShapefile.SelectNone();

                    //select one shape in the selected shapes
                    LandShapefile.ShapeSelected[selectedShapesIndexes[n]] = true;

                    //intersect the selected shape with the major grid
                    var objSelected = new object();
                    Grid25Shapefile.SelectByShapefile(LandShapefile, tkSpatialRelation.srIntersects, true, ref objSelected);
                    _intersectedMajorGrids = (int[])objSelected;

                    if (StatusUpdate != null)
                    {
                        CreateInlandGridEventArgs e = new CreateInlandGridEventArgs();
                        e.Status = "Major grids intersected";
                        e.GridCount = _intersectedMajorGrids.Length;
                        StatusUpdate(e);
                    }

                    PopulateMinorGrid();
                    if (GetMinorGridsWithinLand()) ProcessInlandGrids();
                }
            }
        }

        /// <summary>
        /// Intersects all land area shapes with grid25 major grids
        /// </summary>
        private static void ListIntersectLandArea()
        {
            if (SetUpMInorGridShapefile())
            {
                //enumerate all shapes in the land shapefile
                for (int n = 0; n < LandShapefile.NumShapes; n++)
                {
                    //make sure there are no selections
                    LandShapefile.SelectNone();

                    //select one shape
                    LandShapefile.ShapeSelected[n] = true;

                    //do the intersection, with the intersection result saved in the objSelected array
                    var objSelected = new object();
                    Grid25Shapefile.SelectByShapefile(LandShapefile, tkSpatialRelation.srIntersects, SelectedOnly: true, ref objSelected);
                    _intersectedMajorGrids = (int[])objSelected;

                    //raise event
                    if (StatusUpdate != null)
                    {
                        CreateInlandGridEventArgs e = new CreateInlandGridEventArgs();
                        e.Status = "Major grids intersected";
                        e.GridCount = _intersectedMajorGrids.Length;
                        StatusUpdate(e);
                    }

                    PopulateMinorGrid();
                    if (GetMinorGridsWithinLand()) ProcessInlandGrids();
                }
            }
        }

        /// <summary>
        /// create minor grid shapefile
        /// </summary>
        /// <returns></returns>
        private static bool SetUpMInorGridShapefile()
        {
            MinorGridsShapefile = new Shapefile();
            if (MinorGridsShapefile.CreateNewWithShapeID("", ShpfileType.SHP_POLYGON))
            {
                _iFldName = MinorGridsShapefile.EditAddField("name", FieldType.STRING_FIELD, 1, 8);
                _iFldInland = MinorGridsShapefile.EditAddField("inland", FieldType.BOOLEAN_FIELD, 1, 1);
                MinorGridsShapefile.GeoProjection = global.MappingForm.MapControl.GeoProjection;

                _inlandMinorGrids = new Shapefile();
                if (_inlandMinorGrids.CreateNewWithShapeID("", ShpfileType.SHP_POLYGON))
                {
                    _inlandMinorGrids.EditAddField("name", FieldType.STRING_FIELD, 1, 8);
                    _inlandMinorGrids.EditAddField("inland", FieldType.BOOLEAN_FIELD, 1, 1);
                }

                return true;
            }
            return false;
        }

        /// <summary>
        /// Intersect minor grids with the currently selected land shapefile. Returns true if current land shape contains minor grid
        /// </summary>
        private static bool GetMinorGridsWithinLand()
        {
            var objSelected = new object();
            var ifldInland = MinorGridsShapefile.FieldIndexByName["inland"];

            //this operation selects those minor grids within the selected land shape.
            MinorGridsShapefile.SelectByShapefile(LandShapefile, tkSpatialRelation.srWithin, true, ref objSelected);

            try
            {
                _withinMinorGrids = (int[])objSelected;
                for (int n = 0; n < _withinMinorGrids.Length; n++)
                {
                    MinorGridsShapefile.EditCellValue(ifldInland, _withinMinorGrids[n], true);
                    MinorGridsShapefile.ShapeSelected[_withinMinorGrids[n]] = true;
                }

                if (StatusUpdate != null)
                {
                    CreateInlandGridEventArgs e = new CreateInlandGridEventArgs();
                    e.Status = "Inland grids created";
                    e.GridCount = _withinMinorGrids.Length;
                    StatusUpdate(e);
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "CreateInlandPointDatabase.cs", "GetMinorGridsWithinLand");
                return false;
            }
        }

        /// <summary>
        /// Inspect database and create point table if not found
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        private static bool CheckandCreateTable(string fileName)
        {
            var dbe = new DBEngine();
            var tableFound = false;

            if (File.Exists(fileName))
            {
                var dbData = dbe.OpenDatabase(fileName);
                foreach (TableDef td in dbData.TableDefs)
                {
                    if (td.Name == "tblGrid25Inland")
                    {
                        tableFound = true;
                        break;
                    }
                }

                if (!tableFound)
                {
                    tableFound = CreateInlandGridTable(dbData);
                }
            }

            if (!tableFound)
            {
                var dbData = dbe.CreateDatabase(fileName, LanguageConstants.dbLangGeneral);
                tableFound = CreateInlandGridTable(dbData);
            }
            return tableFound;
        }

        /// <summary>
        /// Helper function to create the inland grid table
        /// </summary>
        /// <param name="dbData"></param>
        /// <returns></returns>
        private static bool CreateInlandGridTable(dao.Database dbData)
        {
            var sql = @"Create table tblGrid25Inland
                            (grid_name TEXT(8),
                             zone TEXT(3),
                             x DOUBLE,
                             y DOUBLE)";

            dbData.Execute(sql);

            sql = "Create index idxPrimary on tblGrid25Inland (grid_name, zone) with PRIMARY";

            dbData.Execute(sql);

            return true;
        }
    }
}
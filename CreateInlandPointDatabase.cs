using dao;          //we are using DAO because ADOx does not work in well creating new database and examining tables in existing databases
using MapWinGIS;
using System;
using System.IO;

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
        public static FishingGrid.fadUTMZone UTMZone { get; set; }
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

                    ProcessInlandGrids();
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
        ///Process inland grids created by intersecting minor grids with land shapes
        /// </summary>
        private static void ProcessInlandGrids()
        {
            if (MinorGridsShapefile.NumSelected > 0)
            {
                if (_inlandMinorGrids.NumShapes == 0)
                {
                    _inlandMinorGrids = MinorGridsShapefile.ExportSelection();
                }
                else
                {
                    _inlandMinorGrids.Merge(false, MinorGridsShapefile, true); //   = MinorGridsInlandShapefile.ExportSelection();
                }
                _inlandMinorGrids.DefaultDrawingOptions.FillVisible = false;
                _inlandMinorGrids.DefaultDrawingOptions.LineWidth = 1.1f;
                _inlandMinorGrids.DefaultDrawingOptions.LineColor = new Utils().ColorByName(tkMapColor.Red);

                //clear all shapes from the minorgrid shapefile so that during the next intersection process, the next selected
                //land shape will not deal with grids that do not intersect with it.
                MinorGridsShapefile.EditClear();
            }
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
        }

        /// <summary>
        /// Creates minor grid cells inside major grids intersected with land shapefiles
        /// </summary>
        private static void PopulateMinorGrid()
        {
            FishingGrid.UTMZone = UTMZone;

            int ifldGridNo = Grid25Shapefile.FieldIndexByName["grid_no"];
            var gridNumber = 0;

            //enumerate all intersected major grids
            for (int n = 0; n < _intersectedMajorGrids.Length; n++)
            {
                gridNumber = (int)Grid25Shapefile.CellValue[Grid25Shapefile.FieldIndexByName["grid_no"], _intersectedMajorGrids[n]];

                //get the origin of the current major grid
                var origin = FishingGrid.MajorGridOrigin(gridNumber);

                //build a minor grid, a point at a time
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
                            var iShp = MinorGridsShapefile.EditAddShape(shp);
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
                    GetMinorGridsWithinLand();
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
                    GetMinorGridsWithinLand();
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
        /// Intersect minor grids with the currently selected land shapefile
        /// </summary>
        private static void GetMinorGridsWithinLand()
        {
            var objSelected = new object();
            var ifldInland = MinorGridsShapefile.FieldIndexByName["inland"];

            //this select operation currently uses the currenty selected land shape
            MinorGridsShapefile.SelectByShapefile(LandShapefile, tkSpatialRelation.srWithin, true, ref objSelected);

            var proceed = true;
            try
            {
                _withinMinorGrids = (int[])objSelected;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "CreateInlandPointDatabase.cs", "GetMinorGridsWithinLand");
                proceed = false;
            }

            if (proceed)
            {
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
            }

            if (!tableFound)
            {
                var dbData = dbe.CreateDatabase(fileName, dao.LanguageConstants.dbLangGeneral);
                var sql = @"Create table tblGrid25Inland
                            (grid_name TEXT(8),
                             zone TEXT(3),
                             x DOUBLE,
                             y DOUBLE)";

                dbData.Execute(sql);

                sql = "Create index idxPrimary on tblGrid25Inland (grid_name, zone) with PRIMARY";

                dbData.Execute(sql);

                tableFound = true;
            }
            return tableFound;
        }
    }
}
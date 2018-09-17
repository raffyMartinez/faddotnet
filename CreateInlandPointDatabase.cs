using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ADOX;
using MapWinGIS;
using System.Data.OleDb;
using System.Data;
using System.ComponentModel;

namespace FAD3
{
    public static class CreateInlandPointDatabase
    {
        public static Shapefile LandShapefile { get; set; }
        public static Shapefile FastPolygonShapefile { get; set; }
        public static Shapefile Grid25Shapefile { get; set; }
        public static Shapefile MinorGridsShapefile { get; private set; }
        private static Shapefile _inlandMinorGrids;
        public static string FileName { get; set; }
        private static int[] _intersectedMajorGrids;
        private static int[] _withinMinorGrids;
        public static FishingGrid.fadUTMZone UTMZone { get; set; }
        private static int _iFldName;
        private static int _iFldInland;
        private static int[] _intersectedLandShapes;
        private static string _conString;

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
                if (File.Exists(FileName))
                {
                    File.Delete(FileName);
                }

                if (CreateTable(FileName))
                {
                    if (FastPolygonShapefile?.ShapefileType == ShpfileType.SHP_POLYGON)
                    {
                        ListIintersectFastPolygon();
                    }
                    else
                    {
                        if (LandShapefile.NumSelected > 0)
                        {
                            ListIntersectSelectedLandArea();
                        }
                        else
                        {
                            ListIntersectLandArea();
                        }
                    }

                    ProcessInlandGrids();
                    UpdateDatabase();

                    global.MappingForm.MapLayersHandler.AddLayer(_inlandMinorGrids, "Inland grids", true, true);
                }
            }
        }

        /// <summary>
        /// Intersect fast polygons with grid25 major grids
        /// </summary>
        private static void ListIintersectFastPolygon()
        {
            var objSelected = new object();
            for (int n = 0; n < FastPolygonShapefile.NumShapes; n++)
            {
                LandShapefile.SelectNone();
                LandShapefile.ShapeSelected[n] = true;
                LandShapefile.SelectByShapefile(FastPolygonShapefile, tkSpatialRelation.srWithin, true, ref objSelected);
                _intersectedLandShapes = (int[])objSelected;

                Grid25Shapefile.SelectByShapefile(FastPolygonShapefile, tkSpatialRelation.srIntersects, true, ref objSelected);
                _intersectedMajorGrids = (int[])objSelected;

                CreateMinorGridPoints();

                for (int y = 0; y < _intersectedLandShapes.Length; y++)
                {
                    LandShapefile.SelectNone();
                    LandShapefile.ShapeSelected[_intersectedLandShapes[y]] = true;
                    GetMinorGridsWithinLand();
                }
            }

            //raise event
            if (StatusUpdate != null)
            {
                CreateInlandGridEventArgs e = new CreateInlandGridEventArgs();
                e.Status = "Major grids intersected";
                e.GridCount = _intersectedMajorGrids.Length;
                StatusUpdate(e);
            }

            for (int n = 0; n < _intersectedLandShapes.Length; n++)
            {
                LandShapefile.SelectNone();
                LandShapefile.ShapeSelected[_intersectedLandShapes[n]] = true;
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
        /// get landshapefile shapes that intersect with fast polygon
        /// </summary>
        private static void IntersectLandWithFastPolygons()
        {
            var objSelected = new object();
            LandShapefile.SelectByShapefile(FastPolygonShapefile, tkSpatialRelation.srIntersects, false, ref objSelected);
            _intersectedLandShapes = (int[])objSelected;
        }

        private static void UpdateDatabase()
        {
            var zoneName = FishingGrid.UTMZoneName;
            var gridName = "";

            using (var con = new OleDbConnection(_conString))
            {
                con.Open();
                for (int n = 0; n < _inlandMinorGrids.NumShapes; n++)
                {
                    gridName = (string)_inlandMinorGrids.CellValue[_iFldName, n];

                    var sql = $@"Insert into tblGrid25Inland (grid_name,[zone],x,y) values(
                              '{gridName}',
                              '{zoneName}',
                              {_inlandMinorGrids.Shape[n].Centroid.x},
                              {_inlandMinorGrids.Shape[n].Centroid.y}
                    )";

                    using (OleDbCommand update = new OleDbCommand(sql, con))
                    {
                        try
                        {
                            update.ExecuteNonQuery();
                        }
                        catch { }
                    }
                }
            }
        }

        /// <summary>
        /// Creates minor grid cells inside major grids intersected with land shapefiles
        /// </summary>
        private static void CreateMinorGridPoints()
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

                    CreateMinorGridPoints();
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

                    CreateMinorGridPoints();
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
        /// Create table for storing inland minor grids
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        private static bool CreateTable(string FileName)
        {
            Catalog cat = new Catalog();
            var success = true;
            try
            {
                cat.Create("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                   FileName + ";Jet OLEDB:Engine Type=5");
                _conString = ("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName);
            }
            catch (Exception ex)
            {
                success = false;
                Logger.Log(ex.Message, "CreateInlandPointDatabase.cs", "Start()");
            }
            if (success)
            {
                ADOX.Table table = new ADOX.Table();
                table.Name = "tblGrid25Inland";
                table.ParentCatalog = cat;

                table.Columns.Append("grid_name", DataTypeEnum.adVarWChar, 8);
                table.Columns.Append("zone", DataTypeEnum.adVarWChar, 3);
                table.Columns.Append("x", DataTypeEnum.adInteger);
                table.Columns.Append("y", DataTypeEnum.adInteger);
                table.Keys.Append("PrimaryKey", KeyTypeEnum.adKeyPrimary, "grid_name");
                table.Keys["PrimaryKey"].Columns.Append("zone");

                cat.Tables.Append(table);
                cat = null;
            }

            return success;
        }
    }
}
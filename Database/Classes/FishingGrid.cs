using FAD3.Database.Classes;
using Oware;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Reflection;
using System.Drawing;

namespace FAD3
{
    /// <summary>
    /// Holds various functions useful in creating and validating fishing grids
    /// </summary>
    public static class FishingGrid
    {
        private static string _targetAreaGuid;
        public static Grid25Struct _grid25 = new Grid25Struct();
        private static fadUTMZone _utmZone = fadUTMZone.utmZone_Undefined;
        private static int _zoneNumber = 50;
        private static string _zoneLetter = "N";
        private static fadGridType _gt = fadGridType.gridTypeNone;
        private static List<string> _UTMZones = new List<string>();
        private static string _appPath;
        private static fadSubgridStyle _subGridStyle = fadSubgridStyle.SubgridStyleNone;
        private static List<string> _subGri2dStyleList = new List<string>();
        public static PointF UpperLeftExtent { get; internal set; }
        public static PointF LowerRighttExtent { get; internal set; }

        /// <summary>
        /// Constructor
        /// </summary>
        static FishingGrid()
        {
            _UTMZones.Add("50N");
            _UTMZones.Add("51N");
            _appPath = global.ApplicationPath;

            _subGri2dStyleList.Add("None");
            _subGri2dStyleList.Add("2 x 2");
            _subGri2dStyleList.Add("3 x 3");
        }

        /// <summary>
        /// Returns the length in meters of one side of a major grid
        /// </summary>
        public static int MajorGridSizeMeters
        {
            get { return _grid25.MajorGridSizeMeters; }
        }

        /// <summary>
        /// given a long-lat pair and UTM zone, returns a tuple that contains the grid25 cell name and the UTM coordinates of the centroid of the grid25 cell
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <param name="utmZone"></param>
        /// <param name="datumName"></param>
        /// <returns></returns>
        public static (string grid25Grid, double easting, double northing, bool isValid) LongLatToGrid25(double longitude, double latitude, fadUTMZone utmZone = fadUTMZone.utmZone51N, string datumName = "WGS 84")
        {
            bool proceed = true;
            switch (utmZone)
            {
                case fadUTMZone.utmZone50N:
                    proceed = longitude >= 114 && longitude <= 120;
                    break;

                case fadUTMZone.utmZone51N:
                    proceed = longitude >= 120 && longitude <= 126;
                    break;

                default:
                    proceed = false;
                    break;
            }

            if (proceed && latitude > 0)
            {
                var ll2UTM = new LatLngUTMConverter(datumName);
                var result = ll2UTM.convertLatLngToUtm(latitude, longitude);
                double mgEasting = result.Easting;
                double mgNorthing = result.Northing;
                var minorGridResult = utmCoordinatesToGrid25(mgEasting, mgNorthing, utmZone);
                return (minorGridResult.grid25Name, minorGridResult.Easting, minorGridResult.Northing, true);
            }
            return ("", 0, 0, proceed);
        }

        /// <summary>
        /// given a point with xy coordinates in UTM with zone, gives out equivalent grid25 name and UTM coordinates of grid25 centroid
        /// </summary>
        /// <param name="easting"></param>
        /// <param name="northing"></param>
        /// <param name="utmZone"></param>
        /// <returns></returns>
        public static (string grid25Name, int Easting, int Northing) utmCoordinatesToGrid25(double easting, double northing, fadUTMZone utmZone)
        {
            SetGrid25Parameters(utmZone);
            int mjCol = 1 + (int)(easting - _grid25.MajorGridXOrigin) / 50000;
            int mjRow = (int)(northing - _grid25.MajorGridYOrigin) / 50000;
            int mgNumber = mjRow * (_grid25.MajorGridColumns) + mjCol;
            int mnCol = (int)(((easting / _grid25.MajorGridSizeMeters) - (int)(easting / _grid25.MajorGridSizeMeters)) * _grid25.MajorGridSizeMeters);
            int mnRow = (int)(((northing / _grid25.MajorGridSizeMeters) - (int)(northing / _grid25.MajorGridSizeMeters)) * _grid25.MajorGridSizeMeters);
            mnCol /= _grid25.MinorGridCellSizeMeters;
            mnRow /= _grid25.MinorGridCellSizeMeters;
            var col = (char)('A' + mnCol);
            int outEasting = (int)(easting / _grid25.MinorGridCellSizeMeters) * _grid25.MinorGridCellSizeMeters + (_grid25.MinorGridCellSizeMeters / 2);
            int outNorthing = (int)(northing / _grid25.MinorGridCellSizeMeters) * _grid25.MinorGridCellSizeMeters + (_grid25.MinorGridCellSizeMeters / 2);
            return ($"{mgNumber.ToString()}-{col}{(25 - mnRow).ToString()}", outEasting, outNorthing);
        }

        /// <summary>
        /// Returns length in meters of one side of a minor grid
        /// </summary>
        public static int MinorGridSizeMeters
        {
            get { return _grid25.MinorGridCellSizeMeters; }
        }

        /// <summary>
        /// Sets subgridding of minor grid for finer grid resolution.
        /// Choices are none, 4 subgrids, or 9 subgrids.
        /// </summary>
        public static fadSubgridStyle SubGridStyle
        {
            get { return _subGridStyle; }
            set
            {
                _subGridStyle = value;
                SaveSubGridType();
            }
        }

        /// <summary>
        /// Returns a list containing possible subgrid styles
        /// </summary>
        public static List<string> SubGridStyles
        {
            get { return _subGri2dStyleList; }
        }

        /// <summary>
        /// Returns the type of fishing grid being used
        /// </summary>
        public static fadGridType GridType
        {
            get { return _gt; }
        }

        /// <summary>
        /// Compares two grid25 locations validates them according to the following:
        /// ULGrid must be  upper left of the LRgrid which must be at the lower right position
        /// </summary>
        /// <param name="ULGrid"></param>
        /// <param name="LRGrid"></param>
        /// <returns></returns>
        public static bool ValidGridCorners(string ULGrid, string LRGrid)
        {
            var EastingUL = 0;
            var NorthingUL = 0;
            var EastingLR = 0;
            var NorthingLR = 0;

            Grid25_to_UTM(ULGrid, out EastingUL, out NorthingUL);
            Grid25_to_UTM(LRGrid, out EastingLR, out NorthingLR);

            return EastingUL <= EastingLR && NorthingUL >= NorthingLR;
        }

        /// <summary>
        /// /Returns if a fishing grid name is valid
        /// </summary>
        /// <param name="UTMZone"></param>
        /// <param name="FGGridName"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static bool ValidFGName(fadUTMZone UTMZone, string FGGridName, out string ErrMsg)
        {
            bool ValidName = false;
            ParseGridName(UTMZone, FGGridName, out ValidName, out ErrMsg);
            return ValidName;
        }

        /// <summary>
        /// Returns the name of the grid type in use
        /// </summary>
        public static string GridTypeName
        {
            get
            {
                var gridTypeName = "";
                switch (_gt)
                {
                    case fadGridType.gridTypeNone:
                        gridTypeName = "Not defined";
                        break;

                    case fadGridType.gridTypeGrid25:
                        gridTypeName = "Grid25";
                        if (!IsCompleteGrid25)
                            gridTypeName += " (Incomplete)";
                        break;

                    case fadGridType.gridTypeOther:
                        gridTypeName = "Other grid";
                        break;
                }
                return gridTypeName;
            }
        }

        /// <summary>
        /// returns the UTM zone from the inputted string zone name
        /// </summary>
        /// <param name="ZoneName"></param>
        /// <returns></returns>
        public static fadUTMZone ZoneFromZoneName(string ZoneName)
        {
            fadUTMZone myZone = fadUTMZone.utmZone_Undefined;
            switch (ZoneName)
            {
                case "50N":
                    myZone = fadUTMZone.utmZone50N;
                    break;

                case "51N":
                    myZone = fadUTMZone.utmZone51N;
                    break;
            }
            return myZone;
        }

        /// <summary>
        /// returns the zone name of the current UTM zone
        /// </summary>
        public static string UTMZoneName
        {
            get
            {
                var ZoneName = "";
                switch (_utmZone)
                {
                    case fadUTMZone.utmZone50N:
                        ZoneName = "50N";
                        break;

                    case fadUTMZone.utmZone51N:
                        ZoneName = "51N";
                        break;
                }
                return ZoneName;
            }
        }

        /// <summary>
        /// returns if the current grid is a complete grid25
        /// </summary>
        public static bool IsCompleteGrid25
        {
            get
            {
                return _gt == fadGridType.gridTypeGrid25 && _grid25.UTMZone != fadUTMZone.utmZone_Undefined
                        && _grid25.BoundsEx.Count > 0 && _grid25.GridSet.Count > 0;
            }
        }

        /// <summary>
        /// returns a struct containg the elements of a grid, and using out parameters returns a bool if the name is
        /// valid or not and possibly an error message
        /// </summary>
        /// <param name="UTMZone"></param>
        /// <param name="GridName"></param>
        /// <param name="ValidName"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static GridNameStruct ParseGridName(fadUTMZone UTMZone, string GridName, out bool ValidName, out string ErrMsg)
        {
            ErrMsg = "";
            GridNameStruct myNameStruct = new GridNameStruct();
            var arr = GridName.Split('-');
            ValidName = arr.Length == 2;
            if (ValidName)
            {
                try
                {
                    int MajorGrid = int.Parse(arr[0]);
                    SetGrid25Parameters(UTMZone);
                    ValidName = MajorGrid <= _grid25.MaxGridNumber;
                    if (ValidName)
                    {
                        var col = arr[1].Substring(0, 1).ToUpper().ToCharArray()[0];
                        try
                        {
                            var row = int.Parse(arr[1].Substring(1, arr[1].Length - 1));
                            ValidName = (row > 0 && row < 26 && col >= 'A' && col < 'Z');
                            if (!ValidName)
                            {
                                ErrMsg = "Column names must be from A to Y \r\nand row names mut be from 1 to 25";
                            }
                            else
                            {
                                myNameStruct.MajorGridNumber = MajorGrid;
                                myNameStruct.ColumnName = col.ToString();
                                myNameStruct.ColumnNameChar = col;
                                myNameStruct.RowName = row;
                            }
                        }
                        catch
                        {
                            ValidName = false;
                            ErrMsg = "Column names must be from A to Y \r\nand row names mut be from 1 to 25";
                        }
                    }
                    else
                    {
                        ErrMsg = "MajorGrid number must not exceed " + _grid25.MaxGridNumber;
                    }
                }
                catch
                {
                    ValidName = false;
                    ErrMsg = arr[0] + " is not a correct major grid name";
                }
            }
            else
            {
                ErrMsg = "Fishing ground grid name is not correct";
            }

            return myNameStruct;
        }

        /// <summary>
        /// returns a list of the UTM zones that cover the fishing grounds
        /// </summary>
        public static List<string> UTMZones
        {
            get
            {
                return _UTMZones;
            }
        }

        /// <summary>
        /// manually refreshes the grid data after changing the guid of the target area
        /// </summary>
        public static void Refresh()
        {
            _gt = SetupFishingGrid(_targetAreaGuid);
            GetSubgridType();
        }

        /// <summary>
        /// sets and gets the target area guid of the fishing grid
        /// </summary>
        public static string TargetAreaGuid
        {
            get { return _targetAreaGuid; }
            set
            {
                _targetAreaGuid = value;
                _gt = SetupFishingGrid(_targetAreaGuid);
                GetSubgridType();
            }
        }

        /// <summary>
        /// gets and sets the UTM zone of the fishing grid
        /// </summary>
        public static fadUTMZone UTMZone
        {
            get { return _utmZone; }
            set
            {
                _utmZone = value;
                SetGrid25Parameters(_utmZone);
            }
        }

        /// <summary>
        /// returns a struct containing data of the grid25
        /// </summary>
        public static Grid25Struct Grid25
        {
            get { return _grid25; }
        }

        ///// <summary>
        ///// categories of corners used in defining extents
        ///// </summary>
        //public enum fadCornerType
        //{
        //    cornerTypeUndefined,
        //    cornerTypeUpperLeft,
        //    cornerTypeLowerRight
        //}

        ///// <summary>
        ///// defines categories of grid types
        ///// </summary>
        //public enum fadGridType
        //{
        //    gridTypeNone,
        //    gridTypeGrid25,
        //    gridTypeOther
        //}

        ///// <summary>
        ///// UTM zones that completely covers fishing grounds
        ///// </summary>
        //public enum fadUTMZone
        //{
        //    utmZone_Undefined,
        //    utmZone50N,
        //    utmZone51N
        //}

        ///// <summary>
        ///// categories of subgridding minor grids for finer resolution of fishing grounds
        ///// </summary>
        //public enum fadSubgridSyle
        //{
        //    SubgridStyleNone,
        //    SubgridStyle4,
        //    SubgridStyle9
        //}

        /// <summary>
        /// struct that holds data of a minor grid
        /// </summary>
        public struct GridNameStruct
        {
            public int MajorGridNumber { get; set; }
            public string ColumnName { get; set; }
            public char ColumnNameChar { get; set; }
            public int RowName { get; set; }
        }

        /// <summary>
        /// struct of data defining a fishing ground map
        /// </summary>
        public struct LLBounds
        {
            public double ulX;
            public double ulY;
            public double lrX;
            public double lrY;
            public string ulGridName;
            public string lrGridName;
            public string gridDescription;
            public bool primaryMap;

            public bool IsInisde(double x, double y)
            {
                return x >= ulX && x <= lrX && y <= ulY && y >= lrY;
            }

            public bool MakePrimaryMap()
            {
                //if (!primaryMap && _grid25.Bounds.Count > 1)
                if (!primaryMap && _grid25.BoundsEx.Count > 1)
                {
                    _grid25.BoundsEx.Remove(PrimaryFishingGroundMapName());
                    primaryMap = UpdatePrimaryMap(gridDescription, ulGridName, lrGridName);
                    return primaryMap;
                }
                return false;
            }
        }

        public static string PrimaryFishingGroundMapName()
        {
            var item = new LLBounds();
            foreach (var bounds in _grid25.BoundsEx)
            {
                if (bounds.Value.primaryMap)
                {
                    item = bounds.Value;
                    break;
                }
            }
            return item.gridDescription;
        }

        private static bool UpdatePrimaryMap(string description, string upperCorner, string lowerCorner)
        {
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                var sql = $"Update tblAOI set GridDescription = '{description}', UpperLeftGrid= '{upperCorner}', LowerRightGrid = '{lowerCorner}' where AOIGuid = {{{_targetAreaGuid}}}";
                OleDbCommand update = new OleDbCommand(sql, conn);
                conn.Open();
                return update.ExecuteNonQuery() > 0;
            }
        }

        public static List<string> InlandPoints
        {
            get
            {
                List<string> inlandPoints = new List<string>();
                var conString = $@"Provider=Microsoft.JET.OLEDB.4.0;data source= {_appPath}\grid25inland.mdb";
                using (var con = new OleDbConnection(conString))
                {
                    try
                    {
                        con.Open();
                        string query = $"Select grid_name from tblGrid25Inland where [zone] = '{UTMZoneName}'";
                        using (var dt = new DataTable())
                        {
                            var adapter = new OleDbDataAdapter(query, con);
                            adapter.Fill(dt);
                            foreach (DataRow row in dt.Rows)
                            {
                                inlandPoints.Add(row["grid_name"].ToString());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex.Message, "FishingGrid.cs", "MinorGridIsInland");
                    }
                }
                return inlandPoints;
            }
        }

        /// <summary>
        /// returns a bool if a minor grid is located inland
        /// </summary>
        /// <param name="MinorGridName"></param>
        /// <returns></returns>
        public static bool MinorGridIsInland(string MinorGridName)
        {
            var conString = $@"Provider=Microsoft.JET.OLEDB.4.0;data source= {_appPath}\grid25inland.mdb";

            bool IsInland = false;
            using (var con = new OleDbConnection(conString))
            {
                try
                {
                    con.Open();
                    string query = $"Select grid_name from tblGrid25Inland where grid_name ='{MinorGridName}' and [zone] = '{UTMZoneName}'";
                    using (var dt = new DataTable())
                    {
                        var adapter = new OleDbDataAdapter(query, con);
                        adapter.Fill(dt);
                        IsInland = dt.Rows.Count > 0;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, "FishingGrid.cs", "MinorGridIsInland");
                }
            }

            return IsInland;
        }

        public static bool MinorGridIsInland(string MinorGridName, string utmZoneName)
        {
            var conString = $@"Provider=Microsoft.JET.OLEDB.4.0;data source= {_appPath}\grid25inland.mdb";

            bool IsInland = false;
            using (var con = new OleDbConnection(conString))
            {
                try
                {
                    con.Open();
                    string query = $"Select grid_name from tblGrid25Inland where grid_name ='{MinorGridName}' and [zone] = '{utmZoneName}'";
                    using (var dt = new DataTable())
                    {
                        var adapter = new OleDbDataAdapter(query, con);
                        adapter.Fill(dt);
                        IsInland = dt.Rows.Count > 0;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, "FishingGrid.cs", "MinorGridIsInland");
                }
            }

            return IsInland;
        }

        public static (int x, int y) MajorGridOrigin(int gridNo)
        {
            return (GridXOrigin(gridNo), GridYOrigin(gridNo));
        }

        /// <summary>
        /// sets the parameters of grid25 grids depending on the inputted zone
        /// </summary>
        /// <param name="Zone"></param>
        private static void SetGrid25Parameters(fadUTMZone Zone)
        {
            //set major grids in a utm zone
            _grid25.UTMZone = Zone;
            switch (_grid25.UTMZone)
            {
                case fadUTMZone.utmZone50N:
                    _grid25.MajorGridXOrigin = 300000;
                    //_grid25.MajorGridYOrigin = 800000;
                    _grid25.MajorGridYOrigin = 400000;
                    _grid25.MajorGridColumns = 15;
                    _grid25.MaxGridNumber = 315;
                    break;

                case fadUTMZone.utmZone51N:
                    _grid25.MajorGridXOrigin = -500000;
                    _grid25.MajorGridYOrigin = 350000;
                    _grid25.MajorGridColumns = 30;
                    _grid25.MaxGridNumber = 1230;
                    break;
            }
            _grid25.MajorGridSizeMeters = 50000;
            _grid25.MinorGridCellSizeMeters = 2000;
        }

        /// <summary>
        /// Sets up the fishing ground
        /// 1. reads the grid system used
        /// 1.1 if the grid system is Grid25
        /// 1.1.1 reads the UTM zone in the database
        /// 1.1.2 reads the upper and lower left grid of a fishing ground's extent
        /// 1.1.3 setup the major grid numbers inside the extent
        /// 1.1.4 setup the limits of the extent in degree decimal
        /// 1.2 if the grid system is the older system used (other grid)
        /// 1.2.1 TODO: reads the parameters of the older grid used
        /// </summary>
        /// <param name="AOIGuid"></param>
        /// <returns></returns>
        public static fadGridType SetupFishingGrid(string AOIGuid)
        {
            var gt = fadGridType.gridTypeNone;
            using (var con = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    _utmZone = fadUTMZone.utmZone_Undefined;
                    con.Open();
                    string query = $"Select * from tblAOIOtherGrid where AOIGUID ={{{AOIGuid}}}";
                    using (var dt = new DataTable())
                    {
                        var adapter = new OleDbDataAdapter(query, con);
                        adapter.Fill(dt);
                        gt = dt.Rows.Count > 0 ? fadGridType.gridTypeOther : fadGridType.gridTypeNone;
                        if (gt == fadGridType.gridTypeNone)
                        {
                            query = $"Select UseGrid25, UTMZone, UpperLeftGrid, LowerRightGrid, GridDescription from tblAOI where AOIGuid = {{{AOIGuid}}}";
                            using (adapter = new OleDbDataAdapter(query, con))
                            {
                                adapter.Fill(dt);
                                if (dt.Rows.Count > 0)
                                {
                                    DataRow dr = dt.Rows[0];
                                    gt = bool.Parse(dr["UseGrid25"].ToString()) ? fadGridType.gridTypeGrid25 : fadGridType.gridTypeNone;
                                    if (gt == fadGridType.gridTypeGrid25)
                                    {
                                        var myZone = dr["UTMZone"].ToString();
                                        var UTMZoneSet = true;
                                        switch (myZone)
                                        {
                                            case "51N":
                                                _utmZone = fadUTMZone.utmZone51N;
                                                _zoneNumber = 51;
                                                _zoneLetter = "N";
                                                break;

                                            case "50N":
                                                _utmZone = fadUTMZone.utmZone50N;
                                                _zoneNumber = 50;
                                                _zoneLetter = "N";
                                                break;

                                            default:
                                                UTMZoneSet = false;
                                                break;
                                        }
                                        _grid25.BoundsEx.Clear();

                                        if (UTMZoneSet)
                                        {
                                            SetGrid25Parameters(_utmZone);

                                            //read fishing grid bounds
                                            var myBound = new LLBounds();
                                            var x = 0D; var y = 0D;
                                            var lr = dr["LowerRightGrid"].ToString();
                                            var ul = dr["UpperLeftGrid"].ToString();

                                            if (lr.Length > 0 && ul.Length > 0)
                                            {
                                                Grid25ToLL(lr, out x, out y, fadCornerType.cornerTypeLowerRight);
                                                myBound.lrX = x;
                                                myBound.lrY = y;

                                                x = y = 0D;
                                                Grid25ToLL(ul, out x, out y, fadCornerType.cornerTypeUpperLeft);
                                                myBound.ulX = x;
                                                myBound.ulY = y;

                                                myBound.gridDescription = dr["GridDescription"].ToString();
                                                myBound.lrGridName = lr;
                                                myBound.ulGridName = ul;
                                                myBound.primaryMap = true;
                                                _grid25.BoundsEx.Add(myBound.gridDescription, myBound);
                                            }

                                            //read additional grids
                                            query = $"Select UpperLeft, LowerRight, GridDescription from tblAdditionalAOIExtent where AOIGuid ={{{AOIGuid}}}";
                                            using (adapter = new OleDbDataAdapter(query, con))
                                            {
                                                adapter.Fill(dt);
                                                if (dt.Rows.Count > 0)
                                                {
                                                    for (int i = 0; i < dt.Rows.Count; i++)
                                                    {
                                                        dr = dt.Rows[i];
                                                        x = 0D; y = 0D;
                                                        lr = dr["LowerRight"].ToString();
                                                        ul = dr["UpperLeft"].ToString();

                                                        if (lr.Length > 0 && ul.Length > 0)
                                                        {
                                                            Grid25ToLL(dr["LowerRight"].ToString(), out x, out y, fadCornerType.cornerTypeLowerRight);
                                                            myBound = new LLBounds();
                                                            myBound.lrX = x;
                                                            myBound.lrY = y;

                                                            Grid25ToLL(dr["UpperLeft"].ToString(), out x, out y, fadCornerType.cornerTypeUpperLeft);
                                                            myBound.ulX = x;
                                                            myBound.ulY = y;

                                                            myBound.gridDescription = dr["GridDescription"].ToString();
                                                            myBound.lrGridName = lr;
                                                            myBound.ulGridName = ul;

                                                            _grid25.BoundsEx.Add(myBound.gridDescription, myBound);
                                                        }
                                                    }
                                                }
                                            }
                                            CalculateGridSet();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch { }
            }
            return gt;
        }

        /// <summary>
        /// returns longitude and latitude of a grid25 minor grid
        /// </summary>
        /// <param name="gridName"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="CornerType"></param>
        private static void Grid25ToLL(string gridName, out double X, out double Y, fadCornerType CornerType = fadCornerType.cornerTypeUndefined)
        {
            int x_mtr, y_mtr = 0;
            X = Y = 0;
            MinorGridCentroid(gridName, out x_mtr, out y_mtr);
            if (CornerType != fadCornerType.cornerTypeUndefined)
            {
                switch (CornerType)
                {
                    case fadCornerType.cornerTypeLowerRight:
                        x_mtr += 1000;
                        y_mtr -= 1000;
                        break;

                    case fadCornerType.cornerTypeUpperLeft:
                        x_mtr -= 1000;
                        y_mtr += 1000;
                        break;
                }
                LatLngUTMConverter llc = new LatLngUTMConverter("WGS 84");
                var LatLong = new LatLngUTMConverter.LatLng();
                LatLong = llc.convertUtmToLatLng(x_mtr, y_mtr, _zoneNumber, _zoneLetter);
                X = LatLong.Lng;
                Y = LatLong.Lat;
            }
        }

        /// <summary>
        ///Fills a list containing the major grid numbers found inside the extents
        /// of a fishing ground
        /// </summary>
        private static void CalculateGridSet()
        {
            _grid25.GridSet.Clear();
            int c = 0;
            foreach (var item in _grid25.BoundsEx)
            {
                var arr = item.Value.ulGridName.Split('-');
                var ul = int.Parse(arr[0]);
                arr = item.Value.lrGridName.Split('-');
                var lr = int.Parse(arr[0]);

                var k = lr;
                var gHeight = 1;
                while (k < ul)
                {
                    k += _grid25.MajorGridColumns;
                    gHeight++;
                }
                var gWidth = (k - ul) + 1;

                for (int n = 0; n < gHeight; n++)
                {
                    for (int nn = 0; nn < gWidth; nn++)
                    {
                        _grid25.GridSet.Add((ul - (_grid25.MajorGridColumns * n) + nn).ToString());
                    }
                }
                var ptUL = Grid25ToLatLong(item.Value.ulGridName, _utmZone);
                var ptLR = Grid25ToLatLong(item.Value.lrGridName, _utmZone);

                if (c == 0)
                {
                    UpperLeftExtent = new PointF((float)ptUL.longitude, (float)ptUL.latitude);
                    LowerRighttExtent = new PointF((float)ptLR.longitude, (float)ptLR.latitude);
                }
                else
                {
                    var ulX = UpperLeftExtent.X;
                    var ulY = UpperLeftExtent.Y;
                    if (ptUL.latitude > ulY)
                    {
                        ulY = (float)ptUL.latitude;
                    }
                    if (ptUL.longitude < ulX)
                    {
                        ulX = (float)ptUL.longitude;
                    }
                    UpperLeftExtent = new PointF(ulX, ulY);

                    var lrX = LowerRighttExtent.X;
                    var lrY = LowerRighttExtent.Y;
                    if (ptLR.latitude < lrY)
                    {
                        lrY = (float)ptLR.latitude;
                    }
                    if (ptLR.longitude > lrX)
                    {
                        lrX = (float)ptLR.longitude;
                    }
                    LowerRighttExtent = new PointF(lrX, lrY);
                }

                c++;
            }
        }

        /// <summary>
        /// returns true if a major grid is found in a list of major grids of a fishing ground
        /// </summary>
        /// <param name="MajorGridName"></param>
        /// <returns></returns>
        public static bool MajorGridFound(string MajorGridName)
        {
            return _grid25.GridSet.Contains(MajorGridName);
        }

        /// <summary>
        /// returns a list of additional fishing grounds in a fishing effort/sampling
        /// </summary>
        /// <param name="SamplingGuid"></param>
        /// <returns></returns>
        public static List<String> AdditionalFishingGrounds(string SamplingGuid)
        {
            var myList = new List<string>();
            using (var dt = new DataTable())
            {
                using (var conection = new OleDbConnection(global.ConnectionString))
                {
                    try
                    {
                        conection.Open();
                        var query = $"Select GridName,SubGrid from tblGrid where SamplingGUID ={{{SamplingGuid}}}";

                        using (var adapter = new OleDbDataAdapter(query, conection))
                        {
                            adapter.Fill(dt);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                var dr = dt.Rows[i];
                                string fg = dr["GridName"].ToString();
                                string subGrid = dr["subGrid"].ToString();
                                if (subGrid.Length > 0)
                                {
                                    fg += $"-{subGrid}";
                                }
                                myList.Add(fg);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                    }
                }
            }

            return myList;
        }

        /// <summary>
        /// saves to the database the subgridding type used in a target area
        /// </summary>
        /// <returns></returns>
        private static bool SaveSubGridType()
        {
            var Success = false;
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    var sql = $"Update tblAOI set SubgridStyle = {(int)_subGridStyle} where AOIGuid ={{{TargetAreaGuid}}}";
                    OleDbCommand update = new OleDbCommand(sql, conn);
                    conn.Open();
                    Success = (update.ExecuteNonQuery() > 0);
                    conn.Close();
                }
                catch { }
            }
            return Success;
        }

        /// <summary>
        /// gets the subgrid style used in a target area
        /// </summary>
        private static void GetSubgridType()
        {
            using (var dt = new DataTable())
            {
                using (var conection = new OleDbConnection(global.ConnectionString))
                {
                    try
                    {
                        conection.Open();
                        string query = $"SELECT SubgridStyle FROM tblAOI WHERE AOIGuid= {{{_targetAreaGuid}}}";
                        using (var adapter = new OleDbDataAdapter(query, conection))
                        {
                            adapter.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                DataRow dr = dt.Rows[0];
                                switch (dr["SubgridStyle"].ToString())
                                {
                                    case "-1":
                                    case "0":
                                        _subGridStyle = fadSubgridStyle.SubgridStyleNone;
                                        break;

                                    case "1":
                                        _subGridStyle = fadSubgridStyle.SubgridStyle4;
                                        break;

                                    case "2":
                                        _subGridStyle = fadSubgridStyle.SubgridStyle9;
                                        break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                    }
                }
            }
        }

        /// <summary>
        /// returns the column position of a major grid inside the grid25 grid system
        /// </summary>
        /// <param name="GridNo"></param>
        /// <returns></returns>
        private static int MajorGridColPosition(int GridNo)
        {
            var rv = 0;
            var colCount = _grid25.MajorGridColumns;
            if (GridNo > colCount)
            {
                double d = GridNo / colCount;
                rv = GridNo - ((int)(Math.Floor(d) * colCount));
                if (rv == 0)
                {
                    rv = colCount;
                }
            }
            else
            {
                rv = GridNo;
            }
            return rv;
        }

        /// <summary>
        /// returns the y origin of a major grid
        /// </summary>
        /// <param name="GridNo"></param>
        /// <returns></returns>
        private static int GridYOrigin(int GridNo)
        {
            return MajorGridRowPosition(GridNo) * _grid25.MajorGridSizeMeters + (_grid25.MajorGridYOrigin - _grid25.MajorGridSizeMeters);
        }

        private static int MajorGridRowPosition(int GridNo)
        {
            double d = ((double)GridNo / _grid25.MajorGridColumns);
            if (d == Math.Floor(d))
            {
                d--;
            }
            return (int)Math.Floor(d) + 1;
        }

        /// <summary>
        /// returns the x origin of a major grid
        /// </summary>
        /// <param name="GridNo"></param>
        /// <returns></returns>
        private static int GridXOrigin(int GridNo)
        {
            return MajorGridColPosition(GridNo) * _grid25.MajorGridSizeMeters + (_grid25.MajorGridXOrigin + (-50000));
        }

        /// <summary>
        /// returns centroid x and y position of a minor grid
        /// </summary>
        /// <param name="minorGridName"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private static void MinorGridCentroid(string minorGridName, out int x, out int y)
        {
            x = y = 0;
            if (minorGridName.Length > 0)
            {
                var arr = minorGridName.Split('-');
                var MajorGrid = int.Parse(arr[0].Trim());
                var Success = MajorGrid <= _grid25.MaxGridNumber;

                if (Success)
                {
                    var MinorGrid = arr[1].Trim();
                    var MajorX = GridXOrigin(MajorGrid);
                    var MajorY = GridYOrigin(MajorGrid);
                    var MinorCol = MinorGrid.ToLower()[0] - 96;
                    var MinorRow = 25 - int.Parse(MinorGrid.Substring(1, MinorGrid.Length - 1));
                    x = MajorX + (MinorCol * 2000) - 1000;
                    y = MajorY + (MinorRow * 2000) + 1000;

                    if (MinorCol > 0 && MinorCol < 26 && MinorRow > 0 && MinorRow < 26)
                        CleanGridName();
                }
            }
        }

        /// <summary>
        /// returns using value tuple, the longitude,latitude equivalent of a fishing grid
        /// </summary>
        /// <param name="GridName"></param>
        /// <param name="utmZone"></param>
        /// <returns></returns>
        public static (double latitude, double longitude) Grid25ToLatLong(string GridName, fadUTMZone utmZone)
        {
            int x, y = 0;
            MinorGridCentroid(GridName, out x, out y);
            var converter = new LatLngUTMConverter("WGS 84");
            var zoneNumber = 0;
            if (utmZone == fadUTMZone.utmZone50N)
            {
                zoneNumber = 50;
            }
            else if (utmZone == fadUTMZone.utmZone51N)
            {
                zoneNumber = 51;
            }
            var result = converter.convertUtmToLatLng(x, y, zoneNumber, "N");
            return (result.Lat, result.Lng);
        }

        /// <summary>
        /// returns a string equivalent of the UTM position of a grid name
        /// </summary>
        /// <param name="GridName"></param>
        /// <returns></returns>
        public static string Grid25_to_UTM(string GridName)
        {
            int x, y = 0;
            MinorGridCentroid(GridName, out x, out y);
            return UTMZoneName + " " + x + " " + y;
        }

        /// <summary>
        /// returns using out parameters the UTM easting and northing coordinates of a grid
        /// </summary>
        /// <param name="gridName"></param>
        /// <param name="easting"></param>
        /// <param name="northing"></param>
        /// <returns></returns>
        public static string Grid25_to_UTM(string gridName, out int easting, out int northing)
        {
            easting = northing = 0;
            MinorGridCentroid(gridName, out easting, out northing);
            return UTMZoneName + " " + easting + " " + northing;
        }

        public static Point Grid25ToUTMPoint(string gridName)
        {
            int easting = 0; int northing = 0;
            MinorGridCentroid(gridName, out easting, out northing);
            return new Point(easting, northing);
        }

        private static void CleanGridName()
        {
        }

        public static void ReadGridDetails(string AOIGuid)
        {
        }

        /// <summary>
        /// deletes additional fishing ground maps of a target area
        /// </summary>
        /// <param name="TargetAreaGuid"></param>
        public static void DeleteAdditionalFishingGroundMaps(string TargetAreaGuid)
        {
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                var sql = $"Delete * from tblAdditionalAOIExtent where AOIGuid = {{{TargetAreaGuid}}}";
                OleDbCommand update = new OleDbCommand(sql, conn);
                conn.Open();
                update.ExecuteNonQuery();
            }
        }

        public static bool DeleteFishingGroundMap(string name, string targetAreaGuid)
        {
            bool proceed = true;
            if (Grid25.BoundsEx.Count == 1)
            {
                return false;
            }
            else
            {
                if (name == PrimaryFishingGroundMapName())
                {
                    proceed = false;
                    foreach (var item in _grid25.BoundsEx)
                    {
                        if (!item.Value.primaryMap)
                        {
                            name = item.Value.gridDescription;
                            proceed = item.Value.MakePrimaryMap();
                            break;
                        }
                    }
                }

                if (proceed)
                {
                    using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
                    {
                        var sql = $"Delete * from tblAdditionalAOIExtent where AOIGuid = {{{targetAreaGuid}}} AND GridDescription='{name}'";
                        OleDbCommand update = new OleDbCommand(sql, conn);
                        conn.Open();
                        proceed = update.ExecuteNonQuery() > 0;
                    }
                }
            }
            return proceed;
        }

        public static bool AddGrid25Map(string targetAreaGuid, fadSubgridStyle subgridStyle, fadUTMZone zone, string ulg, string lrg, string mapName, bool firstMap = false)
        {
            string utmzone = "";
            bool success = false;
            switch (zone)
            {
                case fadUTMZone.utmZone50N:
                    utmzone = "50N";
                    break;

                case fadUTMZone.utmZone51N:
                    utmzone = "51N";
                    break;

                case fadUTMZone.utmZone_Undefined:
                    break;
            }
            string sql = "";
            if (firstMap)
            {
                sql = $@"Update tblAOI set
                         UseGrid25 = {true},
                         UTMZone = '{utmzone}',
                         SubgridStyle = {(int)subgridStyle},
                         GridDescription = '{mapName}',
                         UpperLeftGrid = '{ulg}',
                         LowerRightGrid = '{lrg}'
                         where AOIGuid = {{{targetAreaGuid}}}";
            }
            else
            {
                sql = $@"Insert into tblAdditionalAOIExtent
                            (AOIGuid, GridDescription, UpperLeft, LowerRight, RowNumber)
                            values
                            ({{{targetAreaGuid}}},'{mapName}','{ulg}','{lrg}', {{{Guid.NewGuid().ToString()}}})";
            }
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                OleDbCommand update = new OleDbCommand(sql, conn);
                conn.Open();
                try
                {
                    success = (update.ExecuteNonQuery() > 0);
                }
                catch (OleDbException)
                {
                    success = false;
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex.Message, ex.StackTrace);
                }
            }
            return success;
        }

        /// <summary>
        /// saves the grid25 parameters of a target area to the database
        /// </summary>
        /// <param name="TargetAreaGuid"></param>
        /// <param name="UseGrid25"></param>
        /// <param name="UTMZone"></param>
        /// <param name="SubGridStyle"></param>
        /// <param name="Maps"></param>
        /// <param name="FirstMap"></param>
        /// <returns></returns>
        public static bool SaveTargetAreaGrid25(string TargetAreaGuid, bool UseGrid25, string UTMZone = "",
                               int SubGridStyle = 0, Dictionary<string, (string MapName, string ULGrid, string LRGrid)> Maps = null,
                               string FirstMap = "")
        {
            var sql = "";
            var Success = false;
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                if (UseGrid25)
                {
                    sql = $@"Update tblAOI set
                         UseGrid25 = {UseGrid25},
                         UTMZone = '{UTMZone}',
                         SubgridStyle = {SubGridStyle},
                         GridDescription = '{Maps[FirstMap].MapName}',
                         UpperLeftGrid = '{Maps[FirstMap].ULGrid}',
                         LowerRightGrid = '{Maps[FirstMap].LRGrid}'
                         where AOIGuid = {{{TargetAreaGuid}}}";
                }
                else
                {
                    sql = $@"Update tblAOI set
                         UseGrid25 = {UseGrid25},
                         UTMZone = '',
                         SubgridStyle = 0,
                         GridDescription = '',
                         UpperLeftGrid = '',
                         LowerRightGrid = ''
                         where AOIGuid = {{{TargetAreaGuid}}}";
                }

                OleDbCommand update = new OleDbCommand(sql, conn);
                conn.Open();
                Success = (update.ExecuteNonQuery() > 0);

                if (Success && Maps.Count > 1 && UseGrid25)
                {
                    Success = SaveAdditionalFishingGroundMaps(TargetAreaGuid, FirstMap, Maps);
                }
                else
                {
                    DeleteAdditionalFishingGroundMaps(TargetAreaGuid);
                }
            }

            return Success;
        }

        /// <summary>
        /// saves any additional fishing ground maps of a target area to the database
        /// </summary>
        /// <param name="TargetAreaGuid"></param>
        /// <param name="FirstMap"></param>
        /// <param name="Maps"></param>
        /// <returns></returns>
        private static bool SaveAdditionalFishingGroundMaps(string TargetAreaGuid, string FirstMap, Dictionary<string, (string MapName, string ULGrid, string LRGrid)> Maps)
        {
            var SaveCount = 0;

            DeleteAdditionalFishingGroundMaps(TargetAreaGuid);

            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                foreach (var item in Maps.Values)
                {
                    if (item.MapName != FirstMap)
                    {
                        var sql = $@"Insert into tblAdditionalAOIExtent
                            (AOIGuid, GridDescription, UpperLeft, LowerRight, RowNumber)
                            values
                            ({{{TargetAreaGuid}}},'{item.MapName}','{item.ULGrid}','{item.LRGrid}', {{{Guid.NewGuid().ToString()}}})";
                        OleDbCommand update = new OleDbCommand(sql, conn);
                        if (update.ExecuteNonQuery() > 0) SaveCount++;
                    }
                }
            }

            return SaveCount > 0;
        }

        /// <summary>
        /// holds the data of a grid25 system
        /// </summary>
        public struct Grid25Struct
        {
            private static List<string> _GridSet;
            public int MinorGridCellSizeMeters { get; set; }
            public fadUTMZone UTMZone { get; set; }
            public int MajorGridColumns { get; set; }
            public int MaxGridNumber { get; set; }
            public int MajorGridSizeMeters { get; set; }
            public int MajorGridXOrigin { get; set; }
            public int MajorGridYOrigin { get; set; }
            private static Dictionary<string, LLBounds> _boundsEx;

            public Dictionary<string, LLBounds> BoundsEx
            {
                get { return _boundsEx; }
            }

            public List<string> GridSet
            {
                get { return _GridSet; }
            }

            static Grid25Struct()
            {
                _boundsEx = new Dictionary<string, LLBounds>();
                _GridSet = new List<string>();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;
using Oware;

namespace FAD3
{
    public static class FishingGrid
    {
        private static string _AOIGuid;
        public static Grid25Struct _grid25 = new Grid25Struct();
        private static fadUTMZone _utmZone = fadUTMZone.utmZone_Undefined;
        private static int ZoneNumber = 50;
        private static string ZoneLetter = "N";
        private static fadGridType _gt = fadGridType.gridTypeNone;
        private static List<string> _UTMZones = new List<string>();
        private static string _appPath;

        static FishingGrid()
        {
            _UTMZones.Add("Zone 50N");
            _UTMZones.Add("Zone 51N");
            _appPath = global.ApplicationPath;
        }

        public static fadGridType GridType
        {
            get { return _gt; }
        }

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

        public static string UTMZoneName
        {
            get
            {
                var ZoneName = "";
                switch (_utmZone)
                {
                    case fadUTMZone.utmZone50N:
                        ZoneName = "Zone 50N";
                        break;
                    case fadUTMZone.utmZone51N:
                        ZoneName = "Zone 51N";
                        break;
                }
                return ZoneName;
            }
        }

        public static bool IsCompleteGrid25
        {
            get
            {
                return  _gt == fadGridType.gridTypeGrid25 && _grid25.UTMZone != fadUTMZone.utmZone_Undefined
                        && _grid25.Bounds.Count > 0 && _grid25.GridSet.Count > 0;
 
            }
        }

        public static List<string> UTMZones
        {
            get
            {
                return _UTMZones;
            }
        }

        public static string AOIGuid
        {
            get { return _AOIGuid; }
            set
            {
                _AOIGuid = value;
                _gt = SetupFishingGrid(_AOIGuid);
            }
        }

        public static fadUTMZone UTMZone
        {
            get { return _utmZone; }
            set { _utmZone = value; }
        }

        public static Grid25Struct Grid25
        {
            get { return _grid25; }
        }


        public enum fadCornerType
        {
            cornerTypeUndefined,
            cornerTypeUpperLeft,
            cornerTypeLowerRight
        }

        public enum fadGridType
        {
            gridTypeNone,
            gridTypeGrid25,
            gridTypeOther
        }

        public enum fadUTMZone
        {
            utmZone_Undefined,
            utmZone50N,
            utmZone51N
        }

        public struct LLBounds
        {
            public double ulX;
            public double ulY;
            public double lrX;
            public double lrY;
            public string ulGridName;
            public string lrGridName;
            public string gridDescription;
        }


        public static bool MinorGridIsInland(string MinorGridName)
        {
            var conString = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + _appPath + "\\grid25inland.mdb";
            bool IsInland = false;
            using (var con = new OleDbConnection(conString))
            {
                try
                {
                    con.Open();
                    string query = "Select grid_name from tblGrid25Inland where grid_name ='" + MinorGridName + "'";
                    using (var dt = new DataTable())
                    {
                        var adapter = new OleDbDataAdapter(query, con);
                        adapter.Fill(dt);
                        IsInland = dt.Rows.Count > 0;
                    }
                }
                catch { }
            }
            return IsInland;
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
                    string query = "Select * from tblAOIOtherGrid where AOIGUID ='{" + AOIGuid + "}'";
                    using (var dt = new DataTable())
                    {
                        var adapter = new OleDbDataAdapter(query, con);
                        adapter.Fill(dt);
                        gt = dt.Rows.Count > 0 ? fadGridType.gridTypeOther : fadGridType.gridTypeNone;
                        if (gt == fadGridType.gridTypeNone)
                        {
                            query = "Select UseGrid25, UTMZone, UpperLeftGrid, LowerRightGrid, GridDescription from tblAOI where AOIGuid = '{" + AOIGuid + "}'";
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
                                                ZoneNumber = 51;
                                                ZoneLetter = "N";
                                                break;
                                            case "50N":
                                                _utmZone = fadUTMZone.utmZone50N;
                                                ZoneNumber = 50;
                                                ZoneLetter = "N";
                                                break;
                                            default:
                                                UTMZoneSet = false;
                                                break;
                                        }

                                        _grid25.Bounds.Clear();

                                        if (UTMZoneSet)
                                        {
                                            //set major grids in a utm zone
                                            _grid25.UTMZone = _utmZone;
                                            switch (_grid25.UTMZone)
                                            {
                                                case fadUTMZone.utmZone51N:
                                                    _grid25.MajorGridXOrigin = -500000;
                                                    _grid25.MajorGridYOrigin = 350000;
                                                    _grid25.MajorGridColumns = 30;
                                                    _grid25.MaxGridNumber = 1230;
                                                    _grid25.CellSize = 2000;
                                                    break;
                                                case fadUTMZone.utmZone50N:
                                                    _grid25.MajorGridXOrigin = 300000;
                                                    _grid25.MajorGridYOrigin = 800000;
                                                    _grid25.MajorGridColumns = 15;
                                                    _grid25.MaxGridNumber = 270;
                                                    _grid25.CellSize = 2000;
                                                    break;
                                            }
                                            _grid25.MajorGridSizeMeters = 50000;

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


                                                _grid25.Bounds.Add(myBound);
                                            }

                                            //read additional grids
                                            query = "Select UpperLeft, LowerRight, GridDescription from tblAdditionalAOIExtent where AOIGuid ='{" + AOIGuid + "}'";
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
                                                            myBound.lrX = x;
                                                            myBound.lrY = y;

                                                            myBound.gridDescription = dr["GridDescription"].ToString();
                                                            myBound.lrGridName = lr;
                                                            myBound.ulGridName = ul;

                                                            _grid25.Bounds.Add(myBound);
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
                LatLong = llc.convertUtmToLatLng(x_mtr, y_mtr, ZoneNumber, ZoneLetter);
                X = LatLong.Lng;
                Y = LatLong.Lat;
            }
        }

        /// <summary>
        ///Fills a list containing the major grid numbers found inside the extents
        /// of a fishing ground
        /// </summary>
        static void CalculateGridSet()
        {
            _grid25.GridSet.Clear();
            foreach (var item in _grid25.Bounds)
            {
                var arr = item.ulGridName.Split('-');
                var ul = int.Parse(arr[0]);
                arr = item.lrGridName.Split('-');
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
            }
        }

        static int MajorGridColPosition(int GridNo)
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


        static int GridYOrigin(int GridNo)
        {
            return MajorGridRowPosition(GridNo) * _grid25.MajorGridSizeMeters + (_grid25.MajorGridYOrigin - _grid25.MajorGridSizeMeters);
        }

        static int MajorGridRowPosition(int GridNo)
        {
            double d = (GridNo / _grid25.MajorGridColumns);
            return (int)Math.Floor(d) + 1;
        }

        static int GridXOrigin(int GridNo)
        {
            return MajorGridColPosition(GridNo) * _grid25.MajorGridSizeMeters + (_grid25.MajorGridXOrigin + (-50000));
        }

        static void MinorGridCentroid(string minorGridName, out int x, out int y)
        {
            var arr = minorGridName.Split('-');
            var MajorGrid = int.Parse(arr[0].ToString().Trim());
            var Success = MajorGrid <= _grid25.MaxGridNumber;
            x = y = 0;
            if (Success)
            {
                var MinorGrid = arr[1].ToString().Trim();
                var MajorX = GridXOrigin(MajorGrid);
                var MajorY = GridYOrigin(MajorGrid);
                var arr1 = MinorGrid.Substring(0, 1).ToLower().ToCharArray();
                var MinorCol = (int)arr1[0] - 96;
                var MinorRow = 25 - int.Parse(MinorGrid.Substring(1, MinorGrid.Length - 1));
                x = MajorX + (MinorCol * 2000) - 1000;
                y = MajorY + (MinorRow * 2000) + 1000;

                if (MinorCol > 0 && MinorCol < 26 && MinorRow > 0 && MinorRow < 26)
                    CleanGridName();
            }
        }

        public static string Grid25_to_UTM(string GridName)
        {
            int x, y = 0;
            MinorGridCentroid(GridName, out x, out y);
            return UTMZoneName + " " + x + " " + y;

        }

        private static void CleanGridName()
        {

        }

        public static void ReadGridDetails(string AOIGuid)
        {
            ;
        }



        public struct Grid25Struct
        {
            private static List<string> _GridSet;
            public int CellSize { get; set; }
            public fadUTMZone UTMZone { get; set; }
            public int MajorGridColumns { get; set; }
            public int MaxGridNumber { get; set; }
            public int MajorGridSizeMeters { get; set; }
            public int MajorGridXOrigin { get; set; }
            public int MajorGridYOrigin { get; set; }
            private static List<LLBounds> _bounds;

            public List<LLBounds> Bounds
            {
                get { return _bounds; }
                //set { _bounds = value;  }
            }

            public List<string> GridSet
            {
                get { return _GridSet; }
                //set { _GridSet = value; }
            }

            static Grid25Struct()
            {
                _bounds = new List<LLBounds>();
                _GridSet = new List<string>();
            }
        }
    }

}

using AxMapWinGIS;
using FAD3.Database.Classes;
using MapWinGIS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Reflection;

namespace FAD3
{
    public class FishingGroundMappingHandler : IDisposable
    {
        private bool _disposed;
        private GeoProjection _geoProjection = new GeoProjection();
        public MapLayersHandler MapLayersHandler { get; set; }
        public AxMap MapControl { get; set; }
        public bool RemoveInland { get; set; }

        public FishingGroundMappingHandler(GeoProjection geoProjection = null)
        {
            _geoProjection.SetWgs84();
            if (geoProjection != null)
            {
                _geoProjection = geoProjection;
            }
        }

        public void set_GeoProjection(GeoProjection geoProjection)
        {
            _geoProjection = geoProjection;
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
                _geoProjection = null;
                _disposed = true;
            }
        }

        /// <summary>
        /// Map a single fishing ground when coming from a selected sampling
        /// </summary>
        /// <param name="fishingGround"></param>
        /// <param name="utmZone"></param>
        /// <returns></returns>
        public bool MapFishingGround(string fishingGround, fadUTMZone utmZone, string layerName = "", bool testIfInland = false)
        {
            var sf = new Shapefile();
            bool success = false;
            var fgLayerName = "Fishing ground";
            if (layerName.Length > 0)
            {
                fgLayerName = layerName;
            }
            if (sf.CreateNew("", ShpfileType.SHP_POINT))
            {
                var shp = new Shape();
                if (shp.Create(ShpfileType.SHP_POINT))
                {
                    var iShp = 0;
                    if (_geoProjection.IsGeographic)
                    {
                        var result = FishingGrid.Grid25ToLatLong(fishingGround, utmZone);
                        iShp = shp.AddPoint(result.longitude, result.latitude);
                    }
                    else
                    {
                        FishingGrid.Grid25_to_UTM(fishingGround, out int x, out int y);
                        iShp = shp.AddPoint(x, y);
                    }
                    if (iShp >= 0 && sf.EditInsertShape(shp, 0))
                    {
                        MapLayersHandler.RemoveLayer(fgLayerName);
                        sf.GeoProjection = _geoProjection;
                        var ifldLabel = sf.EditAddField("Label", FieldType.STRING_FIELD, 1, 15);
                        sf.EditCellValue(ifldLabel, iShp, fgLayerName);
                        sf.CollisionMode = tkCollisionMode.AllowCollisions;
                        SymbolizeFishingGround(sf, fgLayerName, testIfInland);

                        success = MapLayersHandler.AddLayer(sf, fgLayerName, true, true) >= 0;
                    }
                }
            }
            return success;
        }

        private void SymbolizeFishingGround(Shapefile sf, string pointName, bool testForInland)
        {
            if (testForInland && pointName != "Fishing ground")
            {
                sf.DefaultDrawingOptions.SetDefaultPointSymbol(tkDefaultPointSymbol.dpsAsterisk);
                sf.Labels.Generate("[Label]", tkLabelPositioning.lpCenter, true);
            }
            else
            {
                sf.DefaultDrawingOptions.PointShape = tkPointShapeType.ptShapeCircle;
            }
            sf.DefaultDrawingOptions.LineVisible = false;
            sf.DefaultDrawingOptions.PointSize = 9;
            sf.DefaultDrawingOptions.FillColor = new Utils().ColorByName(tkMapColor.Red);
        }

        public bool MapSamplingFishingGround(string samplingGuid, fadUTMZone utmZone, string layerName)
        {
            var success = false;
            List<string> fg = new List<string>();
            var sql = $@"SELECT FishingGround FROM tblSampling WHERE SamplingGUID={{{samplingGuid}}}
                        UNION ALL
                        SELECT GridName from tblGrid WHERE SamplingGUID={{{samplingGuid}}}";

            DataTable dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    var adapter = new OleDbDataAdapter(sql, conection);
                    adapter.Fill(dt);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        fg.Add(dr["FishingGround"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }

            if (fg.Count > 0)
            {
                var sf = new Shapefile();
                if (sf.CreateNew("", ShpfileType.SHP_POINT))
                {
                    foreach (var item in fg)
                    {
                        var shp = new Shape();
                        if (shp.Create(ShpfileType.SHP_POINT))
                        {
                            var iShp = 0;

                            var result = FishingGrid.Grid25ToLatLong(item, utmZone);
                            iShp = shp.AddPoint(result.longitude, result.latitude);

                            if (iShp >= 0 && sf.EditInsertShape(shp, 0))
                            {
                                MapLayersHandler.RemoveLayer(layerName);
                                sf.GeoProjection = _geoProjection;
                                var ifldLabel = sf.EditAddField("Label", FieldType.STRING_FIELD, 1, 15);
                                sf.EditCellValue(ifldLabel, iShp, item);
                                sf.CollisionMode = tkCollisionMode.AllowCollisions;
                            }
                        }
                    }
                    sf.DefaultDrawingOptions.SetDefaultPointSymbol(tkDefaultPointSymbol.dpsCircle);
                    sf.DefaultDrawingOptions.FillColor = new Utils().ColorByName(tkMapColor.Red);
                    sf.DefaultDrawingOptions.PointSize = 7;
                    sf.DefaultDrawingOptions.LineVisible = false;
                    success = MapLayersHandler.AddLayer(sf, layerName, true, true) >= 0;
                }
            }
            return success;
        }

        /// <summary>
        /// maps fishing grounds belonging to a target area, landing site, or type of gear gear
        /// </summary>
        /// <param name="aoiGUID"></param>
        /// <param name="samplingYears"></param>
        /// <param name="utmZone"></param>
        /// <param name="Aggregated"></param>
        /// <param name="notInclude1"></param>
        /// <param name="landingSiteGuid"></param>
        /// <param name="gearVariationGuid"></param>
        public void MapFishingGrounds(string aoiGUID, string samplingYears, fadUTMZone utmZone,
            bool Aggregated = true, bool notInclude1 = false, string landingSiteGuid = "",
            string gearVariationGuid = "")
        {
            var query = "";
            var sf = new Shapefile();
            var ifldAOI = 0;
            var ifldLS = 0;
            var ifldGear = 0;
            var ifldYear = 0;
            var ifldFG = 0;
            var ifldCount = 0;
            var ifldMaxWt = 0;
            var ifldMinWt = 0;
            var ifldAfgWt = 0;

            var ifldEnumerator = 0;
            var ifldGearClass = 0;
            var ifldNumberHauls = 0;
            var ifldNumberFisher = 0;
            var ifldDateSet = 0;
            var ifldTimeSet = 0;
            var ifldDateHauled = 0;
            var ifldTimeHauled = 0;
            var ifldVessel = 0;
            var ifldHP = 0;
            var ifldCatchWt = 0;
            if (aoiGUID.Length > 0 && samplingYears.Length > 0 && sf.CreateNewWithShapeID("", ShpfileType.SHP_POINT))
            {
                ifldAOI = sf.EditAddField("AOIName", FieldType.STRING_FIELD, 1, 255);
                sf.Field[ifldAOI].Alias = "Target area name";

                if (Aggregated)
                {
                    if (landingSiteGuid.Length > 0)
                    {
                        ifldLS = sf.EditAddField("LSName", FieldType.STRING_FIELD, 1, 255);
                        sf.Field[ifldLS].Alias = "Landing site name";
                    }

                    if (gearVariationGuid.Length > 0)
                    {
                        ifldGear = sf.EditAddField("GearName", FieldType.STRING_FIELD, 1, 255);
                        sf.Field[ifldGear].Alias = "Gear variation used";
                    }

                    ifldYear = sf.EditAddField("Year", FieldType.INTEGER_FIELD, 1, 4);
                    sf.Field[ifldYear].Alias = "Year sampled";

                    ifldFG = sf.EditAddField("fg", FieldType.STRING_FIELD, 1, 25);
                    sf.Field[ifldFG].Alias = "Fishing ground";

                    ifldCount = sf.EditAddField("n", FieldType.INTEGER_FIELD, 1, 4);

                    ifldMaxWt = sf.EditAddField("MaxWt", FieldType.DOUBLE_FIELD, 2, 8);
                    sf.Field[ifldMaxWt].Alias = "Maximum catch weight";

                    ifldMinWt = sf.EditAddField("MinWt", FieldType.DOUBLE_FIELD, 2, 8);
                    sf.Field[ifldMinWt].Alias = "Minimum catch weight";

                    ifldAfgWt = sf.EditAddField("AvgWt", FieldType.DOUBLE_FIELD, 2, 8);
                    sf.Field[ifldAfgWt].Alias = "Average catch weight";

                    if (gearVariationGuid.Length > 0)
                    {
                        query = $@"SELECT tblAOI.AOIName, tblLandingSites.LSName, tblGearVariations.Variation, tblSampling.FishingGround,
                                Year([SamplingDate]) AS samplingYear, Count(tblSampling.SamplingGUID) AS n, Max(tblSampling.WtCatch) AS MaxCatch,
                                Min(tblSampling.WtCatch) AS MinCatch, Avg(tblSampling.WtCatch) AS AvgCatch FROM tblGearVariations
                                INNER JOIN ((tblAOI INNER JOIN tblLandingSites ON tblAOI.AOIGuid = tblLandingSites.AOIGuid)
                                INNER JOIN tblSampling ON tblLandingSites.LSGUID = tblSampling.LSGUID) ON tblGearVariations.GearVarGUID = tblSampling.GearVarGUID
                                WHERE tblLandingSites.LSGUID={{{landingSiteGuid}}} AND tblSampling.GearVarGUID={{{gearVariationGuid}}}
                                GROUP BY tblAOI.AOIName, tblLandingSites.LSName, tblGearVariations.Variation, tblSampling.FishingGround, Year([SamplingDate]) ";

                        if (notInclude1)
                        {
                            query += $"HAVING Count(tblSampling.SamplingGUID)>1 AND Year([SamplingDate]) In ({samplingYears})";
                        }
                        else
                        {
                            query += $"HAVING Year([SamplingDate]) In ({samplingYears})";
                        }
                    }
                    else if (landingSiteGuid.Length > 0)
                    {
                        query = $@"SELECT tblAOI.AOIName, tblLandingSites.LSName, tblSampling.FishingGround, Year([SamplingDate]) AS samplingYear,
                            Count(tblSampling.SamplingGUID) AS n, Max(tblSampling.WtCatch) AS MaxCatch, Min(tblSampling.WtCatch) AS MinCatch,
                            Avg(tblSampling.WtCatch) AS AvgCatch FROM (tblAOI INNER JOIN tblLandingSites ON tblAOI.AOIGuid = tblLandingSites.AOIGuid)
                            INNER JOIN tblSampling ON tblLandingSites.LSGUID = tblSampling.LSGUID WHERE tblLandingSites.LSGUID={{{landingSiteGuid}}}
                            GROUP BY tblAOI.AOIName, tblLandingSites.LSName, tblSampling.FishingGround, Year([SamplingDate]) ";

                        if (notInclude1)
                        {
                            query += $"HAVING Count(tblSampling.SamplingGUID)>1 AND Year([SamplingDate]) In ({samplingYears})";
                        }
                        else
                        {
                            query += $"HAVING Year([SamplingDate]) In ({samplingYears})";
                        }
                    }
                    else
                    {
                        query = $@"SELECT tblAOI.AOIName, tblSampling.FishingGround, Year([SamplingDate]) AS samplingYear,
                            Count(tblSampling.SamplingGUID) AS n, Max(tblSampling.WtCatch) AS MaxCatch,
                            Min(tblSampling.WtCatch) AS MinCatch, Avg(tblSampling.WtCatch) AS AvgCatch
                            FROM tblAOI INNER JOIN tblSampling ON tblAOI.AOIGuid = tblSampling.AOI
                            GROUP BY tblAOI.AOIName, tblSampling.FishingGround, tblSampling.AOI, Year([SamplingDate]) ";

                        if (notInclude1)
                        {
                            query += $"HAVING tblSampling.AOI= {{{aoiGUID}}} AND Count(tblSampling.SamplingGUID)>1 AND Year([SamplingDate]) In ({samplingYears})";
                        }
                        else
                        {
                            query += $"HAVING tblSampling.AOI= {{{aoiGUID}}} AND Year([SamplingDate]) In ({samplingYears})";
                        }
                    }
                }
                else //not aggregated
                {
                    ifldEnumerator = sf.EditAddField("Enumerat", FieldType.STRING_FIELD, 1, 100);
                    sf.Field[ifldEnumerator].Alias = "Name of enumerator";

                    ifldLS = sf.EditAddField("LSName", FieldType.STRING_FIELD, 1, 255);
                    sf.Field[ifldLS].Alias = "Landing site name";

                    ifldGearClass = sf.EditAddField("GearClass", FieldType.STRING_FIELD, 1, 100);
                    sf.Field[ifldGearClass].Alias = "Gear class used";

                    ifldGear = sf.EditAddField("GearName", FieldType.STRING_FIELD, 1, 255);
                    sf.Field[ifldGear].Alias = "Gear variation used";

                    ifldYear = sf.EditAddField("Year", FieldType.INTEGER_FIELD, 1, 4);
                    sf.Field[ifldYear].Alias = "Year sampled";

                    ifldFG = sf.EditAddField("fg", FieldType.STRING_FIELD, 1, 25);
                    sf.Field[ifldFG].Alias = "Fishing ground";

                    ifldNumberHauls = sf.EditAddField("NoHauls", FieldType.INTEGER_FIELD, 1, 2);
                    sf.Field[ifldNumberHauls].Alias = "Number of hauls";

                    ifldNumberFisher = sf.EditAddField("NoFishers", FieldType.INTEGER_FIELD, 1, 2);
                    sf.Field[ifldNumberFisher].Alias = "Number of fishers";

                    ifldDateSet = sf.EditAddField("DateSet", FieldType.DATE_FIELD, 1, 2);
                    sf.Field[ifldDateSet].Alias = "Date gear set";

                    ifldTimeSet = sf.EditAddField("TimeSet", FieldType.DATE_FIELD, 1, 2);
                    sf.Field[ifldTimeSet].Alias = "Time gear set";

                    ifldDateHauled = sf.EditAddField("DateHaul", FieldType.DATE_FIELD, 1, 2);
                    sf.Field[ifldDateHauled].Alias = "Date gear hauled";

                    ifldTimeHauled = sf.EditAddField("TimeHaul", FieldType.DATE_FIELD, 1, 2);
                    sf.Field[ifldTimeHauled].Alias = "Time gear hauled";

                    ifldVessel = sf.EditAddField("VesType", FieldType.STRING_FIELD, 1, 30);
                    sf.Field[ifldVessel].Alias = "Type of vessel used";

                    ifldHP = sf.EditAddField("hp", FieldType.STRING_FIELD, 1, 2);
                    sf.Field[ifldHP].Alias = "Engine hp";

                    ifldCatchWt = sf.EditAddField("CatchWt", FieldType.DOUBLE_FIELD, 2, 8);
                    sf.Field[ifldCatchWt].Alias = "Catch weight";

                    query = @"SELECT tblAOI.AOIName, tblEnumerators.EnumeratorName, tblLandingSites.LSName, tblGearClass.GearClassName,
                            tblGearVariations.Variation, Year([SamplingDate]) AS samplingYear, tblSampling.FishingGround, tblSampling.NoHauls,
                            tblSampling.NoFishers, tblSampling.DateSet, tblSampling.TimeSet, tblSampling.DateHauled, tblSampling.TimeHauled,
                            tblSampling.VesType, tblSampling.hp, tblSampling.WtCatch FROM (tblAOI INNER JOIN tblLandingSites ON
                            tblAOI.AOIGuid = tblLandingSites.AOIGuid) INNER JOIN ((tblGearClass INNER JOIN tblGearVariations ON
                            tblGearClass.GearClass = tblGearVariations.GearClass) INNER JOIN (tblEnumerators RIGHT JOIN tblSampling ON
                            tblEnumerators.EnumeratorID = tblSampling.Enumerator) ON tblGearVariations.GearVarGUID = tblSampling.GearVarGUID) ON
                            tblLandingSites.LSGUID = tblSampling.LSGUID ";

                    if (gearVariationGuid.Length > 0)
                    {
                        query += $@"WHERE Year([SamplingDate]) In({samplingYears}) AND tblSampling.LSGUID ={{{landingSiteGuid}}}
                                AND tblSampling.GearVarGUID ={{{gearVariationGuid}}}";
                    }
                    else if (landingSiteGuid.Length > 0)
                    {
                        query += $"WHERE Year([SamplingDate]) In({samplingYears}) AND tblSampling.LSGUID ={{{landingSiteGuid}}}";
                    }
                    else
                    {
                        query += $"WHERE Year([SamplingDate]) In({samplingYears}) AND tblSampling.AOI ={{{aoiGUID}}}";
                    }
                }

                using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;data source=" + global.MDBPath))
                {
                    conection.Open();
                    using (var adapter = new OleDbDataAdapter(query, conection))
                    using (var myDT = new DataTable())
                    {
                        adapter.Fill(myDT);
                        //var n = 0;
                        foreach (DataRow dr in myDT.Rows)
                        {
                            var fg = dr["FishingGround"].ToString();
                            if (fg.Length > 0)
                            {
                                var shp = new MapWinGIS.Shape();

                                if (shp.Create(ShpfileType.SHP_POINT))
                                {
                                    var proceed = false;
                                    if (RemoveInland && !FishingGrid.MinorGridIsInland(fg))
                                    {
                                        proceed = true;
                                    }
                                    else if (!RemoveInland)
                                    {
                                        proceed = true;
                                    }

                                    if (proceed)
                                    {
                                        var iShp = 0;
                                        if (_geoProjection.IsGeographic)
                                        {
                                            var result = FishingGrid.Grid25ToLatLong(fg, utmZone);
                                            iShp = shp.AddPoint(result.longitude, result.latitude);
                                        }
                                        else
                                        {
                                            FishingGrid.Grid25_to_UTM(fg, out int x, out int y);
                                            iShp = shp.AddPoint(x, y);
                                        }
                                        if (iShp >= 0 && sf.EditInsertShape(shp, 0))
                                        {
                                            sf.EditCellValue(ifldAOI, iShp, dr["AOIName"].ToString());
                                            sf.EditCellValue(ifldYear, iShp, int.Parse(dr["samplingYear"].ToString()));
                                            sf.EditCellValue(ifldFG, iShp, dr["FishingGround"].ToString());

                                            //aggregated
                                            if (Aggregated)
                                            {
                                                if (landingSiteGuid.Length > 0)
                                                {
                                                    sf.EditCellValue(ifldLS, iShp, dr["LSName"].ToString());
                                                }
                                                if (gearVariationGuid.Length > 0)
                                                {
                                                    sf.EditCellValue(ifldGear, iShp, dr["Variation"].ToString());
                                                }
                                                sf.EditCellValue(ifldCount, iShp, int.Parse(dr["n"].ToString()));
                                                sf.EditCellValue(ifldMaxWt, iShp, double.Parse(dr["MaxCatch"].ToString()));
                                                sf.EditCellValue(ifldMinWt, iShp, double.Parse(dr["MinCatch"].ToString()));
                                                sf.EditCellValue(ifldAfgWt, iShp, double.Parse(dr["AvgCatch"].ToString()));
                                                Console.WriteLine($"n-{sf.CellValue[ifldCount, iShp]}, AvgCatch -{sf.CellValue[ifldAfgWt, iShp]}");
                                            }

                                            //not aggregated
                                            else
                                            {
                                                sf.EditCellValue(ifldEnumerator, iShp, dr["EnumeratorName"].ToString());
                                                sf.EditCellValue(ifldLS, iShp, dr["LSName"].ToString());
                                                sf.EditCellValue(ifldGearClass, iShp, dr["GearClassName"].ToString());
                                                sf.EditCellValue(ifldGear, iShp, dr["Variation"].ToString());

                                                if (dr["NoHauls"].ToString().Length > 0)
                                                    sf.EditCellValue(ifldNumberHauls, iShp, int.Parse(dr["NoHauls"].ToString()));

                                                if (dr["NoFishers"].ToString().Length > 0)
                                                    sf.EditCellValue(ifldNumberFisher, iShp, int.Parse(dr["NoFishers"].ToString()));

                                                if (dr["DateSet"].ToString().Length > 0)
                                                    sf.EditCellValue(ifldDateSet, iShp, DateTime.Parse(dr["DateSet"].ToString()));

                                                if (dr["TimeSet"].ToString().Length > 0)
                                                    sf.EditCellValue(ifldTimeSet, iShp, DateTime.Parse(dr["TimeSet"].ToString()));

                                                if (dr["DateHauled"].ToString().Length > 0)
                                                    sf.EditCellValue(ifldDateHauled, iShp, DateTime.Parse(dr["DateHauled"].ToString()));

                                                if (dr["TimeHauled"].ToString().Length > 0)
                                                    sf.EditCellValue(ifldTimeHauled, iShp, DateTime.Parse(dr["TimeHauled"].ToString()));

                                                if (dr["VesType"].ToString().Length > 0)
                                                {
                                                    var vesselType = "Motorized";
                                                    switch (int.Parse(dr["VesType"].ToString()))
                                                    {
                                                        case 1:
                                                            vesselType = "Motorized";
                                                            break;

                                                        case 2:
                                                            vesselType = "Non-motorized";
                                                            break;

                                                        case 3:
                                                            vesselType = "No vessel used";
                                                            break;

                                                        case 4:
                                                            vesselType = "Not provided";
                                                            break;
                                                    }
                                                    sf.EditCellValue(ifldVessel, iShp, vesselType);
                                                }

                                                if (dr["hp"].ToString().Length > 0)
                                                    sf.EditCellValue(ifldHP, iShp, dr["hp"].ToString());

                                                sf.EditCellValue(ifldCatchWt, iShp, double.Parse(dr["WtCatch"].ToString()));
                                            }
                                        }
                                    }
                                }
                            }

                            //n++;
                        }
                    }
                }
                MapLayersHandler.RemoveLayer("Fishing grounds");
                if (sf.NumShapes > 0)
                {
                    sf.GeoProjection = _geoProjection;
                    sf.CollisionMode = tkCollisionMode.AllowCollisions;
                    sf.DefaultDrawingOptions.PointShape = tkPointShapeType.ptShapeCircle;
                    sf.DefaultDrawingOptions.FillColor = new Utils().ColorByName(tkMapColor.Red);
                    sf.DefaultDrawingOptions.LineColor = new Utils().ColorByName(tkMapColor.White);
                    ClassificationType classificationType = ClassificationType.None;
                    if (Aggregated)
                    {
                        classificationType = ClassificationType.NaturalBreaks;
                        ShapefileLayerHelper.CategorizeNumericPointLayer(sf, ifldCount);
                    }
                    else
                    {
                        sf.DefaultDrawingOptions.PointSize = 7;
                    }
                    sf.SelectionAppearance = tkSelectionAppearance.saDrawingOptions;
                    sf.SelectionDrawingOptions.PointShape = tkPointShapeType.ptShapeCircle;

                    if (ShapefileLayerHelper.ExtentsPosition(MapControl.Extents, sf.Extents) == ExtentCompare.excoOutside)
                    {
                        var newExtent = MapControl.Extents;
                        newExtent.MoveTo(sf.Extents.Center.x, sf.Extents.Center.y);
                        MapControl.Extents = newExtent;
                    }
                    var h = MapLayersHandler.AddLayer(sf, "Fishing grounds", true, true);
                    MapLayersHandler[h].ClassificationType = classificationType;
                }
            }
        }
    }
}
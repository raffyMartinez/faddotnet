using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapWinGIS;
using System.Data.OleDb;
using System.Data;

namespace FAD3
{
    public class FishingGroundMappingHandler : IDisposable
    {
        private bool _disposed;
        private GeoProjection _geoProjection = new GeoProjection();
        public MapLayersHandler MapLayersHandler { get; set; }

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

        public bool MapFishingGround(string fishingGround, FishingGrid.fadUTMZone utmZone)
        {
            var sf = new Shapefile();
            bool success = false;
            if (sf.CreateNew("", ShpfileType.SHP_POINT))
            {
                var shp = new Shape();
                if (shp.Create(ShpfileType.SHP_POINT))
                {
                    var result = FishingGrid.Grid25ToLatLong(fishingGround, utmZone);
                    var iShp = shp.AddPoint(result.longitude, result.latitude);
                    if (iShp >= 0 && sf.EditInsertShape(shp, 0))
                    {
                        MapLayersHandler.RemoveLayer("Fishing ground");
                        sf.GeoProjection = _geoProjection;
                        success = MapLayersHandler.AddLayer(sf, "Fishing ground", true, true) >= 0;
                    }
                }
            }
            return success;
        }

        private void SetShapefileFields(Shapefile sf, bool isAggregated, bool HasLandingSite, bool HasGear)
        {
            if (isAggregated)
            {
                var ifldAOI = sf.EditAddField("AOIName", FieldType.STRING_FIELD, 1, 255);
                sf.Field[ifldAOI].Alias = "Target area name";
                var ifldLS = 0;
                if (HasLandingSite)
                {
                    ifldLS = sf.EditAddField("LSName", FieldType.STRING_FIELD, 1, 255);
                    sf.Field[ifldLS].Alias = "Landing site name";
                }
                var ifldGear = 0;
                if (HasGear)
                {
                    ifldGear = sf.EditAddField("GearName", FieldType.STRING_FIELD, 1, 255);
                    sf.Field[ifldGear].Alias = "Gear variation used";
                }
                var ifldYear = sf.EditAddField("Year", FieldType.INTEGER_FIELD, 1, 4);
                sf.Field[ifldYear].Alias = "Year sampled";
                var ifldFG = sf.EditAddField("fg", FieldType.STRING_FIELD, 1, 25);
                sf.Field[ifldFG].Alias = "Fishing ground";
                var ifldNumber = sf.EditAddField("n", FieldType.INTEGER_FIELD, 1, 4);
                var ifldMaxWt = sf.EditAddField("MaxWt", FieldType.DOUBLE_FIELD, 2, 8);
                sf.Field[ifldMaxWt].Alias = "Maximum catch weight";
                var ifldMinWt = sf.EditAddField("MinWt", FieldType.DOUBLE_FIELD, 2, 8);
                sf.Field[ifldMinWt].Alias = "Minimum catch weight";
                var ifldAfgWt = sf.EditAddField("AvgWt", FieldType.DOUBLE_FIELD, 2, 8);
                sf.Field[ifldAfgWt].Alias = "Average catch weight";
            }
            else
            {
            }
        }

        public void MapFishingGrounds(string aoiGUID, string samplingYears, FishingGrid.fadUTMZone utmZone, bool Aggregated = true, string landingSiteGuid = "", string gearVariationGuid = "")
        {
            var query = "";
            var sf = new Shapefile();
            var ifldAOI = 0;
            var ifldLS = 0;
            var ifldGear = 0;
            var ifldYear = 0;
            var ifldFG = 0;
            var ifldNumber = 0;
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

                    ifldNumber = sf.EditAddField("n", FieldType.INTEGER_FIELD, 1, 4);

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
                                GROUP BY tblAOI.AOIName, tblLandingSites.LSName, tblGearVariations.Variation, tblSampling.FishingGround, Year([SamplingDate])
                                HAVING Year([SamplingDate]) In ({samplingYears})";
                    }
                    else if (landingSiteGuid.Length > 0)
                    {
                        query = $@"SELECT tblAOI.AOIName, tblLandingSites.LSName, tblSampling.FishingGround, Year([SamplingDate]) AS samplingYear,
                            Count(tblSampling.SamplingGUID) AS n, Max(tblSampling.WtCatch) AS MaxCatch, Min(tblSampling.WtCatch) AS MinCatch,
                            Avg(tblSampling.WtCatch) AS AvgCatch FROM (tblAOI INNER JOIN tblLandingSites ON tblAOI.AOIGuid = tblLandingSites.AOIGuid)
                            INNER JOIN tblSampling ON tblLandingSites.LSGUID = tblSampling.LSGUID WHERE tblLandingSites.LSGUID={{{landingSiteGuid}}}
                            GROUP BY tblAOI.AOIName, tblLandingSites.LSName, tblSampling.FishingGround, Year([SamplingDate])
                            HAVING Year([SamplingDate]) In ({samplingYears})";
                    }
                    else
                    {
                        query = $@"SELECT tblAOI.AOIName, tblSampling.FishingGround, Year([SamplingDate]) AS samplingYear,
                            Count(tblSampling.SamplingGUID) AS n, Max(tblSampling.WtCatch) AS MaxCatch,
                            Min(tblSampling.WtCatch) AS MinCatch, Avg(tblSampling.WtCatch) AS AvgCatch
                            FROM tblAOI INNER JOIN tblSampling ON tblAOI.AOIGuid = tblSampling.AOI
                            GROUP BY tblAOI.AOIName, tblSampling.FishingGround, tblSampling.AOI, Year([SamplingDate])
                            HAVING tblSampling.AOI= {{{aoiGUID}}} AND Year([SamplingDate]) In ({samplingYears})";
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

                    query = @"SELECT tblAOI.AOIName, tblEnumerators.EnumeratorName, tblLandingSites.LSName,
                                tblGearClass.GearClassName, tblGearVariations.Variation, Year([SamplingDate]) AS samplingYear,
                                tblSampling.FishingGround, tblSampling.NoHauls, tblSampling.NoFishers, tblSampling.DateSet,
                                tblSampling.TimeSet, tblSampling.DateHauled, tblSampling.TimeHauled, tblSampling.VesType, tblSampling.hp,
                                tblSampling.WtCatch FROM tblGearClass INNER JOIN(tblEnumerators INNER JOIN (tblGearVariations
                                INNER JOIN ((tblAOI INNER JOIN tblLandingSites ON tblAOI.AOIGuid = tblLandingSites.AOIGuid)
                                INNER JOIN tblSampling ON tblLandingSites.LSGUID = tblSampling.LSGUID) ON
                                tblGearVariations.GearVarGUID = tblSampling.GearVarGUID) ON tblEnumerators.EnumeratorID = tblSampling.Enumerator)
                                ON tblGearClass.GearClass = tblGearVariations.GearClass ";

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

                using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;data source=" + global.mdbPath))
                {
                    conection.Open();
                    using (var adapter = new OleDbDataAdapter(query, conection))
                    using (var myDT = new DataTable())
                    {
                        adapter.Fill(myDT);
                        var n = 0;
                        foreach (DataRow dr in myDT.Rows)
                        {
                            var shp = new MapWinGIS.Shape();
                            if (shp.Create(ShpfileType.SHP_POINT))
                            {
                                var fg = dr["FishingGround"].ToString();
                                var result = FishingGrid.Grid25ToLatLong(fg, utmZone);
                                var iShp = shp.AddPoint(result.longitude, result.latitude);
                                if (iShp >= 0 && sf.EditInsertShape(shp, n))
                                {
                                    sf.EditCellValue(ifldAOI, n, dr["AOIName"].ToString());
                                    sf.EditCellValue(ifldYear, n, int.Parse(dr["samplingYear"].ToString()));
                                    sf.EditCellValue(ifldFG, n, dr["FishingGround"].ToString());

                                    //aggregated
                                    if (Aggregated)
                                    {
                                        if (landingSiteGuid.Length > 0)
                                        {
                                            sf.EditCellValue(ifldLS, n, dr["LSName"].ToString());
                                        }
                                        if (gearVariationGuid.Length > 0)
                                        {
                                            sf.EditCellValue(ifldGear, n, dr["Variation"].ToString());
                                        }
                                        sf.EditCellValue(ifldNumber, n, int.Parse(dr["n"].ToString()));
                                        sf.EditCellValue(ifldMaxWt, n, double.Parse(dr["MaxCatch"].ToString()));
                                        sf.EditCellValue(ifldMinWt, n, double.Parse(dr["MinCatch"].ToString()));
                                        sf.EditCellValue(ifldAfgWt, n, double.Parse(dr["AvgCatch"].ToString()));
                                    }

                                    //not aggregated
                                    else
                                    {
                                        sf.EditCellValue(ifldEnumerator, n, dr["EnumeratorName"].ToString());
                                        sf.EditCellValue(ifldLS, n, dr["LSName"].ToString());
                                        sf.EditCellValue(ifldGearClass, n, dr["GearClassName"].ToString());
                                        sf.EditCellValue(ifldGear, n, dr["Variation"].ToString());

                                        if (dr["NoHauls"].ToString().Length > 0)
                                            sf.EditCellValue(ifldNumberHauls, n, int.Parse(dr["NoHauls"].ToString()));

                                        if (dr["NoFishers"].ToString().Length > 0)
                                            sf.EditCellValue(ifldNumberFisher, n, int.Parse(dr["NoFishers"].ToString()));

                                        if (dr["DateSet"].ToString().Length > 0)
                                            sf.EditCellValue(ifldDateSet, n, DateTime.Parse(dr["DateSet"].ToString()));

                                        if (dr["TimeSet"].ToString().Length > 0)
                                            sf.EditCellValue(ifldTimeSet, n, DateTime.Parse(dr["TimeSet"].ToString()));

                                        if (dr["DateHauled"].ToString().Length > 0)
                                            sf.EditCellValue(ifldDateHauled, n, DateTime.Parse(dr["DateHauled"].ToString()));

                                        if (dr["TimeHauled"].ToString().Length > 0)
                                            sf.EditCellValue(ifldTimeHauled, n, DateTime.Parse(dr["TimeHauled"].ToString()));

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
                                            sf.EditCellValue(ifldVessel, n, vesselType);
                                        }

                                        if (dr["hp"].ToString().Length > 0)
                                            sf.EditCellValue(ifldHP, n, dr["hp"].ToString());

                                        sf.EditCellValue(ifldCatchWt, n, double.Parse(dr["WtCatch"].ToString()));
                                    }
                                }
                            }
                            n++;
                        }
                    }
                }
                MapLayersHandler.RemoveLayer("Fishing grounds");
                sf.GeoProjection = _geoProjection;
                MapLayersHandler.AddLayer(sf, "Fishing grounds", true, true);
            }
        }
    }
}
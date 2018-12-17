using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxMapWinGIS;
using MapWinGIS;
using FAD3.Mapping.Classes;
using FAD3.Database.Classes;
using System.Data;
using System.Data.OleDb;

namespace FAD3.Mapping.Classes
{
    public class OccurenceMapping
    {
        public OccurenceDataType DataType { get; internal set; }
        public string ItemName { get; internal set; }
        public AxMap MapControl { get; internal set; }
        public MapLayersHandler MapLayersHandler { get; set; }
        public MapInterActionHandler MapInteractionHandler { get; set; }
        public event EventHandler<OccurenceMapEventArgs> RequestOccurenceInfo;
        public bool MapInSelectedTargetArea { get; internal set; }
        public bool Aggregate { get; internal set; }
        public bool ExcludeOne { get; internal set; }
        public string SelectedTargetAreaGuid { get; internal set; }
        public string ItemToMapGuid { get; internal set; }
        public List<int> SamplingYears { get; internal set; }

        public OccurenceMapping(OccurenceDataType dataType, string itemName, AxMap mapControl)
        {
            DataType = dataType;
            ItemName = itemName;
            MapControl = mapControl;
        }

        public bool MapOccurence()
        {
            OccurenceMapEventArgs e = new OccurenceMapEventArgs(DataType);
            RequestOccurenceInfo?.Invoke(this, e);
            if (!e.Cancel)
            {
                MapInSelectedTargetArea = e.MapInSelectedTargetArea;
                Aggregate = e.Aggregate;
                ExcludeOne = e.ExcludeOne;
                SelectedTargetAreaGuid = e.SelectedTargetAreaGuid;
                ItemToMapGuid = e.ItemToMapGuid;
                SamplingYears = e.SamplingYears;
                switch (DataType)
                {
                    case OccurenceDataType.Species:
                        return SpeciesOccurenceMapping();

                    case OccurenceDataType.Gear:
                        return GearOccurenceMapping();
                }
            }
            return false;
        }

        private string YearsToCSV()
        {
            string sYears = "";
            foreach (var item in SamplingYears)
            {
                sYears += item + ",";
            }
            return sYears.Trim(',');
        }

        private bool SpeciesOccurenceMapping()
        {
            string sql = "";
            if (Aggregate)
            {
                if (MapInSelectedTargetArea)
                {
                    sql = $@"SELECT tblAOI.AOIName,
                                tblSampling.FishingGround,
                                Count(tblSampling.SamplingGUID) AS n,
                                tblAOI.UTMZone
                            FROM (tblSampling INNER JOIN
                                tblAOI ON
                                tblSampling.AOI = tblAOI.AOIGuid) INNER JOIN
                                tblCatchComp ON tblSampling.SamplingGUID = tblCatchComp.SamplingGUID
                            WHERE tblCatchComp.NameGUID = {{{ItemToMapGuid}}} AND
                                Year([SamplingDate]) In ({YearsToCSV()}) AND
                                tblSampling.FishingGround Is Not Null AND
                                tblAOI.UTMZone Is Not Null AND
                                tblAOI.AOIGuid={{{SelectedTargetAreaGuid}}}
                            GROUP BY tblAOI.AOIName,
                                tblSampling.FishingGround,
                                tblAOI.UTMZone";
                }
                else
                {
                    sql = $@"SELECT tblAOI.AOIName,
                              tblSampling.FishingGround,
                              Count(tblSampling.SamplingGUID) AS n,
                              tblAOI.UTMZone
                            FROM (tblSampling INNER JOIN
                              tblCatchComp ON
                              tblSampling.SamplingGUID = tblCatchComp.SamplingGUID) INNER JOIN
                              tblAOI ON tblSampling.AOI = tblAOI.AOIGuid
                            WHERE Year([SamplingDate]) In ({YearsToCSV()}) AND
                              tblSampling.FishingGround Is Not Null AND
                              tblAOI.UTMZone Is Not Null AND
                              tblCatchComp.NameGUID={{{ItemToMapGuid}}}
                            GROUP BY tblAOI.AOIName,
                              tblSampling.FishingGround,
                              tblAOI.UTMZone";
                }
            }
            else
            {
                if (MapInSelectedTargetArea)
                {
                    sql = $@"SELECT tblAOI.AOIName,
                                tblEnumerators.EnumeratorName,
                                tblSampling.*,
                                tblLandingSites.LSName,
                                tblGearClass.GearClassName,
                                tblGearVariations.Variation,
                                tblAOI.UTMZone
                            FROM tblEnumerators
                                RIGHT JOIN ((tblAOI INNER JOIN tblLandingSites ON
                                tblAOI.AOIGuid = tblLandingSites.AOIGuid) INNER JOIN
                                ((tblGearClass INNER JOIN tblGearVariations ON
                                tblGearClass.GearClass = tblGearVariations.GearClass) INNER JOIN
                                (tblSampling INNER JOIN tblCatchComp ON
                                tblSampling.SamplingGUID = tblCatchComp.SamplingGUID) ON
                                tblGearVariations.GearVarGUID = tblSampling.GearVarGUID) ON
                                tblLandingSites.LSGUID = tblSampling.LSGUID) ON
                                tblEnumerators.EnumeratorID = tblSampling.Enumerator
                            WHERE tblSampling.AOI={{{SelectedTargetAreaGuid}}} AND
                                tblAOI.UTMZone Is Not Null AND
                                tblCatchComp.NameGUID={{{ItemToMapGuid}}} AND
                                Year([SamplingDate]) In ({YearsToCSV()}) AND
                                tblSampling.FishingGround Is Not Null
                            ORDER BY tblAOI.AOIName,
                                tblGearClass.GearClassName,
                                tblGearVariations.Variation,
                                tblSampling.SamplingDate";
                }
                else
                {
                    sql = $@"SELECT tblAOI.AOIName,
                                tblEnumerators.EnumeratorName,
                                tblSampling.*,
                                tblLandingSites.LSName,
                                tblGearClass.GearClassName,
                                tblGearVariations.Variation,
                                tblAOI.UTMZone
                            FROM tblEnumerators
                                RIGHT JOIN ((tblAOI INNER JOIN tblLandingSites ON
                                tblAOI.AOIGuid = tblLandingSites.AOIGuid) INNER JOIN
                                ((tblGearClass INNER JOIN tblGearVariations ON
                                tblGearClass.GearClass = tblGearVariations.GearClass) INNER JOIN
                                (tblSampling INNER JOIN tblCatchComp ON
                                tblSampling.SamplingGUID = tblCatchComp.SamplingGUID) ON
                                tblGearVariations.GearVarGUID = tblSampling.GearVarGUID) ON
                                tblLandingSites.LSGUID = tblSampling.LSGUID) ON
                                tblEnumerators.EnumeratorID = tblSampling.Enumerator
                            WHERE tblCatchComp.NameGUID={{{ItemToMapGuid}}} AND
                                Year([SamplingDate]) In ({YearsToCSV()}) AND
                                tblSampling.FishingGround Is Not Null AND
                                tblAOI.UTMZone Is Not Null
                            ORDER BY tblAOI.AOIName,
                                tblGearClass.GearClassName,
                                tblGearVariations.Variation,
                                tblSampling.SamplingDate";
                }
            }
            try
            {
                var sf = new Shapefile();
                if (sf.CreateNew("", ShpfileType.SHP_POINT))
                {
                    sf.GeoProjection = MapControl.GeoProjection;
                    var ifldTargetArea = -1;
                    var ifldCount = -1;
                    var ifldFishingGround = -1;
                    var ifldSpeciesName = -1;
                    var ifldGearClass = -1;
                    var ifldGearVariation = -1;
                    var ifldCatchWt = -1;
                    var ifldReferenceNumber = -1;
                    var ifldSamplingDate = -1;
                    var ifldVesselType = -1;
                    var ifldLandindSite = -1;
                    var ifldEnumerator = -1;
                    if (Aggregate)
                    {
                        ifldTargetArea = sf.EditAddField("TargetArea", FieldType.STRING_FIELD, 1, 100);
                        ifldFishingGround = sf.EditAddField("FG", FieldType.STRING_FIELD, 1, 8);
                        ifldSpeciesName = sf.EditAddField("Species", FieldType.STRING_FIELD, 1, 100);
                        ifldCount = sf.EditAddField("n", FieldType.INTEGER_FIELD, 1, 4);
                    }
                    else
                    {
                        ifldReferenceNumber = sf.EditAddField("RefNo", FieldType.STRING_FIELD, 1, 20);
                        ifldEnumerator = sf.EditAddField("Enumerator", FieldType.STRING_FIELD, 1, 30);
                        ifldSpeciesName = sf.EditAddField("Species", FieldType.STRING_FIELD, 1, 100);
                        ifldTargetArea = sf.EditAddField("TargetArea", FieldType.STRING_FIELD, 1, 100);
                        ifldLandindSite = sf.EditAddField("LandingSite", FieldType.STRING_FIELD, 1, 50);
                        ifldSamplingDate = sf.EditAddField("SamplingDate", FieldType.DATE_FIELD, 1, 1);
                        ifldGearClass = sf.EditAddField("GearClass", FieldType.STRING_FIELD, 1, 25);
                        ifldGearVariation = sf.EditAddField("GearVariation", FieldType.STRING_FIELD, 1, 35);
                        ifldFishingGround = sf.EditAddField("FishingGround", FieldType.STRING_FIELD, 1, 8);
                        ifldCatchWt = sf.EditAddField("CatchWeight", FieldType.DATE_FIELD, 2, 10);
                        ifldVesselType = sf.EditAddField("Vessel", FieldType.STRING_FIELD, 1, 25);
                    }

                    using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
                    {
                        conn.Open();
                        var adapter = new OleDbDataAdapter(sql, conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            for (int n = 0; n < dt.Rows.Count; n++)
                            {
                                DataRow dr = dt.Rows[n];
                                var fg = dr["FishingGround"].ToString();
                                var zone = dr["UTMZone"].ToString();
                                fadUTMZone utmZone = FishingGrid.ZoneFromZoneName(zone);
                                FishingGrid.UTMZone = utmZone;
                                var latlong = FishingGrid.Grid25ToLatLong(fg, utmZone);
                                var shp = new Shape();
                                if (shp.Create(ShpfileType.SHP_POINT) && shp.AddPoint(latlong.longitude, latlong.latitude) >= 0)
                                {
                                    var iShp = sf.EditAddShape(shp);
                                    if (Aggregate)
                                    {
                                        if (sf.EditCellValue(ifldTargetArea, iShp, dr["AOIName"].ToString()))
                                        {
                                            sf.EditCellValue(ifldCount, iShp, (int)dr["n"]);
                                            sf.EditCellValue(ifldSpeciesName, iShp, ItemName);
                                            sf.EditCellValue(ifldFishingGround, iShp, fg);
                                        }
                                    }
                                    else
                                    {
                                        sf.EditCellValue(ifldSpeciesName, iShp, ItemName);
                                        sf.EditCellValue(ifldEnumerator, iShp, dr["EnumeratorName"].ToString());
                                        sf.EditCellValue(ifldTargetArea, iShp, dr["AOIName"].ToString());
                                        sf.EditCellValue(ifldFishingGround, iShp, fg);
                                        sf.EditCellValue(ifldGearClass, iShp, dr["GearClassName"].ToString());
                                        sf.EditCellValue(ifldGearVariation, iShp, dr["Variation"].ToString());
                                        sf.EditCellValue(ifldReferenceNumber, iShp, dr["RefNo"].ToString());
                                        sf.EditCellValue(ifldSamplingDate, iShp, string.Format("{0:MMM-dd-yyyy}", (DateTime)dr["SamplingDate"]));
                                        sf.EditCellValue(ifldCatchWt, iShp, (double)dr["WtCatch"]);
                                        sf.EditCellValue(ifldVesselType, iShp, FishingVessel.VesselTypeFromVesselTypeNumber((int)dr["VesType"]));
                                        sf.EditCellValue(ifldLandindSite, iShp, dr["LSName"].ToString());
                                    }
                                }
                            }
                        }
                    }
                    sf.DefaultDrawingOptions.PointShape = tkPointShapeType.ptShapeCircle;
                    if (Aggregate)
                    {
                        ShapefileLayerHelper.CategorizeNumericPointLayer(sf, ifldCount);
                        sf.DefaultDrawingOptions.LineVisible = true;
                        sf.DefaultDrawingOptions.LineColor = new Utils().ColorByName(tkMapColor.White);
                        sf.CollisionMode = tkCollisionMode.AllowCollisions;
                    }
                    else
                    {
                        sf.DefaultDrawingOptions.PointSize = 8;
                    }

                    MapLayersHandler.AddLayer(sf, $"Species mapping: {ItemName} ({YearsToCSV()})", mappingMode: fad3MappingMode.occurenceMappingSpeciesAggregated);
                    MapControl.Redraw();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "OcuurenceMapping", "SpeciesOccurence");
            }
            return true;
        }

        private bool GearOccurenceMapping()
        {
            string sql = "";
            if (Aggregate)
            {
                if (MapInSelectedTargetArea)
                {
                    sql = $@"SELECT tblGearClass.GearClassName,
                                tblGearVariations.Variation,
                                tblSampling.FishingGround,
                                Count(tblSampling.SamplingGUID) AS n,
                                tblAOI.AOIName,
                                tblAOI.UTMZone
                            FROM tblAOI INNER JOIN
                                ((tblGearClass INNER JOIN
                                tblGearVariations ON
                                tblGearClass.GearClass = tblGearVariations.GearClass) INNER JOIN
                                tblSampling ON tblGearVariations.GearVarGUID = tblSampling.GearVarGUID) ON
                                tblAOI.AOIGuid = tblSampling.AOI
                            WHERE tblGearVariations.GearVarGUID={{{ItemToMapGuid}}} AND
                                tblSampling.AOI={{{SelectedTargetAreaGuid}}} AND
                                tblAOI.UTMZone Is Not Null AND
                                Year([SamplingDate]) In ({YearsToCSV()}) AND
                                tblSampling.FishingGround Is Not Null
                            GROUP BY tblGearClass.GearClassName,
                                tblGearVariations.Variation,
                                tblSampling.FishingGround,
                                tblAOI.AOIName,
                                tblAOI.UTMZone";
                }
                else
                {
                    sql = $@"SELECT tblGearClass.GearClassName,
                                tblGearVariations.Variation,
                                tblSampling.FishingGround,
                                Count(tblSampling.SamplingGUID) AS n,
                                tblAOI.AOIName,
                                tblAOI.UTMZone
                        FROM tblAOI INNER JOIN
                            ((tblGearClass INNER JOIN
                            tblGearVariations ON
                            tblGearClass.GearClass = tblGearVariations.GearClass) INNER JOIN
                            tblSampling ON tblGearVariations.GearVarGUID = tblSampling.GearVarGUID) ON
                            tblAOI.AOIGuid = tblSampling.AOI
                        WHERE tblGearVariations.GearVarGUID={{{ItemToMapGuid}}} AND
                            tblAOI.UTMZone Is Not Null AND
                            Year([SamplingDate]) In ({YearsToCSV()}) AND
                            tblSampling.FishingGround Is Not Null
                        GROUP BY tblGearClass.GearClassName,
                            tblGearVariations.Variation,
                            tblSampling.FishingGround,
                            tblAOI.AOIName,
                            tblAOI.UTMZone";
                }
            }
            else
            {
                if (MapInSelectedTargetArea)
                {
                    sql = $@"SELECT tblAOI.AOIName,
                                tblGearClass.GearClassName,
                                tblGearVariations.Variation,
                                tblLandingSites.LSName,
                                tblEnumerators.EnumeratorName,
                                tblSampling.*,
                                tblAOI.UTMZone
                            FROM (tblAOI INNER JOIN
                                tblLandingSites ON
                                tblAOI.AOIGuid = tblLandingSites.AOIGuid) INNER JOIN
                                ((tblGearClass INNER JOIN
                                tblGearVariations ON
                                tblGearClass.GearClass = tblGearVariations.GearClass) INNER JOIN
                                (tblEnumerators RIGHT JOIN
                                tblSampling ON
                                tblEnumerators.EnumeratorID = tblSampling.Enumerator) ON
                                tblGearVariations.GearVarGUID = tblSampling.GearVarGUID) ON
                                tblLandingSites.LSGUID = tblSampling.LSGUID
                            WHERE tblGearVariations.GearVarGUID={{{ItemToMapGuid}}} AND
                                tblSampling.AOI={{{SelectedTargetAreaGuid}}} AND
                                tblAOI.UTMZone Is Not Null AND
                                    Year([SamplingDate]) In ({YearsToCSV()}) AND tblSampling.FishingGround Is Not Null
                            ORDER BY tblAOI.AOIName,
                                tblGearClass.GearClassName,
                                tblGearVariations.Variation,
                                tblSampling.SamplingDate";
                }
                else
                {
                    sql = $@"SELECT tblAOI.AOIName,
                                tblGearClass.GearClassName,
                                tblGearVariations.Variation,
                                tblLandingSites.LSName,
                                tblEnumerators.EnumeratorName,
                                tblSampling.*,
                                tblAOI.UTMZone
                            FROM (tblAOI INNER JOIN
                                tblLandingSites ON
                                tblAOI.AOIGuid = tblLandingSites.AOIGuid) INNER JOIN
                                ((tblGearClass INNER JOIN
                                tblGearVariations ON
                                tblGearClass.GearClass = tblGearVariations.GearClass) INNER JOIN
                                (tblEnumerators RIGHT JOIN
                                tblSampling ON
                                tblEnumerators.EnumeratorID = tblSampling.Enumerator) ON
                                tblGearVariations.GearVarGUID = tblSampling.GearVarGUID) ON
                                tblLandingSites.LSGUID = tblSampling.LSGUID
                            WHERE tblGearVariations.GearVarGUID={{{ItemToMapGuid}}} AND
                                tblAOI.UTMZone Is Not Null AND
                                Year([SamplingDate]) In ({YearsToCSV()}) AND tblSampling.FishingGround Is Not Null
                            ORDER BY tblAOI.AOIName,
                                tblGearClass.GearClassName,
                                tblGearVariations.Variation,
                                tblSampling.SamplingDate";
                }
            }
            try
            {
                var sf = new Shapefile();
                if (sf.CreateNew("", ShpfileType.SHP_POINT))
                {
                    sf.GeoProjection = MapControl.GeoProjection;
                    var ifldTargetArea = -1;
                    var ifldCount = -1;
                    var ifldFishingGround = -1;
                    var ifldGearClass = -1;
                    var ifldGearVariation = -1;
                    var ifldCatchWt = -1;
                    var ifldReferenceNumber = -1;
                    var ifldSamplingDate = -1;
                    var ifldVesselType = -1;
                    var ifldLandindSite = -1;
                    var ifldEnumerator = -1;
                    if (Aggregate)
                    {
                        ifldTargetArea = sf.EditAddField("TargetArea", FieldType.STRING_FIELD, 1, 100);
                        ifldGearClass = sf.EditAddField("GearClass", FieldType.STRING_FIELD, 1, 25);
                        ifldGearVariation = sf.EditAddField("GearVariation", FieldType.STRING_FIELD, 1, 100);
                        ifldFishingGround = sf.EditAddField("FG", FieldType.STRING_FIELD, 1, 8);
                        ifldCount = sf.EditAddField("n", FieldType.INTEGER_FIELD, 1, 4);
                    }
                    else
                    {
                        ifldReferenceNumber = sf.EditAddField("RefNo", FieldType.STRING_FIELD, 1, 20);
                        ifldTargetArea = sf.EditAddField("TargetArea", FieldType.STRING_FIELD, 1, 100);
                        ifldLandindSite = sf.EditAddField("LandingSite", FieldType.STRING_FIELD, 1, 50);
                        ifldSamplingDate = sf.EditAddField("SamplingDate", FieldType.DATE_FIELD, 1, 1);
                        ifldGearClass = sf.EditAddField("GearClass", FieldType.STRING_FIELD, 1, 25);
                        ifldGearVariation = sf.EditAddField("GearVariation", FieldType.STRING_FIELD, 1, 35);
                        ifldFishingGround = sf.EditAddField("FishingGround", FieldType.STRING_FIELD, 1, 8);
                        ifldCatchWt = sf.EditAddField("CatchWeight", FieldType.DATE_FIELD, 2, 10);
                        ifldVesselType = sf.EditAddField("Vessel", FieldType.STRING_FIELD, 1, 25);
                        ifldEnumerator = sf.EditAddField("Enumerator", FieldType.STRING_FIELD, 1, 30);
                    }

                    using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
                    {
                        conn.Open();
                        var adapter = new OleDbDataAdapter(sql, conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            for (int n = 0; n < dt.Rows.Count; n++)
                            {
                                DataRow dr = dt.Rows[n];
                                var fg = dr["FishingGround"].ToString();
                                var zone = dr["UTMZone"].ToString();
                                fadUTMZone utmZone = FishingGrid.ZoneFromZoneName(zone);
                                FishingGrid.UTMZone = utmZone;
                                var latlong = FishingGrid.Grid25ToLatLong(fg, utmZone);
                                var shp = new Shape();
                                if (shp.Create(ShpfileType.SHP_POINT) && shp.AddPoint(latlong.longitude, latlong.latitude) >= 0)
                                {
                                    var iShp = sf.EditAddShape(shp);
                                    if (Aggregate)
                                    {
                                        if (sf.EditCellValue(ifldTargetArea, iShp, dr["AOIName"].ToString()))
                                        {
                                            sf.EditCellValue(ifldCount, iShp, (int)dr["n"]);
                                            sf.EditCellValue(ifldGearClass, iShp, dr["GearClassName"].ToString());
                                            sf.EditCellValue(ifldGearVariation, iShp, ItemName);
                                            sf.EditCellValue(ifldFishingGround, iShp, fg);
                                        }
                                    }
                                    else
                                    {
                                        sf.EditCellValue(ifldTargetArea, iShp, dr["AOIName"].ToString());
                                        sf.EditCellValue(ifldFishingGround, iShp, fg);
                                        sf.EditCellValue(ifldGearClass, iShp, dr["GearClassName"].ToString());
                                        sf.EditCellValue(ifldGearVariation, iShp, dr["Variation"].ToString());
                                        sf.EditCellValue(ifldReferenceNumber, iShp, dr["RefNo"].ToString());
                                        sf.EditCellValue(ifldEnumerator, iShp, dr["EnumeratorName"].ToString());
                                        sf.EditCellValue(ifldSamplingDate, iShp, string.Format("{0:MMM-dd-yyyy}", (DateTime)dr["SamplingDate"]));
                                        sf.EditCellValue(ifldCatchWt, iShp, (double)dr["WtCatch"]);
                                        sf.EditCellValue(ifldVesselType, iShp, FishingVessel.VesselTypeFromVesselTypeNumber((int)dr["VesType"]));
                                        sf.EditCellValue(ifldLandindSite, iShp, dr["LSName"].ToString());
                                    }
                                }
                            }
                        }
                    }
                    sf.DefaultDrawingOptions.PointShape = tkPointShapeType.ptShapeCircle;
                    if (Aggregate)
                    {
                        ShapefileLayerHelper.CategorizeNumericPointLayer(sf, ifldCount);
                        sf.DefaultDrawingOptions.LineVisible = true;
                        sf.DefaultDrawingOptions.LineColor = new Utils().ColorByName(tkMapColor.White);
                        sf.CollisionMode = tkCollisionMode.AllowCollisions;
                    }
                    else
                    {
                        sf.DefaultDrawingOptions.PointSize = 8;
                    }

                    MapLayersHandler.AddLayer(sf, $"Gear mapping: {ItemName} ({YearsToCSV()})", mappingMode: fad3MappingMode.occurenceMappingGearAggregated);
                    MapControl.Redraw();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "OcuurenceMapping", "SpeciesOccurence");
            }
            return true;
        }
    }
}
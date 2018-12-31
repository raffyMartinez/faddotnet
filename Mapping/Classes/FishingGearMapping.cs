using FAD3.Database.Classes;
using MapWinGIS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Reflection;

namespace FAD3.Mapping.Classes
{
    public static class FishingGearMapping
    {
        public static string SaveMapFolder { get; set; }

        public static Shapefile MapThisGear(string aoiGuid, string gearVarGuid, List<int> samplingYear, bool aggregate = false, bool notInclude1 = false, bool RemoveInland = false)
        {
            var query = "";
            var dt = new DataTable();
            var sf = new Shapefile();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    var years = "";
                    foreach (var item in samplingYear)
                    {
                        years += $"{item},";
                    }
                    years = years.Trim(',');

                    if (aggregate)
                    {
                        if (!notInclude1)
                        {
                            query = $@"SELECT tblSampling.FishingGround, Count(tblSampling.SamplingGUID) AS n FROM tblSampling
                                GROUP BY tblSampling.AOI, tblSampling.GearVarGUID, Year([SamplingDate]), tblSampling.FishingGround
                                HAVING tblSampling.AOI={{{aoiGuid}}} AND tblSampling.GearVarGUID= {{{gearVarGuid}}} AND Year([SamplingDate]) In ({years})";
                        }
                        else
                        {
                            query = $@"SELECT tblSampling.FishingGround, Count(tblSampling.SamplingGUID) AS n FROM tblSampling
                                    GROUP BY tblSampling.FishingGround, tblSampling.AOI, tblSampling.GearVarGUID, Year([SamplingDate])
                                    HAVING Count(tblSampling.SamplingGUID) > 1 AND tblSampling.AOI = {{{aoiGuid}}} AND tblSampling.GearVarGUID = {{{gearVarGuid}}}
                                    AND Year([SamplingDate]) In ({years})";
                        }
                    }
                    else
                    {
                        query = $@"SELECT tblGearClass.GearClassName, tblGearVariations.Variation, tblAOI.AOIName, tblSampling.RefNo, tblSampling.SamplingDate,
                                  tblSampling.SamplingTime, tblSampling.FishingGround, tblSampling.TimeSet, tblSampling.DateSet, tblSampling.TimeHauled,
                                  tblSampling.DateHauled, tblSampling.NoHauls, tblSampling.NoFishers, tblSampling.Engine, tblSampling.hp, tblSampling.WtCatch,
                                  tblSampling.WtSample, tblLandingSites.LSName, temp_VesselType.VesselType FROM tblGearClass INNER JOIN
                                  (tblGearVariations INNER JOIN (((tblAOI INNER JOIN tblLandingSites ON tblAOI.AOIGuid = tblLandingSites.AOIGuid) INNER JOIN
                                  tblSampling ON tblLandingSites.LSGUID = tblSampling.LSGUID) INNER JOIN temp_VesselType ON tblSampling.VesType = temp_VesselType.VesselTypeNo)
                                  ON tblGearVariations.GearVarGUID = tblSampling.GearVarGUID) ON tblGearClass.GearClass = tblGearVariations.GearClass
                                  WHERE tblSampling.AOI= {{{aoiGuid}}} AND tblSampling.GearVarGUID= {{{gearVarGuid}}} AND Year([SamplingDate]) In ({years})";
                    }
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);

                    var fishingGround = "";
                    var iShp = 0;
                    var pointAdded = false;
                    var iFldFG = 0;
                    var iFLdCount = 0;
                    fadUTMZone utmZone = FishingGrid.UTMZone;
                    if (sf.CreateNewWithShapeID("", ShpfileType.SHP_POINT))
                    {
                        sf.GeoProjection = global.MappingForm.GeoProjection;

                        if (aggregate)
                        {
                            iFldFG = sf.EditAddField("fishingGround", FieldType.STRING_FIELD, 1, 9);
                            iFLdCount = sf.EditAddField("n", FieldType.INTEGER_FIELD, 1, 1);
                        }
                        else
                        {
                        }

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow dr = dt.Rows[i];
                            fishingGround = dr["FishingGround"].ToString();
                            var proceed = false;
                            if (RemoveInland && !FishingGrid.MinorGridIsInland(fishingGround))
                            {
                                proceed = true;
                            }
                            else if (!RemoveInland)
                            {
                                proceed = true;
                            }

                            if (proceed)
                            {
                                var shp = new Shape();
                                if (shp.Create(ShpfileType.SHP_POINT))
                                {
                                    if (sf.GeoProjection.IsGeographic)
                                    {
                                        var result = FishingGrid.Grid25ToLatLong(fishingGround, utmZone);
                                        pointAdded = shp.AddPoint(result.longitude, result.latitude) >= 0;
                                    }
                                    else
                                    {
                                    }

                                    if (pointAdded)
                                    {
                                        iShp = sf.EditAddShape(shp);
                                        if (iShp >= 0)
                                        {
                                            if (aggregate)
                                            {
                                                sf.EditCellValue(iFldFG, iShp, fishingGround);
                                                sf.EditCellValue(iFLdCount, iShp, (int)dr["n"]);
                                            }
                                            else
                                            {
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }
            if (sf.NumShapes >= 0)
            {
                return sf;
            }
            else
            {
                return null;
            }
        }
    }
}
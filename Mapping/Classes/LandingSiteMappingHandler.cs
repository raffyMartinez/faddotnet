using FAD3.Database.Classes;
using MapWinGIS;
using Oware;
using System;
using System.Data;
using System.Data.OleDb;
using System.Reflection;

namespace FAD3
{
    public static class LandingSiteMappingHandler
    {
        public static bool ShowLandingSitesOnMap(MapLayersHandler layersHandler, string aoiGUID, GeoProjection gp, ref string layerName, bool uniqueLayerName = false)
        {
            layerName = "Landing sites";
            var myDT = new DataTable();
            var iShp = -1;
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT Municipality, ProvinceName, LSName, cx, cy
                                    FROM Provinces INNER JOIN (Municipalities INNER JOIN tblLandingSites ON Municipalities.MunNo = tblLandingSites.MunNo)
                                    ON Provinces.ProvNo = Municipalities.ProvNo
                                    WHERE tblLandingSites.AOIGuid={{{aoiGUID}}}";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(myDT);
                    if (myDT.Rows.Count > 0)
                    {
                        var sf = new Shapefile();
                        if (sf.CreateNewWithShapeID("", ShpfileType.SHP_POINT))
                        {
                            var ifldName = sf.EditAddField("Name", FieldType.STRING_FIELD, 1, 50);
                            var ifldLGU = sf.EditAddField("LGU", FieldType.STRING_FIELD, 1, 50);
                            var ifldX = sf.EditAddField("x", FieldType.DOUBLE_FIELD, 8, 12);
                            var ifldY = sf.EditAddField("y", FieldType.DOUBLE_FIELD, 8, 12);
                            sf.GeoProjection = gp;

                            for (int i = 0; i < myDT.Rows.Count; i++)
                            {
                                DataRow dr = myDT.Rows[i];
                                if (dr["cx"].ToString().Length > 0 && dr["cy"].ToString().Length > 0)
                                {
                                    var x = (double)dr["cx"];
                                    var y = (double)dr["cy"];
                                    var name = dr["LSName"].ToString();
                                    var LGU = $"{dr["Municipality"].ToString()}, {dr["ProvinceName"].ToString()}";
                                    var shp = new Shape();
                                    if (shp.Create(ShpfileType.SHP_POINT))
                                    {
                                        if (global.MappingMode == fad3MappingMode.grid25Mode)
                                        {
                                            var converter = new LatLngUTMConverter("WGS 84");
                                            var result = converter.convertLatLngToUtm(y, x);
                                            x = result.Easting;
                                            y = result.Northing;
                                        }

                                        shp.AddPoint(x, y);

                                        iShp = sf.EditAddShape(shp);
                                        if (iShp >= 0)
                                        {
                                            sf.EditCellValue(ifldName, iShp, name);
                                            sf.EditCellValue(ifldLGU, iShp, LGU);
                                            sf.EditCellValue(ifldX, iShp, x);
                                            sf.EditCellValue(ifldY, iShp, y);
                                        }
                                    }
                                }
                            }
                            sf.DefaultDrawingOptions.PointShape = tkPointShapeType.ptShapeCircle;
                            sf.DefaultDrawingOptions.FillColor = new Utils().ColorByName(tkMapColor.Red);
                            sf.DefaultDrawingOptions.PointSize = 7;
                            sf.DefaultDrawingOptions.LineVisible = false;
                            if (sf.Labels.Generate("[Name]", tkLabelPositioning.lpCenter, false) > 0)
                            {
                                sf.Labels.FontSize = 7;
                                sf.Labels.FontBold = true;
                                sf.Labels.FrameVisible = false;
                            }
                            if (iShp >= 0)
                            {
                                layersHandler.AddLayer(sf, layerName, true, uniqueLayerName);
                            }
                        }
                    }
                }
                catch (Exception ex) { Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name); }
            }

            return iShp >= 0;
        }
    }
}
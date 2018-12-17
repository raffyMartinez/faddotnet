using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapWinGIS;
using FAD3.Database.Classes;

namespace FAD3
{
    public static class ShapefileLayerHelper
    {
        public static void CategorizeNumericPointLayer(Shapefile sf, int classificationField = 1,
                                                       tkClassificationType Method = tkClassificationType.ctNaturalBreaks,
                                                       int numberOfClasses = 5,
                                                       int maximumPointSize = 28)
        {
            float ptSize = 0;
            if (sf.Categories.Generate(classificationField, Method, numberOfClasses))
            {
                for (int n = 0; n < sf.Categories.Count; n++)
                {
                    var category = sf.Categories.Item[n];
                    ptSize = maximumPointSize * ((float)(n + 1) / sf.Categories.Count);
                    category.DrawingOptions.PointSize = ptSize;
                    category.DrawingOptions.LineColor = new Utils().ColorByName(tkMapColor.White);
                }
            }
        }

        /// <summary>
        /// Creates a new shapefile from a point shapefile where each point is located in the center of a grid25 cell. All fields in the source point shapefile are copied to the new shapefile
        /// </summary>
        /// <param name="pointShapefile"></param>
        /// <param name="utmZone"></param>
        /// <returns></returns>
        public static Shapefile ConvertToGrid25(Shapefile pointShapefile, fadUTMZone utmZone,
            fad3ActionType inlandAction = fad3ActionType.atIgnore,
            fad3ActionType outsideMapAction = fad3ActionType.atIgnore,
            bool includeCoordinates = false)
        {
            var sf = new Shapefile();
            var listFields = new List<string>();
            var zoneName = string.Empty;
            if (sf.CreateNewWithShapeID("", ShpfileType.SHP_POINT))
            {
                var gp = new GeoProjection();
                switch (utmZone)
                {
                    case fadUTMZone.utmZone50N:
                        gp.SetWgs84Projection(tkWgs84Projection.Wgs84_UTM_zone_50N);
                        zoneName = "50N";
                        break;

                    case fadUTMZone.utmZone51N:
                        gp.SetWgs84Projection(tkWgs84Projection.Wgs84_UTM_zone_51N);
                        zoneName = "51N";
                        break;
                }

                sf.GeoProjection = gp;

                //recreate all fields from source to destination
                for (int f = 0; f < pointShapefile.NumFields; f++)
                {
                    listFields.Add(pointShapefile.Field[f].Name);
                    sf.EditAddField(listFields[f], pointShapefile.Field[f].Type, pointShapefile.Field[f].Precision, pointShapefile.Field[f].Width);
                }
                var ifldGrid = sf.EditAddField("grid25", FieldType.STRING_FIELD, 1, 8);

                var ifldInlandAction = -1;
                if (inlandAction == fad3ActionType.atTakeNote)
                {
                    ifldInlandAction = sf.EditAddField("isInland", FieldType.BOOLEAN_FIELD, 1, 1);
                }

                var ifldOutsideAction = -1;
                if (outsideMapAction == fad3ActionType.atTakeNote)
                {
                    ifldOutsideAction = sf.EditAddField("isOutsid", FieldType.BOOLEAN_FIELD, 1, 1);
                }

                //create fields for coordinate data
                var ifldCoordinatesX = -1;
                var ifldCoordinatesY = -1;
                if (includeCoordinates)
                {
                    ifldCoordinatesX = sf.EditAddField("Grid25X", FieldType.INTEGER_FIELD, 0, 8);
                    ifldCoordinatesY = sf.EditAddField("Grid25Y", FieldType.INTEGER_FIELD, 0, 8);
                }

                for (int n = 0; n < pointShapefile.NumShapes; n++)
                {
                    var shp = new Shape();
                    if (shp.Create(ShpfileType.SHP_POINT))
                    {
                        //get the x,y coordinates of the source point shape
                        var x = pointShapefile.Shape[n].Point[0].x;
                        var y = pointShapefile.Shape[n].Point[0].y;

                        //call the function that returns the coordinates of the grid center where the point is located
                        var result = FishingGrid.utmCoordinatesToGrid25(x, y, utmZone);

                        var removeInland = false;
                        var isInland = false;
                        if (inlandAction != fad3ActionType.atIgnore)
                        {
                            isInland = FishingGrid.MinorGridIsInland(result.grid25Name, zoneName);
                            removeInland = isInland && inlandAction == fad3ActionType.atRemove;
                        }

                        if (!removeInland)
                        {
                            //create a new point shape and add it to the destination shapefile
                            shp.AddPoint(result.Easting, result.Northing);
                            var iShp = sf.EditAddShape(shp);

                            //update the destination shapefile fields
                            foreach (var item in listFields)
                            {
                                var ifldDestination = sf.FieldIndexByName[item];
                                var ifldSource = pointShapefile.FieldIndexByName[item];
                                sf.EditCellValue(ifldDestination, iShp, pointShapefile.CellValue[ifldSource, n]);
                            }
                            sf.EditCellValue(ifldGrid, iShp, result.grid25Name);

                            //update coordinate fields if required
                            if (includeCoordinates)
                            {
                                sf.EditCellValue(ifldCoordinatesX, iShp, result.Easting);
                                sf.EditCellValue(ifldCoordinatesY, iShp, result.Northing);
                            }

                            //update isInland field if required
                            if (ifldInlandAction >= 0)
                            {
                                sf.EditCellValue(ifldInlandAction, iShp, isInland);
                            }
                        }
                    }
                }

                return sf;
            }
            return null;
        }

        public static ShpfileType ShapefileType2D(MapWinGIS.ShpfileType shpType)
        {
            if (shpType == ShpfileType.SHP_POLYGON || shpType == ShpfileType.SHP_POLYGONM || shpType == ShpfileType.SHP_POLYGONZ)
            {
                return ShpfileType.SHP_POLYGON;
            }
            else if (shpType == ShpfileType.SHP_POLYLINE || shpType == ShpfileType.SHP_POLYLINEM || shpType == ShpfileType.SHP_POLYLINEZ)
            {
                return ShpfileType.SHP_POLYLINE;
            }
            else if (shpType == ShpfileType.SHP_POINT || shpType == ShpfileType.SHP_POINTM || shpType == ShpfileType.SHP_POINTZ ||
                     shpType == ShpfileType.SHP_MULTIPOINT || shpType == ShpfileType.SHP_MULTIPOINTM || shpType == ShpfileType.SHP_MULTIPOINTZ)
            {
                return ShpfileType.SHP_POINT;
            }
            else
            {
                return ShpfileType.SHP_NULLSHAPE;
            }
        }

        public static ExtentCompare ExtentsPosition(Extents ext1, Extents ext2)
        {
            ExtentCompare exco = ExtentCompare.excoSimilar;
            var pointInside = false;
            ext1.GetBounds(out double xMin1, out double yMin1, out double zMin1, out double xMax1, out double yMax1, out double zMax1);
            ext2.GetBounds(out double xMin2, out double yMin2, out double zMin2, out double xMax2, out double yMax2, out double zMax2);

            if (xMax1 == xMax2 && yMax1 == yMax2 && xMin1 == xMin2 && yMin1 == yMin2)
            {
                exco = ExtentCompare.excoSimilar;
            }
            else if (xMax1 > xMax2 && xMin1 < xMin2 && yMax1 > yMax2 && yMin1 < yMin2)
            {
                exco = ExtentCompare.excoInside;
            }
            else
            {
                pointInside = xMin2 > xMin1 && xMin2 < xMax1 && yMax2 < yMax1 && yMax2 > yMin1;
                if (!pointInside)
                {
                    pointInside = xMax2 > xMin1 && xMax2 < xMax1 && yMax2 < yMax1 && yMax2 > yMin1;
                }
                if (!pointInside)
                {
                    pointInside = xMin2 > xMin1 && xMin2 < xMax1 && yMin2 < yMax1 && yMin2 > yMin1;
                }
                if (!pointInside)
                {
                    pointInside = xMax2 > xMin1 && xMax2 < xMax1 && yMax2 < yMax1 && yMin2 > yMin1;
                }

                if (pointInside)
                {
                    exco = ExtentCompare.excoCrossing;
                }
                else
                {
                    exco = ExtentCompare.excoOutside;
                }
            }
            return exco;
        }
    }
}
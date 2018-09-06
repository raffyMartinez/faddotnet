using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapWinGIS;

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

        public static global.ExtentCompare ExtentsPosition(Extents ext1, Extents ext2)
        {
            global.ExtentCompare exco = global.ExtentCompare.excoSimilar;
            var pointInside = false;
            ext1.GetBounds(out double xMin1, out double yMin1, out double zMin1, out double xMax1, out double yMax1, out double zMax1);
            ext2.GetBounds(out double xMin2, out double yMin2, out double zMin2, out double xMax2, out double yMax2, out double zMax2);

            if (xMax1 == xMax2 && yMax1 == yMax2 && xMin1 == xMin2 && yMin1 == yMin2)
            {
                exco = global.ExtentCompare.excoSimilar;
            }
            else if (xMax1 > xMax2 && xMin1 < xMin2 && yMax1 > yMax2 && yMin1 < yMin2)
            {
                exco = global.ExtentCompare.excoInside;
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
                    exco = global.ExtentCompare.excoCrossing;
                }
                else
                {
                    exco = global.ExtentCompare.excoOutside;
                }
            }
            return exco;
        }
    }
}
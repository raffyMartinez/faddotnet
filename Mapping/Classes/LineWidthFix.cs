using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapWinGIS;

namespace FAD3.Mapping
{
    public static class LineWidthFix
    {
        public static void FixLineWidth(ShapeDrawingOptions options)
        {
            if (options.LineWidth == 1F)
            {
                options.LineWidth = 1.1F;
            }
        }
    }
}
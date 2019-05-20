using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MapWinGIS;
using AxMapWinGIS;
using FAD3.Database.Classes;

namespace FAD3.Mapping.Classes
{
    internal class LegendFrame
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public double hDrawLayer { get; set; }
        public string Caption { get; set; }
        public Font Font { get; set; }
        public MapWinGIS.Point TopLeftCorner { get; set; }
        public int Padding { get; set; }
        public string LegendCornerPosition { get; set; }
        public MapLegendPosition MapLegendPosition { get; set; }
        public MapWinGIS.Point LegendAnchorCorner { get; set; }
        private AxMap _mapControl;
        private int _hLegend;
        public uint BorderColor { get; set; }
        public Dictionary<int, LegendLabel> LegendLabels { get; set; }

        public int AddLayerLabels(LegendLabel label)
        {
            return 0;
        }

        public LegendFrame(string caption, int hLegend, AxMap mapControl)
        {
            Padding = 20;
            Caption = caption;
            _hLegend = hLegend;
            _mapControl = mapControl;
            Font = new Font("Arial", 20, FontStyle.Bold, GraphicsUnit.Pixel);
        }

        public bool SetUpFrame()
        {
            double[] x = new double[5];
            double[] y = new double[5];
            switch (MapLegendPosition)
            {
                case MapLegendPosition.PositionFromCorner:
                    switch (LegendCornerPosition)
                    {
                        case "topLeft":
                            x[0] = LegendAnchorCorner.x;
                            y[0] = LegendAnchorCorner.y;

                            x[1] = LegendAnchorCorner.x + Width;
                            y[1] = LegendAnchorCorner.y;

                            x[2] = LegendAnchorCorner.x + Width;
                            y[2] = LegendAnchorCorner.y + 100;

                            x[3] = LegendAnchorCorner.x;
                            y[3] = LegendAnchorCorner.y + 100;

                            x[4] = LegendAnchorCorner.x;
                            y[4] = LegendAnchorCorner.y;
                            break;

                        case "topRight":

                            break;

                        case "bottomLeft":

                            break;

                        case "bottomRight":

                            break;
                    }
                    break;

                case MapLegendPosition.PositionFromDefinedExtent:
                    break;
            }
            object xObj = x;
            object yObj = y;
            _mapControl.DrawWidePolygonEx(_hLegend, ref xObj, ref yObj, 5, new Utils().ColorByName(tkMapColor.White), true, 2);
            _mapControl.DrawWidePolygonEx(_hLegend, ref xObj, ref yObj, 5, BorderColor, false, 2);
            MapWinGIS.Point topLeft = GetFrameTopLeft(y, x);

            var lbls = _mapControl.get_DrawingLabels(_hLegend);
            var cat = lbls.AddCategory("Legend");
            cat.FontBold = true;
            cat.FrameVisible = false;
            cat.FontSize = 12;
            cat.Alignment = tkLabelAlignment.laCenterRight;
            if (lbls != null)
            {
                lbls.AddLabel(Caption, topLeft.x + Padding, topLeft.y + Padding, 0, 0);
            }

            return x.Length > 0 && y.Length > 0;
        }

        private MapWinGIS.Point GetFrameTopLeft(double[] y, double[] x)
        {
            MapWinGIS.Point pt = new MapWinGIS.Point();
            double top = 0; double left = 0;
            for (int n = 0; n < y.Length; n++)
            {
                if (n == 0)
                {
                    top = y[n];
                }
                else
                {
                    if (y[n] < top)
                    {
                        top = y[n];
                    }
                }
            }

            for (int n = 0; n < x.Length; n++)
            {
                if (n == 0)
                {
                    left = x[n];
                }
                else
                {
                    if (x[n] < left)
                    {
                        left = x[n];
                    }
                }
            }
            pt.y = top;
            pt.x = LegendAnchorCorner.x;
            return pt;
        }
    }
}
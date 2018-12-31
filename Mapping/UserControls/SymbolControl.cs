using System;
using System.Collections.Generic;
using System.Drawing;

namespace FAD3.Mapping.UserControls
{
    internal partial class SymbolControl : ListControl
    {
        private List<MapWinGIS.ShapeDrawingOptions> _icons = new List<MapWinGIS.ShapeDrawingOptions>();

        public SymbolControl()
        {
            InitializeComponent();
            this.ItemCount = 17;
            this.CellWidth = 24;
            this.CellHeight = 24;

            for (int i = 0; i < this.ItemCount; i++)
            {
                MapWinGIS.ShapeDrawingOptions sdo = new MapWinGIS.ShapeDrawingOptions();
                sdo.SetDefaultPointSymbol((MapWinGIS.tkDefaultPointSymbol)i);
                sdo.PointSize = 0.8f * this.CellWidth;
                sdo.FillColor = Colors.ColorToUInteger(Color.Orange);
                _icons.Add(sdo);
            }
            this.OnDrawItem += new OnDrawItemDelegate(PointSymbolControl_OnDrawItem);
        }

        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                for (int i = 0; i < _icons.Count; i++)
                {
                    _icons[i].FillColor = Colors.ColorToUInteger(value);
                }
                base.Redraw();
            }
        }

        private void PointSymbolControl_OnDrawItem(System.Drawing.Graphics graphics, System.Drawing.RectangleF rect, int itemIndex, bool selected)
        {
            IntPtr ptr = graphics.GetHdc();
            MapWinGIS.ShapeDrawingOptions sdo = _icons[itemIndex];
            sdo.DrawPoint(ptr, rect.X + 1.0f, rect.Y + 1.0f, (int)rect.Width - 2, (int)rect.Height - 2, Colors.ColorToUInteger(this.BackColor));
            graphics.ReleaseHdc();
        }
    }
}
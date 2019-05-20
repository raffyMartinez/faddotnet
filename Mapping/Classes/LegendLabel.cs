using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAD3.Mapping.Classes
{
    public class LegendLabel
    {
        public string Caption { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public int LayerHandle { get; set; }
        public List<string> CategoryLabels { get; set; }

        public LegendLabel(int layerHandle, string caption, double width)
        {
            Caption = caption;
            Width = width;
        }
    }
}
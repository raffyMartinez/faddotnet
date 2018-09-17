using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FAD3
{
    public class CreateInlandGridEventArgs : EventArgs
    {
        public string Status { get; set; }
        public string StatusDescription { get; set; }
        public string LandShapefileFileName { get; set; }
        public int LandShapeFileCurrentShapeHandle { get; set; }
        public string LandShapeFileCurrentShapeName { get; set; }
        public int GridsIntersected { get; set; }
        public int GridsWithin { get; set; }
        public int GridsCreated { get; set; }
        public int GridCount { get; set; }
    }
}
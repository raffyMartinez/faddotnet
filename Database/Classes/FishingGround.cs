using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISO_Classes;

namespace FAD3.Database.Classes
{
    public class FishingGround
    {
        public string GridName { get; set; }
        public int? SubGrid { get; set; }
        public Coordinate Coordinate { get; set; }

        public FishingGround(Coordinate coordinate)
        {
            Coordinate = coordinate;
        }

        public FishingGround(string gridName, int? subGrid = null)
        {
            GridName = gridName;
            SubGrid = subGrid;
        }
    }
}
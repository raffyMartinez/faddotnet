using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISO_Classes;

namespace FAD3.Mapping.Classes
{
    public class LocationCoordinate
    {
        public string BarangayName { get; set; }
        public int LGUNumber { get; set; }
        public string LGUName { get; set; }
        public Coordinate Coordinate { get; set; }
        public string ProvinceName { get; set; }
        public string LGULevel { get; set; }
        public bool HasCoordinate { get; set; }

        public LocationCoordinate(string provinceName, string lguName, int lguNumber, Coordinate coordinate, string lguLevel)
        {
            ProvinceName = provinceName;
            LGUName = lguName;
            LGUNumber = lguNumber;
            Coordinate = coordinate;
            LGULevel = lguLevel;
        }
    }
}
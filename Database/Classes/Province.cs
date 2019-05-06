using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAD3.Database.Classes
{
    public class Province
    {
        public int ProvinceNumber { get; set; }
        public string ProvinceName { get; set; }
        public int RegionNumber { get; set; }

        public Province(int provNumber, string provName, int provRegion)
        {
            ProvinceNumber = provNumber;
            ProvinceName = provName;
            RegionNumber = provRegion;
        }
    }
}
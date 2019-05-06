using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAD3.Database.Classes
{
    public class LGU
    {
        public int ProvinceNumber { get; set; }
        public int MunicipalityNumber { get; set; }
        public string MunicipalityName { get; set; }
        public double xCoord { get; set; }
        public double yCoord { get; set; }
        public bool IsCoastal { get; set; }
        public int WRIAdminID { get; set; }

        public LGU(int provinceNumber, int municipalityNumber, string municipalityName,
            double x, double y, bool isCoastal, int wriID)
        {
            ProvinceNumber = provinceNumber;
            MunicipalityNumber = municipalityNumber;
            MunicipalityName = municipalityName;
            xCoord = x;
            yCoord = y;
            IsCoastal = isCoastal;
            WRIAdminID = wriID;
        }
    }
}
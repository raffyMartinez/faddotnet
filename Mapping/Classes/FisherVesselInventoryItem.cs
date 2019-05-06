using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAD3.Mapping.Classes
{
    public class FisherVesselInventoryItem
    {
        public double X { get; set; }
        public double Y { get; set; }
        public string Municipality { get; set; }
        public string Barangay { get; set; }
        public string Province { get; set; }
        public string InventoryProjectName { get; set; }
        public string ProvinceName { get; set; }
        public int CountCommercial { get; set; }
        public int CountMunicipalMotorized { get; set; }
        public int CountMunicipalNonMotorized { get; set; }
        public int CountFisher { get; set; }

        public FisherVesselInventoryItem(double x, double y, string inventoryProjectName, string provinceName)
        {
            X = x;
            Y = y;
            InventoryProjectName = inventoryProjectName;
            ProvinceName = provinceName;
        }
    }
}
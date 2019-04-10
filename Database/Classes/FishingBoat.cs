using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAD3.Database.Classes
{
    public class FishingBoat
    {
        public Guid BoatGuid { get; set; }
        public string BoatName { get; set; }
        public string OwnerName { get; set; }
        public Landingsite LandingSite { get; set; }
        public string Engine { get; set; }
        public double? EngineHp { get; set; }
        public double? BoatWidth { get; set; }
        public double? BoatHeight { get; set; }
        public double? BoatLength { get; set; }

        public FishingBoat(Guid boatGuid, Landingsite landingSite, string boatName, string ownerName="")
        {
            BoatGuid = boatGuid;
            LandingSite = landingSite;
            BoatName = boatName;
            OwnerName = ownerName;
        }
        public void Dimension(double width, double height, double length)
        {
            BoatWidth = width;
            BoatLength = length;
            BoatHeight = height;
        }

        public double? GrossTonnage
        {
            get
            {
                if (BoatWidth != null
                    && BoatLength != null
                    && BoatHeight != null)
                {
                    return 0;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}

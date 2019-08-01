using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAD3.Database.Classes
{
    public class TreeLevel
    {
        public string TargetAreaGuid { get; set; }
        public string TargetAreaName { get; set; }
        public string GearVariationGuid { get; set; }
        public string GearVariationName { get; set; }
        public string LandingSiteGuid { get; set; }
        public string LandingSiteName { get; set; }
        public string MonthYear { get; set; }

        public TreeLevel(string targetAreaGuid, string targetAreaName, string landingSiteGuid = "",
            string landingSiteName = "", string gearVariationGuid = "", string gearVariationName = "", string monthYear = "")
        {
            TargetAreaGuid = targetAreaGuid;
            TargetAreaName = targetAreaName;
            LandingSiteGuid = landingSiteGuid;
            LandingSiteName = landingSiteName;
            GearVariationGuid = gearVariationGuid;
            GearVariationName = gearVariationName;
            MonthYear = monthYear;
        }

        public string GetNodeName(string level)
        {
            string nodeName = "";
            switch (level)
            {
                case "gear":
                    nodeName = $"{LandingSiteGuid}|{GearVariationGuid}";
                    break;

                case "landing_site":
                    //nodeName = $"{LandingSiteGuid}|{GearVariationGuid}";
                    break;

                case "sampling":
                    nodeName = $"{LandingSiteGuid}|{GearVariationGuid}|{MonthYear}";
                    break;
            }

            return nodeName;
        }
    }
}
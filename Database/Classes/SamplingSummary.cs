using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAD3.Database.Classes
{
    public class SamplingSummary
    {
        public string TargetAreaName { get; set; }
        public string LandingSiteName { get; set; }
        public string GearClassName { get; set; }
        public string GearClassGuid { get; set; }
        public string EnumeratorName { get; set; }
        public int CatchCompositionRows { get; internal set; }
        public string GearSpecsIndicator { get; internal set; }
        public string FirstFishingGround { get; internal set; }
        public string GearVariationName { get; set; }
        public int? SubGrid { get; set; }
        public Sampling ParentSampling { get; internal set; }
        public string OperatingExpenseIndicator { get; set; }

        public SamplingSummary(Sampling parent, int catchCompositionRows, string firstFishingGround)
        {
            ParentSampling = parent;
            CatchCompositionRows = catchCompositionRows;
            FirstFishingGround = firstFishingGround;
        }

        public SamplingSummary(Sampling parent)
        {
            ParentSampling = parent;
        }
    }
}
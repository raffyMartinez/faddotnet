using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FAD3.Database.Classes;

namespace FAD3.Database.Classes
{
    public class Sampling
    {
        public string TargetAreaGuid { get; set; }
        public string SamplingGUID { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime? DateEncoded { get; set; }
        public DateTime SamplingDateTime { get; set; }
        public DateTime? GearSettingDateTime { get; set; }
        public DateTime? GearHaulingDateTime { get; set; }

        //public List<(string FishingGround, string SubGrid)> FishingGrounds { get; set; }
        public string LandingSiteGuid { get; set; }

        public string GearVariationGuid { get; set; }
        public double? CatchWeight { get; set; }
        public double? SampleWeight { get; set; }
        public string EnumeratorGuid { get; set; }
        public int? NumberOfHauls { get; set; }
        public int? NumberOfFishers { get; set; }
        public Dictionary<string, CatchLine> CatchComposition { get; set; }
        public List<FishingGround> FishingGroundList { get; set; }
        public CatchMonitoringSamplingType SamplingType { get; set; }

        public string Notes { get; set; }

        public bool HasLiveFish { get; set; }

        public FishingVessel FishingVessel { get; set; }

        public Sampling()
        {
        }

        public Sampling(string samplingGUID, DateTime samplingDateTime, string landingSiteGuid, string referenceNumber)
        {
            SamplingGUID = samplingGUID;
            SamplingDateTime = samplingDateTime;
            LandingSiteGuid = landingSiteGuid;
            ReferenceNumber = referenceNumber;
        }
    }
}
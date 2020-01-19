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
        public fad3DataStatus DataStatus { get; set; }
        public SamplingSummary SamplingSummary { get; set; }
        public string TargetAreaGuid { get; set; }
        public string SamplingGUID { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime? DateEncoded { get; set; }
        public DateTime SamplingDateTime { get; set; }
        public DateTime? GearSettingDateTime { get; set; }
        public DateTime? GearHaulingDateTime { get; set; }
        public TargetArea TargetArea { get; set; }

        public ExpensePerOperation ExpensePerOperation { get; set; }

        //public List<(string FishingGround, string SubGrid)> FishingGrounds { get; set; }
        public string LandingSiteGuid { get; set; }

        public string GearClassName { get; set; }
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
        public bool IsNew { get; set; }
        public FishingVessel FishingVessel { get; set; }

        public void ClearFishingGroundList()
        {
            if (FishingGroundList.Count > 0)
            {
                FishingGroundList.Clear();
            }
        }

        public Sampling()
        {
            SamplingSummary = new SamplingSummary(this);
        }

        public void AddFishingGround(FishingGround fg)
        {
            if (FishingGroundList == null)
            {
                FishingGroundList = new List<FishingGround>();
            }
            if (!FishingGroundList.Contains(fg))
            {
                FishingGroundList.Add(fg);
            }
        }

        public int? FirstSubGrid
        {
            get
            {
                int? subGrid = null; ;
                if (FishingGroundList != null && FishingGroundList.Count > 0 && FishingGroundList[0].SubGrid != null)
                {
                    subGrid = FishingGroundList[0].SubGrid;
                }
                return subGrid;
            }
        }

        public string FirstFishingGround

        {
            get
            {
                string fg = "";
                if (FishingGroundList != null && FishingGroundList.Count > 0)
                {
                    fg = FishingGroundList[0].GridName;
                    if (FishingGroundList[0].SubGrid != null)
                    {
                        fg += $"-{FishingGroundList[0].SubGrid.ToString()}";
                    }
                }
                return fg;
            }
        }

        public string AdditionalFishingGrounds
        {
            get
            {
                string fg = "";
                int n = 0;
                if (FishingGroundList.Count > 1)
                {
                    foreach (FishingGround item in FishingGroundList)
                    {
                        if (n > 0)
                        {
                            string sub = item.SubGrid == null ? "" : $"-{ item.SubGrid.ToString()}";
                            fg += $"{item.GridName}{sub}, ";
                        }
                        n++;
                    }
                }
                return fg.Trim(',', ' ', '-');
            }
        }

        public Sampling(string targetAreaGuid, string samplingGUID, DateTime samplingDateTime, string landingSiteGuid, string referenceNumber)
        {
            TargetAreaGuid = targetAreaGuid;
            SamplingGUID = samplingGUID;
            SamplingDateTime = samplingDateTime;
            LandingSiteGuid = landingSiteGuid;
            ReferenceNumber = referenceNumber;
        }
    }
}
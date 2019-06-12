using FAD3.Database.Classes;

namespace FAD3
{
    public class CatchLine
    {
        public string CatchCompGUID { get; set; }
        public string CatchName { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string CatchNameGUID { get; set; }

        public fad3DataStatus dataStatus { get; set; }

        public double CatchWeight { get; set; }

        public string SamplingGUID { get; set; }
        public int? CatchCount { get; set; }
        public string CatchDetailRowGUID { get; set; }
        public int? CatchSubsampleCount { get; set; }
        public double? CatchSubsampleWt { get; set; }
        public bool FromTotalCatch { get; set; }
        public bool LiveFish { get; set; }
        public Identification NameType { get; set; }
        public int? TaxaNumber { get; set; }
        public int Sequence { get; set; }
        public int? FishBaseSpeciesNumber { get; set; }
        public bool IsListedFB { get; set; }

        public CatchLine(string nameGUID, string name1, string name2, Identification nameType, int? taxaNumber, int? fishBaseSPNo, bool isListedFB = false)
        {
            CatchNameGUID = nameGUID;
            Name1 = name1;
            Name2 = name2;
            NameType = nameType;
            TaxaNumber = taxaNumber;
            FishBaseSpeciesNumber = fishBaseSPNo;
            IsListedFB = isListedFB;
        }

        public CatchLine(string inSamplingGUID)
        {
            SamplingGUID = inSamplingGUID;
        }

        public CatchLine(int sequence, string name1, string name2, string catchName, string samplingGUID,
                              string catchCompGuid, string catchNameGuid,
                              double catchWeight, int? catchCount = null,
                              int? taxaNumber = null)
        {
            Sequence = sequence;
            Name1 = name1;
            Name2 = name2;
            CatchName = catchName;
            SamplingGUID = samplingGUID;
            CatchCompGUID = catchCompGuid;
            CatchNameGUID = catchNameGuid;
            CatchWeight = catchWeight;
            CatchCount = catchCount;
            CatchSubsampleWt = 0;
            CatchSubsampleCount = 0;
            FromTotalCatch = false;
            NameType = Identification.Scientific;
            LiveFish = false;
            CatchDetailRowGUID = "";
            TaxaNumber = taxaNumber;
            dataStatus = fad3DataStatus.statusFromDB;
        }

        public CatchLine(int sequence, string name1, string name2, string catchName, string samplingGuid,
                              string catchLineGuid, string catchNameGuid,
                              double catchWeight, int? taxaNumber = null)
        {
            Sequence = sequence;
            Name1 = name1;
            Name2 = name2;
            CatchName = catchName;
            SamplingGUID = samplingGuid;
            CatchCompGUID = catchLineGuid;
            CatchNameGUID = catchNameGuid;
            CatchWeight = catchWeight;
            CatchCount = null;
            CatchSubsampleWt = 0;
            CatchSubsampleCount = 0;
            FromTotalCatch = false;
            NameType = Identification.Scientific;
            LiveFish = false;
            CatchDetailRowGUID = "";
            TaxaNumber = taxaNumber;
            dataStatus = fad3DataStatus.statusFromDB;
        }
    }
}
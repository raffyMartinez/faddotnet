using FAD3.Database.Classes;

namespace FAD3
{
    public class GMSLineClass
    {
        public string RowGuid { get; set; }
        public string CatchRowGUID { get; set; }
        public FishCrabGMS GMS { get; set; }
        public double? GonadWeight { get; set; }
        public double? Length { get; set; }
        public Sex Sex { get; set; }
        public Taxa Taxa { get; set; }
        public string TaxaName { get; set; }
        public double? Weight { get; set; }
        public fad3DataStatus DataStatus { get; set; }
        public string SamplingGuid { get; set; }
        public int Sequence { get; set; }

        public GMSLineClass(string inCatchRowGUID)
        {
            CatchRowGUID = inCatchRowGUID;
        }

        public GMSLineClass(string inCatchRowGuid, string inRowGuid, double? inLength, double? inWeight,
                            double? inGonadWeight, Sex inSex, FishCrabGMS inGMS, string inTaxaName,
                            Taxa inTaxa, fad3DataStatus inDataStatus, int inSequence)
        {
            RowGuid = inRowGuid;
            CatchRowGUID = inCatchRowGuid;
            Length = inLength;
            Weight = inWeight;
            GonadWeight = inGonadWeight;
            Sex = inSex;
            GMS = inGMS;
            TaxaName = inTaxaName;
            Taxa = inTaxa;
            DataStatus = inDataStatus;
            Sequence = inSequence;
        }
    }
}
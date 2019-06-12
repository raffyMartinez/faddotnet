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
        public int GMSNumeric { get; set; }
        public string TaxaName { get; set; }
        public double? Weight { get; set; }
        public fad3DataStatus DataStatus { get; set; }
        public string SamplingGuid { get; set; }
        public int Sequence { get; set; }

        public GMSLineClass(string catchRowGuid)
        {
            CatchRowGUID = catchRowGuid;
        }

        public GMSLineClass(string catchRowGuid, string rowGuid, double? length, double? weight,
                            double? gonadWeight, Sex sex, FishCrabGMS gms, string taxaName,
                            Taxa taxa, fad3DataStatus dataStatus, int sequence)
        {
            RowGuid = rowGuid;
            CatchRowGUID = catchRowGuid;
            Length = length;
            Weight = weight;
            GonadWeight = gonadWeight;
            Sex = sex;
            GMS = gms;
            TaxaName = taxaName;
            Taxa = taxa;
            DataStatus = dataStatus;
            Sequence = sequence;
            GMSNumeric = (int)gms;
        }
    }
}
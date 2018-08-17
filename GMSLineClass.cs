using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FAD3
{
    public class GMSLineClass
    {
        public string RowGuid { get; set; }
        public string CatchRowGUID { get; set; }
        public GMSManager.FishCrabGMS GMS { get; set; }
        public double? GonadWeight { get; set; }
        public double? Length { get; set; }
        public GMSManager.sex Sex { get; set; }
        public GMSManager.Taxa Taxa { get; set; }
        public string TaxaName { get; set; }
        public double? Weight { get; set; }
        public global.fad3DataStatus DataStatus { get; set; }
        public string SamplingGuid { get; set; }
        public int Sequence { get; set; }

        public GMSLineClass(string inCatchRowGUID)
        {
            CatchRowGUID = inCatchRowGUID;
        }

        public GMSLineClass(string inCatchRowGuid, string inRowGuid, double? inLength, double? inWeight,
                            double? inGonadWeight, GMSManager.sex inSex, GMSManager.FishCrabGMS inGMS, string inTaxaName,
                            GMSManager.Taxa inTaxa, global.fad3DataStatus inDataStatus, int inSequence)
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public fad3DataStatus dataStatus
        {
            get;
            set;
        }

        public double CatchWeight
        {
            get;
            set;
        }

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

        public CatchLine(string inSamplingGUID)
        {
            SamplingGUID = inSamplingGUID;
        }

        public CatchLine(int inSequence, string inName1, string inName2, string inCatchName, string inSamplingGUID,
                              string inCatchCompGUID, string inCatchNameGUID,
                              double inCatchWeight, int? inCatchCount = null,
                              int? inTaxaNumber = null)
        {
            Sequence = inSequence;
            Name1 = inName1;
            Name2 = inName2;
            CatchName = inCatchName;
            SamplingGUID = inSamplingGUID;
            CatchCompGUID = inCatchCompGUID;
            CatchNameGUID = inCatchNameGUID;
            CatchWeight = inCatchWeight;
            CatchCount = inCatchCount;
            CatchSubsampleWt = 0;
            CatchSubsampleCount = 0;
            FromTotalCatch = false;
            NameType = Identification.Scientific;
            LiveFish = false;
            CatchDetailRowGUID = "";
            TaxaNumber = inTaxaNumber;
            dataStatus = fad3DataStatus.statusFromDB;
        }

        public CatchLine(int inSequence, string inName1, string inName2, string inCatchName, string inSamplingGUID,
                              string inCatchLineGUID, string inCatchNameGUID,
                              double inCatchWeight, int? inTaxaNumber = null)
        {
            Sequence = inSequence;
            Name1 = inName1;
            Name2 = inName2;
            CatchName = inCatchName;
            SamplingGUID = inSamplingGUID;
            CatchCompGUID = inCatchLineGUID;
            CatchNameGUID = inCatchNameGUID;
            CatchWeight = inCatchWeight;
            CatchCount = null;
            CatchSubsampleWt = 0;
            CatchSubsampleCount = 0;
            FromTotalCatch = false;
            NameType = Identification.Scientific;
            LiveFish = false;
            CatchDetailRowGUID = "";
            TaxaNumber = inTaxaNumber;
            dataStatus = fad3DataStatus.statusFromDB;
        }
    }
}
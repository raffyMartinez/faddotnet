using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAD3.Database.Classes
{
    public class LengthFreqItem
    {
        public int Freq { get; set; }
        public float Length { get; set; }
        public string CatchCompRowGuid { get; set; }

        public LengthFreqItem(int freq, float length, string catchCompRowGuid)
        {
            Length = length;
            Freq = freq;
            CatchCompRowGuid = catchCompRowGuid;
        }
    }
}
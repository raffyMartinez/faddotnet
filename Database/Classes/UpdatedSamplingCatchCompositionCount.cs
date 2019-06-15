using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAD3.Database.Classes
{
    public class UpdatedSamplingCatchCompositionCount
    {
        public string SamplingGuid { get; set; }
        public int CatchRowsCount { get; set; }

        public UpdatedSamplingCatchCompositionCount(string samplingGuid, int catchRowsCount)
        {
            SamplingGuid = samplingGuid;
            CatchRowsCount = catchRowsCount;
        }
    }
}
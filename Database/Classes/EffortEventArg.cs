using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAD3.Database.Classes
{
    public class EffortEventArg : EventArgs
    {
        public DateTime SamplingDate { get; internal set; }
        public string GearVarGuid { get; internal set; }
        public string LandingSiteGuid { get; internal set; }

        public EffortEventArg(DateTime samplingDate, string gearVarGuid, string landingSiteGuid)
        {
            SamplingDate = samplingDate;
            GearVarGuid = gearVarGuid;
            LandingSiteGuid = landingSiteGuid;
        }
    }
}
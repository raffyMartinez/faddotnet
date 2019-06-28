using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAD3.Database.Classes
{
    public class SamplingEventArgs : EventArgs
    {
        public string ReferenceNumber { get; set; }
        public string Status { get; set; }
        public string StatusMessage { get; set; }
        public SamplingRecordStatus RecordStatus { get; set; }
        public int RecordCount { get; set; }
        public string CatchName { get; set; }
        public string GearVariationName { get; set; }
        public string GearLocalName { get; set; }
        public string TargetAreaName { get; set; }
        public string TargetAreaGuid { get; set; }
        public SamplingToFromXML SamplingToFromXML { get; set; }
        public string NewTargetAreaName { get; set; } = "";
        public string NewTargetAreaGuid { get; set; } = "";
        public bool UseExistingTargetArea { get; set; }

        public SamplingEventArgs(string status)
        {
            Status = status;
        }

        public SamplingEventArgs(SamplingRecordStatus status)
        {
            RecordStatus = status;
        }

        public SamplingEventArgs(SamplingRecordStatus status, string statusMessage, int importCount)
        {
            RecordStatus = status;
            StatusMessage = statusMessage;
            RecordCount = importCount;
        }

        public SamplingEventArgs(string status, string refNumber)
        {
            Status = status;
            ReferenceNumber = refNumber;
        }

        public SamplingEventArgs(SamplingRecordStatus status, int count)
        {
            RecordStatus = status;
            RecordCount = count;
        }

        public SamplingEventArgs(SamplingRecordStatus status, string refNumber)
        {
            RecordStatus = status;
            ReferenceNumber = refNumber;
        }

        public SamplingEventArgs(SamplingRecordStatus status, string name1, string name2)
        {
            RecordStatus = status;
            CatchName = $"{name1} {name2}";
        }
    }
}
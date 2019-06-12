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
        public ExportSamplingStatus ExportStatus { get; set; }
        public int RecordCount { get; set; }
        public string CatchName { get; set; }
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

        public SamplingEventArgs(ExportSamplingStatus status)
        {
            ExportStatus = status;
        }

        public SamplingEventArgs(ExportSamplingStatus status, string statusMessage, int importCount)
        {
            ExportStatus = status;
            StatusMessage = statusMessage;
            RecordCount = importCount;
        }

        public SamplingEventArgs(string status, string refNumber)
        {
            Status = status;
            ReferenceNumber = refNumber;
        }

        public SamplingEventArgs(ExportSamplingStatus status, int count)
        {
            ExportStatus = status;
            RecordCount = count;
        }

        public SamplingEventArgs(ExportSamplingStatus status, string refNumber)
        {
            ExportStatus = status;
            ReferenceNumber = refNumber;
        }

        public SamplingEventArgs(ExportSamplingStatus status, string name1, string name2)
        {
            ExportStatus = status;
            CatchName = $"{name1} {name2}";
        }
    }
}
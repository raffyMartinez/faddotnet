using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAD3.Mapping.Classes
{
    public class ParseCSVEventArgs : EventArgs
    {
        public int TimeField { get; internal set; }
        public int LatitudeField { get; internal set; }
        public int LongitudeField { get; internal set; }
        public string[] Fields { get; internal set; }
        public bool ProceedRead { get; internal set; }
        public string TimePeriodProcessed { get; internal set; }
        public int CoordinatesRead { get; internal set; }
        public bool FinishedRead { get; internal set; }
        public string StatusText { get; internal set; }

        public void SetColumns(int latitudeColumn, int longitudeColumn)
        {
            LatitudeField = latitudeColumn;
            LongitudeField = longitudeColumn;
            ProceedRead = true;
        }

        public ParseCSVEventArgs(int timeField, int latField, int lonField)
        {
            TimeField = timeField;
            LatitudeField = latField;
            LongitudeField = lonField;
        }

        public ParseCSVEventArgs(string[] fields)
        {
            Fields = fields;
        }

        public ParseCSVEventArgs(int coordinatesRead, string timePeriod)
        {
            CoordinatesRead = coordinatesRead;
            TimePeriodProcessed = timePeriod;
        }

        public ParseCSVEventArgs(string timePeriod, bool finishRead)
        {
            TimePeriodProcessed = timePeriod;
            FinishedRead = finishRead;
        }

        public ParseCSVEventArgs(string statusText)
        {
            StatusText = statusText;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAD3.Mapping.Classes
{
    public class ERDDAPMetadataReadEventArgs : EventArgs
    {
        public string DataTitle { get; internal set; }
        public string DataAbstract { get; internal set; }
        public DateTime BeginPosition { get; internal set; }
        public DateTime EndPosition { get; internal set; }
        public string FileIdentifier { get; internal set; }
        public string URL { get; internal set; }
        public Dictionary<string, (string unit, string description)> DataParameters { get; internal set; }
        public string Credit { get; internal set; }
        public int TemporalSize { get; internal set; }
        public double RowSize { get; set; }
        public double ColumnSize { get; set; }
        public string LegalConstraints { get; set; }
        public double NorthBound { get; internal set; }
        public double SouthBound { get; internal set; }
        public double EastBound { get; internal set; }
        public double WestBound { get; internal set; }
        public Dictionary<string, (string name, int size, double spacing)> Dimensions { get; set; }

        public void SetBounds(double west, double east, double north, double south)
        {
            NorthBound = north;
            SouthBound = south;
            EastBound = east;
            WestBound = west;
        }

        public ERDDAPMetadataReadEventArgs(string dataTitle, string dataAbstract, DateTime beginPosition,
                DateTime endPosition, string fileIdentifier, string url, string credit,
                int temporalSize, Dictionary<string, (string unit, string description)> dataParameters)
        {
            DataTitle = dataTitle;
            DataAbstract = dataAbstract;
            BeginPosition = beginPosition;
            EndPosition = endPosition;
            FileIdentifier = fileIdentifier;
            URL = url;
            DataParameters = dataParameters;
            Credit = credit;
            TemporalSize = temporalSize;
        }
    }
}
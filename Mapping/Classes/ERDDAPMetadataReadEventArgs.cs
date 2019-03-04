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
        public DateTime DateStart { get; internal set; }
        public DateTime DateEnd { get; internal set; }
        public string FileIdentifier { get; internal set; }
        public string URL { get; internal set; }
        public Dictionary<string, string> DataParameters { get; internal set; }
        public string Credit { get; internal set; }
        public int TemporalSize { get; internal set; }
        public double RowSize { get; set; }
        public double ColumnSize { get; set; }

        public ERDDAPMetadataReadEventArgs(string dataTitle, string dataAbstract, string dateStart,
                string dateEnd, string fileIdentifier, string url, string credit,
                int temporalSize, Dictionary<string, string> dataParameters)
        {
            DataTitle = dataTitle;
            DataAbstract = dataAbstract;
            DateStart = DateTime.Parse(dateStart);
            DateEnd = DateTime.Parse(dateEnd);
            FileIdentifier = fileIdentifier;
            URL = url;
            DataParameters = dataParameters;
            Credit = credit;
            TemporalSize = temporalSize;
        }
    }
}
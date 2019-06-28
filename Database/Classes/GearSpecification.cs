using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAD3.Database.Classes
{
    public class GearSpecification
    {
        public string Property { get; set; }
        public string Type { get; set; }
        public string Notes { get; set; }
        public string RowGuid { get; set; }
        public fad3DataStatus DataStatus { get; set; }
        public int Sequence { get; set; }

        public GearSpecification()
        {
        }

        public GearSpecification(string property, string type, string rowGuid, int sequence)
        {
            Property = property;
            Type = type;
            RowGuid = rowGuid;
            Sequence = sequence;
        }
    }
}
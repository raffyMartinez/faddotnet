using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAD3.Database.Classes
{
    public class GearVariation
    {
        public string VariationName { get; set; }
        public string VariationGuid { get; set; }
        public string GearClassName { get; set; }
        public string GearClassGuid { get; set; }
        public string VariationName2 { get; set; }
        public short MetaphoneKey1 { get; set; }
        public short MetaphoneKey2 { get; set; }

        public GearVariation(string variationGUID, string variationName, string gearClassGuid, short key1, short key2)
        {
            VariationName = variationName;
            VariationGuid = variationGUID;
            GearClassGuid = gearClassGuid;
            MetaphoneKey1 = key1;
            MetaphoneKey2 = key2;
        }
    }
}
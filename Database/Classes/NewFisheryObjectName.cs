using MetaphoneCOM;
using System;

namespace FAD3.Database.Classes
{
    public class NewFisheryObjectName
    {
        public string ObjectGUID { get; internal set; }
        public string NewName { get; internal set; }
        public FisheryObjectNameType NameType { get; internal set; }
        public short Key1 { get; internal set; }
        public short Key2 { get; internal set; }
        public string UseThisName { get; set; }

        public NewFisheryObjectName(string newName, FisheryObjectNameType nameType)
        {
            ObjectGUID = Guid.NewGuid().ToString();
            NewName = newName;
            NameType = nameType;
            var mph = new DoubleMetaphoneShort();
            mph.ComputeMetaphoneKeys(newName, out short key1, out short key2);
            Key1 = key1;
            Key2 = key2;
        }
    }
}
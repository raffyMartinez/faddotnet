using System;

namespace FAD3.Database.Classes
{
    public class FisheriesInventoryImportEventArg : EventArgs
    {
        public string ProjectName { get; internal set; }
        public string ProjectGuid { get; internal set; }
        public string TargetAreaName { get; internal set; }
        public string TargetAreaGuid { get; internal set; }
        public ImportInventoryAction ImportInventoryAction { get; set; }
        public FisheriesInventoryLevel InventoryLevel { get; internal set; }
        public bool Cancel { get; set; }
        public string CancelReason { get; set; }

        public FisheriesInventoryImportEventArg(string levelName, string levelGuid, FisheriesInventoryLevel inventoryLevel)
        {
            InventoryLevel = inventoryLevel;
            switch (inventoryLevel)
            {
                case FisheriesInventoryLevel.Project:
                    ProjectGuid = levelGuid;
                    ProjectName = levelName;
                    break;

                case FisheriesInventoryLevel.TargetArea:
                    TargetAreaGuid = levelGuid;
                    TargetAreaName = levelName;
                    break;
            }
        }
    }
}
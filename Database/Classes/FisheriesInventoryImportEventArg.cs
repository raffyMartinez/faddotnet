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
        public bool ImportCompleted { get; internal set; }
        public string GearVariationImported { get; internal set; }
        public string BarangayName { get; internal set; }

        public FisheriesInventoryImportEventArg(string levelName, string levelGuid, FisheriesInventoryLevel inventoryLevel, string barangay = "")
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

                case FisheriesInventoryLevel.Gear:
                    GearVariationImported = levelName;
                    BarangayName = barangay;
                    break;
            }
        }

        public FisheriesInventoryImportEventArg(bool importCompleted, string projectName, string projectGuid)
        {
            ImportCompleted = importCompleted;
            ProjectName = projectName;
            ProjectGuid = projectGuid;
        }
    }
}
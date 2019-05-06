using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAD3.Database.Classes
{
    public class InventoryItem
    {
        public string InventoryName { get; set; }
        public DateTime DateConducted { get; set; }
        public string TargetArea { get; set; }

        public InventoryItem(string inventoryName, DateTime dateConducted, string targetArea)
        {
            InventoryName = inventoryName;
            DateConducted = dateConducted;
            TargetArea = targetArea;
        }
    }
}
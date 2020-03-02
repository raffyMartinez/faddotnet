using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAD3.Database.Classes
{
    public class FishingExpenseItemsPerOperation
    {
        public string ExpenseItem { get; set; }
        public double? ItemCost { get; set; }
        public string Unit { get; set; }
        public double? UnitQuantity { get; set; }
        public fad3DataStatus DataStats { get; internal set; }
        public string Key { get; set; }

        public FishingExpenseItemsPerOperation(string key, string expenseItem, double? itemCost, string unit, double? unitQuantity, fad3DataStatus dataStatus)
        {
            ExpenseItem = expenseItem;
            ItemCost = itemCost;
            Unit = unit;
            UnitQuantity = unitQuantity;
            DataStats = dataStatus;
            Key = key;
        }
    }
}
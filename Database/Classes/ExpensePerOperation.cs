using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;
using System.Data.OleDb;

namespace FAD3.Database.Classes
{
    public class ExpensePerOperation
    {
        public string SamplingGuid { get; set; }
        public double? CostOfFishing { get; set; }
        public double? ReturnOfInvestment { get; set; }
        public double? IncomeFromFishSale { get; set; }
        public double? WeightFishConsumed { get; set; }
        public fad3DataStatus DataStatus { get; internal set; }
        private Dictionary<string, FishingExpenseItemsPerOperation> _expenseItemsList = new Dictionary<string, FishingExpenseItemsPerOperation>();

        public Dictionary<string, FishingExpenseItemsPerOperation> ExpenseItemsList
        {
            get { return _expenseItemsList; }
        }

        public void AddExpenseItem(string key, FishingExpenseItemsPerOperation item)
        {
            _expenseItemsList.Add(key, item);
        }

        public void AddExpenseItem(string key, string operationExpenseItem, double cost, fad3DataStatus dataStatus, string unit, double unitQuantity, string expenseItemGuid = "")
        {
            FishingExpenseItemsPerOperation expenseItem = new FishingExpenseItemsPerOperation(key, operationExpenseItem, cost, unit, unitQuantity, dataStatus);
            if (dataStatus == fad3DataStatus.statusNew)
            {
                expenseItemGuid = Guid.NewGuid().ToString();
            }
            _expenseItemsList.Add(expenseItemGuid, expenseItem);
        }

        public ExpensePerOperation(string samplingGuid)
        {
            SamplingGuid = samplingGuid;
        }

        public string Summary
        {
            get
            {
                string summary = $@"Cost of fishing: {(CostOfFishing == null ? "-" : CostOfFishing.ToString())}
                       Return of investment: {(ReturnOfInvestment == null ? "-" : ReturnOfInvestment.ToString())}
                       Income from fish sales: {(IncomeFromFishSale == null ? "-" : IncomeFromFishSale.ToString())}
                       Weight of fish consumed: {(WeightFishConsumed == null ? "-" : WeightFishConsumed.ToString())}";

                if (_expenseItemsList.Count > 0)
                {
                    summary += "\r\n\r\nCost items\r\n";
                    foreach (var item in _expenseItemsList)
                    {
                        summary += $"{item.Value.ExpenseItem}: {item.Value.ItemCost.ToString()}\r\n";
                    }
                }

                while (summary.Contains("  "))
                {
                    summary = summary.Replace("  ", " ");
                }
                while (summary.Contains("\r\n "))
                {
                    summary = summary.Replace("\r\n ", "\r\n");
                }
                return summary;
            }
        }

        public ExpensePerOperation(string samplingGuid,
            double? costOfFishing,
            double? returnOfInvestment,
            double? incomeFromFishSale,
            double? weightFishConsumed,
            fad3DataStatus dataStatus)
        {
            SamplingGuid = samplingGuid;
            CostOfFishing = costOfFishing;
            ReturnOfInvestment = returnOfInvestment;
            IncomeFromFishSale = incomeFromFishSale;
            WeightFishConsumed = weightFishConsumed;
            DataStatus = dataStatus;
            _expenseItemsList = new Dictionary<string, FishingExpenseItemsPerOperation>();
        }
    }
}
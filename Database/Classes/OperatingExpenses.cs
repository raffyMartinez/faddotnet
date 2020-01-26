using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace FAD3.Database.Classes
{
    public static class OperatingExpenses
    {
        public static List<string> ExpenseUnitsSelection { get; internal set; } = new List<string>();
        public static List<string> ExpenseItemsSelection { get; internal set; } = new List<string>();
        public static string SamplingExpenses { get; internal set; }

        public static void GetExpenseUnitsSelection()
        {
            string sql = "Select Distinct Unit from tblFishingExpenseItems";
            using (var con = new OleDbConnection(global.ConnectionString))
            {
                using (var dt = new DataTable())
                {
                    con.Open();
                    var adapter = new OleDbDataAdapter(sql, con);
                    adapter.Fill(dt);
                    foreach (DataRow dr in dt.Rows)
                    {
                        ExpenseUnitsSelection.Add(dr["Unit"].ToString());
                    }
                }
            }
        }

        public static void GetExpenseItemsSelection()
        {
            string sql = "Select Distinct ExpenseItem from tblFishingExpenseItems";
            using (var con = new OleDbConnection(global.ConnectionString))
            {
                using (var dt = new DataTable())
                {
                    con.Open();
                    var adapter = new OleDbDataAdapter(sql, con);
                    adapter.Fill(dt);
                    foreach (DataRow dr in dt.Rows)
                    {
                        ExpenseItemsSelection.Add(dr["ExpenseItem"].ToString());
                    }
                }
            }
        }

        public static bool AddExpenseItemToSelection(string item)
        {
            if (!ExpenseItemsSelection.Contains(item))
            {
                ExpenseItemsSelection.Add(item);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool AddExpenseUnitToSelection(string item)
        {
            if (!ExpenseUnitsSelection.Contains(item))
            {
                ExpenseUnitsSelection.Add(item);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool Delete(string samplingGuid)
        {
            string sql = "";
            bool success = false;
            using (var con = new OleDbConnection(global.ConnectionString))
            {
                con.Open();

                sql = $"Delete * from tblFishingExpenseItems where SamplingGuid = {{{samplingGuid}}}";
                using (OleDbCommand update = new OleDbCommand(sql, con))
                {
                    update.ExecuteNonQuery();
                }

                sql = $"Delete * from tblFishingExpense where SamplingGUID = {{{samplingGuid}}}";
                using (OleDbCommand update = new OleDbCommand(sql, con))
                {
                    success = update.ExecuteNonQuery() > 0;
                }
            }
            return success;
        }

        public static bool Update(ExpensePerOperation exp)
        {
            bool success = false;
            string sql = "";
            using (var con = new OleDbConnection(global.ConnectionString))
            {
                con.Open();
                if (exp.DataStatus == fad3DataStatus.statusNew)
                {
                    sql = $@"Insert into tblFishingExpense (SamplingGUID, CostOfFishing, ReturnOfInvestment, IncomeFromFishSold,FishWeightForConsumption)
                       values({{{exp.SamplingGuid}}},
                                {(exp.CostOfFishing == null ? "null" : exp.CostOfFishing.ToString())},
                                {(exp.ReturnOfInvestment == null ? "null" : exp.ReturnOfInvestment.ToString())},
                                {(exp.IncomeFromFishSale == null ? "null" : exp.IncomeFromFishSale.ToString())},
                                {(exp.WeightFishConsumed == null ? "null" : exp.WeightFishConsumed.ToString())}
                        )";
                }
                else
                {
                    sql = $@"Update tblFishingExpense set
                               CostOfFishing =  {(exp.CostOfFishing == null ? "null" : exp.CostOfFishing.ToString())},
                               ReturnOfInvestment = {(exp.ReturnOfInvestment == null ? "null" : exp.ReturnOfInvestment.ToString())},
                               IncomeFromFishSold = {(exp.IncomeFromFishSale == null ? "null" : exp.IncomeFromFishSale.ToString())},
                               FishWeightForConsumption = {(exp.WeightFishConsumed == null ? "null" : exp.WeightFishConsumed.ToString())}
                               Where SamplingGuid = {{{exp.SamplingGuid}}}";
                }
                using (OleDbCommand update = new OleDbCommand(sql, con))
                {
                    success = update.ExecuteNonQuery() > 0;
                    if (success)
                    {
                        sql = $"Delete * from tblFishingExpenseItems where SamplingGuid = {{{exp.SamplingGuid}}}";
                        using (OleDbCommand update1 = new OleDbCommand(sql, con))
                        {
                            update1.ExecuteNonQuery();
                        }

                        foreach (var item in exp.ExpenseItemsList)
                        {
                            sql = $@"Insert into tblFishingExpenseItems (SamplingGuid, ExpenseRow, ExpenseItem, Cost, Unit, UnitQuantity)
                                    values({{{exp.SamplingGuid}}},
                                            {{{item.Key}}},
                                            '{item.Value.ExpenseItem}',
                                            {item.Value.ItemCost.ToString()},
                                            '{item.Value.Unit}',
                                            {item.Value.UnitQuantity.ToString()})";
                            using (OleDbCommand update1 = new OleDbCommand(sql, con))
                            {
                                update1.ExecuteNonQuery();
                            }
                        }

                        SamplingExpenses = $@"Cost of fishing: {(exp.CostOfFishing == null ? "-" : exp.CostOfFishing.ToString())}
                                              Return of investment: {(exp.ReturnOfInvestment == null ? "-" : exp.ReturnOfInvestment.ToString())}
                                              Income from fish sales: {(exp.IncomeFromFishSale == null ? "-" : exp.IncomeFromFishSale.ToString())}
                                              Weight of fish consumed: {(exp.WeightFishConsumed == null ? "-" : exp.WeightFishConsumed.ToString())}";

                        while (SamplingExpenses.Contains("  "))
                        {
                            SamplingExpenses = SamplingExpenses.Replace("  ", " ");
                        }
                        while (SamplingExpenses.Contains("\r\n "))
                        {
                            SamplingExpenses = SamplingExpenses.Replace("\r\n ", "\r\n");
                        }
                    }
                }
            }
            return success;
        }

        public static (ExpensePerOperation exp, bool success) ReadData(string samplingGuid, bool readComplete = false)
        {
            ExpensePerOperation exp = new ExpensePerOperation(samplingGuid);
            bool success = false;
            string sql = $"Select * from tblFishingExpense where SamplingGuid ={{{samplingGuid}}}";
            using (var con = new OleDbConnection(global.ConnectionString))
            {
                using (var dt = new DataTable())
                {
                    con.Open();
                    var adapter = new OleDbDataAdapter(sql, con);
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        if (double.TryParse(dr["CostOfFishing"].ToString(), out double costOfFishing))
                        {
                            SamplingExpenses = $"Cost of fishing: {costOfFishing.ToString()}\r\n";
                            exp.CostOfFishing = costOfFishing;
                        }
                        else
                        {
                            SamplingExpenses = $"Cost of fishing: -\r\n";
                        }
                        if (double.TryParse(dr["ReturnOfInvestment"].ToString(), out double roi))
                        {
                            exp.ReturnOfInvestment = roi;
                            SamplingExpenses += $"Return of investment: {roi.ToString()}\r\n";
                        }
                        else
                        {
                            SamplingExpenses += $"Return of investment: -\r\n";
                        }
                        if (double.TryParse(dr["IncomeFromFishSold"].ToString(), out double income))
                        {
                            exp.IncomeFromFishSale = income;
                            SamplingExpenses += $"Income from fish sales: {income.ToString()}\r\n";
                        }
                        else
                        {
                            SamplingExpenses += $"Income from fish sales: -\r\n";
                        }
                        if (double.TryParse(dr["FishWeightForConsumption"].ToString(), out double weightFishConsumed))
                        {
                            exp.WeightFishConsumed = weightFishConsumed;
                            SamplingExpenses += $"Weight of fish consumed: {weightFishConsumed.ToString()}";
                        }
                        else
                        {
                            SamplingExpenses += $"Weight of fish consumed: -";
                        }
                        success = true;
                    }
                    else
                    {
                        SamplingExpenses = "Cost of fishing: -\r\nReturn of investment: -\r\nIncome from fish sales: -\r\nWeight of fish consumed: -";
                    }
                }
                if (success)
                {
                    Dictionary<string, FishingExpenseItemsPerOperation> expenseItemsList = new Dictionary<string, FishingExpenseItemsPerOperation>();
                    sql = $"Select * from tblFishingExpenseItems where SamplingGuid={{{samplingGuid}}}";
                    //using (var con1 = new OleDbConnection(global.ConnectionString))
                    //{
                    using (var dt = new DataTable())
                    {
                        //con1.Open();
                        //var adapter = new OleDbDataAdapter(sql, con1);
                        var adapter = new OleDbDataAdapter(sql, con);
                        adapter.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            SamplingExpenses += "\r\n\r\nCost items:\r\n";
                            foreach (DataRow dr in dt.Rows)
                            {
                                string key = dr["ExpenseRow"].ToString();
                                string item = dr["ExpenseItem"].ToString();
                                double cost = (double)dr["Cost"];
                                string unit = dr["Unit"].ToString();
                                double unitQuantity = (double)dr["UnitQuantity"];
                                FishingExpenseItemsPerOperation expenseItem = new FishingExpenseItemsPerOperation(key, item, cost, unit, unitQuantity, fad3DataStatus.statusFromDB);
                                SamplingExpenses += $"{item}: {cost.ToString()}\r\n";
                                exp.AddExpenseItem(key, expenseItem);
                            }
                        }
                    }
                    //}
                }
                return (exp, success);
            }
        }
    }
}
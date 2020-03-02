using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FAD3.Database.Classes;

namespace FAD3.Database.Forms
{
    public partial class FishingOperationCostsForm : Form
    {
        private string _samplingGUID;
        private static FishingOperationCostsForm _instance;
        private ExpensePerOperation _expensePerOperation;

        //public bool IsNew { get; internal set; } = false;
        private SamplingForm _parentForm;
        private fad3DataStatus _dataStatus;
        private bool _expensesDeleted = false;

        public static FishingOperationCostsForm GetInstance(string samplingGUID, SamplingForm parentForm, bool hasExpenseData)
        {
            if (_instance == null)
            {
                _instance = new FishingOperationCostsForm(samplingGUID, parentForm, hasExpenseData);
            }
            return _instance;
        }

        public FishingOperationCostsForm(string samplingGUID, SamplingForm parentForm, bool hasExpenseData)
        {
            InitializeComponent();
            _samplingGUID = samplingGUID;
            _parentForm = parentForm;
            if (hasExpenseData)
            {
                _dataStatus = fad3DataStatus.statusFromDB;
            }
            else
            {
                _dataStatus = fad3DataStatus.statusNew;
            }
        }

        private bool PreSaveExpense(fad3DataStatus dataStatus)
        {
            double? operatingCost = null;
            double? roi = null;
            double? incomeSales = null;
            double? weightConsumed = null;

            if (double.TryParse(txtCostOfFishing.Text, out double c))
            {
                operatingCost = c;
            }
            if (double.TryParse(txtROI.Text, out double r))
            {
                roi = r;
            }
            if (double.TryParse(txtIncomeSales.Text, out double s))
            {
                incomeSales = s;
            }
            if (double.TryParse(txtWeightConsumed.Text, out double w))
            {
                weightConsumed = w;
            }

            ExpensePerOperation exp = new ExpensePerOperation(_samplingGUID, operatingCost, roi, incomeSales, weightConsumed, dataStatus);
            foreach (ListViewItem lvi in lvExpenseItems.Items)
            {
                double? noOfUnits = null;
                if (double.TryParse(lvi.SubItems[3].Text, out double v))
                {
                    noOfUnits = v;
                }
                FishingExpenseItemsPerOperation fpe = new FishingExpenseItemsPerOperation(lvi.Name, lvi.Text, double.Parse(lvi.SubItems[1].Text), lvi.SubItems[2].Text, noOfUnits, _dataStatus);
                exp.AddExpenseItem(lvi.Name, fpe);
            }

            if (operatingCost != null || roi != null || incomeSales != null || weightConsumed != null)
            {
                _parentForm.ExpensePerOperation = exp;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ValidateExpenses()
        {
            return (txtCostOfFishing.Text.Length > 0
                  || txtIncomeSales.Text.Length > 0
                  || txtROI.Text.Length > 0
                  || txtWeightConsumed.Text.Length > 0);
        }

        private void ShowEditItemForm(ListViewItem lvi = null)
        {
            if (ValidateExpenses())
            {
                using (EditOperatingExpenseItemForm eof = new EditOperatingExpenseItemForm(this))
                {
                    if (lvi != null)
                    {
                        eof.ExpenseItem = lvi.Text;
                        eof.ItemCost = double.Parse(lvi.SubItems[1].Text);
                        eof.Unit = lvi.SubItems[2].Text;
                        eof.UnitQuantity = double.Parse(lvi.SubItems[3].Text);
                    }
                    eof.ShowDialog(this);
                    if (eof.DialogResult == DialogResult.OK)
                    {
                        if (lvi == null)
                        {
                            AddNewExpenseLine(eof.ExpenseItem, eof.ItemCost, eof.Unit, eof.UnitQuantity);
                        }
                        else
                        {
                            lvi.Text = eof.ExpenseItem;
                            lvi.SubItems[1].Text = eof.ItemCost.ToString();
                            lvi.SubItems[2].Text = eof.Unit;
                            lvi.SubItems[3].Text = eof.UnitQuantity.ToString();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please enter operating expense items first", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnDelete":
                    DialogResult deleteDr = MessageBox.Show("Delete fishing operating costs for this sampling?", "Please confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (deleteDr == DialogResult.Yes && OperatingExpenses.Delete(_samplingGUID))
                    {
                        lvExpenseItems.Items.Clear();
                        foreach (Control c in Controls)
                        {
                            if (c.GetType().Name == "TextBox")
                            {
                                c.Text = "";
                            }
                        }
                        _expensesDeleted = true;
                        _parentForm.SamplingFishingOperatingExpenseDeleted();
                        Close();
                    }
                    break;

                case "btnAdd":
                    ShowEditItemForm();
                    break;

                case "btnRemove":
                    if (lvExpenseItems.SelectedItems.Count > 0)
                    {
                        lvExpenseItems.Items.Remove(lvExpenseItems.SelectedItems[0]);
                    }
                    break;

                case "btnOK":
                    if (ValidateExpenses() && PreSaveExpense(_dataStatus))
                    {
                        _parentForm.UpdateExpenses();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Cannot accept blank expense. At least one item must be filled up", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

                case "btnCancel":
                    Close();
                    break;
            }
        }

        public void AddNewExpenseLine(string expenseItem, double cost, string unit, double? unitQuantity)
        {
            var lvi = lvExpenseItems.Items.Add(Guid.NewGuid().ToString(), expenseItem, null);
            lvi.SubItems.Add(cost.ToString());
            lvi.SubItems.Add(unit);
            lvi.SubItems.Add(unitQuantity.ToString());
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
            global.SaveFormSettings(this);
        }

        private void ReadData()
        {
            ExpensePerOperation exp = new ExpensePerOperation("");
            if (_parentForm.ExpensePerOperation == null)
            {
                var result = OperatingExpenses.ReadData(_samplingGUID);
                exp = result.exp;
            }
            else
            {
                exp = _parentForm.ExpensePerOperation;
            }
            //IsNew = !result.success;
            txtCostOfFishing.Text = exp.CostOfFishing.ToString();
            txtIncomeSales.Text = exp.IncomeFromFishSale.ToString();
            txtROI.Text = exp.ReturnOfInvestment.ToString();
            txtWeightConsumed.Text = exp.WeightFishConsumed.ToString();
            foreach (var item in exp.ExpenseItemsList)
            {
                var lvi = lvExpenseItems.Items.Add(item.Key, item.Value.ExpenseItem, null);
                lvi.SubItems.Add(item.Value.ItemCost.ToString());
                lvi.SubItems.Add(item.Value.Unit);
                lvi.SubItems.Add(item.Value.UnitQuantity.ToString());
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            global.LoadFormSettings(this);

            ReadData();
        }

        private void OnFieldValidating(object sender, CancelEventArgs e)
        {
            string msg = "";
            string s = ((Control)sender).Text;

            if (s.Length > 0)
            {
                if (double.TryParse(s, out double v) && v > 0)
                {
                    switch (((Control)sender).Name)
                    {
                        case "txtCostOfFishing":
                            break;

                        case "txtROI":
                            break;

                        case "txtIncomeSales":
                            break;

                        case "txtWeightConsumed":
                            break;
                    }
                }
                else
                {
                    msg = "Expected value is numeric and greater than zero";
                }
            }
            if (msg.Length > 0)
            {
                e.Cancel = true;
                MessageBox.Show(msg, "Validation errpr", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            var hitTestItem = lvExpenseItems.HitTest(e.X, e.Y).Item;
            if (hitTestItem != null)
            {
                if (e.Button == MouseButtons.Left && e.Clicks == 2)
                {
                    ShowEditItemForm(hitTestItem);
                }
            }
            else
            {
                if (e.Clicks == 2)
                {
                    ShowEditItemForm();
                }
            }
        }
    }
}
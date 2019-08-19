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
    public partial class EditOperatingExpenseItemForm : Form
    {
        public string ExpenseItem { get; set; }
        public double ItemCost { get; set; }
        public string Unit { get; set; }
        public double UnitQuantity { get; set; }
        private FishingOperationCostsForm _parent;

        public EditOperatingExpenseItemForm(FishingOperationCostsForm parentForm)
        {
            InitializeComponent();
            _parent = parentForm;
            foreach (var item in OperatingExpenses.ExpenseItemsSelection)
            {
                cboSelection.Items.Add(item);
            }
            foreach (var item in OperatingExpenses.ExpenseUnitsSelection)
            {
                cboUnit.Items.Add(item);
            }
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    if (txtCost.Text.Length > 0
                        && cboSelection.Text.Length > 0
                        && cboUnit.Text.Length > 0
                        && txtUnitQuantity.Text.Length > 0)

                    {
                        ExpenseItem = cboSelection.Text;
                        ItemCost = double.Parse(txtCost.Text);
                        Unit = cboUnit.Text;
                        UnitQuantity = double.Parse(txtUnitQuantity.Text);
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("Please fill up all fields", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

                case "btnCancel":
                    DialogResult = DialogResult.Cancel;
                    break;
            }
        }

        private void OnFieldValidating(object sender, CancelEventArgs e)
        {
            string s = ((Control)sender).Text;
            string controlName = ((Control)sender).Name;
            string msg = "";
            if (s.Length > 0)
            {
                switch (controlName)
                {
                    case "txtCost":
                    case "txtUnitQuantity":
                        if (double.TryParse(s, out double v))
                        {
                            if (v < 0)
                            {
                                msg = "Expected value is numeric and must not be less than zero";
                                e.Cancel = true;
                            }
                        }
                        else
                        {
                            msg = "Expected value is numeric and must not be less than zero";
                            e.Cancel = true;
                        }
                        break;

                    case "cboSelection":
                    case "cboUnit":
                        ComboBox cbo = (ComboBox)sender;
                        if (s.Length < 2)
                        {
                            e.Cancel = true;
                            msg = "Item is too short. Make it at least 3 letters long";
                        }
                        else
                        {
                            if (!cbo.Items.Contains(s))
                            {
                                DialogResult dr = MessageBox.Show($"{s} was not found in the selection\r\nDo you want to add {s} to the list?", "Confirmation needed",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                if (dr == DialogResult.Yes)
                                {
                                    switch (controlName)
                                    {
                                        case "cboSelection":

                                            if (OperatingExpenses.AddExpenseItemToSelection(s))
                                            {
                                                cboSelection.Items.Add(s);
                                                cboSelection.Text = s;
                                            }
                                            break;

                                        case "cboUnit":

                                            if (OperatingExpenses.AddExpenseUnitToSelection(s))
                                            {
                                                cboUnit.Items.Add(s);
                                                cboUnit.Text = s;
                                            }
                                            break;
                                    }
                                }
                                else
                                {
                                    msg = "Select an item in the drop-down list";
                                    e.Cancel = true;
                                }
                            }
                        }
                        break;
                }
            }

            if (e.Cancel)
            {
                MessageBox.Show(msg, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            if (ExpenseItem?.Length > 0)
            {
                cboSelection.Text = ExpenseItem;
                txtCost.Text = ItemCost.ToString();
                cboUnit.Text = Unit;
                txtUnitQuantity.Text = UnitQuantity.ToString();
            }
        }
    }
}
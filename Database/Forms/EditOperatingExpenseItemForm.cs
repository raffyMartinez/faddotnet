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
        private FishingOperationCostsForm _parent;

        public EditOperatingExpenseItemForm(FishingOperationCostsForm parentForm)
        {
            InitializeComponent();
            _parent = parentForm;
            foreach (var item in OperatingExpenses.ExpenseItemsSelection)
            {
                cboSelection.Items.Add(item);
            }
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    if (txtCost.Text.Length > 0 && cboSelection.Text.Length > 0)

                    {
                        ExpenseItem = cboSelection.Text;
                        ItemCost = double.Parse(txtCost.Text);
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
            string msg = "";
            if (s.Length > 0)
            {
                switch (((Control)sender).Name)
                {
                    case "txtCost":
                        ;
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
                        if (s.Length < 2)
                        {
                            e.Cancel = true;
                            msg = "Item name is too short. Make it at least 3 letters long";
                        }
                        else
                        {
                            if (!cboSelection.Items.Contains(s))
                            {
                                DialogResult dr = MessageBox.Show($"{s} was not found in the selection\r\nDo you want to add to the list", "Confirmation needed",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                if (dr == DialogResult.Yes)
                                {
                                    if (OperatingExpenses.AddExpenseItemToSelection(s))
                                    {
                                        cboSelection.Items.Add(s);
                                        cboSelection.Text = s;
                                    }
                                }
                                else
                                {
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
            if (ExpenseItem.Length > 0)
            {
                cboSelection.Text = ExpenseItem;
                txtCost.Text = ItemCost.ToString();
            }
        }
    }
}
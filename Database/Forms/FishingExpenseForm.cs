using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FAD3.Database.Forms
{
    public partial class FishingExpenseForm : Form
    {
        private GearInventoryEditForm _parentForm;
        private string _expenseItem;
        private double _cost;
        private string _source;
        private string _notes;

        public string ExpenseItem
        {
            set { _expenseItem = value; }
            get { return cboExpenseItem.Text; }
        }

        public double Cost
        {
            set { _cost = value; }
            get { return double.Parse(txtCost.Text); }
        }

        public string Source
        {
            set { _source = value; }
            get { return cboSource.Text; }
        }

        public string Notes
        {
            set { _notes = value; }
            get { return txtNotes.Text; }
        }

        public FishingExpenseForm(GearInventoryEditForm parent)
        {
            InitializeComponent();
            _parentForm = parent;
        }

        public FishingExpenseForm(GearInventoryEditForm parent, string expenseItem, double cost, string source, string notes)
        {
            InitializeComponent();
            _expenseItem = expenseItem;
            _cost = cost;
            _source = source;
            _notes = notes;
            _parentForm = parent;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            foreach (var item in Gear.ExpenseItems)
            {
                cboExpenseItem.Items.Add(item);
            }
            cboExpenseItem.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboExpenseItem.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            foreach (var item in Gear.PaymentSources)
            {
                cboSource.Items.Add(item);
            }
            cboSource.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboSource.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            if (_cost > 0)
            {
                txtCost.Text = _cost.ToString();
                txtNotes.Text = _notes;
                cboExpenseItem.Text = _expenseItem;
                cboSource.Text = _source;
            }
        }

        private bool ValidateForm()
        {
            return cboExpenseItem.Text.Length > 0 && cboSource.Text.Length > 0 && txtCost.Text.Length > 0;
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    if (ValidateForm())
                    {
                        //_parentForm.RefreshFishingExpense(_expenseItem, cboExpenseItem.Text, double.Parse(txtCost.Text), cboSource.Text, txtNotes.Text);
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("Please provide values for expense item, cost, and source of money", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

                case "btnCancel":
                    DialogResult = DialogResult.Cancel;
                    break;
            }
        }

        private void OnFieldValidating(object sender, CancelEventArgs e)
        {
            var s = ((Control)sender).Text;
            var name = ((Control)sender).Name;
            if (s.Length > 0)
            {
                switch (name)
                {
                    case "txtCost":
                        const string costMsg = "Expected value must be a number greater than zero";
                        if (!double.TryParse(s, out double v))
                        {
                            e.Cancel = true;
                        }
                        else
                        {
                            e.Cancel = v <= 0;
                        }
                        if (e.Cancel)
                        {
                            MessageBox.Show(costMsg, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;

                    case "cboExpenseItem":
                    case "cboSource":
                        var cbo = (ComboBox)sender;
                        if (!cbo.Items.Contains(s))
                        {
                            var msg = $"The list does not contain '{s}' \r\nDo you want to add a new item?";
                            if (MessageBox.Show(msg, "Item not found", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                            {
                                switch (name)
                                {
                                    case "cboExpenseItem":
                                        Gear.AddExpense(s);
                                        break;

                                    case "cboSource":
                                        Gear.AddPaymentSource(s);
                                        break;
                                }
                            }
                            else
                            {
                                e.Cancel = true;
                            }
                        }
                        break;
                }
            }
        }
    }
}
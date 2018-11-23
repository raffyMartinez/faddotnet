using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            get { return cboExpenseItem.Text; }
        }

        public double Cost
        {
            get { return double.Parse(txtCost.Text); }
        }

        public string Source
        {
            get { return cboSource.Text; }
        }

        public string Notes
        {
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
            txtCost.Text = _cost.ToString();
            txtNotes.Text = _notes;
            cboExpenseItem.Text = _expenseItem;
            cboSource.Text = _source;
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
                    }
                    break;

                case "btnCancel":
                    break;
            }
        }

        private void OnFieldValidating(object sender, CancelEventArgs e)
        {
            switch (((Control)sender).Name)
            {
                case "txtCost":
                    break;

                case "cboExpenseItem":
                case "cboSource":
                    break;
            }
        }
    }
}
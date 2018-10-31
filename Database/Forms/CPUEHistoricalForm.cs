using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FAD3.Database.Forms;

namespace FAD3.Database.Forms
{
    public partial class CPUEHistoricalForm : Form
    {
        private GearInventoryEditForm _parentForm;
        private int _catchWt;

        public CPUEHistoricalForm(GearInventoryEditForm parentForm, string decade)
        {
            InitializeComponent();
            lblTitle.Text += $" {decade}";
            _parentForm = parentForm;

            cboUnit.Items.Add("kilos");
            cboUnit.Items.Add("banyera");
            cboUnit.Items.Add("ice box");
            cboUnit.SelectedIndex = 0;
        }

        public void CatchValue(int? catchWt, string unit)
        {
            if (catchWt != null && unit.Length > 0)
            {
                txtCPUE.Text = catchWt.ToString();
                cboUnit.Text = unit;
            }
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    if (txtCPUE.Text.Length > 0 && cboUnit.Text.Length > 0)
                    {
                        _parentForm.HistoricalCPUE(_catchWt, cboUnit.Text);
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Please provide catch weight and unit");
                    }
                    break;

                case "btnCancel":
                    Close();
                    break;
            }
        }

        private void OnTextBoxValidating(object sender, CancelEventArgs e)
        {
            var s = ((TextBox)sender).Text;
            if (s.Length > 0)
            {
                e.Cancel = !int.TryParse(s, out int catchWt);
                if (!e.Cancel)
                {
                    _catchWt = catchWt;
                    e.Cancel = _catchWt <= 0;
                }
            }
            if (e.Cancel)
            {
                MessageBox.Show("Only whole numbers greater than zero are accepted", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
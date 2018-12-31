using FAD3.Database.Classes;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FAD3.Database.Forms
{
    public partial class CPUEHistoricalForm : Form
    {
        private int _catchWt;
        private bool _byDecade;
        private int? _decadeYear;
        private GearInventoryEditForm _parentForm;
        public int DecadeYear { get; set; }
        public int CPUE { get; set; }
        public bool ByDecade { get; set; }
        public string CPUEUnit { get; set; }
        public string Notes { get; set; }
        public fad3DataStatus DataStatus { get; set; }
        private bool _duplicatedYear;

        public CPUEHistoricalForm(GearInventoryEditForm parent)
        {
            InitializeComponent();
            _parentForm = parent;
        }

        private bool FormValidated()
        {
            if (ByDecade)
            {
                return txtCPUE.Text.Length > 0 && cboUnit.Text.Length > 0;
            }
            else
            {
                _duplicatedYear = false;
                if (txtHistoryYear.Text.Length > 0)
                {
                    if (DataStatus == fad3DataStatus.statusNew && _parentForm.HistoryList.Items.ContainsKey(txtHistoryYear.Text))
                    {
                        MessageBox.Show("Year already in the list", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _duplicatedYear = true;
                        return false;
                    }
                    else
                    {
                        return txtCPUE.Text.Length > 0 && cboUnit.Text.Length > 0;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    if (FormValidated())
                    {
                        //_parentForm.HistoricalCPUE(_catchWt, cboUnit.Text, txtNotes.Text);
                        if (!ByDecade)
                        {
                            DecadeYear = int.Parse(txtHistoryYear.Text);
                        }
                        CPUE = int.Parse(txtCPUE.Text);
                        CPUEUnit = cboUnit.Text;
                        Notes = txtNotes.Text;
                        DialogResult = DialogResult.OK;
                        //Close();
                    }
                    else
                    {
                        if (!_duplicatedYear)
                        {
                            MessageBox.Show("Please provide catch weight and unit");
                        }
                    }
                    break;

                case "btnCancel":
                    DialogResult = DialogResult.Cancel;
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

        private void OnFormLoad(object sender, EventArgs e)
        {
            if (ByDecade)
            {
                lblTitle.Text += $" {DecadeYear.ToString()}s";
            }
            else
            {
                lblTitle.Visible = false;
                txtHistoryYear.Size = txtCPUE.Size;
                txtHistoryYear.Location = txtCPUE.Location;
                txtHistoryYear.Top = txtCPUE.Top - txtCPUE.Height - 6;
                txtHistoryYear.Visible = true;
                lblHistoryYear.Visible = true;
                lblHistoryYear.Top = txtHistoryYear.Top + 3;
                lblHistoryYear.Left = lblValue.Left;
                lblHistoryYear.Text = "Year";
            }
            cboUnit.Items.Add("kilos");
            cboUnit.Items.Add("banyera");
            cboUnit.Items.Add("ice box");
            cboUnit.SelectedIndex = 0;
            global.LoadFormSettings(this, true);
            if (DataStatus != fad3DataStatus.statusNew)
            {
                txtCPUE.Text = CPUE.ToString();
                txtNotes.Text = Notes;
                cboUnit.Text = CPUEUnit;
                if (!ByDecade)
                {
                    txtHistoryYear.Text = DecadeYear.ToString();
                    txtHistoryYear.Focus();
                }
            }
            else
            {
                if (!ByDecade)
                {
                    txtHistoryYear.Focus();
                }
            }
        }

        private void CPUEHistoricalForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            global.SaveFormSettings(this);
        }
    }
}
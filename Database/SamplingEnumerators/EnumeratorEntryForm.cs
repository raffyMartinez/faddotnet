using FAD3.Database.Classes;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FAD3
{
    public partial class EnumeratorEntryForm : Form
    {
        private string _enumeratorName;
        private DateTime _dateHired;
        private bool _isActive;
        private EnumeratorForm _parentForm;
        private fad3DataStatus _dataStatus;
        private string _enumeratorGuid;
        private string _targetAreaGuid;
        public string EnumeratorName { get; internal set; }
        public DateTime DateHired { get; internal set; }
        public string EnumeratorGuid { get; internal set; }

        public EnumeratorEntryForm(EnumeratorForm Parent)
        {
            InitializeComponent();
            _parentForm = Parent;
            _dataStatus = fad3DataStatus.statusNew;
        }

        public EnumeratorEntryForm(string enumeratorName, string targetAreaGuid)
        {
            InitializeComponent();
            _dataStatus = fad3DataStatus.statusNew;
            txtName.Text = enumeratorName;
            _targetAreaGuid = targetAreaGuid;
        }

        public EnumeratorEntryForm(string EnumeratorGuid, string EnumeratorName, DateTime DateHired, bool IsActive, EnumeratorForm Parent)
        {
            InitializeComponent();
            _enumeratorName = EnumeratorName;
            _dateHired = DateHired;
            _isActive = IsActive;
            _parentForm = Parent;

            _enumeratorGuid = EnumeratorGuid;
            txtName.Text = _enumeratorName;
            txtHireDate.Text = DateHired.ToString("MMM-dd-yyyy");
            chkActive.Checked = IsActive;
        }

        private void Onbutton_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonOK":
                    if (txtName.Text.Length > 0 && txtHireDate.Text.Length > 0)
                    {
                        _enumeratorName = txtName.Text;
                        _dateHired = DateTime.Parse(txtHireDate.Text);
                        _isActive = chkActive.Checked;

                        if (_dataStatus == fad3DataStatus.statusNew)
                        {
                            _enumeratorGuid = Guid.NewGuid().ToString();
                        }

                        if (_dataStatus == fad3DataStatus.statusNew || _dataStatus == fad3DataStatus.statusEdited)
                        {
                            if (_parentForm != null)
                            {
                                _parentForm.EditedEnumerator(_enumeratorGuid, _enumeratorName, _dateHired, _isActive, _dataStatus);
                            }
                            else
                            {
                                _enumeratorGuid = "";
                                var result = Enumerators.SaveNewTargetAreaEnumerator(_targetAreaGuid, _enumeratorName, _dateHired, _isActive);
                                if (result.success)
                                {
                                    _enumeratorGuid = result.newGuid;
                                    EnumeratorName = _enumeratorName;
                                    DateHired = _dateHired;
                                    EnumeratorGuid = _enumeratorGuid;
                                    DialogResult = DialogResult.OK;
                                }
                            }

                            Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please provide name and hire date", "Validation error",
                                         MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;

                case "buttonCancel":
                    DialogResult = DialogResult.Cancel;
                    Close();
                    break;
            }
        }

        private void OnTextBox_Validating(object sender, CancelEventArgs e)
        {
            var msg = "";
            ((TextBox)sender).With(o =>
            {
                var s = o.Text;
                if (s.Length > 0)
                {
                    switch (o.Name)
                    {
                        case "txtName":
                            msg = "Name must be at least 5 letters long";
                            e.Cancel = s.Length < 6;
                            break;

                        case "txtHireDate":
                            e.Cancel = !DateTime.TryParse(s, out DateTime HireDate);
                            if (e.Cancel)
                            {
                                msg = "Please enter a valid date";
                            }
                            else
                            {
                                e.Cancel = HireDate > DateTime.Today;
                                if (e.Cancel)
                                {
                                    msg = "Hire date cannot be a future date";
                                }
                                else
                                {
                                    o.Text = DateTime.Parse(o.Text).ToString("MMM-dd-yyyy");
                                }
                            }
                            break;
                    }
                }
            });

            if (e.Cancel)
                MessageBox.Show(msg, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            if (_dataStatus != fad3DataStatus.statusNew) _dataStatus = fad3DataStatus.statusEdited;
        }

        private void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            if (_dataStatus != fad3DataStatus.statusNew) _dataStatus = fad3DataStatus.statusEdited;
        }

        private void OnTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ((TextBox)sender).With(o =>
                {
                    var s = o.Text;
                    if (s.Length > 0)
                    {
                        switch (o.Name)
                        {
                            case "txtName":
                                txtHireDate.Focus();
                                break;

                            case "txtHireDate":
                                chkActive.Focus();
                                break;
                        }
                    }
                });
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            global.LoadFormSettings(this, true);
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            global.SaveFormSettings(this);
        }
    }
}
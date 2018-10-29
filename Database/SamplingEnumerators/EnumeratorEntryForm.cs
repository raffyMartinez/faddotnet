using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FAD3.GUI.Classes;

namespace FAD3
{
    public partial class EnumeratorEntryForm : Form
    {
        private string _EnumeratorName;
        private DateTime _DateHired;
        private bool _IsActive;
        private EnumeratorForm _ParentForm;
        private fad3DataStatus _DataStatus;
        private string _EnumeratorGuid;

        public EnumeratorEntryForm(EnumeratorForm Parent)
        {
            InitializeComponent();
            _ParentForm = Parent;
            _DataStatus = fad3DataStatus.statusNew;
        }

        public EnumeratorEntryForm(string EnumeratorGuid, string EnumeratorName, DateTime DateHired, bool IsActive, EnumeratorForm Parent)
        {
            InitializeComponent();
            _EnumeratorName = EnumeratorName;
            _DateHired = DateHired;
            _IsActive = IsActive;
            _ParentForm = Parent;

            _EnumeratorGuid = EnumeratorGuid;
            txtName.Text = _EnumeratorName;
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
                        _EnumeratorName = txtName.Text;
                        _DateHired = DateTime.Parse(txtHireDate.Text);
                        _IsActive = chkActive.Checked;
                        if (_DataStatus == fad3DataStatus.statusNew) _EnumeratorGuid = Guid.NewGuid().ToString();

                        if (_DataStatus == fad3DataStatus.statusNew || _DataStatus == fad3DataStatus.statusEdited)
                            _ParentForm.EditedEnumerator(_EnumeratorGuid, _EnumeratorName, _DateHired, _IsActive, _DataStatus);

                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Please provide name and hire date", "Validation error",
                                         MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;

                case "buttonCancel":
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
            if (_DataStatus != fad3DataStatus.statusNew) _DataStatus = fad3DataStatus.statusEdited;
        }

        private void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            if (_DataStatus != fad3DataStatus.statusNew) _DataStatus = fad3DataStatus.statusEdited;
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
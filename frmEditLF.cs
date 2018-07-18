using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FAD3
{
    public partial class frmEditLF : Form
    {
        private string _RowGUID;
        private string _CatchCompRowGUID;
        private string _CurrentCatchName;
        private frmLenFreq _ParentForm;
        private bool _IsNew;

        public string CurrentCatchName
        {
            get { return _CurrentCatchName; }
            set
            {
                _CurrentCatchName = value;
                lblTitle.Text = _CurrentCatchName;
            }
        }

        public void AddNew()
        {
            _IsNew = true;
        }

        public void LFData(string RowGUID, double LenClass, long Freq)
        {
            _RowGUID = RowGUID;
            txtLen.Text = LenClass.ToString();
            txtFreq.Text = Freq.ToString();
            _IsNew = false;
        }

        public new frmLenFreq ParentForm
        {
            get { return _ParentForm; }
            set { _ParentForm = value; }
        }
            
            public string CatchCompRowGUID
        {
            get { return _CatchCompRowGUID; }
            set { _CatchCompRowGUID = value; }
        }
        public frmEditLF()
        {
            InitializeComponent();
        }

        private void frmEditLF_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtFreq.Text.Length > 0 && txtLen.Text.Length > 0)
            {
                if (_IsNew)
                {
                    _RowGUID = Guid.NewGuid().ToString();
                }

                double LenClass = double.Parse(txtLen.Text);
                long Freq = long.Parse(txtFreq.Text);
                if (sampling.UpdateLF(_IsNew, LenClass, Freq, _CatchCompRowGUID, _RowGUID))
                {
                    _ParentForm.EditedLF(_IsNew, _RowGUID,LenClass,Freq);
                    if (checkContinue.Checked==false)
                    {
                        this.Close();
                    }
                    else
                    {
                        txtLen.Text = "";
                        txtFreq.Text = "";
                        txtLen.Focus();
                    }
                }
            }
        }

        private void txtLen_Validating(object sender, CancelEventArgs e)
        {
            TextBox t;
            t = (TextBox)sender;
            if (t.Text.Length>0) {
                string msg=string.Empty;
                if (t.Tag.ToString() == "class")
                {
                    double num;
                    if (double.TryParse(t.Text,out num))
                    {
                        if (num<=0)
                        {
                            msg = "Valid values are numbers greater than zero";
                        }
                    }
                    else
                    {
                        msg = "Valid values are numbers greater than zero";
                    }
                }
                else if (t.Tag.ToString() == "freq")
                {
                    long num;
                    if (long.TryParse(txtFreq.Text,out num))
                    {
                        if (num<1)
                        {
                            msg = "Valid values are whole numbers equal or greater than one";
                        }

                    }
                    else
                    {
                        msg = "Valid values are whole numbers equal or greater than one";
                    }
                }

                if (msg.Length>0)
                {
                    e.Cancel = true;
                    MessageBox.Show(msg);
                }
            }

        }
    }
}

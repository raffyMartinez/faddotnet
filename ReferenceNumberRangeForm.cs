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
    public partial class ReferenceNumberRangeForm : Form
    {
        private long _MinVal = 0;
        private long _MaxVal = 0;

        public ReferenceNumberRangeForm()
        {
            InitializeComponent();
        }


        private void ButtonClick(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b.Name == "buttonCancel")
                this.Close();
            else if (b.Name == "buttonReset")
            {
                ReferenceNumberManager.SetRefNoRange(true);
                this.Close();
            }
            else
            {
                if (_MinVal == 0 && _MaxVal == 0)
                    this.Close();
                else
                {
                    if (_MaxVal > _MinVal)
                    {
                        ReferenceNumberManager.SetRefNoRange(false, _MinVal, _MaxVal);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Maximum value must be greater than the minimum", "Validation error");
                    }
                }

            }
        }

        private void ValidateTextBox(object sender, CancelEventArgs e)
        {
            TextBox t = (TextBox)sender;
            string val = t.Text;
            long rv = 0;
            if (val.Length > 0 && long.TryParse(val, out rv) && rv >= 0)
            {
                if (t.Name == "textBoxMinVal")
                    _MinVal = rv;
                else
                    _MaxVal = rv;

            }
            else
            {
                e.Cancel = val.Length > 0;
                if(e.Cancel)
                  MessageBox.Show("Only numbers greater than zero are accepted", "Validation error");
                
            }
        }

        private void frmRefNoRange_Load(object sender, EventArgs e)
        {
            long min, max = 0;
            ReferenceNumberManager.ReadRefNoRange();
            ReferenceNumberManager.GetRefNoRange(out min, out max);
            if (min>=0 && max>0)
            {
                _MinVal = min;
                _MaxVal = max;
                textBoxMinVal.Text = _MinVal.ToString();
                textBoxMaxVal.Text = _MaxVal.ToString();
            }
        }
    }
}

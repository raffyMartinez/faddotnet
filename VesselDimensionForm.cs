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
    public partial class VesselDimensionForm : Form
    {
        SamplingForm _Parent = new SamplingForm();

        public VesselDimensionForm()
        {
            InitializeComponent();
        }

        public SamplingForm Parent_Form
        {
            get { return _Parent; }
            set { _Parent = value; }
        }

        public void VesselDimension(string length, string width, string height)
        {
            textHeight.Text = height;
            textLength.Text = length;
            textWidth.Text = width;
        }

        private void textVessel_Validating(object sender, CancelEventArgs e)
        {
            var txt = (TextBox)sender;
            var s = txt.Text;
            var msg = "";
            var val = 0D;

            if(s.Length>0)
            {
                try
                {
                    val = double.Parse(s);
                }
                catch
                {
                    msg = "Value should be a number greater than zero";
                    e.Cancel = true;
                }
                finally
                {
                    e.Cancel = val <= 0;
                    if (e.Cancel)
                        msg = "Value should be a number greater than zero";       
                }
            }

            if (e.Cancel)
                MessageBox.Show(msg, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void buttonVessel_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (btn.Name == "buttonOk")
            {
                if (textLength.Text.Length>0 && textHeight.Text.Length>0 && textWidth.Text.Length>0)
                {
                    _Parent.VesselDimension(textLength.Text, textWidth.Text, textHeight.Text);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Please provide all values", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if(btn.Name=="buttonCancel")
            {
                this.Close();
            }
        }
    }
}

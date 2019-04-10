using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FAD3.Mapping.Forms
{
    public partial class Grid25GeographicDisplayOptionsForm : Form
    {
        public int MinorGridFontSize { get; set; }
        public double MinorGridDistance { get; set; }
        public int MajorGridFontSize { get; set; }
        public bool ShowSubgrid { get; set; }


        public Grid25GeographicDisplayOptionsForm(int minorGridFontSize, double minorGridDistance, int majorGridFontSize, bool showSubgrid)
        {
            MinorGridFontSize = minorGridFontSize;
            MinorGridDistance = minorGridDistance;
            MajorGridFontSize = majorGridFontSize;
            ShowSubgrid = showSubgrid;
            InitializeComponent();
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch(((Button)sender).Name)
            {
                case "btnOk":
                    if (txtMajorGridLabelSize.Text.Length > 0
                        && txtMinorGridLabelSize.Text.Length > 0)
                    {
                        MinorGridDistance = double.Parse(cboMinorGridDistance.Text);
                        MinorGridFontSize = int.Parse(txtMinorGridLabelSize.Text);
                        MajorGridFontSize = int.Parse(txtMajorGridLabelSize.Text);
                        ShowSubgrid = chkShowSubgrids.Checked;
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("Please provide all values", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;
                case "btnCancel":
                    DialogResult = DialogResult.Cancel;
                    break;
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            global.LoadFormSettings(this, true);
            cboMinorGridDistance.Items.Add("0");
            cboMinorGridDistance.Items.Add("0.5");
            cboMinorGridDistance.Items.Add("1");
            cboMinorGridDistance.Items.Add("1.5");
            cboMinorGridDistance.Items.Add("2");
            cboMinorGridDistance.Items.Add("2.5");
            cboMinorGridDistance.Items.Add("3");
            cboMinorGridDistance.Items.Add("3.5");
            cboMinorGridDistance.Items.Add("4");
            cboMinorGridDistance.Text = MinorGridDistance.ToString(); 
            txtMajorGridLabelSize.Text = MajorGridFontSize.ToString();
            txtMinorGridLabelSize.Text = MinorGridFontSize.ToString();
            chkShowSubgrids.Checked = ShowSubgrid;
        }

        private void OnFieldValidating(object sender, CancelEventArgs e)
        {
            string s = ((TextBox)sender).Text;
            if (s.Length > 0)
            {
                switch (((Control)sender).Name)
                {
                    case "txtMinorGridLabelSize":
                    case "txtMajorGridLabelSize":
                        if(int.TryParse(s,out int v))
                        {
                            if(v<0)
                            {
                                e.Cancel = true;
                            }
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                        break;
                }

                if(e.Cancel)
                {
                    MessageBox.Show("Only whole numbers greater than zero are accepted", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            global.SaveFormSettings(this);
        }
    }
}

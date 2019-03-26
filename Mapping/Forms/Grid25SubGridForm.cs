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
    public partial class Grid25SubGridForm : Form
    {
        public int SubGridCount { get; set; }
        public int SubGridSize { get; internal set; }

        public Grid25SubGridForm()
        {
            InitializeComponent();
        }

        private void OnCheckChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked)
            {
                switch (rb.Name)
                {
                    case "rbNone":
                        SubGridCount = 0;
                        SubGridSize = 0;
                        break;

                    case "rb4":
                        SubGridCount = 4;
                        SubGridSize = 1000;
                        break;

                    case "rb9":
                        SubGridCount = 9;
                        SubGridSize = 666;
                        break;

                    case "rb16":
                        SubGridCount = 16;
                        SubGridSize = 500;
                        break;

                    case "rb25":
                        SubGridCount = 25;
                        SubGridSize = 400;
                        break;
                }
                if (SubGridCount == 0)
                {
                    lblDescription.Text = "No subgrids will be created";
                }
                else
                {
                    lblDescription.Text = $"{SubGridCount} subgrids with sides {SubGridSize} meters will be created";
                }
            }
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":

                    DialogResult = DialogResult.OK;
                    break;

                case "btnCancel":
                    DialogResult = DialogResult.Cancel;
                    break;
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            switch (SubGridCount)
            {
                case 0:
                    rbNone.Checked = true;
                    break;

                case 4:
                    rb4.Checked = true;
                    break;

                case 9:
                    rb9.Checked = true;
                    break;

                case 16:
                    rb16.Checked = true;
                    break;

                case 25:
                    rb25.Checked = true;
                    break;
            }
        }
    }
}
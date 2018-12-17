using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FAD3.Database.Classes;

namespace FAD3.Database.Forms
{
    public partial class LGUItemForm : Form
    {
        private FisheriesInventoryLevel _level;
        public string LevelName { get; internal set; }

        public LGUItemForm(FisheriesInventoryLevel level)
        {
            InitializeComponent();
            _level = level;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            switch (_level)
            {
                case FisheriesInventoryLevel.Root:
                    label.Text = "Province";
                    break;

                case FisheriesInventoryLevel.Province:
                    label.Text = "Municipality";
                    break;

                case FisheriesInventoryLevel.Municipality:
                    label.Text = "Barangay";
                    break;

                case FisheriesInventoryLevel.Barangay:
                    label.Text = "Sitio";
                    break;
            }
        }

        private bool ValidateForm()
        {
            return textBoxLevel.Text.Length > 0;
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    if (ValidateForm())
                    {
                        LevelName = textBoxLevel.Text;
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("Please provide name of unit", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

                case "btnCancel":
                    DialogResult = DialogResult.Cancel;
                    break;
            }
        }
    }
}
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

namespace FAD3.Mapping.Forms
{
    public partial class SelectUTMZoneForm : Form
    {
        public fadUTMZone UTMZone { get; internal set; }
        public bool CreateFileWithoutInland { get; internal set; }

        public SelectUTMZoneForm()
        {
            InitializeComponent();
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnIgnore":
                    DialogResult = DialogResult.Ignore;
                    break;

                case "btnOk":
                    if (rbtnZone50N.Checked)
                    {
                        UTMZone = fadUTMZone.utmZone50N;
                    }
                    else if (rbtnZone51N.Checked)
                    {
                        UTMZone = fadUTMZone.utmZone51N;
                    }

                    CreateFileWithoutInland = chkCreateFile.Checked;

                    DialogResult = DialogResult.OK;

                    break;

                case "btnCancel":
                    DialogResult = DialogResult.Cancel;
                    break;
            }
        }
    }
}
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
using FAD3.GUI.Classes;

namespace FAD3.Mapping.Forms
{
    public partial class SelectUTMZoneForm : Form
    {
        public fadUTMZone UTMZone { get; internal set; }

        public SelectUTMZoneForm()
        {
            InitializeComponent();
        }
        private void SetUpTooltips()
        {
            // Create the ToolTip and associate with the Form container.
            ToolTip tt = new ToolTip();

            // Set up the delays for the ToolTip.
            tt.AutoPopDelay = TooltipGlobal.AutoPopDelay;
            tt.InitialDelay = TooltipGlobal.InitialDelay;
            tt.ReshowDelay = TooltipGlobal.ReshowDelay;

            // Force the ToolTip text to be displayed whether or not the form is active.
            tt.ShowAlways = TooltipGlobal.ShowAlways;

            tt.SetToolTip(rbtnZone50N, "For areas that are mostly in Palawan");
            tt.SetToolTip(rbtnZone51N, "For majority of the Philippines except Palawan");
            tt.SetToolTip(btnOk, "Closes the form and saves choice of UTM zone");
            tt.SetToolTip(btnIgnore, "Closes the form and ignores UTM zone so that inland points will not be blanked");
            tt.SetToolTip(btnCancel, "Closes the form");
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


                    DialogResult = DialogResult.OK;

                    break;

                case "btnCancel":
                    DialogResult = DialogResult.Cancel;
                    break;
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            SetUpTooltips();
        }
    }
}
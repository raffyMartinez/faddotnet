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
    public partial class AboutFadForm : Form
    {
        private List<String> componentList = new List<string>();

        public AboutFadForm()
        {
            InitializeComponent();
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
            global.LoadFormSettings(this, FormBorderStyle == FormBorderStyle.FixedToolWindow);
            labelApp.Text = "Fisheries Assessment Database 3";
            FontFamily ff = new FontFamily("Microsoft Sans Serif");
            labelApp.Font = new Font(ff, 13F, FontStyle.Bold);
            labelApp.Size = new Size(this.Width, labelApp.Height);
            labelApp.Location = new Point((this.Width - labelApp.Width) / 2, 25);
            labelApp.TextAlign = ContentAlignment.MiddleCenter;

            labelCredits.Text = "";
            if (global.HasMPH)
            {
                componentList.Add("DoubleMetaphone by Adam Nelson");
            }
            componentList.Add($"Mapwindows Mapping Components version {axMap.VersionNumber}\r\nwww.Mapwindows.org");
            componentList.Add($"Microsoft net framework: {Environment.Version}");
            foreach (var item in componentList)
            {
                labelCredits.Text += $"{item}\r\n";
            }
            labelCredits.Text = $"This software make use of the following {(componentList.Count == 1 ? "component:" : "components:")}\r\n{labelCredits.Text}\r\n";

            labelCredits.Text += "Written by: Raffy Martinez\r\n";
            labelCredits.Text += "raffy.martinez@gmail.com";
            labelCredits.Font = new Font(ff, 8F, FontStyle.Regular);
            labelCredits.Location = new Point((this.Width - labelCredits.Width) / 2, labelApp.Location.Y + (labelApp.Size.Height * 2));
            labelCredits.TextAlign = ContentAlignment.MiddleCenter;
        }

        private void AboutFadForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            global.SaveFormSettings(this);
        }
    }
}
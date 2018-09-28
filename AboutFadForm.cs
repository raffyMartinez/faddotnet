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
        public AboutFadForm()
        {
            InitializeComponent();
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
            labelApp.Text = "Fisheries Assessment Database 3";
            FontFamily ff = new FontFamily("Microsoft Sans Serif");
            labelApp.Font = new Font(ff, 13F, FontStyle.Bold);
            labelApp.Size = new Size(this.Width, labelApp.Height);
            labelApp.Location = new Point((this.Width - labelApp.Width) / 2, 25);
            labelApp.TextAlign = ContentAlignment.MiddleCenter;

            labelCredits.Text = "";
            if (global.HasMPH)
            {
                labelCredits.Text = "The following software comoponents are used by the software:\r\nDoubleMetaphone by Adam Nelson";
            }
            if (global.HasMPH)
            {
                labelCredits.Text += $"\r\nMapwindows Mapping Components version {axMap.VersionNumber}\r\nhttps://www.mapwindow.org";
            }
            else
            {
                labelCredits.Text += $"The following software comoponents are used by the software:\r\nMapwindows Mapping Components version {axMap.VersionNumber}\r\nhttps://www.mapwindow.org";
            }

            if (labelCredits.Text.Length > 0)
                labelCredits.Text += "\r\n\r\n";

            labelCredits.Text += "Written by: Raffy Martinez\r\n";
            labelCredits.Text += "raffy.martinez@gmail.com";
            labelCredits.Font = new Font(ff, 8F, FontStyle.Regular);
            labelCredits.Location = new Point((this.Width - labelCredits.Width) / 2, labelApp.Location.Y + (labelApp.Size.Height * 2));
            labelCredits.TextAlign = ContentAlignment.MiddleCenter;

            labelNetFramework.With(o =>
            {
                o.Location = new Point(0, labelCredits.Top + labelCredits.Height + 10);
                o.Text = $"Net framework: {Environment.Version}";
                o.Font = new Font(ff, 8F, FontStyle.Regular);
                o.TextAlign = ContentAlignment.MiddleCenter;
            });
        }
    }
}
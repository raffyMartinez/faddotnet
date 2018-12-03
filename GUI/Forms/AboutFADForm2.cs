using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FAD3.GUI.Forms
{
    public partial class AboutFADForm2 : Form
    {
        private List<String> componentList = new List<string>();

        public AboutFADForm2()
        {
            InitializeComponent();
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            //global.LoadFormSettings(this, FormBorderStyle == FormBorderStyle.FixedToolWindow);
            labelApp.Text = "Fisheries Assessment Database 3";
            FontFamily ff = new FontFamily("Microsoft Sans Serif");
            labelApp.Font = new Font(ff, 13F, FontStyle.Bold);
            labelApp.Size = new Size(this.Width, labelApp.Height);
            labelApp.Location = new Point((this.Width - labelApp.Width) / 2, 25);

            labelCredits.Text = "";
            if (global.HasMPH)
            {
                componentList.Add("DoubleMetaphone by Adam Nelson\r\n");
            }
            componentList.Add($"Microsoft net framework: {Environment.Version}\r\n");
            componentList.Add("Coordinate class by Jaime Olivares (http://github.com/jaime-olivares/coordinate)\r\n");
            componentList.Add("Lat-Long to UTM Converter (https://github.com/shahid28/utm-latlng)\r\n");
            componentList.Add("Ports of Fisher/Jenks natural breaks algorithm by Philipp Schöpf (https://github.com/pschoepf/naturalbreaks)\r\n");
            componentList.Add("SimMetrics - SimMetrics is a java library of Similarity or Distance Metrics by Sam Chapman (http://www.dcs.shef.ac.uk/~sam/stringmetrics.html)\r\n");
            foreach (var item in componentList)
            {
                labelCredits.Text += $"{item}\r\n";
            }
            labelCredits.Text = $"This software make use of the following {(componentList.Count == 1 ? "component:" : "components:")}\r\n{labelCredits.Text}\r\n";

            labelCredits.Text += "Mapping component is not installed.\r\nMapping functions are not available\r\n\r\n";

            labelCredits.Text += "Written by: Raffy Martinez\r\n";
            labelCredits.Text += "raffy.martinez@gmail.com";
            labelCredits.Font = new Font(ff, 8F, FontStyle.Regular);
            labelCredits.Location = new Point((this.Width - labelCredits.Width) / 2, labelApp.Location.Y + (labelApp.Size.Height * 2));
            Text = "About FAD";
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            //global.SaveFormSettings(this);
        }
    }
}
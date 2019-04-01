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
    public partial class FishingBoatForm : Form
    {
        public string TreeLevel { get; set; }
        private static FishingBoatForm _instance;
        public Landingsite LandingSite { get; set; }
        public TargetArea TargetArea { get; set; }
        private FishingBoats _fishingBoats;

        public static FishingBoatForm GetInstance()
        {
            if (_instance == null) _instance = new FishingBoatForm();
            return _instance;
        }
        public FishingBoatForm()
        {
            InitializeComponent();
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch(((Button)sender).Name)
            {
                case "btnClose":
                    Close();
                    break;
                case "btnAdd":
                    if (txtBoatName.Text.Length > 0 && TreeLevel=="landing_site")
                    {
                        Guid guid = Guid.NewGuid();
                        FishingBoat fb = new FishingBoat(guid,  LandingSite, txtBoatName.Text, txtOwnerName.Text);
                        if(txtHeight.Text.Length>0 
                            && txtWidth.Text.Length>0
                            && txtLength.Text.Length>0)
                        {
                            fb.Dimension(double.Parse(txtWidth.Text), Double.Parse(txtHeight.Text), double.Parse(txtLength.Text));
                        }
                        fb.Engine = txtEngineName.Text;
                        if(txtHp.Text.Length>0)
                        {
                            fb.EngineHp = double.Parse(txtHp.Text);
                        }
                        if(_fishingBoats.Add(fb))
                        {
                            lvBoats.Items.Add(fb.BoatGuid.ToString(), fb.BoatName, null);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Name of boat must be provided", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;
                case "btnRemove":
                    break;
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            _instance = null;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            switch(TreeLevel)
            {
                case "target_area":
                    _fishingBoats = new FishingBoats(TargetArea);
                    btnRemove.Visible = false;
                    btnAdd.ImageKey = "rightarrow";
                    btnRemove.ImageKey = "leftarrow";
                    break;
                case "landing_site":
                    btnAdd.Enabled = true;
                    btnRemove.Enabled = true;
                    txtLandingSite.Text = LandingSite.LandingSiteName;
                    _fishingBoats = new FishingBoats(LandingSite);
                    break;

            }
            foreach (FishingBoat fb in _fishingBoats)
            {
                lvBoats.Items.Add(fb.BoatGuid.ToString(), fb.BoatName, null);
            }
            txtLandingSite.Enabled = false;
        }

        private void OnLVMouseDown(object sender, MouseEventArgs e)
        {
            txtBoatName.Text = "";
            txtOwnerName.Text = "";
            txtLandingSite.Text = "";
            txtHeight.Text = "";
            txtLength.Text = "";
            txtWidth.Text = "";
            txtEngineName.Text = "";
            txtHp.Text = "";

            var item = lvBoats.HitTest(e.X,e.Y).Item;
            if(item!=null)
            {
                FishingBoat fb = _fishingBoats[item.Name];
                txtBoatName.Text = fb.BoatName;
                txtOwnerName.Text = fb.OwnerName;
                txtLandingSite.Text = fb.LandingSite.LandingSiteName;
                txtLength.Text = fb.BoatLength != null ? fb.BoatLength.ToString() : "";
                txtWidth.Text = fb.BoatWidth != null ? fb.BoatWidth.ToString() : "";
                txtHeight.Text = fb.BoatHeight != null ? fb.BoatHeight.ToString() : "";
                txtEngineName.Text = fb.Engine != null ? fb.Engine : "";
                txtHp.Text = fb.EngineHp != null ? fb.EngineHp.ToString() : "";
            }
        }

        private void OnTextValidating(object sender, CancelEventArgs e)
        {

        }
    }
}

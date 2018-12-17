using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FAD3.Database.Classes;

namespace FAD3
{
    public partial class FishingGroundDefinitionForm : Form
    {
        private fadUTMZone _UTMZone;
        private string _MapDescription;
        private string _ULGrid;
        private string _LRGrid;
        private TargetAreaForm _Parent_form;

        public fadUTMZone UTMZone
        {
            get { return _UTMZone; }
            set { _UTMZone = value; }
        }

        public FishingGroundDefinitionForm(TargetAreaForm Parent)
        {
            InitializeComponent();
            _Parent_form = Parent;
        }

        public FishingGroundDefinitionForm(TargetAreaForm Parent, fadUTMZone UTMZone, string MapDescription, string ULGrid, string LRGrid)
        {
            InitializeComponent();
            _MapDescription = MapDescription;
            _ULGrid = ULGrid;
            _LRGrid = LRGrid;
            textBoxDescription.Text = _MapDescription;
            textBoxLRGrid.Text = _LRGrid;
            textBoxULGrid.Text = _ULGrid;
            _Parent_form = Parent;
        }

        private void OntextBoxGrid_Validating(object sender, CancelEventArgs e)
        {
            ((TextBox)sender).With(o =>
            {
                var s = o.Text;
                var msg = "";
                if (s.Length > 0)
                {
                    switch (o.Name)
                    {
                        case "textBoxULGrid":
                        case "textBoxLRGrid":
                            e.Cancel = FishingGrid.ValidFGName(_UTMZone, o.Text, out msg) == false;
                            if (!e.Cancel)
                                o.Text = o.Text.ToUpper();

                            break;

                        case "textBoxDescription":
                            if (o.Text.Length < 6)
                            {
                                e.Cancel = true;
                                msg = "Description is too short";
                            }
                            break;
                    }
                }

                if (e.Cancel)
                    MessageBox.Show(msg, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            });
        }

        private void Onbutton_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonOK":
                    if (FishingGrid.ValidGridCorners(textBoxULGrid.Text, textBoxLRGrid.Text))
                    {
                        _Parent_form.SetFishingGround(textBoxDescription.Text,
                                                      textBoxULGrid.Text,
                                                      textBoxLRGrid.Text);
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Upper left grid must be at the left and top of lower right grid",
                                         "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;

                case "buttonCancel":
                    Close();
                    break;
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            global.LoadFormSettings(this, true);
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            global.SaveFormSettings(this);
        }
    }
}
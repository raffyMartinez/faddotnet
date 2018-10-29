using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FAD3.Mapping.Forms;
using FAD3.Mapping.Classes;
using System.IO;

namespace FAD3
{
    public partial class GraticuleForm : Form
    {
        private static GraticuleForm _instance;
        private MapperForm _parentForm;
        public Graticule Graticule { get; internal set; }

        public event EventHandler GraticuleRemoved;

        public static GraticuleForm GetInstance(MapperForm parent)
        {
            if (_instance == null) _instance = new GraticuleForm(parent);
            return _instance;
        }

        public GraticuleForm(MapperForm parent)
        {
            InitializeComponent();
            _parentForm = parent;
        }

        private void ShowGraticule()
        {
            SetAllLabelsPositionToAboveParent();
            Graticule.Configure(txtName.Text, int.Parse(txtLabelSize.Text), int.Parse(txtNumberOfGridlines.Text),
                                int.Parse(txtBordeWidth.Text), int.Parse(txtGridlineWidth.Text), chkShowGrid.Checked,
                                chkBold.Checked, chkLeft.Checked, chkRight.Checked, chkTop.Checked, chkBottom.Checked);
            Graticule.ShowGraticule();
        }

        private void SetAllLabelsPositionToAboveParent()
        {
            foreach (var layer in global.MappingForm.MapLayersHandler)
            {
                if (layer.Labels != null)
                {
                    layer.Labels.VerticalPosition = MapWinGIS.tkVerticalPosition.vpAboveParentLayer;
                }
            }
        }

        private void OnButton_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    ShowGraticule();
                    Close();
                    break;

                case "btnApply":
                    ShowGraticule();
                    break;

                case "btnCancel":
                    Close();
                    break;

                case "btnRemove":
                    GraticuleRemoved?.Invoke(this, EventArgs.Empty);
                    Close();
                    break;
            }
        }

        private void GraticuleForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
            _parentForm = null;
            CleanUp();
        }

        private void OnGraticuleForm_Load(object sender, EventArgs e)
        {
            txtName.Text = "Graticule";
            txtLabelSize.Text = "8";
            txtBordeWidth.Text = "2";
            txtGridlineWidth.Text = "1";
            txtNumberOfGridlines.Text = "5";
            chkBottom.Checked = true;
            chkTop.Checked = true;
            chkLeft.Checked = true;
            chkRight.Checked = true;
            chkShowGrid.Checked = true;
            Graticule = _parentForm.Graticule;
            Graticule.GraticuleExtentChanged += OnGraticuleExtentChanged;
            Graticule.MapRedrawNeeded += OnRedrawNeeded;
        }

        private void OnRedrawNeeded(object sender, EventArgs e)
        {
            RefreshPreview();
        }

        private void OnGraticuleExtentChanged(object sender, EventArgs e)
        {
            RefreshPreview();
        }

        private void OnCheckChange(object sender, EventArgs e)
        {
            var chkBox = (CheckBox)sender;
            switch (chkBox.Name)
            {
                case "chkTitle":
                    lnkTitle.Visible = chkBox.Checked;
                    break;

                case "chkNote":
                    lnkNote.Visible = chkBox.Checked;
                    break;
            }
        }

        private void OnLinkClick(object sender, LinkLabelLinkClickedEventArgs e)
        {
            fadMapTextType mapGraticuleTextType = fadMapTextType.mapTextTypeNone;
            switch (((LinkLabel)sender).Name)
            {
                case "lnkTitle":
                    mapGraticuleTextType = fadMapTextType.mapTextTypeTitle;
                    break;

                case "lnkNote":
                    mapGraticuleTextType = fadMapTextType.mapTextTypeNote;
                    break;
            }

            var configForm = GraticuleTextHelperForm.GetInstance(mapGraticuleTextType, this);
            if (configForm.Visible)
            {
                configForm.BringToFront();
            }
            else
            {
                configForm.Show(this);
            }
        }

        private void CleanUp()
        {
        }

        private void RefreshPreview()
        {
            if (picPreview.Image != null)
            {
                picPreview.Image.Dispose();
            }

            var tempFileName = global.MappingForm.SaveTempMapImage();
            if (tempFileName.Length > 0)
            {
                picPreview.ImageLocation = tempFileName;

                picPreview.Load();
                //Rectangle rect = picPreview.ClientRectangle;
                //Bitmap bmp = new Bitmap(rect.Width, rect.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                //Graphics g = Graphics.FromImage(bmp);
                //IntPtr ptr = g.GetHdc();

                //global.MappingForm.MapControl.SnapShotToDC(ptr, global.MappingForm.MapControl.Extents, picPreview.Width);
                //g.ReleaseHdc();

                //picPreview.Image = bmp;
                picPreview.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void OnTabsIndexChanged(object sender, EventArgs e)
        {
            switch (((TabControl)sender).SelectedTab.Name)
            {
                case "tabConfigureGrid":
                    break;

                case "tabConfigureText":
                    chkTitle.Enabled = _parentForm.Graticule.GridVisible;
                    chkNote.Enabled = chkTitle.Enabled;
                    RefreshPreview();

                    break;
            }
        }
    }
}
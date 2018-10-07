using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MapWinGIS;

namespace FAD3.Mapping.Forms
{
    public partial class PolygonLayerSymbologyForm : Form
    {
        private static PolygonLayerSymbologyForm _instance;
        private LayerPropertyForm _parentForm;
        private MapLayer _mapLayer;
        private ShapeDrawingOptions _options = null;
        private bool _noEvents;

        public static PolygonLayerSymbologyForm GetInstance(LayerPropertyForm parent, MapLayer mapLayer)
        {
            if (_instance == null) return new PolygonLayerSymbologyForm(parent, mapLayer);
            return _instance;
        }

        public PolygonLayerSymbologyForm(LayerPropertyForm parent, MapLayer mapLayer)
        {
            InitializeComponent();
            _parentForm = parent;
            _mapLayer = mapLayer;
            _options = ((Shapefile)_mapLayer.LayerObject).DefaultDrawingOptions;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            cboFillType.Items.Clear();
            cboFillType.Items.Add("Solid");
            cboFillType.Items.Add("Hatch");
            cboFillType.Items.Add("Gradient");
            cboFillType.SelectedIndex = 0;

            cboGradientType.Items.Clear();
            cboGradientType.Items.Add("Linear");
            cboGradientType.Items.Add("Rectangular");
            cboGradientType.Items.Add("Circle");

            cboGradientBounds.Items.Clear();
            cboGradientBounds.Items.Add("Whole layer");
            cboGradientBounds.Items.Add("Per-shape");

            cboVerticesType.Items.Clear();
            cboVerticesType.Items.Add("Square");
            cboVerticesType.Items.Add("Circle");

            icbHatchType.ComboStyle = UserControls.ImageComboStyle.HatchStyle;
            icbLineStyle.ComboStyle = UserControls.ImageComboStyle.LineStyle;
            icbLineWidth.ComboStyle = UserControls.ImageComboStyle.LineWidth;

            grpGradient.Location = grpHatch.Location;

            Options2GUI();
            DrawPreview();

            // -----------------------------------------------------
            // adding event handlers
            // -----------------------------------------------------
            // fill
            chkFill.CheckedChanged += new EventHandler(GUI2Options);
            clpFillColor.SelectedColorChanged += new EventHandler(GUI2Options);

            // hatch
            icbHatchType.SelectedIndexChanged += new EventHandler(GUI2Options);
            chkTransparentBackground.CheckedChanged += new EventHandler(GUI2Options);
            clpHatchBack.SelectedColorChanged += new EventHandler(GUI2Options);

            // gradient
            clpGradient2.SelectedColorChanged += new EventHandler(GUI2Options);
            udGradientRotation.ValueChanged += new EventHandler(GUI2Options);
            cboGradientType.SelectedIndexChanged += new EventHandler(GUI2Options);
            cboGradientBounds.SelectedIndexChanged += new EventHandler(GUI2Options);

            // outline
            chkOutline.CheckedChanged += new EventHandler(GUI2Options);
            icbLineStyle.SelectedIndexChanged += new EventHandler(GUI2Options);
            icbLineWidth.SelectedIndexChanged += new EventHandler(GUI2Options);
            clpOutline.SelectedColorChanged += new EventHandler(GUI2Options);

            // vertices
            chkVertices.CheckedChanged += new EventHandler(GUI2Options);
            cboVerticesType.SelectedIndexChanged += new EventHandler(GUI2Options);
            clpVertexColor.SelectedColorChanged += new EventHandler(GUI2Options);
            chkVerticesFillVisible.CheckedChanged += new EventHandler(GUI2Options);
            udVerticesSize.ValueChanged += new EventHandler(GUI2Options);

            transpFill.ValueChanged += OnTransparencyChange;
            transpOutline.ValueChanged += OnTransparencyChange;
        }

        private void OnTransparencyChange(object sender, byte value)
        {
            GUI2Options(null, null);
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
        }

        private void Options2GUI()
        {
            _noEvents = true;

            // options
            icbLineStyle.SelectedIndex = (int)_options.LineStipple;
            icbLineWidth.SelectedIndex = (int)_options.LineWidth - 1;
            cboFillType.SelectedIndex = (int)_options.FillType;
            chkOutline.Checked = _options.LineVisible;
            clpOutline.Color = Colors.UintToColor(_options.LineColor);
            chkFill.Checked = _options.FillVisible;

            // hatch
            icbHatchType.SelectedIndex = (int)_options.FillHatchStyle;
            chkTransparentBackground.Checked = _options.FillBgTransparent;
            clpHatchBack.Color = Colors.UintToColor(_options.FillBgColor);

            // gradient
            cboGradientType.SelectedIndex = (int)_options.FillGradientType;
            clpGradient2.Color = Colors.UintToColor(_options.FillColor2);
            udGradientRotation.Value = (decimal)_options.FillRotation;

            clpFillColor.Color = Colors.UintToColor(_options.FillColor);
            cboGradientBounds.SelectedIndex = (int)_options.FillGradientBounds;
            chkOutline.Checked = _options.LineVisible;

            // texture
            //udScaleX.SetValue(_options.PictureScaleX);
            //udScaleY.SetValue(_options.PictureScaleY);

            // vertices
            chkVertices.Checked = _options.VerticesVisible;
            chkVerticesFillVisible.Checked = _options.VerticesFillVisible;
            udVerticesSize.SetValue(_options.VerticesSize);
            clpVertexColor.Color = Colors.UintToColor(_options.VerticesColor);
            cboVerticesType.SelectedIndex = (int)_options.VerticesType;

            // transparency
            //cboFillTransparency.Text = ((int)((double)(_options.FillTransparency) / 2.55 + 0.5)).ToString();
            //cboLineTransparency.Text = ((int)((double)(_options.LineTransparency) / 2.55 + 0.5)).ToString();
            transpFill.Value = (byte)_options.FillTransparency;
            transpOutline.Value = (byte)_options.LineTransparency;

            _noEvents = false;
        }

        private void GUI2Options(object sender, EventArgs e)
        {
            if (_noEvents)
            {
                return;
            }

            // fill
            _options.FillVisible = chkFill.Checked;
            _options.FillType = (tkFillType)cboFillType.SelectedIndex;
            _options.FillColor = Colors.ColorToUInteger(clpFillColor.Color);

            // hatch
            _options.FillHatchStyle = (tkGDIPlusHatchStyle)icbHatchType.SelectedIndex;
            _options.FillBgTransparent = chkTransparentBackground.Checked;
            _options.FillBgColor = Colors.ColorToUInteger(clpHatchBack.Color);

            // gradient
            _options.FillGradientType = (tkGradientType)cboGradientType.SelectedIndex;
            _options.FillColor2 = Colors.ColorToUInteger(clpGradient2.Color);
            _options.FillRotation = (double)udGradientRotation.Value;
            _options.FillGradientBounds = (tkGradientBounds)cboGradientBounds.SelectedIndex;

            //// texture
            //_options.PictureScaleX = (double)udScaleX.Value;
            //_options.PictureScaleY = (double)udScaleY.Value;

            // outline
            _options.LineStipple = (tkDashStyle)icbLineStyle.SelectedIndex;
            _options.LineWidth = (float)icbLineWidth.SelectedIndex + 1;
            _options.LineVisible = chkOutline.Checked;
            _options.LineColor = Colors.ColorToUInteger(clpOutline.Color);

            // vertices
            _options.VerticesVisible = chkVertices.Checked;
            _options.VerticesFillVisible = chkVerticesFillVisible.Checked;
            _options.VerticesSize = (int)udVerticesSize.Value;
            _options.VerticesColor = Colors.ColorToUInteger(clpVertexColor.Color);
            _options.VerticesType = (tkVertexType)cboVerticesType.SelectedIndex;

            // transparency
            _options.LineTransparency = (float)transpOutline.Value;
            _options.FillTransparency = (float)transpFill.Value;

            btnApply.Enabled = true;

            DrawPreview();
        }

        private void DrawPreview()
        {
            if (_noEvents)
            {
                return;
            }

            if (pctPreview.Image != null)
            {
                pctPreview.Image.Dispose();
            }

            Rectangle rect = pctPreview.ClientRectangle;
            Bitmap bmp = new Bitmap(rect.Width, rect.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);
            IntPtr ptr = g.GetHdc();

            // creating shape to draw
            _options.DrawRectangle(ptr, 40.0f, 40.0f, rect.Width - 80, rect.Height - 80, true, rect.Width, rect.Height, Colors.ColorToUInteger(this.BackColor));

            g.ReleaseHdc();
            pctPreview.Image = bmp;
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            grpHatch.Visible = false;
            grpGradient.Visible = false;

            switch (cboFillType.SelectedIndex)
            {
                case 0:
                    clpFillColor.Visible = true;
                    break;

                case 1:
                    grpHatch.Visible = true;
                    break;

                case 2:
                    grpGradient.Visible = true;
                    break;
            }
        }
    }
}
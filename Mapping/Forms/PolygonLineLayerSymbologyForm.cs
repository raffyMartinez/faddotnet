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
    public partial class PolygonLineLayerSymbologyForm : Form
    {
        private static PolygonLineLayerSymbologyForm _instance;
        private LayerPropertyForm _parentForm;
        private MapLayer _mapLayer;
        private ShapeDrawingOptions _options = null;
        private bool _noEvents;
        private ShpfileType _shpFileType = ShpfileType.SHP_POLYGON;

        public static PolygonLineLayerSymbologyForm GetInstance(LayerPropertyForm parent, MapLayer mapLayer)
        {
            if (_instance == null) return new PolygonLineLayerSymbologyForm(parent, mapLayer);
            return _instance;
        }

        public PolygonLineLayerSymbologyForm(LayerPropertyForm parent, MapLayer mapLayer)
        {
            InitializeComponent();
            _parentForm = parent;
            _mapLayer = mapLayer;
            _options = ((Shapefile)_mapLayer.LayerObject).DefaultDrawingOptions;
            _shpFileType = ((Shapefile)_mapLayer.LayerObject).ShapefileType;
            LineWidthFix.FixLineWidth((Shapefile)_mapLayer.LayerObject);
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
            chkFill.CheckedChanged += GUI2Options;
            clpFillColor.SelectedColorChanged += GUI2Options;

            // hatch
            icbHatchType.SelectedIndexChanged += GUI2Options;
            chkTransparentBackground.CheckedChanged += GUI2Options;
            clpHatchBack.SelectedColorChanged += GUI2Options;

            // gradient
            clpGradient2.SelectedColorChanged += GUI2Options;
            udGradientRotation.ValueChanged += GUI2Options;
            cboGradientType.SelectedIndexChanged += GUI2Options;
            cboGradientBounds.SelectedIndexChanged += GUI2Options;

            // outline
            chkOutline.CheckedChanged += GUI2Options;
            icbLineStyle.SelectedIndexChanged += GUI2Options;
            icbLineWidth.SelectedIndexChanged += GUI2Options;
            clpOutline.SelectedColorChanged += GUI2Options;

            // vertices
            chkVertices.CheckedChanged += GUI2Options;
            cboVerticesType.SelectedIndexChanged += GUI2Options;
            clpVertexColor.SelectedColorChanged += GUI2Options;
            chkVerticesFillVisible.CheckedChanged += GUI2Options;
            udVerticesSize.ValueChanged += GUI2Options;

            transpFill.ValueChanged += OnTransparencyChange;
            transpOutline.ValueChanged += OnTransparencyChange;

            if (_shpFileType != ShpfileType.SHP_POLYGON)
            {
                tabsProperties.TabPages.Remove(tabsProperties.TabPages["tabFill"]);
                chkVertices.Location = chkOutline.Location;
                chkOutline.Location = chkFill.Location;
                chkFill.Visible = false;
                Text = "Polyline layer symbology";
            }
            else
            {
                Text = "Polygon layer symbology";
            }
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
            LineWidthFix.FixLineWidth((Shapefile)_mapLayer.LayerObject);

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
            if (_shpFileType == ShpfileType.SHP_POLYGON)
            {
                _options.DrawRectangle(ptr, 40.0f, 40.0f, rect.Width - 80, rect.Height - 80, true, rect.Width, rect.Height, Colors.ColorToUInteger(this.BackColor));
            }
            else
            {
                _options.DrawLine(ptr, 40.0f, 40.0f, rect.Width - 80, rect.Height - 80, true, rect.Width, rect.Height, Colors.ColorToUInteger(this.BackColor));
            }

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

        private void ApplyOptions()
        {
            _parentForm.Parentform.ShapefileLayerPropertyChanged();
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    ApplyOptions();
                    Close();
                    break;

                case "btnCancel":
                    Close();
                    break;

                case "btnApply":
                    ApplyOptions();
                    break;
            }
        }
    }
}
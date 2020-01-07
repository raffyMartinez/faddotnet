using FAD3.Mapping.UserControls;
using MapWinGIS;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FAD3
{
    public partial class PointLayerSymbologyForm : Form
    {
        private static PointLayerSymbologyForm _instance;
        private MapLayer _mapLayer;
        private Shapefile _shapeFile;
        private ShpfileType _shpFileType;
        private ShapeDrawingOptions _options;
        private bool _noEvents;
        private LayerPropertyForm _parentForm;
        private ShapeDrawingOptions _originalOptions;

        public static PointLayerSymbologyForm GetInstance(LayerPropertyForm parent, MapLayer mapLayer)
        {
            if (_instance == null) _instance = new PointLayerSymbologyForm(parent, mapLayer);
            return _instance;
        }

        public PointLayerSymbologyForm(LayerPropertyForm parent, MapLayer mapLayer)
        {
            InitializeComponent();
            _mapLayer = mapLayer;
            _shapeFile = mapLayer.LayerObject as Shapefile;
            _shpFileType = _shapeFile.ShapefileType;
            _options = _shapeFile.DefaultDrawingOptions;
            _originalOptions = _options;
            _parentForm = parent;
        }

        private void RefreshCharacterMap(string fontName)
        {
            characterControl1.SetFontName(fontName);
        }

        private void DrawPreview()
        {
            if (_noEvents)
            {
                return;
            }

            if (picPreview.Image != null)
            {
                picPreview.Image.Dispose();
            }

            Rectangle rect = picPreview.ClientRectangle;
            Bitmap bmp = new Bitmap(rect.Width, rect.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);
            IntPtr ptr = g.GetHdc();
            //int ptr = g.GetHdc().ToInt32();

            // creating shape to draw
            _options.DrawPoint(ptr, 0.0f, 0.0f, rect.Width, rect.Height, FAD3.Mapping.Colors.ColorToUInteger(this.BackColor));

            g.ReleaseHdc();
            picPreview.Image = bmp;
        }

        private void LoadCharacterFonts()
        {
            foreach (System.Drawing.FontFamily family in System.Drawing.FontFamily.Families)
            {
                string name = family.Name.ToLower();

                if (name == "webdings" ||
                    name == "wingdings" ||
                    name == "wingdings 2" ||
                    name == "wingdings 3")
                {
                    comboCharacterFont.Items.Add(family.Name);
                    comboCharacterFont.SelectedIndex = 0;
                }
            }

            RefreshCharacterMap(comboCharacterFont.Text);
        }

        private void ShapeDrawOptionsToGUI()
        {
            _noEvents = true;

            udSize.SetValue(_options.PointSize);
            udRotation.SetValue(_options.PointRotation);
            rectSymbolColor.FillColor = FAD3.Mapping.Colors.UintToColor(_options.FillColor);

            //point
            comboPointType.SelectedIndex = (int)_options.PointShape;
            udNumberOfSides.SetValue(_options.PointSidesCount);
            udSideRatio.SetValue(_options.PointSidesRatio * 10);

            //appearance
            comboLineWidth.SelectedIndex = (int)_options.LineWidth - 1;
            transpFillColor.Value = (byte)_options.FillTransparency;
            chkFillVisible.Checked = _options.FillVisible;
            chkOutlineVisible.Checked = _options.LineVisible;
            rectOutlineColor.FillColor = FAD3.Mapping.Colors.UintToColor(_options.LineColor);

            var color = Mapping.Colors.UintToColor(_options.FillColor);
            symbolControl1.ForeColor = color;
            characterControl1.ForeColor = color;
            transpFillColor.BandColor = color;
            comboPointType.Color1 = color;

            characterControl1.SelectedCharacterCode = (byte)_options.PointCharacter;

            _noEvents = false;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            _noEvents = true;
            comboPointType.ComboStyle = ImageComboStyle.PointShape;
            comboLineWidth.ComboStyle = ImageComboStyle.LineWidth;
            characterControl1.SelectionChanged += OnCharacterControlSelectionChanged;
            symbolControl1.SelectionChanged += OnSymbolControlSelectionChanged;
            global.LoadFormSettings(this, true);
            LoadCharacterFonts();
            ShapeDrawOptionsToGUI();
            _noEvents = false;
            DrawPreview();
        }

        private void OnSymbolControlSelectionChanged()
        {
            tkDefaultPointSymbol symbol = (tkDefaultPointSymbol)symbolControl1.SelectedIndex;
            _options.SetDefaultPointSymbol(symbol);

            if (!_noEvents)
                btnApply.Enabled = true;

            ShapeDrawOptionsToGUI();
            DrawPreview();
        }

        private void OnCharacterControlSelectionChanged()
        {
            if (!_noEvents)
                btnApply.Enabled = true;

            _options.PointType = tkPointSymbolType.ptSymbolFontCharacter;
            _options.PointCharacter = Convert.ToInt16(characterControl1.SelectedCharacterCode);
            DrawPreview();
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            global.SaveFormSettings(this);
            _instance = null;
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

                case "btnApply":
                    ApplyOptions();
                    break;

                case "btnCancel":
                    _options = _originalOptions;
                    _parentForm.Parentform.ShapefileLayerPropertyChanged();
                    Close();
                    break;
            }
        }

        private void OnComboFontSelectionIndexChanged(object sender, EventArgs e)
        {
            if (!_noEvents)
            {
                btnApply.Enabled = true;
            }
            _options.FontName = comboCharacterFont.Text;
            RefreshCharacterMap(comboCharacterFont.Text);
        }

        private void ApplyOptionsToGUI(object sender, EventArgs e)
        {
            if (_noEvents)
            {
                return;
            }

            _options.PointSize = (float)udSize.Value;
            _options.PointRotation = (double)udRotation.Value;
            _options.FillTransparency = (float)transpFillColor.Value;
            _options.PointSidesCount = (int)udNumberOfSides.Value;
            _options.PointSidesRatio = (float)udSideRatio.Value / 10;
            _options.PointShape = (tkPointShapeType)comboPointType.SelectedIndex;

            _options.FillColor = Mapping.Colors.ColorToUInteger(rectSymbolColor.FillColor);
            _options.LineColor = Mapping.Colors.ColorToUInteger(rectOutlineColor.FillColor);

            _options.FillVisible = chkFillVisible.Checked;
            _options.LineVisible = chkOutlineVisible.Checked;

            _options.LineWidth = (float)comboLineWidth.SelectedIndex + 1;

            DrawPreview();
        }

        private void OnColorDoubleClick(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog
            {
                AllowFullOpen = true,
            };
            colorDialog.ShowDialog();
            var rect = (Microsoft.VisualBasic.PowerPacks.RectangleShape)sender;
            rect.FillColor = colorDialog.Color;

            if (rect.Name == "rectSymbolColor")
            {
                characterControl1.ForeColor = rect.FillColor;
                symbolControl1.ForeColor = rect.FillColor;
                comboPointType.Color1 = rect.FillColor;
                transpFillColor.BandColor = rect.FillColor;
            }
            else if (rect.Name == "rectOutlineColor")
            {
            }
            ApplyOptionsToGUI(null, null);
        }

        private void OnTransparencyValueChanged(object sender, byte value)
        {
            ApplyOptionsToGUI(null, null);
        }
    }
}
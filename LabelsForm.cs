using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cyotek.Windows.Forms;
using MapWinGIS;

namespace FAD3
{
    public partial class LabelsForm : Form
    {
        private Shapefile _shapeFile;
        private FontComboBox _fontComboBox;
        private static LabelsForm _instance;
        private MapLayer _mapLayer;

        public static LabelsForm GetInstance(MapLayer mapLayer)
        {
            if (_instance == null) return new LabelsForm(mapLayer);
            return _instance;
        }

        public LabelsForm(MapLayer mapLayer)
        {
            InitializeComponent();
            _mapLayer = mapLayer;
            _shapeFile = (Shapefile)_mapLayer.LayerObject;
        }

        private void CleanUp()
        {
            _shapeFile = null;
            _fontComboBox = null;
            _mapLayer = null;
        }

        private uint ColorToUInt(Color clr)
        {
            return (uint)(clr.R + (clr.G << 8) + (clr.B << 16));
        }

        private void FillListboxFields()
        {
            for (int n = 0; n < _shapeFile.NumFields; n++)
            {
                listboxFields.Items.Add(_shapeFile.Field[n].Name);
            }
        }

        private void FillComboVerticalPosition(ComboBox cbo)
        {
            var kv = new KeyValuePair<tkVerticalPosition, string>(tkVerticalPosition.vpAboveAllLayers, "Above all layers");
            cbo.Items.Add(kv);

            kv = new KeyValuePair<tkVerticalPosition, string>(tkVerticalPosition.vpAboveParentLayer, "Above parent layer");
            cbo.Items.Add(kv);
        }

        private void FillComboFrameType(ComboBox cbo)
        {
            var kv = new KeyValuePair<tkLabelFrameType, string>(tkLabelFrameType.lfRectangle, "Rectangle");
            cbo.Items.Add(kv);

            kv = new KeyValuePair<tkLabelFrameType, string>(tkLabelFrameType.lfRoundedRectangle, "Rounded rectangle");
            cbo.Items.Add(kv);

            kv = new KeyValuePair<tkLabelFrameType, string>(tkLabelFrameType.lfPointedRectangle, "Pointed rectangle");
            cbo.Items.Add(kv);
        }

        private Color UIntToColor(uint color)
        {
            byte a = (byte)(color >> 24);
            byte r = (byte)(color >> 16);
            byte g = (byte)(color >> 8);
            byte b = (byte)(color >> 0);
            return Color.FromArgb(a, r, g, b);
        }

        private void TestExpression(string expression)
        {
            var result = ShapefileLabelHandler.TestExpression(expression, (Shapefile)_mapLayer.LayerObject);
            if (result.isValid)
            {
                //txtLabelSourceField.Text = _mapLayer.Expression;
                lblResult.Text = "Valid expression";
                lblResult.ForeColor = Color.Green;
            }
            else
            {
                lblResult.Text = result.message;
                lblResult.ForeColor = Color.Red;
            }
        }

        private void SetUpUI()
        {
            //source field tab
            FillListboxFields();
            chkLayerIsLabeled.Checked = _mapLayer.IsLabeled;
            if (chkLayerIsLabeled.Checked)
            {
                lblResult.Text = string.Empty;
                if (_mapLayer.LabelSource == "Expression")
                {
                    txtLabelSourceField.Text = _mapLayer.Expression;
                    TestExpression(_mapLayer.Expression);
                }
                else
                {
                    txtLabelSourceField.Text = (string)listboxFields.Items[_mapLayer.LabelField];
                    lblResult.Text = "No expression";
                    lblResult.ForeColor = Color.Black;
                }
            }
            chkLabelsVisible.Checked = _shapeFile.Labels.Visible;
            chkAvoidCollision.Checked = _shapeFile.Labels.AvoidCollisions;
            txtCollisionBuffer.Text = _shapeFile.Labels.CollisionBuffer.ToString();
            chkRemoveDuplicates.Checked = _shapeFile.Labels.RemoveDuplicates;
            chkAutoOffset.Checked = _shapeFile.Labels.AutoOffset;
            txtFontOffsetX.Text = _shapeFile.Labels.OffsetX.ToString();
            txtFontOffsetY.Text = _shapeFile.Labels.OffsetY.ToString();
            FillComboVerticalPosition(comboVerticalPosition);
            comboVerticalPosition.SelectedIndex = (int)_shapeFile.Labels.VerticalPosition;
            comboVerticalPosition.DisplayMember = "value";

            //font tab
            _fontComboBox.Text = _shapeFile.Labels.FontName;
            txtFontSize.Text = _shapeFile.Labels.FontSize.ToString();
            txtFontTransparency.Text = _shapeFile.Labels.FontTransparency.ToString();
            chkItalic.Checked = _shapeFile.Labels.FontItalic;
            chkUnderline.Checked = _shapeFile.Labels.FontUnderline;
            chkBold.Checked = _shapeFile.Labels.FontBold;
            chkOutlineVisible.Checked = _shapeFile.Labels.FontOutlineVisible;
            rectFontColor.FillColor = FADColors.UintToColor(_shapeFile.Labels.FontColor);
            rectOutlineColor.FillColor = FADColors.UintToColor(_shapeFile.Labels.FontOutlineColor);
            switch (_shapeFile.Labels.Alignment)
            {
                case tkLabelAlignment.laTopLeft:
                    optionTL.Checked = true;
                    break;

                case tkLabelAlignment.laTopCenter:
                    optionTC.Checked = true;
                    break;

                case tkLabelAlignment.laTopRight:
                    optionTR.Checked = true;
                    break;

                case tkLabelAlignment.laCenterLeft:
                    optionCL.Checked = true;
                    break;

                case tkLabelAlignment.laCenter:
                    optionCC.Checked = true;
                    break;

                case tkLabelAlignment.laCenterRight:
                    optionCR.Checked = true;
                    break;

                case tkLabelAlignment.laBottomLeft:
                    optionBL.Checked = true;
                    break;

                case tkLabelAlignment.laBottomCenter:
                    optionBC.Checked = true;
                    break;

                case tkLabelAlignment.laBottomRight:
                    optionBL.Checked = true;
                    break;
            }

            //frame tab
            chkFrameVisible.Checked = _shapeFile.Labels.FrameVisible;
            FillComboFrameType(comboFrameType);
            comboFrameType.DisplayMember = "value";
            comboFrameType.SelectedIndex = (int)_shapeFile.Labels.FrameType;
            txtFramePaddingX.Text = _shapeFile.Labels.FramePaddingX.ToString();
            txtFramePaddingY.Text = _shapeFile.Labels.FramePaddingY.ToString();
            txtFrameTransparency.Text = _shapeFile.Labels.FrameTransparency.ToString();
            txtFrameLineWidth.Text = _shapeFile.Labels.FrameOutlineWidth.ToString();
            rectFrameLineColor.FillColor = FADColors.UintToColor(_shapeFile.Labels.FrameOutlineColor);
            rectFrameFillColor.FillColor = FADColors.UintToColor(_shapeFile.Labels.FrameBackColor);
            switch (_shapeFile.Labels.InboxAlignment)
            {
                case tkLabelAlignment.laTopLeft:
                    optionFrameTL.Checked = true;
                    break;

                case tkLabelAlignment.laTopCenter:
                    optionFrameTC.Checked = true;
                    break;

                case tkLabelAlignment.laTopRight:
                    optionFrameTR.Checked = true;
                    break;

                case tkLabelAlignment.laCenterLeft:
                    optionFrameCL.Checked = true;
                    break;

                case tkLabelAlignment.laCenter:
                    optionFrameCC.Checked = true;
                    break;

                case tkLabelAlignment.laCenterRight:
                    optionFrameCR.Checked = true;
                    break;

                case tkLabelAlignment.laBottomLeft:
                    optionFrameBL.Checked = true;
                    break;

                case tkLabelAlignment.laBottomCenter:
                    optionFrameBC.Checked = true;
                    break;

                case tkLabelAlignment.laBottomRight:
                    optionFrameBR.Checked = true;
                    break;
            }

            //decorations
            chkHaloVisible.Checked = _shapeFile.Labels.HaloVisible;
            chkShadowVisible.Checked = _shapeFile.Labels.ShadowVisible;
            rectHaloColor.FillColor = FADColors.UintToColor(_shapeFile.Labels.HaloColor);
            rectShadowColor.FillColor = FADColors.UintToColor(_shapeFile.Labels.ShadowColor);
            txtHaloSize.Text = _shapeFile.Labels.HaloSize.ToString();
            txtShadowOffsetX.Text = _shapeFile.Labels.ShadowOffsetX.ToString();
            txtShadowOffsetY.Text = _shapeFile.Labels.ShadowOffsetY.ToString();

            //visibility
            txtVisibilityExpression.Text = _shapeFile.Labels.VisibilityExpression;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            _fontComboBox = new FontComboBox();
            tabFont.Controls.Add(_fontComboBox);
            _fontComboBox.Location = new System.Drawing.Point(95, 14);
            _fontComboBox.Width = 155;
            SetUpUI();
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _mapLayer.IsLabeled = _shapeFile.Labels.Count > 0;
            if (_mapLayer.IsLabeled && txtLabelSourceField.Text.Length > 0)
            {
                _mapLayer.LabelSource = "Expression";
                _mapLayer.Expression = txtLabelSourceField.Text;
            }
            CleanUp();
            _instance = null;
        }

        private void OnShapeDoubleClick(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog
            {
                AllowFullOpen = true,
            };
            colorDialog.ShowDialog();
            var rect = (Microsoft.VisualBasic.PowerPacks.RectangleShape)sender;
            rect.FillColor = colorDialog.Color;
        }

        private void ApplyLabelProperties()
        {
            if (!chkLayerIsLabeled.Checked)
            {
                _shapeFile.Labels.Clear();
            }
            else
            {
                if (!chkLabelsVisible.Checked)
                {
                    _shapeFile.Labels.Visible = false;
                }
                else
                {
                    _shapeFile.Labels.Visible = true;
                    _shapeFile.Labels.Expression = $"[{listboxFields.Text}]";
                    _shapeFile.Labels.RemoveDuplicates = chkRemoveDuplicates.Checked;
                    _shapeFile.Labels.AvoidCollisions = chkAvoidCollision.Checked;
                    _shapeFile.Labels.CollisionBuffer = int.Parse(txtCollisionBuffer.Text);
                    _shapeFile.Labels.AutoOffset = chkAutoOffset.Checked;
                    _shapeFile.Labels.OffsetX = double.Parse(txtFontOffsetX.Text);
                    _shapeFile.Labels.OffsetY = double.Parse(txtFontOffsetY.Text);
                    _shapeFile.Labels.VerticalPosition = ((KeyValuePair<tkVerticalPosition, string>)comboVerticalPosition.SelectedItem).Key;

                    _shapeFile.Labels.FontName = _fontComboBox.Text;
                    _shapeFile.Labels.FontTransparency = int.Parse(txtFrameTransparency.Text);
                    _shapeFile.Labels.FontSize = int.Parse(txtFontSize.Text);
                    _shapeFile.Labels.FontBold = chkBold.Checked;
                    _shapeFile.Labels.FontItalic = chkItalic.Checked;
                    _shapeFile.Labels.FontUnderline = chkUnderline.Checked;
                    _shapeFile.Labels.FontOutlineVisible = chkOutlineVisible.Checked;
                    _shapeFile.Labels.FontOutlineColor = FADColors.ColorToUInteger(rectOutlineColor.FillColor);
                    _shapeFile.Labels.FontColor = FADColors.ColorToUInteger(rectFontColor.FillColor);

                    if (optionTL.Checked)
                        _shapeFile.Labels.Alignment = tkLabelAlignment.laTopLeft;
                    else if (optionTC.Checked)
                        _shapeFile.Labels.Alignment = tkLabelAlignment.laTopCenter;
                    else if (optionTR.Checked)
                        _shapeFile.Labels.Alignment = tkLabelAlignment.laTopRight;
                    else if (optionCL.Checked)
                        _shapeFile.Labels.Alignment = tkLabelAlignment.laCenterLeft;
                    else if (optionCC.Checked)
                        _shapeFile.Labels.Alignment = tkLabelAlignment.laCenter;
                    else if (optionCR.Checked)
                        _shapeFile.Labels.Alignment = tkLabelAlignment.laCenterRight;
                    else if (optionBL.Checked)
                        _shapeFile.Labels.Alignment = tkLabelAlignment.laBottomLeft;
                    else if (optionBC.Checked)
                        _shapeFile.Labels.Alignment = tkLabelAlignment.laBottomCenter;
                    else if (optionBR.Checked)
                        _shapeFile.Labels.Alignment = tkLabelAlignment.laBottomRight;

                    _shapeFile.Labels.FrameVisible = chkFrameVisible.Checked;
                    _shapeFile.Labels.FrameTransparency = int.Parse(txtFrameTransparency.Text);
                    _shapeFile.Labels.FrameOutlineWidth = int.Parse(txtFrameLineWidth.Text);
                    _shapeFile.Labels.FrameType = ((KeyValuePair<tkLabelFrameType, string>)comboFrameType.SelectedItem).Key;
                    _shapeFile.Labels.FrameBackColor = FADColors.ColorToUInteger(rectFrameFillColor.FillColor);
                    _shapeFile.Labels.FrameOutlineColor = FADColors.ColorToUInteger(rectFrameLineColor.FillColor);
                    _shapeFile.Labels.FramePaddingX = int.Parse(txtFramePaddingX.Text);
                    _shapeFile.Labels.FramePaddingY = int.Parse(txtFramePaddingY.Text);

                    if (optionFrameTL.Checked)
                        _shapeFile.Labels.InboxAlignment = tkLabelAlignment.laTopLeft;
                    else if (optionFrameTC.Checked)
                        _shapeFile.Labels.InboxAlignment = tkLabelAlignment.laTopCenter;
                    else if (optionFrameTR.Checked)
                        _shapeFile.Labels.InboxAlignment = tkLabelAlignment.laTopRight;
                    else if (optionFrameCL.Checked)
                        _shapeFile.Labels.InboxAlignment = tkLabelAlignment.laCenterLeft;
                    else if (optionFrameCC.Checked)
                        _shapeFile.Labels.InboxAlignment = tkLabelAlignment.laCenter;
                    else if (optionFrameCR.Checked)
                        _shapeFile.Labels.InboxAlignment = tkLabelAlignment.laCenterRight;
                    else if (optionFrameBL.Checked)
                        _shapeFile.Labels.InboxAlignment = tkLabelAlignment.laBottomLeft;
                    else if (optionFrameBC.Checked)
                        _shapeFile.Labels.InboxAlignment = tkLabelAlignment.laBottomCenter;
                    else if (optionFrameBR.Checked)
                        _shapeFile.Labels.InboxAlignment = tkLabelAlignment.laBottomRight;

                    _shapeFile.Labels.HaloVisible = chkHaloVisible.Checked;
                    _shapeFile.Labels.HaloSize = int.Parse(txtHaloSize.Text);
                    _shapeFile.Labels.HaloColor = FADColors.ColorToUInteger(rectHaloColor.FillColor);

                    _shapeFile.Labels.ShadowVisible = chkShadowVisible.Checked;
                    _shapeFile.Labels.ShadowColor = FADColors.ColorToUInteger(rectShadowColor.FillColor);
                    _shapeFile.Labels.ShadowOffsetX = int.Parse(txtShadowOffsetX.Text);
                    _shapeFile.Labels.ShadowOffsetY = int.Parse(txtShadowOffsetY.Text);

                    if (_mapLayer.LabelsVisibilityExpression?.Length > 0)
                    {
                        _shapeFile.Labels.VisibilityExpression = _mapLayer.LabelsVisibilityExpression;
                    }

                    if (_shapeFile.Labels.Count == 0)
                    {
                        _shapeFile.Labels.Generate(_shapeFile.Labels.Expression, tkLabelPositioning.lpCenter, true);
                    }
                    else
                    {
                        if (_shapeFile.Labels.Expression != txtLabelSourceField.Text)
                        {
                            _shapeFile.Labels.Expression = ShapefileLabelHandler.FixExpression(txtLabelSourceField.Text);
                        }
                    }
                }
            }
            global.MappingForm.MapControl.Redraw();
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    ApplyLabelProperties();
                    Close();
                    break;

                case "btnApply":
                    ApplyLabelProperties();
                    break;

                case "btnCancel":
                    Close();
                    break;

                case "btnClear":
                    PromptRemoveLabels();
                    break;

                case "btnNewLine":
                    txtLabelSourceField.Text += Environment.NewLine;
                    break;

                case "btnQuotes":
                    txtLabelSourceField.Text += "\" \"";
                    break;

                case "btnPlus":
                    txtLabelSourceField.Text += " + ";
                    break;

                case "btnDefineVisibilityExpression":
                    var visibilityQueryForm = VisibilityQueryForm.GetInstance(_mapLayer);
                    visibilityQueryForm.VisibilityExpression = txtVisibilityExpression.Text;
                    visibilityQueryForm.LabelVisibilityMode = true;
                    if (!visibilityQueryForm.Visible)
                    {
                        visibilityQueryForm.Show(this);
                    }
                    else
                    {
                        visibilityQueryForm.BringToFront();
                    }

                    break;
            }
        }

        private void PromptRemoveLabels()
        {
            if (MessageBox.Show("Remove labels?", "FAD3",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _shapeFile.Labels.Clear();
                txtLabelSourceField.Text = string.Empty;
                //Labels lb = _shapeFile.Labels;
                //for (int i = 0; i < lb.Count; i++)
                //{
                //    for (int j = 0; j < lb.get_NumParts(i); j++)
                //    {
                //        lb.get_Label(i, j).Text = "";
                //    }
                //}

                //listBox1.Enabled = true;
                //btnPlus.Enabled = true;
                //btnQuotes.Enabled = true;
                //btnNewLine.Enabled = true;
                //richTextBox1.Enabled = true;
                //richTextBox1.Text = "";

                //lb.SavingMode = tkSavingMode.modeXMLOverwrite;
                //lb.Synchronized = true;
                //if (!lb.Synchronized)
                //{
                //    lb.Clear();
                //}
            }
        }

        private void OnListFieldsDblCLick(object sender, EventArgs e)
        {
            txtLabelSourceField.Text = $"[{listboxFields.Text}]";
        }

        private void txtLabelSourceField_TextChanged(object sender, EventArgs e)
        {
            TestExpression(txtLabelSourceField.Text);
        }
    }
}
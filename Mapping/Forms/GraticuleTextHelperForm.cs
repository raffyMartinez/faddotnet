using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FAD3.Mapping.Classes;

namespace FAD3.Mapping.Forms
{
    public partial class GraticuleTextHelperForm : Form
    {
        private static GraticuleTextHelperForm _instance;
        private fadMapTextType _mapTextType;
        private GraticuleForm _parentForm;

        public static GraticuleTextHelperForm GetInstance(fadMapTextType mapText, GraticuleForm parentForm)
        {
            if (_instance == null) _instance = new GraticuleTextHelperForm(mapText, parentForm);
            return _instance;
        }

        public GraticuleTextHelperForm(fadMapTextType mapTextType, GraticuleForm parentForm)
        {
            InitializeComponent();
            _mapTextType = mapTextType;
            _parentForm = parentForm;
        }

        private void OnGraticuleTextHelperForm_Closed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
        }

        private void OnGraticuleTextHelperForm_Load(object sender, EventArgs e)
        {
            var textHelper = global.MappingForm.Graticule.GraticuleTextHelper;
            switch (_mapTextType)
            {
                case fadMapTextType.mapTextTypeNote:
                    Text = "Configure note";
                    chkBold.Visible = false;
                    udFontSize.Minimum = 7;
                    udFontSize.Maximum = 12;
                    udFontSize.Value = 10;

                    txtText.Text = textHelper.NoteText;
                    break;

                case fadMapTextType.mapTextTypeTitle:
                    Text = "Configure title";
                    udFontSize.Minimum = 12;
                    udFontSize.Maximum = 15;
                    udFontSize.Value = 13;

                    txtText.Text = textHelper.TitleText;
                    break;
            }
            cboTextAlign.Items.Add("Left");
            cboTextAlign.Items.Add("Center");
            cboTextAlign.Items.Add("Right");
            cboTextAlign.SelectedIndex = 1;
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    if (txtText.Text.Length > 0)
                    {
                        var textHelper = _parentForm.Graticule.GraticuleTextHelper;
                        textHelper.MapTextType = _mapTextType;
                        textHelper.TextBold = chkBold.Checked;
                        textHelper.TextSize = (int)udFontSize.Value;
                        switch (cboTextAlign.Text)
                        {
                            case "Left":
                                textHelper.TextAlignment = MapWinGIS.tkLabelAlignment.laCenterLeft;
                                break;

                            case "Center":
                                textHelper.TextAlignment = MapWinGIS.tkLabelAlignment.laCenter;
                                break;

                            case "Right":
                                textHelper.TextAlignment = MapWinGIS.tkLabelAlignment.laCenterRight;
                                break;
                        }
                        textHelper.SetText(txtText.Text);
                        Close();
                    }
                    else
                    {
                        if (_mapTextType == fadMapTextType.mapTextTypeTitle)
                        {
                            MessageBox.Show("Map title cannot be blank");
                        }
                        else
                        {
                            MessageBox.Show("Note cannot be blank");
                        }
                    }

                    break;

                case "btnCancel":
                    Close();
                    break;
            }
        }
    }
}
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
    public partial class SamplingForm : Form
    {
        private Dictionary<string, sampling.UserInterfaceStructure> _uis = new Dictionary<string, sampling.UserInterfaceStructure>();
        private sampling _sampling;
        private string _samplingGUID = "";
        private ListView _lv;
        private Control _topControl;
        private string _AOIGuid = "";
        private int _widestLabel = 0;
        private int _controlWidth = 200;
        private int _yPos;
        private string _AOIName = "";
        private string _landingSiteName = "";
        private string _landingSiteGuid = "";
        private string _gearClassName = "";
        private string _gearClassGuid = "";
        private string _gearVarName = "";
        private string _gearVarGuid = "";
        private string _gearRefCode = "";
        private MainForm _parent;
        private bool _isNew;
        private aoi _aoi;
        private TextBox _txtVesselDimension = new TextBox();

        private string _vesLength = "";
        private string _vesWidth = "";
        private string _vesHeight = "";

        private string _enumeratorGuid = "";

        private string _datePrompt = "";
        private string _timePrompt = "";
        private DateTime _samplingDate;
        private bool _samplingDateSet = false;
        private DateTime _dateSetAdjust;

        private string _newReferenceNumber = "";

        private bool _sampledGearSpecIsEdited;

        private List<string> _fishingGrounds;

        public bool SampledGearSpecIsEdited
        {
            get { return _sampledGearSpecIsEdited; }
            set
            {
                _sampledGearSpecIsEdited = value;
                if (_sampledGearSpecIsEdited)
                    panelUI.Controls["textGearSpecs"].Text = ManageGearSpecsClass.PreSavedSampledGearSpec();
            }
        }

        public void NewReferenceNumber(string newRefNumber)
        {
            _newReferenceNumber = newRefNumber;
            panelUI.Controls["textReferenceNumber"].Text = _newReferenceNumber;
        }

        public List<string> FishingGrounds
        {
            get { return _fishingGrounds; }
            set
            {
                _fishingGrounds = value;

                if (_fishingGrounds.Count > 0)
                {
                    ((TextBox)panelUI.Controls["textFishingGround"]).Text = _fishingGrounds[0];

                    if (_fishingGrounds.Count > 1)
                        ((TextBox)panelUI.Controls["textAdditionalFishingGround"]).With(o =>
                        {
                            o.Clear();
                            for (int i = 1; i < _fishingGrounds.Count; i++)
                                o.Text += _fishingGrounds[i] + ", ";

                            o.Text = o.Text.Substring(0, o.Text.Length - 2);
                        });
                }
            }
        }

        public void VesselDimension(string length, string width, string height)
        {
            _vesHeight = height;
            _vesLength = length;
            _vesWidth = width;

            try
            {
                _txtVesselDimension.Text = "(LxWxH): " + _vesLength + " x " + _vesWidth + " x " + _vesHeight;
            }
            catch
            {
                ;
            }
        }

        public string AOIName
        {
            get { return _AOIName; }
            set { _AOIName = value; }
        }

        public string AOIGuid
        {
            get { return _AOIGuid; }
            set { _AOIGuid = value; }
        }

        public string LandingSiteName
        {
            get { return _landingSiteName; }
            set { _landingSiteName = value; }
        }

        public string LandingSiteGuid
        {
            get { return _landingSiteGuid; }
            set { _landingSiteGuid = value; }
        }

        public string GearClassName
        {
            get { return _gearClassName; }
            set { _gearClassName = value; }
        }

        public string GearClassGuid
        {
            get { return _gearClassGuid; }
            set { _gearClassGuid = value; }
        }

        public string GearVarName
        {
            get { return _gearVarName; }
            set { _gearVarName = value; }
        }

        public string GearVarGuid
        {
            get { return _gearVarGuid; }
            set { _gearVarGuid = value; }
        }

        public aoi AOI
        {
            get { return _aoi; }
            set { _aoi = value; }
        }

        public MainForm Parent_Form
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public bool IsNew
        {
            get { return _isNew; }
            set { _isNew = value; }
        }

        public string SamplingGUID
        {
            get { return _samplingGUID; }
            set { _samplingGUID = value; }
        }

        public void ListViewSamplingDetail(ListView lv)
        {
            _lv = lv;
        }

        public SamplingForm()
        {
            InitializeComponent();
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            global.SaveFormSettings(this);
            ManageGearSpecsClass.SampledGearSpecs.Clear();
            if (IsNew) _parent.NewSamplingDataEntryCancelled();
        }

        private void OnUIRowRead(object sender, UIRowFromXML e)
        {
            int ht = 16;
            int x = 0;

            int Spacing = 15;
            string cType;
            int SizeAdjust = 50;
            string ButtonText;
            int ControlHt = 0;
            Control ctl = new Control();

            //setup the labels
            Label lbl = new Label
            {
                Name = "label" + e.Key,
                Text = e.RowLabel,
                AutoSize = true,
                Size = new System.Drawing.Size(79, ht),
                Tag = e.Key
            };
            panelUI.Controls.Add(lbl);
            lbl.Location = new Point(x, _yPos);
            if (lbl.Width > _widestLabel)
                _widestLabel = lbl.Width;

            try
            {
                cType = e.Control.ToString();
            }
            catch
            {
                cType = "";
            }

            //setup error labels
            if (cType != "Spacer")
            {
                Font f = new Font(lbl.Font.FontFamily, 8, FontStyle.Bold);
                Label lblError = new Label()
                {
                    Name = "errLabel" + e.Key,
                    Text = "!",
                    AutoSize = true,
                    Size = new Size(3, 40),
                    Font = f,
                    Tag = e.Key,
                    Visible = false,
                    ForeColor = Color.Red
                };
                panelUI.Controls.Add(lblError);
                lblError.Location = new Point(x, _yPos);
            }

            //setup the field controls
            switch (cType)
            {
                case "TextBox":
                    ctl = new TextBox
                    {
                        Name = "text" + e.Key
                    };
                    ((TextBox)ctl).ReadOnly = e.ReadOnly;
                    break;

                case "ComboBox":
                    ctl = new ComboBox
                    {
                        Name = "combo" + e.Key,
                    };

                    switch (e.Key)
                    {
                        case "Enumerator":
                            //((ComboBox)ctl).DataSource = new BindingSource(_aoi.Enumerators, null);
                            Enumerators.AOIEnumeratorsList(_AOIGuid, (ComboBox)ctl);
                            break;

                        case "TargetArea":
                            //((ComboBox)ctl).DataSource = new BindingSource(_aoi.AOIs, null);
                            aoi.getAOIsEx((ComboBox)ctl);
                            break;

                        case "LandingSite":
                            //((ComboBox)ctl).DataSource = new BindingSource(_aoi.LandingSites, null);
                            aoi.LandingSitesFromAOI(_AOIGuid, (ComboBox)ctl);
                            break;

                        case "GearClass":
                            //((ComboBox)ctl).DataSource = new BindingSource(global.GearClass, null);
                            gear.GetGearClassEx((ComboBox)ctl);
                            break;

                        case "FishingGear":
                            if (!_isNew)
                            {
                                gear.GearClassUsed = _lv.Items["GearClass"].Tag.ToString();
                            }

                            if (_gearClassGuid.Length == 0)
                            {
                                if (_isNew)
                                {
                                    _gearClassGuid = ((KeyValuePair<string, string>)((ComboBox)panelUI.Controls["comboGearClass"]).Items[0]).Key;
                                }
                                else
                                {
                                    _gearClassGuid = gear.GearClassUsed;
                                }
                            }

                            gear.GearVariationsUsage(_gearClassGuid, _AOIGuid, (ComboBox)ctl);
                            //var MySource = global.GearVariationsUsage(_GearClassGuid, _AOIGuid, (ComboBox)ctl);
                            //if (MySource.Count > 0)
                            //{
                            //    ((ComboBox)ctl).DataSource = new BindingSource(MySource, null);
                            //}
                            break;

                        case "TypeOfVesselUsed":
                            //System.Diagnostics.Debug.Assert(global.VesselTypeDict.Count > 0, "source has no rows");
                            ((ComboBox)ctl).DataSource = new BindingSource(global.VesselTypeDict, null);
                            break;
                    }
                    ((ComboBox)ctl).With(o =>
                    {
                        o.DisplayMember = "Value";
                        o.ValueMember = "Key";
                        o.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                        o.AutoCompleteSource = AutoCompleteSource.ListItems;
                    });
                    break;

                case "Spacer":
                    ControlHt = 35;
                    break;

                case "DateMask":
                    ctl = new MaskedTextBox
                    {
                        Name = "dtxt" + e.Key,
                    };

                    ((MaskedTextBox)ctl).With(o =>
                    {
                        o.Mask = "LLL-00-0000";
                        o.TextMaskFormat = MaskFormat.IncludePromptAndLiterals;
                        if (_datePrompt.Length == 0)
                            _datePrompt = o.Text;
                    });

                    break;

                case "TimeMask":
                    ctl = new MaskedTextBox
                    {
                        Name = "ttxt" + e.Key,
                    };

                    ((MaskedTextBox)ctl).With(o =>
                    {
                        o.Mask = "00:00";
                        o.TextMaskFormat = MaskFormat.IncludePromptAndLiterals;
                        if (_timePrompt.Length == 0)
                            _timePrompt = o.Text;
                    });

                    break;

                case "Check":
                    ctl = new CheckBox
                    {
                        Name = "chk" + e.Key
                    };
                    break;

                default:
                    break;
            }

            System.Type type = ctl.GetType();
            if (type.Name != "Control" && cType != "Spacer")
            {
                ToolTip tt = new ToolTip
                {
                    ToolTipTitle = ctl.Text
                };
                tt.SetToolTip(ctl, e.ToolTip);

                panelUI.Controls.Add(ctl);

                ctl.With(o =>
                 {
                     o.Location = new System.Drawing.Point(x, _yPos);
                     o.Tag = e.Key;
                     if (type.Name != "CheckBox")
                     {
                         o.Size = new System.Drawing.Size(_controlWidth, ht + SizeAdjust);
                         if (e.Height != 1)
                         {
                             o.Size = new System.Drawing.Size(_controlWidth, ht * e.Height);
                             ((TextBox)o).Multiline = true;
                         }
                     }
                     ControlHt = o.Height;
                 });

                if (!_isNew)
                {
                    if (type.Name != "CheckBox")
                    {
                        ctl.Text = _lv.Items[e.Key].SubItems[1].Text;

                        switch (e.Key)
                        {
                            case "Enumerator":
                                _enumeratorGuid = ((KeyValuePair<string, string>)((ComboBox)ctl).SelectedItem).Key;
                                break;

                            case "FishingGear":
                                _gearVarName = ctl.Text;
                                _gearVarGuid = ((KeyValuePair<string, string>)((ComboBox)ctl).SelectedItem).Key;
                                break;

                            case "ReferenceNumber":
                                var arr = ctl.Text.Split('-');
                                _gearRefCode = arr[1];
                                break;

                            case "TargetArea":
                                _AOIName = ctl.Text;
                                _AOIGuid = ((KeyValuePair<string, string>)((ComboBox)ctl).SelectedItem).Key;
                                break;

                            case "AdditionalFishingGround":
                                //var myList = FishingGrid.AdditionalFishingGrounds(_samplingGUID);

                                //foreach (var item in myList)
                                //    ((TextBox)ctl).Text += item + "\r\n";
                                break;

                            case "GearSpecs":
                                ctl.Text = ManageGearSpecsClass.GetSampledSpecsEx(_samplingGUID);
                                break;
                        }
                    }
                    else
                    {
                        ((CheckBox)ctl).Checked = _lv.Items[e.Key].SubItems[1].Text == "True";
                    }
                }
                else
                {
                    switch (e.Key)
                    {
                        case "SamplingDate":
                            if (_isNew)
                                _topControl = ctl;
                            break;

                        case "TargetArea":
                            ctl.Text = _AOIName;
                            break;

                        case "LandingSite":
                            ctl.Text = _landingSiteName;
                            break;

                        case "GearClass":
                            if (_gearClassName.Length > 0)
                                ctl.Text = _gearClassName;
                            else
                            {
                                ((ComboBox)ctl).With(o =>
                                {
                                    var item = o.Items[0];
                                    o.SelectedItem = item;
                                });
                            }
                            _gearClassGuid = ((KeyValuePair<string, string>)((ComboBox)ctl).SelectedItem).Key;
                            break;

                        case "FishingGear":
                            if (_gearVarName.Length > 0)
                                ctl.Text = _gearVarName;
                            else
                            {
                                ((ComboBox)ctl).With(o =>
                                {
                                    if (o.Items.Count > 0)
                                    {
                                        var item = o.Items[0];
                                        o.SelectedItem = item; //combobox text is set here
                                    }
                                });
                            }

                            if (ctl.Text.Length > 0)
                                _gearVarGuid = ((KeyValuePair<string, string>)((ComboBox)ctl).SelectedItem).Key;

                            break;

                        case "Enumerator":
                            ctl.Text = "";
                            break;

                        case "TypeOfVesselUsed":
                            ctl.Text = "Motorized";
                            break;
                    }
                }
                if (type.Name == "ComboBox")
                {
                    ((ComboBox)ctl).SelectionLength = 0;
                    ((ComboBox)ctl).SelectionStart = 0;
                }

                //define the events that the fields will respond to
                ctl.Validating += OnFieldValidate;
                ctl.TextChanged += OnFieldChange;
                ctl.Validated += OnFieldValidated;
                ctl.GotFocus += OnFieldGotFocus;

                if (type.Name == "ComboBox")
                    ((ComboBox)ctl).SelectedIndexChanged += OnComboSelectedIndexChanged;
                else if (cType == "DateMask")
                    ((MaskedTextBox)ctl).KeyDown += OnmaskedText_KeyDown;

                if (e.Key == "VesselDimension")
                    _txtVesselDimension = (TextBox)ctl;
            }

            //making the controls not visible speeds up form drawing
            ctl.Visible = false;

            //setup the buttons
            try
            {
                ButtonText = e.ButtonText;
            }
            catch
            {
                ButtonText = "";
            }
            if (ButtonText.Length > 0)
            {
                Button btn = new Button
                {
                    Name = "btn" + e.Key,
                    Text = ButtonText,
                    Size = new System.Drawing.Size(_controlWidth / 2, (int)(ht * 1.7)),
                    Tag = e.Key
                };
                panelUI.Controls.Add(btn);
                btn.Location = new Point(x, _yPos - 3);
                btn.Click += OnbuttonSamplingFields_Click;

                switch (e.Key)
                {
                    case "Enumerator":
                        if (!IsNew) _topControl = btn;
                        break;

                    case "ReferenceNumber":
                        btn.Enabled = IsNew;
                        break;
                }
            }
            _yPos += ControlHt + Spacing;
        }

        private void OnFieldGotFocus(object sender, EventArgs e)
        {
            switch (((Control)sender).Tag.ToString())
            {
                case "DateSet":
                case "DateHauled":
                    if (!_samplingDateSet)
                    {
                        var theSamplingDate = GetFieldText("SamplingDate");
                        if (DateTime.TryParse(theSamplingDate, out _samplingDate))
                        {
                            _samplingDateSet = true;
                        }
                    }
                    _dateSetAdjust = _samplingDate;
                    break;
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            this.Size = new Size(Width, _lv.Height);
            global.LoadFormSettings(this, true);
            _sampling = new sampling();
            _sampling.OnUIRowRead += new sampling.ReadUIElement(OnUIRowRead);

            panelUI.SuspendLayout();
            _sampling.ReadUIFromXML();
            AdustControlsPosition();
            panelUI.ResumeLayout();

            if (_isNew)
                Text = "New sampling";
            else
            {
                Text = "Sampling detail";
                _gearClassName = _lv.Items["GearClass"].SubItems[1].Text;
            }

            SetFieldsVisible();
            buttonCancel.Visible = true;
            buttonOK.Visible = true;

            CancelButton = buttonCancel;
            _topControl.Select();
        }

        private void SetFieldsVisible()
        {
            foreach (Control ctl in panelUI.Controls)
            {
                var tName = ctl.GetType().Name;
                if (tName != "Button" && tName != "Label")
                {
                    ctl.Visible = true;
                }
            }
        }

        private void AdustControlsPosition()
        {
            foreach (Control c in panelUI.Controls)
            {
                System.Type type = c.GetType();
                if (type.Name != "Label")
                {
                    if (type.Name == "Button")
                    {
                        c.Left = _widestLabel + _controlWidth + 40;
                    }
                    else
                    {
                        c.Left = _widestLabel + 25;
                    }
                }
                else if (c.Name.Substring(0, 8) == "errLabel")
                {
                    c.Left = _widestLabel + _controlWidth + 25;
                }
            }
        }

        private bool SaveEdits()
        {
            var EffortData = new Dictionary<string, string>();
            foreach (Control c in panelUI.Controls)
            {
                var typeName = c.GetType().Name;
                if (typeName != "Button" && typeName != "Label")
                {
                    switch (typeName)
                    {
                        case "ComboBox":
                            var key = "";
                            if (c.Name != "comboTypeOfVesselUsed")
                            {
                                if (c.Text.Length > 0)
                                {
                                    key = ((KeyValuePair<string, string>)((ComboBox)c).SelectedItem).Key;
                                }
                            }
                            else
                            {
                                if (c.Text.Length > 0)
                                {
                                    var cbo = ((ComboBox)panelUI.Controls["comboTypeOfVesselUsed"]);
                                    if (cbo.Items.Count > 0)
                                    {
                                        var v = int.Parse(cbo.SelectedValue.ToString());
                                        key = v.ToString();
                                    }
                                }
                            }
                            EffortData.Add(c.Tag.ToString(), key);
                            break;

                        case "TextBox":
                            EffortData.Add(c.Tag.ToString(), c.Text);
                            break;

                        case "MaskedTextBox":
                            // var s = "";
                            var s = ((MaskedTextBox)c).MaskCompleted ? c.Text : "";
                            EffortData.Add(c.Tag.ToString(), s);
                            break;

                        case "CheckBox":
                            EffortData.Add(c.Tag.ToString(), ((CheckBox)c).Checked.ToString());
                            break;
                    }
                }
            }

            EffortData.Add("VesLength", _vesLength);
            EffortData.Add("VesHeight", _vesHeight);
            EffortData.Add("VesWidth", _vesWidth);

            if (_isNew)
                _samplingGUID = Guid.NewGuid().ToString();
            EffortData.Add("SamplingGUID", _samplingGUID);

            EffortData.Add("SamplingType", "1");

            PopulateFGList();

            if (_sampling.UpdateEffort(_isNew, EffortData, _fishingGrounds))
                return ManageGearSpecsClass.SaveSampledGearSpecs();
            else
                return false;
        }

        private void OnbuttonSamplingFields_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "btnEnumerator":
                    var ef = EnumeratorForm.GetInstance(_enumeratorGuid);
                    if (!ef.Visible)
                    {
                        ef.Show(this);
                    }
                    else
                    {
                        ef.BringToFront();
                        ef.SetParent(_enumeratorGuid);
                    }
                    break;

                case "btnGearClass":
                    GearCodesUsageForm form = GearCodesUsageForm.GetInstance(_gearVarGuid, _AOIGuid, _gearClassName);
                    if (!form.Visible)
                    {
                        form.Show(this);
                    }
                    else
                    {
                        form.BringToFront();
                    }

                    form.With(o =>
                    {
                        //o.GearClassName = _GearClassName;
                        o.GearRefCode = _gearRefCode;
                        o.Parent_Form = this;
                        o.TargetArea(_AOIName, _AOIGuid);
                        o.GearVariation(_gearVarName, _gearVarGuid);
                    });
                    break;

                case "btnGearSpecs":
                    SampledGear_SpecsForm sgf = SampledGear_SpecsForm.GetInstance(_gearVarGuid, _gearVarName, this);

                    if (!sgf.Visible)
                    {
                        sgf.Show(this);
                    }
                    else
                    {
                        sgf.BringToFront();
                    }
                    sgf.SamplingGUID = _samplingGUID;
                    break;

                case "btnVesselDimension":
                    VesselDimensionForm f = new VesselDimensionForm();
                    if (_vesLength.Length > 0 && _vesHeight.Length > 0 && _vesWidth.Length > 0)
                        f.VesselDimension(_vesLength, _vesWidth, _vesHeight);
                    f.Parent_Form = this;
                    f.ShowDialog(this);
                    break;

                case "btnReferenceNumber":
                    if (CheckRequiredForRefNumber())
                    {
                        var txt = (MaskedTextBox)panelUI.Controls["dtxtSamplingDate"];
                        ReferenceNumberManager.SetAOI_GearVariation(_AOIGuid, _gearVarGuid, DateTime.Parse(txt.Text));
                        ReferenceNumberForm grf = new ReferenceNumberForm();
                        grf.Parent_Form = this;
                        grf.ShowDialog(this);
                    }
                    else
                    {
                        ShowReferenceNumberRequiredErrorLabel();
                        MessageBox.Show("Some required fields not filled up", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

                case "btnFishingGround":
                    PopulateFGList();
                    FishingGroundForm fg = new FishingGroundForm(_AOIGuid, this);
                    fg.FishingGrounds = _fishingGrounds;
                    fg.Show(this);
                    break;
            }
        }

        private void ShowReferenceNumberRequiredErrorLabel()
        {
            foreach (Control c in panelUI.Controls)
            {
                if (c.Name.Substring(0, 8) == "errLabel")
                {
                    switch (c.Tag.ToString())
                    {
                        case "SamplingDate":
                        case "SamplingTime":
                        case "GearClass":
                        case "TypeOfVesselUsed":
                        case "FishingGear":
                        case "Enumerator":
                        case "TargetArea":
                        case "LandingSite":
                            c.Visible = true;
                            break;
                    }
                }
            }
        }

        private bool CheckRequiredForRefNumber()
        {
            var valid = true;
            foreach (Control c in panelUI.Controls)
            {
                if (c.GetType().Name != "Label" && c.GetType().Name != "Button")
                {
                    switch (c.Tag.ToString())
                    {
                        case "SamplingDate":
                            valid = (c.Text != _datePrompt);
                            break;

                        case "SamplingTime":
                            valid = (c.Text != _timePrompt);
                            break;

                        case "GearClass":
                        case "TypeOfVesselUsed":
                        case "FishingGear":
                        case "Enumerator":
                        case "TargetArea":
                        case "LandingSite":
                            if (c.Text.Length == 0)
                            {
                                valid = false;
                            }

                            break;
                    }

                    if (!valid) break;
                }
            }
            return valid;
        }

        private void PopulateFGList()
        {
            _fishingGrounds = new List<string>();
            ((TextBox)panelUI.Controls["textFishingGround"]).With(o =>
            {
                if (o.Text.Length > 0)
                {
                    _fishingGrounds.Add(o.Text);
                }
            });

            var t = (TextBox)panelUI.Controls["textAdditionalFishingGround"];
            var arr = t.Text.Length > 0 ? t.Text.Split(',') : null;
            for (int n = 0; arr != null && n < arr.Length; n++)
                _fishingGrounds.Add(arr[n].Trim());
        }

        private void OnbuttonSampling_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "buttonOK":
                    if (ValidateForm())
                    {
                        if (SaveEdits())
                        {
                            if (IsNew) ReferenceNumberManager.UpdateRefCodeCounter();
                            _parent.RefreshCatchDetail(_samplingGUID, _isNew, _samplingDate, _gearVarGuid, _landingSiteGuid);
                            this.Close();
                        }
                    }
                    break;

                case "buttonCancel":
                    if (IsNew) _parent.NewSamplingDataEntryCancelled();
                    this.Close();
                    break;
            }
        }

        private void ShowRequiredErrorLabel(bool Visible)
        {
            foreach (Control c in panelUI.Controls)
            {
                if (c.Name.Substring(0, 8) == "errLabel")
                {
                    sampling.UserInterfaceStructure ui = sampling.uis[c.Tag.ToString()];
                    c.Visible = ui.Required && Visible;
                }
            }
        }

        private void ShowGearDateTimeErrorLabel(bool Visible)
        {
            foreach (Control c in panelUI.Controls)
            {
                if (c.Name == "errLabelDateSet" || c.Name == "errLabelTimeSet" ||
                    c.Name == "errLabelDateHauled" || c.Name == "errLabelTimeHauled")
                {
                    c.Visible = Visible;
                }
            }
        }

        private void ShowSamplingGearDateTimeErrorLabel(bool Visible)
        {
            foreach (Control c in panelUI.Controls)
            {
                if (c.Name == "errLabelDateSet" || c.Name == "errLabelTimeSet" ||
                    c.Name == "errLabelDateHauled" || c.Name == "errLabelTimeHauled" ||
                    c.Name == "errLabelSamplingDate" || c.Name == "errLabelSamplingTime")
                {
                    c.Visible = Visible;
                }
            }
        }

        private void ShowWeightError(bool Visible)
        {
            foreach (Control c in panelUI.Controls)
            {
                if (c.Name == "errLabelWeightOfCatch" || c.Name == "errLabelWeightOfSample")
                {
                    c.Visible = Visible;
                }
            }
        }

        private bool ValidateForm()
        {
            bool isValidated = true;
            bool GearDateTimeIsEmpty = false;
            string msg = "";

            //Step 1. All required fields should be present
            foreach (Control c in panelUI.Controls)
            {
                Type t = c.GetType();
                if (t.Name != "Label" && t.Name != "Button")
                {
                    //we want to get the UserInterfaceStructure element specified
                    //in the tag of the control
                    sampling.UserInterfaceStructure ui = sampling.uis[c.Tag.ToString()];

                    if (ui.Required && c.Text.Length == 0)
                    {
                        isValidated = false;
                        msg = "A required field is missing";
                        ShowRequiredErrorLabel(true);
                        break;
                    }
                }
            }

            //Step 2. Datetime for set and haul should all be filled in or should all be blank
            if (isValidated)
            {
                isValidated = (
                        panelUI.Controls["dtxtDateSet"].Text != _datePrompt &&
                        panelUI.Controls["ttxtTimeSet"].Text != _timePrompt &&
                        panelUI.Controls["dtxtDateHauled"].Text != _datePrompt &&
                        panelUI.Controls["ttxtTimeHauled"].Text != _timePrompt
                    );
                if (!isValidated)
                {
                    GearDateTimeIsEmpty = (
                        panelUI.Controls["dtxtDateSet"].Text == _datePrompt &&
                        panelUI.Controls["ttxtTimeSet"].Text == _timePrompt &&
                        panelUI.Controls["dtxtDateHauled"].Text == _datePrompt &&
                        panelUI.Controls["ttxtTimeHauled"].Text == _timePrompt
                        );

                    isValidated = GearDateTimeIsEmpty;
                    if (!isValidated)
                    {
                        ShowGearDateTimeErrorLabel(true);
                        msg = "Date and times of gear set and haul should \r\n" +
                              "all be filled in or should all be blank";
                    }
                    else
                    {
                        //if catch weight > zero then confirm if datetime of gear set and haul is empty
                        if (panelUI.Controls["textWeightOfCatch"].Text.Length > 0 && double.Parse(panelUI.Controls["textWeightOfCatch"].Text) > 0 && GearDateTimeIsEmpty)
                        {
                            ShowGearDateTimeErrorLabel(true);
                            DialogResult dr = MessageBox.Show("Confirm that date and time of gear set\r\n" +
                                                               "and gear haul are empty", "Please validate",
                                                               MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                            isValidated = (dr == DialogResult.Yes);
                        }
                    }
                }
            }

            //Step 3. Datetime for set and haul should be before sampling date and time and
            //   Date set should be before date hauled
            if (isValidated && GearDateTimeIsEmpty == false)
            {
                DateTime samplingDate = DateTime.Parse(panelUI.Controls["dtxtSamplingDate"].Text.ToString());
                samplingDate = samplingDate.Add(TimeSpan.Parse(panelUI.Controls["ttxtSamplingTime"].Text.ToString()));

                DateTime setDate = DateTime.Parse(panelUI.Controls["dtxtDateSet"].Text.ToString());
                setDate = setDate.Add(TimeSpan.Parse(panelUI.Controls["ttxtTimeSet"].Text.ToString()));

                DateTime haulDate = DateTime.Parse(panelUI.Controls["dtxtDateHauled"].Text.ToString());
                haulDate = haulDate.Add(TimeSpan.Parse(panelUI.Controls["ttxtTimeHauled"].Text.ToString()));

                isValidated = (samplingDate > setDate && samplingDate > haulDate);
                if (isValidated)
                {
                    isValidated = haulDate > setDate;
                    if (!isValidated)
                    {
                        msg = "Date of gear set should be before date of gear haul";
                        ShowGearDateTimeErrorLabel(true);
                    }
                }
                else
                {
                    msg = "Sampling date should be after date of gear set and haul";
                    ShowSamplingGearDateTimeErrorLabel(true);
                }
            }

            //Step 4. There should be catch weight if datetime of gear set and haul is given
            if (isValidated && !GearDateTimeIsEmpty)
            {
                isValidated = (panelUI.Controls["textWeightOfCatch"].Text.Length > 0);
                if (!isValidated)
                {
                    msg = "Weight of catch could not be blank";
                }
            }

            //Step 5. Weight of sample cannot be more than weight of catch
            if (isValidated && panelUI.Controls["textWeightOfCatch"].Text.Length > 0 &&
                panelUI.Controls["textWeightOfSample"].Text.Length > 0)
            {
                isValidated = (double.Parse(panelUI.Controls["textWeightOfCatch"].Text)
                    >= double.Parse(panelUI.Controls["textWeightOfSample"].Text));
                if (!isValidated)
                {
                    ShowWeightError(true);
                    msg = "Weight of catch cannot be less than weight of sample";
                }
            }

            TextBox catchWt = (TextBox)panelUI.Controls["textWeightOfCatch"];
            TextBox fishingGround = (TextBox)panelUI.Controls["textFishingGround"];

            //Step 6. if catch weight is blank then confirm it
            if (isValidated && GearDateTimeIsEmpty && catchWt.Text.Length == 0)
            {
                DialogResult dr = MessageBox.Show("Confirm that weight of catch is blank", "Please validate",
                                                   MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                isValidated = (dr == DialogResult.Yes);
                if (!isValidated) catchWt.Focus();
            }

            //Step 7. If catch is not blank then confirm if fishing ground is blank
            if (isValidated && catchWt.Text.Length > 0)
            {
                isValidated = fishingGround.Text.Length > 0;
                if (!isValidated)
                {
                    DialogResult dr = MessageBox.Show("Confirm that fishing ground is blank", "Please validate",
                                   MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    isValidated = (dr == DialogResult.Yes);
                    if (!isValidated) fishingGround.Focus();
                }
            }

            if (msg.Length > 0)
            {
                MessageBox.Show(msg, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return isValidated;
        }

        private void HideErrorLabels()
        {
            foreach (Control c in panelUI.Controls)
            {
                c.Visible = c.Name.Substring(0, 8) != "errLabel";
            }
        }

        private void OnFieldChange(object sender, EventArgs e)
        {
            HideErrorLabels();
            if (sender.GetType().Name == "ComboBox")
            {
                if (((Control)sender).Name == "comboEnumerator")
                {
                    //System.Diagnostics.Debugger.Break();
                }
            }
        }

        private void OnFieldValidated(object sender, EventArgs e)
        {
            if (sender.GetType().Name == "ComboBox")
            {
                var combo = (ComboBox)sender;
                var targetCombo = new ComboBox();
                var key = "";
                var comboItems = new Dictionary<string, string>();

                if (combo.Text.Length > 0)
                {
                    try
                    {
                        //we get the dictionary key of the current item in the combobox
                        key = ((KeyValuePair<string, string>)combo.SelectedItem).Key;
                    }
                    catch
                    {
                        key = "";
                    }
                    finally
                    {
                        if (key.Length > 0)
                        {
                            switch (combo.Name)
                            {
                                case "comboTargetArea":
                                    targetCombo = (ComboBox)panelUI.Controls["comboLandingSite"];
                                    comboItems = aoi.LandingSitesFromAOI(key);
                                    ChangeComboDataSource(targetCombo, comboItems);

                                    targetCombo = (ComboBox)panelUI.Controls["comboEnumerator"];
                                    comboItems = Enumerators.AOIEnumeratorsList(key);
                                    ChangeComboDataSource(targetCombo, comboItems);

                                    targetCombo = (ComboBox)panelUI.Controls["comboFishingGear"];
                                    string myGearClassGUID = ((KeyValuePair<string, string>)((ComboBox)panelUI.Controls["comboGearClass"]).SelectedItem).Key;
                                    comboItems = gear.GearVariationsUsage(myGearClassGUID, key);
                                    ChangeComboDataSource(targetCombo, comboItems);

                                    break;

                                case "comboLandingSite":
                                    break;

                                case "comboEnumerator":
                                    break;

                                case "comboGearClass":
                                    _gearClassName = ((ComboBox)panelUI.Controls["comboGearClass"]).Text;
                                    string myAOIGUID = ((KeyValuePair<string, string>)((ComboBox)panelUI.Controls["comboTargetArea"]).SelectedItem).Key;
                                    targetCombo = (ComboBox)panelUI.Controls["comboFishingGear"];
                                    comboItems = gear.GearVariationsUsage(key, myAOIGUID);
                                    ChangeComboDataSource(targetCombo, comboItems);
                                    break;

                                case "comboFishingGear":
                                    break;

                                case "comboTypeOfVesselUsed":
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void ChangeComboDataSource(ComboBox cbo, Dictionary<string, string> comboItems)
        {
            cbo.With
            (o =>
            {
                //o.Text = "";
                if (comboItems.Count > 0)
                {
                    o.DataSource = new BindingSource(comboItems, null);
                    o.DisplayMember = "Value";
                    o.ValueMember = "Key";
                    o.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    o.AutoCompleteSource = AutoCompleteSource.ListItems;
                    o.Text = "";
                }
                else
                {
                    o.DataSource = null;
                    o.Items.Clear();
                }
            });
        }

        private void OnComboSelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                switch (((Control)sender).Name)
                {
                    case "comboTargetArea":
                        panelUI.Controls["comboLandingSite"].Text = "";
                        panelUI.Controls["comboFishingGear"].Text = "";
                        panelUI.Controls["comboEnumerator"].Text = "";
                        break;

                    case "comboGearClass":
                        panelUI.Controls["comboFishingGear"].Text = "";
                        break;
                }
            }
            catch { }
        }

        private void OnFieldValidate(object sender, CancelEventArgs e)
        {
            //we want to get the UserInterfaceStructure element specified in the tag of the control to validate
            sampling.UserInterfaceStructure ui = sampling.uis[((Control)sender).Tag.ToString()];

            string v = ((Control)sender).Text;
            string msg = "";
            if (!ui.ReadOnly && v.Length > 0)
            {
                switch (ui.DataType)
                {
                    case "string":
                        break;

                    case "int":
                        try
                        {
                            int x = int.Parse(v);
                            if (x < 0)
                            {
                                msg = "Expected value is a whole number greater than zero";
                            }
                        }
                        catch
                        {
                            msg = "Expected value is a whole number greater than zero";
                        }
                        break;

                    case "datetime":
                        DateTime myDateTime;
                        try
                        {
                            myDateTime = DateTime.Parse(v);
                            if (ui.Control.ToString() == "DateMask")
                            {
                                if (myDateTime > DateTime.Now)
                                {
                                    msg = "Cannot accept a future date";
                                }
                            }
                            else
                            {
                                if (v.Length != 5)
                                {
                                    msg = "Expected time value should be in a 24 hour format";
                                }
                            }
                        }
                        catch
                        {
                            if (v != _datePrompt && v != _timePrompt)
                            {
                                if (ui.Control.ToString() == "DateMask")
                                {
                                    msg = "Expected value is a date";
                                }
                                else
                                {
                                    msg = "Expected value is time in a 24-hour format";
                                }
                            }
                        }

                        break;

                    case "lookup":
                        var key = "";
                        var cbo = (ComboBox)panelUI.Controls["combo" + ui.Key];
                        switch (ui.Key)
                        {
                            case "TargetArea":
                                key = ((KeyValuePair<string, string>)cbo.SelectedItem).Key;
                                e.Cancel = !Enumerators.AOIHaveEnumerators(key);
                                if (e.Cancel)
                                    msg = "Cannot use the selected target area because it does not have enumerators";
                                else
                                {
                                    _AOIName = cbo.Text;
                                    _AOIGuid = key;
                                }

                                break;

                            case "GearClass":
                                break;

                            case "FishingGear":
                                _gearVarName = cbo.Text;
                                _gearVarGuid = ((KeyValuePair<string, string>)cbo.SelectedItem).Key;
                                break;
                        }
                        break;

                    case "double":
                        try
                        {
                            double x = double.Parse(v);
                            if (x <= 0)
                            {
                                if (ui.Key == "WeightOfCatch")
                                {
                                    var bt = MessageBoxButtons.YesNo;
                                    DialogResult dr = MessageBox.Show("Accept zero catch?", "Please verify", bt, MessageBoxIcon.Exclamation);
                                    if (dr == DialogResult.No)
                                    {
                                        e.Cancel = true;
                                    }
                                }
                                else
                                {
                                    msg = "Expected value is number greater than zero";
                                }
                            }
                        }
                        catch
                        {
                            msg = "Expected value is a number greater than zero";
                        }
                        break;
                }

                if (msg.Length > 0)
                {
                    MessageBox.Show(msg, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    e.Cancel = true;
                }
                else
                {
                    ((Control)sender).With(o =>
                    {
                        switch (o.Tag.ToString())
                        {
                            case "SamplingDate":
                                _samplingDateSet = false;
                                if (o.Text != _datePrompt)
                                {
                                    if (DateTime.TryParse(o.Text, out _samplingDate))
                                        _samplingDateSet = true;
                                }
                                break;

                            case "LandingSite":
                                _landingSiteGuid = ((KeyValuePair<string, string>)((ComboBox)o).SelectedItem).Key;
                                break;

                            case "FishingGear":
                                _gearVarGuid = ((KeyValuePair<string, string>)((ComboBox)o).SelectedItem).Key;
                                break;
                        }
                    });
                }
            }
        }

        private string GetFieldText(string DataType)
        {
            var FieldText = "";
            foreach (Control c in panelUI.Controls)
            {
                if (c.Tag.ToString() == DataType && (c.GetType().Name != "Label" || c.GetType().Name != "Button"))
                {
                    FieldText = c.Text;
                }
            }
            return FieldText;
        }

        private void OnmaskedText_KeyDown(object sender, KeyEventArgs e)
        {
            if (_samplingDateSet)
            {
                var Proceed = false;
                ((MaskedTextBox)sender).With(o =>
                {
                    if (e.KeyCode == Keys.OemMinus || e.KeyCode == Keys.Subtract)
                    {
                        _dateSetAdjust = _dateSetAdjust.AddDays(-1);
                        Proceed = true;
                    }
                    else if (e.KeyCode == Keys.Oemplus || e.KeyCode == Keys.Add)
                    {
                        _dateSetAdjust = _dateSetAdjust.AddDays(1);
                        Proceed = true;
                    }

                    if (Proceed)
                    {
                        switch (o.Tag.ToString())
                        {
                            case "DateSet":
                            case "DateHauled":
                                o.Text = _dateSetAdjust.ToString("MMM-dd-yyyy");
                                break;
                        }
                    }
                });
            }
        }
    }
}
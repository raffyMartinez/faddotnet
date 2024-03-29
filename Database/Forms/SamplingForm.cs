﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FAD3.Database.Classes;
using FAD3.Database.Forms;

namespace FAD3
{
    public partial class SamplingForm : Form
    {
        private Dictionary<string, Samplings.UserInterfaceStructure> _uis = new Dictionary<string, Samplings.UserInterfaceStructure>();
        private Samplings _samplings;
        private string _samplingGUID = "";
        private ListView _lv;
        private Control _topControl;
        private string _targetAreaGuid = "";
        private int _widestLabel = 0;
        private int _controlWidth = 175;
        private int _yPos;
        private string _targetAreaName = "";
        private string _landingSiteName = "";
        private string _landingSiteGuid = "";
        private string _gearClassName = "";
        private string _gearClassGuid = "";
        private string _gearVarName = "";
        private string _gearVarGuid = "";
        private string _gearRefCode = "";
        private MainForm _parentForm;
        private bool _isNew;
        private TargetArea _targetArea;
        private TextBox _txtVesselDimension = new TextBox();
        private TextBox _txtExpenses = new TextBox();
        private ComboBox _cboEngine = new ComboBox();
        private string _engine = "";
        private string _engineHP = "";
        private TextBox _txtEngineHP = new TextBox();
        private bool _hasExpenseData = false;
        private string _vesselType = "";

        private string _vesLength = "";
        private string _vesWidth = "";
        private string _vesHeight = "";

        private string _enumeratorGuid = "";
        public ExpensePerOperation ExpensePerOperation { get; set; }
        public Sampling Sampling { get; set; }
        private Sampling _editedSampling = new Sampling();

        private string _datePrompt = "";
        private string _timePrompt = "";
        private DateTime _samplingDate;
        private bool _samplingDateSet = false;
        private DateTime _dateSetAdjust;
        private DateTime _dateUpdated;
        private string _newReferenceNumber = "";

        private bool _sampledGearSpecIsEdited;

        private List<string> _fishingGrounds;

        private Dictionary<string, ComboBox> _comboBoxes = new Dictionary<string, ComboBox>();

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

        public void GearVariationUseRefresh()
        {
            var targetCombo = new ComboBox();
            var key = "";
            var comboItems = new Dictionary<string, string>();

            _gearClassName = ((ComboBox)panelUI.Controls["comboGearClass"]).Text;
            string myAOIGUID = ((KeyValuePair<string, string>)((ComboBox)panelUI.Controls["comboTargetArea"]).SelectedItem).Key;
            key = ((KeyValuePair<string, string>)((ComboBox)panelUI.Controls["comboGearClass"]).SelectedItem).Key;
            targetCombo = (ComboBox)panelUI.Controls["comboFishingGear"];
            comboItems = Gears.GearVariationsUsage(key, myAOIGUID);
            ChangeComboDataSource(targetCombo, comboItems);
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
                    else
                    {
                        ((TextBox)panelUI.Controls["textAdditionalFishingGround"]).Clear();
                    }
                }
                else
                {
                    ((TextBox)panelUI.Controls["textFishingGround"]).Text = "";
                    ((TextBox)panelUI.Controls["textAdditionalFishingGround"]).Text = "";
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

        public string TargetAreaName
        {
            get { return _targetAreaName; }
            set { _targetAreaName = value; }
        }

        public string TargetAreaGuid
        {
            get { return _targetAreaGuid; }
            set { _targetAreaGuid = value; }
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

        public TargetArea TargetArea
        {
            get { return _targetArea; }
            set { _targetArea = value; }
        }

        public MainForm Parent_Form
        {
            get { return _parentForm; }
            set
            {
                _parentForm = value;
                _parentForm.SamplingDetailClosed += OnSamplingDetailClosed;
            }
        }

        private void OnSamplingDetailClosed(object sender, EventArgs e)
        {
            Close();
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
            _samplings.OnUIRowRead -= new Samplings.ReadUIElement(OnUIRowRead);
            _samplings = null;
        }

        private void OnUIRowRead(object sender, UIRowFromXML e)
        {
            int controlHeight = 16;
            int x = 0;

            int spacing = 15;
            string controlType;
            int sizeAdjust = 50;
            string buttonText;
            string e_key = e.Key;
            int controlSpacing = 0;
            Control ctl = new Control();

            //setup the labels
            Label lbl = new Label
            {
                Name = "label" + e_key,
                Text = e.RowLabel,
                AutoSize = true,
                Size = new Size(60, controlHeight),
                Tag = e_key
            };
            panelUI.Controls.Add(lbl);
            lbl.Location = new Point(x, _yPos);
            if (lbl.Width > _widestLabel)
                _widestLabel = lbl.Width;

            try
            {
                controlType = e.Control.ToString();
            }
            catch
            {
                controlType = "";
            }

            //setup error labels
            if (controlType != "Spacer")
            {
                Font f = new Font(lbl.Font.FontFamily, 8, FontStyle.Bold);
                Label lblError = new Label()
                {
                    Name = "errLabel" + e_key,
                    Text = "!",
                    AutoSize = true,
                    Size = new Size(3, 40),
                    Font = f,
                    Tag = e_key,
                    Visible = false,
                    ForeColor = Color.Red
                };
                panelUI.Controls.Add(lblError);
                lblError.Location = new Point(x, _yPos);
            }

            //setup the field controls
            switch (controlType)
            {
                case "TextBox":
                    ctl = new TextBox
                    {
                        Name = "text" + e_key
                    };
                    ((TextBox)ctl).ReadOnly = e.ReadOnly;
                    break;

                case "ComboBox":
                    ctl = new ComboBox
                    {
                        Name = "combo" + e_key,
                    };

                    switch (e_key)
                    {
                        case "Enumerator":
                            Enumerators.AOIEnumeratorsList(_targetAreaGuid, (ComboBox)ctl);
                            break;

                        case "TargetArea":
                            TargetArea.GetTargetAreasEx((ComboBox)ctl);
                            break;

                        case "LandingSite":
                            TargetArea.LandingSitesFromTargetArea(_targetAreaGuid, (ComboBox)ctl);
                            break;

                        case "GearClass":
                            ComboBox cbo = (ComboBox)ctl;
                            foreach (var item in Gears.GearClasses)
                            {
                                KeyValuePair<string, string> gear = new KeyValuePair<string, string>(item.Key, item.Value.GearClassName);
                                cbo.Items.Add(gear);
                            }

                            break;

                        case "Engine":
                            foreach (var item in Samplings.Engines)
                            {
                                ((ComboBox)ctl).Items.Add(item);
                            }
                            break;

                        case "FishingGear":
                            if (!_isNew)
                            {
                                Gears.GearClassUsed = _lv.Items["GearClass"].Tag.ToString();
                            }

                            if (_gearClassGuid.Length == 0)
                            {
                                if (_isNew)
                                {
                                    _gearClassGuid = ((KeyValuePair<string, string>)((ComboBox)panelUI.Controls["comboGearClass"]).Items[0]).Key;
                                }
                                else
                                {
                                    _gearClassGuid = Gears.GearClassUsed;
                                }
                            }

                            Gears.GearVariationsUsage(_gearClassGuid, _targetAreaGuid, (ComboBox)ctl);
                            break;

                        case "TypeOfVesselUsed":
                            ((ComboBox)ctl).DataSource = new BindingSource(global.VesselTypeDict, null);
                            break;
                    }
                    ((ComboBox)ctl).With(o =>
                    {
                        if (e_key != "Engine")
                        {
                            o.DisplayMember = "Value";
                            o.ValueMember = "Key";
                        }
                        o.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                        o.AutoCompleteSource = AutoCompleteSource.ListItems;
                    });
                    break;

                case "Spacer":
                    controlSpacing = 35;
                    break;

                case "DateMask":
                    ctl = new MaskedTextBox
                    {
                        Name = "dtxt" + e_key,
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
                        Name = "ttxt" + e_key,
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
                        Name = "chk" + e_key
                    };
                    break;

                default:
                    break;
            }

            Type type = ctl.GetType();
            if (type.Name != "Control" && controlType != "Spacer")
            {
                //setup tooltip of the control
                ToolTip tt = new ToolTip
                {
                    ToolTipTitle = ctl.Text
                };
                tt.SetToolTip(ctl, e.ToolTip);

                //add control to the form
                panelUI.Controls.Add(ctl);

                //setup control size and position
                ctl.With(o =>
                 {
                     o.Location = new Point(x, _yPos);
                     o.Tag = e_key;
                     if (type.Name != "CheckBox")
                     {
                         o.Size = new System.Drawing.Size(_controlWidth, controlHeight + sizeAdjust);
                         if (e.Height != 1)
                         {
                             o.Size = new System.Drawing.Size(_controlWidth, controlHeight * e.Height);
                             ((TextBox)o).Multiline = true;
                         }
                     }
                     controlSpacing = o.Height;
                 });

                //set the text of the control for both new and saved samplings
                if (!_isNew)
                {
                    if (type.Name != "CheckBox")
                    {
                        //the text of control is derived from the text of the catch details listview items
                        ctl.Text = _lv.Items[e_key].SubItems[1].Text;

                        switch (e_key)
                        {
                            case "TypeOfVesselUsed":
                                _vesselType = ctl.Text;
                                break;

                            case "Enumerator":
                                _enumeratorGuid = Sampling.EnumeratorGuid;
                                break;

                            case "FishingGear":
                                _gearVarName = Sampling.SamplingSummary.GearVariationName;
                                _gearVarGuid = Sampling.GearVariationGuid;
                                break;

                            case "ReferenceNumber":
                                _gearRefCode = Sampling.ReferenceNumber.Split('-')[1];
                                break;

                            case "TargetArea":
                                _targetAreaName = Sampling.SamplingSummary.TargetAreaName;
                                break;

                            case "AdditionalFishingGround":
                                break;

                            case "GearSpecs":
                                ctl.Text = ManageGearSpecsClass.GetSampledSpecsEx(_samplingGUID);
                                break;
                        }
                    }
                    else
                    {
                        ((CheckBox)ctl).Checked = _lv.Items[e_key].SubItems[1].Text == "Yes";
                    }
                }
                else
                {
                    //setup control contents for a new sampling
                    switch (e_key)
                    {
                        case "SamplingDate":
                            if (_isNew)
                                _topControl = ctl;
                            break;

                        case "TargetArea":
                            ctl.Text = _targetAreaName;
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
                                    o.SelectedItem = o.Items[0];
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
                                        o.SelectedItem = o.Items[0];
                                    }
                                });
                            }

                            if (ctl.Text.Length > 0)
                            {
                                _gearVarGuid = ((KeyValuePair<string, string>)((ComboBox)ctl).SelectedItem).Key;
                                _gearVarName = ((ComboBox)ctl).Text;
                            }

                            break;

                        case "Enumerator":
                            ctl.Text = "";
                            break;

                        case "TypeOfVesselUsed":
                            ctl.Text = "Motorized";
                            break;
                    }
                }

                //turn of selected text in combobox textbox
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
                else if (controlType == "DateMask")
                    ((MaskedTextBox)ctl).KeyDown += OnmaskedText_KeyDown;

                try
                {
                    switch (e_key)
                    {
                        case "VesselDimension":
                            _txtVesselDimension = (TextBox)ctl;
                            break;

                        case "OperatingExpenses":
                            _txtExpenses = (TextBox)ctl;
                            _txtExpenses.ScrollBars = ScrollBars.Both;
                            break;

                        case "Engine":
                            _cboEngine = (ComboBox)ctl;
                            _cboEngine.Enabled = _vesselType == "Motorized";
                            break;

                        case "EngineHorsepower":
                            _txtEngineHP = (TextBox)ctl;
                            _txtEngineHP.Enabled = _vesselType == "Motorized";
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex.Message, ex.StackTrace);
                }
            }

            //making the controls not visible speeds up form drawing
            ctl.Visible = false;

            //setup the buttons
            try
            {
                buttonText = e.ButtonText;
            }
            catch
            {
                buttonText = "";
            }
            if (buttonText.Length > 0)
            {
                Button btn = new Button
                {
                    Name = "btn" + e_key,
                    Text = buttonText,
                    Size = new Size(_controlWidth / 2, (int)(controlHeight * 1.7)),
                    Tag = e_key
                };
                panelUI.Controls.Add(btn);
                btn.Location = new Point(x, _yPos - 3);
                btn.Click += OnbuttonSamplingFields_Click;

                switch (e_key)
                {
                    case "Enumerator":
                        if (!IsNew) _topControl = btn;
                        break;

                    case "ReferenceNumber":
                        btn.Enabled = IsNew;
                        break;
                }
            }

            //setup horizontal position for next control
            _yPos += controlSpacing + spacing;
        }

        public void SamplingFishingOperatingExpenseDeleted()
        {
            ExpensePerOperation = null;
            OperatingExpenses.ReadData(_samplingGUID);
            _txtExpenses.Text = OperatingExpenses.SamplingExpenses;
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

            if (!_isNew)
            {
                _targetAreaGuid = Sampling.TargetAreaGuid;
                _samplingGUID = Sampling.SamplingGUID;
            }
            else
            {
                _samplingGUID = Guid.NewGuid().ToString();
            }

            _samplings = _parentForm.Samplings;
            _samplings.OnUIRowRead += new Samplings.ReadUIElement(OnUIRowRead);
            panelUI.SuspendLayout();

            _samplings.ReadUIFromXML();
            //the form's field contents are filled up after ReadUIFromXML()

            AdustControlsPosition();
            panelUI.ResumeLayout();

            if (_isNew)
            {
                Text = "New sampling";
            }
            else
            {
                Text = "Sampling detail";
                _gearClassName = _lv.Items["GearClass"].SubItems[1].Text;
                var result = OperatingExpenses.ReadData(SamplingGUID);
                _hasExpenseData = result.success;
                _txtExpenses.Text = OperatingExpenses.SamplingExpenses;
                panelUI.Controls["btnVesselDimension"].Enabled = panelUI.Controls["comboTypeOfVesselUsed"].Text == "Motorized" || panelUI.Controls["comboTypeOfVesselUsed"].Text == "Non-Motorized";
            }

            SetFieldsVisible();
            buttonCancel.Visible = true;
            buttonOK.Visible = true;

            CancelButton = buttonCancel;
            _topControl.Select();
        }

        public void UpdateExpenses()
        {
            _txtExpenses.Text = ExpensePerOperation.Summary;
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
                        c.Left = _widestLabel + 15;
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
            _editedSampling.SamplingGUID = _samplingGUID;

            VesselType vesType = VesselType.NotDetermined;
            string engineUsed = "";
            double? engineHp = null;

            foreach (Control c in panelUI.Controls)
            {
                string val = "";
                string tag = "";
                var typeName = c.GetType().Name;
                if (typeName != "Button" && typeName != "Label")
                {
                    tag = c.Tag.ToString();
                    switch (typeName)
                    {
                        case "ComboBox":
                            var key = "";
                            if (c.Name != "comboTypeOfVesselUsed" && c.Name != "comboEngine")
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
                                    switch (c.Name)
                                    {
                                        case "comboTypeOfVesselUsed":
                                            var cbo = ((ComboBox)panelUI.Controls["comboTypeOfVesselUsed"]);
                                            if (cbo.Items.Count > 0)
                                            {
                                                var v = int.Parse(cbo.SelectedValue.ToString());
                                                key = v.ToString();
                                            }
                                            break;

                                        case "comboEngine":
                                            key = c.Text;
                                            break;
                                    }
                                }
                            }
                            val = key;
                            break;

                        case "TextBox":
                            val = c.Text;
                            break;

                        case "MaskedTextBox":
                            val = ((MaskedTextBox)c).MaskCompleted ? c.Text : "";
                            break;

                        case "CheckBox":
                            val = ((CheckBox)c).Checked.ToString();
                            break;
                    }

                    if (val.Length > 0)
                    {
                        switch (tag)
                        {
                            case "TargetArea":
                                _editedSampling.TargetAreaGuid = val;
                                break;

                            case "LandingSite":
                                _editedSampling.LandingSiteGuid = val;
                                break;

                            case "SamplingDate":
                                _editedSampling.SamplingDateTime = DateTime.Parse(val);
                                break;

                            case "SamplingTime":
                                _editedSampling.SamplingDateTime = _editedSampling.SamplingDateTime.Add(new TimeSpan(DateTime.Parse(val).Hour, DateTime.Parse(val).Minute, 0));
                                break;

                            case "Enumerator":
                                _editedSampling.EnumeratorGuid = val;
                                break;

                            case "GearClass":
                                break;

                            case "FishingGear":
                                _editedSampling.GearVariationGuid = val;
                                _editedSampling.GearClassName = "";
                                break;

                            case "GearSpecs":
                                break;

                            case "ReferenceNumber":
                                _editedSampling.ReferenceNumber = val;
                                break;

                            case "WeightOfCatch":
                                _editedSampling.CatchWeight = double.Parse(val);
                                break;

                            case "WeightOfSample":
                                _editedSampling.SampleWeight = double.Parse(val);

                                break;

                            case "HasLiveFish":
                                _editedSampling.HasLiveFish = bool.Parse(val);
                                break;

                            case "FishingGround":
                                break;

                            case "DateSet":
                                _editedSampling.GearSettingDateTime = DateTime.Parse(val);

                                break;

                            case "TimeSet":
                                TimeSpan ts = new TimeSpan(DateTime.Parse(val).Hour, DateTime.Parse(val).Minute, 0);
                                _editedSampling.GearSettingDateTime = _editedSampling.GearSettingDateTime.Value.Add(ts);

                                break;

                            case "DateHauled":
                                _editedSampling.GearHaulingDateTime = DateTime.Parse(val);

                                break;

                            case "TimeHauled":
                                ts = new TimeSpan(DateTime.Parse(val).Hour, DateTime.Parse(val).Minute, 0);
                                _editedSampling.GearHaulingDateTime = _editedSampling.GearHaulingDateTime.Value.Add(ts);

                                break;

                            case "NumberOfHauls":
                                _editedSampling.NumberOfHauls = int.Parse(val);
                                break;

                            case "NumberOfFishers":
                                _editedSampling.NumberOfFishers = int.Parse(val);
                                break;

                            case "TypeOfVesselUsed":
                                vesType = (VesselType)int.Parse(val);
                                break;

                            case "Engine":
                                engineUsed = val;
                                break;

                            case "EngineHorsepower":
                                engineHp = double.Parse(val);
                                break;

                            case "Notes":
                                _editedSampling.Notes = val;
                                break;
                        }
                    }
                }
            }

            if (vesType == VesselType.Motorized || vesType == VesselType.NonMotorized)
            {
                FishingVessel v = new FishingVessel(vesType);
                if (_vesHeight.Length > 0)
                {
                    v.Length = double.Parse(_vesLength);
                    v.Depth = double.Parse(_vesHeight);
                    v.Breadth = double.Parse(_vesWidth);
                }
                v.EngineHorsepower = engineHp;
                v.Engine = engineUsed;
                _editedSampling.FishingVessel = v;
            }

            PopulateFGList(_editedSampling);

            _dateUpdated = DateTime.Now;
            _editedSampling.DateEncoded = DateTime.Now;
            _editedSampling.IsNew = _isNew;
            _editedSampling.SamplingType = CatchMonitoringSamplingType.FisheryDependent;

            if (_samplings.UpdateEffort(_editedSampling))
            {
                return ManageGearSpecsClass.SaveSampledGearSpecsEx(_samplingGUID);
            }
            else
            {
                return false;
            }
            //if (_samplings.UpdateEffort(_isNew, EffortData, _fishingGrounds, _dateUpdated))
            //    return ManageGearSpecsClass.SaveSampledGearSpecs(_samplingGUID);
            //else
            //    return false;
        }

        private void OnbuttonSamplingFields_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "btnOperatingExpenses":
                    if (CheckRequiredForRefNumber())
                    {
                        FishingOperationCostsForm fcf = FishingOperationCostsForm.GetInstance(_samplingGUID, this, _hasExpenseData);
                        if (fcf.Visible)
                        {
                            fcf.BringToFront();
                        }
                        else
                        {
                            fcf.Show(this);
                        }
                    }
                    else
                    {
                        ShowReferenceNumberRequiredErrorLabel(showRefNumberError: true);
                        MessageBox.Show("Some required fields not filled up", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

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

                    GearCodesUsageForm form = GearCodesUsageForm.GetInstance(_gearVarGuid, _targetAreaGuid, _gearClassName);
                    form.With(o =>
                    {
                        o.GearRefCode = _gearRefCode;
                        o.Parent_Form = this;
                        o.TargetArea(_targetAreaName, _targetAreaGuid);
                        o.GearVariation(_gearVarName, _gearVarGuid);
                    });
                    if (!form.Visible)
                    {
                        form.Show(this);
                    }
                    else
                    {
                        form.BringToFront();
                    }

                    break;

                case "btnGearSpecs":
                    if (_gearVarName.Length > 0 && _gearVarGuid.Length > 0)
                    {
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
                    }
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
                        ReferenceNumberManager.SetAOI_GearVariation(_targetAreaGuid, _gearVarGuid, DateTime.Parse(txt.Text));
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
                    if (FishingGrid.IsCompleteGrid25)
                    {
                        PopulateFGList();
                        FishingGroundForm fg = new FishingGroundForm(_targetAreaGuid, this);
                        fg.FishingGrounds = _fishingGrounds;
                        fg.Show(this);
                    }
                    else
                    {
                        MessageBox.Show("Cannot specify fishing ground because\r\n" +
                                        "target area is not setup for Grid25", "Grid25 is not setup",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;
            }
        }

        private void ShowReferenceNumberRequiredErrorLabel1(bool showRefNumberError = false)
        {
            foreach (Control c in panelUI.Controls)
            {
                if (c.Name.Substring(0, 8) == "errLabel")
                {
                    switch (c.Tag.ToString())
                    {
                        case "ReferenceNumber":
                            c.Visible = showRefNumberError;
                            break;

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

        private void ShowReferenceNumberRequiredErrorLabel(bool showRefNumberError = false)
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

                        case "ReferenceNumber":
                            c.Visible = showRefNumberError;
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

        private void PopulateFGList(Sampling s)
        {
            ((TextBox)panelUI.Controls["textFishingGround"]).With(o =>
            {
                if (o.Text.Length > 0)
                {
                    var fgParts = o.Text.Split('-');
                    int? sg = null;
                    if (fgParts.Length == 3)
                    {
                        sg = int.Parse(fgParts[2]);
                    }
                    FishingGround fg = new FishingGround($"{fgParts[0]}-{fgParts[1]}", sg);
                    s.AddFishingGround(fg);
                }
            });

            var t = (TextBox)panelUI.Controls["textAdditionalFishingGround"];
            var arr = t.Text.Length > 0 ? t.Text.Split(',') : null;
            for (int n = 0; arr != null && n < arr.Length; n++)
            {
                var fgParts = arr[n].Trim().Split('-');
                int? sg = null;
                if (fgParts.Length == 3)
                {
                    sg = int.Parse(fgParts[2]);
                }
                FishingGround fg = new FishingGround($"{fgParts[0]}-{fgParts[1]}", sg);
                s.AddFishingGround(fg);
            }
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
                            if (ExpensePerOperation != null && OperatingExpenses.Update(ExpensePerOperation))
                            {
                                _txtExpenses.Text = OperatingExpenses.SamplingExpenses;
                            }
                            _samplings.OnUIRowRead -= new Samplings.ReadUIElement(OnUIRowRead);
                            if (IsNew)
                            {
                                ReferenceNumberManager.UpdateRefCodeCounter();
                            }
                            _parentForm.RefreshCatchDetail(_samplingGUID, _isNew, _samplingDate, _gearVarGuid, _landingSiteGuid);
                            Close();
                        }
                    }
                    break;

                case "buttonCancel":
                    Close();
                    break;
            }
        }

        private void ShowRequiredErrorLabel(bool Visible)
        {
            foreach (Control c in panelUI.Controls)
            {
                if (c.Name.Substring(0, 8) == "errLabel")
                {
                    Samplings.UserInterfaceStructure ui = Samplings.uis[c.Tag.ToString()];
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
                    Samplings.UserInterfaceStructure ui = Samplings.uis[c.Tag.ToString()];

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

            //Step 4. Weight of sample cannot be more than weight of catch
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

            //Step 5. If catch is not blank then confirm if fishing ground is blank
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
                                    comboItems = TargetArea.LandingSitesFromTargetArea(key);
                                    ChangeComboDataSource(targetCombo, comboItems);

                                    targetCombo = (ComboBox)panelUI.Controls["comboEnumerator"];
                                    comboItems = Enumerators.AOIEnumeratorsList(key);
                                    ChangeComboDataSource(targetCombo, comboItems);

                                    targetCombo = (ComboBox)panelUI.Controls["comboFishingGear"];
                                    string myGearClassGUID = ((KeyValuePair<string, string>)((ComboBox)panelUI.Controls["comboGearClass"]).SelectedItem).Key;
                                    comboItems = Gears.GearVariationsUsage(myGearClassGUID, key);
                                    ChangeComboDataSource(targetCombo, comboItems);

                                    break;

                                case "comboLandingSite":
                                    break;

                                case "comboEnumerator":
                                    break;

                                case "comboGearClass":
                                    GearVariationUseRefresh();
                                    break;

                                case "comboFishingGear":
                                    break;

                                case "comboTypeOfVesselUsed":
                                    if (combo.Text != "Motorized")
                                    {
                                        _engine = _cboEngine.Text;
                                        _engineHP = _txtEngineHP.Text;
                                        _cboEngine.Enabled = false;
                                        _txtEngineHP.Enabled = false;
                                        _cboEngine.Text = "";
                                        _txtEngineHP.Text = "";
                                    }
                                    else
                                    {
                                        _cboEngine.Enabled = true;
                                        _txtEngineHP.Enabled = true;
                                        _cboEngine.Text = _engine;
                                        _txtEngineHP.Text = _engineHP;
                                    }
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
                        _gearVarGuid = "";
                        _gearVarName = "";
                        break;

                    case "comboTypeOfVesselUsed":
                        panelUI.Controls["btnVesselDimension"].Enabled = panelUI.Controls["comboTypeOfVesselUsed"].Text == "Motorized" || panelUI.Controls["comboTypeOfVesselUsed"].Text == "Non-Motorized";
                        if (!panelUI.Controls["btnVesselDimension"].Enabled)
                        {
                            panelUI.Controls["textVesselDimension"].Text = "";
                        }

                        if (panelUI.Controls["comboTypeOfVesselUsed"].Text != "Motorized")
                        {
                            panelUI.Controls["textEngineHorsepower"].Text = "";
                            panelUI.Controls["comboEngine"].Text = "";
                        }
                        panelUI.Controls["textEngineHorsepower"].Enabled = panelUI.Controls["comboTypeOfVesselUsed"].Text == "Motorized";
                        panelUI.Controls["comboEngine"].Enabled = panelUI.Controls["comboTypeOfVesselUsed"].Text == "Motorized";

                        break;
                }
            }
            catch { }
        }

        private void OnFieldValidate(object sender, CancelEventArgs e)
        {
            //we want to get the UserInterfaceStructure element specified in the tag of the control to validate
            Samplings.UserInterfaceStructure ui = Samplings.uis[((Control)sender).Tag.ToString()];
            bool emptyComboList = false;
            string controlText = ((Control)sender).Text;
            string msg = "";
            if (!ui.ReadOnly && controlText.Length > 0)
            {
                switch (ui.DataType)
                {
                    case "string":
                        if (ui.Key == "Engine")
                        {
                            var cbEngine = (ComboBox)panelUI.Controls["combo" + ui.Key];
                            if (!cbEngine.Items.Contains(controlText))
                            {
                                if (MessageBox.Show($"{controlText} is not found in the list of engines.\r\nDo you want to add an engine?", "Add a new engine",
                                                    MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                                {
                                    cbEngine.Items.Add(controlText);
                                }
                                else
                                {
                                    e.Cancel = true;
                                }
                            }
                        }
                        break;

                    case "int":
                        try
                        {
                            int x = int.Parse(controlText);
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
                            myDateTime = DateTime.Parse(controlText);
                            if (ui.Control.ToString() == "DateMask")
                            {
                                if (myDateTime > DateTime.Now)
                                {
                                    msg = "Cannot accept a future date";
                                }
                            }
                            else
                            {
                                if (controlText.Length != 5)
                                {
                                    msg = "Expected time value should be in a 24 hour format";
                                }
                            }
                        }
                        catch
                        {
                            if (controlText != _datePrompt && controlText != _timePrompt)
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
                        var isInList = false;
                        var cbo = (ComboBox)panelUI.Controls["combo" + ui.Key];
                        foreach (KeyValuePair<string, string> item in cbo.Items)
                        {
                            if (item.Value == controlText)
                            {
                                isInList = true;
                                break;
                            }
                        }
                        emptyComboList = cbo.Items.Count == 0;
                        if (isInList)
                        {
                            switch (ui.Key)
                            {
                                case "TargetArea":
                                    key = ((KeyValuePair<string, string>)cbo.SelectedItem).Key;
                                    e.Cancel = !Enumerators.TargetAreaHasEnumerators(key);
                                    if (e.Cancel)
                                        msg = "Cannot use the selected target area because it does not have enumerators";
                                    else
                                    {
                                        _targetAreaName = cbo.Text;
                                        _targetAreaGuid = key;
                                    }

                                    break;

                                case "GearClass":
                                    break;

                                case "FishingGear":
                                    _gearVarName = cbo.Text;
                                    _gearVarGuid = ((KeyValuePair<string, string>)cbo.SelectedItem).Key;
                                    break;
                            }
                        }
                        else
                        {
                            if (ui.Key == "Enumerator")
                            {
                                DialogResult dr = MessageBox.Show($"{controlText} is not in the list of enumerators.\r\n" +
                                                        "Would you like to add a new enumerator?",
                                                        "Add a new enumerator",
                                                        MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Information);
                                if (dr == DialogResult.Yes)
                                {
                                    EnumeratorEntryForm enf = new EnumeratorEntryForm(controlText, _targetAreaGuid);
                                    enf.ShowDialog(this);
                                    if (enf.DialogResult == DialogResult.OK)
                                    {
                                        KeyValuePair<string, string> newEnumerator = new KeyValuePair<string, string>(enf.EnumeratorGuid, enf.EnumeratorName);
                                        cbo.Items.Add(newEnumerator);
                                        cbo.Text = newEnumerator.Value;
                                        global.mainForm.RefreshTargetAreaEnumerators(_targetArea.TargetAreaGuid);
                                    }
                                }
                            }
                            else
                            {
                                msg = $"{controlText} is not found in the dropdown list.";
                            }
                        }
                        break;

                    case "double":
                        try
                        {
                            double x = double.Parse(controlText);
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
                    if (!emptyComboList)
                    {
                        MessageBox.Show(msg, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        e.Cancel = true;
                    }
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
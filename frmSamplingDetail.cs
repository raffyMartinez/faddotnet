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
    public partial class frmSamplingDetail : Form
    {
        Dictionary<string, sampling.UserInterfaceStructure> _uis = new Dictionary<string, sampling.UserInterfaceStructure>();
        sampling _sampling;
        string _samplingGUID = "";
        ListView _lv;
        Control _topControl;
        string _AOIGuid = "";
        int _WidestLabel = 0;
        int _ControlWidth = 200;
        int _yPos;
        string _AOIName = "";
        string _LandingSiteName = "";
        string _LandingSiteGuid = "";
        string _GearClassName = "";
        string _GearClassGuid = "";
        string _GearVarName = "";
        string _GearVarGuid = "";
        string _GearRefCode = "";
        frmMain _parent;
        bool _isNew;
        aoi _aoi;
        TextBox _txtVesselDimension = new TextBox();

        string _VesLength = "";
        string _VesWidth = "";
        string _VesHeight = "";

        public void VesselDimension(string length, string width, string height)
        {
            _VesHeight = height;
            _VesLength = length;
            _VesWidth = width;

            try
            {
               _txtVesselDimension.Text = "(LxWxH): " + _VesLength + " x " + _VesWidth + " x " + _VesHeight;
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
            get { return _LandingSiteName; }
            set { _LandingSiteName = value; }
        }

        public string LandingSiteGuid
        {
            get { return _LandingSiteGuid; }
            set { _LandingSiteGuid = value; }
        }

        public string GearClassName
        {
            get { return _GearClassName; }
            set { _GearClassName = value; }
        }

        public string GearClassGuid
        {
            get { return _GearClassGuid; }
            set { _GearClassGuid = value; }
        }

        public string GearVarName
        {
            get { return _GearVarName; }
            set { _GearVarName = value; }
        }

        public string GearVarGuid
        {
            get { return _GearVarGuid; }
            set { _GearVarGuid = value; }
        }

        public  aoi AOI
        {
            get { return _aoi; }
            set { _aoi = value; }
        }

        public frmMain Parent_Form
        {
            get { return _parent; }
            set { _parent = value; }
        }


        public bool IsNew
        {
            get { return _isNew; }
            set { _isNew = value; }
        }

        public string  SamplingGUID
        {
            get { return _samplingGUID; }
            set { _samplingGUID = value; }
        }

        public void LVInterface(ListView lv)
        {
            _lv = lv;
        }

        public frmSamplingDetail()
        {
            InitializeComponent();
        }


        private void frmSamplingDetail_FormClosed(object sender, FormClosedEventArgs e)
        {
            global.SaveFormSettings(this);
            
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
            lbl.Location = new System.Drawing.Point(x, _yPos);
            if (lbl.Width > _WidestLabel)
                _WidestLabel = lbl.Width;

            try
            {
                cType = e.Control.ToString();
            }
            catch
            {
                cType = "";
            }

            //setup error labels
            if (cType !="Spacer") {
                Font f = new Font(lbl.Font.FontFamily, 8, FontStyle.Bold);
                Label lblError = new Label()
                {
                    Name = "errLabel" + e.Key,
                    Text = "!",
                    AutoSize = true,
                    Size = new System.Drawing.Size(3, 40),
                    Font = f,
                    Tag = e.Key,
                    Visible = false,
                    ForeColor = Color.Red
                };
                panelUI.Controls.Add(lblError);
                lblError.Location = new System.Drawing.Point(x, _yPos);
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
                            ((ComboBox)ctl).DataSource = new BindingSource(_aoi.Enumerators, null);
                            break;
                        case "TargetArea":
                            ((ComboBox)ctl).DataSource = new BindingSource(_aoi.AOIs, null);
                            break;
                        case "LandingSite":
                            ((ComboBox)ctl).DataSource = new BindingSource(_aoi.LandingSites, null);
                            break;
                        case "GearClass":
                            ((ComboBox)ctl).DataSource = new BindingSource(global.GearClass, null);
                            break;
                        case "FishingGear":
                            if (!_isNew)
                            {
                                global.GearClassUsed = _lv.Items["GearClass"].Tag.ToString();
                            }
                            ((ComboBox)ctl).DataSource = new BindingSource(global.GearVariationsUsage(global.GearClassUsed, _AOIGuid), null);
                            break;
                        case "TypeOfVesselUsed":
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
                    if (_isNew && e.Key =="SamplingDate")
                    {
                        _topControl = ctl;
                    }
                    break;
                case "TimeMask":
                    ctl = new MaskedTextBox
                    {
                        Name = "ttxt" + e.Key,
                    };
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
            if (type.Name != "Control" && cType !="Spacer")
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
                     if (type.Name !="CheckBox") {
                         o.Size = new System.Drawing.Size(_ControlWidth, ht + SizeAdjust);
                         if (e.Height != 1)
                         {
                             o.Size = new System.Drawing.Size(_ControlWidth, ht * e.Height);
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
                            case "FishingGear":
                                _GearVarName = ctl.Text;
                                _GearVarGuid = ((KeyValuePair<string, string>)((ComboBox)ctl).SelectedItem).Key;
                                break;
                            case "ReferenceNumber":
                                var arr = ctl.Text.Split('-');
                                _GearRefCode = arr[1];
                                break;
                            case "TargetArea":
                                _AOIName = ctl.Text;
                                _AOIGuid = ((KeyValuePair<string, string>)((ComboBox)ctl).SelectedItem).Key;
                                break;

                        }
                    }
                    else
                    {
                        ((CheckBox)ctl).Checked = _lv.Items[e.Key].Text == "True";
                    }
                }
                else
                {
                    switch (e.Key)
                    {
                        case "TargetArea":
                            ctl.Text = _AOIName;
                            break;
                        case "LandingSite":
                            ctl.Text = _LandingSiteName;
                            break;
                        case "GearClass":
                            ctl.Text = _GearClassName;
                            break;
                        case "FishingGear":
                            ctl.Text = _GearVarName;
                            _GearVarGuid = ((KeyValuePair<string, string>)((ComboBox)ctl).SelectedItem).Key;
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
                ctl.Validating += new CancelEventHandler(OnFieldValidate);
                ctl.TextChanged += new EventHandler(OnFieldChange);
                ctl.Validated += new EventHandler(OnFieldValidated);
                if (type.Name == "ComboBox")
                    ((ComboBox)ctl).SelectedIndexChanged += new EventHandler(OnComboSelectedIndexChanged);

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
                    Size = new System.Drawing.Size(_ControlWidth / 2, (int)(ht * 1.7))
                };
                panelUI.Controls.Add(btn);
                btn.Location = new System.Drawing.Point(x, _yPos - 3);
                btn.Click += new EventHandler(buttonSamplingFields_Click);
                if (e.Key == "Enumerator" && !_isNew)
                {
                    _topControl = btn;
                }
            }
            _yPos += ControlHt + Spacing;
        }

        private void 
            frmSamplingDetail_Load(object sender, EventArgs e)
        {
            this.Size = new Size(Width, _lv.Height);
            global.LoadFormSettings(this, true);
            _sampling = new sampling();
            _sampling.OnUIRowRead += new sampling.ReadUIElement(OnUIRowRead);

            panelUI.SuspendLayout();
            _sampling.ReadUIFromXML();
            AdustControlsPosition();
            _topControl.Focus();
            panelUI.ResumeLayout();

            if (_isNew)
                Text = "New sampling";
            else
            {
                Text = "Sampling detail";
                _GearClassName = _lv.Items["GearClass"].SubItems[1].Text;
            }
            //SetupComboDataSource();
            SetFieldsVisible();
            CancelButton = buttonCancel;
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

        private void SetupComboDataSource()
        {
            foreach(Control ctl in panelUI.Controls)
            {
                if (ctl.GetType().Name == "ComboBox")
                {
                    switch (ctl.Tag.ToString())
                    {
                        case "Enumerator":
                            ((ComboBox)ctl).DataSource = new BindingSource(_aoi.Enumerators, null);
                            break;
                        case "TargetArea":
                            ((ComboBox)ctl).DataSource = new BindingSource(_aoi.AOIs, null);
                            break;
                        case "LandingSite":
                            ((ComboBox)ctl).DataSource = new BindingSource(_aoi.LandingSites, null);
                            break;
                        case "GearClass":
                            ((ComboBox)ctl).DataSource = new BindingSource(global.GearClass, null);
                            break;
                        case "FishingGear":
                            if (!_isNew)
                            {
                                global.GearClassUsed = _lv.Items["GearClass"].Tag.ToString();
                            }
                            ((ComboBox)ctl).DataSource = new BindingSource(global.GearVariationsUsage(global.GearClassUsed, _AOIGuid), null);
                            break;
                        case "TypeOfVesselUsed":
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
                }
            }
        }

        private void AdustControlsPosition()
        {
            foreach (Control c in panelUI.Controls)
            {
                System.Type type = c.GetType();
                if (type.Name !="Label")
                {
                    if (type.Name == "Button")
                    {
                        c.Left = _WidestLabel + _ControlWidth + 40;
                    }
                    else
                    {
                        c.Left = _WidestLabel + 25;
                    }
                }
                else if(c.Name.Substring(0,8)=="errLabel")
                {
                    c.Left = _WidestLabel + _ControlWidth + 25;
                }
            }
        }

        private void SaveEdits()
        {
            var EffortData = new Dictionary<string, string>();
            foreach (Control c in panelUI.Controls)
            {
                var typeName = c.GetType().ToString();
                if (typeName != "System.Windows.Forms.Button" && typeName != "System.Windows.Forms.Label")
                {
                    switch (typeName)
                    {
                        case "System.Windows.Forms.ComboBox":
                            var key = "";
                            if (c.Name != "comboTypeOfVesselUsed")
                            {
                                if (c.Text.Length>0) {
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
                        case "System.Windows.Forms.TextBox":
                        case "System.Windows.Forms.MaskedTextBox":
                            EffortData.Add(c.Tag.ToString(), c.Text);
                            break;
                        case "System.Windows.Forms.CheckBox":
                            EffortData.Add(c.Tag.ToString(), ((CheckBox)c).Checked.ToString());
                            break;
                    }
                }
            }

            if (_isNew)
                _samplingGUID = Guid.NewGuid().ToString();

            EffortData.Add("VesLength", _VesLength);
            EffortData.Add("VesHeight", _VesHeight);
            EffortData.Add("VesWidth", _VesWidth);
            EffortData.Add("SamplingGUID", _samplingGUID);
            EffortData.Add("SamplingType", "1");

            if (_sampling.UpdateEffort(_isNew, EffortData))
            {
                ;
            }

        }


        private void buttonSamplingFields_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "btnEnumerator":
                    break;
                case "btnGearClass":
                    frmGearUsage fgu = new frmGearUsage
                    {
                        GearClassName = _GearClassName,
                        GearRefCode = _GearRefCode,
                        Parent_Form = this
                    };
                    fgu.TargetArea(_AOIName, _AOIGuid);
                    fgu.GearVariation(_GearVarName, _GearVarGuid);
                    fgu.ShowDialog(this);
                    break;
                case "btnGearSpecs":
                    break;
                case "btnVesselDimension":
                    frmVesselDimension f = new frmVesselDimension();
                    if(_VesLength.Length>0 && _VesHeight.Length>0 && _VesWidth.Length>0)
                      f.VesselDimension(_VesLength, _VesWidth, _VesHeight);
                    f.Parent = this;
                    f.ShowDialog(this);
                    break;
                case "btnReferenceNumber":
                    break;
                case "btnFishingGround":
                    break;
            }
        }

        private void buttonSampling_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "buttonOK":
                    if (ValidateForm())
                    {
                        SaveEdits();
                        this.Close();
                    }
                    break;
                case "buttonCancel":
                    this.Close();
                    break;
            }
        }

        private void ShowRequiredErrorLabel(bool Visible)
        {
            foreach(Control c in panelUI.Controls)
            {
                if (c.Name.Substring(0,8)=="errLabel") {
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

        private void ShowWeightError(bool Visible)
        {
            foreach (Control c in panelUI.Controls)
            {
                if (c.Name == "errLabelWeightOfCatch" || c.Name == "errLabelWeightOfSample" )
                {
                    c.Visible = Visible;
                }
            }
        }

        private bool ValidateForm()
        {
            bool isValidated=true;
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

                    if (ui.Required && c.Text.Length==0)
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
                    panelUI.Controls["dtxtDateSet"].Text.Length>0 &&
                    panelUI.Controls["ttxtTimeSet"].Text.Length>0 &&
                    panelUI.Controls["dtxtDateHauled"].Text.Length > 0 &&
                    panelUI.Controls["ttxtTimeHauled"].Text.Length > 0
                    );
                if (!isValidated)
                {
                    GearDateTimeIsEmpty = (
                        panelUI.Controls["dtxtDateSet"].Text.Length == 0 &&
                        panelUI.Controls["ttxtTimeSet"].Text.Length == 0 &&
                        panelUI.Controls["dtxtDateHauled"].Text.Length == 0 &&
                        panelUI.Controls["ttxtTimeHauled"].Text.Length == 0
                        );
                    isValidated = GearDateTimeIsEmpty;
                    if(!isValidated)
                    {
                        ShowGearDateTimeErrorLabel(true);
                        msg = "Date and times of gear set and haul should \r\n" +
                              "all be filled in or should all be blank";
                    }
                    else
                    {
                        //if catch weight > zero then confirm if datetime of gear set and haul is empty
                        if (double.Parse(panelUI.Controls["textWeightOfCatch"].Text)>0 && GearDateTimeIsEmpty)
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
            if (isValidated && GearDateTimeIsEmpty==false)
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
                    }
                }
                else
                {
                    msg = "Sampling date should be after date of gear set and haul";
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


            if (msg.Length>0)
            {
                MessageBox.Show(msg,"Validation error",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return isValidated;
        }

        private void OnFieldChange(object sender, EventArgs e)
        {
            ShowGearDateTimeErrorLabel(false);
            ShowRequiredErrorLabel(false);
            ShowWeightError(false);
            if (sender.GetType().ToString() == "System.Windows.Forms.ComboBox")
            {
                if (((Control)sender).Name=="comboEnumerator")
                {
                    //System.Diagnostics.Debugger.Break();
                }
                
            }
        }
        private void OnFieldValidated(object sender, EventArgs e)
        {
            
            if (sender.GetType().ToString() == "System.Windows.Forms.ComboBox")
            {
                var combo = (ComboBox)sender;
                var targetCombo = new ComboBox();
                var key = "";
                var comboItems = new Dictionary<string, string>();

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
                                comboItems = aoi.AOIEnumeratorsList(key);
                                ChangeComboDataSource(targetCombo, comboItems);

                                targetCombo = (ComboBox)panelUI.Controls["comboFishingGear"];
                                string myGearClassGUID = ((KeyValuePair<string, string>)((ComboBox)panelUI.Controls["comboGearClass"]).SelectedItem).Key;
                                comboItems = global.GearVariationsUsage(myGearClassGUID, key);
                                ChangeComboDataSource(targetCombo, comboItems);



                                break;
                            case "comboLandingSite":
                                break;
                            case "comboEnumerator":
                                break;
                            case "comboGearClass":
                                _GearClassName = ((ComboBox)panelUI.Controls["comboGearClass"]).Text;
                                string myAOIGUID = ((KeyValuePair<string, string>)((ComboBox)panelUI.Controls["comboTargetArea"]).SelectedItem).Key;
                                targetCombo = (ComboBox)panelUI.Controls["comboFishingGear"];
                                comboItems = global.GearVariationsUsage(key, myAOIGUID);
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
        private void ChangeComboDataSource(ComboBox cbo, Dictionary<string,string> comboItems)
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
            if (!ui.ReadOnly && v.Length>0) {
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
                            if (ui.Control.ToString()== "DateMask")
                            {
                                if (myDateTime > DateTime.Now)
                                {
                                    msg = "Cannot accept a future date";
                                }
                            }
                            else
                            {
                                if(v.Length != 5)
                                {
                                    msg = "Expected time value should be in a 24 hour format";
                                }
                            }
                        }
                        catch
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
                        break;
                    case "lookup":
                        var key = "";
                        var cbo = (ComboBox)panelUI.Controls["combo" + ui.Key];
                        switch (ui.Key)
                        {
                            case "TargetArea":
                                key = ((KeyValuePair<string, string>)cbo.SelectedItem).Key;
                                e.Cancel = !aoi.AOIHaveEnumeratorsEx(key);
                                if (e.Cancel)
                                    msg = "Cannot use the selected target area because it does not have enumerators";

                                break;
                            case "GearClass":
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
                                    DialogResult dr = MessageBox.Show("Accept zero catch?", "Please verify", bt,MessageBoxIcon.Exclamation);
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

                if  (msg.Length>0)
                {
                    MessageBox.Show(msg,"Validation error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    e.Cancel = true;
                }
            }
        }

    }
}

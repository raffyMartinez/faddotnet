using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FAD3
{
    public partial class GMSDataEntryForm : Form
    {
        private string _CatchName = "";
        private string _CatchRowGuid = "";
        private bool _IsNew = false;
        private int _labelAdjust = 2;
        private TextBox _LastWeight;
        private TextBox _lastLength;
        private TextBox _lastSex;
        private TextBox _lastGMS;
        private TextBox _lastGonadWt;
        private int _row = 1;
        private sampling _sampling;
        private int _spacer = 3;
        private bool _UpdateSequence = false;
        private int _y = 5;
        private GMSManager.Taxa _taxa = GMSManager.Taxa.To_be_determined;
        private int _ctlHeight = 0;
        private int _ctlWidth = 0;
        private ComboBox _cboSex = new ComboBox();
        private ComboBox _cboGMS = new ComboBox();
        private TextBox _CurrentTextBox;
        private bool _ComboBoxesSet = false;

        public GMSDataEntryForm(bool IsNew, sampling sampling, string CatchRowGuid, string CatchName, GMSManager.Taxa taxa)
        {
            InitializeComponent();
            _CatchRowGuid = CatchRowGuid;
            _CatchName = CatchName;
            _sampling = sampling;
            _IsNew = IsNew;
            _taxa = taxa;
        }

        /// <summary>
        /// Populates fields if LF data exists or adds a new row
        /// </summary>
        /// <param name="IsNew"></param>
        private void PopulateFieldControls()
        {
            if (_IsNew)
            {
                //adds a new row of empty fields
                AddRow(IsNew: true);
            }
            else
            {
                foreach (KeyValuePair<string, GMSManager.GMSLine> kv in GMSManager.GMSData(_CatchRowGuid))
                {
                    //adds a row with fields containing the GMS data
                    AddRow(IsNew: false, kv.Value.Length, kv.Value.Weight, kv.Value.Sex, kv.Value.GMS,
                           kv.Value.Taxa, kv.Value.GonadWeight, kv.Value.RowGuid);
                }
            }

            foreach (Control c in panelUI.Controls)
            {
                if (c.GetType().Name == "TextBox")
                {
                    ((TextBox)c).With(o =>
                    {
                        o.Font = Font;
                        o.Height = _ctlHeight;
                    });
                }
            }
        }

        private void AddRow(bool IsNew, double? Len = null, double? Wgt = null,
                            GMSManager.sex Sex = GMSManager.sex.Female,
                            GMSManager.FishCrabGMS GMS = GMSManager.FishCrabGMS.AllTaxaNotDetermined,
                            GMSManager.Taxa taxa = GMSManager.Taxa.Fish, double? GonadWt = null,
                            string RowGuid = "")
        {
            var x = 3;
            Label labelRow = new Label();
            TextBox textLength = new TextBox();
            TextBox textWeight = new TextBox();
            TextBox textGonadWeight = new TextBox();
            TextBox textGMS = new TextBox();
            TextBox textSex = new TextBox();

            //we only add the comboboxes once
            if (_row == 1 && _ComboBoxesSet == false)
            {
                _ComboBoxesSet = true;

                _cboSex.With(o =>
                    {
                        o.Width = 120;
                        o.Name = "cboSex";
                        o.Location = new Point(0, 0);
                        o.Visible = false;
                        o.DropDownStyle = ComboBoxStyle.DropDownList;
                        o.DataSource = Enum.GetValues(typeof(GMSManager.sex));
                        o.AutoCompleteSource = AutoCompleteSource.ListItems;
                        o.Font = Font;
                        _ctlHeight = o.Height;
                        o.Validated += OnComboValidated;
                    });

                _cboGMS.With(o =>
                {
                    o.Width = 120;
                    o.Font = Font;
                    o.Name = "cboGMS";
                    o.Location = new Point(0, 0);
                    o.DropDownStyle = ComboBoxStyle.DropDownList;
                    o.Validated += OnComboValidated;
                    var hasGMSStage = false;
                    var gmsDict = GMSManager.GMSStages(_taxa, ref hasGMSStage);
                    if (hasGMSStage)
                    {
                        o.DataSource = new BindingSource(gmsDict, null);
                        o.DisplayMember = "Value";
                        o.ValueMember = "Key";
                        o.AutoCompleteSource = AutoCompleteSource.ListItems;
                    }
                    else
                    {
                    }
                    o.Visible = false;
                });

                panelUI.Controls.Add(_cboSex);
                panelUI.Controls.Add(_cboGMS);
            }

            panelUI.Controls.With(o =>
            {
                o.Add(labelRow);
                o.Add(textLength);
                o.Add(textWeight);
                o.Add(textSex);
                o.Add(textGMS);
                o.Add(textGonadWeight);
            });

            labelRow.With(o =>
            {
                o.Text = _row.ToString();
                o.Location = new Point(x, _y + _labelAdjust);
                o.Width = 40;
                o.Tag = global.fad3DataStatus.statusFromDB;
                if (IsNew) o.Tag = global.fad3DataStatus.statusNew;
                o.Name = "labelRow";
            });

            textLength.With(o =>
            {
                o.Width = 60;
                o.Name = "textLen";
                o.Location = new Point(labelRow.Left + labelRow.Width + _spacer, _y);
                if (Len != null) o.Text = Len.ToString();
                _ctlWidth = o.Width;
                o.TextChanged += OnTextChanged;
                o.GotFocus += OnTextFocus;
                o.Validating += OnTextValidating;

                //this stores the GMS row RowGuid;
                o.Tag = "";
                if (!IsNew) o.Tag = RowGuid;
            });

            textWeight.With(o =>
            {
                o.Width = 60;
                o.Name = "textWgt";
                o.Location = new Point(textLength.Left + textLength.Width + _spacer, _y);
                if (Wgt != null) o.Text = Wgt.ToString();
                o.TextChanged += OnTextChanged;
                o.GotFocus += OnTextFocus;
                o.Validating += OnTextValidating;
            });

            textSex.With(o =>
            {
                o.Width = 60;
                o.Name = "textSex";
                o.Location = new Point(textWeight.Left + textWeight.Width + _spacer, _y);
                o.Text = Sex.ToString();
                o.Width += (int)(_ctlWidth * 0.5);
                o.TextChanged += OnTextChanged;
                o.GotFocus += OnTextFocus;
                o.Validating += OnTextValidating;
            });

            textGMS.With(o =>
            {
                o.Width = 60;
                o.Name = "textGMS";
                o.Location = new Point(textSex.Left + textSex.Width + _spacer, _y);
                o.Width += _ctlWidth;
                o.Text = GMSManager.GMSStageToString(taxa, GMS);
                o.TextChanged += OnTextChanged;
                o.GotFocus += OnTextFocus;
                o.Validating += OnTextValidating;
            });

            textGonadWeight.With(o =>
            {
                o.Width = 60;
                o.Name = "textGonadWeight";
                o.Location = new Point(textGMS.Left + textGMS.Width + _spacer, _y);
                if (GonadWt != null) o.Text = GonadWt.ToString();
                o.TextChanged += OnTextChanged;
                o.GotFocus += OnTextFocus;
                o.Validating += OnTextValidating;
            });

            if (_row == 1)
            {
                labelCol1.Left = labelRow.Left;
                labelCol2.Left = textLength.Left;
                labelCol3.Left = textWeight.Left;
                labelCol4.Left = textSex.Left;
                labelCol5.Left = textGMS.Left;
                labelCol6.Left = textGonadWeight.Left;
            }

            _y += labelRow.Height + _spacer;

            if (_IsNew || _row >= GMSManager.GMSMeasurementRows)
            {
                _lastGMS = textGMS;
                _lastGonadWt = textGonadWeight;
                _lastLength = textLength;
                _lastSex = textSex;
                _LastWeight = textWeight;
            }
            _row++;
        }

        private void MarkRowAsEdited(TextBox Source)
        {
            foreach (Control c in panelUI.Controls)
            {
                if (c.Name == "labelRow"
                    && c.Location.Y - _labelAdjust == Source.Location.Y
                    && (global.fad3DataStatus)c.Tag == global.fad3DataStatus.statusFromDB)
                {
                    c.Tag = global.fad3DataStatus.statusEdited;
                }
            }
        }

        private Label getRowLabel(Point ControlLocation)
        {
            var myLabel = new Label();
            foreach (Control c in panelUI.Controls)
            {
                if (c.Name == "labelRow" && c.Location.Y == ControlLocation.Y)
                {
                    myLabel = (Label)c;
                    break;
                }
            }
            return myLabel;
        }

        private bool ValidateData()
        {
            bool notValid = false;
            int notValidCount = 0;
            foreach (Control c in panelUI.Controls)
            {
                if (c.GetType().Name == "TextBox")
                {
                    ((TextBox)c).With(o =>
                    {
                        switch (o.Name)
                        {
                            case "textLen":
                                notValid = chkLenght.Checked && o.Text.Length == 0;
                                break;

                            case "textWgt":
                                notValid = chkWeight.Checked && o.Text.Length == 0;
                                break;

                            case "textSex":
                                notValid = chkSex.Checked && o.Text.Length == 0;
                                break;

                            case "textGMS":
                                notValid = chkGMS.Checked && o.Text.Length == 0;
                                break;

                            case "textGonadWeight":
                                notValid = chkGonadWt.Checked && o.Text.Length == 0;
                                break;
                        }

                        if (notValid)
                        {
                            o.BackColor = global.MissingFieldBackColor;
                            notValidCount++;
                        }
                    });
                }
            }
            if (notValidCount > 0)
            {
                MessageBox.Show("Some required fields are blank", "Validation error",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return notValidCount == 0;
        }

        private void OnButton_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonOK":
                    if (ValidateData())
                    {
                        SaveOptionsToRegistry();
                        if (SaveGMS()) Close();
                    }
                    break;

                case "buttonCancel":
                    Close();
                    break;

                case "buttonAdd":

                    if ((chkSex.Checked && _lastSex.Text.Length == 0)
                        // if an option is checked, then its corresponding textbox must not be empty
                        || (chkLenght.Checked && _lastLength.Text.Length == 0)
                        || (chkGMS.Checked && _lastGMS.Text.Length == 0)
                        || (chkGonadWt.Checked && _lastGonadWt.Text.Length == 0)
                        || (chkWeight.Checked && _LastWeight.Text.Length == 0))
                    {
                        if (chkSex.Checked && _lastSex.Text.Length == 0) _lastSex.BackColor = global.MissingFieldBackColor;
                        if (chkLenght.Checked && _lastLength.Text.Length == 0) _lastLength.BackColor = global.MissingFieldBackColor;
                        if (chkGMS.Checked && _lastGMS.Text.Length == 0) _lastGMS.BackColor = global.MissingFieldBackColor;
                        if (chkGonadWt.Checked && _lastGonadWt.Text.Length == 0) _lastGonadWt.BackColor = global.MissingFieldBackColor;
                        if (chkWeight.Checked && _LastWeight.Text.Length == 0) _LastWeight.BackColor = global.MissingFieldBackColor;

                        MessageBox.Show("Please fill up the the data fields", "Validation error",
                                         MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        AddRow(IsNew: true);
                    }
                    break;

                case "buttonRemove":
                    break;
            }
        }

        private bool SaveGMS()
        {
            var gmsRows = new Dictionary<string, (string RowGuid, int Sequence, double length, double? weight,
                                GMSManager.sex sex, GMSManager.FishCrabGMS gms, double? gonadWeight,
                                string catchRowGuid, global.fad3DataStatus DataStatus)>();
            int n = 0;
            foreach (Control c in panelUI.Controls)
            {
                if (c.GetType().Name == "Label")
                {
                    var myRow = GetRowValues((Label)c);
                    var myDataStatus = (global.fad3DataStatus)Enum.Parse(typeof(global.fad3DataStatus), c.Tag.ToString());
                    var myRowGuid = myRow.rowGuid;
                    if (myDataStatus == global.fad3DataStatus.statusNew) myRowGuid = Guid.NewGuid().ToString();
                    gmsRows.Add(myRowGuid, (myRowGuid, n, myRow.length, myRow.weight, myRow.sex, myRow.gms, myRow.gonadWeight, _CatchRowGuid, myDataStatus));
                }
                n++;
            }
            return GMSManager.UpdateGMS(gmsRows);
        }

        private (string rowGuid, int length, double? weight, GMSManager.sex sex, GMSManager.FishCrabGMS gms, double? gonadWeight) GetRowValues(Label Row)
        {
            int len = 0;
            double? wgt = null;
            GMSManager.sex sex = GMSManager.sex.Female;
            GMSManager.FishCrabGMS gms = GMSManager.FishCrabGMS.AllTaxaNotDetermined;
            double? gonadWt = null;
            string rowGuid = "";
            var txtBox = new TextBox();

            foreach (Control c in panelUI.Controls)
            {
                if (c.GetType().Name == "TextBox" && c.Location.Y == Row.Location.Y - _labelAdjust)
                {
                    ((TextBox)c).With(o =>
                    {
                        switch (o.Name)
                        {
                            case "textLen":
                                len = int.Parse(o.Text);
                                rowGuid = o.Tag.ToString();
                                break;

                            case "textWgt":
                                break;

                            case "textSex":
                                sex = (GMSManager.sex)Enum.Parse(typeof(GMSManager.sex), o.Text);
                                break;

                            case "textGMS":
                                gms = GMSManager.MaturityStageFromText(o.Text, _taxa);
                                break;

                            case "textGonadWeight":
                                if (o.Text.Length > 0) gonadWt = double.Parse(o.Text);
                                break;
                        }
                    });
                }
            }
            return (rowGuid, len, wgt, sex, gms, gonadWt);
        }

        private void ShowOptions(bool Visible)
        {
            if (Visible)
                panelOptions.Show();
            else
                panelOptions.Hide();
        }

        private void OnGMSDataEntryForm_Load(object sender, EventArgs e)
        {
            labelTitle.Text = $"GMS data table for {_CatchName}";
            if (_IsNew)
            {
                labelTitle.Text = $"New GMS data table for {_CatchName}";
            }
            PopulateFieldControls();

            RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\FAD3");
            var CheckedOptions = rk.GetValue("GMSData", "NULL").ToString().Split(',');

            //check what is saved in the registry
            if (CheckedOptions[0] != "NULL")
            {
                for (int i = 0; i < CheckedOptions.Length; i++)
                {
                    ((CheckBox)panelOptions.Controls[$"chk{CheckedOptions[i]}"]).Checked = true;
                }
            }

            ShowOptions(false);
        }

        /// <summary>
        /// access the built-in options dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnbuttonOptions_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonOptions":
                    _cboGMS.Hide();
                    _cboSex.Hide();
                    ShowOptions(true);
                    break;

                case "buttonOptionsOK":
                    if (!chkGMS.Checked || !chkSex.Checked || !chkLenght.Checked)
                    {
                        MessageBox.Show("Length, sex, and gonad maturity stage should be checked",
                            "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        ShowOptions(false);
                    }

                    break;

                case "buttonOptionsCancel":
                    ShowOptions(false);
                    break;
            }
        }

        /// <summary>
        /// save what has been checked in the options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveOptionsToRegistry()
        {
            //this is the actual value to save in the registry
            var SaveGMSData = "";

            foreach (Control c in panelOptions.Controls)
            {
                if (c.GetType().Name == "CheckBox")
                {
                    ((CheckBox)c).With(o =>
                    {
                        if (o.Checked)
                        {
                            //append the checked checkboxes
                            SaveGMSData += $"{o.Name.Replace("chk", "")},";
                        }
                    });
                }
            }

            //remove trailing comma and space
            SaveGMSData = SaveGMSData.Trim(new char[] { ' ', ',' });

            //save to registry
            RegistryKey rk = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\FAD3");
            rk.SetValue("GMSData", SaveGMSData, RegistryValueKind.String);
            rk.Close();
        }

        private void OnTextFocus(object sender, EventArgs e)
        {
            _cboSex.Hide();
            _cboGMS.Hide();

            ((TextBox)sender).With(o =>
            {
                _CurrentTextBox = o;

                switch (o.Name)
                {
                    case "textSex":
                        _cboSex.Visible = true;
                        _cboSex.Bounds = o.Bounds;
                        _cboSex.BringToFront();
                        _cboSex.Text = o.Text;
                        break;

                    case "textGMS":
                        _cboGMS.Visible = true;
                        _cboGMS.Bounds = o.Bounds;
                        _cboGMS.BringToFront();
                        _cboGMS.Text = o.Text;
                        break;
                }
            });
        }

        private void OnComboValidated(object sender, EventArgs e)
        {
            _CurrentTextBox.Text = ((ComboBox)sender).Text;
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = SystemColors.Window;
            MarkRowAsEdited((TextBox)sender);
        }

        private void OnTextValidating(object sender, CancelEventArgs e)
        {
        }
    }
}
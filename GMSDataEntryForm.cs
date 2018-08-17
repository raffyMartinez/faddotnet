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
        private ComboBox _cboEditor = new ComboBox();
        private ComboBox _cboGMS = new ComboBox();
        private ComboBox _cboSex = new ComboBox();
        private bool _ComboBoxesSet = false;
        private int _ctlHeight = 0;
        private int _ctlWidth = 0;
        private TextBox _CurrentTextBox;
        private Dictionary<int, GMSLineClass> _GMSData = new Dictionary<int, GMSLineClass>();
        private bool _IsNew = false;
        private int _labelAdjust = 2;
        private TextBox _lastGMS;
        private TextBox _lastGonadWt;
        private TextBox _lastLength;
        private TextBox _lastSex;
        private TextBox _LastWeight;
        private int _row = 1;
        private sampling _sampling;
        private int _ScrollAmount = 0;
        private int _spacer = 3;
        private GMSManager.Taxa _taxa = GMSManager.Taxa.To_be_determined;
        private bool _UpdateSequence = false;
        private int _y = 5;

        public GMSDataEntryForm(bool IsNew, sampling sampling, string CatchRowGuid, string CatchName, GMSManager.Taxa taxa)
        {
            InitializeComponent();
            _CatchRowGuid = CatchRowGuid;
            _CatchName = CatchName;
            _sampling = sampling;
            _IsNew = IsNew;
            _taxa = taxa;
        }

        //private void AddRow(bool IsNew, double? Len = null, double? Wgt = null,
        //                    GMSManager.sex Sex = GMSManager.sex.Female,
        //                    GMSManager.FishCrabGMS GMS = GMSManager.FishCrabGMS.AllTaxaNotDetermined,
        //                    GMSManager.Taxa taxa = GMSManager.Taxa.Fish, double? GonadWt = null,
        //                    string RowGuid = "")
        private void AddRow(bool IsNew, GMSLineClass gmsLine = null)
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
                        o.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                        o.Font = Font;
                        _ctlHeight = o.Height;
                        //o.Validated += OnComboValidated;
                        //o.KeyDown += OnCombo_Keydown;
                    });

                _cboGMS.With(o =>
                {
                    o.Width = 120;
                    o.Font = Font;
                    o.Name = "cboGMS";
                    o.Location = new Point(0, 0);
                    o.DropDownStyle = ComboBoxStyle.DropDownList;
                    o.AutoCompleteSource = AutoCompleteSource.ListItems;
                    o.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    //o.Validated += OnComboValidated;
                    //o.KeyDown += OnCombo_Keydown;
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

            if (IsNew)
            {
                _GMSData.Add(_y, new GMSLineClass(_CatchRowGuid));
                _GMSData[_y].RowGuid = Guid.NewGuid().ToString();
                _GMSData[_y].Sequence = _row;
                _GMSData[_y].DataStatus = global.fad3DataStatus.statusNew;
                _GMSData[_y].Taxa = _taxa;
            }
            else
            {
                _GMSData.Add(_y, gmsLine);
            }

            labelRow.With(o =>
            {
                o.Text = _row.ToString();
                o.Location = new Point(x, _y + _labelAdjust);
                o.Width = 40;
                o.Name = "labelRow";
            });

            textLength.With(o =>
            {
                o.Width = 60;
                o.Name = "textLen";
                o.Location = new Point(labelRow.Left + labelRow.Width + _spacer, _y);
                _ctlWidth = o.Width;
                o.KeyPress += OnTextBoxKeyPress;
                o.TextChanged += OnTextChanged;
                o.GotFocus += OnTextFocus;
                o.Validating += OnTextValidating;
                o.Tag = "";
                if (!IsNew)
                {
                    if (_GMSData[_y].Length != null) o.Text = _GMSData[_y].Length.ToString();
                    o.Tag = o.Text;
                }
            });

            textWeight.With(o =>
            {
                o.Width = 60;
                o.Name = "textWgt";
                o.Location = new Point(textLength.Left + textLength.Width + _spacer, _y);
                o.TextChanged += OnTextChanged;
                o.GotFocus += OnTextFocus;
                o.Validating += OnTextValidating;
                o.KeyPress += OnTextBoxKeyPress;
                if (!IsNew)
                {
                    if (_GMSData[_y].Weight != null) o.Text = _GMSData[_y].Weight.ToString();
                    o.Tag = o.Text;
                }
            });

            textSex.With(o =>
            {
                o.Width = 60;
                o.Name = "textSex";
                o.Location = new Point(textWeight.Left + textWeight.Width + _spacer, _y);
                o.Width += (int)(_ctlWidth * 0.5);
                o.TextChanged += OnTextChanged;
                o.GotFocus += OnTextFocus;
                o.Validating += OnTextValidating;
                o.KeyPress += OnTextBoxKeyPress;
                if (!IsNew)
                {
                    o.Text = _GMSData[_y].Sex.ToString();
                    o.Tag = o.Text;
                }
            });

            textGMS.With(o =>
            {
                o.Width = 60;
                o.Name = "textGMS";
                o.Location = new Point(textSex.Left + textSex.Width + _spacer, _y);
                o.Width += _ctlWidth;
                o.TextChanged += OnTextChanged;
                o.GotFocus += OnTextFocus;
                o.Validating += OnTextValidating;
                o.KeyPress += OnTextBoxKeyPress;
                if (!IsNew)
                {
                    o.Text = GMSManager.GMSStageToString(_GMSData[_y].Taxa, _GMSData[_y].GMS);
                    o.Tag = o.Text;
                }
            });

            textGonadWeight.With(o =>
            {
                o.Width = 60;
                o.Name = "textGonadWeight";
                o.Location = new Point(textGMS.Left + textGMS.Width + _spacer, _y);
                o.TextChanged += OnTextChanged;
                o.GotFocus += OnTextFocus;
                o.Validating += OnTextValidating;
                o.KeyPress += OnTextBoxKeyPress;
                if (!IsNew)
                {
                    if (_GMSData[_y].GonadWeight != null) o.Text = _GMSData[_y].GonadWeight.ToString();
                    o.Tag = o.Text;
                }
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

        private void ControlToFocus(TextBox txt, char key = ' ')
        {
            _CurrentTextBox = txt;
            _cboEditor.Hide();
            bool Proceed = true;
            var s = txt.Text;
            switch (txt.Name)
            {
                case "textSex":
                    _cboEditor = _cboSex;
                    break;

                case "textGMS":
                    _cboEditor = _cboGMS;
                    break;

                default:
                    Proceed = false;
                    break;
            }

            if (Proceed)
            {
                _cboEditor.Show();
                _cboEditor.Bounds = txt.Bounds;
                _cboEditor.BringToFront();
                _cboEditor.Show();
                _cboEditor.Focus();

                if (key > ' ')
                {
                    var itemIndex = _cboEditor.FindString(key.ToString());
                    if (itemIndex > -1)
                    {
                        if (txt.Name == "textSex")
                            _cboEditor.Text = _cboEditor.Items[itemIndex].ToString();
                        else
                        {
                            var gmsStage = (KeyValuePair<GMSManager.FishCrabGMS, string>)_cboEditor.Items[itemIndex];
                            _cboEditor.Text = GMSManager.GMSStageToString(_taxa, gmsStage.Key);
                        }
                    }

                    //_cboEditor.SelectionStart = 0;
                    //_cboEditor.SelectionLength = s.Length;
                }

                if (key > 32)
                {
                    _cboEditor.Text = key.ToString();
                    _cboEditor.SelectionStart = 1;
                }

                SetEditorEvents();
            }
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
                                if (o.Text.Length > 0) wgt = double.Parse(o.Text);
                                break;

                            case "textSex":
                                sex = (GMSManager.sex)Enum.Parse(typeof(GMSManager.sex), o.Text);
                                break;

                            case "textGMS":
                                gms = GMSManager.GMSStageFromString(o.Text, _taxa);
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

        private TextBox GetTextBox(ComboBox fromComboBox = null, TextBox fromTextBox = null, bool GetNext = false)
        {
            var myTextBox = new TextBox();
            var myTextBoxName = "";
            var sourceYPosition = 0;

            if (fromComboBox != null)
            {
                sourceYPosition = fromComboBox.Location.Y;
                switch (fromComboBox.Name)
                {
                    case "cboSex":
                        myTextBoxName = "textSex";
                        if (GetNext) myTextBoxName = "textGMS";
                        break;

                    case "cboGMS":

                        myTextBoxName = "textGMS";
                        if (GetNext) myTextBoxName = "textGonadWeight";
                        break;
                }
            }

            if (fromTextBox != null)
            {
                sourceYPosition = fromTextBox.Location.Y;
                myTextBoxName = fromTextBox.Name;

                switch (myTextBoxName)
                {
                    case "textLen":
                        if (GetNext)
                        {
                            myTextBoxName = "textWgt";
                        }
                        break;

                    case "textWgt":
                        if (GetNext)
                        {
                            myTextBoxName = "textSex";
                        }
                        break;

                    case "textSex":
                        if (GetNext)
                        {
                            myTextBoxName = "textGMS";
                        }
                        break;

                    case "textGMS":
                        if (GetNext)
                        {
                            myTextBoxName = "textGonadWeight";
                        }
                        break;
                }
            }

            foreach (Control c in panelUI.Controls)
            {
                if (c.Name == myTextBoxName && c.Location.Y == sourceYPosition)
                {
                    myTextBox = (TextBox)c;
                    break;
                }
            }
            return myTextBox;
        }

        private bool IsValidDoubleValue(string inValue)
        {
            return double.TryParse(inValue, out double myValue) && myValue > 0;
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
                        _lastLength.Focus();
                    }
                    break;

                case "buttonRemove":
                    break;
            }
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

        private void OncboEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                GetTextBox(_cboEditor, GetNext: true).Focus();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                _cboEditor.Hide();
            }
        }

        private void OncboEditor_Validating(object sender, CancelEventArgs e)
        {
            switch (_cboEditor.Name)
            {
                case "cboSex":
                    _GMSData[_cboEditor.Location.Y + _ScrollAmount].Sex = (GMSManager.sex)Enum.Parse(typeof(GMSManager.sex), _cboEditor.Text);
                    break;

                case "cboGMS":
                    _GMSData[_cboEditor.Location.Y + _ScrollAmount].GMS = GMSManager.GMSStageFromString(_cboEditor.Text, _GMSData[_cboEditor.Location.Y + _ScrollAmount].Taxa);
                    break;
            }

            if (_CurrentTextBox.Text != _cboEditor.Text)
            {
                _CurrentTextBox.Text = _cboEditor.Text;
                SetRowStatusToEdited(_CurrentTextBox);
            }
        }

        private void OnCombo_Keydown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Return)
            //{
            //    GetTextBox((ComboBox)sender, GetNext: true).Focus();
            //}
            //else if (e.KeyCode == Keys.Escape)
            //{
            //    ((ComboBox)sender).Hide();
            //}
        }

        private void OnComboValidated(object sender, EventArgs e)
        {
        }

        private void OnGMSDataEntryForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return && e.Shift)
            {
                var ev = new EventArgs();
                OnButton_Click(buttonAdd, ev);
            }
            else if (e.KeyCode == Keys.Return)
            {
                if (ActiveControl.GetType().Name == "TextBox")
                {
                    if (ActiveControl.Name == "textGonadWeight")
                    {
                        buttonAdd.Focus();
                    }
                    else
                    {
                        GetTextBox(fromTextBox: (TextBox)this.ActiveControl, GetNext: true).Focus();
                    }
                }
            }
        }

        private void OnGMSDataEntryForm_Load(object sender, EventArgs e)
        {
            panelUI.MouseWheel += OnPanelMouseWheel;
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

        private void OnPanelMouseWheel(object sender, MouseEventArgs e)
        {
            _ScrollAmount = panelUI.VerticalScroll.Value;
        }

        private void OnTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            ControlToFocus((TextBox)sender, e.KeyChar);
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = SystemColors.Window;
        }

        private void OnTextFocus(object sender, EventArgs e)
        {
            _cboSex.Hide();
            _cboGMS.Hide();
            _cboEditor.Hide();
        }

        private void OnTextValidating(object sender, CancelEventArgs e)
        {
            var msg = "Expected value is a number greater than zero";
            ((TextBox)sender).With(o =>
            {
                if (o.Text != (o.Tag == null ? "" : o.Tag.ToString()))
                {
                    o.Tag = o.Text;
                    SetRowStatusToEdited(o);
                    o.BackColor = SystemColors.Window;
                }

                switch (o.Name)
                {
                    case "textLen":
                        if (o.Text.Length == 0)
                        {
                            _GMSData[o.Location.Y + _ScrollAmount].Length = null;
                        }
                        else
                        {
                            if (IsValidDoubleValue(o.Text))
                                _GMSData[o.Location.Y + _ScrollAmount].Length = double.Parse(o.Text);
                            else
                                e.Cancel = true;
                        }
                        break;

                    case "textWgt":
                        if (o.Text.Length == 0)
                        {
                            _GMSData[o.Location.Y + _ScrollAmount].Weight = null;
                        }
                        else
                        {
                            if (IsValidDoubleValue(o.Text))
                                _GMSData[o.Location.Y + _ScrollAmount].Weight = double.Parse(o.Text);
                            else
                                e.Cancel = true;
                        }
                        break;

                    case "textGonadWeight":
                        if (o.Text.Length == 0)
                        {
                            _GMSData[o.Location.Y + _ScrollAmount].GonadWeight = null;
                        }
                        else
                        {
                            if (IsValidDoubleValue(o.Text))
                                _GMSData[o.Location.Y + _ScrollAmount].GonadWeight = double.Parse(o.Text);
                            else
                                e.Cancel = true;
                        }
                        break;
                }
            });

            if (e.Cancel)
                MessageBox.Show(msg, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void panelUI_Scroll(object sender, ScrollEventArgs e)
        {
            _ScrollAmount = e.NewValue;
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
                foreach (KeyValuePair<string, GMSLineClass> kv in GMSManager.RetrieveGMSData(_CatchRowGuid))
                {
                    AddRow(IsNew: false, kv.Value);
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

        private bool SaveGMS()
        {
            return GMSManager.UpdateGMSData(_GMSData);
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

        private void SetEditorEvents()
        {
            _cboEditor.KeyDown += OncboEditor_KeyDown;
            //_cboEditor.KeyPress += OncboEditor_KeyPress;
            _cboEditor.Validating += OncboEditor_Validating;
        }

        private void SetRowStatusToEdited(Control source)
        {
            if (_GMSData[source.Location.Y + _ScrollAmount].DataStatus != global.fad3DataStatus.statusNew)
            {
                _GMSData[source.Location.Y + _ScrollAmount].DataStatus = global.fad3DataStatus.statusEdited;
            }
        }

        private void ShowOptions(bool Visible)
        {
            if (Visible)
                panelOptions.Show();
            else
                panelOptions.Hide();
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
    }
}
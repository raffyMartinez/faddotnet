using FAD3.Database.Classes;
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
        private Dictionary<string, GMSLineClass> _gmsData = new Dictionary<string, GMSLineClass>();

        //represents the last row of fields
        private TextBox _lastGMS;
        private TextBox _lastGonadWt;
        private TextBox _lastLength;
        private TextBox _lastSex;
        private TextBox _LastWeight;

        //this combobox will be placed over text fields and will provide the contents for the textbox
        private ComboBox _cboEditor = new ComboBox();

        //holds the actual GMS and Sex values. These are hidden and will be set to _cboEditor
        private ComboBox _cboGMS = new ComboBox();
        private ComboBox _cboSex = new ComboBox();

        private string _catchName = "";
        private string _catchRowGuid = "";
        private bool _comboBoxesSet = false;
        private int _ctlHeight;
        private int _ctlWidth;
        private TextBox _currentTextBox;
        private bool _isNew = false;
        private int _row = 1;
        private Sampling _sampling;
        private int _spacer = 3;
        private Taxa _taxa = Taxa.To_be_determined;
        private bool _updateSequence = false;
        private int _y = 5;
        private MainForm _parent_form;

        private string _currentRow = "";
        private string _currentTextContents = "";
        private bool _errorValidating = false;

        public GMSDataEntryForm(bool isNew, Sampling sampling, string catchRowGuid, string catchName, Taxa taxa, MainForm parent)
        {
            InitializeComponent();
            _catchRowGuid = catchRowGuid;
            _catchName = catchName;
            _sampling = sampling;
            _isNew = isNew;
            _taxa = taxa;
            _parent_form = parent;
        }

        private void SetTextBoxEvents(TextBox t)
        {
            t.KeyPress += OnTextBoxKeyPress;
            t.TextChanged += OnTextChanged;
            t.GotFocus += OnTextFocus;
            t.Validating += OnTextValidating;
            t.DoubleClick += OnTextBoxDoubleClick;
        }

        private void OnTextKeyDown(object sender, KeyEventArgs e)
        {
        }

        private void AddRow(bool IsNew, string key = "", GMSLineClass gmsLine = null)
        {
            const int x = 0;
            int yPos = _y - Math.Abs(panelUI.AutoScrollPosition.Y);

            Label labelRow = new Label();
            TextBox textLength = new TextBox();
            TextBox textWeight = new TextBox();
            TextBox textGonadWeight = new TextBox();
            TextBox textGMS = new TextBox();
            TextBox textSex = new TextBox();

            //we only add the comboboxes once
            if (_row == 1 && _comboBoxesSet == false)
            {
                _comboBoxesSet = true;

                _cboSex.With(o =>
                    {
                        o.Width = 120;
                        o.Name = "cboSex";
                        o.Location = new Point(0, 0);
                        o.Visible = false;
                        o.DropDownStyle = ComboBoxStyle.DropDownList;
                        o.DataSource = Enum.GetValues(typeof(Sex));
                        o.AutoCompleteSource = AutoCompleteSource.ListItems;
                        o.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                        o.Font = Font;
                        _ctlHeight = o.Height;
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
                key = Guid.NewGuid().ToString();
                _gmsData.Add(key, new GMSLineClass(_catchRowGuid));
                _gmsData[key].RowGuid = Guid.NewGuid().ToString();
                _gmsData[key].Sequence = _row;
                _gmsData[key].DataStatus = fad3DataStatus.statusNew;
                _gmsData[key].Taxa = _taxa;
            }

            labelRow.With(o =>
            {
                o.Text = _row.ToString();
                o.Location = new Point(x, yPos);
                o.Width = 40;
                o.TextAlign = ContentAlignment.MiddleLeft;
                o.Name = "labelRow";
                o.Tag = key;
            });

            textLength.With(o =>
            {
                o.Width = 60;
                o.Name = "textLen";
                o.Location = new Point(labelRow.Left + labelRow.Width + _spacer, yPos);
                _ctlWidth = o.Width;
                if (!IsNew)
                {
                    if (_gmsData[key].Length != null) o.Text = _gmsData[key].Length.ToString();
                }
                o.Tag = key;
                SetTextBoxEvents(o);
            });

            textWeight.With(o =>
            {
                o.Width = 60;
                o.Name = "textWgt";
                o.Location = new Point(textLength.Left + textLength.Width + _spacer, yPos);
                if (!IsNew)
                {
                    if (_gmsData[key].Weight != null) o.Text = _gmsData[key].Weight.ToString();
                }
                o.Tag = key;
                SetTextBoxEvents(o);
            });

            textSex.With(o =>
            {
                o.Width = 60;
                o.Name = "textSex";
                o.Location = new Point(textWeight.Left + textWeight.Width + _spacer, yPos);
                o.Width += (int)(_ctlWidth * 0.5);
                if (!IsNew)
                {
                    o.Text = _gmsData[key].Sex.ToString();
                }
                o.Tag = key;
                SetTextBoxEvents(o);
            });

            textGMS.With(o =>
            {
                o.Width = 60;
                o.Name = "textGMS";
                o.Location = new Point(textSex.Left + textSex.Width + _spacer, yPos);
                o.Width += _ctlWidth;
                if (!IsNew)
                {
                    o.Text = GMSManager.GMSStageToString(_gmsData[key].Taxa, _gmsData[key].GMS);
                }
                o.Tag = key;
                SetTextBoxEvents(o);
            });

            textGonadWeight.With(o =>
            {
                o.Width = 60;
                o.Name = "textGonadWeight";
                o.Location = new Point(textGMS.Left + textGMS.Width + _spacer, yPos);
                if (!IsNew)
                {
                    if (_gmsData[key].GonadWeight != null) o.Text = _gmsData[key].GonadWeight.ToString();
                }
                o.Tag = key;
                SetTextBoxEvents(o);
            });

            textLength.Enabled = chkLenght.Checked;
            textWeight.Enabled = chkWeight.Checked;
            textSex.Enabled = chkSex.Checked;
            textGMS.Enabled = chkGMS.Checked;
            textGonadWeight.Enabled = chkGonadWt.Checked;

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

            if (IsNew || _row >= GMSManager.GMSMeasurementRows)
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
            _currentTextBox = txt;
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
                _cboEditor.Tag = txt.Tag;
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
                            var gmsStage = (KeyValuePair<FishCrabGMS, string>)_cboEditor.Items[itemIndex];
                            _cboEditor.Text = GMSManager.GMSStageToString(_taxa, gmsStage.Key);
                        }
                    }

                    _cboEditor.Text = key.ToString();
                }
                else if (s.Length > 0)
                {
                    _cboEditor.Text = s;
                }

                SetEditorEvents();
            }
        }

        private TextBox GetTextBox(ComboBox fromComboBox = null, TextBox fromTextBox = null, bool getNext = false)
        {
            var myTextBox = new TextBox();
            var myTextBoxName = "";
            var sourceTag = "";

            if (fromComboBox != null)
            {
                sourceTag = fromComboBox.Tag.ToString();
                switch (fromComboBox.Name)
                {
                    case "cboSex":
                        myTextBoxName = "textSex";
                        if (getNext) myTextBoxName = "textGMS";
                        break;

                    case "cboGMS":

                        myTextBoxName = "textGMS";
                        if (getNext) myTextBoxName = "textGonadWeight";
                        break;
                }
            }

            if (fromTextBox != null)
            {
                sourceTag = fromTextBox.Tag.ToString();
                myTextBoxName = fromTextBox.Name;

                switch (myTextBoxName)
                {
                    case "textLen":
                        if (getNext)
                        {
                            myTextBoxName = "textWgt";
                        }
                        break;

                    case "textWgt":
                        if (getNext)
                        {
                            myTextBoxName = "textSex";
                        }
                        break;

                    case "textSex":
                        if (getNext)
                        {
                            myTextBoxName = "textGMS";
                        }
                        break;

                    case "textGMS":
                        if (getNext)
                        {
                            myTextBoxName = "textGonadWeight";
                        }
                        break;
                }
            }

            foreach (Control c in panelUI.Controls)
            {
                if (c.Name == myTextBoxName && c.Tag.ToString() == sourceTag)
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
                        if (SaveGMS())
                        {
                            Close();
                            _parent_form.RefreshLF_GMS();
                        }
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
                    GMSLineClass gls = _gmsData[_currentRow];
                    gls.DataStatus = fad3DataStatus.statusForDeletion;
                    _gmsData[_currentRow] = gls;
                    panelUI.Controls.Clear();
                    if (_gmsData.Count == 1 || FieldRowCount() == 0)
                    {
                        PopulateFieldControls(isNew: true);
                    }
                    else
                    {
                        PopulateFieldControls(isNew: false, repopulate: true);
                    }
                    break;
            }
        }

        private void SetupUIFromOptions()
        {
            foreach (Control c in panelUI.Controls)
            {
                switch (c.Name)
                {
                    case "textLen":
                        c.Enabled = chkLenght.Checked;
                        break;

                    case "textWgt":
                        c.Enabled = chkWeight.Checked;
                        break;

                    case "textSex":
                        c.Enabled = chkSex.Checked;
                        break;

                    case "textGMS":
                        c.Enabled = chkGMS.Checked;
                        break;

                    case "textGonadWeight":
                        c.Enabled = chkGonadWt.Checked;
                        break;
                }
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
                    SetupUIFromOptions();
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
                GetTextBox(_cboEditor, getNext: true).Focus();
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
                    _gmsData[_cboEditor.Tag.ToString()].Sex = (Sex)Enum.Parse(typeof(Sex), _cboEditor.Text);
                    break;

                case "cboGMS":
                    _gmsData[_cboEditor.Tag.ToString()].GMS = GMSManager.GMSStageFromString(_cboEditor.Text, _gmsData[_cboEditor.Tag.ToString()].Taxa);
                    break;
            }

            if (_currentTextBox.Text != _cboEditor.Text)
            {
                _currentTextBox.Text = _cboEditor.Text;
                SetRowStatusToEdited(_currentTextBox);
            }
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
                        GetTextBox(fromTextBox: (TextBox)this.ActiveControl, getNext: true).Focus();
                    }
                }
            }
        }

        private void OnGMSDataEntryForm_Load(object sender, EventArgs e)
        {
            labelTitle.Text = $"GMS data table for {_catchName}";
            if (_isNew)
            {
                labelTitle.Text = $"New GMS data table for {_catchName}";
            }
            PopulateFieldControls(_isNew);

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

        private void OnTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            ControlToFocus((TextBox)sender, e.KeyChar);
            if (e.KeyChar == (char)Keys.Return)
            {
                e.Handled = true;
            }
        }

        private void OnTextBoxDoubleClick(object sender, EventArgs e)
        {
            ControlToFocus((TextBox)sender);
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = SystemColors.Window;
        }

        private void OnTextFocus(object sender, EventArgs e)
        {
            _currentRow = ((TextBox)sender).Tag.ToString();
            if (!_errorValidating)
            {
                _currentTextContents = ((TextBox)sender).Text;
            }
            _cboSex.Hide();
            _cboGMS.Hide();
            _cboEditor.Hide();
        }

        private void OnTextValidating(object sender, CancelEventArgs e)
        {
            _errorValidating = false;
            var msg = "Expected value is a number greater than zero";
            ((TextBox)sender).With(o =>
            {
                switch (o.Name)
                {
                    case "textLen":
                        if (o.Text.Length == 0)
                        {
                            _gmsData[o.Tag.ToString()].Length = null;
                        }
                        else
                        {
                            if (IsValidDoubleValue(o.Text))
                                _gmsData[o.Tag.ToString()].Length = double.Parse(o.Text);
                            else
                                e.Cancel = true;
                        }
                        break;

                    case "textWgt":
                        if (o.Text.Length == 0)
                        {
                            _gmsData[o.Tag.ToString()].Weight = null;
                        }
                        else
                        {
                            if (IsValidDoubleValue(o.Text))
                                _gmsData[o.Tag.ToString()].Weight = double.Parse(o.Text);
                            else
                                e.Cancel = true;
                        }
                        break;

                    case "textGonadWeight":
                        if (o.Text.Length == 0)
                        {
                            _gmsData[o.Tag.ToString()].GonadWeight = null;
                        }
                        else
                        {
                            if (IsValidDoubleValue(o.Text))
                                _gmsData[o.Tag.ToString()].GonadWeight = double.Parse(o.Text);
                            else
                                e.Cancel = true;
                        }
                        break;
                }

                //mark the row as edited if resulting edits did not produce
                //validation errors and text content is not the same
                if (o.Text != _currentTextContents && !e.Cancel)
                {
                    _currentTextContents = o.Text;
                    SetRowStatusToEdited(o);
                    o.BackColor = SystemColors.Window;
                }
            });

            if (e.Cancel)
            {
                _errorValidating = false;
                MessageBox.Show(msg, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private int FieldRowCount()
        {
            var n = 0;
            foreach (var item in _gmsData)
            {
                if (item.Value.DataStatus != fad3DataStatus.statusForDeletion)
                {
                    n++;
                }
            }
            return n;
        }

        /// <summary>
        /// Populates fields if GMS data exists or adds a new row
        /// </summary>
        /// <param name="IsNew"></param>
        private void PopulateFieldControls(bool isNew, bool repopulate = false)
        {
            _y = 5;
            _row = 1;
            if (repopulate)
            {
                foreach (var item in _gmsData)
                {
                    if (item.Value.DataStatus != fad3DataStatus.statusForDeletion)
                    {
                        AddRow(IsNew: false, item.Key, item.Value);
                    }
                }
            }
            else
            {
                if (isNew)
                {
                    //adds a new row of empty fields
                    AddRow(IsNew: true);
                }
                else
                {
                    _gmsData = GMSManager.RetrieveGMSData(_catchRowGuid);
                    foreach (var item in _gmsData)
                    {
                        AddRow(IsNew: false, item.Key, item.Value);
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
        }

        private bool SaveGMS()
        {
            return GMSManager.UpdateGMSData(_gmsData);
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
            _cboEditor.Validating += OncboEditor_Validating;
        }

        private void SetRowStatusToEdited(Control source)
        {
            if (_gmsData[source.Tag.ToString()].DataStatus != fad3DataStatus.statusNew)
            {
                _gmsData[source.Tag.ToString()].DataStatus = fad3DataStatus.statusEdited;
            }
        }

        private void ShowOptions(bool visible)
        {
            if (visible)
            {
                panelOptions.Show();
            }
            else
            {
                panelOptions.Hide();
            }
            buttonOK.Enabled = !visible;
            buttonCancel.Enabled = !visible;
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
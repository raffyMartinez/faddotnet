using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FAD3
{
    public partial class CatchCompositionForm : Form
    {
        private Dictionary<int, CatchLineClass> _CatchCompositionData = new Dictionary<int, CatchLineClass>();
        private ComboBox _cboEditor = new ComboBox();
        private bool _ComboBoxesSet = false;
        private ComboBox _comboGenus = new ComboBox();
        private ComboBox _comboIdentificationType = new ComboBox();
        private ComboBox _comboLocalName = new ComboBox();
        private ComboBox _comboSpecies = new ComboBox();
        private int _ctlHeight;
        private int _ctlWidth = 0;
        private string _currentGenus = "";
        private CatchComposition.Identification _CurrentIDType;
        private TextBox _currentTextBox;
        private bool _IsNew;
        private TextBox _lastCount;
        private TextBox _lastIdentification;
        private TextBox _lastName1;
        private TextBox _lastName2;
        private TextBox _lastSubCount;
        private TextBox _lastSubWeight;
        private TextBox _lastWeight;
        private MainForm _parentForm;
        private string _ReferenceNumber;
        private int _row = 1;
        private string _SamplingGuid;
        private int _spacer = 3;
        private int _y = 0;

        public CatchCompositionForm(bool IsNew, MainForm parent, string SamplingGuid, string ReferenceNumber)
        {
            InitializeComponent();
            _parentForm = parent;
            _SamplingGuid = SamplingGuid;
            _IsNew = IsNew;
            _ReferenceNumber = ReferenceNumber;
        }

        private CatchComposition.Identification CurrentIDType
        {
            get { return _CurrentIDType; }
            set { _CurrentIDType = value; }
        }

        private void AddNewRow()
        {
            AddRow(IsNew: true);
            _lastIdentification.Focus();
        }

        private void AddRow(bool IsNew, CatchLineClass CatchLine = null)
        {
            int x = 3;

            Label labelRow = new Label();
            TextBox textIdentificationType = new TextBox();
            TextBox textName1 = new TextBox();
            TextBox textName2 = new TextBox();
            TextBox textWt = new TextBox();
            TextBox textCount = new TextBox();
            TextBox textSubWt = new TextBox();
            TextBox textSubCount = new TextBox();
            CheckBox chkFromTotal = new CheckBox();
            CheckBox chkLiveFish = new CheckBox();

            if (!_ComboBoxesSet)
            {
                _ComboBoxesSet = true;

                _comboIdentificationType.With(o =>
                {
                    o.Width = 120;
                    o.Name = "cboIdentificationType";
                    o.Location = new Point(0, 0);
                    o.Visible = false;
                    o.DropDownStyle = ComboBoxStyle.DropDownList;
                    foreach (var item in CatchComposition.IdentificationTypes())
                    {
                        o.Items.Add(item);
                    }
                    o.DisplayMember = "Value";
                    o.ValueMember = "Key";
                    o.AutoCompleteSource = AutoCompleteSource.ListItems;
                    o.Font = Font;
                    _ctlHeight = o.Height;
                });

                _comboGenus.With(o =>
                {
                    o.Width = 120;
                    o.Font = Font;
                    o.Name = "cboGenus";
                    o.Location = new Point(0, 0);
                    foreach (var item in names.GenusList)
                    {
                        o.Items.Add(item);
                    }
                    o.DropDownStyle = ComboBoxStyle.DropDown;
                    o.AutoCompleteSource = AutoCompleteSource.ListItems;
                    o.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    o.Visible = false;
                });

                _comboSpecies.With(o =>
                {
                    o.Width = 120;
                    o.Font = Font;
                    o.Name = "cboSpecies";
                    o.Location = new Point(0, 0);
                    o.DropDownStyle = ComboBoxStyle.DropDown;
                    //o.DataSource = new BindingSource(gmsDict, null);
                    o.DisplayMember = "Value";
                    o.ValueMember = "Key";
                    o.AutoCompleteSource = AutoCompleteSource.ListItems;
                    o.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    o.Visible = false;
                });

                _comboLocalName.With(o =>
                {
                    o.Width = 120;
                    o.Font = Font;
                    o.Name = "cboLocalName";
                    o.Location = new Point(0, 0);

                    foreach (var item in names.LocalNameListDict)
                    {
                        o.Items.Add(item);
                    }

                    o.DropDownStyle = ComboBoxStyle.DropDown;
                    o.DisplayMember = "Value";
                    o.ValueMember = "Key";
                    o.AutoCompleteSource = AutoCompleteSource.ListItems;
                    o.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    o.Visible = false;
                });

                panelUI.Controls.Add(_comboIdentificationType);
                panelUI.Controls.Add(_comboGenus);
                panelUI.Controls.Add(_comboSpecies);
                panelUI.Controls.Add(_comboLocalName);
            }
            if (IsNew)
                _CatchCompositionData.Add(_y, new CatchLineClass());
            else
                _CatchCompositionData.Add(_y, CatchLine);

            _CatchCompositionData[_y].SamplingGUID = _SamplingGuid;

            labelRow.With(o =>
            {
                o.Name = "labelRow";
                o.Location = new Point(x, _y);
                o.Text = _row.ToString();
                o.Font = Font;
                o.Width = 40;
            });

            textIdentificationType.With(o =>
            {
                _ctlWidth = o.Width = 120;
                o.Height = _ctlHeight;
                o.Location = new Point(labelRow.Left + labelRow.Width + _spacer, _y);
                o.Name = "txtIdentificationType";
                o.Font = Font;
                o.GotFocus += OnTextBoxGotFocus;
                o.Validating += OnTextBoxValidating; ;
                o.TextChanged += OnTextBoxChanged;
                o.KeyPress += OnTextBoxKeyPress;
                o.DoubleClick += OnTextBoxDoubleClick;

                if (!IsNew)
                {
                    CurrentIDType = CatchLine.NameType;
                }
                else
                {
                    var newGUID = Guid.NewGuid().ToString();
                    CurrentIDType = CatchComposition.Identification.Scientific;
                    _CatchCompositionData[o.Location.Y].dataStatus = global.fad3DataStatus.statusNew;
                    _CatchCompositionData[o.Location.Y].NameType = CurrentIDType;
                    _CatchCompositionData[o.Location.Y].CatchCompGUID = newGUID;
                }
                o.Text = CatchComposition.IdentificationTypeToString(CurrentIDType);
            });

            textName1.With(o =>
            {
                o.Width = (int)(_ctlWidth * 1.2);
                o.Height = _ctlHeight;
                o.Location = new Point(textIdentificationType.Left + textIdentificationType.Width + _spacer, _y);
                o.Name = "txtName1";
                o.Font = Font;
                o.GotFocus += OnTextBoxGotFocus;
                o.Validating += OnTextBoxValidating; ;
                o.TextChanged += OnTextBoxChanged;
                o.KeyPress += OnTextBoxKeyPress;
                o.DoubleClick += OnTextBoxDoubleClick;
                if (!IsNew)
                {
                    o.Text = CatchLine.Name1;
                }
            });

            textName2.With(o =>
            {
                o.Width = (int)(_ctlWidth * 1.2);
                o.Height = _ctlHeight;
                o.Location = new Point(textName1.Left + textName1.Width + _spacer, _y);
                o.Name = "txtName2";
                o.Font = Font;
                o.GotFocus += OnTextBoxGotFocus;
                o.Validating += OnTextBoxValidating; ;
                o.TextChanged += OnTextBoxChanged;
                o.KeyPress += OnTextBoxKeyPress;
                o.DoubleClick += OnTextBoxDoubleClick;
                if (!IsNew) o.Text = CatchLine.Name2;
            });

            textWt.With(o =>
            {
                o.Width = (int)(_ctlWidth * 0.4);
                o.Height = _ctlHeight;
                o.Location = new Point(textName2.Left + textName2.Width + _spacer, _y);
                o.Name = "txtWt";
                o.Font = Font;
                o.GotFocus += OnTextBoxGotFocus;
                o.Validating += OnTextBoxValidating; ;
                o.TextChanged += OnTextBoxChanged;
                if (!IsNew) o.Text = o.Text = CatchLine.CatchWeight.ToString();
            });

            textCount.With(o =>
            {
                o.Width = (int)(_ctlWidth * 0.4);
                o.Height = _ctlHeight;
                o.Location = new Point(textWt.Left + textWt.Width + _spacer, _y);
                o.Name = "txtCount";
                o.Font = Font;
                o.GotFocus += OnTextBoxGotFocus;
                o.Validating += OnTextBoxValidating; ;
                o.TextChanged += OnTextBoxChanged;
                if (!IsNew) o.Text = o.Text = CatchLine.CatchCount.ToString();
            });

            textSubWt.With(o =>
            {
                o.Width = (int)(_ctlWidth * 0.4);
                o.Height = _ctlHeight;
                o.Location = new Point(textCount.Left + textCount.Width + _spacer, _y);
                o.Name = "txtSubWt";
                o.Font = Font;
                o.GotFocus += OnTextBoxGotFocus;
                o.Validating += OnTextBoxValidating; ;
                o.TextChanged += OnTextBoxChanged;
                if (!IsNew) o.Text = CatchLine.CatchSubsampleWt.ToString();
            });

            textSubCount.With(o =>
            {
                o.Width = (int)(_ctlWidth * 0.4);
                o.Height = _ctlHeight;
                o.Location = new Point(textSubWt.Left + textSubWt.Width + _spacer, _y);
                o.Name = "txtSubCount";
                o.Font = Font;
                o.GotFocus += OnTextBoxGotFocus;
                o.Validating += OnTextBoxValidating; ;
                o.TextChanged += OnTextBoxChanged;
                if (!IsNew) o.Text = CatchLine.CatchSubsampleCount.ToString();
            });

            chkFromTotal.With(o =>
            {
                o.Location = new Point(textSubCount.Left + textSubCount.Width + _spacer * 2, _y);
                o.Width = labelCol9.Width;
                o.Name = "chkFromTotal";
                o.Font = Font;
                o.Text = "";
                o.CheckStateChanged += OnCheckBoxCheckStateChanged;
                if (!IsNew) o.Checked = CatchLine.FromTotalCatch;
            });

            chkLiveFish.With(o =>
            {
                o.Location = new Point(chkFromTotal.Left + chkFromTotal.Width + _spacer, _y);
                o.Width = labelCol9.Width;
                o.Name = "chkLiveFish";
                o.Font = Font;
                o.Text = "";
                o.CheckStateChanged += OnCheckBoxCheckStateChanged;
                if (!IsNew) o.Checked = CatchLine.LiveFish;
            });

            panelUI.Controls.Add(labelRow);
            panelUI.Controls.Add(textIdentificationType);
            panelUI.Controls.Add(textName1);
            panelUI.Controls.Add(textName2);
            panelUI.Controls.Add(textWt);
            panelUI.Controls.Add(textCount);
            panelUI.Controls.Add(textSubWt);
            panelUI.Controls.Add(textSubCount);
            panelUI.Controls.Add(chkFromTotal);
            panelUI.Controls.Add(chkLiveFish);

            if (_row == 1)
            {
                labelCol1.Left = labelRow.Left;
                labelCol2.Left = textIdentificationType.Left;
                labelCol3.Left = textName1.Left;
                labelCol4.Left = textName2.Left;
                labelCol5.Left = textWt.Left;
                labelCol6.Left = textCount.Left;
                labelCol7.Left = textSubWt.Left;
                labelCol8.Left = textSubCount.Left;
                labelCol9.Left = chkFromTotal.Left;
                labelCol10.Left = chkLiveFish.Left;
                labelSubSample.Left = labelCol7.Left;
                labelSubSample.Width = (labelCol8.Left + labelCol8.Width) - labelCol7.Left;
            }

            if (_IsNew || _row >= CatchComposition.CatchCompositionRows)
            {
                _lastIdentification = textIdentificationType;
                _lastName1 = textName1;
                _lastName2 = textName2;
                _lastWeight = textWt;
                _lastCount = textCount;
                _lastSubWeight = textSubWt;
                _lastSubCount = textSubCount;
            }

            _y += labelRow.Height + _spacer;
            _row++;
        }

        private void cboEditor_KeyDown(object sender, KeyEventArgs e)
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

        private void cboEditor_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void cboEditor_LostFocus(object sender, EventArgs e)
        {
            if (_cboEditor.Name == "cboIdentificationType")
            {
                CurrentIDType = CatchComposition.StringToIdentificationType(_cboEditor.Text);
                _CatchCompositionData[_cboEditor.Location.Y].NameType = CurrentIDType;
                ResetRowColor(_cboEditor);
            }

            var ev = new CancelEventArgs();
            Logger.Log("cboEditor_LostFocus");
            cboEditor_Validating(_cboEditor, ev);
            if (ev.Cancel == false)
            {
                _currentTextBox.Text = _cboEditor.Text;
            }
        }

        private void cboEditor_Validating(object sender, CancelEventArgs e)
        {
            ((ComboBox)sender).With(o =>
            {
                var s = o.Text;
                if (s.Length > 0)
                {
                    switch (o.Name)
                    {
                        case "cboIdentificationType":
                            break;

                        case "cboGenus":
                            if (o.Text != _currentGenus)
                            {
                                FillSpeciesComboBox(o.Text);
                                _currentGenus = o.Text;
                            }
                            break;

                        case "cboSpecies":
                        case "cboLocalName":
                            _CatchCompositionData[o.Location.Y].CatchNameGUID = ((KeyValuePair<string, string>)o.SelectedItem).Key;
                            _CatchCompositionData[o.Location.Y].Name1 = GetTextBoxAtRow(o.Location.Y, "txtName1").Text;
                            _CatchCompositionData[o.Location.Y].Name2 = GetTextBoxAtRow(o.Location.Y, "txtName2").Text;
                            break;
                    }
                }
            });
            e.Cancel = false;
        }

        private void ControlToFocus(TextBox txt, char key = ' ')
        {
            _currentTextBox = txt;
            bool Proceed = true;
            //rowIDType(txt);
            _cboEditor = null;
            var s = txt.Text;
            switch (txt.Name)
            {
                case "txtIdentificationType":
                    _cboEditor = _comboIdentificationType;
                    break;

                case "txtName1":
                case "txtName2":
                    if (CurrentIDType == CatchComposition.Identification.LocalName)
                    {
                        if (txt.Name == "txtName1")
                            _cboEditor = _comboLocalName;
                        else
                        {
                            Proceed = false;
                        }
                    }
                    else
                    {
                        if (txt.Name == "txtName1")
                            _cboEditor = _comboGenus;
                        else
                            _cboEditor = _comboSpecies;
                    }
                    break;

                case "txtLocalName":
                    break;
            }

            if (Proceed)
            {
                _cboEditor.Bounds = txt.Bounds;
                _cboEditor.BringToFront();
                _cboEditor.Show();
                _cboEditor.Focus();

                if (s.Length > 0)
                {
                    _cboEditor.Text = s;
                    _cboEditor.SelectionStart = 0;
                    _cboEditor.SelectionLength = s.Length;
                }

                if (key > 32)
                {
                    _cboEditor.Text = key.ToString();
                    _cboEditor.SelectionStart = 1;
                }

                SetEditorEvents();
            }
        }

        /// <summary>
        /// fills up the species combo box depending on the selected genus
        /// </summary>
        private void FillSpeciesComboBox(string Genus)
        {
            names.Genus = Genus;
            _comboSpecies.Items.Clear();
            foreach (var item in names.speciesList)
            {
                _comboSpecies.Items.Add(item);
            }
            Logger.Log($"*** using {Genus} filled up species combo box***");
        }

        private CheckBox GetCheckBoxAtRow(Label row, string checkBoxName)
        {
            var chk = new CheckBox();
            foreach (Control c in panelUI.Controls)
            {
                if (c.Name == checkBoxName && c.Location.Y == row.Location.Y)
                {
                    chk = (CheckBox)c;
                    break;
                }
            }
            return chk;
        }

        private TextBox GetName1TextBox(ComboBox sourceComboBox)
        {
            var Name1TextBox = new TextBox();
            foreach (Control c in panelUI.Controls)
            {
                if (c.Name == "txtName1" && c.Location.Y == sourceComboBox.Location.Y)
                {
                    Name1TextBox = (TextBox)c;
                    break;
                }
            }
            return Name1TextBox;
        }

        private CheckBox getNextCheckBox(Control source)
        {
            var chk = new CheckBox();
            var Proceed = false;
            var NextCheckBox = "";
            switch (source.Name)
            {
                case "txtSubCount":
                    NextCheckBox = "chkFromTotal";
                    Proceed = true;
                    break;

                case "chkFromTotal":
                    NextCheckBox = "chkLiveFish";
                    Proceed = true;
                    break;
            }

            if (Proceed)
            {
                foreach (Control c in panelUI.Controls)
                {
                    if (c.Name == NextCheckBox && c.Location.Y == source.Location.Y)
                    {
                        chk = (CheckBox)c;
                        break;
                    }
                }
            }

            return chk;
        }

        private TextBox GetTextBox(ComboBox fromComboBox = null, TextBox fromTextBox = null, bool GetNext = false)
        {
            var myTextBox = new TextBox();
            var myTextBoxName = "";
            var sourceYPosition = 0;

            if (fromComboBox != null)
            {
                sourceYPosition = fromComboBox.Location.Y;
                switch (_cboEditor.Name)
                {
                    case "cboIdentificationType":
                        myTextBoxName = "txtIdentificationType";
                        if (GetNext) myTextBoxName = "txtName1";
                        break;

                    case "cboGenus":

                        myTextBoxName = "txtName1";
                        if (GetNext) myTextBoxName = "txtName2";
                        break;

                    case "cboSpecies":
                    case "cboLocalName":
                        myTextBoxName = "txtName2";
                        if (GetNext) myTextBoxName = "txtWt";
                        break;
                }
            }

            if (fromTextBox != null)
            {
                sourceYPosition = fromTextBox.Location.Y;
                myTextBoxName = fromTextBox.Name;

                switch (myTextBoxName)
                {
                    case "txtName2":
                        if (GetNext)
                        {
                            myTextBoxName = "txtWt";
                        }
                        break;

                    case "txtWt":
                        if (GetNext)
                        {
                            myTextBoxName = "txtCount";
                        }
                        break;

                    case "txtCount":
                        if (GetNext)
                        {
                            myTextBoxName = "txtSubWt";
                        }
                        break;

                    case "txtSubWt":
                        if (GetNext)
                        {
                            myTextBoxName = "txtSubCount";
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

        private TextBox GetTextBoxAtRow(int LocationY, string textBoxName)
        {
            var txt = new TextBox();
            foreach (Control c in panelUI.Controls)
            {
                if (c.Name == textBoxName && c.Location.Y == LocationY)
                {
                    txt = (TextBox)c;
                    break;
                }
            }
            return txt;
        }

        private void HideCBOs()
        {
            _comboGenus.Hide();
            _comboIdentificationType.Hide();
            _comboSpecies.Hide();
            _comboLocalName.Hide();
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonOK":
                    if (ValidateForm())
                    {
                        if (CatchComposition.UpdateCatchComposition(_CatchCompositionData))
                        {
                            Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please check for required fields", "Validation error",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

                case "buttonCancel":
                    Close();
                    break;

                case "buttonAdd":
                    var ShowMissing = false;
                    SetIDType(_lastIdentification);
                    _lastIdentification.BackColor = SystemColors.Window;
                    _lastName1.BackColor = SystemColors.Window;
                    _lastName2.BackColor = SystemColors.Window;
                    _lastWeight.BackColor = SystemColors.Window;

                    if (CurrentIDType == CatchComposition.Identification.Scientific)
                    {
                        if (_lastIdentification.Text.Length == 0 || _lastName1.Text.Length == 0
                            || _lastName2.Text.Length == 0 || _lastWeight.Text.Length == 0)
                        {
                            ShowMissing = true;
                        }
                        else
                        {
                            AddNewRow();
                        }
                    }
                    else
                    {
                        if (_lastIdentification.Text.Length == 0 || _lastName1.Text.Length == 0
                            || _lastWeight.Text.Length == 0)
                        {
                            ShowMissing = true;
                        }
                        else
                        {
                            AddNewRow();
                        }
                    }

                    if (ShowMissing)
                    {
                        _lastIdentification.BackColor = global.MissingFieldBackColor;
                        _lastName1.BackColor = global.MissingFieldBackColor;
                        _lastWeight.BackColor = global.MissingFieldBackColor;
                        if (CurrentIDType == CatchComposition.Identification.Scientific)
                            _lastName2.BackColor = global.MissingFieldBackColor;

                        if (_lastIdentification.Text.Length == 0)
                        {
                            _lastIdentification.Focus();
                        }
                        else if (_lastName1.Text.Length == 0)
                        {
                            _lastName1.Focus();
                        }
                        else if (CurrentIDType == CatchComposition.Identification.Scientific && _lastName2.Text.Length == 0)
                        {
                            _lastName2.Focus();
                        }
                        else
                        {
                            _lastWeight.Focus();
                        }

                        MessageBox.Show("The required fields should be filled up", "Validation error",
                                         MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    break;

                case "buttonRemove":
                    break;
            }
        }

        private void OnCheckBoxCheckStateChanged(object sender, EventArgs e)
        {
            ((CheckBox)sender).With(o =>
            {
                SetRowStatusToEdited(o);

                if (o.Name == "chkFromTotal")
                    _CatchCompositionData[o.Location.Y].FromTotalCatch = o.Checked;
                else
                    _CatchCompositionData[o.Location.Y].LiveFish = o.Checked;
            });
        }

        private void OnForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return && e.Shift)
            {
                var ev = new EventArgs();
                OnButtonClick(buttonAdd, ev);
            }
            else if (e.KeyCode == Keys.Return)
            {
                if (this.ActiveControl.GetType().Name == "TextBox"
                    || this.ActiveControl.GetType().Name == "CheckBox")
                {
                    switch (this.ActiveControl.Name)
                    {
                        case "txtIdentificationType":
                        case "txtName1":
                            GetTextBox(fromTextBox: (TextBox)this.ActiveControl, GetNext: true).Focus();
                            break;

                        case "txtName2":
                            if (CurrentIDType == CatchComposition.Identification.LocalName)
                                GetTextBox(fromTextBox: (TextBox)this.ActiveControl, GetNext: true).Focus();
                            break;

                        case "txtWt":
                        case "txtCount":
                        case "txtSubWt":
                            GetTextBox(fromTextBox: (TextBox)this.ActiveControl, GetNext: true).Focus();
                            break;

                        case "txtSubCount":
                        case "chkFromTotal":
                            getNextCheckBox(this.ActiveControl).Focus();
                            break;

                        case "chkLiveFish":
                            buttonAdd.Focus();
                            break;
                    }
                }
            }
        }

        private void OnForm_Load(object sender, EventArgs e)
        {
            labelTitle.Text = $"Catch composition of {_ReferenceNumber}";
            if (_IsNew)
            {
                labelTitle.Text = $"New catch composition of {_ReferenceNumber}";
                AddNewRow();
            }
            else
            {
                foreach (var item in CatchComposition.CatchComp(_SamplingGuid))
                {
                    AddRow(IsNew: false, item.Value);
                }
            }
        }

        private void OnTextBoxChanged(object sender, EventArgs e)
        {
            ((TextBox)sender).With(o =>
            {
                SetRowStatusToEdited(o);

                if (o.Name == "txtIdentificationType")
                    ResetRowColor(o);
                else
                    o.BackColor = SystemColors.Window;
            });
        }

        private void OnTextBoxDoubleClick(object sender, EventArgs e)
        {
            ControlToFocus((TextBox)sender);
        }

        private void OnTextBoxGotFocus(object sender, EventArgs e)
        {
            ((TextBox)sender).With(o =>
            {
                SetIDType(o);
                if (o.Name == "txtName2" && _CurrentIDType == CatchComposition.Identification.Scientific)
                {
                    _comboSpecies.Items.Clear();
                    if (_CatchCompositionData[o.Location.Y].Name1 != _currentGenus)
                    {
                        names.Genus = _CatchCompositionData[o.Location.Y].Name1;
                        foreach (var item in names.speciesList)
                        {
                            _comboSpecies.Items.Add(item);
                        }
                    }
                }
            });
            HideCBOs();
        }

        private void OnTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            ControlToFocus((TextBox)sender, e.KeyChar);
        }

        private void OnTextBoxValidating(object sender, CancelEventArgs e)
        {
            var msg = "";
            ((TextBox)sender).With(o =>
            {
                var s = o.Text;

                if (s.Length == 0)
                {
                    _CatchCompositionData[o.Location.Y].CatchSubsampleWt = null;
                    _CatchCompositionData[o.Location.Y].CatchCount = null;
                    _CatchCompositionData[o.Location.Y].CatchSubsampleCount = null;
                }
                else if (s.Length > 0)
                {
                    switch (o.Name)
                    {
                        case "txtIdentificationType":
                            break;

                        case "txtName1":
                            break;

                        case "txtName2":
                            if (CurrentIDType == CatchComposition.Identification.LocalName && o.Text.Length > 0)
                            {
                                msg = "This field should be blank";
                            }

                            break;

                        case "txtWt":
                        case "txtSubWt":
                            if (double.TryParse(s, out double myWeight))
                            {
                                if (myWeight > 0)
                                {
                                    if (o.Name == "txtWt")
                                        _CatchCompositionData[o.Location.Y].CatchWeight = myWeight;
                                    else
                                    {
                                        _CatchCompositionData[o.Location.Y].CatchSubsampleWt = myWeight;
                                    }
                                }
                                else
                                {
                                    msg = "Expected weight is a number greater than zero";
                                }
                            }
                            else
                            {
                                msg = "Expected weight is a number greater than zero";
                            }
                            break;

                        case "txtCount":
                        case "txtSubCount":
                            if (int.TryParse(s, out int myCount))
                            {
                                if (myCount > 0)
                                {
                                    if (myCount.ToString() != s)
                                    {
                                        msg = "Expected count is a whole number greater than zero";
                                    }
                                    else
                                    {
                                        if (o.Name == "txtCount")
                                        {
                                            _CatchCompositionData[o.Location.Y].CatchCount = myCount;
                                        }
                                        else
                                        {
                                            _CatchCompositionData[o.Location.Y].CatchSubsampleCount = myCount;
                                        }
                                    }
                                }
                                else
                                {
                                    msg = "Expected count is a whole number greater than zero";
                                }
                            }
                            else
                            {
                                msg = "Expected count is a whole number greater than zero";
                            }
                            break;
                    }
                }
            });

            if (msg.Length > 0)
            {
                e.Cancel = true;
                MessageBox.Show(msg, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// dynamically adds field controls for encoding multiple catch names with catch weight and count
        /// </summary>
        /// <param name="IsNew"></param>
        /// <param name="CatchLine"></param>
        private void ResetRowColor(Control sourceLocation)
        {
            int n = 0;
            foreach (Control c in panelUI.Controls)
            {
                if ((c.Name == "txtIdentificationType"
                    || c.Name == "txtName1"
                    || c.Name == "txtName2"
                    || c.Name == "txtWt")
                    && c.Location.Y == sourceLocation.Location.Y)
                {
                    c.BackColor = SystemColors.Window;
                    n++;
                    if (n == 4) break;
                }
            }
        }

        private bool RowHasRequired(TextBox source)
        {
            var hasRequired = false;
            var requiredCount = 0;
            foreach (Control c in panelUI.Controls)
            {
                if (c.GetType().Name == "TextBox" && c.Location.Y == source.Location.Y)
                {
                    if (CurrentIDType == CatchComposition.Identification.Scientific)
                    {
                        if ((c.Name == "txtIdentificationType" || c.Name == "txtName1"
                            || c.Name == "txtName2" || c.Name == "txtWt")
                            && c.Text.Length > 0)
                        {
                            ++requiredCount;
                            if (requiredCount == 4)
                            {
                                hasRequired = true;
                                break;
                            }
                        }
                        else
                        {
                            if (c.Name == "txtIdentificationType" || c.Name == "txtName1"
                            || c.Name == "txtName2" || c.Name == "txtWt")
                                c.BackColor = global.MissingFieldBackColor;
                            //if (requiredCount < 4) break;
                        }
                    }
                    else
                    {
                        if ((c.Name == "txtIdentificationType" || c.Name == "txtName1"
                             || c.Name == "txtWt") && c.Text.Length > 0)
                        {
                            ++requiredCount;
                            if (requiredCount == 3)
                            {
                                hasRequired = true;
                                break;
                            }
                        }
                        else
                        {
                            if (c.Name == "txtIdentificationType" || c.Name == "txtName1" || c.Name == "txtWt")
                                c.BackColor = global.MissingFieldBackColor;
                        }
                    }
                }
            }
            return hasRequired;
        }

        private void SetEditorEvents()
        {
            _cboEditor.KeyDown += cboEditor_KeyDown;
            _cboEditor.KeyPress += cboEditor_KeyPress;
            _cboEditor.LostFocus += cboEditor_LostFocus;
            _cboEditor.Validating += cboEditor_Validating;
        }

        private void SetIDType(TextBox source)
        {
            CurrentIDType = _CatchCompositionData[source.Location.Y].NameType;
        }

        private void SetRowStatusToEdited(Control source)
        {
            if (_CatchCompositionData[source.Location.Y].dataStatus != global.fad3DataStatus.statusNew)
            {
                _CatchCompositionData[source.Location.Y].dataStatus = global.fad3DataStatus.statusEdited;
            }
        }

        private bool ValidateForm()
        {
            var IsValidated = true;
            foreach (Control c in panelUI.Controls)
            {
                if (c.Name == "txtIdentificationType")
                {
                    SetIDType((TextBox)c);
                    if (!RowHasRequired((TextBox)c))
                    {
                        IsValidated = false;
                        break;
                    }
                    //break;
                }
            }
            return IsValidated;
        }
    }
}
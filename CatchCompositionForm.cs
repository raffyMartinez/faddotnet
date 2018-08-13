using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FAD3
{
    public partial class CatchCompositionForm : Form
    {
        private MainForm _parentForm;
        private string _SamplingGuid;
        private bool _IsNew;
        private ComboBox _comboIdentificationType = new ComboBox();
        private ComboBox _comboGenus = new ComboBox();
        private ComboBox _comboSpecies = new ComboBox();
        private ComboBox _comboLocalName = new ComboBox();
        private int _row = 1;
        private int _y = 0;
        private int _ctlHeight;
        private int _ctlWidth = 0;
        private int _spacer = 3;
        private string _ReferenceNumber;
        private bool _ComboBoxesSet = false;

        private TextBox _lastIdentification;
        private TextBox _lastName1;
        private TextBox _lastName2;
        private TextBox _lastWeight;
        private TextBox _lastCount;
        private TextBox _lastSubWeight;
        private TextBox _lastSubCount;

        private TextBox _currentTextBox;
        private ComboBox _cboEditor = new ComboBox();
        private CatchComposition.Identification _CurrentIDType;

        public CatchCompositionForm(bool IsNew, MainForm parent, string SamplingGuid, string ReferenceNumber)
        {
            InitializeComponent();
            _parentForm = parent;
            _SamplingGuid = SamplingGuid;
            _IsNew = IsNew;
            _ReferenceNumber = ReferenceNumber;
        }

        private void SetEditorEvents()
        {
            _cboEditor.KeyDown += cboEditor_KeyDown;
            _cboEditor.KeyPress += cboEditor_KeyPress;
            _cboEditor.LostFocus += cboEditor_LostFocus;
            _cboEditor.Validating += cboEditor_Validating;
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
                            if (o.Text == "Local name")
                                _CurrentIDType = CatchComposition.Identification.LocalName;
                            else
                                _CurrentIDType = CatchComposition.Identification.Scientific;
                            break;

                        case "cboGenus":
                            break;

                        case "cboSpecies":
                            break;

                        case "cboLocalName":
                            break;
                    }
                }
            });
            e.Cancel = false;
        }

        private void cboEditor_LostFocus(object sender, EventArgs e)
        {
            var ev = new CancelEventArgs();
            cboEditor_Validating(_cboEditor, ev);
            if (ev.Cancel == false)
            {
                _currentTextBox.Text = _cboEditor.Text;
                GetTextBox(_cboEditor, GetNext: true).Focus();
            }
        }

        private void cboEditor_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private bool ValidateForm()
        {
            return true;
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonOK":
                    if (ValidateForm())
                    {
                        if (CatchComposition.UpdateCatchComposition())
                        {
                            Close();
                        }
                    }
                    break;

                case "buttonCancel":
                    Close();
                    break;

                case "buttonAdd":
                    rowIDType(_lastName1);
                    if (_lastIdentification.Text.Length == 0
                        || _lastName1.Text.Length == 0
                        || _CurrentIDType == CatchComposition.Identification.Scientific && _lastName2.Text.Length == 0
                        || _lastWeight.Text.Length == 0)
                    {
                        _lastIdentification.BackColor = global.MissingFieldBackColor;
                        _lastName1.BackColor = global.MissingFieldBackColor;
                        _lastName2.BackColor = global.MissingFieldBackColor;
                        _lastWeight.BackColor = global.MissingFieldBackColor;

                        MessageBox.Show("Please fill up the the data fields", "Validation error",
                                         MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        AddRow(IsNew: true);
                        _lastIdentification.Focus();
                    }
                    break;

                case "buttonRemove":
                    break;
            }
        }

        private void AddRow(bool IsNew, CatchComposition.Identification IDType = CatchComposition.Identification.Scientific,
                            string NameGuid = "", string Name1 = "", string Name2 = "", double catchWeight = 0, long? catchCount = null,
                            double? subWeight = null, long? subCount = null, bool FromTotal = false, bool liveFish = false)
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
                o.Text = CatchComposition.IdentificationTypeToString(IDType);
                o.GotFocus += OnTextBoxGotFocus;
                o.Validating += OnTextBoxValidating; ;
                o.TextChanged += OnTextBoxChanged;
                o.KeyPress += OnTextBoxKeyPress;
                o.DoubleClick += OnTextBoxDoubleClick;
            });

            textName1.With(o =>
            {
                o.Width = (int)(_ctlWidth * 1.2);
                o.Height = _ctlHeight;
                o.Location = new Point(textIdentificationType.Left + textIdentificationType.Width + _spacer, _y);
                o.Name = "txtName1";
                o.Font = Font;
                if (!IsNew) o.Text = Name1;
                o.GotFocus += OnTextBoxGotFocus;
                o.Validating += OnTextBoxValidating; ;
                o.TextChanged += OnTextBoxChanged;
                o.KeyPress += OnTextBoxKeyPress;
                o.DoubleClick += OnTextBoxDoubleClick;
            });

            textName2.With(o =>
            {
                o.Width = (int)(_ctlWidth * 1.2);
                o.Height = _ctlHeight;
                o.Location = new Point(textName1.Left + textName1.Width + _spacer, _y);
                o.Name = "txtName2";
                o.Font = Font;
                if (!IsNew) o.Text = Name2;
                o.GotFocus += OnTextBoxGotFocus;
                o.Validating += OnTextBoxValidating; ;
                o.TextChanged += OnTextBoxChanged;
                o.KeyPress += OnTextBoxKeyPress;
                o.DoubleClick += OnTextBoxDoubleClick;
            });

            textWt.With(o =>
            {
                o.Width = (int)(_ctlWidth * 0.4);
                o.Height = _ctlHeight;
                o.Location = new Point(textName2.Left + textName2.Width + _spacer, _y);
                o.Name = "txtWt";
                o.Font = Font;
                if (!IsNew) o.Text = catchWeight.ToString();
                o.GotFocus += OnTextBoxGotFocus;
                o.Validating += OnTextBoxValidating; ;
                o.TextChanged += OnTextBoxChanged;
            });

            textCount.With(o =>
            {
                o.Width = (int)(_ctlWidth * 0.4);
                o.Height = _ctlHeight;
                o.Location = new Point(textWt.Left + textWt.Width + _spacer, _y);
                o.Name = "txtCount";
                o.Font = Font;
                if (!IsNew) o.Text = catchCount.ToString();
                o.GotFocus += OnTextBoxGotFocus;
                o.Validating += OnTextBoxValidating; ;
                o.TextChanged += OnTextBoxChanged;
            });

            textSubWt.With(o =>
            {
                o.Width = (int)(_ctlWidth * 0.4);
                o.Height = _ctlHeight;
                o.Location = new Point(textCount.Left + textCount.Width + _spacer, _y);
                o.Name = "txtSubWt";
                o.Font = Font;
                if (!IsNew) o.Text = subWeight.ToString();
                o.GotFocus += OnTextBoxGotFocus;
                o.Validating += OnTextBoxValidating; ;
                o.TextChanged += OnTextBoxChanged;
            });

            textSubCount.With(o =>
            {
                o.Width = (int)(_ctlWidth * 0.4);
                o.Height = _ctlHeight;
                o.Location = new Point(textSubWt.Left + textSubWt.Width + _spacer, _y);
                o.Name = "txtSubCount";
                o.Font = Font;
                if (!IsNew) o.Text = subCount.ToString();
                o.GotFocus += OnTextBoxGotFocus;
                o.Validating += OnTextBoxValidating; ;
                o.TextChanged += OnTextBoxChanged;
            });

            chkFromTotal.With(o =>
            {
                o.Location = new Point(textSubCount.Left + textSubCount.Width + _spacer * 2, _y);
                o.Width = labelCol9.Width;
                o.Name = "chkFromTotal";
                o.Font = Font;
                o.Text = "";
                if (!IsNew) o.Checked = FromTotal;
            });

            chkLiveFish.With(o =>
            {
                o.Location = new Point(chkFromTotal.Left + chkFromTotal.Width + _spacer, _y);
                o.Width = labelCol9.Width;
                o.Name = "chkLiveFish";
                o.Font = Font;
                o.Text = "";
                if (!IsNew) o.Checked = liveFish;
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

        private void OnTextBoxChanged(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = SystemColors.Window;
        }

        private void rowIDType(TextBox ctl)
        {
            var myIDType = CatchComposition.Identification.Scientific;
            foreach (Control c in panelUI.Controls)
            {
                if (c.Name == "txtIdentificationType" && c.Location.Y == ctl.Location.Y)
                {
                    myIDType = CatchComposition.StringToIdentificationType(c.Text);
                }
            }
            _CurrentIDType = myIDType;
        }

        private void HideCBOs()
        {
            _comboGenus.Hide();
            _comboIdentificationType.Hide();
            _comboSpecies.Hide();
            _comboLocalName.Hide();
        }

        private void ControlToFocus(TextBox txt, char key = ' ')
        {
            _currentTextBox = txt;
            bool Proceed = true;
            rowIDType(txt);
            _cboEditor = null;
            var s = txt.Text;
            switch (txt.Name)
            {
                case "txtIdentificationType":
                    _cboEditor = _comboIdentificationType;
                    break;

                case "txtName1":
                case "txtName2":
                    if (_CurrentIDType == CatchComposition.Identification.LocalName)
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

        private void OnTextBoxGotFocus(object sender, EventArgs e)
        {
            HideCBOs();
        }

        private void OnTextBoxValidating(object sender, CancelEventArgs e)
        {
        }

        private void OnForm_Load(object sender, EventArgs e)
        {
            labelTitle.Text = $"Catch composition of {_ReferenceNumber}";
            if (_IsNew)
            {
                labelTitle.Text = $"New catch composition of {_ReferenceNumber}";
                AddRow(IsNew: true);
            }
            else
            {
                foreach (var item in CatchComposition.CatchComp(_SamplingGuid))
                {
                    AddRow(IsNew: false, item.Value.NameType, item.Value.CatchNameGUID,
                           item.Value.Name1, item.Value.Name2, item.Value.CatchWeight,
                           item.Value.CatchCount, item.Value.CatchSubsampleWt, item.Value.CatchSubsampleCount,
                           item.Value.FromTotalCatch, item.Value.LiveFish);
                }
            }
        }

        private void OnTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            ControlToFocus((TextBox)sender, e.KeyChar);
        }

        private void OnTextBoxDoubleClick(object sender, EventArgs e)
        {
            ControlToFocus((TextBox)sender);
        }

        private TextBox GetTextBox(ComboBox source = null, TextBox fromTextBox = null, bool GetNext = false)
        {
            var myTextBox = new TextBox();
            var myTextBoxName = "";
            var sourceYPosition = 0;

            if (source != null)
            {
                sourceYPosition = source.Location.Y;
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
                switch (fromTextBox.Name)
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

        private void OnForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return && e.Shift)
            {
                var ev = new EventArgs();
                OnButtonClick(buttonAdd, ev);
            }
            else if (e.KeyCode == Keys.Return)
            {
                if (this.ActiveControl.GetType().Name == "TextBox")
                {
                    switch (this.ActiveControl.Name)
                    {
                        case "txtName2":
                            if (_CurrentIDType == CatchComposition.Identification.LocalName)
                                GetTextBox(fromTextBox: (TextBox)this.ActiveControl, GetNext: true).Focus();
                            break;

                        case "txtWt":
                        case "txtCount":
                        case "txtSubWt":
                        case "txtSubCount":
                            GetTextBox(fromTextBox: (TextBox)this.ActiveControl, GetNext: true).Focus();
                            break;
                    }
                }
            }
        }
    }
}
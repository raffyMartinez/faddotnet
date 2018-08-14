using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

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
        private string _currentGenus = "";

        private CatchComposition.Identification CurrentIDType
        {
            get { return _CurrentIDType; }
            set { _CurrentIDType = value; }
        }

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
                            //set the tag of the Name1 textbox of the current row to the name guid of current catch
                            GetName1TextBox(o).Tag = ((KeyValuePair<string, string>)o.SelectedItem).Key;
                            break;
                    }
                }
            });
            e.Cancel = false;
        }

        private void cboEditor_LostFocus(object sender, EventArgs e)
        {
            if (_cboEditor.Name == "cboIdentificationType")
            {
                CurrentIDType = CatchComposition.StringToIdentificationType(_cboEditor.Text);
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

        private void cboEditor_KeyPress(object sender, KeyPressEventArgs e)
        {
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
                            //if (c.Name != "txtName2")
                            //{
                            //    c.BackColor = global.MissingFieldBackColor;
                            //    if (requiredCount != 3) break;
                            //}
                        }
                    }
                }
            }
            return hasRequired;
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

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonOK":
                    if (ValidateForm())
                    {
                        var catchComposition = new Dictionary<string, (global.fad3DataStatus DataStatus, string RowGuid,
                            string NameGuid, CatchComposition.Identification IDType,
                            string SamplingGuid, double CatchWeight, int? CatchCount,
                            double? SubWeight, double? SubCount, bool FromTotal,
                            bool LiveFish)>();
                        var RowLabel = new Label();
                        foreach (Control c in panelUI.Controls)
                        {
                            var DataStatus = global.fad3DataStatus.statusFromDB;
                            var RowGuid = "";
                            var NameGuid = "";
                            var CatchWt = 0D;
                            int? CatchCount = null;
                            double? SubWeight = null;
                            int? SubCount = null;
                            bool LiveFish = false;
                            bool FromTotal = false;

                            if (c.Name == "labelRow")
                            {
                                RowLabel = (Label)c;
                                DataStatus = (global.fad3DataStatus)Enum.Parse(typeof(global.fad3DataStatus), RowLabel.Tag.ToString());
                            }

                            if (DataStatus != global.fad3DataStatus.statusFromDB)
                            {
                                var myIDText = GetTextBoxAtRow(RowLabel, "txtIdentificationType");
                                RowGuid = myIDText.Tag.ToString();
                                CurrentIDType = CatchComposition.StringToIdentificationType(myIDText.Text);
                                NameGuid = GetTextBoxAtRow(RowLabel, "txtName1").Tag.ToString();
                                CatchWt = double.Parse(GetTextBoxAtRow(RowLabel, "txtWt").Text);

                                if (int.TryParse(GetTextBoxAtRow(RowLabel, "txtCount").Text, out int myCount))
                                    CatchCount = myCount;

                                if (double.TryParse(GetTextBoxAtRow(RowLabel, "txtSubWt").Text, out double mySubWt))
                                    SubWeight = mySubWt;

                                if (int.TryParse(GetTextBoxAtRow(RowLabel, "txtSubCount").Text, out int mySubCount))
                                    SubCount = mySubCount;

                                FromTotal = GetCheckBoxAtRow(RowLabel, "chkFromTotal").Checked;
                                LiveFish = GetCheckBoxAtRow(RowLabel, "chkLiveFish").Checked;

                                catchComposition.Add(RowGuid, (DataStatus, RowGuid, NameGuid, _CurrentIDType, _SamplingGuid, CatchWt, CatchCount, SubWeight, SubCount, FromTotal, LiveFish));
                            }
                        }

                        if (CatchComposition.UpdateCatchComposition(catchComposition))
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

        private global.fad3DataStatus GetRowEditStatus(Control source)
        {
            var EditStatus = global.fad3DataStatus.statusFromDB;
            foreach (Control c in panelUI.Controls)
            {
                if (c.Name == "labelRow" && c.Location.Y == source.Location.Y)
                {
                    EditStatus = (global.fad3DataStatus)Enum.Parse(typeof(global.fad3DataStatus), c.Tag.ToString());
                    break;
                }
            }

            return EditStatus;
        }

        /// <summary>
        /// dynamically adds field controls for encoding multiple catch names with catch weight and count
        /// </summary>
        /// <param name="IsNew"></param>
        /// <param name="IDType"></param>
        /// <param name="NameGuid"></param>
        /// <param name="Name1"></param>
        /// <param name="Name2"></param>
        /// <param name="catchWeight"></param>
        /// <param name="catchCount"></param>
        /// <param name="subWeight"></param>
        /// <param name="subCount"></param>
        /// <param name="FromTotal"></param>
        /// <param name="liveFish"></param>
        private void AddRow(bool IsNew, CatchComposition.Identification IDType = CatchComposition.Identification.Scientific,
                            string NameGuid = "", string Name1 = "", string Name2 = "", double catchWeight = 0, long? catchCount = null,
                            double? subWeight = null, long? subCount = null, bool FromTotal = false, bool liveFish = false, string CatchCompositionRowGuid = "")
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

            labelRow.With(o =>
            {
                o.Name = "labelRow";
                o.Location = new Point(x, _y);
                o.Text = _row.ToString();
                o.Font = Font;
                o.Width = 40;
                o.Tag = global.fad3DataStatus.statusFromDB;
                if (IsNew)
                {
                    o.Tag = global.fad3DataStatus.statusNew;
                }
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
                CurrentIDType = IDType;
                o.Text = CatchComposition.IdentificationTypeToString(CurrentIDType);
                if (!IsNew)
                {
                    o.Tag = CatchCompositionRowGuid;
                }
                else
                {
                    o.Tag = Guid.NewGuid().ToString();
                }
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
                    o.Text = Name1;
                    o.Tag = NameGuid;
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
                if (!IsNew) o.Text = Name2;
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
                if (!IsNew) o.Text = catchWeight.ToString();
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
                if (!IsNew) o.Text = catchCount.ToString();
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
                if (!IsNew) o.Text = subWeight.ToString();
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
                if (!IsNew) o.Text = subCount.ToString();
            });

            chkFromTotal.With(o =>
            {
                o.Location = new Point(textSubCount.Left + textSubCount.Width + _spacer * 2, _y);
                o.Width = labelCol9.Width;
                o.Name = "chkFromTotal";
                o.Font = Font;
                o.Text = "";
                o.CheckStateChanged += OnCheckBoxCheckStateChanged;
                if (!IsNew) o.Checked = FromTotal;
            });

            chkLiveFish.With(o =>
            {
                o.Location = new Point(chkFromTotal.Left + chkFromTotal.Width + _spacer, _y);
                o.Width = labelCol9.Width;
                o.Name = "chkLiveFish";
                o.Font = Font;
                o.Text = "";
                o.CheckStateChanged += OnCheckBoxCheckStateChanged;
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

        private void OnCheckBoxCheckStateChanged(object sender, EventArgs e)
        {
            SetRowStatusToEdited((CheckBox)sender);
        }

        private void SetRowStatusToEdited(Control source)
        {
            foreach (Control c in panelUI.Controls)
            {
                if (c.Name == "labelRow" && c.Location.Y == source.Location.Y)
                {
                    var EditStatus = (global.fad3DataStatus)Enum.Parse(typeof(global.fad3DataStatus), c.Tag.ToString());
                    if (EditStatus != global.fad3DataStatus.statusNew)
                        c.Tag = global.fad3DataStatus.statusEdited;
                    break;
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

        //private void rowIDType(TextBox ctl)
        //{
        //    var myIDType = CatchComposition.Identification.Scientific;
        //    foreach (Control c in panelUI.Controls)
        //    {
        //        if (c.Name == "txtIdentificationType" && c.Location.Y == ctl.Location.Y)
        //        {
        //            myIDType = CatchComposition.StringToIdentificationType(c.Text);
        //        }
        //    }
        //    _CurrentIDType = myIDType;
        //}

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

        private void OnTextBoxGotFocus(object sender, EventArgs e)
        {
            SetIDType((TextBox)sender);
            HideCBOs();
        }

        private void SetIDType(TextBox source)
        {
            foreach (Control c in panelUI.Controls)
            {
                if (c.Name == "txtIdentificationType" && c.Location.Y == source.Location.Y)
                {
                    CurrentIDType = CatchComposition.StringToIdentificationType(c.Text);
                    break;
                }
            }
        }

        private void OnTextBoxValidating(object sender, CancelEventArgs e)
        {
            var msg = "";
            ((TextBox)sender).With(o =>
            {
                var s = o.Text;
                if (s.Length > 0)
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

        private void AddNewRow()
        {
            AddRow(IsNew: true);
            _lastIdentification.Focus();
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
                    AddRow(IsNew: false, item.Value.NameType, item.Value.CatchNameGUID,
                           item.Value.Name1, item.Value.Name2, item.Value.CatchWeight,
                           item.Value.CatchCount, item.Value.CatchSubsampleWt, item.Value.CatchSubsampleCount,
                           item.Value.FromTotalCatch, item.Value.LiveFish, item.Value.CatchDetailRowGUID);
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

        private TextBox GetTextBoxAtRow(Label row, string textBoxName)
        {
            var txt = new TextBox();
            foreach (Control c in panelUI.Controls)
            {
                if (c.Name == textBoxName && c.Location.Y == row.Location.Y)
                {
                    txt = (TextBox)c;
                    break;
                }
            }
            return txt;
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
    }
}
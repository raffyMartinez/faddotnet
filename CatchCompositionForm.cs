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
        private int _ScrollAmount = 0;
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

        /// <summary>
        /// Gets and sets the type of identification of a row
        /// </summary>
        private CatchComposition.Identification CurrentIDType
        {
            get { return _CurrentIDType; }
            set { _CurrentIDType = value; }
        }

        /// <summary>
        /// adds a new blank row of fields for catch composition
        /// </summary>
        private void AddNewRow()
        {
            AddRow(IsNew: true);
            _lastIdentification.Focus();
        }

        /// <summary>
        /// adds a row of fields with data from the database
        /// Controls are dynamically generated
        /// </summary>
        /// <param name="IsNew"></param>
        /// <param name="CatchLine - a set of rows of catch composition data from the database"></param>
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

            //adds the combo boxes only once to the panel
            if (!_ComboBoxesSet)
            {
                _ComboBoxesSet = true;

                //combo box to specify type of identification for a catch
                //values are scientific and local name
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
                    o.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    o.Font = Font;
                    _ctlHeight = o.Height;
                });

                //contains all the genus from the database
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

                //contains the species of a genus. Contents will vary depending on the current genus
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

                //contains all the local names from the database
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

                //adds the comboboxes to the panel
                panelUI.Controls.Add(_comboIdentificationType);
                panelUI.Controls.Add(_comboGenus);
                panelUI.Controls.Add(_comboSpecies);
                panelUI.Controls.Add(_comboLocalName);
            }

            //adds the catch composition data from the database to the form level catch composition dictionary
            //the key (_y) is the Y coordinate of each row.
            if (IsNew)
            {
                _CatchCompositionData.Add(_y, new CatchLineClass(_SamplingGuid));
                _CatchCompositionData[_y].Sequence = _row;
            }
            else
                _CatchCompositionData.Add(_y, CatchLine);

            //configure column 1 - the row label
            labelRow.With(o =>
            {
                o.Name = "labelRow";
                o.Location = new Point(x, _y);
                o.Text = _row.ToString();
                o.Font = Font;
                o.Width = 40;
            });

            //configure  textbox that holds the type of identification of a row
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

                    //_ScrollAmount is how much the vertical scrollbar moves. The scrollbar is visible if the number of rows
                    //exceed the height of the panel
                    _CatchCompositionData[o.Location.Y + _ScrollAmount].dataStatus = global.fad3DataStatus.statusNew;
                    _CatchCompositionData[o.Location.Y + _ScrollAmount].NameType = CurrentIDType;
                    _CatchCompositionData[o.Location.Y + _ScrollAmount].CatchCompGUID = newGUID;
                }
                o.Text = CatchComposition.IdentificationTypeToString(CurrentIDType);
                o.Tag = o.Text;
            });

            //configure  textbox that holds the first name of a catch (Genus or local name)
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
                    o.Tag = o.Text;
                }
            });

            //configure  textbox that holds the second name of a catch (species name only)
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
                if (!IsNew)
                {
                    o.Text = CatchLine.Name2;
                    o.Tag = o.Text;
                }
            });

            //configure  textbox that holds the weight of the catch. This is required
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
                if (!IsNew)
                {
                    o.Text = o.Text = CatchLine.CatchWeight.ToString();
                    o.Tag = o.Text;
                }
            });

            //configure  textbox that holds the count of individuals in the row. This could be blank
            //if individuals are numerous. A subsample will be made.
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
                if (!IsNew)
                {
                    o.Text = o.Text = CatchLine.CatchCount.ToString();
                    o.Tag = o.Text;
                }
            });

            //configure  textbox that holds the subsample weight of the row
            //when subsampling, the subsample weight and subsample count must be given.
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
                if (!IsNew)
                {
                    o.Text = CatchLine.CatchSubsampleWt.ToString();
                    o.Tag = o.Text;
                }
            });

            //configure  textbox that holds the subsample count of the row
            //when subsampling, the subsample weight and subsample count must be given.
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
                if (!IsNew)
                {
                    o.Text = CatchLine.CatchSubsampleCount.ToString();
                    o.Tag = o.Text;
                }
            });

            //configures the checkbox if a catch is from the total catch.
            chkFromTotal.With(o =>
            {
                o.Location = new Point(textSubCount.Left + textSubCount.Width + _spacer * 2, _y);
                o.Width = labelCol9.Width;
                o.Name = "chkFromTotal";
                o.Font = Font;
                o.Text = "";
                o.CheckStateChanged += OnCheckBoxCheckStateChanged;
                if (!IsNew)
                {
                    o.Checked = CatchLine.FromTotalCatch;
                }
            });

            //configures the checkbox if a catch is Live Food Fish
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

            //adds all the fields to the panel
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

            //aligns the column header labels to the colums of fields
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

            //assigns all new fields to a variable
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

        private void OncboEditor_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void OncboEditor_Validating(object sender, CancelEventArgs e)
        {
            ((ComboBox)sender).With(o =>
            {
                _CatchCompositionData[o.Location.Y + _ScrollAmount].With(ccd =>
                {
                    var s = o.Text;
                    if (s.Length > 0)
                    {
                        switch (o.Name)
                        {
                            case "cboIdentificationType":
                                CurrentIDType = CatchComposition.StringToIdentificationType(_cboEditor.Text);
                                ccd.NameType = CurrentIDType;
                                ResetRowColor(_cboEditor);
                                break;

                            case "cboGenus":
                                if (o.Text != _currentGenus)
                                {
                                    ccd.Name1 = o.Text;
                                    _currentGenus = o.Text;
                                }
                                break;

                            case "cboSpecies":
                            case "cboLocalName":
                                ccd.CatchNameGUID = ((KeyValuePair<string, string>)o.SelectedItem).Key;
                                ccd.Name1 = GetTextBoxAtRow(o.Location.Y, "txtName1").Text;
                                ccd.Name2 = GetTextBoxAtRow(o.Location.Y, "txtName2").Text;
                                break;
                        }
                    }
                    else
                    {
                        switch (o.Name)
                        {
                            case "cboSpecies":
                                ccd.Name2 = "";
                                break;

                            case "cboGenus":
                            case "cboLocalName":
                                ccd.Name1 = "";
                                break;
                        }
                    }
                });
            });
            if (_currentTextBox.Text != _cboEditor.Text)
            {
                _currentTextBox.Text = _cboEditor.Text;
                SetRowStatusToEdited(_currentTextBox);
            }
            e.Cancel = false;
        }

        /// <summary>
        /// Positions a combobox on top of a textbox field
        /// </summary>
        private void ClearMissingMarkers()
        {
            foreach (Control c in panelUI.Controls)
                if (c.GetType().Name == "TextBox") c.BackColor = SystemColors.Window;
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

                default:
                    Proceed = false;
                    break;
            }

            if (Proceed)
            {
                _cboEditor.Bounds = txt.Bounds;
                _cboEditor.BringToFront();
                _cboEditor.Show();
                _cboEditor.Focus();

                if (key > ' ')
                {
                    var itemIndex = _cboEditor.FindString(key.ToString());
                    if (itemIndex > -1)
                    {
                        if (txt.Name == "txtIdentificationType")
                        {
                            var IdType = (KeyValuePair<CatchComposition.Identification, string>)_cboEditor.Items[itemIndex];
                            _cboEditor.Text = CatchComposition.IdentificationTypeToString(IdType.Key);
                        }
                        else
                        {
                            _cboEditor.Text = _cboEditor.Items[itemIndex].ToString();
                            _cboEditor.SelectionStart = 0;
                            _cboEditor.SelectionLength = s.Length;
                        }
                    }
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
        private void FillSpeciesComboBox()
        {
            _comboSpecies.Items.Clear();
            foreach (var item in names.speciesList)
            {
                _comboSpecies.Items.Add(item);
            }
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
            _cboEditor.Hide();
        }

        private void MarkMissingFields(List<string> MissingFields, int LocationY)
        {
            foreach (var item in MissingFields)
            {
                foreach (Control c in panelUI.Controls)
                {
                    if (c.Name == item && c.Location.Y == LocationY)
                    {
                        c.BackColor = global.MissingFieldBackColor;
                        break;
                    }
                }
            }
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            HideCBOs();
            ClearMissingMarkers();
            switch (((Button)sender).Name)
            {
                case "buttonOK":
                    if (ValidateForm())
                    {
                        if (CatchComposition.UpdateCatchComposition(_CatchCompositionData))
                        {
                            Close();
                            _parentForm.RefreshCatchComposition();
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
                    var result = RowIsValid();

                    if (result.Cancel)
                        MessageBox.Show(result.Message, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        AddNewRow();

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
                    _CatchCompositionData[o.Location.Y + _ScrollAmount].FromTotalCatch = o.Checked;
                else
                    _CatchCompositionData[o.Location.Y + _ScrollAmount].LiveFish = o.Checked;
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
            panelUI.MouseWheel += OnPanelMouseWheel;
            labelTitle.Text = $"Catch composition of {_ReferenceNumber}";
            if (_IsNew)
            {
                labelTitle.Text = $"New catch composition of {_ReferenceNumber}";
                AddNewRow();
            }
            else
            {
                foreach (var item in CatchComposition.RetrieveCatchComposition(_SamplingGuid))
                {
                    AddRow(IsNew: false, item.Value);
                }
            }
        }

        private void OnPanelMouseWheel(object sender, MouseEventArgs e)
        {
            _ScrollAmount = panelUI.VerticalScroll.Value;
        }

        private void OnPanelUI_Scroll(object sender, ScrollEventArgs e)
        {
            _ScrollAmount = e.NewValue;
        }

        private void OnTextBoxChanged(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = SystemColors.Window;
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
                    names.Genus = _CatchCompositionData[o.Location.Y + _ScrollAmount].Name1;
                    FillSpeciesComboBox();
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
                if (o.Text != (o.Tag == null ? "" : o.Tag.ToString()))
                {
                    o.Tag = o.Text;
                    SetRowStatusToEdited(o);

                    if (o.Name == "txtIdentificationType")
                        ResetRowColor(o);
                    else
                        o.BackColor = SystemColors.Window;
                }

                _CatchCompositionData[o.Location.Y + _ScrollAmount].With(ccd =>
                  {
                      var s = o.Text;

                      if (s.Length == 0)
                      {
                          switch (o.Name)
                          {
                              case "txtName1":
                                  ccd.Name1 = "";
                                  break;

                              case "txtName2":
                                  ccd.Name2 = "";
                                  break;

                              case "txtWt":
                                  ccd.CatchWeight = 0;
                                  break;

                              case "txtSubWt":
                                  ccd.CatchSubsampleWt = null;
                                  break;

                              case "txtCount":
                                  ccd.CatchCount = null;
                                  break;

                              case "txtSubCount":
                                  ccd.CatchSubsampleCount = null;
                                  break;
                          }
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
                                              ccd.CatchWeight = myWeight;
                                          else
                                          {
                                              if (myWeight < ccd.CatchWeight && ccd.CatchCount == null)
                                              {
                                                  ccd.CatchSubsampleWt = myWeight;
                                              }
                                              else
                                              {
                                                  msg = "A subsample is valid only if it is less than catch weight\n\r" +
                                                        "and there is no catch count";
                                              }
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
                                                  if (ccd.CatchSubsampleWt == null)
                                                      ccd.CatchCount = myCount;
                                                  else
                                                      msg = "Catch count is only valid if there is no subsample weight";
                                              }
                                              else
                                              {
                                                  if (ccd.CatchWeight > 0 && ccd.CatchSubsampleWt != null)
                                                      ccd.CatchSubsampleCount = myCount;
                                                  else
                                                  {
                                                      msg = "Please enter values for weight and subsample weight";
                                                  }
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
            });

            if (msg.Length > 0)
            {
                e.Cancel = true;
                MessageBox.Show(msg, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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

        /// <summary>
        /// checks if a row has the required data. Has specific messages for missing data.
        /// </summary>
        /// <param name="LocationY"></param>
        /// <returns></returns>
        private bool RowHasRequired(int LocationY)
        {
            var MissingFields = new List<string>();
            var HasRequirements = true;
            _CatchCompositionData[LocationY + _ScrollAmount].With(ccd =>
            {
                if (ccd.NameType == CatchComposition.Identification.Scientific)
                {
                    HasRequirements = ccd.Name1 != null && ccd.Name2 != null && ccd.CatchWeight > 0;
                    if (!HasRequirements)
                    {
                        if (ccd.Name1 == null || ccd.Name1.Length == 0) MissingFields.Add("txtName1");
                        if (ccd.Name2 == null || ccd.Name2.Length == 0) MissingFields.Add("txtName2");
                        if (ccd.CatchWeight == 0) MissingFields.Add("txtWt");
                    }
                }
                else
                {
                    HasRequirements = ccd.Name1.Length > 0 && ccd.CatchWeight > 0;
                    if (!HasRequirements)
                    {
                        if (ccd.Name1.Length == 0) MissingFields.Add("txtName1");
                        if (ccd.CatchWeight == 0) MissingFields.Add("txtWt");
                    }
                }

                if (ccd.CatchCount == null && ccd.CatchSubsampleCount == null && ccd.CatchSubsampleWt == null)
                {
                    MissingFields.Add("txtCount");
                    HasRequirements = false;
                }
                else if (ccd.CatchCount == null)
                {
                    if (ccd.CatchSubsampleCount == null)
                    {
                        MissingFields.Add("txtSubCount");
                        HasRequirements = false;
                    }
                    if (ccd.CatchSubsampleWt == null)
                    {
                        MissingFields.Add("txtSubWt");
                        HasRequirements = false;
                    }
                }
                //}
            });
            MarkMissingFields(MissingFields, LocationY);
            return HasRequirements;
        }

        /// <summary>
        /// determines if a row has valid data. Used to check the validity of all rows during form validation and is different
        /// from RowHasRequired because it has no specific messages regarding what data is missing.
        /// </summary>
        /// <returns></returns>
        private (bool Cancel, string Message) RowIsValid()
        {
            var Cancel = false;
            var msg = "";
            SetIDType(_lastIdentification);
            if (CurrentIDType == CatchComposition.Identification.Scientific)
            {
                Cancel = _lastIdentification.Text.Length == 0 || _lastName1.Text.Length == 0
                    || _lastName2.Text.Length == 0 || _lastWeight.Text.Length == 0;
            }
            else
            {
                Cancel = _lastIdentification.Text.Length == 0 || _lastName1.Text.Length == 0
                    || _lastWeight.Text.Length == 0;
            }

            if (Cancel)
            {
                _lastIdentification.BackColor = _lastIdentification.Text.Length == 0 ? global.MissingFieldBackColor : SystemColors.Window;
                _lastName1.BackColor = _lastName1.Text.Length == 0 ? global.MissingFieldBackColor : SystemColors.Window;
                _lastWeight.BackColor = _lastWeight.Text.Length == 0 ? global.MissingFieldBackColor : SystemColors.Window;

                if (CurrentIDType == CatchComposition.Identification.Scientific)
                {
                    _lastName2.BackColor = _lastName2.Text.Length == 0 ? global.MissingFieldBackColor : SystemColors.Window;
                }

                msg = "The required fields should be filled up (Orange code)";
            }

            _lastCount.BackColor = SystemColors.Window;
            _lastSubCount.BackColor = SystemColors.Window;
            _lastSubWeight.BackColor = SystemColors.Window;

            _CatchCompositionData[_lastIdentification.Location.Y + _ScrollAmount].With(ccd =>
          {
              Cancel = ccd.CatchCount == null && ccd.CatchSubsampleWt == null && ccd.CatchSubsampleCount == null;
              if (Cancel)
              {
                  _lastCount.BackColor = global.ConflictColor1;
                  msg += "\r\nCatch count is missing (Yellow code)";
              }
              else
              {
                  Cancel = ccd.CatchCount == null && (ccd.CatchSubsampleCount == null || ccd.CatchSubsampleWt == null);
                  if (Cancel)
                  {
                      _lastSubCount.BackColor = _lastSubCount.Text.Length == 0 ? global.ConflictColor1 : SystemColors.Window;
                      _lastSubWeight.BackColor = _lastSubWeight.Text.Length == 0 ? global.ConflictColor1 : SystemColors.Window;

                      msg += "\r\nSubsample weight and count is required when there is no catch count (Yellow code)";
                  }
              }
          });

            return (Cancel, msg);
        }

        private void SetEditorEvents()
        {
            _cboEditor.KeyDown += OncboEditor_KeyDown;
            _cboEditor.KeyPress += OncboEditor_KeyPress;
            _cboEditor.Validating += OncboEditor_Validating;
        }

        private void SetIDType(TextBox source)
        {
            CurrentIDType = _CatchCompositionData[source.Location.Y + _ScrollAmount].NameType;
        }

        private void SetRowStatusToEdited(Control source)
        {
            if (_CatchCompositionData[source.Location.Y + _ScrollAmount].dataStatus != global.fad3DataStatus.statusNew)
            {
                _CatchCompositionData[source.Location.Y + _ScrollAmount].dataStatus = global.fad3DataStatus.statusEdited;
            }
        }

        private bool ValidateForm()
        {
            var IsValidated = true;

            foreach (var item in _CatchCompositionData)
            {
                if (!RowHasRequired(item.Key))
                {
                    IsValidated = false;
                }
            }
            return IsValidated;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FAD3.Database.Classes;

namespace FAD3
{
    public partial class CatchCompositionForm : Form
    {
        private Dictionary<string, CatchLine> _CatchCompositionData = new Dictionary<string, CatchLine>();

        //becomes true when _combo comboxes are added to the panel
        private bool _comboBoxesSet = false;

        //holds dropdown values but are never shown
        private ComboBox _comboGenus = new ComboBox();
        private ComboBox _comboIdentificationType = new ComboBox();
        private ComboBox _comboLocalName = new ComboBox();
        private ComboBox _comboSpecies = new ComboBox();

        //is set to the _combo comboboxes depending on the column and is
        //placed on top of the textbox
        private ComboBox _cboEditor = new ComboBox();

        //these represents the last row of fields
        private TextBox _lastCount;
        private TextBox _lastIdentification;
        private TextBox _lastName1;
        private TextBox _lastName2;
        private TextBox _lastSubCount;
        private TextBox _lastSubWeight;
        private TextBox _lastWeight;

        private int _ctlHeight;
        private int _ctlWidth = 0;
        private string _currentGenus = "";
        private Identification _CurrentIDType;
        private TextBox _currentTextBox;
        private bool _isNew;
        private MainForm _parentForm;
        private string _referenceNumber;
        private int _row = 1;
        private string _samplingGuid;
        private int _spacer = 3;
        private int _y;
        private string _newGenus = "";
        private string _currentRow = "";
        private string _currentTextContents = "";
        private bool _errorValidating = false;
        private double _weightOfCatch;
        private double _sumOfWeight;
        private double? _weightOfSample;

        private string _emptySumOfWeightsLabel;

        /// <summary>
        /// Form constructor
        /// </summary>
        /// <param name="IsNew"></param>
        /// <param name="parent"></param>
        /// <param name="samplingGuid"></param>
        /// <param name="referenceNumber"></param>
        public CatchCompositionForm(bool IsNew, MainForm parent, string samplingGuid, string referenceNumber, double weightOfCatch, double? weightOfSample)
        {
            InitializeComponent();
            _parentForm = parent;
            _samplingGuid = samplingGuid;
            _isNew = IsNew;
            _referenceNumber = referenceNumber;
            _weightOfCatch = weightOfCatch;
            _weightOfSample = weightOfSample;
        }

        /// <summary>
        /// Gets and sets the type of identification of a row
        /// </summary>
        private Identification CurrentIDType
        {
            get { return _CurrentIDType; }
            set { _CurrentIDType = value; }
        }

        /// <summary>
        /// adds a new blank row of fields for catch composition
        /// </summary>
        private void AddNewRow()
        {
            AddRow(isNew: true);
            _lastIdentification.Focus();
        }

        /// <summary>
        /// adds a row of fields with data from the database
        /// Controls are dynamically generated
        /// </summary>
        /// <param name="isNew"></param>
        /// <param name="catchLine - a row of fields of catch composition data from the database"></param>
        private void AddRow(bool isNew, string key = "", CatchLine catchLine = null)
        {
            int x = 3;
            int yPos = _y - Math.Abs(panelUI.AutoScrollPosition.Y);

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
            if (!_comboBoxesSet)
            {
                _comboBoxesSet = true;

                //combo box to specify type of identification for a catch
                //values are scientific and local name
                _comboIdentificationType.With(o =>
                {
                    o.Width = 100;
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
                    o.Width = 100;
                    o.Font = Font;
                    o.Name = "cboGenus";
                    o.Location = new Point(0, 0);
                    foreach (var item in Names.GenusList)
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
                    o.Width = 100;
                    o.Font = Font;
                    o.Name = "cboSpecies";
                    o.Location = new Point(0, 0);
                    o.DropDownStyle = ComboBoxStyle.DropDown;
                    o.DisplayMember = "Value";
                    o.ValueMember = "Key";
                    o.AutoCompleteSource = AutoCompleteSource.ListItems;
                    o.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    o.Visible = false;
                });

                //contains all the local names from the database
                _comboLocalName.With(o =>
                {
                    o.Width = 100;
                    o.Font = Font;
                    o.Name = "cboLocalName";
                    o.Location = new Point(0, 0);

                    foreach (var item in Names.LocalNameListDict)
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
            //the key is the rowGUID of the catch composition table
            if (isNew)
            {
                key = Guid.NewGuid().ToString();
                CurrentIDType = Identification.Scientific;
                _currentRow = key;
                _CatchCompositionData.Add(key, new CatchLine(_samplingGuid));
                _CatchCompositionData[key].Sequence = _row;
                _CatchCompositionData[key].dataStatus = fad3DataStatus.statusNew;
                _CatchCompositionData[key].NameType = CurrentIDType;
                _CatchCompositionData[key].CatchCompGUID = key;
            }

            //configure column 1 - the row label
            labelRow.With(o =>
            {
                o.Name = "labelRow";
                o.Location = new Point(x, yPos);
                o.Text = _row.ToString();
                o.Font = Font;
                o.Width = 40;
                o.TextAlign = ContentAlignment.MiddleLeft;
            });

            //configure  textbox that holds the type of identification of a row
            textIdentificationType.With(o =>
            {
                _ctlWidth = o.Width = 100;
                o.Height = _ctlHeight;
                o.Location = new Point(labelRow.Left + labelRow.Width + _spacer, yPos);
                o.Name = "txtIdentificationType";
                o.Font = Font;
                o.Tag = key;
                if (!isNew)
                {
                    CurrentIDType = catchLine.NameType;
                }
                o.Text = CatchComposition.IdentificationTypeToString(CurrentIDType);
                SetTextBoxEvents(o);
            });

            //configure  textbox that holds the first name of a catch (Genus or local name)
            textName1.With(o =>
            {
                o.Width = (int)(_ctlWidth * 1.2);
                o.Height = _ctlHeight;
                o.Location = new Point(textIdentificationType.Left + textIdentificationType.Width + _spacer, yPos);
                o.Name = "txtName1";
                o.Font = Font;
                o.Tag = key;
                if (!isNew)
                {
                    o.Text = catchLine.Name1;
                }
                SetTextBoxEvents(o);
            });

            //configure  textbox that holds the second name of a catch (species name only)
            textName2.With(o =>
            {
                o.Width = (int)(_ctlWidth * 1.2);
                o.Height = _ctlHeight;
                o.Location = new Point(textName1.Left + textName1.Width + _spacer, yPos);
                o.Name = "txtName2";
                o.Font = Font;
                o.Tag = key;
                if (!isNew)
                {
                    o.Text = catchLine.Name2;
                }
                SetTextBoxEvents(o);
            });

            //configure  textbox that holds the weight of the catch. This is required
            textWt.With(o =>
            {
                o.Width = (int)(_ctlWidth * 0.4);
                o.Height = _ctlHeight;
                o.Location = new Point(textName2.Left + textName2.Width + _spacer, yPos);
                o.Name = "txtWt";
                o.Font = Font;
                o.Tag = key;
                if (!isNew)
                {
                    o.Text = catchLine.CatchWeight.ToString();
                    _sumOfWeight += double.Parse(o.Text);
                }
                SetTextBoxEvents(o);
            });

            //configure  textbox that holds the count of individuals in the row. This could be blank
            //if individuals are numerous. A subsample will be made.
            textCount.With(o =>
            {
                o.Width = (int)(_ctlWidth * 0.4);
                o.Height = _ctlHeight;
                o.Location = new Point(textWt.Left + textWt.Width + _spacer, yPos);
                o.Name = "txtCount";
                o.Font = Font;
                o.Tag = key;
                if (!isNew)
                {
                    o.Text = catchLine.CatchCount.ToString();
                }
                SetTextBoxEvents(o);
            });

            //configure  textbox that holds the subsample weight of the row
            //when subsampling, the subsample weight and subsample count must be given.
            textSubWt.With(o =>
            {
                o.Width = (int)(_ctlWidth * 0.4);
                o.Height = _ctlHeight;
                o.Location = new Point(textCount.Left + textCount.Width + _spacer, yPos);
                o.Name = "txtSubWt";
                o.Font = Font;
                o.Tag = key;
                if (!isNew)
                {
                    o.Text = catchLine.CatchSubsampleWt.ToString();
                }
                SetTextBoxEvents(o);
            });

            //configure  textbox that holds the subsample count of the row
            //when subsampling, the subsample weight and subsample count must be given.
            textSubCount.With(o =>
            {
                o.Width = (int)(_ctlWidth * 0.4);
                o.Height = _ctlHeight;
                o.Location = new Point(textSubWt.Left + textSubWt.Width + _spacer, yPos);
                o.Name = "txtSubCount";
                o.Font = Font;
                o.Tag = key;
                if (!isNew)
                {
                    o.Text = catchLine.CatchSubsampleCount.ToString();
                }
                SetTextBoxEvents(o);
            });

            //configures the checkbox if a catch is from the total catch.
            chkFromTotal.With(o =>
            {
                o.Location = new Point(textSubCount.Left + textSubCount.Width + _spacer * 2, yPos);
                o.Width = labelCol9.Width;
                o.Name = "chkFromTotal";
                o.Font = Font;
                o.Text = "";
                o.Tag = key;
                if (!isNew)
                {
                    o.Checked = catchLine.FromTotalCatch;
                }
                o.CheckStateChanged += OnCheckBoxCheckStateChanged;
            });

            //configures the checkbox if a catch is Live Food Fish
            chkLiveFish.With(o =>
            {
                o.Location = new Point(chkFromTotal.Left + chkFromTotal.Width + _spacer, yPos);
                o.Width = labelCol9.Width;
                o.Name = "chkLiveFish";
                o.Font = Font;
                o.Text = "";
                o.Tag = key;
                if (!isNew) o.Checked = catchLine.LiveFish;
                o.CheckStateChanged += OnCheckBoxCheckStateChanged;
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
            if (_isNew || _row >= CatchComposition.CatchCompositionRows)
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

        private void OnTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Return)
            //{
            //    e.Handled = e.SuppressKeyPress = true;
            //}
        }

        private void OncboEditoe_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.Tab)
            {
                e.IsInputKey = true;
                GetTextBox(_cboEditor, GetNext: true).Focus();
            }
        }

        private void OncboEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                GetTextBox(_cboEditor, GetNext: true).Focus();
                //e.Handled = e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                _cboEditor.Hide();
            }
        }

        private void OncboEditor_Validating(object sender, CancelEventArgs e)
        {
            //remove the validating event to prevent multiple validatings
            _cboEditor.Validating -= OncboEditor_Validating;

            //if true we change the currenttextbox text to the editor combobox text
            //this is made false when we process a not-in-list combo text because combo contents
            //could be out of context already
            var CompareNameChanges = true;

            ((ComboBox)sender).With(o =>
            {
                _CatchCompositionData[_currentRow].With(ccd =>
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
                                _newGenus = "";

                                //search for the combobox text in the combobox items
                                if (o.FindString(o.Text) > -1)
                                {
                                    //text is found in the items
                                    //if (o.Text != _currentGenus)
                                    //{
                                    ccd.Name1 = o.Text;
                                    _currentGenus = o.Text;
                                    //}
                                }
                                else
                                {
                                    //text is not found in the items
                                    DialogResult dr = MessageBox.Show($"{o.Text} is a new genus\r\nWould you like to create a new species based on the new genus?",
                                                                      "Item not in list", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                                    if (dr == DialogResult.Yes)
                                    {
                                        _newGenus = o.Text;
                                        _currentTextBox.Text = _newGenus;
                                        _currentGenus = _newGenus;

                                        CompareNameChanges = false;

                                        //empty the current row's name2 field
                                        GetTextBox(null, _currentTextBox, true).Text = "";
                                    }
                                    else
                                    {
                                        o.Text = "";
                                        _currentTextBox.Text = "";
                                        _currentTextBox.Focus();
                                        e.Cancel = true;
                                    }
                                }
                                break;

                            case "cboSpecies":
                            case "cboLocalName":
                                //search the text in the comboboox items
                                var itemPosition = o.FindString(o.Text);
                                var isInList = itemPosition > -1;

                                if (isInList)
                                {
                                    try
                                    {
                                        if (o.SelectedItem == null)
                                            o.SelectedItem = itemPosition;

                                        //we have to test if the found test matches any of the keys
                                        ccd.CatchNameGUID = ((KeyValuePair<string, string>)o.SelectedItem).Key;
                                        ccd.Name1 = GetTextBoxAtRow(o.Tag.ToString(), "txtName1").Text;
                                        ccd.Name2 = GetTextBoxAtRow(o.Tag.ToString(), "txtName2").Text;
                                    }
                                    catch
                                    {
                                        isInList = false;
                                    }
                                }

                                if (!isInList)
                                {
                                    //text not in list
                                    var msg = "";
                                    if (o.Name == "cboSpecies")
                                    {
                                        msg = $"{_currentGenus} {o.Text} is not in the list of species\r\nWould you like to add a new species?";
                                    }
                                    else
                                    {
                                        msg = $"{o.Text} is not in the list of local names\r\nWould you like to add a new local name?";
                                    }

                                    if (_newGenus.Length == 0 && msg.Length > 0)
                                    {
                                        DialogResult dr = MessageBox.Show(msg, "Item not in list", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                                        if (dr == DialogResult.Yes)
                                        {
                                            CompareNameChanges = false;
                                            if (o.Name == "cboSpecies")
                                            {
                                                _currentTextBox.Text = _cboEditor.Text.Trim();
                                                SpeciesNameForm snf = new SpeciesNameForm(_currentGenus, o.Text, this);
                                                snf.ShowDialog(this);
                                            }
                                            else
                                            {
                                                //add new local name here
                                            }
                                        }
                                        else
                                        {
                                            o.Text = "";
                                            _currentTextBox.Text = "";
                                            e.Cancel = true;
                                        }
                                    }
                                    else
                                    {
                                        //add new genus and species here

                                        CompareNameChanges = false;

                                        _currentTextBox.Text = _cboEditor.Text.Trim();
                                        SpeciesNameForm snf = new SpeciesNameForm(_newGenus, o.Text, this);
                                        snf.ShowDialog(this);
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        //combobox text is empty
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

            if (!e.Cancel && _currentTextBox.Text != _cboEditor.Text && CompareNameChanges)
            {
                _currentTextBox.Text = _cboEditor.Text.Trim();
                SetRowStatusToEdited(_currentTextBox);
            }
        }

        public void NewName(bool Accepted, string acceptedGenus = "", string acceptedSpecies = "", string acceptedNewGuid = "")
        {
            if (!Accepted)
            {
                //get the current row name1 and name2 textboxes
                var txtGenus = GetTextBox(fromComboBox: null, fromTextBox: _currentTextBox, GetNext: false, GetPrevious: true);
                var txtSpecies = GetTextBox(fromComboBox: null, fromTextBox: _currentTextBox);

                if (_newGenus.Length > 0)
                {
                    txtSpecies.Text = "";
                    txtGenus.Text = "";
                    _cboEditor.Text = "";
                }
                else
                {
                    if (acceptedGenus.Length > 0 && acceptedSpecies.Length > 0)
                    {
                        txtGenus.Text = acceptedGenus;
                        txtSpecies.Text = acceptedSpecies;
                        _cboEditor.Text = acceptedSpecies;
                        if (acceptedNewGuid.Length > 0)
                        {
                            _cboEditor.Items.Add(new KeyValuePair<string, string>(acceptedNewGuid, acceptedSpecies));
                        }
                    }
                    else
                    {
                        txtSpecies.Text = "";
                    }
                }
            }
            else
            {
                if (_newGenus.Length > 0)
                {
                    _comboGenus.Items.Add(_newGenus);
                    //var item = new KeyValuePair<string, string>(acceptedNewGuid, acceptedSpecies);
                    //_comboSpecies.Items.Add(item);
                    //_cboEditor.Items.Add(item);
                }
            }
            _newGenus = "";
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
            var s = txt.Text;
            _cboEditor = null;

            switch (txt.Name)
            {
                case "txtIdentificationType":
                    _cboEditor = _comboIdentificationType;
                    break;

                case "txtName1":
                case "txtName2":
                    if (CurrentIDType == Identification.LocalName)
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
                _cboEditor.Tag = txt.Tag;
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
                            var kv = (KeyValuePair<Identification, string>)_cboEditor.Items[itemIndex];
                            _cboEditor.Text = CatchComposition.IdentificationTypeToString(kv.Key);
                        }
                        else if (txt.Name == "txtName2")
                        {
                            var kv = (KeyValuePair<string, string>)_cboEditor.Items[itemIndex];
                            _cboEditor.Text = kv.Value;
                        }
                        else if (txt.Name == "txtName1" && _cboEditor.Name == "cboLocalName")
                        {
                            var kv = (KeyValuePair<string, string>)_cboEditor.Items[itemIndex];
                            _cboEditor.Text = kv.Value;
                        }
                        else
                        {
                            _cboEditor.Text = _cboEditor.Items[itemIndex].ToString();
                        }
                        _cboEditor.SelectionStart = 1;
                        _cboEditor.SelectionLength = _cboEditor.Text.Length;
                    }
                    else
                    {
                        _cboEditor.Text = key.ToString();
                        _cboEditor.SelectionStart = 1;
                    }
                }
                else if (s.Length > 0)
                {
                    _cboEditor.Text = s;
                    _cboEditor.SelectionStart = 0;
                    _cboEditor.SelectionLength = _cboEditor.Text.Length;
                }

                //_cboEditor.SelectionLength = _cboEditor.Text.Length;

                SetEditorEvents();
            }
        }

        /// <summary>
        /// fills up the species combo box depending on the selected genus
        /// </summary>
        private void FillSpeciesComboBox()
        {
            _comboSpecies.Items.Clear();
            foreach (var item in Names.SpeciesList)
            {
                _comboSpecies.Items.Add(item);
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
                    if (c.Name == NextCheckBox && c.Tag.ToString() == source.Tag.ToString())
                    {
                        chk = (CheckBox)c;
                        break;
                    }
                }
            }

            return chk;
        }

        private TextBox GetTextBox(ComboBox fromComboBox = null, TextBox fromTextBox = null, bool GetNext = false, bool GetPrevious = false)
        {
            var myTextBox = new TextBox();
            var myTextBoxName = "";
            var sourceTag = "";

            if (fromComboBox != null)
            {
                sourceTag = fromComboBox.Tag.ToString();
                switch (_cboEditor.Name)
                {
                    case "cboIdentificationType":
                        myTextBoxName = "txtIdentificationType";
                        if (GetNext) myTextBoxName = "txtName1";
                        break;

                    case "cboGenus":

                        myTextBoxName = "txtName1";
                        if (GetNext) myTextBoxName = "txtName2";
                        if (GetPrevious) myTextBoxName = "txtName1";
                        break;

                    case "cboSpecies":
                    case "cboLocalName":

                        if (_cboEditor.Name == "cboSpecies")
                        {
                            myTextBoxName = "txtName2";
                            if (GetPrevious) myTextBoxName = "txtName1";
                        }
                        else
                        {
                            myTextBoxName = "txtName1";
                            if (GetPrevious) myTextBoxName = "txtName1";
                        }
                        if (GetNext) myTextBoxName = "txtWt";

                        break;
                }
            }

            if (fromTextBox != null)
            {
                sourceTag = fromTextBox.Tag.ToString();
                myTextBoxName = fromTextBox.Name;

                switch (myTextBoxName)
                {
                    case "txtName1":
                        if (GetNext)
                        {
                            myTextBoxName = "txtName2";
                        }

                        if (GetPrevious)
                        {
                            myTextBoxName = "txtName1";
                        }
                        break;

                    case "txtName2":
                        if (GetNext)
                        {
                            myTextBoxName = "txtWt";
                        }

                        if (GetPrevious)
                        {
                            myTextBoxName = "txtName1";
                        }
                        break;

                    case "txtWt":
                        if (GetNext)
                        {
                            myTextBoxName = "txtCount";
                        }

                        if (GetPrevious)
                        {
                            myTextBoxName = "txtName2";
                        }
                        break;

                    case "txtCount":
                        if (GetNext)
                        {
                            myTextBoxName = "txtSubWt";
                        }

                        if (GetPrevious)
                        {
                            myTextBoxName = "txtWt";
                        }
                        break;

                    case "txtSubWt":
                        if (GetNext)
                        {
                            myTextBoxName = "txtSubCount";
                        }

                        if (GetPrevious)
                        {
                            myTextBoxName = "txtCount";
                        }
                        break;

                    case "txtSubCount":
                        if (GetNext)
                        {
                            myTextBoxName = "txtSubCount";
                        }

                        if (GetPrevious)
                        {
                            myTextBoxName = "txtCount";
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

        private TextBox GetTextBoxAtRow(string key, string textBoxName)
        {
            var txt = new TextBox();
            foreach (Control c in panelUI.Controls)
            {
                if (c.Name == textBoxName && c.Tag.ToString() == key)
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

            if (_cboEditor != null)
                _cboEditor.Hide();
        }

        private void MarkMissingFields(List<string> MissingFields, string key)
        {
            foreach (var item in MissingFields)
            {
                foreach (Control c in panelUI.Controls)
                {
                    if (c.Name == item && c.Tag.ToString() == key)
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
                    _CatchCompositionData[o.Tag.ToString()].FromTotalCatch = o.Checked;
                else
                    _CatchCompositionData[o.Tag.ToString()].LiveFish = o.Checked;
            });
        }

        private void OnForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return && e.Shift)
            {
                var ev = new EventArgs();
                OnButtonClick(buttonAdd, ev);
            }
            else if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Tab)
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
                            if (CurrentIDType == Identification.LocalName)
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
            global.LoadFormSettings(this, true);
            _emptySumOfWeightsLabel = labelSumOfWeight.Text;
            labelTitle.Text = $"Catch composition of {_referenceNumber}";
            if (_isNew)
            {
                labelTitle.Text = $"New catch composition of {_referenceNumber}";
                AddNewRow();
            }
            else
            {
                _CatchCompositionData = CatchComposition.RetrieveCatchComposition(_samplingGuid);
                foreach (var item in _CatchCompositionData)
                {
                    AddRow(isNew: false, item.Key, item.Value);
                }
                labelSumOfWeight.Text += $" {_sumOfWeight.ToString("0.000")}";
                if (_sumOfWeight > _weightOfCatch)
                {
                    labelSumOfWeight.ForeColor = Color.Red;
                }
            }

            labelWtCatch.Text += $" {_weightOfCatch.ToString("0.000")}";
            if (_weightOfSample != null)
            {
                labelWtSample.Text += $" {((double)_weightOfSample).ToString("0.000")}";
            }
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
                _currentRow = o.Tag.ToString();
                if (!_errorValidating)
                {
                    _currentTextContents = o.Text;
                }

                SetIDType(o);
                if (o.Name == "txtName2" && _CurrentIDType == Identification.Scientific)
                {
                    if (_newGenus.Length == 0)
                    {
                        Names.Genus = _CatchCompositionData[_currentRow].Name1;
                        FillSpeciesComboBox();
                    }
                    else
                    {
                        _comboSpecies.Items.Clear();
                        _comboSpecies.Text = "";
                    }
                }
            });
            HideCBOs();
        }

        private void OnTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            ControlToFocus((TextBox)sender, e.KeyChar);
            if (e.KeyChar == (char)Keys.Return)
            {
                e.Handled = true;
            }
        }

        private void ComputeSumOfWeights()
        {
            var weight = 0D;
            foreach (var item in _CatchCompositionData)
            {
                if (item.Value.dataStatus != fad3DataStatus.statusForDeletion) weight += item.Value.CatchWeight;
            }
            labelSumOfWeight.Text = $"{_emptySumOfWeightsLabel} {weight}";

            if (weight > _weightOfCatch)
            {
                labelSumOfWeight.ForeColor = Color.Red;
            }
            else
            {
                labelSumOfWeight.ForeColor = SystemColors.WindowText;
            }
        }

        private void OnTextBoxValidating(object sender, CancelEventArgs e)
        {
            var msg = "";
            _errorValidating = false;

            ((TextBox)sender).With(o =>
            {
                _CatchCompositionData[_currentRow].With(ccd =>
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
                                  ComputeSumOfWeights();
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
                                  if (CurrentIDType == Identification.LocalName && o.Text.Length > 0)
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
                                          {
                                              ccd.CatchWeight = myWeight;
                                              ComputeSumOfWeights();
                                          }
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

                //mark the current row as edited if there are no
                //messages and the text content is different
                if (msg.Length == 0)
                {
                    if (o.Text != _currentTextContents)
                    {
                        _currentTextContents = o.Text;
                        SetRowStatusToEdited(o);

                        if (o.Name == "txtIdentificationType")
                            ResetRowColor(o);
                        else
                            o.BackColor = SystemColors.Window;
                    }
                }
                else
                    _errorValidating = true;
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
                    && c.Tag.ToString() == sourceLocation.Tag.ToString())
                {
                    c.BackColor = SystemColors.Window;
                    n++;
                    if (n == 4) break;
                }
            }
        }

        /// <summary>
        /// checks if a row has the required data. Has specific messages for missing data.
        /// This is called when doing a form validation and check all rows for completeness
        /// </summary>
        /// <param name="LocationY"></param>
        /// <returns></returns>
        private bool RowHasRequired(string key)
        {
            var MissingFields = new List<string>();
            var HasRequirements = true;
            if (_currentRow.Length > 0)
            {
                _CatchCompositionData[_currentRow].With(ccd =>
                {
                    if (ccd.NameType == Identification.Scientific)
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

                MarkMissingFields(MissingFields, key);
                return HasRequirements;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// determines if a row has valid data. Used to check the validity of last row when adding a new row and is different
        /// from RowHasRequired because this has specific messages regarding what data is missing.
        /// </summary>
        /// <returns></returns>
        private (bool Cancel, string Message) RowIsValid()
        {
            var Cancel = false;
            var msg = "";
            SetIDType(_lastIdentification);
            if (CurrentIDType == Identification.Scientific)
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

                if (CurrentIDType == Identification.Scientific)
                {
                    _lastName2.BackColor = _lastName2.Text.Length == 0 ? global.MissingFieldBackColor : SystemColors.Window;
                }

                msg = "The required fields should be filled up (Orange code)";
            }

            _lastCount.BackColor = SystemColors.Window;
            _lastSubCount.BackColor = SystemColors.Window;
            _lastSubWeight.BackColor = SystemColors.Window;

            _CatchCompositionData[_lastIdentification.Tag.ToString()].With(ccd =>
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

        private void SetTextBoxEvents(TextBox t)
        {
            t.GotFocus += OnTextBoxGotFocus;
            t.Validating += OnTextBoxValidating;
            t.TextChanged += OnTextBoxChanged;
            t.KeyUp += OnTextBoxKeyUp;
            t.KeyPress += OnTextBoxKeyPress;
            t.DoubleClick += OnTextBoxDoubleClick;
        }

        private void SetEditorEvents()
        {
            _cboEditor.KeyDown += OncboEditor_KeyDown;
            _cboEditor.Validating += OncboEditor_Validating;
            _cboEditor.PreviewKeyDown += OncboEditoe_PreviewKeyDown;
        }

        private void SetIDType(TextBox source)
        {
            CurrentIDType = _CatchCompositionData[source.Tag.ToString()].NameType;
        }

        private void SetRowStatusToEdited(Control source)
        {
            if (_CatchCompositionData[source.Tag.ToString()].dataStatus != fad3DataStatus.statusNew)
            {
                _CatchCompositionData[source.Tag.ToString()].dataStatus = fad3DataStatus.statusEdited;
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

        private void OnCatchCompositionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            global.SaveFormSettings(this);
        }
    }
}
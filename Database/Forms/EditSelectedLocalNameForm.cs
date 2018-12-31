using FAD3.Database.Classes;
using System;
using System.Windows.Forms;

namespace FAD3.Database.Forms
{
    public partial class EditSelectedLocalNameForm : Form
    {
        private string _selectedName;
        private string _nameGuid;
        private CatchLocalNameSelectedForm _parentForm;
        private Identification _idType;
        private string _language;
        private string _namePair;
        private EditActionType _editActionType;

        public EditSelectedLocalNameForm(string selectedName, string nameGuid, CatchLocalNameSelectedForm parentForm)
        {
            InitializeComponent();
            _selectedName = selectedName;
            _nameGuid = nameGuid;
            _parentForm = parentForm;
            _editActionType = EditActionType.ActionTypeEdit;
        }

        public EditSelectedLocalNameForm(Identification idType, string language, string namePair, CatchLocalNameSelectedForm parentForm)
        {
            InitializeComponent();
            _idType = idType;
            _language = language;
            _namePair = namePair;
            _parentForm = parentForm;
            if (_idType == Identification.LocalName)
            {
                foreach (var item in Names.AllSpeciesDictionary)
                {
                    comboBox.Items.Add(item);
                }
                lblEdit.Text = "Select a species name";
                Text = "Provide a species name";
            }
            else if (_idType == Identification.Scientific)
            {
                foreach (var item in Names.LocalNameListDict)
                {
                    comboBox.Items.Add(item);
                }
                lblEdit.Text = "Select a local/common name";
                Text = "Provide a local name";
            }
            comboBox.ValueMember = "Key";
            comboBox.DisplayMember = "Value";
            comboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox.Visible = true;
            txtLocalName.Visible = false;
            comboBox.Location = txtLocalName.Location;
            comboBox.Size = txtLocalName.Size;
            _editActionType = EditActionType.ActionTypeAdd;
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    switch (_editActionType)
                    {
                        case EditActionType.ActionTypeEdit:
                            if (Names.UpdateLocalName(txtLocalName.Text, _nameGuid))
                            {
                                Close();
                                _parentForm.UpdateList(_idType);
                            }
                            break;

                        case EditActionType.ActionTypeAdd:
                            bool success = false;
                            var sn = "";
                            var ln = "";
                            if (_idType == Identification.LocalName)
                            {
                                sn = comboBox.Text;
                                ln = _namePair;
                            }
                            else
                            {
                                sn = _namePair;
                                ln = comboBox.Text;
                            }
                            Names.SaveNewLocalSpeciesNameLanguage(sn, _language, ln, out success);
                            if (success)
                            {
                                Close();
                                _parentForm.UpdateList(_idType);
                            }

                            break;
                    }

                    break;

                case "btnCancel":
                    Close();
                    break;
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            txtLocalName.Text = _selectedName;
            global.LoadFormSettings(this, true);
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            global.SaveFormSettings(this);
        }
    }
}
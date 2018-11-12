using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FAD3.Database.Classes;
using FAD3.GUI.Classes;

namespace FAD3.Database.Forms
{
    public partial class LocalNameSciNameEditForm : Form
    {
        private string _selectedName;
        private Identification _idType;
        private string _selectedNameGuid;
        private CatchLocalNamesForm _parentForm;
        private static LocalNameSciNameEditForm _instance;

        public static LocalNameSciNameEditForm GetInstance(string selectedName, string selectedNameGuid, Identification idType, CatchLocalNamesForm parentForm)
        {
            if (_instance == null) _instance = new LocalNameSciNameEditForm(selectedName, selectedNameGuid, idType, parentForm);
            return _instance;
        }

        public static LocalNameSciNameEditForm GetInstance()
        {
            if (_instance != null) return _instance;
            return null;
        }

        public void SetSelectedName(string selectedName, string selectedNameGuid, Identification idType)
        {
            _selectedName = selectedName;
            _selectedNameGuid = selectedNameGuid;
            _idType = idType;
            switch (_idType)
            {
                case Identification.LocalName:
                    cboLocalName.Text = _selectedName;
                    break;

                case Identification.Scientific:
                    cboSpeciesName.Text = _selectedName;
                    break;
            }
        }

        public LocalNameSciNameEditForm(string selectedName, string selectedNameGuid, Identification idType, CatchLocalNamesForm parentForm)
        {
            InitializeComponent();
            _selectedName = selectedName;
            _selectedNameGuid = selectedNameGuid;
            _idType = idType;
            _parentForm = parentForm;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            global.LoadFormSettings(this, true);
            foreach (var item in Names.Languages)
            {
                cboLanguage.Items.Add(item);
            }
            cboLanguage.ValueMember = "key";
            cboLanguage.DisplayMember = "value";

            switch (_idType)
            {
                case Identification.LocalName:
                    foreach (var item in Names.AllSpeciesDictionary)
                    {
                        cboSpeciesName.Items.Add(item);
                    }
                    cboSpeciesName.DropDownStyle = ComboBoxStyle.DropDown;
                    cboSpeciesName.DisplayMember = "Value";
                    cboSpeciesName.ValueMember = "Key";
                    cboSpeciesName.AutoCompleteSource = AutoCompleteSource.ListItems;
                    cboSpeciesName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

                    cboLocalName.Text = _selectedName;
                    cboLocalName.Enabled = false;
                    break;

                case Identification.Scientific:
                    foreach (var item in Names.LocalNameListDict)
                    {
                        cboLocalName.Items.Add(item);
                    }

                    cboLocalName.DropDownStyle = ComboBoxStyle.DropDown;
                    cboLocalName.DisplayMember = "Value";
                    cboLocalName.ValueMember = "Key";
                    cboLocalName.AutoCompleteSource = AutoCompleteSource.ListItems;
                    cboLocalName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

                    cboSpeciesName.Text = _selectedName;
                    cboSpeciesName.Enabled = false;
                    break;
            }
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            global.SaveFormSettings(this);
            _instance = null;
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    var success = false;
                    if (cboLocalName.Text.Length > 0
                        && cboLanguage.Text.Length > 0
                        && cboSpeciesName.Text.Length > 0)
                    {
                        switch (_idType)
                        {
                            case Identification.LocalName:

                                success = Names.SaveNewLocalSpeciesNameLanguage(
                            ((KeyValuePair<string, string>)cboSpeciesName.SelectedItem).Key,
                            ((KeyValuePair<string, string>)cboLanguage.SelectedItem).Key,
                            _selectedNameGuid
                            );
                                break;

                            case Identification.Scientific:
                                success = Names.SaveNewLocalSpeciesNameLanguage(
                                    _selectedNameGuid,
                                    ((KeyValuePair<string, string>)cboLanguage.SelectedItem).Key,
                                    ((KeyValuePair<string, string>)cboLocalName.SelectedItem).Key
                                    );
                                break;
                        }
                        if (success)
                        {
                            _parentForm.RefreshLists();
                            switch (_idType)
                            {
                                case Identification.LocalName:
                                    cboSpeciesName.Text = "";
                                    break;

                                case Identification.Scientific:
                                    cboLocalName.Text = "";
                                    cboLocalName.Focus();
                                    break;
                            }
                        }
                        else
                        {
                            MessageBox.Show("The species name-local name-language combination is already in the database",
                                             "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("All fields must be filled up", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

                case "btnCancel":
                    Close();
                    break;
            }
        }

        private void OnComboValidating(object sender, CancelEventArgs e)
        {
            ComboBox cbo = (ComboBox)sender;
            string s = cbo.Text;
            if (s.Length > 0)
            {
                switch (cbo.Name)
                {
                    case "cboLocalName":
                        if (!Names.LocalNamesReverseDictionary.ContainsKey(s))
                        {
                            DialogResult dr = MessageBox.Show($"The local/commom name '{s}' is not listed\r\n" +
                                                                "Do you want to add this to the list?",
                                                                "New local name",
                                                                MessageBoxButtons.YesNo,
                                                                MessageBoxIcon.Information);
                            if (dr == DialogResult.Yes)
                            {
                                NewFisheryObjectName nfo = new NewFisheryObjectName(s, GUI.Classes.FisheryObjectNameType.CatchLocalName);
                                var result = Names.SaveNewLocalName(nfo);
                                if (result.success)
                                {
                                    KeyValuePair<string, string> kv = new KeyValuePair<string, string>(result.newGuid, s);
                                    cbo.Items.Add(kv);
                                    cbo.Text = s;
                                }
                            }
                        }
                        break;

                    case "cboSpeciesName":
                        break;

                    case "cboLanguage":
                        if (!Names.Languages.ContainsValue(s))
                        {
                            DialogResult dr = MessageBox.Show($"The language '{s}' is not listed\r\n" +
                                    "Do you want to add this to the list?",
                                    "New local name",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Information);
                            if (dr == DialogResult.Yes)
                            {
                                var result = Names.SaveNewLanguage(s);
                                if (result.success)
                                {
                                    KeyValuePair<string, string> kv = new KeyValuePair<string, string>(result.guid, s);
                                    cbo.Items.Add(kv);
                                    cbo.Text = s;
                                }
                            }
                        }
                        break;
                }
            }
        }
    }
}
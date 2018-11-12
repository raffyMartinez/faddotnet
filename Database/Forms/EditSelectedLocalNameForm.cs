﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FAD3.GUI.Classes;

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

        public EditSelectedLocalNameForm(string selectedName, string nameGuid, CatchLocalNameSelectedForm parentForm)
        {
            InitializeComponent();
            _selectedName = selectedName;
            _nameGuid = nameGuid;
            _parentForm = parentForm;
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
            }
            else if (_idType == Identification.Scientific)
            {
                foreach (var item in Names.LocalNameListDict)
                {
                    comboBox.Items.Add(item);
                }
                lblEdit.Text = "Select a local/common name";
            }
            comboBox.ValueMember = "Key";
            comboBox.DisplayMember = "Value";
            comboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox.Visible = true;
            txtLocalName.Visible = false;
            comboBox.Location = txtLocalName.Location;
            comboBox.Size = txtLocalName.Size;
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    if (Names.UpdateLocalName(txtLocalName.Text, _nameGuid))
                    {
                        Close();
                        _parentForm.UpdateList(txtLocalName.Text);
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FAD3.Database.Forms
{
    public partial class LocalNameSciNameEditForm : Form
    {
        public LocalNameSciNameEditForm()
        {
            InitializeComponent();
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

            foreach (var item in Names.AllSpeciesDictionary)
            {
                cboSpeciesName.Items.Add(item);
            }
            cboSpeciesName.ValueMember = "key";
            cboSpeciesName.DisplayMember = "value";

            foreach (var item in Names.LocalNameListDict)
            {
                cboLocalName.Items.Add(item);
            }
            cboLocalName.ValueMember = "key";
            cboLocalName.DisplayMember = "value";
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            global.SaveFormSettings(this);
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    if (cboLocalName.Text.Length > 0
                        && cboLanguage.Text.Length > 0
                        && cboSpeciesName.Text.Length > 0)
                    {
                        if (Names.SaveNewLocalSpeciesNameLanguage(
                            ((KeyValuePair<string, string>)cboSpeciesName.SelectedItem).Key,
                            ((KeyValuePair<string, string>)cboLanguage.SelectedItem).Key,
                            ((KeyValuePair<string, string>)cboLocalName.SelectedItem).Key
                            ))
                        {
                            Close();
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
    }
}
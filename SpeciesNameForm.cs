using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FAD3
{
    public partial class SpeciesNameForm : Form
    {
        private string _nameGuid = "";
        private string _species;
        private string _genus;
        private string _taxaName;
        private bool _isNew;
        private string _dialogTitle;

        public SpeciesNameForm()
        {
            InitializeComponent();
            _isNew = true;
            _dialogTitle = "New species";
        }

        public SpeciesNameForm(string genus, string species)
        {
            InitializeComponent();
            _genus = genus;
            _species = species;
            _isNew = true;
            _dialogTitle = $"New species {genus} {species}";
        }

        public SpeciesNameForm(string genus, string species, string nameGuid, string taxaName)
        {
            InitializeComponent();
            _genus = genus;
            _species = species;
            _nameGuid = nameGuid;
            _taxaName = taxaName;
            _isNew = false;
            _dialogTitle = $"Data for the species {genus} {species}";
        }

        private void SpeciesNameForm_Load(object sender, EventArgs e)
        {
            foreach (var item in GMSManager.RetrieveTaxaDictionary())
            {
                cboTaxa.Items.Add(item);
            }
            cboTaxa.DisplayMember = "Value";
            cboTaxa.ValueMember = "Key";
            cboTaxa.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTaxa.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboTaxa.AutoCompleteMode = AutoCompleteMode.Append;

            txtGenus.Text = _genus;
            txtSpecies.Text = _species;

            if (!_isNew)
            {
                var speciesData = names.RetrieveSpeciesData(_nameGuid);
                cboTaxa.Text = GMSManager.TaxaNameFromTaxa(speciesData.taxa);
            }
        }
    }
}
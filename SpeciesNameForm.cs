using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MetaphoneCOM;

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
        private int? _fishBaseSpeciesNumber = null;
        private short _genusMPH1;
        private short _genusMPH2;
        private short _speciesMPH1;
        private short _speciesMPH2;
        private TextBox txtSimilarNames;
        private Form _parentForm = new Form();

        public SpeciesNameForm(Form Parent)
        {
            InitializeComponent();
            _isNew = true;
            _dialogTitle = "New species";
            _parentForm = Parent;
        }

        public SpeciesNameForm(string genus, string species, Form Parent)
        {
            InitializeComponent();
            _genus = genus;
            _species = species;
            _isNew = true;
            _dialogTitle = $"New species {genus} {species}";
            _parentForm = Parent;
        }

        public SpeciesNameForm(string genus, string species, string nameGuid, string taxaName, Form Parent)
        {
            InitializeComponent();
            _genus = genus;
            _species = species;
            _nameGuid = nameGuid;
            _taxaName = taxaName;
            _isNew = false;
            _dialogTitle = $"Data for the species {genus} {species}";
            _parentForm = Parent;
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
                chkInFishbase.Checked = speciesData.inFishbase;
                _fishBaseSpeciesNumber = speciesData.fishBaseNo;
                txtNotes.Text = speciesData.notes;
                _genusMPH1 = speciesData.genusKey1 ?? default;
                _genusMPH2 = speciesData.genusKey2 ?? default;
                _speciesMPH1 = speciesData.speciesKey1 ?? default;
                _speciesMPH2 = speciesData.speciesKey2 ?? default;
            }
            else
            {
                var speciesFishBaseData = names.NameInFishBaseEx(_genus, _species);
                chkInFishbase.Checked = speciesFishBaseData.inFishBase;
                _fishBaseSpeciesNumber = speciesFishBaseData.fishBaseSpeciesNo;
                if (chkInFishbase.Checked) cboTaxa.Text = "Fish";

                var metaPhone = new DoubleMetaphoneShort();
                metaPhone.ComputeMetaphoneKeys(_genus, out short k1, out short k2);
                _genusMPH1 = k1;
                _genusMPH2 = k2;

                metaPhone.ComputeMetaphoneKeys(_species, out k1, out k2);
                _speciesMPH1 = k1;
                _speciesMPH2 = k2;

                var SimilarSoundingList = names.RetrieveSpeciesWithSimilarMetaPhone(_genusMPH1, _genusMPH2, _speciesMPH1, _speciesMPH2);
                if (SimilarSoundingList.Count > 0)
                {
                    Width = (int)(Width * 2);
                    ListView lvSimilarNames = new ListView
                    {
                        Name = "lvSimilarNames",
                        Top = txtSpecies.Top,
                        Left = txtGenus.Left + txtGenus.Width + 20,
                        Height = (txtNotes.Top + txtNotes.Height) - txtSpecies.Top,
                        View = View.List,
                    };
                    lvSimilarNames.Width = Width - (lvSimilarNames.Left + 20);
                    lvSimilarNames.DoubleClick += OnListViewDoubleClick;

                    Label labelSimilar = new Label
                    {
                        Left = lvSimilarNames.Left,
                        Top = labelGenus.Top,
                        Text = "Similar sounding names",
                        AutoSize = false,
                        Width = lvSimilarNames.Width
                    };

                    Controls.Add(lvSimilarNames);
                    Controls.Add(labelSimilar);

                    foreach (var item in SimilarSoundingList)
                    {
                        var lvi = new ListViewItem(new string[] { item.fullName, item.genus, item.species });
                        lvSimilarNames.Items.Add(lvi);
                    }
                }
            }
        }

        private void OnListViewDoubleClick(object sender, EventArgs e)
        {
            var lvi = ((ListView)sender).SelectedItems[0];
            ((CatchCompositionForm)_parentForm).NewName(Accepted: false, lvi.SubItems[1].Text, lvi.SubItems[2].Text);
            Close();
        }

        private bool ValidateForm()
        {
            var cancel = txtGenus.Text.Length == 0 || txtSpecies.Text.Length == 0;
            if (!cancel)
            {
            }
            return true;
        }

        private void Onbutton_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonOK":
                    if (ValidateForm())
                    {
                        if (names.UpdateSpeciesData())
                        {
                            Close();
                            ((CatchCompositionForm)_parentForm).NewName(Accepted: true);
                        }
                    }
                    break;

                case "buttonCancel":
                    ((CatchCompositionForm)_parentForm).NewName(Accepted: false);
                    Close();
                    break;
            }
        }
    }
}
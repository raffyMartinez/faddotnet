using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MetaphoneCOM;
using FAD3.GUI.Classes;

namespace FAD3
{
    public partial class SpeciesNameForm : Form
    {
        private string _nameGuid = "";
        private string _species = "";
        private string _genus = "";
        private string _taxaName;
        private Taxa _taxa = Taxa.To_be_determined;
        private fad3DataStatus _dataStatus = fad3DataStatus.statusFromDB;
        private string _dialogTitle;
        private bool _inFishBase;
        private int? _fishBaseSpeciesNumber = null;
        private short _genusMPH1;
        private short _genusMPH2;
        private short _speciesMPH1;
        private short _speciesMPH2;
        private string _notes = "";
        private Form _parentForm = new Form();
        private int _catchCompositionRecordCount;
        public bool ReadOnly { get; set; }

        public SpeciesNameForm(Form Parent)
        {
            InitializeComponent();
            _dataStatus = fad3DataStatus.statusNew;
            _dialogTitle = "New species";
            _parentForm = Parent;
        }

        public SpeciesNameForm(string speciesName, Form parent)
        {
            InitializeComponent();
            var arr = speciesName.Split(' ');
            for (int n = 0; n < arr.Length; n++)
            {
                if (n == 0)
                {
                    _genus = arr[n];
                }
                else
                {
                    _species = arr[n] + " ";
                }
            }
            _species = _species.Trim(' ');
            _taxa = CatchName.TaxaFromCatchName(_genus, _species);
            _nameGuid = Names.AllSpeciesReverseDictionary[_genus + " " + _species];
            _parentForm = parent;
        }

        public SpeciesNameForm(string genus, string species, Form Parent)
        {
            InitializeComponent();
            _genus = genus;
            _species = species;
            _dataStatus = fad3DataStatus.statusNew;
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
            _dataStatus = fad3DataStatus.statusFromDB;
            _dialogTitle = $"Data for the species {genus} {species}";
            _parentForm = Parent;
        }

        private void SpeciesNameForm_Load(object sender, EventArgs e)
        {
            foreach (var item in CatchName.RetrieveTaxaDictionary())
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

            if (_dataStatus != fad3DataStatus.statusNew)
            {
                var speciesData = Names.RetrieveSpeciesData(_nameGuid);
                _taxaName = cboTaxa.Text = CatchName.TaxaNameFromTaxa(speciesData.taxa);
                _inFishBase = chkInFishbase.Checked = speciesData.inFishbase;
                _fishBaseSpeciesNumber = speciesData.fishBaseNo;
                _notes = txtNotes.Text = speciesData.notes;
                _genusMPH1 = speciesData.genusKey1 ?? default;
                _genusMPH2 = speciesData.genusKey2 ?? default;
                _speciesMPH1 = speciesData.speciesKey1 ?? default;
                _speciesMPH2 = speciesData.speciesKey2 ?? default;
                _catchCompositionRecordCount = Names.CatchCompositionRecordCount(_nameGuid);
                labelRecordCount.Text = _catchCompositionRecordCount.ToString();

                if (_catchCompositionRecordCount == 0)
                {
                    buttonEdit.Text = "Delete";
                }
            }
            else
            {
                labelRecordCount.Text = "0";
                _nameGuid = Guid.NewGuid().ToString();

                if (_genus.Length > 0 && _species.Length > 0)
                {
                    var speciesFishBaseData = Names.NameInFishBaseEx(_genus, _species);
                    _inFishBase = chkInFishbase.Checked = speciesFishBaseData.inFishBase;
                    _fishBaseSpeciesNumber = speciesFishBaseData.fishBaseSpeciesNo;

                    if (chkInFishbase.Checked) cboTaxa.Text = "Fish";

                    var metaPhone = new DoubleMetaphoneShort();
                    metaPhone.ComputeMetaphoneKeys(_genus, out short k1, out short k2);
                    _genusMPH1 = k1;
                    _genusMPH2 = k2;

                    metaPhone.ComputeMetaphoneKeys(_species, out k1, out k2);
                    _speciesMPH1 = k1;
                    _speciesMPH2 = k2;

                    var SimilarSoundingList = Names.RetrieveSpeciesWithSimilarMetaPhone(_genusMPH1, _genusMPH2, _speciesMPH1, _speciesMPH2);
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

                buttonEdit.Enabled = false;
            }
            if (ReadOnly)
            {
                buttonCancel.Visible = false;
                buttonOK.Text = "Close";
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
            var cancel = _genus.Length == 0 || _species.Length == 0 || (cboTaxa.Text.Length == 0);
            if (cancel)
            {
                MessageBox.Show("Genus, species, and taxa are required", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return !cancel;
        }

        private void Onbutton_Click(object sender, EventArgs e)
        {
            if (ReadOnly)
            {
                Close();
            }
            else
            {
                var btn = (Button)sender;
                switch (btn.Name)
                {
                    case "buttonOK":
                        if (ValidateForm())
                        {
                            var willClose = false;
                            if (_dataStatus == fad3DataStatus.statusFromDB)
                            {
                                willClose = true;
                            }
                            else
                            {
                                if (Names.UpdateSpeciesData(_dataStatus, _nameGuid, _genus, _species, _taxa,
                                    _genusMPH1, _genusMPH2, _speciesMPH1, _speciesMPH2, _inFishBase, _fishBaseSpeciesNumber, _notes))
                                {
                                    willClose = true;
                                }
                            }

                            if (_parentForm.GetType().Name != "AllSpeciesForm")
                            {
                                if (_dataStatus == fad3DataStatus.statusNew)
                                {
                                    ((CatchCompositionForm)_parentForm).NewName(Accepted: true, _genus, _species, _nameGuid);
                                }
                                else
                                {
                                    ((CatchCompositionForm)_parentForm).NewName(Accepted: true, _genus, _species);
                                }
                            }

                            if (willClose) Close();
                        }
                        break;

                    case "buttonCancel":
                        if (_parentForm.GetType().Name != "AllSpeciesForm") ((CatchCompositionForm)_parentForm).NewName(Accepted: false);
                        Close();
                        break;

                    case "buttonEdit":
                        if (btn.Text == "Edit")
                        {
                        }
                        else if (btn.Text == "Delete")
                        {
                        }
                        break;
                }
            }
        }

        private void OnTextBoxes_Validating(object sender, CancelEventArgs e)
        {
            var msg = "";
            var o = (TextBox)sender;

            switch (o.Name)
            {
                case "txtGenus":
                case "txtSpecies":
                    if (o.Text.Length < 2 && o.Text.Length > 0)
                    {
                        msg = "Name is too short";
                        e.Cancel = true;
                    }
                    else
                    {
                        var metaPhone = new DoubleMetaphoneShort();
                        if (o.Name == "txtGenus")
                        {
                            _genus = o.Text;
                            metaPhone.ComputeMetaphoneKeys(_genus, out short k1, out short k2);
                            _genusMPH1 = k1;
                            _genusMPH2 = k2;
                        }
                        else
                        {
                            _species = o.Text;
                            metaPhone.ComputeMetaphoneKeys(_species, out short k1, out short k2);
                            _speciesMPH1 = k1;
                            _speciesMPH2 = k2;
                        }
                    }
                    break;

                case "txtNotes":
                    _notes = o.Text;
                    break;
            }

            if (!e.Cancel && _dataStatus != fad3DataStatus.statusNew)
            {
                _dataStatus = o.Text != _genus ? fad3DataStatus.statusEdited : default;
            }
            else if (e.Cancel)
            {
                MessageBox.Show(msg, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            o = null;
        }

        private void OncboTaxa_Validating(object sender, CancelEventArgs e)
        {
            var o = (ComboBox)sender;
            if (_dataStatus != fad3DataStatus.statusNew)
            {
                _dataStatus = o.Text != _taxaName ? fad3DataStatus.statusEdited : default;
            }
            _taxaName = o.Text;
            _taxa = CatchName.TaxaFromTaxaName(_taxaName);

            if (_taxa == Taxa.Fish)
            {
                var fbData = Names.NameInFishBaseEx(_genus, _species);
                chkInFishbase.Checked = _inFishBase = fbData.inFishBase;
                _fishBaseSpeciesNumber = fbData.fishBaseSpeciesNo;
            }
            else
            {
                _inFishBase = false;
                _fishBaseSpeciesNumber = null;
                chkInFishbase.Checked = false;
            }

            o = null;
        }
    }
}
using FAD3.Database.Classes;
using System;
using System.Windows.Forms;

namespace FAD3.Database.Forms
{
    public partial class ExportImportDialogForm : Form
    {
        private ExportImportDataType _dataType;
        private ExportImportDeleteAction _action;
        public string TaxaCSV { get; set; }
        public ExportImportDataType Selection { get; internal set; }
        public bool ExportSelectedTaxa { get; internal set; }

        public ExportImportDialogForm(ExportImportDataType exportDataType, ExportImportDeleteAction action)
        {
            InitializeComponent();
            _dataType = exportDataType;
            _action = action;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            rdButton.Visible = false;
            RadioButton rdButton1 = new RadioButton();
            RadioButton rdButton2 = new RadioButton();
            RadioButton rdButton3 = new RadioButton();
            CheckBox chkBox = new CheckBox();
            var spacing = 4;
            switch (_dataType)
            {
                case ExportImportDataType.SpeciesNames:
                    rdButton.Visible = true;
                    rdButton.Text = "Export species names of catch";
                    if (_action == ExportImportDeleteAction.ActionImport)
                    {
                        rdButton.Text = "Import species names of catch";
                    }

                    rdButton.Tag = ExportImportDataType.SpeciesNames;
                    if (TaxaCSV?.Length > 0)
                    {
                        chkBox.Name = "chkExportSelected";
                        chkBox.Text = "Export selected taxa only";
                        chkBox.Left = rdButton.Left;
                        chkBox.AutoSize = true;
                        chkBox.Top = rdButton.Top + rdButton.Height + spacing;
                        chkBox.Enabled = false;
                        Controls.Add(chkBox);
                    }
                    break;

                case ExportImportDataType.CatchLocalNames:
                    break;

                case ExportImportDataType.GearsVariation:
                    break;

                case ExportImportDataType.LocalNameLanguages:
                    break;

                case ExportImportDataType.CatchLocalNameSpeciesNamePair:

                    break;

                case ExportImportDataType.GearInventoryDataSelect:
                    var enumeratorCaption = "Export enumerators";
                    var inventoryCaption = "Export fishery inventory";
                    if (_action == ExportImportDeleteAction.ActionImport)
                    {
                        enumeratorCaption = "Import enumerators";
                        inventoryCaption = "Import fishery inventory";
                    }

                    rdButton.Text = enumeratorCaption;
                    rdButton.Tag = ExportImportDataType.Enumerators;
                    rdButton.Visible = true;

                    rdButton1.Text = inventoryCaption;
                    rdButton1.Left = rdButton.Left;
                    rdButton1.Top = rdButton.Top + rdButton.Height + spacing;
                    rdButton1.Tag = ExportImportDataType.GearInventory;
                    rdButton1.AutoSize = true;
                    Controls.Add(rdButton1);
                    break;

                case ExportImportDataType.GearNameDataSelect:
                    var gearVarCaption = "Export gear variations";
                    var gearLocalNameCaption = "Export gear local names";
                    var gearClassCaption = "Export gear classes";
                    var gearRefCodeCaption = "Export gear codes";
                    if (_action == ExportImportDeleteAction.ActionImport)
                    {
                        gearVarCaption = "Import gear variations";
                        gearLocalNameCaption = "Import gear local names";
                        gearClassCaption = "Import gear classes";
                        gearRefCodeCaption = "Import gear codes";
                    }
                    rdButton.Text = gearVarCaption;
                    rdButton.Tag = ExportImportDataType.GearsVariation;
                    rdButton.Visible = true;

                    rdButton1.Text = gearLocalNameCaption;
                    rdButton1.Left = rdButton.Left;
                    rdButton1.Top = rdButton.Top + rdButton.Height + spacing;
                    rdButton1.Tag = ExportImportDataType.GearsLocalName;
                    rdButton1.AutoSize = true;
                    Controls.Add(rdButton1);

                    rdButton2.Text = gearClassCaption;
                    rdButton2.Left = rdButton.Left;
                    rdButton2.Top = rdButton1.Top + rdButton1.Height + spacing;
                    rdButton2.Tag = ExportImportDataType.GearsClass;
                    rdButton2.AutoSize = true;
                    Controls.Add(rdButton2);

                    rdButton3.Text = gearRefCodeCaption;
                    rdButton3.Left = rdButton.Left;
                    rdButton3.Top = rdButton2.Top + rdButton2.Height + spacing;
                    rdButton3.Tag = ExportImportDataType.GearsRefCode;
                    rdButton3.AutoSize = true;
                    Controls.Add(rdButton3);

                    break;

                case ExportImportDataType.CatchNametDataSelect:
                    var languageCaption = "Export languages";
                    var localNamesCaption = "Export local names";
                    var namePairCaption = "Export local name - scientific name pairs";
                    var allDataCaption = "Export all";
                    if (_action == ExportImportDeleteAction.ActionImport)
                    {
                        languageCaption = "Import languages";
                        localNamesCaption = "Import local names";
                        namePairCaption = "Import local name - scientific name pairs";
                        allDataCaption = "Import all";
                    }
                    rdButton.Left = 10;
                    rdButton.Text = languageCaption;
                    rdButton.Tag = ExportImportDataType.LocalNameLanguages;
                    rdButton.Visible = true;

                    rdButton1.Text = localNamesCaption;
                    rdButton1.Left = rdButton.Left;
                    rdButton1.Top = rdButton.Top + rdButton.Height + spacing;
                    rdButton1.Tag = ExportImportDataType.CatchLocalNames;
                    rdButton1.AutoSize = true;
                    Controls.Add(rdButton1);

                    rdButton2.Text = namePairCaption;
                    rdButton2.Left = rdButton.Left;
                    rdButton2.Top = rdButton1.Top + rdButton1.Height + spacing;
                    rdButton2.Tag = ExportImportDataType.CatchLocalNameSpeciesNamePair;
                    rdButton2.AutoSize = true;
                    Controls.Add(rdButton2);

                    rdButton3.Text = allDataCaption;
                    rdButton3.Left = rdButton.Left;
                    rdButton3.Top = rdButton2.Top + rdButton2.Height + spacing;
                    rdButton3.Tag = ExportImportDataType.CatchNameAll;
                    rdButton3.AutoSize = true;
                    Controls.Add(rdButton3);
                    break;
            }
            rdButton.CheckedChanged += OnCheckChanged;
            chkBox.CheckedChanged += OnCheckChanged;
            if (_action == ExportImportDeleteAction.ActionImport)
            {
                Text = "Import";
            }
            else
            {
                Text = "Export";
            }

            //adjust form and button depending on position of last radio button
            int lastRowButton = 0;
            foreach (Control c in Controls)
            {
                if (c.GetType().Name == "RadioButton" && c.Location.Y > lastRowButton)
                {
                    lastRowButton = c.Location.Y;
                }
            }

            btnCancel.Top = lastRowButton + 40;
            btnOk.Top = lastRowButton + 40;
            Height = btnOk.Top + btnOk.Height + 40;
        }

        private void OnCheckChanged(object sender, EventArgs e)
        {
            if (sender.GetType().Name == "RadioButton")
            {
                RadioButton rdb = (RadioButton)sender;
                switch (rdb.Name)
                {
                    case "rdButton":
                        if (_dataType == ExportImportDataType.SpeciesNames)
                        {
                            try
                            {
                                Controls["chkExportSelected"].Enabled = rdButton.Checked;
                            }
                            catch
                            {
                                //ignore
                            }
                        }
                        break;
                }
            }
            else if (sender.GetType().Name == "CheckBox")
            {
            }
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    Selection = ExportImportDataType.None;
                    foreach (Control c in Controls)
                    {
                        if (c.GetType().Name == "RadioButton" && c.Tag != null && ((RadioButton)c).Checked)
                        {
                            Selection |= (ExportImportDataType)c.Tag;
                        }
                    }
                    if (_action == ExportImportDeleteAction.ActionExport && _dataType == ExportImportDataType.SpeciesNames)
                    {
                        if (Controls.Contains((CheckBox)Controls["chkExportSelected"]))
                        {
                            ExportSelectedTaxa = ((CheckBox)Controls["chkExportSelected"]).Checked;
                        }
                    }
                    DialogResult = DialogResult.OK;
                    break;

                case "btnCancel":
                    DialogResult = DialogResult.Cancel;
                    break;
            }
        }
    }
}
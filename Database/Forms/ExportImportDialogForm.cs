using System;
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
    public partial class ExportImportDialogForm : Form
    {
        private ExportImportDataType _dataType;
        private ExportImportAction _action;
        public string TaxaCSV { get; set; }
        public ExportImportDataType Selection { get; internal set; }
        public bool ExportSelectedTaxa { get; internal set; }

        public ExportImportDialogForm(ExportImportDataType exportDataType, ExportImportAction action)
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
            CheckBox chkBox = new CheckBox();
            var spacing = 4;
            switch (_dataType)
            {
                case ExportImportDataType.SpeciesNames:
                    rdButton.Visible = true;
                    rdButton.Text = "Export species names of catch";
                    if (_action == ExportImportAction.ActionImport)
                    {
                        rdButton.Text = "Import species names of catch";
                    }

                    rdButton.Tag = ExportImportDataType.SpeciesNames;
                    if (TaxaCSV?.Length > 0)
                    {
                        //checkBox2 = new CheckBox();
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

                case ExportImportDataType.GearNameDataSelect:
                    var gearVarCaption = "Export gear variations";
                    var gearLocalNameCaption = "Export gear local names";
                    var gearClassCaption = "Export gear classes";
                    if (_action == ExportImportAction.ActionImport)
                    {
                        gearVarCaption = "Import gear variations";
                        gearLocalNameCaption = "Import gear local names";
                        gearClassCaption = "Import gear classes";
                    }
                    rdButton.Top = 20;
                    rdButton.Text = gearVarCaption;
                    rdButton.Tag = ExportImportDataType.GearsVariation;
                    rdButton.Visible = true;

                    //checkBox2 = new CheckBox();
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

                    break;

                case ExportImportDataType.CatchNametDataSelect:
                    var languageCaption = "Export languages";
                    var localNamesCaption = "Export local names";
                    var namePairCaption = "Export local name - scientific name pairs";
                    if (_action == ExportImportAction.ActionImport)
                    {
                        languageCaption = "Import languages";
                        localNamesCaption = "Import local names";
                        namePairCaption = "Import local name - scientific name pairs";
                    }
                    rdButton.Top = 20;
                    rdButton.Left = 10;
                    rdButton.Text = languageCaption;
                    rdButton.Tag = ExportImportDataType.LocalNameLanguages;
                    rdButton.Visible = true;

                    //checkBox2 = new CheckBox();
                    rdButton1.Text = localNamesCaption;
                    rdButton1.Left = rdButton.Left;
                    rdButton1.Top = rdButton.Top + rdButton.Height + spacing;
                    rdButton1.Tag = ExportImportDataType.CatchLocalNames;
                    rdButton1.AutoSize = true;
                    Controls.Add(rdButton1);

                    //checkBox3 = new CheckBox();
                    rdButton2.Text = namePairCaption;
                    rdButton2.Left = rdButton.Left;
                    rdButton2.Top = rdButton1.Top + rdButton1.Height + spacing;
                    rdButton2.Tag = ExportImportDataType.CatchLocalNameSpeciesNamePair;
                    rdButton2.AutoSize = true;
                    Controls.Add(rdButton2);
                    break;
            }
            rdButton.CheckedChanged += OnCheckChanged;
            chkBox.CheckedChanged += OnCheckChanged;
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
                    if (_action == ExportImportAction.ActionExport && _dataType == ExportImportDataType.SpeciesNames)
                    {
                        if (Controls.Contains((CheckBox)Controls["chkExportSelected"]))
                        {
                            ExportSelectedTaxa = ((CheckBox)Controls["chkExportSelected"]).Checked;
                        }
                    }
                    DialogResult = DialogResult.OK;
                    //Close();
                    break;

                case "btnCancel":
                    DialogResult = DialogResult.Cancel;
                    //Close();
                    break;
            }
        }
    }
}
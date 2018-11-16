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
            checkBox1.Visible = false;
            CheckBox checkBox2 = new CheckBox();
            CheckBox checkBox3 = new CheckBox();
            var spacing = 4;
            switch (_dataType)
            {
                case ExportImportDataType.SpeciesNames:
                    checkBox1.Visible = true;
                    checkBox1.Text = "Export species names of catch";
                    if (_action == ExportImportAction.ActionImport)
                    {
                        checkBox1.Text = "Import species names of catch";
                    }

                    checkBox1.Tag = ExportImportDataType.SpeciesNames;
                    if (TaxaCSV.Length > 0)
                    {
                        //checkBox2 = new CheckBox();
                        checkBox2.Name = "chkExportSelected";
                        checkBox2.Text = "Export selected taxa only";
                        checkBox2.Left = checkBox1.Left;
                        checkBox2.AutoSize = true;
                        checkBox2.Top = checkBox1.Top + checkBox1.Height + spacing;
                        checkBox2.Enabled = false;
                        Controls.Add(checkBox2);
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
                    if (_action == ExportImportAction.ActionImport)
                    {
                        gearVarCaption = "Import gear variations";
                        gearLocalNameCaption = "Import gear local names";
                    }

                    checkBox1.Text = gearVarCaption;
                    checkBox1.Tag = ExportImportDataType.GearsVariation;
                    checkBox1.Visible = true;

                    //checkBox2 = new CheckBox();
                    checkBox2.Text = gearLocalNameCaption;
                    checkBox2.Left = checkBox1.Left;
                    checkBox2.Top = checkBox1.Top + checkBox1.Height + spacing;
                    checkBox2.Tag = ExportImportDataType.GearsLocalName;
                    checkBox2.AutoSize = true;
                    Controls.Add(checkBox2);
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

                    checkBox1.Text = languageCaption;
                    checkBox1.Tag = ExportImportDataType.LocalNameLanguages;
                    checkBox1.Visible = true;

                    //checkBox2 = new CheckBox();
                    checkBox2.Text = localNamesCaption;
                    checkBox2.Left = checkBox1.Left;
                    checkBox2.Top = checkBox1.Top + checkBox1.Height + spacing;
                    checkBox2.Tag = ExportImportDataType.CatchLocalNames;
                    checkBox2.AutoSize = true;
                    Controls.Add(checkBox2);

                    //checkBox3 = new CheckBox();
                    checkBox3.Text = namePairCaption;
                    checkBox3.Left = checkBox1.Left;
                    checkBox3.Top = checkBox2.Top + checkBox2.Height + spacing;
                    checkBox3.Tag = ExportImportDataType.CatchLocalNameSpeciesNamePair;
                    checkBox3.AutoSize = true;
                    Controls.Add(checkBox3);
                    break;
            }
            checkBox1.CheckedChanged += OnCheckBoxCheckChanged;
        }

        private void OnCheckBoxCheckChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            switch (chk.Name)
            {
                case "checkBox1":
                    if (_dataType == ExportImportDataType.SpeciesNames)
                    {
                        try
                        {
                            Controls["chkExportSelected"].Enabled = checkBox1.Checked;
                        }
                        catch
                        {
                            //ignore
                        }
                    }
                    break;
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
                        if (c.GetType().Name == "CheckBox" && c.Tag != null && ((CheckBox)c).Checked)
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
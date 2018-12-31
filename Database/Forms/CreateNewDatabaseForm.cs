using dao;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FAD3
{
    public partial class CreateNewDatabaseForm : Form
    {
        private string _newMDBFile = "";
        private MainForm _parentForm;
        private List<string> _requiredTables = new List<string>();

        public CreateNewDatabaseForm(MainForm parent)
        {
            InitializeComponent();
            _parentForm = parent;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            global.LoadFormSettings(this, true);
            group2.Visible = false;
            Size = new Size(group1.Width + (group1.Left * 5), Height);
        }

        private void Onbutton_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonOk":
                    if (_newMDBFile.Length > 0)
                    {
                        if (File.Exists(_newMDBFile))
                        {
                            MessageBox.Show($"{_newMDBFile} is an existing file. Delete this file first befor being able to use this filename",
                                              "Cannot use this file",
                                              MessageBoxButtons.OK,
                                              MessageBoxIcon.Information);
                            Close();
                        }
                        else
                        {
                            File.Copy(global.TemplateMDBFile, _newMDBFile);
                            if (UpdateNewMDB())
                            {
                                _parentForm.MRUList.AddFile(_newMDBFile);
                                _parentForm.NewDBFile(_newMDBFile);
                                MessageBox.Show("New database is ready");
                                this.Close();
                            }
                        }
                    }
                    break;

                case "buttonCancel":
                    Close();
                    break;

                case "buttonSelectOK":
                    break;

                case "buttonFileName":
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Title = "Provide filename of new database";
                    if (global.MDBPath.Length == 0)
                    {
                        sfd.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
                    }
                    else
                    {
                        sfd.InitialDirectory = global.MDBPath;
                    }
                    //ofd.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
                    sfd.Filter = "Microsoft Access Data File (.mdb)|*.mdb";
                    sfd.ShowDialog();
                    _newMDBFile = sfd.FileName;

                    if (_newMDBFile.Length > 0)
                    {
                        group2.Visible = true;
                        group1.Visible = false;
                        group2.Location = group1.Location;
                        label2.Text = "Step 2";

                        checkAOI.Checked = true;
                        checkLandingSites.Checked = true;
                        checkGearVar.Checked = true;
                        checkGearLocalNames.Checked = true;
                        checkSciNames.Checked = true;
                        checkFishLocalNames.Checked = true;
                        checkEnumerators.Checked = true;
                        checkInventory.Checked = true;

                        _requiredTables.Add("FBSpecies");
                        _requiredTables.Add("Provinces");
                        _requiredTables.Add("Municipalities");
                        _requiredTables.Add("tblGearClass");
                        _requiredTables.Add("tblTaxa");
                    }
                    break;
            }
        }

        private string TableFieldList(string tableName)
        {
            var dbe = new DBEngine();
            var dbTemplate = dbe.OpenDatabase(global.TemplateMDBFile);
            var fieldList = string.Empty;
            foreach (Field f in dbTemplate.TableDefs[tableName].Fields)
            {
                fieldList += $"{f.Name}, ";
            }
            fieldList = fieldList.Trim(new char[] { ',', ' ' });
            dbTemplate.Close();
            dbTemplate = null;
            return fieldList;
        }

        private bool UpdateNewMDB()
        {
            var dbe = new DBEngine();
            var dbData = dbe.OpenDatabase(global.MDBPath);
            var dbTemplate = dbe.OpenDatabase(global.TemplateMDBFile);
            var sql = "";
            var qd = new dao.QueryDef();
            foreach (var item in _requiredTables)
            {
                //sql = $"Insert Into {item} In '{_newMDBFile}' Select * from {item}";
                sql = $"Insert Into {item} In '{_newMDBFile}' Select {TableFieldList(item)} from {item}";
                // MessageBox.Show(sql);
                qd = dbData.CreateQueryDef("", sql);
                qd.Execute();

                //qd.Close();
                //qd = null;
            }

            if (checkAOI.Checked)
            {
                sql = $"Insert Into tblAOI In '{_newMDBFile}' Select {TableFieldList("tblAOI")} from tblAOI";
                qd = dbData.CreateQueryDef("", sql);
                qd.Execute();

                sql = $"Insert Into tblAdditionalAOIExtent In '{_newMDBFile}' Select {TableFieldList("tblAdditionalAOIExtent")} from tblAdditionalAOIExtent";
                qd = dbData.CreateQueryDef("", sql);
                qd.Execute();
            }

            if (checkAOI.Checked && checkLandingSites.Checked)
            {
                sql = $"Insert Into tblLandingSites In '{_newMDBFile}' Select {TableFieldList("tblLandingSites")} from tblLandingSites";
                qd = dbData.CreateQueryDef("", sql);
                qd.Execute();
            }

            if (checkGearLocalNames.Checked)
            {
                sql = $"Insert Into tblGearLocalNames In '{_newMDBFile}' Select {TableFieldList("tblGearLocalNames")} from tblGearLocalNames";
                qd = dbData.CreateQueryDef("", sql);
                qd.Execute();
            }

            if (checkGearVar.Checked)
            {
                sql = $"Insert Into tblGearVariations In '{_newMDBFile}' Select  {TableFieldList("tblGearVariations")} from tblGearVariations";
                qd = dbData.CreateQueryDef("", sql);
                qd.Execute();

                sql = $"Insert Into tblRefGearCodes In '{_newMDBFile}' Select {TableFieldList("tblRefGearCodes")} from tblRefGearCodes";
                qd = dbData.CreateQueryDef("", sql);
                qd.Execute();

                sql = $"Insert Into tblGearSpecs In '{_newMDBFile}' Select {TableFieldList("tblGearSpecs")} from tblGearSpecs where Version = '2'";
                qd = dbData.CreateQueryDef("", sql);
                qd.Execute();

                if (checkAOI.Checked)
                {
                    sql = $"Insert Into tblRefGearCodes_Usage In '{_newMDBFile}' Select {TableFieldList("tblRefGearCodes_Usage")} from tblRefGearCodes_Usage";
                    qd = dbData.CreateQueryDef("", sql);
                    qd.Execute();

                    if (checkGearLocalNames.Checked)
                    {
                        sql = $"Insert Into tblRefGearUsage_LocalName In '{_newMDBFile}' Select {TableFieldList("tblRefGearUsage_LocalName")} from tblRefGearUsage_LocalName";
                        qd = dbData.CreateQueryDef("", sql);
                        qd.Execute();
                    }
                }
            }

            if (checkSciNames.Checked)
            {
                sql = $"Insert Into tblAllSpecies In '{_newMDBFile}' Select {TableFieldList("tblAllSpecies")} from tblAllSpecies";
                qd = dbData.CreateQueryDef("", sql);
                qd.Execute();
            }

            if (checkFishLocalNames.Checked)
            {
                sql = $"Insert Into tblBaseLocalNames In '{_newMDBFile}' Select {TableFieldList("tblBaseLocalNames")}  from tblBaseLocalNames";
                qd = dbData.CreateQueryDef("", sql);
                qd.Execute();
            }

            if (checkLocalNameToSpeciesName.Checked && checkFishLocalNames.Checked && checkSciNames.Checked)
            {
                sql = $"Insert Into tblLanguages In '{_newMDBFile}' Select {TableFieldList("tblLanguages")}  from tblLanguages";
                qd = dbData.CreateQueryDef("", sql);
                qd.Execute();

                sql = $"Insert Into tblLocalNamesScientific In '{_newMDBFile}' Select {TableFieldList("tblLocalNamesScientific")}  from tblLocalNamesScientific";
                qd = dbData.CreateQueryDef("", sql);
                qd.Execute();
            }

            if (checkAOI.Checked && checkEnumerators.Checked)
            {
                sql = $"Insert Into tblEnumerators In '{_newMDBFile}' Select {TableFieldList("tblEnumerators")} from tblEnumerators";
                qd = dbData.CreateQueryDef("", sql);
                qd.Execute();
            }

            if (checkAOI.Checked && checkGearLocalNames.Checked && checkFishLocalNames.Checked && checkInventory.Checked)
            {
                sql = $"Insert Into tblGearInventories In '{_newMDBFile}' Select {TableFieldList("tblGearInventories")} from tblGearInventories";
                qd = dbData.CreateQueryDef("", sql);
                qd.Execute();

                sql = $"Insert Into tblGearInventoryBarangay In '{_newMDBFile}' Select {TableFieldList("tblGearInventoryBarangay")} from tblGearInventoryBarangay";
                qd = dbData.CreateQueryDef("", sql);
                qd.Execute();

                sql = $"Insert Into tblGearInventoryBarangayData In '{_newMDBFile}' Select {TableFieldList("tblGearInventoryBarangayData")} from tblGearInventoryBarangayData";
                qd = dbData.CreateQueryDef("", sql);
                qd.Execute();

                sql = $"Insert Into tblGearInventoryCatchComposition In '{_newMDBFile}' Select {TableFieldList("tblGearInventoryCatchComposition")} from tblGearInventoryCatchComposition";
                qd = dbData.CreateQueryDef("", sql);
                qd.Execute();

                sql = $"Insert Into tblGearInventoryCPUEHistorical In '{_newMDBFile}' Select {TableFieldList("tblGearInventoryCPUEHistorical")} from tblGearInventoryCPUEHistorical";
                qd = dbData.CreateQueryDef("", sql);
                qd.Execute();

                sql = $"Insert Into tblGearInventoryGearLocalNames In '{_newMDBFile}' Select {TableFieldList("tblGearInventoryGearLocalNames")} from tblGearInventoryGearLocalNames";
                qd = dbData.CreateQueryDef("", sql);
                qd.Execute();

                sql = $"Insert Into tblGearInventoryMonthsUsed In '{_newMDBFile}' Select {TableFieldList("tblGearInventoryMonthsUsed")} from tblGearInventoryMonthsUsed";
                qd = dbData.CreateQueryDef("", sql);
                qd.Execute();

                sql = $"Insert Into tblGearInventoryPeakMonths In '{_newMDBFile}' Select {TableFieldList("tblGearInventoryPeakMonths")} from tblGearInventoryPeakMonths";
                qd = dbData.CreateQueryDef("", sql);
                qd.Execute();
            }

            qd.Close();
            qd = null;

            dbData.Close();
            dbTemplate.Close();
            dbData = null;
            dbTemplate = null;

            return true;
        }

        private void checkAOI_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkAOI.Checked)
            {
                checkLandingSites.Checked = false;
                checkEnumerators.Checked = false;
                checkInventory.Checked = false;

                checkLandingSites.Enabled = false;
                checkEnumerators.Enabled = false;
                checkInventory.Enabled = false;
            }
            else
            {
                checkLandingSites.Enabled = true;
                checkEnumerators.Enabled = true;
                checkInventory.Enabled = true;
            }

            if (!checkGearLocalNames.Checked)
            {
                checkInventory.Checked = false;
                checkInventory.Enabled = false;
            }
            else
            {
                if (checkInventory.Enabled) checkInventory.Enabled = true;
            }

            if (!checkFishLocalNames.Checked)
            {
                checkInventory.Checked = false;
                checkInventory.Enabled = false;
            }
            else
            {
                if (checkInventory.Enabled) checkInventory.Enabled = true;
            }
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            global.SaveFormSettings(this);
        }
    }
}
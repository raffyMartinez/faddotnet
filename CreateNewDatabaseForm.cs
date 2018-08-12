using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FAD3
{
    public partial class CreateNewDatabaseForm : Form
    {
        private string _newMDBFilename = "";
        private MainForm _parentForm;

        public CreateNewDatabaseForm(MainForm parent)
        {
            InitializeComponent();
            _parentForm = parent;
        }

        private void frmNewDB_Load(object sender, EventArgs e)
        {
            group2.Visible = false;
            Size = new Size(group1.Width + (group1.Left * 2), 407);
        }

        private void Onbutton_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonOK":
                    if (_newMDBFilename.Length > 0)
                    {
                        File.Copy(global.mdbPath, _newMDBFilename);
                        if (UpdateNewMDB())
                        {
                            _parentForm.MRUList.AddFile(_newMDBFilename);
                            _parentForm.NewDBFile(_newMDBFilename);
                            MessageBox.Show("New database is ready");
                            this.Close();
                        }
                    }
                    break;

                case "buttonCancel":
                    Close();
                    break;

                case "buttonFileName":
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Title = "Provide filename of new database";
                    if (global.mdbPath.Length == 0)
                    {
                        sfd.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
                    }
                    else
                    {
                        sfd.InitialDirectory = global.mdbPath;
                    }
                    //ofd.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
                    sfd.Filter = "Microsoft Access Data File (.mdb)|*.mdb";
                    sfd.ShowDialog();
                    _newMDBFilename = sfd.FileName;
                    if (_newMDBFilename.Length > 0)
                    {
                        group2.Visible = true;
                        group1.Visible = false;
                        group2.Location = group1.Location;
                        label2.Text = "Step 2";
                        checkAOI.Checked = true;
                        checkGearVar.Checked = true;
                        checkLandingSites.Checked = true;
                        checkLocalNames.Checked = true;
                        checkSciNames.Checked = true;
                        checkEnumerators.Checked = true;
                    }
                    break;
            }
        }

        private bool UpdateNewMDB()
        {
            List<string> tableNames = GetTableList();
            List<string> subTableList = new List<string>();
            bool Success = false;
            string ActionType = "";
            string sql = "";
            OleDbCommand update = new OleDbCommand();
            string myConnString = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + _newMDBFilename; ;
            using (OleDbConnection conn = new OleDbConnection(myConnString))
            {
                foreach (string item in tableNames)
                {
                    ActionType = "None";
                    switch (item)
                    {
                        case "Barangay":
                        case "FBSPecies":
                        case "Municipalities":
                        case "Provinces":
                        case "tblAllSpecies":
                        case "tblBaseLocalNames":
                        case "tblGearClass":
                        case "tblGearLocalNames":
                        case "tblTaxa":
                            break;

                        case "tblGearVariations":
                            if (checkGearVar.Checked == false)
                            {
                                ActionType = "DeleteTableContents";
                            }
                            break;

                        case "tblRefGearCodes":
                            if (checkGearVar.Checked == false)
                            {
                                ActionType = "DeleteTableContents";
                            }
                            break;

                        case "tblRefGearCodes_Usage":
                            if (checkGearVar.Checked == false)
                            {
                                ActionType = "DeleteTableContents";
                            }
                            break;

                        case "tblRefGearUsage_LocalName":
                            if (checkGearVar.Checked == false)
                            {
                                ActionType = "DeleteTableContents";
                            }
                            break;

                        case "tblLandingSites":
                            if (checkLandingSites.Checked == false)
                            {
                                subTableList.Add("tblGrid");
                                subTableList.Add("tblGMS");
                                subTableList.Add("tblLF");
                                subTableList.Add("tblSampledGearSpec");
                                subTableList.Add("tblSampledGearSpec2");
                                subTableList.Add("tblCatchDetail");
                                subTableList.Add("tblCatchComp");
                                subTableList.Add("tblSampling");
                                subTableList.Add("tblLandingSiteEnumerators");
                                ActionType = "DeleteTableContents";
                            }
                            break;

                        case "tblAOI":
                            if (checkAOI.Checked == false)
                            {
                                subTableList.Add("tblGrid");
                                subTableList.Add("tblGMS");
                                subTableList.Add("tblLF");
                                subTableList.Add("tblSampledGearSpec");
                                subTableList.Add("tblSampledGearSpec2");
                                subTableList.Add("tblCatchDetail");
                                subTableList.Add("tblCatchComp");
                                subTableList.Add("tblSampling");
                                subTableList.Add("tblLandingSiteEnumerators");
                                subTableList.Add("tblEnumeratorRating");
                                subTableList.Add("tblEnumerators");
                                subTableList.Add("tblLandingSites");
                                subTableList.Add("tblTFStations");
                                subTableList.Add("tblAOI_GearLocalNames");
                                subTableList.Add("tblAOIGridSize");
                                ActionType = "DeleteTableContents";
                            }
                            break;

                        case "tblAOI_GearLocalNames":
                            if (checkAOI.Checked == false)
                            {
                                ActionType = "DeleteTableContents";
                            }
                            break;

                        case "tblEnumerators":
                            if (checkEnumerators.Checked == false)
                            {
                                subTableList.Add("tblGrid");
                                subTableList.Add("tblGMS");
                                subTableList.Add("tblLF");
                                subTableList.Add("tblSampledGearSpec");
                                subTableList.Add("tblSampledGearSpec2");
                                subTableList.Add("tblCatchDetail");
                                subTableList.Add("tblCatchComp");
                                subTableList.Add("tblSampling");
                                subTableList.Add("tblLandingSiteEnumerators");
                                subTableList.Add("tblEnumeratorRating");
                                ActionType = "DeleteTableContents";
                            }
                            break;

                        case "tblAOIGridSize":
                            if (checkAOI.Checked == false)
                            {
                                ActionType = "DeleteTableContents";
                            }
                            break;

                        case "tblTFStations":
                        case "tblTransactions":
                        case "tblCategories":
                        case "tblDescriptors":
                        case "tblEditingNotes":
                        case "tblEnumeratorRating":
                        case "tblRefCodeCounter":
                        case "tblGridCell":
                        case "tblLandingSiteEnumerators":
                        case "LNVars":
                        case "tblLocalNamesByTA":
                        case "tblMonthNames":
                            ActionType = "DeleteTableContents";
                            break;

                        case "tblSampling":
                            subTableList.Add("tblGrid");
                            subTableList.Add("tblGMS");
                            subTableList.Add("tblLF");
                            subTableList.Add("tblSampledGearSpec");
                            subTableList.Add("tblSampledGearSpec2");
                            subTableList.Add("tblCatchDetail");
                            subTableList.Add("tblCatchComp");
                            ActionType = "DeleteTableContents";
                            break;

                        case "Analysis":
                        case "temp_ComputedWt":
                        case "temp_PrecalcWt":
                        case "temp_toc":
                        case "temp_toc_TF":
                            ActionType = "DeleteTable";
                            break;

                        case "tblCatchComp":
                        case "tblCatchDetail":
                        case "tblLF":
                        case "tblGMS":
                        case "tblGrid":
                        case "tblSampledGearSpec":
                        case "tblSampledGearSpec2":
                        default:
                            break;
                    }

                    if (subTableList.Count > 0)
                    {
                        DeleteSubTables(subTableList);
                        subTableList.Clear();
                    }

                    if (ActionType == "DeleteTableContents")
                    {
                        sql = "Delete * from " + item;
                    }
                    else if (ActionType == "DeleteTable")
                    {
                        sql = "DROP TABLE " + item;
                    }

                    if (ActionType != "None")
                    {
                        try
                        {
                            update.CommandText = sql;
                            update.Connection = conn;
                            conn.Open();
                            update.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(ex);
                        }
                        conn.Close();
                        Success = true;
                    }
                }
            }

            return Success;
        }

        private void DeleteSubTables(List<string> TableList)
        {
            OleDbCommand update = new OleDbCommand();
            string sql = "";
            string myConnString = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + _newMDBFilename; ;
            using (OleDbConnection conn = new OleDbConnection(myConnString))
            {
                try
                {
                    foreach (string item in TableList)
                    {
                        sql = "Delete * from " + item;
                        update.CommandText = sql;
                        update.Connection = conn;
                        conn.Open();
                        update.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
        }

        private List<string> GetTableList()
        {
            List<string> myList = new List<string>();
            DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.OleDb");
            DataTable userTables = null;

            using (DbConnection connection = factory.CreateConnection())
            {
                try
                {
                    // c:\test\test.mdb
                    connection.ConnectionString = global.ConnectionString;
                    // We only want user tables, not system tables
                    string[] restrictions = new string[4];
                    restrictions[3] = "Table";

                    connection.Open();

                    // Get list of user tables
                    userTables = connection.GetSchema("Tables", restrictions);

                    for (int i = 0; i < userTables.Rows.Count; i++)
                        myList.Add(userTables.Rows[i][2].ToString());
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
            return myList;
        }

        private void checkAOI_CheckedChanged(object sender, EventArgs e)
        {
            if (checkAOI.Checked == false)
            {
                checkLandingSites.Checked = false;
                checkEnumerators.Checked = false;
            }
        }
    }
}
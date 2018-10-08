using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;

namespace FAD3.Mapping.Forms
{
    public partial class ChlorophyllForm : Form
    {
        private string _connString;
        private static ChlorophyllForm _instance;
        private string _excelFileName;

        public static ChlorophyllForm GetInstance()
        {
            if (_instance == null) return new ChlorophyllForm();
            return _instance;
        }

        public ChlorophyllForm()
        {
            InitializeComponent();
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            btnOk.Enabled = false;
            btnReadSheet.Enabled = false;
        }

        private void OpenFile()
        {
            var fileOpen = new OpenFileDialog
            {
                Title = "Open MS Excel file",
                Filter = "Excel file|*.xls;*.xlsx|All file types|*.*",
                FilterIndex = 1
            };
            fileOpen.ShowDialog();
            if (fileOpen.FileName.Length > 0 && File.Exists(fileOpen.FileName))
            {
                _excelFileName = fileOpen.FileName;
                txtFile.Text = _excelFileName;
            }
        }

        private void ReadExcelFile()
        {
            var header = "HDR=NO";
            if (chkHasHeader.Checked)
            {
                header = "HDR=YES";
            }
            _connString = $@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={_excelFileName};Extended Properties=""Excel 8.0;{header}""";
            OleDbConnection connection = new OleDbConnection(_connString);
            connection.Open();
            DataTable dtTables = new DataTable();
            dtTables = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (dtTables != null)
            {
                foreach (DataRow r in dtTables.Rows)
                {
                    listSheets.Items.Add(r["TABLE_NAME"].ToString());
                }
            }

            //DataSet dsData = new DataSet();
            //string command = "Select * from [Sheet1$]";

            //OleDbDataAdapter adaptor = new OleDbDataAdapter(command, connection);
        }

        private void ReadSheet()
        {
            OleDbConnection connection = new OleDbConnection(_connString);
            connection.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = connection;
            cmd.CommandText = "Select * from [" + listSheets.Text + "]";
            DataTable excelTable = new DataTable();
            excelTable.TableName = listSheets.Text;

            var adapter = new OleDbDataAdapter();
            adapter.SelectCommand = cmd;

            // adapter.FillSchema(ds
            adapter.Fill(excelTable);

            DataSet ds = new DataSet();
            ds.Tables.Add(excelTable);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
            }

            txtRows.Text = ds.Tables[0].Rows.Count.ToString();
            cboLatitude.Items.Clear();
            cboLongitude.Items.Clear();
            for (int n = 0; n < ds.Tables[0].Columns.Count; n++)
            {
                if (n == 0)
                {
                    cboLatitude.Items.Add(ds.Tables[0].Columns[n].ColumnName);
                    cboLongitude.Items.Add(ds.Tables[0].Columns[n].ColumnName);
                }
                else if (n == 1)
                {
                    cboLatitude.Items.Add(ds.Tables[0].Columns[n].ColumnName);
                    cboLongitude.Items.Add(ds.Tables[0].Columns[n].ColumnName);
                }
                else
                {
                    cboFirstData.Items.Add(ds.Tables[0].Columns[n].ColumnName);
                    cboLastData.Items.Add(ds.Tables[0].Columns[n].ColumnName);
                }
            }
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    break;

                case "btnCancel":
                    Close();
                    break;

                case "btnOpen":
                    OpenFile();
                    break;

                case "btnReadSheet":
                    ReadSheet();
                    break;

                case "btnReadWorkbook":
                    ReadExcelFile();
                    break;
            }
        }

        private void OnListBoxClick(object sender, EventArgs e)
        {
            btnReadSheet.Enabled = true;
        }
    }
}
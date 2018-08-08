/*
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/13/2016
 * Time: 11:50 AM
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using Microsoft.Win32;
using System.Collections.Generic;

namespace FAD3
{
    /// <summary>
    /// Description of frmEnumerator.
    /// </summary>
    public partial class EnumeratorForm : Form
    {
        private string _EnumeratorGUID = "";
        private bool _IsNew = false;
        private aoi _AOI = new aoi();
        private MainForm _parentForm;

        public aoi AOI
        {
            get { return _AOI; }
            set { _AOI = value; }
        }

        public new MainForm ParentForm
        {
            get { return _parentForm; }
            set { _parentForm = value; }
        }

        public void AddNew()
        {
            _IsNew = true;
            this.Text = "Add a new enumerator for " + _AOI.AOIName;
        }

        public EnumeratorForm()
        {
            //default conructor
            InitializeComponent();
            ConfigureListEnumerators();
            Text = "Landing site enumerators";
        }

        public EnumeratorForm(string EnumeratorGUID)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
            _EnumeratorGUID = EnumeratorGUID;
            SamplingEnumerators.EnumeratorGuid = _EnumeratorGUID;
            ConfigureListEnumeratorSamplings();
            Text = "Enumerator details";
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
        }

        private void Button2Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ConfigureListEnumerators()
        {
            labelListView.Visible = false;
            labelHireDate.Visible = false;
            labelEnumeratorName.Visible = false;

            txtHireDate.Visible = false;
            txtName.Visible = false;
            chkActive.Visible = false;

            Width = 450;

            lvEnumerators.With(o =>
            {
                o.Width = Width - 75;
                o.Location = new Point(10, 40);
                o.View = View.Details;
                o.FullRowSelect = true;
                o.Columns.Add("Enumerator name");
                o.Columns.Add("Date hired");
                o.Columns.Add("Active");
                o.Height = buttonOK.Top - o.Top - 10;
            });

            Button btnAdd = new Button
            {
                Width = 30,
                Height = 30,
                Text = "+",
                Location = new Point(lvEnumerators.Left + lvEnumerators.Width + 10, lvEnumerators.Top)
            };
            Button btnRemove = new Button
            {
                Width = btnAdd.Width,
                Height = btnAdd.Height,
                Text = "-",
                Location = new Point(btnAdd.Left, btnAdd.Top + btnAdd.Height + 10)
            };

            btnAdd.Font = new Font(Font.FontFamily.Name, 14, FontStyle.Bold);
            btnRemove.Font = btnAdd.Font;

            Controls.Add(btnAdd);
            Controls.Add(btnRemove);
        }

        private void ConfigureListEnumeratorSamplings()
        {
            buttonCancel.Visible = false;

            labelListView.Visible = true;
            labelHireDate.Visible = true;
            labelEnumeratorName.Visible = true;

            txtHireDate.Visible = true;
            txtName.Visible = true;
            chkActive.Visible = true;

            lvEnumerators.View = View.Details;
            lvEnumerators.FullRowSelect = true;
            lvEnumerators.Columns.Add("Reference no.");
            lvEnumerators.Columns.Add("Landing site");
            lvEnumerators.Columns.Add("Gear used");
            lvEnumerators.Columns.Add("Date sampled");
            lvEnumerators.Columns.Add("Catch weight");
            lvEnumerators.Columns.Add("Catch rows");

            if (!_IsNew)
            {
                ReadData();
                var EnumeratorSamplings = SamplingEnumerators.GetEnumeratorSamplings();
                foreach (KeyValuePair<string,
                    (string RefNo, string LandingSite, string Gear, DateTime SamplingDate, double WtCatch, int Rows)>
                    kv in EnumeratorSamplings)
                {
                    var lvi = lvEnumerators.Items.Add(kv.Key, kv.Value.RefNo, null);
                    lvi.SubItems.Add(kv.Value.LandingSite);
                    lvi.SubItems.Add(kv.Value.Gear);
                    lvi.SubItems.Add(kv.Value.SamplingDate.ToString("MMM-dd-yyyy"));
                    lvi.SubItems.Add(kv.Value.WtCatch.ToString());
                    lvi.SubItems.Add(kv.Value.Rows.ToString());
                }
                Text = "Enumerator data and sampling list";
            }

            foreach (ColumnHeader c in lvEnumerators.Columns)
            {
                c.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            }

            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\FAD3\\ColWidth");
            try
            {
                string rv = rk.GetValue(global.lvContext.EnumeratorSampling.ToString(), "NULL").ToString();
                string[] arr = rv.Split(',');
                int i = 0;
                foreach (var item in lvEnumerators.Columns)
                {
                    ColumnHeader ch = (ColumnHeader)item;
                    ch.Width = Convert.ToInt32(arr[i]);
                    i++;
                }
            }
            catch
            {
                ErrorLogger.Log("Catch and effort column width not found in registry");
            }
        }

        private void FrmEnumeratorLoad(object sender, EventArgs e)
        {
        }

        private void ReadData()
        {
            DataTable dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = "Select * from tblEnumerators where EnumeratorID =\"" + _EnumeratorGUID + "\"";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        txtName.Text = dr["EnumeratorName"].ToString();
                        DateTime dtm = (DateTime)dr["HireDate"];
                        txtHireDate.Text = string.Format("{0:MMM-dd-yyyy}", dtm);
                        chkActive.Checked = Convert.ToBoolean(dr["Active"]);
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }
        }

        private void ListEnumeratorSamplingLeave(object sender, EventArgs e)
        {
            using (MainForm frm = new MainForm())
            {
                MainForm.SaveColumnWidthEx(sender, myContext: global.lvContext.EnumeratorSampling);
            }
        }

        protected bool CheckDate(String date)
        {
            DateTime Temp;

            if (DateTime.TryParse(date, out Temp) == true)
                return true;
            else
                return false;
        }

        private void Button1Click(object sender, EventArgs e)
        {
            Dictionary<string, string> myData = new Dictionary<string, string>();
            myData.Add("TargetArea", _AOI.AOIGUID);
            if (_IsNew)
            {
                myData.Add("EnumeratorId", Guid.NewGuid().ToString());
            }
            else
            {
                myData.Add("EnumeratorId", _EnumeratorGUID);
            }
            myData.Add("EnumeratorName", txtName.Text);
            myData.Add("HireDate", txtHireDate.Text.ToString());
            myData.Add("Active", chkActive.Checked.ToString());
            if (aoi.UpdateEnumeratorData(_IsNew, myData))
            {
                _AOI.HaveEnumerators = true;
                MainForm fr = new MainForm();
                foreach (Form f in Application.OpenForms)
                {
                    if (f.Name == "frmMain")
                    {
                        fr = (MainForm)f;
                        fr.RefreshLV("aoi");
                    }
                }
                this.Close();
            }
        }

        private void TextBox2Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string c = txtHireDate.Text;
            if (c != "")
            {
                if (!CheckDate(c))
                {
                    MessageBox.Show("Please provide a proper date");
                    e.Cancel = true;
                }
            }
        }

        private void listEnumeratorSampling_DoubleClick(object sender, EventArgs e)
        {
            string[] arr = lvEnumerators.SelectedItems[0].Tag.ToString().Split('|');
            Dictionary<string, string> mySampling = new Dictionary<string, string>();
            mySampling.Add("SamplingID", arr[0]);
            mySampling.Add("GearID", arr[1]);
            mySampling.Add("LSGUID", arr[2]);
            mySampling.Add("SamplingDate", arr[3]);
            _parentForm.EnumeratorSelectedSampling(mySampling);
        }
    }
}
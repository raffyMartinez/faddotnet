/*
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/13/2016
 * Time: 11:50 AM
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace FAD3
{
    /// <summary>
    /// Description of frmEnumerator.
    /// </summary>
    public partial class EnumeratorForm : Form
    {
        private aoi _AOI = new aoi();
        private string _EnumeratorGuid = "";
        private bool _IsNew = false;
        private MainForm _parentForm;
        private List<string> _removedEnumerators = new List<string>();

        public EnumeratorForm(MainForm Parent)
        {
            //default conructor
            InitializeComponent();
            _parentForm = Parent;
            ConfigureListEnumerators();
            Text = "Landing site enumerators";
        }

        public EnumeratorForm(string EnumeratorGuid, MainForm Parent)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
            _parentForm = Parent;
            _EnumeratorGuid = EnumeratorGuid;
            Enumerators.EnumeratorGuid = _EnumeratorGuid;
            ConfigureListEnumeratorSamplings();
            Text = "Enumerator details";
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
        }

        public void EditedEnumerator(string enumeratorGuid, string enumeratorName, DateTime dateHired, bool isActive, global.fad3DataStatus dataStatus)

        {
            if (dataStatus == global.fad3DataStatus.statusNew)
            {
                var lvi = lvEnumerators.Items.Add(enumeratorGuid, enumeratorName, null);
                lvi.SubItems.Add(dateHired.ToString("MMM-dd-yyyy"));
                lvi.SubItems.Add(isActive.ToString());
                lvi.Tag = dataStatus;
            }
            else if (dataStatus == global.fad3DataStatus.statusEdited)
            {
                var lvi = lvEnumerators.Items[enumeratorGuid];
                lvi.Text = enumeratorName;
                lvi.SubItems[1].Text = dateHired.ToString("MMM-dd-yyyy");
                lvi.SubItems[2].Text = isActive.ToString();
                lvi.Tag = dataStatus;
            }

            foreach (ColumnHeader col in lvEnumerators.Columns)
            {
                switch (col.Text)
                {
                    case "Enumerator name":
                    case "Date hired":
                        col.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                        break;

                    default:
                        col.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                        break;
                }
            }
        }

        public aoi AOI
        {
            get { return _AOI; }
            set { _AOI = value; }
        }

        public void AddNew()
        {
            _IsNew = true;
            this.Text = "Add a new enumerator for " + _AOI.AOIName;
        }

        protected bool CheckDate(String date)
        {
            DateTime Temp;

            if (DateTime.TryParse(date, out Temp) == true)
                return true;
            else
                return false;
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
                o.Columns.Add("");
                o.Height = buttonOK.Top - o.Top - 10;

                foreach (ColumnHeader col in o.Columns)
                {
                    col.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                }
            });

            Button buttonAdd = new Button
            {
                Width = 30,
                Height = 30,
                Text = "+",
                Location = new Point(lvEnumerators.Left + lvEnumerators.Width + 10, lvEnumerators.Top),
                Name = "buttonAdd"
            };
            Button buttonRemove = new Button
            {
                Width = buttonAdd.Width,
                Height = buttonAdd.Height,
                Text = "-",
                Location = new Point(buttonAdd.Left, buttonAdd.Top + buttonAdd.Height + 10),
                Name = "buttonRemove"
            };

            buttonAdd.Font = new Font(Font.FontFamily.Name, 14, FontStyle.Bold);
            buttonRemove.Font = buttonAdd.Font;

            Controls.Add(buttonAdd);
            Controls.Add(buttonRemove);
            buttonAdd.Click += OnButtonClick;
            buttonRemove.Click += OnButtonClick;

            foreach (KeyValuePair<string, (string EnumeratorName, DateTime DateHired, bool IsActive, global.fad3DataStatus DataStatus)> kv in Enumerators.GetTargetAreaEnumerators())
            {
                var lvi = lvEnumerators.Items.Add(kv.Key, kv.Value.EnumeratorName, null);
                lvi.Tag = kv.Value.DataStatus;
                lvi.SubItems.Add(kv.Value.DateHired.ToString("MMM-dd-yyyy"));
                lvi.SubItems.Add(kv.Value.IsActive.ToString());
            }

            if (lvEnumerators.Items.Count > 0)
            {
                foreach (ColumnHeader col in lvEnumerators.Columns)
                {
                    switch (col.Text)
                    {
                        case "Enumerator name":
                            col.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                            break;

                        case "Date hired":
                        case "":
                        case "Active":
                            col.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                            break;
                    }
                }
            }
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

            lvEnumerators.With(o =>
            {
                o.View = View.Details;
                o.FullRowSelect = true;
                o.Columns.Add("Reference no.");
                o.Columns.Add("Landing site");
                o.Columns.Add("Gear used");
                o.Columns.Add("Date sampled");
                o.Columns.Add("Catch weight");
                o.Columns.Add("Catch rows");
                o.HideSelection = false;
            });

            if (!_IsNew)
            {
                ReadData();
                var EnumeratorSamplings = Enumerators.GetEnumeratorSamplings();
                foreach (KeyValuePair<string,
                    (string RefNo, string LandingSite, string Gear, DateTime SamplingDate, double WtCatch, int Rows, string GUIDs)>
                    kv in EnumeratorSamplings)
                {
                    var lvi = lvEnumerators.Items.Add(kv.Key, kv.Value.RefNo, null);
                    lvi.Tag = kv.Value.GUIDs;
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
                Logger.Log("Catch and effort column width not found in registry");
            }
        }

        private void EnumeratorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_EnumeratorGuid.Length > 0)
                global.SaveFormSettings(this);
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonOK":
                    if (_EnumeratorGuid.Length == 0)
                    {
                        if (lvEnumerators.Items.Count > 0 || _removedEnumerators.Count > 0)
                        {
                            var enumeratorsData = new Dictionary<string, (string enumeratorName, DateTime dateHired, bool isActive, global.fad3DataStatus DataStatus)>();

                            foreach (ListViewItem lvi in lvEnumerators.Items)
                            {
                                var DataStatus = (global.fad3DataStatus)lvi.Tag;

                                if (DataStatus != global.fad3DataStatus.statusFromDB)
                                    enumeratorsData.Add(lvi.Name, (lvi.Text, DateTime.Parse(lvi.SubItems[1].Text), bool.Parse(lvi.SubItems[2].Text), DataStatus));
                            }

                            foreach (var item in _removedEnumerators)
                            {
                                //actually all that is needed to delete an enumerator is the Guid and the DataStatus.
                                //The rest of the values in the tuple are not needed but they have to be filled up because
                                //the tuple does not accept optional parameters.
                                enumeratorsData.Add(item, ("", DateTime.Today, false, global.fad3DataStatus.statusForDeletion));
                            }

                            if ((enumeratorsData.Count > 0 || _removedEnumerators.Count > 0) && Enumerators.SaveTargetAreaEnumerators(enumeratorsData))
                            {
                                Close();

                                //then we refresh mainform to reflect any changes
                                _parentForm.RefreshLV("aoi");
                            }
                            else
                            {
                                if (enumeratorsData.Count == 0 && _removedEnumerators.Count == 0)
                                {
                                    Close();
                                }
                                else
                                {
                                    //something went wrong
                                    Logger.Log("Message = saving enumerator data was not completed\r\n" +
                                                    "Location = EnumeratorForm.cs.OnButtonClick");
                                }
                            }
                        }
                        else if (lvEnumerators.Items.Count == 0)
                        {
                            MessageBox.Show("Please add at least one enumerator", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        Close();
                    }
                    break;

                case "buttonCancel":
                    Close();
                    break;

                case "buttonAdd":

                    var eef = new EnumeratorEntryForm(this);
                    eef.ShowDialog(this);

                    break;

                case "buttonRemove":
                    var selectedEnumeratorGuid = lvEnumerators.SelectedItems[0].Name;
                    var NumberOfSampling = Enumerators.NumberOfSamplingsOfEnumerator(selectedEnumeratorGuid);
                    if (NumberOfSampling == 0)
                    {
                        if (MessageBox.Show("This enumerator was not able to do any sampling.\r\n" +
                                 "Do you still want to remove this enumerator?", "Please verify",
                                 MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
                        {
                            lvEnumerators.Items.Remove(lvEnumerators.Items[selectedEnumeratorGuid]);
                            _removedEnumerators.Add(selectedEnumeratorGuid);
                        }
                    }
                    else
                    {
                        var term = NumberOfSampling == 1 ? "sampling" : "samplings";
                        if (NumberOfSampling > 0)
                        {
                            if (MessageBox.Show($"This enumerator has conducted {NumberOfSampling} {term}.\r\n" +
                                               "You cannot remove this enumerator.\r\n\r\n" +
                                               "Instead, you can make the enumerator inactive by clicking on the OK button.",
                                               "Validation error", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                            {
                                lvEnumerators.SelectedItems[0].SubItems[2].Text = "False";
                                lvEnumerators.SelectedItems[0].Tag = global.fad3DataStatus.statusEdited;
                            }
                        }
                    }
                    break;
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            if (_EnumeratorGuid.Length > 0)
                global.LoadFormSettings(this);
        }

        private void OnlistEnumeratorSampling_DoubleClick(object sender, EventArgs e)
        {
            if (_EnumeratorGuid.Length > 0)
            {
                lvEnumerators.SelectedItems[0].With(o =>
                {
                    string[] arr = o.Tag.ToString().Split('|');
                    Dictionary<string, string> mySampling = new Dictionary<string, string>();
                    mySampling.Add("SamplingID", o.Name);
                    mySampling.Add("LSGUID", arr[0]);
                    mySampling.Add("GearID", arr[1]);
                    mySampling.Add("SamplingDate", o.SubItems[3].Text);
                    _parentForm.EnumeratorSelectedSampling(mySampling);
                });
            }
            else
            {
                lvEnumerators.SelectedItems[0].With(o =>
                {
                    var eef = new EnumeratorEntryForm(o.Name, o.Text, DateTime.Parse(o.SubItems[1].Text), bool.Parse(o.SubItems[2].Text), this);
                    eef.ShowDialog(this);
                });
            }
        }

        private void OnListEnumeratorSamplingLeave(object sender, EventArgs e)
        {
            using (MainForm frm = new MainForm())
            {
                MainForm.SaveColumnWidthEx(sender, myContext: global.lvContext.EnumeratorSampling);
            }
        }

        private void OnTextBoxValidating(object sender, System.ComponentModel.CancelEventArgs e)
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

        private void ReadData()
        {
            DataTable dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = "Select * from tblEnumerators where EnumeratorID =\"" + _EnumeratorGuid + "\"";
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
                    Logger.Log(ex);
                }
            }
        }
    }
}
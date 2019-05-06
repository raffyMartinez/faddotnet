using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Reflection;

namespace FAD3.Database.Forms
{
    public partial class GearVarNameUsageInventoryForm : Form
    {
        private GearInventoryForm _parentForm;
        private static GearVarNameUsageInventoryForm _instance;
        public string GearVariationNameUsed { get; set; }
        public string InventoryGuid { get; set; }
        public List<string> ListLocalNames { get; internal set; }

        public static GearVarNameUsageInventoryForm GetInstance(GearInventoryForm parentForm)
        {
            if (_instance == null) _instance = new GearVarNameUsageInventoryForm(parentForm);
            return _instance;
        }

        public GearVarNameUsageInventoryForm(GearInventoryForm parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;
        }

        public void RefreshList()
        {
            lvDetails.Items.Clear();
            FillList();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            global.LoadFormSettings(this);
            lvDetails.View = View.Details;
            lvDetails.FullRowSelect = true;
            lvDetails.HideSelection = false;
            lvDetails.Columns.Clear();
            lvDetails.Columns.Add("Inventory project");
            lvDetails.Columns.Add("Date implemented");
            lvDetails.Columns.Add("Province");
            lvDetails.Columns.Add("Municipality");
            lvDetails.Columns.Add("Barangay");
            lvDetails.Columns.Add("Sitio");
            lvDetails.Columns.Add("Gear variation");
            lvDetails.Columns.Add("Local names");

            SizeColumns(lvDetails);
            FillList();
            lblTitle.Text = $@"Gear inventories of the fishing gear variation ""{GearVariationNameUsed}""";
            Text = $"Usage of {GearVariationNameUsed}";
        }

        private void FillList()
        {
            lvDetails.Visible = false;
            string sql = $@"SELECT tblGearInventoryBarangayData.DataGuid, tblGearInventories.InventoryName,
                                tblGearInventories.DateConducted, Provinces.ProvinceName, Municipalities.Municipality,
                                tblGearInventoryBarangay.Barangay, tblGearInventoryBarangay.Sitio, tblGearVariations.Variation,
                                tblGearLocalNames.LocalName
                            FROM tblGearVariations
                                INNER JOIN (tblGearLocalNames
                                RIGHT JOIN ((tblGearInventories
                                INNER JOIN ((Provinces
                                INNER JOIN Municipalities ON Provinces.ProvNo = Municipalities.ProvNo)
                                INNER JOIN tblGearInventoryBarangay ON (Municipalities.MunNo = tblGearInventoryBarangay.Municipality)
                                    AND (Municipalities.MunNo = tblGearInventoryBarangay.Municipality))
                                    ON tblGearInventories.InventoryGuid = tblGearInventoryBarangay.InventoryGuid)
                                INNER JOIN (tblGearInventoryBarangayData
                                LEFT JOIN tblGearInventoryGearLocalNames
                                    ON tblGearInventoryBarangayData.DataGuid = tblGearInventoryGearLocalNames.InventoryDataGuid)
                                    ON tblGearInventoryBarangay.BarangayInventoryGuid = tblGearInventoryBarangayData.BarangayInventoryGUID)
                                    ON tblGearLocalNames.LocalNameGUID = tblGearInventoryGearLocalNames.LocalNameGuid)
                                    ON tblGearVariations.GearVarGUID = tblGearInventoryBarangayData.GearVariation
                            WHERE tblGearVariations.Variation='{GearVariationNameUsed}' AND tblGearInventories.InventoryGuid = {{{InventoryGuid}}}
                            ORDER BY tblGearInventories.InventoryName, Provinces.ProvinceName,
                                Municipalities.Municipality, tblGearInventoryBarangay.Barangay, tblGearInventoryBarangay.Sitio";

            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    var myDT = new DataTable();
                    var adapter = new OleDbDataAdapter(sql, conection);
                    adapter.Fill(myDT);
                    for (int i = 0; i < myDT.Rows.Count; i++)
                    {
                        DataRow dr = myDT.Rows[i];
                        string key = dr["DataGuid"].ToString();
                        if (!lvDetails.Items.ContainsKey(key))
                        {
                            var lvi = lvDetails.Items.Add(key, dr["InventoryName"].ToString(), null);
                            lvi.SubItems.Add(((DateTime)dr["DateConducted"]).ToString("MMM-dd-yyy"));
                            lvi.SubItems.Add(dr["ProvinceName"].ToString());
                            lvi.SubItems.Add(dr["Municipality"].ToString());
                            lvi.SubItems.Add(dr["Barangay"].ToString());
                            lvi.SubItems.Add(dr["Sitio"].ToString());
                            lvi.SubItems.Add(dr["Variation"].ToString());
                            lvi.SubItems.Add(dr["LocalName"].ToString());
                        }
                        else
                        {
                            lvDetails.Items[key].SubItems[7].Text += $", {dr["LocalName"].ToString()}";
                        }
                    }
                }
                catch (Exception ex) { Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name); }
            }
            SizeColumns(lvDetails, false);
            lvDetails.Visible = true;
        }

        /// <summary>
        /// Sizes all columns so that it fits the widest column content or the column header content
        /// </summary>
        private void SizeColumns(ListView lv, bool init = true)
        {
            foreach (ColumnHeader c in lv.Columns)
            {
                if (init)
                {
                    c.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                    c.Tag = c.Width;
                }
                else
                {
                    c.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                    c.Width = c.Width > (int)c.Tag ? c.Width : (int)c.Tag;
                }
            }
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
            global.SaveFormSettings(this);
        }

        private void ShowInventoryEditForm()
        {
            using (var inventoryEditForm = new GearInventoryEditForm(_parentForm.TreeLevel, _parentForm.TargetArea, _parentForm.FishingGearInventory, _parentForm))
            {
                inventoryEditForm.EditInventoryLevel(lvDetails.SelectedItems[0].Name);
                inventoryEditForm.ShowDialog(this);
            }
        }

        private void OnListViewDoubleClick(object sender, EventArgs e)
        {
            ShowInventoryEditForm();
        }

        private void OnListViewMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                dropdownMenu.Items.Clear();
                var dropdownItem = dropdownMenu.Items.Add("Edit inventory data for this gear");
                dropdownItem.Name = "menuEditInventoryData";

                dropdownItem = dropdownMenu.Items.Add("Copy text");
                dropdownItem.Name = "menuCopyText";

                dropdownItem = dropdownMenu.Items.Add("List all gear local names");
                dropdownItem.Name = "menuListLocalNames";

                dropdownMenu.Show(Cursor.Position);
            }
        }

        private void OnItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "menuListLocalNames":
                    ListLocalNames = new List<string>();
                    foreach (ListViewItem item in lvDetails.Items)
                    {
                        var names = item.SubItems[7].Text.Split(',');
                        for (int n = 0; n < names.Length; n++)
                        {
                            string name = names[n].Trim().ToLower();
                            if (!ListLocalNames.Contains(name))
                            {
                                ListLocalNames.Add(name);
                            }
                        }
                    }
                    ListGearLocalNamesForm lglnf = new ListGearLocalNamesForm(ListLocalNames, GearVariationNameUsed);
                    lglnf.ShowDialog();
                    break;

                case "menuEditInventoryData":
                    ShowInventoryEditForm();
                    break;

                case "menuCopyText":
                    StringBuilder copyText = new StringBuilder();
                    string colNames = "";
                    foreach (ColumnHeader col in lvDetails.Columns)
                    {
                        colNames += $"{col.Text}\t";
                    }
                    copyText.Append($"{colNames.TrimEnd()}\r\n");

                    foreach (ListViewItem item in lvDetails.Items)
                    {
                        copyText.Append(item.Text);
                        for (int n = 1; n < item.SubItems.Count; n++)
                        {
                            copyText.Append($"\t{item.SubItems[n]?.Text}");
                        }
                        copyText.Append("\r\n");
                    }
                    Clipboard.SetText(copyText.ToString());
                    break;
            }
        }
    }
}
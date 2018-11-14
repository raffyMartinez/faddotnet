using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FAD3.GUI.Classes;
using FAD3.Database.Classes;
using System.Diagnostics;
using FAD3.Database.Forms;

namespace FAD3
{
    public partial class AllSpeciesForm : Form
    {
        private MainForm _parent;
        private Dictionary<string, string> _filters = new Dictionary<string, string>();

        public MainForm parentForm
        {
            get { return _parent; }
        }

        public AllSpeciesForm(MainForm parent)
        {
            InitializeComponent();
            _parent = parent;
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

        private void AllSpeciesForm_Load(object sender, EventArgs e)
        {
            global.LoadFormSettings(this, true);
            dropDownMenu.ItemClicked += OnDropDownMenuItemClicked;
            Text = "List of all species in catch composition of sampled landings";
            //lvTaxa.Columns.Add("");
            lvTaxa.View = View.List;
            //SizeColumns(lvTaxa);
            foreach (var item in CatchName.RetrieveTaxaDictionary())
            {
                lvTaxa.Items.Add(item.Key.ToString(), item.Value, null);
            }
            //SizeColumns(lvTaxa, false);

            lvNames.With(o =>
                {
                    o.View = View.Details;
                    o.FullRowSelect = true;
                    o.Columns.Add("Row");
                    o.Columns.Add("Genus");
                    o.Columns.Add("Species");
                    o.Columns.Add("Taxa");
                    o.Columns.Add("In FishBase");
                    o.Columns.Add("Records");
                    o.Columns.Add("Notes");
                    o.HideSelection = false;
                });
            SizeColumns(lvNames);
            FillListNames();
            SizeColumns(lvNames, false);
        }

        private void OnDropDownMenuItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            e.ClickedItem.Owner.Hide();
            switch (e.ClickedItem.Name)
            {
                case "menuViewSamplings":
                    var catchName = $"{lvNames.SelectedItems[0].SubItems[1].Text} {lvNames.SelectedItems[0].SubItems[2].Text}";
                    var ef = GearSpeciesSamplingsForm.GetInstance(lvNames.SelectedItems[0].Name, catchName, this);
                    if (!ef.Visible)
                    {
                        ef.Show(this);
                    }
                    else
                    {
                        ef.BringToFront();
                        ef.setItemGuid_Name_Parent(lvNames.SelectedItems[0].Name, catchName, this);
                    }

                    break;

                case "menuAddNewName":
                    SpeciesNameForm snf = new SpeciesNameForm(this);
                    snf.ShowDialog(this);
                    break;

                case "menuEditName":
                    ShowNameDetail();
                    break;
            }
        }

        private void FillListNames(Dictionary<string, string> filters = null, bool OnlyWithRecords = false)
        {
            lvNames.Items.Clear();
            int n = 1;
            foreach (var item in Names.RetrieveScientificNames(filters, OnlyWithRecords))
            {
                var inFishBase = item.Value.inFishBase ? "x" : "";
                var recordCount = item.Value.catchCompositionRecordCount == 0 ? "" : item.Value.catchCompositionRecordCount.ToString();
                var lvi = new ListViewItem(new string[] { n.ToString(), item.Value.genus, item.Value.species, item.Value.taxaName, inFishBase, recordCount, item.Value.Notes });
                lvi.Name = item.Value.catchNameGuid;
                lvNames.Items.Add(lvi);
                n++;
            }
        }

        private void OnButtonOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OnButton_Click(object sender, EventArgs e)
        {
            var o = (Button)sender;
            switch (o.Name)
            {
                case "buttonApply":

                    //process taxa select
                    var selectedTaxa = "";
                    foreach (ListViewItem item in lvTaxa.CheckedItems)
                    {
                        var taxaNo = int.Parse(item.Name);
                        selectedTaxa += $"{taxaNo.ToString()},";
                    }
                    selectedTaxa = selectedTaxa.Trim(',');

                    if (selectedTaxa.Length > 0)
                    {
                        if (_filters.Keys.Contains<string>("taxa"))
                            _filters["taxa"] = $" TaxaNo in ({selectedTaxa}) ";
                        else
                            _filters.Add("taxa", $" TaxaNo like ({selectedTaxa}) ");
                    }
                    else
                    {
                        _filters.Remove("taxa");
                    }

                    //process search textbox
                    if (txtSearch.Text.Length > 0)
                    {
                        if (_filters.Keys.Contains<string>("search"))
                            _filters["search"] = $" Genus ALIKE '{txtSearch.Text}%' ";
                        else
                            _filters.Add("search", $" Genus ALIKE '{txtSearch.Text}%' ");
                    }
                    else
                    {
                        _filters.Remove("search");
                    }

                    FillListNames(_filters, chkShowWithRecords.Checked);

                    break;

                case "buttonReset":
                    break;

                case "buttonEdit":
                    ShowNameDetail();
                    break;

                case "buttonAdd":
                    SpeciesNameForm snf = new SpeciesNameForm(this);
                    snf.ShowDialog(this);
                    break;

                case "btnExport":
                    var taxaCSV = "";
                    if (lvTaxa.CheckedItems.Count > 0)
                    {
                        foreach (ListViewItem checkedItem in lvTaxa.CheckedItems)
                        {
                            taxaCSV += ((Taxa)int.Parse(checkedItem.Name)).ToString() + ",";
                        }
                        taxaCSV.Trim(',');
                    }
                    ExportDialogForm edf = new ExportDialogForm(ExportDataType.ExportDataSpecies);
                    edf.TaxaCSV = "";
                    edf.ShowDialog(this);
                    break;

                case "buttonSearch":

                    break;
            }
        }

        private void ShowNameDetail()
        {
            if (lvNames.SelectedItems.Count > 0)
            {
                lvNames.SelectedItems[0].With(o =>
                {
                    var genus = o.SubItems[1].Text;
                    var species = o.SubItems[2].Text;
                    var taxaName = o.SubItems[3].Text;
                    var nameGuid = o.Name;

                    SpeciesNameForm snf = new SpeciesNameForm(genus, species, nameGuid, taxaName, this);
                    snf.ShowDialog(this);
                });
            }
        }

        private void OnlvNames_DoubleClick(object sender, EventArgs e)
        {
            ShowNameDetail();
        }

        private void lvNames_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && lvNames.SelectedItems.Count > 0)
            {
                dropDownMenu.Items.Clear();
                var item = dropDownMenu.Items.Add("View samplings");
                item.Name = "menuViewSamplings";

                item = dropDownMenu.Items.Add("Add new catch name");
                item.Name = "menuAddNewName";

                item = dropDownMenu.Items.Add("Edit catch name");
                item.Name = "menuEditName";

                ToolStripMenuItem subMenu = new ToolStripMenuItem();
                subMenu.Text = "Browse on WWW";

                CatchNameURLGenerator.CatchName = lvNames.SelectedItems[0].SubItems[1].Text + " " + lvNames.SelectedItems[0].SubItems[2].Text;
                var urls = CatchNameURLGenerator.URLS;

                foreach (var url in urls)
                {
                    ToolStripMenuItem subItem = new ToolStripMenuItem();
                    subItem.Text = url.Key;
                    subItem.Tag = url.Value;
                    subMenu.DropDownItems.Add(subItem);
                }

                subMenu.DropDownItemClicked += OnSubMenuDropDownClick;

                dropDownMenu.Items.Add(subMenu);
            }
        }

        private void OnSubMenuDropDownClick(object sender, ToolStripItemClickedEventArgs e)
        {
            e.ClickedItem.OwnerItem.Owner.Hide();
            Process.Start(e.ClickedItem.Tag.ToString());
        }

        private void AllSpeciesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            global.SaveFormSettings(this);
        }
    }
}
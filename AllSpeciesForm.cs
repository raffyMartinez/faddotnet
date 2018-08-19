using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FAD3
{
    public partial class AllSpeciesForm : Form
    {
        private MainForm _parent;
        private Dictionary<string, string> _filters = new Dictionary<string, string>();

        public AllSpeciesForm(MainForm parent)
        {
            InitializeComponent();
            _parent = parent;
        }

        private void AllSpeciesForm_Load(object sender, EventArgs e)
        {
            //listBoxFilter.SelectionMode = SelectionMode.MultiSimple;
            var n = 0;
            foreach (var item in CatchName.RetrieveTaxaDictionary())
            {
                listBoxFilter.Items.Add(item.Value);
            }

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
                });
            FillListNames();
        }

        private void FillListNames(Dictionary<string, string> filters = null, bool OnlyWithRecords = false)
        {
            lvNames.Items.Clear();
            int n = 1;
            foreach (var item in names.RetrieveScientificNames(filters, OnlyWithRecords))
            {
                var inFishBase = item.Value.inFishBase ? "x" : "";
                var recordCount = item.Value.catchCompositionRecordCount == 0 ? "" : item.Value.catchCompositionRecordCount.ToString();
                var lvi = new ListViewItem(new string[] { n.ToString(), item.Value.genus, item.Value.species, item.Value.taxaName, inFishBase, recordCount, item.Value.Notes });
                lvi.Name = item.Value.catchNameGuid;
                lvNames.Items.Add(lvi);
                n++;
            }
            foreach (ColumnHeader ch in lvNames.Columns)
            {
                switch (ch.Name)
                {
                    case "Genus":
                    case "Species":
                    case "Taxa":
                        ch.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                        break;

                    default:
                        ch.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                        break;
                }
            }
        }

        private void OnButton_Click(object sender, EventArgs e)
        {
            var o = (Button)sender;
            switch (o.Name)
            {
                case "buttonApply":

                    //process taxa select
                    var selectedTaxa = "";
                    foreach (var item in listBoxFilter.CheckedItems)
                    {
                        var taxaNo = (int)CatchName.TaxaFromTaxaName(item.ToString());
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

                case "buttonOK":
                    Close();
                    break;

                case "buttonEdit":

                    break;

                case "buttonAdd":

                    break;

                case "buttonSearch":

                    break;
            }
        }

        private void OnlvNames_DoubleClick(object sender, EventArgs e)
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
}
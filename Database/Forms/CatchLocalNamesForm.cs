using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FAD3.Database.Classes;
using FAD3.GUI.Classes;
using System.IO;
using System.Diagnostics;

namespace FAD3.Database.Forms
{
    public partial class CatchLocalNamesForm : Form
    {
        private static CatchLocalNamesForm _instance;
        private Identification _idType;
        private String _listTitle;

        public void SetTreeItem(string itemName)
        {
            foreach (TreeNode item in treeView.Nodes["root"].Nodes)
            {
                if (item.Text == itemName)
                {
                    treeView.SelectedNode = item;
                    TreeNodeMouseClickEventArgs e = new TreeNodeMouseClickEventArgs(item, MouseButtons.Left, 0, 0, 0);
                    OnNodeClick(item, e);
                    break;
                }
            }
        }

        public ComboBox SwichViewCombo
        {
            get { return cboSelectId; }
        }

        public static CatchLocalNamesForm GetInstance(Identification identification)
        {
            if (_instance == null) _instance = new CatchLocalNamesForm(identification);
            return _instance;
        }

        public Identification IDType
        {
            get { return _idType; }
            set
            {
                bool willSetUI = _idType != value;
                _idType = value;

                if (willSetUI)
                {
                    treeView.Nodes.Clear();
                    SetUI();
                }
            }
        }

        public CatchLocalNamesForm(Identification identification)
        {
            InitializeComponent();
            _idType = identification;
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnExport":
                    ExportDialogForm edf = new ExportDialogForm(ExportDataType.ExportDataSelect);
                    edf.ShowDialog(this);
                    break;

                case "btnImport":

                    OpenFileDialog ofd = new OpenFileDialog()
                    {
                        Filter = "text file|*.txt|html file|*.htm;*.html|all files|*.*",
                        FilterIndex = 1,
                        Title = "Open text file"
                    };
                    ofd.ShowDialog();
                    if (ofd.FileName.Length > 0)
                    {
                        switch (Path.GetExtension(ofd.FileName))
                        {
                            case ".txt":

                                int fail = 0;
                                var result = Names.ImportLocalNamestoScientificNames(ofd.FileName, ref fail);
                                var msg = $"{result} local names to scientific name pairs were added to the database";
                                if (fail > 0)
                                {
                                    msg += $"\r\n There were also {fail} records not saved because of key violations";
                                }
                                MessageBox.Show(msg, "Fininshed importing local name - scientific name pairs", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;

                            case ".htm":
                            case ".html":
                                HTMLTableSelectColumnsForm htmlColForm = new HTMLTableSelectColumnsForm(ofd.FileName, CatchNameDataType.CatchSpeciesLocalNamePair);
                                htmlColForm.ShowDialog(this);

                                break;
                        }
                    }

                    break;

                case "btnAdd":
                    if (treeView.SelectedNode.Name != "root")
                    {
                        LocalNameSciNameEditForm lnsef = LocalNameSciNameEditForm.GetInstance(treeView.SelectedNode.Text, treeView.SelectedNode.Name, _idType, this);
                        if (lnsef.Visible)
                        {
                            lnsef.BringToFront();
                        }
                        else
                        {
                            lnsef.Show(this);
                        }
                    }
                    break;

                case "btnRemove":
                    break;

                case "btnOk":
                    break;

                case "btnCancel":
                    Close();
                    break;
            }
        }

        public void RefreshLists()
        {
            TreeNodeMouseClickEventArgs e = new TreeNodeMouseClickEventArgs(treeView.SelectedNode, MouseButtons.Left, 0, 0, 0);
            OnNodeClick(treeView.SelectedNode, e);
        }

        private void SetUI()
        {
            listView.Items.Clear();
            foreach (var item in Names.Languages)
            {
                var lvi = listView.Items.Add(item.Key, item.Value, null);
            }
            SizeColumns(listView);

            switch (_idType)
            {
                case Identification.LocalName:
                    cboSelectId.Text = "Local/common names";
                    lblList.Text = "Species names";
                    Text = "Species name equivalents of local/common names";
                    var treeNode = treeView.Nodes.Add("root", "Local names");
                    treeNode.Tag = "root";
                    Names.GetLocalNames();
                    if (Names.LocalNameListDict.Count > 0)
                    {
                        foreach (var item in Names.LocalNameListDict)
                        {
                            treeNode = treeView.Nodes["root"].Nodes.Add(item.Key, item.Value);
                            treeNode.Tag = "name";
                        }
                        treeView.Nodes["root"].Expand();
                    }
                    else
                    {
                        treeView.Nodes["root"].Nodes.Add("***dummy***");
                    }
                    lblTree.Text = $"Local/common names: ({treeView.Nodes["root"].Nodes.Count} items)";
                    break;

                case Identification.Scientific:
                    cboSelectId.Text = "Species names";
                    lblList.Text = "Local/common names";
                    Text = "Local name/common name equivalents of species names";
                    treeNode = treeView.Nodes.Add("root", "Scientific names");
                    treeNode.Tag = "root";
                    Names.GetAllSpecies();
                    if (Names.AllSpeciesDictionary.Count > 0)
                    {
                        foreach (var item in Names.AllSpeciesDictionary)
                        {
                            treeNode = treeView.Nodes["root"].Nodes.Add(item.Key, item.Value);
                            treeNode.Tag = "name";
                        }
                        treeView.Nodes["root"].Expand();
                    }
                    else
                    {
                        treeView.Nodes["root"].Nodes.Add("***dummy***");
                    }
                    lblTree.Text = $"Species names: ({treeView.Nodes["root"].Nodes.Count} items)";
                    break;
            }
            _listTitle = lblList.Text;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            cboSelectId.Items.Add("Species names");
            cboSelectId.Items.Add("Local/common names");
            listView.View = View.Details;
            listView.FullRowSelect = true;
            listView.Columns.Add("Language");
            switch (_idType)
            {
                case Identification.LocalName:
                    listView.Columns.Add("Equivalent species names");
                    break;

                case Identification.Scientific:
                    listView.Columns.Add("Equivalent local/common names");
                    break;
            }
            listView.Columns.Add("");
            SizeColumns(listView);
            SetUI();
            global.LoadFormSettings(this);
            _listTitle = lblList.Text;
        }

        private void OnFormClose(object sender, FormClosedEventArgs e)
        {
            _instance = null;
            global.SaveFormSettings(this);
        }

        private void OnNodeClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var count = 0;
            var nameGuid = e.Node.Name;
            if (e.Node.Tag.ToString() == "name")
            {
                listView.Items.Clear();
                foreach (var item in Names.Languages)
                {
                    var items = 0;
                    StringBuilder sb = new StringBuilder();
                    var lvi = listView.Items.Add(item.Key, item.Value, null);
                    var nameItems = "";
                    switch (_idType)
                    {
                        case Identification.LocalName:
                            foreach (var names in Names.GetSpeciesNameFromLocalNameLanguage(e.Node.Name, lvi.Name))
                            {
                                //nameItems += names.genus + " " + names.species + ", ";
                                sb.Append(names.genus);
                                sb.Append(" ");
                                sb.Append(names.species);
                                sb.Append(", ");

                                count++;
                                items++;
                            }
                            break;

                        case Identification.Scientific:
                            foreach (var names in Names.GetLocalNameFromSpeciesNameLanguage(e.Node.Name, lvi.Name))
                            {
                                //nameItems += names + ", ";
                                sb.Append(names);
                                sb.Append(", ");
                                count++;
                                items++;
                            }
                            break;
                    }

                    //nameItems = nameItems.Trim(new char[] { ',', ' ' });
                    nameItems = sb.ToString().Trim(new char[] { ',', ' ' });
                    lvi.SubItems.Add(nameItems);
                    lvi.Tag = items;
                }

                LocalNameSciNameEditForm lne = LocalNameSciNameEditForm.GetInstance();
                if (lne != null)
                {
                    lne.SetSelectedName(e.Node.Text, e.Node.Name, _idType);
                }
            }
            SizeColumns(listView, false);

            if (e.Node.Name != "root")
                lblList.Text = $"{_listTitle} of  {e.Node.Text} ({count})";
            else
                lblList.Text = _listTitle;
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

        private void OnListViewDblClick(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                CatchLocalNameSelectedForm clnsf = CatchLocalNameSelectedForm.GetInstance(_idType, listView.SelectedItems[0].Text, treeView.SelectedNode.Text, this);
                if (clnsf.Visible)
                {
                    clnsf.NewSelection(_idType, listView.SelectedItems[0].Text, treeView.SelectedNode.Text);
                    clnsf.BringToFront();
                }
                else
                {
                    clnsf.Show(this);
                }
            }
        }

        private void OnAfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node.FirstNode.Text == "***dummy***")
            {
                e.Node.Nodes.Clear();
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2 && _idType == Identification.LocalName)

            {
                treeView.LabelEdit = true;
                treeView.SelectedNode.BeginEdit();
            }
        }

        private void afterAfterEdit(TreeNode node)
        {
            Names.UpdateLocalName(node.Text, node.Name);
            treeView.LabelEdit = false;
            lblList.Text = _listTitle + " of " + node.Text;
        }

        private void OnAfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            BeginInvoke(new Action(() => afterAfterEdit(e.Node)));
        }

        private void OnIndexChanged(object sender, EventArgs e)
        {
            switch (cboSelectId.Text)
            {
                case "Local/common names":
                    IDType = Identification.LocalName;
                    break;

                case "Species names":
                    IDType = Identification.Scientific;
                    break;
            }
        }

        private void OnContextMenuClick(object sender, ToolStripItemClickedEventArgs e)
        {
            e.ClickedItem.Owner.Hide();
            var itemCount = (int)listView.SelectedItems[0].Tag;
            switch (e.ClickedItem.Name)
            {
                case "itemLocateOnNavigationTree":

                    if (itemCount == 1)
                    {
                        var itemText = listView.SelectedItems[0].SubItems[1].Text;
                        switch (_idType)
                        {
                            case Identification.LocalName:
                                cboSelectId.SelectedIndex = 0;
                                break;

                            case Identification.Scientific:
                                cboSelectId.SelectedIndex = 1;
                                break;
                        }

                        foreach (TreeNode item in treeView.Nodes["root"].Nodes)
                        {
                            if (item.Text == itemText)
                            {
                                treeView.SelectedNode = item;
                                TreeNodeMouseClickEventArgs ee = new TreeNodeMouseClickEventArgs(item, MouseButtons.Left, 0, 0, 0);
                                OnNodeClick(item, ee);
                                break;
                            }
                        }
                    }
                    else if (itemCount > 1)
                    {
                        OnListViewDblClick(null, null);
                    }
                    break;
            }
        }

        private void OnWWWDropDownClick(object sender, ToolStripItemClickedEventArgs e)
        {
            e.ClickedItem.OwnerItem.Owner.Hide();
            if (_idType == Identification.LocalName)
            {
                CatchNameURLGenerator.CatchName = listView.SelectedItems[0].SubItems[1].Text;
            }
            else
            {
                CatchNameURLGenerator.CatchName = treeView.SelectedNode.Text;
            }
            var urls = CatchNameURLGenerator.URLS;
            itemWiki.Tag = urls["Wikipaedia"];
            itemFishBase.Tag = urls["FishBase"];
            Process.Start(e.ClickedItem.Tag.ToString());
        }

        private void OnControlEnter(object sender, EventArgs e)
        {
        }
    }
}
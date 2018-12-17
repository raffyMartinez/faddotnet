using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using System.Windows.Forms;
using FAD3.Database.Classes;
using FAD3.Database.Classes;
using System.IO;
using System.Diagnostics;

namespace FAD3.Database.Forms
{
    public partial class CatchLocalNamesForm : Form
    {
        private static CatchLocalNamesForm _instance;
        private Identification _idType;
        private String _listTitle;
        private string _name;
        private string _activeControl;
        private int _listedNameCount;
        private int _rowsImported;
        private bool _hiddenTree;
        private string _speciesGuid;
        private string _localNameGuid;
        private Form _parentForm;

        public string SpeciesName { get; set; }
        public string LocalName { get; set; }

        public string LocalNameGuid
        {
            get { return _localNameGuid; }
            set
            {
                if (value != _localNameGuid)
                {
                    _localNameGuid = value;
                    _speciesGuid = "";
                    _idType = Identification.LocalName;
                    //FillListSpeciesNames();
                    FillListsNames();
                }
            }
        }

        public string SpeciesGuid
        {
            get { return _speciesGuid; }
            set
            {
                if (value != _speciesGuid)
                {
                    _speciesGuid = value;
                    _localNameGuid = "";
                    _idType = Identification.Scientific;
                    //FillListLocalNames();
                    FillListsNames();
                }
            }
        }

        public void SetTreeItem(string itemName)
        {
            if (itemName == "root")
            {
                treeView.SelectedNode = treeView.Nodes["root"];
            }
            else
            {
                foreach (TreeNode item in treeView.Nodes["root"].Nodes)
                {
                    if (item.Text == itemName)
                    {
                        treeView.SelectedNode = item;
                        break;
                    }
                }
            }
            TreeNodeMouseClickEventArgs e = new TreeNodeMouseClickEventArgs(treeView.SelectedNode, MouseButtons.Left, 0, 0, 0);
            OnNodeClick(treeView.SelectedNode, e);
        }

        public ToolStripComboBox SwichViewCombo
        {
            get { return tbCombo; }
        }

        public static CatchLocalNamesForm GetInstance(Identification identification)
        {
            if (_instance == null) _instance = new CatchLocalNamesForm(identification);
            return _instance;
        }

        public static CatchLocalNamesForm GetInstance(Identification identification, string name)
        {
            if (_instance == null) _instance = new CatchLocalNamesForm(identification, name);
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

        public CatchLocalNamesForm(Identification identification, string name)
        {
            InitializeComponent();
            _idType = identification;
            _name = name;
            Names.RowsImported += OnNamesImportRows;
        }

        private void OnNamesImportRows(object sender, ImportRowsFromFileEventArgs e)
        {
            if (e.DataType == ExportImportDataType.CatchLocalNames
                || e.DataType == ExportImportDataType.CatchLocalNameSpeciesNamePair
                || e.DataType == ExportImportDataType.LocalNameLanguages)
            {
                _rowsImported = e.RowsImported;
                var itemName = "Local names";
                switch (e.DataType)
                {
                    case ExportImportDataType.CatchLocalNames:
                        break;

                    case ExportImportDataType.CatchLocalNameSpeciesNamePair:
                        itemName = "Local names-species names";
                        break;

                    case ExportImportDataType.LocalNameLanguages:
                        itemName = "Languages";

                        break;
                }
                if (e.IsComplete)
                {
                    lblList.Invoke((MethodInvoker)delegate
                    {
                        lblList.Text = $"Finished importing {itemName}: {_rowsImported} items imported";
                    });
                }
                else
                {
                    lblList.Invoke((MethodInvoker)delegate
                    {
                        lblList.Text = $"Importing {itemName}: {_rowsImported} items imported";
                    });
                }
            }
        }

        public CatchLocalNamesForm(string speciesGuid, Form parentForm)
        {
            InitializeComponent();
            _hiddenTree = true;
            _idType = Identification.Scientific;
            _speciesGuid = speciesGuid;
            _parentForm = parentForm;
        }

        public static CatchLocalNamesForm GetInstance(string nameGuid, Identification idType, Form parentForm)
        {
            if (_instance == null) _instance = new CatchLocalNamesForm(nameGuid, idType, parentForm);
            return _instance;
        }

        public static CatchLocalNamesForm GetInstance(string speciesGuid, Form parentForm)
        {
            if (_instance == null) _instance = new CatchLocalNamesForm(speciesGuid, parentForm);
            return _instance;
        }

        public CatchLocalNamesForm(string nameGuid, Identification idType, Form parentForm)
        {
            InitializeComponent();
            _idType = idType;
            _hiddenTree = true;
            _parentForm = parentForm;
            switch (_idType)
            {
                case Identification.LocalName:
                    LocalNameGuid = nameGuid;
                    break;

                case Identification.Scientific:
                    SpeciesGuid = nameGuid;
                    break;
            }
            Names.RowsImported += OnNamesImportRows;
        }

        public CatchLocalNamesForm(Identification identification)
        {
            InitializeComponent();
            _idType = identification;
            Names.RowsImported += OnNamesImportRows;
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
            treeView.Nodes.Clear();
            foreach (var item in Names.Languages)
            {
                var lvi = listView.Items.Add(item.Key, item.Value, null);
            }
            SizeColumns(listView);

            switch (_idType)
            {
                case Identification.LocalName:
                    tbCombo.SelectedIndex = 1;
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
                    tbCombo.SelectedIndex = 0;
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
            tbCombo.SelectionLength = 0;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
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
            _listTitle = lblList.Text;

            global.LoadFormSettings(this);
            if (!_hiddenTree)
            {
                SetUI();
                if (_name?.Length > 0)
                {
                    foreach (TreeNode nd in treeView.Nodes["root"].Nodes)
                    {
                        if (nd.Text == _name)
                        {
                            treeView.SelectedNode = nd;
                            TreeNodeMouseClickEventArgs ee = new TreeNodeMouseClickEventArgs(nd, MouseButtons.Left, 0, 0, 0);
                            OnNodeClick(nd, ee);
                            break;
                        }
                    }
                }
            }
            else
            {
                listView.Anchor = AnchorStyles.None;
                treeView.Hide();
                btnCancel.Hide();
                btnOk.Hide();
                Width -= treeView.Width;
                listView.Location = treeView.Location;
                FormBorderStyle = FormBorderStyle.FixedToolWindow;
                lblTree.Hide();
                lblList.Location = lblTree.Location;
                tbCombo.Visible = false;
                tbComboLabel.Visible = false;
                FillListsNames();
                //listView.Height += (Height - listView.Height-toolBar.Height-lblList.Height-50);
                listView.Height += ClientSize.Height - listView.Height - toolBar.Height - lblList.Height - 15;
                listView.Width += ClientSize.Width - listView.Width - 5;
            }
        }

        private void OnFormClose(object sender, FormClosedEventArgs e)
        {
            _instance = null;

            if (!_hiddenTree)
            {
                global.SaveFormSettings(this);
            }
            else
            {
                switch (_parentForm.GetType().Name)
                {
                    case "AllSpeciesForm":
                        ((AllSpeciesForm)_parentForm).CatchhLocalNamesFormClosed();
                        break;

                    case "MainForm":
                        ((MainForm)_parentForm).CatchhLocalNamesFormClosed();
                        break;
                }
            }
        }

        private void ConfigDropDown()
        {
            contextMenu.Items.Clear();
            if (treeView.SelectedNode != null)
            {
                if (_idType == Identification.Scientific)
                {
                    ToolStripMenuItem menuItem = new ToolStripMenuItem();
                    menuItem.Text = "Details";
                    menuItem.Tag = "showDetail";
                    contextMenu.Items.Add(menuItem);

                    ToolStripSeparator menuSep = new ToolStripSeparator();
                    //menuItem.Text = "-";
                    contextMenu.Items.Add(menuSep);

                    CatchNameURLGenerator.CatchName = treeView.SelectedNode.Text;
                    foreach (var url in CatchNameURLGenerator.URLS)
                    {
                        menuItem = new ToolStripMenuItem();
                        menuItem.Text = url.Key;
                        menuItem.Tag = url.Value;
                        contextMenu.Items.Add(menuItem);
                    }
                }
                else if (_idType == Identification.LocalName)
                {
                    ToolStripMenuItem menuItem = new ToolStripMenuItem();
                    menuItem.Text = "Delete";
                    menuItem.Tag = "deleteItem";
                    menuItem.Enabled = _listedNameCount == 0;
                    contextMenu.Items.Add(menuItem);
                }
            }
        }

        private void FillListsNames()
        {
            listView.Visible = false;
            listView.Items.Clear();
            treeView.Visible = false;
            var items = 0;
            var nameItems = "";
            foreach (var item in Names.Languages)
            {
                StringBuilder sb = new StringBuilder();
                var lvi = listView.Items.Add(item.Key, item.Value, null);
                switch (_idType)
                {
                    case Identification.LocalName:
                        foreach (var names in Names.GetSpeciesNameFromLocalNameLanguage(_localNameGuid, lvi.Name))
                        {
                            sb.Append(names.genus + " " + names.species);
                            sb.Append(", ");
                            _listedNameCount++;
                            items++;
                        }
                        break;

                    case Identification.Scientific:
                        foreach (var names in Names.GetLocalNameFromSpeciesNameLanguage(_speciesGuid, lvi.Name))
                        {
                            sb.Append(names);
                            sb.Append(", ");
                            _listedNameCount++;
                            items++;
                        }
                        break;
                }

                nameItems = sb.ToString().Trim(new char[] { ',', ' ' });
                lvi.SubItems.Add(nameItems);
                lvi.Tag = items;
            }
            SizeColumns(listView, false);
            listView.Visible = true;
            switch (_idType)
            {
                case Identification.LocalName:
                    Text = $"Species names of {LocalName}";
                    lblList.Text = $"List of species names: {items}";
                    break;

                case Identification.Scientific:
                    Text = $"Local names of {SpeciesName}";
                    lblList.Text = $"List of local names: {items}";
                    break;
            }
        }

        private void FillListSpeciesNames()
        {
            listView.Visible = false;
            listView.Items.Clear();
            treeView.Visible = false;
            var items = 0;
            var nameItems = "";
            foreach (var item in Names.Languages)
            {
                StringBuilder sb = new StringBuilder();
                var lvi = listView.Items.Add(item.Key, item.Value, null);
                foreach (var names in Names.GetSpeciesNameFromLocalNameLanguage(_localNameGuid, lvi.Name))
                {
                    sb.Append(names.genus + " " + names.species);
                    sb.Append(", ");
                    _listedNameCount++;
                    items++;
                }
                nameItems = sb.ToString().Trim(new char[] { ',', ' ' });
                lvi.SubItems.Add(nameItems);
                lvi.Tag = items;
            }
            SizeColumns(listView, false);
            listView.Visible = true;
            Text = $"Species names of {LocalName}";
            lblList.Text = $"List of local names: {items}";
        }

        private void FillListLocalNames()
        {
            listView.Visible = false;
            listView.Items.Clear();
            treeView.Visible = false;
            var items = 0;
            var nameItems = "";
            foreach (var item in Names.Languages)
            {
                StringBuilder sb = new StringBuilder();
                var lvi = listView.Items.Add(item.Key, item.Value, null);
                foreach (var names in Names.GetLocalNameFromSpeciesNameLanguage(_speciesGuid, lvi.Name))
                {
                    sb.Append(names);
                    sb.Append(", ");
                    _listedNameCount++;
                    items++;
                }
                nameItems = sb.ToString().Trim(new char[] { ',', ' ' });
                lvi.SubItems.Add(nameItems);
                lvi.Tag = items;
            }
            SizeColumns(listView, false);
            listView.Visible = true;
            Text = $"Local names of {SpeciesName}";
            lblList.Text = $"List of local names: {items}";
        }

        private void OnNodeClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            listView.Hide();
            ConfigDropDown();
            _listedNameCount = 0;
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
                                sb.Append(names.genus);
                                sb.Append(" ");
                                sb.Append(names.species);
                                sb.Append(", ");

                                _listedNameCount++;
                                items++;
                            }
                            break;

                        case Identification.Scientific:
                            foreach (var names in Names.GetLocalNameFromSpeciesNameLanguage(e.Node.Name, lvi.Name))
                            {
                                sb.Append(names);
                                sb.Append(", ");
                                _listedNameCount++;
                                items++;
                            }
                            break;
                    }
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
                lblList.Text = $"{_listTitle} of  {e.Node.Text} ({_listedNameCount})";
            else
                lblList.Text = _listTitle;

            listView.Show();
        }

        private void RefreshNameLists()
        {
            treeView.SelectedNode = treeView.Nodes["root"];
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
            if (_hiddenTree)
            {
                var name = "";
                if (_idType == Identification.Scientific)
                {
                    name = SpeciesName;
                }
                else
                {
                    name = LocalName;
                }
                CatchLocalNameSelectedForm clnsf = CatchLocalNameSelectedForm.GetInstance(_idType, listView.SelectedItems[0].Text, name, this);
                if (clnsf.Visible)
                {
                    clnsf.NewSelection(_idType, listView.SelectedItems[0].Text, name);
                    clnsf.BringToFront();
                }
                else
                {
                    clnsf.Show(this);
                }
            }
            else
            {
                if (treeView.SelectedNode.Name != "root" && listView.SelectedItems.Count > 0)
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

        private void OnContextMenuClick(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Tag.ToString() == "showDetail")
            {
                SpeciesNameForm snf = new SpeciesNameForm(treeView.SelectedNode.Text, this);
                snf.ReadOnly = true;
                snf.ShowDialog(this);
            }
            else if (e.ClickedItem.Tag.ToString() == "deleteItem")
            {
                var nodeKey = treeView.SelectedNode.Name;
                if (Names.DeleteLocalName(nodeKey, treeView.SelectedNode.Text))
                {
                    treeView.Nodes["root"].Nodes.RemoveByKey(nodeKey);
                }
            }
            else
            {
                Process.Start(e.ClickedItem.Tag.ToString());
            }
        }

        private void OnTreeMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenu.Show(treeView, e.Location.X, e.Location.Y);
            }
        }

        private void OnToolBarItemClick(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "tbClose":
                    Close();
                    break;

                case "tbAdd":
                    if (_hiddenTree)
                    {
                        LocalNameSciNameEditForm lnsef = LocalNameSciNameEditForm.GetInstance(SpeciesName, SpeciesGuid, _idType, this);
                        if (lnsef.Visible)
                        {
                            lnsef.BringToFront();
                        }
                        else
                        {
                            lnsef.Show(this);
                        }
                    }
                    else if (treeView.SelectedNode.Name != "root")
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

                case "tbRemove":
                    switch (_activeControl)
                    {
                        case "TreeView":
                            if (treeView.SelectedNode.Name != "root" & _listedNameCount == 0)
                            {
                            }
                            break;

                        case "ListView":
                            break;
                    }
                    break;

                case "tbEdit":
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
                    break;

                case "tbExport":
                case "tbImport":
                    var actionType = ExportImportAction.ActionExport;
                    if (e.ClickedItem.Name == "tbImport")
                    {
                        actionType = ExportImportAction.ActionImport;
                    }
                    using (ExportImportDialogForm eidf = new ExportImportDialogForm(ExportImportDataType.CatchNametDataSelect, actionType))
                    {
                        eidf.ShowDialog(this);
                        if (eidf.DialogResult == DialogResult.OK)
                        {
                            if ((eidf.Selection & ExportImportDataType.CatchLocalNames) == ExportImportDataType.CatchLocalNames)
                            {
                                switch (actionType)
                                {
                                    case ExportImportAction.ActionExport:
                                        ExportData(ExportImportDataType.CatchLocalNames, "Export local names of catch");
                                        break;

                                    case ExportImportAction.ActionImport:
                                        ImportData(ExportImportDataType.CatchLocalNames, "Import local names of catch");
                                        break;
                                }
                            }
                            if ((eidf.Selection & ExportImportDataType.LocalNameLanguages) == ExportImportDataType.LocalNameLanguages)
                            {
                                switch (actionType)
                                {
                                    case ExportImportAction.ActionExport:
                                        ExportData(ExportImportDataType.LocalNameLanguages, "Export local name languages");
                                        break;

                                    case ExportImportAction.ActionImport:
                                        ImportData(ExportImportDataType.LocalNameLanguages, "Import local name languages");
                                        break;
                                }
                            }
                            if ((eidf.Selection & ExportImportDataType.CatchLocalNameSpeciesNamePair) == ExportImportDataType.CatchLocalNameSpeciesNamePair)
                            {
                                switch (actionType)
                                {
                                    case ExportImportAction.ActionExport:
                                        ExportData(ExportImportDataType.CatchLocalNameSpeciesNamePair, "Export local names and species names");
                                        break;

                                    case ExportImportAction.ActionImport:
                                        ImportData(ExportImportDataType.CatchLocalNameSpeciesNamePair, "Import local names and species names");
                                        break;
                                }
                            }
                        }
                    }
                    break;
            }
        }

        private bool ImportData(ExportImportDataType dataType, string title)
        {
            var result = Names.Languages;
            FileDialogHelper.Title = title;
            FileDialogHelper.DialogType = FileDialogType.FileOpen;
            FileDialogHelper.DataFileType = DataFileType.Text | DataFileType.XML | DataFileType.HTML;
            FileDialogHelper.ShowDialog();
            var fileName = FileDialogHelper.FileName;
            if (fileName.Length > 0)
            {
                switch (dataType)
                {
                    case ExportImportDataType.CatchLocalNames:
                        GetImportedRows(fileName, ExportImportDataType.CatchLocalNames);
                        break;

                    case ExportImportDataType.CatchLocalNameSpeciesNamePair:

                        switch (Path.GetExtension(fileName))
                        {
                            case ".htm":
                            case ".html":
                                using (HTMLTableSelectColumnsForm htmlColForm = new HTMLTableSelectColumnsForm(fileName, CatchNameDataType.CatchSpeciesLocalNamePair))
                                {
                                    DialogResult dr = htmlColForm.ShowDialog(this);
                                    if (dr == DialogResult.OK)
                                    {
                                        GetImportedRows(fileName, htmlColForm.SpeciesNameColumn, htmlColForm.LocalNameColumn, htmlColForm.LanguageColumn);
                                    }
                                }
                                break;

                            default:
                                //var importedCount = Names.ImportLocalNamestoScientificNames(fileName);
                                //MessageBox.Show($"{importedCount} local names-species names-languages saved to the database");
                                GetImportedRows(fileName, ExportImportDataType.CatchLocalNameSpeciesNamePair);
                                break;
                        }
                        //switch (Path.GetExtension(fileName))
                        //{
                        //    case ".txt":

                        //        int fail = 0;
                        //        var nameCount = Names.ImportLocalNamestoScientificNames(fileName, ref fail);
                        //        var msg = $"{nameCount} local names to scientific name pairs were added to the database";
                        //        if (fail > 0)
                        //        {
                        //            msg += $"\r\n There were also {fail} records not saved because of key violations";
                        //        }
                        //        MessageBox.Show(msg, "Fininshed importing local name - scientific name pairs", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //        break;

                        //}
                        break;

                    case ExportImportDataType.LocalNameLanguages:
                        var savedLanguages = Names.ImportLanguages(fileName);
                        MessageBox.Show($"{savedLanguages} languages saved to the database");
                        break;
                }
                SetTreeItem("root");
                SetUI();
            }
            return true;
        }

        private async void GetImportedRows(string fileName, int speciesColumn, int localNameColumn, int languageColumn)
        {
            int result = await Names.ImportFromHTMLLocalNamestoScientificNamesAsync(fileName, speciesColumn, localNameColumn, languageColumn);
            lblList.Text = "List of species names";
            MessageBox.Show($"{_rowsImported} local name - species name pairs were saved to the database", "Import successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void GetImportedRows(string fileName, ExportImportDataType dataType)
        {
            switch (dataType)
            {
                case ExportImportDataType.CatchLocalNames:
                    int result = await Names.ImportLocalNamesAsync(fileName);
                    break;

                case ExportImportDataType.CatchLocalNameSpeciesNamePair:
                    result = await Names.ImportLocalNamestoScientificNamesAsync(fileName);
                    break;
            }

            lblList.Text = "List of species names";
            MessageBox.Show($"{_rowsImported} local name - species name pairs were saved to the database", "Import successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool ExportData(ExportImportDataType dataType, string title)
        {
            var success = false;
            FileDialogHelper.Title = title;
            FileDialogHelper.DialogType = FileDialogType.FileSave;
            FileDialogHelper.DataFileType = DataFileType.Text | DataFileType.XML | DataFileType.CSV;
            FileDialogHelper.ShowDialog();
            var fileName = FileDialogHelper.FileName;
            switch (Path.GetExtension(fileName))
            {
                case ".txt":

                    break;

                case ".XML":
                case ".xml":
                    switch (dataType)
                    {
                        case ExportImportDataType.LocalNameLanguages:

                            var count = Names.Languages.Count;

                            if (count > 0)
                            {
                                var n = 0;
                                XmlWriter writer = XmlWriter.Create(fileName);
                                writer.WriteStartDocument();
                                writer.WriteStartElement("Languages");
                                foreach (var language in Names.Languages)
                                {
                                    writer.WriteStartElement("Language");
                                    writer.WriteAttributeString("guid", language.Key);
                                    writer.WriteString(language.Value);
                                    if (count == 1)
                                    {
                                        writer.WriteEndDocument();
                                    }
                                    else
                                    {
                                        if (n < (count - 1))
                                        {
                                            writer.WriteEndElement();
                                        }
                                        else
                                        {
                                            writer.WriteEndDocument();
                                        }
                                    }
                                    n++;
                                }
                                writer.Close();
                                if (n > 0 && count > 0)
                                {
                                    MessageBox.Show($"Succesfully exported {count} languages", "Export successful");
                                }
                            }
                            break;

                        case ExportImportDataType.CatchLocalNames:

                            count = Names.LocalNameListDict.Count;
                            if (count > 0)
                            {
                                var n = 0;
                                XmlWriter writer = XmlWriter.Create(fileName);
                                writer.WriteStartDocument();
                                writer.WriteStartElement("LocalNames");
                                foreach (var localName in Names.LocalNameListDict)
                                {
                                    writer.WriteStartElement("LocalName");
                                    writer.WriteAttributeString("guid", localName.Key);
                                    writer.WriteString(localName.Value);
                                    if (count == 1)
                                    {
                                        writer.WriteEndDocument();
                                    }
                                    else
                                    {
                                        if (n < (count - 1))
                                        {
                                            writer.WriteEndElement();
                                        }
                                        else
                                        {
                                            writer.WriteEndDocument();
                                        }
                                    }
                                    n++;
                                }
                                writer.Close();
                                if (n > 0 && count > 0)
                                {
                                    MessageBox.Show($"Succesfully exported {count} local names", "Export successful");
                                }
                            }

                            break;

                        case ExportImportDataType.CatchLocalNameSpeciesNamePair:
                            var list = Names.GetLocalNameSpeciesNameLanguage();
                            count = list.Count;
                            if (count > 0)
                            {
                                var n = 0;
                                XmlWriter writer = XmlWriter.Create(fileName);
                                writer.WriteStartDocument();
                                writer.WriteStartElement("LocalNamesSpeciesNamesLanguages");
                                foreach (var item in list)
                                {
                                    writer.WriteStartElement("LocalNameSpeciesNameLanguage");
                                    writer.WriteAttributeString("localNameGuid", item.localNameGuid);
                                    writer.WriteAttributeString("speciesNameGuid", item.speciesNameGuid);
                                    writer.WriteAttributeString("languageGuid", item.languageGuid);
                                    if (count == 1)
                                    {
                                        writer.WriteEndDocument();
                                    }
                                    else
                                    {
                                        if (n < (count - 1))
                                        {
                                            writer.WriteEndElement();
                                        }
                                        else
                                        {
                                            writer.WriteEndDocument();
                                        }
                                    }
                                    n++;
                                }
                                writer.Close();
                                if (n > 0 && count > 0)
                                {
                                    MessageBox.Show($"Succesfully exported {count} local names - species names - languages", "Export successful");
                                }
                            }
                            break;
                    }
                    break;

                case ".csv":

                    break;
            }
            return true;
        }

        private void OnFormResize(object sender, EventArgs e)
        {
            tbCombo.Width = 200;
            tbCombo.SelectionLength = 0;
        }

        private void OnToolBarComboSelectedIndexChange(object sender, EventArgs e)
        {
            switch (tbCombo.SelectedIndex)
            {
                case 1:
                    IDType = Identification.LocalName;
                    break;

                case 0:
                    IDType = Identification.Scientific;
                    break;
            }
        }

        private void OnControlMouseDown(object sender, MouseEventArgs e)
        {
            _activeControl = sender.GetType().Name;
        }
    }
}
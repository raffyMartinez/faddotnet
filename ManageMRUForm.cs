using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace FAD3
{

    public partial class ManageMRUForm : Form
    {
        private List<FileInfo> _FileList;
        frmMain _parent_form;

        public ManageMRUForm()
        {
            InitializeComponent();
        }

        public frmMain Parent_form
        {
            get { return _parent_form; }
            set
            {
                _parent_form = value;
                _FileList = _parent_form.MRUList.FileList;

                if (_FileList.Count > 0)
                {
                    for (int i = 0; i < _FileList.Count; i++)
                    {
                        listBoxFiles.Items.Add(_FileList[i].FullName);
                    }
                }

            }
        }

        private void OnMenuItem_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "menuOpen":

                    break;
                case "menuRemove":
                    if (listBoxFiles.Items.Count > 0)
                    {
                        listBoxFiles.Items.Remove(listBoxFiles.Items[listBoxFiles.SelectedIndex]);
                        ((ContextMenuStrip)sender).Items["menuRemove"].Enabled = listBoxFiles.Items.Count > 0;
                        ((ContextMenuStrip)sender).Items["menuOpen"].Enabled = listBoxFiles.Items.Count > 0;
                    }
                    break;
            }

        }

        private void Onbutton_Click(object sender, EventArgs e)
        {
            ((Button)sender).With(o =>
            {
                switch (o.Name)
                {
                    case "buttonOK":
                        _FileList.Clear();
                        foreach (var item in listBoxFiles.Items)
                            _FileList.Add(new FileInfo(item.ToString()));

                        _parent_form.MRUList.FileList = _FileList;
                        this.Close();
                        break;
                    case "buttonCancel":
                        this.Close();
                        break;
                }
            });
        }
    }
}

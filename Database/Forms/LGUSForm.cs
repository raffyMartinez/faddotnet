using FAD3.Database.Classes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FAD3.Database.Forms
{
    public partial class LGUSForm : Form
    {
        private static LGUSForm _instance;
        private TreeNode _nodeParent;
        private int _provinceCount;
        private int _lguCount;
        private FisheriesInventoryLevel _level;

        public static LGUSForm GetInstance()
        {
            if (_instance == null) _instance = new LGUSForm();
            return _instance;
        }

        public LGUSForm()
        {
            InitializeComponent();
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

        private void OnFormLoad(object sender, EventArgs e)
        {
            lv.Columns.Add("Property");
            lv.Columns.Add("Value");
            lv.View = View.Details;
            SizeColumns(lv);

            treeLGUs.Nodes.Add("root", "Provinces and LGUs").Tag = "root";
            foreach (var item in LGUs.GetLGUTree())
            {
                var provNode = treeLGUs.Nodes["root"].Nodes.Add(item.Key.ToString(), item.Value.provinceName);
                _provinceCount++;
                provNode.Tag = "province";
                foreach (var lgu in item.Value.lgus)
                {
                    var munNode = provNode.Nodes.Add(lgu.Key.ToString(), lgu.Value);
                    munNode.Tag = "municipality";
                    _lguCount++;
                }
            }
            global.LoadFormSettings(this);
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
            global.SaveFormSettings(this);
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            Point p = treeLGUs.PointToClient(new Point(e.X, e.Y));
            TreeNode node = treeLGUs.GetNodeAt(p.X, p.Y);
            var tag = node.Tag.ToString();
            Text = tag + " " + node.Text;
            if (node.PrevVisibleNode != null)
            {
                node.PrevVisibleNode.BackColor = Color.White;
            }
            if (node.NextVisibleNode != null)
            {
                node.NextVisibleNode.BackColor = Color.White;
            }
            if (treeLGUs.SelectedNode.Tag.ToString() == "municipality" && node.Tag.ToString() == "province" && node.Text != treeLGUs.SelectedNode.Parent.Text)
            {
                node.BackColor = Color.Aquamarine;
            }
        }

        private void OnItemDrag(object sender, ItemDragEventArgs e)
        {
            _nodeParent = ((TreeNode)e.Item).Parent;
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void OnDragDrop(object sender, DragEventArgs e)
        {
            // Retrieve the client coordinates of the drop location.
            Point targetPoint = treeLGUs.PointToClient(new Point(e.X, e.Y));

            // Retrieve the node at the drop location.
            TreeNode targetNode = treeLGUs.GetNodeAt(targetPoint);

            // Retrieve the node that was dragged.
            //TreeNode draggedNode = e.Data.GetData(typeof(TreeNode));
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

            if (targetNode.Tag.ToString() == "province" && draggedNode.Parent.Text != targetNode.Text)
            {
                // Sanity check
                if (draggedNode == null)
                {
                    return;
                }

                // Did the user drop on a valid target node?
                if (targetNode == null)
                {
                    // The user dropped the node on the treeview control instead
                    // of another node so lets place the node at the bottom of the tree.
                    draggedNode.Remove();
                    treeLGUs.Nodes.Add(draggedNode);
                    draggedNode.Expand();
                }
                else
                {
                    TreeNode parentNode = targetNode;

                    // Confirm that the node at the drop location is not
                    // the dragged node and that target node isn't null
                    // (for example if you drag outside the control)
                    if (!draggedNode.Equals(targetNode) && targetNode != null)
                    {
                        bool canDrop = true;

                        // Crawl our way up from the node we dropped on to find out if
                        // if the target node is our parent.
                        while (canDrop && (parentNode != null))
                        {
                            canDrop = !Object.ReferenceEquals(draggedNode, parentNode);
                            parentNode = parentNode.Parent;
                        }

                        // Is this a valid drop location?
                        if (canDrop)
                        {
                            // Yes. Move the node, expand it, and select it.
                            var result = MessageBox.Show($"Are you sure you want to transfer the LGU of {draggedNode.Text} to the Province of {targetNode.Text}?",
                                                            "Confirmation needed", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            //if (result == DialogResult.Yes && Landingsite.MoveToLandingSite(sourceTag.Item1, destinationTag.Item1))
                            if (result == DialogResult.Yes && LGUs.MoveLGuToProvince(int.Parse(draggedNode.Name), int.Parse(targetNode.Name)))
                            {
                                targetNode.Nodes.Add(draggedNode);
                                draggedNode.Nodes.Clear();
                                //RefreshLandingSiteNodeNodes(targetNode);
                                targetNode.Expand();
                            }
                            targetNode.BackColor = Color.White;
                        }
                    }
                }

                // Optional: Select the dropped node and navigate (however you do it)
                treeLGUs.SelectedNode = draggedNode;
                // NavigateToContent(draggedNode.Tag);
            }
            else
            {
                MessageBox.Show("Can only move an LGU to a different province", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ConfigContextMenu(Control source)
        {
            contextMenu.Items.Clear();
            if (source.GetType().Name == "TreeView")
            {
                switch (treeLGUs.SelectedNode.Tag.ToString())
                {
                    case "root":
                        var item = contextMenu.Items.Add("Add a province");
                        item.Name = "menuAddProvince";
                        break;

                    case "municipality":
                        break;

                    case "province":
                        item = contextMenu.Items.Add("Add an LGU");
                        item.Name = "menuAddLGU";
                        item = contextMenu.Items.Add("Move LGUs");
                        item.Name = "menuMoveLGUs";
                        break;
                }
            }
            else if (source.GetType().Name == "ListView")
            {
            }
        }

        private void OnMenuItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "menuAddProvince":
                case "menuAddLGU":

                    using (LGUItemForm lif = new LGUItemForm(_level))
                    {
                        lif.ShowDialog(this);
                        if (lif.DialogResult == DialogResult.OK)
                        {
                            switch (_level)
                            {
                                case FisheriesInventoryLevel.Root:
                                    break;

                                case FisheriesInventoryLevel.Province:
                                    break;

                                case FisheriesInventoryLevel.Municipality:
                                    break;
                            }
                        }
                        else
                        {
                        }
                    }
                    break;

                case "menuMoveLGUs":
                    break;
            }
        }

        private void OnTreeNodeClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            lv.Items.Clear();
            ListViewItem lvi = new ListViewItem();
            switch (e.Node.Tag.ToString())
            {
                case "root":
                    lvi = lv.Items.Add("Total number of provinces");
                    lvi.SubItems.Add(_provinceCount.ToString());
                    lvi = lv.Items.Add("Total number of LGUs");
                    lvi.SubItems.Add(_lguCount.ToString());
                    _level = FisheriesInventoryLevel.Root;
                    break;

                case "province":

                    var n = 0;
                    foreach (TreeNode node in e.Node.Nodes)
                    {
                        if (n == 0)
                        {
                            lvi = lv.Items.Add(node.Name, "LGUs", null);
                        }
                        else
                        {
                            lvi = lv.Items.Add(node.Name, "", null);
                        }
                        lvi.SubItems.Add(node.Text);
                        n++;
                    }
                    _level = FisheriesInventoryLevel.Province;
                    break;

                case "municipality":
                    _level = FisheriesInventoryLevel.Municipality;
                    break;
            }
            SizeColumns(lv, false);
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ConfigContextMenu((Control)sender);
            }
        }
    }
}
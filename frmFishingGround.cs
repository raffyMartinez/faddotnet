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
    public partial class frmFishingGround : Form
    {
        public frmFishingGround()
        {
            InitializeComponent();
        }

        string _AOIGuid = "";
        frmSamplingDetail _Parent_form;
        FishingGrid.fadGridType _gt;
        static frmFishingGround _instance;
        int _mouseX;
        int _mouseY;
        List<string> _FishingGrounds;
        ListViewItem _selectedItem;


        public List<string> FishingGrounds
        {
            get { return _FishingGrounds; }
            set { _FishingGrounds = value; }
        }


        public frmSamplingDetail Parent_form
        {
            get { return _Parent_form; }
            set { _Parent_form = value; }
        }

        public static frmFishingGround GetInstance()
        {
            if (_instance == null) _instance = new frmFishingGround();
            return _instance;
        }

        public string AOIGuid
        {
            get { return _AOIGuid; }
            set
            {
                _AOIGuid = value;
                //_gt = FishingGrid.SetupFishingGrid(_AOIGuid);
            }
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {

            _instance = null;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            lvGrids.With(o =>
            {
                o.View = View.Details;
                var c = o.Columns.Add("Grid");
                c.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                o.HeaderStyle = ColumnHeaderStyle.None;
            });

            if (FishingGrid.GridType == FishingGrid.fadGridType.gridTypeGrid25)
            {
                tabFG.TabPages["tabGrid25"].Select();
                textBoxZone.Text = FishingGrid.UTMZoneName;
                foreach (var item in _FishingGrounds)
                {
                    lvGrids.Items.Add(item).With(o => { o.Name = item; });
                }
            }
            else if (FishingGrid.GridType == FishingGrid.fadGridType.gridTypeOther)
            {
                tabFG.TabPages["tabGridOther"].Select();

            }
        }

        private void FromListViewToTextBox(string lvText)
        {

            var arr = lvText.Split('-');
            textBoxGridNo.Text = arr[0];
            textBoxColumn.Text = arr[1].Substring(0, 1);
            textBoxRow.Text = arr[1].Substring(1, arr[1].Length - 1);
        }

        private void lvGrids_DoubleClick(object sender, EventArgs e)
        {
            var item = lvGrids.HitTest(_mouseX, _mouseY);
            if (item != null)
            {
                FromListViewToTextBox(item.Item.Text);
                _selectedItem = item.Item;
            }
        }

        private void lvGrids_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseX = e.X;
            _mouseY = e.Y;
            var item = lvGrids.HitTest(_mouseX, _mouseY);

            if (item != null)
                _selectedItem = item.Item;
        }

        private void OntextBoxValidating(object sender, CancelEventArgs e)
        {
            var s = ((TextBox)sender).Text;
            var msg = "";
            if (s.Length > 0)
            {
                switch (((TextBox)sender).Name)
                {
                    case "textBoxGridNo":
                        if (FishingGrid.MajorGridFound(s) == false)
                            msg = "Grid number not found in the maps";
                        break;
                    case "textBoxColumn":
                        if (s.Length == 1)
                        {
                            var c = s.ToUpper().ToArray();
                            if (c[0] < 'A' || c[0] > 'Y')
                            {
                                msg = "Grid column not found";
                            }
                         ((TextBox)sender).Text = s.ToUpper();
                        }
                        else
                        {
                            msg = "Grid column not found";
                        }
                        break;
                    case "textBoxRow":
                        try
                        {
                            var n = int.Parse(s);
                            if (n < 1 || n > 25)
                                msg = "Expected value is a number from 1 to 25";
                        }
                        catch
                        {
                            msg = "Expected value is a number from 1 to 25";
                        }
                        break;
                }
            }

            if (msg.Length > 0)
            {
                e.Cancel = true;
                MessageBox.Show(msg, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void OnbuttonGrid25_Click(object sender, EventArgs e)
        {
            var msg = "";
            var GridName = "";
            switch (((Button)sender).Name)
            {
                case "buttonAdd":
                    if (textBoxColumn.Text.Length > 0 &&
                        textBoxGridNo.Text.Length > 0 &&
                        textBoxRow.Text.Length > 0)
                    {
                        GridName = textBoxGridNo.Text + "-" + textBoxColumn.Text + textBoxRow.Text;

                        if (lvGrids.Items.ContainsKey(GridName))
                        {
                            msg = "Grid name already exists. Please use another";
                        }
                        else
                        {
                            if (FishingGrid.MinorGridIsInland(GridName))
                                msg = "Inputted grid is inland";
                            else
                            {
                                if (_selectedItem != null)
                                    lvGrids.Items[_selectedItem.Name].With(o =>
                                    {
                                        o.Text = GridName;
                                        o.Name = GridName;
                                        _selectedItem = null;
                                    });
                                else
                                {
                                    lvGrids.Items.Add(GridName).With(o => { o.Name = GridName; });
                                }
                                textBoxColumn.Text = "";
                                textBoxRow.Text = "";
                                textBoxGridNo.Text = "";
                                textBoxGridNo.Select();
                            }
                        }
                    }
                    else
                    {
                        msg = "Please fill up all fields";
                    }
                    break;
                case "buttonRemove":
                    lvGrids.Items.Remove(_selectedItem);
                    break;
                case "buttonRemoveAll":
                    lvGrids.Clear();
                    break;
            }

            if (msg.Length > 0)
            {
                MessageBox.Show(msg, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


        }

        private void OnTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            ((TextBox)sender).With(o =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = e.SuppressKeyPress = true;
                    switch (o.Name)
                    {
                        case "textBoxGridNo":
                            textBoxColumn.Select();
                            break;
                        case "textBoxColumn":
                            textBoxRow.Select();
                            break;
                        case "textBoxRow":
                            buttonAdd.Select();
                            break;
                    }
                }
            });
        }

        private void Onbutton_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonOK":
                    _FishingGrounds.Clear();
                    foreach (ListViewItem item in lvGrids.Items)
                    {
                        _FishingGrounds.Add(item.Text);
                    }
                    _Parent_form.FishingGrounds = _FishingGrounds;
                    this.Close();
                    break;
                case "buttonCancel":
                    this.Close();
                    break;
            }
        }
    }
}

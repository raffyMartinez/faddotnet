using FAD3.Database.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace FAD3
{
    public partial class FishingGroundForm : Form
    {
        private static FishingGroundForm _instance;
        private int _mouseX;
        private int _mouseY;
        private List<string> _FishingGrounds;
        private ListViewItem _selectedItem;
        private SamplingForm _parent_form;

        public FishingGroundForm(string AOIGuid, SamplingForm Parent)
        {
            InitializeComponent();
            this.AOIGuid = AOIGuid;
            _parent_form = Parent;
        }

        public List<string> FishingGrounds
        {
            get { return _FishingGrounds; }
            set { _FishingGrounds = value; }
        }

        public SamplingForm Parent_form
        {
            get { return _parent_form; }
            set { _parent_form = value; }
        }

        public static FishingGroundForm GetInstance(string AOIGuid, SamplingForm Parent)
        {
            if (_instance == null) _instance = new FishingGroundForm(AOIGuid, Parent);
            return _instance;
        }

        public string AOIGuid { get; } = "";

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
            global.SaveFormSettings(this);
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

            if (FishingGrid.GridType == fadGridType.gridTypeGrid25)
            {
                tabFG.TabPages["tabGrid25"].Select();
                textBoxZone.Text = FishingGrid.UTMZoneName;
                foreach (var item in _FishingGrounds)
                {
                    lvGrids.Items.Add(item, item, null);
                    if (global.MapIsOpen)
                    {
                        global.MappingForm.MapFishingGround(item, FishingGrid.UTMZone);
                    }
                }
            }
            else if (FishingGrid.GridType == fadGridType.gridTypeOther)
            {
                tabFG.TabPages["tabGridOther"].Select();
            }

            global.LoadFormSettings(this, true);
            global.MapperOpen += OnMapperOpened;
        }

        private void OnMapperOpened(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvGrids.Items)
            {
                global.MappingForm.MapFishingGround(item.Text, FishingGrid.UTMZone);
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
            var gridName = "";
            switch (((Button)sender).Name)
            {
                case "buttonAdd":
                    if (textBoxColumn.Text.Length > 0 &&
                        textBoxGridNo.Text.Length > 0 &&
                        textBoxRow.Text.Length > 0)
                    {
                        gridName = textBoxGridNo.Text + "-" + textBoxColumn.Text + textBoxRow.Text;

                        if (lvGrids.Items.ContainsKey(gridName))
                        {
                            msg = "Grid name already exists. Please use another";
                        }
                        else
                        {
                            if (FishingGrid.MinorGridIsInland(gridName))
                            {
                                if (global.MapIsOpen)
                                {
                                    global.MappingForm.MapFishingGround(gridName, FishingGrid.UTMZone, gridName, true);
                                    DialogResult dr = MessageBox.Show($"{gridName} is located inland\r\n\r\n" +

                                        "Accept this location?",
                                          "Verify fishing ground location",
                                          MessageBoxButtons.YesNo,
                                          MessageBoxIcon.Information);

                                    if (dr == DialogResult.Yes)
                                    {
                                        var lvi = lvGrids.Items.Add(gridName, gridName, null);
                                        lvi.Tag = "new";
                                        textBoxColumn.Text = "";
                                        textBoxRow.Text = "";
                                        textBoxGridNo.Text = "";
                                        textBoxGridNo.Select();
                                    }
                                    else
                                    {
                                        global.MappingForm.MapLayersHandler.RemoveLayer(gridName);
                                    }
                                }
                                else
                                {
                                    msg = $"{gridName} is not accepted because it is located inland";
                                }
                            }
                            else
                            {
                                if (global.MapIsOpen)
                                {
                                    global.MappingForm.MapFishingGround(gridName, FishingGrid.UTMZone, gridName);
                                }
                                if (_selectedItem != null)
                                    lvGrids.Items[_selectedItem.Name].With(o =>
                                    {
                                        o.Text = gridName;
                                        o.Name = gridName;
                                        _selectedItem = null;
                                    });
                                else
                                {
                                    var lvi = lvGrids.Items.Add(gridName, gridName, null);
                                    lvi.Tag = "new";
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
                    if (global.MapIsOpen)
                    {
                        global.MappingForm.MapLayersHandler.RemoveLayer(_selectedItem.Text);
                    }
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
                    Parent_form.FishingGrounds = _FishingGrounds;
                    Close();
                    break;

                case "buttonCancel":
                    if (global.MapIsOpen)
                    {
                        foreach (ListViewItem item in lvGrids.Items)
                        {
                            if (item.Tag?.ToString() == "new")
                            {
                                global.MappingForm.MapLayersHandler.RemoveLayer(item.Text);
                            }
                        }
                    }
                    Close();
                    break;

                case "buttonGrids":
                    FishingGroundExtentsForm fgf = new FishingGroundExtentsForm(_parent_form.Parent_Form.TargetAreaGuid);
                    fgf.Show(this);
                    break;
            }
        }
    }
}
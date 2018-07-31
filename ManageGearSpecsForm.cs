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
    public partial class ManageGearSpecsForm : Form
    {
        string _GearVarGuid;
        string _GearVarName;
        List<ManageGearSpecsClass.GearSpecification> _GearSpecs = new List<ManageGearSpecsClass.GearSpecification>();
        ListViewHitTestInfo _HitItem;
        int HiddenColIndex;
        List<string> _DeletedSpecsRow = new List<string>();

        public ManageGearSpecsForm(string GearVarGuid, string GearVarName = null)
        {
            InitializeComponent();
            _GearVarGuid = GearVarGuid;
            if (GearVarName == null)
                ManageGearSpecsClass.GearVarGuid(GearVarGuid);
            else
            {
                _GearVarName = GearVarName;
                ManageGearSpecsClass.GearVariation(_GearVarGuid, _GearVarName);
            }

            _GearSpecs = ManageGearSpecsClass.GearSpecifications;
        }

        private void ManageGearSpecs_Load(object sender, EventArgs e)
        {
            labelTitle.Text += " " + _GearVarName;
            comboBoxType.Items.Add("Numeric");
            comboBoxType.Items.Add("Text");
            comboBoxType.Items.Add("Yes/No");
            comboBoxType.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBoxType.AutoCompleteMode = AutoCompleteMode.Suggest;

            lvSpecs.With(o =>
            {
                o.View = View.Details;
                var c = o.Columns.Add("Property");
                c.Width = frmMain.TextWidth(c.Text, lvSpecs.Font);
                o.Columns.Add("Type");
                o.Columns.Add("Notes");
                c = o.Columns.Add("DataStatus");
                c.Width = 0;
                HiddenColIndex = c.Index;
                o.FullRowSelect = true;

                lvSpecs.ColumnWidthChanging += OnlvSpecs_ColumnWidthChanging;
            });

            if (_GearSpecs != null)
                foreach (ManageGearSpecsClass.GearSpecification item in _GearSpecs)
                {
                    var lvi = new ListViewItem(new string[] { item.Property, item.Type, item.Notes, item.DataStatus.ToString() });
                    lvi.Name = item.RowGuid;
                    lvSpecs.Items.Add(lvi);
                }

            AdjustColWidths();

        }

        private void AdjustColWidths()
        {
            foreach (ColumnHeader c in lvSpecs.Columns)
            {
                if (c.Text != "DataStatus")
                {
                    var w = c.Width;
                    c.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                    if (c.Width < w) c.Width = w;
                }
            }
        }

        private void Onbutton_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonOK":
                    if (SaveProperties()) Close();
                    break;
                case "buttonCancel":
                    Close();
                    break;
                case "buttonAdd":
                    AddPropertyToList();
                    ClearFields();
                    break;
                case "buttonRemove":
                    if (lvSpecs.SelectedItems.Count > 0)
                    {
                        _DeletedSpecsRow.Add(lvSpecs.SelectedItems[0].Name);
                        lvSpecs.Items.Remove(lvSpecs.SelectedItems[0]);
                    }
                    ClearFields();
                    break;
                case "buttonRemoveAll":
                    lvSpecs.Clear();
                    ClearFields();
                    break;
            }
        }

        private void ClearFields()
        {
            textBoxNotes.Clear();
            textBoxPropertyName.Clear();
            comboBoxType.Text = "";
        }

        private bool SaveProperties()
        {
            _GearSpecs.Clear();
            foreach (ListViewItem lvi in lvSpecs.Items)
            {
                global.fad3DataStatus ds;
                Enum.TryParse(lvi.SubItems[3].Text, out ds);
                var spec = new ManageGearSpecsClass.GearSpecification
                {
                    Property = lvi.Text,
                    Type = lvi.SubItems[1].Text,
                    Notes = lvi.SubItems[2].Text,
                    DataStatus = ds,
                    RowGuid = lvi.Name,
                };

                if (spec.Sequence != lvi.Index + 1 && ds != global.fad3DataStatus.statusNew)
                {
                    spec.DataStatus = global.fad3DataStatus.statusEdited;
                    spec.Sequence = lvi.Index + 1;
                }

                _GearSpecs.Add(spec);
            }

            foreach (var item in _DeletedSpecsRow)
            {
                var spec = new ManageGearSpecsClass.GearSpecification
                {
                    RowGuid = item,
                    DataStatus = global.fad3DataStatus.statusForDeletion
                };
                _GearSpecs.Add(spec);
            }

            return ManageGearSpecsClass.SaveGearSpecs(_GearSpecs);
        }

        private void AddPropertyToList()
        {
            if (textBoxPropertyName.TextLength > 0 && comboBoxType.Text != "")
            {
                if (lvSpecs.SelectedItems.Count == 0)
                {
                    var lvi = new ListViewItem(new string[] { textBoxPropertyName.Text, comboBoxType.Text, textBoxNotes.Text, global.fad3DataStatus.statusNew.ToString() });
                    lvi.Name = Guid.NewGuid().ToString();
                    lvSpecs.Items.Add(lvi);
                }
                else
                {
                    lvSpecs.SelectedItems[0].With(o =>
                    {
                        o.Text = textBoxPropertyName.Text;
                        o.SubItems[1].Text = comboBoxType.Text;
                        o.SubItems[2].Text = textBoxNotes.Text;
                        o.SubItems[3].Text = global.fad3DataStatus.statusEdited.ToString();
                    });
                }

                AdjustColWidths();
            }
        }

        private void OntextBox_Validating(object sender, CancelEventArgs e)
        {
            var s = ((TextBox)sender).Text;
            if (s.Length > 0 && s.Length < 5)
            {
                MessageBox.Show("Text is too short", "Validation error",
                                 MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
            }
        }

        private void OnlvSpecs_DoubleClick(object sender, EventArgs e)
        {
            if (_HitItem.Item != null)
            {
                _HitItem.Item.With(o =>
                {
                    textBoxPropertyName.Text = o.Text;
                    comboBoxType.Text = o.SubItems[1].Text;
                    textBoxNotes.Text = o.SubItems[2].Text;
                });

            }
        }

        private void OnlvSpecs_MouseDown(object sender, MouseEventArgs e)
        {
            _HitItem = lvSpecs.HitTest(e.X, e.Y);
        }

        private void OnlvSpecs_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            if (e.ColumnIndex == HiddenColIndex)
            {
                e.Cancel = true;
                e.NewWidth = lvSpecs.Columns[e.ColumnIndex].Width;

            }
        }

        private void OncomboBoxType_Validating(object sender, CancelEventArgs e)
        {
            ((ComboBox)sender).With(o =>
            {
                e.Cancel = !o.Items.Contains(o.Text);
                if (e.Cancel)
                {
                    MessageBox.Show("Please select an item in the list", "Validation error",
                                     MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            });
        }
    }
}

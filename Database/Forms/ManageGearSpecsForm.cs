using FAD3.Database.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace FAD3
{
    public partial class ManageGearSpecsForm : Form
    {
        private string _gearVarGuid;
        private string _gearVarName;
        private List<GearSpecification> _gearSpecs = new List<GearSpecification>();
        private ListViewHitTestInfo _hitItem;
        private int _hiddenColIndex;
        private List<string> _deletedSpecsRow = new List<string>();
        private SampledGear_SpecsForm _parentForm;

        public ManageGearSpecsForm(string GearVarGuid, string GearVarName = null, SampledGear_SpecsForm parent = null)
        {
            InitializeComponent();
            _gearVarGuid = GearVarGuid;
            if (GearVarName == null)
                ManageGearSpecsClass.GearVarGuid(GearVarGuid);
            else
            {
                _gearVarName = GearVarName;
                ManageGearSpecsClass.GearVariation(_gearVarGuid, _gearVarName);
            }

            _gearSpecs = ManageGearSpecsClass.GearSpecifications;

            if (parent != null) _parentForm = parent;
        }

        private void ManageGearSpecs_Load(object sender, EventArgs e)
        {
            labelTitle.Text += " " + _gearVarName;
            comboBoxType.Items.Add("Numeric");
            comboBoxType.Items.Add("Text");
            comboBoxType.Items.Add("Yes/No");
            comboBoxType.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBoxType.AutoCompleteMode = AutoCompleteMode.Suggest;

            lvSpecs.With(o =>
            {
                o.View = View.Details;
                var c = o.Columns.Add("Property");
                c.Width = MainForm.TextWidth(c.Text, lvSpecs.Font);
                o.Columns.Add("Type");
                o.Columns.Add("Notes");
                c = o.Columns.Add("DataStatus");
                c.Width = 0;
                _hiddenColIndex = c.Index;
                o.FullRowSelect = true;

                lvSpecs.ColumnWidthChanging += OnlvSpecs_ColumnWidthChanging;
            });

            if (_gearSpecs != null)
                foreach (GearSpecification item in _gearSpecs)
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
                    if (_parentForm != null) _parentForm.RefreshSpecs();
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
                        _deletedSpecsRow.Add(lvSpecs.SelectedItems[0].Name);
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
            _gearSpecs.Clear();
            foreach (ListViewItem lvi in lvSpecs.Items)
            {
                fad3DataStatus ds;
                Enum.TryParse(lvi.SubItems[3].Text, out ds);
                var spec = new GearSpecification
                {
                    Property = lvi.Text,
                    Type = lvi.SubItems[1].Text,
                    Notes = lvi.SubItems[2].Text,
                    DataStatus = ds,
                    RowGuid = lvi.Name,
                };

                if (spec.Sequence != lvi.Index + 1 && ds != fad3DataStatus.statusNew)
                {
                    spec.DataStatus = fad3DataStatus.statusEdited;
                    spec.Sequence = lvi.Index + 1;
                }

                _gearSpecs.Add(spec);
            }

            foreach (var item in _deletedSpecsRow)
            {
                var spec = new GearSpecification
                {
                    RowGuid = item,
                    DataStatus = fad3DataStatus.statusForDeletion
                };
                _gearSpecs.Add(spec);
            }

            return ManageGearSpecsClass.SaveGearSpecs(_gearSpecs);
        }

        private void AddPropertyToList()
        {
            if (textBoxPropertyName.TextLength > 0 && comboBoxType.Text != "")
            {
                if (lvSpecs.SelectedItems.Count == 0)
                {
                    var lvi = new ListViewItem(new string[] { textBoxPropertyName.Text, comboBoxType.Text, textBoxNotes.Text, fad3DataStatus.statusNew.ToString() });
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
                        o.SubItems[3].Text = fad3DataStatus.statusEdited.ToString();
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
            if (_hitItem.Item != null)
            {
                _hitItem.Item.With(o =>
                {
                    textBoxPropertyName.Text = o.Text;
                    comboBoxType.Text = o.SubItems[1].Text;
                    textBoxNotes.Text = o.SubItems[2].Text;
                });
            }
        }

        private void OnlvSpecs_MouseDown(object sender, MouseEventArgs e)
        {
            _hitItem = lvSpecs.HitTest(e.X, e.Y);
        }

        private void OnlvSpecs_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            if (e.ColumnIndex == _hiddenColIndex)
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
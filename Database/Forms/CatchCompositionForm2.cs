using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FAD3.Database.Forms
{
    public partial class CatchCompositionForm2 : Form
    {
        private MainForm _parentForm;
        private string _samplingGuid;
        private double _weightOfCatch;
        private double? _weightOfSample;
        private bool _isNew;
        private string _referenceNumber;
        private string _keyPressed;
        private List<string> _listGenera;
        private int _row;
        private DataGridViewComboBoxColumn tbcIdentification = new DataGridViewComboBoxColumn();
        private DataGridViewComboBoxColumn tbcName1 = new DataGridViewComboBoxColumn();
        private DataGridViewComboBoxColumn tbcName2 = new DataGridViewComboBoxColumn();

        private enum ListContent
        {
            genus,
            species,
            localNames
        }

        public CatchCompositionForm2(bool isNew, MainForm parent, string samplingGuid, string referenceNumber, double weightOfCatch, double? weightOfSample)
        {
            _isNew = isNew;
            InitializeComponent();
            _parentForm = parent;
            _samplingGuid = samplingGuid;
            _weightOfCatch = weightOfCatch;
            _weightOfSample = weightOfSample;
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

        private void FillColumnComboUsingList(DataGridViewComboBoxColumn columnCombo, ListContent content, List<string> list = null, Dictionary<string, string> dict = null)
        {
            columnCombo.Items.Clear();
            switch (content)
            {
                case ListContent.genus:
                    foreach (var item in list)
                    {
                        columnCombo.Items.Add(item);
                    }
                    break;

                case ListContent.species:
                case ListContent.localNames:
                    foreach (KeyValuePair<string, string> kv in dict)
                    {
                        columnCombo.Items.Add(kv);
                    }
                    columnCombo.DisplayMember = "Value";
                    columnCombo.ValueMember = "Value";
                    break;
            }

            columnCombo.MaxDropDownItems = 8;
        }

        private void ConfigGridView()
        {
            Names.GetGenus_LocalNames();

            //DataGridViewTextBoxColumn tbcText = new DataGridViewTextBoxColumn();
            //tbcText.HeaderText = "Row";
            //dgCatchComposition.Columns.Add(tbcText);
            DataGridViewCheckBoxColumn tbcCheck = new DataGridViewCheckBoxColumn();
            DataGridViewTextBoxColumn tbcText = new DataGridViewTextBoxColumn();
            DataGridViewComboBoxColumn tbcCombo = new DataGridViewComboBoxColumn();
            DataGridViewComboBoxColumn tbcIdentification = new DataGridViewComboBoxColumn();
            tbcIdentification.HeaderText = "Identification";
            tbcIdentification.Items.Add("Scientific");
            tbcIdentification.Items.Add("Local name");
            tbcIdentification.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            dgCatchComposition.Columns.Add(tbcIdentification);

            //tbcText.HeaderText = "Name";
            //dgCatchComposition.Columns.Add(tbcText);
            tbcName1 = new DataGridViewComboBoxColumn();
            tbcName1.HeaderText = "Name 1";
            FillColumnComboUsingList(tbcName1, ListContent.genus, Names.GenusList);
            tbcName1.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            dgCatchComposition.Columns.Add(tbcName1);

            tbcName2 = new DataGridViewComboBoxColumn();
            tbcName2.HeaderText = "Name 2";
            tbcName2.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            tbcName2.DisplayMember = "Value";
            tbcName2.ValueMember = "Key";
            tbcName2.DisplayStyleForCurrentCellOnly = true;

            dgCatchComposition.Columns.Add(tbcName2);
            tbcText = new DataGridViewTextBoxColumn();
            tbcText.HeaderText = "Weight";
            dgCatchComposition.Columns.Add(tbcText);
            tbcText = new DataGridViewTextBoxColumn();
            tbcText.HeaderText = "Count";
            dgCatchComposition.Columns.Add(tbcText);
            tbcText = new DataGridViewTextBoxColumn();
            tbcText.HeaderText = "Subsample weight";
            dgCatchComposition.Columns.Add(tbcText);
            tbcText = new DataGridViewTextBoxColumn();
            tbcText.HeaderText = "Subsample count";
            dgCatchComposition.Columns.Add(tbcText);
            tbcCheck.HeaderText = "From total";
            dgCatchComposition.Columns.Add(tbcCheck);
            tbcCheck = new DataGridViewCheckBoxColumn();
            tbcCheck.HeaderText = "Live fish";
            dgCatchComposition.Columns.Add(tbcCheck);

            dgCatchComposition.AllowUserToAddRows = false;
        }

        private void ConfigGenusLocalNames()
        {
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            global.LoadFormSettings(this, true);
            ConfigGridView();
            ConfigGenusLocalNames();
            //_emptySumOfWeightsLabel = labelSumOfWeight.Text;
            labelTitle.Text = $"Catch composition of {_referenceNumber}";
            if (_isNew)
            {
                labelTitle.Text = $"New catch composition of {_referenceNumber}";
                //AddNewRow();
            }
            else
            {
                //_CatchCompositionData = CatchComposition.RetrieveCatchComposition(_samplingGuid);
                //foreach (var item in _CatchCompositionData)
                //{
                //    AddRow(isNew: false, item.Key, item.Value);
                //}
                //labelSumOfWeight.Text += $" {_sumOfWeight.ToString("0.000")}";
                //if (_sumOfWeight > _weightOfCatch)
                //{
                //    labelSumOfWeight.ForeColor = Color.Red;
                //}
            }
            AddRow();
        }

        private void AddRow(bool validate = false)
        {
            bool proceed = true;
            if (validate)
            {
                proceed = false;
                DataGridViewCellCollection cells = dgCatchComposition.Rows[dgCatchComposition.Rows.Count - 1].Cells;
                if (cells[0].Value?.ToString().Length > 0)
                {
                    switch (cells[0].Value.ToString())
                    {
                        case "Scientific":
                            proceed = cells[1].Value?.ToString().Length > 0
                                && cells[2].Value?.ToString().Length > 0
                                && cells[3].Value?.ToString().Length > 0
                                && (cells[4].Value?.ToString().Length > 0
                                  || cells[5].Value?.ToString().Length > 0
                                  && cells[6].Value?.ToString().Length > 0);
                            break;

                        case "Local name":
                            proceed = cells[1].Value?.ToString().Length > 0
                                && cells[3].Value?.ToString().Length > 0
                                && (cells[4].Value?.ToString().Length > 0
                                  || cells[5].Value?.ToString().Length > 0
                                  && cells[6].Value?.ToString().Length > 0);
                            break;
                    }
                }
            }
            if (proceed)
            {
                dgCatchComposition.Rows.Add();
                _row = dgCatchComposition.Rows.Count - 1;
                dgCatchComposition.Rows[_row].Cells[0].Value = "Scientific";
            }
        }

        private void OnCellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
            }
        }

        private void OnCellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                //e.Cancel = true;
            }
            else
            {
            }
            Console.WriteLine($"cell begin edit");
        }

        private void OnGridKeyPress(object sender, KeyPressEventArgs e)
        {
            Console.WriteLine($"keypress");
        }

        private void OnGridKeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine($"key down {e.KeyCode}{e.ToString()}");
            if (e.KeyCode == Keys.Enter && e.Shift == true)
            {
                //MessageBox.Show("hey");
                AddRow(validate: true);
            }
        }

        private void OnFormKeyDown(object sender, KeyEventArgs e)
        {
            _keyPressed = e.KeyCode.ToString();
            Text = _keyPressed;
            Console.WriteLine($"form keydown {_keyPressed}");
            //if (e.KeyCode == Keys.Enter && e.Shift == true)
            //{
            //    MessageBox.Show("hey");
            //}
        }

        private void OnCellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case 1:
                    if (dgCatchComposition.Rows[_row].Cells[1].Value?.ToString().Length > 0)
                    {
                        switch (dgCatchComposition.Rows[_row].Cells[0].Value)
                        {
                            case "Scientific":
                                dgCatchComposition.Rows[_row].Cells[2].Value = "";
                                Names.Genus = dgCatchComposition.Rows[_row].Cells[1].Value.ToString();
                                FillColumnComboUsingList(tbcName2, ListContent.species, dict: Names.SpeciesList);
                                break;

                            case "Local name":
                                break;
                        }
                    }
                    break;

                case 3:
                    dgCatchComposition.Rows[_row].Cells[2].DetachEditingControl();
                    break;
            }
            //if (e.ColumnIndex == 1 && dgCatchComposition.Rows[_row - 1].Cells[1].Value?.ToString().Length > 0)
            //{
            //    switch (dgCatchComposition.Rows[_row - 1].Cells[0].Value)
            //    {
            //        case "Scientific":
            //            dgCatchComposition.Rows[_row - 1].Cells[2].Value = "";
            //            Names.Genus = dgCatchComposition.Rows[_row - 1].Cells[1].Value.ToString();
            //            FillColumnComboUsingList(tbcName2, ListContent.species, dict: Names.SpeciesList);
            //            break;

            //        case "Local name":
            //            break;
            //    }
            //}
        }

        private void OnCellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //_row = e.RowIndex;
        }

        private void OnRowEnter(object sender, DataGridViewCellEventArgs e)
        {
            _row = e.RowIndex;
        }

        private void OnDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }
    }
}
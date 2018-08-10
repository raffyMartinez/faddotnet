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
    public partial class frmEditGMS : Form
    {
        private string _CatchCompRowGUID;
        private bool _IsNew;
        private string _RowGUID;
        private GMSManager.Taxa _taxa;
        private string _CatchNameGUID;
        private string _CatchName;
        private GMSManager.FishCrabGMS _stage = GMSManager.FishCrabGMS.AllTaxaNotDetermined;

        public string CatchName
        {
            get { return _CatchName; }
            set
            {
                _CatchName = value;
                lblTitle.Text = _CatchName;
            }
        }

        public void FocusStart()
        {
            txtLen.Focus();
        }

        public GMSManager.Taxa taxa
        {
            set
            {
                _taxa = value;
                bool Success = false;
                comboStage.DataSource = new BindingSource(GMSManager.GMSStages(_taxa, ref Success), null);
                if (Success)
                {
                    comboStage.ValueMember = "Key";
                    comboStage.DisplayMember = "Value";
                    comboStage.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    comboStage.AutoCompleteSource = AutoCompleteSource.ListItems;
                    comboStage.Enabled = true;
                }
                else
                {
                    comboStage.DataSource = null;
                    comboStage.Enabled = false;
                    comboStage.Text = "Not determined";
                }
            }
            get { return _taxa; }
        }

        public string CatchNameGUID
        {
            get { return _CatchNameGUID; }
            set { _CatchNameGUID = value; }
        }

        public void AddNew()
        {
            _IsNew = true;
        }

        public void GMSData(string RowGUID, double? length, double? weight,
                            GMSManager.sex sex, GMSManager.FishCrabGMS stage,
                            double? gonadwt, GMSManager.Taxa taxa)
        {
            _RowGUID = RowGUID;
            txtLen.Text = length.ToString();
            txtWt.Text = weight.ToString();
            txtWtGonad.Text = gonadwt.ToString();
            _taxa = taxa;
            string myStage = GMSManager.GMSStageToString(_taxa, stage);
            comboStage.Text = myStage;
            comboSex.Text = sex.ToString();
            _IsNew = false;
            Text = "Edit GMS data";
        }

        public string CatchCompRowGUID
        {
            get { return _CatchCompRowGUID; }
            set { _CatchCompRowGUID = value; }
        }

        public frmEditGMS()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmEditGMS_Load(object sender, EventArgs e)
        {
            comboSex.DataSource = Enum.GetValues(typeof(GMSManager.sex));
            comboSex.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboSex.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            double? len = txtLen.Text.Length > 0 ? double.Parse(txtLen.Text) : (double?)null;
            double? wt = txtWt.Text.Length > 0 ? double.Parse(txtWt.Text) : (double?)null;
            double? gonadwt = txtWtGonad.Text.Length > 0 ? double.Parse(txtWtGonad.Text) : (double?)null;
            GMSManager.sex sex = (GMSManager.sex)Enum.Parse(typeof(GMSManager.sex), comboSex.Text);
            GMSManager.FishCrabGMS stage = GMSManager.MaturityStageFromText(comboStage.Text, _taxa);
            if (_IsNew)
            {
                _RowGUID = Guid.NewGuid().ToString();
            }
            if (GMSManager.UpdateGMS(_IsNew, wt, len, sex, stage, gonadwt, _CatchCompRowGUID, _RowGUID))
            {
                if (chkContinue.Checked)
                {
                    txtLen.Text = "";
                    txtWtGonad.Text = "";
                    txtWt.Text = "";
                    comboSex.Text = "Juvenile";
                    comboStage.Text = "Not determined";
                    txtLen.Focus();
                }
                else
                {
                    this.Close();
                }
            }
        }
    }
}
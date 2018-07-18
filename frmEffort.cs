/*
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/12/2016
 * Time: 9:29 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;

namespace FAD3
{
    /// <summary>
    /// Description of frmEffort.
    /// </summary>
    public partial class frmEffort : Form
    {
        private enum DateComparisonResult
        {
            Earlier = -1,
            Later = 1,
            TheSame = 0
        };

        private enum VesselType
        {
            Motorized=1,
            NonMotorized,
            NoBoatUsed,
            BoatInfoMissing
        }


        private sampling _Sampling = new sampling();
        private aoi _AOI = new aoi();
        private Dictionary<string, string> _CatchAndEffort = new Dictionary<string, string>();
        private bool _IsNew = false;
        private landingsite _LandingSite = new landingsite();
        private Dictionary<string, string> _GearClassLetterCodes = new Dictionary<string, string>();
        private frmMain _parentForm;
        private Dictionary<string, string> _NewSamplingData;
        private bool _InvokedFromList;

        public bool InvokedFromList
        {
            get { return _InvokedFromList; }
            set { _InvokedFromList = value; }
        }

        public Dictionary<string, string> NewSamplingData
        {
            get { return _NewSamplingData; }
            set { _NewSamplingData = value; }
        }

        public new void ParentForm(frmMain f)
        {
            _parentForm = f;
        }    

        public void FishingGrounds(List<string> fg_list)
        {
            foreach (var item in fg_list)
            {
                listFG.Items.Add(item.ToString());
            }
        }
        public void AddNew()
        {
            _IsNew = true;
        }

        public sampling Sampling
        {
            get { return _Sampling; }
            set
            {
                _Sampling = value;
                _IsNew = false;
            }
        }

        public aoi AOI
        {
            get { return _AOI; }
            set { _AOI = value; }
        }

        public landingsite LandingSite
        {
            get { return _LandingSite; }
            set { _LandingSite = value; }
        }

        public frmEffort()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //

            /*foreach (Form f in Application.OpenForms)
			{
				if(f.Name == "frmMain"){
					frmMain frm = (frmMain)f;
					//_Sampling = frm.Sampling;
					_AOI = frm.AOI;
				}
			}*/

            _GearClassLetterCodes = global.GearClassLetterCodes;

        }

        void Button2Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void Label5Click(object sender, EventArgs e)
        {

        }

        void Label18Click(object sender, EventArgs e)
        {

        }

        void FrmEffortLoad(object sender, EventArgs e)
        {
            string tag = "";
            string[] arr = new string[1];


            comboGearClass.DataSource = new BindingSource(global.GearClass, null);
            comboGearClass.DisplayMember = "Value";
            comboGearClass.ValueMember = "Key";
            comboGearClass.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboGearClass.AutoCompleteSource = AutoCompleteSource.ListItems;

            if (_IsNew)
            {
                if (_LandingSite.GearVarGUID == "")
                {
                    KeyValuePair<string, string> kv = (KeyValuePair<string, string>)comboGearClass.Items[0];
                    comboGearClass.Text = kv.Value;
                    global.GearClassUsed = kv.Key;
                    FillComboGearVar();
                }
            }

            comboLS.DataSource = new BindingSource(_AOI.LandingSites, null);
            comboLS.DisplayMember = "Value";
            comboLS.ValueMember = "Key";
            comboLS.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboLS.AutoCompleteSource = AutoCompleteSource.ListItems;

            comboAOI.DataSource = new BindingSource(_AOI.AOIs, null);
            comboAOI.DisplayMember = "Value";
            comboAOI.ValueMember = "Key";
            comboAOI.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboAOI.AutoCompleteSource = AutoCompleteSource.ListItems;






            try
            {
                comboEnumerators.DataSource = new BindingSource(_AOI.Enumerators, null);
                comboEnumerators.DisplayMember = "Value";
                comboEnumerators.ValueMember = "Key";
                comboEnumerators.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                comboEnumerators.AutoCompleteSource = AutoCompleteSource.ListItems;
            }
            catch
            {
                comboEnumerators.DataSource = null;
                comboEnumerators.Text = "";
                ErrorLogger.Log("error filling the enumerator combo. Maybe no enumerators for the AOI");
            }


            if (_IsNew)
            {
                comboAOI.Text = _AOI.AOIName;
                if (_InvokedFromList==false)
                {
                    comboLS.Text = _LandingSite.LandingSiteName;
                    if (_LandingSite.GearClassNameFromGearVar != "")
                    {
                        comboGearClass.Text = _LandingSite.GearClassNameFromGearVar;
                    }
                    comboGearVariations.Text = _LandingSite.GearVariationName;
                    comboEnumerators.Text = "";
                }
                else
                {
                    comboLS.Text = _NewSamplingData["LandingSite"];
                    comboGearClass.Text = _NewSamplingData["ClassName"];
                    global.GearClassUsed = _NewSamplingData["ClassGuid"];
                    FillComboGearVar();
                    comboGearVariations.Text = _NewSamplingData["GearName"];

                }

            }
            else
            {


                _CatchAndEffort = _Sampling.CatchAndEffort();
                foreach (Control c in this.tableLayoutPanel1.Controls)
                {
                    try { tag = c.Tag.ToString(); }
                    catch { tag = ""; }
                    try
                    {
                        if (tag != "")
                        {
                            //Debug.WriteLine("control tag: " + tag);
                            foreach (KeyValuePair<string, string> kv in _CatchAndEffort)
                            {
                                //Debug.WriteLine("kv " + kv.Key);
                                if (kv.Key == tag)
                                {
                                    arr = kv.Value.Split('|');
                                    c.Text = arr[0];
                                    if (kv.Key == "GearClass")
                                    {
                                        global.GearClassUsed = arr[1];
                                        FillComboGearVar();
                                    }
                                    if (kv.Key == "Fishing ground" && kv.Value.Length>0)
                                    {
                                        listFG.Items.Add(kv.Value.ToString().ToUpper());
                                        foreach (var item in _Sampling.OtherFishingGround())
                                        {
                                            listFG.Items.Add(item.ToString().ToUpper());
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.Log(ex);
                    }
                }

                switch (_CatchAndEffort["Type of fishing vessel"])
                {
                    case "Motorized":
                        radioMotorized.Checked = true;
                        txtEngineHP.Text = _CatchAndEffort["Engine hp"];
                        break;
                    case "Non-Motorized":
                        radioNonMotorized.Checked = true;
                        break;
                    case "No vessel used":
                        radioNoBoat.Checked = true;
                        break;
                    case "Not provided":
                        break;
                }

                string[] arrVD = _CatchAndEffort["Vessel dimension (L x W x H)"].Split('x');
                if (arrVD.Length>0)
                {
                    txtVesL.Text = arrVD[0];
                    txtVesW.Text = arrVD[1];
                    txtVesH.Text = arrVD[2];
                }
            }
        }

        void ComboGearClassValidated(object sender, EventArgs e)
        {
            string key = ((KeyValuePair<string, string>)this.comboGearClass.SelectedItem).Key;
            SetGearVarCombo(key);

        }

        void FillComboGearVar()
        {
            try
            {
                comboGearVariations.DataSource = new BindingSource(global.GearVariations, null);
                comboGearVariations.DisplayMember = "Value";
                comboGearVariations.ValueMember = "Key";
                comboGearVariations.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                comboGearVariations.AutoCompleteSource = AutoCompleteSource.ListItems;
            }
            catch
            {
                ErrorLogger.Log("gear class is not known, cannot generate gear var list");
            }
        }
        void SetGearVarCombo(string GearClass)
        {
            global.GearClassUsed = GearClass;
            //comboBox2.Items.Clear();
            comboGearVariations.DataSource = new BindingSource(global.GearVariations, null);
            comboGearVariations.DisplayMember = "Value";
            comboGearVariations.ValueMember = "Key";
            comboGearVariations.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboGearVariations.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        protected bool CheckDate(String date, string DateOrTime, ref string Returnmsg)
        {
            DateTime Temp;
            string msg;
            bool Success = false;
            if (DateOrTime=="Date")
            {
                Success = date.Length == 11;
                msg = "Date is too short";
            }
            else
            {
                Success = date.Length == 5;
                msg = "Time is too short";
            }

            if (Success) {
                Success = DateTime.TryParse(date, out Temp) == true;
                if (DateOrTime=="Date")
                {
                    Success = Temp.CompareTo(DateTime.Today) < 1;
                    msg = "Cannot use a future date";
                }
                else
                {
                    if (Success == false)
                    {
                        msg = "Time provided is not correct";
                    }
                }

            }
            Returnmsg = msg;
            return Success;
        }

        void ButtonGenerateClick(object sender, EventArgs e)
        {
            if (maskDateSampling.Text != "" && comboGearVariations.Text != "" && comboGearClass.Text != "" && comboAOI.Text != "")
            {
                string RefNo = _AOI.AOILetter;
                DateTime dt = Convert.ToDateTime(maskDateSampling.Text);
                RefNo += (string.Format("{0:yy}", dt));
                string GearVar = ((KeyValuePair<string, string>)this.comboGearVariations.SelectedItem).Key;
                RefNo += "-" + gear.GearCodeFromGearVar(GearVar);
                long myCounter = 0;
                if (gear.GetGearCodeCounter(RefNo, ref myCounter))
                {
                    RefNo += "-" + string.Format("{0:00000}",myCounter);
                };
                txtRefNo.Text = RefNo;
            }

        }

        void ListBox1_DoubleClick(object sender, EventArgs e)
        {
            ShowGridSetup();
        }

        void ShowGridSetup()
        {
            frmFG f = new frmFG();
            f.MajorGridList(_AOI.MajorGrids);
            if (listFG.Items.Count > 0)
            {
                List<string> myList = new List<string>();
                foreach (var item in listFG.Items)
                {
                    myList.Add(item.ToString());
                }
                f.FishingGrounds = myList;
            }

            f.ShowDialog();

            //this will wait until the modal form closes
            if (f.FishingGrounds.Count > 0)
            {
                listFG.Items.Clear();
                foreach (var item in f.FishingGrounds)
                {
                    listFG.Items.Add(item.ToString());
                }
            }
        }

        private void linkLabel1_MouseClick(object sender, MouseEventArgs e)
        {
            ShowGridSetup();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void maskDateSampling_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MaskedTextBox t = (MaskedTextBox)sender;
            string dv = t.Text;
            string msg="";
            string tag = t.Tag.ToString().Substring(0, 4);
            if (dv != "")
            {
                if (!CheckDate(dv, tag, ref msg))
                {
                    switch (tag)
                    {
                        case "Date":
                            MessageBox.Show(msg);
                            break;
                        case "Time":
                            MessageBox.Show(msg);
                            break;
                    }

                    e.Cancel = true;
                }
            }
        }

        private bool ValidateEffort()
        {
            bool Success = false;
            string msg = "";

            //we remove the prompts when getting text property
            foreach (Control c in tableLayoutPanel1.Controls)
            {
                MaskedTextBox mt = new MaskedTextBox();
                if (c is MaskedTextBox)
                {
                    mt = (MaskedTextBox)c;
                    mt.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                }
            }

            // we make sure that all date-time fields are present
            if (maskDateSampling.Text.Length>0 &&
                maskDateSet.Text.Length > 0 &&
                maskDateHaul.Text.Length > 0 &&
                maskTimeSampling.Text.Length > 0 &&
                maskTimeSet.Text.Length > 0 &&
                maskTimeHaul.Text.Length > 0
                )
            {

                //we include literals again in order for dates and times to be
                //parsed correctly
                foreach (Control c in tableLayoutPanel1.Controls)
                {
                    MaskedTextBox mt = new MaskedTextBox();
                    if (c is MaskedTextBox)
                    {
                        mt = (MaskedTextBox)c;
                        mt.TextMaskFormat = MaskFormat.IncludeLiterals;
                    }
                }

                //we parse the values in the date-time fields into DateTime variables
                string SamplingDateTime = maskDateSampling.Text + " " + maskTimeSampling.Text;
                string SetDateTime = maskDateSet.Text + " " + maskTimeSet.Text;
                string HaulDateTime = maskDateHaul.Text + " " + maskTimeHaul.Text;
                DateTime SamplingDate = DateTime.Parse(SamplingDateTime);
                DateTime SetDate = DateTime.Parse(SetDateTime);
                DateTime HaulDate = DateTime.Parse(HaulDateTime);

                //then we make sure that the date-times are plausible.
                //sampling date should be later than haul date and
                //haul date should be later than set date
                DateComparisonResult CompareResult;
                CompareResult = (DateComparisonResult)SamplingDate.CompareTo(HaulDate);
                if (CompareResult == DateComparisonResult.Later)
                {
                    CompareResult = (DateComparisonResult)HaulDate.CompareTo(SetDate);
                    if (CompareResult==DateComparisonResult.Later)
                    {
                        Success = true;
                    }
                    else
                    {
                        msg = "Haul date should be after set date";
                    }
                }
                else
                {
                    msg = "Sampling date should be after date gear was hauled";
                }

                //we proceed with our validation if the date-times are plausible
                if (Success)
                {
                    //then we make sure the following controls are not empty
                    if (comboAOI.Text.Length>0 &&
                        comboLS.Text.Length>0 &&
                        listFG.Items.Count>0 &&
                        txtCatchWt.Text.Length>0 &&
                        comboGearClass.Text.Length>0 &&
                        comboGearVariations.Text.Length>0 &&
                        comboEnumerators.Text.Length>0 &&
                        txtRefNo.Text.Length>0 && 
                        txtNoFishers.Text.Length>0 &&
                        txtNoHauls.Text.Length>0
                        )
                    {
                        //if there is sample weight
                        if (txtSampleWt.Text.Length>0)
                        {
                            //we test if catch wt is greater than sameple wt
                            double CatchWt = double.Parse(txtCatchWt.Text);
                            double SampleWt = double.Parse(txtSampleWt.Text);
                            if (CatchWt < SampleWt)
                            {
                                Success = false;
                                msg = "Sample weight cannot be greater than catch weight";
                            }
                        }
                               
                    }
                    else
                    {
                        Success = false;
                        msg = "Please provide values in highlited fields";
                        HighlightRequired();
                    }
                }

                //we determine if vessel type is included
                if (Success )
                {
                    Success = (radioMotorized.Checked || radioNonMotorized.Checked || radioNoBoat.Checked);
                    if (Success == false)
                    {
                        msg = "Include type of vessel";
                        Success = false;
                    }
                }

                //we determine that either all or none of vessel size fields are filled
                if (Success)
                {
                    Success = (txtVesH.Text.Length == 0 && txtVesH.Text.Length == 0 && txtVesH.Text.Length == 0) ||
                              (txtVesH.Text.Length > 0 && txtVesH.Text.Length > 0 && txtVesH.Text.Length > 0);
                    if (Success==false)
                    {
                        msg = "You must answer all fields for vessel size or leave them all blank";
                    }
                }
            }
            else
            {
                Success = false;
                msg = "Please provide values in highlited fields";
                HighlightRequired();
            }


            if (msg.Length>0)
            {
                MessageBox.Show(msg);
            }
            return Success;
        }

        private void HighlightRequired()
        { 
            foreach (Control c in tableLayoutPanel1.Controls)
            {
                if (c is TextBox || c is ComboBox || c is ListBox || c is MaskedTextBox)
                {
                    switch (c.Name)
                    {
                        case "txtSampleWt":
                        case "txtNotes":
                            break;
                        default:
                            c.BackColor = Color.LightPink;
                            break;
                    }

                    if (c is MaskedTextBox)
                    {
                        MaskedTextBox mt = new MaskedTextBox();
                        mt = (MaskedTextBox)c;
                        mt.TextMaskFormat = MaskFormat.IncludeLiterals;
                    }
                }
            }

            panel1.BackColor = Color.LightPink;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ValidateEffort())
            {
                Dictionary<string, string> EffortData = new Dictionary<string, string>();
                if (_IsNew)
                {
                    EffortData.Add("SamplingGUID", Guid.NewGuid().ToString());
                }
                else
                {
                    EffortData.Add("SamplingGUID", _Sampling.SamplingGUID);
                }
                EffortData.Add("RefNo",txtRefNo.Text);
                EffortData.Add("Enumerator",((KeyValuePair<string,string>)comboEnumerators.SelectedItem).Key);
                EffortData.Add("GearVarGUID",((KeyValuePair<string,string>)comboGearVariations.SelectedItem).Key);
                EffortData.Add("AOI", ((KeyValuePair<string, string>)comboAOI.SelectedItem).Key);
                EffortData.Add("SamplingDate",maskDateSampling.Text);
                EffortData.Add("SamplingTime",maskTimeSampling.Text);
                EffortData.Add("FishingGround", listFG.Items[0].ToString());
                EffortData.Add("TimeSet",maskTimeSet.Text);
                EffortData.Add("DateSet",maskDateSet.Text);
                EffortData.Add("TimeHauled",maskTimeHaul.Text);
                EffortData.Add("DateHauled",maskDateHaul.Text);
                EffortData.Add("NoHauls",txtNoHauls.Text);
                EffortData.Add("NoFishers",txtNoFishers.Text);
                EffortData.Add("hp",txtEngineHP.Text);
                EffortData.Add("WtCatch",txtCatchWt.Text);
                EffortData.Add("WtSample",txtSampleWt.Text);
                EffortData.Add("len", txtVesL.Text);
                EffortData.Add("wdt",txtVesW.Text);
                EffortData.Add("hgt",txtVesH.Text);
                EffortData.Add("LSGUID",((KeyValuePair<string,string>)comboLS.SelectedItem).Key);
                EffortData.Add("Notes",txtNotes.Text);

                VesselType vesType = VesselType.BoatInfoMissing;
                if (radioMotorized.Checked)
                {
                    vesType = VesselType.Motorized;
                }
                else if (radioNonMotorized.Checked)
                {
                    vesType = VesselType.NonMotorized;
                }
                else if (radioNoBoat.Checked)
                {
                    vesType = VesselType.NoBoatUsed;
                }
                EffortData.Add("VesType", ((int)vesType).ToString());

                //not found in the code but exist in database
                EffortData.Add("SamplingType","1");
                EffortData.Add("HasLiveFish",checkHasLiveFish.Checked.ToString());
                if(_Sampling.UpdateEffort(_IsNew, EffortData))
                {
                    _parentForm.SamplingUpdate (_IsNew, EffortData["AOI"],  EffortData["LSGUID"], comboGearVariations.Text, EffortData["GearVarGUID"],  EffortData["SamplingDate"]);
                    this.Close();
                }
                
            }
        }

        private void txtCatchWt_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TextBox t = new TextBox();
            string msg = "";
            t = (TextBox)sender;
            if (t.Text.Length > 0)
            {
                double num;

                //its okay when the contents are parsed to a number
                if (double.TryParse(t.Text, out num) == true)
                {
                    if (t.Tag.ToString() == "Weight of catch")
                    {
                        //we accept answers zero and greater
                        if (num < 0)
                        {
                            msg = "Weight of catch cannot be less than zero";
                        }
                    }
                    else
                    {
                        if (t.Tag.ToString() == "Number of hauls" ||
                            t.Tag.ToString()== "Number of fishers")
                        {
                            //value must not be less than one and should be a whole number
                            if (num < 1 || num % 1 !=0)
                            {
                                msg = "Number should be a whole number greater than zero";
                            }
                        }
                        else if (num <= 0)
                        {
                            msg = "Number should be greater than zero";
                        }
                    }
                }
                else
                {
                    msg = "Only numeric values are accepted";
                }
            }

            if (msg.Length>0)
            {
                e.Cancel = true;
                MessageBox.Show(msg);
            }
        }

        private void comboGearClass_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ComboBox cbo = (ComboBox)sender;
            if (cbo.Text.ToString().Length>0)
            { 
                string s;
                try { s = cbo.SelectedItem.ToString(); }
                catch { s = ""; }

                if (s == "")
                {
                    e.Cancel = true;
                    MessageBox.Show("Item is not found");
                }
            }
        }

        private void maskDateSet_KeyUp(object sender, KeyEventArgs e)
        {
            MaskedTextBox mt = (MaskedTextBox)sender;
            if (e.KeyCode == Keys.Oemplus)
            {
                if (mt.MaskFull == false)
                {
                    mt.Text = maskDateSampling.Text;
                }
                else
                {
                    DateTime dt = DateTime.Parse(mt.Text);
                    mt.Text = string.Format("{0:MMM-dd-yyyy}", dt.AddDays(1));
                }
            }
            else if (e.KeyCode == Keys.OemMinus)
            {
                if (mt.MaskFull == false)
                {
                    mt.Text = maskDateSampling.Text;
                }
                else
                {
                    DateTime dt = DateTime.Parse(mt.Text);
                    mt.Text = string.Format("{0:MMM-dd-yyyy}", dt.AddDays(-1));
                }
            }
            
        }

        private void comboLS_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listFG_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (e.Modifiers==Keys.Shift)
                {
                    ShowGridSetup();
                }
            }
        }
    }
}
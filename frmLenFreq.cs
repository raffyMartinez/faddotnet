using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Windows.Forms.DataVisualization.Charting;

namespace FAD3
{
    public partial class frmLenFreq : Form
    {

        private frmEditLF _frmEditLF;
        private frmEditGMS _frmEditGMS;
        private frmMain _ParentForm;
        private string _CurrentCatchName;
        private string _CurrentCatchNameGUID;
        private global.Taxa _CurrentCatchTaxa;
        private string _CurrentCatchRowNo;
        private Dictionary<string, string[]> _LFData;
        private Dictionary<string, string[]> _GMSData;
        private global.CatchMeristics _Meristics;


        public bool InEditLF
        {
            get { return _frmEditLF != null; }
        }

        public bool InEditGMS
        {
            get { return _frmEditGMS != null; }
        }

        public global.CatchMeristics Meristics
        {
            get { return _Meristics; }
            set
            {
                _Meristics = value;
                if (_Meristics == global.CatchMeristics.LengtFreq)
                {
                    tabLFGMS.SelectTab("tabLF");
                }
                else if (_Meristics ==global.CatchMeristics.LengthWeightSexMaturity)
                {
                    tabLFGMS.SelectTab("tabGMS");
                }
            }
        }

        public string CurrentCatchNameGUID
        {
            get { return _CurrentCatchNameGUID; }
            set
            {
                _CurrentCatchNameGUID = value;
                _CurrentCatchTaxa = global.TaxaFromCatchNameGUID(_CurrentCatchNameGUID);
            }
        }
        void FillGMS()
        {
            //fils the list view the GMS data of the currently
            //selected catch.
            lvGMS.Items.Clear();
            int i = 0;
            foreach (var item in _GMSData)
            {
                ListViewItem lvi = new ListViewItem();
                i++;
                lvi.Text = i.ToString();                 //row number column 1
                lvi.Tag = item.Key;                      //rowguid of gms
                lvi.Name = item.Key;                     //rowguid of gms
                string[] arr = item.Value;


                //we will add length formatted to 0.00
                double num;
                if (double.TryParse(arr[0],out num))
                {
                    lvi.SubItems.Add(string.Format("{0:0.00}", num));
                }
                else
                {
                    lvi.SubItems.Add("");
                }

                //we will add weight formatted to 0.00
                if (double.TryParse(arr[1], out num))
                {
                    lvi.SubItems.Add(string.Format("{0:0.00}", num));
                }
                else
                {
                    lvi.SubItems.Add("");
                }


                global.sex sex = (global.sex)int.Parse(arr[2]); //sex
                global.FishCrabGMS stage = (global.FishCrabGMS)int.Parse(arr[3]); //maturity stage
                                                                                  //gonad weight is arr[4]  
                global.Taxa taxa = (global.Taxa)int.Parse(arr[5]); //taxa

                lvi.SubItems.Add(sex.ToString());                                 // sex column 4

                string stage_description = global.GMSStage(taxa, stage);
                lvi.SubItems.Add(stage_description);                              // maturity stage column 5

                //we will add gonadweight formatted to 0.00
                if (double.TryParse(arr[4], out num))
                {
                    lvi.SubItems.Add(string.Format("{0:0.00}", num));
                }
                else
                {
                    lvi.SubItems.Add("");
                }

                lvGMS.Items.Add(lvi);
            }
            if (tabLFGMS.SelectedTab ==tabLFGMS.TabPages["tabGMS"])
            {
                lblTitle.Text = "Total number of measurements: " + lvGMS.Items.Count;
            }
        }


        public string CurrentCatchRowNo
        {
            get { return _CurrentCatchRowNo; }
            set
            {
                _CurrentCatchRowNo = value;
                _LFData = sampling.LFData(_CurrentCatchRowNo);
                _GMSData = sampling.GMSData(_CurrentCatchRowNo);
                FillLF();
                FillGMS();
                if (_LFData.Count>0)
                {
                    FillChart();
                }
                else
                {
                    chart1.Visible = false;
                }
            }
        }

        private void FillChart()
        {
            chart1.Visible = true;
            chart1.Series.Clear();
            Series series = chart1.Series.Add("LenFreq");
            series.ChartType = SeriesChartType.Column;

            try
            {
                Dictionary<double, long> Dic = lvLF.Items
                    .Cast<ListViewItem>()
                    .ToDictionary(x => double.Parse(x.SubItems[1].Text), x => long.Parse(x.SubItems[2].Text));
                var list = Dic.Keys.ToList();
                list.Sort();
                foreach (var key in list)
                {
                    string x = key.ToString();
                    long y = Dic[key];
                    series.Points.AddXY(x, y);
                }

                chart1.Series["LenFreq"].SetCustomProperty("PixelPointWidth", "18");
                chart1.ChartAreas[0].RecalculateAxesScale();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex);
            }
        }

        private void FillLF()
        {
            lvLF.Items.Clear();
            int i = 0;
            long SumOfFreq = 0;

            foreach (var item in _LFData)
            {
                ListViewItem lvi = new ListViewItem();
                i++;
                lvi.Text = i.ToString();
                lvi.Tag = item.Key;
                lvi.Name = item.Key;
                string[] arr = item.Value;
                lvi.SubItems.Add(arr[0]);
                lvi.SubItems.Add(arr[1]);
                SumOfFreq += long.Parse(arr[1]);
                lvLF.Items.Add(lvi);
            }


            chart1.Top = tabLF.ClientRectangle.Top + 30;
            if (tabLFGMS.SelectedTab==tabLFGMS.TabPages["tabLF"])
            {
                lblTitle.Text = "Total number of measurements: " + SumOfFreq.ToString();
            }

            int w = 0;
            foreach(var item in lvLF.Columns)
            {
                w += ((ColumnHeader)item).Width;
            }
            chart1.Left = w;
            chart1.Width = lvLF.Width - w;
            chart1.Height = lvLF.ClientSize.Height-25;
            chart1.Legends.Clear();
            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Verdana", 7f);
            chart1.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
            chart1.ChartAreas[0].AxisY.LabelStyle.Font = new System.Drawing.Font("Verdana", 7f);
            chart1.ChartAreas[0].AxisY.Interval = 1;
            //chart1.ChartAreas[0].AxisY.

        }

        public string CurrentCatchName
        {
            get { return _CurrentCatchName; }
            set
            {
                _CurrentCatchName = value;
                Text = _CurrentCatchName;

            }
        }

        public void EditedGMSEx(bool IsNew, double? wt, double? len, string sex,
                      string stage, double? gonadwt, string RowGUID)
        {
            if (IsNew)
            {
                ListViewItem lvi = lvGMS.Items.Add((lvGMS.Items.Count + 1).ToString());

                lvi.SubItems.Add(len == null ? "" : string.Format("{0:0.00}", len));
                lvi.SubItems.Add(wt == null ? "" : string.Format("{0:0.00}", wt));

                //lvi.SubItems.Add(len);
                //lvi.SubItems.Add(wt);
                lvi.SubItems.Add(sex);
                lvi.SubItems.Add(stage);
                //lvi.SubItems.Add(gonadwt);
                lvi.SubItems.Add(gonadwt == null ? "" : string.Format("{0:0.00}", gonadwt));
                lvi.Tag = RowGUID;
                lvi.Name = RowGUID;

            }
            else
            {
                ListViewItem lvi = lvGMS.Items[RowGUID];
                lvi.SubItems[1].Text = len == null ? "" : string.Format("{0:0.00}", len);
                lvi.SubItems[2].Text = wt == null ? "" : string.Format("{0:0.00}", wt);
                lvi.SubItems[3].Text = sex;
                lvi.SubItems[4].Text = stage;
                lvi.SubItems[5].Text = gonadwt == null ? "" : string.Format("{0:0.00}", gonadwt);
            }

            ComputeTotalMeasurements(lvGMS);
        }

        public void EditedGMS(bool IsNew, string wt, string len, string sex,
                              string stage, string gonadwt, string RowGUID)
        {
            if (IsNew)
            {
                ListViewItem lvi = lvGMS.Items.Add((lvGMS.Items.Count + 1).ToString());
                lvi.SubItems.Add(len);
                lvi.SubItems.Add(wt);
                lvi.SubItems.Add(sex);
                lvi.SubItems.Add(stage);
                lvi.SubItems.Add(gonadwt);
                lvi.Tag = RowGUID;
                lvi.Name = RowGUID;

            }
            else
            {
                ListViewItem lvi = lvGMS.Items[RowGUID];
                lvi.SubItems[1].Text = len;
                lvi.SubItems[2].Text = wt;
                lvi.SubItems[3].Text = sex;
                lvi.SubItems[4].Text = stage;
                lvi.SubItems[5].Text = gonadwt;
            }

            ComputeTotalMeasurements(lvGMS);
        }

        public void EditedLF(bool isNew, string RowGUID, double LenClass, long Freq)
        {
            if (isNew) {
                ListViewItem lvi = lvLF.Items.Add((lvLF.Items.Count + 1).ToString());
                lvi.Name = RowGUID;
                lvi.Tag = RowGUID;
                lvi.SubItems.Add(LenClass.ToString());
                lvi.SubItems.Add(Freq.ToString());
            }
            else
            {
                ListViewItem lvi = lvLF.Items[RowGUID];
                lvi.SubItems[1].Text = LenClass.ToString();
                lvi.SubItems[2].Text = Freq.ToString();
            }

            ComputeTotalMeasurements(lvLF);
            FillChart();
        }

        private void ComputeTotalMeasurements(ListView lv)
        {
            long n = 0;
            if(lv.Name == "lvLF") {
                foreach (ListViewItem item in lv.Items)
                {
                        n += long.Parse(item.SubItems[2].Text);
                }
            }
            else
            {
                n = lv.Items.Count;
            }
            lblTitle.Text = "Total number of measurements: " + n.ToString();
        }

        public new frmMain ParentForm
        {
            get { return _ParentForm; }
            set { _ParentForm = value; }
        }

        public frmLenFreq()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLenFreq_Load(object sender, EventArgs e)
        {
            lvLF.View = View.Details;
            lvLF.Columns.Add("Row");
            lvLF.Columns.Add("Class");
            lvLF.Columns.Add("Freq");
            lvLF.FullRowSelect = true;
            ApplySavedWidth(lvLF,global.lvContext.LengthFreq);

            lvGMS.View = View.Details;
            lvGMS.FullRowSelect = true;
            lvGMS.Columns.Add("Row");
            lvGMS.Columns.Add("Length");
            lvGMS.Columns.Add("Weight");
            lvGMS.Columns.Add("Sex");
            lvGMS.Columns.Add("Maturity stage");
            lvGMS.Columns.Add("Weight of gonads");
            ApplySavedWidth(lvGMS, global.lvContext.GMS);
        }

        private void ApplySavedWidth(ListView lv, global.lvContext whatLV)
        {
            try
            {
                RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\FAD3\\ColWidth");
                string rv = rk.GetValue(whatLV.ToString(), "NULL").ToString();
                string[] arr = rv.Split(',');
                int i = 0;
                foreach (var item in lv.Columns)
                {
                    ColumnHeader ch = (ColumnHeader)item;
                    ch.Width = Convert.ToInt32(arr[i]);
                    i++;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex);
            }
        }
        private void lvLF_MouseDown(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo lvht = lvLF.HitTest(e.Location);
            addToolStripMenuItem.Text = "New LF";
            deleteToolStripMenuItem.Text = "Delete LF";
            addToolStripMenuItem.Tag = "lf";
            deleteToolStripMenuItem.Tag = "lf";
            if (lvht.Location == ListViewHitTestLocations.None)
            {
                deleteToolStripMenuItem.Enabled = false;
            }
            else if (lvht.Location == ListViewHitTestLocations.Label)
            {
                deleteToolStripMenuItem.Enabled = true;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void OpenGMS(bool isNew)
        {
            if (_frmEditGMS == null)
            {
                _frmEditGMS = new frmEditGMS();
                _frmEditGMS.FormClosed += (o, ea) => _frmEditGMS = null;
                _frmEditGMS.ParentForm = this;
                _frmEditGMS.CatchCompRowGUID = _CurrentCatchRowNo;
                _frmEditGMS.CatchNameGUID = _CurrentCatchNameGUID;
                _frmEditGMS.taxa = _CurrentCatchTaxa;
                _frmEditGMS.CatchName = _CurrentCatchName;

                if (isNew)
                {
                    _frmEditGMS.AddNew();
                    _frmEditGMS.Text = "New GMS item";
                }
                else
                {
                    _frmEditGMS.Text = "Edit GMS item";
                }
            }
            else
            {
                _frmEditGMS.WindowState = FormWindowState.Normal;
            }

            try
            {
                _frmEditGMS.Show(this);
            }
            catch
            {
                _frmEditGMS.Focus();
            }
            _frmEditGMS.FocusStart();
        }

        private void OpenLF(bool isNew)
        {
            if (_frmEditLF == null)
            {
                _frmEditLF = new frmEditLF();
                _frmEditLF.FormClosed += (o, ea) => _frmEditLF = null;
                _frmEditLF.ParentForm = this;
                _frmEditLF.CatchCompRowGUID = _CurrentCatchRowNo;
                _frmEditLF.CurrentCatchName = _CurrentCatchName;
                if (isNew)
                {
                    _frmEditLF.AddNew();
                    _frmEditLF.Text = "New length frequency item";
                }
                else
                {
                    _frmEditLF.Text = "Edit length frequency item";
                }
            }
            else
            {
                _frmEditLF.WindowState = FormWindowState.Normal;
            }

            try
            {
                _frmEditLF.Show(this);
            }
            catch
            {
                _frmEditLF.Focus();
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (addToolStripMenuItem.Tag.ToString() == "lf")
            {
                OpenLF(true);
            }
            else if (addToolStripMenuItem.Tag.ToString() == "gms")
            {
                OpenGMS(true);
            }
        }

        private void lvLF_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem lvi = lvLF.SelectedItems[0];
            OpenLF(false);
            _frmEditLF.CurrentCatchName = _CurrentCatchName;
            _frmEditLF.LFData(lvi.Tag.ToString(), double.Parse(lvi.SubItems[1].Text), long.Parse(lvi.SubItems[2].Text));
        }



        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (deleteToolStripMenuItem.Tag.ToString() == "lf")
            {
                ListViewItem lvi = lvLF.SelectedItems[0];
                if (sampling.DeleteLFLine(lvi.Name))
                {
                    lvLF.Items.Remove(lvi);
                    
                    // we will renumber the first column
                    foreach (ListViewItem i in lvLF.Items)
                    {
                        i.Text = (i.Index + 1).ToString();
                    }

                    ComputeTotalMeasurements(lvLF);
                    FillChart();
                }
            }
            else if (deleteToolStripMenuItem.Tag.ToString() == "gms")
            {
                ListViewItem lvi = lvGMS.SelectedItems[0];
                if (sampling.DeleteGMSLine(lvi.Name))
                {
                    lvGMS.Items.Remove(lvi);
                    
                    // we will renumber the first column
                    foreach (ListViewItem i in lvGMS.Items)
                    {
                        i.Text = (i.Index + 1).ToString();
                    }

                    ComputeTotalMeasurements(lvGMS);
                    FillChart();
                }
            }
        }

        private void lvGMS_MouseDown(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo lvht = lvLF.HitTest(e.Location);
            addToolStripMenuItem.Text = "New GMS";
            addToolStripMenuItem.Tag = "gms";
            deleteToolStripMenuItem.Text = "Delete GMS";
            deleteToolStripMenuItem.Tag = "gms";
            if (lvht.Location == ListViewHitTestLocations.None)
            {
                deleteToolStripMenuItem.Enabled = false;
            }
            else if (lvht.Location == ListViewHitTestLocations.Label)
            {
                deleteToolStripMenuItem.Enabled = true;
            }
        }

        private void lvGMS_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem lvi = lvGMS.SelectedItems[0];
            OpenGMS(false);

            double? len = (lvi.SubItems[1].Text == "") ? (double?)null : double.Parse(lvi.SubItems[1].Text);
            double? wt = (lvi.SubItems[2].Text == "") ? (double?)null : double.Parse(lvi.SubItems[2].Text);
            double? gonadwt = (lvi.SubItems[5].Text == "") ? (double?)null : double.Parse(lvi.SubItems[5].Text) ;

            string myStage = lvi.SubItems[4].Text;
            global.FishCrabGMS stage = global.MaturityStageFromText(myStage, _CurrentCatchTaxa);

            string mySex = lvi.SubItems[3].Text;
            global.sex sex = (global.sex)Enum.Parse(typeof(global.sex), mySex);

            _frmEditGMS.CatchName = _CurrentCatchName;
            _frmEditGMS.GMSData(lvi.Name,len,wt,sex,stage, gonadwt,_CurrentCatchTaxa);
        }

        private void tabLFGMS_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView lv = new ListView();
            if (((TabControl)sender).SelectedTab.Name=="tabLF")
            {
                lv = lvLF;
            }
            else if (((TabControl)sender).SelectedTab.Name == "tabGMS")
            {
                lv = lvGMS;
            }

            ComputeTotalMeasurements(lv);
        }

        private void lvLF_Leave(object sender, EventArgs e)
        {
            frmMain.SaveColumnWidthEx(sender, myContext: global.lvContext.LengthFreq);
        }

        private void lvGMS_Leave(object sender, EventArgs e)
        {
            frmMain.SaveColumnWidthEx(sender, myContext: global.lvContext.GMS);
        }
    }
}

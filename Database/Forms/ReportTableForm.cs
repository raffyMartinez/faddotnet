using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FAD3.Database.Classes;

using System.Data;

namespace FAD3.Database.Forms
{
    public partial class ReportTableForm : Form
    {
        private static ReportTableForm _instance;

        public TargetArea TargetArea { get; set; }
        public string Topic { get; set; }
        public List<int> Years { get; set; }
        private DataSet _dataSet;

        public void ShowReport()
        {
            lvTable.Visible = false;
            ReportGeneratorClass.TargetArea = TargetArea;
            ReportGeneratorClass.Topic = Topic;
            ReportGeneratorClass.Years = Years;
            ReportGeneratorClass.Generate();
            _dataSet = ReportGeneratorClass.DataSet;
            lvTable.Columns.Clear();
            var ch = lvTable.Columns.Add("Row");
            foreach (DataColumn col in _dataSet.Tables[0].Columns)
            {
                ch = lvTable.Columns.Add(col.ColumnName);
                switch (col.DataType.Name)
                {
                    case "Double":
                    case "Int32":
                    case "DateTime":
                    case "Decimal":
                        ch.TextAlign = HorizontalAlignment.Right;
                        break;
                }
            }

            SizeColumns(lvTable);

            bool done = false;
            double weightCatch = 0;
            double? weightSample = null;
            double weightSpecies = 0;
            bool fromTotal = false;
            int? countSpecies = null;
            double? subSampleWeight = null;
            int? subSampleCount = null;
            foreach (DataRow dr in _dataSet.Tables[0].Rows)
            {
                var colName = "";
                ListViewItem lvi = new ListViewItem();
                for (int n = 0; n < _dataSet.Tables[0].Columns.Count; n++)
                {
                    if (n == 0)
                    {
                        lvi = lvTable.Items.Add((lvTable.Items.Count + 1).ToString());
                    }

                    switch (dr[n].GetType().Name)
                    {
                        case "DateTime":
                            DateTime dt = DateTime.Parse(dr[n].ToString());
                            if (dt.Year == 1899)
                            {
                                lvi.SubItems.Add(string.Format("{0:HH:mm}", dt));
                            }
                            else
                            {
                                lvi.SubItems.Add(string.Format("{0:MMM-dd-yyyy}", dt));
                            }

                            break;

                        case "Double":
                            lvi.SubItems.Add(((double)dr[n]).ToString("N2"));
                            break;

                        case "Decimal":
                            lvi.SubItems.Add(((Decimal)dr[n]).ToString("N2"));
                            break;

                        default:
                            lvi.SubItems.Add(dr[n].ToString());
                            break;
                    }

                    colName = lvTable.Columns[n].Text;

                    switch (Topic)
                    {
                        case "catch":
                            if (colName == "From total")
                            {
                                countSpecies = null;
                                subSampleCount = null;
                                subSampleWeight = null;
                                weightSample = null;
                                weightCatch = (double)dr["Weight of catch"];
                                if (double.TryParse(dr["Weight of sample"].ToString(), out double wtSample))
                                {
                                    weightSample = wtSample;
                                }
                                weightSpecies = (double)dr["Weight"];
                                fromTotal = (bool)dr["From total"];

                                if (int.TryParse(dr["Count"].ToString(), out int count))
                                {
                                    countSpecies = count;
                                }
                                if (countSpecies == null)
                                {
                                    subSampleWeight = (double)dr["Subsample weight"];
                                    subSampleCount = (int)dr["Subsample count"];
                                }
                                done = true;
                            }
                            if (done)
                            {
                                if (fromTotal)
                                {
                                    lvi.SubItems[n + 1].Text = weightSpecies.ToString("N2");
                                    if (countSpecies == null)
                                    {
                                        lvi.SubItems.Add(((int)((weightSpecies / subSampleWeight) * subSampleCount)).ToString());
                                    }
                                    else
                                    {
                                        lvi.SubItems.Add(countSpecies.ToString());
                                    }
                                }
                                else if (countSpecies == null)
                                {
                                    lvi.SubItems[n + 1].Text = weightSpecies.ToString("N2");
                                    lvi.SubItems.Add(((int)((weightSpecies / subSampleWeight) * subSampleCount)).ToString());
                                }
                                else
                                {
                                    if (weightSample != null)
                                    {
                                        lvi.SubItems[n + 1].Text = (weightSpecies * (double)(weightCatch / weightSample)).ToString("N2");
                                        lvi.SubItems.Add(((int)(countSpecies * (weightCatch / weightSample))).ToString());
                                    }
                                    else
                                    {
                                        lvi.SubItems[n + 1].Text = "";
                                        lvi.SubItems.Add("");
                                    }
                                }
                                done = false;
                            }

                            break;
                    }
                }
            }

            SizeColumns(lvTable, false);
            lvTable.Visible = true;
        }

        public static ReportTableForm GetInstance(TargetArea targetArea, string topic, List<int> years)
        {
            if (_instance == null) return new ReportTableForm(targetArea, topic, years);
            return _instance;
        }

        public ReportTableForm(TargetArea targetArea, string topic, List<int> years)
        {
            InitializeComponent();
            TargetArea = targetArea;
            Topic = topic;
            Years = years;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            global.LoadFormSettings(this);
            lvTable.View = View.Details;
            lvTable.FullRowSelect = true;
            ShowReport();
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

        private void ReportTableForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _instance = null;
            global.SaveFormSettings(this);
        }
    }
}
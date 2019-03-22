using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using FAD3.Mapping.Classes;

namespace FAD3.GUI.Forms
{
    public partial class GraphForm : Form
    {
        public string DataFile { get; set; }
        private bool _readingDataLines;
        private string _xVariableName;
        private Dictionary<int, string> _series;
        private bool _hasLongName;
        private static GraphForm _instance;
        private Font _newFont;

        public static GraphForm GetInstance()
        {
            if (_instance == null) _instance = new GraphForm();
            return _instance;
        }

        private string getAfter(string strSource, string strStart)
        {
            int Start;
            if (strSource.Contains(strStart))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                return strSource.Substring(Start, strSource.Length - Start);
            }
            else
            {
                return "";
            }
        }

        private string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

        public void MapSpatioTemporalData(SeriesChartType chartType)
        {
            //chartType = SeriesChartType.StackedColumn100;
            if (DataFile.Length > 0)
            {
                _hasLongName = false;
                string variableLongName = "";
                string variableUnit = "";
                int seriesNo = 0;
                chart.Series.Clear();
                TextFieldParser tfp = new TextFieldParser(new StringReader(""));
                using (StreamReader strm = new StreamReader(DataFile, true))
                {
                    int inlandPoints = 0;
                    string line;
                    while ((line = strm.ReadLine()) != null)
                    {
                        if (line == "#BeginData#")
                        {
                            _readingDataLines = true;
                        }
                        else if (line == "#EndData#")
                        {
                            _readingDataLines = false;
                            chart.ChartAreas[0].AxisY.Minimum = 0;
                            chart.ChartAreas[0].AxisY.Maximum = 100;
                            chart.ChartAreas[0].AxisX.Title = _xVariableName;
                            chart.ChartAreas[0].AxisX.IsLabelAutoFit = false;
                            chart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                            //chart.Series["Null"].Color = Color.AntiqueWhite;
                        }
                        else if (line.Contains("Inland points:"))
                        {
                            inlandPoints = int.Parse(getAfter(line, "Inland points:").Trim());
                        }
                        else if (line.Contains("Variable unit:"))
                        {
                            variableUnit = getAfter(line, "Variable unit:").Trim();
                            chart.Legends[0].Name = variableUnit;
                            chart.Legends[0].Title = variableUnit;
                        }
                        else if (line.Contains("Variable long name:"))
                        {
                            _hasLongName = true;
                            variableLongName = getAfter(line, "Variable long name:").Trim();
                            Text = $"Time series: {variableLongName}";
                        }
                        else if (line.Contains("Grid variable:"))
                        {
                            _xVariableName = line.Substring(line.IndexOf("Grid variable:") + "Grid Variable:".Length, line.Length - (line.IndexOf("Grid variable:") + "Grid Variable:".Length)).Trim();
                        }
                        else if (line.Contains("Category "))
                        {
                            string seriesName = getBetween(line, ": ", " Color");
                            _series.Add(seriesNo, seriesName);
                            var chartSeries = chart.Series.Add(seriesName);
                            chartSeries.Color = Mapping.Colors.FromARGBString($"{getAfter(line, "Color:")}");
                            chartSeries.ChartType = chartType;
                            seriesNo++;
                        }
                        else if (_readingDataLines)
                        {
                            string[] fields;
                            tfp = new TextFieldParser(new StringReader(line));
                            tfp.HasFieldsEnclosedInQuotes = true;
                            tfp.SetDelimiters("\t");

                            while (!tfp.EndOfData)
                            {
                                fields = tfp.ReadFields();
                                if (fields[0] == "Time period")
                                {
                                }
                                else
                                {
                                    for (int n = 1; n < fields.Count(); n++)
                                    {
                                        var dp = new DataPoint();
                                        if (n == fields.Count() - 1)
                                        {
                                            dp = chart.Series[_series[n - 1]].Points.Add(int.Parse(fields[n]) - inlandPoints);
                                        }
                                        else
                                        {
                                            dp = chart.Series[_series[n - 1]].Points.Add(int.Parse(fields[n]));
                                        }
                                        dp.AxisLabel = DateTime.Parse(fields[0]).ToString("MMM-dd-yyyy");
                                    }
                                }
                            }
                        }
                    }
                }
                if (!_hasLongName)
                {
                    Text = $"Time series: {_xVariableName}";
                }
            }
        }

        public GraphForm()
        {
            InitializeComponent();
            _series = new Dictionary<int, string>();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            global.LoadFormSettings(this);
            chart.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
            txtLabelInterval.Text = chart.ChartAreas[0].AxisX.LabelStyle.Interval.ToString();
            chart.ChartAreas[0].AxisX.MinorTickMark.Enabled = true;
            chart.ChartAreas[0].AxisX.MinorTickMark.Interval = 1;
            txtMinorGridInterval.Text = chart.ChartAreas[0].AxisX.MinorTickMark.Interval.ToString();
            chart.ChartAreas[0].AxisX.MinorTickMark.Size = 1;
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            global.SaveFormSettings(this);
            _instance = null;
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnApply":
                    chart.ChartAreas[0].AxisX.LabelStyle.Interval = int.Parse(txtLabelInterval.Text);
                    chart.ChartAreas[0].AxisX.MinorTickMark.Interval = int.Parse(txtMinorGridInterval.Text);
                    if (_newFont != null)
                    {
                        chart.ChartAreas[0].AxisX.LabelStyle.Font = _newFont;
                    }
                    tabsChart.SelectedTab = tabsChart.TabPages["tabGraph"];
                    _newFont = null;
                    //tabGraph.Select();
                    break;

                case "btnXaxisLabelFont":
                    _newFont = null;
                    FontDialog fd = new FontDialog();
                    fd.Font = chart.ChartAreas[0].AxisX.LabelStyle.Font;
                    DialogResult dr = fd.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        _newFont = fd.Font;
                    }
                    break;
            }
        }
    }
}
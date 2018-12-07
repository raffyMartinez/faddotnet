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
using FAD3.GUI.Classes;
using ClosedXML.Excel;
using System.IO;

namespace FAD3.Database.Forms
{
    public partial class GearInventoryTabularForm : Form
    {
        private Dictionary<string, string> _columnDataType = new Dictionary<string, string>();
        private static GearInventoryTabularForm _instance;
        private string _inventoryGuid;
        private FishingGearInventory _inventory;
        private Dictionary<string, (string month, string type)> _monthsFishing = new Dictionary<string, (string month, string type)>();

        private Dictionary<string, (int countCommercial, int countMotorized, int countNonMotorized,
            int countNoBoat, int? maxCPUE, int? minCPUE,
            int? upperMode, int? lowerMode, int numberDaysUsed,
            string cpueUnit, string Notes, int dominantPercent,
            int? averageCPUE, int? cpueMode, double? equivalentKg)> _inventoryData = new
            Dictionary<string, (int countCommercial, int countMotorized, int countNonMotorized,
            int countNoBoat, int? maxCPUE, int? minCPUE,
            int? upperMode, int? lowerMode, int numberDaysUsed,
            string cpueUnit, string Notes, int dominantPercent,
            int? averageCPUE, int? cpueMode, double? equivalentKg)>();

        private Dictionary<string, (string projectName, string province, string lgu, string barangay, string sitio,
            string enumerator, DateTime surveyDate, string gearClass, string gearVariation)> _headers = new
            Dictionary<string, (string projectName, string province, string lgu, string barangay, string sitio,
            string enumerator, DateTime surveyDate, string gearClass, string gearVariation)>();

        public bool ShowProjectColumn { get; set; }
        public string InventoryProjectName { get; set; }

        public static GearInventoryTabularForm GetInstance(FishingGearInventory inventory, string inventoryGuid)
        {
            if (_instance == null) return new GearInventoryTabularForm(inventory, inventoryGuid);
            return _instance;
        }

        public GearInventoryTabularForm(FishingGearInventory inventory, string inventoryGuid)
        {
            InitializeComponent();
            _inventory = inventory;
            _inventoryGuid = inventoryGuid;
            ShowProjectColumn = true;
        }

        private void OnNodeAfterSelect(object sender, TreeViewEventArgs e)
        {
            switch (e.Node.Name)
            {
                case "nodeProject":
                    ShowProject();
                    break;

                case "nodeFisherVessel":
                    ShowFisherVessel();
                    break;

                case "nodeGear":
                    ShowGearLocalNames();
                    break;

                case "nodeGearCount":
                    ShowGearCounts();
                    break;

                case "nodeGearOperation":
                    ShowGearDaysInUse();
                    break;

                case "nodeMonths":
                    ShowMonthsFishing();
                    break;

                case "nodePeak":
                    ShowMonthsPeak();
                    break;

                case "nodeCPUE":
                    ShowCPUE();
                    break;

                case "nodeGearCPUEHistory":
                    ShowCPUEHistory();
                    break;

                case "nodeCatchComp":
                    ShowCatchComposition();
                    break;

                case "nodeAccessories":
                    ShowFishingAccessories();
                    break;

                case "nodeExpenses":
                    ShowExpenses();
                    break;

                case "nodeNotes":
                    ShowNotes();
                    break;

                case "nodeRespondents":
                    ShowRespondents();
                    break;
            }
        }

        private void ShowCPUEHistory()
        {
            FillHeaderRows(showCPUETrend: true);
        }

        private void ShowNotes()
        {
            FillHeaderRows();
            listResults.Columns.Add("Notes");
            SizeColumns(listResults);
            foreach (ListViewItem lvi in listResults.Items)
            {
                lvi.SubItems.Add(_inventoryData[lvi.Name].Notes);
            }
            SizeColumns(listResults, false);
        }

        private void ShowFishingAccessories()
        {
            FillHeaderRows();
            listResults.Columns.Add("Accessories used in fishing");
            SizeColumns(listResults);
            foreach (ListViewItem lvi in listResults.Items)
            {
                var accessories = "";
                foreach (var accessory in _inventory.GetFishingAccessories(lvi.Name))
                {
                    accessories += accessory + ", ";
                }
                lvi.SubItems.Add(accessories.Trim(new char[] { ',', ' ' }));
            }
            SizeColumns(listResults, false);
        }

        private void ShowCatchComposition()
        {
            ColumnHeader col = new ColumnHeader();
            FillHeaderRows();
            listResults.Columns.Add("Composition of dominant catch");
            listResults.Columns.Add("Composition of non-dominant catch");
            col = listResults.Columns.Add("Percentage of dominance");
            AddColumnDataType(col, "int");
            SizeColumns(listResults);
            foreach (ListViewItem lvi in listResults.Items)
            {
                var catchComp = _inventory.GetCatchComposition(lvi.Name);
                var names = "";
                foreach (var name in _inventory.GetCatchComposition(lvi.Name))
                {
                    if (name.isDominant)
                    {
                        names += name.localName + ", ";
                    }
                }
                lvi.SubItems.Add(names.Trim(new char[] { ',', ' ' }));

                names = "";
                foreach (var name in _inventory.GetCatchComposition(lvi.Name))
                {
                    if (!name.isDominant)
                    {
                        names += name.localName + ", ";
                    }
                }
                lvi.SubItems.Add(names.Trim(new char[] { ',', ' ' }));

                lvi.SubItems.Add(_inventoryData[lvi.Name].dominantPercent.ToString());
            }
            SizeColumns(listResults, false);
        }

        private void ShowCPUE()
        {
            var col = new ColumnHeader();
            FillHeaderRows();
            col = listResults.Columns.Add("Maximum CPUE");
            AddColumnDataType(col, "int");
            col = listResults.Columns.Add("Minimum CPUE");
            AddColumnDataType(col, "int");
            col = listResults.Columns.Add("Average CPUE");
            AddColumnDataType(col, "int");
            col = listResults.Columns.Add("Upper CPUE mode");
            AddColumnDataType(col, "int");
            col = listResults.Columns.Add("Lower CPUE mode");
            AddColumnDataType(col, "int");
            col = listResults.Columns.Add("CPUE mode");
            AddColumnDataType(col, "int");
            listResults.Columns.Add("Unit");
            col = listResults.Columns.Add("Equivalent kg");
            AddColumnDataType(col, "double");
            SizeColumns(listResults);
            foreach (ListViewItem lvi in listResults.Items)
            {
                lvi.SubItems.Add(_inventoryData[lvi.Name].maxCPUE.ToString());
                lvi.SubItems.Add(_inventoryData[lvi.Name].minCPUE.ToString());
                lvi.SubItems.Add(_inventoryData[lvi.Name].averageCPUE.ToString());
                lvi.SubItems.Add(_inventoryData[lvi.Name].upperMode.ToString());
                lvi.SubItems.Add(_inventoryData[lvi.Name].lowerMode.ToString());
                lvi.SubItems.Add(_inventoryData[lvi.Name].cpueMode.ToString());
                lvi.SubItems.Add(_inventoryData[lvi.Name].cpueUnit);
                lvi.SubItems.Add(_inventoryData[lvi.Name].equivalentKg.ToString());
            }
            SizeColumns(listResults, false);
        }

        private void ShowExpenses()
        {
            FillHeaderRows(showExpenses: true);
        }

        private void AddColumnDataType(ColumnHeader col, string type)
        {
            if (!_columnDataType.ContainsKey(col.Text))
            {
                _columnDataType.Add(col.Text, type)
;
            }
        }

        private void FillHeaderRows(bool showExpenses = false, bool showCPUETrend = false)
        {
            var col = new ColumnHeader();
            listResults.Visible = false;
            listResults.Clear();
            if (ShowProjectColumn)
            {
                listResults.Columns.Add("Project");
            }
            listResults.Columns.Add("Province");
            listResults.Columns.Add("LGU");
            listResults.Columns.Add("Barangay");
            listResults.Columns.Add("Sitio");
            listResults.Columns.Add("Enumerator");
            col = listResults.Columns.Add("Date surveyed");
            AddColumnDataType(col, "date");
            listResults.Columns.Add("Gear class");
            listResults.Columns.Add("Gear variation");

            if (showExpenses)
            {
                listResults.Columns.Add("Expense item");
                col = listResults.Columns.Add("Cost");
                AddColumnDataType(col, "double");
                listResults.Columns.Add("Source");
                listResults.Columns.Add("Notes");
                SizeColumns(listResults);
            }

            if (showCPUETrend)
            {
                listResults.Columns.Add("Decade");
                col = listResults.Columns.Add("CPUE");
                AddColumnDataType(col, "int");
                listResults.Columns.Add("Unit");
                SizeColumns(listResults);
            }

            ListViewItem lvi = new ListViewItem();
            foreach (var item in _headers)
            {
                if (ShowProjectColumn)
                {
                    lvi = listResults.Items.Add(item.Key, item.Value.projectName, null);
                    lvi.SubItems.Add(item.Value.province);
                }
                else
                {
                    lvi = listResults.Items.Add(item.Key, item.Value.province, null);
                }
                lvi.SubItems.Add(item.Value.lgu);
                lvi.SubItems.Add(item.Value.barangay);
                var sitio = item.Value.sitio;
                if (sitio.Length > 0)
                {
                    lvi.SubItems.Add(sitio);
                }
                else
                {
                    lvi.SubItems.Add("Entire barangay");
                }
                lvi.SubItems.Add(item.Value.enumerator);
                lvi.SubItems.Add(string.Format("{0:MMM-dd-yyyy}", item.Value.surveyDate));
                lvi.SubItems.Add(item.Value.gearClass);
                lvi.SubItems.Add(item.Value.gearVariation);

                if (showExpenses)
                {
                    var costLine = 0;
                    foreach (var expense in _inventory.GetExpenses(lvi.Name))
                    {
                        if (costLine > 0)
                        {
                            for (int n = 1; n < 10; n++)
                            {
                                if (n == 1)
                                {
                                    lvi = listResults.Items.Add("");
                                }
                                else
                                {
                                    lvi.SubItems.Add("");
                                }
                            }
                        }
                        lvi.SubItems.Add(expense.expenseItem);
                        lvi.SubItems.Add(expense.cost.ToString());
                        lvi.SubItems.Add(expense.source);
                        lvi.SubItems.Add(expense.notes);
                        costLine++;
                    }
                }

                if (showCPUETrend)
                {
                    var cpueLine = 0;
                    foreach (var cpue in _inventory.GetCPUEHistorical(lvi.Name))
                    {
                        if (cpueLine > 0)
                        {
                            for (int n = 1; n < 10; n++)
                            {
                                if (n == 1)
                                {
                                    lvi = listResults.Items.Add("");
                                }
                                else
                                {
                                    lvi.SubItems.Add("");
                                }
                            }
                        }
                        lvi.SubItems.Add(cpue.decade.ToString() + "s");
                        lvi.SubItems.Add(cpue.cpue.ToString());
                        lvi.SubItems.Add(cpue.unit);
                        cpueLine++;
                    }
                }
            }

            if (showExpenses || showCPUETrend)
            {
                SizeColumns(listResults, false);
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            treeInventory.SelectedNode = treeInventory.Nodes["treeInventory"];
            _inventoryData = _inventory.GetInventoryData(_inventoryGuid);
            _headers = _inventory.GetGearRowHeaders(_inventoryGuid);
            ShowProject();
            global.LoadFormSettings(this);
            Text = "Inventory of fishers, fishing vessels and gears tables";
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
            global.SaveFormSettings(this);
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
            lv.Visible = !init;
        }

        private void ShowProject()
        {
            var col = new ColumnHeader();
            listResults.Clear();
            listResults.Columns.Add("Project name");
            col = listResults.Columns.Add("Date implemented");
            AddColumnDataType(col, "date");
            SizeColumns(listResults);
            var result = _inventory.GetInventory(_inventoryGuid);
            var lvi = listResults.Items.Add(result.inventoryName);
            lvi.SubItems.Add(string.Format("{0:MMM-dd-yyyy}", result.dateImplemented));
            SizeColumns(listResults, false);
        }

        private void ShowMonthsPeak()
        {
            FillHeaderRows();
            listResults.Columns.Add("Jan");
            listResults.Columns.Add("Feb");
            listResults.Columns.Add("Mar");
            listResults.Columns.Add("Apr");
            listResults.Columns.Add("May");
            listResults.Columns.Add("Jun");
            listResults.Columns.Add("Jul");
            listResults.Columns.Add("Aug");
            listResults.Columns.Add("Sep");
            listResults.Columns.Add("Oct");
            listResults.Columns.Add("Nov");
            listResults.Columns.Add("Dec");
            SizeColumns(listResults);
            foreach (ListViewItem lvi in listResults.Items)
            {
                for (int n = 1; n < 13; n++)
                {
                    lvi.SubItems.Add("");
                }

                foreach (var month in _inventory.GetMonthsFishing(lvi.Name, true))
                {
                    lvi.SubItems[month + 8].Text = "x";
                }
            }
            SizeColumns(listResults, false);
        }

        private void ShowMonthsFishing()
        {
            FillHeaderRows();
            listResults.Columns.Add("Jan");
            listResults.Columns.Add("Feb");
            listResults.Columns.Add("Mar");
            listResults.Columns.Add("Apr");
            listResults.Columns.Add("May");
            listResults.Columns.Add("Jun");
            listResults.Columns.Add("Jul");
            listResults.Columns.Add("Aug");
            listResults.Columns.Add("Sep");
            listResults.Columns.Add("Oct");
            listResults.Columns.Add("Nov");
            listResults.Columns.Add("Dec");
            SizeColumns(listResults);
            foreach (ListViewItem lvi in listResults.Items)
            {
                for (int n = 1; n < 13; n++)
                {
                    lvi.SubItems.Add("");
                }

                foreach (var month in _inventory.GetMonthsFishing(lvi.Name))
                {
                    lvi.SubItems[month + 8].Text = "x";
                }
            }
            SizeColumns(listResults, false);
        }

        private void ShowGearCounts()
        {
            var col = new ColumnHeader();
            FillHeaderRows();
            col = listResults.Columns.Add("Count in commercial vessels");
            AddColumnDataType(col, "int");
            col = listResults.Columns.Add("Count in motorized municipal vessels");
            AddColumnDataType(col, "int");
            col = listResults.Columns.Add("Count in non-motorized municipal vessels");
            AddColumnDataType(col, "int");
            col = listResults.Columns.Add("Count in no vessels");
            AddColumnDataType(col, "int");
            col = listResults.Columns.Add("Total number of gears");
            AddColumnDataType(col, "int");
            SizeColumns(listResults);
            foreach (ListViewItem lvi in listResults.Items)
            {
                lvi.SubItems.Add(_inventoryData[lvi.Name].countCommercial.ToString());
                lvi.SubItems.Add(_inventoryData[lvi.Name].countMotorized.ToString());
                lvi.SubItems.Add(_inventoryData[lvi.Name].countNonMotorized.ToString());
                lvi.SubItems.Add(_inventoryData[lvi.Name].countNoBoat.ToString());
                lvi.SubItems.Add((_inventoryData[lvi.Name].countNoBoat +
                    _inventoryData[lvi.Name].countCommercial +
                    _inventoryData[lvi.Name].countMotorized +
                    _inventoryData[lvi.Name].countNonMotorized).ToString());
            }
            SizeColumns(listResults, false);
        }

        private void ShowGearLocalNames()
        {
            FillHeaderRows();
            listResults.Columns.Add("Local names");
            SizeColumns(listResults);
            foreach (ListViewItem lvi in listResults.Items)
            {
                var localNames = "";
                foreach (var name in _inventory.GetGearLocalNamesInventory(lvi.Name))
                {
                    localNames += name + ", ";
                }
                lvi.SubItems.Add(localNames.Trim(new char[] { ',', ' ' }));
            }
            SizeColumns(listResults, false);
        }

        /// <summary>
        /// returns month string based on inputted month number
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private string MonthFromNumber(int m)
        {
            string month = "";
            switch (m)
            {
                case 1:
                    month = "January";
                    break;

                case 2:
                    month = "February";
                    break;

                case 3:
                    month = "March";
                    break;

                case 4:
                    month = "April";
                    break;

                case 5:
                    month = "May";
                    break;

                case 6:
                    month = "June";
                    break;

                case 7:
                    month = "July";
                    break;

                case 8:
                    month = "August";
                    break;

                case 9:
                    month = "September";
                    break;

                case 10:
                    month = "October";
                    break;

                case 11:
                    month = "November";
                    break;

                case 12:
                    month = "December";
                    break;
            }
            return month;
        }

        private void ShowGearDaysInUse()
        {
            var col = new ColumnHeader();
            FillHeaderRows();
            col = listResults.Columns.Add("Number of days in use per month");
            AddColumnDataType(col, "int");
            SizeColumns(listResults);
            foreach (ListViewItem lvi in listResults.Items)
            {
                lvi.SubItems.Add(_inventoryData[lvi.Name].numberDaysUsed.ToString());
            }
            SizeColumns(listResults, false);
        }

        private void ShowRespondents()
        {
            var col = new ColumnHeader();
            var rowCount = 0;
            listResults.Clear();
            listResults.Columns.Add("Project");
            listResults.Columns.Add("Province");
            listResults.Columns.Add("LGU");
            listResults.Columns.Add("Barangay");
            listResults.Columns.Add("Sitio");
            listResults.Columns.Add("Enumerator");
            col = listResults.Columns.Add("Date surveyed");
            AddColumnDataType(col, "date");
            col = listResults.Columns.Add("Respondents");
            AddColumnDataType(col, "string");
            SizeColumns(listResults);
            foreach (var item in _inventory.GetFisherVesselInventory(_inventoryGuid))
            {
                var lvi = listResults.Items.Add(item.brgyInventoryGuid, item.project, null);
                lvi.SubItems.Add(item.province);
                lvi.SubItems.Add(item.lgu);
                lvi.SubItems.Add(item.barangay);
                var sitio = item.sitio;
                if (sitio.Length > 0)
                {
                    lvi.SubItems.Add(sitio);
                }
                else
                {
                    lvi.SubItems.Add("Entire barangay");
                }
                lvi.SubItems.Add(item.enumerator);
                lvi.SubItems.Add(string.Format("{0:MMM-dd-yyyy}", item.dateSurveyed));

                rowCount = 0;
                foreach (var responder in (_inventory.GetSitioRespondents(lvi.Name)))
                {
                    if (rowCount == 0)
                    {
                        lvi.SubItems.Add(responder);
                    }
                    else
                    {
                        for (int n = 0; n < 7; n++)
                        {
                            if (n == 0)
                            {
                                lvi = listResults.Items.Add("");
                            }
                            else
                            {
                                lvi.SubItems.Add("");
                            }
                        }
                        lvi.SubItems.Add(responder);
                    }
                    rowCount++;
                }
            }
            SizeColumns(listResults, false);
        }

        private void ShowFisherVessel()
        {
            var col = new ColumnHeader();
            listResults.Clear();
            listResults.Columns.Add("Project");
            listResults.Columns.Add("Province");
            listResults.Columns.Add("LGU");
            listResults.Columns.Add("Barangay");
            listResults.Columns.Add("Sitio");
            listResults.Columns.Add("Enumerator");
            col = listResults.Columns.Add("Date surveyed");
            AddColumnDataType(col, "date");
            col = listResults.Columns.Add("Number of fishers");
            AddColumnDataType(col, "int");
            col = listResults.Columns.Add("Number of commercial vessels");
            AddColumnDataType(col, "int");
            col = listResults.Columns.Add("Number of municipal motorized vessels");
            AddColumnDataType(col, "int");
            col = listResults.Columns.Add("Number of municipal non motorized vessels");
            AddColumnDataType(col, "int");

            SizeColumns(listResults);

            foreach (var item in _inventory.GetFisherVesselInventory(_inventoryGuid))
            {
                var lvi = listResults.Items.Add(item.project);
                lvi.SubItems.Add(item.province);
                lvi.SubItems.Add(item.lgu);
                lvi.SubItems.Add(item.barangay);
                var sitio = item.sitio;
                if (sitio.Length > 0)
                {
                    lvi.SubItems.Add(sitio);
                }
                else
                {
                    lvi.SubItems.Add("Entire barangay");
                }
                lvi.SubItems.Add(item.enumerator);
                lvi.SubItems.Add(string.Format("{0:MMM-dd-yyyy}", item.dateSurveyed));
                lvi.SubItems.Add(item.fisherCount.ToString());
                lvi.SubItems.Add(item.commercialCount.ToString());
                lvi.SubItems.Add(item.motorizedCount.ToString());
                lvi.SubItems.Add(item.nonMotorizedCount.ToString());
            }
            SizeColumns(listResults, false);
        }

        private DataTable ListViewToDataTable(ListView lv, string tableName)
        {
            DataTable dt = new DataTable();
            dt.TableName = tableName;
            foreach (ColumnHeader ch in lv.Columns)
            {
                DataColumn col = new DataColumn();
                col.ColumnName = ch.Text;
                if (_columnDataType.ContainsKey(col.ColumnName))
                {
                    switch (_columnDataType[col.ColumnName])
                    {
                        case "date":
                            col.DataType = typeof(DateTime);
                            break;

                        case "int":
                            col.DataType = typeof(int);
                            break;

                        case "double":
                            col.DataType = typeof(double);
                            break;
                    }
                }
                else
                {
                    col.DataType = typeof(string);
                }
                dt.Columns.Add(col);
            }

            foreach (ListViewItem lvi in lv.Items)
            {
                var col = 0;
                DataRow row = dt.NewRow();
                foreach (var subItem in lvi.SubItems)
                {
                    if (lvi.SubItems[col].Text.Length > 0)
                    {
                        row[lv.Columns[col].Text] = lvi.SubItems[col].Text;
                    }
                    col++;
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        private void PrintNodesRecursive(TreeNode nd, XLWorkbook wb)
        {
            TreeViewEventArgs e = new TreeViewEventArgs(nd);
            OnNodeAfterSelect(null, e);
            var wks = wb.Worksheets.Add(ListViewToDataTable(listResults, nd.Text));
            wks.Name = nd.Text;
            foreach (TreeNode subNode in nd.Nodes)
            {
                PrintNodesRecursive(subNode, wb);
            }
        }

        private void ExportInventoryXL(string fileName)
        {
            var wb = new XLWorkbook();

            foreach (TreeNode nd in treeInventory.Nodes)
            {
                PrintNodesRecursive(nd, wb);
            }
            wb.SaveAs(fileName);
            treeInventory.SelectedNode = treeInventory.Nodes["nodeProject"];
            ShowProject();
            MessageBox.Show($"Inventory data successfully save to {fileName}");
        }

        private void ExportInventoryXML(string fileName)
        {
            ;
        }

        private void OnToolBarItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "tsbExport":
                    FileDialogHelper.Title = "Provide filename for exporting fisher, vessel and fishing gear inventory";
                    FileDialogHelper.DialogType = FileDialogType.FileSave;
                    FileDialogHelper.DataFileType = DataFileType.Text | DataFileType.XML | DataFileType.CSV | DataFileType.Excel;
                    FileDialogHelper.FileName = InventoryProjectName;
                    FileDialogHelper.ShowDialog();

                    var fileName = FileDialogHelper.FileName;

                    if (fileName.Length > 0)
                    {
                        switch (Path.GetExtension(fileName))
                        {
                            case ".txt":

                                break;

                            case ".XML":
                            case ".xml":
                                ExportInventoryXML(fileName);
                                break;

                            case ".xlsx":
                                ExportInventoryXL(fileName);
                                break;
                        }
                    }
                    break;

                case "tsbClose":
                    Close();
                    break;
            }
        }
    }
}
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

/// <summary>
/// Provides a read-only view of the expenses and fishing gear specifications of a sampled fish landing
/// </summary>
///

namespace FAD3.Database.Forms
{
    public partial class ViewGearSpec_ExpensesForm : Form
    {
        private static ViewGearSpec_ExpensesForm _instance;
        public string SamplingGuid { get; set; }
        public string RefNumber { get; set; }
        public MainForm MainForm { get; set; }

        public static ViewGearSpec_ExpensesForm GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ViewGearSpec_ExpensesForm();
            }
            return _instance;
        }

        public ViewGearSpec_ExpensesForm()
        {
            InitializeComponent();
        }

        public void LoadSpecsAndExpenses()
        {
            lblRefNumber.Text = $"Reference No: {RefNumber}";
            if (SamplingGuid.Length > 0)
            {
                var s = ManageGearSpecsClass.GetSampledSpecsEx(SamplingGuid);
                if (s.Length == 0)
                    txtGearSpecs.Text = "Gear specs not found";
                else
                {
                    txtGearSpecs.Text = s;
                }

                var result = OperatingExpenses.ReadData(SamplingGuid, true);
                if (result.success)
                {
                    var expenseItem = result.exp;
                    txtExpenses.Text = OperatingExpenses.SamplingExpenses;
                }
                else
                {
                    txtExpenses.Text = "Fishing operation expenses not found";
                }
            }
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
            global.SaveFormSettings(this);
            MainForm.SpecExpenseViewwerFormStatus(visible: false);
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            global.LoadFormSettings(this, true);
            MainForm.SpecExpenseViewwerFormStatus(visible: true);
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
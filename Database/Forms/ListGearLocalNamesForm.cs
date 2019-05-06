using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;

namespace FAD3.Database.Forms
{
    public partial class ListGearLocalNamesForm : Form
    {
        private List<string> _localNamesList;
        private string _gearVariationName;

        public ListGearLocalNamesForm(List<string> localNames, string gearVariationName)
        {
            InitializeComponent();
            _localNamesList = localNames;
            _gearVariationName = gearVariationName;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            Text = $"Local names of {_gearVariationName}";
            foreach (var item in _localNamesList)
            {
                listBoxLocalNames.Items.Add(item);
            }
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        private void OnDropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "menuCopyText":
                    StringBuilder copyText = new StringBuilder();
                    foreach (var item in listBoxLocalNames.Items)
                    {
                        copyText.Append($"{item}\r\n");
                    }
                    Clipboard.SetText(copyText.ToString());
                    break;
            }
        }
    }
}
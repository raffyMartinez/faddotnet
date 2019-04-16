using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace FAD3
{
    public class mru
    {
        // The application's name.
        private string _ApplicationName;

        // A list of the files.
        private int _NumFiles;
        private List<FileInfo> FileInfos;

        // The File menu.
        private ToolStripMenuItem _MyMenu;

        // The menu items we use to display files.
        private ToolStripMenuItem[] MenuItems;

        // Raised when the user selects a file from the MRU list.
        public delegate void FileSelectedEventHandler(string file_name);
        public event FileSelectedEventHandler FileSelected;

        //Raise when the user wants to manage the MRU
        public delegate void ManageMRUEventHandler(object sender, EventArgs e);
        public event ManageMRUEventHandler ManageMRU;

        // Constructor.
        public mru()
        {
            //default
        }

        public mru(string application_name, ToolStripMenuItem menu, int num_files)
        {
            _ApplicationName = application_name;
            _MyMenu = menu;
            _NumFiles = num_files;
            FileInfos = new List<FileInfo>();

            // Make the menu items we may later need.
            MenuItems = new ToolStripMenuItem[_NumFiles + 1];
            for (int i = 0; i < _NumFiles; i++)
            {
                MenuItems[i] = new ToolStripMenuItem();
                MenuItems[i].Visible = false;
                _MyMenu.DropDownItems.Add(MenuItems[i]);
            }

            // Reload items from the registry.
            LoadFiles();

            // Display the items.
            ShowFiles();

            //Add menu item to manage this list
            if (FileInfos.Count > 0)
            {
                _MyMenu.DropDownItems.Add("-");
                var MenuItem = _MyMenu.DropDownItems.Add("Manage this list");
                MenuItem.Name = "menuManageMRUList";
                MenuItem.ToolTipText = "Manage list of recent items";
                MenuItem.Click += ManageMRU_Click;
            }
        }

        public List<FileInfo> FileList
        {
            get { return FileInfos; }
            set
            {
                FileInfos = value;
                ShowFiles();
                SaveFiles();
            }
        }

        // Load saved items from the Registry.
        private void LoadFiles()
        {
            // Reload items from the registry.
            for (int i = 0; i < _NumFiles; i++)
            {
                string file_name = (string)RegistryTools.GetSetting(
                    _ApplicationName, "FilePath" + i.ToString(), "");
                if (file_name != "")
                {
                    FileInfos.Add(new FileInfo(file_name));
                }
            }
        }

        // Save the current items in the Registry.
        private void SaveFiles()
        {
            // Delete the saved entries.
            for (int i = 0; i < _NumFiles; i++)
            {
                RegistryTools.DeleteSetting(_ApplicationName, "FilePath" + i.ToString());
            }

            // Save the current entries.
            int index = 0;
            foreach (FileInfo file_info in FileInfos)
            {
                RegistryTools.SaveSetting(_ApplicationName,
                    "FilePath" + index.ToString(), file_info.FullName);
                index++;
            }
        }

        // Remove a file's info from the list.
        private void RemoveFileInfo(string file_name)
        {
            // Remove occurrences of the file's information from the list.
            for (int i = FileInfos.Count - 1; i >= 0; i--)
            {
                if (FileInfos[i].FullName == file_name) FileInfos.RemoveAt(i);
            }
        }

        // Add a file to the list, rearranging if necessary.
        public void AddFile(string file_name)
        {
            // Remove the file from the list.
            RemoveFileInfo(file_name);

            // Add the file to the beginning of the list.
            FileInfos.Insert(0, new FileInfo(file_name));

            // If we have too many items, remove the last one.
            if (FileInfos.Count > _NumFiles) FileInfos.RemoveAt(_NumFiles);

            // Display the files.
            ShowFiles();

            // Update the Registry.
            SaveFiles();
        }

        // Remove a file from the list, rearranging if necessary.
        public void RemoveFile(string file_name)
        {
            // Remove the file from the list.
            RemoveFileInfo(file_name);

            // Display the files.
            ShowFiles();

            // Update the Registry.
            SaveFiles();
        }

        // Display the files in the menu items.
        private void ShowFiles()
        {
            for (int i = 0; i < FileInfos.Count; i++)
            {
                MenuItems[i].Text = string.Format("&{0} {1}", i + 1, FileInfos[i].Name);
                MenuItems[i].Visible = true;
                MenuItems[i].Tag = FileInfos[i];
                MenuItems[i].Click -= File_Click;
                MenuItems[i].Click += File_Click;
                MenuItems[i].ToolTipText = FileInfos[i].FullName;
            }
            for (int i = FileInfos.Count; i < _NumFiles; i++)
            {
                MenuItems[i].Visible = false;
                MenuItems[i].Click -= File_Click;
            }
        }

        // The user selected a file from the menu.
        private void File_Click(object sender, EventArgs e)
        {
            // Don't bother if no one wants to catch the event.
            if (FileSelected != null)
            {
                // Get the corresponding FileInfo object.
                ToolStripMenuItem menu_item = sender as ToolStripMenuItem;
                FileInfo file_info = menu_item.Tag as FileInfo;

                // Raise the event.
                FileSelected(file_info.FullName);
            }
        }

        // The user wants to be able to manage the list MRU files
        // How this is implemented is left to the user.
        private void ManageMRU_Click(object sender, EventArgs e)
        {
            //Raise the event
            ManageMRU(sender, e);
        }
    }
}
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
    public partial class Grid25SaveForm : Form
    {
        private bool _saveAsShapefile;
        private static Grid25SaveForm _instance;
        private Grid25GenerateForm _parentForm;

        public void SaveType(bool SaveAsShapefile)
        {
            _saveAsShapefile = SaveAsShapefile;
            Text = "Save fishing grid as shapefile";
            lblSave.Text = "File name";
            if (!_saveAsShapefile)
            {
                lblSave.Text = "Resolution (DPI)";
                Text = "Save fishing grid as image";
            }
        }

        public static Grid25SaveForm GetInstance(Grid25GenerateForm Parent)
        {
            if (_instance == null) _instance = new Grid25SaveForm(Parent);
            return _instance;
        }

        public Grid25SaveForm(Grid25GenerateForm Parent)
        {
            InitializeComponent();
            _parentForm = Parent;
        }

        private void OnGrid25SaveForm_Load(object sender, EventArgs e)
        {
            if (_saveAsShapefile)
            {
                txtSave.Text = "file name";
                txtSave.SelectAll();
                txtSave.SelectionStart = 0;
            }
            else
            {
                txtSave.Text = "150";
            }
        }

        private void OnGrid25SaveForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
        }

        private void OnTxtSave_Validating(object sender, CancelEventArgs e)
        {
            if (_saveAsShapefile)
            {
                //input must be an integer
            }
            else
            {
                //input must be a string of reasonable length
            }
        }

        private void OnButton_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOK":
                    if (txtSave.Text.Length > 0)
                    {
                        if (_saveAsShapefile)
                        {
                            //_parentForm.Grid25MajorGrid.Save(txtSave.Text);
                            FolderBrowserDialog fbd = new FolderBrowserDialog();
                            fbd.Description = "Select folder to save fishing ground grid map";
                            fbd.SelectedPath = _parentForm.GetSavedGridsFolder();
                            DialogResult result = FolderBrowserLauncher.ShowFolderBrowser(fbd);
                            if (result == DialogResult.OK && fbd.SelectedPath.Length > 0)
                            {
                                if (!_parentForm.Grid25MajorGrid.Save($@"{fbd.SelectedPath}\{txtSave.Text}"))
                                {
                                    Logger.Log("Not all grid25 shapefiles were saved.", "Grid25SaveForm", "OnButton_Click");
                                }
                            }
                        }
                        else
                        {
                            SaveFileDialog sfd = new SaveFileDialog();
                            sfd.Title = "Provide file name of image file";
                            sfd.Filter = "jpeg|*.jpg|tiff|*.tif";
                            sfd.FilterIndex = 2;
                            sfd.AddExtension = true;
                            sfd.InitialDirectory = _parentForm.GetSavedGridsFolder();
                            DialogResult result = sfd.ShowDialog();
                            if (result == DialogResult.OK && sfd.FileName.Length > 0)
                            {
                                if (_parentForm.Grid25MajorGrid.Save(int.Parse(txtSave.Text), sfd.FileName))
                                {
                                    Close();
                                }
                                else
                                {
                                    Logger.Log("Was not able to save map to image.", "Grid25SaveForm", "OnButton_Click SaveMapToImage");
                                }
                            }
                        }
                    }
                    break;

                case "btnCancel":
                    Close();
                    break;
            }
        }
    }
}
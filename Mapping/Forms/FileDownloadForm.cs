using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace FAD3.Mapping.Forms
{
    public partial class FileDownloadForm : Form
    {
        private string _url;
        private string _fileName;

        public FileDownloadForm(string url, string fileName)
        {
            InitializeComponent();
            _url = url;
            _fileName = fileName;
            lblDownloadFile.Text = _fileName;
        }

        private void DownloadFile()
        {
            using (WebClient wc = new WebClient())
            {
                wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                try
                {
                    wc.DownloadFileAsync(

                        // Param1 = Link of file
                        new Uri(_url),

                        // Param2 = Path to save
                        _fileName
                    );
                }
                catch (WebException wex)
                {
                    MessageBox.Show(wex.Message, "Web error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (InvalidOperationException ioex)
                {
                    MessageBox.Show(ioex.Message, "Web error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, "FileDownloadForm.cs", "DownloadERDDAPData");
                }
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            DownloadFile();
            Text = $"Downloading {_fileName}";
        }

        // Event to track the progress
        private void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            if (progressBar.Value == 100)
            {
                lblTitle.Text = "Finished download!";
            }
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
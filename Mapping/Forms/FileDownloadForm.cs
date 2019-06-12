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
using FAD3.Database.Classes;

namespace FAD3.Mapping.Forms
{
    public partial class FileDownloadForm : Form
    {
        private string _url;
        private string _fileName;
        private SamplingToFromXML _samplingToFromXML;
        private TargetArea _targetArea;
        private int _exportedCount = 0;
        private int _totalCount = 0;
        private string _formText;
        private bool _inImport;
        private bool _enableOK;
        private string _action;
        private string _titleText;

        public FileDownloadForm(SamplingToFromXML esxml, TargetArea targetArea, bool importing = false)
        {
            InitializeComponent();
            _samplingToFromXML = esxml;
            _samplingToFromXML.OnExportSamplingStatus += OnExportSamplingStatus;
            _targetArea = targetArea;
            _formText = "Exporting fish catch monitoring data";
            if (importing)
            {
                _formText = "Importing fish catch monitoring data";
            }
            ControlBox = false;
            btnOk.Enabled = false;
        }

        private void OnExportSamplingStatus(object sender, SamplingEventArgs e)
        {
            bool finishedExport = false;
            string caption = "";
            switch (e.ExportStatus)
            {
                //case ExportSamplingStatus.WhereToSaveImport:
                //    _targetAreaName = e.TargetAreaName;

                //    SetupImportLocation();
                //    break;

                case ExportSamplingStatus.StartImport:
                    _action = "Importing";
                    _inImport = true;
                    _titleText = $"Importing {_samplingToFromXML.TargetAreaName}";
                    _enableOK = false;
                    btnCancel.Invoke((MethodInvoker)delegate
                    {
                        btnCancel.Hide();
                    });
                    break;

                case ExportSamplingStatus.StartExport:
                    _action = "Exporting";
                    _titleText = $"Exporting { _targetArea.TargetAreaName}";
                    btnCancel.Invoke((MethodInvoker)delegate
                    {
                        btnCancel.Hide();
                    });
                    break;

                case ExportSamplingStatus.Header:
                    caption = $"{_action} header";
                    break;

                case ExportSamplingStatus.Extents:
                    caption = $"{_action} target area extents";
                    break;

                case ExportSamplingStatus.Enumerator:
                    caption = $"{_action} enumerators";
                    break;

                case ExportSamplingStatus.FishingGears:
                    caption = $"{_action} fishing gears";
                    break;

                case ExportSamplingStatus.LandingSites:
                    caption = $"{_action} landing sites";
                    break;

                case ExportSamplingStatus.BeginSamplings:
                    _exportedCount = 0;
                    if (e.RecordCount > 0)
                    {
                        progressBar.Invoke((MethodInvoker)delegate
                        {
                            progressBar.Value = 1;
                            progressBar.Minimum = 1;
                            progressBar.Maximum = e.RecordCount;
                        });
                    }
                    break;

                case ExportSamplingStatus.Samplings:
                    _exportedCount++;
                    caption = $"{_action} sampling ({_exportedCount} of {progressBar.Maximum.ToString()}): {e.ReferenceNumber}";
                    break;

                case ExportSamplingStatus.Taxa:
                    caption = $"{_action} taxa";
                    break;

                case ExportSamplingStatus.BeginCatchNames:
                    _exportedCount = 0;
                    if (e.RecordCount > 0)
                    {
                        progressBar.Invoke((MethodInvoker)delegate
                        {
                            progressBar.Value = 1;
                            progressBar.Minimum = 1;
                            progressBar.Maximum = e.RecordCount;
                        });
                    }
                    _totalCount = e.RecordCount;
                    break;

                case ExportSamplingStatus.CatchNames:
                    _exportedCount++;
                    caption = $"{_action} catch name: {e.CatchName}";
                    break;

                case ExportSamplingStatus.EndExport:
                    _enableOK = true;
                    finishedExport = true;
                    caption = "Finished exporting";
                    break;

                case ExportSamplingStatus.EndImport:
                    _enableOK = true;
                    caption = "Finished importing";
                    _inImport = false;
                    break;
            }
            btnOk.Invoke((MethodInvoker)delegate
            {
                btnOk.Enabled = _enableOK;
            });
            lblDownloadFile.Invoke((MethodInvoker)delegate
            {
                lblDownloadFile.Text = caption;
            });
            lblTitle.Invoke((MethodInvoker)delegate
            {
                lblTitle.Text = _titleText;
            });
            progressBar.Invoke((MethodInvoker)delegate
            {
                if (finishedExport)
                {
                    progressBar.Maximum = 100;
                    progressBar.Value = 100;
                }
                else if (_exportedCount > 0)
                {
                    progressBar.Increment(1);
                }
            });
        }

        public FileDownloadForm(string url, string fileName)
        {
            InitializeComponent();
            _url = url;
            _fileName = fileName;
            _formText = $"Downloading {_fileName}";
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
                    if (wex.Status != WebExceptionStatus.ProtocolError)
                    {
                        throw;
                    }

                    var response = wex.Response as HttpWebResponse;

                    if (response.StatusCode == HttpStatusCode.GatewayTimeout ||
                        response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        lblDownloadError.Visible = true;
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.GatewayTimeout:
                                lblDownloadError.Text = "Error 504: Gateway Timeout";
                                break;

                            case HttpStatusCode.Forbidden:
                                lblDownloadError.Text = "Error 43: Forbidden";
                                break;
                        }
                        return;
                    }
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
            lblDownloadFile.Text = _fileName;
            DownloadFile();
            lblDownloadError.Visible = false;
            Text = _formText;
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
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    if (_inImport)
                    {
                    }
                    else
                    {
                        Close();
                    }
                    break;

                case "btnCancel":
                    Close();
                    break;
            }
        }
    }
}
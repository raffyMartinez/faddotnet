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
    public partial class ProgessIndicatorForm : Form
    {
        private string _url;
        private string _fileName;
        private SamplingToFromXML _samplingToFromXML;
        private TargetArea _targetArea;
        private int _processedCount = 0;
        private int _totalCount = 0;
        private bool _inImport;
        private bool _enableOK;
        private string _action;
        private string _titleText;
        private string _targetAreaName;
        private int _importedCount;
        public ExportImportDataType ExportImportDataType { get; set; }
        public ExportImportDeleteAction ExportImportDeleteAction { get; set; }

        public ProgessIndicatorForm(SamplingToFromXML esxml, TargetArea targetArea, bool importing = false)
        {
            InitializeComponent();
            _samplingToFromXML = esxml;
            _samplingToFromXML.OnExportSamplingStatus += OnExportSamplingStatus;
            _targetArea = targetArea;
            ControlBox = false;
            btnOk.Enabled = false;
        }

        private void SetupProgressBar(int max, int value = 1)
        {
            if (max > 0)
            {
                progressBar.Invoke((MethodInvoker)delegate
                {
                    progressBar.Maximum = max;
                    progressBar.Minimum = 1;
                    progressBar.Value = value;
                });
            }
        }

        private void OnExportSamplingStatus(object sender, SamplingEventArgs e)
        {
            bool finishedExport = false;
            string caption = "";
            switch (e.RecordStatus)
            {
                //case ExportSamplingStatus.WhereToSaveImport:
                //    _targetAreaName = e.TargetAreaName;

                //    SetupImportLocation();
                //    break;

                case SamplingRecordStatus.StartImport:
                    _action = "Importing";
                    _inImport = true;
                    _titleText = $"Importing {_samplingToFromXML.TargetAreaName}";
                    _enableOK = false;
                    btnCancel.Invoke((MethodInvoker)delegate
                    {
                        btnCancel.Hide();
                    });
                    break;

                case SamplingRecordStatus.StartExport:
                    _action = "Exporting";
                    _titleText = $"Exporting { _targetArea.TargetAreaName}";
                    btnCancel.Invoke((MethodInvoker)delegate
                    {
                        btnCancel.Hide();
                    });
                    break;

                case SamplingRecordStatus.Header:
                    caption = $"{_action} header";
                    break;

                case SamplingRecordStatus.Extents:
                    caption = $"{_action} target area extents";
                    break;

                case SamplingRecordStatus.Enumerator:
                    caption = $"{_action} enumerators";
                    break;

                case SamplingRecordStatus.BeginFishingGears:
                    _processedCount = 0;
                    SetupProgressBar(e.RecordCount);
                    break;

                case SamplingRecordStatus.FishingGears:
                    caption = $"{_action} fishing gear: {e.GearVariationName}";
                    _processedCount++;
                    break;

                case SamplingRecordStatus.LandingSites:
                    caption = $"{_action} landing sites";
                    break;

                case SamplingRecordStatus.BeginGearLocalNames:
                    _processedCount = 0;
                    SetupProgressBar(e.RecordCount);
                    break;

                case SamplingRecordStatus.BeginSamplings:
                    _processedCount = 0;
                    SetupProgressBar(e.RecordCount);
                    //if (e.RecordCount > 0)
                    //{
                    //    progressBar.Invoke((MethodInvoker)delegate
                    //    {
                    //        progressBar.Value = 1;
                    //        progressBar.Minimum = 1;
                    //        progressBar.Maximum = e.RecordCount;
                    //    });
                    //}
                    break;

                case SamplingRecordStatus.Samplings:
                    _processedCount++;
                    caption = $"{_action} sampling ({_processedCount} of {progressBar.Maximum.ToString()}): {e.ReferenceNumber}";
                    break;

                case SamplingRecordStatus.Taxa:
                    caption = $"{_action} taxa";
                    break;

                case SamplingRecordStatus.BeginCatchNames:
                    _processedCount = 0;
                    SetupProgressBar(e.RecordCount);
                    //if (e.RecordCount > 0)
                    //{
                    //    progressBar.Invoke((MethodInvoker)delegate
                    //    {
                    //        progressBar.Value = 1;
                    //        progressBar.Minimum = 1;
                    //        progressBar.Maximum = e.RecordCount;
                    //    });
                    //}
                    _totalCount = e.RecordCount;
                    break;

                case SamplingRecordStatus.CatchNames:
                    _processedCount++;
                    caption = $"{_action} catch name: {e.CatchName}";
                    break;

                case SamplingRecordStatus.EndExport:
                    _enableOK = true;
                    finishedExport = true;
                    caption = "Finished exporting";
                    break;

                case SamplingRecordStatus.EndImport:
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
                else if (_processedCount > 0)
                {
                    progressBar.Increment(1);
                }
            });
        }

        public ProgessIndicatorForm(string targetAreaName)
        {
            InitializeComponent();
            btnCancel.Hide();
            _targetAreaName = targetAreaName;
            Samplings.OnDeleteSamplingStatus += OnDeleteSampling;
        }

        private void OnDeleteSampling(object sender, SamplingEventArgs e)
        {
            switch (e.RecordStatus)
            {
                case SamplingRecordStatus.BeginDeleteSampling:
                    lblTitle.Invoke((MethodInvoker)delegate
                    {
                        lblTitle.Text = $"Deleting samplings from {_targetAreaName}";
                    });
                    SetupProgressBar(e.RecordCount);
                    break;

                case SamplingRecordStatus.DeleteSampling:
                    lblDownloadFile.Invoke((MethodInvoker)delegate
                    {
                        lblDownloadFile.Text = $"Deleted {e.ReferenceNumber}";
                    });
                    progressBar.Invoke((MethodInvoker)delegate
                    {
                        progressBar.Increment(1);
                    });
                    break;

                case SamplingRecordStatus.EndDeleteSampling:
                    btnOk.Invoke((MethodInvoker)delegate
                    {
                        btnOk.Enabled = true;
                    });
                    progressBar.Invoke((MethodInvoker)delegate
                    {
                        progressBar.Maximum = 100;
                        progressBar.Value = 100;
                    });
                    lblDownloadFile.Invoke((MethodInvoker)delegate
                    {
                        lblDownloadFile.Text = "Finished deleting samplings";
                    });
                    break;
            }
        }

        public ProgessIndicatorForm(string url, string fileName)
        {
            InitializeComponent();
            _url = url;
            _fileName = fileName;
            if (url.Length == 0)
            {
                Names.OnRowsImported += OnRowsImported;
            }
            btnCancel.Hide();
            ControlBox = false;
            btnOk.Enabled = false;
        }

        private void OnRowsImported(object sender, ImportRowsFromFileEventArgs e)
        {
            if (e.ImportedName?.Length > 0)
            {
                progressBar.Invoke((MethodInvoker)delegate
                {
                    progressBar.Increment(1);
                    _importedCount++;
                });
                lblDownloadFile.Invoke((MethodInvoker)delegate
                {
                    lblDownloadFile.Text = $"Imported {e.ImportedName}";
                });
            }
            else if (e.IsComplete)
            {
                btnOk.Invoke((MethodInvoker)delegate
                {
                    btnOk.Enabled = true;
                });
                lblDownloadFile.Invoke((MethodInvoker)delegate
                {
                    lblDownloadFile.Text = $"Finished importing {_importedCount} names";
                });
                progressBar.Invoke((MethodInvoker)delegate
                {
                    progressBar.Maximum = 100;
                    progressBar.Value = 100;
                });
            }
            else if (e.RowsImported >= 0)
            {
                SetupProgressBar(_importedCount + 50, _importedCount + 1);

                if (e.RowsImported == 0)
                {
                    lblTitle.Invoke((MethodInvoker)delegate
                    {
                        lblTitle.Text = $"Importing data in {global.EllipsisString(_fileName)}";
                    });
                }
            }
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
            string dataDescription = "";
            lblDownloadFile.Text = _fileName;
            if (_url.Length > 0)
            {
                DownloadFile();
            }
            lblDownloadError.Visible = false;
            switch (ExportImportDataType)
            {
                case ExportImportDataType.TargetAreaData:
                    dataDescription = "catch composition data";
                    break;

                case ExportImportDataType.SpeciesNames:
                    dataDescription = "species names";
                    break;

                case ExportImportDataType.ERDDAP:
                    dataDescription = "ERDDAP data";
                    break;
            }
            switch (ExportImportDeleteAction)
            {
                case ExportImportDeleteAction.ActionExport:
                    dataDescription = $"Exporting  {dataDescription}";
                    break;

                case ExportImportDeleteAction.ActionImport:
                    dataDescription = $"Importing  {dataDescription}";
                    break;

                case ExportImportDeleteAction.ActionDelete:
                    dataDescription = $"Deleting  {dataDescription}";
                    break;
            }
            Text = dataDescription;
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
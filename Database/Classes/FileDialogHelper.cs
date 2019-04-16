using System.Windows.Forms;

namespace FAD3.Database.Classes
{
    public static class FileDialogHelper
    {
        public static FileDialogType DialogType { get; set; }
        public static string Title { get; set; }
        public static string FileName { get; internal set; }
        public static string Filter { get; internal set; }
        public static int FilterIndex { get; set; }
        public static DataFileType DataFileType { get; set; }
        private static FileDialog _dlg;

        public static void ShowDialog()
        {
            if (DialogType == FileDialogType.FileOpen)
            {
                _dlg = new OpenFileDialog();
                _dlg.Title = "Open a file";
            }
            else
            {
                _dlg = new SaveFileDialog();
                _dlg.Title = "Save a file";
            }
            _dlg.Filter = "Text file|*.txt|All files|*.*";
            _dlg.FilterIndex = 1;

            if (DataFileType != DataFileType.None)
            {
                Filter = "";

                if ((DataFileType & DataFileType.Text) == DataFileType.Text)
                {
                    Filter += "Text file|*.txt";
                }
                if ((DataFileType & DataFileType.XML) == DataFileType.XML)
                {
                    Filter = Filter.Length > 0 ? Filter += "|XML file|*.xml" : "XML file|*.xml";
                }
                if ((DataFileType & DataFileType.HTML) == DataFileType.HTML)
                {
                    Filter = Filter.Length > 0 ? Filter += "|HTML file|*.html;*.htm" : "HTML file|*.html;*.htm";
                }
                if ((DataFileType & DataFileType.Excel) == DataFileType.Excel)
                {
                    Filter = Filter.Length > 0 ? Filter += "|Excel file|*.xlsx" : "Excel file|*.xlsx";
                }
                if ((DataFileType & DataFileType.CSV) == DataFileType.CSV)
                {
                    Filter = Filter.Length > 0 ? Filter += "|Comma separated value file|*.csv" : "Comma separated value file|*.csv";
                }
                _dlg.Filter = Filter += "|All files|*.*";
            }
            else
            {
                _dlg.Filter = "Text file|*.txt|All files|*.*";
            }

            if (Title?.Length > 0)
            {
                _dlg.Title = Title;
            }
            _dlg.FileName = FileName;
            FilterIndex = _dlg.FilterIndex;
            _dlg.ShowDialog();
            FileName = _dlg.FileName;
        }
    }
}
using System;

namespace FAD3.Database.Classes
{
    public class ImportRowsFromFileEventArgs : EventArgs
    {
        public int RowsImported { get; set; }
        public ExportImportDataType DataType { get; set; }
        public bool IsComplete { get; internal set; }
        public string ImportedExportedName { get; set; }
        public string LocalNameSciNameLanguage { get; set; }

        public ImportRowsFromFileEventArgs(int rowsImported, ExportImportDataType dataType, bool isComplete)
        {
            RowsImported = rowsImported;
            DataType = dataType;
            IsComplete = isComplete;
        }

        public ImportRowsFromFileEventArgs(string spName, string localName, string language)
        {
            LocalNameSciNameLanguage = $"{localName}, {language}, {spName}";
        }

        public ImportRowsFromFileEventArgs(string name1, string name2)
        {
            ImportedExportedName = $"{name1} {name2}";
        }

        public ImportRowsFromFileEventArgs(int rowsImported)
        {
            RowsImported = rowsImported;
        }

        public ImportRowsFromFileEventArgs(bool isComplete)
        {
            IsComplete = isComplete;
        }
    }
}
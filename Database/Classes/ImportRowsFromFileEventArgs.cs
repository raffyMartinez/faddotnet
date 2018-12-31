using System;

namespace FAD3.Database.Classes
{
    public class ImportRowsFromFileEventArgs : EventArgs
    {
        public int RowsImported { get; internal set; }
        public ExportImportDataType DataType { get; internal set; }
        public bool IsComplete { get; internal set; }

        public ImportRowsFromFileEventArgs(int rowsImported, ExportImportDataType dataType, bool isComplete)
        {
            RowsImported = rowsImported;
            DataType = dataType;
            IsComplete = IsComplete;
        }
    }
}
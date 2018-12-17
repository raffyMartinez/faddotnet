using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FAD3.Database.Classes;

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
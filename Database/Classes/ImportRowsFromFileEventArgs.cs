﻿using System;

namespace FAD3.Database.Classes
{
    public class ImportRowsFromFileEventArgs : EventArgs
    {
        public int RowsImported { get; set; }
        public ExportImportDataType DataType { get; internal set; }
        public bool IsComplete { get; internal set; }
        public string ImportedName { get; set; }

        public ImportRowsFromFileEventArgs(int rowsImported, ExportImportDataType dataType, bool isComplete)
        {
            RowsImported = rowsImported;
            DataType = dataType;
            IsComplete = IsComplete;
        }

        public ImportRowsFromFileEventArgs(string name1, string name2)
        {
            ImportedName = $"{name1} {name2}";
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
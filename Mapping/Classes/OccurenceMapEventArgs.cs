using FAD3.Database.Classes;
using System;
using System.Collections.Generic;

namespace FAD3.Mapping.Classes
{
    public class OccurenceMapEventArgs : EventArgs
    {
        public OccurenceDataType OccurenceDataType { get; internal set; }
        public bool Cancel { get; set; }
        public bool MapInSelectedTargetArea { get; set; }
        public bool Aggregate { get; set; }
        public bool ExcludeOne { get; set; }
        public string SelectedTargetAreaGuid { get; set; }
        public List<int> SamplingYears { get; set; }
        public string ItemToMapGuid { get; set; }

        public OccurenceMapEventArgs(OccurenceDataType occurenceType)
        {
            OccurenceDataType = occurenceType;
        }
    }
}
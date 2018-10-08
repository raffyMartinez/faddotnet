using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FAD3
{
    public class CreateInlandGridEventArgs : EventArgs
    {
        public string Status { get; set; }
        public string StatusDescription { get; set; }
        public int GridCount { get; set; }
    }
}
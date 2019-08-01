using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAD3.Database.Classes
{
    public class TreeLevelTag
    {
        public TreeLevel TreeLevel { get; internal set; }
        public string TreeLevelName { get; internal set; }
        public bool IsRoot { get; internal set; }

        public TreeLevelTag(bool isRoot, TreeLevel treeLevel = null, string name = "")
        {
            if (isRoot)
            {
                TreeLevelName = "root";
                IsRoot = true;
            }
            else
            {
                TreeLevel = treeLevel;
                TreeLevelName = name;
                IsRoot = false;
            }
        }
    }
}
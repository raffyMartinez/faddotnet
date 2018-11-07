using System;

namespace FAD3.Mapping.Classes
{
    public class ExtentDraggedBoxEventArgs : EventArgs
    {
        public double Top { get; internal set; }
        public double Bottom { get; internal set; }
        public double Left { get; internal set; }
        public double Right { get; internal set; }
        public bool InDrag { get; internal set; }

        public ExtentDraggedBoxEventArgs(double top, double bottom, double left, double right, bool inDrag)
        {
            Top = top;
            Bottom = bottom;
            Left = left;
            Right = right;
            InDrag = inDrag;
        }
    }
}
using System;

namespace TuesPechkin
{
    public class FinishEventArgs : EventArgs
    {
        public IDocument Document { get; set; }

        public bool Success { get; set; }
    }
}
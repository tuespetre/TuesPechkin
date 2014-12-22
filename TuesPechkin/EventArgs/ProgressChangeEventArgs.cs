using System;

namespace TuesPechkin
{
    public class ProgressChangeEventArgs : EventArgs
    {
        public IDocument Document { get; set; }

        public int Progress { get; set; }

        public string ProgressDescription { get; set; }
    }
}
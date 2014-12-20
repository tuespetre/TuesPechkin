using System;

namespace TuesPechkin
{
    public class BeginEventArgs : EventArgs
    {
        public IDocument Document { get; set; }

        public int ExpectedPhaseCount { get; set; }
    }
}
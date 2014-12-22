using System;

namespace TuesPechkin
{
    public class ErrorEventArgs : EventArgs
    {
        public IDocument Document { get; set; }

        public string ErrorMessage { get; set; }
    }
}
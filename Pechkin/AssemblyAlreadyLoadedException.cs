using System;
using System.Collections.Generic;
using System.Text;

namespace TuesPechkin
{
    [Serializable]
    public class AssemblyAlreadyLoadedException : Exception
    {
        public AssemblyAlreadyLoadedException() { }
        public AssemblyAlreadyLoadedException(string message) : base(message) { }
        public AssemblyAlreadyLoadedException(string message, Exception inner) : base(message, inner) { }
        protected AssemblyAlreadyLoadedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}

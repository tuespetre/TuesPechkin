using System;
using System.Collections.Generic;
using System.Text;

namespace TuesPechkin
{
    [Serializable]
    public sealed class StaticDeployment : IDeployment
    {
        public string Path { get; private set; }

        public StaticDeployment(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            Path = path;
        }
    }
}

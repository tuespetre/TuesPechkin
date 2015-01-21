using System;
using System.Collections.Generic;
using System.Text;

namespace TuesPechkin
{
    [Serializable]
    public sealed class TempFolderDeployment : IDeployment
    {
        public string Path { get; private set; }

        public TempFolderDeployment()
        {
            // Scope it to the application
            Path = System.IO.Path.Combine(
                System.IO.Path.GetTempPath(),
                AppDomain.CurrentDomain.BaseDirectory.GetHashCode().ToString());

            // Scope it by bitness, too
            Path = System.IO.Path.Combine(
                Path,
                IntPtr.Size.ToString());
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using TuesPechkin.Wkhtmltox.Properties;
using SysPath = System.IO.Path;

namespace TuesPechkin.Wkhtmltox
{
    [Serializable]
    public class WinEmbeddedDeployment : MemoryDeployment
    {
        public WinEmbeddedDeployment(IDeployment physical) : base(physical) { }

        protected override Stream GetStream()
        {
            var raw = (IntPtr.Size == 8) ? Resources.wkhtmltox_64_dll : Resources.wkhtmltox_32_dll;

            return new GZipStream(new MemoryStream(raw), CompressionMode.Decompress);
        }
    }
}

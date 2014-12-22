using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using TuesPechkin.Properties;
using SysPath = System.IO.Path;

namespace TuesPechkin
{
    [Serializable]
    public class Win32EmbeddedDeployment : EmbeddedDeployment
    {
        public Win32EmbeddedDeployment(IDeployment physical) : base(physical) { }

        protected override IEnumerable<KeyValuePair<string, Stream>> GetContents()
        {
            var raw = Resources.wkhtmltox_32_dll;

            return new []
            { 
                new KeyValuePair<string, Stream>(
                    key: WkhtmltoxBindings.DLLNAME,
                    value: new GZipStream(new MemoryStream(raw), CompressionMode.Decompress))
            };
        }
    }
}

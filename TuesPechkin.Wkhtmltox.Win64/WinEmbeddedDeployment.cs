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
    public class Win64EmbeddedDeployment : EmbeddedDeployment
    {
        public Win64EmbeddedDeployment(IDeployment physical) : base(physical) { }

        protected override IEnumerable<KeyValuePair<string, Stream>> GetContents()
        {
            var raw = Resources.wkhtmltox_64_dll;

            return new []
            { 
                new KeyValuePair<string, Stream>(
                    key: WkhtmltoxBindings.DLLNAME,
                    value: new GZipStream(new MemoryStream(raw), CompressionMode.Decompress))
            };
        }
    }
}

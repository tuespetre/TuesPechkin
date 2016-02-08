using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.CompilerServices;
using TuesPechkin.Wkhtmltox.AnyCPU.Properties;

namespace TuesPechkin.Wkhtmltox.AnyCPU
{
    [Serializable]
    public class WinAnyCPUEmbeddedDeployment : EmbeddedDeployment
    {
        public WinAnyCPUEmbeddedDeployment(IDeployment physical)
            : base(physical)
        {
        }

        public override string Path
        {
            get
            {
                return System.IO.Path.Combine(
                    base.Path,
                    GetType().Assembly.GetName().Version.ToString());
            }
        }

        protected override IEnumerable<KeyValuePair<string, Stream>> GetContents()
        {
            if (Environment.Is64BitProcess)
                return Loadx64();
            return Loadx86();
            
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private IEnumerable<KeyValuePair<string, Stream>> Loadx86()
        {
            return new[]
            {
                    new KeyValuePair<string, Stream>(
                        key: WkhtmltoxBindings.DLLNAME,
                        value: new GZipStream(
                            new MemoryStream(Resources.wkhtmltox_32_dll),
                            CompressionMode.Decompress))
            };
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private IEnumerable<KeyValuePair<string, Stream>> Loadx64()
        {
            return new[]
            {
                    new KeyValuePair<string, Stream>(
                        key: WkhtmltoxBindings.DLLNAME,
                        value: new GZipStream(
                            new MemoryStream(Resources.wkhtmltox_64_dll),
                            CompressionMode.Decompress))
            };
        }
    }
}

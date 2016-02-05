using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.CompilerServices;

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
            if (IntPtr.Size == 4)
                return Loadx86();
            return Loadx64();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private IEnumerable<KeyValuePair<string, Stream>> Loadx86()
        {
            var assembly = Assembly.GetAssembly(typeof(Win32EmbeddedDeployment));
            var stream = assembly.GetManifestResourceStream("TuesPechkin.wkhtmltox_32.dll.gz");
            return new[]
            {
                    new KeyValuePair<string, Stream>(
                        key: WkhtmltoxBindings.DLLNAME,
                        value: new GZipStream(
                            stream,
                            CompressionMode.Decompress))
            };
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private IEnumerable<KeyValuePair<string, Stream>> Loadx64()
        {
            var assembly = Assembly.GetAssembly(typeof(Win64EmbeddedDeployment));
            var stream = assembly.GetManifestResourceStream("TuesPechkin.wkhtmltox_64.dll.gz");
            return new[]
            {
                    new KeyValuePair<string, Stream>(
                        key: WkhtmltoxBindings.DLLNAME,
                        value: new GZipStream(
                            stream,
                            CompressionMode.Decompress))
            };
        }
    }
}

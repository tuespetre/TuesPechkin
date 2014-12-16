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
    public class EmbeddedAssembly : NestingAssembly, IAssembly
    {
        /// <summary>
        /// Loads the assembly from its path. Throws AssemblyAlreadyLoadedException
        /// if the assembly was already loaded.
        /// </summary>
        /// <param name="pathOverride">This parameter is ignored.</param>
        public override void Load(string pathOverride = null)
        {
            if (Loaded)
            {
                throw new AssemblyAlreadyLoadedException();
            }

            var raw = (IntPtr.Size == 8) ? Resources.wkhtmltox_64_dll : Resources.wkhtmltox_32_dll;

            var path = SetupUnmanagedAssembly("wkhtmltox.dll", raw);

            WrappedAssembly = new StandardAssembly(path);

            WrappedAssembly.Load(path);

            Path = path;

            Loaded = true;
        }

        private static string SetupUnmanagedAssembly(string fileName, byte[] assemblyRaw)
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetName();
            var basePath = SysPath.Combine(
                SysPath.GetTempPath(),
                String.Format(
                    "{0}{1}_{2}_{3}",
                    assemblyName.Name.ToString(),
                    assemblyName.Version.ToString(),
                    IntPtr.Size == 8 ? "x64" : "x86",
                    String.Join(
                        String.Empty,
                        AppDomain.CurrentDomain.BaseDirectory.Split(
                            SysPath.GetInvalidFileNameChars()))));

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            fileName = SysPath.Combine(basePath, fileName);

            WriteStreamToFile(fileName, new GZipStream(new MemoryStream(assemblyRaw), CompressionMode.Decompress));

            return fileName;
        }

        private static void WriteStreamToFile(string fileName, Stream stream)
        {
            if (!File.Exists(fileName))
            {
                var writeBuffer = new byte[8192];
                var writeLength = 0;

                using (var newFile = File.Open(fileName, FileMode.Create))
                {
                    while ((writeLength = stream.Read(writeBuffer, 0, writeBuffer.Length)) > 0)
                    {
                        newFile.Write(writeBuffer, 0, writeLength);
                    }
                }
            }
        }
    }
}

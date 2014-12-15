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
    public class EmbeddedAssembly : IAssembly
    {
        public IAssembly InnerAssembly { get; private set; }

        public string Path
        {
            get
            {
                return InnerAssembly.Path;
            }
        }

        public Version Version
        {
            get 
            { 
                return InnerAssembly.Version; 
            }
        }

        public EmbeddedAssembly()
        {
            var raw = (IntPtr.Size == 8) ? Resources.wkhtmltox_64_dll : Resources.wkhtmltox_32_dll;

            var path = SetupUnmanagedAssembly("wkhtmltox.dll", raw);

            InnerAssembly = new StandardAssembly(path, new Version(0, 9, 0));
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

            WinApiHelper.LoadLibrary(fileName);

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

        public void AddObject(IntPtr converter, IntPtr objectConfig, byte[] html)
        {
            InnerAssembly.AddObject(converter, objectConfig, html);
        }

        public void AddObject(IntPtr converter, IntPtr objectConfig, string html)
        {
            InnerAssembly.AddObject(converter, objectConfig, html);
        }

        public IntPtr CreateConverter(IntPtr globalSettings)
        {
            return InnerAssembly.CreateConverter(globalSettings);
        }

        public IntPtr CreateGlobalSettings()
        {
            return InnerAssembly.CreateGlobalSettings();
        }

        public IntPtr CreateObjectSettings()
        {
            return InnerAssembly.CreateObjectSettings();
        }

        public void DestroyConverter(IntPtr converter)
        {
            InnerAssembly.DestroyConverter(converter);
        }

        public byte[] GetConverterResult(IntPtr converter)
        {
            return InnerAssembly.GetConverterResult(converter);
        }

        public string GetGlobalSetting(IntPtr setting, string name)
        {
            return InnerAssembly.GetGlobalSetting(setting, name);
        }

        public int GetHttpErrorCode(IntPtr converter)
        {
            return InnerAssembly.GetHttpErrorCode(converter);
        }

        public string GetObjectSetting(IntPtr setting, string name)
        {
            return InnerAssembly.GetObjectSetting(setting, name);
        }

        public int GetPhaseCount(IntPtr converter)
        {
            return InnerAssembly.GetPhaseCount(converter);
        }

        public string GetPhaseDescription(IntPtr converter, int phase)
        {
            return InnerAssembly.GetPhaseDescription(converter, phase);
        }

        public int GetPhaseNumber(IntPtr converter)
        {
            return InnerAssembly.GetPhaseNumber(converter);
        }

        public string GetProgressDescription(IntPtr converter)
        {
            return InnerAssembly.GetProgressDescription(converter);
        }

        public bool PerformConversion(IntPtr converter)
        {
            return InnerAssembly.PerformConversion(converter);
        }

        public void SetErrorCallback(IntPtr converter, Util.StringCallback callback)
        {
            InnerAssembly.SetErrorCallback(converter, callback);
        }

        public void SetFinishedCallback(IntPtr converter, Util.IntCallback callback)
        {
            InnerAssembly.SetFinishedCallback(converter, callback);
        }

        public int SetGlobalSetting(IntPtr setting, string name, string value)
        {
            return InnerAssembly.SetGlobalSetting(setting, name, value);
        }

        public int SetObjectSetting(IntPtr setting, string name, string value)
        {
            return InnerAssembly.SetObjectSetting(setting, name, value);
        }

        public void SetPhaseChangedCallback(IntPtr converter, Util.VoidCallback callback)
        {
            InnerAssembly.SetPhaseChangedCallback(converter, callback);
        }

        public void SetProgressChangedCallback(IntPtr converter, Util.IntCallback callback)
        {
            InnerAssembly.SetProgressChangedCallback(converter, callback);
        }

        public void SetWarningCallback(IntPtr converter, Util.StringCallback callback)
        {
            InnerAssembly.SetWarningCallback(converter, callback);
        }
    }
}

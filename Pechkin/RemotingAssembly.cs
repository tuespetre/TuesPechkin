using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using TuesPechkin.EventHandlers;
using ErrorEventHandler = TuesPechkin.EventHandlers.ErrorEventHandler;
using SysAssembly = System.Reflection.Assembly;
using SysPath = System.IO.Path;

namespace TuesPechkin
{
    public class RemotingAssembly : MarshalByRefObject, IAssembly
    {
        public string Path { get; private set; }

        public Version Version { get; private set; }

        public RemotingAssembly(string path, Version version)
        {
            Path = path;
            Version = version;

            SetupAppDomain();

            var handle = Activator.CreateInstanceFrom(
                RemoteDomain,
                typeof(StandardAssembly).Assembly.Location,
                typeof(StandardAssembly).FullName,
                false,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, 
                null, 
                new object[] { Path, Version },
                null,
                null,
                null);

            RemoteAssembly = handle.Unwrap() as IAssembly;
        }

        public void Unload()
        {
            TearDownAppDomain(null, EventArgs.Empty);
        }

        public void AddObject(IntPtr converter, IntPtr objectConfig, byte[] html)
        {
            RemoteAssembly.AddObject(converter, objectConfig, html);
        }

        public void AddObject(IntPtr converter, IntPtr objectConfig, string html)
        {
            RemoteAssembly.AddObject(converter, objectConfig, html);
        }

        public IntPtr CreateConverter(IntPtr globalSettings)
        {
            return RemoteAssembly.CreateConverter(globalSettings);
        }

        public IntPtr CreateGlobalSettings()
        {
            return RemoteAssembly.CreateGlobalSettings();
        }

        public IntPtr CreateObjectSettings()
        {
            return RemoteAssembly.CreateObjectSettings();
        }

        public void DestroyConverter(IntPtr converter)
        {
            RemoteAssembly.DestroyConverter(converter);
        }

        public byte[] GetConverterResult(IntPtr converter)
        {
            return RemoteAssembly.GetConverterResult(converter);
        }

        public string GetGlobalSetting(IntPtr setting, string name)
        {
            return RemoteAssembly.GetGlobalSetting(setting, name);
        }

        public int GetHttpErrorCode(IntPtr converter)
        {
            return RemoteAssembly.GetHttpErrorCode(converter);
        }

        public string GetObjectSetting(IntPtr setting, string name)
        {
            return RemoteAssembly.GetObjectSetting(setting, name);
        }

        public int GetPhaseCount(IntPtr converter)
        {
            return RemoteAssembly.GetPhaseCount(converter);
        }

        public string GetPhaseDescription(IntPtr converter, int phase)
        {
            return RemoteAssembly.GetPhaseDescription(converter, phase);
        }

        public int GetPhaseNumber(IntPtr converter)
        {
            return RemoteAssembly.GetPhaseNumber(converter);
        }

        public string GetProgressDescription(IntPtr converter)
        {
            return RemoteAssembly.GetProgressDescription(converter);
        }

        public bool PerformConversion(IntPtr converter)
        {
            return RemoteAssembly.PerformConversion(converter);
        }

        public void SetErrorCallback(IntPtr converter, Util.StringCallback callback)
        {
            RemoteAssembly.SetErrorCallback(converter, callback);
        }

        public void SetFinishedCallback(IntPtr converter, Util.IntCallback callback)
        {
            RemoteAssembly.SetFinishedCallback(converter, callback);
        }

        public int SetGlobalSetting(IntPtr setting, string name, string value)
        {
            return RemoteAssembly.SetGlobalSetting(setting, name, value);
        }

        public int SetObjectSetting(IntPtr setting, string name, string value)
        {
            return RemoteAssembly.SetObjectSetting(setting, name, value);
        }

        public void SetPhaseChangedCallback(IntPtr converter, Util.VoidCallback callback)
        {
            RemoteAssembly.SetPhaseChangedCallback(converter, callback);
        }

        public void SetProgressChangedCallback(IntPtr converter, Util.IntCallback callback)
        {
            RemoteAssembly.SetProgressChangedCallback(converter, callback);
        }

        public void SetWarningCallback(IntPtr converter, Util.StringCallback callback)
        {
            RemoteAssembly.SetWarningCallback(converter, callback);
        }

        private IAssembly RemoteAssembly { get; set; }

        private AppDomain RemoteDomain { get; set; }

        private void SetupAppDomain()
        {
            var assemblyLocation = SysAssembly.GetExecutingAssembly().Location;
            var dirName = SysPath.GetDirectoryName(assemblyLocation);

            var setup = new AppDomainSetup
            {
                ApplicationBase = dirName,
                LoaderOptimization = LoaderOptimization.SingleDomain
            };

            RemoteDomain = AppDomain.CreateDomain(
                string.Format("tuespechkin_{0}", Guid.NewGuid()),
                null,
                setup);

            RemoteDomain.SetData("path", Path);

            RemoteDomain.DoCallBack(() =>
            {
                var path = AppDomain.CurrentDomain.GetData("path") as string;

                SysAssembly.LoadFile(path);
                WkhtmltoxBindings.wkhtmltopdf_init(0);
            });

            if (AppDomain.CurrentDomain.IsDefaultAppDomain() == false)
            {
                AppDomain.CurrentDomain.DomainUnload += TearDownAppDomain;
            }
        }

        private void TearDownAppDomain(object sender, EventArgs e)
        {
            AppDomain.Unload(RemoteDomain);

            foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
            {
                if (module.FileName == Path)
                {
                    while (WinApiHelper.FreeLibrary(module.BaseAddress))
                    {
                    }
                }
            }
        }
    }
}
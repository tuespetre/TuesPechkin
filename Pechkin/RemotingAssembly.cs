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
    public class RemotingAssembly<TAssembly> : IAssembly
        where TAssembly : MarshalByRefObject, IAssembly, new()
    {
        public bool Loaded { get; private set; }

        public string Path { get; private set; }

        public RemotingAssembly(TAssembly prototype)
        {
            if (prototype == null)
            {
                throw new ArgumentNullException("prototype");
            }

            Prototype = prototype;
        }

        public void Load(string pathOverride = null)
        {
            if (Loaded)
            {
                throw new AssemblyAlreadyLoadedException();
            }

            if (pathOverride != null)
            {
                Path = pathOverride;
            }

            SetupAppDomain();

            var handle = Activator.CreateInstanceFrom(
                RemoteDomain,
                typeof(TAssembly).Assembly.Location,
                typeof(TAssembly).FullName,
                false,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                null,
                null,
                null,
                null);

            RemoteAssembly = handle.Unwrap() as IAssembly;
            RemoteAssembly.Load(Path);
            Path = RemoteAssembly.Path;

            Loaded = true;
        }

        public void Unload()
        {
            TearDownAppDomain(null, EventArgs.Empty);
        }

        #region forwarding calls
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
        #endregion

        private IAssembly Prototype { get; set; }

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

            RemoteDomain.SetData("path", assemblyLocation);

            RemoteDomain.DoCallBack(() =>
            {
                var path = AppDomain.CurrentDomain.GetData("path") as string;

                SysAssembly.LoadFile(path);
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
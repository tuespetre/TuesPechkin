using System;
using System.Collections.Generic;
using System.Text;

namespace TuesPechkin
{
    public abstract class NestingAssembly : MarshalByRefObject, IAssembly
    {
        protected IAssembly WrappedAssembly { get; set; }

        public bool Loaded { get; protected set; }

        public string Path { get; protected set; }

        public abstract void Load(string pathOverride = null);

        public void AddObject(IntPtr converter, IntPtr objectConfig, byte[] html)
        {
            WrappedAssembly.AddObject(converter, objectConfig, html);
        }

        public void AddObject(IntPtr converter, IntPtr objectConfig, string html)
        {
            WrappedAssembly.AddObject(converter, objectConfig, html);
        }

        public IntPtr CreateConverter(IntPtr globalSettings)
        {
            return WrappedAssembly.CreateConverter(globalSettings);
        }

        public IntPtr CreateGlobalSettings()
        {
            return WrappedAssembly.CreateGlobalSettings();
        }

        public IntPtr CreateObjectSettings()
        {
            return WrappedAssembly.CreateObjectSettings();
        }

        public void DestroyConverter(IntPtr converter)
        {
            WrappedAssembly.DestroyConverter(converter);
        }

        public byte[] GetConverterResult(IntPtr converter)
        {
            return WrappedAssembly.GetConverterResult(converter);
        }

        public string GetGlobalSetting(IntPtr setting, string name)
        {
            return WrappedAssembly.GetGlobalSetting(setting, name);
        }

        public int GetHttpErrorCode(IntPtr converter)
        {
            return WrappedAssembly.GetHttpErrorCode(converter);
        }

        public string GetObjectSetting(IntPtr setting, string name)
        {
            return WrappedAssembly.GetObjectSetting(setting, name);
        }

        public int GetPhaseCount(IntPtr converter)
        {
            return WrappedAssembly.GetPhaseCount(converter);
        }

        public string GetPhaseDescription(IntPtr converter, int phase)
        {
            return WrappedAssembly.GetPhaseDescription(converter, phase);
        }

        public int GetPhaseNumber(IntPtr converter)
        {
            return WrappedAssembly.GetPhaseNumber(converter);
        }

        public string GetProgressDescription(IntPtr converter)
        {
            return WrappedAssembly.GetProgressDescription(converter);
        }

        public bool PerformConversion(IntPtr converter)
        {
            return WrappedAssembly.PerformConversion(converter);
        }

        public void SetErrorCallback(IntPtr converter, Util.StringCallback callback)
        {
            WrappedAssembly.SetErrorCallback(converter, callback);
        }

        public void SetFinishedCallback(IntPtr converter, Util.IntCallback callback)
        {
            WrappedAssembly.SetFinishedCallback(converter, callback);
        }

        public int SetGlobalSetting(IntPtr setting, string name, string value)
        {
            return WrappedAssembly.SetGlobalSetting(setting, name, value);
        }

        public int SetObjectSetting(IntPtr setting, string name, string value)
        {
            return WrappedAssembly.SetObjectSetting(setting, name, value);
        }

        public void SetPhaseChangedCallback(IntPtr converter, Util.VoidCallback callback)
        {
            WrappedAssembly.SetPhaseChangedCallback(converter, callback);
        }

        public void SetProgressChangedCallback(IntPtr converter, Util.IntCallback callback)
        {
            WrappedAssembly.SetProgressChangedCallback(converter, callback);
        }

        public void SetWarningCallback(IntPtr converter, Util.StringCallback callback)
        {
            WrappedAssembly.SetWarningCallback(converter, callback);
        }
    }
}

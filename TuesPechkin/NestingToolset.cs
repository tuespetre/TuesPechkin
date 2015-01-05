using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TuesPechkin
{
    public abstract class NestingToolset : MarshalByRefObject, IToolset
    {
        public abstract event EventHandler Unloaded;

        internal event EventHandler BeforeUnload;

        public IDeployment Deployment { get; protected set; }

        public bool Loaded { get; protected set; }

        protected IToolset NestedToolset { get; set; }

        public abstract void Load(IDeployment deployment = null);

        public abstract void Unload();

        public override object InitializeLifetimeService()
        {
            return null;
        }

        internal void OnBeforeUnload(object sender)
        {
            if (BeforeUnload != null)
            {
                BeforeUnload(sender, EventArgs.Empty);
            }
        }

        public void AddObject(IntPtr converter, IntPtr objectConfig, byte[] html)
        {
            NestedToolset.AddObject(converter, objectConfig, html);
        }

        public void AddObject(IntPtr converter, IntPtr objectConfig, string html)
        {
            NestedToolset.AddObject(converter, objectConfig, html);
        }

        public IntPtr CreateConverter(IntPtr globalSettings)
        {
            return NestedToolset.CreateConverter(globalSettings);
        }

        public IntPtr CreateGlobalSettings()
        {
            return NestedToolset.CreateGlobalSettings();
        }

        public IntPtr CreateObjectSettings()
        {
            return NestedToolset.CreateObjectSettings();
        }

        public void DestroyConverter(IntPtr converter)
        {
            NestedToolset.DestroyConverter(converter);
        }

        public byte[] GetConverterResult(IntPtr converter)
        {
            return NestedToolset.GetConverterResult(converter);
        }

        public string GetGlobalSetting(IntPtr setting, string name)
        {
            return NestedToolset.GetGlobalSetting(setting, name);
        }

        public int GetHttpErrorCode(IntPtr converter)
        {
            return NestedToolset.GetHttpErrorCode(converter);
        }

        public string GetObjectSetting(IntPtr setting, string name)
        {
            return NestedToolset.GetObjectSetting(setting, name);
        }

        public int GetPhaseCount(IntPtr converter)
        {
            return NestedToolset.GetPhaseCount(converter);
        }

        public string GetPhaseDescription(IntPtr converter, int phase)
        {
            return NestedToolset.GetPhaseDescription(converter, phase);
        }

        public int GetPhaseNumber(IntPtr converter)
        {
            return NestedToolset.GetPhaseNumber(converter);
        }

        public string GetProgressDescription(IntPtr converter)
        {
            return NestedToolset.GetProgressDescription(converter);
        }

        public bool PerformConversion(IntPtr converter)
        {
            return NestedToolset.PerformConversion(converter);
        }

        public void SetErrorCallback(IntPtr converter, StringCallback callback)
        {
            NestedToolset.SetErrorCallback(converter, callback);
        }

        public void SetFinishedCallback(IntPtr converter, IntCallback callback)
        {
            NestedToolset.SetFinishedCallback(converter, callback);
        }

        public int SetGlobalSetting(IntPtr setting, string name, string value)
        {
            return NestedToolset.SetGlobalSetting(setting, name, value);
        }

        public int SetObjectSetting(IntPtr setting, string name, string value)
        {
            return NestedToolset.SetObjectSetting(setting, name, value);
        }

        public void SetPhaseChangedCallback(IntPtr converter, VoidCallback callback)
        {
            NestedToolset.SetPhaseChangedCallback(converter, callback);
        }

        public void SetProgressChangedCallback(IntPtr converter, IntCallback callback)
        {
            NestedToolset.SetProgressChangedCallback(converter, callback);
        }

        public void SetWarningCallback(IntPtr converter, StringCallback callback)
        {
            NestedToolset.SetWarningCallback(converter, callback);
        }
    }
}

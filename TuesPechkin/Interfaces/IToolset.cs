using System;
using System.Runtime.Serialization;

namespace TuesPechkin
{
    public interface IToolset
    {
        /// <summary>
        /// Loads the toolset's deployment from its path. It must behave idempotently,
        /// performing no operation if the deployment is already loaded.
        /// </summary>
        /// <param name="deployment">
        /// Optionally supplies a specific deployment to be used.
        /// </param>
        void Load(IDeployment deployment = null);

        /// <summary>
        /// Gets whether the toolset's deployment has been loaded.
        /// </summary>
        bool Loaded { get; }

        /// <summary>
        /// Unloads the toolset's deployment.
        /// </summary>
        void Unload();

        /// <summary>
        /// Fires when the toolset's deployment is unloaded.
        /// </summary>
        event EventHandler Unloaded;

        /// <summary>
        /// The deployment loaded by (or to be loaded by) the toolset.
        /// </summary>
        IDeployment Deployment { get; }

        #region calls to native API
        void AddObject(IntPtr converter, IntPtr objectConfig, byte[] html);
        void AddObject(IntPtr converter, IntPtr objectConfig, string html);
        IntPtr CreateConverter(IntPtr globalSettings);
        IntPtr CreateGlobalSettings();
        IntPtr CreateObjectSettings();
        void DestroyConverter(IntPtr converter);
        byte[] GetConverterResult(IntPtr converter);
        string GetGlobalSetting(IntPtr setting, string name);
        int GetHttpErrorCode(IntPtr converter);
        string GetObjectSetting(IntPtr setting, string name);
        int GetPhaseCount(IntPtr converter);
        string GetPhaseDescription(IntPtr converter, int phase);
        int GetPhaseNumber(IntPtr converter);
        string GetProgressDescription(IntPtr converter);
        bool PerformConversion(IntPtr converter);
        void SetErrorCallback(IntPtr converter, StringCallback callback);
        void SetFinishedCallback(IntPtr converter, IntCallback callback);
        int SetGlobalSetting(IntPtr setting, string name, string value);
        int SetObjectSetting(IntPtr setting, string name, string value);
        void SetPhaseChangedCallback(IntPtr converter, VoidCallback callback);
        void SetProgressChangedCallback(IntPtr converter, IntCallback callback);
        void SetWarningCallback(IntPtr converter, StringCallback callback);
        #endregion
    }
}

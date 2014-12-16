using System;
using TuesPechkin.Util;

namespace TuesPechkin
{
    public interface IAssembly
    {
        /// <summary>
        /// Loads the assembly from its path. Throws AssemblyAlreadyLoadedException
        /// if the assembly was already loaded.
        /// </summary>
        /// <param name="pathOverride">
        /// Optionally supplies a specific assembly path. This path must overwrite 
        /// the Path property of the IAssembly.
        /// </param>
        void Load (string pathOverride = null);

        /// <summary>
        /// Gets whether the assembly has been loaded.
        /// </summary>
        bool Loaded { get; }

        /// <summary>
        /// Gets the path from which the assembly is loaded.
        /// </summary>
        string Path { get; }

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

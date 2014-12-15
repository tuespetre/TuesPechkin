using System;

namespace TuesPechkin
{
    public interface IAssembly
    {
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
        string Path { get; }
        bool PerformConversion(IntPtr converter);
        void SetErrorCallback(IntPtr converter, TuesPechkin.Util.StringCallback callback);
        void SetFinishedCallback(IntPtr converter, TuesPechkin.Util.IntCallback callback);
        int SetGlobalSetting(IntPtr setting, string name, string value);
        int SetObjectSetting(IntPtr setting, string name, string value);
        void SetPhaseChangedCallback(IntPtr converter, TuesPechkin.Util.VoidCallback callback);
        void SetProgressChangedCallback(IntPtr converter, TuesPechkin.Util.IntCallback callback);
        void SetWarningCallback(IntPtr converter, TuesPechkin.Util.StringCallback callback);
        Version Version { get; }
    }
}

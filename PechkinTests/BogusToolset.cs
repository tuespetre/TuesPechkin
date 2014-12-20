using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuesPechkin.Tests
{
    public class BogusToolset : IToolset
    {
        public event EventHandler Unloaded;

        public void Unload()
        {
            if (Unloaded != null)
            {
                Unloaded(this, EventArgs.Empty);
            }
        }

        public void Load(IDeployment deployment = null)
        {
            throw new NotImplementedException();
        }

        public bool Loaded
        {
            get { throw new NotImplementedException(); }
        }

        public IDeployment Deployment
        {
            get { throw new NotImplementedException(); }
        }

        public void AddObject(IntPtr converter, IntPtr objectConfig, byte[] html)
        {
            throw new NotImplementedException();
        }

        public void AddObject(IntPtr converter, IntPtr objectConfig, string html)
        {
            throw new NotImplementedException();
        }

        public IntPtr CreateConverter(IntPtr globalSettings)
        {
            throw new NotImplementedException();
        }

        public IntPtr CreateGlobalSettings()
        {
            throw new NotImplementedException();
        }

        public IntPtr CreateObjectSettings()
        {
            throw new NotImplementedException();
        }

        public void DestroyConverter(IntPtr converter)
        {
            throw new NotImplementedException();
        }

        public byte[] GetConverterResult(IntPtr converter)
        {
            throw new NotImplementedException();
        }

        public string GetGlobalSetting(IntPtr setting, string name)
        {
            throw new NotImplementedException();
        }

        public int GetHttpErrorCode(IntPtr converter)
        {
            throw new NotImplementedException();
        }

        public string GetObjectSetting(IntPtr setting, string name)
        {
            throw new NotImplementedException();
        }

        public int GetPhaseCount(IntPtr converter)
        {
            throw new NotImplementedException();
        }

        public string GetPhaseDescription(IntPtr converter, int phase)
        {
            throw new NotImplementedException();
        }

        public int GetPhaseNumber(IntPtr converter)
        {
            throw new NotImplementedException();
        }

        public string GetProgressDescription(IntPtr converter)
        {
            throw new NotImplementedException();
        }

        public bool PerformConversion(IntPtr converter)
        {
            throw new NotImplementedException();
        }

        public void SetErrorCallback(IntPtr converter, Util.StringCallback callback)
        {
            throw new NotImplementedException();
        }

        public void SetFinishedCallback(IntPtr converter, Util.IntCallback callback)
        {
            throw new NotImplementedException();
        }

        public int SetGlobalSetting(IntPtr setting, string name, string value)
        {
            throw new NotImplementedException();
        }

        public int SetObjectSetting(IntPtr setting, string name, string value)
        {
            throw new NotImplementedException();
        }

        public void SetPhaseChangedCallback(IntPtr converter, Util.VoidCallback callback)
        {
            throw new NotImplementedException();
        }

        public void SetProgressChangedCallback(IntPtr converter, Util.IntCallback callback)
        {
            throw new NotImplementedException();
        }

        public void SetWarningCallback(IntPtr converter, Util.StringCallback callback)
        {
            throw new NotImplementedException();
        }

        public void SetUp(bool useGraphics = false)
        {
            throw new NotImplementedException();
        }

        public void TearDown()
        {
            throw new NotImplementedException();
        }
    }
}

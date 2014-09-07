using System;
using System.Collections.Generic;
using System.Text;

namespace TuesPechkin.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class GlobalSettingAttribute : WkhtmltopdfSettingAttribute
    {
        public GlobalSettingAttribute(string settingName)
        {
            this.SettingName = settingName;
        }

        public override void Apply(IntPtr config, object value)
        {
            Apply(config, value, PechkinStatic.SetGlobalSetting);
        }
    }
}

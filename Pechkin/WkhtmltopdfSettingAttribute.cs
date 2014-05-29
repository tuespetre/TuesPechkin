using System;

namespace TuesPechkin
{
    [AttributeUsage(AttributeTargets.Property)]
    public class WkhtmltopdfSettingAttribute : Attribute
    {
        public WkhtmltopdfSettingAttribute(string settingName)
        {
            this.SettingName = settingName;
        }

        public string SettingName { get; set; }
    }
}
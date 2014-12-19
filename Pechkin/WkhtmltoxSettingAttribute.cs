using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TuesPechkin.Util;

namespace TuesPechkin
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class WkhtmltoxSettingAttribute : Attribute
    {
        public string Name { get; set; }

        public WkhtmltoxSettingAttribute(string name)
        {
            Name = name;
        }
    }
}

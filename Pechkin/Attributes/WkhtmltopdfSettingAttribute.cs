using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TuesPechkin.Util;

namespace TuesPechkin.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    internal abstract class WkhtmltopdfSettingAttribute : Attribute
    {
        public string SettingName { get; set; }

        public abstract void Apply(IAssembly assembly, IntPtr config, object value);

        protected void Apply(IntPtr config, object value, Func<IntPtr, string, string, int> applicator)
        {
            Action<string, string> apply = (n, v) => applicator(config, n, v);
            var type = value.GetType();

            if (type == typeof(double?))
            {
                apply(
                    ((double?)value).Value.ToString("0.##", CultureInfo.InvariantCulture),
                    SettingName);
            }
            else if (type == typeof(bool?))
            {
                apply(
                    ((bool?)value).Value ? "true" : "false",
                    SettingName);
            }
            else if (type == typeof(Dictionary<string, string>))
            {
                var dictionary = (Dictionary<string, string>)value;

                foreach (var entry in dictionary)
                {
                    if (entry.Key == null || entry.Value == null)
                    {
                        continue;
                    }

                    apply(SettingName + ".append", null);
                    apply(string.Format("{0}[0]", SettingName), entry.Key + "," + entry.Value);
                }
            }
            else
            {
                apply(SettingName, value.ToString());
            }
        }
    }
}

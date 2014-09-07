using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace TuesPechkin
{
    internal static class SettingApplicator
    {
        public static void ApplySettings(IntPtr config, object settings, bool global = false)
        {
            if (settings == null)
            {
                return;
            }

            var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            foreach (var property in settings.GetType().GetProperties(bindingFlags))
            {
                var attributes = property.GetCustomAttributes(true);

                if (attributes.Length == 0 || !(attributes[0] is WkhtmltopdfSettingAttribute))
                {
                    continue;
                }

                var attribute = attributes[0] as WkhtmltopdfSettingAttribute;
                var rawValue = property.GetValue(settings, null);

                if (rawValue == null)
                {
                    continue;
                }

                if (property.PropertyType != typeof(Dictionary<string, string>))
                {
                    var value = GetStringValue(property, rawValue);
                    Set(global, config, attribute.SettingName, value);
                }
                else
                {
                    var dictionary = (Dictionary<string, string>)rawValue;
                    var index = 0;

                    foreach (var entry in dictionary)
                    {
                        if (entry.Key == null || entry.Value == null)
                        {
                            continue;
                        }

                        var name = string.Format("{0}[{1}]", attribute.SettingName, index);
                        var value = entry.Key + "," + entry.Value;

                        Set(global, config, attribute.SettingName + ".append", null);
                        Set(global, config, name, value);

                        index++;
                    }
                }
            }
        }

        private static string GetStringValue(PropertyInfo property, object value)
        {
            var type = property.PropertyType;

            if (type == typeof(double?) || type == typeof(double))
            {
                return ((double?)value).Value.ToString("0.##", CultureInfo.InvariantCulture);
            }
            else if (type == typeof(bool?) || type == typeof(bool))
            {
                return ((bool?)value).Value ? "true" : "false";
            }
            else
            {
                return value.ToString();
            }
        }

        private static void Set(bool global, IntPtr config, string name, string value)
        {
            if (global)
            {
                PechkinStatic.SetGlobalSetting(config, name, value);
            }
            else
            {
                PechkinStatic.SetObjectSetting(config, name, value);
            }
        }
    }
}
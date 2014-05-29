using System;
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

                var value = GetStringValue(property, rawValue);

                if (global)
                {
                    PechkinStatic.SetGlobalSetting(config, attribute.SettingName, value);
                }
                else
                {
                    PechkinStatic.SetObjectSetting(config, attribute.SettingName, value);
                }
            }
        }

        private static string GetStringValue(PropertyInfo property, object value)
        {
            var u = Nullable.GetUnderlyingType(property.PropertyType);

            if (property.PropertyType == typeof(double?))
            {
                return ((double?)value).Value.ToString("0.##", CultureInfo.InvariantCulture);
            }
            else if (property.PropertyType == typeof(bool?))
            {
                return ((bool?)value).Value ? "true" : "false";
            }
            else
            {
                return value.ToString();
            }
        }
    }
}
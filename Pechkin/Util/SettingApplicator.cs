using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using TuesPechkin.Attributes;

namespace TuesPechkin
{
    internal static class SettingApplicator
    {
        public static void ApplySettings(IToolset assembly, IntPtr config, object settings)
        {
            if (settings == null)
            {
                return;
            }

            var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            foreach (var property in settings.GetType().GetProperties(bindingFlags))
            {
                var attributes = property.GetCustomAttributes(true);
                var rawValue = property.GetValue(settings, null);

                if (rawValue == null || attributes.Length == 0)
                {
                    continue;
                }
                else if (attributes[0] is WkhtmltopdfSettingAttribute)
                {
                    var attribute = attributes[0] as WkhtmltopdfSettingAttribute;

                    attribute.Apply(assembly, config, rawValue);
                }
                else if (attributes[0] is SettingBagAttribute)
                {
                    ApplySettings(assembly, config, rawValue);
                }
            }
        }
    }
}
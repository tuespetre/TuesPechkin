using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using TuesPechkin.Util;

namespace TuesPechkin
{
    /// <summary>
    /// Static class with utility methods for the interface.
    /// Acts mostly as a facade over PechkinBindings with log tracing.
    /// </summary>
    [Serializable]
    internal static class PechkinStatic
    {
        public static IntPtr CreateGlobalSetting()
        {
            Tracer.Trace("T:" + Thread.CurrentThread.Name + " Creating global settings (wkhtmltopdf_create_global_settings)");

            return PechkinBindings.wkhtmltopdf_create_global_settings();
        }

        public static IntPtr CreateObjectSettings()
        {
            Tracer.Trace("T:" + Thread.CurrentThread.Name + " Creating object settings (wkhtmltopdf_create_object_settings)");

            return PechkinBindings.wkhtmltopdf_create_object_settings();
        }

        public static int SetGlobalSetting(IntPtr setting, string name, string value)
        {
            Tracer.Trace(
                String.Format(
                    "T:{0} Setting global setting '{1}' to '{2}' for config {3}",
                    Thread.CurrentThread.Name,
                    name,
                    value,
                    setting));

            return PechkinBindings.wkhtmltopdf_set_global_setting(setting, name, value);
        }

        public static string GetGlobalSetting(IntPtr setting, string name)
        {
            Tracer.Trace("T:" + Thread.CurrentThread.Name + " Getting global setting (wkhtmltopdf_get_global_setting)");

            byte[] buf = new byte[2048];

            PechkinBindings.wkhtmltopdf_get_global_setting(setting, name, ref buf, buf.Length);

            int walk = 0;
            while (walk < buf.Length && buf[walk] != 0)
            {
                walk++;
            }

            byte[] buf2 = new byte[walk];
            Array.Copy(buf, 0, buf2, 0, walk);

            return Encoding.UTF8.GetString(buf2);
        }

        public static int SetObjectSetting(IntPtr setting, string name, string value)
        {
            Tracer.Trace(
                String.Format(
                    "T:{0} Setting object setting '{1}' to '{2}' for config {3}",
                    Thread.CurrentThread.Name,
                    name,
                    value,
                    setting));

            return PechkinBindings.wkhtmltopdf_set_object_setting(setting, name, value);
        }

        public static string GetObjectSetting(IntPtr setting, string name)
        {
            Tracer.Trace("T:" + Thread.CurrentThread.Name + " Getting object setting (wkhtmltopdf_get_object_setting)");

            byte[] buf = new byte[2048];

            PechkinBindings.wkhtmltopdf_get_object_setting(setting, name, ref buf, buf.Length);

            int walk = 0;
            while (walk < buf.Length && buf[walk] != 0)
            {
                walk++;
            }

            byte[] buf2 = new byte[walk];
            Array.Copy(buf, 0, buf2, 0, walk);

            return Encoding.UTF8.GetString(buf2);
        }

        public static IntPtr CreateConverter(IntPtr globalSettings)
        {
            Tracer.Trace("T:" + Thread.CurrentThread.Name + " Creating converter (wkhtmltopdf_create_converter)");

            return PechkinBindings.wkhtmltopdf_create_converter(globalSettings);
        }

        public static void DestroyConverter(IntPtr converter)
        {
            Tracer.Trace("T:" + Thread.CurrentThread.Name + " Destroying converter (wkhtmltopdf_destroy_converter)");

            PechkinBindings.wkhtmltopdf_destroy_converter(converter);
        }

        public static void SetWarningCallback(IntPtr converter, StringCallback callback)
        {
            Tracer.Trace("T:" + Thread.CurrentThread.Name + " Setting warning callback (wkhtmltopdf_set_warning_callback)");
            
            PechkinBindings.wkhtmltopdf_set_warning_callback(converter, callback);
        }

        public static void SetErrorCallback(IntPtr converter, StringCallback callback)
        {
            Tracer.Trace("T:" + Thread.CurrentThread.Name + " Setting error callback (wkhtmltopdf_set_error_callback)");
            
            PechkinBindings.wkhtmltopdf_set_error_callback(converter, callback);
        }

        public static void SetFinishedCallback(IntPtr converter, IntCallback callback)
        {
            Tracer.Trace("T:" + Thread.CurrentThread.Name + " Setting finished callback (wkhtmltopdf_set_finished_callback)");

            PechkinBindings.wkhtmltopdf_set_finished_callback(converter, callback);
        }

        public static void SetPhaseChangedCallback(IntPtr converter, VoidCallback callback)
        {
            Tracer.Trace("T:" + Thread.CurrentThread.Name + " Setting phase change callback (wkhtmltopdf_set_phase_changed_callback)");

            PechkinBindings.wkhtmltopdf_set_phase_changed_callback(converter, callback);
        }

        public static void SetProgressChangedCallback(IntPtr converter, IntCallback callback)
        {
            Tracer.Trace("T:" + Thread.CurrentThread.Name + " Setting progress change callback (wkhtmltopdf_set_progress_changed_callback)");

            PechkinBindings.wkhtmltopdf_set_progress_changed_callback(converter, callback);
        }

        public static bool PerformConversion(IntPtr converter)
        {
            Tracer.Trace("T:" + Thread.CurrentThread.Name + " Starting conversion (wkhtmltopdf_convert)");

            return PechkinBindings.wkhtmltopdf_convert(converter) != 0;
        }

        public static void AddObject(IntPtr converter, IntPtr objectConfig, string html)
        {
            Tracer.Trace("T:" + Thread.CurrentThread.Name + " Adding string object (wkhtmltopdf_add_object)");

            PechkinBindings.wkhtmltopdf_add_object(converter, objectConfig, html);
        }

        public static void AddObject(IntPtr converter, IntPtr objectConfig, byte[] html)
        {
            Tracer.Trace("T:" + Thread.CurrentThread.Name + " Adding byte[] object (wkhtmltopdf_add_object)");

            PechkinBindings.wkhtmltopdf_add_object(converter, objectConfig, html);
        }

        public static int GetPhaseNumber(IntPtr converter)
        {
            Tracer.Trace("T:" + Thread.CurrentThread.Name + " Requesting current phase (wkhtmltopdf_current_phase)");

            return PechkinBindings.wkhtmltopdf_current_phase(converter);
        }

        public static int GetPhaseCount(IntPtr converter)
        {
            Tracer.Trace("T:" + Thread.CurrentThread.Name + " Requesting phase count (wkhtmltopdf_phase_count)");

            return PechkinBindings.wkhtmltopdf_phase_count(converter);
        }

        public static string GetPhaseDescription(IntPtr converter, int phase)
        {
            Tracer.Trace("T:" + Thread.CurrentThread.Name + " Requesting phase description (wkhtmltopdf_phase_description)");

            return Marshal.PtrToStringAnsi(PechkinBindings.wkhtmltopdf_phase_description(converter, phase));
        }

        public static string GetProgressDescription(IntPtr converter)
        {
            Tracer.Trace("T:" + Thread.CurrentThread.Name + " Requesting progress string (wkhtmltopdf_progress_string)");

            return Marshal.PtrToStringAnsi(PechkinBindings.wkhtmltopdf_progress_string(converter));
        }

        public static int GetHttpErrorCode(IntPtr converter)
        {
            Tracer.Trace("T:" + Thread.CurrentThread.Name + " Requesting http error code (wkhtmltopdf_http_error_code)");

            return PechkinBindings.wkhtmltopdf_http_error_code(converter);
        }

        public static byte[] GetConverterResult(IntPtr converter)
        {
            Tracer.Trace("T:" + Thread.CurrentThread.Name + " Requesting converter result (wkhtmltopdf_get_output)");

            IntPtr tmp;
            var len = PechkinBindings.wkhtmltopdf_get_output(converter, out tmp);
            var output = new byte[len];
            Marshal.Copy(tmp, output, 0, output.Length);
            return output;
        }
    }
}

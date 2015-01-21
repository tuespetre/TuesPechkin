using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.InteropServices;

namespace TuesPechkin
{
    [Serializable]
    public unsafe static class WkhtmltoxBindings
    {
        public const string DLLNAME = "wkhtmltox.dll";
        private const CharSet CHARSET = CharSet.Unicode;

        #region pdf bindings
        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern void wkhtmltopdf_add_object(IntPtr converter, IntPtr objectSettings,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8Marshaler))]
            String data);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern void wkhtmltopdf_add_object(IntPtr converter, IntPtr objectSettings, byte[] data);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltopdf_convert(IntPtr converter);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern IntPtr wkhtmltopdf_create_converter(IntPtr globalSettings);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern IntPtr wkhtmltopdf_create_global_settings();

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern IntPtr wkhtmltopdf_create_object_settings();

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltopdf_current_phase(IntPtr converter);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltopdf_deinit();

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern void wkhtmltopdf_destroy_converter(IntPtr converter);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltopdf_extended_qt();

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static unsafe extern int wkhtmltopdf_get_global_setting(IntPtr settings,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8Marshaler))]
            String name,
            byte* value, int valueSize);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltopdf_get_object_setting(IntPtr settings,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8Marshaler))]
            String name,
            byte* value, int vs);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltopdf_get_output(IntPtr converter, out IntPtr data);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltopdf_http_error_code(IntPtr converter);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltopdf_init(int useGraphics);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltopdf_phase_count(IntPtr converter);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern IntPtr wkhtmltopdf_phase_description(IntPtr converter, int phase);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern IntPtr wkhtmltopdf_progress_string(IntPtr converter);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern void wkhtmltopdf_set_error_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)]
                                                                 StringCallback callback);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern void wkhtmltopdf_set_finished_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)]
                                                                    IntCallback callback);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltopdf_set_global_setting(IntPtr settings,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8Marshaler))]
            String name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8Marshaler))]
            String value);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltopdf_set_object_setting(IntPtr settings,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8Marshaler))]
            String name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8Marshaler))]
            String value);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern void wkhtmltopdf_set_phase_changed_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)]
                                                                         VoidCallback callback);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern void wkhtmltopdf_set_progress_changed_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)]
                                                                            IntCallback callback);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern void wkhtmltopdf_set_warning_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)]
                                                                   StringCallback callback);
        #endregion

        #region image bindings
        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltoimage_convert(IntPtr converter);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern IntPtr wkhtmltoimage_create_converter(IntPtr globalSettings, byte[] data);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern IntPtr wkhtmltoimage_create_global_settings();

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltoimage_current_phase(IntPtr converter);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltoimage_deinit();

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern void wkhtmltoimage_destroy_converter(IntPtr converter);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltoimage_extended_qt();

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static unsafe extern int wkhtmltoimage_get_global_setting(IntPtr settings,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8Marshaler))]
            String name,
            byte* value, int valueSize);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltoimage_get_output(IntPtr converter, out IntPtr data);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltoimage_http_error_code(IntPtr converter);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltoimage_init(int useGraphics);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltoimage_phase_count(IntPtr converter);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern IntPtr wkhtmltoimage_phase_description(IntPtr converter, int phase);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern IntPtr wkhtmltoimage_progress_string(IntPtr converter);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern void wkhtmltoimage_set_error_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)]
                                                                 StringCallback callback);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern void wkhtmltoimage_set_finished_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)]
                                                                    IntCallback callback);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltoimage_set_global_setting(IntPtr settings,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8Marshaler))]
            String name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8Marshaler))]
            String value);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern void wkhtmltoimage_set_phase_changed_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)]
                                                                         VoidCallback callback);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern void wkhtmltoimage_set_progress_changed_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)]
                                                                            IntCallback callback);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern void wkhtmltoimage_set_warning_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)]
                                                                   StringCallback callback);
        #endregion
    }
}
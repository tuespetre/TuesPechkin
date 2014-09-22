using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.InteropServices;
using TuesPechkin.Properties;
using TuesPechkin.Util;

namespace TuesPechkin
{
    [Serializable]
    internal unsafe static class PechkinBindings
    {
        public const string DLLNAME = "wkhtmltox.dll";
        private const CharSet CHARSET = CharSet.Unicode;

        public static string TocXslFilename { get; private set; }

        static PechkinBindings()
        {
            var raw = (IntPtr.Size == 8) ? Resources.wkhtmltox_64_dll : Resources.wkhtmltox_32_dll;

            if (Factory.AssemblyPath == null)
            {
                SetupUnmanagedAssembly("wkhtmltox.dll", raw);
            }
            else
            {
                WinApiHelper.LoadLibrary(Factory.AssemblyPath);
            }
        }

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

        [DllImport(DLLNAME, CharSet = CHARSET)]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static extern String wkhtmltopdf_version();

        private static void SetupUnmanagedAssembly(string fileName, byte[] assemblyRaw)
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetName();
            var basePath = Path.Combine(
                Path.GetTempPath(),
                String.Format(
                    "{0}{1}_{2}_{3}",
                    assemblyName.Name.ToString(),
                    assemblyName.Version.ToString(),
                    IntPtr.Size == 8 ? "x64" : "x86",
                    AppDomain.CurrentDomain.BaseDirectory.GetHashCode()));

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            fileName = Path.Combine(basePath, fileName);

            WriteStreamToFile(fileName, () => new GZipStream(
                new MemoryStream(assemblyRaw), 
                CompressionMode.Decompress));

            TocXslFilename = Path.Combine(basePath, "toc.xsl");

            WriteStreamToFile(TocXslFilename, () => new MemoryStream(Resources.toc));

            WinApiHelper.LoadLibrary(fileName);
        }

        private static void WriteStreamToFile(string fileName, Func<Stream> streamFactory)
        {
            if (!File.Exists(fileName))
            {
                var stream = streamFactory();
                var writeBuffer = new byte[8192];
                var writeLength = 0;

                using (var newFile = File.Open(fileName, FileMode.Create))
                {
                    while ((writeLength = stream.Read(writeBuffer, 0, writeBuffer.Length)) > 0)
                    {
                        newFile.Write(writeBuffer, 0, writeLength);
                    }
                }
            }
        }
    }
}
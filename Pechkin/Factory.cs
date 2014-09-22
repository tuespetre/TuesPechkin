using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using System.Security;
using System.Security.Principal;
using System.Threading;
using TuesPechkin.Util;

namespace TuesPechkin
{
    /// <summary>
    /// Static class used to obtain instances of a PDF converter implementing the 
    /// IPechkin interface.
    /// </summary>
    public static class Factory
    {
        private static readonly object setupLock = new object();

        /// <summary>
        /// The AppDomain used to encapsulate calls to the wkhtmltopdf library
        /// </summary>
        private static AppDomain operatingDomain = null;

        /// <summary>
        /// When initializing the library, TuesPechkin will look for a value set
        /// here to load the wkhtmltopdf library. Use this to specify the location
        /// of a desired version of wkhtmltox.dll; it must be the full filename including the path.
        /// </summary>
        public static string AssemblyPath { get; set; }

        /// <summary>
        /// When set to true, Pechkin.Factory will set up wkhtmltopdf to use X11 graphics mode.
        /// Default value is false. Changing the value of this property will have no effect on the behavior
        /// of wkhtmltopdf once the library has been initialized.
        /// </summary>
        public static Boolean UseX11Graphics { get; set; }

        /// <summary>
        /// Returns an instance of a PDF converter that implements the IPechkin interface.
        /// </summary>
        /// <param name="config">A GlobalSettings object for the converter to apply.</param>
        /// <returns>IPechkin</returns>
        public static IPechkin Create()
        { 
            EnsureAppDomainSetup();
            
            ObjectHandle handle = Activator.CreateInstanceFrom(
                Factory.operatingDomain,
                Assembly.GetExecutingAssembly().Location,
                typeof(SimplePechkin).FullName,
                false,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                null,
                null,
                null,
                null);

            IPechkin instance = handle.Unwrap() as IPechkin;

            return new Proxy(instance);
        }

        private static void EnsureAppDomainSetup()
        {         
            if (Factory.operatingDomain == null)
            {
                lock (setupLock)
                {
                    if (Factory.operatingDomain == null)
                    {
                        Factory.SetupAppDomain();
                    }
                }
            }
        }

        /// <summary>
        /// Creates and initializes a private AppDomain and therein loads and initializes the
        /// wkhtmltopdf library. Attaches to the current AppDomain's DomainUnload event in IIS environments 
        /// to ensure that on re-deploy, the library is freed so the new AppDomain will be able to use it.
        /// </summary>
        internal static void SetupAppDomain()
        {
            SynchronizedDispatcher.Invoke(() =>
            {
                var assemblyLocation = Assembly.GetExecutingAssembly().Location;
                var dirName = System.IO.Path.GetDirectoryName(assemblyLocation);

                var setup = new AppDomainSetup() 
                { 
                    ApplicationBase = dirName, 
                    LoaderOptimization = LoaderOptimization.SingleDomain 
                };

                var domain = Factory.operatingDomain = AppDomain.CreateDomain("pechkin_internal_domain", null, setup);

                domain.SetData("useX11Graphics", UseX11Graphics);
                domain.SetData("extraPaths", ExtraAssemblyPaths);

                domain.DoCallBack(() =>
                {
                    foreach (var path in AppDomain.CurrentDomain.GetData("extraPaths") as List<string>)
                    {
                        Assembly.LoadFile(path);
                    }

                    var useX11Graphics = (bool)AppDomain.CurrentDomain.GetData("useX11Graphics");
                    PechkinBindings.wkhtmltopdf_init(useX11Graphics ? 1 : 0);
                });
            });

            if (AppDomain.CurrentDomain.IsDefaultAppDomain() == false)
            {
                AppDomain.CurrentDomain.DomainUnload += Factory.TearDownAppDomain;
            }
        }

        /// <summary>
        /// Unloads the private AppDomain and the wkhtmltopdf library, and if applicable, destroys
        /// the synchronization thread.
        /// </summary>
        /// <param name="sender">Typically a null value, not used in the method.</param>
        /// <param name="e">Typically EventArgs.Empty, not used in the method.</param>
        internal static void TearDownAppDomain(object sender, EventArgs e)
        {
            if (Factory.operatingDomain != null)
            {
                SynchronizedDispatcher.Invoke(() =>
                {
                    Factory.operatingDomain.DoCallBack(() => PechkinBindings.wkhtmltopdf_deinit());
                });

                AppDomain.Unload(Factory.operatingDomain);

                foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
                {
                    if (module.ModuleName == PechkinBindings.DLLNAME)
                    {
                        while (WinApiHelper.FreeLibrary(module.BaseAddress))
                        {
                        }
                    }
                }

                Factory.operatingDomain = null;
            }
        }

        // This is here mostly to enable testing of the bindings
        // Delicious spaghetti, spicy meatball, etc.
        internal static readonly List<string> ExtraAssemblyPaths = new List<string>();

        // This is here mostly to enable testing of the bindings
        // Delicious spaghetti, spicy meatball, etc.
        // Parmesan: in your callback, set "testdata" to be pulled
        // from the appdomain
        internal static object FreeFormCallback(CrossAppDomainDelegate callback)
        {
            EnsureAppDomainSetup();

            object result = null;

            SynchronizedDispatcher.Invoke(() =>
            {
                operatingDomain.DoCallBack(callback);
                result = operatingDomain.GetData("testdata");
            });

            return result;
        }
    }
}
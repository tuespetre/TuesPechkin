using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Remoting;
using Pechkin.Util;

namespace Pechkin
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
        /// When set to true, Pechkin.Factory will set up wkhtmltopdf to use X11 graphics mode.
        /// Default value is false. Changing the value of this property will have no effect on the behavior
        /// of wkhtmltopdf once the library has been initialized.
        /// </summary>
        public static Boolean UseX11Graphics { get; set; }

        /// <summary>
        /// Returns an instance of a PDF converter that implements the IPechkin interface.
        /// </summary>
        /// <param name="config">A GlobalConfig object for the converter to apply.</param>
        /// <returns>IPechkin</returns>
        public static IPechkin Create(GlobalConfig config)
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

            ObjectHandle handle = Activator.CreateInstanceFrom(
                Factory.operatingDomain,
                Assembly.GetExecutingAssembly().Location,
                typeof(SimplePechkin).FullName,
                false,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new object[] { config },
                null,
                null,
                null);

            IPechkin instance = handle.Unwrap() as IPechkin;

            return new Proxy(instance);
        }

        /// <summary>
        /// Creates and initializes a private AppDomain and therein loads and initializes the
        /// wkhtmltopdf library. Attaches to the current AppDomain's DomainUnload event in IIS environments 
        /// to ensure that on re-deploy, the library is freed so the new AppDomain will be able to use it.
        /// </summary>
        private static void SetupAppDomain()
        {
            var dirName = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var setup = new AppDomainSetup() { ApplicationBase = dirName };
            Factory.operatingDomain = AppDomain.CreateDomain("pechkin_internal_domain", null, setup);

            Func<object> del = () =>
            {
                Factory.operatingDomain.SetData("useX11Graphics", Factory.UseX11Graphics);

                Factory.operatingDomain.DoCallBack(() =>
                {
                    PechkinBindings.wkhtmltopdf_init((bool)AppDomain.CurrentDomain.GetData("useX11Graphics") ? 1 : 0);
                });

                return null;
            };

            SynchronizedDispatcher.Invoke(del);

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
        private static void TearDownAppDomain(object sender, EventArgs e)
        {
            SynchronizedDispatcher.Terminate();

            if (Factory.operatingDomain != null)
            {
                Func<object> del = () =>
                {
                    Factory.operatingDomain.DoCallBack(() => PechkinBindings.wkhtmltopdf_deinit());

                    return null;
                };

                SynchronizedDispatcher.Invoke(del);

                AppDomain.Unload(Factory.operatingDomain);

                foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
                {
                    if (module.ModuleName == "wkhtmltox.dll")
                    {
                        while (WinApiHelper.FreeLibrary(module.BaseAddress))
                        {
                        }
                    }
                }

                Factory.operatingDomain = null;
            }
        }
    }
}
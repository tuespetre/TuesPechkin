using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TuesPechkin;
using Xunit;
using Assert = Xunit.Assert;

namespace PechkinTests
{
    [Serializable]
    public class LibraryBindingTests
    {
        [Fact]
        public void wkhtmltopdf_get_object_setting_single()
        {
            // Arrange
            const string name = "page";
            const string value = "http://www.google.com";
            string result = null;

            // Act
            WrappedCallback(ref result, () =>
            {
                var setting = PechkinStatic.CreateObjectSettings();
                PechkinStatic.SetObjectSetting(setting, name, value);
                Return(PechkinStatic.GetObjectSetting(setting, name));
            });

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public void wkhtmltopdf_set_object_setting_multiple()
        {
            // Arrange
            const string name1 = "load.cookies.append";
            const string value1 = null;
            const string name2 = "load.cookies[0]";
            const string value2 = 
        }

        private void WrappedCallback<T>(ref T data, CrossAppDomainDelegate callback)
        {
            Factory.TearDownAppDomain(null, null);

            var appDomainSetup = new AppDomainSetup 
            { 
                ApplicationBase = AppDomain.CurrentDomain.BaseDirectory 
            };

            var domain = AppDomain.CreateDomain(
                Guid.NewGuid().ToString(), 
                null, 
                appDomainSetup);

            domain.DoCallBack(() =>
            {
                Factory.ExtraAssemblyPaths.Add(
                    Assembly.GetExecutingAssembly().Location);
            });

            domain.SetData("callback", callback);

            domain.DoCallBack(() =>
            {
                var freeform = (CrossAppDomainDelegate)AppDomain.CurrentDomain.GetData("callback");

                AppDomain.CurrentDomain.SetData(
                    "ret",
                    Factory.FreeFormCallback(freeform));
            });

            data = (T)domain.GetData("ret");

            AppDomain.Unload(domain);
        }

        private void Return(object data) 
        {
            AppDomain.CurrentDomain.SetData("testdata", data);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace TuesPechkin.Tests
{
    [TestClass]
    public abstract class TuesPechkinTests
    {
        protected const string TEST_WK_VER = "0.12.1";
        protected const string TEST_URL = "www.google.com";

        // Simulates 1.x.x
        protected static readonly ThreadSafeConverter converter;

        protected static readonly IToolset toolset;
                
        static TuesPechkinTests()
        {
            Debug.Listeners.Add(new DefaultTraceListener());

            toolset =
                new RemotingToolset<PdfToolset>(
                    new StaticDeployment(
                        Path.Combine(
                            AppDomain.CurrentDomain.BaseDirectory,
                            "wk-ver",
                            TEST_WK_VER)));

            converter = new ThreadSafeConverter(toolset);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            converter.Invoke(() => toolset.Unload());
        }

        protected AppDomain GetAppDomain(string name)
        {
            return AppDomain.CreateDomain(name, null, AppDomain.CurrentDomain.SetupInformation);
        }

        protected static string GetDeploymentPath()
        {
            return Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "wk-ver",
                TEST_WK_VER);
        }

        protected IConverter GetNewConverter()
        {
            return new StandardConverter(GetNewToolset());
        }

        protected IToolset GetNewToolset()
        {
            return new PdfToolset(GetNewDeployment());
        }

        protected static IDeployment GetNewDeployment()
        {
            return new StaticDeployment(GetDeploymentPath());
        }

        protected HtmlToPdfDocument Document(params ObjectSettings[] objects)
        {
            var doc = new HtmlToPdfDocument();
            doc.Objects.AddRange(objects);

            return doc;
        }

        protected ObjectSettings StringObject()
        {
            var html = GetResourceString("TuesPechkin.Tests.Resources.page.html");

            return new ObjectSettings { HtmlText = html };
        }

        protected ObjectSettings UrlObject()
        {
            return new ObjectSettings { PageUrl = TEST_URL };
        }

        protected static string GetResourceString(string name)
        {
            if (name == null)
            {
                return null;
            }

            Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);

            if (s == null)
            {
                return null;
            }

            return new StreamReader(s).ReadToEnd();
        }
    }
}

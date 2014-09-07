using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TuesPechkin;
using Xunit;
using Assert = Xunit.Assert;

namespace PechkinTests
{
    [Serializable]
    public class PechkinTests
    {
        static PechkinTests()
        {
            Debug.Listeners.Add(new DefaultTraceListener());
        }

        public static string GetResourceString(string name)
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

        [Fact]
        public void BubblesExceptionsFromSyncedThread()
        {
            Assert.Throws<ApplicationException>(() =>
            {
                SynchronizedDispatcher.Invoke(() => { throw new ApplicationException(); });
            });
        }

        [Fact]
        public void ConvertsAfterAppDomainRecycles()
        {
            // arrange
            Factory.TearDownAppDomain(null, null);

            var domain1 = this.GetAppDomain("testing_unload_1");
            byte[] result1 = null;
            var domain2 = this.GetAppDomain("testing_unload_2");
            byte[] result2 = null;

            // act
            domain1.DoCallBack(() =>
            {
                byte[] convert = Factory.Create().Convert("<p>some html</p>");
                AppDomain.CurrentDomain.SetData("result1", convert);
            });
            result1 = domain1.GetData("result1") as byte[];
            AppDomain.Unload(domain1);

            domain2.DoCallBack(() =>
            {
                byte[] convert = Factory.Create().Convert("<p>some html</p>");
                AppDomain.CurrentDomain.SetData("result2", convert);
            });
            result2 = domain2.GetData("result2") as byte[];
            AppDomain.Unload(domain2);

            // assert
            Assert.NotNull(result1);
            Assert.NotNull(result2);
        }

        [Fact]
        public void HandlesConcurrentThreads()
        {
            string html = GetResourceString("PechkinTests.Resources.page.html");
            int numberOfTasks = 10;
            int completed = 0;

            var tasks = Enumerable.Range(0, numberOfTasks).Select(i => new Task(() =>
            {
                Debug.WriteLine(String.Format("#{0} started", i + 1));
                IPechkin sc = Factory.Create();
                Assert.NotNull(sc.Convert(html));
                completed++;
                Debug.WriteLine(String.Format("#{0} completed", i + 1));
            }));

            Parallel.ForEach(tasks, task => task.Start());

            while (completed < numberOfTasks)
            {
                Thread.Sleep(1000);
            }
        }

        [Fact]
        public void ObjectIsHappilyGarbageCollected()
        {
            string html = GetResourceString("PechkinTests.Resources.page.html");

            IPechkin c = Factory.Create();

            byte[] ret = c.Convert(html);

            Assert.NotNull(ret);

            c = Factory.Create();
            ret = c.Convert(html);

            Assert.NotNull(ret);

            GC.Collect();
        }

        [Fact]
        public void OneObjectPerformsTwoConversionSequentially()
        {
            string html = GetResourceString("PechkinTests.Resources.page.html");

            IPechkin c = Factory.Create();

            byte[] ret = c.Convert(html);

            Assert.NotNull(ret);

            ret = c.Convert(html);

            Assert.NotNull(ret);
        }

        [Fact]
        public void ResultIsPdf()
        {
            string html = GetResourceString("PechkinTests.Resources.page.html");

            IPechkin c = Factory.Create();

            byte[] ret = c.Convert(html);

            Assert.NotNull(ret);

            byte[] right = Encoding.UTF8.GetBytes("%PDF");

            Assert.True(right.Length <= ret.Length);

            byte[] test = new byte[right.Length];
            Array.Copy(ret, 0, test, 0, right.Length);

            for (int i = 0; i < right.Length; i++)
            {
                Assert.Equal(right[i], test[i]);
            }
        }

        [Fact]
        public void ReturnsResultFromFile()
        {
            string html = GetResourceString("PechkinTests.Resources.page.html");

            string fn = string.Format("{0}.html", Path.GetTempFileName());
            FileStream fs = new FileStream(fn, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            sw.Write(html);

            sw.Close();

            IPechkin c = Factory.Create();

            byte[] ret = c.Convert(new ObjectSettings() { PageUrl = fn });

            Assert.NotNull(ret);

            File.Delete(fn);
        }

        [Fact]
        public void ReturnsResultFromString()
        {
            string html = GetResourceString("PechkinTests.Resources.page.html");

            IPechkin c = Factory.Create();

            byte[] ret = c.Convert(html);

            Assert.NotNull(ret);
        }

        [Fact]
        public void UnloadsWkhtmltoxWhenAppDomainUnloads()
        {
            // arrange
            Factory.TearDownAppDomain(null, null);

            var domain = this.GetAppDomain("testing_unload");

            // act
            domain.DoCallBack(() => Factory.Create().Convert("<p>some html</p>"));
            AppDomain.Unload(domain);

            // assert
            Assert.False(Process.GetCurrentProcess()
                                .Modules
                                .Cast<ProcessModule>()
                                .Any(m => m.ModuleName == "wkhtmltox.dll"));
        }

        private AppDomain GetAppDomain(string name)
        {
            // This is setup for using the Visual Studio test runner 
            // Specifically, the xUnit.net runner by Outercurve Foundation

            var appDomainSetup = new AppDomainSetup { ApplicationBase = AppDomain.CurrentDomain.BaseDirectory };
            var domain = AppDomain.CreateDomain(name, null, appDomainSetup);

            return domain;
        }
    }
}
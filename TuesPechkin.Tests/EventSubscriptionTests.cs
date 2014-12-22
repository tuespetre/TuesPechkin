using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuesPechkin.Tests
{
    [TestClass]
    public class EventSubscriptionTests : TuesPechkinTests
    {
        private void RunConversion(Action<IConverter> subscribe, IDocument document = null)
        {
            subscribe(converter);
            converter.Convert(document ?? Document(UrlObject(), StringObject()));
        }

        [TestMethod]
        public void BeginDoesNotBlockConversion()
        {
            var count = 0;

            RunConversion(c => c.Begin += (s, a) => count++);

            Assert.IsTrue(count > 0);
        }

        // Not fully implemented yet; haven't figured out how to force Error
        [TestMethod, Ignore]
        public void ErrorDoesNotBlockConversion()
        {
            var count = 0;

            RunConversion(
                c => c.Warning += (s, a) => count++,
                Document(new ObjectSettings
                {
                    PageUrl = "nonexistent.website.com",
                    LoadSettings =
                    {
                        ErrorHandling = LoadSettings.ContentErrorHandling.Abort
                    }
                }));

            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void FinishDoesNotBlockConversion()
        {
            var count = 0;

            RunConversion(c => c.Finish += (s, a) => count++);

            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void PhaseChangeDoesNotBlockConversion()
        {
            var count = 0;

            RunConversion(c => c.PhaseChange += (s, a) => count++);

            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void ProgressChangeDoesNotBlockConversion()
        {
            var count = 0;

            RunConversion(c => c.ProgressChange += (s, a) => count++);

            Assert.IsTrue(count > 0);
        }

        // Not fully implemented yet; haven't figured out how to force Error
        [TestMethod, Ignore]
        public void WarningDoesNotBlockConversion()
        {
            var count = 0;

            RunConversion(
                c => c.Warning += (s, a) => count++,
                Document(new ObjectSettings 
                { 
                    PageUrl = "nonexistent.website.com",
                    LoadSettings =
                    {
                        ErrorHandling = LoadSettings.ContentErrorHandling.Abort
                    }
                }));

            Assert.IsTrue(count > 0);
        }
    }
}

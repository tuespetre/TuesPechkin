using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace TuesPechkin.TestWebApp.Controllers
{
    public class HomeController : Controller
    {
        private static string specificPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "../TuesPechkin.Tests/wk-ver/0.12.2");

        private static string randomPath = Path.Combine(
            Path.GetTempPath(),
            Guid.NewGuid().ToString(),
            "wkhtmltox.dll");

        private static IConverter converter =
            new ThreadSafeConverter(
                new RemotingToolset<PdfToolset>(
                    new Win32EmbeddedDeployment(
                        new StaticDeployment(
                            randomPath))));

        // GET: /Home/
        public ActionResult Index()
        {
            var doc = new HtmlToPdfDocument();
            doc.Objects.Add(new ObjectSettings { PageUrl = "www.google.com " });
       
            for (var i = 0; i < 5; i++)
            {
                var result = converter.Convert(doc);
                var path = Path.Combine(Path.GetTempPath(), String.Format("{0}.pdf", i));

                System.IO.File.WriteAllBytes(path, result);
            }

            return this.View();
        }

        [HttpGet]
        public FileResult ScratchPad()
        {
            var doc = new HtmlToPdfDocument();
            var obj = new ObjectSettings();

            obj.PageUrl = Url.Action("PostAnything", "Home", routeValues: null, protocol: Request.Url.Scheme);
            obj.LoadSettings.CustomHeaders.Add("X-MY-HEADER", "my value");
            obj.LoadSettings.Cookies.Add("my_awesome_cookie", "cookie value");
            obj.LoadSettings.PostItems.Add(new PostItem 
            { 
                Name = "my_special_value", 
                Value = "is an amazing value" 
            });

            doc.Objects.Add(obj);

            var result = converter.Convert(doc);

            return File(result, "application/pdf");
        }

        public ActionResult PostAnything()
        {
            return View();
        }
    }
}
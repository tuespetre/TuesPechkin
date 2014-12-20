using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace TuesPechkin.TestWebApp.Controllers
{
    public class HomeController : Controller
    {
        private static IConverter converter =
            new ThreadSafeConverter(
                new PdfToolset(
                    new Win64EmbeddedDeployment(
                        new StaticDeployment(
                            Path.Combine(Path.GetTempPath(), "wkhtmltox.dll")))));

        //
        // GET: /Home/
        public ActionResult Index()
        {
            var html = "<p>Just some test HTML</p>";
            var doc = new HtmlDocument(html);
            
            for (var i = 0; i < 5; i++)
            {
                var result = converter.Convert(doc);
                var path = Path.Combine(Path.GetTempPath(), String.Format("{0}.pdf", i));

                System.IO.File.WriteAllBytes(path, result);
            }

            return this.View();
        }

        /*[HttpGet]
        public FileResult ScratchPad()
        {
            var doc = new HtmlDocument();
            var obj = new ObjectSettings();

            obj.PageUrl = Url.Action("PostAnything", "Home", routeValues: null, protocol: Request.Url.Scheme);
            obj.LoadSettings.CustomHeaders.Add("X-MY-HEADER", "my value");
            obj.LoadSettings.Cookies.Add("my_awesome_cookie", "cookie value");

            var converter = Factory.Create();
            var result = converter.Convert(obj);

            return File(result, "application/pdf");
        }*/

        public ActionResult PostAnything()
        {
            return View();
        }
    }
}
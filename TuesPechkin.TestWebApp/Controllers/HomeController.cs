using System;
using System.Linq;
using System.Web.Mvc;

namespace TuesPechkin.TestWebApp.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            /*var html = "<p>Just some test HTML</p>";
            
            for (var i = 0; i < 5; i++)
            {
                var converter = Factory.Create();
                var result = converter.Convert(html);
                var path = this.Server.MapPath(String.Format("/{0}.pdf", i));

                System.IO.File.WriteAllBytes(path, result);
            }*/

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

            var converter = Factory.Create();
            var result = converter.Convert(obj);

            return File(result, "application/pdf");
        }

        public ActionResult PostAnything()
        {
            return View();
        }
    }
}
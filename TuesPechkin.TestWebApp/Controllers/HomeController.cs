using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TuesPechkin.TestWebApp.Controllers
{
    public class HomeController : AsyncController
    {
        //
        // GET: /Home/
        public async Task<ActionResult> Index()
        {
            var html = "<p>Just some test HTML</p>";

            for (var i = 0; i < 5; i++)
            {
                var converter = Factory.Create();
                var path = this.Server.MapPath(String.Format("/{0}.pdf", i));

                using (var file = System.IO.File.Open(path, FileMode.Create))
                {
                    var result = new MemoryStream(converter.Convert(html));

                    result.CopyTo(file);
                }
            }

            return this.View();
        }
    }
}
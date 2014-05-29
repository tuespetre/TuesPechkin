using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TuesPechkin;

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
                converter.Convert(html);
            }

            return this.View();
        }
    }
}
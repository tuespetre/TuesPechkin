using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Pechkin;

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
                var converter = Factory.Create(new GlobalConfig());
                converter.Convert(html);
            }

            return this.View();
        }
    }
}
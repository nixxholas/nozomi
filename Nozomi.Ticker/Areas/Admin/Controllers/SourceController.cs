using Microsoft.AspNetCore.Mvc;

namespace Nozomi.Ticker.Areas.Admin.Controllers
{
    public class SourceController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return
            View();
        }
    }
}
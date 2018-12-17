using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Ticker.Models;

namespace Nozomi.Ticker.Areas
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : BaseViewController<HomeController>
    {
        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager) 
            : base(logger, userManager)
        {
        }
        
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
        
        [Route("/swagger")]
        public IActionResult Docs()
        {
            return new RedirectResult("~/docs");
        }

        public IActionResult Pricing()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Tutorials()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}
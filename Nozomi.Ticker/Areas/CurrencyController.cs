using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Service.Identity.Managers;

namespace Nozomi.Ticker.Areas
{
    public class CurrencyController : BaseViewController<CurrencyController>
    {
        public CurrencyController(ILogger<CurrencyController> logger, NozomiSignInManager signInManager, 
            NozomiUserManager userManager) 
            : base(logger, signInManager, userManager)
        {
        }

        [Route("/{abbrv}")]
        public IActionResult View(string abbrv)
        {
            return View();
        }
    }
}
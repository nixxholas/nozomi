using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Service.Identity.Managers;
using Nozomi.Ticker.Controllers;

namespace Nozomi.Ticker.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[controller]/manage/[action]")]
    [Authorize(Roles = "Owner, Administrator, Staff")]
    public class CurrencyController : BaseViewController<CurrencyController>
    {
        public CurrencyController(ILogger<CurrencyController> logger, NozomiSignInManager signInManager,
            NozomiUserManager userManager)
            : base(logger, signInManager, userManager)
        {
        }

        [HttpGet]
        public async Task<IActionResult> CreateCurrency()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return View();
        }
    }
}
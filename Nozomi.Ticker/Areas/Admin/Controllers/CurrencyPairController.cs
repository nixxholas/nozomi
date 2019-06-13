using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Identity.ViewModels.Manage.CurrencyPair;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Ticker.Controllers;

namespace Nozomi.Ticker.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Owner, Administrator, Staff")]
    public class CurrencyPairController : AreaBaseViewController<CurrencyPairController>
    {
        private readonly ICurrencyPairEvent _currencyPairEvent;
        private readonly ICurrencyPairService _currencyPairService;

        public CurrencyPairController(ILogger<CurrencyPairController> logger, NozomiSignInManager signInManager,
            NozomiUserManager userManager, ICurrencyPairEvent currencyPairEvent,
            ICurrencyPairService currencyPairService)
            : base(logger, signInManager, userManager)
        {
            _currencyPairEvent = currencyPairEvent;
            _currencyPairService = currencyPairService;
        }

        #region Get CurrencyPairs

        [HttpGet]
        public async Task<IActionResult> CurrencyPairs()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            return View(new CurrencyPairViewModel
            {
                CurrencyPairs = _currencyPairEvent.GetAll()
            });
        }

        #endregion
    }
}
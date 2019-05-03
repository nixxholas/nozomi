using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data.ViewModels.Admin.Currency;
using Nozomi.Infra.Admin.Service.Events.Interfaces;
using Nozomi.Service.Identity.Managers;
using Nozomi.Ticker.Controllers;

namespace Nozomi.Ticker.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[controller]/manage/[action]")]
    [Authorize(Roles = "Owner, Administrator, Staff")]
    public class CurrencyController : BaseViewController<CurrencyController>
    {
        private readonly ICurrencyAdminEvent _currencyAdminEvent;
        
        public CurrencyController(ILogger<CurrencyController> logger, NozomiSignInManager signInManager,
            NozomiUserManager userManager, ICurrencyAdminEvent currencyAdminEvent)
            : base(logger, signInManager, userManager)
        {
            _currencyAdminEvent = currencyAdminEvent;
        }

        [Route("{abbreviation}")]
        public IActionResult Currency([FromRoute]string abbreviation)
        {
            var vm = new CurrencyViewModel
            {
                Currency = _currencyAdminEvent.GetCurrencyByAbbreviation(abbreviation)  
            };
            
            return View(vm);
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
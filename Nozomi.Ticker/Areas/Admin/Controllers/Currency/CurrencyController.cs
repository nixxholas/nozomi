using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Models;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.Currency;
using Nozomi.Data.ViewModels.Admin.Currency;
using Nozomi.Infra.Admin.Service.Events.Interfaces;
using Nozomi.Infra.Admin.Service.Services.Interfaces;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Ticker.Controllers;

namespace Nozomi.Ticker.Areas.Admin.Controllers.Currency
{
    [Area("Admin")]
    [Authorize(Roles = "Owner, Administrator, Staff")]
    public class CurrencyController : AreaBaseViewController<CurrencyController>
    {
        private readonly ICurrencyAdminEvent _currencyAdminEvent;
        private readonly ICurrencyService _currencyService;
        private readonly ICurrencyTypeAdminEvent _currencyTypeAdminEvent;
        private readonly ISourceEvent _sourceEvent;
        private readonly ICurrencyPairEvent _currencyPairEvent;
        
        public CurrencyController(ILogger<CurrencyController> logger, ICurrencyAdminEvent currencyAdminEvent, 
            ICurrencyService currencyService, ICurrencyTypeAdminEvent currencyTypeAdminEvent, 
            ISourceEvent sourceEvent, ICurrencyPairEvent currencyPairEvent, SignInManager<User> signInManager,
            UserManager<User> userManager)
            : base(logger, signInManager, userManager)
        {
            _currencyAdminEvent = currencyAdminEvent;
            _currencyService = currencyService;
            _currencyTypeAdminEvent = currencyTypeAdminEvent;
            _sourceEvent = sourceEvent;
            _currencyPairEvent = currencyPairEvent;
        }

        #region Get Currencies

        [HttpGet]
        public async Task<IActionResult> Currencies()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var vm = new CurrenciesViewModel
            {
                Currencies = _currencyAdminEvent.GetAll(),
                CurrencyTypes = _currencyTypeAdminEvent.GetAllActive(),
                CurrencySources = _sourceEvent.GetAllActive()
            };

            return View(vm);
        }

        #endregion

        #region Get Currency(slug)

        [HttpGet("{slug}")]
        public IActionResult Currency([FromRoute] string slug)
        {
            var currency = _currencyAdminEvent.GetCurrencyBySlug(slug, true);
            var vm = new CurrencyViewModel
            {
                Currency = currency,
                CurrencySourcesOptions = _sourceEvent.GetAllCurrencySourceOptions(currency.CurrencySources),
                CurrencyTypes = _currencyTypeAdminEvent.GetAllActive(),
                CurrencyPairs = _currencyPairEvent.GetAllByMainCurrency(slug)
            };

            return View(vm);
        }

        #endregion

        #region PUT Edit Currency

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(long id, UpdateCurrency updateCurrency)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (id != updateCurrency.Id)
            {
                return BadRequest();
            }

            var result = _currencyService.Update(updateCurrency);

            if (result.ResultType.Equals(NozomiResultType.Success)) return Ok(result);

            // Update failed.
            return NotFound();
        }

        #endregion

        #region POST CreateCurrency

        [HttpPost]
        public async Task<IActionResult> CreateCurrency(CreateCurrency createCurrency)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var result = _currencyService.Create(createCurrency);

            if (result.ResultType.Equals(NozomiResultType.Success)) return Ok(result);

            // Create failed
            return NotFound();
        }

        #endregion

        #region Delete DeleteCurrency

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurrency(long id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (_currencyService.Delete(id).ResultType.Equals(NozomiResultType.Success)) return Ok();

            // Update failed.
            return NotFound();
        }

        #endregion
    }
}
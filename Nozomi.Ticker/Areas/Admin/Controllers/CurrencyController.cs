using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Identity.ViewModels.Manage.Currency;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.Currency;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ViewModels.Admin.Currency;
using Nozomi.Infra.Admin.Service.Events.Interfaces;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Ticker.Controllers;

namespace Nozomi.Ticker.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Owner, Administrator, Staff")]
    public class CurrencyController : AreaBaseViewController<CurrencyController>
    {
        private readonly ICurrencyEvent _currencyEvent;
        private readonly ICurrencyAdminEvent _currencyAdminEvent;
        private readonly ICurrencyService _currencyService;
        private readonly ICurrencyTypeEvent _currencyTypeEvent;
        private readonly ISourceEvent _sourceEvent;


        public CurrencyController(ILogger<CurrencyController> logger, NozomiSignInManager signInManager,
            NozomiUserManager userManager, ICurrencyEvent currencyEvent, ICurrencyAdminEvent currencyAdminEvent,
            ICurrencyService currencyService, ICurrencyTypeEvent currencyTypeEvent, ISourceEvent sourceEvent)
            : base(logger, signInManager, userManager)
        {
            _currencyEvent = currencyEvent;
            _currencyAdminEvent = currencyAdminEvent;
            _currencyService = currencyService;
            _currencyTypeEvent = currencyTypeEvent;
            _sourceEvent = sourceEvent;
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
                Currencies = _currencyEvent.GetAllDTO(),
                CurrencyTypes = _currencyTypeEvent.GetAllActive(),
                CurrencySources = _sourceEvent.GetAllActive()
            };
            
            return View(vm);
        }
        
        #endregion

        #region Get Currency(id)
        
        [HttpGet("{abbreviation}")]
        public IActionResult Currency([FromRoute] string abbreviation)
        {
            var currency = _currencyAdminEvent.GetCurrencyByAbbreviation(abbreviation);
            var vm = new CurrencyViewModel
            {
                Currency = currency,
                CurrencySourcesOptions = _sourceEvent.GetAllCurrencySourceOptions(currency.CurrencySources)
            };

            return View(vm);
        }
        #endregion
        
        #region PUT EditCurrency
        

        [HttpPut("{id}")]
        public async Task<IActionResult> EditCurrency(long id, UpdateCurrency updateCurrency)
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

            if (_currencyService.Update(updateCurrency).ResultType.Equals(NozomiResultType.Success)) return Ok();

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

            var result = _currencyService.Create((createCurrency));
            
            if (result.ResultType.Equals(NozomiResultType.Success)) return Ok(result);

            // Create failed
            return NotFound();
        }
        
        #endregion
        
        #region POST AddCurrencySource

        [HttpPost]
        public async Task<IActionResult> CreateCurrencySource(CurrencySource currencySource)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var result = _currencyService.CreateCurrencySource(currencySource);
            
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

            if(_currencyService.Delete(id).ResultType.Equals(NozomiResultType.Success)) return Ok();

            // Update failed.
            return NotFound();
        }

        #endregion

    }
}
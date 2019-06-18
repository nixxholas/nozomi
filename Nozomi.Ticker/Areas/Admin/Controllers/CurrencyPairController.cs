using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Identity.ViewModels.Manage.CurrencyPair;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyPair;
using Nozomi.Preprocessing;
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
        private readonly ICurrencyEvent _currencyEvent;
        private readonly ISourceEvent _sourceEvent;

        public CurrencyPairController(ILogger<CurrencyPairController> logger, NozomiSignInManager signInManager,
            NozomiUserManager userManager, ICurrencyPairEvent currencyPairEvent,
            ICurrencyPairService currencyPairService, ICurrencyEvent currencyEvent, ISourceEvent sourceEvent)
            : base(logger, signInManager, userManager)
        {
            _currencyPairEvent = currencyPairEvent;
            _currencyPairService = currencyPairService;
            _currencyEvent = currencyEvent;
            _sourceEvent = sourceEvent;
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

            var vm = new CurrencyPairViewModel
            {
                CurrencyPairs = _currencyPairEvent.GetAll(),
                Currencies = _currencyEvent.GetAllActive(),
                Sources = _sourceEvent.GetAllActive(),
                CurrencyPairTypes = NozomiServiceConstants.currencyPairType
            };

            return View(vm);
        }

        #endregion
        
        #region Post CurrencyPairs
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCurrencyPair createCurrencyPair)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
                return BadRequest(new NozomiResult<string>(NozomiResultType.Failed, "Invalid payload."));

            var result = _currencyPairService.Create(createCurrencyPair);

            if (result.ResultType.Equals(NozomiResultType.Success)) return Ok(result);

            // Create failed
            return BadRequest(new NozomiResult<string>(NozomiResultType.Failed, "Invalid payload."));
        }
        
        #endregion
        
        #region Update Currency Pair
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] UpdateCurrencyPair updateCurrencyPair, long id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
                return BadRequest(new NozomiResult<string>(NozomiResultType.Failed, "Invalid payload."));

            var result = _currencyPairService.Update(updateCurrencyPair);

            if (result.ResultType.Equals(NozomiResultType.Success)) return Ok(result);

            // Create failed
            return BadRequest(new NozomiResult<string>(NozomiResultType.Failed, "Invalid payload."));
        }
        
        #endregion
        
    }
}
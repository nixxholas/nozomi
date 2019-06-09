using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.Models.Currency;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Ticker.Controllers;

namespace Nozomi.Ticker.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Owner, Administrator, Staff")]
    public class CurrencySourceController : AreaBaseViewController<CurrencySourceController>
    {
        private readonly ICurrencySourceService _currencySourceService;

        public CurrencySourceController(ILogger<CurrencySourceController> logger, NozomiSignInManager signInManager,
            NozomiUserManager userManager, ICurrencySourceService currencySourceService) 
            : base(logger, signInManager, userManager)
        {
            _currencySourceService = currencySourceService;
        }
        
        #region POST Create

        [HttpPost]
        public async Task<IActionResult> Create(CurrencySource currencySource)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var result = _currencySourceService.Create(currencySource);

            if (result.ResultType.Equals(NozomiResultType.Success)) return Ok(result);

            // Create failed
            return NotFound();
        }

        #endregion
        #region Delete Currency Source

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var result = _currencySourceService.Delete(id);

            if (result.ResultType.Equals(NozomiResultType.Success)) return Ok(result);

            // Create failed
            return NotFound();
        }

        #endregion
    }
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Models;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencySource;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Ticker.Controllers;

namespace Nozomi.Ticker.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Owner, Administrator, Staff")]
    public class CurrencySourceController : AreaBaseViewController<CurrencySourceController>
    {
        private readonly ICurrencySourceService _currencySourceService;

        public CurrencySourceController(ILogger<CurrencySourceController> logger, 
            ICurrencySourceService currencySourceService, SignInManager<User> signInManager,
            UserManager<User> userManager)
            : base(logger, signInManager, userManager)
        {
            _currencySourceService = currencySourceService;
        }
        
        #region POST Create

        [HttpPost]
        public async Task<IActionResult> Create(CreateCurrencySource currencySource)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
                return BadRequest("Please submit proper request data.");

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
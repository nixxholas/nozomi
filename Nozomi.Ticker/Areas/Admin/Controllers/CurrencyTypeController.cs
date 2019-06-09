using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.Models.Currency;
using Nozomi.Infra.Admin.Service.Events.Interfaces;
using Nozomi.Infra.Admin.Service.Services.Interfaces;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Ticker.Controllers;

namespace Nozomi.Ticker.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Owner, Administrator, Staff")]
    public class CurrencyTypeController : AreaBaseViewController<CurrencyTypeController>
    {
        private readonly ICurrencyTypeAdminEvent _currencyTypeAdminEvent;
        private readonly ICurrencyTypeService _currencyTypeService;

        public CurrencyTypeController(ILogger<CurrencyTypeController> logger, NozomiSignInManager signInManager,
            NozomiUserManager userManager, ICurrencyTypeAdminEvent currencyTypeAdminEvent, 
            ICurrencyTypeService currencyTypeService) 
            : base(logger, signInManager, userManager)
        {
            _currencyTypeAdminEvent = currencyTypeAdminEvent;
            _currencyTypeService = currencyTypeService;
        }
        
        #region View all CurrencyTypes

        [HttpGet]
        public IActionResult CurrencyTypes()
        {
            return View(_currencyTypeAdminEvent.GetAll());
        }
         
        #endregion
        
        #region View CurrencyType

        [HttpGet("{id}")]
        public IActionResult Edit(long id)
        {
            return View(_currencyTypeAdminEvent.Get(id));
        }
        
        #endregion

        [HttpPut]
        public async Task<IActionResult> Update(CurrencyType currencyType)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var result = _currencyTypeService.Create(currencyType);
            
            return Edit(currencyType.Id);
        }
        
        #region POST Create

        [HttpPost]
        public async Task<IActionResult> Create(CurrencyType currencyType)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var result = _currencyTypeService.Create(currencyType);

            if (result > 0) return Ok(result);

            // Create failed
            return NotFound();
        }

        #endregion

        #region Delete

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // TODO: Make full use of the hard delete option.
            var result = _currencyTypeService.Delete(id, false, user.Id);

            if (result) return Ok("Currency Type successfully deleted!");

            // Create failed
            return BadRequest("Invalid Currency Type.");
        }

        #endregion
    }
}
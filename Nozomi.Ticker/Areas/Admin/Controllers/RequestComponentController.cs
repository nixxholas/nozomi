using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Models;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyPairComponent;
using Nozomi.Data.AreaModels.v1.RequestComponent;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Ticker.Controllers;

namespace Nozomi.Ticker.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Owner, Administrator, Staff")]
    public class RequestComponentController : AreaBaseViewController<RequestComponentController>
    {
        private readonly IComponentService _componentService;
        
        public RequestComponentController(ILogger<RequestComponentController> logger, 
            IComponentService componentService, SignInManager<User> signInManager,
            UserManager<User> userManager)
            : base(logger, signInManager, userManager)
        {
            _componentService = componentService;
        }
        
        #region POST RequestComponent

        [HttpPost]
        public async Task<IActionResult> CreateRequestComponent(CreateRequestComponent createRequestComponent)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user withID '{_userManager.GetUserId(User)}'.");
            }

            var result = _componentService.Create(createRequestComponent);
            
            if (result.ResultType.Equals(NozomiResultType.Success)) return Ok(result);

            return NotFound();
        }
        
        #endregion
        
        #region PUT RequestComponent

        [HttpPut("{id}")]
        public async Task<IActionResult> EditRequestComponent(long id, UpdateRequestComponent updateRequestComponent)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user withID '{_userManager.GetUserId(User)}'.");
            }

            if (id != updateRequestComponent.Id)
            {
                return BadRequest();
            }

            var result = _componentService.Update(updateRequestComponent);
            
            if (result.ResultType.Equals(NozomiResultType.Success)) return Ok(result);

            return NotFound();
        }
        
        #endregion
        
        #region DELETE RequestComponent

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequestComponent(long id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user withID '{_userManager.GetUserId(User)}'.");
            }

            var result = _componentService.Delete(id);
            
            if (result.ResultType.Equals(NozomiResultType.Success)) return Ok(result);

            return NotFound();
        }
        
        #endregion
    }
}
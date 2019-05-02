using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data.AreaModels.v1.RequestComponent;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Identity.Managers;
using Nozomi.Ticker.Controllers;

namespace Nozomi.Ticker.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[controller]/manage/[action]")]
    [Authorize(Roles = "Owner, Administrator, Staff")]
    public class RequestComponentController : BaseViewController<RequestComponentController>
    {
        private readonly IRequestEvent _requestEvent;
        
        public RequestComponentController(ILogger<RequestComponentController> logger, NozomiSignInManager signInManager,
            NozomiUserManager userManager, IRequestEvent requestEvent)
            : base(logger, signInManager, userManager)
        {
            _requestEvent = requestEvent;
        }
        
        /// <summary>
        /// Allows you to create a request component relative to the request.
        /// </summary>
        /// <param name="id">Request Id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> CreateRequestComponent(long id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var req = _requestEvent.GetActive(id);
            
            if (req == null)
                ModelState.AddModelError("InvalidRequestException", "Invalid Request.");

            return View(new CreateRequestComponent
            {
                RequestId = req.Id
            });
        }
        
    }
}
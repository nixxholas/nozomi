using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data.ViewModels.Manage;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Identity.Managers;
using Nozomi.Ticker.Controllers;

namespace Nozomi.Ticker.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[controller]/manage/[action]")]
    [Authorize(Roles = "Owner, Administrator, Staff")]
    public class RequestController : BaseViewController<RequestController>
    {
        private readonly IRequestEvent _requestEvent;
        
        public RequestController(ILogger<RequestController> logger, NozomiSignInManager signInManager,
            NozomiUserManager userManager, IRequestEvent requestEvent)
            : base(logger, signInManager, userManager)
        {
            _requestEvent = requestEvent;
        }
        
        [HttpGet]
        public async Task<IActionResult> Requests()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            
            return View();
        }

        [HttpGet("{guid}")]
        public async Task<IActionResult> Request([FromRoute]Guid guid)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var req = _requestEvent.GetByGuid(guid, true);
            
            return View(new RequestViewModel
            {
                Request = req.ToDTO()
            });
        }
    }
}
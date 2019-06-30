using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Identity.ViewModels.Manage.Tickers;
using Nozomi.Data;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Ticker.Controllers;

namespace Nozomi.Ticker.Areas.Admin.Controllers
{
    
    [Area("Admin")]
    [Authorize(Roles = "Owner, Administrator, Staff")]
    public class TickerController : AreaBaseViewController<TickerController>
    {
        private readonly ITickerService _tickerService;
        
        public TickerController(ILogger<TickerController> logger, NozomiSignInManager signInManager,
            NozomiUserManager userManager, ITickerService tickerService)
            : base(logger, signInManager, userManager)
        {
            _tickerService = tickerService;
        }
        
        [HttpGet]
        [Authorize(Roles = "Owner, Administrator, Staff")]
        public async Task<IActionResult> Create()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var vm = new CreateTickerViewModel();

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTickerInputModel vm)
        {   
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // If it works
            if (ModelState.IsValid)
            {
                var res = _tickerService.Create(vm);

                if (res.ResultType.Equals(NozomiResultType.Success))
                {
                    return Ok();
                }
                else
                {
                    
                }
            }

            // TODO: Implementation of error messages
            vm.StatusMessage = "There was something erroneous with your submission.";
            return RedirectToAction("Create");
        }
    }
}
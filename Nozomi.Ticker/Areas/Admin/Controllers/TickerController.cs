using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Admin.Domain.AreaModels.Tickers;
using Nozomi.Base.Auth.Models;
using Nozomi.Data;
using Nozomi.Infra.Admin.Service.Services.Interfaces;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Ticker.Controllers;

namespace Nozomi.Ticker.Areas.Admin.Controllers
{
    
    [Area("Admin")]
    [Authorize(Roles = "Owner, Administrator, Staff")]
    public class TickerController : AreaBaseViewController<TickerController>
    {
        private readonly ITickerService _tickerService;
        
        public TickerController(ILogger<TickerController> logger, SignInManager<User> signInManager,
            UserManager<User> userManager, ITickerService tickerService)
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
                    return View();
                }
                else
                {
                    return View(new CreateTickerViewModel(vm));
                }
            }

            // TODO: Implementation of error messages
            vm.StatusMessage = "There was something erroneous with your submission.";
            return View(new CreateTickerViewModel(vm));
        }
    }
}
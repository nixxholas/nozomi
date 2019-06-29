using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Infra.Admin.Service.Services.Interfaces;
using Nozomi.Service.Identity.Managers;
using Nozomi.Ticker.Controllers;

namespace Nozomi.Ticker.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Owner, Administrator, Staff")]
    public class ExchangeController : AreaBaseViewController<ExchangeController>
    {
        private readonly IExchangeService _exchangeService;
        
        public ExchangeController(IExchangeService exchangeService,
            ILogger<ExchangeController> logger, NozomiSignInManager signInManager, 
            NozomiUserManager userManager) 
            : base(logger, signInManager, userManager)
        {
            _exchangeService = exchangeService;
        }
    }
}
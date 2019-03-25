using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Identity.Managers;

namespace Nozomi.Ticker.Areas
{
    public class CurrencyController : BaseViewController<CurrencyController>
    {
        private ICurrencyEvent _currencyEvent { get; set; }
        
        public CurrencyController(ILogger<CurrencyController> logger, NozomiSignInManager signInManager, 
            NozomiUserManager userManager, ICurrencyEvent currencyEvent) 
            : base(logger, signInManager, userManager)
        {
            _currencyEvent = currencyEvent;
        }

        [Route("{controller}/{abbrv}")]
        public IActionResult View(string abbrv)
        {
            // First obtain all 'ABBRV' objects first, 
            var currency = _currencyEvent.Get(abbrv, true);
            
            // DTO this sitch
            
            return View();
        }
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Models;
using Nozomi.Service.Events.Analysis.Interfaces;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Ticker.Controllers;

namespace Nozomi.Ticker.Areas.Users.Controllers
{
    [Area("Users")]
    public class CurrencyController : BaseViewController<CurrencyController>
    {
        private IAnalysedHistoricItemEvent _analysedHistoricItemEvent;
        private ICurrencyEvent _currencyEvent;
        
        public CurrencyController(ILogger<CurrencyController> logger, SignInManager<User> signInManager, 
            UserManager<User> userManager, IAnalysedHistoricItemEvent analysedHistoricItemEvent, 
            ICurrencyEvent currencyEvent) 
            : base(logger, signInManager, userManager)
        {
            _analysedHistoricItemEvent = analysedHistoricItemEvent;
            _currencyEvent = currencyEvent;
        }

        [HttpGet("/[controller]/{abbrv}")]
        public IActionResult View(string abbrv)
        {
            var currency = _currencyEvent.GetCurrencyByAbbreviation(abbrv, true);

            if (currency != null)
            {
                return View(currency);
            }
            
            ViewData["Error"] = "Invalid currency.";
            
            return RedirectToAction("Index", "Home");
        }
    }
}
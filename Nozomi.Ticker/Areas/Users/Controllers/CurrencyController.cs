using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Infra.Analysis.Service.Events.Analysis.Interfaces;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Identity.Managers;
using Nozomi.Ticker.Controllers;

namespace Nozomi.Ticker.Areas.Users.Controllers
{
    [Area("Users")]
    public class CurrencyController : BaseViewController<CurrencyController>
    {
        private IAnalysedHistoricItemEvent _analysedHistoricItemEvent;
        private ICurrencyEvent _currencyEvent;
        
        public CurrencyController(ILogger<CurrencyController> logger, NozomiSignInManager signInManager, 
            NozomiUserManager userManager, IAnalysedHistoricItemEvent analysedHistoricItemEvent, 
            ICurrencyEvent currencyEvent) 
            : base(logger, signInManager, userManager)
        {
            _analysedHistoricItemEvent = analysedHistoricItemEvent;
            _currencyEvent = currencyEvent;
        }

        [HttpGet("/[controller]/{abbrv}")]
        public IActionResult View(string abbrv)
        {
            var currency = _currencyEvent.GetCurrencyByAbbreviation(abbrv);

            if (currency != null)
            {
                if (currency.AnalysedComponents.Any(ac =>
                    ac.ComponentType.Equals(AnalysedComponentType.HourlyAveragePrice)))
                {
                    var aComp = currency.AnalysedComponents
                        .SingleOrDefault(ac => ac.ComponentType.Equals(AnalysedComponentType.HourlyAveragePrice));

                    aComp.AnalysedHistoricItems = _analysedHistoricItemEvent.GetAll(aComp.Id, TimeSpan.FromHours(72));
                }
                
                return View(currency);
            }
            
            ViewData["Error"] = "Invalid currency.";
            
            return RedirectToAction("Index", "Home");
        }
    }
}
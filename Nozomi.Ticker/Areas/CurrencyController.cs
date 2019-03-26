using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Nozomi.Data.ResponseModels.Currency;
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

            if (currency != null && currency.Count > 0)
            {
                var result = new AbbrvUniqueCurrencyResponse(currency.FirstOrDefault());
                
                // DTO this sitch
                foreach (var similarCurr in currency)
                {
                    foreach (var aComp in similarCurr.AnalysedComponents)
                    {
                        if (!result.AnalysedComponents
                            .Any(ac => ac.Id.Equals(aComp.Id)))
                        {
                            result.AnalysedComponents.Add(aComp);
                        }
                    }

                    foreach (var cReq in similarCurr.CurrencyRequests)
                    {
                        if (!result.CurrencyRequests.Any(cr => cr.Id.Equals(cReq.Id)))
                        {
                            result.CurrencyRequests.Add(cReq);
                        }
                    }

                    foreach (var pCPair in similarCurr.PartialCurrencyPairs)
                    {
                        if (!result.PartialCurrencyPairs.Any(pcp => pcp.IsMain.Equals(pCPair.IsMain)
                                                                    && pcp.CurrencyId.Equals(pCPair.CurrencyId)
                                                                    && pcp.CurrencyPairId.Equals(pCPair.CurrencyPairId))
                        )
                        {
                            result.PartialCurrencyPairs.Add(pCPair);
                        }
                    }
                }

                return View(result);
            }
            
            return RedirectToAction("Index", "Home");
        }
    }
}
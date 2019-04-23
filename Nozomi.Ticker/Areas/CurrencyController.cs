using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.ResponseModels.Currency;
using Nozomi.Data.ResponseModels.PartialCurrencyPair;
using Nozomi.Data.ResponseModels.Source;
using Nozomi.Infra.Analysis.Service.Events.Analysis.Interfaces;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Identity.Managers;

namespace Nozomi.Ticker.Areas
{
    public class CurrencyController : BaseViewController<CurrencyController>
    {
        private IAnalysedHistoricItemEvent _analysedHistoricItemEvent;
        private ICurrencyEvent _currencyEvent;
        private IPartialCurrencyPairEvent _partialCurrencyPairEvent;
        
        public CurrencyController(ILogger<CurrencyController> logger, NozomiSignInManager signInManager, 
            NozomiUserManager userManager, IAnalysedHistoricItemEvent analysedHistoricItemEvent, 
            ICurrencyEvent currencyEvent, IPartialCurrencyPairEvent partialCurrencyPairEvent) 
            : base(logger, signInManager, userManager)
        {
            _analysedHistoricItemEvent = analysedHistoricItemEvent;
            _currencyEvent = currencyEvent;
            _partialCurrencyPairEvent = partialCurrencyPairEvent;
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
                            if (aComp.ComponentType.Equals(AnalysedComponentType.HourlyAveragePrice))
                            {
                                _analysedHistoricItemEvent.GetAll(aComp.Id, TimeSpan.FromDays(3));
                            }
                            
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

                    foreach (var pCPair in similarCurr.PartialCurrencyPairs
                        .Where(pcp => pcp.IsMain))
                    {
                        // Form the TickerPair abbreviation first
                        var tickerPairStr = pCPair.Currency.Abbrv +
                                            similarCurr.PartialCurrencyPairs
                                                .SingleOrDefault(pcp => !pcp.IsMain
                                                                        && pcp.CurrencyPairId.Equals(
                                                                            pCPair.CurrencyPairId))
                                                ?.Currency.Abbrv;

                        // Make sure tickerPairStr is not a solo piece of shit
                        if (!tickerPairStr.Equals(pCPair.Currency.Abbrv, 
                            StringComparison.InvariantCultureIgnoreCase))
                        {
                            // Make sure this is a main currency and that the TickerPairs collection
                            // does not contain this TickerPair yet.
                            if (!result.TickerPairs.Any(tp => tp.PairAbbreviation.Equals(tickerPairStr,
                                StringComparison.InvariantCultureIgnoreCase)))
                            {
                                // Since all is good, let's toss it in
                                result.TickerPairs.Add(new CondensedTickerPair
                                {
                                    PairAbbreviation = tickerPairStr,
                                    Sources = new List<SourceResponse>
                                    {
                                        new SourceResponse
                                        {
                                            Abbreviation = similarCurr.CurrencySource.Abbreviation,
                                            Name = similarCurr.CurrencySource.Name
                                        }
                                    }
                                });
                            }
                            // Since the TickerPair already exists,
                            else
                            {
                                // Pull the TickerPair
                                var tickerPair = result.TickerPairs.SingleOrDefault(tp =>
                                    tp.PairAbbreviation.Equals(tickerPairStr,
                                        StringComparison.InvariantCultureIgnoreCase));
                                
                                // Check if the source exists first
                                if (tickerPair.Sources.Any(s => s.Abbreviation
                                    .Equals(similarCurr.CurrencySource.Abbreviation,
                                        StringComparison.InvariantCultureIgnoreCase)))
                                {
                                    // It does, so let's ignore.
                                    // If it reaches here it means that we have a duplicate.
                                }
                                else
                                {
                                    // Does not, add it in.
                                    tickerPair.Sources.Add(new SourceResponse
                                    {
                                        Abbreviation = similarCurr.Abbrv,
                                        Name = similarCurr.Name
                                    });
                                }
                            }
                        }
                    }

                    var counterPCPs = _partialCurrencyPairEvent.ObtainCounterPCPs(similarCurr.PartialCurrencyPairs);

                    if (counterPCPs != null && counterPCPs.Count > 0)
                    {
                        foreach (var pCPair in similarCurr.PartialCurrencyPairs)
                        {
                            var tickerPairStr = pCPair.Currency.Abbrv +
                                                counterPCPs
                                                    .SingleOrDefault(pcp =>
                                                        pcp.CurrencyPairId.Equals(pCPair.CurrencyPairId))
                                                    ?.Currency.Abbrv;
                            
                            if (!result.TickerPairs.Any(tp => tp.PairAbbreviation
                                    .Equals(tickerPairStr, StringComparison.InvariantCultureIgnoreCase)))
                            {
                                result.TickerPairs.Add(new CondensedTickerPair
                                {
                                    PairAbbreviation = tickerPairStr,
                                    Sources = new List<SourceResponse>
                                    {
                                        new SourceResponse
                                        {
                                            Abbreviation = similarCurr.CurrencySource.Abbreviation,
                                            Name = similarCurr.CurrencySource.Name
                                        }
                                    }
                                });
                            }
                            else
                            {
                                // Add since it exists
                                result.TickerPairs.SingleOrDefault(tp => tp.PairAbbreviation
                                    .Equals(tickerPairStr, StringComparison.InvariantCultureIgnoreCase)
                                    && !tp.Sources.Any(s => s.Abbreviation
                                        .Equals(similarCurr.CurrencySource.Abbreviation, 
                                            StringComparison.InvariantCultureIgnoreCase)))
                                    ?.Sources.Add(new SourceResponse
                                    {
                                        Abbreviation = similarCurr.CurrencySource.Abbreviation,
                                        Name = similarCurr.CurrencySource.Name
                                    });
                            }
                        }
                    }

                    if (!result.CurrencySources.Any(cs => cs.Id.Equals(similarCurr.CurrencySourceId)))
                    {
                        result.CurrencySources.Add(similarCurr.CurrencySource);
                    }
                }

                return View(result);
            }
            
            return RedirectToAction("Index", "Home");
        }
    }
}
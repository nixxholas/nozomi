using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core;
using Nozomi.Base.Core.Helpers.Enumerable;
using Nozomi.Data.AreaModels.v1.Currency;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.Models.Web.Websocket;
using Nozomi.Data.ResponseModels.Currency;
using Nozomi.Data.ResponseModels.PartialCurrencyPair;
using Nozomi.Data.ResponseModels.Source;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class CurrencyEvent : BaseEvent<CurrencyEvent, NozomiDbContext>, ICurrencyEvent
    {
        private readonly ICurrencyPairEvent _currencyPairEvent;
        private readonly ICurrencyCurrencyPairEvent _currencyCurrencyPairEvent;

        public CurrencyEvent(ILogger<CurrencyEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork,
            ICurrencyPairEvent currencyPairEvent, ICurrencyCurrencyPairEvent currencyCurrencyPairEvent)
            : base(logger, unitOfWork)
        {
            _currencyPairEvent = currencyPairEvent;
            _currencyCurrencyPairEvent = currencyCurrencyPairEvent;
        }

        public Currency Get(long id, bool track = false)
        {
            var query = _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking();

            if (track)
            {
                query = query
                    .Include(c => c.AnalysedComponents)
                    .Include(c => c.CurrencySources)
                    .ThenInclude(cs => cs.Source)
                    .Include(c => c.CurrencyPairSourceCurrencies)
                    .ThenInclude(pcp => pcp.CurrencyPair)
                    .Include(c => c.CurrencyRequests)
                    .ThenInclude(cr => cr.RequestComponents)
                    .Include(c => c.CurrencyRequests)
                    .ThenInclude(cr => cr.AnalysedComponents);
            }

            return query
                .SingleOrDefault(c => c.Id.Equals(id));
        }

        /// <summary>
        /// Public API for obtaining data specific to a currency with its abbreviation.
        /// </summary>
        /// <param name="abbreviation"></param>
        /// <returns></returns>
        public Currency GetCurrencyByAbbreviation(string abbreviation, bool track = false)
        {
            // First obtain all 'ABBRV' objects first, 
//            var currency = _unitOfWork.GetRepository<Currency>()
//                .GetQueryable()
//                .AsNoTracking()
//                .Where(c => c.Abbreviation.Equals(abbreviation, StringComparison.InvariantCultureIgnoreCase)
//                            && c.DeletedAt == null && c.IsEnabled)
//                .Include(c => c.AnalysedComponents)
//                .Include(c => c.CurrencySource)
//                .Include(c => c.CurrencyPairSourceCurrencies)
//                .ThenInclude(pcp => pcp.Currency)
//                .Include(c => c.CurrencyCurrencyPairs)
//                .ThenInclude(pcp => pcp.CurrencyPair)
//                .ThenInclude(cp => cp.CurrencyPairRequests)
//                .ThenInclude(cpr => cpr.AnalysedComponents)
//                .Include(c => c.CurrencyCurrencyPairs)
//                .ThenInclude(pcp => pcp.CurrencyPair)
//                .ThenInclude(cp => cp.CurrencyPairRequests)
//                .ThenInclude(cpr => cpr.RequestComponents)
//                .Include(c => c.CurrencyRequests)
//                .ThenInclude(cr => cr.RequestComponents)
//                .ToList();
//
//            // Filter it
//            var cpACs = currency
//                .SelectMany(c => c.CurrencyCurrencyPairs)
//                .Select(pcp => pcp.CurrencyPair)
//                .Where(cp => cp.CounterCurrency.Equals(CoreConstants.GenericCounterCurrency,
//                    StringComparison.InvariantCultureIgnoreCase))
//                .SelectMany(cp => cp.CurrencyPairRequests)
//                .SelectMany(cpr => cpr.AnalysedComponents
//                    .Where(ac => ac.DeletedAt == null && ac.IsEnabled))
//                .ToList();
//
//            if (currency.Count > 0)
//            {
//                var result = new AbbrvUniqueCurrencyResponse(currency.FirstOrDefault());
//
//                // DTO this sitch
//                foreach (var similarCurr in currency)
//                {
//                    // Currency-based ACs
//                    foreach (var aComp in similarCurr.AnalysedComponents)
//                    {
//                        if (!result.AnalysedComponents
//                            .Any(ac => ac.Id.Equals(aComp.Id)))
//                        {
//                            result.AnalysedComponents.Add(aComp);
//                        }
//                    }
//
//                    // Currency Pair-based ACs
//                    foreach (var cpAComp in cpACs)
//                    {
//                        if (!result.AnalysedComponents
//                            .Any(ac => ac.Id.Equals(cpAComp.Id)))
//                        {
//                            result.AnalysedComponents.Add(cpAComp);
//                        }
//                    }
//
//                    foreach (var cReq in similarCurr.CurrencyRequests)
//                    {
//                        if (!result.CurrencyRequests.Any(cr => cr.Id.Equals(cReq.Id)))
//                        {
//                            result.CurrencyRequests.Add(cReq);
//                        }
//                    }
//
//                    foreach (var pCPair in similarCurr.CurrencyCurrencyPairs
//                        .Where(ccp => ccp.Currency.Abbreviation
//                            .Equals(ccp.CurrencyPair.MainCurrency, StringComparison.InvariantCultureIgnoreCase)))
//                    {
//                        // Form the TickerPair abbreviation first
//                        var tickerPairStr = pCPair.Currency.Abbreviation +
//                                            similarCurr.CurrencyCurrencyPairs
//                                                .SingleOrDefault(ccp => ccp.Currency.Abbreviation
//                                                                            .Equals(ccp.CurrencyPair.CounterCurrency,
//                                                                                StringComparison
//                                                                                    .InvariantCultureIgnoreCase)
//                                                                        && ccp.CurrencyPairId.Equals(
//                                                                            pCPair.CurrencyPairId))
//                                                ?.Currency.Abbreviation;
//
//                        // Make sure tickerPairStr is not a solo piece of shit
//                        if (!tickerPairStr.Equals(pCPair.Currency.Abbreviation,
//                            StringComparison.InvariantCultureIgnoreCase))
//                        {
//                            // Make sure this is a main currency and that the TickerPairs collection
//                            // does not contain this TickerPair yet.
//                            if (!result.TickerPairs.Any(tp => tp.PairAbbreviation.Equals(tickerPairStr,
//                                StringComparison.InvariantCultureIgnoreCase)))
//                            {
//                                // Since all is good, let's toss it in
//                                result.TickerPairs.Add(new CondensedTickerPair
//                                {
//                                    PairAbbreviation = tickerPairStr,
//                                    Sources = new List<SourceResponse>
//                                    {
//                                        new SourceResponse
//                                        {
//                                            Abbreviation = similarCurr.CurrencySource.Abbreviation,
//                                            Name = similarCurr.CurrencySource.Name
//                                        }
//                                    }
//                                });
//                            }
//                            // Since the TickerPair already exists,
//                            else
//                            {
//                                // Pull the TickerPair
//                                var tickerPair = result.TickerPairs.SingleOrDefault(tp =>
//                                    tp.PairAbbreviation.Equals(tickerPairStr,
//                                        StringComparison.InvariantCultureIgnoreCase));
//
//                                // Check if the source exists first
//                                if (tickerPair.Sources.Any(s => s.Abbreviation
//                                    .Equals(similarCurr.CurrencySource.Abbreviation,
//                                        StringComparison.InvariantCultureIgnoreCase)))
//                                {
//                                    // It does, so let's ignore.
//                                    // If it reaches here it means that we have a duplicate.
//                                }
//                                else
//                                {
//                                    // Does not, add it in.
//                                    tickerPair.Sources.Add(new SourceResponse
//                                    {
//                                        Abbreviation = similarCurr.Abbreviation,
//                                        Name = similarCurr.Name
//                                    });
//                                }
//                            }
//                        }
//                    }
//
//                    var counterPCPs = _currencyCurrencyPairEvent
//                        .ObtainCounterCurrencyPairs(similarCurr.CurrencyCurrencyPairs);
//
//                    if (counterPCPs != null && counterPCPs.Count > 0)
//                    {
//                        foreach (var pCPair in similarCurr.CurrencyCurrencyPairs)
//                        {
//                            var tickerPairStr = pCPair.Currency.Abbreviation +
//                                                counterPCPs
//                                                    .SingleOrDefault(pcp =>
//                                                        pcp.CurrencyPairId.Equals(pCPair.CurrencyPairId))
//                                                    ?.Currency.Abbreviation;
//
//                            if (!result.TickerPairs.Any(tp => tp.PairAbbreviation
//                                .Equals(tickerPairStr, StringComparison.InvariantCultureIgnoreCase)))
//                            {
//                                result.TickerPairs.Add(new CondensedTickerPair
//                                {
//                                    PairAbbreviation = tickerPairStr,
//                                    Sources = new List<SourceResponse>
//                                    {
//                                        new SourceResponse
//                                        {
//                                            Abbreviation = similarCurr.CurrencySource.Abbreviation,
//                                            Name = similarCurr.CurrencySource.Name
//                                        }
//                                    }
//                                });
//                            }
//                            else
//                            {
//                                // Add since it exists
//                                result.TickerPairs.SingleOrDefault(tp => tp.PairAbbreviation
//                                                                             .Equals(tickerPairStr,
//                                                                                 StringComparison
//                                                                                     .InvariantCultureIgnoreCase)
//                                                                         && !tp.Sources.Any(s => s.Abbreviation
//                                                                             .Equals(
//                                                                                 similarCurr.CurrencySource
//                                                                                     .Abbreviation,
//                                                                                 StringComparison
//                                                                                     .InvariantCultureIgnoreCase)))
//                                    ?.Sources.Add(new SourceResponse
//                                    {
//                                        Abbreviation = similarCurr.CurrencySource.Abbreviation,
//                                        Name = similarCurr.CurrencySource.Name
//                                    });
//                            }
//                        }
//                    }
//
//                    if (!result.CurrencySources.Any(cs => cs.Id.Equals(similarCurr.CurrencySourceId)))
//                    {
//                        result.CurrencySources.Add(similarCurr.CurrencySource);
//                    }
//                }
//
//                return result;
//            }

            var query = _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking();

            if (track)
            {
                query = query.Include(c => c.AnalysedComponents)
                    .Include(c => c.CurrencyPairSourceCurrencies)
                    .ThenInclude(c => c.CurrencySource)
                    .Include(c => c.CurrencyRequests)
                    .ThenInclude(cr => cr.RequestComponents);
            }

            return query
                .SingleOrDefault(c => c.Abbreviation.Equals(abbreviation, StringComparison.InvariantCultureIgnoreCase));
        }

        public decimal GetCirculatingSupply(AnalysedComponent analysedComponent)
        {
            // If its a currency-based ac
            if (analysedComponent.CurrencyId > 0)
            {
                // Obtain the currency that is required
                var curr = _unitOfWork.GetRepository<Currency>()
                    .GetQueryable()
                    .AsNoTracking()
                    .SingleOrDefault(c => c.Id.Equals(analysedComponent.CurrencyId)
                                          && c.IsEnabled && c.DeletedAt == null);

                // Safetynet
                if (curr == null)
                {
                    return decimal.MinusOne;
                }

                // Denomination safetynet
                if (curr.Denominations <= 0)
                {
                    curr.Denominations = 1; // Neutraliser
                }

                // Then, we obtain the circulating supply.

                // TODO: Validate with multiple sources.

#if DEBUG
                var testCResQ = _unitOfWork.GetRepository<CurrencyRequest>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                    .Include(cp => cp.Currency)
                    // Where the partial currency pair's main currency is equal to the currency that is required
                    .Where(cr => cr.Currency.Abbreviation.Equals(curr.Abbreviation, StringComparison.InvariantCultureIgnoreCase))
                    .Include(cpr => cpr.RequestComponents)
                    // Null checks
                    .Where(cpr => cpr.IsEnabled && cpr.DeletedAt == null
                                                && cpr.RequestComponents
                                                    .Any(rc => rc.DeletedAt == null && rc.IsEnabled
                                                                                    && !string.IsNullOrEmpty(rc.Value)))
                    // Obtain only the circulating supply
                    .SelectMany(cpr => cpr.RequestComponents
                        .Where(rc => rc.ComponentType.Equals(ComponentType.Circulating_Supply)))
                    .FirstOrDefault();
#endif
                var reqComp = _unitOfWork.GetRepository<CurrencyRequest>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                    .Include(cp => cp.Currency)
                    // Where the partial currency pair's main currency is equal to the currency that is required
                    .Where(cr => cr.Currency.Abbreviation.Equals(curr.Abbreviation,
                        StringComparison.InvariantCultureIgnoreCase))
                    .Include(cpr => cpr.RequestComponents)
                    // Null checks
                    .Where(cpr => cpr.IsEnabled && cpr.DeletedAt == null
                                                && cpr.RequestComponents
                                                    .Any(rc =>
                                                        rc.DeletedAt == null && rc.IsEnabled
                                                                             && !string.IsNullOrEmpty(rc.Value)))
                    // Obtain only the circulating supply
                    .SelectMany(cpr => cpr.RequestComponents
                        .Where(rc =>
                            rc.ComponentType.Equals(ComponentType.Circulating_Supply)))
                    .FirstOrDefault();

                return reqComp != null
                    ? decimal.Parse(reqComp.Value ?? "0") /
                      (reqComp.IsDenominated ? (decimal) Math.Pow(10, curr.Denominations) : decimal.One)
                    : decimal.Zero;
            }
            else
                // It means that this request 
            {
                // Request-based currency obtaining
                var currencyCPR = _unitOfWork.GetRepository<CurrencyPairRequest>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(cpr => cpr.Id.Equals(analysedComponent.RequestId)
                                  && cpr.DeletedAt == null && cpr.IsEnabled)
                    .Include(cpr => cpr.CurrencyPair)
                    .ThenInclude(cp => cp.CurrencyPairCurrencies)
                    .ThenInclude(pcp => pcp.Currency)
                    .Select(cpr => cpr.CurrencyPair)
                    .SelectMany(cp => cp.CurrencyPairCurrencies
                        .Where(ccp => ccp.Currency.Abbreviation
                            .Equals(ccp.CurrencyPair.MainCurrency, StringComparison.InvariantCultureIgnoreCase)))
                    .Select(pcp => pcp.Currency)
                    .FirstOrDefault();
#if DEBUG
                var t_cPReq = _unitOfWork.GetRepository<CurrencyPairRequest>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(cpr => cpr.Id.Equals(analysedComponent.RequestId)
                                  && cpr.DeletedAt == null && cpr.IsEnabled);
#endif

                if (currencyCPR != null)
                {
                    var component = _unitOfWork.GetRepository<CurrencyPairRequest>()
                        .GetQueryable()
                        .AsNoTracking()
                        .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                        .Include(cp => cp.CurrencyPair)
                        .ThenInclude(cp => cp.CurrencyPairCurrencies)
                        .ThenInclude(pcp => pcp.Currency)
                        // Where the partial currency pair's main currency is equal to the currency that is required
                        .Where(cpr => cpr.CurrencyPair.CurrencyPairCurrencies
                            .Any(ccp => ccp.Currency.Abbreviation
                                            .Equals(ccp.CurrencyPair.MainCurrency,
                                                StringComparison.InvariantCultureIgnoreCase)
                                        && ccp.Currency.Abbreviation.Equals(currencyCPR.Abbreviation,
                                            StringComparison.InvariantCultureIgnoreCase)))
                        .Include(cpr => cpr.RequestComponents)
                        // Null checks
                        .Where(cpr => cpr.IsEnabled && cpr.DeletedAt == null
                                                    && cpr.RequestComponents
                                                        .Any(rc =>
                                                            rc.DeletedAt == null && rc.IsEnabled
                                                                                 && !string.IsNullOrEmpty(rc.Value)))
                        // Obtain only the circulating supply
                        .SelectMany(cpr => cpr.RequestComponents
                            .Where(rc =>
                                rc.ComponentType.Equals(ComponentType.Circulating_Supply)))
                        .FirstOrDefault();

                    return component != null
                        ? decimal.Parse(component.Value ?? "0") /
                          (component.IsDenominated ? (decimal) Math.Pow(10, currencyCPR.Denominations) : decimal.One)
                        : decimal.Zero;
                }
                else
                {
                    var currencyWsr = _unitOfWork.GetRepository<WebsocketRequest>()
                        .GetQueryable()
                        .AsNoTracking()
                        .Where(cpr => cpr.Id.Equals(analysedComponent.RequestId)
                                      && cpr.DeletedAt == null && cpr.IsEnabled)
                        .Include(cpr => cpr.CurrencyPair)
                        .ThenInclude(cp => cp.CurrencyPairCurrencies)
                        .ThenInclude(pcp => pcp.Currency)
                        .Select(cpr => cpr.CurrencyPair)
                        .SelectMany(cp => cp.CurrencyPairCurrencies
                            .Where(ccp => ccp.Currency.Abbreviation
                                .Equals(ccp.CurrencyPair.MainCurrency, StringComparison.InvariantCultureIgnoreCase)))
                        .Select(pcp => pcp.Currency)
                        .FirstOrDefault();

                    var component = _unitOfWork.GetRepository<WebsocketRequest>()
                        .GetQueryable()
                        .AsNoTracking()
                        .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                        .Include(cp => cp.CurrencyPair)
                        .ThenInclude(cp => cp.CurrencyPairCurrencies)
                        .ThenInclude(pcp => pcp.Currency)
                        // Where the partial currency pair's main currency is equal to the currency that is required
                        .Where(cpr => cpr.CurrencyPair.CurrencyPairCurrencies
                            .Any(ccp => ccp.Currency.Abbreviation
                                            .Equals(ccp.CurrencyPair.MainCurrency,
                                                StringComparison.InvariantCultureIgnoreCase)
                                        && ccp.Currency.Abbreviation.Equals(currencyWsr.Abbreviation,
                                            StringComparison.InvariantCultureIgnoreCase)))
                        .Include(cpr => cpr.RequestComponents)
                        // Null checks
                        .Where(cpr => cpr.IsEnabled && cpr.DeletedAt == null
                                                    && cpr.RequestComponents
                                                        .Any(rc =>
                                                            rc.DeletedAt == null && rc.IsEnabled
                                                                                 && !string.IsNullOrEmpty(rc.Value)))
                        // Obtain only the circulating supply
                        .SelectMany(cpr => cpr.RequestComponents
                            .Where(rc =>
                                rc.ComponentType.Equals(ComponentType.Circulating_Supply)))
                        .FirstOrDefault();

                    return component != null
                        ? decimal.Parse(component.Value ?? "0") /
                          (component.IsDenominated ? (decimal) Math.Pow(10, currencyWsr.Denominations) : decimal.One)
                        : decimal.Zero;
                }
            }

            return decimal.MinusOne;
        }

        public ICollection<Currency> GetAll(bool includeNested = false)
        {
            var query = _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking();

            if (includeNested)
            {
                query = query
                    .Include(c => c.AnalysedComponents)
                    .Include(c => c.CurrencyPairSourceCurrencies)
                    .Include(c => c.CurrencyType)
                    .Include(c => c.CurrencySources)
                    .Include(c => c.CurrencyRequests)
                    .Include(c => c.CurrencyProperties);
            }

            return query.ToList();
        }
        
        public ICollection<Currency> GetAllNonDeleted(bool includeNested = false)
        {
            var query = _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
                .Where(c => c.DeletedAt == null);

            if (includeNested)
            {
                query = query
                    .Include(c => c.AnalysedComponents)
                    .Include(c => c.CurrencyPairSourceCurrencies)
                    .Include(c => c.CurrencyType)
                    .Include(c => c.CurrencySources)
                    .Include(c => c.CurrencyRequests)
                    .Include(c => c.CurrencyProperties);
            }

            return query.ToList();
        }

        public ICollection<DetailedCurrencyResponse> GetAllDetailed(string typeShortForm = "CRYPTO",
            int index = 0, int daysOfData = 1)
        {
            // Resultant collection
            var res = new List<DetailedCurrencyResponse>();

            // Initial query
            var currencies = _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
                .Include(c => c.CurrencyType)
                .Where(c => c.IsEnabled && c.DeletedAt == null);

            if (!string.IsNullOrEmpty(typeShortForm))
            {
                currencies = currencies.Where(c
                    => c.CurrencyType.TypeShortForm
                        .Equals(typeShortForm, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                // Default to crypto
                currencies = currencies.Where(c => c.CurrencyType.TypeShortForm
                    .Equals("CRYPTO", StringComparison.InvariantCultureIgnoreCase));
            }

            currencies = currencies
                .Include(c => c.AnalysedComponents)
                .ThenInclude(ac => ac.AnalysedHistoricItems)
                .Include(c => c.CurrencyPairSourceCurrencies)
                .ThenInclude(ccp => ccp.CurrencyPair)
                // TODO: Exclude this rule
                .Where(c => c.CurrencyPairSourceCurrencies.Any(ccp => ccp.CurrencyPair.CounterCurrency
                    .Equals(CoreConstants.GenericCounterCurrency, StringComparison.InvariantCultureIgnoreCase)))
//                .ThenInclude(pcp => pcp.Currency)
//                .Include(c => c.CurrencyCurrencyPairs)
//                .ThenInclude(pcp => pcp.CurrencyPair)
//                .ThenInclude(cp => cp.CurrencyPairRequests)
//                .ThenInclude(cpr => cpr.AnalysedComponents)
//                .ThenInclude(ac => ac.AnalysedHistoricItems)
                .Skip(20 * index)
                .Take(20)
                .Select(c => new Currency
                {
                    Id = c.Id,
                    CurrencyTypeId = c.CurrencyTypeId,
                    CurrencyType = c.CurrencyType,
                    Abbreviation = c.Abbreviation,
                    Name = c.Name,
                    Denominations = c.Denominations,
                    DenominationName = c.DenominationName,
                    CurrencySourceId = c.CurrencySourceId,
                    WalletTypeId = c.WalletTypeId,
                    AnalysedComponents = c.AnalysedComponents
                        .Where(ac => AnalysisConstants.CompactAnalysedComponentTypes.Contains(ac.ComponentType)
                                     || AnalysisConstants.LiveAnalysedComponentTypes.Contains(ac.ComponentType))
                        .Select(ac => new AnalysedComponent
                        {
                            Id = ac.Id,
                            ComponentType = ac.ComponentType,
                            Value = ac.Value,
                            Delay = ac.Delay,
                            RequestId = ac.RequestId,
                            AnalysedHistoricItems = ac.AnalysedHistoricItems
                                .OrderByDescending(ahi => ahi.HistoricDateTime)
                                .Where(ahi => ahi.HistoricDateTime < DateTime.UtcNow.Subtract(TimeSpan.FromDays(daysOfData)))
                                .Take(200) // Always limit the payload
                                .ToList()
                        })
                        .ToList(),
//                    CurrencyCurrencyPairs = c.CurrencyCurrencyPairs
//                        .Where(ccp =>
//                            ccp.CurrencyPair.MainCurrency.Equals(c.Abbrv, StringComparison.InvariantCultureIgnoreCase)
//                            && ccp.CurrencyPair.CounterCurrency.Contains(CoreConstants.GenericCounterCurrency))
//                        .Select(pcp => new CurrencyCurrencyPair
//                        {
//                            CurrencyId = pcp.CurrencyId,
//                            CurrencyPair = new CurrencyPair
//                            {
//                                Id = pcp.CurrencyPair.Id,
//                                CurrencyPairType = pcp.CurrencyPair.CurrencyPairType,
//                                APIUrl = pcp.CurrencyPair.APIUrl,
//                                DefaultComponent = pcp.CurrencyPair.DefaultComponent,
//                                CounterCurrency = pcp.CurrencyPair.CounterCurrency,
//                                CurrencySourceId = pcp.CurrencyPair.CurrencySourceId,
//                                CurrencyPairRequests = pcp.CurrencyPair.CurrencyPairRequests
//                                    .Where(cpr => cpr.DeletedAt == null && cpr.IsEnabled)
//                                    .Select(cpr => new CurrencyPairRequest
//                                    {
//                                        Id = cpr.Id,
//                                        CurrencyPairId = cpr.CurrencyPairId,
//                                        AnalysedComponents = cpr.AnalysedComponents
//                                            .Where(ac =>
//                                                AnalysisConstants.CompactAnalysedComponentTypes.Contains(
//                                                    ac.ComponentType)
//                                                || AnalysisConstants.LiveAnalysedComponentTypes.Contains(
//                                                    ac.ComponentType))
//                                            .Select(ac => new AnalysedComponent
//                                            {
//                                                Id = ac.Id,
//                                                ComponentType = ac.ComponentType,
//                                                Value = ac.Value,
//                                                Delay = ac.Delay,
//                                                RequestId = ac.RequestId,
//                                                CurrencyId = ac.CurrencyId,
//                                                AnalysedHistoricItems = ac.AnalysedHistoricItems
//                                                    .OrderByDescending(ahi => ahi.HistoricDateTime)
//                                                    .Where(ahi => ahi.HistoricDateTime <
//                                                                  DateTime.UtcNow.Subtract(TimeSpan.FromDays(daysOfData)))
//                                                    .Take(200) // Always limit the payload
//                                                    .ToList()
//                                            })
//                                            .ToList()
//                                    })
//                                    .ToList(),
//                            },
//                            CurrencyPairId = pcp.CurrencyPairId
//                        })
//                        .ToList()
                });
            
            #if DEBUG
            var currenciesColl = currencies.ToList();
            #endif
            
            // TODO: We'll need to figure out how we can factor in non-USD counter currency tickers

            var abbreviations = currencies.Select(c => c.Abbreviation).Distinct().ToList();
            foreach (var uniqueCurr in abbreviations)
            {
                res.Add(new DetailedCurrencyResponse(currencies.Where(c => c.Abbreviation
                    .Equals(uniqueCurr, StringComparison.InvariantCultureIgnoreCase))
                    .ToList()));
            }

//            foreach (var currency in currencies)
//            {
//                // Do not add duplicates
//                if (!res.Any(item =>
//                    item.Abbreviation.Equals(currency.Abbrv, StringComparison.InvariantCultureIgnoreCase)))
//                {
//                    res.Add(new DetailedCurrencyResponse(currency));
//                }
//                // Since there already is a duplicate
//                else
//                {
//                    // Populate it further
//                    res.SingleOrDefault(item => item.Abbreviation.Equals(currency.Abbrv,
//                            StringComparison.InvariantCultureIgnoreCase))
//                        ?.Populate(currency);
//                }
//            }

            res = res
                .OrderByDescending(c => c.MarketCap)
                .ToList();

            return res;
        }

        public DetailedCurrencyResponse GetDetailedById(long currencyId, ICollection<ComponentType> componentTypes)
        {
            var query = _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Include(cp => cp.CurrencyPairCurrencies)
                .ThenInclude(pcp => pcp.Currency)
                // Select any ticker that contains that abbreviation
                .Where(cp => cp.CurrencyPairCurrencies
                                 .Any(pcp => pcp.Currency.Id.Equals(currencyId)
                                             // Make sure we're analyzing the main currency, not the sub.
                                             && pcp.Currency.Abbreviation.Equals(pcp.CurrencyPair.MainCurrency))
                             // Ensure that the counter currency is the generic counter currency
                             && cp.CurrencyPairCurrencies
                                 .Any(pcp => pcp.Currency.Abbreviation.Equals(CoreConstants.GenericCounterCurrency,
                                                 StringComparison.InvariantCultureIgnoreCase)
                                             && pcp.Currency.Abbreviation.Equals(pcp.CurrencyPair.CounterCurrency)))
                .Include(cp => cp.CurrencyPairRequests)
                .ThenInclude(cpr => cpr.RequestComponents)
                .ThenInclude(rcd => rcd.RcdHistoricItems)
                .Where(cp => cp.CurrencyPairRequests
                    .Any(cpr => cpr.IsEnabled && cpr.DeletedAt == null
                                              && cpr.RequestComponents
                                                  .Any(rc => rc.DeletedAt == null && rc.IsEnabled
                                                                                  && componentTypes.Contains(
                                                                                      rc.ComponentType)
                                                                                  && !string.IsNullOrEmpty(rc.Value)
                                                                                  && rc.RcdHistoricItems != null
                                                                                  && rc.RcdHistoricItems
                                                                                      .Any(rcdhi =>
                                                                                          rcdhi.DeletedAt == null &&
                                                                                          rcdhi.IsEnabled))));

            return new DetailedCurrencyResponse(query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abbreviation"></param>
        /// <param name="componentTypes"></param>
        /// <returns></returns>
        public DetailedCurrencyResponse GetDetailedByAbbreviation(string abbreviation,
            ICollection<ComponentType> componentTypes)
        {
            var query = _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Include(cp => cp.CurrencyPairCurrencies)
                .ThenInclude(pcp => pcp.Currency)
                // Select any ticker that contains that abbreviation
                .Where(cp => cp.CurrencyPairCurrencies
                                 .Any(pcp => pcp.Currency.Abbreviation.Equals(abbreviation,
                                                 StringComparison.InvariantCultureIgnoreCase)
                                             // Make sure we're analyzing the main currency, not the sub.
                                             && pcp.Currency.Abbreviation.Equals(pcp.CurrencyPair.MainCurrency))
                             // Ensure that the counter currency is the generic counter currency
                             && cp.CurrencyPairCurrencies
                                 .Any(pcp => pcp.Currency.Abbreviation.Equals(CoreConstants.GenericCounterCurrency,
                                                 StringComparison.InvariantCultureIgnoreCase)
                                             && pcp.Currency.Abbreviation.Equals(pcp.CurrencyPair.CounterCurrency)))
                .Include(cp => cp.CurrencyPairRequests)
                .ThenInclude(cpr => cpr.RequestComponents)
                .ThenInclude(rcd => rcd.RcdHistoricItems)
                .Where(cp => cp.CurrencyPairRequests
                    .Any(cpr => cpr.IsEnabled && cpr.DeletedAt == null
                                              && cpr.RequestComponents
                                                  .Any(rc => rc.DeletedAt == null && rc.IsEnabled
                                                                                  && componentTypes.Contains(
                                                                                      rc.ComponentType)
                                                                                  && !string.IsNullOrEmpty(rc.Value)
                                                                                  && rc.RcdHistoricItems != null
                                                                                  && rc.RcdHistoricItems
                                                                                      .Any(rcdhi =>
                                                                                          rcdhi.DeletedAt == null &&
                                                                                          rcdhi.IsEnabled))));

            return new DetailedCurrencyResponse(abbreviation, query);
        }

        public bool Any(CreateCurrency createCurrency)
        {
            if (createCurrency != null && createCurrency.IsValid())
            {
                return _unitOfWork.GetRepository<Currency>()
                    .Get(c => c.CurrencySourceId.Equals(createCurrency.CurrencySourceId)
                              && c.Abbreviation.Equals(createCurrency.Abbrv)).Any();
            }

            return false;
        }

        public IEnumerable<Currency> GetAllActive(bool includeNested = false)
        {
            if (includeNested)
            {
                return _unitOfWork.GetRepository<Currency>().GetQueryable()
                    .AsNoTracking()
                    .Where(c => c.DeletedAt == null)
                    .Where(c => c.IsEnabled)
                    .Include(c => c.CurrencyPairSourceCurrencies);
            }
            else
            {
                return _unitOfWork.GetRepository<Currency>().GetQueryable()
                    .AsNoTracking()
                    .Where(c => c.DeletedAt == null)
                    .Where(c => c.IsEnabled);
            }
        }

        /// <summary>
        /// Gets all active.
        /// 
        /// </summary>
        /// <param name="includeNested"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetAllActiveObsc(bool includeNested = false)
        {
            if (includeNested)
            {
                return _unitOfWork.GetRepository<Currency>().GetQueryable()
                    .AsNoTracking()
                    .Where(c => c.DeletedAt == null)
                    .Where(c => c.IsEnabled)
                    .Include(c => c.CurrencyType)
                    .Select(c => new
                    {
                        id = c.Id,
                        abbrv = c.Abbreviation,
                        CurrencyType =
                            new
                            {
                                name = c.CurrencyType.Name,
                                typeShortForm = c.CurrencyType.TypeShortForm
                            },
                        name = c.Name,
                        walletTypeId = c.WalletTypeId
                    });
            }
            else
            {
                return _unitOfWork.GetRepository<Currency>().GetQueryable()
                    .AsNoTracking()
                    .Where(c => c.DeletedAt == null)
                    .Where(c => c.IsEnabled)
                    .Select(c => new
                    {
                        id = c.Id,
                        abbrv = c.Abbreviation,
                        name = c.Name,
                        walletTypeId = c.WalletTypeId
                    });
            }
        }

        /// <summary>
        /// Gets all active distinctively.
        /// 
        /// This is to prevent duplicates for displaying purposes. The user would normally
        /// be prompted after selecting a choice from this result before creating
        /// a final decision.
        /// </summary>
        /// <returns>The all active distinct.</returns>
        /// <param name="includeNested">If set to <c>true</c> include nested.</param>
        public IEnumerable<dynamic> GetAllActiveDistinctObsc(bool includeNested = false)
        {
            if (includeNested)
            {
                return _unitOfWork.GetRepository<Currency>().GetQueryable()
                    .AsNoTracking()
                    .Where(c => c.DeletedAt == null)
                    .Where(c => c.IsEnabled)
                    .Include(c => c.CurrencyType)
                    .DistinctBy(c => c.Abbreviation)
                    .Select(c => new
                    {
                        Id = c.Id,
                        Abbrv = c.Abbreviation,
                        CurrencyType =
                            new
                            {
                                name = c.CurrencyType.Name,
                                typeShortForm = c.CurrencyType.TypeShortForm
                            },
                        Name = c.Name
                    });
            }
            else
            {
                return _unitOfWork.GetRepository<Currency>().GetQueryable()
                    .AsNoTracking()
                    .Where(c => c.DeletedAt == null)
                    .Where(c => c.IsEnabled)
                    .DistinctBy(c => c.Abbreviation)
                    .Select(c => new
                    {
                        Id = c.Id,
                        Abbrv = c.Abbreviation,
                        Name = c.Name
                    });
            }
        }

        /// <summary>
        /// Gets all pairs with currency rs with CurrencyId <=> CurrencyId
        /// 
        /// In this API, we'll only map currency to currency. Selection of which
        /// currencypair will only come at the margin setup section.
        /// 
        /// Key => CurrencyId
        /// Value => Tuple<long (CurrencyId), string (CurrencyName), string (Abbreviation)>
        /// </summary>
        /// <returns>All the pairs with currency rs.</returns>
        public IDictionary<long, IDictionary<long, Tuple<string, string>>> GetAllCurrencyPairings()
        {
            // Prep the result
            IDictionary<long, IDictionary<long, Tuple<string, string>>> result =
                new Dictionary<long, IDictionary<long, Tuple<string, string>>>();

            // Deque (Thought of using this because c++)
            // A double-ended queue, which provides O(1) indexed access, 
            // O(1) removals, insertions to the front and back, O(N) to everywhere else
            var pcPairs = new List<CurrencyPairSourceCurrency>(_unitOfWork.GetRepository<CurrencyPairSourceCurrency>()
                .GetQueryable()
                .AsNoTracking()
                .Include(cp => cp.Currency));

            // You see. two pcps make up one cpair.
            // If odd, c# defaults to rounding down (not really rounding... it just removes the decimals)
            long dataCount = pcPairs.Count();
            for (var i = 0; i < dataCount / 2; i++)
            {
                // pop!
                var currPcPair = pcPairs.First();
                pcPairs.Remove(currPcPair);

                var currCurrencyId = currPcPair.CurrencyId;

                // Retrieve the the counter/mainpair
                var subPCPair = pcPairs // Not the same currency
                    .SingleOrDefault(pcp => pcp.CurrencyPairId.Equals(currPcPair.CurrencyPairId) // Same Currency pair
                                            && !pcp.CurrencyId.Equals(currCurrencyId));

                // Second layer check
                if (subPCPair != null && !subPCPair.CurrencyId.Equals(currCurrencyId))
                {
                    // Remove the pair
                    pcPairs.Remove(subPCPair);

                    // Add the pair

                    // If let's BTC's currencyid already exists and if it exists, the subpair hasn't existed yet
                    if (result.ContainsKey(currCurrencyId) && !result[currCurrencyId].ContainsKey(subPCPair.CurrencyId))
                    {
                        // Add the next pair in
                        result[currCurrencyId].Add(subPCPair.CurrencyId,
                            Tuple.Create(subPCPair.Currency.Name, subPCPair.Currency.Abbreviation));
                    }
                    else
                    {
                        // If not, add it
                        result.Add(currCurrencyId, new Dictionary<long, Tuple<string, string>>()
                        {
                            // Add item on initialization
                            {subPCPair.CurrencyId, Tuple.Create(subPCPair.Currency.Name, subPCPair.Currency.Abbreviation)}
                        });
                    }
                }
                else
                {
                    {
                        // Wow bad
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieves all the pairs with the currency rs in WalletTypeId <=> CurrencyId
        /// 
        /// Note that CurrencyId's WalletTypeId CANNOT match WalletTypeId.
        /// 
        /// Key = WalletTypeId, Value => Key = CounterCurrencyId. Value = CurrencyPairId
        /// </summary>
        /// <returns>All the pairs</returns>
        public IDictionary<long, IDictionary<long, long>> GetAllWalletTypeCurrencyPairings()
        {
            // Prep the result
            IDictionary<long, IDictionary<long, long>> result = new Dictionary<long, IDictionary<long, long>>();

            var pcPairs = _unitOfWork.GetRepository<CurrencyPairSourceCurrency>()
                .GetQueryable()
                .AsNoTracking()
                .Include(cp => cp.Currency)
                .Include(cp => cp.CurrencyPair)
                .ToList();

            // You see. two pcps make up one cpair.
            // If odd, c# defaults to rounding down (not really rounding... it just removes the decimals)
            long dataCount = pcPairs.Count();
            for (var i = 0; i < dataCount / 2; i++)
            {
                // pop!
                var currPCPair = pcPairs.First(cpcp => cpcp.Currency.Abbreviation.Equals(cpcp.CurrencyPair.MainCurrency));
                pcPairs.Remove(currPCPair);

                var currCurrencyId = currPCPair.CurrencyId;

                // Retrieve the the counter/mainpair
                var subPCPair = pcPairs // Not the same currency
                    .SingleOrDefault(pcp => pcp.CurrencyPairId.Equals(currPCPair.CurrencyPairId) // Same Currency pair
                                            && pcp.Currency.Abbreviation.Equals(pcp.CurrencyPair.CounterCurrency));

                // Second layer check
                if (subPCPair != null && !subPCPair.CurrencyId.Equals(currCurrencyId))
                {
                    long currCurrencySourceId =
                        _unitOfWork.GetRepository<CurrencyPair>()
                            .Get(predicate: cp => cp.Id.Equals(subPCPair.CurrencyPairId))
                            .Single().CurrencySourceId;

                    // Now let's see what pair this is
                    // We just need to make sure this is a semi-crypto pair because this API specifically only retrieves
                    // crypto-related pairs
                    if (currPCPair.Currency.WalletTypeId > 0 && subPCPair.Currency.WalletTypeId > 0
                    ) // Crypto-Crypto Pair
                    {
                        var walletTypeId = currPCPair.Currency.WalletTypeId;

                        // Remove the pair
                        pcPairs.Remove(subPCPair);

                        // Add the pair

                        // If let's BTC's currencyid already exists and if it exists, the subpair hasn't existed yet
                        if (result.ContainsKey(walletTypeId) &&
                            !result[walletTypeId].ContainsKey(subPCPair.CurrencyId))
                        {
                            // Add the next pair in
                            result[walletTypeId].Add(subPCPair.CurrencyId, currCurrencySourceId);
                        }
                        else
                        {
                            // If not, add it
                            result.Add(walletTypeId, new Dictionary<long, long>()
                            {
                                // Add item on initialization
                                {subPCPair.CurrencyId, currCurrencySourceId}
                            });
                        }
                    }
                    else if (currPCPair.Currency.WalletTypeId > 0 && subPCPair.Currency.WalletTypeId.Equals(0)
                    ) // Crypto-Fiat Pair
                    {
                        var walletTypeId = currPCPair.Currency.WalletTypeId;

                        // Remove the pair
                        pcPairs.Remove(subPCPair);

                        // Add the pair

                        // If let's BTC's currencyid already exists and if it exists, the subpair hasn't existed yet
                        if (result.ContainsKey(walletTypeId) &&
                            !result[walletTypeId].ContainsKey(subPCPair.CurrencyId))
                        {
                            // Add the next pair in
                            result[walletTypeId].Add(subPCPair.CurrencyId, currCurrencySourceId);
                        }
                        else
                        {
                            // If not, add it
                            result.Add(walletTypeId, new Dictionary<long, long>()
                            {
                                // Add item on initialization
                                {subPCPair.CurrencyId, currCurrencySourceId}
                            });
                        }
                    }

                    // If you really want to implement Fiat-Crypto, do it here when you really need it
                }
            }

            return result;
        }
    }
}
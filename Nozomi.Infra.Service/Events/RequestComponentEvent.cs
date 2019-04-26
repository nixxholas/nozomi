using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core;
using Nozomi.Base.Core.Helpers.Enumerable;
using Nozomi.Data;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class RequestComponentEvent : BaseEvent<RequestComponentEvent, NozomiDbContext>, IRequestComponentEvent
    {
        public RequestComponentEvent(ILogger<RequestComponentEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork)
            : base(logger, unitOfWork)
        {
        }

        public ICollection<RequestComponent> AllByRequestId(long requestId, bool includeNested = false)
        {
            if (requestId <= 0) return null;

            return includeNested
                ? _unitOfWork.GetRepository<RequestComponent>()
                    .GetQueryable(rc => rc.RequestId.Equals(requestId) && rc.DeletedAt == null && rc.IsEnabled)
                    .AsNoTracking()
                    .Include(rc => rc.Request)
                    .ToList()
                : _unitOfWork.GetRepository<RequestComponent>()
                    .GetQueryable(rc => rc.RequestId.Equals(requestId) && rc.DeletedAt == null && rc.IsEnabled)
                    .AsNoTracking()
                    .ToList();
        }

        public ICollection<RequestComponent> All(int index = 0, bool includeNested = false)
        {
            return includeNested
                ? _unitOfWork.GetRepository<RequestComponent>()
                    .GetQueryable(rc => rc.DeletedAt == null && rc.IsEnabled)
                    .AsNoTracking()
                    .Include(rc => rc.Request)
                    .Skip(index * 20)
                    .Take(20)
                    .ToList()
                : _unitOfWork.GetRepository<RequestComponent>()
                    .GetQueryable(rc => rc.DeletedAt == null && rc.IsEnabled)
                    .AsNoTracking()
                    .Skip(index * 20)
                    .Take(20)
                    .ToList();
        }

        public decimal ComputeDifference(string baseCurrencyAbbrv, string comparingCurrencyAbbrv,
            ComponentType componentType)
        {
            // Make sure it's comparable first
            if (EnumerableHelper.IsComparable(componentType))
            {
                switch (componentType)
                {
                    case ComponentType.Ask:
                    case ComponentType.Bid:
                    case ComponentType.Low:
                    case ComponentType.High:
                    case ComponentType.Daily_Change:
                    case ComponentType.VOLUME:
                        // Since it's comparable, lets get the exchange rate
                        return _unitOfWork.GetRepository<CurrencyPair>()
                            .GetQueryable()
                            .AsNoTracking()
                            .Where(cp => cp.IsEnabled && cp.DeletedAt == null)
                            .Include(cp => cp.CurrencyPairCurrencies)
                            .ThenInclude(pcp => pcp.Currency)
                            .Where(cp => cp.CurrencyPairCurrencies.FirstOrDefault(ccp => ccp.Currency.Abbrv
                                                 .Equals(ccp.CurrencyPair.MainCurrency, StringComparison.InvariantCultureIgnoreCase))
                                             .Currency.Abbrv
                                             .Equals(baseCurrencyAbbrv, StringComparison.InvariantCultureIgnoreCase)
                                         && cp.CurrencyPairCurrencies.FirstOrDefault(ccp => ccp.Currency.Abbrv
                                                 .Equals(ccp.CurrencyPair.CounterCurrency, StringComparison.InvariantCultureIgnoreCase))
                                             .Currency.Abbrv
                                             .Equals(comparingCurrencyAbbrv,
                                                 StringComparison.InvariantCultureIgnoreCase))
                            .Include(cp => cp.CurrencyPairRequests)
                            .ThenInclude(cpr => cpr.RequestComponents)
                            .Where(cPair => cPair.CurrencyPairRequests.Any(cpr => cpr.IsEnabled && cpr.DeletedAt == null
                                                                                                && cpr.RequestComponents
                                                                                                    .Any(rc =>
                                                                                                        rc.ComponentType
                                                                                                            .Equals(
                                                                                                                componentType))))
                            .SelectMany(cp => cp.CurrencyPairRequests
                                .SelectMany(cpr => cpr.RequestComponents
                                    .Select(rc => decimal.Parse(rc.Value))))
                            .DefaultIfEmpty()
                            .Average();
                    default:
                        // Can't compute lol.
                        return decimal.Zero;
                }
            }

            return decimal.Zero;
        }

        // TODO: Ask .NET Core devs for advice to optimise this...
        // TODO: 100% sure this is 100x un-optimised... fuck this query
        public void ConvertToGenericCurrency(ICollection<RequestComponent> requestComponents)
        {
            if (requestComponents != null && requestComponents.Count > 0)
            {
                // Iterate all of the reqcomps for conversion
                foreach (var reqCom in requestComponents)
                {
                    // Obtain the current req com's counter currency first
                    var counterCurr = _unitOfWork.GetRepository<Currency>()
                        .GetQueryable()
                        .AsNoTracking()
                        .Include(c => c.CurrencyCurrencyPairs)
                        .ThenInclude(pcp => pcp.CurrencyPair)
                        .ThenInclude(cp => cp.CurrencyPairRequests)
                        .ThenInclude(cpr => cpr.RequestComponents)
                        .SingleOrDefault(c => c.CurrencyCurrencyPairs
                                // Make sure we're not converting if we don't have to.
                            .Where(ccp => ccp.Currency.Abbrv
                                              .Equals(ccp.CurrencyPair.CounterCurrency, StringComparison.InvariantCultureIgnoreCase)
                                          && !ccp.Currency.Abbrv.Equals(CoreConstants.GenericCounterCurrency,
                                              StringComparison.InvariantCultureIgnoreCase))
                            .Select(pcp => pcp.CurrencyPair)
                            .SelectMany(cp => cp.CurrencyPairRequests)
                            .SelectMany(cpr => cpr.RequestComponents)
                            .Any(rc => rc.Id.Equals(reqCom.Id)));

                    // Null check
                    if (counterCurr != null)
                    {
                        // Obtain the conversion rate
                        var conversionRate = _unitOfWork
                            .GetRepository<CurrencyCurrencyPair>()
                            .GetQueryable()
                            .AsNoTracking()
                            .Where(ccp => ccp.Currency.Abbrv
                                              .Equals(ccp.CurrencyPair.MainCurrency, StringComparison.InvariantCultureIgnoreCase) &&
                                          ccp.Currency.Abbrv.Equals(counterCurr.Abbrv,
                                              StringComparison.InvariantCultureIgnoreCase))
                            .Where(ccp => ccp.Currency.Abbrv
                                              .Equals(ccp.CurrencyPair.CounterCurrency, StringComparison.InvariantCultureIgnoreCase) && 
                                          ccp.Currency.Abbrv.Equals(CoreConstants.GenericCounterCurrency,
                                              StringComparison.InvariantCultureIgnoreCase))
                            .Include(pcp => pcp.CurrencyPair)
                            .ThenInclude(cp => cp.CurrencyPairRequests)
                            .ThenInclude(cpr => cpr.AnalysedComponents)
                            .SelectMany(pcp => pcp.CurrencyPair.CurrencyPairRequests)
                            .SelectMany(cpr => cpr.AnalysedComponents)
                            .FirstOrDefault(ac => ac.ComponentType.Equals(AnalysedComponentType.CurrentAveragePrice));

                        if (conversionRate != null && decimal.TryParse(conversionRate.Value, out var conversionVal))
                        {
                            // Since we've gotten the conversion rate, let's convert.
                            reqCom.Value = (decimal.Parse(reqCom.Value) * conversionVal).ToString(CultureInfo.InvariantCulture);

                            if (reqCom.RcdHistoricItems != null && reqCom.RcdHistoricItems.Count > 0)
                            {
                                foreach (var rcdhi in reqCom.RcdHistoricItems)
                                {
                                    rcdhi.Value = (decimal.Parse(rcdhi.Value) * conversionVal)
                                        .ToString(CultureInfo.InvariantCulture);
                                }
                            }
                        }
                    }
                    else
                    {
                        _logger.LogWarning($" Request Component: {reqCom.Id} does not have a counter currency.");

                        requestComponents.Remove(reqCom);
                    }
                }
            }
        }

        public ICollection<RequestComponent> GetByMainCurrency(string mainCurrencyAbbrv,
            ICollection<ComponentType> componentTypes)
        {
            return _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable(cp => cp.DeletedAt == null && cp.IsEnabled)
                .AsNoTracking()
                .Include(cp => cp.CurrencyPairCurrencies)
                .ThenInclude(pcp => pcp.Currency)
                .Where(cp => cp.CurrencyPairCurrencies
                    .FirstOrDefault(ccp => ccp.Currency.Abbrv
                        .Equals(ccp.CurrencyPair.MainCurrency, StringComparison.InvariantCultureIgnoreCase))
                    .Currency.Abbrv.Equals(mainCurrencyAbbrv,
                        StringComparison.InvariantCultureIgnoreCase))
                .Include(cp => cp.CurrencyPairRequests)
                .ThenInclude(cpr => cpr.RequestComponents)
                .SelectMany(cp => cp.CurrencyPairRequests
                    .Where(cpr => cpr.RequestComponents != null && cpr.RequestComponents.Count > 0))
                .SelectMany(cpr => cpr.RequestComponents)
                .ToList();
        }

        /// <summary>
        /// Allows the caller to obtain all RequestComponents relevant to the currency
        /// pair in question via the abbreviation method. (i.e. ETHUSD)
        ///
        /// i.e. If the Currency Pair binded to the AnalysedComponent has a ticker abbreviation
        /// of ETHUSD, we will lookup for ALL RequestComponents related to that ticker abbreviation.
        ///
        /// TODO: Implement a predicate parameter feature to allow item filtering at the query level.
        /// </summary>
        /// <param name="analysedComponentId">The unique identifier of the analysed component
        /// that is related to the ticker in question.</param>
        /// <returns>Collection of request components related to the component</returns>
        public ICollection<RequestComponent> GetAllByCorrelation(long analysedComponentId, bool track = false)
        {
            // First, obtain the correlation PCPs
            var correlPCPs = _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cp => cp.IsEnabled && cp.DeletedAt == null)
                .Include(cp => cp.CurrencyPairRequests)
                .ThenInclude(cpr => cpr.AnalysedComponents)
                .Include(cp => cp.WebsocketRequests)
                    .ThenInclude(wsr => wsr.AnalysedComponents)
                .Where(cp => cp.CurrencyPairRequests
                    .Any(cpr => cpr.DeletedAt == null && cpr.IsEnabled
                                                      // We can ignore disabled or deleted ACs, just using this 
                                                      // to find the correlation
                                                      && cpr.AnalysedComponents.Any(ac =>
                                                          ac.Id.Equals(analysedComponentId)))
                || cp.WebsocketRequests
                    .Any(cpr => cpr.DeletedAt == null && cpr.IsEnabled
                                                      // We can ignore disabled or deleted ACs, just using this 
                                                      // to find the correlation
                                                      && cpr.AnalysedComponents.Any(ac =>
                                                          ac.Id.Equals(analysedComponentId))))
                .Include(cp => cp.CurrencyPairCurrencies)
                .ThenInclude(pcp => pcp.Currency)
                .SelectMany(cp => cp.CurrencyPairCurrencies
                    .Select(pcp => new CurrencyCurrencyPair
                    {
                        Currency = pcp.Currency,
                        CurrencyPair = pcp.CurrencyPair,
                        CurrencyId = pcp.CurrencyId,
                        CurrencyPairId = pcp.CurrencyPairId
                    }))
                .DefaultIfEmpty()
                .ToList();

            var firstCcp = correlPCPs.FirstOrDefault();
            
            if (correlPCPs.Count < 2 || firstCcp == null)
            {
                var query = _unitOfWork.GetRepository<Currency>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(cp => cp.IsEnabled && cp.DeletedAt == null)
                    .Include(c => c.CurrencyRequests)
                    .ThenInclude(cpr => cpr.AnalysedComponents)
                    .Where(c => c.CurrencyRequests
                        .Any(cr => cr.DeletedAt == null && cr.IsEnabled
                                                        // We can ignore disabled or deleted ACs, just using this 
                                                        // to find the correlation
                                                        && cr.AnalysedComponents.Any(ac =>
                                                            ac.Id.Equals(analysedComponentId))))
                    .Include(c => c.CurrencyRequests)
                    .ThenInclude(c => c.RequestComponents);
                
                if (track)
                {
                    query.ThenInclude(rc => rc.RcdHistoricItems);
                }
                
                return query
                    .SelectMany(c => c.CurrencyRequests)
                    .SelectMany(cr => cr.RequestComponents)
                    .Select(rc => new RequestComponent
                    {
                        Id = rc.Id,
                        ComponentType = rc.ComponentType,
                        Identifier = rc.Identifier,
                        IsDenominated = rc.IsDenominated,
                        QueryComponent = rc.QueryComponent,
                        Value = rc.Value,
                        RcdHistoricItems = rc.RcdHistoricItems
                    })
                    .ToList();
            }

            var tickerPairStr = string.Concat(firstCcp.CurrencyPair.MainCurrency, firstCcp.CurrencyPair.CounterCurrency);

            var queryRes = _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Include(cp => cp.CurrencyPairCurrencies)
                .ThenInclude(pcp => pcp.Currency)
                .Where(cp => string.Concat(cp.MainCurrency, cp.CounterCurrency)
                        .Equals(tickerPairStr, StringComparison.InvariantCultureIgnoreCase)
//                    // Make sure the main currencies are identical
//                    cp.PartialCurrencyPairs.FirstOrDefault(pcp => pcp.IsMain).Currency.Abbrv
//                        .Equals(correlPCPs.FirstOrDefault(cpcp => cpcp.IsMain).Currency.Abbrv, 
//                            StringComparison.InvariantCultureIgnoreCase)
//                    // Make sure the counter currencies are identical
//                    && cp.PartialCurrencyPairs.FirstOrDefault(pcp => !pcp.IsMain).Currency.Abbrv
//                        .Equals(correlPCPs.FirstOrDefault(cpcp => !cpcp.IsMain).Currency.Abbrv,
//                            StringComparison.InvariantCultureIgnoreCase)
                )
                .Include(cp => cp.CurrencyPairRequests)
                    .ThenInclude(cpr => cpr.RequestComponents)
                .Include(cp => cp.WebsocketRequests)
                    .ThenInclude(wsr => wsr.RequestComponents);

            if (track)
            {
                queryRes.ThenInclude(rc => rc.RcdHistoricItems);
            }
            
            // Also slot in the web socket request components as well
            
            
            return queryRes
                .SelectMany(cp => cp.CurrencyPairRequests)
                .SelectMany(cpr => cpr.RequestComponents)
                .Select(rc => new RequestComponent
                {
                    Id = rc.Id,
                    ComponentType = rc.ComponentType,
                    Identifier = rc.Identifier,
                    IsDenominated = rc.IsDenominated,
                    QueryComponent = rc.QueryComponent,
                    Value = rc.Value,
                    RcdHistoricItems = rc.RcdHistoricItems
                })
                .Concat(queryRes
                    .SelectMany(cp => cp.WebsocketRequests)
                    .SelectMany(cpr => cpr.RequestComponents)
                    .Select(rc => new RequestComponent
                    {
                        Id = rc.Id,
                        ComponentType = rc.ComponentType,
                        Identifier = rc.Identifier,
                        IsDenominated = rc.IsDenominated,
                        QueryComponent = rc.QueryComponent,
                        Value = rc.Value,
                        RcdHistoricItems = rc.RcdHistoricItems
                    })
                )
                .ToList();
//            if (predicate != null)
//            {
//                return finalQuery
//                    .SelectMany(cp => cp.CurrencyPairRequests
//                        .SelectMany(cpr => cpr.RequestComponents
//                            .Where(predicate)))
//                    .ToList();
//            }
//            else
//            {
//                return finalQuery
//                    .SelectMany(cp => cp.CurrencyPairRequests
//                        .SelectMany(cpr => cpr.RequestComponents))
//                    .ToList();
//            }
        }

        /// <summary>
        /// Obtains all components with the base currency stated.
        /// </summary>
        /// <param name="currencyId">Base Currency Id</param>
        /// <returns></returns>
        public ICollection<RequestComponent> GetAllByCurrency(long currencyId, bool track = false)
        {
            // First, obtain the currency in question
            var qCurrency = _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
                .SingleOrDefault(c => c.Id.Equals(currencyId));

            if (qCurrency == null) return null;

            var query = _unitOfWork.GetRepository<CurrencyPairRequest>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cpr => cpr.IsEnabled && cpr.DeletedAt == null)
                .Include(cpr => cpr.CurrencyPair)
                .ThenInclude(cp => cp.CurrencyPairCurrencies)
                // Filter via qCurrency
                .Where(cpr => cpr.CurrencyPair.CurrencyPairCurrencies
                    .FirstOrDefault(ccp => ccp.Currency.Abbrv
                        .Equals(ccp.CurrencyPair.MainCurrency, StringComparison.InvariantCultureIgnoreCase))
                    .CurrencyId.Equals(qCurrency.Id))
                .Include(cpr => cpr.RequestComponents);

            if (track)
            {
                query.ThenInclude(rc => rc.RcdHistoricItems);
            }
            
            return query
                .Where(cpr => cpr.RequestComponents
                    .Any(rc => rc.DeletedAt == null
                               && rc.IsEnabled))
                .SelectMany(cpr => cpr.RequestComponents
                    .Select(rc => new RequestComponent
                    {
                        Id = rc.Id,
                        ComponentType = rc.ComponentType,
                        Identifier = rc.Identifier,
                        IsDenominated = rc.IsDenominated,
                        QueryComponent = rc.QueryComponent,
                        Value = rc.Value,
                        RcdHistoricItems = rc.RcdHistoricItems
                    }))
                .ToList();
        }

        public NozomiResult<RequestComponent> Get(long id, bool includeNested = false)
        {
            if (includeNested)
                return new NozomiResult<RequestComponent>(_unitOfWork.GetRepository<RequestComponent>().GetQueryable()
                    .Include(rc => rc.Request)
                    .SingleOrDefault(rc => rc.Id.Equals(id) && rc.IsEnabled && rc.DeletedAt == null));

            return new NozomiResult<RequestComponent>(_unitOfWork.GetRepository<RequestComponent>()
                .Get(rc => rc.Id.Equals(id) && rc.DeletedAt == null && rc.IsEnabled)
                .SingleOrDefault());
        }
    }
}
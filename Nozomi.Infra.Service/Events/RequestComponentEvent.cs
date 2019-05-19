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

        public ICollection<RequestComponent> GetAllByRequest(long requestId, bool includeNested = false)
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
                        .Include(c => c.CurrencyPairSourceCurrencies)
                        .ThenInclude(pcp => pcp.CurrencyPair)
                        .ThenInclude(cp => cp.CurrencyPairRequests)
                        .ThenInclude(cpr => cpr.RequestComponents)
                        .SingleOrDefault(c => c.CurrencyPairSourceCurrencies
                                // Make sure we're not converting if we don't have to.
                            .Where(cpsc => cpsc.CurrencySource.Currency.Abbreviation
                                              .Equals(cpsc.CurrencyPair.CounterCurrency, StringComparison.InvariantCultureIgnoreCase)
                                           // make sure the counter currency is not the generic counter currency
                                          && !cpsc.CurrencyPair.CounterCurrency.Contains(CoreConstants.GenericCounterCurrency,
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
                            .GetRepository<CurrencyPairSourceCurrency>()
                            .GetQueryable()
                            .AsNoTracking()
                            // Make sure the counter currency is the main currency
                            .Where(cpsc => cpsc.CurrencyPair.MainCurrency
                                              .Contains(counterCurr.Abbreviation, StringComparison.InvariantCultureIgnoreCase)
                                           // Make sure the counter currency is the generic counter currency
                                           && cpsc.CurrencyPair.CounterCurrency.Equals(CoreConstants.GenericCounterCurrency,
                                              StringComparison.InvariantCultureIgnoreCase))
                            .Include(pcp => pcp.CurrencyPair)
                            .ThenInclude(cp => cp.CurrencyPairRequests)
                            .ThenInclude(cpr => cpr.AnalysedComponents)
                            .SelectMany(pcp => pcp.CurrencyPair.CurrencyPairRequests)
                            .SelectMany(cpr => cpr.AnalysedComponents)
                            .FirstOrDefault(ac => ac.ComponentType.Equals(AnalysedComponentType.CurrentAveragePrice));

                        // Make sure we can actually convert tho
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
                .Where(cp => cp.MainCurrency.Equals(mainCurrencyAbbrv, StringComparison.InvariantCultureIgnoreCase))
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
            var analysedComponent = _unitOfWork.GetRepository<AnalysedComponent>()
                .GetQueryable()
                .AsNoTracking()
                .SingleOrDefault(ac => ac.Id.Equals(analysedComponentId));

            if (analysedComponent != null)
            {
                // Request-based tracking
                if (analysedComponent.RequestId != null && analysedComponent.RequestId > 0)
                {
                    var query = _unitOfWork.GetRepository<RequestComponent>()
                        .GetQueryable()
                        .AsNoTracking()
                        .Where(rc => rc.RequestId.Equals(analysedComponent.RequestId));

                    if (track)
                    {
                        query = query.Include(rc => rc.RcdHistoricItems);
                    }
                
                    return query.ToList();
                } 
                // Currency-based tracking
                else if (analysedComponent.CurrencyId != null && analysedComponent.CurrencyId > 0)
                {
                    var query = _unitOfWork.GetRepository<CurrencyRequest>()
                        .GetQueryable()
                        .AsNoTracking()
                        .Where(cr => cr.CurrencyId.Equals(analysedComponent.CurrencyId))
                        .Include(cr => cr.RequestComponents);

                    if (track)
                    {
                        query.ThenInclude(rc => rc.RcdHistoricItems);
                    }
                
                    return query.SelectMany(rc => rc.RequestComponents).ToList();
                }
            }

            return null;

//            // First, obtain the correlation PCPs
//            var correlPCPs = _unitOfWork.GetRepository<CurrencyPair>()
//                .GetQueryable()
//                .AsNoTracking()
//                .Where(cp => cp.IsEnabled && cp.DeletedAt == null)
//                .Include(cp => cp.CurrencyPairRequests)
//                .ThenInclude(cpr => cpr.AnalysedComponents)
//                .Include(cp => cp.WebsocketRequests)
//                    .ThenInclude(wsr => wsr.AnalysedComponents)
//                .Where(cp => cp.CurrencyPairRequests
//                    .Any(cpr => cpr.DeletedAt == null && cpr.IsEnabled
//                                                      // We can ignore disabled or deleted ACs, just using this 
//                                                      // to find the correlation
//                                                      && cpr.AnalysedComponents.Any(ac =>
//                                                          ac.Id.Equals(analysedComponentId)))
//                || cp.WebsocketRequests
//                    .Any(cpr => cpr.DeletedAt == null && cpr.IsEnabled
//                                                      // We can ignore disabled or deleted ACs, just using this 
//                                                      // to find the correlation
//                                                      && cpr.AnalysedComponents.Any(ac =>
//                                                          ac.Id.Equals(analysedComponentId))))
//                .Include(cp => cp.CurrencyPairCurrencies)
//                .ThenInclude(pcp => pcp.Currency)
//                .SelectMany(cp => cp.CurrencyPairCurrencies
//                    .Select(pcp => new CurrencyPairSourceCurrency
//                    {
//                        Currency = pcp.Currency,
//                        CurrencyPair = pcp.CurrencyPair,
//                        CurrencyId = pcp.CurrencyId,
//                        CurrencyPairId = pcp.CurrencyPairId
//                    }))
//                .DefaultIfEmpty()
//                .ToList();
//
//            var firstCcp = correlPCPs.FirstOrDefault();
//            
//            if (correlPCPs.Count < 2 || firstCcp == null)
//            {
//                var query = _unitOfWork.GetRepository<Currency>()
//                    .GetQueryable()
//                    .AsNoTracking()
//                    .Where(cp => cp.IsEnabled && cp.DeletedAt == null)
//                    .Include(c => c.CurrencyRequests)
//                    .ThenInclude(cpr => cpr.AnalysedComponents)
//                    .Where(c => c.CurrencyRequests
//                        .Any(cr => cr.DeletedAt == null && cr.IsEnabled
//                                                        // We can ignore disabled or deleted ACs, just using this 
//                                                        // to find the correlation
//                                                        && cr.AnalysedComponents.Any(ac =>
//                                                            ac.Id.Equals(analysedComponentId))))
//                    .Include(c => c.CurrencyRequests)
//                    .ThenInclude(c => c.RequestComponents);
//                
//                if (track)
//                {
//                    query.ThenInclude(rc => rc.RcdHistoricItems);
//                }
//                
//                return query
//                    .SelectMany(c => c.CurrencyRequests)
//                    .SelectMany(cr => cr.RequestComponents)
//                    .Select(rc => new RequestComponent
//                    {
//                        Id = rc.Id,
//                        ComponentType = rc.ComponentType,
//                        Identifier = rc.Identifier,
//                        IsDenominated = rc.IsDenominated,
//                        QueryComponent = rc.QueryComponent,
//                        Value = rc.Value,
//                        RcdHistoricItems = rc.RcdHistoricItems
//                    })
//                    .ToList();
//            }
//
//            var tickerPairStr = string.Concat(firstCcp.CurrencyPair.MainCurrency, firstCcp.CurrencyPair.CounterCurrency);
//
//            var queryRes = _unitOfWork.GetRepository<CurrencyPair>()
//                .GetQueryable()
//                .AsNoTracking()
//                .Include(cp => cp.CurrencyPairCurrencies)
//                .ThenInclude(pcp => pcp.Currency)
//                .Where(cp => string.Concat(cp.MainCurrency, cp.CounterCurrency)
//                        .Equals(tickerPairStr, StringComparison.InvariantCultureIgnoreCase)
//                )
//                .Include(cp => cp.CurrencyPairRequests)
//                    .ThenInclude(cpr => cpr.RequestComponents)
//                        .ThenInclude(rc => rc.RcdHistoricItems)
//                .Include(cp => cp.WebsocketRequests)
//                    .ThenInclude(wsr => wsr.RequestComponents)
//                        .ThenInclude(rc => rc.RcdHistoricItems);
//            
//            #if DEBUG
//            var testReqComps = queryRes
//                .SelectMany(cp => cp.CurrencyPairRequests)
//                .SelectMany(cpr => cpr.RequestComponents)
//                .Select(rc => new RequestComponent
//                {
//                    Id = rc.Id,
//                    ComponentType = rc.ComponentType,
//                    Identifier = rc.Identifier,
//                    IsDenominated = rc.IsDenominated,
//                    QueryComponent = rc.QueryComponent,
//                    Value = rc.Value,
//                    RcdHistoricItems = rc.RcdHistoricItems.Where(rcdhi => rcdhi.CreatedAt < 
//                                                                          DateTime.UtcNow.Subtract(TimeSpan.FromDays(7)))
//                        .ToList()
//                })
//                .ToList();
//            
//            if (analysedComponentId == 12 || analysedComponentId == 18 || analysedComponentId == 21)
//            {
//                var cprReqCompsTest = queryRes
//                    .SelectMany(cp => cp.CurrencyPairRequests)
//                    .SelectMany(cpr => cpr.RequestComponents)
//                    .Select(rc => new RequestComponent
//                    {
//                        Id = rc.Id,
//                        ComponentType = rc.ComponentType,
//                        Identifier = rc.Identifier,
//                        IsDenominated = rc.IsDenominated,
//                        QueryComponent = rc.QueryComponent,
//                        Value = rc.Value,
//                        RcdHistoricItems = rc.RcdHistoricItems
//                    }).ToList();
//                
//                var res = queryRes
//                    .SelectMany(cp => cp.CurrencyPairRequests)
//                    .SelectMany(cpr => cpr.RequestComponents)
//                    .Select(rc => new RequestComponent
//                    {
//                        Id = rc.Id,
//                        ComponentType = rc.ComponentType,
//                        Identifier = rc.Identifier,
//                        IsDenominated = rc.IsDenominated,
//                        QueryComponent = rc.QueryComponent,
//                        Value = rc.Value,
//                        RcdHistoricItems = rc.RcdHistoricItems
//                    })
//                    .Concat(queryRes
//                        .SelectMany(cp => cp.WebsocketRequests)
//                        .SelectMany(cpr => cpr.RequestComponents)
//                        .Select(rc => new RequestComponent
//                        {
//                            Id = rc.Id,
//                            ComponentType = rc.ComponentType,
//                            Identifier = rc.Identifier,
//                            IsDenominated = rc.IsDenominated,
//                            QueryComponent = rc.QueryComponent,
//                            Value = rc.Value,
//                            RcdHistoricItems = rc.RcdHistoricItems
//                        })
//                    )
//                    .ToList();
//
//                var testReqCom = res.FirstOrDefault();
//            }
//#endif
//            
//            return queryRes
//                .SelectMany(cp => cp.CurrencyPairRequests)
//                .SelectMany(cpr => cpr.RequestComponents)
//                .Select(rc => new RequestComponent
//                {
//                    Id = rc.Id,
//                    ComponentType = rc.ComponentType,
//                    Identifier = rc.Identifier,
//                    IsDenominated = rc.IsDenominated,
//                    QueryComponent = rc.QueryComponent,
//                    Value = rc.Value,
//                    RcdHistoricItems = rc.RcdHistoricItems
//                })
//                .Concat(queryRes
//                    .SelectMany(cp => cp.WebsocketRequests)
//                    .SelectMany(cpr => cpr.RequestComponents)
//                    .Select(rc => new RequestComponent
//                    {
//                        Id = rc.Id,
//                        ComponentType = rc.ComponentType,
//                        Identifier = rc.Identifier,
//                        IsDenominated = rc.IsDenominated,
//                        QueryComponent = rc.QueryComponent,
//                        Value = rc.Value,
//                        RcdHistoricItems = rc.RcdHistoricItems
//                    })
//                )
//                .ToList();
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
            
            // Now that we know what Currency it is, we make full use of its Abbreviation

            var query = _unitOfWork.GetRepository<CurrencyPairRequest>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cpr => cpr.IsEnabled && cpr.DeletedAt == null)
                .Include(cpr => cpr.CurrencyPair)
                .ThenInclude(cp => cp.CurrencyPairCurrencies)
                // Filter via qCurrency
                .Where(cpr => cpr.CurrencyPair.CurrencyPairCurrencies
                    .FirstOrDefault(ccp => ccp.Currency.Abbreviation
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
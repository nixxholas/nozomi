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
using Nozomi.Data.Models.Web.Websocket;
using Nozomi.Preprocessing;
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

        public ICollection<RequestComponent> GetByMainCurrency(string mainCurrencyAbbrv,
            ICollection<ComponentType> componentTypes)
        {
            return _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable(cp => cp.DeletedAt == null && cp.IsEnabled)
                .AsNoTracking()
                .Where(cp => cp.MainCurrencyAbbrv.Equals(mainCurrencyAbbrv, StringComparison.InvariantCultureIgnoreCase))
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
        public ICollection<RequestComponent> GetAllByCorrelation(long analysedComponentId, bool track = false
            , int index = 0)
        {
            var analysedComponent = _unitOfWork.GetRepository<AnalysedComponent>()
                .GetQueryable()
                .AsNoTracking()
                .SingleOrDefault(ac => ac.Id.Equals(analysedComponentId));

            if (analysedComponent != null)
            {
                var res = new List<RequestComponent>();
                
                // CurrencyPair-based tracking
                if (analysedComponent.CurrencyPairId != null && analysedComponent.CurrencyPairId > 0)
                {
                    var query = _unitOfWork.GetRepository<CurrencyPair>()
                        .GetQueryable()
                        .AsNoTracking()
                        .Where(cp => cp.DeletedAt == null && cp.IsEnabled
                                                          && cp.Id.Equals(analysedComponent.CurrencyPairId));

                    if (track)
                    {
                        query = query
                            .Include(cp => cp.WebsocketRequests)
                            .ThenInclude(wsr => wsr.RequestComponents)
                            .ThenInclude(rc => rc.RcdHistoricItems)
                            .Include(cp => cp.CurrencyPairRequests)
                            .ThenInclude(cpr => cpr.RequestComponents)
                            .ThenInclude(rc => rc.RcdHistoricItems);
                    }
                    else
                    {
                        query = query
                            .Include(cp => cp.WebsocketRequests)
                            .ThenInclude(wsr => wsr.RequestComponents)
                            .Include(cp => cp.CurrencyPairRequests)
                            .ThenInclude(cpr => cpr.RequestComponents)
                            .Select(cp => new CurrencyPair
                            {
                                Id = cp.Id,
                                CurrencyPairType = cp.CurrencyPairType,
                                APIUrl = cp.APIUrl,
                                DefaultComponent = cp.DefaultComponent,
                                SourceId = cp.SourceId,
                                CurrencyPairRequests = cp.CurrencyPairRequests
                                    .Where(cpr => cpr.DeletedAt == null && cpr.IsEnabled)
                                    .Select(cpr => new CurrencyPairRequest
                                    {
                                        Id = cpr.Id,
                                        Guid = cpr.Guid,
                                        RequestType = cpr.RequestType,
                                        DataPath = cpr.DataPath,
                                        Delay = cpr.Delay,
                                        FailureDelay = cpr.FailureDelay,
                                        RequestComponents = cpr.RequestComponents
                                            .Select(rc => new RequestComponent
                                            {
                                                Id = rc.Id,
                                                ComponentType = rc.ComponentType,
                                                Identifier = rc.Identifier,
                                                QueryComponent = rc.QueryComponent,
                                                IsDenominated = rc.IsDenominated,
                                                AnomalyIgnorance = rc.AnomalyIgnorance,
                                                Value = rc.Value,
                                                RequestId = rc.RequestId,
                                                RcdHistoricItems = rc.RcdHistoricItems
                                                    .Where(rcdhi => rcdhi.DeletedAt == null && rcdhi.IsEnabled)
                                                    .Skip(index * NozomiServiceConstants.RequestComponentTakeoutLimit)
                                                    .Take(NozomiServiceConstants.RequestComponentTakeoutLimit)
                                                    .ToList()
                                            })
                                            .ToList()
                                    })
                                    .ToList(),
                                WebsocketRequests = cp.WebsocketRequests
                                    .Where(wsr => wsr.DeletedAt == null && wsr.IsEnabled)
                                    .Select(wsr => new WebsocketRequest
                                    {
                                        Id = wsr.Id,
                                        Guid = wsr.Guid,
                                        RequestType = wsr.RequestType,
                                        DataPath = wsr.DataPath,
                                        Delay = wsr.Delay,
                                        FailureDelay = wsr.FailureDelay,
                                        RequestComponents = wsr.RequestComponents
                                            .Select(rc => new RequestComponent
                                            {
                                                Id = rc.Id,
                                                ComponentType = rc.ComponentType,
                                                Identifier = rc.Identifier,
                                                QueryComponent = rc.QueryComponent,
                                                IsDenominated = rc.IsDenominated,
                                                AnomalyIgnorance = rc.AnomalyIgnorance,
                                                Value = rc.Value,
                                                RequestId = rc.RequestId,
                                                RcdHistoricItems = rc.RcdHistoricItems
                                                    .Where(rcdhi => rcdhi.DeletedAt == null && rcdhi.IsEnabled)
                                                    .Skip(index * NozomiServiceConstants.RequestComponentTakeoutLimit)
                                                    .Take(NozomiServiceConstants.RequestComponentTakeoutLimit)
                                                    .ToList()
                                            })
                                            .ToList()
                                    })
                                    .ToList()
                            });
                    }

                    var currencyPair = query.SingleOrDefault();

                    if (currencyPair != null)
                    {
                        foreach (var wsr in currencyPair.WebsocketRequests)
                        {
                            if (wsr.RequestComponents != null && wsr.RequestComponents.Count > 0)
                                res.AddRange(wsr.RequestComponents);
                        }

                        foreach (var cpr in currencyPair.CurrencyPairRequests)
                        {
                            if (cpr.RequestComponents != null && cpr.RequestComponents.Count > 0)
                                res.AddRange(cpr.RequestComponents);
                        }
                
                        return res;
                    }
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
                        query.ThenInclude(rc => rc.RcdHistoricItems)
                            .Select(cr => new CurrencyRequest
                            {
                                Id = cr.Id,
                                Guid = cr.Guid,
                                RequestType = cr.RequestType,
                                DataPath = cr.DataPath,
                                Delay = cr.Delay,
                                FailureDelay = cr.FailureDelay,
                                RequestComponents = cr.RequestComponents
                                    .Select(rc => new RequestComponent
                                    {
                                        Id = rc.Id,
                                        ComponentType = rc.ComponentType,
                                        Identifier = rc.Identifier,
                                        QueryComponent = rc.QueryComponent,
                                        IsDenominated = rc.IsDenominated,
                                        AnomalyIgnorance = rc.AnomalyIgnorance,
                                        Value = rc.Value,
                                        RequestId = rc.RequestId,
                                        RcdHistoricItems = rc.RcdHistoricItems
                                            .Where(rcdhi => rcdhi.DeletedAt == null && rcdhi.IsEnabled)
                                            .Skip(index * NozomiServiceConstants.RequestComponentTakeoutLimit)
                                            .Take(NozomiServiceConstants.RequestComponentTakeoutLimit)
                                            .ToList()
                                    })
                                    .ToList()
                            });
                    }

                    var currencyReqs = query.ToList();

                    foreach (var cr in currencyReqs)
                    {
                        if (cr.RequestComponents != null && cr.RequestComponents.Count > 0)
                            res.AddRange(cr.RequestComponents);
                    }
                
                    return res;
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
        public ICollection<RequestComponent> GetAllByCurrency(long currencyId, bool track = false, int index = 0)
        {
            // First, obtain the currency in question
            var qCurrency = _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
                .Where(c => c.Id.Equals(currencyId))
                .Include(c => c.CurrencyRequests)
                .ThenInclude(cr => cr.RequestComponents);

            if (qCurrency.SingleOrDefault() == null) return null;

            if (track)
            {
                qCurrency.ThenInclude(rc => rc.RcdHistoricItems);
            }
            
            return qCurrency
                .Where(c => c.CurrencyRequests.Count > 0 
                            && c.CurrencyRequests.Any(cr => cr.RequestComponents
                                   .Any(rc => rc.DeletedAt == null && rc.IsEnabled)))
                .SelectMany(cpr => cpr.CurrencyRequests
                    .SelectMany(cr => cr.RequestComponents
                    .Select(rc => new RequestComponent
                    {
                        Id = rc.Id,
                        ComponentType = rc.ComponentType,
                        Identifier = rc.Identifier,
                        IsDenominated = rc.IsDenominated,
                        QueryComponent = rc.QueryComponent,
                        Value = rc.Value,
                        RcdHistoricItems = rc.RcdHistoricItems
                            .Skip(index * NozomiServiceConstants.RequestComponentTakeoutLimit)
                            .Take(NozomiServiceConstants.RequestComponentTakeoutLimit)
                            .ToList()
                    })))
                .ToList();
        }

        public ICollection<RequestComponent> GetAllTickerPairCompsByCurrency(long currencyId, bool track = false)
        {
            // First, obtain the currency in question
            var qCurrency = _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
                .Where(c => c.Id.Equals(currencyId));

            if (qCurrency.SingleOrDefault() == null) return null;

            if (track)
            {
                qCurrency = qCurrency
                    .Include(c => c.CurrencySources)
                        .ThenInclude(cs => cs.Source)
                            .ThenInclude(s => s.CurrencyPairs)
                                .ThenInclude(cp => cp.CurrencyPairRequests)
                                    .ThenInclude(cpr => cpr.RequestComponents)
                                        .ThenInclude(rc => rc.RcdHistoricItems)
                    .Include(c => c.CurrencySources)
                        .ThenInclude(cs => cs.Source)
                            .ThenInclude(s => s.CurrencyPairs)
                                .ThenInclude(cp => cp.WebsocketRequests)
                                    .ThenInclude(cpr => cpr.RequestComponents)
                                        .ThenInclude(rc => rc.RcdHistoricItems);
            }
            else
            {
                qCurrency = qCurrency
                    .Include(c => c.CurrencySources)
                        .ThenInclude(cs => cs.Source)
                            .ThenInclude(s => s.CurrencyPairs)
                                .ThenInclude(cp => cp.CurrencyPairRequests)
                                    .ThenInclude(cpr => cpr.RequestComponents)
                    .Include(c => c.CurrencySources)
                        .ThenInclude(cs => cs.Source)
                            .ThenInclude(s => s.CurrencyPairs)
                                .ThenInclude(cp => cp.WebsocketRequests)
                                    .ThenInclude(cpr => cpr.RequestComponents);
            }

            return qCurrency
                .SelectMany(c => c.CurrencySources
                    .Where(cs => cs.IsEnabled && cs.DeletedAt == null)
                    .SelectMany(cs => cs.Source
                        .CurrencyPairs
                        .Where(cp => cp.IsEnabled && cp.DeletedAt == null
                                     && cp.CounterCurrencyAbbrv.Equals(CoreConstants.GenericCounterCurrency))
                        .SelectMany(cp => cp.CurrencyPairRequests
                            .Where(cpr => cpr.IsEnabled && cpr.DeletedAt == null)
                            .SelectMany(cpr => cpr.RequestComponents
                                .Where(rc => rc.IsEnabled && rc.DeletedAt == null)
                                .Select(rc => new RequestComponent
                                {
                                    Id = rc.Id,
                                    ComponentType = rc.ComponentType,
                                    Identifier = rc.Identifier,
                                    QueryComponent = rc.QueryComponent,
                                    IsDenominated = rc.IsDenominated,
                                    AnomalyIgnorance = rc.AnomalyIgnorance,
                                    Value = rc.Value,
                                    RequestId = rc.RequestId,
                                    RcdHistoricItems = rc.RcdHistoricItems
                                        .Where(rcdhi => rcdhi.IsEnabled && rcdhi.DeletedAt == null)
                                        .ToList()
                                })))))
                .Concat(qCurrency
                    .SelectMany(c => c.CurrencySources
                        .Where(cs => cs.IsEnabled && cs.DeletedAt == null)
                        .SelectMany(cs => cs.Source
                            .CurrencyPairs
                            .Where(cp => cp.IsEnabled && cp.DeletedAt == null)
                            .SelectMany(cp => cp.WebsocketRequests
                                .Where(cpr => cpr.IsEnabled && cpr.DeletedAt == null)
                                .SelectMany(cpr => cpr.RequestComponents
                                    .Where(rc => rc.IsEnabled && rc.DeletedAt == null)
                                    .Select(rc => new RequestComponent
                                    {
                                        Id = rc.Id,
                                        ComponentType = rc.ComponentType,
                                        Identifier = rc.Identifier,
                                        QueryComponent = rc.QueryComponent,
                                        IsDenominated = rc.IsDenominated,
                                        AnomalyIgnorance = rc.AnomalyIgnorance,
                                        Value = rc.Value,
                                        RequestId = rc.RequestId,
                                        RcdHistoricItems = rc.RcdHistoricItems
                                            .Where(rcdhi => rcdhi.IsEnabled && rcdhi.DeletedAt == null)
                                            .ToList()
                                    }))))))
                .ToList();

//            return qCurrency
//                .Where(c => c.CurrencyRequests.Count > 0 
//                            && c.CurrencyRequests.Any(cr => cr.RequestComponents
//                                .Any(rc => rc.DeletedAt == null && rc.IsEnabled)))
//                .SelectMany(cpr => cpr.CurrencyRequests
//                    .SelectMany(cr => cr.RequestComponents
//                        .Select(rc => new RequestComponent
//                        {
//                            Id = rc.Id,
//                            ComponentType = rc.ComponentType,
//                            Identifier = rc.Identifier,
//                            IsDenominated = rc.IsDenominated,
//                            QueryComponent = rc.QueryComponent,
//                            Value = rc.Value,
//                            RcdHistoricItems = rc.RcdHistoricItems
//                        })))
//                .ToList();
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
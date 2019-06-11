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
    public class RequestComponentEvent : BaseEvent<RequestComponentEvent, NozomiDbContext, RequestComponent>, 
        IRequestComponentEvent
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

        public long GetPredicateCount(Expression<Func<RequestComponent, bool>> predicate)
        {
            if (predicate == null)
                return long.MinValue;

            return QueryCount(predicate);
        }

        public ICollection<RequestComponent> GetByMainCurrency(string mainCurrencyAbbrv,
            ICollection<ComponentType> componentTypes)
        {
            return _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable(cp => cp.DeletedAt == null && cp.IsEnabled)
                .AsNoTracking()
                .Where(cp => cp.MainCurrencyAbbrv.Equals(mainCurrencyAbbrv, StringComparison.InvariantCultureIgnoreCase))
                .Include(cp => cp.Requests)
                .ThenInclude(cpr => cpr.RequestComponents)
                .SelectMany(cp => cp.Requests
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
            , int index = 0, Func<RequestComponent, bool> predicate = null)
        {
            var analysedComponent = _unitOfWork.GetRepository<AnalysedComponent>()
                .GetQueryable()
                .AsNoTracking()
                .SingleOrDefault(ac => ac.Id.Equals(analysedComponentId));

            if (analysedComponent != null)
            {
                var query = _unitOfWork.GetRepository<Request>()
                    .GetQueryable()
                    .AsNoTracking();
                
                // CurrencyPair-based tracking
                if (analysedComponent.CurrencyPairId != null && analysedComponent.CurrencyPairId > 0)
                {
                    query = query
                        .Where(r => r.DeletedAt == null && r.IsEnabled
                                                          && r.CurrencyPairId.Equals(analysedComponent.CurrencyPairId))
                        .Include(cpr => cpr.RequestComponents);

                    if (track)
                    {
                        query
                            .Include(r => r.RequestComponents)
                            .ThenInclude(rc => rc.RcdHistoricItems);
                    }
                    
                    if (predicate != null)
                        return query
                            .SelectMany(r => r.RequestComponents
                                .Where(predicate)
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
                                }))
                            .ToList();

                    return query
                        .SelectMany(r => r.RequestComponents
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
                                }))
                        .ToList();
                } 
                // Currency-based tracking
                else if (analysedComponent.CurrencyId != null && analysedComponent.CurrencyId > 0)
                {
                    query = query
                        .Where(r => r.CurrencyId.Equals(analysedComponent.CurrencyId))
                        .Include(cr => cr.RequestComponents);

                    if (track)
                    {
                        query
                            .Include(cr => cr.RequestComponents)
                            .ThenInclude(rc => rc.RcdHistoricItems);
                    }
                    
                    if (predicate != null)
                        return query
                            .SelectMany(r => r.RequestComponents
                                .Where(predicate)
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
                                }))
                            .ToList();

                    return query
                        .SelectMany(cr => cr.RequestComponents
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
                            }))
                        .ToList();
                }
            }

            return null;
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
                .Include(c => c.Requests)
                .ThenInclude(cr => cr.RequestComponents);

            if (qCurrency.SingleOrDefault() == null) return null;

            if (track)
            {
                qCurrency.ThenInclude(rc => rc.RcdHistoricItems);
            }
            
            return qCurrency
                .Where(c => c.Requests.Count > 0 
                            && c.Requests.Any(cr => cr.RequestComponents
                                   .Any(rc => rc.DeletedAt == null && rc.IsEnabled)))
                .SelectMany(cpr => cpr.Requests
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

        public ICollection<RequestComponent> GetAllTickerPairCompsByCurrency(long currencyId, bool track = false, 
            int index = 0)
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
                    .ThenInclude(cp => cp.Requests)
                    .ThenInclude(cpr => cpr.RequestComponents)
                    .ThenInclude(rc => rc.RcdHistoricItems);
            }
            else
            {
                qCurrency = qCurrency
                    .Include(c => c.CurrencySources)
                    .ThenInclude(cs => cs.Source)
                    .ThenInclude(s => s.CurrencyPairs)
                    .ThenInclude(cp => cp.Requests)
                    .ThenInclude(cpr => cpr.RequestComponents);
            }

            return qCurrency
                .SelectMany(c => c.CurrencySources
                    .Where(cs => cs.IsEnabled && cs.DeletedAt == null)
                    .SelectMany(cs => cs.Source
                        .CurrencyPairs
                        .Where(cp => cp.IsEnabled && cp.DeletedAt == null
                                                  && cp.CounterCurrencyAbbrv.Equals(
                                                      CoreConstants.GenericCounterCurrency))
                        .SelectMany(cp => cp.Requests
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
                                        .Skip(index * NozomiServiceConstants.RequestComponentTakeoutLimit)
                                        .Take(NozomiServiceConstants.RequestComponentTakeoutLimit)
                                        .ToList()
                                })))))
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
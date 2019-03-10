using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core.Helpers.Enumerable;
using Nozomi.Data;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
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
            
            return includeNested ? 
                _unitOfWork.GetRepository<RequestComponent>()
                    .GetQueryable(rc => rc.RequestId.Equals(requestId) && rc.DeletedAt == null && rc.IsEnabled)
                    .AsNoTracking()
                    .Include(rc => rc.RequestComponentDatum)
                    .Include(rc => rc.Request)
                    .ToList() :
                _unitOfWork.GetRepository<RequestComponent>()
                    .GetQueryable(rc => rc.RequestId.Equals(requestId) && rc.DeletedAt == null && rc.IsEnabled)
                    .AsNoTracking()
                    .ToList();
        }

        public ICollection<RequestComponent> All(int index = 0, bool includeNested = false)
        {
            return includeNested ?
                _unitOfWork.GetRepository<RequestComponent>()
                    .GetQueryable(rc => rc.DeletedAt == null && rc.IsEnabled)
                    .AsNoTracking()
                    .Include(rc => rc.RequestComponentDatum)
                    .Include(rc => rc.Request)
                    .Skip(index * 20)
                    .Take(20)
                    .ToList() :
                _unitOfWork.GetRepository<RequestComponent>()
                    .GetQueryable(rc => rc.DeletedAt == null && rc.IsEnabled)
                    .AsNoTracking()
                    .Skip(index * 20)
                    .Take(20)
                    .ToList();
        }

        public decimal ComputeDifference(string baseCurrencyAbbrv, string comparingCurrencyAbbrv, ComponentType componentType)
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
                            .Include(cp => cp.PartialCurrencyPairs)
                            .ThenInclude(pcp => pcp.Currency)
                            .Where(cp => cp.PartialCurrencyPairs.FirstOrDefault(pcp => pcp.IsMain).Currency.Abbrv
                                             .Equals(baseCurrencyAbbrv, StringComparison.InvariantCultureIgnoreCase)
                                         && cp.PartialCurrencyPairs.FirstOrDefault(pcp => !pcp.IsMain).Currency.Abbrv
                                             .Equals(comparingCurrencyAbbrv, StringComparison.InvariantCultureIgnoreCase))
                            .Include(cp => cp.CurrencyPairRequests
                                .Where(cpr => cpr.IsEnabled && cpr.DeletedAt == null))
                            .ThenInclude(cpr => cpr.RequestComponents
                                .Where(rc => rc.ComponentType.Equals(componentType)))
                            .ThenInclude(rc => rc.RequestComponentDatum)
                            .SelectMany(cp => cp.CurrencyPairRequests
                                .SelectMany(cpr => cpr.RequestComponents
                                    .Select(rc => decimal.Parse(rc.RequestComponentDatum.Value))))
                            .DefaultIfEmpty()
                            .Average();
                    default:
                        // Can't compute lol.
                        return decimal.Zero;
                }
            }

            return decimal.Zero;
        }

        public ICollection<RequestComponent> GetByMainCurrency(string mainCurrencyAbbrv, 
            ICollection<ComponentType> componentTypes)
        {
            return _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable(cp => cp.DeletedAt == null && cp.IsEnabled)
                .AsNoTracking()
                .Include(cp => cp.PartialCurrencyPairs)
                .ThenInclude(pcp => pcp.Currency)
                .Where(cp => cp.PartialCurrencyPairs
                    .FirstOrDefault(pcp => pcp.IsMain).Currency.Abbrv.Equals(mainCurrencyAbbrv,
                        StringComparison.InvariantCultureIgnoreCase))
                .Include(cp => cp.CurrencyPairRequests)
                .ThenInclude(cpr => cpr.RequestComponents)
                .ThenInclude(rc => rc.RequestComponentDatum)
                .SelectMany(cp => cp.CurrencyPairRequests
                    .Where(cpr => cpr.RequestComponents != null && cpr.RequestComponents.Count > 0))
                .SelectMany(cpr => cpr.RequestComponents)
                .ToList();
        }

        /// <summary>
        /// Allows the caller to obtain all RequestComponents relevant to the currency
        /// pair in question via the abbreviation method. (i.e. ETHUSD)
        /// </summary>
        /// <param name="analysedComponentId">The unique identifier of the analysed component
        /// that is related to the ticker in question.</param>
        /// <returns>Collection of request components related to the component</returns>
        public ICollection<RequestComponent> GetAllByCorelation(long analysedComponentId)
        {
            // First, obtain the correlation PCPs
            var correlPCPs = _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cp => cp.IsEnabled && cp.DeletedAt == null)
                .Include(cp => cp.CurrencyPairRequests
                    .Where(cpr => cpr.DeletedAt == null && cpr.IsEnabled))
                .ThenInclude(cpr => cpr.AnalysedComponents
                        // We can ignore disabled or deleted ACs, just using this 
                        // to find the correlation
                    .Where(ac => ac.Id.Equals(analysedComponentId)))
                .Include(cp => cp.PartialCurrencyPairs)
                .ThenInclude(pcp => pcp.Currency)
                .SelectMany(cp => cp.PartialCurrencyPairs)
                .ToList();
            
            // Then we return
            return _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cp => cp.IsEnabled && cp.DeletedAt == null)
                .Include(cp => cp.PartialCurrencyPairs)
                .ThenInclude(pcp => pcp.Currency)
                .Where(cp => 
                    // Make sure the main currencies are identical
                    cp.PartialCurrencyPairs.FirstOrDefault(pcp => pcp.IsMain).Currency.Abbrv
                        .Equals(correlPCPs.FirstOrDefault(pcp => pcp.IsMain).Currency.Abbrv)
                    // Make sure the counter currencies are identical
                    && cp.PartialCurrencyPairs.FirstOrDefault(pcp => !pcp.IsMain).Currency.Abbrv
                        .Equals(correlPCPs.FirstOrDefault(pcp => !pcp.IsMain).Currency.Abbrv))
                .Include(cp => cp.CurrencyPairRequests
                    .Where(cpr => cpr.IsEnabled && cpr.DeletedAt == null))
                .ThenInclude(cpr => cpr.RequestComponents
                    .Where(rc => rc.IsEnabled && rc.DeletedAt == null))
                .ThenInclude(rc => rc.RequestComponentDatum)
                .SelectMany(cp => cp.CurrencyPairRequests
                    .SelectMany(cpr => cpr.RequestComponents))
                .ToList();
        }

        public NozomiResult<RequestComponent> Get(long id, bool includeNested = false)
        {
            if (includeNested)
                return new NozomiResult<RequestComponent>(_unitOfWork.GetRepository<RequestComponent>().GetQueryable()
                    .Include(rc => rc.Request)
                    .Include(rc => rc.RequestComponentDatum)
                    .SingleOrDefault(rc => rc.Id.Equals(id) && rc.IsEnabled && rc.DeletedAt == null));
            
            return new NozomiResult<RequestComponent>(_unitOfWork.GetRepository<RequestComponent>()
                .Get(rc => rc.Id.Equals(id) && rc.DeletedAt == null && rc.IsEnabled)
                .SingleOrDefault());
        }
    }
}
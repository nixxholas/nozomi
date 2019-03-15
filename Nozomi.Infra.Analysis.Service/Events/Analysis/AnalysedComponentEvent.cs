using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Infra.Analysis.Service.Events.Analysis.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;

namespace Nozomi.Infra.Analysis.Service.Events.Analysis
{
    public class AnalysedComponentEvent : BaseEvent<AnalysedComponentEvent, NozomiDbContext>, IAnalysedComponentEvent
    {
        public AnalysedComponentEvent(ILogger<AnalysedComponentEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public IEnumerable<AnalysedComponent> GetAll(bool filter = false, bool track = false)
        {
            var query = _unitOfWork.GetRepository<AnalysedComponent>()
                .GetQueryable()
                .AsNoTracking();

            if (filter)
            {
                query.Where(ac => ac.IsEnabled && ac.DeletedAt == null
                                  &&  (ac.Delay == 0 || 
                                       DateTime.UtcNow > ac.ModifiedAt.AddMilliseconds(ac.Delay).ToUniversalTime()));
            }
            
            if (track)
            {
                query
                    .Include(ac => ac.AnalysedHistoricItems)
                    .Include(ac => ac.Request)
                    .ThenInclude(r => r.RequestComponents)
                    .ThenInclude(rc => rc.RequestComponentDatum)
                    .ThenInclude(rcd => rcd.RcdHistoricItems);
            }

            return query;
        }

        public ICollection<AnalysedComponent> GetAllByCurrency(long currencyId)
        {
            // First, obtain the currency in question
            var qCurrency = _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
                .SingleOrDefault(c => c.Id.Equals(currencyId));

            if (qCurrency == null) return null;

            // Then we return
            var finalQuery = _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cp => cp.IsEnabled && cp.DeletedAt == null)
                .Include(cp => cp.PartialCurrencyPairs)
                .ThenInclude(pcp => pcp.Currency)
                .Where(cp =>
                    // Make sure the main currencies are identical
                    cp.PartialCurrencyPairs.FirstOrDefault(pcp => pcp.IsMain).Currency.Abbrv
                        .Equals(qCurrency.Abbrv, StringComparison.InvariantCultureIgnoreCase))
                .Include(cp => cp.CurrencyPairRequests)
                .ThenInclude(cpr => cpr.AnalysedComponents)
                .ThenInclude(rc => rc.AnalysedHistoricItems)
                .Where(cp => cp.CurrencyPairRequests.Any(cpr => cpr.IsEnabled && cpr.DeletedAt == null
                                                                              && cpr.AnalysedComponents
                                                                                  .Any(ac =>
                                                                                      ac.IsEnabled && ac.DeletedAt ==
                                                                                                   null
                                                                                                   && ac.AnalysedHistoricItems.Count > 0)))
                .SelectMany(cp => cp.CurrencyPairRequests)
                .SelectMany(cpr => cpr.AnalysedComponents);

            if (!finalQuery.Any()) return new List<AnalysedComponent>();
            
            return finalQuery
                .Select(ac => new AnalysedComponent
                {
                    Id = ac.Id,
                    ComponentType = ac.ComponentType,
                    AnalysedHistoricItems = ac.AnalysedHistoricItems
                })
                .ToList();
        }

        public string GetCurrencyAbbreviation(AnalysedComponent analysedComponent)
        {
            return _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
                .Where(c => c.DeletedAt == null && c.IsEnabled)
                .Include(c => c.AnalysedComponents)
                .FirstOrDefault(c => c.AnalysedComponents
                    .Any(ac => ac.Id.Equals(analysedComponent.Id)))
                ?.Abbrv;
        }
    }
}
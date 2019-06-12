using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class CurrencyPairEvent : BaseEvent<CurrencyPairEvent, NozomiDbContext>, ICurrencyPairEvent
    {
        public CurrencyPairEvent(ILogger<CurrencyPairEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public ICollection<CurrencyPair> GetAllByCounterCurrency(string counterCurrencyAbbrv = 
            CoreConstants.GenericCounterCurrency)
        {
            return _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cp => cp.DeletedAt == null && cp.IsEnabled
                             && cp.CounterCurrencyAbbrv.Equals(counterCurrencyAbbrv, 
                                 StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        public ICollection<CurrencyPair> GetAllByTickerPairAbbreviation(string tickerPairAbbreviation, bool track = false)
        {
            if (!string.IsNullOrEmpty(tickerPairAbbreviation))
            {
                var query = _unitOfWork.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(cp => cp.DeletedAt == null && cp.IsEnabled 
                        && string.Concat(cp.MainCurrencyAbbrv, cp.CounterCurrencyAbbrv)
                        .Equals(tickerPairAbbreviation, StringComparison.InvariantCultureIgnoreCase));

                if (track)
                {
                    query.Include(cp => cp.Source)
                        .ThenInclude(s => s.SourceCurrencies)
                        .ThenInclude(sc => sc.Currency);
                }

                return query.ToList();
            }

            return null;
        }

        public ICollection<AnalysedComponent> GetAnalysedComponents(long analysedComponentId, bool track = false)
        {
            if (analysedComponentId <= 0)
            {
                var query = _unitOfWork.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                    .Include(cp => cp.AnalysedComponents)
                    // Look for the Currency pair that contains the component requested
                    .Where(cp => cp.AnalysedComponents
                        .Any(ac => ac.Id.Equals(analysedComponentId) && ac.CurrencyPairId.Equals(cp.Id)));

                if (track)
                {
                    query = query
                        .Include(cp => cp.Requests)
                        .ThenInclude(r => r.RequestComponents)
                        .Include(cp => cp.AnalysedComponents)
                        .ThenInclude(ac => ac.AnalysedHistoricItems);
                }

                return query
                    .SingleOrDefault()
                    ?.AnalysedComponents;
            }

            return null;
        }
    }
}
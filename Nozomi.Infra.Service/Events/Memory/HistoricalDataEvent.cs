using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ResponseModels;
using Nozomi.Data.ResponseModels.Currency;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Memory.Interfaces;

namespace Nozomi.Service.Events.Memory
{
    public class HistoricalDataEvent : BaseEvent<HistoricalDataEvent, NozomiDbContext>, IHistoricalDataEvent
    {
        public HistoricalDataEvent(ILogger<HistoricalDataEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork)
            : base(logger, unitOfWork)
        {
        }

        public ICollection<DistinctiveCurrencyResponse> GetSimpleCurrencyHistory(long sourceId, long days = 7)
        {
            if (!_unitOfWork.GetRepository<Source>().GetQueryable().Any(s => s.Id.Equals(sourceId)))
                return new List<DistinctiveCurrencyResponse>();

            return _unitOfWork.GetRepository<Source>().GetQueryable()
                       .Include(s => s.Currencies)
                       .ThenInclude(c => c.PartialCurrencyPairs)
                       .ThenInclude(pcp => pcp.CurrencyPair)
                       .ThenInclude(cp => cp.CurrencyPairRequests)
                       .ThenInclude(cpr => cpr.RequestComponents)
                       .ThenInclude(rc => rc.RequestComponentDatum)
                       .ThenInclude(rcd => rcd.RcdHistoricItems)
                       .SingleOrDefault(s => s.Id.Equals(sourceId))
                       ?.Currencies
                       .Select(c => new DistinctiveCurrencyResponse
                       {
                           Name = c.Name,
                           Abbreviation = c.Abbrv,
                           LastUpdated = c.ModifiedAt,
//                           WeeklyAvgPrice = c.PartialCurrencyPairs
//                               .Select(pcp => pcp.CurrencyPair)
//                               .SelectMany(cp => cp.CurrencyPairRequests
//                                   .SelectMany(cpr => cpr.RequestComponents
//                                       .Where(rc =>
//                                           rc.ComponentType.Equals(ComponentType.Ask)
//                                           || rc.ComponentType.Equals(ComponentType.Bid))
//                                       .SelectMany(rc => rc.RequestComponentDatum
//                                           .RcdHistoricItems
//                                           .Where(rcdhi => rcdhi.CreatedAt >
//                                                           DateTime.UtcNow.Subtract(TimeSpan.FromDays(days)))
//                                           .Select(rcdhi => decimal.Parse(rcdhi.Value))
//                                           .DefaultIfEmpty(0))
//                                   ))
//                               .Average(),
//                           DailyVolume = c.PartialCurrencyPairs
//                               .Select(pcp => pcp.CurrencyPair)
//                               .SelectMany(cp => cp.CurrencyPairRequests
//                                   .SelectMany(cpr => cpr.RequestComponents
//                                       .Where(rc => rc.ComponentType.Equals(ComponentType.VOLUME))
//                                       .Select(rc => decimal.Parse(rc.RequestComponentDatum.Value))))
//                               .FirstOrDefault()
                       })
                       .ToList() ?? null;
        }
    }
}
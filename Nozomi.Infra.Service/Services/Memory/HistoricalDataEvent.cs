using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using Nozomi.Data.CurrencyModels;
using Nozomi.Data.ResponseModels;
using Nozomi.Preprocessing;
using Nozomi.Service.Services.Memory.Interfaces;

namespace Nozomi.Service.Services.Memory
{
    public class HistoricalDataEvent : IHistoricalDataEvent
    {
        public ICollection<DistinctiveCurrencyResponse> GetSimpleCurrencyHistory(long sourceId, long days = 7)
        {
            if (!NozomiServiceConstants.Sources.Any(s => s.Id.Equals(sourceId))) 
                return new List<DistinctiveCurrencyResponse>();

//            var waprice = NozomiServiceConstants.Sources
//                .SingleOrDefault(s => s.Id.Equals(sourceId))
//                ?.Currencies
//                .Select(c => c.PartialCurrencyPairs
//                .Select(pcp => pcp.CurrencyPair)
//                .SelectMany(cp => cp.CurrencyPairRequests
//                    .SelectMany(cpr => cpr.RequestComponents
//                        .SelectMany(rc => rc.RequestComponentDatum
//                            .RcdHistoricItems
//                            .Where(rcdhi => rcdhi.CreatedAt >
//                                            DateTime.UtcNow.Subtract(TimeSpan.FromDays(days)))
//                            .Select(rcdhi => decimal.Parse(rcdhi.Value))
//                            .DefaultIfEmpty(0))
//                    ))
//                .Average())
//            ;
            
            return NozomiServiceConstants.Sources
                .SingleOrDefault(s => s.Id.Equals(sourceId))
                ?.Currencies
                .Select(c => new DistinctiveCurrencyResponse
                    {
                        Name = c.Name,
                        Abbreviation = c.Abbrv,
                        LastUpdated = c.ModifiedAt,
                        WeeklyAvgPrice = c.PartialCurrencyPairs
                            .Select(pcp => pcp.CurrencyPair)
                            .SelectMany(cp => cp.CurrencyPairRequests
                                .SelectMany(cpr => cpr.RequestComponents
                                    .Where(rc => 
                                        rc.ComponentType.Equals(ComponentType.Ask)
                                    || rc.ComponentType.Equals(ComponentType.Bid))
                                    .SelectMany(rc => rc.RequestComponentDatum
                                        .RcdHistoricItems
                                        .Where(rcdhi => rcdhi.CreatedAt >
                                                        DateTime.UtcNow.Subtract(TimeSpan.FromDays(days)))
                                        .Select(rcdhi => decimal.Parse(rcdhi.Value))
                                        .DefaultIfEmpty(0))
                                ))
                            .Average(),
                        DailyVolume = c.PartialCurrencyPairs
                            .Select(pcp => pcp.CurrencyPair)
                            .SelectMany(cp => cp.CurrencyPairRequests
                                .SelectMany(cpr => cpr.RequestComponents
                                    .Where(rc => rc.ComponentType.Equals(ComponentType.VOLUME))
                                    .SelectMany(rc => rc.RequestComponentDatum
                                        .RcdHistoricItems
                                        .Where(rcdhi => rcdhi.CreatedAt >
                                                        DateTime.UtcNow.Subtract(TimeSpan.FromDays(days)))
                                        .Select(rcdhi => decimal.Parse(rcdhi.Value))
                                        .DefaultIfEmpty(0))
                                ))
                            .Sum()
                    })
                .ToList();
        }
    }
}
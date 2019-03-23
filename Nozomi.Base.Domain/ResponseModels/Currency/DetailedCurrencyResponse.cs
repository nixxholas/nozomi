using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Nozomi.Base.Core;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.ResponseModels.RequestComponent;

namespace Nozomi.Data.ResponseModels.Currency
{
    /// <summary>
    /// Detailed Currency Response specific to a currency.
    /// </summary>
    public class DetailedCurrencyResponse : DistinctiveCurrencyResponse
    {
        public DetailedCurrencyResponse()
        {
        }

        public DetailedCurrencyResponse(Models.Currency.Currency currency)
        {
            if (currency != null)
            {
                Name = currency.Name;
                Abbreviation = currency.Abbrv;
                LastUpdated = DateTime.UtcNow;

                if (currency.PartialCurrencyPairs != null && currency.PartialCurrencyPairs.Count > 0)
                {
                    // Obtain via the Request method
                    var query = currency.PartialCurrencyPairs
                        // TODO: Need to factor in the counter currency.
                            // Make sure all PCPs obtained have this currency as the main.
                            .Where(pcp => pcp.IsMain && pcp.Currency.Abbrv.Equals(currency.Abbrv,
                                               StringComparison.InvariantCultureIgnoreCase)
                                          && pcp.CurrencyPair.PartialCurrencyPairs.FirstOrDefault(spcp => !spcp.IsMain)
                                              .Currency.Abbrv.Equals(CoreConstants.GenericCounterCurrency))
                            .Select(pcp => pcp.CurrencyPair)
                            .SelectMany(cpr => cpr.CurrencyPairRequests)
                            .Where(cpr => cpr.DeletedAt == null && cpr.IsEnabled)
                            .SelectMany(cpr => cpr.AnalysedComponents)
                            .ToList();

                    if (query.Count > 0)
                    {
                        foreach (var aComp in query)
                        {
                            switch (aComp.ComponentType)
                            {
                                case AnalysedComponentType.CurrentAveragePrice:
                                    if (AveragePrice <= 0 
                                        && decimal.TryParse(aComp.Value, out var avgPrice))
                                    {
                                        AveragePrice = avgPrice;
                                    }
                                    break;
                                case AnalysedComponentType.DailyPricePctChange:
                                    if (DailyAvgPricePctChange.Equals(-200)
                                    && decimal.TryParse(aComp.Value, out var dailyAvgPricePctChange))
                                    {
                                        DailyAvgPricePctChange = dailyAvgPricePctChange;
                                    }
                                    break;
                            }
                        }
                    }
                }

                if (currency.AnalysedComponents.Any(ac =>
                    ac.DeletedAt == null && ac.IsEnabled
                                         && ac.ComponentType.Equals(AnalysedComponentType.CurrentAveragePrice)))
                {
                    var currencyAP = currency.AnalysedComponents.FirstOrDefault(ac =>
                            ac.DeletedAt == null && ac.IsEnabled
                                                 && ac.ComponentType.Equals(AnalysedComponentType.CurrentAveragePrice))
                        ?.Value;

                    if (string.IsNullOrEmpty(currencyAP)
                        && currencyAP.Equals("0", StringComparison.InvariantCultureIgnoreCase))
                    {
                        AveragePrice = decimal.Parse(currencyAP);
                    }
                }
                // Daily average percentage change via the Currency
                if (currency.AnalysedComponents.Any(ac =>
                    ac.DeletedAt == null && ac.IsEnabled
                                         && ac.ComponentType.Equals(AnalysedComponentType.DailyPricePctChange)))
                {
                    var currencyDAPPC = currency.AnalysedComponents.FirstOrDefault(ac =>
                            ac.DeletedAt == null && ac.IsEnabled
                                                 && ac.ComponentType.Equals(AnalysedComponentType.DailyPricePctChange))
                        ?.Value;

                    if (string.IsNullOrEmpty(currencyDAPPC)
                        && currencyDAPPC.Equals("0", StringComparison.InvariantCultureIgnoreCase))
                    {
                        DailyAvgPricePctChange = decimal.Parse(currencyDAPPC);
                    }
                }
                // Market cap is usually stored with a currency-based AC.
                // Thus we directly obtain the value like that.
                MarketCap = decimal.Parse(currency.AnalysedComponents.FirstOrDefault(ac =>
                                                  ac.DeletedAt == null && ac.IsEnabled
                                                                       && ac.ComponentType.Equals(AnalysedComponentType.MarketCap))
                                              ?.Value ?? "0");
            }
        }
        
        /// <summary>
        /// These constructors rely on the same counter currency to properly operate.
        /// </summary>
        /// <param name="currencyPairs"></param>
        public DetailedCurrencyResponse(IQueryable<Models.Currency.CurrencyPair> currencyPairs)
        {
            if (currencyPairs != null && currencyPairs.Any())
            {
                var curr = currencyPairs
                    .Select(cp => cp.PartialCurrencyPairs
                        .Select(pcp => pcp.Currency)
                        .FirstOrDefault())
                    .FirstOrDefault();

                if (curr != null)
                {
                    Name = curr.Name;
                    Abbreviation = curr.Abbrv;
                    LastUpdated = currencyPairs
                        .Select(cp => cp.CurrencyPairRequests
                            .Select(cpr => cpr.RequestComponents
                                // Always take the latest
                                .OrderBy(rc => rc.ModifiedAt)
                                .FirstOrDefault())
                            .FirstOrDefault().ModifiedAt)
                        .FirstOrDefault();
                    ConfigureHistoricals(currencyPairs.ToList());

                    // TODO: take conversion rates into account
//                    WeeklyAvgPrice = currencyPairs
//                        .SelectMany(cp => cp.PartialCurrencyPairs)
//                        .Select(pcp => pcp.CurrencyPair)
//                        .SelectMany(cp => cp.CurrencyPairRequests)
//                        .SelectMany(cpr => cpr.RequestComponents
//                            .Where(rc =>
//                                rc.ComponentType.Equals(ComponentType.Ask) ||
//                                rc.ComponentType.Equals(ComponentType.Bid)))
//                        .Select(rc => rc.RequestComponentDatum)
//                        .SelectMany(rcd => rcd.RcdHistoricItems
//                            .Where(rcdhi => rcdhi.CreatedAt >
//                                            DateTime.UtcNow.Subtract(TimeSpan.FromDays(7))))
//                        .Select(rcdhi => decimal.Parse(rcdhi.Value))
//                        .DefaultIfEmpty()
//                        .Average();

                    // TODO: take conversion rates into account
//                    DailyVolume = currencyPairs
//                        .SelectMany(cp => cp.PartialCurrencyPairs)
//                        .Select(pcp => pcp.CurrencyPair)
//                        .SelectMany(cp => cp.CurrencyPairRequests)
//                        .SelectMany(cpr => cpr.RequestComponents
//                            .Where(rc => rc.ComponentType.Equals(ComponentType.VOLUME)))
//                        .Select(rc => rc.RequestComponentDatum)
//                        .SelectMany(rcd => rcd.RcdHistoricItems
//                            .Where(rcdhi => rcdhi.CreatedAt >
//                                            DateTime.UtcNow.Subtract(TimeSpan.FromHours(24))))
//                        .Select(rcdhi => decimal.Parse(rcdhi.Value))
//                        .DefaultIfEmpty()
//                        .Sum();

//                    Historical = currencyPairs
//                        .SelectMany(cp => cp.PartialCurrencyPairs)
//                        .Select(pcp => pcp.CurrencyPair)
//                        .SelectMany(cp => cp.CurrencyPairRequests)
//                        .SelectMany(cpr => cpr.RequestComponents)
//                        .ToDictionary(rc => rc.ComponentType,
//                            rc => rc.RequestComponentDatum
//                                .RcdHistoricItems
//                                .DefaultIfEmpty()
//                                .Select(rcdhi => new ComponentHistoricalDatum
//                                {
//                                    CreatedAt = rcdhi.CreatedAt,
//                                    Value = rcdhi.Value
//                                })
//                                .ToList());
                }
            }
        }

        public DetailedCurrencyResponse(string abbrv, IQueryable<Models.Currency.CurrencyPair> currencyPairs)
        {
            if (currencyPairs != null && currencyPairs.Any())
            {
                #if DEBUG
                var cPairs = currencyPairs.ToList();
                #endif
                
                var curr = currencyPairs
                    .Select(cp => cp.PartialCurrencyPairs
                        .Select(pcp => pcp.Currency)
                        .FirstOrDefault(c => c.Abbrv.Equals(abbrv, StringComparison.InvariantCultureIgnoreCase)))
                    .FirstOrDefault();

                if (curr != null)
                {
                    Name = curr.Name;
                    Abbreviation = curr.Abbrv;
                    LastUpdated = currencyPairs
                        .Select(cp => cp.CurrencyPairRequests
                            .Select(cpr => cpr.RequestComponents
                                // Always take the latest
                                .OrderBy(rc => rc.ModifiedAt)
                                .FirstOrDefault())
                            .FirstOrDefault().ModifiedAt)
                        .FirstOrDefault();
                    ConfigureHistoricals(currencyPairs.ToList());
                }
            }
        }

        private void ConfigureHistoricals(List<Models.Currency.CurrencyPair> currencyPairs)
        {
            Historical = new Dictionary<ComponentType, List<ComponentHistoricalDatum>>();
            
            foreach (var reqComp in currencyPairs
                .SelectMany(cp => cp.CurrencyPairRequests
                    .SelectMany(cpr => cpr.RequestComponents)))
            {
                if (reqComp.RequestComponentDatum != null &&
                    reqComp.RequestComponentDatum.RcdHistoricItems != null &&
                    reqComp.RequestComponentDatum.RcdHistoricItems.Count > 0)
                {
                    var rcdhiList = reqComp.RequestComponentDatum.RcdHistoricItems;

                    foreach (var rcdhi in rcdhiList)
                    {
                        if (Historical.ContainsKey(reqComp.ComponentType))
                        {
                            Historical[reqComp.ComponentType].Add(new ComponentHistoricalDatum
                            {
                                CreatedAt = rcdhi.HistoricDateTime,
                                Value = rcdhi.Value
                            });
                        }
                        else
                        {
                            Historical.Add(reqComp.ComponentType, new List<ComponentHistoricalDatum>());
                        }
                    }
                }
            }
        }
        
        public decimal AveragePrice { get; set; }
        
        public decimal DailyAvgPricePctChange { get; set; }
        
        public decimal MarketCap { get; set; }

        public Dictionary<ComponentType, List<ComponentHistoricalDatum>> Historical { get; set; }
    }
}
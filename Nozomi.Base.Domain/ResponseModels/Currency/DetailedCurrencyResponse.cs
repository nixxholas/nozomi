using System;
using System.Collections.Generic;
using System.Globalization;
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
        /// <summary>
        /// Obtain the live average price, averaged across ALL sources.
        /// </summary>
        public decimal AveragePrice { get; set; }
        
        public decimal DailyAvgPctChange { get; set; }
        
        public decimal DailyVolume { get; set; }
        
        public decimal MarketCap { get; set; }

        public Dictionary<ComponentType, List<ComponentHistoricalDatum>> Historical { get; set; }

        public List<decimal> AveragePriceHistory { get; set; }

        public DetailedCurrencyResponse()
        {
        }

        public DetailedCurrencyResponse(ICollection<Models.Currency.Currency> currencies)
        {
            if (currencies != null && currencies.Count > 0)
            {
                var first = currencies.FirstOrDefault();

                if (first != null)
                {
                    // Aggregate non-compounded properties first
                    Name = first.Name;
                    Abbreviation = first.Abbrv;
                    LastUpdated = first.ModifiedAt;

                    foreach (var exchangeUniqueCurr in currencies)
                    {
                        // Direct AnalysedComponents first
                        foreach (var currAc in exchangeUniqueCurr.AnalysedComponents)
                        {
                            // Make sure the value is parse-able
                            if (!string.IsNullOrEmpty(currAc.Value) &&
                                decimal.TryParse(currAc.Value, out var val))
                            {
                                switch (currAc.ComponentType)
                                {
                                    case AnalysedComponentType.CurrentAveragePrice:
                                        // Is the AveragePrice used yet? 
                                        if (AveragePrice <= decimal.Zero)
                                        {
                                            AveragePrice = (AveragePrice + val) / 2;
                                        }
                                        // Nope
                                        else
                                        {
                                            AveragePrice = val;
                                        }

                                        break;
                                    case AnalysedComponentType.DailyPricePctChange:
                                        // Is the DailyAvgPctChange used yet? 
                                        if (DailyAvgPctChange != decimal.Zero)
                                        {
                                            DailyAvgPctChange = (DailyAvgPctChange + val) / 2;
                                        }
                                        // Nope
                                        else
                                        {
                                            DailyAvgPctChange = val;
                                        }
                                        break;
                                    case AnalysedComponentType.DailyVolume:
                                        // Is the DailyVolume used yet?
                                        if (DailyVolume <= decimal.Zero)
                                        {
                                            DailyVolume = (DailyVolume + val) / 2;
                                        }
                                        // Nope
                                        else
                                        {
                                            DailyVolume = val;
                                        }
                                        break;
                                    case AnalysedComponentType.MarketCap:
                                        // Is the MarketCap used yet?
                                        if (MarketCap <= decimal.Zero)
                                        {
                                            MarketCap = (MarketCap + val) / 2;
                                        }
                                        // Nope
                                        else
                                        {
                                            MarketCap = val;
                                        }
                                        break;
                                    case AnalysedComponentType.HourlyAveragePrice:
                                        if (AveragePriceHistory == null || AveragePriceHistory.Count == 0)
                                        {
                                            AveragePriceHistory = new List<decimal>()
                                            {
                                                decimal.Parse(currAc.Value)
                                            };
                                            
                                            AveragePriceHistory.AddRange(currAc.AnalysedHistoricItems.Select(ahi => 
                                                    decimal.Parse(ahi.Value)));
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public DetailedCurrencyResponse(Models.Currency.Currency currency)
        {
            if (currency != null)
            {
                Name = currency.Name;
                Abbreviation = currency.Abbrv;
                LastUpdated = DateTime.UtcNow;

                // Obtain analysed component data from partial currency pairs.
                if (currency.CurrencyCurrencyPairs != null && currency.CurrencyCurrencyPairs.Count > 0)
                {
                    // Obtain via the Request method
                    var query = currency.CurrencyCurrencyPairs
                            .Select(pcp => pcp.CurrencyPair)
                            .SelectMany(cpr => cpr.CurrencyPairRequests)
                            .SelectMany(cpr => cpr.AnalysedComponents)
                            .Select(ac => new AnalysedComponent
                            {
                                Id = ac.Id,
                                ComponentType = ac.ComponentType,
                                Value = ac.Value,
                                Delay = ac.Delay,
                                RequestId = ac.RequestId,
                                CurrencyId = ac.CurrencyId,
                                AnalysedHistoricItems = ac.AnalysedHistoricItems
                            })
                            .ToList();

                    if (query.Count > 0)
                    {
                        foreach (var aComp in query)
                        {
                            switch (aComp.ComponentType)
                            {
                                case AnalysedComponentType.CurrentAveragePrice:
                                    if (decimal.TryParse(aComp.Value, out var avgPrice))
                                    {
                                        AveragePrice = avgPrice;
                                    }
                                    break;
                                case AnalysedComponentType.HourlyAveragePrice:
                                    if (aComp.AnalysedHistoricItems.Count > 0)
                                    {
                                        AveragePriceHistory = aComp.AnalysedHistoricItems
                                            .Where(ahi => ahi.DeletedAt == null && ahi.IsEnabled
                                                                                && !string.IsNullOrEmpty(ahi.Value))
                                            .Select(ahi => decimal.Parse(ahi.Value))
                                            .ToList();
                                    }
                                    break;
                                case AnalysedComponentType.DailyPricePctChange:
                                    if (decimal.TryParse(aComp.Value, out var dailyAvgPricePctChange))
                                    {
                                        DailyAvgPctChange = Math.Round(dailyAvgPricePctChange, 1);
                                    }
                                    break;
                                case AnalysedComponentType.DailyVolume:
                                    if (decimal.TryParse(aComp.Value, out var dailyVol))
                                    {
                                        if (DailyVolume > 0)
                                            DailyVolume = (AveragePrice + dailyVol) / 2;
                                        else
                                            DailyVolume = dailyVol;
                                    }
                                    break;
                            }
                        }
                    }
                }

                // If the average price is still not computed,
                if (AveragePrice <= 0 && 
                    currency.AnalysedComponents.Any(ac =>
                    ac.DeletedAt == null && ac.IsEnabled
                                         && ac.ComponentType.Equals(AnalysedComponentType.CurrentAveragePrice)))
                {
                    // Compute the average price directly with a currency-binded analysed component.
                    var currencyAP = currency.AnalysedComponents.FirstOrDefault(ac =>
                        ac.DeletedAt == null && ac.IsEnabled
                                             && ac.ComponentType.Equals(AnalysedComponentType.CurrentAveragePrice));
                    var currencyAPVal = currencyAP?.Value;

                    if (!string.IsNullOrEmpty(currencyAPVal))
                    {
                        AveragePrice = decimal.Parse(currencyAPVal);
                    }
                }
                
                // If the historical price is still not computed,
                if ((AveragePriceHistory == null || AveragePriceHistory.Count == 0) 
                    && currency.AnalysedComponents.Any(ac => ac.DeletedAt == null && ac.IsEnabled
                                                             && ac.ComponentType.Equals(AnalysedComponentType.HourlyAveragePrice)))
                {
                    // Compute the hourly average prices directly with a currency-binded analysed component.
                    var currencyHAP = currency.AnalysedComponents
                        .FirstOrDefault(ac => ac.DeletedAt == null && ac.IsEnabled
                                                                   && ac.ComponentType.Equals(AnalysedComponentType
                                                                       .HourlyAveragePrice));

                    // Make sure there is actually data
                    if (currencyHAP?.AnalysedHistoricItems != null 
                        && currencyHAP.AnalysedHistoricItems
                            .Any(ahi => ahi.HistoricDateTime >= DateTime.UtcNow.Subtract(TimeSpan.FromDays(1))))
                    {
                            AveragePriceHistory = currencyHAP.AnalysedHistoricItems
                                .Where(ahi => ahi.HistoricDateTime >= DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)))
                                .OrderByDescending(ahi => ahi.HistoricDateTime)
                                .DefaultIfEmpty()
                                .Select(ahi => decimal.Parse(ahi.Value))
                                .ToList();
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
                        DailyAvgPctChange = Math.Round(decimal.Parse(currencyDAPPC), 1);
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
                    .Select(cp => cp.CurrencyPairCurrencies
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
                    .Select(cp => cp.CurrencyPairCurrencies
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
                if (reqComp.RcdHistoricItems != null &&
                    reqComp.RcdHistoricItems.Count > 0)
                {
                    var rcdhiList = reqComp.RcdHistoricItems;

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

        /// <summary>
        /// Allows multiple currency objects that are identical to merge its history.
        /// </summary>
        /// <param name="currency"></param>
        public void Populate(Models.Currency.Currency currency)
        {
            if (currency != null)
            {
                if (currency.CurrencyCurrencyPairs != null && currency.CurrencyCurrencyPairs.Count > 0)
                {
                    // Obtain via the Request method
                    var query = currency.CurrencyCurrencyPairs
                            .Select(pcp => pcp.CurrencyPair)
                            .SelectMany(cpr => cpr.CurrencyPairRequests)
                            .SelectMany(cpr => cpr.AnalysedComponents)
                            .ToList();

                    if (query.Count > 0)
                    {
                        foreach (var aComp in query)
                        {
                            switch (aComp.ComponentType)
                            {
                                case AnalysedComponentType.CurrentAveragePrice:
                                    if (decimal.TryParse(aComp.Value, out var avgPrice))
                                    {
                                        if (AveragePrice > 0)
                                            AveragePrice = (AveragePrice + avgPrice) / 2;
                                        else
                                            AveragePrice = avgPrice;
                                    }
                                    break;
                                case AnalysedComponentType.DailyPricePctChange:
                                    if (decimal.TryParse(aComp.Value, out var dailyAvgPricePctChange) && 
                                        aComp.ModifiedAt >= DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)))
                                    {
                                        if (DailyAvgPctChange != 0)
                                            DailyAvgPctChange =
                                                (DailyAvgPctChange + dailyAvgPricePctChange) / 2;
                                        else
                                            DailyAvgPctChange = dailyAvgPricePctChange;
                                    }
                                    break;
                                case AnalysedComponentType.DailyVolume:
                                    if (decimal.TryParse(aComp.Value, out var dailyVol) && 
                                        aComp.ModifiedAt >= DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)))
                                    {
                                        if (DailyVolume > 0)
                                            DailyVolume = (AveragePrice + dailyVol) / 2;
                                        else
                                            DailyVolume = dailyVol;
                                    }
                                    break;
                            }
                        }
                    }
                }

                // Obtain via the currency method
                // These values are more consistent throughout the entire currency as they provide the total average
                // that factors non generic counter currencies pairs.
                if (currency.AnalysedComponents.Any(ac =>
                    ac.DeletedAt == null && ac.IsEnabled
                                         && ac.ComponentType.Equals(AnalysedComponentType.CurrentAveragePrice)))
                {
                    var currencyAP = currency.AnalysedComponents.FirstOrDefault(ac =>
                        ac.DeletedAt == null && ac.IsEnabled
                                             && ac.ComponentType.Equals(AnalysedComponentType.CurrentAveragePrice));
                    var currencyAPVal = currencyAP?.Value;

                    if (string.IsNullOrEmpty(currencyAPVal)
                        && currencyAPVal.Equals("0", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (AveragePrice > 0)
                            AveragePrice = (AveragePrice + decimal.Parse(currencyAPVal)) / 2;
                        else
                            AveragePrice = decimal.Parse(currencyAPVal);

                        // TODO: Granulate AveragePriceHistory
//                        if (currencyAP.AnalysedHistoricItems != null && currencyAP.AnalysedHistoricItems.Count > 0)
//                        {
//                            AveragePriceHistory = currencyAP.AnalysedHistoricItems
//                                .OrderByDescending(ahi => ahi.HistoricDateTime)
//                                .DefaultIfEmpty()
//                                .Select(ahi => decimal.Parse(ahi.Value))
//                                .ToList();
//                        }
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
                        if (DailyAvgPctChange != decimal.Zero)
                            DailyAvgPctChange = Math.Round(DailyAvgPctChange + decimal.Parse(currencyDAPPC) / 2, 1);
                        else
                            DailyAvgPctChange = Math.Round(decimal.Parse(currencyDAPPC), 1);
                    }
                }
                
                // Market cap is usually stored with a currency-based AC.
                // Thus we directly obtain the value like that.
                if (currency.AnalysedComponents.Any(ac =>
                    ac.DeletedAt == null && ac.IsEnabled
                                         && ac.ComponentType.Equals(AnalysedComponentType.MarketCap)))
                {
                    if (MarketCap <= decimal.Zero)
                        MarketCap = decimal.Parse(currency.AnalysedComponents.FirstOrDefault(ac =>
                                                          ac.DeletedAt == null && ac.IsEnabled
                                                                               && ac.ComponentType.Equals(
                                                                                   AnalysedComponentType.MarketCap))
                                                      ?.Value ?? MarketCap.ToString(CultureInfo.InvariantCulture));
                    else
                        MarketCap = (MarketCap + decimal.Parse(currency.AnalysedComponents.FirstOrDefault(ac =>
                                                                       ac.DeletedAt == null && ac.IsEnabled
                                                                                            && ac.ComponentType.Equals(
                                                                                                AnalysedComponentType
                                                                                                    .MarketCap))
                                                                   ?.Value ?? MarketCap.ToString(CultureInfo
                                                                   .InvariantCulture)))
                                    / 2;
                }
            }
        }
    }
}
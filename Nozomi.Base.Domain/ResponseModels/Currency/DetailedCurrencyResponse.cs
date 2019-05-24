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

        public DetailedCurrencyResponse(Models.Currency.Currency currency)
        {
            // Aggregate non-compounded properties first
            Name = currency.Name;
            Abbreviation = currency.Abbreviation;
            LastUpdated = currency.ModifiedAt;

            foreach (var ac in currency.AnalysedComponents)
            {
                switch (ac.ComponentType)
                {
                    case AnalysedComponentType.DailyVolume:
                        DailyVolume = decimal.Parse(ac.Value ?? "0");
                        break;
                    case AnalysedComponentType.MarketCap:
                        MarketCap = decimal.Parse(ac.Value ?? "0");
                        break;
                    case AnalysedComponentType.CurrentAveragePrice:
                        AveragePrice = decimal.Parse(ac.Value ?? "0");
                        break;
                    case AnalysedComponentType.HourlyAveragePrice:
                        if (ac.AnalysedHistoricItems != null && ac.AnalysedHistoricItems.Count > 0)
                        {
                            AveragePriceHistory = ac.AnalysedHistoricItems
                                .Where(ahi => !string.IsNullOrEmpty(ahi.Value)
                                              && decimal.TryParse(ahi.Value, out var junk)
                                              && ahi.HistoricDateTime > DateTime.UtcNow.Subtract(TimeSpan.FromDays(7)))
                                .Select(ahi => decimal.Parse(ahi.Value))
                                .ToList();    
                        }

                        break;
                    case AnalysedComponentType.DailyPricePctChange:
                        DailyAvgPctChange = decimal.Parse(ac.Value ?? "0");
                        break;
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
    }
}
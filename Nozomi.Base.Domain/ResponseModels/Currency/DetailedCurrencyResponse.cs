using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Nozomi.Base.Core;
using Nozomi.Base.Core.Helpers.Native.Numerals;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.ResponseModels.RequestComponent;
using Nozomi.Data.ResponseModels.TickerPair;

namespace Nozomi.Data.ResponseModels.Currency
{
    /// <summary>
    /// Detailed Currency Response specific to a currency.
    /// </summary>
    public class DetailedCurrencyResponse : CurrencyResponse
    {
        /// <summary>
        /// Obtain the live average price, averaged across ALL sources.
        /// </summary>
        public decimal AveragePrice { get; set; }

        public decimal DailyAvgPctChange { get; set; }

        public decimal DailyVolume { get; set; }

        public decimal MarketCap { get; set; }

        public Dictionary<AnalysedComponentType, List<ComponentHistoricalDatum>> Historical { get; set; }

        public List<decimal> AveragePriceHistory { get; set; }
        
        public DateTime LastUpdated { get; set; }

        public DetailedCurrencyResponse()
        {
        }

        public DetailedCurrencyResponse(Models.Currency.Currency currency, ICollection<CurrencyTickerPair> currencyTickerPairs)
        {
            // Aggregate non-compounded properties first
            Name = currency.Name;
            Abbreviation = currency.Abbreviation;
            Slug = currency.Slug;
            LastUpdated = currency.ModifiedAt;
            LogoPath = currency.LogoPath;

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
                                .Where(ahi => NumberHelper.IsNumericDecimal(ahi.Value)
                                              && ahi.HistoricDateTime > DateTime.UtcNow.Subtract(TimeSpan.FromDays(7)))
                                .OrderBy(ahi => ahi.HistoricDateTime)
                                .Select(ahi => decimal.Parse(ahi.Value))
                                .ToList();
                        }

                        break;
                    case AnalysedComponentType.DailyPricePctChange:
                        DailyAvgPctChange = decimal.Parse(ac.Value ?? "0");
                        break;
                    default:
                        if (Historical == null)
                            Historical = new Dictionary<AnalysedComponentType, List<ComponentHistoricalDatum>>();

                        Historical.Add(ac.ComponentType, new List<ComponentHistoricalDatum>());
                        
                        Historical[ac.ComponentType].Add(new ComponentHistoricalDatum()
                        {
                            CreatedAt = ac.ModifiedAt,
                            Value = ac.Value
                        });
                        
                        if (ac.AnalysedHistoricItems != null && ac.AnalysedHistoricItems.Count > 0)
                        {
                            Historical[ac.ComponentType].AddRange(ac.AnalysedHistoricItems
                                .Select(ahi => new ComponentHistoricalDatum()
                                {
                                    CreatedAt = ahi.HistoricDateTime,
                                    Value = ahi.Value
                                }));
                        }

                        Historical[ac.ComponentType] = Historical[ac.ComponentType]
                            .OrderByDescending(chd => chd.CreatedAt)
                            .ToList();
                        break;
                }
            }
        }
    }
}
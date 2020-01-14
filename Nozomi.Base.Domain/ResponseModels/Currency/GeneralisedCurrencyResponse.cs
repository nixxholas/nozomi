using System;
using System.Collections.Generic;
using System.Linq;
using Nozomi.Base.BCL.Helpers.Native.Numerals;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.ResponseModels.TickerPair;

namespace Nozomi.Data.ResponseModels.Currency
{
    /// <summary>
    /// Detailed Currency Response specific to a currency.
    /// </summary>
    public class GeneralisedCurrencyResponse : CurrencyResponse
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

        public GeneralisedCurrencyResponse()
        {
        }

        public GeneralisedCurrencyResponse(Models.Currency.Currency currency, ICollection<CurrencyTickerPair> currencyTickerPairs)
        {
            // Aggregate non-compounded properties first
            Name = currency.Name;
            Abbreviation = currency.Abbreviation;
            Slug = currency.Slug;
            LastUpdated = currency.ModifiedAt;
            LogoPath = currency.LogoPath;

            if (currency.Requests != null && currency.Requests.Count > 0)
            {
                var reqComps = currency.Requests
                    .SelectMany(r => r.RequestComponents)
                    .ToList();

                foreach (var reqComp in reqComps)
                {
                    switch (reqComp.ComponentType)
                    {
                        case ComponentType.DailyVolume:
                            DailyVolume = decimal.Parse(reqComp.Value ?? "0");
                            break;
                    }
                }
            }

            foreach (var ac in currency.AnalysedComponents)
            {
                switch (ac.ComponentType)
                {
                    case AnalysedComponentType.DailyVolume:
                        if (DailyVolume <= 0)
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
                    case AnalysedComponentType.DailyAveragePrice:
                        AveragePriceHistory = ac.AnalysedHistoricItems
                            .Where(ahi => NumberHelper.IsNumericDecimal(ahi.Value)
                                          && ahi.HistoricDateTime > DateTime.UtcNow.Subtract(TimeSpan.FromDays(31)))
                            .OrderBy(ahi => ahi.HistoricDateTime)
                            .Select(ahi => decimal.Parse(ahi.Value))
                            .ToList();
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
using System;
using System.Collections.Generic;
using System.Linq;
using Nozomi.Base.BCL.Helpers.Enumerator;
using Nozomi.Base.BCL.Helpers.Native.Numerals;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.ResponseModels.CurrencyProperty;
using Nozomi.Data.ResponseModels.RequestComponent;
using Nozomi.Data.ResponseModels.TickerPair;

namespace Nozomi.Data.ResponseModels.Currency
{
    public class DetailedCurrencyResponse : GeneralisedCurrencyResponse
    {
        public string Description { get; set; }
        
        public new List<DateValuePair<decimal>> AveragePriceHistory { get; set; }
        
        public ICollection<RequestComponentResponse> RequestComponents { get; set; }
        
        public ICollection<CurrencyPropertyResponse> Properties { get; set; }
        
        public DetailedCurrencyResponse(Models.Currency.Currency currency, ICollection<CurrencyTickerPair> currencyTickerPairs)
        {
            // Aggregate non-compounded properties first
            Name = currency.Name;
            Abbreviation = currency.Abbreviation;
            Description = currency.Description;
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
                                .Select(ahi => new DateValuePair<decimal>() {
                                    Time = ahi.HistoricDateTime, 
                                    Value = decimal.Parse(ahi.Value)
                                })
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
        
        public DetailedCurrencyResponse(Models.Currency.Currency currency, 
            ICollection<Models.Web.Component> requestComponents)
        {
            // Aggregate non-compounded properties first
            Id = currency.Id;
            Name = currency.Name;
            Abbreviation = currency.Abbreviation;
            Description = currency.Description;
            Slug = currency.Slug;
            LastUpdated = currency.ModifiedAt;
            LogoPath = currency.LogoPath;
            // CurrencyType = currency.CurrencyType.Name; // Omitted for brevity

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
                                .Select(ahi => new DateValuePair<decimal>() {
                                    Time = ahi.HistoricDateTime, 
                                    Value = decimal.Parse(ahi.Value)
                                })
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

            if (requestComponents != null && requestComponents.Count > 0)
            {
                RequestComponents = requestComponents.Select(rc => new RequestComponentResponse()
                    {
                        Name = rc.ComponentType.GetDescription(),
                        Timestamp = rc.ModifiedAt,
                        Value = rc.Value
                    })
                    .ToList();
            }

            if (currency.CurrencyProperties != null && currency.CurrencyProperties.Count > 0)
            {
                Properties = currency.CurrencyProperties
                    .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                    .Select(p => new CurrencyPropertyResponse
                    {
                        Type = p.Type.GetDescription(),
                        Value = p.Value
                    })
                    .ToList();
            }
        }
    }
}
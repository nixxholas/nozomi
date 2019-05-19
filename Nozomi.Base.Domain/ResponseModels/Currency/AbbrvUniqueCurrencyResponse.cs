//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Nozomi.Base.Core;
//using Nozomi.Data.Models.Currency;
//using Nozomi.Data.Models.Web;
//using Nozomi.Data.Models.Web.Analytical;
//using Nozomi.Data.ResponseModels.PartialCurrencyPair;
//using Nozomi.Data.ResponseModels.Source;
//
//namespace Nozomi.Data.ResponseModels.Currency
//{
//    public class AbbrvUniqueCurrencyResponse : DistinctiveCurrencyResponse
//    {
//        public AbbrvUniqueCurrencyResponse() {}
//
//        public AbbrvUniqueCurrencyResponse(Models.Currency.Currency currency)
//        {
//            Abbreviation = currency.Abbreviation;
//            Name = currency.Name;
//            LastUpdated = currency.ModifiedAt;
//            CurrencyType = currency.CurrencyType;
//            Description = currency.Description;
//            Denominations = currency.Denominations;
//            DenominationName = currency.DenominationName;
//            CurrencySources = currency.CurrencySources.Select(cs => cs.Source).ToList();
//            AnalysedComponents = currency.AnalysedComponents;
//            CurrencyRequests = currency.CurrencyRequests;
//        }
//
//        public AbbrvUniqueCurrencyResponse(ICollection<Models.Currency.Currency> currencies)
//        {
//            var currency = currencies.FirstOrDefault();
//
//            if (currency != null)
//            {
//                Abbreviation = currency.Abbreviation;
//                Name = currency.Name;
//                LastUpdated = currency.ModifiedAt;
//                CurrencyType = currency.CurrencyType;
//                Description = currency.Description;
//                Denominations = currency.Denominations;
//                DenominationName = currency.DenominationName;
//
//                // DTO this sitch
//                foreach (var similarCurr in currencies)
//                {
//                    // Populate the Currency Requests into this object
//                    foreach (var cReq in similarCurr.CurrencyRequests)
//                    {
//                        if (!CurrencyRequests.Any(cr => cr.Id.Equals(cReq.Id)))
//                        {
//                            CurrencyRequests.Add(cReq);
//                        }
//                    }
//
//                    foreach (var currencyCurrencyPair in similarCurr.CurrencyPairSourceCurrencies)
//                    {
//                        // Form the TickerPair abbreviation first
//                        var tickerPairStr = string.Concat(currencyCurrencyPair.CurrencyPair.MainCurrency,
//                            currencyCurrencyPair.CurrencyPair.CounterCurrency);
//
//                        // Safetynet
//                        if (!string.IsNullOrEmpty(tickerPairStr))
//                        {
//                            // Make sure this is a main currency and that the TickerPairs collection
//                            // does not contain this TickerPair yet.
//                            if (!TickerPairs.Any(tp => tp.PairAbbreviation.Equals(tickerPairStr,
//                                StringComparison.InvariantCultureIgnoreCase)))
//                            {
//                                // Since all is good, let's toss it in
//                                TickerPairs.Add(new CondensedTickerPair
//                                {
//                                    PairAbbreviation = tickerPairStr,
//                                    Sources = new List<SourceResponse>
//                                    {
//                                        new SourceResponse
//                                        {
//                                            Abbreviation = similarCurr.CurrencySource.Abbreviation,
//                                            Name = similarCurr.CurrencySource.Name
//                                        }
//                                    }
//                                });
//                            }
//                            // Since the TickerPair already exists,
//                            else
//                            {
//                                // Pull the TickerPair
//                                var tickerPair = TickerPairs.SingleOrDefault(tp =>
//                                    tp.PairAbbreviation.Equals(tickerPairStr,
//                                        StringComparison.InvariantCultureIgnoreCase));
//
//                                // Check if the source exists first
//                                if (tickerPair.Sources.Any(s => s.Abbreviation
//                                    .Equals(similarCurr.CurrencySource.Abbreviation,
//                                        StringComparison.InvariantCultureIgnoreCase)))
//                                {
//                                    // It does, so let's ignore.
//                                    // If it reaches here it means that we have a duplicate.
//                                }
//                                else
//                                {
//                                    // Does not, add it in.
//                                    tickerPair.Sources.Add(new SourceResponse
//                                    {
//                                        Abbreviation = similarCurr.Abbreviation,
//                                        Name = similarCurr.Name
//                                    });
//                                }
//                            }
//                        }
//                    }
//
//                    if (!CurrencySources.Any(cs => cs.Id.Equals(similarCurr.CurrencySourceId)))
//                    {
//                        CurrencySources.Add(similarCurr.CurrencySource);
//                    }
//                }
//
//                AnalysedComponents = currencies
//                    .SelectMany(c => c.CurrencyPairSourceCurrencies)
//                    .Select(pcp => pcp.CurrencyPair)
//                    .Where(cp => cp.CounterCurrency.Equals(CoreConstants.GenericCounterCurrency,
//                        StringComparison.InvariantCultureIgnoreCase))
//                    .SelectMany(cp => cp.CurrencyPairRequests)
//                    .SelectMany(cpr => cpr.AnalysedComponents)
//                    .ToList();
//            }
//        }
//        
//        public CurrencyType CurrencyType { get; set; }
//        
//        public string Description { get; set; }
//
//        public int Denominations { get; set; } = 0;
//        
//        public string DenominationName { get; set; }
//
//        public ICollection<Models.Currency.Source> CurrencySources { get; set; } = new List<Models.Currency.Source>();
//        
//        public ICollection<AnalysedComponent> AnalysedComponents { get; set; } = new List<AnalysedComponent>();
//        
//        public ICollection<CurrencyRequest> CurrencyRequests { get; set; } = new List<CurrencyRequest>();
//
//        public ICollection<CondensedTickerPair> TickerPairs { get; set; } = new List<CondensedTickerPair>();
//    }
//}
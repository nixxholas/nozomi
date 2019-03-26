using System.Collections.Generic;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Data.ResponseModels.Currency
{
    public class AbbrvUniqueCurrencyResponse : DistinctiveCurrencyResponse
    {
        public AbbrvUniqueCurrencyResponse() {}

        public AbbrvUniqueCurrencyResponse(Models.Currency.Currency currency)
        {
            Abbreviation = currency.Abbrv;
            Name = currency.Name;
            LastUpdated = currency.ModifiedAt;
            CurrencyType = currency.CurrencyType;
            Denominations = currency.Denominations;
            DenominationName = currency.DenominationName;
            CurrencySources = new List<Models.Currency.Source>
            {
                currency.CurrencySource
            };
            AnalysedComponents = currency.AnalysedComponents;
            CurrencyRequests = currency.CurrencyRequests;
            PartialCurrencyPairs = currency.PartialCurrencyPairs;
        }
        
        public CurrencyType CurrencyType { get; set; }

        public int Denominations { get; set; } = 0;
        
        public string DenominationName { get; set; }

        public ICollection<Models.Currency.Source> CurrencySources { get; set; }
        
        public ICollection<AnalysedComponent> AnalysedComponents { get; set; }
        
        public ICollection<CurrencyRequest> CurrencyRequests { get; set; }

        public ICollection<Models.Currency.PartialCurrencyPair> PartialCurrencyPairs { get; set; }
    }
}
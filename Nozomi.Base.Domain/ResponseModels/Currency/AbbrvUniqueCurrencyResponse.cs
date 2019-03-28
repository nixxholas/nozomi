using System.Collections.Generic;
using System.Linq;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.ResponseModels.PartialCurrencyPair;

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
            Description = currency.Description;
            Denominations = currency.Denominations;
            DenominationName = currency.DenominationName;
            CurrencySources = new List<Models.Currency.Source>
            {
                currency.CurrencySource
            };
            AnalysedComponents = currency.AnalysedComponents;
            CurrencyRequests = currency.CurrencyRequests;
        }
        
        public CurrencyType CurrencyType { get; set; }
        
        public string Description { get; set; }

        public int Denominations { get; set; } = 0;
        
        public string DenominationName { get; set; }

        public ICollection<Models.Currency.Source> CurrencySources { get; set; }
        
        public ICollection<AnalysedComponent> AnalysedComponents { get; set; }
        
        public ICollection<CurrencyRequest> CurrencyRequests { get; set; }

        public ICollection<CondensedTickerPair> TickerPairs { get; set; } = new List<CondensedTickerPair>();
    }
}
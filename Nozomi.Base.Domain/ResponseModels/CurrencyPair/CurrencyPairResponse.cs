using Nozomi.Data.Models.Currency;

namespace Nozomi.Data.ResponseModels.CurrencyPair
{
    public class CurrencyPairResponse
    {
        public long Id { get; set; }
        
        public CurrencyPairType CurrencyPairType { get; set; }
    }
}
using Nozomi.Base.Core;

namespace Nozomi.Data.Models.Currency
{
    public class CurrencyCurrencyPair : BaseEntityModel
    {
        public long CurrencyId { get; set; }
        
        public Currency Currency { get; set; }
        
        public long CurrencyPairId { get; set; }
        
        public CurrencyPair CurrencyPair { get; set; }
    }
}
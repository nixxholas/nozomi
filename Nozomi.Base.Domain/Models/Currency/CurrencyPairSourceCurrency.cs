using Nozomi.Base.Core;

namespace Nozomi.Data.Models.Currency
{
    public class CurrencyPairSourceCurrency : BaseEntityModel
    {
        public long CurrencySourceId { get; set; }
        
        public CurrencySource CurrencySource { get; set; }
        
        public long CurrencyPairId { get; set; }
        
        public CurrencyPair CurrencyPair { get; set; }
    }
}
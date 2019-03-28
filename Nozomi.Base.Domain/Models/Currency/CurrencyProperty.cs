using Nozomi.Base.Core;

namespace Nozomi.Data.Models.Currency
{
    public class CurrencyProperty : BaseEntityModel
    {
        public long Id { get; set; }
        
        public CurrencyPropertyType Type { get; set; }
        
        public string Value { get; set; }
        
        public long CurrencyId { get; set; }
        
        public Currency Currency { get; set; }
    }
}
using Nozomi.Base.Core;
using Nozomi.Base.Core.Models;

namespace Nozomi.Data.Models.Currency
{
    public class CurrencyProperty : Entity
    {
        public long Id { get; set; }
        
        public CurrencyPropertyType Type { get; set; }
        
        public string Value { get; set; }
        
        public long CurrencyId { get; set; }
        
        public Currency Currency { get; set; }

        public bool IsValid()
        {
            return Type >= 0 && !string.IsNullOrEmpty(Value) && CurrencyId > 0;
        }
    }
}
using Nozomi.Data.Models.Currency;

namespace Nozomi.Data.AreaModels.v1.CurrencyProperty
{
    public class UpdateCurrencyProperty
    {
        public long Id { get; set; }
        public CurrencyPropertyType Type { get; set; }
        
        public string Value { get; set; }
        
        public bool IsEnabled { get; set; }
    }
}
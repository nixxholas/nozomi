using System.ComponentModel.DataAnnotations;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Data.AreaModels.v1.CurrencyProperty
{
    public class CreateCurrencyProperty
    {
        public CurrencyPropertyType Type { get; set; } = CurrencyPropertyType.Generic;
        
        [Required]
        public string Value { get; set; }
        
        [Required]
        public long CurrencyId { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Value) && CurrencyId > 0;
        }
    }
}
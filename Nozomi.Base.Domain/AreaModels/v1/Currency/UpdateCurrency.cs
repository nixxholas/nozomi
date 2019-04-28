using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nozomi.Data.AreaModels.v1.Currency
{
    public class UpdateCurrency
    {
        [Required]
        public long Id { get; set; }
        
        [Required]
        public long CurrencyTypeId { get; set; }

        [Required]
        public string Abbrv { get; set; } // USD? MYR? IND?

        [Required]
        public string Name { get; set; }

        [Required]
        public long CurrencySourceId { get; set; }

        public bool IsEnabled { get; set; } = true;

        // This will have a number if it is a crypto pair to peg to proper entities
        public long WalletTypeId { get; set; } = 0;
        
        public ICollection<Models.Currency.CurrencyCurrencyPair> CurrencyCurrencyPairs { get; set; }

        public bool IsValid()
        {
            return CurrencyTypeId > 0 && !string.IsNullOrEmpty(Abbrv) && !string.IsNullOrEmpty(Name)
                   && CurrencySourceId > 0;
        }
    }
}
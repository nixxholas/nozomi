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
        public string Abbreviation { get; set; } // USD? MYR? IND?
        
        public string LogoPath { get; set; }
        
        [Required]
        public string Slug { get; set; }
        
        public string Description { get; set; }

        [Required] 
        public int Denominations { get; set; } = 0;
        
        public string DenomationName { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public long CurrencySourceId { get; set; }

        public bool IsEnabled { get; set; } = true;

        // This will have a number if it is a crypto pair to peg to proper entities
        public long WalletTypeId { get; set; } = 0;
        
        public bool IsValid()
        {
            return CurrencyTypeId > 0 && !string.IsNullOrEmpty(Abbreviation)
                                      && !string.IsNullOrEmpty(Slug)
                                      && !string.IsNullOrEmpty(Name);
        }
    }
}
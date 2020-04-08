using System.ComponentModel.DataAnnotations;
using Nozomi.Data.Models.Categorisation;

namespace Nozomi.Data.AreaModels.v1.CurrencyProperty
{
    public class UpdateCurrencyProperty
    {
        [Required]
        public long Id { get; set; }
        
        [Required]
        public ItemPropertyType Type { get; set; }
        
        [Required]
        public string Value { get; set; }
        
        public bool IsEnabled { get; set; }
    }
}
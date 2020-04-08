using System.ComponentModel.DataAnnotations;
using Nozomi.Data.Models.Categorisation;

namespace Nozomi.Data.AreaModels.v1.CurrencyProperty
{
    public class CreateCurrencyProperty
    {
        public ItemPropertyType Type { get; set; }
     
        [Required]
        public string Value { get; set; }
        
        [Required]
        public long CurrencyId { get; set; }
        
        public bool IsEnabled { get; set; }
    }
}
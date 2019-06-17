using System.ComponentModel.DataAnnotations;

namespace Nozomi.Data.AreaModels.v1.CurrencySource
{
    public class CreateCurrencySource
    {
        [Required]
        public long CurrencyId { get; set; }
        
        [Required]
        public long SourceId { get; set; }
    }
}
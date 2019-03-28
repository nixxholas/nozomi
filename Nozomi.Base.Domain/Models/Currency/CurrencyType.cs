using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nozomi.Base.Core;

namespace Nozomi.Data.Models.Currency
{
    public class CurrencyType : BaseEntityModel
    {
        [Key]
        public long Id { get; set; }

        [MaxLength(12)]
        public string TypeShortForm { get; set; }

        public string Name { get; set; }

        public ICollection<Currency> Currencies { get; set; }

        public bool IsValid()
        {
            return (!string.IsNullOrEmpty(TypeShortForm) && !string.IsNullOrEmpty(Name));
        }
    }
}

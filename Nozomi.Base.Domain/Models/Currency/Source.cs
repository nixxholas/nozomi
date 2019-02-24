using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Nozomi.Base.Core;

namespace Nozomi.Data.Models.Currency
{
    public class Source : BaseEntityModel
    {
        [Key]
        public long Id { get; set; }

        // Short form for the currency source if needed.
        public string Abbreviation { get; set; }

        public string Name { get; set; }

        public string APIDocsURL { get; set; }
        
        [NotMapped]
        public long PairCount { get; set; }

        // =========== RELATIONS ============ //
        public ICollection<Currency> Currencies { get; set; }
        public ICollection<CurrencyPair> CurrencyPairs { get; set; }

        public bool IsValid()
        {
            return (!string.IsNullOrEmpty(Abbreviation) && !string.IsNullOrEmpty(Name));
        }
    }
}

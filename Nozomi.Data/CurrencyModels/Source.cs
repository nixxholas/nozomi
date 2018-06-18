using Counter.SDK.SharedModels;
using CounterCore.Data.Models.CommonCurrencyModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nozomi.Data.CurrencyModels
{
    public class Source : BaseEntityModel
    {
        [Key]
        public long Id { get; set; }

        // Short form for the currency source if needed.
        public string Abbreviation { get; set; }

        public string Name { get; set; }

        public string APIDocsURL { get; set; }

        // =========== RELATIONS ============ //
        public ICollection<Currency> Currencies { get; set; }
        public ICollection<CurrencyPair> CurrencyPairs { get; set; }

        public bool IsValid()
        {
            return (!string.IsNullOrEmpty(Abbreviation) && !string.IsNullOrEmpty(Name));
        }
    }
}

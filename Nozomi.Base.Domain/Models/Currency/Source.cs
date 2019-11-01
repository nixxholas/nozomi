﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Nozomi.Base.Core;

namespace Nozomi.Data.Models.Currency
{
    public class Source : Entity
    {
        public long Id { get; set; }

        // Short form for the currency source if needed.
        public string Abbreviation { get; set; }

        public string Name { get; set; }

        public string APIDocsURL { get; set; }

        public long PairCount()
        {
            if (CurrencyPairs != null)
                return CurrencyPairs.Count;

            return -1;
        }

        // =========== RELATIONS ============ //
        public long SourceTypeId { get; set; }
        
        public SourceType SourceType { get; set; }
        
        public ICollection<CurrencySource> SourceCurrencies { get; set; }
        public ICollection<CurrencyPair> CurrencyPairs { get; set; }

        public bool IsValid()
        {
            return (!string.IsNullOrEmpty(Abbreviation) && !string.IsNullOrEmpty(Name));
        }
    }
}

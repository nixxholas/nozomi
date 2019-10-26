﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Nozomi.Base.Core;
using Nozomi.Base.Core.Models;

namespace Nozomi.Data.Models.Currency
{
    public class Source : Entity
    {
        public Source () {}

        public Source(string abbreviation, string name, string apiDocsUrl)
        {
            Abbreviation = abbreviation;
            Name = name;
            APIDocsURL = apiDocsUrl;
        }
        
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
        public ICollection<CurrencySource> SourceCurrencies { get; set; }
        public ICollection<CurrencyPair> CurrencyPairs { get; set; }

        public bool IsValid()
        {
            return (!string.IsNullOrEmpty(Abbreviation) && !string.IsNullOrEmpty(Name));
        }
    }
}

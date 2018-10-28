using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Nozomi.Data.AreaModels.v1.CurrencySource
{
    public abstract class UpdateSource
    {
        public long Id { get; set; }
        
        public string Abbreviation { get; set; }
        
        public string Name { get; set; }
        
        public string APIDocsURL { get; set; }
        
        public ICollection<UpdateSourceCurrency> UpdateSourceCurrencies { get; set; }
        
        public ICollection<UpdateCurrencyPair> UpdateCurrencyPairs { get; set; }

        public abstract class UpdateSourceCurrency
        {
            [Required]
            public long Id { get; set; }
            
            [Required]
            [DefaultValue(0)]
            public long CurrencySourceId { get; set; }
            
            // I've left these here to tell you that we're not supposed to modify the Currency here,
            // but to allow editors to associate or dissociate this with the Source that we're updating.
//            [DefaultValue(0)]
//            public long CurrencyTypeId { get; set; }
//            
//            public string Abbrv { get; set; }
//            
//            [DefaultValue(0)]
//            public long WalletTypeId { get; set; }
        }

        public abstract class UpdateCurrencyPair
        {
            [Required]
            public long Id { get; set; }
            
            [Required]
            [DefaultValue(0)]
            public long CurrencySourceId { get; set; }
        }
    }
}
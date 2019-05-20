using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Nozomi.Data.AreaModels.v1.CurrencySource
{
    public class UpdateSource : CreateSource
    {
        public long Id { get; set; }
        
        public bool IsEnabled { get; set; }
        
        public ICollection<UpdateSourceCurrency> UpdateSourceCurrencies { get; set; }
        
        public ICollection<UpdateCurrencyPair> UpdateCurrencyPairs { get; set; }

        public class UpdateSourceCurrency
        {
            [Required]
            public long Id { get; set; }
            
            [Required]
            [DefaultValue(0)]
            public long SourceId { get; set; }
            
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

        public class UpdateCurrencyPair
        {
            [Required]
            public long Id { get; set; }
            
            [Required]
            [DefaultValue(0)]
            public long CurrencySourceId { get; set; }
        }
    }
}
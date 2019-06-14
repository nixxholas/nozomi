using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Nozomi.Data.AreaModels.v1.CurrencyPairRequest;
using Nozomi.Data.AreaModels.v1.PartialCurrencyPair;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Data.AreaModels.v1.CurrencyPair
{
    /// <summary>
    /// Please update.
    /// </summary>
    [Obsolete]
    public class CreateCurrencyPair
    {
        public CurrencyPairType CurrencyPairType { get; set; }
        
        public string APIUrl { get; set; }
        
        public string DefaultComponent { get; set; }
        
        public long SourceId { get; set; }
        
        public string MainCurrencyAbbrv { get; set; }
        
        public string CounterCurrencyAbbrv { get; set; }

        public bool IsValid()
        {
            return CurrencyPairType >= 0 && !string.IsNullOrEmpty(APIUrl) && SourceId > 0
                   && MainCurrencyAbbrv != null && CounterCurrencyAbbrv != null;
        }
    }
}
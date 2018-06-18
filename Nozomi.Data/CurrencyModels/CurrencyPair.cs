using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Counter.SDK.SharedModels;

namespace Nozomi.Data.CurrencyModels
{
    public class CurrencyPair : BaseEntityModel
    {
        [Key]
        public long Id { get; set; }

        public CurrencyPairType CurrencyPairType { get; set; }

        public string APIUrl { get; set; }
        
        /// <summary>
        /// Which CPC to rely on by default?
        /// </summary>
        public string DefaultComponent { get; set; }

        public long CurrencySourceId { get; set; }
        public Source CurrencySource { get; set; }

        // =========== RELATIONS ============ //
        public ICollection<CurrencyPairComponent> CurrencyPairComponents { get; set; }
        public ICollection<PartialCurrencyPair> PartialCurrencyPairs { get; set; }

        public bool IsValid()
        {
            var firstPair = PartialCurrencyPairs.First();
            var lastPair = PartialCurrencyPairs.Last();
            
            return (CurrencyPairType > 0) && (!string.IsNullOrEmpty(APIUrl)) 
                                          && (!string.IsNullOrEmpty(DefaultComponent))
                                          && (CurrencySourceId > 0)
                                          && (CurrencyPairComponents.Count > 0)
                                          && (PartialCurrencyPairs.Count == 2)
                                          && (firstPair.CurrencyId != lastPair.CurrencyId)
                                          && (!firstPair.IsMain == lastPair.IsMain);
        }
    }
}

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
        [Required]
        public CurrencyPairType CurrencyPairType { get; set; }
        
        public string ApiUrl { get; set; }
        
        public string DefaultComponent { get; set; }
        
        public long CurrencySourceId { get; set; }
        
        public ICollection<CreatePartialCurrencyPair> PartialCurrencyPairs { get; set; }
        
        public ICollection<CreateCurrencyPairRequest> CurrencyPairRequests { get; set; }

        public bool IsValid()
        {
            return CurrencyPairType >= 0 && !string.IsNullOrEmpty(ApiUrl) && CurrencySourceId > 0 
                   && PartialCurrencyPairs != null && PartialCurrencyPairs.Count == 2 
                   && PartialCurrencyPairs.Count(pcp => pcp.IsMain) == 1 
                   && !PartialCurrencyPairs.SingleOrDefault(pcp => pcp.IsMain).CurrencyId
                       .Equals(PartialCurrencyPairs.SingleOrDefault(pcp => !pcp.IsMain).CurrencyId);
        }
    }
}
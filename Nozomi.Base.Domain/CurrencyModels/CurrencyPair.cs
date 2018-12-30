﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Nozomi.Base.Core;
using Nozomi.Data.AreaModels.v1.CurrencyPair;
using Nozomi.Data.WebModels;
using Swashbuckle.AspNetCore.Annotations;

namespace Nozomi.Data.CurrencyModels
{
    [SwaggerSchemaFilter(typeof(CurrencyPairSchemaFilter))]
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
        public ICollection<CurrencyPairRequest> CurrencyPairRequests { get; set; }
        public ICollection<PartialCurrencyPair> PartialCurrencyPairs { get; set; }

        public bool IsValid()
        {
            var firstPair = PartialCurrencyPairs.First();
            var lastPair = PartialCurrencyPairs.Last();
            
            return (CurrencyPairType > 0) && (!string.IsNullOrEmpty(APIUrl)) 
                                          && (!string.IsNullOrEmpty(DefaultComponent))
                                          && (CurrencySourceId > 0)
                                          && (PartialCurrencyPairs.Count == 2)
                                          && (firstPair.CurrencyId != lastPair.CurrencyId)
                                          && (!firstPair.IsMain == lastPair.IsMain);
        }
    }
}

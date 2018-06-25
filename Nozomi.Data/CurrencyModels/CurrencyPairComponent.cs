using System;
using System.Collections;
using System.Collections.Generic;
using Counter.SDK.SharedModels;
using System.ComponentModel.DataAnnotations;
using Nozomi.Data.WebModels;

namespace Nozomi.Data.CurrencyModels
{
    public class CurrencyPairComponent : CurrencyPairRequestComponent
    {
        public long CurrencyPairId { get; set; }
        public CurrencyPair CurrencyPair { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(QueryComponent) && CurrencyPairId > 0;
        }
    }
}

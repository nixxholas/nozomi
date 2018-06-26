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

        public bool IsValueAbnormal(string incomingValue)
        {
            switch (ComponentType)
            {
                case ComponentType.Ask:
                    decimal.TryParse(incomingValue, out decimal askRes);
                    return (askRes <= 0);
                case ComponentType.Bid:
                    decimal.TryParse(incomingValue, out decimal bidRes);
                    return bidRes <= 0;
                default:
                    return false;
            }
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(QueryComponent) && CurrencyPairId > 0;
        }
    }
}

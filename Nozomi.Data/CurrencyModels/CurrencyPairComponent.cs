using System;
using System.Collections;
using System.Collections.Generic;
using Counter.SDK.SharedModels;
using System.ComponentModel.DataAnnotations;

namespace Nozomi.Data.CurrencyModels
{
    public class CurrencyPairComponent : BaseEntityModel
    {
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// ASK? BID?
        /// </summary>
        /// <value>The type of the component.</value>
        public ComponentType ComponentType { get; set; }

        /// <summary>
        /// Gets or sets the query component of the JSON data retrieved from the APIUrl.
        /// </summary>
        /// <value>The query component.</value>
        /// 
        /// Example: MainNest.Nest2.property3
        /// 
        /// If the component is actually an array, just toss a number. We'll attempt
        /// to pass thing parameter to an integer first anyway, so as to be able
        /// to distinguish if we're offloading data from an array or object.
        public string QueryComponent { get; set; }

        public decimal Value { get; set; }

        public long CurrencyPairId { get; set; }
        public CurrencyPair CurrencyPair { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(QueryComponent) && CurrencyPairId > 0;
        }
    }
}

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Nozomi.Data.ResponseModels.Source;

namespace Nozomi.Data.ResponseModels.PartialCurrencyPair
{
    public class CondensedTickerPair
    {
        /// <summary>
        /// Utilize CurrencyPairIds to properly construct the object.
        /// </summary>
        [HiddenInput(DisplayValue = false)]
        public long CurrencyPairId { get; set; }
        
        public string PairAbbreviation { get; set; }
        
        public ICollection<SourceResponse> Sources { get; set; }
    }
}
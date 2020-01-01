using System.Collections.Generic;
using Nozomi.Data.ResponseModels.Source;

namespace Nozomi.Data.ResponseModels.PartialCurrencyPair
{
    public class CondensedTickerPair
    {
        public string PairAbbreviation { get; set; }
        
        public ICollection<SourceResponse> Sources { get; set; }
    }
}
using System.Collections.Generic;
using Nozomi.Data.ResponseModels.Source;

namespace Nozomi.Data.ResponseModels.TickerPair
{
    public class TickerPairResponse
    {
        public string Key { get; set; }
        
        public ICollection<SourceResponse> Sources { get; set; }
    }
}
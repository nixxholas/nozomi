using System.Collections.Generic;

namespace Nozomi.Analysis.Base.Domain.Responses.Hub.Asset
{
    public class CurrencyAssetResponse
    {
        public string Abbrv { get; set; }
        
        public string Name { get; set; }
        
        public Dictionary<string, decimal> Properties { get; set; }
    }
}
using System.Collections.Generic;
using Nozomi.Data.AreaModels.v1.CurrencyPairComponent;
using Nozomi.Data.AreaModels.v1.RequestProperty;
using Nozomi.Data.WebModels;

namespace Nozomi.Data.AreaModels.v1.CurrencyPairRequest
{
    public class CreateCurrencyPairRequest
    {
        public long CurrencyPairId { get; set; }
        
        public RequestType RequestType { get; set; }
        
        public string DataPath { get; set; }
        
        public int Delay { get; set; }
        
        public ICollection<CreateCurrencyPairComponent> RequestComponents { get; set; }
        
        public ICollection<CreateRequestProperty> RequestProperties { get; set; }
    }
}
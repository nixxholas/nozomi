using System.Collections.Generic;
using Nozomi.Data.AreaModels.v1.CurrencyPairComponent;
using Nozomi.Data.AreaModels.v1.RequestProperty;

namespace Nozomi.Data.AreaModels.v1.CurrencyPairRequest
{
    public class UpdateCurrencyPairRequest : CreateCurrencyPairRequest
    {
        public long Id { get; set; }
        
        public bool IsEnabled { get; set; }
        
        public new ICollection<UpdateCurrencyPairComponent> RequestComponents { get; set; }
        
        public new ICollection<UpdateRequestProperty> RequestProperties { get; set; }
    }
}
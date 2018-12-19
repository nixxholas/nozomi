using System.Collections;
using System.Collections.Generic;
using Nozomi.Data.AreaModels.v1.CurrencyPairComponent;
using Nozomi.Data.AreaModels.v1.RequestProperty;
using Nozomi.Data.WebModels;

namespace Nozomi.Data.AreaModels.v1.CurrencyPairRequest
{
    public class UpdateCurrencyPairRequest
    {
        public long Id { get; set; }
        
        public long CurrencyPairId { get; set; }
        
        public RequestType RequestType { get; set; }
        
        public string DataPath { get; set; }
        
        public int Delay { get; set; }
        
        public bool IsEnabled { get; set; }
        
        public ICollection<UpdateCurrencyPairComponent> CurrencyPairComponents { get; set; }
        
        public ICollection<UpdateRequestProperty> RequestProperties { get; set; }
    }
}
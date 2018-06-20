using System;
using System.Collections.Generic;
using System.Text;

namespace Nozomi.Data.WebModels
{
    public class CurrencyPairRequestComponent : RequestComponent
    {
        public new CurrencyPairRequest Request { get; set; }
        
        public CurrencyPairRequestComponentType ComponentType { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Nozomi.Data.WebModels
{
    public class CurrencyPairRequest : Request
    {
        public new ICollection<CurrencyPairRequestComponent> RequestComponents { get; set; }
    }
}

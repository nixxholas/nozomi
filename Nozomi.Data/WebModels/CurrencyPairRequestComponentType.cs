using System;
using System.Collections.Generic;
using System.Text;

namespace Nozomi.Data.WebModels
{
    public enum CurrencyPairRequestComponentType
    {
        UNKNOWN = -1,
        ASK = 0,
        BID = 1,
        HIGH = 3,
        LOW = 4
    }
}

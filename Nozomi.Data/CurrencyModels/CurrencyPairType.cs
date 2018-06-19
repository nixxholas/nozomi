using System;

namespace Nozomi.Data.CurrencyModels
{ 
    public enum CurrencyPairType
    {
        UNKNOWN = 0, // Not usable until set.
        TRADEABLE = 1, // Can be used for customers
        EXCHANGEABLE = 2 // Only used for currency exchange
    }
}

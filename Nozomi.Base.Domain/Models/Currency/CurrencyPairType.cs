using System.ComponentModel;

namespace Nozomi.Data.Models.Currency
{ 
    public enum CurrencyPairType
    {
        [Description("None")]
        NONE = -1, // No type for now.
        [Description("Unknown")]
        UNKNOWN = 0, // Not usable until set.
        [Description("Tradeable")]
        TRADEABLE = 1, // Can be used for customers
        [Description("Exchangeable")]
        EXCHANGEABLE = 2 // Only used for currency exchange
    }
}

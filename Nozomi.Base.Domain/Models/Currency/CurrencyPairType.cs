using System.ComponentModel;

namespace Nozomi.Data.Models.Currency
{ 
    public enum CurrencyPairType
    {
        [Description("Unknown")]
        UNKNOWN = 0, // Not usable until set.
        [Description("Tradeable")]
        TRADEABLE = 1, // Can be used for customers
        [Description("Exchangeable")]
        EXCHANGEABLE = 2 // Only used for currency exchange
    }
}

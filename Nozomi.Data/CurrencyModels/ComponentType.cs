using System;
using System.ComponentModel;

namespace Nozomi.Data.CurrencyModels
{
    public enum ComponentType
    {
        [Description("Unknown")]
        Unknown = 0,
        [Description("Ask")]
        Ask = 1,
        [Description("Bid")]
        Bid = 2
    }
}

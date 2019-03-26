using System.ComponentModel;

namespace Nozomi.Data.Models.Currency
{
    public enum CurrencyPropertyType
    {
        [Description("Website")]
        Website = 1,
        [Description("Twitter")]
        Twitter = 10,
        [Description("Medium")]
        Medium = 11
    }
}
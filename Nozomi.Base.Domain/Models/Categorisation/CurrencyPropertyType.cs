using System.ComponentModel;

namespace Nozomi.Data.Models.Categorisation
{
    public enum CurrencyPropertyType
    {
        Generic = 0,
        [Description("Website")]
        Website = 1,
        [Description("Twitter")]
        Twitter = 10,
        [Description("Medium")]
        Medium = 11
    }
}
using System.ComponentModel;

namespace Nozomi.Data.ViewModels.Currency
{
    public enum CurrencySortingEnum
    {
        [Description("None")]
        None = 0,
        [Description("Name")]
        Name = 1,
        [Description("Abbreviation")]
        Abbreviation = 2,
        [Description("Slug")]
        Slug = 3,
        [Description("Type")]
        Type = 4
    }
}
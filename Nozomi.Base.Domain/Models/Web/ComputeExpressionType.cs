using System.ComponentModel;

namespace Nozomi.Data.Models.Web
{
    public enum ComputeExpressionType
    {
        [Description("Hardcoded value. Doesn't change over time unless you do something about it.")]
        Generic = 1,
        [Description("Raw value obtained from a component.")]
        Raw = 2,
        [Description("Another compute's value.")]
        Computed = 3
    }
}
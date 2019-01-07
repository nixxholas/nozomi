using System.ComponentModel;

namespace Nozomi.Base.Identity.Models.Subscription
{
    public enum Plan
    {
        [Description("Basic")]
        Basic = 0,
        [Description("Plus")]
        Plus = 1,
        [Description("Business")]
        Business = 2
    }
}
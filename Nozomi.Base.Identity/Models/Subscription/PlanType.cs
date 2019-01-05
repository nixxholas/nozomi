using System.ComponentModel;

namespace Nozomi.Base.Identity.Models.Subscription
{
    public enum PlanType
    {
        [Description("basic")]
        Basic = 1,
        [Description("plus")]
        Plus = 2,
        // Business Basic and Business Plus in the future.
    }
}
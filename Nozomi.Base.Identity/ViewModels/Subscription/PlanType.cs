using System.ComponentModel;

namespace Nozomi.Base.Identity.ViewModels.Subscription
{
    public enum PlanType
    {
        [Description("basic")]
        Basic = 0,
        [Description("plus")]
        Plus = 2000,
        // Business Basic and Business Plus in the future.
    }
}
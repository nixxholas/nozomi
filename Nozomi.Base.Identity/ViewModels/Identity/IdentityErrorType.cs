using System.ComponentModel;

namespace Nozomi.Base.Identity.ViewModels.Identity
{
    public enum IdentityErrorType
    {
        // Deprecated
        [Description("There was a problem with the creation of your account relating to our payment service partner.")]
        CreateAccountStripeIssue = 100,
        [Description("There was a problem with the propagation of your account relating to our payment service partner.")]
        CreateAccountStripeSourceIssue = 101,
        [Description("There was a problem obtaining Stripe metadata for you.")]
        CreateAccountStripeCustomerIdIssue = 102
    }
}
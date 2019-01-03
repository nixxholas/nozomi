using System.ComponentModel;

namespace Nozomi.Base.Identity.Models.Identity
{
    public enum IdentityErrorType
    {
        [Description("There was a problem with the creation of your account relating to our payment service partner.")]
        CreateAccountStripeIssue = 100,
        [Description("There was a problem with the propagation of your account relating to our payment service partner.")]
        CreateAccountStripeSourceIssue = 101
    }
}
using System.ComponentModel;

namespace Nozomi.Base.Identity.Models.Identity
{
    public enum IdentityErrorType
    {
        [Description("There was a problem with the creation of your account.")]
        CreateAccountStripeIssue = 100
    }
}
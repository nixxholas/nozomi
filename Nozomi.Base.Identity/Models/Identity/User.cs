using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Nozomi.Base.Identity.Models.Subscription;

namespace Nozomi.Base.Identity.Models.Identity
{
    public class User : IdentityUser<long>
    {
        public string StripeCustomerId { get; set; }
        
        public string StripeSourceId { get; set; }
        
        public virtual ICollection<ApiToken> ApiTokens { get; set; }
        
        public virtual ICollection<UserSubscription> UserSubscriptions { get; set; }
        
        public virtual ICollection<UserClaim> UserClaims { get; set; }

        public virtual ICollection<UserLogin> UserLogins { get; set; }
        
        public virtual ICollection<UserRole> UserRoles { get; set; }
        
        public virtual ICollection<UserToken> UserTokens { get; set; }
    }
}
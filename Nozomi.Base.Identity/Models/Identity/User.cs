using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Nozomi.Base.Identity.Models.Subscription;

namespace Nozomi.Base.Identity.Models.Identity
{
    public class User : IdentityUser<long>
    {
        public string StripeSourceId { get; set; }
        
        public virtual ICollection<DevKey> DevKeys { get; set; }
        
        public virtual ICollection<UserClaim> UserClaims { get; set; }

        public virtual ICollection<UserLogin> UserLogins { get; set; }
        
        public virtual ICollection<UserRole> UserRoles { get; set; }
        
        public virtual ICollection<UserToken> UserTokens { get; set; }
    }
}
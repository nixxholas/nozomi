using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Nozomi.Base.Identity.Models.Identity
{
    public class User : IdentityUser<long>
    {
        public virtual ICollection<UserClaim> UserClaims { get; }
        
        public virtual ICollection<UserLogin> UserLogins { get; }
        
        public virtual ICollection<UserRole> UserRoles { get; }
        
        public virtual ICollection<UserToken> UserTokens { get; }
    }
}
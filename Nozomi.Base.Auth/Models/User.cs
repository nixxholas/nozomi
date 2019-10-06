using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Nozomi.Base.Auth.Models.Wallet;

namespace Nozomi.Base.Auth.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class User : IdentityUser
    {
        public ICollection<Address> Addresses { get; set; }
        
        public virtual ICollection<UserClaim> UserClaims { get; set; }

        public virtual ICollection<UserLogin> UserLogins { get; set; }
        
        public virtual ICollection<UserRole> UserRoles { get; set; }
        
        public virtual ICollection<UserToken> UserTokens { get; set; }
    }
}
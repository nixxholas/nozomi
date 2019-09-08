using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Nozomi.Base.Auth.Models
{
    public class Role : IdentityRole
    {
        public virtual ICollection<RoleClaim> RoleClaims { get; set; }
        
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
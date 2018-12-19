using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Nozomi.Base.Identity.Models.Identity
{
    public class Role : IdentityRole<long>
    {
        public ICollection<RoleClaim> RoleClaims { get; set; }
        
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
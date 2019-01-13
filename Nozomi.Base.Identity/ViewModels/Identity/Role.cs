using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Nozomi.Base.Identity.ViewModels.Identity
{
    public class Role : IdentityRole<long>
    {
        public virtual ICollection<RoleClaim> RoleClaims { get; set; }
        
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
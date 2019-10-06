using System;
using Microsoft.AspNetCore.Identity;

namespace Nozomi.Base.Auth.Models
{
    public class RoleClaim : IdentityRoleClaim<string>
    {
        public virtual Role Role { get; set; }
    }
}
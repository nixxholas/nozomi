using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Nozomi.Base.Identity.Models.Identity
{
    public class RoleClaim : IdentityRoleClaim<long>
    {
        public Role Role { get; set; }
    }
}
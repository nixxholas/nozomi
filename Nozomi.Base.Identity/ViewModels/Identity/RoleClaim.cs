using Microsoft.AspNetCore.Identity;

namespace Nozomi.Base.Identity.ViewModels.Identity
{
    public class RoleClaim : IdentityRoleClaim<long>
    {
        public virtual Role Role { get; set; }
    }
}
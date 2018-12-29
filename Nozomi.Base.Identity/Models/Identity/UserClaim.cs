using Microsoft.AspNetCore.Identity;

namespace Nozomi.Base.Identity.Models.Identity
{
    public class UserClaim : IdentityUserClaim<long>
    {
        public virtual User User { get; set; }
    }
}
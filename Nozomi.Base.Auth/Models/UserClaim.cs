using Microsoft.AspNetCore.Identity;

namespace Nozomi.Base.Auth.Models
{
    public class UserClaim : IdentityUserClaim<string>
    {
        public virtual User User { get; set; }
    }
}
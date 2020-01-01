using Microsoft.AspNetCore.Identity;

namespace Nozomi.Base.Auth.Models
{
    public class UserToken : IdentityUserToken<string>
    {
        public virtual User User { get; set; }
    }
}
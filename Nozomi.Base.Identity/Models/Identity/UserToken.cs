using Microsoft.AspNetCore.Identity;

namespace Nozomi.Base.Identity.Models.Identity
{
    public class UserToken : IdentityUserToken<long>
    {
        public virtual User User { get; set; }
    }
}
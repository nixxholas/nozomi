using Microsoft.AspNetCore.Identity;

namespace Nozomi.Base.Identity.Models.Identity
{
    public class UserLogin : IdentityUserLogin<long>
    {
        public User User { get; set; }
    }
}
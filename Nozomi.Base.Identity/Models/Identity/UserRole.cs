using Microsoft.AspNetCore.Identity;

namespace Nozomi.Base.Identity.Models.Identity
{
    public class UserRole : IdentityUserRole<long>
    {
        public Role Role { get; set; }
        
        public User User { get; set; }
    }
}
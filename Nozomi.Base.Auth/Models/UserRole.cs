using Microsoft.AspNetCore.Identity;

namespace Nozomi.Base.Auth.Models
{
    public class UserRole : IdentityUserRole<string>
    {
        public virtual Role Role { get; set; }
        
        public virtual User User { get; set; }
    }
}
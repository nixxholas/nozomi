using Microsoft.AspNetCore.Identity;

namespace Nozomi.Base.Identity.ViewModels.Identity
{
    public class UserRole : IdentityUserRole<long>
    {
        public virtual Role Role { get; set; }
        
        public virtual User User { get; set; }
    }
}
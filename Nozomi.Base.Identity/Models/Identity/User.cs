using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Nozomi.Base.Identity.Models.Identity
{
    public class User : IdentityUser<long>
    {
        public ICollection<UserClaim> UserClaims { get; }
    }
}
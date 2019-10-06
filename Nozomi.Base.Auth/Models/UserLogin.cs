using System;
using Microsoft.AspNetCore.Identity;

namespace Nozomi.Base.Auth.Models
{
    public class UserLogin : IdentityUserLogin<string>
    {
        public virtual User User { get; set; }
    }
}
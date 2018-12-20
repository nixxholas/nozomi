using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Service.Identity.Managers;

namespace Nozomi.Service.Identity.Factories
{
    public class NozomiUserClaimsPrincipalFactory: UserClaimsPrincipalFactory<User, Role>
    {
        public new NozomiUserManager UserManager;
        
        public NozomiUserClaimsPrincipalFactory( 
            NozomiUserManager userManager,
            RoleManager<Role> roleManager, 
            IOptions<IdentityOptions> optionsAccessor) 
            : base(userManager, roleManager, optionsAccessor)
        {
            UserManager = userManager;
        }
 
        public override async Task<ClaimsPrincipal> CreateAsync(User user)
        {
            var principal = await base.CreateAsync(user);
            var identity = (ClaimsIdentity)principal.Identity;
 
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Role, "user")
            };
 
            identity.AddClaims(claims);
            return principal;
        }
    }
}
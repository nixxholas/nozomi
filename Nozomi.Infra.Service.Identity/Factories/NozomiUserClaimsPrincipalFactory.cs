using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
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
        public new RoleManager<Role> RoleManager;
        
        public NozomiUserClaimsPrincipalFactory(NozomiUserManager userManager, RoleManager<Role> roleManager, 
            IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }
        
        /// <summary>
        /// Creates a <see cref="T:System.Security.Claims.ClaimsPrincipal" /> from an user asynchronously.
        /// </summary>
        /// <param name="user">The user to create a <see cref="T:System.Security.Claims.ClaimsPrincipal" /> from.</param>
        /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous creation
        /// operation, containing the created <see cref="T:System.Security.Claims.ClaimsPrincipal" />.</returns>
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
        
        /// <summary>Generate the claims for a user.</summary>
        /// <param name="user">The user to create a <see cref="T:System.Security.Claims.ClaimsIdentity" /> from.</param>
        /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous creation operation, containing the created <see cref="T:System.Security.Claims.ClaimsIdentity" />.</returns>
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            try
            {
                var userId = await UserManager.GetUserIdAsync(user);
                var userNameAsync = await UserManager.GetUserNameAsync(user);
                
                var id = new ClaimsIdentity("Identity.Application", 
                    this.Options.ClaimsIdentity.UserNameClaimType, this.Options.ClaimsIdentity.RoleClaimType);
                id.AddClaim(new Claim(this.Options.ClaimsIdentity.UserIdClaimType, userId));
                id.AddClaim(new Claim(this.Options.ClaimsIdentity.UserNameClaimType, userNameAsync));
                
                ClaimsIdentity claimsIdentity;
                if (this.UserManager.SupportsUserSecurityStamp)
                {
                    claimsIdentity = id;
                    string type = this.Options.ClaimsIdentity.SecurityStampClaimType;
                    claimsIdentity.AddClaim(new Claim(type, await this.UserManager.GetSecurityStampAsync(user)));
                    claimsIdentity = (ClaimsIdentity) null;
                    type = (string) null;
                }
                if (this.UserManager.SupportsUserClaim)
                {
                    claimsIdentity = id;
                    claimsIdentity.AddClaims((IEnumerable<Claim>) await this.UserManager.GetClaimsAsync(user));
                    claimsIdentity = (ClaimsIdentity) null;
                }
                return id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ClaimsIdentity();
            }
        }
    }
}
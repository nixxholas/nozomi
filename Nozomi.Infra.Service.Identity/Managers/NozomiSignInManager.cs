using System;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nozomi.Base.Identity;
using Nozomi.Base.Identity.Models.Identity;

namespace Nozomi.Service.Identity.Managers
{
    public class NozomiSignInManager : SignInManager<User>
    {
        public NozomiSignInManager(NozomiUserManager userManager, IHttpContextAccessor contextAccessor, 
            IUserClaimsPrincipalFactory<User> claimsFactory, IOptions<IdentityOptions> optionsAccessor, 
            ILogger<NozomiSignInManager> logger, IAuthenticationSchemeProvider schemes) : 
            base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes)
        {
        }
        
        /// <summary>
        /// Returns true if the principal has an identity with the application cookie identity
        /// </summary>
        /// <param name="principal">The <see cref="ClaimsPrincipal"/> instance.</param>
        /// <returns>True if the user is logged in with identity.</returns>
        public override bool IsSignedIn(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            return principal?.Identities != null &&
                   principal.Identities.Any(i => i.AuthenticationType == IdentityConstants.ApplicationScheme);
        }
        
        /// <summary>
        /// Attempts to sign in the specified <paramref name="userName"/> and <paramref name="password"/> combination
        /// as an asynchronous operation.
        /// </summary>
        /// <param name="userName">The user name to sign in.</param>
        /// <param name="password">The password to attempt to sign in with.</param>
        /// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed.</param>
        /// <param name="lockoutOnFailure">Flag indicating if the user account should be locked if the sign in fails.</param>
        /// <returns>The task object representing the asynchronous operation containing the <see name="SignInResult"/>
        /// for the sign-in attempt.</returns>
        public override async Task<SignInResult> PasswordSignInAsync(string emailOrUsername, string password,
            bool isPersistent, bool lockoutOnFailure)
        {
            if (IsValidEmail(emailOrUsername))
            {
                var user = await UserManager.FindByEmailAsync(emailOrUsername);
            
                if (user == null)
                {
                    return SignInResult.Failed;
                }

                return await base.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
            }
            else
            {
                var user = await UserManager.FindByNameAsync(emailOrUsername);
            
                if (user == null)
                {
                    return SignInResult.Failed;
                }

                return await base.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
            }
        }
        
        /// <summary>
        /// Regenerates the user's application cookie, whilst preserving the existing
        /// AuthenticationProperties like rememberMe, as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user whose sign-in cookie should be refreshed.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public override async Task RefreshSignInAsync(User user)
        {
            var auth = await Context.AuthenticateAsync(IdentityConstants.ApplicationScheme);
            var authenticationMethod = auth?.Principal?.FindFirstValue(ClaimTypes.AuthenticationMethod);
            await SignInAsync(user, auth?.Properties, authenticationMethod);
        }
        
        /// <summary>
        /// Signs the current user out of the application.
        /// </summary>
        public override async Task SignOutAsync()
        {
            await Context.SignOutAsync(IdentityConstants.ApplicationScheme);
            await Context.SignOutAsync(IdentityConstants.ExternalScheme);
            await Context.SignOutAsync(IdentityConstants.TwoFactorUserIdScheme);
        }
        
        private bool IsValidEmail(string emailaddress)
        {
            try
            {
                var m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
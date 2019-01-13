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
using Nozomi.Base.Identity.ViewModels.Identity;

namespace Nozomi.Service.Identity.Managers
{
    public class NozomiSignInManager : SignInManager<User>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private HttpContext _context;
        private new readonly NozomiUserManager UserManager;
        
        public NozomiSignInManager(NozomiUserManager userManager, IHttpContextAccessor contextAccessor, 
            IUserClaimsPrincipalFactory<User> claimsFactory, IOptions<IdentityOptions> optionsAccessor, 
            ILogger<NozomiSignInManager> logger, IAuthenticationSchemeProvider schemes) : 
            base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes)
        {
            _contextAccessor = contextAccessor;
            UserManager = userManager;
        }
        
        /// <summary>
        /// The <see cref="T:Microsoft.AspNetCore.Http.HttpContext" /> used.
        /// </summary>
        public new HttpContext Context
        {
            get
            {
                var httpContext = _context ?? _contextAccessor?.HttpContext;
                if (httpContext != null)
                    return httpContext;
                throw new InvalidOperationException("HttpContext must not be null.");
            }
            set => _context = value;
        }
        
        /// <summary>
        /// Returns a flag indicating whether the specified user can sign in.
        /// </summary>
        /// <param name="user">The user whose sign-in status should be returned.</param>
        /// <returns>
        /// The task object representing the asynchronous operation, containing a flag that is true
        /// if the specified user can sign-in, otherwise false.
        /// </returns>
        public override async Task<bool> CanSignInAsync(User user)
        {
            if (Options.SignIn.RequireConfirmedEmail && !(await UserManager.IsEmailConfirmedAsync(user)))
            {
                Logger.LogWarning(0, "User {userId} cannot sign in without a confirmed email.", await UserManager.GetUserIdAsync(user));
                return false;
            }
            if (Options.SignIn.RequireConfirmedPhoneNumber && !(await UserManager.IsPhoneNumberConfirmedAsync(user)))
            {
                Logger.LogWarning(1, "User {userId} cannot sign in without a confirmed phone number.", await UserManager.GetUserIdAsync(user));
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Creates a <see cref="ClaimsPrincipal"/> for the specified <paramref name="user"/>, as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user to create a <see cref="ClaimsPrincipal"/> for.</param>
        /// <returns>The task object representing the asynchronous operation, containing the ClaimsPrincipal for the specified user.</returns>
        public override async Task<ClaimsPrincipal> CreateUserPrincipalAsync(User user) 
            => await ClaimsFactory.CreateAsync(user);
        
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

                return await PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
            }
            else
            {
                var user = await UserManager.FindByNameAsync(emailOrUsername);
            
                if (user == null)
                {
                    return SignInResult.Failed;
                }

                return await PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
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
        /// Signs in the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user to sign-in.</param>
        /// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed.</param>
        /// <param name="authenticationMethod">Name of the method used to authenticate the user.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public override Task SignInAsync(User user, bool isPersistent, string authenticationMethod = null)
        {
            return SignInAsync(user, new AuthenticationProperties { IsPersistent = isPersistent }, authenticationMethod);
        }

        /// <summary>
        /// Signs in the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user to sign-in.</param>
        /// <param name="authenticationProperties">Properties applied to the login and authentication cookie.</param>
        /// <param name="authenticationMethod">Name of the method used to authenticate the user.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public override async Task SignInAsync(User user, AuthenticationProperties authenticationProperties, string authenticationMethod = null)
        {
            try
            {
                var userPrincipal = await CreateUserPrincipalAsync(user);
                // Review: should we guard against CreateUserPrincipal returning null?
                if (authenticationMethod != null)
                {
                    userPrincipal.Identities.First().AddClaim(new Claim(ClaimTypes.AuthenticationMethod, authenticationMethod));
                }

                await Context.SignInAsync(IdentityConstants.ApplicationScheme,
                    userPrincipal,
                    authenticationProperties ?? new AuthenticationProperties());
                // https://github.com/aspnet/Security/issues/1131#issuecomment-280896191
                // Context.User = userPrincipal;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
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
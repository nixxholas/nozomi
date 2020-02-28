// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bogus;
using IdentityModel;
using IdentityModel.Client;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nozomi.Auth.Controllers.Home;
using Nozomi.Base.Auth.Events;
using Nozomi.Base.Auth.Global;
using Nozomi.Base.Auth.Models;
using Nozomi.Base.Auth.Models.Wallet;
using Nozomi.Base.Auth.ViewModels.Account;
using Nozomi.Base.BCL.Helpers.Native.Text;
using Nozomi.Base.Blockchain.Auth.Query.Validating;
using Nozomi.Infra.Auth.Services.Address;
using Nozomi.Infra.Auth.Services.Stripe;
using Nozomi.Infra.Auth.Services.User;
using Nozomi.Infra.Blockchain.Auth.Events.Interfaces;
using Nozomi.Preprocessing.Events.Interfaces;

namespace Nozomi.Auth.Controllers.Account
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class AccountController : BaseController<AccountController>
    {
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IAddressEvent _addressEvent;
        private readonly IValidatingEvent _validatingEvent;
        private readonly IEventService _events;
        private readonly IAddressService _addressService;
        private readonly IStripeService _stripeService;
        private readonly IUserService _userService;

        public AccountController(
            ILogger<AccountController> logger,
            IWebHostEnvironment webHostEnvironment,
            IEmailSender emailSender,
            RoleManager<Role> roleManager,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IAddressEvent addressEvent,
            IValidatingEvent validatingEvent,
            IEventService events, 
            IAddressService addressService,
            IStripeService stripeService,
            IUserService userService) : base(logger, webHostEnvironment)
        {
            _emailSender = emailSender;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _addressEvent = addressEvent;
            _validatingEvent = validatingEvent;
            _events = events;
            _addressService = addressService;
            _stripeService = stripeService;
            _userService = userService;
        }
        
        #region Helpers

        protected void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        protected Task<User> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        protected IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code, string returnUrl)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);

            var vm = BuildConfirmEmailViewModel(result.Succeeded, returnUrl);
            
            // return View(result.Succeeded ? "ConfirmEmail" : "Error");
            return View(vm);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            var vm = BuildRegisterViewModel(returnUrl);
            
            return View(vm);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterInputModel model, string button = null)
        {
            ViewData["ReturnUrl"] = model.ReturnUrl;

            if (!string.IsNullOrEmpty(button) && button.Equals("register"))
            {
                if (model.IsValid())
                {
                    var user = new User {UserName = model.Username, Email = model.Email};
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        // Default role binding
                        var roleResult = await _userManager.AddToRoleAsync(user, "User");

                        if (roleResult.Succeeded)
                        {
                            // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                            // Send an email with this link
                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            var callbackUrl = Url.Action("ConfirmEmail", "Account",
                                new {userId = user.Id, code = code, returnUrl = model.ReturnUrl}, protocol: 
                                HttpContext.Request.Scheme);
                            await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                                "Please confirm your account by clicking this link: <a href=\"" + callbackUrl +
                                "\">link</a>");
                            // await _signInManager.SignInAsync(user, isPersistent: false);
                            _logger.LogInformation(3, "User created a new account with password.");
                            
                            // TODO: Inform about successful registration, inform the user to confirm his email.

                            return RedirectToAction("Login", "Account", new
                            {
                                returnUrl = model.ReturnUrl,
                                userEmail = model.Email
                            });
                        }

                        ModelState.AddModelError(user.Id, AccountOptions.FailedToJoinRole);
                    }
                    else
                    {
                        foreach (IdentityError resultError in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, resultError.Description);    
                        }
                    }
                }
                else
                {
                    // Default error state when empty form is being submitted
                    ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
                }
            }
            else if (!string.IsNullOrEmpty(button) && button.Equals("cancel") && !string.IsNullOrEmpty(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            // If we got this far, something failed, redisplay form
            // return BadRequest(model);
            var vm = BuildRegisterViewModelFromInput(model);
            return View(vm);
        }

        private async Task<User> Web3Create(string signature, string address, string message, string clientId = null)
        {
            if (_addressEvent.IsBinded(address))
            {
                return null;
            }

            var fakeUser = new Faker<User>()
                //Basic rules using built-in generators
                .RuleFor(u => u.UserName, (f, u) => f.Internet.UserName(f.Name.FirstName(), f.Name.LastName()))
                .RuleFor(u => u.NormalizedUserName, (f, u) => u.UserName.ToUpper())
                //.RuleFor(u => u.Avatar, f => f.Internet.Avatar())
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(f.Name.FirstName(), f.Name.LastName()))
                //.RuleFor(u => u.SomethingUnique, f => $"Value {f.UniqueIndex}")
                .FinishWith((f, u) => { Console.WriteLine("User Created! Id={0}", u.Id); });

            var generatedFakeUser = fakeUser.Generate();
            var user = new User {UserName = generatedFakeUser.UserName, Email = generatedFakeUser.Email};
            var result = await _userManager.CreateAsync(user, signature);

            if (result.Succeeded)
            {
                // Default role binding
                var roleResult = await _userManager.AddToRoleAsync(user, "User");

                if (roleResult.Succeeded)
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                    // Send an email with this link
//                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
//                    var callbackUrl = Url.Action("ConfirmEmail", "Account", 
//                        new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
//                    await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
//                        "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    //_logger.LogInformation(3, "User created a new account with password.");

                    //return Redirect(model.ReturnUrl);

                    var createdAddress = _addressService.Create(user.Id, address, AddressType.Ethereum);
                    if (string.IsNullOrEmpty(createdAddress))
                        await _events.RaiseAsync(new UserCreateClaimFailureEvent(user.UserName, 
                            "Address creation failed", "Web3Create",clientId));

                    var createdDefaultAddressClaim = await _userManager.AddClaimAsync(user, 
                        new Claim(NozomiJwtClaimTypes.DefaultWallet, address));

                    return !string.IsNullOrWhiteSpace(createdAddress) && createdDefaultAddressClaim.Succeeded 
                        ? user : null;
                }

                ModelState.AddModelError(user.Id, AccountOptions.FailedToJoinRole);
            }

            return null;
        }

        [HttpPost]
        public async Task<IActionResult> Web3Register([FromBody] Web3InputModel model, string returnUrl = null)
        {
            // check if we are in the context of an authorization request
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            
            ViewData["ReturnUrl"] = returnUrl;
            
            if (ModelState.IsValid && _validatingEvent.ValidateOwner(new ValidateOwnerQuery
            {
                ClaimerAddress = model.Address, RawMessage = model.Message, Signature = model.Signature
            }))
            {
                var user = await Web3Create(model.Signature, model.Address, model.Message);
                if (user != null && !string.IsNullOrWhiteSpace(user.UserName))
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                    // Send an email with this link
//                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
//                    var callbackUrl = Url.Action("ConfirmEmail", "Account", 
//                        new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
//                    await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
//                        "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(3, "User created a new account with password.");

                    //return Redirect(model.ReturnUrl);
                    return Ok();
                }
            }

            return BadRequest(model);
        }

        /// <summary>
        /// Entry point into the login workflow
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl, string userEmail = null)
        {
            // build a model so we know what to show on the login page
            var vm = await BuildLoginViewModelAsync(returnUrl);

            if (vm.IsExternalLoginOnly)
            {
                // we only have one option for logging in and it's an external provider
                return RedirectToAction("Challenge", "External", new {provider = vm.ExternalLoginScheme, returnUrl});
            }

            // TODO: This is extremely hacky, make a more beautified version of informing the user he/she has successfully registered
            if (!string.IsNullOrEmpty(userEmail))
            {
                var user = await _userManager.FindByEmailAsync(userEmail);

                if (!user.EmailConfirmed)
                    vm.UserRegistrationSuccessful = true;
            }

            return View(vm);
        }

        // Backed up Login
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Login(LoginInputModel model, string button)
//        {
//            // check if we are in the context of an authorization request
//            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
//
//            // the user clicked the "cancel" button
//            if (button != "login")
//            {
//                if (context != null)
//                {
//                    // if the user cancels, send a result back into IdentityServer as if they 
//                    // denied the consent (even if this client does not require consent).
//                    // this will send back an access denied OIDC error response to the client.
//                    await _interaction.GrantConsentAsync(context, ConsentResponse.Denied);
//
//                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
//                    if (await _clientStore.IsPkceClientAsync(context.ClientId))
//                    {
//                        // if the client is PKCE then we assume it's native, so this change in how to
//                        // return the response is for better UX for the end user.
//                        return View("Redirect", new RedirectViewModel {RedirectUrl = model.ReturnUrl});
//                    }
//
//                    return Redirect(model.ReturnUrl);
//                }
//                else
//                {
//                    // since we don't have a valid context, then we just go back to the home page
//                    return Redirect("~/");
//                }
//            }
//
//            if (ModelState.IsValid)
//            {
//                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password,
//                    model.RememberLogin, lockoutOnFailure: true);
//                if (result.Succeeded)
//                {
//                    var user = await _userManager.FindByNameAsync(model.Username);
//                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName,
//                        clientId: context?.ClientId));
//
//                    if (context != null)
//                    {
//                        if (await _clientStore.IsPkceClientAsync(context.ClientId))
//                        {
//                            // if the client is PKCE then we assume it's native, so this change in how to
//                            // return the response is for better UX for the end user.
//                            return View("Redirect", new RedirectViewModel {RedirectUrl = model.ReturnUrl});
//                        }
//
//                        // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
//                        return Redirect(model.ReturnUrl);
//                    }
//
//                    // request for a local page
//                    if (Url.IsLocalUrl(model.ReturnUrl))
//                    {
//                        return Redirect(model.ReturnUrl);
//                    }
//                    else if (string.IsNullOrEmpty(model.ReturnUrl))
//                    {
//                        return Redirect("~/");
//                    }
//                    else
//                    {
//                        // user might have clicked on a malicious link - should be logged
//                        throw new Exception("invalid return URL");
//                    }
//                }
//
//                await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials",
//                    clientId: context?.ClientId));
//                ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
//            }
//
//            // something went wrong, show form with error
//            var vm = await BuildLoginViewModelAsync(model);
//            return View(vm);
//        }

        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model, string button)
        {
            // check if we are in the context of an authorization request
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            
            // the user clicked the "register" button
            if (!string.IsNullOrWhiteSpace(button)
                && button.Equals("register", StringComparison.InvariantCultureIgnoreCase))
            {
                return RedirectToAction("Register", "Account", 
                    new { returnUrl = model.ReturnUrl });
            }

            // the user clicked the "cancel" button
            if (!string.IsNullOrWhiteSpace(button)
                && button.Equals("cancel", StringComparison.InvariantCultureIgnoreCase))
            {
                if (context != null)
                {
                    // if the user cancels, send a result back into IdentityServer as if they 
                    // denied the consent (even if this client does not require consent).
                    // this will send back an access denied OIDC error response to the client.
                    await _interaction.GrantConsentAsync(context, ConsentResponse.Denied);

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    if (await _clientStore.IsPkceClientAsync(context.ClientId))
                    {
                        // if the client is PKCE then we assume it's native, so this change in how to
                        // return the response is for better UX for the end user.
                        return View("Redirect", new RedirectViewModel {RedirectUrl = model.ReturnUrl});
                    }

                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    // since we don't have a valid context, then we just go back to the home page
                    return Redirect("~/");
                }
            }

            // The user is logging in through password authentication
            if (model.IsValidForPassAuth())
            {
                var user = new User();
                if (model.UsernameBasedIsValid())
                    user = await _userManager.FindByNameAsync(model.Username);
                else if (model.EmailBasedIsValid())
                    user = await _userManager.FindByEmailAsync(model.Username);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
                } else {
                    if (!user.EmailConfirmed && !_webHostEnvironment.IsDevelopment())
                {
                    await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "email not confirmed",
                        clientId: context?.ClientId));
                    ModelState.AddModelError(string.Empty, AccountOptions.EmailNotConfirmed);
                }
                    else
                    {
                        var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password,
                            model.RememberLogin, lockoutOnFailure: true);
                        if (result.Succeeded)
                        {
                            await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName,
                                clientId: context?.ClientId));

                            if (context != null)
                            {
                                if (await _clientStore.IsPkceClientAsync(context.ClientId))
                                {
                                    // if the client is PKCE then we assume it's native, so this change in how to
                                    // return the response is for better UX for the end user.
                                    return View("Redirect", new RedirectViewModel {RedirectUrl = model.ReturnUrl});
                                }

                                // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                                return Redirect(model.ReturnUrl);
                            }

                            // request for a local page
                            if (Url.IsLocalUrl(model.ReturnUrl))
                            {
                                return Redirect(model.ReturnUrl);
                            }
                            else if (string.IsNullOrEmpty(model.ReturnUrl))
                            {
                                return Redirect("~/");
                            }
                            else
                            {
                                // user might have clicked on a malicious link - should be logged
                                throw new Exception("invalid return URL");
                            }
                        }

                        await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials",
                            clientId: context?.ClientId));
                        ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
                    }
                }
            }
            else if (model.IsValid() && !string.IsNullOrWhiteSpace(model.Message)
                                     && !string.IsNullOrWhiteSpace(model.Address)
                                     && !string.IsNullOrWhiteSpace(model.Signature))
            {
                var addrEntity = _addressEvent.Authenticate(model.Address, model.Signature, model.Message);

                // Is this address already registered?
                if (addrEntity != null)
                {
                    // Sign in
                    var user = await _userManager.FindByIdAsync(addrEntity.UserId);
                    await _signInManager.SignInAsync(user, true);

                    // Raise an event to say that the sign in is successful
                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName,
                        clientId: context?.ClientId));

                    if (context != null)
                    {
                        if (await _clientStore.IsPkceClientAsync(context.ClientId))
                        {
                            // if the client is PKCE then we assume it's native, so this change in how to
                            // return the response is for better UX for the end user.
                            return View("Redirect", new RedirectViewModel {RedirectUrl = model.ReturnUrl});
                        }

                        // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                        return Redirect(model.ReturnUrl);
                    }

                    // request for a local page
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect("~/");
                    }
                    else
                    {
                        // user might have clicked on a malicious link - should be logged
                        throw new Exception("invalid return URL");
                    }
                }
                else
                {
                    // Nope, let's try to get this shit registered.
                    var user = await Web3Create(model.Signature, model.Address, model.Message);

                    if (user != null && !string.IsNullOrWhiteSpace(user.UserName))
                    {
                        await _signInManager.SignInAsync(user, true);

                        // Raise an event to say that the sign in is successful
                        await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName,
                            clientId: context?.ClientId));

                        if (context != null)
                        {
                            if (await _clientStore.IsPkceClientAsync(context.ClientId))
                            {
                                // if the client is PKCE then we assume it's native, so this change in how to
                                // return the response is for better UX for the end user.
                                return View("Redirect", new RedirectViewModel {RedirectUrl = model.ReturnUrl});
                            }

                            // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                            return Redirect(model.ReturnUrl);
                        }

                        // request for a local page
                        if (Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        else if (string.IsNullOrEmpty(model.ReturnUrl))
                        {
                            return Redirect("~/");
                        }
                        else
                        {
                            // user might have clicked on a malicious link - should be logged
                            throw new Exception("invalid return URL");
                        }
                    }
                }

                await _events.RaiseAsync(new UserLoginFailureEvent(model.Address, "invalid web3 credentials",
                    clientId: context?.ClientId));
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
            }
            else
            {
                // Default error message when empty invalid form is submitted
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
            }

            // something went wrong, show form with error
            var vm = await BuildLoginViewModelAsync(model);
            return View(vm);
        }

        /// <summary>
        /// Show logout page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            // build a model so the logout page knows what to display
            var vm = await BuildLogoutViewModelAsync(logoutId);

            if (vm.ShowLogoutPrompt == false)
            {
                // if the request for logout was properly authenticated from IdentityServer, then
                // we don't need to show the prompt and can just log the user out directly.
                return await Logout(vm);
            }

            return View(vm);
        }

        /// <summary>
        /// Handle logout page postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            // build a model so the logged out page knows what to display
            var vm = await BuildLoggedOutViewModelAsync(model.LogoutId,
                // https://stackoverflow.com/questions/38772394/how-can-i-get-url-referrer-in-asp-net-core-mvc
                HttpContext.Request.Headers["Referer"]);

            if (User?.Identity.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await _signInManager.SignOutAsync();

                // raise the logout event
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            // check if we need to trigger sign-out at an upstream identity provider
            if (vm.TriggerExternalSignout)
            {
                // build a return URL so the upstream provider will redirect back
                // to us after the user has logged out. this allows us to then
                // complete our single sign-out processing.
                string url = Url.Action("Logout", new {logoutId = vm.LogoutId});

                // this triggers a redirect to the external provider for sign-out
                return SignOut(new AuthenticationProperties {RedirectUri = url}, vm.ExternalAuthenticationScheme);
            }

            return View("LoggedOut", vm);
        }

        //
        // GET: /Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword(string returnUrl)
        {
            ForgotPasswordInputModel model = BuildForgotPasswordInputViewModel(returnUrl);
            
            return View(model);
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordInputModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // Send an email with this link
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", 
                    new ResetPasswordInputModel { Email = user.Email, Code = code }, protocol: HttpContext.Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                    "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                return View("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        
        //
        // GET: /Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation), "Account");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation), "Account");
            }
            AddErrors(result);
            
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/SendCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl = null, bool rememberMe = false)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(user);
            var factorOptions = userFactors
                .Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody]UpdateUserInputModel vm)
        {
            var user = await _userManager.FindByIdAsync(((ClaimsIdentity) User.Identity)
                .Claims.FirstOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject)
                                            || c.Type.Equals(ClaimTypes.NameIdentifier))?.Value);
            
            // Modify the user's profile
            if (user != null)
            {
                await _userService.Update(vm, user.Id);

                return Ok("Account details updated successfully!");
            }

            return BadRequest("Invalid input/s, please ensure that the entries are correctly filled!");
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpHead]
        public async Task<IActionResult> Bootstripe()
        {
            var user = await _userManager.FindByIdAsync(((ClaimsIdentity) User.Identity)
                .Claims.FirstOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject)
                                            || c.Type.Equals(ClaimTypes.NameIdentifier))?.Value);
            
            // Modify the user's profile
            if (user != null)
            {
                if (_userService.HasStripe(user.Id))
                {
                    return BadRequest("Stripe already linked!!");
                }
                else
                {
                    // Setup stripe
                    await _stripeService.PropagateCustomer(user);
                    
                    // Obtain claim
                    var stripeCustomerIdClaim = (await _userManager.GetClaimsAsync(user))
                        .FirstOrDefault(c => c.Type.Equals(NozomiJwtClaimTypes.StripeCustomerId));

                    if (stripeCustomerIdClaim == null)
                    {
                        _logger.LogInformation($"Bootstripe: {user.Id} has failed to propagate Stripe.");
                        return BadRequest("There was an issue with the Stripe link up. Please try again later!");
                    }

                    _logger.LogInformation($"Bootstripe: {user.Id} successful symlink with Stripe with ID " +
                                           $"{stripeCustomerIdClaim.Value}");
                    return Ok("Account details updated successfully!");
                }
            }

            return BadRequest("Invalid input/s, please ensure that the entries are correctly filled!");
        }

        /*****************************************/
        /* helper APIs for the AccountController */
        /*****************************************/
        private ConfirmEmailViewModel BuildConfirmEmailViewModel(bool succeeded, string returnUrl)
        {
            var vm = new ConfirmEmailViewModel
            {
                Succeeded = succeeded,
                PostLogoutRedirectUri = returnUrl
            };
            
            return vm;
        }
        
        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
            {
                var local = context.IdP == IdentityServer4.IdentityServerConstants.LocalIdentityProvider;

                // this is meant to short circuit the UI and only trigger the one external IdP
                var vm = new LoginViewModel
                {
                    EnableLocalLogin = local,
                    ReturnUrl = returnUrl,
                    Username = context?.LoginHint,
                };

                if (!local)
                {
                    vm.ExternalProviders = new[] {new ExternalProvider {AuthenticationScheme = context.IdP}};
                }

                return vm;
            }

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null ||
                            (x.Name.Equals(AccountOptions.WindowsAuthenticationSchemeName,
                                StringComparison.OrdinalIgnoreCase))
                )
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName,
                    AuthenticationScheme = x.Name
                }).ToList();

            var allowLocal = true;
            if (context?.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider =>
                            client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }

        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new LogoutViewModel {LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt};

            if (User?.Identity.IsAuthenticated != true)
            {
                // if the user is not authenticated, then just show logged out page
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            return vm;
        }

        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId, string referralUrl = null)
        {
            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if ((string.IsNullOrEmpty(vm.PostLogoutRedirectUri) || string.IsNullOrWhiteSpace(vm.PostLogoutRedirectUri))
                && StringHelper.IsUrl(referralUrl))
                vm.PostLogoutRedirectUri = referralUrl;

            if (User?.Identity.IsAuthenticated == true)
            {
                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if (idp != null && idp != IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if (providerSupportsSignout)
                    {
                        if (vm.LogoutId == null)
                        {
                            // if there's no current logout context, we need to create one
                            // this captures necessary info from the current logged in user
                            // before we signout and redirect away to the external IdP for signout
                            vm.LogoutId = await _interaction.CreateLogoutContextAsync();
                        }

                        vm.ExternalAuthenticationScheme = idp;
                    }
                }
            }

            return vm;
        }

        private RegisterViewModel BuildRegisterViewModel(string returnUrl, string userName = null, string email = null)
        {
            var vm = new RegisterViewModel
            {
                ReturnUrl = returnUrl,
                Username = userName,
                Email = email
            };

            return vm;
        }

        private RegisterViewModel BuildRegisterViewModelFromInput(RegisterInputModel inputModel = null)
        {
            var vm = new RegisterViewModel();

            if (inputModel != null)
            {
                vm.Email = inputModel.Email;
                vm.Username = inputModel.Username;
                vm.ReturnUrl = inputModel.ReturnUrl;
            }
            
            return vm;
        }

        private ForgotPasswordInputModel BuildForgotPasswordInputViewModel(string returnUrl, string email = null)
        {
            ForgotPasswordInputModel vm = new ForgotPasswordInputModel
            {
                Email = email,
                ReturnUrl = returnUrl
            };

            return vm;
        }
    }
}
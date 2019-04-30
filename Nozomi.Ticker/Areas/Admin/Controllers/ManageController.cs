using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Base.Identity.Models.Subscription;
using Nozomi.Base.Identity.ViewModels.Manage;
using Nozomi.Base.Identity.ViewModels.Manage.ApiTokens;
using Nozomi.Base.Identity.ViewModels.Manage.PaymentMethods;
using Nozomi.Base.Identity.ViewModels.Manage.Tickers;
using Nozomi.Base.Identity.ViewModels.Manage.TwoFactorAuthentication;
using Nozomi.Data.AreaModels.v1.CurrencySource;
using Nozomi.Data.AreaModels.v1.RequestComponent;
using Nozomi.Data.ViewModels.Manage;
using Nozomi.Preprocessing.Events.Interfaces;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Identity.Events.Auth.Interfaces;
using Nozomi.Service.Identity.Events.Interfaces;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Identity.Services.Interfaces;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Ticker.Controllers;

namespace Nozomi.Ticker.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[controller]/[action]")]
    public class ManageController : BaseViewController<ManageController>
    {
        private readonly IApiTokenEvent _apiTokenEvent;
        private readonly IRequestEvent _requestEvent;
        private readonly ISourceEvent _sourceEvent;
        private readonly IStripeEvent _stripeEvent;
        private readonly ISourceService _sourceService;
        private readonly IStripeService _stripeService;
        private readonly ITickerService _tickerService;
        private readonly ISmsSender _smsSender;
        private readonly UrlEncoder _urlEncoder;
        
        public ManageController(ILogger<ManageController> logger, NozomiSignInManager signInManager, 
            NozomiUserManager userManager, ISmsSender smsSender, IRequestEvent requestEvent, 
            IStripeService stripeService, IStripeEvent stripeEvent, IApiTokenEvent apiTokenEvent, 
            ISourceEvent sourceEvent, ITickerService tickerService, ISourceService sourceService, UrlEncoder urlEncoder) 
            : base(logger, signInManager, userManager)
        {
            _smsSender = smsSender;
            _apiTokenEvent = apiTokenEvent;
            _requestEvent = requestEvent;
            _stripeEvent = stripeEvent;
            _stripeService = stripeService;
            _sourceEvent = sourceEvent;
            _tickerService = tickerService;
            _sourceService = sourceService;
            _urlEncoder = urlEncoder;
        }
        
        //
        // GET: /Manage/Index
        [HttpGet]
        public async Task<IActionResult> Index(ManageIndexMessageId? message = null)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                _logger.LogWarning($"Bad session with '{User}'");
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new IndexViewModel
            {
                Sources = _sourceEvent.GetAllActive(true).ToList(),
                StatusMessage = message == ManageIndexMessageId.Error ? "An error has occured."
                    : "",
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile(EditProfileMessageId? message = null)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            
            var vm = new EditProfileViewModel
            {
                Email = user.Email,
                HasPassword = await _userManager.HasPasswordAsync(user),
                PhoneNumber = await _userManager.GetPhoneNumberAsync(user),
                TwoFactor = await _userManager.GetTwoFactorEnabledAsync(user),
                Logins = await _userManager.GetLoginsAsync(user),
                BrowserRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user),
                AuthenticatorKey = await _userManager.GetAuthenticatorKeyAsync(user),
                EmailConfirmed = user.EmailConfirmed,
                Username = user.UserName,
                StatusMessage = 
                    message == EditProfileMessageId.SetPasswordSuccess ? "Your password has been set."
                    : message == EditProfileMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                    : message == EditProfileMessageId.Error ? "An error has occurred."
                    : message == EditProfileMessageId.AddPhoneSuccess ? "Your phone number was added."
                    : message == EditProfileMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                    : ""
            };

            return View(vm);
        }
        
        #region Currency APIs
        [HttpGet]
        [Authorize(Roles="Owner, Administrator, Staff")]
        public async Task<IActionResult> CreateCurrency()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            
            return View();
        }
        #endregion
        
        #region Request APIs

        [HttpGet]
        [Authorize(Roles = "Owner, Administrator, Staff")]
        public async Task<IActionResult> Requests()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            
            return View();
        }

        [HttpGet("[controller]/[action]/{guid}")]
        [Authorize(Roles = "Owner, Administrator, Staff")]
        public async Task<IActionResult> Request([FromRoute]Guid guid)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var req = _requestEvent.GetByGuid(guid, true);
            
            return View(new RequestViewModel
            {
                Request = req.ToDTO()
            });
        }
        
        #endregion

        #region Request Component APIs

        /// <summary>
        /// Allows you to create a request component relative to the request.
        /// </summary>
        /// <param name="id">Request Id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Owner, Administrator, Staff")]
        public async Task<IActionResult> CreateRequestComponent(long id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var req = _requestEvent.GetActive(id);
            
            if (req == null)
                ModelState.AddModelError("InvalidRequestException", "Invalid Request.");

            return View(new CreateRequestComponent
            {
                RequestId = req.Id
            });
        }
        
        #endregion

        #region Source APIs
        [HttpGet]
        [Authorize(Roles = "Owner, Administrator, Staff")]
        public async Task<IActionResult> CreateSource()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Owner, Administrator, Staff")]
        public async Task<IActionResult> CreateSource(CreateSource createSource)
        {  
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // If it works
            if (!ModelState.IsValid)
            {
                
            }

            var res = _sourceService.Create(createSource);
            
            return RedirectToAction("CreateSource");
        }

        [HttpGet]
        [Authorize(Roles = "Owner, Administrator, Staff")]
        public async Task<IActionResult> Sources()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Will be using IndexViewModel for now because it does the same thing
            var vm = new IndexViewModel
            {
                Sources = _sourceEvent.GetAll(true).ToList()
            };

            return View(vm);
        }
        
        [HttpPut("{id}")]
        [Authorize(Roles = "Owner, Administrator, Staff")]
        public async Task<IActionResult> EditSource(long id, UpdateSource updateSource)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (id != updateSource.Id)
            {
                return BadRequest();
            }

            if (_sourceService.StaffSourceUpdate(updateSource)) return Ok();

            // Update failed.
            return NotFound();
        }
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Owner, Administrator, Staff")]
        public async Task<IActionResult> DeleteSource(long id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if(_sourceService.Delete(id)) return Ok();

            // Update failed.
            return NotFound();
        }
        

        #endregion

        #region Ticker APIs
        [HttpGet]
        [Authorize(Roles = "Owner, Administrator, Staff")]
        public async Task<IActionResult> CreateTicker()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var vm = new CreateTickerViewModel();

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Owner, Administrator, Staff")]
        public async Task<IActionResult> CreateTicker(CreateTickerInputModel vm)
        {   
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // If it works
            if (!ModelState.IsValid)
            {
                
            }

            var res = _tickerService.Create(vm);

            // TODO: Implementation of error messages
            vm.StatusMessage = "There was something erroneous with your submission.";
            return RedirectToAction("CreateTicker");
        }
        #endregion

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel account)
        {
            EditProfileMessageId? message = EditProfileMessageId.Error;
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.RemoveLoginAsync(user, account.LoginProvider, account.ProviderKey);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    message = EditProfileMessageId.RemoveLoginSuccess;
                }
            }
            return RedirectToAction(nameof(ManageLogins), new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public IActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var user = await GetCurrentUserAsync();
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.PhoneNumber);
            await _smsSender.SendSmsAsync(model.PhoneNumber, "Your security code is: " + code);
            return RedirectToAction(nameof(VerifyPhoneNumber), new { PhoneNumber = model.PhoneNumber });
        }

        [HttpPost]
        [Produces("text/json")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DownloadPersonalData()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            _logger.LogInformation("User with ID '{UserId}' asked for their personal data.", _userManager.GetUserId(User));

            // Only include personal data for download
            var personalData = new Dictionary<string, string>();
            var personalDataProps = typeof(User).GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
            foreach (var p in personalDataProps)
            {
                personalData.Add(p.Name, p.GetValue(user)?.ToString() ?? "null");
            }

            Response.Headers.Add("Content-Disposition", "attachment; filename=PersonalData.json");
            return new FileContentResult(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(personalData)), 
                "text/json");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePersonalData(DeletePersonalDataInputModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (await _userManager.HasPasswordAsync(user))
            {
                if (!await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    ModelState.AddModelError(string.Empty, "Password not correct.");
                    
                    return Redirect("~/");
                }
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID " +
                                                    $"'{user.Id}'.");
            }

            await _signInManager.SignOutAsync();

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", user.Id);

            return Redirect("~/");
        }

        //
        // POST: /Manage/ResetAuthenticatorKey
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetAuthenticatorKey()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                _logger.LogInformation(1, "User reset authenticator key.");
            }
            return RedirectToAction(nameof(Index), "Manage");
        }

        //
        // POST: /Manage/GenerateRecoveryCode
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateRecoveryCode()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var codes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 5);
                _logger.LogInformation(1, "User generated new recovery code.");
                return View("DisplayRecoveryCodes", new DisplayRecoveryCodesViewModel { Codes = codes });
            }
            return View("Error");
        }
        
        //
        // GET: /Manage/TwoFactorAuthentication
        [HttpGet]
        public async Task<IActionResult> TwoFactorAuthentication(TwoFactorAuthenticationMessageId? message = null,
            string[] recoveryCodes = null)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var vm = new TwoFAViewModel
            {
                HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null,
                Is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(user),
                IsMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user),
                StatusMessage = 
                    message == TwoFactorAuthenticationMessageId.Enable2FASuccess ? "2FA is now enabled on your account."
                    : message == TwoFactorAuthenticationMessageId.Enable2FAInvalidCode ? "Invalid 2FA auth code."
                    : message == TwoFactorAuthenticationMessageId.Enable2FAError ? "There was a problem enabling 2FA on your account."
                    : message == TwoFactorAuthenticationMessageId.Disable2FASuccess ? "Your 2FA configuration has been successfully disabled."    
                    : message == TwoFactorAuthenticationMessageId.Disable2FAError ? "There was a problem disabling 2FA on your account."
                    : message == TwoFactorAuthenticationMessageId.Error ? "An unknown error has occurred. Please contact our administrator."
                    : ""
            };
            
            // If 2fa is not enabled
            if (!vm.Is2faEnabled)
            {
                // Load the authenticator key & QR code URI to display on the form
                var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
                
                // If there isn't any unformatted key
                if (string.IsNullOrEmpty(unformattedKey))
                {
                    // Regenerate
                    await _userManager.ResetAuthenticatorKeyAsync(user);
                    unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
                }

                vm.SharedKey = GenerateSharedKey(unformattedKey);
                vm.AuthenticatorUri = GenerateAuthenticatorUri(user.Email, unformattedKey);
            } else if (vm.Is2faEnabled)
            {
                if (recoveryCodes != null)
                {
                    vm.RecoveryCodes = recoveryCodes;
                }
                
                vm.RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user);
            }
            
            return View(vm);
        }

        //
        // POST: /Manage/TwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TwoFactorAuthentication(TwoFAViewModel model)
        {
            var user = await GetCurrentUserAsync();
            if (user != null && ModelState.IsValid)
            {
                // Strip spaces and hypens
                var verificationCode = model.Code
                    .Replace(" ", string.Empty)
                    .Replace("-", string.Empty);

                var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                    user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

                if (!is2faTokenValid)
                {
                    return RedirectToAction(nameof(TwoFactorAuthentication), 
                        new { Message = TwoFactorAuthenticationMessageId.Enable2FAInvalidCode});
                }
                
                await _userManager.SetTwoFactorEnabledAsync(user, true);
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation(1, "User enabled two-factor authentication.");
                
                if (await _userManager.CountRecoveryCodesAsync(user) == 0)
                {
                    var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
                    return RedirectToAction(nameof(TwoFactorAuthentication), 
                        new
                        {
                            Message = TwoFactorAuthenticationMessageId.Enable2FASuccess,
                            RecoveryCodes = recoveryCodes
                        });
                }
                else
                {
                    return RedirectToAction(nameof(TwoFactorAuthentication), 
                        new { Message = TwoFactorAuthenticationMessageId.Enable2FASuccess});
                }
            }
            
            return RedirectToAction(nameof(TwoFactorAuthentication), 
                new { Message = TwoFactorAuthenticationMessageId.Enable2FAError});
        }
        
        #region 2FA Helpers
        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        
        private string GenerateSharedKey(string unformattedKey)
        {
            return FormatKey(unformattedKey);
        }

        private string GenerateAuthenticatorUri(string email, string unformattedKey)
        {
            return GenerateQrCodeUri(email, unformattedKey);
        }

        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                AuthenticatorUriFormat,
                _urlEncoder.Encode("Nozomi.Ticker"),
                _urlEncoder.Encode(email),
                unformattedKey);
        }
        #endregion

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DisableTwoFactorAuthentication()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, false);
                await _userManager.ResetAuthenticatorKeyAsync(user);
                await _signInManager.RefreshSignInAsync(user);
                _logger.LogInformation(2, "User disabled two-factor authentication.");
                return RedirectToAction(nameof(TwoFactorAuthentication), 
                    new { Message = TwoFactorAuthenticationMessageId.Disable2FASuccess});
            }
            return RedirectToAction(nameof(TwoFactorAuthentication), 
                new { Message = TwoFactorAuthenticationMessageId.Disable2FAError});
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        [HttpGet]
        public async Task<IActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(await GetCurrentUserAsync(), phoneNumber);
            // Send an SMS to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.ChangePhoneNumberAsync(user, model.PhoneNumber, model.Code);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = EditProfileMessageId.AddPhoneSuccess });
                }
            }
            // If we got this far, something failed, redisplay the form
            ModelState.AddModelError(string.Empty, "Failed to verify phone number");
            return View(model);
        }

        //
        // GET: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePhoneNumber()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.SetPhoneNumberAsync(user, null);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = EditProfileMessageId.RemovePhoneSuccess });
                }
            }
            return RedirectToAction(nameof(Index), new { Message = EditProfileMessageId.Error });
        }
        
        //
        // GET: /Manage/Profile
        [HttpGet]
        public IActionResult Profile()
        {
            return View();
        }

        //
        // GET: /Manage/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword(ChangePasswordMessageId? message = null)
        {
            return View(new ChangePasswordViewModel
            {
                StatusMessage = 
                    message == ChangePasswordMessageId.ChangePasswordSuccess ? "Your password has been changed." 
                    : message == ChangePasswordMessageId.Error ? "An error has occurred"
                    : ""
            });
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(3, "User changed their password successfully.");
                    return RedirectToAction(nameof(ChangePassword), new { Message = 
                        ChangePasswordMessageId.ChangePasswordSuccess });
                }
                AddErrors(result);
                return View(model);
            }
            return RedirectToAction(nameof(ChangePassword), new { Message = ChangePasswordMessageId.Error });
        }

//        //
//        // GET: /Manage/SetPassword
//        [HttpGet]
//        public IActionResult SetPassword()
//        {
//            return View();
//        }
//
//        //
//        // POST: /Manage/SetPassword
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(model);
//            }
//
//            var user = await GetCurrentUserAsync();
//            if (user != null)
//            {
//                var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
//                if (result.Succeeded)
//                {
//                    await _signInManager.SignInAsync(user, isPersistent: false);
//                    return RedirectToAction(nameof(Index), new { Message = EditProfileMessageId.SetPasswordSuccess });
//                }
//                AddErrors(result);
//                return View(model);
//            }
//            return RedirectToAction(nameof(Index), new { Message = EditProfileMessageId.Error });
//        }

        //GET: /Manage/ManageLogins
        [HttpGet]
        public async Task<IActionResult> ManageLogins(EditProfileMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == EditProfileMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == EditProfileMessageId.AddLoginSuccess ? "The external login was added."
                : message == EditProfileMessageId.Error ? "An error has occurred."
                : "";
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await _userManager.GetLoginsAsync(user);
            var schemes = await _signInManager.GetExternalAuthenticationSchemesAsync();
            var otherLogins = schemes.Where(auth => userLogins.All(ul => auth.Name != ul.LoginProvider)).ToList();
            ViewData["ShowRemoveButton"] = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            var redirectUrl = Url.Action("LinkLoginCallback", "Manage");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
            return Challenge(properties, provider);
        }

        //
        // GET: /Manage/LinkLoginCallback
        [HttpGet]
        public async Task<ActionResult> LinkLoginCallback()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var info = await _signInManager.GetExternalLoginInfoAsync(await _userManager.GetUserIdAsync(user));
            if (info == null)
            {
                return RedirectToAction(nameof(ManageLogins), new { Message = EditProfileMessageId.Error });
            }
            var result = await _userManager.AddLoginAsync(user, info);
            var message = result.Succeeded ? EditProfileMessageId.AddLoginSuccess : EditProfileMessageId.Error;
            return RedirectToAction(nameof(ManageLogins), new { Message = message });
        }

        [HttpGet]
        public async Task<IActionResult> Plans()
        {
            var user = await GetCurrentUserAsync();
            
            return View();
        }

        //
        // GET: /Manage/PaymentMethods
        [HttpGet]
        public async Task<IActionResult> PaymentMethods(PaymentMethodsViewModel vm) // TODO: Success obj
        {
            if (vm == null) vm = new PaymentMethodsViewModel();

            // Auth checks
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            vm.Cards = await _stripeEvent.Cards(user.StripeCustomerId);
            vm.Customer = await _stripeEvent.User(user.StripeCustomerId);
            
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> PostNewCard([FromForm]AddNewCardInputModel vm)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("PaymentMethods"); // TODO: Failure obj
            }

            var user = await GetCurrentUserAsync();
            
            if (user == null) 
            {
                return RedirectToAction("PaymentMethods", 
                new PaymentMethodsViewModel()
                {
                    CardholderName = vm.CardholderName,
                    CardToken = vm.CardToken
                });
            }
            
            await _stripeService.AddCard(user, vm.CardToken);
            
            return RedirectToAction("PaymentMethods"); // TODO: Success obj
        }

        public async Task<IActionResult> SetNewDefaultCard(string cardId)
        {
            if (string.IsNullOrEmpty(cardId))
            {
                return RedirectToAction("PaymentMethods"); // TODO: Failure obj
            }

            var user = await GetCurrentUserAsync();

            if (user == null || string.IsNullOrEmpty(user.StripeCustomerId))
            {
                return RedirectToAction("PaymentMethods"); // TODO: Failure obj
            }

            if (await _stripeService.SetDefaultCard(user.StripeCustomerId, cardId))
            {
                return RedirectToAction("PaymentMethods"); // TODO: Success obj
            }
            
            return RedirectToAction("PaymentMethods"); // TODO: Failure obj
        }

        public async Task<IActionResult> RemoveCard(string cardId)
        {
            if (string.IsNullOrEmpty(cardId))
            {
                return RedirectToAction("PaymentMethods"); // TODO: Failure obj
            }
            
            var user = await GetCurrentUserAsync();

            if (user == null || string.IsNullOrEmpty(user.StripeCustomerId))
            {
                return RedirectToAction("PaymentMethods"); // TODO: Failure obj
            }

            if (await _stripeService.RemoveCard(user.StripeCustomerId, cardId))
            {
                return RedirectToAction("PaymentMethods"); // TODO: Success obj
            }
            
            return RedirectToAction("PaymentMethods"); // TODO: Failure obj
        }

        [HttpGet("planType")]
        public async Task<IActionResult> Subscribe(PlanType planType)
        {
            var user = await GetCurrentUserAsync();

            if (user == null || string.IsNullOrEmpty(user.StripeCustomerId))
            {
                return BadRequest("Are you logged in?");
            }

            var res = await _stripeService.Subscribe(user.StripeCustomerId, planType);

            if (!string.IsNullOrEmpty(res.Id))
            {
                return Ok("Subscription successful!");
            }

            return BadRequest("Something unexpected happened with the subscription.");
        }

        [HttpGet]
        public async Task<IActionResult> Unsubscribe(PlanType planType)
        {
            var user = await GetCurrentUserAsync();
            
            if (user == null || string.IsNullOrEmpty(user.StripeCustomerId))
            {
                return BadRequest("Are you logged in?");
            }

            var res = await _stripeService.CancelSubscription(user.StripeCustomerId, planType);

            if (res) return Ok();

            return BadRequest();
        }

        public async Task<IActionResult> ApiTokens()
        {
            var user = await GetCurrentUserAsync();
            
            if (user == null)
            {
                return BadRequest("Are you logged in?");
            }
            
            var model = new ApiTokensViewModel
            {
                ApiTokens = (await _apiTokenEvent.ApiTokensByUserId(user.Id, true))
                    .Select(tok => tok.ToApiTokenResult()).ToList()
            };
            
            return View(model);
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public enum ChangePasswordMessageId
        {
            ChangePasswordSuccess,
            Error
        }

        public enum EditProfileMessageId
        {
            AddPhoneSuccess,
            AddLoginSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        public enum ManageIndexMessageId
        {
            Error
        }

        public enum TwoFactorAuthenticationMessageId
        {
            Enable2FASuccess,
            Enable2FAError,
            Enable2FAInvalidCode,
            Disable2FASuccess,
            Disable2FAError,
            Error
        }
        #endregion
    }
}
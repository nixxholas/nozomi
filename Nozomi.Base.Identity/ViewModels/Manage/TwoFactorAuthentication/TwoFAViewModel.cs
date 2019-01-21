using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Nozomi.Base.Core;

namespace Nozomi.Base.Identity.ViewModels.Manage.TwoFactorAuthentication
{
    public class TwoFAViewModel : BaseViewModel
    {

        public string SharedKey { get; set; }

        public string AuthenticatorUri { get; set; }

        [TempData]
        public string[] RecoveryCodes { get; set; }
        public bool HasAuthenticator { get; set; }

        public int RecoveryCodesLeft { get; set; }

        public bool Is2faEnabled { get; set; }

        public bool IsMachineRemembered { get; set; }
        
        [Required]
        [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Verification Code")]
        public string Code { get; set; }
    }
}
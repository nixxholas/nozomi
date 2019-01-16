using System.ComponentModel.DataAnnotations;

namespace Nozomi.Base.Identity.ViewModels.Account
{
    public class VerifyAuthenticatorCodeInputModel
    {
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
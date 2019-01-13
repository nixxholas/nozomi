using System.ComponentModel.DataAnnotations;

namespace Nozomi.Base.Identity.ViewModels.Areas.Account
{
    public class VerifyAuthenticatorCodeViewModel : VerifyAuthenticatorCodeInputModel
    {
        [Required]
        public string Code { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }
    }
}
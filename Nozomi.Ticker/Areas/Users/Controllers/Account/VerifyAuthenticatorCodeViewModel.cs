using System.ComponentModel.DataAnnotations;

namespace Nozomi.Ticker.Areas.Users.Controllers.Account
{
    public class VerifyAuthenticatorCodeViewModel : VerifyAuthenticatorCodeInputModel
    {
        [Required]
        public string Code { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }
    }
}
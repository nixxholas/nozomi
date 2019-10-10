using System.ComponentModel.DataAnnotations;

namespace Nozomi.Ticker.Areas.Users.Controllers.Account
{
    public class VerifyCodeViewModel : VerifyCodeInputModel
    {
        [Required]
        public string Code { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }
    }
}
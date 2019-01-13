using System.ComponentModel.DataAnnotations;

namespace Nozomi.Base.Identity.ViewModels.Areas.Account
{
    public class VerifyCodeInputModel
    {
        [Required]
        public string Provider { get; set; }

        public string ReturnUrl { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
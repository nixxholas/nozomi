using System.ComponentModel.DataAnnotations;

namespace Nozomi.Base.Identity.ViewModels.Areas.Account
{
    public class ForgotPasswordInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
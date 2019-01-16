using System.ComponentModel.DataAnnotations;

namespace Nozomi.Base.Identity.ViewModels.Account
{
    public class ForgotPasswordInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
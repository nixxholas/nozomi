using System.ComponentModel.DataAnnotations;

namespace Nozomi.Base.Identity.Models.Areas.Account
{
    public class ForgotPasswordInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
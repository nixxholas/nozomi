using System.ComponentModel.DataAnnotations;

namespace Nozomi.Data.AreaModels.Account
{
    public class ForgotPasswordInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
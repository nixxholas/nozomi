using System.ComponentModel.DataAnnotations;

namespace Nozomi.Base.Auth.ViewModels.Account
{
    public class ForgotPasswordInputModel
    {
        [Required] [EmailAddress] public string Email { get; set; }
    }
}
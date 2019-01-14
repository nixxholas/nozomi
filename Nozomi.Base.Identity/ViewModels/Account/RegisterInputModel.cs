using System.ComponentModel.DataAnnotations;

namespace Nozomi.Base.Identity.ViewModels.Account
{
    public class RegisterInputModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        
        public string ReturnUrl { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Nozomi.Base.Identity.ViewModels.Areas.Account
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
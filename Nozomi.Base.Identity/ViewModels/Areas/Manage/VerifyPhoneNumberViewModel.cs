using System.ComponentModel.DataAnnotations;
using Nozomi.Base.Core;

namespace Nozomi.Base.Identity.ViewModels.Areas.Manage
{
    public class VerifyPhoneNumberViewModel : BaseViewModel
    {
        [Required]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }
}
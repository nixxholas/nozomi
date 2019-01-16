using System.ComponentModel.DataAnnotations;
using Nozomi.Base.Core;

namespace Nozomi.Base.Identity.ViewModels.Manage
{
    public class AddPhoneNumberViewModel : BaseViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }
}
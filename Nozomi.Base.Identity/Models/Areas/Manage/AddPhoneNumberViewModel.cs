using System.ComponentModel.DataAnnotations;

namespace Nozomi.Base.Identity.Models.Areas.Manage
{
    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }
}
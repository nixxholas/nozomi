using System.ComponentModel.DataAnnotations;
using Nozomi.Base.BCL;

namespace Nozomi.Ticker.Areas.Users.Controllers.Manage
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
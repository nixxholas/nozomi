using System.ComponentModel.DataAnnotations;
using Nozomi.Base.BCL;

namespace Nozomi.Ticker.Areas.Users.Controllers.Manage
{
    public class AddPhoneNumberViewModel : BaseViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }
}
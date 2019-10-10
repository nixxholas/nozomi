using System.ComponentModel.DataAnnotations;
using Nozomi.Base.Core;

namespace Nozomi.Ticker.Areas.Users.Controllers.Manage
{
    public class DeletePersonalDataInputModel : BaseViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
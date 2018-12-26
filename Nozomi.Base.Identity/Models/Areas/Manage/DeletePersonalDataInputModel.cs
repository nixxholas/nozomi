using System.ComponentModel.DataAnnotations;

namespace Nozomi.Base.Identity.Models.Areas.Manage
{
    public class DeletePersonalDataInputModel : BaseViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
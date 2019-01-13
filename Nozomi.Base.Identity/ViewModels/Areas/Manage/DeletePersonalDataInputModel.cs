using System.ComponentModel.DataAnnotations;
using Nozomi.Base.Core;

namespace Nozomi.Base.Identity.ViewModels.Areas.Manage
{
    public class DeletePersonalDataInputModel : BaseViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
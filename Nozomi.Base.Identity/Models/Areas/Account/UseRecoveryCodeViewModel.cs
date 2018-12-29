using System.ComponentModel.DataAnnotations;

namespace Nozomi.Base.Identity.Models.Areas.Account
{
    public class UseRecoveryCodeViewModel : UseRecoveryCodeInputModel
    {
        [Required]
        public string Code { get; set; }
    }
}
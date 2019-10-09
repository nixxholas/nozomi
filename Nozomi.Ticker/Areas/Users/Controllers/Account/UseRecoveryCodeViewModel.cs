using System.ComponentModel.DataAnnotations;

namespace Nozomi.Ticker.Areas.Users.Controllers.Account
{
    public class UseRecoveryCodeViewModel : UseCodeRecoveryInputModel
    {
        [Required]
        public string Code { get; set; }
    }
}
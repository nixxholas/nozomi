using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nozomi.Base.BCL;

namespace Nozomi.Ticker.Areas.Users.Controllers.Manage
{
    public class DisplayRecoveryCodesViewModel : BaseViewModel
    {
        [Required]
        public IEnumerable<string> Codes { get; set; }
    }
}
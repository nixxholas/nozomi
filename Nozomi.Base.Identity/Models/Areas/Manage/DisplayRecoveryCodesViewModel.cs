using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nozomi.Base.Identity.Models.Areas.Manage
{
    public class DisplayRecoveryCodesViewModel
    {
        [Required]
        public IEnumerable<string> Codes { get; set; }
    }
}
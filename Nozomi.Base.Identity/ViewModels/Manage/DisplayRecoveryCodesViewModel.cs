using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nozomi.Base.Core;

namespace Nozomi.Base.Identity.ViewModels.Manage
{
    public class DisplayRecoveryCodesViewModel : BaseViewModel
    {
        [Required]
        public IEnumerable<string> Codes { get; set; }
    }
}
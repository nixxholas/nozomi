using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Nozomi.Base.Core;
using Nozomi.Data.CurrencyModels;

namespace Nozomi.Base.Identity.ViewModels.Manage
{
    public class IndexViewModel : BaseViewModel
    {
        public bool HasPassword { get; set; }

        public IList<UserLoginInfo> Logins { get; set; }

        public string PhoneNumber { get; set; }

        public bool TwoFactor { get; set; }

        public bool BrowserRemembered { get; set; }

        public string AuthenticatorKey { get; set; }
        
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; } = false;

        public ICollection<Source> Sources { get; set; } = new List<Source>();
    }
}
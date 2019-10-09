using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Nozomi.Base.Core;

namespace Nozomi.Ticker.Areas.Users.Controllers.Manage
{
    public class EditProfileViewModel : BaseViewModel
    {
        public bool BrowserRemembered { get; set; }

        public string AuthenticatorKey { get; set; }
        
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; } = false;
        
        public bool HasPassword { get; set; }
        
        public string PhoneNumber { get; set; }

        public bool TwoFactor { get; set; }
        
        public string Username { get; set; }

        public IList<UserLoginInfo> Logins { get; set; }
    }
}
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Nozomi.Base.Core;

namespace Nozomi.Base.Identity.ViewModels.Areas.Manage
{
    public class ManageLoginsViewModel : BaseViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }

        public IList<AuthenticationScheme> OtherLogins { get; set; }
    }
}
using Nozomi.Base.Core;

namespace Nozomi.Base.Identity.ViewModels.Manage.TwoFactorAuthentication
{
    public class TwoFAViewModel : BaseViewModel
    {
        public bool HasAuthenticator { get; set; }

        public int RecoveryCodesLeft { get; set; }

        public bool Is2faEnabled { get; set; }

        public bool IsMachineRemembered { get; set; }
    }
}
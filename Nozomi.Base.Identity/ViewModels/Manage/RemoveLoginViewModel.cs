using Nozomi.Base.Core;

namespace Nozomi.Base.Identity.ViewModels.Manage
{
    public class RemoveLoginViewModel : BaseViewModel
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
    }
}
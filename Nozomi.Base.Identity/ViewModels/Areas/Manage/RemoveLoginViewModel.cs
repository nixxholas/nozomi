using Nozomi.Base.Core;

namespace Nozomi.Base.Identity.ViewModels.Areas.Manage
{
    public class RemoveLoginViewModel : BaseViewModel
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
    }
}
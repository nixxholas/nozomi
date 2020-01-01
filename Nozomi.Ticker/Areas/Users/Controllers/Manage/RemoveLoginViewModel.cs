using Nozomi.Base.BCL;

namespace Nozomi.Ticker.Areas.Users.Controllers.Manage
{
    public class RemoveLoginViewModel : BaseViewModel
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
    }
}
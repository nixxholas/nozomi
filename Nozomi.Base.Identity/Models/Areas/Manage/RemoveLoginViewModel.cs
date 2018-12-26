namespace Nozomi.Base.Identity.Models.Areas.Manage
{
    public class RemoveLoginViewModel : BaseViewModel
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
    }
}
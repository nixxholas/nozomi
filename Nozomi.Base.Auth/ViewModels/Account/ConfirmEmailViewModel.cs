namespace Nozomi.Base.Auth.ViewModels.Account
{
    public class ConfirmEmailViewModel
    {
        public bool Succeeded { get; set; }
        
        public string PostLogoutRedirectUri { get; set; }
    }
}
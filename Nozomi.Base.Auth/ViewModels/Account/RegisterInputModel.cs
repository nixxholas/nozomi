namespace Nozomi.Base.Auth.ViewModels.Account
{
    public class RegisterInputModel
    {
        public string Username { get; set; }
        
        public string Email { get; set; }
        
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
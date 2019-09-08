namespace Nozomi.Auth.Controllers.Account
{
    public class RegisterInputModel
    {
        public string Username { get; set; }
        
        public string Email { get; set; }
        
        public string Password { get; set; }
        
        /// <summary>
        /// The user-claimed address
        /// </summary>
        public string Address { get; set; }
        
        /// <summary>
        /// The raw hash of the user-signed message
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// The raw message
        /// </summary>
        public string Message { get; set; }

        public string ReturnUrl { get; set; }
    }
}
namespace Nozomi.Auth.Controllers.Account
{
    public class Web3LoginInputModel
    {
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
    }
}
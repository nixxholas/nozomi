using System.ComponentModel.DataAnnotations;

namespace Nozomi.Auth.Controllers.Account
{
    public class Web3LoginInputModel
    {
        /// <summary>
        /// The user-claimed address
        /// </summary>
        [Required]
        public string Address { get; set; }
        
        /// <summary>
        /// The raw hash of the user-signed message
        /// </summary>
        [Required]
        public string Signature { get; set; }

        /// <summary>
        /// The raw message
        /// </summary>
        [Required]
        public string Message { get; set; }
    }
}
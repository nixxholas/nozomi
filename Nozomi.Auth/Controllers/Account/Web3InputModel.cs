using System.ComponentModel.DataAnnotations;

namespace Nozomi.Auth.Controllers.Account
{
    public class Web3InputModel
    {
        /// <summary>
        /// The address that the user wants to authenticate with
        /// </summary>
        [Required]
        public string Address { get; set; }
        
        /// <summary>
        /// The signed message the user is submitting, we'll be making use of this to obtain the address.
        /// </summary>
        [Required]
        public string Signature { get; set; }
        
        /// <summary>
        /// The raw message of the signature.
        /// </summary>
        [Required]
        public string Message { get; set; }
    }
}
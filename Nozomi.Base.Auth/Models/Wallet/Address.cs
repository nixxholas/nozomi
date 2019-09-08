using System;
using Microsoft.AspNetCore.Identity;

namespace Nozomi.Base.Auth.Models.Wallet
{
    public class Address
    {
        public string Hash { get; set; }
        
        public AddressType Type { get; set; }
        
        public string UserId { get; set; }
        
        public User User { get; set; }
    }
}
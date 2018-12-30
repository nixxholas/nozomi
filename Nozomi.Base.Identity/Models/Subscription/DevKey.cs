using System;
using System.Security.Cryptography;
using Nozomi.Base.Core;
using Nozomi.Base.Identity.Models.Identity;

namespace Nozomi.Base.Identity.Models.Subscription
{
    public class DevKey : BaseEntityModel
    {
        public DevKey(long userId)
        {
            if (userId <= 0) throw new ArgumentException("Invalid UserId.");
            
            UserId = userId;
            
            var key = new byte[32];
            using (var generator = RandomNumberGenerator.Create())
                generator.GetBytes(key);
            
            Key = Convert.ToBase64String(key);
        }
        
        public long Id { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public string Key { get; set; }
    }
}
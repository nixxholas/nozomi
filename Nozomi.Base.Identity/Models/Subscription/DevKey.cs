using System;
using System.Security.Cryptography;
using Nozomi.Base.Core;

namespace Nozomi.Base.Identity.Models.Subscription
{
    public class DevKey : BaseEntityModel
    {
        public DevKey() {}
        
        public DevKey(long userSubscriptionId)
        {
            if (userSubscriptionId <= 0) throw new ArgumentException("Invalid UserSubscriptionId.");
            
            UserSubscriptionId = userSubscriptionId;
            Key = GenerateAPIKey(userSubscriptionId.ToString());
        }
        
        public long Id { get; set; }
        
        public long UserSubscriptionId { get; set; }
        
        public UserSubscription UserSubscription { get; set; }
        
        public string Key { get; set; }

        public string GenerateAPIKey(string userId)
        {
            var key = new byte[32];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(key);
            }

            return Convert.ToBase64String(key);
        }
    }
}
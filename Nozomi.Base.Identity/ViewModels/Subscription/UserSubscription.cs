using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Nozomi.Base.Core;
using Nozomi.Base.Identity.ViewModels.Identity;

namespace Nozomi.Base.Identity.ViewModels.Subscription
{
    public class UserSubscription : BaseEntityModel
    {
        public long Id { get; set; }
        
        // From stripe.
        public string SubscriptionId { get; set; }
        
        public PlanType PlanType { get; set; }
        
        [NotMapped]
        public Stripe.Subscription Subscription { get; set; }
        
        public virtual ICollection<DevKey> DevKeys { get; set; }
        
        public long UserId { get; set; }
        
        public User User { get; set; }
    }
}
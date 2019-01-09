using System.Collections.Generic;
using System.Threading.Tasks;
using Nozomi.Base.Identity.Models.Subscription;
using Stripe;
using Plan = Stripe.Plan;

namespace Nozomi.Service.Identity.Events.Interfaces
{
    public interface IStripeEvent
    {
        Task<bool> CancelSubscription(string stripeCustomerId);
        
        Task<Card> Card(string stripeCustomerId, string stripeCardId);
        
        Task<ICollection<Card>> Cards(string stripeCustId);
        
        Task<Subscription> Subscribe(string stripeCustId, PlanType planType);

        Task<ICollection<Plan>> Plans(PlanListOptions options);

        Task<ICollection<Product>> Products(ProductListOptions options);

        Task<ICollection<Subscription>> Subscriptions(SubscriptionListOptions options);

        Task<Customer> User(string stripeCustomerId);
    }
}
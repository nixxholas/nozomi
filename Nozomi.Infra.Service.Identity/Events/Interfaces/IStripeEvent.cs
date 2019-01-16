using System.Collections.Generic;
using System.Threading.Tasks;
using Stripe;
using Plan = Stripe.Plan;

namespace Nozomi.Service.Identity.Events.Interfaces
{
    public interface IStripeEvent
    {
        Task<Card> Card(string stripeCustomerId, string stripeCardId);
        
        Task<ICollection<Card>> Cards(string stripeCustId);

        Task<ICollection<Plan>> Plans(PlanListOptions options);

        Task<ICollection<Product>> Products(ProductListOptions options);

        Task<ICollection<Subscription>> Subscriptions(SubscriptionListOptions options);

        Task<Customer> User(string stripeCustomerId);
    }
}
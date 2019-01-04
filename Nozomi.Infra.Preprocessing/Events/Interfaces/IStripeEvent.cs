using System.Collections.Generic;
using System.Threading.Tasks;
using Stripe;

namespace Nozomi.Preprocessing.Events.Interfaces
{
    public interface IStripeEvent
    {
        bool Subscribe();

        Task<ICollection<Plan>> Plans(PlanListOptions options);

        Task<ICollection<Product>> Products(ProductListOptions options);

        Task<ICollection<Subscription>> Subscriptions(SubscriptionListOptions options);
    }
}
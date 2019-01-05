using System.Collections.Generic;
using System.Threading.Tasks;
using Nozomi.Base.Identity.Models.Subscription;
using Stripe;

namespace Nozomi.Service.Identity.Events.Interfaces
{
    public interface IStripeEvent
    {
        Task<Subscription> Subscribe(string stripeCustId, PlanType planType);

        Task<ICollection<Plan>> Plans(PlanListOptions options);

        Task<ICollection<Product>> Products(ProductListOptions options);

        Task<ICollection<Subscription>> Subscriptions(SubscriptionListOptions options);
    }
}
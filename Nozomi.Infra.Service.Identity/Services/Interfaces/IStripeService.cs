using System.Threading.Tasks;
using Stripe;

namespace Nozomi.Service.Identity.Services.Interfaces
{
    public interface IStripeService
    {
        Task<bool> CreateProduct(ProductCreateOptions options);

        Task<bool> CreateSubscription(SubscriptionCreateOptions options);
    }
}
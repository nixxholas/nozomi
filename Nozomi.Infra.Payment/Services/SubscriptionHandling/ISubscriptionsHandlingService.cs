using System.Threading.Tasks;
using Stripe;

namespace Nozomi.Infra.Payment.Services.SubscriptionHandling
{
    public interface ISubscriptionsHandlingService
    {
        Task SubscriptionCancelled(Subscription subscription);
    }
}
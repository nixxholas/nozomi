using System.Threading.Tasks;
using Nozomi.Base.Auth.Models;
using Stripe;

namespace Nozomi.Infra.Payment.Services.SubscriptionHandling
{
    public interface ISubscriptionsHandlingService
    {
        Task Subscribe(string planId, User user);
        Task ChangePlan(string planId, User user);
        Task SubscriptionCancelled(Subscription subscription);
    }
}
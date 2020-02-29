using System.Collections.Generic;
using System.Threading.Tasks;
using Stripe;

namespace Nozomi.Infra.Auth.Events.UserEvent
{
    public interface IUserEvent
    {
        bool HasStripe(string userId);

        bool HasDefaultPaymentMethod(string userId);

        string GetStripeCustomerId(string userId);

        string GetUserActiveSubscriptionId(string userId);
    }
}
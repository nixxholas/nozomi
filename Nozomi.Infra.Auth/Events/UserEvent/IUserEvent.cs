using System.Collections.Generic;
using System.Threading.Tasks;
using Nozomi.Base.Auth.Models;
using Stripe;

namespace Nozomi.Infra.Auth.Events.UserEvent
{
    public interface IUserEvent
    {
        bool HasStripe(string userId);

        bool HasDefaultPaymentMethod(string userId);

        bool HasPaymentMethod(string userId, string paymentMethodId);

        string GetStripeCustomerId(string userId);

        string GetUserActiveSubscriptionId(string userId);

        IEnumerable<string> GetUserPaymentMethods(string userId);
        
        Task<User> GetUserByCustomerId(string id);
    }
}
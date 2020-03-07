using System.Collections.Generic;
using System.Threading.Tasks;
using Stripe;

namespace Nozomi.Infra.Auth.Events.Stripe
{
    public interface IStripeEvent
    {

        bool PaymentMethodExists(string stripeUserId, string paymentMethodId);

        Task<Base.Auth.Models.User> GetUserByCustomerId(string id);
    }
}
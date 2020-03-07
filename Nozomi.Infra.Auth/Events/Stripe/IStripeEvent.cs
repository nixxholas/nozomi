using System.Collections.Generic;
using System.Threading.Tasks;
using Stripe;

namespace Nozomi.Infra.Auth.Events.Stripe
{
    public interface IStripeEvent
    {

        Task<IEnumerable<PaymentMethod>> ListPaymentMethods(string stripeUserId, string paymentMethodType = "card");

        bool PaymentMethodExists(string stripeUserId, string paymentMethodId);

        Task<Base.Auth.Models.User> GetUserByCustomerId(string id);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using Stripe;

namespace Nozomi.Infra.Auth.Events.Stripe
{
    public interface IStripeEvent
    {
        Task<string> GetUserCurrentPlanIdAsync(string userId);
        
        bool IsDefaultPlan(string planId);
        
        /// <summary>
        /// Obtain all plans stashed for our product
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Plan>> Plans(bool activeOnly = true);

        Plan Plan(string planId);

        bool PlanExists(string planId);

        Task<IEnumerable<Card>> Cards(Base.Auth.Models.User user);

        bool CardExists(string stripeUserId, string cardId);
        
        Task<IEnumerable<PaymentMethod>> ListPaymentMethods(string stripeUserId, string paymentMethodType = "card");

        bool PaymentMethodExists(string stripeUserId, string paymentMethodId);

        Task<Base.Auth.Models.User> GetUserByCustomerId(string id);
    }
}
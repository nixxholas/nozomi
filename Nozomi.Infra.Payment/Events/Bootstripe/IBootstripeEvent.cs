using System.Collections.Generic;
using System.Threading.Tasks;
using Stripe;

namespace Nozomi.Infra.Payment.Events.Bootstripe
{
    public interface IBootstripeEvent
    {
        bool IsDefaultPlan(string planId);
        
        Task<IEnumerable<Plan>> GetPlans(bool activeOnly = true);

        Plan GetPlan(string planId);

        bool PlanExists(string planId);

        Task<IEnumerable<Card>> Cards(Base.Auth.Models.User user);

        bool CardExists(string stripeUserId, string cardId);
        
        Task<IEnumerable<PaymentMethod>> ListPaymentMethods(string stripeUserId, string paymentMethodType = "card");

        bool PaymentMethodExists(string stripeUserId, string paymentMethodId);
    }
}
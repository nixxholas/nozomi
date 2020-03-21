using System.Collections.Generic;
using System.Threading.Tasks;
using Nozomi.Base.Auth.Models;
using Stripe;

namespace Nozomi.Infra.Payment.Events.Bootstripe
{
    public interface IBootstripeEvent
    {
        bool IsDefaultPlan(string planId);

        Task<string> GetUserCurrentPlanIdAsync(string userId);
        
        Task<string> GetPlanMetadataValue(string planId, string metadataKey);
        
        Task<IEnumerable<Plan>> GetPlans(bool activeOnly = true);

        Plan GetPlan(string planId);

        bool PlanExists(string planId);

        Task<IEnumerable<PaymentMethod>> ListPaymentMethods(User user, string paymentMethodType = "card");

        Task<bool> PaymentMethodBelongsToUser(User user, string paymentMethodId);

        Task<bool> PaymentMethodExists(string paymentMethodId);
    }
}
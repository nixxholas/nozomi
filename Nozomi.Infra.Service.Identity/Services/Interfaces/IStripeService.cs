using System.Threading.Tasks;
using Stripe;

namespace Nozomi.Service.Identity.Services.Interfaces
{
    public interface IStripeService
    {
        /// <summary>
        /// Allows us to create a plan for the user to pick to unlock more features.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<bool> CreatePlan(PlanCreateOptions options);
        
        Task<bool> CreateProduct(ProductCreateOptions options);

        /// <summary>
        /// Allows us to subscribe a user to a plan so that he/she can get charged.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<bool> CreateSubscription(SubscriptionCreateOptions options);
    }
}
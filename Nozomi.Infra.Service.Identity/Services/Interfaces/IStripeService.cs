using System.Threading.Tasks;
using Nozomi.Base.Identity.Models.Identity;
using Stripe;

namespace Nozomi.Service.Identity.Services.Interfaces
{
    public interface IStripeService
    {
        Task<bool> AddCard(User user, string cardToken);

        Task<bool> SetDefaultCard(string stripeCustomerId, string cardToken);
        
        Task<string> CreateStripeCustomer(User user);

        Task<bool> CreateSource(User user);
        
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
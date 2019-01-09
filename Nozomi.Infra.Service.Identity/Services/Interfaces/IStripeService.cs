using System.Threading.Tasks;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Base.Identity.Models.Subscription;
using Stripe;

namespace Nozomi.Service.Identity.Services.Interfaces
{
    public interface IStripeService
    {
        //=============== STRIPE PRE-CONFIGURATION ==============//

        /// <summary>
        /// Propagates the default plans that are set for initial adoption. 
        /// </summary>
        void ConfigureStripePlans();
        
        //=========== END OF STRIPE PRE-CONFIGURATION ===========//

        Task<bool> AddCard(User user, string cardToken);

        Task<bool> RemoveCard(string stripeCustomerId, string cardId);

        Task<bool> SetDefaultCard(string stripeCustomerId, string cardId);
        
        Task<string> CreateStripeCustomer(User user);

        Task<bool> CreateSource(User user);
        
        Task<Subscription> Subscribe(string stripeCustId, PlanType planType);
        
        Task<bool> CancelSubscription(string stripeCustomerId);

        Task<bool> UpdateCustomerSubscription(string stripeCustomerId, string planId);
        
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
        Task<bool> UpdateCustomerSubscription(SubscriptionCreateOptions options);
    }
}
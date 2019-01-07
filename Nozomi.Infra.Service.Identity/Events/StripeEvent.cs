using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Nozomi.Base.Core.Configurations;
using Nozomi.Base.Core.Helpers.Enumerator;
using Nozomi.Base.Identity.Models.Subscription;
using Nozomi.Service.Identity.Events.Interfaces;
using Stripe;
using Plan = Stripe.Plan;

namespace Nozomi.Service.Identity.Events
{
    public class StripeEvent : IStripeEvent
    {
        private readonly IOptions<StripeSettings> _options;

        public StripeEvent(IOptions<StripeSettings> options)
        {
            _options = options;
            
            StripeConfiguration.SetApiKey(options.Value.SecretKey);
        }

        public async Task<ICollection<Card>> Cards(string stripeCustId)
        {
            var cardService = new CardService();
            return (await cardService.ListAsync(stripeCustId))?.Data;
        }

        public async Task<Subscription> Subscribe(string stripeCustId, PlanType planType)
        {
            var subService = new SubscriptionService();

            var items = new List<SubscriptionItemOption> {
                new SubscriptionItemOption {
                    PlanId = planType.GetDescription()
                }
            };
            var options = new SubscriptionCreateOptions {
                CustomerId = stripeCustId,
                Items = items
            };

            return await subService.CreateAsync(options);
        }

        public async Task<ICollection<Plan>> Plans(PlanListOptions options)
        {
            var planService = new PlanService();
            var plans = await planService.ListAsync(options);

            return plans.Data;
        }

        public async Task<ICollection<Product>> Products(ProductListOptions options)
        {
            var productService = new ProductService();
            var products = await productService.ListAsync(options);

            return products.Data;
        }

        public async Task<ICollection<Subscription>> Subscriptions(SubscriptionListOptions options)
        {
            var subService = new SubscriptionService();
            var subs = await subService.ListAsync(options);

            return subs.Data;
        }

        public async Task<Customer> User(string stripeCustomerId)
        {
            var customerService = new CustomerService();
            return await customerService.GetAsync(stripeCustomerId);
        }
    }
}
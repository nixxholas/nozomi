using System;
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

        public async Task<bool> CancelSubscription(string stripeCustomerId)
        {
            var customerService = new CustomerService();
            var customer = await customerService.GetAsync(stripeCustomerId);

            if (customer == null) return false;
            
            var subService = new SubscriptionService();
            var subListOptions = new SubscriptionListOptions
            {
                CustomerId = customer.Id,
                Status = SubscriptionStatuses.Active
            };
            var subs = await subService.ListAsync(subListOptions);

            if (subs?.Data.Count > 0)
            {
                foreach (var sub in subs)
                {
                    var subCancelOptions = new SubscriptionCancelOptions
                    {
                        InvoiceNow = true,
                        Prorate = true
                    };

                    var cancelRes = await subService.CancelAsync(sub.Id, subCancelOptions);

                    if (cancelRes?.CanceledAt == null) return false;
                }

                var basicSubRes = await Subscribe(stripeCustomerId, PlanType.Basic);

                return basicSubRes != null && basicSubRes.CanceledAt == null;
            }

            return false;
        }

        public async Task<Card> Card(string stripeCustomerId, string stripeCardId)
        {
            var cardService = new CardService();
            return await cardService.GetAsync(stripeCustomerId, stripeCardId);
        }

        public async Task<ICollection<Card>> Cards(string stripeCustId)
        {
            var cardService = new CardService();
            return (await cardService.ListAsync(stripeCustId))?.Data;
        }

        public async Task<Subscription> Subscribe(string stripeCustId, PlanType planType)
        {
            var subService = new SubscriptionService();
            
            var subListOptions = new SubscriptionListOptions
            {
                CustomerId = stripeCustId,
                Status = SubscriptionStatuses.Active
            };
            var currSub = await subService.ListAsync(subListOptions);

            if (currSub != null && currSub.Data.Count > 0)
            {
                // An active subscription exists
                foreach (var sub in currSub.Data)
                {
                    // If the subscription is active
                    if (sub.Start < DateTime.Now && sub.EndedAt == null && sub.CanceledAt == null)
                    {
                        // End it and start the new one.
                        var updateSubOptions = new SubscriptionUpdateOptions
                        {
                            CancelAtPeriodEnd = true
                        };

                        var res = await subService.UpdateAsync(sub.Id, updateSubOptions);

                        if (res == null || !res.CancelAtPeriodEnd) return null;
                    }
                }
                
                return null;
            }

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
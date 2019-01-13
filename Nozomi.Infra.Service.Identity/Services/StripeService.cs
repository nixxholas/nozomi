using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nozomi.Base.Core.Configurations;
using Nozomi.Base.Core.Helpers.Enumerator;
using Nozomi.Base.Identity.ViewModels.Identity;
using Nozomi.Base.Identity.ViewModels.Subscription;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Identity.Data;
using Nozomi.Service.Identity.Services.Interfaces;
using Stripe;
using CardUpdateOptions = Stripe.Issuing.CardUpdateOptions;

namespace Nozomi.Service.Identity.Services
{
    /// <summary>
    /// The command segment of the entire Stripe payment service for Nozomi.
    /// </summary>
    public class StripeService : BaseService<StripeService, NozomiAuthContext>, IStripeService
    {
        private static readonly string ServiceName = "[StripeService]";
        private readonly IOptions<StripeSettings> _options;
        
        public StripeService(ILogger<StripeService> logger, IUnitOfWork<NozomiAuthContext> unitOfWork,
            IOptions<StripeSettings> options) : 
            base(logger, unitOfWork)
        {
            _options = options;
            
            StripeConfiguration.SetApiKey(_options.Value.SecretKey);
        }

        public void ConfigureStripePlans()
        {
            var planService = new PlanService();
            var plans = planService.List();

            // If nothing is there
            if (plans.Data.Count.Equals(0))
            {
                // Let's populate it
                
                // DEFAULT
                var defaultPlanOptions = new PlanCreateOptions
                {
                    Id = PlanType.Basic.GetDescription(),
                    Active = true,
                    Amount = 0, // Free.
                    Currency = "usd",
                    Interval = PlanIntervals.Month,
                    Product = new PlanProductCreateOptions
                    {
                        Active = true,
                        Id = PlanType.Basic.GetDescription(),
                        Name = PlanType.Basic.GetDescription()
                    }
                };

                var defaultPlan = planService.Create(defaultPlanOptions);
                
                var plusPlanOptions = new PlanCreateOptions
                {
                    Id = PlanType.Plus.GetDescription(),
                    Active = true,
                    Amount = 2000,
                    Currency = "usd",
                    Interval = PlanIntervals.Month,
                    Product = new PlanProductCreateOptions
                    {
                        Active = true,
                        Id = PlanType.Plus.GetDescription(),
                        Name = PlanType.Plus.GetDescription()
                    }
                };

                var plusPlan = planService.Create(plusPlanOptions);
            }
            else
            {
                _logger.LogWarning("There is a plan that is populated on the Stripe account.");
            }
        }

        public async Task<bool> AddCard(User user, string cardToken)
        {
            if (string.IsNullOrEmpty(user.StripeCustomerId)) return false;
            
            // Stripe-sided checks
            var customerService = new CustomerService();
            var customer = await customerService.GetAsync(user.StripeCustomerId);
            if (customer == null) return false;
            
            // Create the card binding
            var cardOptions = new CardCreateOptions {
                SourceToken = cardToken
            };

            var cardService = new CardService();
            var card = cardService.Create(customer.Id, cardOptions);

            // If it's the first card,
            if (string.IsNullOrEmpty(customer.DefaultSourceId))
            {
                // Set it as the default source alongside its subscription as well.
                var updatedCustomer = await customerService.UpdateAsync(customer.Id, new CustomerUpdateOptions()
                {
                    DefaultSource = card.Id
                });
                
                var subService = new SubscriptionService();
                var subscription = await subService.GetAsync(customer.Subscriptions
                    .FirstOrDefault(sub => sub.EndedAt == null && 
                                           sub.CanceledAt == null)?.Id);

                if (subscription != null)
                {
                    var subUpdateOptions = new SubscriptionUpdateOptions
                    {
                        DefaultSource = card.Id
                    };
                    var res = await subService.UpdateAsync(subscription.Id, subUpdateOptions);

                    return string.IsNullOrEmpty(res.DefaultSourceId);
                }
                
                return string.IsNullOrEmpty(updatedCustomer.DefaultSourceId);
            }

            return string.IsNullOrEmpty(card.Id);
        }

        public async Task<bool> RemoveCard(string stripeCustomerId, string cardId)
        {
            if (string.IsNullOrEmpty(stripeCustomerId) || string.IsNullOrEmpty(cardId)) return false;

            var customerService = new CustomerService();
            var customer = await customerService.GetAsync(stripeCustomerId);

            // Don't remove default cards.
            if (customer == null || customer.DefaultSourceId.Equals(cardId)) return false;
            
            // Remove it since it's redundant
            var cardService = new CardService();
            var card = await cardService.GetAsync(stripeCustomerId, cardId);

            // Can't delete a deleted card
            if (card == null || card.Deleted) return false;

            var res = await cardService.DeleteAsync(stripeCustomerId, cardId);

            return res.Deleted;
        }

        public async Task<bool> SetDefaultCard(string stripeCustomerId, string cardId)
        {
            if (string.IsNullOrEmpty(stripeCustomerId) || string.IsNullOrEmpty(cardId)) return false;
            
            var customerService = new CustomerService();
            var customer = await customerService.GetAsync(stripeCustomerId);
            if (customer == null) return false;

            var customerOptions = new CustomerUpdateOptions
            {
                DefaultSource = cardId
            };
            var result = await customerService.UpdateAsync(stripeCustomerId, customerOptions);
            if (result == null) return false;
            
            var subService = new SubscriptionService();
            var subscription = await subService.GetAsync(customer.Subscriptions
                .FirstOrDefault(sub => sub.EndedAt == null && 
                                       sub.CanceledAt == null)?.Id);

            if (subscription != null)
            {
                var subUpdateOptions = new SubscriptionUpdateOptions
                {
                    DefaultSource = cardId
                };
                var res = await subService.UpdateAsync(subscription.Id, subUpdateOptions);

                return string.IsNullOrEmpty(res.DefaultSourceId);
            }

            return result.DefaultSourceId.Equals(cardId);
        }

        public async Task<string> CreateStripeCustomer(User user)
        {
            var customerOptions = new CustomerCreateOptions {
                Description = "Customer for " + user.Email,
                Email = user.Email,
                PlanId = PlanType.Basic.GetDescription()
            };

            var customerService = new CustomerService();
            return (await customerService.CreateAsync(customerOptions)).Id;
        }

        public async Task<bool> CreateSource(User user)
        {
            // TODO!
            var sourceOptions = new SourceCreateOptions
            {
                Type = SourceType.Card,
                Currency = "usd",
                Usage = "reusable",
                Owner = new SourceOwnerOptions
                {
                    Email = user.Email
                }
            };

            var sourceService = new SourceService();
            var source = await sourceService.CreateAsync(sourceOptions);

            if (!string.IsNullOrEmpty(source.Id))
            {
                user.StripeSourceId = source.Id;
                return true;
            }
            else
            {
                // Failed, send it back immediately.
                return false;
            }
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
                    if (sub.Start < DateTime.Now && sub.EndedAt == null && sub.CanceledAt == null
                        // Make sure its not the basic plan.
                        && !sub.Plan.Id.Equals(PlanType.Basic.GetDescription()))
                    {
                        // End it and start the new one.
                        var updateSubOptions = new SubscriptionUpdateOptions
                        {
                            CancelAtPeriodEnd = true
                        };

                        var updateRes = await subService.UpdateAsync(sub.Id, updateSubOptions);

                        if (updateRes == null || !updateRes.CancelAtPeriodEnd) return null;
                    }
                }
            }

            var items = new List<SubscriptionItemOption> {
                new SubscriptionItemOption {
                    PlanId = planType.GetDescription()
                }
            };
            var options = new SubscriptionCreateOptions {
                CustomerId = stripeCustId,
                Items = items,
                Prorate = true
            };

            return await subService.CreateAsync(options);
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
                    // Make sure basic is never cancelled.
                    if (!sub.Plan.Id.Equals(PlanType.Basic.GetDescription())
                        && sub.CanceledAt != null)
                    {
                        var subCancelOptions = new SubscriptionCancelOptions
                        {
                            InvoiceNow = true,
                            Prorate = true
                        };

                        var cancelRes = await subService.CancelAsync(sub.Id, subCancelOptions);

                        if (cancelRes?.CanceledAt == null) return false;
                    }
                }

                return true;
            }

            return false;
        }

        public async Task<bool> CancelSubscription(string stripeCustomerId, PlanType planType)
        {
            var customerService = new CustomerService();
            var customer = await customerService.GetAsync(stripeCustomerId);

            if (customer == null) return false;

            var foundSub = customer.Subscriptions
                .SingleOrDefault(sub => sub.Plan.Id.Equals(planType.GetDescription()));

            if (foundSub != null && !foundSub.Plan.Id.Equals(PlanType.Basic.GetDescription()))
            {
                var subService = new SubscriptionService();
                var options = new SubscriptionCancelOptions
                {
                    InvoiceNow = true,
                    Prorate = true
                };
                var cancelRes = await subService.CancelAsync(foundSub.Id, options);

                return cancelRes?.CanceledAt != null;
            }

            return false;
        }

        public async Task<bool> UpdateCustomerSubscription(string stripeCustomerId, string planId)
        {
            var subscriptionService = new SubscriptionService();
            
            var items = new List<SubscriptionItemOption> {
                new SubscriptionItemOption {
                    PlanId = planId
                }
            };
            var subOptions = new SubscriptionCreateOptions
            {
                CustomerId = stripeCustomerId,
                Items = items
            };

            return string.IsNullOrEmpty((await subscriptionService.CreateAsync(subOptions))?.Id);
        }

        public async Task<bool> CreatePlan(PlanCreateOptions options)
        {
            var planService = new PlanService();
            var res = await planService.CreateAsync(options);

            if (res != null)
            {
                _logger.LogInformation(ServiceName + ": Stripe plan creation successful.");
                return true;
            }

            _logger.LogWarning(ServiceName + ": Stripe plan creation failed.");
            return false;
        }

        public async Task<bool> CreateProduct(ProductCreateOptions options)
        {
            var productService = new ProductService();
            var res = await productService.CreateAsync(options);

            if (res != null)
            {
                _logger.LogInformation(ServiceName + ": Stripe product creation successful.");
                return true;
            }

            _logger.LogWarning(ServiceName + ": Stripe product creation failed.");
            return false;
        }

        public async Task<bool> UpdateCustomerSubscription(SubscriptionCreateOptions options)
        {
            var subService = new SubscriptionService();
            var res = await subService.CreateAsync(options);

            if (res != null)
            {
                _logger.LogInformation(ServiceName + ": Stripe subscription creation successful.");
                return true;
            }
            
            _logger.LogWarning(ServiceName + ": Stripe subscription creation failed.");
            return false;
        }
    }
}
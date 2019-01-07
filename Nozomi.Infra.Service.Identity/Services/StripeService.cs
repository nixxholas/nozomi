using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nozomi.Base.Core.Configurations;
using Nozomi.Base.Core.Helpers.Enumerator;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Base.Identity.Models.Subscription;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Identity.Data;
using Nozomi.Service.Identity.Services.Interfaces;
using Stripe;
using Plan = Nozomi.Base.Identity.Models.Subscription.Plan;

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
            if (plans.TotalCount.Equals(0))
            {
                // Let's populate it
                
                // DEFAULT
                var defaultPlanOptions = new PlanCreateOptions
                {
                    Id = Plan.Basic.GetDescription(),
                    Active = true,
                    Amount = 0, // Free.
                    Currency = "usd",
                    Interval = PlanIntervals.Month,
                    Product = new PlanProductCreateOptions
                    {
                        Active = true,
                        Id = Plan.Basic.GetDescription(),
                        Name = Plan.Basic.GetDescription()
                    }
                };

                planService.Create(defaultPlanOptions);
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

            if (string.IsNullOrEmpty(customer.DefaultSourceId))
            {
                await customerService.UpdateAsync(customer.Id, new CustomerUpdateOptions()
                {
                    DefaultSource = card.Id
                });
            }

            return string.IsNullOrEmpty(card.Id);
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

            return result.DefaultSourceId.Equals(cardId);
        }

        public async Task<string> CreateStripeCustomer(User user)
        {
            var customerOptions = new CustomerCreateOptions {
                Description = "Customer for " + user.Email,
                Email = user.Email,
                //PlanId = PlanType.Basic.GetDescription() // TODO
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

        public async Task<bool> CreateSubscription(SubscriptionCreateOptions options)
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
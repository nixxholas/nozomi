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
        }

        public async Task<bool> AddCard(User user, string cardToken)
        {
            if (string.IsNullOrEmpty(user.StripeCustomerId)) return false;
            
            StripeConfiguration.SetApiKey(_options.Value.SecretKey);
            
            // Stripe-sided checks
            var customerService = new CustomerService();
            var customer = await customerService.GetAsync(user.StripeCustomerId);
            if (customer == null) return false;
            
            // Create the card binding
            var sourceService = new SourceService();
            var result = await sourceService.CreateAsync(new SourceCreateOptions()
            {
                Token = cardToken,
                Customer = customer.Id
            });

            return (result != null);
        }

        public async Task<User> ConfigureUserForStripe(User user)
        {
            StripeConfiguration.SetApiKey(_options.Value.SecretKey);
            
            var customerOptions = new CustomerCreateOptions {
                Description = "Customer for " + user.Email,
                Email = user.Email,
                //PlanId = PlanType.Basic.GetDescription() // TODO
            };

            var customerService = new CustomerService();
            var customer = await customerService.CreateAsync(customerOptions);

            if (!string.IsNullOrEmpty(customer.Id))
            {
                user.StripeCustomerId = customer.Id;
            }
            else
            {
                // Failed, send it back immediately.
                return user;
            }

            return user;
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
            StripeConfiguration.SetApiKey(_options.Value.SecretKey);
            
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
            StripeConfiguration.SetApiKey(_options.Value.SecretKey);
            
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
            StripeConfiguration.SetApiKey(_options.Value.SecretKey);
            
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
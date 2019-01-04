using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Nozomi.Base.Core.Configurations;
using Nozomi.Preprocessing.Events.Interfaces;
using Stripe;

namespace Nozomi.Preprocessing.Events
{
    public class StripeEvent : IStripeEvent
    {
        private readonly IOptions<StripeSettings> _options;

        public StripeEvent(IOptions<StripeSettings> options)
        {
            _options = options;
        }

        public bool Subscribe()
        {
            var customers = new CustomerService();
            var charges = new ChargeService();

            var customer = customers.Create(new CustomerCreateOptions {
                Email = "",
                SourceToken = "" // STRIPE TOKEN
            });

            var charge = charges.Create(new ChargeCreateOptions {
                Amount = 500,
                Description = "Sample Charge",
                Currency = "usd",
                CustomerId = customer.Id
            });

            return true;
        }

        public async Task<ICollection<Plan>> Plans(PlanListOptions options)
        {
            var planService = new PlanService();
            var plans = await planService.ListAsync(options);

            return plans.Data;
        }
    }
}
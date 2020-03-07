using Microsoft.Extensions.Logging;
using Nozomi.Infra.Auth.Events.Stripe;
using Nozomi.Infra.Payment.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Stripe;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Nozomi.Infra.Payment.Services.Bootstripe;

namespace Nozomi.Infra.Payment.Services
{
    class DisputesService : BaseService<DisputesService>, IDisputesService
    {
        private readonly IStripeEvent _stripeEvent;
        private readonly IBootstripeService _bootstripeService;

        public DisputesService(ILogger<DisputesService> logger, IStripeEvent stripeEvent, IBootstripeService bootstripeService) : base(logger) {
            _stripeEvent = stripeEvent;
            _bootstripeService = bootstripeService;
        }

        [Obsolete]
        public async Task FundsWithdrawn(Dispute dispute)
        {
            var methodName = "DisputeCreated";
            PerformDisputePreCheck(dispute, methodName);

            var user = await _stripeEvent.GetUserByCustomerId(dispute.Charge.CustomerId);

            if(user == null)
                throw new NullReferenceException($"{_serviceName} {methodName}: Unable to find user tied to customer id.");

            //TODO: Retrieve free plan ID from appsettings
            await _bootstripeService.ChangePlan("plan_GeGfCW7hwiQYQk", user);
            return;
        }

        public async Task DisputeLost(Dispute dispute)
        {
            var methodName = "DisputeLost";
            PerformDisputePreCheck(dispute, methodName);
            
            if(!dispute.Status.Equals("lost"))
                throw new InvalidOperationException($"{_serviceName} {methodName}: Dispute is not lost");

            var customerId = dispute.Charge.CustomerId;
            
            var user = await _stripeEvent.GetUserByCustomerId(dispute.Charge.CustomerId);

            if(user == null)
                throw new NullReferenceException($"{_serviceName} {methodName}: Unable to find user tied to customer id.");

            //TODO: Retrieve free plan ID from appsettings
            await _bootstripeService.ChangePlan("plan_GeGfCW7hwiQYQk", user);
        }

        private void PerformDisputePreCheck(Dispute dispute, string methodName) {
            if (dispute == null)
                throw new NullReferenceException($"{_serviceName} {methodName}: Dispute is null.");
            if (string.IsNullOrEmpty(dispute.Charge.CustomerId))
                throw new NullReferenceException($"{_serviceName} {methodName}: CustomerId is null.");
        }
    }
}

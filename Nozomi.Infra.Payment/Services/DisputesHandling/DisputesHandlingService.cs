using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nozomi.Infra.Auth.Events.UserEvent;
using Nozomi.Infra.Payment.Services.Bootstripe;
using Nozomi.Infra.Payment.Services.SubscriptionHandling;
using Nozomi.Preprocessing.Abstracts;
using Stripe;

namespace Nozomi.Infra.Payment.Services.DisputesHandling
{
    class DisputesHandlingService : BaseService<DisputesHandlingService>, IDisputesHandlingService
    {
        private readonly IBootstripeService _bootstripeService;
        private readonly IUserEvent _userEvent;
        private readonly ISubscriptionsHandlingService _subscriptionsHandlingService;

        public DisputesHandlingService(ILogger<DisputesHandlingService> logger, IBootstripeService bootstripeService, ISubscriptionsHandlingService subscriptionsHandlingService, IUserEvent userEvent) : base(logger) {
            _bootstripeService = bootstripeService;
            _subscriptionsHandlingService = subscriptionsHandlingService;
            _userEvent = userEvent;
        }

        public async Task DisputeClosed(Dispute dispute)
        {
            var methodName = "DisputeClosed";
            PerformDisputePreCheck(dispute, methodName);

            switch (dispute.Status.ToLower())
            {
                case "lost":
                    await DisputeLost(dispute);
                    return;
                default:
                    throw new InvalidOperationException($"{_serviceName} {methodName}: Dispute is not closed");
            }
        }

        [Obsolete]
        public async Task FundsWithdrawn(Dispute dispute)
        {
            var methodName = "DisputeCreated";
            PerformDisputePreCheck(dispute, methodName);

            var user = await _userEvent.GetUserByCustomerId(dispute.Charge.CustomerId);

            if(user == null)
                throw new NullReferenceException($"{_serviceName} {methodName}: Unable to find user tied to customer id.");

            //TODO: Retrieve free plan ID from appsettings
            await _subscriptionsHandlingService.ChangePlan("plan_GeGfCW7hwiQYQk", user);
            return;
        }

        public async Task DisputeLost(Dispute dispute)
        {
            var methodName = "DisputeLost";
            PerformDisputePreCheck(dispute, methodName);
            
            if(!dispute.Status.Equals("lost"))
                throw new InvalidOperationException($"{_serviceName} {methodName}: Dispute is not lost");

            var customerId = dispute.Charge.CustomerId;
            
            var user = await _userEvent.GetUserByCustomerId(customerId);

            if(user == null)
                throw new NullReferenceException($"{_serviceName} {methodName}: Unable to find user tied to customer id: {customerId}");

            //TODO: Retrieve free plan ID from appsettings
            await _subscriptionsHandlingService.ChangePlan("plan_GeGfCW7hwiQYQk", user);
        }

        private void PerformDisputePreCheck(Dispute dispute, string methodName) {
            if (dispute == null)
                throw new NullReferenceException($"{_serviceName} {methodName}: Dispute is null.");
            if (string.IsNullOrEmpty(dispute.Charge.CustomerId))
                throw new NullReferenceException($"{_serviceName} {methodName}: CustomerId is null.");
        }
    }
}

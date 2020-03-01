using Microsoft.Extensions.Logging;
using Nozomi.Infra.Auth.Events.Stripe;
using Nozomi.Infra.Payment.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Stripe;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nozomi.Infra.Payment.Services
{
    class DisputesService : BaseService<DisputesService>, IDisputesService
    {
        private readonly IStripeEvent _stripeEvent;

        public DisputesService(ILogger<DisputesService> logger, IStripeEvent stripeEvent) : base(logger) {
            _stripeEvent = stripeEvent;
        }

        public async Task DisputeClosed(Dispute dispute)
        {
            var methodName = "DisputeClosed";
            PerformDisputePreCheck(dispute, methodName);

            var user = await _stripeEvent.GetUserByCustomerId(dispute.Charge.CustomerId);

            if (user == null)
                throw new NullReferenceException($"{_serviceName} {methodName}: Unable to find user tied to customer id.");

            switch (dispute.Status.ToLower()) {
                case "warning_needs_response":
                    //await _quotaService.DowngradeQuota();
                    return;
                case "warning_under_review":
                    //await _quotaService.DowngradeQuota();
                    return;
                case "warning_closed":
                    //await _quotaService.DowngradeQuota();
                    return;
                case "needs_response":
                    //await _quotaService.DowngradeQuota();
                    return;
                case "under_review":
                    //await _quotaService.DowngradeQuota();
                    return;
                case "charge_refunded":
                    //await _quotaService.DowngradeQuota();
                    return;
                case "won":
                    //await _quotaService.UpgradeQuota();
                    return;
                case "lost":
                    //await _quotaService.DowngradeQuota();
                    return;
                default:
                    throw new InvalidOperationException($"{_serviceName} {methodName}: Unable to find user tied to customer id.");
            }
        }

        public async Task DisputeCreated(Dispute dispute)
        {
            var methodName = "DisputeCreated";
            PerformDisputePreCheck(dispute, methodName);

            var user = await _stripeEvent.GetUserByCustomerId(dispute.Charge.CustomerId);

            if(user == null)
                throw new NullReferenceException($"{_serviceName} {methodName}: Unable to find user tied to customer id.");

            return;
        }

        public Task DisputeUpdated(Dispute dispute)
        {
            throw new NotImplementedException();
        }

        private void PerformDisputePreCheck(Dispute dispute, string methodName) {
            if (dispute == null)
                throw new NullReferenceException($"{_serviceName} {methodName}: Dispute is null.");
            if (string.IsNullOrEmpty(dispute.Charge.CustomerId))
                throw new NullReferenceException($"{_serviceName} {methodName}: CustomerId is null.");
        }
    }
}

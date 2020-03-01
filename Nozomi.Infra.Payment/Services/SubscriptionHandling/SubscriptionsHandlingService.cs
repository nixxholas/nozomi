using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nozomi.Infra.Auth.Events.Stripe;
using Nozomi.Infra.Auth.Events.UserEvent;
using Nozomi.Infra.Auth.Services.QuotaClaims;
using Nozomi.Infra.Payment.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Stripe;

namespace Nozomi.Infra.Payment.Services.SubscriptionHandling
{
    public class SubscriptionsHandlingService: BaseService<SubscriptionsHandlingService>, ISubscriptionsHandlingService
    {
        private readonly IQuotaClaimsService _quotaClaimsService;
        private readonly IUserEvent _userEvent;
        private readonly IStripeEvent _stripeEvent;
        
        public SubscriptionsHandlingService(ILogger<SubscriptionsHandlingService> logger, IQuotaClaimsService quotaClaimsService, IUserEvent userEvent, IStripeEvent stripeEvent) : base(logger)
        {
            _quotaClaimsService = quotaClaimsService;
            _userEvent = userEvent;
            _stripeEvent = stripeEvent;
        }

        public async Task SubscriptionCancelled(Subscription subscription)
        {
            const string methodName = "SubscriptionCancelled";
            PerformSubscriptionPrecheck(subscription, methodName);
            
            if(!subscription.Status.ToLower().Equals("canceled "))
                throw new InvalidOperationException($"{_serviceName} {methodName}: ");

            var customerId = subscription.CustomerId;
            var user = await _stripeEvent.GetUserByCustomerId(customerId);

            if (user == null)
                throw new NullReferenceException(
                    $"{_serviceName} {methodName}: Unable to retrieve user based on customer id {customerId}");
            
            //TODO: Get default plan quota
            var quota = 5000;
            _quotaClaimsService.SetQuota(user.Id, quota);
        }

        private void PerformSubscriptionPrecheck(Subscription subscription, string methodName)
        {
            if(subscription == null)
                throw new ArgumentNullException($"{_serviceName} {methodName}: Subscription is null");
        }
    }
}
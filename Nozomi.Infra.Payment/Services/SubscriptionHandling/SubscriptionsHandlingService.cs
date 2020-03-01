using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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
        
        public SubscriptionsHandlingService(ILogger<SubscriptionsHandlingService> logger, IQuotaClaimsService quotaClaimsService, IUserEvent userEvent) : base(logger)
        {
            _quotaClaimsService = quotaClaimsService;
            _userEvent = userEvent;
        }

        public Task SubscriptionCancelled(Subscription subscription)
        {
            throw new System.NotImplementedException();
        }
    }
}
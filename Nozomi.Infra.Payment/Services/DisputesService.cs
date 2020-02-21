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
        private readonly IQuotaService _quotaService;
        private readonly IStripeEvent _stripeEvent;

        public DisputesService(ILogger<DisputesService> logger, IQuotaService quotaService, IStripeEvent stripeEvent) : base(logger) {
            _quotaService = quotaService;
            _stripeEvent = stripeEvent;
        }

        public Task DisputeClosed(Dispute dispute)
        {
            throw new NotImplementedException();
        }

        public Task DisputeCreated(Dispute dispute)
        {
            throw new NotImplementedException();
        }

        public Task DisputeUpdated(Dispute dispute)
        {
            throw new NotImplementedException();
        }
    }
}

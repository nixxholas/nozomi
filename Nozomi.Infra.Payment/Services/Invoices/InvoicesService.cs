using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Global;
using Nozomi.Base.BCL.Configurations;
using Nozomi.Infra.Auth.Events.Stripe;
using Nozomi.Infra.Auth.Services.QuotaClaims;
using Nozomi.Infra.Payment.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;
using Nozomi.Repo.BCL.Repository;
using Stripe;

namespace Nozomi.Infra.Payment.Services
{
    class InvoicesService : BaseService<InvoicesService>, IInvoicesService
    {
        private readonly IStripeEvent _stripeEvent;
        private readonly IQuotaClaimsService _quotaClaimsService;
        public InvoicesService(ILogger<InvoicesService> logger, IStripeEvent stripeEvent, IQuotaClaimsService quotaClaimsService) : base(logger) {
            _stripeEvent = stripeEvent;
            _quotaClaimsService = quotaClaimsService;
        }

        public async Task InvoiceFinalized(Invoice invoice)
        {
            var methodName = "InvoiceFinalized";
            PerformInvoicePrecheck(invoice, methodName);

            var user = await _stripeEvent.GetUserByCustomerId(invoice.CustomerId);

            if(user == null)
                throw new NullReferenceException($"{_serviceName} {methodName}: Unable to find user tied to customer id.");
            
            _quotaClaimsService.RestUsage(user.Id);

            return;
        }

        private void PerformInvoicePrecheck(Invoice invoice, string methodName)
        {
            if (invoice == null)
                throw new NullReferenceException($"{_serviceName} {methodName}: Invoice is null.");
            if (string.IsNullOrEmpty(invoice.CustomerId))
                throw new NullReferenceException($"{_serviceName} {methodName}: Customer id is null");
        }
    }
}

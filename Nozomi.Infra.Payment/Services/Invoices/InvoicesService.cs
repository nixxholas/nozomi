using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Global;
using Nozomi.Base.BCL.Configurations;
using Nozomi.Infra.Auth.Events.Stripe;
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
        private readonly IQuotaService _quotaService;
        public InvoicesService(ILogger<InvoicesService> logger, IStripeEvent stripeEvent, IQuotaService quotaService) : base(logger) {
            _stripeEvent = stripeEvent;
            _quotaService = quotaService;
        }

        public async Task InvoiceCreated(Invoice invoice)
        {
            throw new NotImplementedException();
        }

        public async Task InvoiceFinalized(Invoice invoice)
        {
            throw new NotImplementedException();
        }

        public async Task InvoicePaid(Invoice invoice)
        {
            var methodName = "InvoicePaid";
            PerformInvoicePrecheck(invoice, methodName);

            if (!invoice.Paid)
                throw new InvalidOperationException($"{_serviceName} {methodName}: Invoice is not paid.");


            var user = await _stripeEvent.GetUserByCustomerId(invoice.CustomerId);

            if(user == null)
                throw new NullReferenceException($"{_serviceName} {methodName}: Unable to find user tied to customer id.");

            await _quotaService.ResetQuota();

            return;
        }

        public async Task InvoicePaymentFailed(Invoice invoice)
        {
            var methodName = "InvoicePaymentFailed";
            PerformInvoicePrecheck(invoice, methodName);

            if (invoice.Paid)
                throw new InvalidOperationException($"{_serviceName} {methodName}: Invoice is paid.");


            var user = await _stripeEvent.GetUserByCustomerId(invoice.CustomerId);

            if (user == null)
                throw new NullReferenceException($"{_serviceName} {methodName}: Unable to find user tied to customer id.");

            await _quotaService.DowngradeQuota();

            return;
        }

        public async Task InvoiceUpcoming(Invoice invoice)
        {
            throw new NotImplementedException();
        }

        private void PerformInvoicePrecheck(Invoice invoice, string methodName) {
            if (invoice == null)
                throw new NullReferenceException($"{_serviceName} {methodName}: Invoice is null.");
            if (string.IsNullOrEmpty(invoice.CustomerId))
                throw new NullReferenceException($"{_serviceName} {methodName}: Customer id is null");
        }
    }
}

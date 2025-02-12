﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nozomi.Infra.Auth.Events.UserEvent;
using Nozomi.Infra.Auth.Services.QuotaClaim;
using Nozomi.Preprocessing.Abstracts;
using Stripe;

namespace Nozomi.Infra.Payment.Services.InvoicesHandling
{
    public class InvoicesHandlingService : BaseService<InvoicesHandlingService>, IInvoicesHandlingService
    {
        private readonly IUserEvent _userEvent;
        private readonly IQuotaClaimService _quotaClaimService;
        
        public InvoicesHandlingService(ILogger<InvoicesHandlingService> logger, IUserEvent userEvent, 
            IQuotaClaimService quotaClaimService) : base(logger) {
            _userEvent = userEvent;
            _quotaClaimService = quotaClaimService;
        }

        public async Task InvoiceFinalized(Invoice invoice)
        {
            var methodName = "InvoiceFinalized";
            PerformInvoicePrecheck(invoice, methodName);

            var user = await _userEvent.GetUserByCustomerId(invoice.CustomerId);

            if (user == null)
                throw new NullReferenceException($"{_serviceName} {methodName}: " +
                                                 "Unable to find user tied to customer id.");
            
            _quotaClaimService.ResetUsage(user.Id);
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

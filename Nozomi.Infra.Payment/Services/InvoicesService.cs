using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
    class InvoicesService : IInvoicesService
    {
        private readonly StripeEvent _stripeEvent;
        public InvoicesService() {
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
            if (invoice.Paid) {
                //Call quota service to update user's quota
            }

            return;
        }

        public async Task InvoicePaymentFailed(Invoice invoice)
        {
            throw new NotImplementedException();
        }

        public async Task InvoiceUpcoming(Invoice invoice)
        {
            throw new NotImplementedException();
        }
    }
}

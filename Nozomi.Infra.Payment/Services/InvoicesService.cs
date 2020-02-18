using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Nozomi.Base.Auth.Global;
using Nozomi.Base.BCL.Configurations;
using Nozomi.Infra.Payment.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;
using Nozomi.Repo.BCL.Repository;
using Stripe;

namespace Nozomi.Infra.Payment.Services
{
    class InvoicesService : IInvoicesService
    {
        public Task InvoiceCreated(Invoice invoice)
        {
            throw new NotImplementedException();
        }

        public Task InvoiceFinalized(Invoice invoice)
        {
            throw new NotImplementedException();
        }

        public Task InvoicePaid(Invoice invoice)
        {
            throw new NotImplementedException();
        }

        public Task InvoicePaymentFailed(Invoice invoice)
        {
            throw new NotImplementedException();
        }

        public Task InvoiceUpcoming(Invoice invoice)
        {
            throw new NotImplementedException();
        }
    }
}

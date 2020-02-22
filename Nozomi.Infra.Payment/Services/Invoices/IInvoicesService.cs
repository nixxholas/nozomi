using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stripe;

namespace Nozomi.Infra.Payment.Services.Interfaces
{
    public interface IInvoicesService
    {
        Task InvoiceCreated(Invoice invoice);

        Task InvoiceFinalized(Invoice invoice);

        Task InvoicePaid(Invoice invoice);

        Task InvoicePaymentFailed(Invoice invoice);

        Task InvoiceUpcoming(Invoice invoice);

    }
}

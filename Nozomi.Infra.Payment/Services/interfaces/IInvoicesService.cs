using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stripe;

namespace Nozomi.Infra.Payment.Services.Interfaces
{
    public interface IInvoicesService
    {
        Task InvoiceCreated(Event stripeEvent);

        Task InvoiceFinalized(Event stripeEvent);

        Task InvoicePaid(Event stripeEvent);

        Task InvoicePaymentFailed(Event stripeEvent);

        Task InvoiceUpcoming(Event stripeEvent);

    }
}

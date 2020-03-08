using System.Threading.Tasks;
using Stripe;

namespace Nozomi.Infra.Payment.Services.InvoicesHandling
{
    public interface IInvoicesHandlingService
    {
        Task InvoiceFinalized(Invoice invoice);

    }
}

using System.Threading.Tasks;
using Stripe;

namespace Nozomi.Infra.Payment.Services.DisputesHandling
{
    public interface IDisputesHandlingService
    {
        Task DisputeClosed(Dispute dispute);
        Task FundsWithdrawn(Dispute dispute);

        Task DisputeLost(Dispute dispute);
    }
}

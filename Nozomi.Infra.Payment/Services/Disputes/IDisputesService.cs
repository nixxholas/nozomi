using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stripe;

namespace Nozomi.Infra.Payment.Services.Interfaces
{
    public interface IDisputesService
    {
        Task FundsWithdrawn(Dispute dispute);

        Task DisputeLost(Dispute dispute);
    }
}

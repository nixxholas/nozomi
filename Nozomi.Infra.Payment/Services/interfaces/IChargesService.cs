using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stripe;

namespace Nozomi.Infra.Payment.Services.Interfaces
{
    public interface IChargesService
    {
        Task ChargeSucceeded(Charge charge);

        Task ChargeFailed(Charge charge);

        Task ChargeRefunded(Charge charge);

        Task ChargeUpdated(Charge charge);

        Task ChargExpired(Charge charge);
    }
}

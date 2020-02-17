using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stripe;

namespace Nozomi.Infra.Payment.Services.Interfaces
{
    public interface IChargesService
    {
        Task ChargeSucceeded(Event stripeEvent);

        Task ChargeFailed(Event stripeEvent);

        Task ChargeRefunded(Event stripeEvent);

        Task ChargeUpdated(Event stripeEvent);

        Task ChargExpired(Event stripeEvent);
    }
}

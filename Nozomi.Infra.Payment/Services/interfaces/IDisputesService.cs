﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stripe;

namespace Nozomi.Infra.Payment.Services.Interfaces
{
    public interface IDisputesService
    {
        Task DisputeCreated(Event stripeEvent);

        Task DisputeUpdated(Event stripeEvent);

        Task DisputeClosed(Event stipeEvent);
    }
}

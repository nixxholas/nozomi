﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Stripe;

namespace Nozomi.Infra.Auth.Events.UserEvent
{
    public interface IUserEvent
    {
        bool HasStripe(string userId);

        void AddPaymentMethod(string userId, string paymentMethodId);

        void RemovePaymentMethod(string userId, string paymentMethodId);
    }
}
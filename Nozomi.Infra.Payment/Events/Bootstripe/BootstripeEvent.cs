using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Models;
using Nozomi.Preprocessing.Abstracts;
using Stripe;

namespace Nozomi.Infra.Payment.Events.Bootstripe
{
    public class BootstripeEvent : BaseEvent<BootstripeEvent>, IBootstripeEvent
    {
        public BootstripeEvent(ILogger<BootstripeEvent> logger) : base(logger)
        {
        }

        public bool IsDefaultPlan(string planId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Plan>> Plans(bool activeOnly = true)
        {
            throw new System.NotImplementedException();
        }

        public Plan Plan(string planId)
        {
            throw new System.NotImplementedException();
        }

        public bool PlanExists(string planId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Card>> Cards(User user)
        {
            throw new System.NotImplementedException();
        }

        public bool CardExists(string stripeUserId, string cardId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<PaymentMethod>> ListPaymentMethods(string stripeUserId, string paymentMethodType = "card")
        {
            throw new System.NotImplementedException();
        }

        public bool PaymentMethodExists(string stripeUserId, string paymentMethodId)
        {
            throw new System.NotImplementedException();
        }
    }
}
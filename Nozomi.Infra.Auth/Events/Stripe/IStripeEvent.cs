using System.Collections.Generic;
using System.Threading.Tasks;
using Stripe;

namespace Nozomi.Infra.Auth.Events.Stripe
{
    public interface IStripeEvent
    {
        /// <summary>
        /// Obtain all plans stashed for our product
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Plan>> Plans(bool activeOnly = true);

        Task<IEnumerable<Card>> Cards(Base.Auth.Models.User user);
    }
}
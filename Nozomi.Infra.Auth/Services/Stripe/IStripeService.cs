using Stripe;
using System.Threading.Tasks;

namespace Nozomi.Infra.Auth.Services.Stripe
{
    public interface IStripeService
    {
        Task PropagateCustomer(Base.Auth.Models.User user);
        
        Task AddCard(string stripeCardId, Base.Auth.Models.User user);

        void RemoveCard(string stripeCardId, Base.Auth.Models.User user);

        void Subscribe(Plan plan, Base.Auth.Models.User user);

        void Unsubscribe(Base.Auth.Models.User user);

        void ChangeSubscription(Plan plan, Base.Auth.Models.User user);
        
    }
}
using System.Threading.Tasks;

namespace Nozomi.Infra.Auth.Services.Stripe
{
    public interface IStripeService
    {
        Task PropagateCustomer(Base.Auth.Models.User user);
        
        void addCard(string stripeCardId, Base.Auth.Models.User user);

        void removeCard(string stripeCardId, Base.Auth.Models.User user);
    }
}
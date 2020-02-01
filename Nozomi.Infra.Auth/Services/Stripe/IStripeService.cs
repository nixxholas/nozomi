using Stripe;
using System.Threading.Tasks;

namespace Nozomi.Infra.Auth.Services.Stripe
{
    public interface IStripeService
    {
        Task PropagateCustomer(Base.Auth.Models.User user);
        
        void addCard(string stripeCardId, Base.Auth.Models.User user);

        void removeCard(string stripeCardId, Base.Auth.Models.User user);

        void subscribePlan(Plan plan, Base.Auth.Models.User user);

        void cancelPlan(Base.Auth.Models.User user);

        void changePlan(Plan plan, Base.Auth.Models.User user);
        
    }
}
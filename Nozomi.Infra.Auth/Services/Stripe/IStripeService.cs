using Stripe;
using System.Threading.Tasks;

namespace Nozomi.Infra.Auth.Services.Stripe
{
    public interface IStripeService
    {
        Task PropagateCustomer(Base.Auth.Models.User user);
        
        Task AddPaymentMethod(string paymentMethodId, Base.Auth.Models.User user);

        Task RemovePaymentMethod(string paymentMethodId, Base.Auth.Models.User user);

        Task Subscribe(Plan plan, Base.Auth.Models.User user);

        Task Unsubscribe(Base.Auth.Models.User user);

        void ChangeSubscription(Plan plan, Base.Auth.Models.User user);
        
    }
}
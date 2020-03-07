using Stripe;
using System.Threading.Tasks;

namespace Nozomi.Infra.Auth.Services.Stripe
{
    public interface IStripeService
    {
        Task Unsubscribe(Base.Auth.Models.User user);
    }
}
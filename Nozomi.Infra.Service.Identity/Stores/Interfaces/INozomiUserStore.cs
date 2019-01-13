using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Nozomi.Base.Identity.ViewModels.Identity;

namespace Nozomi.Service.Identity.Stores.Interfaces
{
    public interface INozomiUserStore : IUserStore<User>
    {
        Task ForceConfirmEmailAsync(long userId);
        Task ForceConfirmEmailAsync(User user);
        Task<bool> PropagateStripeCustomerId(long userId, string stripeCustomerId);
    }
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Nozomi.Base.Identity.Models.Identity;

namespace Nozomi.Service.Identity.Stores.Interfaces
{
    public interface INozomiUserStore : IUserStore<User>
    {
        Task ForceConfirmEmailAsync(long userId);
        Task ForceConfirmEmailAsync(User user);
    }
}
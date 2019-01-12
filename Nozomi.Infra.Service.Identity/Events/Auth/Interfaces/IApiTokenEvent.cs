using System.Collections.Generic;
using System.Threading.Tasks;
using Nozomi.Base.Identity.Models.Identity;

namespace Nozomi.Service.Identity.Events.Auth.Interfaces
{
    public interface IApiTokenEvent
    {
        Task<ICollection<ApiToken>> ApiTokensByUserId(long userId, bool onlyFunctional = false);

        bool IsValid(string key);
    }
}
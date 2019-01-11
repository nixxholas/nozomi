using System;
using System.Threading.Tasks;
using Nozomi.Base.Identity.Models.Identity;

namespace Nozomi.Service.Identity.Services.Interfaces
{
    public interface IApiTokenService
    {
        Task<ApiToken> GenerateTokenAsync(long userId, string label = null);

        Task<bool> RevokeTokenAsync(Guid tokenGuid, long userId = 0);
    }
}
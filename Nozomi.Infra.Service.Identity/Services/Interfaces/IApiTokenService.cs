using System;
using System.Threading.Tasks;
using Nozomi.Base.Identity.ViewModels.Identity;

namespace Nozomi.Service.Identity.Services.Interfaces
{
    public interface IApiTokenService
    {
        Task<bool> BanToken(Guid tokenGuid, long userId);
        
        Task<ApiToken> GenerateTokenAsync(long userId, string label = null);

        Task<bool> IsTokenBanned(Guid tokenGuid);

        Task<bool> RevokeTokenAsync(Guid tokenGuid, long userId = 0);
    }
}
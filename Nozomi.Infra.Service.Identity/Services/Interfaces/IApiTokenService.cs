using System;
using System.Threading.Tasks;

namespace Nozomi.Service.Identity.Services.Interfaces
{
    public interface IApiTokenService
    {
        Task<dynamic> GenerateTokenAsync(long userId);

        Task<bool> RevokeTokenAsync(Guid tokenGuid);
    }
}
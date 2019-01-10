using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Identity.Data;
using Nozomi.Service.Identity.Services.Interfaces;

namespace Nozomi.Service.Identity.Services
{
    public class ApiTokenService : BaseService<StripeService, NozomiAuthContext>, IApiTokenService
    {
        public ApiTokenService(ILogger<StripeService> logger, IUnitOfWork<NozomiAuthContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public Task<dynamic> GenerateTokenAsync(long userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RevokeTokenAsync(Guid tokenGuid)
        {
            throw new NotImplementedException();
        }
        
        private string GenerateAPIKey(string userId)
        {
            var key = new byte[64];
            using (var generator = RandomNumberGenerator.Create(userId))
            {
                generator.GetBytes(key);
            }
            return Convert.ToBase64String(key);
        }

        private byte[] GenerateAPIKeyBytes(string userId)
        {
            var key = new byte[64];
            using (var generator = RandomNumberGenerator.Create(userId))
            {
                generator.GetBytes(key);
            }

            return key;
        }
    }
}
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Identity.Models.Identity;
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

        public Task<bool> BanToken(Guid tokenGuid, long userId = 0)
        {
            var apiToken = _unitOfWork.GetRepository<ApiToken>()
                .Get(at => at.Guid.Equals(tokenGuid) && at.IsEnabled && at.DeletedAt == null)
                .SingleOrDefault();

            if (apiToken != null)
            {
                // BANN
                apiToken.IsEnabled = false;
                
                _unitOfWork.GetRepository<ApiToken>().Update(apiToken);
                _unitOfWork.Commit(userId);
                
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task<ApiToken> GenerateTokenAsync(long userId, string label = null)
        {
            var key = GenerateAPIKeyBytes(userId.ToString());
            
            var hmac = new HMACSHA512(key);
            var hmacSecret = hmac.ComputeHash(GenerateAPIKeyBytes(userId.ToString()));
            
            var apiToken = new ApiToken
            {
                UserId = userId,
                Label = label,
                Key = Convert.ToBase64String(key),
                Secret = Convert.ToBase64String(hmacSecret),
                LastAccessed = DateTime.UtcNow
            };
            
            _unitOfWork.GetRepository<ApiToken>()
                .Add(apiToken);
            _unitOfWork.Commit(userId);

            return Task.FromResult(apiToken);
        }

        public Task<bool> IsTokenBanned(Guid tokenGuid)
        {
            return Task.FromResult(_unitOfWork.GetRepository<ApiToken>()
                .GetQueryable()
                .Any(at => at.Guid.Equals(tokenGuid) && (!at.IsEnabled || at.DeletedAt != null)));
        }

        public Task<bool> RevokeTokenAsync(Guid tokenGuid, long userId = 0)
        {
            var apiToken = _unitOfWork.GetRepository<ApiToken>()
                .Get(at => at.Guid.Equals(tokenGuid) && at.DeletedAt == null)
                .SingleOrDefault();

            if (apiToken != null)
            {
                apiToken.DeletedAt = DateTime.UtcNow;
                apiToken.DeletedBy = userId;
                
                _unitOfWork.GetRepository<ApiToken>().Update(apiToken);
                _unitOfWork.Commit(userId);

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        private byte[] GenerateAPIKeyBytes(string userId)
        {
            var key = new byte[64];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(key);
            
            // With Parity
            // https://stackoverflow.com/questions/5349321/generate-random-bytes-for-tripledes-key-c-sharp
            for (var i = 0; i < key.Length; ++i)
            {
                int keyByte = key[i] & 0xFE;
                var parity = 0;
                for (var b = keyByte; b != 0; b >>= 1) parity ^= b & 1;
                key[i] = (byte)(keyByte | (parity == 0 ? 1 : 0));
            }

            return key;
        }
    }
}
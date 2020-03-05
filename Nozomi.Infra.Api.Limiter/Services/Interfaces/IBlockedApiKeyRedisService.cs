using System;

namespace Nozomi.Infra.Api.Limiter.Services.Interfaces
{
    public interface IBlockedApiKeyRedisService
    {
        void Add(string key, string value);

        void Remove(string key);
    }
}